Color Tracking (Red Detection)

This script extends the example `ColorTracking.cs` with HSV-based red color detection and a small runtime IMGUI panel for tuning.

How to use

1. Attach `ColorTracking.cs` to a GameObject in your scene (the example scene should already use it).
2. Assign `ResultPreview` to a `RawImage` UI element so you can see the camera preview.
3. Press Play.
4. Use the small control panel in the top-left corner to adjust HSV thresholds and `MinContourArea`.
   - `Show Mask` will display the binary mask used for detection.
   - Use `Reset to defaults` to restore recommended starting values.

Tuning tips

- Red wraps around the hue scale. Two hue ranges (low and high) are used to capture both ends of the hue circle.
- If the object is dim or lighting is poor, lower `ValLow` and/or `SatLow`.
- Increase `MinContourArea` to ignore small noisy blobs.
- If detection is jittery, consider smoothing the centroid or applying a larger morphological kernel.

Notes

- The IMGUI panel is intended for quick tuning. For production or nicer UX, create a proper Unity UI (sliders, labels) and hook them to the public fields.
- The script uses OpenCVForUnity and requires that package to be installed and configured in the project.
