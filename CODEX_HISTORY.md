# Codex Handoff History

This file records implementation context for future bots/agents working in this repository.

## 2026-04-22 - Pattern2 preprocess, GPU, and CPU threading

Scope:
- Module: `Pattern/Pattern2.h`, `Pattern/Pattern2.cpp`.
- Added preprocess pipeline for `Pattern2` stable matching:
  - `Pattern2FeatureDomain`: `Gray`, `Edge`, `GrayPlusEdge`.
  - `Pattern2EdgeMethod`: `SobelMag`, `ScharrMag`, `Canny`, `Laplacian`.
  - `Pattern2DenoiseMethod`: `None`, `Gaussian`, `Median`, `Bilateral`.
  - `Pattern2PreprocessConfig`.
  - `Pattern2StableConfig::Preprocess`.
- Removed unused `PatternMatchOptions` from `Pattern2.h`.
- Added template preprocess snapshot fields into `s_TemplData` so learn-time preprocess is replayed during match.
- Added stable template edge caches:
  - `tplPreprocessedGray`
  - `tplEdgeMagnitude`
  - `tplEdgeBinary`
  - `s_StableScaleTemplate::tplEdgeMagnitude`
- Added helper pipeline:
  - `ApplyGrayPipeline`
  - `ComputeEdgeMagnitude`
  - `ComputeEdgeBinary`
  - `FilterEdgeByLength`
  - `ApplyFullPreprocess`
- Added `PreviewPreprocessed` API for C# debug/tuning.
- Added preprocess presets:
  - `PresetGeneralGray`
  - `PresetUnevenLighting`
  - `PresetMetalShiny`
  - `PresetPCBOrText`
  - `PresetLowContrast`
- Added GPU option using OpenCV OpenCL/UMat, not CUDA:
  - `Pattern2StableConfig::EnableGpu`
  - `Pattern2::SetGpuEnabled(bool)`
  - `Pattern2::IsGpuAvailable()`
  - `s_TemplData::vecPyramidGpu`
  - `Img::MatchTemplateGpu`
  - `Img::GetRotatedROIGpu`
  - source/template pyramid UMat caching to reduce repeated Mat-to-UMat upload.
- Added CPU multi-thread option:
  - `Pattern2StableConfig::EnableCpuMultiThread`
  - `Pattern2StableConfig::CpuThreads`
  - legacy `Match(..., useMultiThread, numThreads)` now parallelizes top-layer angle scan on CPU.
  - GPU path ignores CPU multi-thread flag.

Build verification:
- Command: `MSBuild Pattern\Pattern.vcxproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal`
- Result: pass, output `Pattern\x64\Release\BeeCpp.dll`.
- Remaining warnings are existing project warnings around conversion, SIMD/OpenCV headers, and old code paths.

Notes for future agents:
- The GPU implementation uses OpenCV OpenCL/UMat because the project links `opencv_world455.lib`, with no explicit `opencv_cuda*` dependencies.
- GPU speed still depends on OpenCL runtime and image/template sizes. Small templates or frequent result download can still favor CPU.
- CPU multi-threading parallelizes only the top-layer angle scan. Refinement/NMS remains mostly sequential.
- Keep unrelated modified files out of commits unless the user explicitly asks; this worktree commonly has many unrelated local edits.

## 2026-04-22 - PropetyTool index/thread cleanup

Scope:
- Follow-up after `PropetyTool` was split into payload (`Propety`, `Propety2`) plus `ToolState`.
- Goal: remove noisy direct access through `Common.PropetyTools[indexProg][indexTool]` and make future bots use resolver helpers.

Common helpers added:
- `Common.TryGetTool(int indexProg, int indexTool)`
- `Common.TryGetTool(int indexTool)`
- `Common.TryGetToolList(int indexProg)`
- `Common.TryGetCurrentToolList()`
- `Common.EnsureToolList(int indexProg)`
- `Common.EnsureCurrentToolList()`
- `Common.SetToolList(int indexProg, List<PropetyTool> tools)`

Refactor summary:
- Replaced direct two-level tool lookup across the C# project with `TryGetTool(...)`.
- Replaced program/thread list access patterns with `TryGetToolList(...)`, `EnsureToolList(...)`, or `SetToolList(...)`.
- Added local `OwnerTool` cache in `BeeInterface/Tool/*` classes during the earlier phase, and normalized event subscribe patterns with `-=` before `+=`.
- Additional duplicate-subscribe fixes:
  - `BeeCore/Items/ItemTool.cs`: `ScoreChanged`
  - `BeeInterface/Tool/ToolCounter.cs`: `PercentChange`
  - `BeeInterface/Tool/ToolYolo.cs`: `PercentChange`

Verification:
- `Select-String` for `Common.PropetyTools[` / `BeeCore.Common.PropetyTools[` outside `Common.cs`: 0 matches.
- `Select-String` for `PropetyTools[` now only matches internal implementation lines in `BeeCore/Common.cs`.
- Event subscribe balance for `StatusToolChanged`, `ScoreChanged`, `PercentChange`: `+= 43`, `-= 43`.
- Full build is still blocked by existing native/dependency issues:
  - `Pattern/BarcodeCore.cpp`: missing `ZXing/ReadBarcode.h`
  - `Pattern/RansacLineCore.cpp`: `std::clamp` unavailable in current native compile settings
  - `BeeCV/Resource.rc`: undefined `VER_MAJOR`
  - `BeeNativeOnnx` / `BeeNativeRCNN`: missing `openvino/openvino.hpp`
- C# project-only builds with project references disabled are blocked by missing metadata DLLs under project-specific output paths, for example `Pattern/x64/Debug/BeeCpp.dll` and `BeeGlobal/bin/x64/Debug/BeeGlobal.dll`.

Notes for future agents:
- Use `Common.TryGetTool(...)` for a single tool reference.
- Use `Common.TryGetToolList(...)` when checking whether a program/thread list exists without creating it.
- Use `Common.EnsureToolList(...)` when the code intends to mutate/add/remove/show tools and the list should exist.
- Use `Common.SetToolList(...)` instead of assigning `Common.PropetyTools[indexProg] = ...`.
- Keep `Common.PropetyTools` itself public for backward compatibility, but avoid new indexed call sites outside `Common.cs`.
- This repo has unrelated dirty/generated files after builds, especially native version files. Do not revert them unless the user explicitly asks.

## Ongoing convention - Update this file after each Codex task

Future agents should append a dated section to `CODEX_HISTORY.md` after finishing a task. Include:
- What changed and the main files touched.
- Verification commands and results.
- Known blockers or risks.
- Any files intentionally left dirty or intentionally not committed.

## 2026-05-03 - P3L.0 ToolViewBase framework

Scope:
- Started from `docs/architecture/preview/CODE_PREVIEW.md`, `TASK_CARDS_P3_LITE.md`, and `code_map.json`.
- Implemented the first mandatory Phase 3 Lite card only; later cards depend on this framework and were intentionally not started.

Changes:
- Added `BeeInterface/Tool/_Base/IToolView.cs`.
- Added `BeeInterface/Tool/_Base/ToolTabContext.cs`.
- Added `BeeInterface/Tool/_Base/ToolTabRegistry.cs`.
- Added `BeeInterface/Tool/_Base/ToolViewBase.cs`.
- Added `BeeInterface/Tool/_Base/ToolViewBase.Designer.cs`.
- Updated `BeeInterface/BeeInterface.csproj` to compile the five new files.

Implementation notes:
- Used `BeeInterface.Tool._Base` namespace and C# 7.3-compatible syntax.
- `ToolViewBase` provides fixed tabs: General, ROI, Params, Result.
- `ToolTabRegistry` supports dynamic tabs by `ToolKind` without adding a new UI singleton dependency beyond the registry itself.
- `ToolViewBase.OwnerTool` resolves through `BeeCore.Common.TryGetTool(BeeGlobal.Global.IndexProgChoose, index)` and does not access `Common.PropetyTools` directly.
- Existing `ToolXxx` files and all `BeeCore/Unit/*` persisted engine classes were left untouched.

Verification:
- Full command: `MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal`.
- Result: failed on existing project/reference issues before validating BeeInterface changes. Observed blockers include BeeCore missing OpenCvSharp types such as `Mat`, `Rect`, `MatType`, and `Line2D`.
- Targeted command: `MSBuild BeeInterface/BeeInterface.csproj /t:Build /p:Configuration=Release /p:Platform=x64 /p:BuildProjectReferences=false /v:minimal`.
- Result: failed on existing OpenCvSharp reference resolution errors in BeeInterface files.
- New-file sanity compile: Roslyn `csc.exe /target:library /langversion:7.3` against existing `BeeCore/bin/x64/Release/BeeCore.dll` and `BeeCore/bin/x64/Release/BeeGlobal.dll`.
- Result: pass.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches.

Known state:
- Worktree was dirty before this task and remains dirty.
- Running the full build also touched generated/native version files, including `OKNG/version.h`; per repo rules, these were not reverted.

## 2026-05-03 - P3L.1 Shared Tool UI controls

Scope:
- Continued the Phase 3 Lite sequence from `docs/architecture/preview/TASK_CARDS_P3_LITE.md`.
- Implemented P3L.1 only; no existing `ToolXxx` files were edited.

Changes:
- Added `BeeInterface/Custom/RoiToolbar.cs`, `.Designer.cs`, and `.resx`.
- Added `BeeInterface/Custom/ScoreThresholdBar.cs`, `.Designer.cs`, and `.resx`.
- Added `BeeInterface/Custom/ResultMiniGrid.cs`, `.Designer.cs`, and `.resx`.
- Updated `BeeInterface/BeeInterface.csproj` with compile and embedded resource entries for the three new UserControls.

Implementation notes:
- New controls use the existing root `BeeInterface` namespace, matching the current custom-control convention.
- `RoiToolbar` raises `RoiActionClicked` with a typed `RoiAction` enum and supports visibility flags for polygon, mask, and sampling button groups.
- `ScoreThresholdBar` wraps the existing `AdjustBarEx` and exposes a WinForms-style `EventHandler ValueChanged`.
- `ResultMiniGrid` provides `SetRow`, `Clear`, and OK/NG coloring without depending on any tool engine class.

Verification:
- Focused compile passed with Roslyn C# 7.3 for the new controls plus their dependencies `RJButton` and `AdjustBarEx`.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches.
- Full solution build was not re-run for this card because P3L.0 already confirmed the current checkout is blocked by existing OpenCvSharp reference errors in BeeCore/BeeInterface.

Known state:
- Worktree was dirty before this task and remains dirty.
- P3L.2 can now consume the new controls when piloting `ToolCircle`, but full build verification still needs the OpenCvSharp/MSBuild baseline fixed or reproduced on a machine with the expected reference paths.

## 2026-05-03 - P3L.2 Circle runner pilot, conservative pass

Scope:
- Continued Phase 3 Lite with the `ToolCircle` pilot.
- Kept `BeeCore/Unit/Circle.cs` untouched to preserve BinaryFormatter save/load compatibility.
- Did not edit `ToolCircle.Designer.cs`.

Changes:
- Added `BeeCore/Func/Engines/CircleEngineRunner.cs`.
- Updated `BeeCore/BeeCore.csproj` to compile the new runner.
- Updated `BeeInterface/Tool/ToolCircle.cs` to delegate:
  - owner wait-state setup,
  - view-state reads from `PropetyTool`/`Circle`,
  - score writes to `PropetyTool`,
  - calibration flag setup,
  - selected-tool run requests.

Implementation notes:
- The actual Circle algorithm was already in `BeeCore.Circle.DoWork(...)` and `Circle.Complete()`, not in `ToolCircle.cs`.
- Because of that existing split, this pass extracted a runner facade and owner/view-state boundary rather than moving the full algorithm.
- `CircleEngineRunner.Run(...)` wraps `Circle.DoWork(...)` and optionally `Circle.Complete()` for future non-UI callers.
- `ToolCircle` still owns UI-only control binding and ROI button behavior.
- Existing `StatusToolChanged` and `ScoreChanged` subscriptions still use the required `-=` then `+=` pattern.

