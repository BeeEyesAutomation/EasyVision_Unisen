# Camera Calibration Dependency and Flow Tree

## Purpose

This document is the detailed dependency/processing tree for the standalone camera calibration module. It records the intended flow before implementation starts, so native, C# core, UI, and future inspection integration stay separated.

## Dependency Tree

```text
CameraCalibration standalone module
|
+-- Native BeeCpp layer
|   |
|   +-- CameraCalibrationCore
|   |   |
|   |   +-- detect calibration grid
|   |   |   +-- chessboard inner corners
|   |   |   +-- symmetric circle grid
|   |   |   +-- asymmetric circle grid
|   |   |
|   |   +-- solve lens calibration
|   |   |   +-- camera matrix 3x3
|   |   |   +-- distortion coefficients
|   |   |   +-- image size
|   |   |   +-- reprojection error
|   |   |   +-- per-frame residuals
|   |   |
|   |   +-- build correction model
|   |   |   +-- undistort maps
|   |   |   +-- optional planar homography
|   |   |   +-- preview remap output
|   |   |
|   |   +-- automatic scale detection
|   |   |   +-- known sample detector
|   |   |   +-- measured pixel span/diameter/edge distance
|   |   |   +-- mm-per-pixel result
|   |   |
|   |   +-- live guidance diagnostics
|   |       +-- pattern found/not found
|   |       +-- pose/tilt estimate
|   |       +-- operator direction hint data
|   |       +-- optional residual vectors for debug view
|   |
|   +-- CameraCalibrationCli
|       |
|       +-- managed input DTOs
|       +-- managed result DTOs
|       +-- cloned debug image buffers
|       +-- no unmanaged buffer escape without ownership rule
|
+-- C# core layer
|   |
|   +-- BeeGlobal.CameraCalibrationConfig
|   |   |
|   |   +-- standalone serializable calibration settings
|   |       |
|   |       +-- ActiveProfileId
|   |       +-- ApplyToPreview
|   |       +-- ApplyToInspectionInput (future/off by default)
|   |       +-- Profiles[]
|   |           |
|   |           +-- CameraCalibrationProfile
|   |               |
|   |               +-- ProfileId
|   |               +-- CameraId
|   |               +-- CameraName
|   |               +-- ImageWidth/ImageHeight
|   |               +-- Lens/working-distance metadata
|   |               +-- Pattern type, rows, columns, spacingMm
|   |               +-- CameraMatrix[9]
|   |               +-- DistortionCoefficients[]
|   |               +-- RectificationHomography[9] (optional)
|   |               +-- MeanReprojectionError
|   |               +-- PerFrameErrors[]
|   |               +-- ScaleMmPerPixel
|   |               +-- ScaleSampleRealSizeMm
|   |               +-- ScaleSamplePixelSize
|   |               +-- CreatedAt/UpdatedAt
|   |               +-- IsValidatedForPreview
|   |               +-- IsValidatedForInspection
|   |
|   +-- BeeGlobal.Global
|   |   |
|   |   +-- static CameraCalibrationConfig CameraCalibration
|   |
|   +-- BeeCore.Data persistence
|   |   |
|   |   +-- Access.SaveCameraCalibration(path, CameraCalibrationConfig)
|   |   +-- Access.LoadCameraCalibration(path)
|   |   +-- SaveData.CameraCalibration(Global.CameraCalibration)
|   |   +-- LoadData.CameraCalibration()
|   |   +-- storage: Common\CameraCalibration.config
|   |
|   +-- CameraCalibrationService
|   |   |
|   |   +-- capture frame metadata
|   |   +-- call BeeCpp CLI solve/detect methods
|   |   +-- validate profile against camera/resolution
|   |   +-- update Global.CameraCalibration profile list
|   |
|   +-- CameraPreviewCorrection
|   |   |
|   |   +-- read active profile from Global.CameraCalibration
|   |   +-- lazily build/reuse correction maps
|   |   +-- correct preview frame only
|   |
|   +-- CameraScaleCalibrationService
|       |
|       +-- call automatic sample detector
|       +-- compute/store mm-per-pixel
|       +-- update Global.CameraCalibration scale fields first
|       +-- sync Global.Config.Scale only after explicit operator apply/save
|
+-- WinForms UI layer
|   |
|   +-- standalone calibration form/control
|   |   |
|   |   +-- camera/profile selector
|   |   +-- calibration pattern settings
|   |   +-- frame capture list and quality state
|   |   +-- solve/reprojection summary
|   |   +-- before/after preview
|   |   +-- automatic scale sample detection
|   |   +-- simple live alignment guidance
|   |
|   +-- settings entry point
|       |
|       +-- SettingStep1 or GeneralSetting launches standalone UI only
|       +-- live frames are requested through BeeCore/Func/Camera.cs only
|       +-- UI never calls camera SDK/read loops directly
|
+-- Future inspection integration layer
    |
    +-- opt-in global switch
    |   +-- ApplyToInspectionInput defaults false
    |   +-- existing tools keep raw frame until validated
    |
    +-- frame source branch
    |   +-- raw frame path remains available
    |   +-- corrected frame path created by shared service
    |   +-- each tool can later declare raw/corrected compatibility
    |
    +-- ROI/coordinate branch
    |   +-- legacy ROI remains in raw coordinates by default
    |   +-- corrected-coordinate migration requires explicit validation
    |   +-- mapping helpers needed before automatic migration
    |
    +-- scale branch
        +-- Global.Config.Scale can be updated from calibration
        +-- per-tool scale overrides remain respected where already supported
        +-- corrected scale is valid only on calibrated plane
```

