using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityIntegration;
using OpenCVForUnity.UnityIntegration.Helper.Source2Mat;
using UnityEngine.UI;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

/// <summary>
/// Simple red-object detector using the webcam and OpenCV for Unity.
/// Attach to a GameObject with a Renderer (or set `displayObject` to the target).
/// It will show the webcam feed with detections drawn on top.
/// </summary>
public class ColorDetection : MonoBehaviour
{
    [Header("Display")]
    // If null, the script will use the GameObject this component is attached to
    public GameObject displayObject;
    // Optional UI display: if set, the RawImage will receive the camera feed texture
    public RawImage ResultPreview;

    // UI text overlays for FPS and coordinates (optional)
    public Text FpsText;
    public Text CoordsText;

    [Header("Detection")]
    // Minimum contour area to be considered an object (px)
    public double minContourArea = 800.0;

    // Webcam helper provided by OpenCVForUnity (handles permissions/platform differences)
    WebCamTexture2MatHelper webCamTexture2MatHelper;

    // Mats used during processing
    Mat rgbaMat;
    Mat rgbMat;
    Mat hsvMat;
    Mat mask1;
    Mat mask2;
    Mat mask;
    Mat hierarchy;
    Mat kernel;

    Texture2D texture;

    // FPS measurement
    public float fpsUpdateInterval = 0.5f;
    float fpsAccum = 0f; // FPS accumulated over the interval
    int fpsFrames = 0; // Frames drawn in the interval
    float fpsTimeLeft; // time left for current interval
    float currentFps = 0f;

    // Last detected object's centroid and area
    bool lastDetected = false;
    int lastCx = 0;
    int lastCy = 0;
    double lastArea = 0.0;

    void Start()
    {
        Debug.Log("ColorDetection: Starting initialization...");

        // Ensure we have a target to display the texture
        if (displayObject == null)
        {
            Debug.Log("ColorDetection: No display object assigned, using this GameObject");
            displayObject = gameObject;
        }

        StartCoroutine(CheckPermissionsAndInitialize());
    }

    private IEnumerator CheckPermissionsAndInitialize()
    {
        bool granted = false;

        // Request camera permission
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            yield return new WaitForSeconds(0.1f);
        }
        granted = Permission.HasUserAuthorizedPermission(Permission.Camera);
        #elif PLATFORM_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }
        granted = Application.HasUserAuthorization(UserAuthorization.WebCam);
        #else
        granted = true;
        #endif

        if (!granted)
        {
            Debug.LogError("ColorDetection: Camera permission is denied! Please grant camera access in your device settings.");
            yield break;
        }