Verification:
- Focused compile passed with Roslyn C# 7.3 for `BeeCore/Func/Engines/CircleEngineRunner.cs` against existing `BeeCore/bin/x64/Release/BeeCore.dll` and `BeeCore/bin/x64/Release/BeeGlobal.dll`.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches.
- Event pair check in `ToolCircle.cs`: `StatusToolChanged` and `ScoreChanged` both retain paired `-=` / `+=`.
- Full solution build remains blocked by the previously recorded OpenCvSharp reference errors in BeeCore/BeeInterface.

Known state:
- P3L.2 is a conservative pilot pass, not the full 30% LOC reduction from the task card.
- Further `ToolCircle` slimming should happen only after the build/reference baseline is healthy, because the remaining code is mostly UI event handling and Designer-bound control state.

## 2026-05-03 - P3L.3 Width runner pilot, conservative pass

Scope:
- Continued Phase 3 Lite with the `ToolWidth` pilot.
- Kept `BeeCore/Unit/Width.cs` untouched during this step. It was already dirty before P3L.3 and remains a pre-existing local edit.
- Did not edit `ToolWidth.Designer.cs`.

Changes:
- Added `BeeCore/Func/Engines/WidthEngineRunner.cs`.
- Updated `BeeCore/BeeCore.csproj` to compile the new runner.
- Updated `BeeInterface/Tool/ToolWidth.cs` to delegate:
  - owner wait-state setup,
  - view-state reads from `PropetyTool`/`Width`,
  - score writes to `PropetyTool`,
  - calibration flag setup,
  - selected-tool run requests.

Implementation notes:
- The actual Width algorithm was already in `BeeCore.Width.DoWork(...)` and `Width.Complete()`.
- This pass mirrors the P3L.2 Circle approach: a runner facade and owner/view-state boundary, not a full UI rewrite.
- `WidthEngineRunner.Run(...)` wraps `Width.DoWork(...)` and optionally `Width.Complete()` for future non-UI callers.
- Existing `StatusToolChanged` and `ScoreChanged` subscriptions still use the required `-=` then `+=` pattern.
- The Git diff for `ToolWidth.cs` includes prior local edits unrelated to this runner pass; those were not reverted.

Verification:
- Focused compile passed with Roslyn C# 7.3 for `BeeCore/Func/Engines/WidthEngineRunner.cs` against existing release DLLs: `BeeCore.dll`, `BeeGlobal.dll`, and `OpenCvSharp.dll`.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches.
- Event pair check in `ToolWidth.cs`: `StatusToolChanged` and `ScoreChanged` both retain paired `-=` / `+=`.
- Full solution build remains blocked by the previously recorded OpenCvSharp reference errors in BeeCore/BeeInterface.

Known state:
- P3L.3 is a conservative pilot pass, not a full LOC reduction.
- Before starting bulk Group A work, restore a reliable build/reference baseline so `ToolWidth` and its pre-existing local changes can be validated end to end.

## 2026-05-03 - P3L.4 Measure runner pass

Scope:
- Started P3L.4 bulk Group A work with `ToolMeasure`.
- Chose `ToolMeasure` because it was not listed as dirty before this step.
- Kept `BeeCore/Unit/Measure.cs` untouched to preserve persisted engine layout.
- Did not edit `ToolMeasure.Designer.cs`.

Changes:
- Added `BeeCore/Func/Engines/MeasureEngineRunner.cs`.
- Updated `BeeCore/BeeCore.csproj` to compile the new runner.
- Updated `BeeInterface/Tool/ToolMeasure.cs` to delegate:
  - owner wait-state setup,
  - score/view-state reads from `PropetyTool`/`Measure`,
  - score writes to `PropetyTool`,
  - selected-tool worker start requests.

Implementation notes:
- `ToolMeasure` still owns point-selection combobox binding and drawing/edit UI.
- The actual Measure algorithm remains in `BeeCore.Measure.DoWork(...)` and `Measure.Complete()`.
- Existing `StatusToolChanged` subscription retains the required `-=` then `+=` pattern.

Verification:
- Focused compile passed with Roslyn C# 7.3 for `BeeCore/Func/Engines/MeasureEngineRunner.cs` against existing release DLLs: `BeeCore.dll` and `BeeGlobal.dll`.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches.
- Event pair check in `ToolMeasure.cs`: `StatusToolChanged` retains paired `-=` / `+=`.
- Full solution build remains blocked by the previously recorded OpenCvSharp reference errors in BeeCore/BeeInterface.

Known state:
- This is a conservative runner pass, not a full ToolMeasure UI rewrite.
- Continue P3L.4 one tool at a time, preferring Group A files without unrelated local edits.

## 2026-05-03 - P3L.4 Edge runner pass

Scope:
- Continued P3L.4 bulk Group A work with `ToolEdge`.
- Kept `BeeCore/Unit/Edge.cs` untouched during this step. It was already dirty before this pass and remains a pre-existing local edit.
- Did not edit `ToolEdge.Designer.cs`.

Changes:
- Added `BeeCore/Func/Engines/EdgeEngineRunner.cs`.
- Updated `BeeCore/BeeCore.csproj` to compile the new runner.
- Updated `BeeInterface/Tool/ToolEdge.cs` to delegate:
  - owner wait-state setup,
  - score/view-state reads from `PropetyTool`/`Edge`,
  - score writes to `PropetyTool`,
  - calibration flag setup,
  - selected-tool run requests.

Implementation notes:
- `ToolEdge` still owns ROI edit UI, direction button state, and parameter event handlers.
- The actual Edge algorithm remains in `BeeCore.Edge.DoWork(...)` and `Edge.Complete()`.
- Existing `StatusToolChanged` and `ScoreChanged` subscriptions retain the required `-=` then `+=` pattern.

Verification:
- Focused compile passed with Roslyn C# 7.3 for `BeeCore/Func/Engines/EdgeEngineRunner.cs` against existing release DLLs: `BeeCore.dll`, `BeeGlobal.dll`, `OpenCvSharp.dll`, and `BeeCpp.dll`.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches.
- Event pair check in `ToolEdge.cs`: `StatusToolChanged` and `ScoreChanged` retain paired `-=` / `+=`.
- Full solution build remains blocked by the previously recorded OpenCvSharp reference errors in BeeCore/BeeInterface.

Known state:
- This is a conservative runner pass, not a full ToolEdge UI rewrite.
- `BeeCore/Unit/Edge.cs` has unrelated local edits from before this step; do not treat them as part of this runner pass.

## 2026-05-03 - P3L.4 Crop runner pass

Scope:
- Continued P3L.4 bulk Group A work with `ToolCrop`.
- Kept `BeeCore/Unit/Crop.cs` untouched during this step.
- Did not edit `ToolCrop.Designer.cs`.

Changes:
- Added `BeeCore/Func/Engines/CropEngineRunner.cs`.
- Updated `BeeCore/BeeCore.csproj` to compile the new runner.
- Updated `BeeInterface/Tool/ToolCrop.cs` to delegate:
  - owner wait-state setup,
  - path view-state reads from `Crop`,
  - selected-tool worker start requests.

Implementation notes:
- `ToolCrop` still owns folder selection, clear-state UI, and output display wiring.
- The actual Crop algorithm remains in `BeeCore.Crop.DoWork(...)` and `Crop.Complete()`.
- Existing `StatusToolChanged` subscription retains the required `-=` then `+=` pattern.

Verification:
- Focused compile passed with Roslyn C# 7.3 for `BeeCore/Func/Engines/CropEngineRunner.cs` against existing release DLLs: `BeeCore.dll`, `BeeGlobal.dll`, and `OpenCvSharp.dll`.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches.
- Event pair check in `ToolCrop.cs`: `StatusToolChanged` retains paired `-=` / `+=`.
- Full solution build remains blocked by the previously recorded OpenCvSharp reference errors in BeeCore/BeeInterface.

Known state:
- This is a conservative runner pass, not a full ToolCrop UI rewrite.
- Continue P3L.4 one tool at a time, preferring Group A files without unrelated local edits.

## 2026-05-03 - P3L.4 remaining Group A runner batch

Scope:
- Continued P3L.4 with six remaining safe Group A tool UCs:
  - `ToolEdgePixel`,
  - `ToolCorner` / `MeasureCorner`,
  - `ToolIntersect`,
  - `ToolColorArea`,
  - `ToolCheckMissing`,
  - `ToolPosition_Adjustment`.
- Kept all matching `BeeCore/Unit/*` engine files untouched during this batch.
- Did not edit any Designer files.
- Deferred `ToolCounter`: current `ToolCounter` is wired to a `Yolo` payload and `CounterInfor.*` is already deleted in the working tree, so it needs a separate cleanup decision before a safe runner pass.

Changes:
- Added:
  - `BeeCore/Func/Engines/EdgePixelEngineRunner.cs`,
  - `BeeCore/Func/Engines/CornerEngineRunner.cs`,
  - `BeeCore/Func/Engines/IntersectEngineRunner.cs`,
  - `BeeCore/Func/Engines/ColorAreaEngineRunner.cs`,
  - `BeeCore/Func/Engines/CheckMissingEngineRunner.cs`,
  - `BeeCore/Func/Engines/PositionAdjustmentEngineRunner.cs`.
- Updated `BeeCore/BeeCore.csproj` to compile the new runners.
- Updated the six Tool UCs to delegate owner score reads, owner wait-state setup, score writes, calibration flags where present, and selected-tool run/start requests.
- Tightened the `ToolCheckMissing` `StatusToolChanged` subscription so `-=` is immediately followed by `+=`.

Implementation notes:
- Tool-specific parameter binding, ROI editing, drawing, and UI layout remain owned by each `ToolXxx.cs`.
- Actual algorithms remain in their existing engine payload classes and `DoWork(...)` / `Complete()` methods.
- This is still the conservative runner-facade pattern, not a full 30% LOC extraction.

Verification:
- Focused compile passed with Roslyn C# 7.3 for the six new runner files against existing release DLLs: `BeeCore.dll`, `BeeGlobal.dll`, and `OpenCvSharp.dll`.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches.
- Event pair check in the six edited Tool UCs: all found `StatusToolChanged` / `ScoreChanged` subscriptions retain paired `-=` / `+=`.
- Full solution build remains blocked by the previously recorded OpenCvSharp reference errors in BeeCore/BeeInterface and by unrelated dirty tree state.

Known state:
- P3L.4 is complete for the active/safe Group A runner passes except `ToolCounter`.
- Before P3L.5, decide whether `ToolCounter` should be removed as orphan UI, rewired to `BeeCore.Counter`, or treated as part of the Learning/Yolo tooling.

## 2026-05-03 - P3L.5 Group B dynamic tab registration

Scope:
- Started P3L.5 with a registration-only pass for Group B dynamic tabs.
- Avoided `BeeCore/Unit/Patterns.cs`, `Pattern/Pattern2.cpp`, `Pattern/Pattern2.h`, and the dirty `ToolPattern` files because Pattern2 work is already active in the working tree.
- Did not edit any Group B Designer files.

Changes:
- Added `BeeInterface/Tool/_Base/GroupBToolTabRegistrar.cs`.
- Registered `Preset` tabs for `Pattern`, `Patterns`, `MultiPattern`, `MatchingShape`, `Pitch`, `Barcode`, `OCR`, and `CraftOCR`.
- Registered `Preprocess` tabs for `Pattern`, `Patterns`, `MultiPattern`, `MatchingShape`, `Pitch`, `OCR`, and `CraftOCR`.
- Wired `GroupBToolTabRegistrar.RegisterDefaults()` into app startup in `Program.cs`.
- Updated `BeeInterface/BeeInterface.csproj` to compile the registrar.

Implementation notes:
- The registrar uses the existing `ToolTabRegistry`, `ResultMiniGrid`, and `ScoreThresholdBar`.
- Current Group B UCs still inherit `UserControl`, so these tabs become visible only when a Group B tool moves onto `ToolViewBase` or explicitly hosts registry tabs.
- This pass intentionally does not bind new tab controls back into persisted engine payloads.

