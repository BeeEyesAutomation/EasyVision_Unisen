# Pin Pitch Measure Map

## ID Rules

- Prefix: `PP`
- Format: `PP-xxx`
- Next ID: `PP-009`
- Reuse existing IDs.
- Do not duplicate IDs.

## Status values: `open` | `wip` | `partial` | `done` | `blocked`
## Risk values: `low` | `medium` | `high`

## Entries

| ID | Status | Risk | Area | Files | Depends on | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| PP-001 | done | medium | Native pin-center detector | `Pattern/PinPitchCore.h`, `Pattern/PinPitchCore.cpp`, `Pattern/PinPitchCli.h`, `Pattern/PinPitchCli.cpp`, `Pattern/VisionGeometryCore.h`, `Pattern/VisionGeometryCore.cpp`, `Pattern/Pattern.vcxproj` | - | Added independent BeeCpp `PinPitchCore`, thin `BeeCppCli.PinPitchCli`, and shared `VisionGeometryCore` helpers. Detector returns P1..P4 centers, confidence metrics, pitch values, row line/residual, and debug image buffer. Release x64 build passed with 424 warnings. |
| PP-002 | done | medium | Width point-to-line mode | `BeeCore/Unit/Width.cs`, `BeeInterface/Tool/ToolWidth.cs`, `BeeCore/Func/Engines/WidthEngineRunner.cs`, `BeeGlobal/Enums.cs` | PP-001 | Added `WidthMeasureMode.PointToLine`, selected point source/index state, point-to-line nominal/tolerance scoring, and runtime projection from selected source point to reference line L. Release x64 build passed with 424 warnings after one transient BeeCV prebuild stream error cleared on rerun. |
| PP-003 | done | medium | Reference line L using RANSAC | `BeeCore/Unit/Width.cs`, `Pattern/RansacLineCore.*`, `Pattern/RansacLineCli.*` | PP-002 | Width `PointToLine` now reuses existing `RansacLine.FindBestLine` for reference line L. No new line fitting implementation was added. |
| PP-004 | partial | medium | Pitch and Width result models/scoring | `BeeCore/Unit/Pitch.cs`, `BeeCore/Unit/Width.cs`, `BeeGlobal/Enums.cs` | PP-001, PP-003 | Added `PitchMeasureMode.PinPitch` core path, PinPitch result fields, P1..P4 global `listP_Center` output, adjacent pitch/span scoring, and Width PointToLine scoring. Added explicit PinPitch arrange mode before pitch calculation (`X`, `Y`, `RowProjection`) so pin order no longer depends on line orientation. Remaining: polish PinPitch result presentation and validation after real sample tuning. |
| PP-005 | partial | medium | ToolPitch and ToolWidth UI modes | `BeeInterface/Tool/ToolPitch.cs`, `BeeInterface/Tool/ToolWidth.cs` | PP-004 | Added lightweight programmatic UI panels for Pitch PeakRoot/PinPitch mode and Width ParallelLines/PointToLine mode with source point selection and nominal/tolerance inputs. ToolPitch now exposes PinPitch arrange mode. Remaining: designer-level layout polish and full operator workflow validation. |
| PP-006 | open | low | Validation dataset and calibration check | `docs/PinPitchMeasurePlan/*`, `tests/*` | PP-005 | Build a small replay set from OK/NG pin images. Record mm-per-pixel, expected pitch, tolerance, measured centers, center repeatability, and point-to-line distance repeatability. Target repeatability must be demonstrated before claiming `+-0.05mm`. |
| PP-007 | done | medium | Edge-boundary mask + edge-geometry center + fallback | `Pattern/PinPitchCore.h`, `Pattern/PinPitchCore.cpp`, `Pattern/PinPitchCli.h`, `Pattern/PinPitchCli.cpp`, `BeeCore/Unit/Pitch.cs`, `docs/PinPitchMeasurePlan/trial/_run_trial.py` | PP-001, PP-004 | Runtime sample (4 metallic pads) cho thấy threshold mask bias về bright reflection và bóng mờ pin pass threshold. Thêm `useEdgeBoundary` (Canny→close→fill), `useEdgeGeometryCenter` (midpoint projection bounds). Trial trên 6 ảnh: 4/6 EDGE đủ pin với center shift đúng 1–3px; `C_bg5` EDGE chính xác hơn (loại noise blob 256px); `10_pinrow_raw` miss pin contrast yếu → đã thêm fallback trong `measure()` retry threshold mask khi candidates < expectedCount. Default ON cho PinPitch. Release x64 build pass, errors=0, warnings=424. |
| PP-008 | done | medium | Gradient refinement per-candidate (CLAHE+Sobel) | `Pattern/PinPitchCore.h`, `Pattern/PinPitchCore.cpp`, `Pattern/PinPitchCli.h`, `Pattern/PinPitchCli.cpp`, `BeeCore/Unit/Pitch.cs`, `docs/PinPitchMeasurePlan/trial/_run_trial.py` | PP-007 | Runtime screenshot 2: green box quá nhỏ — chỉ phủ bright reflection. Canny global thiếu sensitivity ở biên pad-vs-background contrast yếu. Thêm `RefineByGradient(gray, cand, opts)` chạy **per-candidate** sau detection: crop patch quanh seed, CLAHE boost contrast, Sobel magnitude → threshold → close → contour bao quanh seed, area ≥ 0.8× seed. Pass aspect/fill validate mới apply. Default ON. Trial confirms boxes preserved/expanded, không bị shrink. Release x64 build pass, errors=0, warnings=424. |

## Detailed Plans

- `CoreGuiImplementationPlan.md`: complete core + GUI implementation plan for ToolPitch PinPitch mode and ToolWidth PointToLine mode.