## C++ Implementation Tree

Follow the existing `Pattern/*Core` + `Pattern/*Cli` style. Keep native math in `BeeCpp` and the managed wrapper in the existing C++/CLI namespace style already used by `RansacLineCli` and `PinPitchCli`.

```text
Pattern/
|
+-- CameraCalibrationCore.h
|   |
|   +-- enum class CalibrationPatternType
|   |   +-- Chessboard
|   |   +-- SymmetricCircleGrid
|   |   +-- AsymmetricCircleGrid
|   |
|   +-- enum class CalibrationGuidanceAction
|   |   +-- Unknown
|   |   +-- Ok
|   |   +-- TargetNotFound
|   |   +-- RotateLeft
|   |   +-- RotateRight
|   |   +-- TiltUp
|   |   +-- TiltDown
|   |   +-- MoveCloser
|   |   +-- MoveFarther
|   |   +-- NotPerpendicular
|   |
|   +-- struct CalibrationGridSpec
|   |   +-- patternType
|   |   +-- rows
|   |   +-- columns
|   |   +-- spacingMm
|   |   +-- useFastCheck
|   |
|   +-- struct CalibrationFrameCore
|   |   +-- found
|   |   +-- imageWidth/imageHeight
|   |   +-- imagePoints
|   |   +-- objectPoints
|   |   +-- coverageRect
|   |   +-- qualityScore
|   |   +-- message
|   |   +-- debugBGR
|   |
|   +-- struct CameraCalibrationProfileCore
|   |   +-- success
|   |   +-- imageWidth/imageHeight
|   |   +-- cameraMatrix 3x3
|   |   +-- distortionCoefficients
|   |   +-- rectificationHomography 3x3 optional
|   |   +-- meanReprojectionError
|   |   +-- perFrameErrors
|   |   +-- residualVectors
|   |   +-- message
|   |
|   +-- struct ScaleCalibrationCore
|   |   +-- found
|   |   +-- realSizeMm
|   |   +-- pixelSize
|   |   +-- mmPerPixel
|   |   +-- confidence
|   |   +-- sampleBox
|   |   +-- debugBGR
|   |
|   +-- struct LiveGuidanceCore
|   |   +-- found
|   |   +-- action
|   |   +-- score
|   |   +-- tiltXDeg/tiltYDeg/rotationDeg
|   |   +-- messageCode
|   |   +-- debugBGR optional
|   |
|   +-- class CameraCalibrationCore
|       +-- DetectGrid(...)
|       +-- Solve(...)
|       +-- UndistortPreview(...)
|       +-- DetectScaleSample(...)
|       +-- AnalyzeLiveGuidance(...)
|
+-- CameraCalibrationCore.cpp
|   |
|   +-- ConvertToGray
|   +-- BuildObjectPoints
|   +-- DetectChessboard
|   +-- DetectCircleGrid
|   +-- ScoreGridCoverage
|   +-- DrawGridDebug
|   +-- ComputeReprojectionErrors
|   +-- BuildRectificationHomography optional
|   +-- DetectKnownScaleSample
|   +-- ClassifyGuidanceAction
|
+-- CameraCalibrationCli.h
|   |
|   +-- managed enums mirroring native enums
|   +-- value/ref result DTOs with arrays for matrices/vectors
|   +-- ref class CameraCalibration
|       +-- DetectGrid(IntPtr imageData, ...)
|       +-- Solve(array<CalibrationFrameCli^>^ frames, ...)
|       +-- UndistortPreview(IntPtr inputData, IntPtr outputData, ...)
|       +-- DetectScaleSample(IntPtr imageData, ...)
|       +-- AnalyzeLiveGuidance(IntPtr imageData, ...)
|
+-- CameraCalibrationCli.cpp
|   |
|   +-- Mat view construction from IntPtr/width/height/stride/type
|   +-- Native-to-managed result mapping
|   +-- Managed array cloning for matrices, distortion, errors, points
|   +-- Debug BGR copy into caller-owned or managed buffer
|
+-- Pattern.vcxproj
    |
    +-- add CameraCalibrationCore.h/cpp
    +-- add CameraCalibrationCli.h/cpp
```

