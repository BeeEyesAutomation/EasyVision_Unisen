# Camera Calibration Preview Map

## ID Rules

- Prefix: `CP`
- Format: `CP-xxx`
- Next ID: `CP-007`
- Reuse existing IDs.
- Do not duplicate IDs.

## Status values: `open` | `wip` | `partial` | `done` | `blocked`
## Risk values: `low` | `medium` | `high`

## Purpose

This sub-map coordinates preview-only work under `CameraCalibrationPlan`. It owns preview behavior, frame routing, corrected display, and live guidance because live guidance runs on preview frames.

Read this file only when working on preview-related entries: CC-003 preview service, CC-005 standalone UI, CC-006 live guidance, or CC-007 future corrected-frame integration.

## Preview Decisions

- Preview is the first consumer of camera correction.
- Inspection input remains raw until CC-007 explicitly enables an opt-in corrected branch. This preview-first rule is strict.
- Live preview frames must come through `BeeCore/Func/Camera.cs` or existing view/camera abstraction.
- UI code must not call camera SDK/read loops directly.
- This sub-map may scope changes to `BeeCore/Func/Camera.cs` when frame source/subscription work is required.
- Corrected preview must preserve display frame size unless the operator explicitly chooses a rectified plane view.
- Guidance UI shows one primary action at a time.
- Debug overlays are optional and must not become the default operator workflow.
- Preview verification is targeted: build `BeeInterface` at batch end; run full solution only for cross-project contract risk.

## Entries

| ID | Status | Risk | Area | Files | Depends on | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| CP-001 | open | medium | Preview frame source contract | `BeeCore/Func/Camera.cs`, `BeeInterface/Group/View.cs`, `docs/CameraCalibrationPlan/*` | CC-003 | Define how calibration UI requests or subscribes to the latest live frame through the existing camera/view path. No SDK calls from UI. Preserve Start -> Read -> Stop ownership in `Camera.cs`. |
| CP-002 | open | medium | Corrected preview service | `BeeCore/Func/CameraPreviewCorrection.cs`, `BeeCore/Func/CameraCalibrationProfileStore.cs`, `docs/CameraCalibrationPlan/*` | CC-003 | Read active `Global.CameraCalibration` profile, validate camera/resolution, cache undistort maps per profile/resolution, and return preview-corrected frames only. Do not feed tools. |
| CP-003 | open | low | Before/after preview UI | new `BeeInterface/Calibration/*Preview*` UI files, `BeeInterface/BeeInterface.csproj` | CP-002, CC-005 | Show raw/corrected preview, profile status, resolution mismatch status, and save/apply state. Keep controls simple and operator-facing. |
| CP-004 | open | low | Calibration capture preview state | new `BeeInterface/Calibration/*Capture*` UI files | CC-001, CC-002, CC-005 | Show captured calibration frames as accepted/rejected with short quality status. Use BeeCpp grid-detect result overlays only when available. |
| CP-005 | open | medium | Live guidance preview | new `BeeInterface/Calibration/*Guide*` UI files, `BeeInterface/Group/View.cs` if needed | CC-006, CP-001 | Throttle guidance sampling separately from display FPS, call guidance service, and show one action: OK, target not found, rotate, tilt, move closer/farther, or not perpendicular. |
| CP-006 | open | medium | Future corrected-frame preview bridge | `BeeCore/Func/*`, `BeeInterface/Group/View.cs`, compatible pilot tool files | CC-007, CP-002 | When future integration starts, provide an explicit raw/corrected preview bridge and coordinate mapping preview. Must remain opt-in and reversible. |

## Flow

```text
Camera.cs / View frame path
|
+-- raw frame
|   |
|   +-- display raw preview
|   +-- keep inspection tools raw until CC-007
|
+-- CameraPreviewCorrection
|   |
|   +-- validate active Global.CameraCalibration profile
|   +-- reuse maps for profile + resolution
|   +-- return corrected preview frame
|
+-- Calibration UI
    |
    +-- before/after display
    +-- capture quality list
    +-- live guidance one-action status
    +-- optional debug overlay
```

## Done Criteria

| ID | Done when |
| --- | --- |
| CP-001 | Calibration UI has a defined frame source through Camera/View abstraction and no direct SDK read path. |
| CP-002 | Preview correction can return raw fallback or corrected frame based on active profile validity without changing inspection input. |
| CP-003 | Operator can see raw/corrected preview and profile validity status. |
| CP-004 | Captured calibration frames show accepted/rejected status and short quality reason. |
| CP-005 | Guidance updates without blocking live preview and shows one primary action. |
| CP-006 | Future corrected preview bridge is opt-in, reversible, and does not migrate legacy ROI automatically. |