Verification:
- Focused compile passed with Roslyn C# 7.3 for the registrar plus required base/shared-control sources.
- The compile emitted expected CS0436 duplicate-type warnings because the focused check references the existing `BeeInterface.dll` while also compiling the new base-tab source files.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches.
- Full solution build remains blocked by the previously recorded OpenCvSharp reference errors and unrelated dirty tree state.

Known state:
- P3L.5 is only scaffolded safely; per-tool Group B runner extraction and real tab hosting still need follow-up once Pattern2 conflicts are resolved.

Follow-up fix:
- Fixed missing `GroupBToolTabRegistrar` compile inclusion in `BeeInterface/BeeInterface.csproj`.
- Removed the direct startup dependency from `Program.cs`.
- Moved `GroupBToolTabRegistrar.RegisterDefaults()` into `ToolViewBase.BuildDynamicTabs()` so registration lives with the tab host.
- Focused compile passed for `ToolViewBase` + Group B registrar integration. Expected duplicate-type warnings remained because the focused check references the existing `BeeInterface.dll` while compiling the same source types.

## 2026-05-03 - P3L.6 dedup discovery blocked

Scope:
- Inspected P3L.6 duplicate targets before deleting or merging files.
- No code files were removed in this step.

Findings:
- `CustomGui.cs` currently exists in:
  - `BeeCore/Func/CustomGui.cs` as `BeeCore.CustomGui`,
  - `BeeInterface/CustomGui.cs` as `BeeInterface.Gui`.
- The old preview expected three `CustomGui.cs` files, but this working tree has two and they expose different public type names/namespaces.
- `Converts.cs` currently exists in:
  - `BeeCore/Converts.cs` as `BeeCore.Convert2`,
  - `BeeCore/Func/Converts.cs` as `BeeCore.Func.Converts`.
- These are not direct duplicate classes; removing either would require reference updates and a clean build baseline.
- `ShapeEditing` still has overlap between `BeeCore/ShapeEditing` and `BeeInterface/ShapeEditing`, but the Interface side contains large UI implementations while Core contains small abstractions/helpers. A safe merge requires a full reference map and buildable baseline.

Decision:
- P3L.6 is blocked for mutation in this dirty working tree.
- Do not delete duplicate-looking files until the full solution can build and the public API/reference map is confirmed.

Verification:
- PowerShell discovery found the duplicate candidates listed above.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches from the prior P3L.5 verification.

Known state:
- P3L.6 remains a cleanup follow-up, not completed.
- The safe next cleanup is to create an API usage map for `BeeCore.CustomGui`, `BeeInterface.Gui`, `BeeCore.Convert2`, and `BeeCore.Func.Converts`, then migrate call sites one class at a time after the build baseline is restored.

## 2026-05-03 - BeeInterface Debug x64 build baseline restored

Scope:
- Responded to the compile error: `GroupBToolTabRegistrar` missing from the current context.
- Continued verification until `BeeInterface` built under `Debug|x64`.

Changes:
- Added `Tool\_Base\GroupBToolTabRegistrar.cs` to `BeeInterface/BeeInterface.csproj`.
- Removed the direct `Program.cs` dependency on `GroupBToolTabRegistrar`.
- Moved `GroupBToolTabRegistrar.RegisterDefaults()` into `ToolViewBase.BuildDynamicTabs()`.
- Updated `Pattern/Pattern.vcxproj` `Debug|x64` to match available local Barcode/OpenCV dependencies:
  - added `C:\OpenCV4.5\Barcode\include`,
  - added `C:\OpenCV4.5\Barcode\lib`,
  - set C++17,
  - linked `ZXing.lib`,
  - used release CRT/iterator settings because only release `ZXing.lib` is present,
  - linked `opencv_world455.lib`.
- Updated `BeeInterface/BeeInterface.csproj` OpenCvSharp hint paths from missing `C:\Lib\*.dll` to repo-local/package assemblies.
- Updated `BeeInterface/Group/View.cs` Pylon frame wrapping from obsolete `new Mat(...)` to `Mat.FromPixelData(...)`.

Verification:
- `MSBuild BeeInterface\BeeInterface.csproj /t:Build /p:Configuration=Debug /p:Platform=x64 /v:minimal` passed.
- This build also built `Pattern`, `BeeCV`, `PylonCli`, `PLC_Communication`, `BeeGlobal`, and `BeeCore` as dependencies.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches.

Known state:
- Build passes with existing warnings.
- `BeeInterface/Group/View.cs` is not valid UTF-8; the `Mat.FromPixelData` edit was done as a byte-exact replacement to preserve existing file encoding.

## 2026-05-03 - P3L.7 Barcode ToolViewBase pilot

Scope:
- Started the Group B pilot with `ToolBarcode`.
- Kept `BeeCore/Unit/Barcode.cs` and `BeeInterface/Tool/ToolBarcode.Designer.cs` untouched.
- Reused the existing base-tab framework instead of rewriting the Barcode Designer.

Files touched:
- `BeeCore/Func/Engines/BarcodeEngineRunner.cs` (new runner facade).
- `BeeCore/BeeCore.csproj` (compile the new runner).
- `BeeInterface/Tool/_Base/ToolViewBase.cs` (added `RequestDynamicTabsForKind` support).
- `BeeInterface/Tool/ToolBarcode.cs` (inherits `ToolViewBase`, delegates owner score/wait/scan/run work to the runner, and rehosts legacy controls into General / ROI / Params / Result tabs).
- `CODEX_HISTORY.md` (this entry).

Implementation notes:
- Barcode now requests the `"Barcode"` dynamic tab registration, so the existing Group B `Preset` tab can be built by `ToolViewBase`.
- Runtime tab mapping:
  - General: PLC/send-result controls plus Inspect / OK-Cancel.
  - ROI: area editor and scan/choose controls.
  - Params: mode, program index, and offset controls.
  - Result: score threshold and sample preview.
- No symbology combobox exists in the current `ToolBarcode.Designer.cs`; the plan text was stale on that point.

Verification:
- `MSBuild BeeInterface\BeeInterface.csproj /t:Build /p:Configuration=Debug /p:Platform=x64 /v:minimal` via VS 2022 MSBuild absolute path: pass.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches.
- `ToolBarcode.cs` event pair check: `StatusToolChanged` `+=` count = `-=` count = 1; `ScoreChanged` `+=` count = `-=` count = 1.
- `git diff -- BeeInterface/Tool/ToolBarcode.Designer.cs`: empty.

Blockers / left dirty:
- Manual smoke was not run because no representative `.prog` was provided/opened in this turn.
- The worktree still contains many pre-existing dirty files from earlier P3L passes, including Pattern2 and Group A runner work; do not treat them as part of this Barcode pilot.

## 2026-05-03 - P3L.8 ToolCounter bypass

Scope:
- Resolved the user decision for ToolCounter as bypass/deprecate.
- Did not migrate Counter to `ToolViewBase`.
- Did not touch `BeeCore/Unit/Counter.cs`.

Files touched:
- `BeeInterface/Tool/ToolCounter.cs` (marked the control obsolete).
- `CODEX_HISTORY.md` (this entry).

Implementation notes:
- `ToolCounter` is now annotated `[Obsolete("Bypassed; use the Learning/Yolo workflow instead.")]`.
- No active `TypeTool.Counter` enum value exists in `BeeGlobal/Enums.cs`.
- No `new ToolCounter(...)` or palette registration path was found; `Load.NewTool()` already exposes Learning/MultiLearning and does not expose Counter. Therefore there was no Counter button to remove.

Verification:
- `MSBuild BeeInterface\BeeInterface.csproj /t:Build /p:Configuration=Debug /p:Platform=x64 /v:minimal` via VS 2022 MSBuild absolute path: pass.

Blockers / left dirty:
- Counter source remains in the project for compatibility and future cleanup, but it is bypassed from the active tool workflow.
- The broader dirty worktree remains unchanged outside this scoped marker.

## 2026-05-03 - P3L.9 Persistence round-trip safety net

Scope:
- Added a focused persistence test harness for `Access.SaveProg` / `Access.LoadProg`.
- Used a synthesized program because no representative `.prog` fixture was provided in this turn.
- Did not touch any `BeeCore/Unit/*.cs` payload layout.

Files touched:
- `tests/BeeCore.Persistence.Tests/BeeCore.Persistence.Tests.csproj` (new .NET Framework x64 console harness).
- `tests/BeeCore.Persistence.Tests/Program.cs` (new tests).
- `EasyVision.sln` (added the test project for IDE discoverability).
- `CODEX_HISTORY.md` (this entry).

Implementation notes:
- Avoided adding xUnit/NuGet dependencies in the dirty worktree; the harness is a normal console executable that exits nonzero on test failure.
- The synthetic program contains Circle, Width, Pattern (`Patterns`), and Yolo payloads inside `PropetyTool` wrappers.
- Tests implemented:
  - `LoadProgram_ReturnsSameToolCount`.
  - `RoundTrip_PreservesPropetyToolFrozenKeys`.
  - `RoundTrip_PreservesScore`.
- The test project copies runtime DLLs from `BeeCore/bin/<platform>/<configuration>` plus existing OpenCV native DLLs from `bin/<configuration>` so payload constructors can run outside the main app output folder.

Verification:
- Build: `MSBuild tests\BeeCore.Persistence.Tests\BeeCore.Persistence.Tests.csproj /t:Build /p:Configuration=Debug /p:Platform=x64 /v:minimal` passed.
- Run: `tests\BeeCore.Persistence.Tests\bin\x64\Debug\BeeCore.Persistence.Tests.exe` passed all 3 tests.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches.

Blockers / left dirty:
- This is not an xUnit project because the repo has no xUnit packages and adding a new NuGet dependency was avoided.
- No committed `.prog` fixture was added; future work can add a curated `tests/testdata/sample.prog` once the user selects a representative project.
- Generated `tests/BeeCore.Persistence.Tests/bin` and `obj` folders were removed after verification.

## 2026-05-03 - P3L.11 OCR ToolViewBase pilot

Scope:
- Migrated `ToolOCR` as the second Group B pilot after user selected OCR.
- Skipped P3L.10 doc-only usage maps for this turn because the user explicitly requested `ToolOCR`.
- Kept `BeeCore/Unit/OCR.cs` and `BeeInterface/Tool/ToolOCR.Designer.cs` untouched.

Files touched:
- `BeeCore/Func/Engines/OCREngineRunner.cs` (new runner facade).
- `BeeCore/BeeCore.csproj` (compile the new runner).
- `BeeInterface/Tool/ToolOCR.cs` (inherits `ToolViewBase`, delegates owner score/wait/model-load/run work to the runner, and rehosts legacy controls into General / ROI / Params / Result tabs).
- `CODEX_HISTORY.md` (this entry).

Implementation notes:
- OCR now requests the `"OCR"` dynamic tab registration, so the existing Group B `Preset` and `Preprocess` tabs are built by `ToolViewBase`.
- Runtime tab mapping:
  - General: Inspect / OK-Cancel.
  - ROI: ROI visibility button plus `EditRectRot`.
  - Params: allow-list, matching content, limit-area, PLC value controls, and legacy filter panel.
  - Result: score threshold.
- Existing `StatusToolChanged` and `ScoreChanged` subscriptions retain the required `-=` then `+=` pattern.

Verification:
- `MSBuild BeeInterface\BeeInterface.csproj /t:Build /p:Configuration=Debug /p:Platform=x64 /v:minimal` via VS 2022 MSBuild absolute path: pass.
- `MSBuild tests\BeeCore.Persistence.Tests\BeeCore.Persistence.Tests.csproj /t:Build /p:Configuration=Debug /p:Platform=x64 /v:minimal`: pass.
- `tests\BeeCore.Persistence.Tests\bin\x64\Debug\BeeCore.Persistence.Tests.exe`: pass, 3/3 tests.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches.
- `ToolOCR.cs` event pair check: `StatusToolChanged` `+=` count = `-=` count = 1; `ScoreChanged` `+=` count = `-=` count = 1.
- `git diff -- BeeInterface/Tool/ToolOCR.Designer.cs`: empty.

