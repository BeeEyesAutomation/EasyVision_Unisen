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
