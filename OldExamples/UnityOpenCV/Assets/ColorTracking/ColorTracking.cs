#if !OPENCV_DONT_USE_WEBCAMTEXTURE_API

using System;
using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityIntegration;
using CVRect = OpenCVForUnity.CoreModule.Rect;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OpenCVForUnityExample
{
    /// <summary>
    /// WebCamTextureToMat Example
    /// An example of converting a WebCamTexture image to OpenCV's Mat format.
    /// </summary>
    public class ColorTracking : MonoBehaviour
    {
        // Public Fields
        [Header("Output")]
        /// <summary>
        /// The RawImage for previewing the result.
        /// </summary>
        public RawImage ResultPreview;

        [Space(10)]

        /// <summary>
        /// The requested device index dropdown.
        /// </summary>
        public Dropdown RequestedDeviceIndexDropdown;

        /// <summary>
        /// The requested device name dropdown.
        /// </summary>
        public Dropdown RequestedDeviceNameDropdown;

    [Space(10)]
    [Header("Detection")]
    [Tooltip("Show the binary mask instead of the camera preview.")]
    public bool ShowMask = false;

    [Tooltip("Hue low for lower red range (0-179).")]
    public int HueLow1 = 0;
    [Tooltip("Hue high for lower red range (0-179).")]
    public int HueHigh1 = 10;

    [Tooltip("Hue low for upper red range (0-179).")]
    public int HueLow2 = 160;
    [Tooltip("Hue high for upper red range (0-179).")]
    public int HueHigh2 = 179;

    [Tooltip("Saturation low (0-255).")]
    public int SatLow = 100;
    [Tooltip("Saturation high (0-255).")]
    public int SatHigh = 255;

    [Tooltip("Value low (0-255).")]
    public int ValLow = 100;
    [Tooltip("Value high (0-255).")]
    public int ValHigh = 255;

    [Tooltip("Minimum contour area (pixels) to consider as detection.")]
    public double MinContourArea = 1000.0;

        /// <summary>
        /// Set the name of the device to use.
        /// </summary>
        [SerializeField, TooltipAttribute("Set the name of the device to use.")]
        public string RequestedDeviceName = null;

        /// <summary>
        /// Set the width of WebCamTexture.
        /// </summary>
        [SerializeField, TooltipAttribute("Set the width of WebCamTexture.")]
        public int RequestedWidth = 640;

        /// <summary>
        /// Set the height of WebCamTexture.
        /// </summary>
        [SerializeField, TooltipAttribute("Set the height of WebCamTexture.")]
        public int RequestedHeight = 480;

        /// <summary>
        /// Set FPS of WebCamTexture.
        /// </summary>
        [SerializeField, TooltipAttribute("Set FPS of WebCamTexture.")]
        public int RequestedFPS = 30;

        /// <summary>
        /// Set whether to use the front facing camera.
        /// </summary>
        [SerializeField, TooltipAttribute("Set whether to use the front facing camera.")]
        public bool RequestedIsFrontFacing = false;

        // Private Fields
        /// <summary>
        /// The webcam texture.
        /// </summary>
        private WebCamTexture _webCamTexture;

        /// <summary>
        /// The webcam device.
        /// </summary>
        private WebCamDevice _webCamDevice;

        /// <summary>
        /// The rgba mat.
        /// </summary>
        private Mat _rgbaMat;

    // Helper Mats for color detection
    private Mat _rgbMat;
    private Mat _hsvMat;
    private Mat _mask;
    private Mat _mask1;
    private Mat _mask2;
    private Mat _hierarchy;

        /// <summary>
        /// The colors.
        /// </summary>
        private Color32[] _colors;

        /// <summary>
        /// The texture.
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// Indicates whether this instance is waiting for initialization to complete.
        /// </summary>
        private bool _isInitWaiting = false;

        /// <summary>
        /// Indicates whether this instance has been initialized.
        /// </summary>
        private bool _hasInitDone = false;

        /// <summary>
        /// The FPS monitor.
        /// </summary>
        private FpsMonitor _fpsMonitor;

#if ((UNITY_IOS || UNITY_WEBGL) && UNITY_2018_1_OR_NEWER) || (UNITY_ANDROID && UNITY_2018_3_OR_NEWER)
        private bool _isUserRequestingPermission;
#endif

        public GameObject cursour;

        // Unity Lifecycle Methods
        private void Start()
        {
            _fpsMonitor = GetComponent<FpsMonitor>();


            // Retrieves available camera devices and populates dropdown menus with options:
            // one for selecting by device index and another by device name.
            // Adds a default "(EMPTY)" option to indicate no device selected.
            WebCamDevice[] devices = WebCamTexture.devices;

            RequestedDeviceIndexDropdown.ClearOptions();
            var deviceIndexOptions = new List<string>();
            deviceIndexOptions.Add("Device Index (EMPTY)");
            for (int i = 0; i < devices.Length; i++)
            {
                deviceIndexOptions.Add(i.ToString());
            }
            RequestedDeviceIndexDropdown.AddOptions(deviceIndexOptions);

            RequestedDeviceNameDropdown.ClearOptions();
            var deviceNameOptions = new List<string>();
            deviceNameOptions.Add("Device Name (EMPTY)");
            for (int i = 0; i < devices.Length; i++)
            {
                deviceNameOptions.Add(i + ": " + devices[i].name);
            }
            RequestedDeviceNameDropdown.AddOptions(deviceNameOptions);


            Initialize();
        }

        private void Update()
        {
            if (_hasInitDone && _webCamTexture.isPlaying && _webCamTexture.didUpdateThisFrame)
            {
                // Copy webcam frame into _rgbaMat
                OpenCVMatUtils.WebCamTextureToMat(_webCamTexture, _rgbaMat, _colors);

                // Prepare helper mats if necessary
                if (_rgbMat == null) _rgbMat = new Mat();
                if (_hsvMat == null) _hsvMat = new Mat();
                if (_mask == null) _mask = new Mat(_rgbaMat.rows(), _rgbaMat.cols(), CvType.CV_8UC1);
                if (_mask1 == null) _mask1 = new Mat(_rgbaMat.rows(), _rgbaMat.cols(), CvType.CV_8UC1);
                if (_mask2 == null) _mask2 = new Mat(_rgbaMat.rows(), _rgbaMat.cols(), CvType.CV_8UC1);
                if (_hierarchy == null) _hierarchy = new Mat();

                // Convert to HSV
                Imgproc.cvtColor(_rgbaMat, _rgbMat, Imgproc.COLOR_RGBA2RGB);
                Imgproc.cvtColor(_rgbMat, _hsvMat, Imgproc.COLOR_RGB2HSV);

                // Threshold for red (two ranges because hue wraps)
                Scalar lower1 = new Scalar(HueLow1, SatLow, ValLow);
                Scalar upper1 = new Scalar(HueHigh1, SatHigh, ValHigh);
                Scalar lower2 = new Scalar(HueLow2, SatLow, ValLow);
                Scalar upper2 = new Scalar(HueHigh2, SatHigh, ValHigh);

                Core.inRange(_hsvMat, lower1, upper1, _mask1);
                Core.inRange(_hsvMat, lower2, upper2, _mask2);
                Core.add(_mask1, _mask2, _mask);

                // Morphological open to remove noise
                Mat kernel = Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(5, 5));
                Imgproc.morphologyEx(_mask, _mask, Imgproc.MORPH_OPEN, kernel);

                // Find contours
                List<MatOfPoint> contours = new List<MatOfPoint>();
                Imgproc.findContours(_mask, contours, _hierarchy, Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_SIMPLE);

                // Find largest contour above min area
                double maxArea = 0;
                int maxIdx = -1;
                for (int i = 0; i < contours.Count; i++)
                {
                    double area = Imgproc.contourArea(contours[i]);
                    if (area > maxArea && area >= MinContourArea)
                    {
                        maxArea = area;
                        maxIdx = i;
                    }
                }

                if (maxIdx != -1)
                {
                    Moments mu = Imgproc.moments(contours[maxIdx]);
                    if (mu.get_m00() != 0)
                    {
                        double cx = mu.get_m10() / mu.get_m00();
                        double cy = mu.get_m01() / mu.get_m00();

                        // Draw detection marker
                        Imgproc.circle(_rgbaMat, new Point(cx, cy), 10, new Scalar(0, 255, 0, 255), 3);
                        float screenX = Map((float)cx, 0, _rgbaMat.width(), -10, 10);
                        float screenY = Map((float)cy, 0, _rgbaMat.height(), 10, -10);
                        cursour.transform.position = new Vector3(screenX, screenY, 0);
                        CVRect cvRect = Imgproc.boundingRect(contours[maxIdx]);
                        Imgproc.rectangle(_rgbaMat, cvRect.tl(), cvRect.br(), new Scalar(0, 255, 0, 255), 2);
                    }
                }

                // Show mask or original with overlay
                if (ShowMask)
                {
                    Mat rgbaMask = new Mat();
                    Imgproc.cvtColor(_mask, rgbaMask, Imgproc.COLOR_GRAY2RGBA);
                    OpenCVMatUtils.MatToTexture2D(rgbaMask, _texture, _colors);
                    rgbaMask.Dispose();
                }
                else
                {
                    OpenCVMatUtils.MatToTexture2D(_rgbaMat, _texture, _colors);
                }

                // Cleanup
                kernel.Dispose();
                foreach (var c in contours) c.Dispose();
            }
        }

        public float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

        /// <summary>
        /// Simple IMGUI controls to tune detection parameters at runtime.
        /// This avoids creating extra UI objects in the scene and is useful for quick tuning.
        /// </summary>
        /// 
        /*
        private void OnGUI()
        {
            // Small panel in the top-left corner
            GUILayout.BeginArea(new UnityEngine.Rect(10, 10, 280, 420), GUI.skin.box);
            GUILayout.Label("Red Detection (HSV)");

            ShowMask = GUILayout.Toggle(ShowMask, "Show Mask");

            GUILayout.Label($"HueLow1: {HueLow1}");
            HueLow1 = Mathf.RoundToInt(GUILayout.HorizontalSlider(HueLow1, 0, 179));
            GUILayout.Label($"HueHigh1: {HueHigh1}");
            HueHigh1 = Mathf.RoundToInt(GUILayout.HorizontalSlider(HueHigh1, 0, 179));

            GUILayout.Label($"HueLow2: {HueLow2}");
            HueLow2 = Mathf.RoundToInt(GUILayout.HorizontalSlider(HueLow2, 0, 179));
            GUILayout.Label($"HueHigh2: {HueHigh2}");
            HueHigh2 = Mathf.RoundToInt(GUILayout.HorizontalSlider(HueHigh2, 0, 179));

            GUILayout.Label($"SatLow: {SatLow}");
            SatLow = Mathf.RoundToInt(GUILayout.HorizontalSlider(SatLow, 0, 255));
            GUILayout.Label($"SatHigh: {SatHigh}");
            SatHigh = Mathf.RoundToInt(GUILayout.HorizontalSlider(SatHigh, 0, 255));

            GUILayout.Label($"ValLow: {ValLow}");
            ValLow = Mathf.RoundToInt(GUILayout.HorizontalSlider(ValLow, 0, 255));
            GUILayout.Label($"ValHigh: {ValHigh}");
            ValHigh = Mathf.RoundToInt(GUILayout.HorizontalSlider(ValHigh, 0, 255));

            GUILayout.Label($"MinContourArea: {MinContourArea:F0}");
            MinContourArea = GUILayout.HorizontalSlider((float)MinContourArea, 0f, 20000f);

            if (GUILayout.Button("Reset to defaults"))
            {
                HueLow1 = 0; HueHigh1 = 10;
                HueLow2 = 160; HueHigh2 = 179;
                SatLow = 100; SatHigh = 255;
                ValLow = 100; ValHigh = 255;
                MinContourArea = 1000.0;
                ShowMask = false;
            }

            GUILayout.EndArea();
        }
        */

        private void OnDestroy()
        {
            Dispose();
        }

        // Public Methods
        /// <summary>
        /// Raises the back button click event.
        /// </summary>
        public void OnBackButtonClick()
        {
            SceneManager.LoadScene("OpenCVForUnityExample");
        }

        /// <summary>
        /// Raises the play button click event.
        /// </summary>
        public void OnPlayButtonClick()
        {
            if (_hasInitDone)
                _webCamTexture.Play();
        }

        /// <summary>
        /// Raises the pause button click event.
        /// </summary>
        public void OnPauseButtonClick()
        {
            if (_hasInitDone)
                _webCamTexture.Pause();
        }

        /// <summary>
        /// Raises the stop button click event.
        /// </summary>
        public void OnStopButtonClick()
        {
            if (_hasInitDone)
                _webCamTexture.Stop();
        }

        /// <summary>
        /// Raises the change camera button click event.
        /// </summary>
        public void OnChangeCameraButtonClick()
        {
            if (_hasInitDone)
            {
                RequestedDeviceName = null;
                RequestedIsFrontFacing = !RequestedIsFrontFacing;
                Initialize();
            }
        }

        /// <summary>
        /// Raises the requested device index dropdown value changed event.
        /// </summary>
        public void OnRequestedDeviceIndexDropdownValueChanged(int result)
        {
            RequestedDeviceNameDropdown.value = 0;

            int index = result - 1; // Offset the default item
            RequestedDeviceName = (index >= 0) ? index.ToString() : string.Empty;
            Initialize();
        }

        /// <summary>
        /// Raises the requested device name dropdown value changed event.
        /// </summary>
        public void OnRequestedDeviceNameDropdownValueChanged(int result)
        {
            RequestedDeviceIndexDropdown.value = 0;

            WebCamDevice[] devices = WebCamTexture.devices;
            int index = result - 1; // Offset the default item
            RequestedDeviceName = (index >= 0) ? devices[index].name : string.Empty;
            Initialize();
        }

        // Private Methods
        /// <summary>
        /// Initializes webcam texture.
        /// </summary>
        private void Initialize()
        {
            if (_isInitWaiting)
                return;

#if UNITY_ANDROID && !UNITY_EDITOR
            // Set the requestedFPS parameter to avoid the problem of the WebCamTexture image becoming low light on some Android devices (e.g. Google Pixel, Pixel2).
            // https://forum.unity.com/threads/android-webcamtexture-in-low-light-only-some-models.520656/
            // https://forum.unity.com/threads/released-opencv-for-unity.277080/page-33#post-3445178
            if (RequestedIsFrontFacing)
            {
                int rearCameraFPS = RequestedFPS;
                RequestedFPS = 15;
                StartCoroutine(_Initialize());
                RequestedFPS = rearCameraFPS;
            }
            else
            {
                StartCoroutine(_Initialize());
            }
#else
            StartCoroutine(_Initialize());
#endif
        }

        /// <summary>
        /// Initializes webcam texture by coroutine.
        /// </summary>
        private IEnumerator _Initialize()
        {
            if (_hasInitDone)
                Dispose();

            _isInitWaiting = true;

            // Checks camera permission state.
#if (UNITY_IOS || UNITY_WEBGL) && UNITY_2018_1_OR_NEWER
            UserAuthorization mode = UserAuthorization.WebCam;
            if (!Application.HasUserAuthorization(mode))
            {
                _isUserRequestingPermission = true;
                yield return Application.RequestUserAuthorization(mode);

                float timeElapsed = 0;
                while (_isUserRequestingPermission)
                {
                    if (timeElapsed > 0.25f)
                    {
                        _isUserRequestingPermission = false;
                        break;
                    }
                    timeElapsed += Time.deltaTime;

                    yield return null;
                }
            }

            if (!Application.HasUserAuthorization(mode))
            {
                if (_fpsMonitor != null)
                {
                    _fpsMonitor.ConsoleText = "Camera permission is denied.";
                }
                _isInitWaiting = false;
                yield break;
            }
#elif UNITY_ANDROID && UNITY_2018_3_OR_NEWER
            string permission = UnityEngine.Android.Permission.Camera;
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(permission))
            {
                _isUserRequestingPermission = true;
                UnityEngine.Android.Permission.RequestUserPermission(permission);

                float timeElapsed = 0;
                while (_isUserRequestingPermission)
                {
                    if (timeElapsed > 0.25f)
                    {
                        _isUserRequestingPermission = false;
                        break;
                    }
                    timeElapsed += Time.deltaTime;

                    yield return null;
                }
            }

            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(permission))
            {
                if (_fpsMonitor != null)
                {
                    _fpsMonitor.ConsoleText = "Camera permission is denied.";
                }
                _isInitWaiting = false;
                yield break;
            }