Blockers / left dirty:
- Manual smoke was not run because no representative OCR `.prog` was opened in this turn.
- Generated `tests/BeeCore.Persistence.Tests/bin` and `obj` folders were removed after verification.
- P3L.10 remains open and should still be done before deeper dedup work.

## 2026-05-03 - P3L.10 CustomGui / Converts usage maps

Scope:
- Completed the doc-only P3L.10 unblocker after P3L.11.
- No code or project files were changed.
- Focused on duplicate-looking helpers that P3L.6 previously blocked from safe deletion.

Files touched:
- `docs/architecture/preview/CUSTOMGUI_USAGE_MAP.md` (new).
- `docs/architecture/preview/CONVERTS_USAGE_MAP.md` (new).
- `CODEX_HISTORY.md` (this entry).

Findings:
- `BeeCore/Func/CustomGui.cs` exposes `BeeCore.CustomGui` and still has active callers in `BeeInterface/Group/ToolSettings.cs` and `BeeUi/Unit/EditTool.cs`.
- `BeeInterface/CustomGui.cs` exposes `BeeInterface.Gui` with the same helper implementation but no active call sites found in current source.
- `BeeCore/Converts.cs` exposes `BeeCore.Convert2.NumberFromString(...)` and is actively used by `BeeCore/Unit/Yolo.cs`.
- `BeeCore/Func/Converts.cs` exposes `BeeCore.Func.Converts` and is actively used by Barcode, CheckMissing, ColorArea, CraftOCR, MultiPattern, Patterns, PositionAdj, and VisualMatch engine paths.
- `ShapeEditing` overlap was documented in the CustomGui map because current usage is UI-hosted through `BeeInterface.ShapeEditing.ImageCanvasControl` in `View.Designer.cs`, while `BeeCore.ShapeEditing` contains contracts/state/helper abstractions.

Verification:
- PowerShell reference discovery was run across non-bin/obj C# files for `CustomGui`, `Gui`, `Convert2`, `Converts`, and `ShapeEditing` namespace usage.
- Project inclusion was checked in `BeeCore/BeeCore.csproj` and `BeeInterface/BeeInterface.csproj`.
- No build was run because this card is documentation-only and changes no compiled files.

Known state:
- P3L.10 does not merge or delete duplicate files; it only records the safe migration recipes.
- Later dedup should not replace `Convert2.NumberFromString(...)` with `Converts.StringtoDouble(...)` without checking semantics, because they parse different formats.

## 2026-05-03 — FIX.1 OpenCvSharp version mismatch [STARTED]

Scope:
- Runtime exception: `Method not found: 'OpenCvSharp.Scalar OpenCvSharp.Scalar.op_Implicit(Double)'`.
- Root cause: `BeeCore.csproj` and `BeeUi.csproj` still pin `OpenCvSharp, Version=1.0.0.0, PublicKeyToken=6adad1e807fea099` (legacy 3.x signed package) and HintPath `..\BeeCore\bin\x64\Release\OpenCvSharp.dll` — a stale 3.x DLL left in the build output. Meanwhile `BeeGlobal` and `BeeInterface` reference the modern NuGet `OpenCvSharp4 4.11.0.20250507` (unsigned). Mixed metadata = at runtime, calls compiled against 4.x bind to 3.x and fail.
- `OpenCvSharp.UserInterface` does not exist in 4.x — grep confirms zero source code uses `using OpenCvSharp.UserInterface` / `CvWindow` / `PictureBoxIpl`. Safe to remove.
- No `app.config` pins the 3.x PublicKeyToken, so no binding redirect needs adjustment.

Plan:
- [ ] Patch BeeCore.csproj — replace 3 OpenCvSharp references with NuGet 4.11 paths; drop UserInterface entirely.
- [ ] Patch BeeUi.csproj — same as BeeCore.
- [ ] Patch BeeGlobal.csproj — drop UserInterface reference (already on NuGet).
- [ ] Patch BeeInterface.csproj — drop UserInterface reference (already on NuGet).
- [ ] Confirm BeeCore + BeeUi packages.config already lists OpenCvSharp4 4.11.x.
- [ ] Delete stale OpenCvSharp*.dll under all `bin/` (keep `packages/` copies).
- [ ] MSBuild verify Debug|x64 BeeInterface.

## 2026-05-03 — DOC.1 FULL_PROJECT_REORG_PLAN.md hardening

Scope:
- Doc-only edit on `docs/architecture/preview/FULL_PROJECT_REORG_PLAN.md`.
- Goal: enforce read-history-first + execute-once + per-step append rules so future Codex/Claude agents do not repeat completed work or skip the history log.
- No source code, csproj, or build outputs touched.

Files touched:
- `docs/architecture/preview/FULL_PROJECT_REORG_PLAN.md` (4 edits, ~+150 / −60 net lines).
- `CODEX_HISTORY.md` (this entry).

Changes:
- Section 0 rewritten as "Hard Rules for Codex / Claude". Added §0.1 mandatory pre-flight (READ → DO → RECORD), §0.2 execute-once rule, §0.3 per-step history append spec, §0.4 (preserves original user goals), §0.5 forbidden actions, §0.6 stop-and-ask triggers. P3L.5 cited as cautionary example of partial completion.
- Section 1 snapshot updated to reflect P3L.7 → P3L.12 completion. Open / blocked list trimmed to truly open items (P3L.13 Release baseline, Pattern2 conflict, real `.prog` fixture, dirty Edge/Width files, non-UTF8 View.cs).
- Section 2 Block 0 table now has a Status column. 6 of 7 tasks marked ✅ Done; only P3L.13 remains. Exit-criteria checklist annotated with status.
- Section 12 split into §12.1 Resolved (4 items: ToolCounter=bypass, OCR-first, LOC=250, console harness) and §12.2 Open (9 questions: Pattern2 freeze, Group B order, View.cs encoding, Edge/Width edits, CI timing, OpenVINO, .prog consumers, Propety rename, real fixture).
- Section 13 working rules expanded into 5 sub-sections: §13.1 read-do-record loop diagram, §13.2 execute-once enforcement, §13.3 per-step append spec, §13.4 invariant-protection rules, §13.5 process rules.

Verification:
- `Read` on the file confirmed all 16 top-level sections still present (Grep `^## \d+\.` returns 16 hits at expected line numbers).
- No build was run because this card is documentation-only and changes no compiled files.
- This history entry itself follows the §0.3 per-step append spec (started as `[STARTED]`, finalized at task end).

Notes for future agents:
- The per-step append rule (§0.3 / §13.3) is the most important new rule. Honor it — do not batch all writes to CODEX_HISTORY.md at the end of a task.
- §0.2 execute-once rule means: before starting any P-task, grep `CODEX_HISTORY.md` for the ID. If found, STOP and ask the user whether to treat as a follow-up fix (`<id>-fix1`) or skip.
- Open questions are now in §12.2; do not surface §12.1 items again.

Blockers / left dirty:
- None for this doc-only task. The pre-existing dirty worktree state (Pattern2, Edge.cs, Width.cs, etc.) is unchanged.

## 2026-05-03 - P3L.12 Tool slimming protocol + Circle split

Scope:
- Continued the full plan after P3L.10/P3L.11.
- Defined the tool slimming protocol and applied the first pass to `ToolCircle`.
- Kept `BeeCore/Unit/Circle.cs` and the serialized `Circle` payload layout untouched.
- Did not intentionally edit `BeeInterface/Tool/ToolCircle.Designer.cs`; it already has unrelated local diffs in this dirty worktree.

Files touched:
- `docs/architecture/preview/TOOL_SLIMMING_PROTOCOL.md` (new).
- `BeeInterface/Tool/ToolCircle.cs` (main lifecycle reduced).
- `BeeInterface/Tool/ToolCircle.Roi.cs` (new partial for crop/area/mask/shape behavior).
- `BeeInterface/Tool/ToolCircle.Parameters.cs` (new partial for score/status/parameter/layout handlers).
- `BeeInterface/BeeInterface.csproj` (compile the new partial files).
- `CODEX_HISTORY.md` (this entry).

Implementation notes:
- `ToolCircle.cs` now owns constructor, owner cache, `LoadPara`, selected-tool run shell, and empty designer stubs.
- ROI shape behavior stayed UI-owned in `ToolCircle.Roi.cs` because it depends on WinForms control state and `Global.TypeCrop`.
- Parameter/status handlers stayed UI-owned in `ToolCircle.Parameters.cs`; no persisted fields or engine algorithms were moved.
- `ToolCircle` main file line count is now 136 by the protocol command, under the 250-line ceiling.
- Existing `StatusToolChanged` and `ScoreChanged` subscriptions retain paired `-=` / `+=` in `LoadPara`.

Verification:
- `MSBuild` was not on PATH, so the VS 2022 MSBuild absolute path was used.
- `MSBuild BeeInterface/BeeInterface.csproj /t:Build /p:Configuration=Debug /p:Platform=x64 /v:minimal`: pass with existing warnings.
- `MSBuild tests/BeeCore.Persistence.Tests/BeeCore.Persistence.Tests.csproj /t:Build /p:Configuration=Debug /p:Platform=x64 /v:minimal`: pass.
- `tests/BeeCore.Persistence.Tests/bin/x64/Debug/BeeCore.Persistence.Tests.exe`: pass, 3/3 tests.
- PowerShell guard for `Common.PropetyTools[` outside `BeeCore/Common.cs`: 0 matches.

Known state:
- Generated `tests/BeeCore.Persistence.Tests/bin` and `obj` folders were removed after verification.
- `ToolCircle.Designer.cs` still shows pre-existing unrelated local diffs; they were not reverted.
- This pass is a structural split, not a designer cleanup and not a full extraction of ROI UI state into reusable controls.

## 2026-05-03 - P3L.13 Release x64 baseline restored

Scope:
- Continued the remaining full-plan step after P3L.12.
- Restored and documented the full solution `Release|x64` baseline.
- Kept this scoped to build/reference/pre-build fixes and documentation.

Files touched:
- `tools/update_native_version.ps1` (new shared native version-header helper).
- `BeeCV/BeeCV.vcxproj` (uses the shared version helper).
- `OKNG/OKNG.vcxproj` (uses the shared version helper).
- `BeeCore/BeeCore.csproj` (Release-compatible local OpenCvSharp/LibUsb hint paths).
- `BeeUi/BeeUi.csproj` (Release-compatible local OpenCvSharp hint paths).
- `docs/architecture/baseline_build.md` (new baseline summary).
- `docs/architecture/baseline_build_release.log` (new saved Release build log).
- `CODEX_HISTORY.md` (this entry).

Implementation notes:
- First Release attempt exposed native `version.h` write/quoting problems in `BeeCV` and `OKNG`.
- Replaced the fragile inline pre-build PowerShell commands with `tools/update_native_version.ps1`.
- `BeeCore` cannot use the current NuGet OpenCvSharp assembly without broader source changes because that assembly turns legacy `Mat(...)` constructors into compile-time errors. For this baseline, `BeeCore` and `BeeUi` use the existing compatible local OpenCvSharp assemblies from `BeeCore/bin/x64/Release`.
- No serialized `BeeCore/Unit/*` payload fields were changed.

Verification:
- Full command: `MSBuild EasyVision.sln /m:1 /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal /flp:logfile=docs/architecture/baseline_build_release.log;verbosity=minimal`.
- Result: pass.
- Saved log summary: 0 errors, 447 warnings, 15 built outputs.
- Release persistence run: `tests/BeeCore.Persistence.Tests/bin/Release/BeeCore.Persistence.Tests.exe`: pass, 3/3 tests.