## C++ Step Plan

### CC-001 Native Core Steps

1. Add `CameraCalibrationCore.h` with only native structs/enums/class declarations. Keep all fields serialization-neutral and use `std::vector<double>`, `std::vector<cv::Point2f>`, `cv::Mat`, and simple primitives.
2. Add `CameraCalibrationCore.cpp` with utility functions first: gray conversion, object point generation, coverage scoring, and debug drawing.
3. Implement `DetectGrid(...)`:
   - Chessboard: use OpenCV chessboard corner detection and subpixel refinement.
   - Symmetric/asymmetric circle grid: use OpenCV circle-grid detection.
   - Return `found=false` with a short message when the target is missing; do not throw for normal detection failure.
4. Implement `Solve(...)`:
   - Require at least the minimum accepted frames.
   - Call OpenCV calibration using accepted image/object points.
   - Compute per-frame reprojection errors and mean error.
   - Store camera matrix and distortion coefficients in the profile result.
5. Implement `UndistortPreview(...)`:
   - Accept source `cv::Mat`, profile matrix/distortion, and destination size.
   - Use map/remap or undistort path that preserves frame size for preview.
   - Do not change inspection input here.
6. Implement `DetectScaleSample(...)`:
   - Input is the current preview/corrected frame and real sample size in mm.
   - Return sample pixel size, confidence, and debug overlay.
   - Keep first version conservative; if sample is ambiguous, return `found=false`.
7. Implement `AnalyzeLiveGuidance(...)`:
   - Reuse grid or known planar target detection.
   - Compute simple pose/tilt metrics.
   - Return one `CalibrationGuidanceAction`, not a long list of advice.
8. Update `Pattern.vcxproj` to include the new native files.
9. Verify `Pattern.vcxproj` Release x64 only after all CC-001 native files are added.