#endif

            // Creates a WebCamTexture with settings closest to the requested name, resolution, and frame rate.
            var devices = WebCamTexture.devices;
            if (devices.Length == 0)
            {
                Debug.LogError("Camera device does not exist.");
                _isInitWaiting = false;
                yield break;
            }

            if (!String.IsNullOrEmpty(RequestedDeviceName))
            {
                // Try to parse requestedDeviceName as an index
                int requestedDeviceIndex = -1;
                if (Int32.TryParse(RequestedDeviceName, out requestedDeviceIndex))
                {
                    if (requestedDeviceIndex >= 0 && requestedDeviceIndex < devices.Length)
                    {
                        _webCamDevice = devices[requestedDeviceIndex];
                        _webCamTexture = new WebCamTexture(_webCamDevice.name, RequestedWidth, RequestedHeight, RequestedFPS);
                    }
                }
                else
                {
                    // Search for a device with a matching name
                    for (int cameraIndex = 0; cameraIndex < devices.Length; cameraIndex++)
                    {
                        if (devices[cameraIndex].name == RequestedDeviceName)
                        {
                            _webCamDevice = devices[cameraIndex];
                            _webCamTexture = new WebCamTexture(_webCamDevice.name, RequestedWidth, RequestedHeight, RequestedFPS);
                            break;
                        }
                    }
                }
                if (_webCamTexture == null)
                    Debug.Log("Cannot find camera device " + RequestedDeviceName + ".");
            }

            if (_webCamTexture == null)
            {
                var prioritizedKinds = new WebCamKind[]
                {
                    WebCamKind.WideAngle,
                    WebCamKind.Telephoto,
                    WebCamKind.UltraWideAngle,
                    WebCamKind.ColorAndDepth
                };

                // Checks how many and which cameras are available on the device
                foreach (var kind in prioritizedKinds)
                {
                    foreach (var device in devices)
                    {
                        if (device.kind == kind && device.isFrontFacing == RequestedIsFrontFacing)
                        {
                            _webCamDevice = device;
                            _webCamTexture = new WebCamTexture(_webCamDevice.name, RequestedWidth, RequestedHeight, RequestedFPS);
                            break;
                        }
                    }
                    if (_webCamTexture != null) break;
                }
            }

            if (_webCamTexture == null)
            {
                _webCamDevice = devices[0];
                _webCamTexture = new WebCamTexture(_webCamDevice.name, RequestedWidth, RequestedHeight, RequestedFPS);
            }

            // Starts the camera.
            _webCamTexture.Play();

            while (true)
            {
                if (_webCamTexture.didUpdateThisFrame)
                {
                    Debug.Log("name:" + _webCamTexture.deviceName + " width:" + _webCamTexture.width + " height:" + _webCamTexture.height + " fps:" + _webCamTexture.requestedFPS);
                    Debug.Log("videoRotationAngle:" + _webCamTexture.videoRotationAngle + " videoVerticallyMirrored:" + _webCamTexture.videoVerticallyMirrored + " isFrongFacing:" + _webCamDevice.isFrontFacing);

                    _isInitWaiting = false;
                    _hasInitDone = true;

                    OnInited();

                    break;
                }
                else
                {
                    yield return null;
                }
            }
        }