Known state:
- Generated `tests/BeeCore.Persistence.Tests/bin` and `obj` folders were removed after verification.
- Native version files are generated/touched by builds and remain part of the existing dirty worktree pattern.
- The Release baseline still has many warnings, including native conversion warnings, architecture mismatch warnings, and package version conflict warnings in `BeeMain`.

## 2026-05-07 - Centralize MethordEdge + 3 phương pháp Extract Edge mới

Scope:
- Refactor toàn bộ pipeline Extract Edge (used by Edge, Edge2, Width, Circle, Pitch, EdgePixel, Corner, AutoTrig, PositionAdj, Patterns, MultiPattern, MultiOnnx, CheckMissing, Intersect, Tool2Edge).
- Mục tiêu: gom switch trùng ở 14 nơi → 1 method centralizer; thêm 3 phương pháp mới hỗ trợ hình phức tạp (metallic/uneven lighting/thin edges); fix bug `StrongEdges` ≡ `Stable` ở `Edge.cs` cũ.

Files touched:
- `BeeGlobal/Enums.cs` — `MethordEdge` enum: thêm 3 value `UltraThin`, `Adaptive`, `DenoiseFirst` (cuối enum để giữ ordinal cũ — backward-compat serialization OK).
- `BeeCore/Algorithm/Filters.cs` — thêm 2 method `public static`:
  - `AdaptiveEdge(Mat raw)` — Cv2.AdaptiveThreshold Gaussian + Canny + morph close, blockSize auto từ kích thước ảnh.
  - `ApplyEdgeMethod(Mat src, MethordEdge method, int thresholdBinary = 127)` — centralizer chuyển hướng tới method tương ứng.
- `BeeInterface/EdgeButtonsHelper.cs` (NEW) — static helper class gắn 3 button (UltraThin/Adaptive/Denoise) lên row mới của TableLayoutPanel; có `ExtraButtons.ResetAll()` và `Highlight(MethordEdge)`.
- `BeeInterface/BeeInterface.csproj` — đăng ký `EdgeButtonsHelper.cs`.
- 14 Unit files (`BeeCore/Unit/*.cs`): thay switch 4-5 case bằng 1 dòng `Filters.ApplyEdgeMethod(...)`.
  - Edge.cs, Edge2.cs, Width.cs, Circle.cs, Pitch.cs, EdgePixel.cs, MeasureCorner.cs, AutoTrig.cs, PositionAdj.cs, Patterns.cs, CheckMissing.cs (2 switch), MultiPattern.cs (2 switch — DoWork inline + Processing wrapper), MultiOnnx.cs (Processing wrapper, giữ nhánh đặc biệt `EdgeForCenterline` + `GetStrongEdgesOnly(thresh)`), Intersect.cs.
- 9 Tool UI files (`BeeInterface/Tool/Tool*.cs`): thêm field `_extraEdgeBtns`, gắn helper trong constructor, cập nhật `LoadPara` switch để reset state + 3 case mới gộp gọi `Highlight()`.
  - ToolEdge / Tool2Edge / ToolWidth / ToolCircle / ToolPitch / ToolEdgePixel / ToolCorner / ToolAutoTrig / ToolPosition_Adjustment.

Implementation notes:
- 3 phương pháp mới mapping:
  - `UltraThin` → `Filters.GetUltraThinEdgesFast(src)` (đã tồn tại — morphological gradient cho cạnh 1px).
  - `Adaptive` → `Filters.AdaptiveEdge(src)` (mới — adaptive Gaussian threshold cho ánh sáng không đều).
  - `DenoiseFirst` → `Filters.RemoveWhiteNoiseThenEdge(src)` (đã tồn tại — xoá glare/noise trước rồi edge).
- **Bug fix:** Trước đây trong `BeeCore/Unit/Edge.cs`, case `MethordEdge.StrongEdges` và `MethordEdge.Stable` cùng gọi `GetStrongEdgesStable()` (2 case giống hệt). Inconsistent với `Width.cs` dùng `GetStrongEdgesOnly()` cho `StrongEdges`. Sau refactor: cả 2 đều qua `ApplyEdgeMethod` — `StrongEdges` → `GetStrongEdgesOnly` (percentile), `Stable` → `GetStrongEdgesStable` (Sobel + Otsu) đúng semantic.
- `MultiOnnx.Processing` và `MultiPattern.Processing` giữ nhánh đặc biệt cho `CloseEdges` (EdgeForCenterline) và `StrongEdges` (custom thresh param), còn lại uỷ thác centralizer qua `default:`.
- UI helper dùng pattern programmatic add row vào TableLayoutPanel có sẵn (panel name khác nhau giữa các tool: `tableLayoutPanel15` / `lay31` / `lay61` / `layEdge`) → KHÔNG sửa Designer.cs để tránh rủi ro break designer.
- Threshold panel name không đồng nhất (`layThreshod` ở hầu hết tool, `lay32` ở ToolCircle, `lay62` ở ToolWidth) → mỗi tool truyền lambda riêng.

Known state:
- 9 file khác trong `BeeCore/Unit/` còn dùng `case MethordEdge.` riêng đã được centralize. `MultiOnnx.cs` và `MultiPattern.cs` có 1 switch còn case literal vì giữ nhánh đặc biệt.
- Build verify: chưa chạy ở pass này (đề nghị user chạy `MSBuild EasyVision.sln /p:Configuration=Release /p:Platform=x64`).
- Smoke test cần thiết: load 1 project mẫu, mở từng tool có Extract Edge, click qua 8 button (cũ + 3 mới), confirm UI không exception và inspection chạy đúng.
- Files Edge2.cs / Tool2Edge.{cs,Designer.cs,resx} / Edge2EngineRunner.cs vẫn untracked — user sẽ commit/track.

Notes for future agents:
- Pattern clone tool: `Edge2` (suffix Unit) ↔ `Tool2Edge` (prefix Form). Khi update Edge logic, **luôn** apply cho cả `Edge.cs` + `Edge2.cs` và `ToolEdge.cs` + `Tool2Edge.cs`. Chi tiết ở `~/.claude/projects/E--Code-EasyVision-Unisen/memory/project_clone_tools.md`.
- Switch `MethordEdge` mới chỉ còn ở `MultiOnnx.Processing` và `MultiPattern.Processing` (cần giữ thresh param + EdgeForCenterline) — đừng centralize tiếp 2 chỗ này.
- Khi thêm phương pháp Extract Edge mới: chỉ sửa 3 nơi: enum (Enums.cs), case trong `Filters.ApplyEdgeMethod`, và (nếu muốn expose UI) thêm button trong `EdgeButtonsHelper.Make/Attach` + trường tương ứng trong `ExtraButtons.Highlight`.

## 2026-05-07 - Fix RANSAC line detect khi crop ROI lớn + nhiễu rìa

Scope:
- Fix bug `RansacLine.FindLongestParallelPair` (Edge2 / Tool2Edge) bỏ qua line khi ROI lớn có noise rìa (vành tròn ngoài, glare metallic).
- User report: ROI nhỏ vừa khít → detect được; ROI lớn cùng vật → KHÔNG detect.

Files touched:
- `Pattern/RansacLineCore.cpp` line 1031-1034: đổi `imageDiag` → `std::min(W, H)` cho `minLenPx`.
- `BeeCore/Unit/Edge2.cs` line 157-158: bật pre-filter `Filters.ClearNoiseLengh(matProcess, SizeClearBig)` khi `IsClearNoiseBig=true` (trước đó comment-out).

Root cause:
- `FindLongestParallelPair` dùng `imageDiag = sqrt(W²+H²)` để tính `minLenPx = minLengthRatio × imageDiag`. Với `AspectLen=0.6f` mặc định + crop 1000×1000 → minLenPx = 848px. Line vật lý ~300px → reject ở line 1071. Crop nhỏ → diag nhỏ → cùng line vật lý pass.
- Hàm chị em `FindBestLine` (line 1323) dùng `ComputeMinLenPx` với `ratio × W` hoặc `ratio × H` tuỳ direction — nhất quán hơn. Sửa `FindLongestParallelPair` dùng `min(W,H)` đồng nhất ngữ nghĩa.
- Pre-filter `IsClearNoiseBig` trước đó bị comment-out → user enable checkbox cũng không tác dụng. Bây giờ dùng `ClearNoiseLengh` (filter theo bounding-box length) — phù hợp loại fragment ngắn ở rìa noise, giữ line dài thật.

Implementation notes:
- `FindLongestParallelPair` chỉ có 1 caller (`Edge2.cs` qua `RansacLine.FindLongestParallelPair` CLI binding) → đổi semantic minLenPx an toàn, không impact tool khác.
- Sau fix: `AspectLen=0.6` với crop 1000×1000 → minLenPx = 600px (vs 848 trước đó). Crop hẹp 200×1500 → minLenPx = 120px (vs 1513 — gần như loại mọi line).
- Cần rebuild native `Pattern.dll` cho fix có hiệu lực: `MSBuild EasyVision.sln /t:Pattern:Build /p:Configuration=Release /p:Platform=x64`.

Verification:
- Build native chưa chạy ở pass này.
- User cần smoke test: load lại project mẫu có Edge2 với ROI lớn, click Test → confirm detect được parallel lines.
- Nếu vẫn fail: tăng `RansacIterations` (200 → 800-1500), giảm `AspectLen` (0.6 → 0.3), bật `IsClearNoiseBig` với `SizeClearBig` 30-100.

Notes for future agents:
- Khi tune RANSAC line: parameter `AspectLen` (= `minLengthRatio` native) là ratio so với `min(W,H)` chứ KHÔNG phải diagonal nữa. Update doc/UI hint nếu có.
- Với ROI noise dày: nên enable `IsClearNoiseBig` (UI checkbox) — gọi `ClearNoiseLengh` filter contour length. Khác với `IsClearNoiseSmall` (= `ClearNoise` filter area).
- Nếu user tiếp tục báo fail RANSAC: cân nhắc adaptive iterations (tăng theo số non-zero points), hoặc thêm pre-step `Cv2.Dilate` 1-2px để dày line vs noise.

## 2026-05-07 - Fix Edge/Edge2 listP_Center bỏ rotation (Measure vẽ lệch)

Scope:
- User report: Tool Measure vẽ line màu đỏ KHÔNG khớp với line tím Edge2 detect → góc đo lệch khi ROI Edge2 bị xoay (vd inherit rotation từ Position_Adjustment).

Files touched:
- `BeeCore/Unit/Edge2.cs` line 179-182: thay phép tịnh tiến đơn giản `(PosCenter - rect/2 + p)` bằng `Matrix(Translate→Rotate→Translate).TransformPoints` — đồng nhất với DrawResult line 416-418.
- `BeeCore/Unit/Edge.cs` line 165-166: cùng fix.

Root cause:
- `Edge2.DrawResult` vẽ line dùng `mat.Translate(PosCenter) → mat.Rotate(_rectRotation) → mat.Translate(_rect.X, _rect.Y)` → line vẽ đúng ngay cả khi ROI xoay.
- Nhưng `listP_Center` lưu global coords lại tính bằng `PosCenter.X - W/2 + X1` → **chỉ tịnh tiến, KHÔNG xoay**. Khi `_rectRotation ≠ 0`, listP_Center sai vị trí.
- Measure đọc `tool.Propety2.listP_Center[0..1]` (qua `TryCopyLineFromToolPoints`) → vẽ line đỏ tại vị trí không-xoay → lệch khỏi line tím Edge2 → góc Measure tính sai (sai cả số lẫn vị trí).
- `_rect.X = -W/2`, `_rect.Y = -H/2` (xem `BeeGlobal/RectRotate.cs:157`) — nên Matrix.Translate(_rect.X, _rect.Y) tương đương translate(-W/2, -H/2). Fix đúng convention chuẩn.

Implementation notes:
- Dùng `System.Drawing.Drawing2D.Matrix.TransformPoints` để khớp **đúng** sequence transform với DrawResult — tránh sai số khi tự viết rotation matrix bằng tay.
- Fix áp dụng cho cả Edge.cs (cùng bug pattern) — ngay cả nếu user hiện không dùng Edge với ROI xoay, fix proactive đảm bảo nhất quán.
- Khi rotation = 0: kết quả y hệt code cũ (Matrix với rotation 0 = identity rotation).

