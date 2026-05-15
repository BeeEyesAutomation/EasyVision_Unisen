# Plan - Camera Calibration, Scale, and Live Distortion Guidance

## Goal

Add a camera calibration workflow that can be used from C# while keeping heavy image processing in BeeCpp:

- Calibrate lens distortion and perspective using square chessboard or circular calibration grid images.
- Undistort or rectify live camera frames for operator preview first.
- Calibrate pixel-to-mm scale by automatically detecting a known physical sample.
- Show simple live distortion/pose guidance so operators can adjust camera angle and working direction before production runs.

## Confirmed Decisions

- Build this as an independent module for all tools, not as logic owned by any single inspection tool.
- Persist the full calibration profile in a separate serializable class and global variable, not inside `Global.Config`.
- Add a dedicated global variable such as `Global.CameraCalibration` backed by a separate load/save path.
- First implementation is preview-first: show corrected/flattened image to help the operator align the camera. Do not feed corrected frames into all tools until the standalone module is validated.
- Scale calibration should detect the known sample automatically. Manual input should only be the real-world sample size in mm and optional detection settings.
- Live guidance UI should stay simple: direction/angle hints for the operator, not an engineering-heavy residual dashboard as the primary UI.
- Keep the feature separated as much as possible at first; integrate deeper into the software only after the standalone workflow is stable.

## Design Position

BeeCpp should own OpenCV-heavy calibration, sample detection, remap, and diagnostic math. C# should own standalone calibration persistence, runtime selection, camera lifecycle orchestration, and WinForms interaction.

The detailed dependency tree and processing flows are captured in `tree.md`. That file is the source of truth for implementation order, profile contents, preview flow, automatic scale flow, live guidance flow, and future inspection integration branches.

The feature should be delivered in slices because it crosses native C++, C++/CLI, C# core, camera live preview, and operator UI:

| Slice | Owner | Purpose |
| --- | --- | --- |
| CC-001 | BeeCpp C++ | Detect square/circle calibration grids and compute camera matrix, distortion coefficients, reprojection error, and per-point residuals. |
| CC-002 | BeeCpp C++ / C++/CLI | Expose calibration, undistort, rectify, and diagnostic outputs through a stable managed wrapper. |
| CC-003 | C# core | Persist global calibration profiles in `Global.CameraCalibration` through separate load/save APIs and expose preview-only correction service without changing camera SDK lifecycle. |
| CC-004 | C# scale calibration | Convert pixel distance to mm from an automatically detected known sample size. |
| CC-005 | WinForms UI | Add a standalone operator wizard for grid capture, solve, save, scale calibration, and preview comparison. |
| CC-006 | Live UI guidance | Add simple direction guidance for camera angle and working distance adjustment. |
| CC-007 | C# integration | After standalone validation, optionally apply corrected frames/scale to inspection tools. |

## Execution Constraints

Use these constraints to keep future implementation focused and token-efficient:

- Read only these three planning files before starting a calibration task unless a build error or compile reference requires more context: `Plan.md`, `tree.md`, and `camera-calibration-map.md`.
- Read `preview-map.md` only for preview-related work: CC-003 preview service, CC-005 standalone UI, CC-006 live guidance, or CC-007 corrected-frame integration.
- Treat `tree.md` as the source of truth for dependency order and flow. Do not re-plan architecture unless a listed stop condition is hit.
- Work one `CC-xxx` entry at a time. Do not implement future integration while working on standalone preview/calibration entries.
- Keep changes focused on the selected `CC-xxx` decision/API. If a directly related compile file must be touched, fix it without asking and note it in the session log.
- Do not create extra design docs, diagrams, or alternative trees. Update `Plan.md`, `tree.md`, or `camera-calibration-map.md` only when a real decision changes.
- Do not re-ask confirmed decisions: standalone module, separate `Global.CameraCalibration`, separate save/load, preview-first, automatic scale detection, simple guidance UI, and future integration opt-in.
- Prefer deterministic OpenCV/BeeCpp image processing for runtime behavior. Do not add AI model calls to live camera, calibration solve, scale measurement, or production inspection.
- Live camera access must go through the existing `BeeCore/Func/Camera.cs` abstraction. UI and calibration forms must not call camera SDK/read loops directly.
- Keep UI text and operator flow simple. Show one main guidance action at a time; keep residual/vector details as optional debug output.
- If a small implementation detail is missing, choose the option that matches `tree.md` and existing repo patterns instead of asking. Ask only for stop conditions or risky behavior changes.
- Build only when AGENTS build rules require it. Markdown-only plan updates must not run MSBuild.
- Run the minimum verification for the current `CC-xxx` batch at the end of that batch. Do not test after every small step.
- When verification fails, use the failure output to inspect and fix the nearest directly related file first. Re-run the same narrow verification once fixed.
- Final responses for calibration work must use the required AGENTS return format and avoid long explanations.