### CC-002 C++/CLI Wrapper Steps

1. Add `CameraCalibrationCli.h` after native structs are stable.
2. Define managed enums and DTOs with primitive fields and arrays:
   - Matrix arrays are `array<double>^` length 9.
   - Distortion coefficients are `array<double>^`.
   - Points/errors use managed arrays or compact value structs.
3. Add `CameraCalibrationCli.cpp` mapping helpers:
   - Validate input pointer, width, height, stride, and type.
   - Wrap input buffers as non-owning `cv::Mat` views.
   - Clone/copy debug images before returning to C#.
4. Expose only minimal methods needed by C#:
   - `DetectGrid`
   - `Solve`
   - `UndistortPreview`
   - `DetectScaleSample`
   - `AnalyzeLiveGuidance`
5. Keep unmanaged ownership simple:
   - Do not return raw native pointers to C#.
   - C# owns any managed arrays returned by CLI.
   - Caller-owned output buffers must be explicitly documented in method names/parameters.
6. Update `Pattern.vcxproj` to include CLI files.
7. Verify `Pattern.vcxproj` Release x64, then continue to CC-003 only after the wrapper compiles.

## C++ Non-Goals

- Do not modify existing `RansacLine*`, `PinPitch*`, `Pitch*`, `Pattern2*`, or barcode code for calibration.
- Do not change existing C++/CLI signatures.
- Do not add camera SDK calls to `Pattern/`; native calibration receives image buffers only.
- Do not add AI model calls or network calls.
- Do not implement inspection pipeline correction in CC-001 or CC-002.

## C# Implementation Tree

Use this tree for CC-003 and CC-004. Do not create different names unless the compiler or project structure forces it.

```text
BeeGlobal/
|
+-- CameraCalibrationConfig.cs
|   |
|   +-- [Serializable] CameraCalibrationConfig
|   |   +-- string ActiveProfileId
|   |   +-- bool ApplyToPreview
|   |   +-- bool ApplyToInspectionInput
|   |   +-- List<CameraCalibrationProfile> Profiles
|   |   +-- CameraCalibrationProfile GetActiveProfile()
|   |   +-- CameraCalibrationProfile FindProfile(cameraId, width, height)
|   |
|   +-- [Serializable] CameraCalibrationProfile
|   |   +-- string ProfileId
|   |   +-- string CameraId
|   |   +-- string CameraName
|   |   +-- int ImageWidth/ImageHeight
|   |   +-- int PatternType
|   |   +-- int PatternRows/PatternColumns
|   |   +-- double PatternSpacingMm
|   |   +-- double[] CameraMatrix
|   |   +-- double[] DistortionCoefficients
|   |   +-- double[] RectificationHomography
|   |   +-- double MeanReprojectionError
|   |   +-- double[] PerFrameErrors
|   |   +-- double ScaleMmPerPixel
|   |   +-- double ScaleSampleRealSizeMm
|   |   +-- double ScaleSamplePixelSize
|   |   +-- DateTime CreatedAt/UpdatedAt
|   |   +-- bool IsValidatedForPreview
|   |   +-- bool IsValidatedForInspection
|
+-- Global.cs
    |
    +-- public static CameraCalibrationConfig CameraCalibration

BeeCore/Data/
|
+-- Access.cs
|   +-- SaveCameraCalibration(path, CameraCalibrationConfig)
|   +-- LoadCameraCalibration(path)
|
+-- LoadData.cs
|   +-- CameraCalibration()
|
+-- SaveData.cs
    +-- CameraCalibration(CameraCalibrationConfig config)

BeeCore/Func/
|
+-- CameraCalibrationProfileStore.cs
|   +-- GetActive()
|   +-- Upsert(profile)
|   +-- Save()
|
+-- CameraCalibrationService.cs
|   +-- DetectGrid(frame)
|   +-- Solve(frames)
|   +-- ValidateProfileForFrame(profile, width, height, cameraId)
|
+-- CameraPreviewCorrection.cs
|   +-- TryCorrectPreview(frame, profile, out corrected)
|   +-- cache maps per profile/resolution
|
+-- CameraScaleCalibrationService.cs
    +-- DetectScaleSample(frame, realSizeMm)
    +-- ApplyScaleToProfile(profile, scaleResult)
    +-- ApplyScaleToGlobalConfig(profile)
```