Verification:
- User cần test lại với ROI Edge2 inherit rotation từ Position_Adjustment → confirm line đỏ Measure overlap chính xác lên line tím Edge2 → góc đo đúng.
- Build: `MSBuild EasyVision.sln /p:Configuration=Release /p:Platform=x64`. Pure C# fix, không cần rebuild native.

Notes for future agents:
- **Convention chung trong repo**: local point trong matProcess (ROI-cropped image) → global = `Matrix(Translate(PosCenter) * Rotate(_rectRotation) * Translate(_rect.X, _rect.Y))` áp dụng. KHÔNG dùng phép tịnh tiến đơn giản nếu ROI có thể xoay.
- Tool nào output `listP_Center` (Edge, Edge2, Circle, Pitch, ...) cần check có apply rotation không. Nếu không, mọi consumer (Measure, Position_Adjustment, ...) sẽ vẽ/tính sai khi ROI xoay.
- `_rect.X/Y` = `-W/2, -H/2` luôn (per `RectRotate.cs:157`), nên `mat.Translate(_rect.X, _rect.Y)` ≡ shift tới ROI center.

## 2026-05-07 - PinPitch/Width planning handoff

Scope:
- Added planning docs for pin pitch measurement and point-to-line distance measurement.
- No code implementation yet; markdown/session/history only.

Decisions:
- Use existing `TypeTool.Pitch`; add `PitchMeasureMode.PinPitch` for P1..P4 center detection and pin pitch values.
- Use existing `TypeTool.Width`; add `WidthMeasureMode.PointToLine` for one selected point/pin center to one line `L`.
- Do not put distance-to-line controls/results inside Pitch. Pitch owns centers/pitch; Width owns distance/width.
- Use existing `RansacLine.FindBestLine` for reference line `L`; do not duplicate RANSAC line fitting.
- C++ native should use independent `PinPitchCore` + thin `PinPitchCli`; optional `VisionGeometryCore` for shared geometry helpers.
- Do not inherit `PinPitchCore` from existing `PitchCore`; `PitchCore` remains for crest/root profile pitch.

Planning files:
- `docs/PinPitchMeasurePlan/AGENTS.md`
- `docs/PinPitchMeasurePlan/Plan.md`
- `docs/PinPitchMeasurePlan/CoreGuiImplementationPlan.md`
- `docs/PinPitchMeasurePlan/pin-pitch-map.md`

Next implementation order:
1. `PP-001`: native `PinPitchCore/Cli` center detector.
2. `PP-002`: Width `PointToLine` core mode.
3. `PP-003`: line `L` detection using `RansacLine.FindBestLine`.
4. `PP-004/PP-005`: result scoring and GUI for Pitch/Width.

Build:
- Not run. Changes are markdown-only.

## 2026-05-07 - Agent permission and English documentation rule

Scope:
- Updated repository agent rules and PinPitch planning docs.
- No code implementation; documentation/session/history only.

Changes:
- Added `Agent Permission and Language Rules` to root `AGENTS.md`.
- Agents have standing permission for in-scope inspection, edits, map/session updates, and non-destructive verification commands without asking first.
- Agents should only ask when a repository stop condition is hit, destructive filesystem/git-history work is requested, out-of-scope fixes are required after a failed build, or ambiguity would make a reasonable assumption risky.
- All agent-facing documentation must be written in English: plans, map entries, AGENTS files, session logs, history notes, implementation notes, comments for future agents, and handoff notes.
- User-facing final summaries can remain in the user's language unless requested otherwise.
- Updated `docs/PinPitchMeasurePlan/AGENTS.md` with the English-only rule for that planning folder.
- Converted remaining Vietnamese planning text in `CoreGuiImplementationPlan.md` and `Plan.md` to English.

Build:
- Not run. Changes are markdown-only.

## 2026-05-07 - PP-001 native PinPitch detector

Scope:
- Implemented the native PinPitch detector foundation for ToolPitch PinPitch mode.

Files added:
- `Pattern/VisionGeometryCore.h`
- `Pattern/VisionGeometryCore.cpp`
- `Pattern/PinPitchCore.h`
- `Pattern/PinPitchCore.cpp`
- `Pattern/PinPitchCli.h`
- `Pattern/PinPitchCli.cpp`

Files updated:
- `Pattern/Pattern.vcxproj`
- `Pattern/version.h`
- `Pattern/.version.hash`
- `docs/PinPitchMeasurePlan/pin-pitch-map.md`

Implementation notes:
- `PinPitchCore` is independent from existing `PitchCore`; no inheritance was added.
- `PinPitchCli` is a thin C++/CLI wrapper in namespace `BeeCppCli`, matching existing `PitchCli` style.
- `VisionGeometryCore` contains reusable distance/projection/sort/fit-line helpers.
- The detector accepts 1/3/4-channel 8-bit input, builds a bright-object mask with Otsu or manual threshold, filters contour candidates by area/aspect/fill ratio, estimates centers primarily from rotated contour geometry, and falls back to weighted centroid only when geometry is weak.
- The result exposes pins, adjacent pitch values, P1-P4 span, row line, row residual, scale, and optional debug BGR buffer with `FreeBuffer` ownership.

Verification:
- Release x64 build passed.
- Warning count: 424 existing-style warnings.

Next:
- PP-002: extend Width with `PointToLine` mode and point source selection.

## 2026-05-07 - PP-002/PP-005 C# core and lightweight GUI wiring

Scope:
- Continued PinPitch/Width integration on the managed C# side.

Files updated:
- `BeeGlobal/Enums.cs`
- `BeeCore/Unit/Pitch.cs`
- `BeeCore/Unit/Width.cs`
- `BeeCore/Func/Engines/WidthEngineRunner.cs`
- `BeeInterface/Tool/ToolPitch.cs`
- `BeeInterface/Tool/ToolWidth.cs`
- `docs/PinPitchMeasurePlan/pin-pitch-map.md`

Implementation notes:
- Added `PitchMeasureMode` and `WidthMeasureMode` enums.
- Pitch now has a `PinPitch` core path that calls `BeeCppCli.PinPitchCli`, maps local pin centers back to global coordinates, fills `listP_Center` as P1..P4, stores P12/P23/P34/P1-P4 values, and draws a simple P1..P4 overlay.
- Width now has `PointToLine` mode that resolves one selected source point, detects reference line L through `RansacLine.FindBestLine`, projects the point to L, writes distance to `WidthResult`, and draws point/line/foot/distance overlay.
- ToolPitch now has a lightweight programmatic panel for PeakRoot/PinPitch mode, expected pins, nominal pitch, tolerance, and projected pitch.
- ToolWidth now has a lightweight programmatic panel for ParallelLines/PointToLine mode, point source, point index, nominal distance, and tolerance.
- Designer files were intentionally not edited in this pass to reduce WinForms designer churn.

Verification:
- First Release x64 build passed.
- A warning-count rerun hit the known transient `BeeCV/version.h` prebuild stream error, then passed on rerun.
- Final Release x64 build passed with 424 warnings.

Next:
- Validate with the sample pin image and tune PinPitch threshold/area/fill options.
- Polish ToolPitch/ToolWidth designer layout if the programmatic panels are not acceptable for operators.

## 2026-05-07 - PP-004/PP-005 PinPitch arrange mode

Scope:
- Fixed PinPitch ordering before pitch calculation.

Files updated:
- `Pattern/PinPitchCore.h`
- `Pattern/PinPitchCore.cpp`
- `Pattern/PinPitchCli.h`
- `Pattern/PinPitchCli.cpp`
- `BeeGlobal/Enums.cs`
- `BeeCore/Unit/Pitch.cs`
- `BeeInterface/Tool/ToolPitch.cs`
- `docs/PinPitchMeasurePlan/pin-pitch-map.md`

Implementation notes:
- Replaced the native `sortHorizontal` option with explicit `PinArrangeMode`: `X`, `Y`, and `RowProjection`.
- PinPitch now assigns P1..Pn after arranging detected centers by the selected mode, then computes adjacent pitch and P1-P4 span.
- Managed Pitch now stores `PinPitchArrangeMode` and passes it through the C++/CLI wrapper.
- ToolPitch now exposes an `Arrange` combo box so operators can choose X or Y ordering for the sample orientation instead of relying on line orientation.

Verification:
- Release x64 build passed.
- Warning count: 460 existing-style warnings.

Next:
- Validate `Arrange=X` on the horizontal sample image and use `Arrange=Y` for vertical pin stacks.

## 2026-05-12 - P3X.1.1 BeeNativeSegAI skeleton

Scope:
- Started the Tool Segment AI native foundation from `docs/AiPlan`.

Files updated:
- `BeeNativeSegAI/BeeNativeSegAI.vcxproj`
- `BeeNativeSegAI/pch.h`
- `BeeNativeSegAI/framework.h`
- `BeeNativeSegAI/dllmain.cpp`
- `BeeNativeSegAI/SegAINativeExport.h`
- `BeeNativeSegAI/BeeNativeSegAI.cpp`
- `EasyVision.sln`

Implementation notes:
- Added a standalone `BeeNativeSegAI` x64 dynamic library project using v143/C++17.
- Linked only OpenCV 4.5.5 for the Phase 1 native skeleton.
- Added `SEGAI_GetVersion` and `SEGAI_GetBuildInfo` exports.
- Added a Release x64 post-build copy into the root `bin\x64\Release` output folder, matching the actual `BeeMain.csproj` output path in this repo.

Verification:
- `MSBuild BeeNativeSegAI\BeeNativeSegAI.vcxproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed.
- Build produced `BeeNativeSegAI\x64\Release\BeeNativeSegAI.dll`; generated project artifacts were cleaned after verification, and the copied runtime DLL remains at `bin\x64\Release\BeeNativeSegAI.dll`.
- `dumpbin /EXPORTS` shows `SEGAI_GetVersion` and `SEGAI_GetBuildInfo`.

Next:
- P3X.1.2: implement `SegFeatureExtractor::ExtractCPU` 24-D feature extraction.

## 2026-05-12 - P3X.1.2 SegmentAI CPU feature extractor

Scope:
- Implemented the native CPU feature extraction foundation for Tool Segment AI.

Files updated:
- `BeeNativeSegAI/SegFeatureCore.h`
- `BeeNativeSegAI/SegFeatureCore.cpp`
- `BeeNativeSegAI/BeeNativeSegAI.cpp`
- `BeeNativeSegAI/SegAINativeExport.h`
- `BeeNativeSegAI/BeeNativeSegAI.vcxproj`

Implementation notes:
- Added `BeeSegAI::FeatureConfig` and `BeeSegAI::SegFeatureExtractor`.
- Implemented `ExtractCPU` with the planned 24 feature planes: uniform LBP, HSV mean/std, four Gabor magnitudes, gradient magnitude/orientation, Laplacian, ROI position, three intensity neighborhood means, three edge-density scales, local contrast, and distance-to-ROI-edge.
- Added `PackSamples` and `PlanesToInterleaved` for trainer/inferer use in later tasks.
- Added `SEGAI_RunFeatureSelfTest` as a native diagnostic export that runs feature extraction on a synthetic BGR image and validates plane count, type, size, value range, packing, and ROI interleaving.
- Left `ExtractGpu` as an explicit P3X.1.11 stub so this task stays CPU-only.

Verification:
- `MSBuild BeeNativeSegAI\BeeNativeSegAI.vcxproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed.
- PowerShell/C# P/Invoke smoke call returned `SEGAI_RunFeatureSelfTest=0`.
- Generated project artifacts were cleaned after verification; the copied runtime DLL remains at `bin\x64\Release\BeeNativeSegAI.dll`.

Next:
- P3X.1.3: implement `SegTrainer` and `.segai` save/load round-trip.

## 2026-05-12 - P3X.1.3 SegmentAI trainer and segai file format

Scope:
- Implemented RTrees training and `.segai` model persistence for the native SegmentAI layer.

