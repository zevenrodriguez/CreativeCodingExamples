// Simple see-through water for URP.
// Works on cubes, planes, or any mesh: the ripples use WORLD position,
// so they line up across every face of a cube instead of repeating per face.
Shader "Custom/SimpleWater"
{
    Properties
    {
        _ShallowColor   ("Shallow Color", Color) = (0.25, 0.75, 0.85, 1)
        _DeepColor      ("Deep Color", Color)    = (0.02, 0.20, 0.40, 1)
        _Transparency   ("Opacity", Range(0, 1)) = 0.6

        _RippleSize     ("Ripple Size", Float)            = 2.0
        _RippleSpeed    ("Ripple Speed", Float)           = 1.0
        _RippleStrength ("Ripple Strength", Range(0, 1))  = 0.35

        _Shininess      ("Shininess", Range(8, 256))      = 96
        _EdgeGlow       ("Edge Glow (Fresnel)", Range(0.5, 8)) = 3
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType"     = "Transparent"
            "Queue"          = "Transparent"
        }

        Pass
        {
            Name "Water"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha   // see-through
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _ShallowColor;
                half4 _DeepColor;
                half  _Transparency;
                float _RippleSize;
                float _RippleSpeed;
                half  _RippleStrength;
                half  _Shininess;
                half  _EdgeGlow;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS   : TEXCOORD1;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionCS = TransformWorldToHClip(OUT.positionWS);
                OUT.normalWS   = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float3 n = normalize(IN.normalWS);
                float3 a = abs(n);

                // Pick a flat "map" of the world for this face:
                // top/bottom use X-Z, side faces use Z-Y or X-Y.
                float2 uv;
                float3 tangent, bitangent;
                if (a.y > a.x && a.y > a.z) { uv = IN.positionWS.xz; tangent = float3(1,0,0); bitangent = float3(0,0,1); }
                else if (a.x > a.z)         { uv = IN.positionWS.zy; tangent = float3(0,0,1); bitangent = float3(0,1,0); }
                else                        { uv = IN.positionWS.xy; tangent = float3(1,0,0); bitangent = float3(0,1,0); }

                // --- Ripples: three sine waves moving in different directions ---
                float t = _Time.y * _RippleSpeed;
                float2 p = uv * _RippleSize;

                float w1 = p.x + t;
                float w2 = p.y * 1.3 + t * 1.1;
                float w3 = (p.x + p.y) * 0.7 - t * 0.8;

                float height = (sin(w1) + sin(w2) + sin(w3)) / 3.0;   // -1 .. 1

                // Slope of the ripples (used to bend the normal so light sparkles)
                float slopeX = cos(w1) + cos(w3) * 0.7;
                float slopeY = cos(w2) * 1.3 + cos(w3) * 0.7;
                n = normalize(n - (tangent * slopeX + bitangent * slopeY) * _RippleStrength * 0.3);

                // --- Lighting ---
                Light sun = GetMainLight();
                float3 viewDir = GetWorldSpaceNormalizeViewDir(IN.positionWS);

                half  diffuse  = saturate(dot(n, sun.direction)) * 0.5 + 0.5;
                half3 halfDir  = normalize(sun.direction + viewDir);
                half  sparkle  = pow(saturate(dot(n, halfDir)), _Shininess);
                half  fresnel  = pow(1.0 - saturate(dot(n, viewDir)), _EdgeGlow);

                // Mix deep and shallow color using the ripple height and the viewing angle.
                half3 color = lerp(_DeepColor.rgb, _ShallowColor.rgb, saturate(height * 0.5 + 0.5) * 0.5 + fresnel * 0.5);
                color = color * diffuse * sun.color + sparkle * sun.color;

                half alpha = saturate(lerp(_Transparency, 1.0, fresnel) + sparkle);
                return half4(color, alpha);
            }
            ENDHLSL
        }
    }
}
