# Camera Calibration Map

## ID Rules

- Prefix: `CC`
- Format: `CC-xxx`
- Next ID: `CC-008`
- Reuse existing IDs.
- Do not duplicate IDs.

## Status values: `open` | `wip` | `partial` | `done` | `blocked`
## Risk values: `low` | `medium` | `high`

## Entries

| ID | Status | Risk | Area | Files | Depends on | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| CC-001 | done | medium | BeeCpp calibration solver | `Pattern/CameraCalibrationCore.h`, `Pattern/CameraCalibrationCore.cpp`, `Pattern/Pattern.vcxproj`, `docs/CameraCalibrationPlan/*` | - | Add native calibration core for chessboard and circle-grid detection, camera matrix/distortion solve, reprojection error, preview undistort, automatic scale-sample detection, live guidance action, frame quality, and debug overlay. Follow `tree.md` C++ Step Plan CC-001. |
| CC-002 | done | medium | BeeCpp C++/CLI managed wrapper | `Pattern/CameraCalibrationCli.h`, `Pattern/CameraCalibrationCli.cpp`, `Pattern/Pattern.vcxproj` | CC-001 | Expose managed calibration inputs/results, preview undistort, scale sample result, simple guidance action, diagnostic residual vectors, and cloned debug buffers to C# without changing unrelated wrapper APIs. Follow `tree.md` C++ Step Plan CC-002. |
| CC-003 | done | medium | Standalone C# calibration class and global variable | `BeeGlobal/Global.cs`, new `BeeGlobal/*CameraCalibration*.cs`, `BeeCore/Data/Access.cs`, `BeeCore/Data/LoadData.cs`, `BeeCore/Data/SaveData.cs`, `BeeCore/Func/*`, `BeeCore/BeeCore.csproj`, `BeeGlobal/BeeGlobal.csproj` | CC-002 | Add independent serializable calibration class for all tools, expose it as `Global.CameraCalibration`, and persist it through separate load/save APIs and file such as `Common\\CameraCalibration.config`. Key profiles by camera ID/resolution/setup. Keep runtime service preview-only at this stage and preserve camera Start -> Read -> Stop lifecycle. |
| CC-004 | done | medium | Automatic pixel-to-mm scale calibration | `Pattern/CameraCalibrationCore.*`, `Pattern/CameraCalibrationCli.*`, `BeeCore/Func/*`, `BeeInterface/*` | CC-003 | Add automatic known-sample detection for scale calibration. Operator enters the real sample size in mm; software detects sample pixels, stores mm-per-pixel in `Global.CameraCalibration`, and syncs `Global.Config.Scale` only after explicit operator apply/save. |
| CC-005 | done | medium | Standalone camera calibration WinForms UI | `BeeInterface/Steps/SettingStep1.cs`, `BeeInterface/GeneralSetting.cs`, `BeeInterface/Group/View.cs`, new `BeeInterface/*Calibration*` UI files | CC-003, CC-004 | Added a standalone calibration form launched from settings. It supports camera/profile selection, pattern settings, capture list review, solve/save flow, before/after corrected preview, automatic scale detection, and explicit scale apply/save without feeding inspection tools. |
| CC-006 | done | medium | Simple live direction guidance UI | `BeeInterface/Group/View.cs`, new `BeeInterface/*Calibration*` UI files, `Pattern/CameraCalibrationCore.*`, `Pattern/CameraCalibrationCli.*` | CC-005 | Added throttled live guidance in the calibration form. It shows one simple operator action at a time, reuses the preview frame path, and keeps guidance work off the main preview loop. |
| CC-007 | open | medium | Optional inspection pipeline integration | `BeeCore/Func/*`, `BeeCore/Unit/*`, `BeeInterface/Group/View.cs` | CC-005, CC-006 | After standalone preview workflow is stable, add explicit opt-in for corrected frames/scale to feed inspection tools. Validate ROI coordinate compatibility before enabling by default. |

## Detailed Plan

- `Plan.md`: end-to-end feature plan, assumptions, requirements, technical slices, risks, and verification.
- `tree.md`: dependency tree, processing flow, separate `Global.CameraCalibration` profile contents/load-save path, and future inspection integration branches.
- `preview-map.md`: preview-only sub-map for frame source, corrected display, capture preview state, live guidance, and future corrected-frame bridge. It may include `BeeCore/Func/Camera.cs` when frame source work is needed.

## Work Rules

- Pick one open `CC-xxx` entry and finish or block it before moving to the next.
- Use `Plan.md` and `tree.md` as source of truth; do not create additional plan files.
- Use `preview-map.md` only for preview-related work. Do not read it for CC-001/CC-002 unless a preview API compile error requires it.
- Keep live camera access routed through `BeeCore/Func/Camera.cs`.
- Keep runtime calibration deterministic with BeeCpp/OpenCV, not AI model calls.
- Do not build for markdown-only updates.
- Verify narrowly once at the end of the current `CC-xxx` batch.
- On failure, fix the directly related file indicated by the test/build result before scanning broader code.

## Minimal Start Commands

Use these commands only when the selected entry needs context from code. Avoid broad scans.

| ID | First context commands | Targeted verify |
| --- | --- | --- |
| CC-001 | `Get-Content Pattern\\PinPitchCore.h -TotalCount 120`; `Get-Content Pattern\\Pattern.vcxproj -TotalCount 220` | Build `Pattern\\Pattern.vcxproj` Release x64 |
| CC-002 | `Get-Content Pattern\\RansacLineCli.h -TotalCount 180`; `Get-Content Pattern\\PinPitchCli.h -TotalCount 180` | Build `Pattern\\Pattern.vcxproj` Release x64 |
| CC-003 | `Get-Content BeeGlobal\\Global.cs -TotalCount 380`; `Get-Content BeeCore\\Data\\Access.cs -TotalCount 170`; `Get-Content BeeCore\\Data\\LoadData.cs -TotalCount 240`; `Get-Content BeeCore\\Data\\SaveData.cs -TotalCount 220` | Build `BeeGlobal`, then `BeeCore` |
| CC-004 | Read only files changed by CC-001/CC-002/CC-003 plus the target service file | Build changed native/C# project only at batch end |
| CC-005 | `Get-Content BeeInterface\\Steps\\SettingStep1.cs -TotalCount 140`; read new calibration UI files after creation | Build `BeeInterface` |
| CC-006 | Read calibration UI files and the smallest needed `View.cs` frame path section | Build `BeeInterface` |
| CC-007 | Read only pilot tool/frame source files named before starting integration | Targeted project, then full solution only if contracts cross projects |

## Done Criteria

| ID | Done when |
| --- | --- |
| CC-001 | Native core compiles and exposes deterministic grid solve, preview undistort, scale sample detection, and guidance result without camera SDK calls. |
| CC-002 | Managed wrapper compiles and returns cloned/managed data without leaking native pointers. |
| CC-003 | `Global.CameraCalibration` loads missing-file defaults, saves to `Common\\CameraCalibration.config`, and does not mutate `Default.config`. |
| CC-004 | Scale detection updates active calibration profile and syncs `Global.Config.Scale` only through explicit apply/save. |
| CC-005 | Standalone UI opens from settings, captures/solves/saves profile, and shows corrected preview without feeding tools. |
| CC-006 | Live guidance shows one simple action and does not block live preview. |
| CC-007 | Corrected frame integration is opt-in, reversible, and proven on one compatible pilot path while legacy raw path remains available. |