Files updated:
- `BeeNativeSegAI/SegTrainerCore.h`
- `BeeNativeSegAI/SegTrainerCore.cpp`
- `BeeNativeSegAI/SegAIFileFormat.h`
- `BeeNativeSegAI/SegAIFileFormat.cpp`
- `BeeNativeSegAI/BeeNativeSegAI.cpp`
- `BeeNativeSegAI/SegAINativeExport.h`
- `BeeNativeSegAI/BeeNativeSegAI.vcxproj`

Implementation notes:
- Added `BeeSegAI::SegTrainer` with sample ingestion, deterministic class-balanced subsampling, RTrees training, model save/load, and single-pixel prediction helper.
- Added `.segai` binary wrapper with 72-byte header, feature config fields, RTrees YAML payload, and CRC32 validation.
- Mask labels accepted by native training: `1` or `>=200` for defect, `2` or `[100,199]` for normal, `0` for ignore.
- Extended `SEGAI_RunFeatureSelfTest` to train on a synthetic BGR/mask pair, save a `.segai`, load it back, verify threshold/min-area metadata, and predict one defect plus one normal pixel.

Verification:
- `MSBuild BeeNativeSegAI\BeeNativeSegAI.vcxproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed.
- PowerShell/C# P/Invoke smoke call returned `SEGAI_RunFeatureSelfTest=0`.
- `dumpbin /EXPORTS` shows `SEGAI_GetVersion`, `SEGAI_GetBuildInfo`, and `SEGAI_RunFeatureSelfTest`.
- Generated project artifacts were cleaned after verification; the copied runtime DLL remains at `bin\x64\Release\BeeNativeSegAI.dll`.

Next:
- P3X.1.4: implement `SegInferer::Predict` CPU-only mask inference.

## 2026-05-12 - P3X.1.4/P3X.1.5 SegmentAI inferer and native exports

Scope:
- Implemented CPU-only native inference plus full C exports and a standalone native test harness.

Files updated:
- `BeeNativeSegAI/SegInferCore.h`
- `BeeNativeSegAI/SegInferCore.cpp`
- `BeeNativeSegAI/SegAINativeExport.cpp`
- `BeeNativeSegAI/SegAINativeExport.h`
- `BeeNativeSegAI/BeeNativeSegAI.cpp`
- `BeeNativeSegAI/BeeNativeSegAI.vcxproj`
- `BeeNativeSegAI/test/SegAITest.cpp`
- `BeeNativeSegAI/test/SegAITest.vcxproj`
- `BeeNativeSegAI/test/data/.gitignore`
- `BeeNativeSegAI/test/data/.gitkeep`

Implementation notes:
- Added `BeeSegAI::SegInferer` with CPU feature extraction, RTrees prediction over the ROI, connected-components filtering by `minDefectArea`, full-size binary output mask, and scalar defect score.
- Added full `SEGAI_*` C exports for trainer lifecycle, ROI setup, sample add/clear/count, training, save, inferer lifecycle, load, GPU flag plumbing, predict, and buffer free.
- Added `SegAITest.exe` project. If no real sample image/mask pair is supplied, it generates a synthetic checkerboard defect sample, trains, saves `.segai`, loads it through the infer export path, predicts, writes an output mask, and reports IoU.
- Generated synthetic images/model/output are runtime verification artifacts and were cleaned after verification; `test/data/.gitignore` prevents PNG/`.segai` artifacts from being committed accidentally, and `.gitkeep` preserves the folder.

Verification:
- `MSBuild BeeNativeSegAI\BeeNativeSegAI.vcxproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed.
- `MSBuild BeeNativeSegAI\test\SegAITest.vcxproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed.
- `SegAITest.exe` synthetic fallback completed with `score=0.0747223`, `iou=0.999388`, and `out_pred.png` generated before cleanup.
- PowerShell/C# P/Invoke smoke call returned `SEGAI_RunFeatureSelfTest=0`.
- `dumpbin /EXPORTS` shows all planned `SEGAI_*` trainer, inferer, buffer, and diagnostic symbols.

Next:
- P3X.1.6: implement C# wrapper `BeeCore/Func/NativeSegAI.cs`.

## 2026-05-12 - SegmentAI weld crop sample validation

Scope:
- Tested the native SegmentAI pipeline with user-provided weld sample crops and `test.png`.

Files updated:
- `BeeNativeSegAI/SegTrainerCore.cpp`
- `BeeNativeSegAI/test/SegAITest.cpp`

Implementation notes:
- `SegTrainer::AddSample` now allows single-class samples. This supports the practical workflow where one crop is all NG/defect and another crop is all OK/normal; `Train` still validates that the total dataset contains both classes.
- Added a `SegAITest.exe NG.png OK.png test.png` mode. It trains from the NG and OK crops, template-matches those crops back onto the test image to define tight weld ROIs, predicts both ROIs, and writes `out_weld_pred.png` plus `out_weld_overlay.png`.
- Runtime PNG/`.segai` outputs in `BeeNativeSegAI/test/data` remain ignored by `.gitignore`.

Verification:
- `MSBuild BeeNativeSegAI\BeeNativeSegAI.vcxproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed.
- `MSBuild BeeNativeSegAI\test\SegAITest.vcxproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed.
- Ran `SegAITest.exe BeeNativeSegAI\test\data\NG.png BeeNativeSegAI\test\data\OK.png BeeNativeSegAI\test\data\test.png`.
- Result: `trained_defect_pixels=175698`, `trained_normal_pixels=179296`, `left_roi=296,244,498,425`, `right_roi=1222,224,473,456`, `left_score=0.770725`, `right_score=0.176672`, `ratio_left_over_right=4.36247`.
- PowerShell/C# P/Invoke smoke call returned `SEGAI_RunFeatureSelfTest=0`.

Next:
- P3X.1.6 remains next: implement C# wrapper `BeeCore/Func/NativeSegAI.cs`.

## 2026-05-13 - P3X.1.6 NativeSegAI C# wrapper

Scope:
- Added the managed P/Invoke wrapper for the native SegmentAI DLL.

Files updated:
- `BeeCore/Func/NativeSegAI.cs`
- `BeeCore/BeeCore.csproj`

Implementation notes:
- Added `NativeSegAITrainer` with lifecycle, ROI setup, sample add, sample count, train, save, clear, and safe disposal.
- Added `NativeSegAIInferer` with lifecycle, load, GPU flag plumbing, predict, static GPU availability, static native self-test, and safe disposal.
- `Predict` copies the native mask into a managed `byte[]` and always calls `SEGAI_FreeBuffer` in `finally`, preserving native buffer ownership.
- Added `OpenCvSharp.Mat` overloads for BGR image/mask input while keeping low-level `IntPtr` overloads for future tool integration.

Verification:
- `MSBuild BeeCore\BeeCore.csproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed with existing repository warnings.
- `bash tools/check_propety_tools.sh` passed.
- PowerShell loaded `BeeCore\bin\x64\Release\BeeCore.dll` and returned `BeeCore.NativeSegAIInferer.SEGAI_RunFeatureSelfTest=0`.

Next:
- P3X.1.7: implement `BeeCore/Unit/SegmentAI.cs` POCO plus `DoWork`/`Complete`/`Train` flow.

## 2026-05-13 - P3X.1.7/P3X.1.8/P3X.1.9 SegmentAI core object, factory registration, and MaskPainter

Scope:
- Added the managed SegmentAI tool object, engine runner bridge, DataTool registration, and the reusable mask painting UI control.

Files updated:
- `BeeCore/Unit/SegmentAI.cs`
- `BeeCore/Func/Engines/SegmentAIEngineRunner.cs`
- `BeeCore/BeeCore.csproj`
- `BeeGlobal/Enums.cs`
- `BeeInterface/DataTool.cs`
- `BeeInterface/Tool/ToolSegmentAI.cs`
- `BeeInterface/Custom/MaskPainter.cs`
- `BeeInterface/Custom/MaskPainter.Designer.cs`
- `BeeInterface/Custom/MaskPainter.resx`
- `BeeInterface/BeeInterface.csproj`

Implementation notes:
- Added serializable `SegmentAI` and `SegSample` models with ROI/crop/mask state, training sample storage, native train/save/load flow, `DoWork` prediction, and `Complete` propagation back to the owning tool.
- Added `SegmentAIEngineRunner` so the runtime path can execute SegmentAI without embedding native wrapper details in WinForms.
- Added `TypeTool.SegmentAI` and registered it in `DataTool`. `ToolSegmentAI` is currently a minimal bridge control so factory creation works; the full operator UI remains scoped to P3X.1.10.
- Added `MaskPainter`, a WinForms UserControl that loads an image, paints NG/OK/erase labels, supports undo/clear, renders a translucent overlay, and exports label bytes compatible with native masks (`0` ignore, `1` defect, `2` normal).

Verification:
- `MSBuild BeeCore\BeeCore.csproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed with existing repository warnings.
- `MSBuild BeeInterface\BeeInterface.csproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed with existing repository warnings.
- `MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed; captured log contains 425 warning lines, all from existing repository/native dependency warning categories.
- `bash tools/check_propety_tools.sh` passed.

Next:
- P3X.1.10: implement the full `ToolSegmentAI` form using `MaskPainter`, sample management, train controls, threshold controls, and preview overlay.

## 2026-05-13 - P3X.1.10 ToolSegmentAI form

Scope:
- Replaced the temporary SegmentAI bridge control with the first full WinForms operator UI.

Files updated:
- `BeeInterface/Tool/ToolSegmentAI.cs`
- `BeeInterface/Tool/ToolSegmentAI.Designer.cs`
- `BeeInterface/Tool/ToolSegmentAI.resx`
- `BeeInterface/BeeInterface.csproj`

Implementation notes:
- Added a 4-tab UI: General, Training Data, Train, and Inference.
- Added collapsible RJButton-backed parameter sections for storage, sample annotation, train settings, and inference settings.
- Added sample import flow: Add Sample opens image files, launches a modal `MaskPainter`, saves the copied source image plus a visible grayscale mask (`255` defect, `128` normal), and registers the sample in `SegmentAI.samples`.
- Added sample list preview with NG/OK overlay and mask pixel counts.
- Added async training flow with cancellation, progress display, log output, and model reload after successful save.
- Added inference test flow with image browse, threshold/min-area/GPU controls, native prediction call, score/result labels, and preview overlay.
- Preserved event balance by wiring every UI event with `-=` before `+=`.

Verification:
- `MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed.
- Captured build log contains 425 warning lines and 0 real error lines. The warning count is within the planned cap and matches existing repository warning categories.
- `bash tools/check_propety_tools.sh` passed.
- Event balance check for `ToolSegmentAI.cs` and `ToolSegmentAI.Designer.cs` returned `plus=22 minus=22 equal=True`.

Not run:
- Manual app smoke test for adding a SegmentAI tool, painting 5 samples, training, and testing preview. This requires interactive EasyVision UI operation.

Next:
- P3X.1.11: implement and benchmark the GPU/UMat SegmentAI inference path.

## 2026-05-14 - P3X.1.11 SegmentAI GPU/UMat inference path

Scope:
- Implemented the first GPU-assisted SegmentAI inference path and benchmark harness updates.

Files updated:
- `BeeNativeSegAI/SegFeatureCore.cpp`
- `BeeNativeSegAI/SegInferCore.h`
- `BeeNativeSegAI/SegInferCore.cpp`
- `BeeNativeSegAI/BeeNativeSegAI.cpp`
- `BeeNativeSegAI/test/SegAITest.cpp`
- `BeeNativeSegAI/test/SegAITest.vcxproj`