## Default Implementation Choices

Use these names and defaults unless compile constraints prove they cannot fit the repo:

| Topic | Default choice |
| --- | --- |
| Native namespace | `BeeCpp` |
| Managed wrapper namespace | Match existing `Pattern/*Cli` style already used by `RansacLineCli` and `PinPitchCli`; do not introduce a new managed namespace. |
| Native files | `Pattern/CameraCalibrationCore.h`, `Pattern/CameraCalibrationCore.cpp` |
| CLI files | `Pattern/CameraCalibrationCli.h`, `Pattern/CameraCalibrationCli.cpp` |
| Global calibration class | `BeeGlobal.CameraCalibrationConfig` |
| Profile class | `BeeGlobal.CameraCalibrationProfile` |
| Global variable | `BeeGlobal.Global.CameraCalibration` |
| Storage path | `Common\\CameraCalibration.config` |
| Access API | `Access.SaveCameraCalibration(...)`, `Access.LoadCameraCalibration(...)` |
| Load/save API | `LoadData.CameraCalibration()`, `SaveData.CameraCalibration(...)` |
| Preview apply default | `ApplyToPreview = true` only after operator saves a valid profile |
| Inspection apply default | `ApplyToInspectionInput = false` |
| Runtime image correction | Preview-only until CC-007 |
| Runtime algorithm | OpenCV/BeeCpp only, no AI calls |
| Camera frame source | Through `BeeCore/Func/Camera.cs`; UI never reads camera SDK directly |
| Scale sync | Store in `Global.CameraCalibration` first; update `Global.Config.Scale` only after explicit operator apply/save |
| Error handling | Normal detection miss returns `found=false` and short status; do not throw for expected no-target cases |

## Per-ID Decision/API Checklist

### CC-001 Native BeeCpp Core

- Entry files: `Pattern/CameraCalibrationCore.h`, `Pattern/CameraCalibrationCore.cpp`, `Pattern/Pattern.vcxproj`.
- Implement in this order: header structs/enums, source utilities, `DetectGrid`, `Solve`, `UndistortPreview`, `DetectScaleSample`, `AnalyzeLiveGuidance`, project file entries.
- Do not create CLI types here.
- Done when native files compile as part of `Pattern.vcxproj`.
- Verify once with `Pattern.vcxproj` Release x64 after the native file batch is complete.

### CC-002 C++/CLI Wrapper

- Entry files: `Pattern/CameraCalibrationCli.h`, `Pattern/CameraCalibrationCli.cpp`, `Pattern/Pattern.vcxproj`.
- Implement in this order: managed enums, managed DTOs, native-to-managed mapping helpers, `DetectGrid`, `Solve`, `UndistortPreview`, `DetectScaleSample`, `AnalyzeLiveGuidance`, project file entries.
- Do not create C# persistence or UI here.
- Done when C# can reference the managed wrapper types without native pointer ownership leaks.
- Verify once with `Pattern.vcxproj` Release x64 at the end of the wrapper batch.

### CC-003 Standalone C# Calibration State

- Entry files: `BeeGlobal/Global.cs`, new `BeeGlobal/*CameraCalibration*.cs`, `BeeGlobal/BeeGlobal.csproj`, `BeeCore/Data/Access.cs`, `BeeCore/Data/LoadData.cs`, `BeeCore/Data/SaveData.cs`, `BeeCore/BeeCore.csproj`, `BeeCore/Func/*`.
- Implement in this order: serializable BeeGlobal classes, `Global.CameraCalibration`, Access load/save methods, LoadData/SaveData methods, lightweight profile service, preview-only correction service stub.
- Do not add UI controls here.
- Do not change `Global.Config` except optional explicit scale sync helper.
- Done when separate calibration config can load missing-file default and save/load round trip without touching `Default.config`.
- Verify once at the end of the batch: build `BeeGlobal`, then `BeeCore` only if BeeCore persistence/service code changed.

### CC-004 Automatic Scale Calibration

- Entry files: `Pattern/CameraCalibrationCore.*`, `Pattern/CameraCalibrationCli.*`, `BeeCore/Func/*`, `BeeInterface/*` only if a minimal trigger/control is required by the selected implementation.
- Implement in this order: native sample detector, CLI result mapping, C# service method, profile scale update, explicit `Global.Config.Scale` apply path.
- Do not auto-change shared scale during detection.
- Done when sample detection can return found/not-found, pixel size, mm-per-pixel, and confidence/debug data.
- Verify only the changed layer: native wrapper if native/CLI changed, otherwise the smallest C# project touched.