## C# Step Plan

### CC-003 Standalone Calibration State Steps

1. Create `BeeGlobal/CameraCalibrationConfig.cs`.
2. Add serializable fields only; avoid OpenCvSharp, Bitmap, Mat, or non-serializable references in BeeGlobal classes.
3. Add `Global.CameraCalibration` in `BeeGlobal/Global.cs`.
4. Add project entry in `BeeGlobal/BeeGlobal.csproj`.
5. Add `Access.SaveCameraCalibration(...)` and `Access.LoadCameraCalibration(...)` using the existing Base64 helper pattern.
6. Add `LoadData.CameraCalibration()` with missing-file default.
7. Add `SaveData.CameraCalibration(...)` writing `Common\\CameraCalibration.config`.
8. Add BeeCore service/store files only after persistence compiles.
9. Verify `BeeGlobal`, then `BeeCore`.

### CC-004 Scale Service Steps

1. Extend native/CLI scale result only if CC-001/CC-002 did not already expose the needed fields.
2. Add `CameraScaleCalibrationService` method that accepts current preview frame plus real size in mm.
3. Store detected scale in the active `CameraCalibrationProfile`.
4. Add explicit apply method that copies `profile.ScaleMmPerPixel` into `Global.Config.Scale`.
5. Do not update `Global.Config.Scale` from detection alone.
6. Verify only projects touched by the chosen implementation.

## UI Implementation Tree

Use this tree for CC-005 and CC-006. Keep the first UI standalone.

```text
BeeInterface/
|
+-- Calibration/
|   |
|   +-- FormCameraCalibration.cs
|   +-- FormCameraCalibration.Designer.cs
|   +-- CameraCalibrationPreviewPanel.cs
|   +-- CameraCalibrationGuidePanel.cs
|   +-- CameraCalibrationCaptureList.cs
|
+-- Steps/SettingStep1.cs
|   |
|   +-- open standalone calibration form
|
+-- GeneralSetting.cs
    |
    +-- optional entry point only if SettingStep1 is not the right settings surface
```

## UI Step Plan

### CC-005 Standalone UI Steps

1. Add a standalone form/control with no pipeline integration.
2. Add a settings entry button that opens the form.
3. Bind profile list from `Global.CameraCalibration`.
4. Add pattern settings: type, rows, columns, spacing mm.
5. Add capture list: accepted/rejected/status/error.
6. Request frames through `Camera.cs` path; do not call SDK directly.
7. Call C# calibration service for detect/solve.
8. Show before/after preview.
9. Save through `SaveData.CameraCalibration`.
10. Verify `BeeInterface`.

### CC-006 Live Guidance Steps

1. Add a timer/throttle independent from live preview FPS.
2. Use latest frame supplied by camera/view path.
3. Call guidance service.
4. Display one primary action.
5. Show "target not found" when no pattern/sample is detected.
6. Keep residual/vector overlay behind an optional debug toggle.
7. Verify preview stays responsive before adding any extra UI.

## Calibration Solve Flow

```text
Operator opens standalone calibration UI
|
+-- select camera/profile slot
+-- choose grid type and physical spacing
+-- capture N frames
|   |
|   +-- frame sent to BeeCpp grid detector
|   +-- detector returns found points + quality/debug overlay
|   +-- UI marks frame accepted/rejected
|
+-- solve profile
|   |
|   +-- BeeCpp calibrateCamera-style solve
|   +-- return full matrix/distortion/profile data
|   +-- C# validates resolution/camera identity
|   +-- UI shows reprojection score and preview result
|
+-- operator saves
    |
    +-- store full profile in Global.CameraCalibration
    +-- SaveData.CameraCalibration(Global.CameraCalibration)
    +-- profile becomes available for preview correction
```

