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