#if ((UNITY_IOS || UNITY_WEBGL) && UNITY_2018_1_OR_NEWER) || (UNITY_ANDROID && UNITY_2018_3_OR_NEWER)
        private IEnumerator OnApplicationFocus(bool hasFocus)
        {
            yield return null;

            if (_isUserRequestingPermission && hasFocus)
                _isUserRequestingPermission = false;
        }
#endif

        /// <summary>
        /// Releases all resource.
        /// </summary>
        private void Dispose()
        {
            _isInitWaiting = false;
            _hasInitDone = false;

            if (_webCamTexture != null)
            {
                _webCamTexture.Stop();
                WebCamTexture.Destroy(_webCamTexture);
                _webCamTexture = null;
            }
            _rgbaMat?.Dispose(); _rgbaMat = null;
            _rgbMat?.Dispose(); _rgbMat = null;
            _hsvMat?.Dispose(); _hsvMat = null;
            _mask?.Dispose(); _mask = null;
            _mask1?.Dispose(); _mask1 = null;
            _mask2?.Dispose(); _mask2 = null;
            _hierarchy?.Dispose(); _hierarchy = null;

            if (_texture != null) Texture2D.Destroy(_texture); _texture = null;
        }

        /// <summary>
        /// Raises the webcam texture initialized event.
        /// </summary>
        private void OnInited()
        {
            if (_colors == null || _colors.Length != _webCamTexture.width * _webCamTexture.height)
                _colors = new Color32[_webCamTexture.width * _webCamTexture.height];
            if (_texture == null || _texture.width != _webCamTexture.width || _texture.height != _webCamTexture.height)
                _texture = new Texture2D(_webCamTexture.width, _webCamTexture.height, TextureFormat.RGBA32, false);

            _rgbaMat = new Mat(_webCamTexture.height, _webCamTexture.width, CvType.CV_8UC4, new Scalar(0, 0, 0, 255));
            OpenCVMatUtils.MatToTexture2D(_rgbaMat, _texture, _colors);

            // Allocate helper mats for color detection
            _rgbMat = new Mat(_rgbaMat.rows(), _rgbaMat.cols(), CvType.CV_8UC3);
            _hsvMat = new Mat(_rgbaMat.rows(), _rgbaMat.cols(), CvType.CV_8UC3);
            _mask = new Mat(_rgbaMat.rows(), _rgbaMat.cols(), CvType.CV_8UC1);
            _mask1 = new Mat(_rgbaMat.rows(), _rgbaMat.cols(), CvType.CV_8UC1);
            _mask2 = new Mat(_rgbaMat.rows(), _rgbaMat.cols(), CvType.CV_8UC1);
            _hierarchy = new Mat();

            ResultPreview.texture = _texture;
            ResultPreview.GetComponent<AspectRatioFitter>().aspectRatio = (float)_texture.width / _texture.height;


            if (_fpsMonitor != null)
            {
                _fpsMonitor.Add("deviceName", _webCamDevice.name.ToString());
                if (_webCamDevice.depthCameraName != null)
                    _fpsMonitor.Add("depthCameraName", _webCamDevice.depthCameraName.ToString());
                _fpsMonitor.Add("kind", _webCamDevice.kind.ToString());
                _fpsMonitor.Add("isFrontFacing", _webCamDevice.isFrontFacing.ToString());
                _fpsMonitor.Add("isAutoFocusPointSupported", _webCamDevice.isAutoFocusPointSupported.ToString());
                _fpsMonitor.Add("width", _rgbaMat.width().ToString());
                _fpsMonitor.Add("height", _rgbaMat.height().ToString());
                _fpsMonitor.Add("videoRotationAngle", _webCamTexture.videoRotationAngle.ToString());
                _fpsMonitor.Add("videoVerticallyMirrored", _webCamTexture.videoVerticallyMirrored.ToString());

                if (_webCamDevice.availableResolutions != null)
                {
                    _fpsMonitor.Add("availableResolutions", "[" + _webCamDevice.availableResolutions.Length.ToString() + "]");
                    var resolutions = _webCamDevice.availableResolutions;
                    for (int i = 0; i < resolutions.Length; i++)
                    {
                        _fpsMonitor.Add(" " + i, resolutions[i].ToString());
                    }
                }

                _fpsMonitor.Add("orientation", Screen.orientation.ToString());
            }
        }
    }
}

#endif