Implementation notes:
- Implemented `SegFeatureExtractor::ExtractGpu` using `cv::UMat` for HSV/Gabor/gradient/Laplacian/edge-density/contrast features, while keeping LBP and ROI-distance helper generation compatible by bridging through CPU where OpenCV lacks a direct vectorized UMat equivalent.
- Added a mutex-protected OpenCL runtime scope in `SegInferCore.cpp`, following the repository pattern used by `Pattern2.cpp`.
- Split inference into explicit CPU and GPU-assisted paths. The GPU path now crops to the target ROI first, extracts features on `UMat`, merges ROI planes on the GPU side, downloads one interleaved sample matrix, and keeps RTrees prediction plus connected-component filtering on CPU.
- Extended the native self-test so OpenCL-capable machines also validate GPU plane extraction and CPU/GPU mask agreement.
- Updated `SegAITest` to benchmark a 1280x960 synthetic image with a 512x512 ROI, print CPU/GPU timings plus agreement, and verify GPU agreement >= 99% when OpenCL is available.
- Fixed `SegAITest.vcxproj` library lookup so the standalone benchmark links reliably against `BeeNativeSegAI.lib`.

Verification:
- `MSBuild BeeNativeSegAI\BeeNativeSegAI.vcxproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed.
- `MSBuild BeeNativeSegAI\test\SegAITest.vcxproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed.
- Warm benchmark on the current machine (`NVIDIA RTX A500 Laptop GPU`, OpenCL available):
  - Run 1 after rebuild: `cpu_ms=97.545400`, `gpu_ms=281.908400`, `gpu_agreement=1.000000`, `iou=1.000000`.
  - Run 2 warm cache: `cpu_ms=104.258500`, `gpu_ms=161.238200`, `gpu_agreement=1.000000`, `iou=1.000000`.
- The GPU path now meets the planned latency target (`<= 200ms`) on warm runs and preserves 100% mask agreement in the benchmark.

Residual gap:
- The current GPU-assisted path does **not** yet beat the improved ROI-cropped CPU baseline on this machine (`gpu_speedup=0.646612x` warm), so the original DoD target of >= 1.5x speedup remains unmet.

Next:
- Continue P3X.1.11 optimization before moving on: reduce GPU download overhead further or move additional post-feature work off CPU so the GPU path can beat the ROI-cropped CPU baseline.

## 2026-05-14 - P3X.1.11 follow-up tuning

Scope:
- Ran an additional optimization pass on the same GPU-assisted inference path.

Files updated:
- `BeeNativeSegAI/SegInferCore.cpp`

Implementation notes:
- Re-tested OpenCV RTrees batch prediction and reverted it because it was slower than the existing parallel per-row prediction in this codebase.
- Switched the GPU sample-matrix handoff from explicit `copyTo` to `getMat(cv::ACCESS_READ)` after GPU-side plane merge/reshape, reducing one explicit copy in the hot path.

Verification:
- `MSBuild BeeNativeSegAI\BeeNativeSegAI.vcxproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed.
- `MSBuild BeeNativeSegAI\test\SegAITest.vcxproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed.
- Warm benchmark after this pass:
  - Run 1: `cpu_ms=119.318700`, `gpu_ms=279.091800`, `gpu_agreement=1.000000`, `iou=1.000000`.
  - Run 2: `cpu_ms=71.089500`, `gpu_ms=159.662100`, `gpu_agreement=1.000000`, `iou=1.000000`.

Status:
- The GPU path remains functionally correct and still satisfies the warm latency target (`~160 ms`), but the CPU ROI-cropped path improved further and is now significantly faster on the current machine.

Next:
- Keep P3X.1.11 open. The remaining work is architectural optimization, not bug fixing: reduce GPU/CPU synchronization cost or move more of the downstream pipeline off CPU before attempting P3X.1.12.

## 2026-05-14 - P3X.1.12 runtime integration follow-up

Scope:
- Closed the remaining generic runtime compatibility gaps for `SegmentAI` inside the shared tool pipeline.

Files updated:
- `BeeCore/Unit/SegmentAI.cs`

Implementation notes:
- Changed `SetModel` to accept the optional boolean argument used by generic clone/reload paths (`SetModel(true)`), keeping existing call sites compatible.
- Added `MaxThread` so `PropetyTool.RunToolAsync()` can schedule `SegmentAI` through the same dynamic concurrency path as the other tools.
- Implemented `DrawResult(Graphics)` so camera/view overlay code can render the last segmentation mask and label through the existing `tool.Propety2.DrawResult(g)` convention.
- Added a helper that converts the latest byte-mask result into an overlay `Mat` before passing it to the shared rotated-ROI drawing helper.

Verification:
- `MSBuild BeeCore\BeeCore.csproj /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed.
- `MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` reached the final copy stage, then failed because `EasyVision (8920)` held `BeeCV.dll`, `BeeCpp.dll`, `BeeCore.dll`, `BeeInterface.dll`, `BeeUi.dll`, and `PylonCli.dll` open.

Status:
- The `SegmentAI` tool now matches the generic runtime expectations of the app pipeline at compile time.
- Manual smoke for live overlay/reload/FPS is still pending, so P3X.1.12 remains partial rather than done.

Next:
- Re-run the full solution build with `EasyVision.exe` closed, then execute the planned manual smoke flow for saved-project reload, overlay rendering, and scan-loop FPS.

## 2026-05-14 - P3X.1.12 build unblock + startup smoke

Scope:
- Cleared the build blocker and verified that the full solution now produces the updated binaries.

Files updated:
- `CODEX_HISTORY.md`

Implementation notes:
- Re-ran the full `EasyVision.sln` Release x64 build after `EasyVision.exe` released the output DLLs.
- Performed a minimal startup smoke by launching the freshly built `bin\Release\EasyVision.exe`, waiting 8 seconds, and confirming the process stayed alive before shutting down the test instance.

Verification:
- `MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` passed.
- Startup smoke passed: launched `EasyVision.exe` test instance `PID 26028`, confirmed `AliveAfter8s = True`, then terminated that test instance.

Status:
- Build and basic startup are now green.
- The remaining P3X.1.12 work is manual functional smoke for SegmentAI project reload, overlay visibility, and scan-loop FPS.

Next:
- Run the in-app SegmentAI smoke scenario on a real project/tool instance, then move to P3X.1.13 once overlay/reload/FPS behavior is confirmed.

## 2026-05-14 - P3X.1.12 real-data weld inference smoke

Scope:
- Verified the native SegmentAI weld sample flow against the real `NG.png`, `OK.png`, and `test.png` assets already stored under `BeeNativeSegAI/test/data`.

Files updated:
- `CODEX_HISTORY.md`

Implementation notes:
- Reused the existing `SegAITest.exe NG.png OK.png test.png` mode that trains from the two weld crops, template-matches them back into the full test image, predicts both weld ROIs, and writes `out_weld_pred.png` plus `out_weld_overlay.png`.
- This check validates the practical left-weld-NG / right-weld-OK scenario without requiring interactive WinForms steps.

Verification:
- Ran:
  - `SegAITest.exe BeeNativeSegAI/test/data/NG.png BeeNativeSegAI/test/data/OK.png BeeNativeSegAI/test/data/test.png`
- Result:
  - `left_score=0.771179`
  - `right_score=0.176176`
  - `ratio_left_over_right=4.37733`
  - `left_roi=296,244,498,425`
  - `right_roi=1222,224,473,456`
- Output artifacts refreshed:
  - `BeeNativeSegAI/test/data/out_weld_pred.png`
  - `BeeNativeSegAI/test/data/out_weld_overlay.png`

Status:
- Real-image native smoke is consistent with the expected behavior: the left weld is scored much more defect-like than the right weld.
- Remaining P3X.1.12 scope is now narrowed to in-app manual overlay/reload/FPS confirmation only.

Next:
- Perform the final manual EasyVision SegmentAI smoke, then proceed to P3X.1.13 hardening if the UI behavior matches the native results.

## 2026-06-04 - Multi-Program / Multi-Camera / Multi-Thread (T1-T12)

Scope:
- Feature: configurable ProcessingMode (Single / Multi-1Cam / Multi-MultiCam) + ThreadingMode (Sequential / Parallel) with camera<->program mapping.
- Plan: docs/architecture/PLAN_MultiProgram_MultiCamera_Threading.md.

Files touched:
- BeeGlobal/Enums.cs: added enums ProcessingMode, ThreadingMode (T1).
- BeeGlobal/Config.cs: added ProcessingMode, ThreadingMode, NumProgram, NumCamera, MaxParallelPrograms, ProgramCameraMap (int[4]); SyncLegacyFlags() keeps IsMultiProg/IsMultiCamera in sync (T2).
- BeeCore/Data/LoadData.cs: migrate legacy IsMultiProg=true -> MultiProgramMultiCamera after deserialize; default ProgramCameraMap; SyncLegacyFlags (T3).
- BeeGlobal/Global.cs: SelectProgram() switches on ProcessingMode and reads ProgramCameraMap; identity map {0,1,2,3} + MultiProgramMultiCamera reproduces legacy IndexCCCD=indexProg exactly (T4).
- BeeCore/Func/ProgramRunPlan.cs (NEW): ProgramRunUnit + ProgramRunPlanner.Build()/BuildSingle() for the 3 modes (T5).
- BeeCore/Checking.cs: moved per-program state IndexToolAuto/IsDoneTrig to instance fields (race-free for parallel); mirrors to Global.* for legacy readers (T6).
- BeeInterface/Group/View.cs:
  - RunProcessing(): routes through ProgramRunPlanner; Sequential per-unit driver (T7); Parallel branch RunProcessingParallel + RunUnitParallelAsync with SemaphoreSlim(MaxParallelPrograms) throttle + per-unit TaskCompletionSource barrier (T9).
  - Read state: EnsureCamerasForPlan() instantiates null camera slots referenced by plan; parallel camera read via Task.WaitAll only in MultiProgramMultiCamera+Parallel; all other modes keep legacy foreach bit-identical (T8).
- BeeCore/Func/Camera.cs: added non-breaking SumResult(int programIndex) overload; old SumResult() delegates to it. Parallel path aggregates each program's own tool list instead of racing on Global.IndexProgChoose (T9).
- BeeInterface/GeneralSetting.cs + .Designer.cs: added RJButtons btnMulti1Cam (3rd mode) + btnParallel (threading toggle) into layP2 2x2 grid; ApplyProcessingMode/RefreshModeButtons single source of truth (T10); txtCamMap TextBoxAuto comma-list editor for ProgramCameraMap, enabled only in Multi-MultiCam (T11).

Build verification:
- Command: MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64
- Result: pass, 0 errors. EasyVision.exe + all native DLLs produced.
- tools/check_propety_tools.sh region: 0 direct Common.PropetyTools[] access in new code.

Notes for future agents:
- Tool image reads are already snapshot-safe: every Unit clones listCamera[IndexCCD].matRaw at run time; matRaw is stable through the Checking phase (next Read only fires after all programs finish), so parallel needs no extra Mat snapshot.
- Tool.IndexCCD defaults to 0 and is persisted per-tool in .prog (not pulled from Global.IndexCCCD). Per-program camera binding lives in each tool's serialized IndexCCD; ProgramCameraMap controls IndexCCCD for result aggregation/camera Read.
- Checking self-drives: setting StatusProcessing spawns a Task that calls ProcessingAll until Done/None. Parallel reuses Checking1..4 (one per program index).
- DrawResult() in Camera.cs still reads Global.IndexProgChoose/TriggerNum/StatusProcessing for overlay text (display only). Parallel path points display globals at the last plan program after gather. If multi-program overlays are needed later, DrawResult must be parameterized too.

Blockers / left dirty:
- UI exposes Multi-1Cam, Parallel toggle, and comma-list camera map. A richer DataGridView mapping editor was deferred in favor of the low-risk TextBoxAuto comma list (Designer cannot be opened in VS in this environment).
- Parallel mode not smoke-tested against hardware/sample project in this session (build-verified only). 6-combo smoke matrix (plan section 10.1) still to be run by a human on a real rig; recommend starting with SimCam.
- NumProgram/NumCamera/MaxParallelPrograms have no dedicated numeric UI control; NumProgram/NumCamera derive from NumTrig via ApplyProcessingMode, MaxParallelPrograms defaults to 2 (config-file editable).