## Preview Correction Flow

Detailed preview-only tasks are tracked in `preview-map.md`.

```text
Camera live frame
|
+-- raw frame remains original source of truth
|
+-- if Global.CameraCalibration.ApplyToPreview
|   |
|   +-- CameraPreviewCorrection checks active profile
|   +-- reject profile if camera ID/resolution mismatch
|   +-- build/reuse undistort maps
|   +-- produce corrected preview frame
|   +-- UI displays before/after or corrected-only preview
|
+-- inspection tools still receive raw frame in first implementation
```

## Automatic Scale Flow

```text
Operator enters known sample real size in mm
|
+-- UI captures or uses current corrected preview frame
+-- BeeCpp detects known sample automatically
|   |
|   +-- sample found
|   |   +-- compute pixel size
|   |   +-- compute mm-per-pixel
|   |   +-- return confidence/debug overlay
|   |
|   +-- sample not found
|       +-- UI shows simple retry guidance
|
+-- operator saves scale
    |
    +-- store scale data in active calibration profile
    +-- save Global.CameraCalibration separately
    +-- update Global.Config.Scale only after explicit apply/save
```

## Live Guidance Flow

```text
Live guidance enabled
|
+-- throttle sampling rate separate from preview FPS
+-- capture latest preview frame
+-- BeeCpp detects grid/known planar target
|
+-- if target found
|   |
|   +-- estimate pose/tilt
|   +-- classify simple direction hint
|   +-- UI shows one primary action:
|       +-- OK
|       +-- rotate left/right
|       +-- tilt up/down
|       +-- move closer/farther
|       +-- camera not perpendicular
|
+-- if target not found
    |
    +-- UI shows "target not found" and suggests placing the grid/sample
    +-- optional debug view can show residual/vector details when available
```

## Future Integration Flow

Future integration must be explicit and reversible:

```text
Inspection trigger/read
|
+-- camera produces raw Mat
|
+-- raw branch
|   |
|   +-- existing tools continue unchanged
|   +-- legacy ROI and shape coordinates remain valid
|
+-- corrected branch (future opt-in)
    |
    +-- validate active profile
    +-- correct frame through shared service
    +-- expose corrected frame to compatible tools only
    +-- use coordinate mapping helper for ROI migration/preview overlays
    +-- use calibration scale only on calibrated plane
```

## Stop Conditions For Implementation

- If the profile cannot be serialized cleanly in the separate `CameraCalibrationConfig` file, stop and redesign the storage shape before writing UI.
- If preview correction changes frame dimensions unexpectedly, stop before connecting to any inspection branch.
- If automatic sample detection is unstable on real images, keep manual `Global.Config.Scale` as fallback and mark scale automation partial.
- If live guidance adds visible preview lag, throttle diagnostics further before adding more UI.

## Token-Efficient Implementation Rules

- Start from the selected `CC-xxx` row, then read only the files named in that row.
- Use existing project patterns before inventing new abstractions.
- Keep each patch tied to one responsibility: native core, CLI wrapper, persistence, preview service, scale service, UI, or future integration.
- Do not inspect unrelated tools unless the selected entry touches future inspection integration.
- Do not scan the full repository for every step. Search only for symbols needed by the current file scope.
- Verify once at the end of the current batch with the smallest project/build that can prove the changed layer works.
- If verification fails, fix the directly related file named by the error first. Expand investigation only when the direct fix is not enough.
- Keep notes short in `.codex-session.md`: completed ID, result, changed files, next ID, blocker.
- If implementation diverges from this tree, update this file in the same session and explain the reason in one sentence.