        // Check available webcams
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.LogError("ColorDetection: No webcam found!");
            yield break;
        }
        
        Debug.Log($"ColorDetection: Found {devices.Length} camera(s):");
        foreach (var device in devices)
        {
            Debug.Log($"Camera: {device.name} (isFrontFacing: {device.isFrontFacing})");
        }

        try
        {
            // Check if we already have the helper component
            webCamTexture2MatHelper = gameObject.GetComponent<WebCamTexture2MatHelper>();
            if (webCamTexture2MatHelper == null)
            {
                Debug.Log("ColorDetection: Adding WebCamTexture2MatHelper component");
                webCamTexture2MatHelper = gameObject.AddComponent<WebCamTexture2MatHelper>();
            }

            if (webCamTexture2MatHelper == null)
            {
                Debug.LogError("ColorDetection: Failed to create WebCamTexture2MatHelper component!");
                yield break;
            }

            // Configure the helper
            webCamTexture2MatHelper.RequestedWidth = 640;
            webCamTexture2MatHelper.RequestedHeight = 480;
            webCamTexture2MatHelper.RequestedFPS = 30;
            webCamTexture2MatHelper.OutputColorFormat = Source2MatHelperColorFormat.RGBA;
            webCamTexture2MatHelper.TimeoutFrameCount = 300; // Reduced timeout for faster failure detection            // Subscribe to lifecycle events so we can create/destroy textures and mats
            webCamTexture2MatHelper.OnInitialized.AddListener(OnWebCamTexture2MatHelperInitialized);
            webCamTexture2MatHelper.OnDisposed.AddListener(OnWebCamTexture2MatHelperDisposed);
            webCamTexture2MatHelper.OnErrorOccurred.AddListener(OnWebCamTexture2MatHelperErrorOccurred);

            Debug.Log("ColorDetection: Starting WebCamTexture2MatHelper initialization");
            webCamTexture2MatHelper.Initialize();
        }
        catch (System.Exception e)
        {
            Debug.LogError("ColorDetection: Error during initialization: " + e.ToString());
        }
    }

    /// <summary>
    /// Called by WebCamTextureToMatHelper when initialization is complete.
    /// We create Mats and the Texture2D for display here.
    /// </summary>
    public void OnWebCamTexture2MatHelperInitialized()
    {
        Debug.Log("ColorDetection: WebCam initialized successfully, creating Mats and texture");
        
        if (webCamTexture2MatHelper == null)
        {
            Debug.LogError("ColorDetection: Helper is null in OnInitialized!");
            return;
        }

        Mat camMat = webCamTexture2MatHelper.GetMat();
        if (camMat == null)
        {
            Debug.LogError("ColorDetection: Failed to get initial Mat from helper!");
            return;
        }

        Debug.Log($"ColorDetection: Got Mat with size {camMat.width()}x{camMat.height()}");

        rgbaMat = new Mat(camMat.rows(), camMat.cols(), CvType.CV_8UC4);
        rgbMat = new Mat(camMat.rows(), camMat.cols(), CvType.CV_8UC3);
        hsvMat = new Mat(camMat.rows(), camMat.cols(), CvType.CV_8UC3);
        mask1 = new Mat(camMat.rows(), camMat.cols(), CvType.CV_8UC1);
        mask2 = new Mat(camMat.rows(), camMat.cols(), CvType.CV_8UC1);
        mask = new Mat(camMat.rows(), camMat.cols(), CvType.CV_8UC1);
        hierarchy = new Mat();
        kernel = Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(5, 5));

        texture = new Texture2D(camMat.cols(), camMat.rows(), TextureFormat.RGBA32, false);

        // If a RawImage UI is assigned, use it for display and set aspect ratio
        if (ResultPreview != null)
        {
            ResultPreview.texture = texture;
            var ar = ResultPreview.GetComponent<AspectRatioFitter>();
            if (ar != null)
                ar.aspectRatio = (float)texture.width / texture.height;
        }
        else
        {
            var renderer = displayObject.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.mainTexture = texture;
            else
            {
                // If no Renderer found, try RawImage on displayObject
                var rawImage = displayObject.GetComponent<UnityEngine.UI.RawImage>();
                if (rawImage != null)
                    rawImage.texture = texture;
            }
        }

        // Initialize FPS timer
        fpsTimeLeft = fpsUpdateInterval;
    }

    /// <summary>
    /// Called when the helper is disposed.
    /// </summary>
    public void OnWebCamTexture2MatHelperDisposed()
    {
        Debug.Log("OnWebCamTexture2MatHelperDisposed");
        if (texture != null)
        {
            Texture2D.Destroy(texture);
            texture = null;
        }
    }

    /// <summary>
    /// Called when an error occurs in the helper.
    /// </summary>
    public void OnWebCamTexture2MatHelperErrorOccurred(Source2MatHelperErrorCode errorCode, string message)
    {
        Debug.LogError($"ColorDetection: Camera error occurred - {errorCode}: {message}");
        if (errorCode == Source2MatHelperErrorCode.CAMERA_DEVICE_NOT_EXIST)
        {
            Debug.LogError("ColorDetection: No camera device found! Please ensure a webcam is connected.");
        }
        else if (errorCode == Source2MatHelperErrorCode.CAMERA_PERMISSION_DENIED)
        {
            Debug.LogError("ColorDetection: Camera permission denied! Please grant camera access.");
        }
        else if (errorCode == Source2MatHelperErrorCode.TIMEOUT)
        {
            Debug.LogError("ColorDetection: Camera initialization timed out! Try restarting the application.");
        }
    }

    void Update()
    {
        if (webCamTexture2MatHelper == null)
        {
            Debug.LogError("ColorDetection: Helper is null in Update!");
            return;
        }

        if (!webCamTexture2MatHelper.IsPlaying())
        {
            Debug.Log("ColorDetection: Helper is not playing, starting playback");
            webCamTexture2MatHelper.Play();
            return;
        }

        // Only process when a new frame is available
        if (!webCamTexture2MatHelper.DidUpdateThisFrame())
            return;

        Mat camMat = webCamTexture2MatHelper.GetMat();
        if (camMat == null)
            return;

        // Convert to RGB and HSV
        Imgproc.cvtColor(camMat, rgbMat, Imgproc.COLOR_RGBA2RGB);
        Imgproc.cvtColor(rgbMat, hsvMat, Imgproc.COLOR_RGB2HSV);

        // Thresholds for red color (two ranges because hue wraps)
        // You may need to tweak these values for your lighting/camera
        Scalar lowerRed1 = new Scalar(0, 100, 100);
        Scalar upperRed1 = new Scalar(10, 255, 255);
        Scalar lowerRed2 = new Scalar(160, 100, 100);
        Scalar upperRed2 = new Scalar(179, 255, 255);

        Core.inRange(hsvMat, lowerRed1, upperRed1, mask1);
        Core.inRange(hsvMat, lowerRed2, upperRed2, mask2);
        Core.add(mask1, mask2, mask);

        // Clean up noise
        Imgproc.morphologyEx(mask, mask, Imgproc.MORPH_OPEN, kernel);
        Imgproc.morphologyEx(mask, mask, Imgproc.MORPH_CLOSE, kernel);

        // Find contours
        List<MatOfPoint> contours = new List<MatOfPoint>();
        Imgproc.findContours(mask, contours, hierarchy, Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_SIMPLE);

        // Draw detections onto the original RGBA frame (camMat)
        // Track the largest detection to report coordinates
        lastDetected = false;
        lastArea = 0.0;
        for (int i = 0; i < contours.Count; i++)
        {
            double area = Imgproc.contourArea(contours[i]);
            if (area < minContourArea)
                continue;

            // Bounding rect
            OpenCVForUnity.CoreModule.Rect rect = Imgproc.boundingRect(contours[i]);
            Imgproc.rectangle(camMat, rect.tl(), rect.br(), new Scalar(0, 255, 0, 255), 3);

            // Centroid
            Moments m = Imgproc.moments(contours[i]);
            if (m.get_m00() != 0)
            {
                int cx = (int)(m.get_m10() / m.get_m00());
                int cy = (int)(m.get_m01() / m.get_m00());
                Imgproc.circle(camMat, new Point(cx, cy), 6, new Scalar(255, 0, 0, 255), -1);

                // Keep the largest detection for reporting
                if (area > lastArea)
                {
                    lastArea = area;
                    lastCx = cx;
                    lastCy = cy;
                    lastDetected = true;
                }
            }

            // Optionally draw the contour
            Imgproc.drawContours(camMat, contours, i, new Scalar(255, 255, 0, 255), 2);
        }

        // Push the processed frame to the texture for display
        OpenCVMatUtils.MatToTexture2D(camMat, texture);

        // Update FPS counter
        fpsTimeLeft -= Time.deltaTime;
        fpsAccum += (Time.deltaTime > 0f) ? (1.0f / Time.deltaTime) : 0f;
        fpsFrames++;
        if (fpsTimeLeft <= 0f)
        {
            currentFps = fpsAccum / fpsFrames;
            fpsTimeLeft = fpsUpdateInterval;
            fpsAccum = 0f;
            fpsFrames = 0;
            if (FpsText != null)
                FpsText.text = string.Format("FPS: {0:F1}", currentFps);
        }

        // Update coordinates overlay
        if (CoordsText != null)
        {
            if (lastDetected)
                CoordsText.text = string.Format("Detected: x={0}, y={1}  area={2:F0}", lastCx, lastCy, lastArea);
            else
                CoordsText.text = "Detected: none";
        }
    }

    void OnDestroy()
    {
        if (webCamTexture2MatHelper != null)
        {
            webCamTexture2MatHelper.OnInitialized.RemoveListener(OnWebCamTexture2MatHelperInitialized);
            webCamTexture2MatHelper.OnDisposed.RemoveListener(OnWebCamTexture2MatHelperDisposed);
            webCamTexture2MatHelper.OnErrorOccurred.RemoveListener(OnWebCamTexture2MatHelperErrorOccurred);

            webCamTexture2MatHelper.Dispose();
            webCamTexture2MatHelper = null;
        }

        // Release Mats
        if (rgbaMat != null) rgbaMat.Dispose();
        if (rgbMat != null) rgbMat.Dispose();
        if (hsvMat != null) hsvMat.Dispose();
        if (mask1 != null) mask1.Dispose();
        if (mask2 != null) mask2.Dispose();
        if (mask != null) mask.Dispose();
        if (hierarchy != null) hierarchy.Dispose();
        if (kernel != null) kernel.Dispose();
    }
}
