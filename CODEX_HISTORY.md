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