### CC-005 Standalone WinForms UI

- Entry files: new `BeeInterface/*Calibration*` UI files, `BeeInterface/Steps/SettingStep1.cs` or `BeeInterface/GeneralSetting.cs`, `BeeInterface/BeeInterface.csproj`.
- Implement in this order: standalone form/control shell, open button from settings, profile selector, grid settings, capture list, solve button, before/after preview, save button.
- Live frame requests must route through `Camera.cs`.
- Do not feed corrected frames to tools.
- Done when operator can open the UI, solve/save a profile, and see corrected preview.
- Verify once with `BeeInterface` at the end of the UI batch; full solution only if cross-project references require it.

### CC-006 Simple Live Guidance UI

- Entry files: existing calibration UI files, `BeeInterface/Group/View.cs` only if frame subscription requires it, `BeeInterface/BeeInterface.csproj`.
- Implement in this order: throttled frame sampling, call guidance service, one-action status display, optional debug overlay.
- Do not add complex engineering charts as the default UI.
- Done when guidance updates without blocking preview and shows target-not-found when no grid/sample exists.
- Verify once with `BeeInterface` at the end of the guidance batch.

### CC-007 Optional Inspection Integration

- Entry files: `BeeCore/Func/*`, compatible `BeeCore/Unit/*`, `BeeInterface/Group/View.cs`.
- Start only after CC-005 and CC-006 are stable.
- Implement in this order: opt-in flag, corrected frame branch, raw frame fallback, ROI mapping helper, compatible tool pilot.
- Do not enable by default.
- Done when one selected compatible tool can opt into corrected input while legacy tools remain raw.
- Verify targeted project first; run full solution only when the corrected-frame contract crosses project boundaries and targeted builds already pass.

## Assumptions To Validate

- The project can use OpenCV calibration APIs already available to BeeCpp through the existing native dependency set.
- The target calibration patterns are known before capture: chessboard inner-corner count or symmetric/asymmetric circle-grid dimensions.
- Calibration is stored globally but should be keyed by camera ID/resolution/lens setup inside `Global.CameraCalibration`; changing lens, focus, mounting height, or resolution invalidates the active profile.
- Runtime undistortion for the first slice must not block the WinForms UI thread or reduce operator preview FPS.
- Existing C# persisted tool payload names should not be renamed because legacy programs depend on serialized type names.

## Functional Requirements

1. Pattern input
   - Support square chessboard and circular grid modes.
   - Store grid rows/columns, square pitch or circle spacing in mm, and expected image resolution.
   - Capture multiple calibration frames with pass/fail quality per frame.

2. Calibration solve
   - Return the full calibration profile: camera matrix, distortion coefficients, image size, optional rectification/homography, mean reprojection error, per-frame error, and per-point residuals.
   - Reject frames with poor grid coverage, low corner count, or excessive residuals.
   - Produce a debug image overlay with detected grid points and residual vectors.

3. Image correction
   - Provide undistort map generation and frame remap.
   - Provide optional perspective rectification when the calibration grid defines a desired planar working surface.
   - First slice applies correction to preview only.
   - Later integration may apply correction to tool-processing input after ROI/scale compatibility is validated.

4. Scale calibration
   - Automatically detect the known sample and measure pixel-to-mm scale after optional correction.
   - Store scale with camera profile, resolution, timestamp, sample description, and validation residual.
   - Warn if calibration profile resolution does not match current camera frame size.

5. Live distortion/pose guidance
   - Detect a grid or known planar pattern in live preview when guidance mode is active.
   - Show simple OK/NG guidance and direction hints such as rotate left/right, tilt up/down, move closer/farther, and camera not perpendicular.
   - Keep residual/vector data available as an optional debug layer, not the main operator UI.
   - Compute pose/tilt guidance from homography or solvePnP so the operator can adjust camera direction and angle.
   - Keep guidance sampling throttled so camera live preview remains responsive.

## Technical Plan

### Native BeeCpp

Create a dedicated native calibration module in `Pattern/` unless the team decides a separate native DLL is needed later:

- `Pattern/CameraCalibrationCore.h`
- `Pattern/CameraCalibrationCore.cpp`
- `Pattern/CameraCalibrationCli.h`
- `Pattern/CameraCalibrationCli.cpp`
- update `Pattern/Pattern.vcxproj`

Native data should include:

- `CalibrationPatternType`: chessboard, symmetric circle grid, asymmetric circle grid.
- `CalibrationGridSpec`: rows, columns, spacingMm, pattern type.
- `CalibrationFrameResult`: found flag, points, coverage bounds, reprojection error, debug image.
- `CameraCalibrationResult`: camera matrix, distortion coefficients, image size, total error, per-frame errors.
- `DistortionDiagnosticResult`: residual vectors, heatmap/vector data, pose/tilt estimate.

### Managed C# Contract

Expose thin C++/CLI types under the existing BeeCpp managed namespace. Keep ownership rules explicit:

- Native returns managed arrays or cloned byte buffers that C# owns after the call.
- No unmanaged buffer may escape without a clear `FreeHGlobal` or wrapper-owned disposal path.
- Do not change existing P/Invoke/native wrapper signatures unrelated to calibration.

C# core should add a small standalone profile model, likely under `BeeCore/Func/` or a future `BeeCore/Calibration/` folder, and store the active/global profile list in a separate `BeeGlobal.CameraCalibrationConfig` object:

- `CameraCalibrationProfile`
- `CameraCalibrationConfig` / `CameraCalibrationSettings` serializable class in `BeeGlobal`
- `Global.CameraCalibration` static variable in `BeeGlobal.Global`
- `Access.SaveCameraCalibration(...)` / `Access.LoadCameraCalibration(...)`
- `SaveData.CameraCalibration(...)` / `LoadData.CameraCalibration()`
- Separate file such as `Common\\CameraCalibration.config`
- `CameraCalibrationProfileStore` backed by `Global.CameraCalibration`
- `CameraCalibrationService`
- `CameraPreviewCorrection` helper for preview-only correction
- optional later `CameraFrameCorrection` helper for applying correction consistently to inspection inputs.

`CameraCalibrationProfile` must use serialization-friendly fields because it will be saved separately through the same Base64/BinaryFormatter-style persistence pattern used by the existing config files: arrays for matrix/distortion/homography values, primitive metadata fields, and explicit validation flags for preview and future inspection use.

`Global.Config.Scale` remains the existing shared scale used by current tools. Automatic calibration scale should update the calibration profile first and only sync to `Global.Config.Scale` after an explicit operator save/apply action.

### UI Flow

Add a standalone camera calibration surface reachable from camera/settings UI, not from a specific inspection tool:

1. Select camera and calibration mode.
2. Enter pattern dimensions and physical pitch.
3. Capture calibration frames with quality indicators.
4. Solve and review reprojection error.
5. Toggle before/after undistort preview.
6. Run automatic scale calibration from a known sample size.
7. Enable live guidance and show simple direction hints.
8. Save profile into `Global.CameraCalibration` and its separate calibration config file.
9. Later, after validation, choose whether the profile can apply to inspection input.

## Risk Notes

- Applying undistort to every live frame can be expensive. Generate maps once per resolution and reuse them; throttle live diagnostics.
- Scale in mm is only valid on the calibrated plane.
- Perspective rectification changes image geometry and can affect existing ROI coordinates. The first implementation must stay preview-only until profile application to inspection input is intentionally enabled.
- Circle-grid detection can fail under glare or low contrast. The UI should allow frame rejection and show why a frame is low quality.
- Live pose guidance must not call blocking SDK reads on the WinForms dispatch thread.

## Verification Plan

- Prefer one targeted verification at the end of a coherent `CC-xxx` batch.
- Do not add native smoke tests unless compile passes and sample/synthetic data is already available.
- For CC-001/CC-002, build `Pattern.vcxproj` Release x64 at the end of the batch.
- For CC-003, build `BeeGlobal`; build `BeeCore` only if BeeCore persistence/service code changed.
- For CC-005/CC-006, build `BeeInterface` at the end of the UI batch.
- Run full `EasyVision.sln` Release x64 only for cross-project contract integration, final slice validation, or when targeted builds pass but integration risk remains.
- If a targeted verify fails, fix the directly related file indicated by the error and rerun the same targeted verify. Expand scope only when the direct fix is not enough.
- Manual UI smoke:
  - Capture 8-15 grid frames.
  - Solve profile and verify reprojection error display.
  - Toggle undistort preview.
  - Calibrate known sample scale through automatic detection.
  - Confirm simple live guidance updates while camera preview remains responsive.
  - Confirm save/load round trip through `Global.CameraCalibration` and the separate calibration config file.

## Supporting Documents

- `tree.md`: dependency tree, processing flow, profile contents, and future integration branches.
- `camera-calibration-map.md`: implementation tracker.
- `preview-map.md`: preview-only sub-map for frame routing, corrected display, capture preview state, and live guidance. It may include `BeeCore/Func/Camera.cs` frame-source work and must not affect inspection input before CC-007.
