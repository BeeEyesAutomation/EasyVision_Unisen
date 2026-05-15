# Plan — Tool Segment AI (IV4-like) cho EasyVision_Unisen

> Phiên bản chi tiết. Mỗi mục lớn (Native / C# / UI / Integration) đều có code skeleton, signature đầy đủ, file format spec. Task Cards atomic ở §13 theo template CLAUDE.md §11.1 — có thể copy thẳng vào `docs/architecture/tasks/`.

---

## 1. Context

EasyVision_Unisen hiện có 3 AI tool (`ToolYolo`, `ToolMultiOnnx`, `ToolOKNG`) — tất cả **inference-only** với model pre-trained do dev cung cấp. Không có công cụ để **operator tự train tại chỗ** trên ảnh thực ngoài line. Khách hàng yêu cầu tool tương đương **Keyence IV4 "AI Segmentation"**: operator vẽ vài mask Normal/Defect lên ảnh mẫu, app tự học, sau đó phân đoạn pixel real-time trên line.

**Mục tiêu Phase 1**: pilot `ToolSegmentAI` end-to-end (MVP) — binary mask defect, brush + polygon labeling, train classic ML CPU, **infer Intel iGPU** qua OpenCL UMat, tích hợp scan pipeline.

**4 quyết định user đã chốt:**

| Decision | Choice | Hệ quả |
|---|---|---|
| Engine | Hybrid: classic ML (`cv::ml::RTrees`) train CPU → file `.segai` → infer Intel iGPU qua UMat | KHÔNG cần thêm dependency (oneDNN/libtorch/SYCL). OpenCV 4.5.5 đã có RTrees + OpenCL. |
| Class output | Binary Normal/Defect | Mask 1 channel, score scalar. Phase 2 mở rộng multi-class. |
| Labeling UI | Brush + Polygon kết hợp | Build mới `MaskPainter` UserControl + tái dùng polygon của `EditRectRot`. |
| Scope Phase 1 | MVP end-to-end 4-5 tuần | 13 Task Cards atomic, mỗi card ≤ 2 ngày. |

**Hạ tầng tái dùng** (đã verify):
- OpenVINO 2025.4.0: `C:\Program Files (x86)\Intel\openvino_2025.4.0\runtime\` ([BeeNativeOnnx/BeeNativeOnnx.vcxproj:75](BeeNativeOnnx/BeeNativeOnnx.vcxproj:75)). Phase 1 chưa cần OpenVINO, để dành Phase 2 cho NN inference.
- OpenCV 4.5.5: `C:\OpenCV4.5\opencv\build\` với `opencv_world455.lib` (có `cv::ml`, `cv::ocl`, UMat).
- Pattern `Form ↔ Unit ↔ NativeXxx`: [BeeCore/Func/NativeYolo.cs](BeeCore/Func/NativeYolo.cs), [BeeInterface/Tool/ToolYolo.cs](BeeInterface/Tool/ToolYolo.cs), [BeeNativeOnnx/YoloNativeExport.h](BeeNativeOnnx/YoloNativeExport.h).
- ROI editor `EditRectRot` với Polygon mode: [BeeInterface/Group/EditRectRot.cs](BeeInterface/Group/EditRectRot.cs).
- Factory: [BeeInterface/DataTool.cs:297-426](BeeInterface/DataTool.cs:297).
- Async worker pattern: `PropetyTool.worker` (BackgroundWorker) + `PercentChange` event.

---

## 1.5 Pre-flight Checklist (state of repo trước khi kick off P3X.1.1)

| Item | Status | Note |
|---|---|---|
| OpenCV 4.5.5 path `C:\OpenCV4.5\opencv\build\` | ✅ verified | Có sẵn trên dev machine. |
| OpenVINO 2025.4.0 path `C:\Program Files (x86)\Intel\openvino_2025.4.0\` | ✅ verified | Phase 1 chưa cần; để dành Phase 2. |
| Baseline build pass | ✅ 447 warnings, 0 errors | [docs/architecture/baseline_build.md](docs/architecture/baseline_build.md) — 2026-05-03. **So sánh với 447, KHÔNG phải 424.** |
| `docs/architecture/tasks/` folder | ✅ tạo rồi | Để file Task Card vào đây theo CLAUDE.md §11.1. |
| `tools/check_propety_tools.sh` | ✅ tạo rồi | Convention guard. Pass clean (0 violation hiện tại). |
| `CODEX_HISTORY.md` location | ⚠️ ở **root** repo | Chưa migrate vào `docs/architecture/` (CLAUDE.md Task P0.1 chưa làm). Agent append vào root file. |
| `Global.Project` (string) | ✅ tồn tại | Dùng trong path: `"Program\\" + Global.Project + "\\SegAI_NNN"`. |
| `Global.PathRoot` | ❌ KHÔNG tồn tại | Đừng dùng. Path là **relative** từ working dir (xem [BeeCore/Data/SaveData.cs:65](BeeCore/Data/SaveData.cs:65)). |
| Sample test images cho P3X.1.5 standalone test | ⏳ **user cần cung cấp** | ≥ 5 cặp `sample_defect_NN.png` + `sample_mask_NN.png` (ảnh BGR + mask CV_8UC1 với 1=defect, 2=normal, 0=ignore). Đặt vào `BeeNativeSegAI/test/data/`. Nếu chưa có, agent dùng synthetic checkerboard fallback. |
| OpenCL Intel iGPU runtime check | ⏳ chưa verify | Trước Task P3X.1.11, chạy snippet `cv::ocl::Device::getDefault()` → confirm vendor "Intel" + OpenCL ≥ 1.2. Nếu fail → GPU path fall back CPU, performance miss target. |
| `BeeNativeSegAI.vcxproj` GUID | ⏳ chưa tạo | VS auto-gen khi Add New Project, hoặc PowerShell `[guid]::NewGuid()`. |
| Reference LBP cho P3X.1.2 verify | ⏳ optional | Python `skimage.feature.local_binary_pattern` chạy trên 1 sample, dump `.npy`. Nếu không có, dùng synthetic visual check. |

---

## 2. Approach Summary

**Hybrid pixel classifier**: feature extraction trên iGPU (UMat) → Random Trees predict CPU (vectorized parallel_for).

**Training (CPU)**:
1. Với mỗi sample (ảnh + mask binary user vẽ), iterate pixel trong ROI:
   - Pixel có mask=1 → label = `1` (Defect)
   - Pixel có mask=0 (nhưng vẫn trong ROI và trong vùng "normal sample" user marked) → label = `0` (Normal)
2. Extract feature vector **24-D** per pixel (chi tiết §4).
3. Subsample để cân bằng class (`max_pixels_per_class = 50_000`, downsample nếu vượt).
4. Train `cv::ml::RTrees`: 100 trees, max depth 12, min sample count 10, OOB error termination.
5. Save model `.segai` (format §5).

**Inference (Intel iGPU)**:
1. Load `.segai` → instantiate `RTrees` + feature config.
2. Per frame: feature stack extract trên `cv::UMat` (filter2D/Sobel/cvtColor auto-OpenCL khi `cv::ocl::setUseOpenCL(true)`).
3. Download feature stack về CPU `cv::Mat` (24 × H × W float32).
4. `cv::parallel_for_` lặp các row, mỗi row gọi `RTrees::predict` batched 1 hàng tại 1 lần.
5. Post-process: threshold + connected components + filter `min_defect_area`.
6. Return mask `CV_8UC1` + score (scalar = số pixel defect / số pixel ROI).

**Target performance**:
- Train 5 ảnh 1280×960: ≤ 90s (single thread CPU baseline, scale với `Common.NumThreads`).
- Infer 1280×960 (ROI 512×512): ≤ 200ms ≈ 5 FPS trên Intel UHD 770 / Iris Xe.
- Memory peak < 1.5 GB.

**Lý do không dùng CNN/U-Net**: Không có training framework C++ trong repo. Tự viết SGD trên iGPU = block ≥ 2 tháng. Classic ML đủ cho ≥ 85% case industrial defect.

---

## 3. Architecture Overview

```
┌─────────────────────────── UI Layer ───────────────────────────┐
│ BeeInterface/Tool/ToolSegmentAI.cs (Form, 4 tab)               │
│   ↳ uses BeeInterface/Custom/MaskPainter.cs (brush+polygon)    │
│   ↳ uses BeeInterface/Group/EditRectRot.cs (ROI Area/Crop/Mask)│
│   ↳ subscribes Propety.PercentChange → progress bar            │
└────────────────────────────┬───────────────────────────────────┘
                             │ binds
                             ▼
┌──────────────────────── Domain Layer ──────────────────────────┐
│ BeeCore/Unit/SegmentAI.cs (POCO + DoWork/Complete)             │
│   ↳ Propety2 fields: ROIs, threshold, samples list, modelPath  │
│   ↳ NativeSegAI* refs (NonSerialized)                          │
│                                                                 │
│ BeeCore/Func/Engines/SegmentAIEngineRunner.cs (static facade)  │
│   ↳ Run(SegmentAI, Mat, ROIs, complete) → SegmentAIRunResult   │
└────────────────────────────┬───────────────────────────────────┘
                             │ P/Invoke via DllImport
                             ▼
┌────────────────── C# Native Wrapper Layer ─────────────────────┐
│ BeeCore/Func/NativeSegAI.cs                                    │
│   ↳ NativeSegAITrainer : IDisposable                           │
│   ↳ NativeSegAIInferer : IDisposable                           │
│   ↳ struct SegPredictResult (StructLayout.Sequential)          │
└────────────────────────────┬───────────────────────────────────┘
                             │ DllImport("BeeNativeSegAI.dll")
                             ▼
┌─────────────────────── Native C++ Layer ───────────────────────┐
│ BeeNativeSegAI/                                                │
│   ├── SegAINativeExport.h/.cpp  (extern "C" exports)           │
│   ├── SegFeatureCore.h/.cpp     (24-D extractor CPU + UMat)    │
│   ├── SegTrainerCore.h/.cpp     (wrap cv::ml::RTrees)          │
│   ├── SegInferCore.h/.cpp       (load .segai, predict mask)    │
│   ├── SegAIFileFormat.h/.cpp    (.segai header + I/O)          │
│   ├── pch.h, framework.h        (precompiled)                  │
│   └── BeeNativeSegAI.vcxproj    (toolset v143, C++17)          │
└────────────────────────────────────────────────────────────────┘
```

**Dependency edges (build order)**:
```
BeeNativeSegAI.dll          (no upstream deps in solution)
       ↓
BeeGlobal.dll  →  BeeCore.dll  →  BeeInterface.dll  →  BeeUi.exe → BeeMain.exe
```

---

## 4. Feature Vector Spec (24-D)

Tại mỗi pixel `p = (x, y)` trong ROI, extract **24 chiều float32**. Hash-stable thứ tự — KHÔNG đổi sau khi đã ship model, hoặc bump version trong `.segai` header.

| Idx | Group | Feature | Kernel/Window | Notes |
|---|---|---|---|---|
| 0 | LBP | LBP code 8-neighbor (uniform mapped to [0..58]) | 3×3 | Local Binary Pattern, gray channel. Map uniform → [0..58], non-uniform → 59. Normalize / 59.0 ∈ [0,1]. |
| 1-3 | HSV mean | mean(H), mean(S), mean(V) | 5×5 box filter | Convert BGR→HSV first. H ∈ [0,179]/179, S∈[0,255]/255, V∈[0,255]/255. |
| 4-6 | HSV std | std(H), std(S), std(V) | 5×5 box filter | sqrt(E[X²] − E[X]²). |
| 7-10 | Gabor mag | abs(Gabor)θ=0°, 45°, 90°, 135° | 7×7 kernel, σ=2.0, λ=4.0 | Gray channel. `cv::getGaborKernel(Size(7,7), 2.0, θ, 4.0, 0.5)`. Normalize / 255. |
| 11 | Gradient mag | sqrt(Sx² + Sy²) | Sobel 3×3 | Normalize / 255. |
| 12 | Gradient orient | atan2(Sy, Sx) mapped [0,1] | Sobel 3×3 | (θ+π)/(2π). |
| 13 | Laplacian | abs(L) | Laplacian 3×3 | Normalize / 255. |
| 14-15 | Position | x_norm = (x - x_roi)/w_roi, y_norm = (y - y_roi)/h_roi | — | ∈[0,1]. Giúp model học spatial bias trong ROI. |
| 16-18 | Intensity neighborhood | mean(gray)@3×3, mean@7×7, mean@15×15 | Box filter 3 scales | Normalize /255. |
| 19-21 | Edge density | count(edge)@5×5, 11×11, 21×21 | Canny (low=50, high=150) → box filter | Normalize /(window_area). |
| 22 | Local contrast | (V_max - V_min) trong 5×5 | dilate/erode 5×5 | / 255. |
| 23 | Distance to ROI edge | min(x-x0, x1-x, y-y0, y1-y)/max(w,h)/2 | — | ∈[0,1]. |

**Total**: 24 float32 per pixel. Cho ảnh 1280×960: 24 × 1280 × 960 × 4 = **112 MB** feature stack per frame. Có thể tile khi tight memory.

**`SegFeatureExtractor` class** (C++):

```cpp
// SegFeatureCore.h
#pragma once
#include <opencv2/opencv.hpp>

namespace BeeSegAI {

struct FeatureConfig {
    int lbpRadius = 1;           // hardcoded 1 in v1
    int hsvWindow = 5;
    int gaborSize = 7;
    float gaborSigma = 2.0f;
    float gaborLambda = 4.0f;
    cv::Rect roi;                // pixel coords
};

class SegFeatureExtractor {
public:
    static constexpr int kNumFeatures = 24;

    SegFeatureExtractor() = default;
    void Configure(const FeatureConfig& cfg);

    // CPU: src = BGR CV_8UC3, out = CV_32F with 24 channels (or 24 separate planes).
    // Shape: 1 plane per feature, each H×W float32.
    void ExtractCPU(const cv::Mat& srcBgr, std::vector<cv::Mat>& outPlanes);

    // GPU UMat: feature stack as 24 UMat planes (OpenCL backend auto via cv::ocl).
    void ExtractGpu(const cv::UMat& srcBgr, std::vector<cv::UMat>& outPlanes);

    // Pack 24 planes into row-major sample matrix at given pixel positions.
    // pixels: list of (x,y); out: CV_32F shape (N, 24).
    void PackSamples(const std::vector<cv::Mat>& planes,
                     const std::vector<cv::Point>& pixels,
                     cv::Mat& outSamples);

    // Convert 24 planes → interleaved (H*W, 24) for batched predict.
    void PlanesToInterleaved(const std::vector<cv::Mat>& planes,
                             const cv::Rect& roi,
                             cv::Mat& outRows);  // (roi.area(), 24)

private:
    FeatureConfig cfg_;
    // Precomputed kernels (cached after Configure):
    cv::Mat gaborKernels_[4];
    cv::Mat lbpLookup_;   // 256 → uniform-mapped idx
};

} // namespace BeeSegAI
```

---

## 5. `.segai` File Format Spec

Binary little-endian. Đóng gói RTrees model + feature config + threshold.

```
Offset  Size    Field                       Type
0       8       Magic                       char[8] = "SEGAI\0\0\0"
8       4       Version                     uint32_t = 1
12      4       FeatureFlags                uint32_t = bitmask (Phase 1: 0x00FFFFFF = all 24)
16      4       NumClasses                  uint32_t = 2 (binary)
20      4       FeatureCount                uint32_t = 24
24      4       DefectThreshold (default)   float32 = 0.5
28      4       MinDefectArea (default)     uint32_t = 50
32      4       LBPRadius                   uint32_t = 1
36      4       HSVWindow                   uint32_t = 5
40      4       GaborSize                   uint32_t = 7
44      4       GaborSigma                  float32 = 2.0
48      4       GaborLambda                 float32 = 4.0
52      4       ReservedA                   uint32_t = 0
56      8       PayloadSize                 uint64_t = N bytes of RTrees YAML/XML blob
64      4       Crc32                       uint32_t = CRC32 of payload
68      4       ReservedB                   uint32_t = 0
72      N       Payload (RTrees blob)       byte[N]
```

**Serialize logic**:
```cpp
// SegAIFileFormat.h
struct SegAIHeader {
    char     magic[8];
    uint32_t version;
    uint32_t featureFlags;
    uint32_t numClasses;
    uint32_t featureCount;
    float    defectThreshold;
    uint32_t minDefectArea;
    uint32_t lbpRadius;
    uint32_t hsvWindow;
    uint32_t gaborSize;
    float    gaborSigma;
    float    gaborLambda;
    uint32_t reservedA;
    uint64_t payloadSize;
    uint32_t crc32;
    uint32_t reservedB;
};  // sizeof = 72

bool SaveSegai(const std::wstring& path, cv::Ptr<cv::ml::RTrees> model,
               const FeatureConfig& cfg, float threshold, uint32_t minArea);
bool LoadSegai(const std::wstring& path, cv::Ptr<cv::ml::RTrees>& outModel,
               FeatureConfig& outCfg, float& outThreshold, uint32_t& outMinArea);
```

**Payload encoding**: `cv::ml::RTrees::save("temp.yml")` → đọc YAML text → ghi vào payload. Hoặc dùng `cv::FileStorage` với `MEMORY` flag để tránh tmp file.

**Compat policy**: nếu `magic != "SEGAI"` hoặc `version != 1` → reject load. Phase 2 nâng version=2 với feature flags mở rộng.

---

## 6. Memory Ownership C++ ↔ C#

Quy ước rõ ràng để tránh leak / double-free.

| Resource | Allocated by | Freed by | Lifetime |
|---|---|---|---|
| `_handle` của `NativeSegAITrainer`/`NativeSegAIInferer` | `SEGAI_*Create` (C++) | `SEGAI_*Destroy` (C++) call từ `Dispose()` C# | Match instance lifetime. `~NativeSegAI*()` finalizer fallback. |
| Mask buffer trả về từ `SEGAI_InferPredict` | C++ heap (`new uint8_t[w*h]`) | `SEGAI_FreeBuffer` C++ | C# copy ngay vào `byte[]` rồi gọi free; KHÔNG giữ `IntPtr` lâu. |
| BGR input buffer cho Predict | C# pinned `GCHandle` từ `cv::Mat.Data` | C# unpin sau call | Sync (per call), không async. |
| RTrees blob (file `.segai`) | C++ ifstream | scope local | — |
| Mat / UMat trong native | C++ stack/heap | RAII | — |

**`SEGAI_FreeBuffer` API**:
```cpp
YAPI void SEGAI_FreeBuffer(void* ptr);  // simple delete[] (uint8_t*)ptr;
```

**Pattern C# Predict** (an toàn):
```csharp
public byte[] Predict(IntPtr bgr, int w, int h, int step, float threshold, out float score) {
    score = 0;
    if (_handle == IntPtr.Zero) return null;
    IntPtr maskPtr = IntPtr.Zero;
    int maskW = 0, maskH = 0;
    float scoreOut = 0;
    int rc = SEGAI_InferPredict(_handle, bgr, w, h, step, threshold,
                                out maskPtr, out maskW, out maskH, out scoreOut);
    if (rc != 0 || maskPtr == IntPtr.Zero) return null;
    try {
        var data = new byte[maskW * maskH];
        Marshal.Copy(maskPtr, data, 0, data.Length);
        score = scoreOut;
        return data;
    }
    finally {
        SEGAI_FreeBuffer(maskPtr);
    }
}
```

---

## 7. Threading Model

| Phase | Thread | Detail |
|---|---|---|
| UI events (click Train/Test) | UI thread (Form) | Validate state, sync controls, kick off worker. |
| Long ops (Train/Test single) | `OwnerTool.worker` BackgroundWorker | DoWork calls into `SegmentAIEngineRunner` → native. |
| Native train | C++ worker threads | `cv::ml::RTrees::train` already multi-threads internally. |
| Native feature extract | OpenCL kernels (iGPU) + parallel_for_ (CPU fallback) | `cv::setUseOptimized(true)` + `cv::setNumThreads(Common.NumThreads)`. |
| Native predict batched | `cv::parallel_for_(Range(0,H), ...)` chia row | Each thread predicts its row strip; RTrees predict is thread-safe sau khi load. |
| Scan pipeline | `BeeCore/Func/Camera.cs` thread | Calls `SegmentAIEngineRunner.Run` synchronously. Inferer instance pre-loaded từ `SegmentAI.SetModel`. |

**Cancel pattern**: training có thể dài (90s+). `worker.CancellationPending` polled mỗi 5% progress trong `RTrees::TermCriteria` callback. Native expose `SEGAI_TrainerCancel(handle)` set flag → C++ check flag mỗi epoch.

---

## 8. Native C++ Files — Chi Tiết

### 8.1 `BeeNativeSegAI/BeeNativeSegAI.vcxproj`

Template lấy từ [BeeNativeOnnx/BeeNativeOnnx.vcxproj](BeeNativeOnnx/BeeNativeOnnx.vcxproj). Khác biệt chính:
- `<RootNamespace>BeeNativeSegAI</RootNamespace>`
- `<ProjectGuid>` mới (generate UUID).
- `<ConfigurationType>DynamicLibrary</ConfigurationType>` (cả Debug|x64 và Release|x64).
- IncludePath: **chỉ** `C:\OpenCV4.5\opencv\build\include` (không OpenVINO Phase 1).
- LibraryPath: **chỉ** `C:\OpenCV4.5\opencv\build\x64\vc15\lib`.
- `<AdditionalDependencies>opencv_world455.lib;%(AdditionalDependencies)</AdditionalDependencies>` (Release|x64). Debug|x64 link `opencv_world455d.lib`.
- `<LanguageStandard>stdcpp17</LanguageStandard>` (đã có sẵn ở Release; nhân bản cho Debug).
- `<PreprocessorDefinitions>NDEBUG;_USRDLL;BEESEGAI_EXPORTS;%(PreprocessorDefinitions)</PreprocessorDefinitions>`.
- ItemGroup ClCompile: 5 file `.cpp`. ClInclude: 5 file `.h` + `pch.h`/`framework.h`.

Output expected: `BeeNativeSegAI/x64/Release/BeeNativeSegAI.dll`, copy về `bin/x64/Release/` của BeeMain qua post-build event (giống Pattern.vcxproj — kiểm tra cú pháp).

### 8.2 `SegAINativeExport.h` (full)

```cpp
#pragma once
#include <stdint.h>

#ifdef BEESEGAI_EXPORTS
#define SAPI __declspec(dllexport)
#else
#define SAPI __declspec(dllimport)
#endif

extern "C" {
    // ===== Trainer =====
    SAPI void*    SEGAI_TrainerCreate();
    SAPI void     SEGAI_TrainerDestroy(void* handle);
    SAPI int      SEGAI_TrainerSetROI(void* handle, int x, int y, int w, int h);
    // mask: CV_8UC1, 0 = ignore, 1 = defect, 2 = normal
    SAPI int      SEGAI_TrainerAddSample(void* handle,
                                         const uint8_t* bgr, int w, int h, int step,
                                         const uint8_t* mask, int maskStep);
    SAPI void     SEGAI_TrainerClearSamples(void* handle);
    // progress: 0..100. cancelFlag: pointer to int set by C# to cancel (or null).
    SAPI int      SEGAI_TrainerTrain(void* handle,
                                     int numTrees, int maxDepth, int minSampleCount,
                                     void(*progressCb)(int, void*), void* userData,
                                     const volatile int* cancelFlag);
    SAPI int      SEGAI_TrainerSave(void* handle, const wchar_t* path,
                                    float defectThreshold, uint32_t minDefectArea);
    SAPI int      SEGAI_TrainerSampleCount(void* handle, int* outDefectPixels, int* outNormalPixels);

    // ===== Inferer =====
    SAPI void*    SEGAI_InferCreate();
    SAPI void     SEGAI_InferDestroy(void* handle);
    SAPI int      SEGAI_InferLoad(void* handle, const wchar_t* path);
    SAPI int      SEGAI_InferSetGpu(void* handle, int enable);   // 1 = use UMat
    SAPI int      SEGAI_InferGetGpuAvailable();                  // global cv::ocl check
    // Returns mask via outMaskPtr (caller MUST call SEGAI_FreeBuffer).
    // Mask: CV_8UC1, 0 = normal, 255 = defect (post-thresholded).
    SAPI int      SEGAI_InferPredict(void* handle,
                                     const uint8_t* bgr, int w, int h, int step,
                                     int roiX, int roiY, int roiW, int roiH,
                                     float threshold,
                                     uint8_t** outMaskPtr, int* outMaskW, int* outMaskH,
                                     float* outScore);

    // ===== Buffer =====
    SAPI void     SEGAI_FreeBuffer(void* ptr);

    // ===== Diagnostic =====
    SAPI int      SEGAI_GetVersion();   // returns 1
    SAPI const char* SEGAI_GetBuildInfo();   // e.g. "BeeNativeSegAI 1.0 / OpenCV 4.5.5 / OCL=1"
}
```

Return codes: 0 = OK, -1 = invalid handle, -2 = invalid input, -3 = no samples, -4 = train failed, -5 = file I/O, -6 = format mismatch.

### 8.3 `SegFeatureCore.cpp` — pseudo-code các bước

```cpp
void SegFeatureExtractor::ExtractCPU(const cv::Mat& bgr, std::vector<cv::Mat>& planes) {
    cv::Mat gray, hsv, lbp;
    cv::cvtColor(bgr, gray, cv::COLOR_BGR2GRAY);
    cv::cvtColor(bgr, hsv,  cv::COLOR_BGR2HSV);

    planes.resize(kNumFeatures);

    // 0: LBP
    ComputeLBP(gray, planes[0]);   // CV_32F /59

    // 1-3, 4-6: HSV mean/std via integral image
    cv::Mat hsvCh[3]; cv::split(hsv, hsvCh);
    for (int c = 0; c < 3; ++c) {
        cv::Mat mean, var;
        BoxMeanStd(hsvCh[c], cfg_.hsvWindow, mean, var);
        mean.convertTo(planes[1 + c], CV_32F, c == 0 ? 1.0/179.0 : 1.0/255.0);
        cv::sqrt(var, var);
        var.convertTo(planes[4 + c], CV_32F, c == 0 ? 1.0/179.0 : 1.0/255.0);
    }

    // 7-10: Gabor 4 directions
    for (int i = 0; i < 4; ++i) {
        cv::Mat out;
        cv::filter2D(gray, out, CV_32F, gaborKernels_[i]);
        out = cv::abs(out);
        out.convertTo(planes[7 + i], CV_32F, 1.0/255.0);
    }

    // 11: gradient magnitude
    cv::Mat sx, sy;
    cv::Sobel(gray, sx, CV_32F, 1, 0, 3);
    cv::Sobel(gray, sy, CV_32F, 0, 1, 3);
    cv::Mat mag;
    cv::magnitude(sx, sy, mag);
    mag.convertTo(planes[11], CV_32F, 1.0/255.0);

    // 12: orientation
    cv::Mat ang;
    cv::phase(sx, sy, ang, false);  // radians [0, 2π]
    ang.convertTo(planes[12], CV_32F, 1.0/(2.0*CV_PI));

    // 13: Laplacian
    cv::Mat lap;
    cv::Laplacian(gray, lap, CV_32F, 3);
    lap = cv::abs(lap);
    lap.convertTo(planes[13], CV_32F, 1.0/255.0);

    // 14-15: position
    FillPositionPlanes(planes[14], planes[15], cfg_.roi, gray.size());

    // 16-18: intensity neighborhood
    cv::Mat blur3, blur7, blur15;
    cv::boxFilter(gray, blur3,  CV_32F, cv::Size(3, 3));
    cv::boxFilter(gray, blur7,  CV_32F, cv::Size(7, 7));
    cv::boxFilter(gray, blur15, CV_32F, cv::Size(15, 15));
    blur3.convertTo (planes[16], CV_32F, 1.0/255.0);
    blur7.convertTo (planes[17], CV_32F, 1.0/255.0);
    blur15.convertTo(planes[18], CV_32F, 1.0/255.0);

    // 19-21: edge density 3 scales
    cv::Mat edges;
    cv::Canny(gray, edges, 50, 150);
    edges.convertTo(edges, CV_32F, 1.0/255.0);
    cv::Mat ed5, ed11, ed21;
    cv::boxFilter(edges, ed5,  CV_32F, cv::Size(5, 5));
    cv::boxFilter(edges, ed11, CV_32F, cv::Size(11, 11));
    cv::boxFilter(edges, ed21, CV_32F, cv::Size(21, 21));
    planes[19] = ed5; planes[20] = ed11; planes[21] = ed21;

    // 22: local contrast (V channel of HSV)
    cv::Mat dil, ero;
    cv::dilate(hsvCh[2], dil, cv::getStructuringElement(cv::MORPH_RECT, cv::Size(5,5)));
    cv::erode (hsvCh[2], ero, cv::getStructuringElement(cv::MORPH_RECT, cv::Size(5,5)));
    cv::Mat contrast = dil - ero;
    contrast.convertTo(planes[22], CV_32F, 1.0/255.0);

    // 23: distance to ROI edge
    FillDistanceToRoi(planes[23], cfg_.roi);
}
```

**`ExtractGpu` chỉ khác ở**: nhận `cv::UMat`, gọi mọi op trên UMat (OpenCV tự dispatch sang OpenCL khi `cv::ocl::useOpenCL() == true`). Output planes cũng là `cv::UMat`.

### 8.4 `SegTrainerCore.cpp` — Train flow

```cpp
int SegTrainer::Train(int numTrees, int maxDepth, int minSampleCount,
                      std::function<void(int)> progressCb,
                      const volatile int* cancelFlag) {
    if (samples_.empty()) return -3;

    // 1. Allocate sample matrix.
    int totalDefect = 0, totalNormal = 0;
    for (auto& s : samples_) {
        totalDefect += s.defectPixels.size();
        totalNormal += s.normalPixels.size();
    }
    int total = totalDefect + totalNormal;
    if (totalDefect == 0 || totalNormal == 0) return -2;

    // 2. Class balance: cap each class at maxPerClass.
    const int maxPerClass = 50000;
    int useDef = std::min(totalDefect, maxPerClass);
    int useNor = std::min(totalNormal, maxPerClass);

    cv::Mat allFeat(useDef + useNor, SegFeatureExtractor::kNumFeatures, CV_32F);
    cv::Mat allLab (useDef + useNor, 1, CV_32S);

    int row = 0;
    int progressBase = 0;

    // 3. For each sample, extract features then pack rows.
    for (size_t i = 0; i < samples_.size(); ++i) {
        if (cancelFlag && *cancelFlag) return -10;
        auto& s = samples_[i];

        std::vector<cv::Mat> planes;
        extractor_.ExtractCPU(s.bgr, planes);

        // Subsample pixels for this image.
        auto defSel = SubsampleN(s.defectPixels, useDef * s.defectPixels.size() / totalDefect);
        auto norSel = SubsampleN(s.normalPixels, useNor * s.normalPixels.size() / totalNormal);

        for (auto& px : defSel) { PackOneRow(planes, px, allFeat.ptr<float>(row)); allLab.at<int>(row) = 1; ++row; }
        for (auto& px : norSel) { PackOneRow(planes, px, allFeat.ptr<float>(row)); allLab.at<int>(row) = 0; ++row; }

        progressBase = (i + 1) * 40 / samples_.size();   // 0..40
        if (progressCb) progressCb(progressBase);
    }

    // 4. Configure RTrees.
    model_ = cv::ml::RTrees::create();
    model_->setMaxDepth(maxDepth);
    model_->setMinSampleCount(minSampleCount);
    model_->setRegressionAccuracy(0);
    model_->setUseSurrogates(false);
    model_->setMaxCategories(15);
    model_->setPriors(cv::Mat());
    model_->setCalculateVarImportance(true);
    model_->setActiveVarCount(0);   // sqrt(numFeat)
    model_->setTermCriteria(cv::TermCriteria(
        cv::TermCriteria::MAX_ITER + cv::TermCriteria::EPS,
        numTrees, 0.01));

    // 5. Train (RTrees does not expose per-tree progress; report 40→90 monolithic).
    if (progressCb) progressCb(50);
    auto td = cv::ml::TrainData::create(allFeat, cv::ml::ROW_SAMPLE, allLab);
    bool ok = model_->train(td);
    if (!ok) return -4;

    if (progressCb) progressCb(100);
    return 0;
}
```

### 8.5 `SegInferCore.cpp` — Predict flow

```cpp
int SegInferer::Predict(const cv::Mat& bgr, const cv::Rect& roi, float threshold,
                        cv::Mat& outMask, float& outScore) {
    if (!model_ || model_->empty()) return -1;

    // 1. Feature extract (UMat path if GPU enabled).
    std::vector<cv::Mat> planes;
    if (useGpu_ && cv::ocl::useOpenCL()) {
        cv::UMat usrc = bgr.getUMat(cv::ACCESS_READ);
        std::vector<cv::UMat> uplanes;
        extractor_.ExtractGpu(usrc, uplanes);
        planes.resize(uplanes.size());
        for (size_t i = 0; i < uplanes.size(); ++i)
            uplanes[i].copyTo(planes[i]);   // download to CPU for RTrees::predict
    } else {
        extractor_.ExtractCPU(bgr, planes);
    }

    // 2. Build sample matrix for pixels in ROI.
    cv::Rect r = roi & cv::Rect(0, 0, bgr.cols, bgr.rows);
    int N = r.area();
    cv::Mat samples;
    extractor_.PlanesToInterleaved(planes, r, samples);  // (N, 24)

    // 3. Batched predict using parallel_for_.
    cv::Mat predRaw(N, 1, CV_32F);
    cv::parallel_for_(cv::Range(0, N), [&](const cv::Range& rng) {
        cv::Mat sub = samples.rowRange(rng.start, rng.end);
        cv::Mat res;
        model_->predict(sub, res);
        res.copyTo(predRaw.rowRange(rng.start, rng.end));
    });

    // 4. Reshape to ROI mask, threshold.
    cv::Mat roiPred = predRaw.reshape(1, r.height);  // r.height × r.width float32
    cv::Mat roiMask;
    cv::threshold(roiPred, roiMask, threshold, 255, cv::THRESH_BINARY);
    roiMask.convertTo(roiMask, CV_8U);

    // 5. Connected components filter min area.
    cv::Mat labels, stats, centroids;
    int n = cv::connectedComponentsWithStats(roiMask, labels, stats, centroids, 8);
    cv::Mat filtered = cv::Mat::zeros(roiMask.size(), CV_8U);
    for (int i = 1; i < n; ++i) {
        if (stats.at<int>(i, cv::CC_STAT_AREA) >= (int)minDefectArea_) {
            filtered.setTo(255, labels == i);
        }
    }

    // 6. Compose full-size mask (zero outside ROI).
    outMask = cv::Mat::zeros(bgr.size(), CV_8U);
    filtered.copyTo(outMask(r));

    outScore = (float)cv::countNonZero(filtered) / (float)r.area();
    return 0;
}
```

### 8.6 Standalone test exe (Week 1 verify)

Tạo `BeeNativeSegAI/test/SegAITest.cpp` — không vào main vcxproj, build riêng:
```cpp
int main(int argc, char** argv) {
    auto trainer = SEGAI_TrainerCreate();
    cv::Mat bgr = cv::imread(argv[1]);
    cv::Mat mask = cv::imread(argv[2], cv::IMREAD_GRAYSCALE);
    SEGAI_TrainerSetROI(trainer, 0, 0, bgr.cols, bgr.rows);
    SEGAI_TrainerAddSample(trainer, bgr.data, bgr.cols, bgr.rows, (int)bgr.step,
                           mask.data, (int)mask.step);
    SEGAI_TrainerTrain(trainer, 100, 12, 10, nullptr, nullptr, nullptr);
    SEGAI_TrainerSave(trainer, L"out.segai", 0.5f, 50);
    SEGAI_TrainerDestroy(trainer);

    auto inf = SEGAI_InferCreate();
    SEGAI_InferLoad(inf, L"out.segai");
    uint8_t* mp = nullptr; int mw=0, mh=0; float sc=0;
    SEGAI_InferPredict(inf, bgr.data, bgr.cols, bgr.rows, (int)bgr.step,
                       0, 0, bgr.cols, bgr.rows, 0.5f, &mp, &mw, &mh, &sc);
    cv::Mat outMask(mh, mw, CV_8U, mp);
    cv::imwrite("out_pred.png", outMask);
    SEGAI_FreeBuffer(mp);
    SEGAI_InferDestroy(inf);
    return 0;
}
```

---

## 9. C# Files — Chi Tiết

### 9.1 `BeeCore/Func/NativeSegAI.cs` (skeleton ≈ 350 LOC)

```csharp
using System;
using System.Runtime.InteropServices;

namespace BeeCore
{
    public class NativeSegAITrainer : IDisposable
    {
        const string DLL = "BeeNativeSegAI.dll";

        public delegate void ProgressCallback(int percent, IntPtr userData);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SEGAI_TrainerCreate();
        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void   SEGAI_TrainerDestroy(IntPtr h);
        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int    SEGAI_TrainerSetROI(IntPtr h, int x, int y, int w, int hh);
        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int    SEGAI_TrainerAddSample(IntPtr h,
            IntPtr bgr, int w, int hh, int step,
            IntPtr mask, int maskStep);
        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int    SEGAI_TrainerTrain(IntPtr h,
            int numTrees, int maxDepth, int minSampleCount,
            ProgressCallback cb, IntPtr userData, IntPtr cancelFlag);
        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern int    SEGAI_TrainerSave(IntPtr h, string path,
            float defectThreshold, uint minDefectArea);

        private IntPtr _h;
        private int _cancel;
        private GCHandle _cancelPin;

        public NativeSegAITrainer() {
            _h = SEGAI_TrainerCreate();
            if (_h == IntPtr.Zero) throw new Exception("SEGAI_TrainerCreate failed");
            _cancelPin = GCHandle.Alloc(_cancel, GCHandleType.Pinned);
        }
        public bool IsOpened => _h != IntPtr.Zero;
        public void SetRoi(int x, int y, int w, int h) => SEGAI_TrainerSetROI(_h, x, y, w, h);
        public bool AddSample(IntPtr bgr, int w, int h, int step, IntPtr mask, int maskStep)
            => SEGAI_TrainerAddSample(_h, bgr, w, h, step, mask, maskStep) == 0;
        public bool Train(int numTrees, int maxDepth, int minSampleCount,
                          Action<int> onProgress, out int rc) {
            ProgressCallback cb = (p, ud) => onProgress?.Invoke(p);
            rc = SEGAI_TrainerTrain(_h, numTrees, maxDepth, minSampleCount,
                                    cb, IntPtr.Zero, _cancelPin.AddrOfPinnedObject());
            GC.KeepAlive(cb);
            return rc == 0;
        }
        public void Cancel() { _cancel = 1; }
        public bool Save(string path, float threshold, uint minArea)
            => SEGAI_TrainerSave(_h, path, threshold, minArea) == 0;
        public void Dispose() {
            if (_h != IntPtr.Zero) { SEGAI_TrainerDestroy(_h); _h = IntPtr.Zero; }
            if (_cancelPin.IsAllocated) _cancelPin.Free();
            GC.SuppressFinalize(this);
        }
        ~NativeSegAITrainer() { Dispose(); }
    }

    public class NativeSegAIInferer : IDisposable
    {
        // ... mirrors NativeSegAITrainer, calls SEGAI_Infer* APIs
        // Predict returns byte[] mask + score, calls SEGAI_FreeBuffer internally
    }
}
```

### 9.2 `BeeCore/Unit/SegmentAI.cs` (skeleton ≈ 400 LOC)

```csharp
namespace BeeCore
{
    [Serializable]
    public class SegSample
    {
        public string ImagePath;            // tương đối: Program\<Proj>\SegAI_001\samples\sample01.png
        public string MaskPath;             // sample01_mask.png
        public List<List<Point>> BrushStrokes = new List<List<Point>>();  // mỗi stroke = list điểm
        public List<int> BrushSizes = new List<int>();
        public List<bool> BrushIsDefect = new List<bool>();  // true=defect, false=normal
        public List<List<Point>> Polygons = new List<List<Point>>();
        public List<bool> PolygonIsDefect = new List<bool>();
        public DateTime Created;
    }

    [Serializable]
    public class SegmentAI
    {
        // Standard fields theo Yolo.cs pattern
        public bool IsIni = false;
        public int Index = -1;
        public int IndexCCD = 0;
        public RectRotate rotArea, rotCrop, rotMask, rotLimit;
        [NonSerialized] public RectRotate rotAreaAdjustment;
        [NonSerialized] public RectRotate rotMaskAdjustment;
        public RectRotate rotPositionAdjustment;
        public TypeCrop TypeCrop;

        public string pathModel = "";          // relative: Program\<Proj>\SegAI_001\model.segai
        public string pathSamplesFolder = "";  // relative
        public List<SegSample> samples = new List<SegSample>();

        // Train hyperparams
        public int numTrees = 100;
        public int maxDepth = 12;
        public int minSampleCount = 10;

        // Inference hyperparams
        public float defectThreshold = 0.5f;
        public int minDefectArea = 50;
        public bool enableGpu = true;

        // Results (NonSerialized)
        [NonSerialized] public NativeSegAIInferer inferer;
        [NonSerialized] public byte[] lastMask;
        [NonSerialized] public int lastMaskW, lastMaskH;
        [NonSerialized] public float lastScore;
        [NonSerialized] public bool IsOK;
        [NonSerialized] public int Counter;
        [NonSerialized] public BackgroundWorker worker;

        // Events (NonSerialized)
        [field: NonSerialized] public event Action<int> PercentChange;
        [field: NonSerialized] public event Action ScoreChanged;
        [field: NonSerialized] public event Action StatusToolChanged;

        public object Clone() => this.MemberwiseClone();

        public void SetModel() {
            // Init ROIs if null (theo Yolo.cs pattern)
            if (rotArea == null) rotArea = DataTool.NewRotRect(TypeCrop.Area);
            if (rotCrop == null) rotCrop = DataTool.NewRotRect(TypeCrop.Crop);
            if (rotMask == null) rotMask = DataTool.NewRotRect(TypeCrop.Crop);
            if (rotLimit == null) rotLimit = DataTool.NewRotRect(TypeCrop.Crop);

            // Load inferer if model file exists.
            // NOTE: paths are RELATIVE to app working dir (e.g. "Program\\<Proj>\\SegAI_001\\model.segai"),
            // theo convention SaveData.cs (Global.PathRoot KHÔNG tồn tại — đừng dùng).
            if (!string.IsNullOrEmpty(pathModel) && File.Exists(pathModel)) {
                inferer?.Dispose();
                inferer = new NativeSegAIInferer();
                inferer.Load(pathModel);
                inferer.SetGpu(enableGpu);
            }
        }

        public void DoWork(RectRotate roiArea, RectRotate roiMask) {
            if (inferer == null || !inferer.IsOpened) {
                IsOK = false;
                return;
            }
            // Get cropped BGR Mat from OwnerTool.CCD image using rotArea (Translate→Rotate→Translate)
            // — see memory `project_roi_local_to_global` for matrix discipline.
            Mat crop = CropFromOwnerCcd(roiArea, out Matrix localToGlobal);

            // Convert to IntPtr + predict
            IntPtr bgrPtr = crop.Data;
            lastMask = inferer.Predict(bgrPtr, crop.Cols, crop.Rows, (int)crop.Step(),
                                       0, 0, crop.Cols, crop.Rows,
                                       defectThreshold, out lastScore);
            lastMaskW = crop.Cols; lastMaskH = crop.Rows;
            IsOK = lastScore < defectThreshold;
            PercentChange?.Invoke(100);
            ScoreChanged?.Invoke();
        }

        public void Complete() {
            // Draw overlay onto OwnerTool result image using lastMask
            // Apply localToGlobal matrix to position mask correctly on full frame
            StatusToolChanged?.Invoke();
        }

        public bool Train(Action<int> onProgress, CancellationToken ct) {
            if (samples.Count == 0) return false;
            using (var trainer = new NativeSegAITrainer()) {
                trainer.SetRoi(0, 0, /* sample size */ 0, 0);
                foreach (var s in samples) {
                    if (ct.IsCancellationRequested) { trainer.Cancel(); return false; }
                    var (bgrPtr, w, h, step) = LoadSampleImage(s);
                    var (maskPtr, mStep) = BuildMaskFromAnnotations(s, w, h);
                    trainer.AddSample(bgrPtr, w, h, step, maskPtr, mStep);
                }
                bool ok = trainer.Train(numTrees, maxDepth, minSampleCount, onProgress, out _);
                if (!ok) return false;
                // Relative path từ working dir. Pattern: SaveData.cs:65 "Program\\" + Project.
                Directory.CreateDirectory(Path.GetDirectoryName(pathModel));
                return trainer.Save(pathModel, defectThreshold, (uint)minDefectArea);
            }
        }

        private (IntPtr, int, int, int) LoadSampleImage(SegSample s) { /* Cv2.ImRead etc */ throw new NotImplementedException(); }
        private (IntPtr, int) BuildMaskFromAnnotations(SegSample s, int w, int h) {
            // Rasterize BrushStrokes (Bresenham line + circle stamp) and Polygons (Cv2.FillPoly)
            // mask: 0 = ignore, 1 = defect, 2 = normal
            throw new NotImplementedException();
        }
        private Mat CropFromOwnerCcd(RectRotate roi, out Matrix m) { throw new NotImplementedException(); }
    }
}
```

### 9.3 `BeeCore/Func/Engines/SegmentAIEngineRunner.cs`

```csharp
using OpenCvSharp;

namespace BeeCore.Func.Engines
{
    public static class SegmentAIEngineRunner
    {
        public sealed class SegmentAIRunResult
        {
            public bool IsOK;
            public float Score;
            public byte[] Mask;
            public int MaskW, MaskH;
            public int DefectPixelCount;
        }

        public static SegmentAIRunResult Run(SegmentAI propety, RectRotate roiArea, RectRotate roiMask, bool complete) {
            propety.DoWork(roiArea, roiMask);
            if (complete) propety.Complete();
            return new SegmentAIRunResult {
                IsOK = propety.IsOK,
                Score = propety.lastScore,
                Mask = propety.lastMask,
                MaskW = propety.lastMaskW,
                MaskH = propety.lastMaskH,
                DefectPixelCount = propety.Counter,
            };
        }
    }
}
```

### 9.4 `BeeGlobal/Enums.cs` — thêm enum value

Verify next free value bằng grep `enum TypeTool` trong [BeeGlobal/Enums.cs](BeeGlobal/Enums.cs). Theo Explore: `CheckMissing=30` đã có; thêm:
```csharp
public enum TypeTool {
    // ... existing ...
    CheckMissing = 30,
    SegmentAI    = 31,
}
```
**Cảnh báo backward-compat**: nếu enum value đã serialize vào `ClassProject.json` của project khách hàng, KHÔNG đổi giá trị enum khác. Chỉ thêm cuối.

### 9.5 `BeeInterface/DataTool.cs` — thêm factory case

Tại [DataTool.cs:297-426](BeeInterface/DataTool.cs:297), trong `New()` switch, sau case `MultiLearning` thêm:
```csharp
case TypeTool.SegmentAI:
    control = new ToolSegmentAI();
    if (IsNew)
        control.Propety = new SegmentAI();
    break;
```

---

## 10. UI Files — Chi Tiết

### 10.1 `BeeInterface/Custom/MaskPainter.cs` (≈ 600 LOC total)

**Quy ước Designer** (memory `feedback_ui_in_designer`): toàn bộ control declaration, layout, event wiring → `.Designer.cs`. File `.cs` chỉ chứa handler body + Bind/Refresh.

**.Designer.cs**:
```csharp
partial class MaskPainter
{
    private System.ComponentModel.IContainer components = null;
    private Cyotek.Windows.Forms.ImageBox imageBox;
    private System.Windows.Forms.Panel panelToolbar;
    private System.Windows.Forms.TrackBar trackBrushSize;
    private System.Windows.Forms.Label lblBrushSize;
    private System.Windows.Forms.Button btnBrushDefect;
    private System.Windows.Forms.Button btnBrushNormal;
    private System.Windows.Forms.Button btnEraser;
    private System.Windows.Forms.Button btnPolygonDefect;
    private System.Windows.Forms.Button btnPolygonNormal;
    private System.Windows.Forms.Button btnClear;
    private System.Windows.Forms.Button btnUndo;
    private System.Windows.Forms.NumericUpDown numOpacity;

    private void InitializeComponent() {
        // ... designer-generated, but wire all events here too:
        this.imageBox.MouseDown -= ImageBox_MouseDown;
        this.imageBox.MouseDown += ImageBox_MouseDown;
        this.imageBox.MouseMove -= ImageBox_MouseMove;
        this.imageBox.MouseMove += ImageBox_MouseMove;
        this.imageBox.MouseUp   -= ImageBox_MouseUp;
        this.imageBox.MouseUp   += ImageBox_MouseUp;
        this.imageBox.Paint     -= ImageBox_Paint;
        this.imageBox.Paint     += ImageBox_Paint;
        // ... other event subs (always -= before +=)
    }
}
```

**.cs (handlers only)**:
```csharp
public partial class MaskPainter : UserControl
{
    public enum PaintMode { Brush, Polygon, Eraser }
    public enum LabelClass { Defect, Normal }

    private Bitmap _backgroundImage;
    private Bitmap _maskOverlay;       // BGRA, alpha 128 cho display
    private bool[,] _maskDefect;       // bool grid same size as image
    private bool[,] _maskNormal;
    private PaintMode _mode = PaintMode.Brush;
    private LabelClass _labelClass = LabelClass.Defect;
    private int _brushSize = 8;
    private bool _drawing = false;
    private Point _lastPt;
    private List<Point> _currentPolygon = new List<Point>();
    private Stack<Action> _undoStack = new Stack<Action>();

    public event EventHandler MaskChanged;

    public MaskPainter() { InitializeComponent(); }

    public void LoadBackground(Bitmap bmp) {
        _backgroundImage = bmp;
        _maskOverlay = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);
        _maskDefect = new bool[bmp.Height, bmp.Width];
        _maskNormal = new bool[bmp.Height, bmp.Width];
        imageBox.Image = _backgroundImage;
        imageBox.Invalidate();
    }

    public byte[] BuildMaskBytes() {
        // Return CV_8UC1: 1 = defect, 2 = normal, 0 = ignore
        int w = _backgroundImage.Width, h = _backgroundImage.Height;
        var data = new byte[w * h];
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++) {
                if (_maskDefect[y, x]) data[y * w + x] = 1;
                else if (_maskNormal[y, x]) data[y * w + x] = 2;
            }
        return data;
    }

    private void ImageBox_MouseDown(object sender, MouseEventArgs e) {
        if (_backgroundImage == null) return;
        var imgPt = imageBox.PointToImage(e.Location);
        _drawing = true;
        _lastPt = imgPt;
        if (_mode == PaintMode.Polygon) {
            _currentPolygon.Add(imgPt);
        } else {
            StampBrush(imgPt);
        }
    }

    private void ImageBox_MouseMove(object sender, MouseEventArgs e) {
        if (!_drawing || _mode == PaintMode.Polygon) return;
        var imgPt = imageBox.PointToImage(e.Location);
        DrawLineBresenham(_lastPt, imgPt);   // stamps brush along the line
        _lastPt = imgPt;
        imageBox.Invalidate();
    }

    private void ImageBox_MouseUp(object sender, MouseEventArgs e) {
        _drawing = false;
        MaskChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ImageBox_Paint(object sender, PaintEventArgs e) {
        // Render mask overlay on top of background
        if (_maskOverlay != null) {
            e.Graphics.DrawImage(_maskOverlay, imageBox.GetImageViewPort());
        }
        // Render in-progress polygon
        if (_mode == PaintMode.Polygon && _currentPolygon.Count > 0) {
            using (var pen = new Pen(_labelClass == LabelClass.Defect ? Color.Red : Color.Lime, 2)) {
                for (int i = 1; i < _currentPolygon.Count; i++)
                    e.Graphics.DrawLine(pen, _currentPolygon[i-1], _currentPolygon[i]);
            }
        }
    }

    private void StampBrush(Point p) {
        // Mark _maskDefect or _maskNormal in radius _brushSize, update _maskOverlay
    }

    private void DrawLineBresenham(Point a, Point b) {
        // Bresenham then call StampBrush per pixel
    }
}
```

### 10.2 `BeeInterface/Tool/ToolSegmentAI.cs` — Form (≈ 800 LOC)

**Tab structure** (mỗi tab là 1 `TabPage` trong `TabControl`):

| Tab | Controls |
|---|---|
| General | `EditRectRot1` (ROI Area/Crop/Mask/Limit). Header `RJButton` collapsible. |
| Training Data | `DataGridView` samples list (cột: thumbnail, defect/normal pixel count, ngày tạo). `Button` Add Sample / Remove / Edit. Khi Edit → mở `MaskPainter` fullscreen modal. |
| Train | `NumericUpDown` numTrees, maxDepth, minSampleCount. `Button` Start / Cancel. `ProgressBar` + `Label` status. `TextBox` lblLog. |
| Inference | `TrackBar` threshold (0..1, step 0.01). `NumericUpDown` minDefectArea. `CheckBox` enableGpu. `Button` Test / Apply. `PictureBox` previewOverlay. `Label` lblScore. |

**Collapsible param sections** (memory `feedback_collapsible_param_sections`):
- Mỗi section có header `RJButton` với `IsTouch=true`.
- Click header → toggle `Visible` của `TableLayoutPanel` con.

**Designer.cs control hierarchy**:
```
ToolSegmentAI (UserControl)
├── tabControl (TabControl, Dock=Fill)
│   ├── tabGeneral (TabPage)
│   │   ├── btnHeaderRoi (RJButton, header section)
│   │   ├── tablePanelRoi (TableLayoutPanel, collapsible)
│   │   │   └── editRectRot1 (EditRectRot)
│   ├── tabData (TabPage)
│   │   ├── btnHeaderSamples (RJButton)
│   │   ├── tablePanelSamples (TableLayoutPanel)
│   │   │   ├── gridSamples (DataGridView)
│   │   │   ├── btnAddSample, btnRemoveSample, btnEditMask (Button)
│   ├── tabTrain (TabPage)
│   │   ├── btnHeaderHyperparams (RJButton)
│   │   ├── tablePanelHyperparams (TableLayoutPanel)
│   │   │   ├── numTrees, numDepth, numMinSamples (NumericUpDown)
│   │   ├── btnHeaderTrain (RJButton)
│   │   ├── tablePanelTrain (TableLayoutPanel)
│   │   │   ├── btnTrainStart, btnTrainCancel (Button)
│   │   │   ├── progressTrain (ProgressBar)
│   │   │   ├── lblTrainStatus (Label)
│   │   │   ├── txtTrainLog (TextBox, Multiline)
│   ├── tabInference (TabPage)
│   │   ├── btnHeaderInferParams (RJButton)
│   │   ├── tablePanelInferParams (TableLayoutPanel)
│   │   │   ├── trackThreshold (TrackBar)
│   │   │   ├── numMinDefectArea (NumericUpDown)
│   │   │   ├── chkEnableGpu (CheckBox)
│   │   ├── btnTest, btnApply (Button)
│   │   ├── picPreview (PictureBox)
│   │   ├── lblScore (Label)
```

**Event wiring** (Designer.cs, mọi `+=` đều có `-=` ngay trên):
```csharp
btnTrainStart.Click -= btnTrainStart_Click;  btnTrainStart.Click += btnTrainStart_Click;
btnTest.Click       -= btnTest_Click;        btnTest.Click       += btnTest_Click;
editRectRot1.RotateCurentChanged -= EditRectRot_RotateCurentChanged;
editRectRot1.RotateCurentChanged += EditRectRot_RotateCurentChanged;
// ... etc
```

**Handler bodies (.cs)**:
```csharp
public partial class ToolSegmentAI : UserControl
{
    public SegmentAI Propety { get; set; }
    private PropetyTool OwnerTool => Common.TryGetTool(/* index */, out var t) ? t : null;

    public ToolSegmentAI() {
        InitializeComponent();
        if (Propety == null) Propety = new SegmentAI();
    }

    public void LoadPara() {
        editRectRot1.Rot = new List<RectRotate> { Propety.rotArea, Propety.rotCrop, Propety.rotMask, Propety.rotLimit };
        numTrees.Value = Propety.numTrees;
        numDepth.Value = Propety.maxDepth;
        numMinSamples.Value = Propety.minSampleCount;
        trackThreshold.Value = (int)(Propety.defectThreshold * 100);
        numMinDefectArea.Value = Propety.minDefectArea;
        chkEnableGpu.Checked = Propety.enableGpu;
        RefreshSamplesGrid();
        // Subscribe PercentChange (always -= before +=)
        Propety.PercentChange -= Propety_PercentChange;
        Propety.PercentChange += Propety_PercentChange;
    }

    private void btnTrainStart_Click(object sender, EventArgs e) {
        if (Propety.samples.Count == 0) {
            MessageBox.Show("Add at least 1 sample"); return;
        }
        Propety.numTrees = (int)numTrees.Value;
        Propety.maxDepth = (int)numDepth.Value;
        Propety.minSampleCount = (int)numMinSamples.Value;
        // Compose model path
        if (string.IsNullOrEmpty(Propety.pathModel)) {
            string folder = $"Program\\{Global.Project}\\SegAI_{OwnerTool?.IndexTool:D3}";
            Propety.pathModel = Path.Combine(folder, "model.segai");
        }
        // Run via OwnerTool.worker
        OwnerTool.worker.DoWork -= TrainDoWork;
        OwnerTool.worker.DoWork += TrainDoWork;
        OwnerTool.worker.RunWorkerAsync();
    }

    private void TrainDoWork(object sender, DoWorkEventArgs e) {
        var cts = new CancellationTokenSource();
        Propety.Train(p => Invoke((Action)(() => progressTrain.Value = p)), cts.Token);
    }

    private void btnTest_Click(object sender, EventArgs e) {
        Propety.defectThreshold = trackThreshold.Value / 100f;
        Propety.minDefectArea = (int)numMinDefectArea.Value;
        Propety.enableGpu = chkEnableGpu.Checked;
        Propety.SetModel();
        var result = SegmentAIEngineRunner.Run(Propety, Propety.rotArea, Propety.rotMask, true);
        RenderPreview(result);
    }

    private void Propety_PercentChange(int p) {
        if (InvokeRequired) Invoke((Action)(() => progressTrain.Value = p));
        else progressTrain.Value = p;
    }
}
```

---

## 11. Pipeline Integration Points

### 11.1 Scan loop hook

Trong [BeeCore/Func/Camera.cs](BeeCore/Func/Camera.cs) hoặc [BeeCore/Vision.cs](BeeCore/Vision.cs) (cần Read trước Week 4 để xác định chính xác), khi từng tool được iterate trong vòng scan:

```csharp
// Pseudo-code, exact location TBD Week 4:
foreach (var tool in toolList) {
    switch (tool.TypeTool) {
        // ... existing cases ...
        case TypeTool.SegmentAI:
            var segAi = tool.Propety as SegmentAI;
            if (segAi != null && segAi.inferer != null) {
                SegmentAIEngineRunner.Run(segAi, segAi.rotArea, segAi.rotMask, true);
            }
            break;
    }
}
```

### 11.2 Project load/save

Khi project load tại [BeeCore/Data/LoadData.cs:22-30](BeeCore/Data/LoadData.cs:22):
- `SegmentAI` POCO đã `[Serializable]` → BinaryFormatter handle tự động.
- Sau deserialize, gọi `SegmentAI.SetModel()` để re-instantiate `NativeSegAIInferer` từ `pathModel`.
- Trigger: `DataTool.BuildProjectUI()` sau khi `CreateControls()` xong, call `(tool.Propety as SegmentAI)?.SetModel()`.

### 11.3 File system layout cho 1 SegmentAI tool

```
Program\<ProjectName>\
├── project.json (or .prog)
└── SegAI_001\                       # SegmentAI tool #1 in this project
    ├── model.segai                  # binary model (only after Train succeeds)
    ├── samples\
    │   ├── sample01.png             # original BGR sample
    │   ├── sample01_mask.png        # rasterized mask (CV_8UC1: 1=def, 2=nor, 0=ig)
    │   ├── sample01_meta.json       # brush/polygon vector data
    │   └── ... sample02..N
    └── log.txt                      # train history
```

---

## 12. Build & Solution Updates

### 12.1 `EasyVision.sln`

Trong VS: Add → Existing Project → `BeeNativeSegAI/BeeNativeSegAI.vcxproj`. Lưu sln.

Mở **Project Dependencies** dialog:
- `BeeCore` depends on `BeeNativeSegAI` (build order — DLL phải có trước khi C# binding compile lookup).

Configuration Manager (Release|x64):
- Tick Build cho `BeeNativeSegAI`.
- Output to `BeeNativeSegAI\x64\Release\BeeNativeSegAI.dll`.

Post-build event của `BeeNativeSegAI.vcxproj`:
```xml
<PostBuildEvent>
  <Command>copy /Y "$(OutDir)BeeNativeSegAI.dll" "$(SolutionDir)BeeMain\bin\x64\Release\"</Command>
</PostBuildEvent>
```

### 12.2 `BeeCore/BeeCore.csproj`

Thêm vào ItemGroup `<Compile>`:
```xml
<Compile Include="Func\NativeSegAI.cs" />
<Compile Include="Unit\SegmentAI.cs" />
<Compile Include="Func\Engines\SegmentAIEngineRunner.cs" />
```

### 12.3 `BeeInterface/BeeInterface.csproj`

Thêm:
```xml
<Compile Include="Custom\MaskPainter.cs">
  <SubType>UserControl</SubType>
</Compile>
<Compile Include="Custom\MaskPainter.Designer.cs">
  <DependentUpon>MaskPainter.cs</DependentUpon>
</Compile>
<EmbeddedResource Include="Custom\MaskPainter.resx">
  <DependentUpon>MaskPainter.cs</DependentUpon>
</EmbeddedResource>

<Compile Include="Tool\ToolSegmentAI.cs">
  <SubType>UserControl</SubType>
</Compile>
<Compile Include="Tool\ToolSegmentAI.Designer.cs">
  <DependentUpon>ToolSegmentAI.cs</DependentUpon>
</Compile>
<EmbeddedResource Include="Tool\ToolSegmentAI.resx">
  <DependentUpon>ToolSegmentAI.cs</DependentUpon>
</EmbeddedResource>
```

---

## 13. Task Cards Chi Tiết (13 atomic cards)

> Mỗi card ≤ 2 ngày dev. Theo template [CLAUDE.md §11.1](CLAUDE.md). Đặt vào `docs/architecture/tasks/YYYY-MM-DD-P3X.1.<n>-<slug>.md`.

### Task P3X.1.1 — Tạo project BeeNativeSegAI skeleton

- **Mục tiêu**: Có project mới build pass empty, link OpenCV, output `BeeNativeSegAI.dll`.
- **Preconditions**: baseline build pass (Phase 0 done). git status clean hoặc user confirm.
- **In-scope**: `BeeNativeSegAI/BeeNativeSegAI.vcxproj`, `BeeNativeSegAI/pch.h`, `framework.h`, `dllmain.cpp`, `BeeNativeSegAI.cpp` (stub có `SEGAI_GetVersion` → return 1). `EasyVision.sln`.
- **Out-of-scope**: feature/trainer/inferer code, C# binding.
- **Steps**:
  1. Copy `BeeNativeOnnx/BeeNativeOnnx.vcxproj` thành template, thay GUID + RootNamespace + AdditionalDependencies (chỉ OpenCV).
  2. Tạo pch.h, framework.h, dllmain.cpp standard Windows DLL boilerplate.
  3. Tạo `SegAINativeExport.h` (header only Phase 1.1 — chỉ `SEGAI_GetVersion`, `SEGAI_GetBuildInfo`).
  4. Add vào sln.
  5. Build Release|x64.
- **Verify**: `MSBuild BeeNativeSegAI/BeeNativeSegAI.vcxproj /t:Build /p:Configuration=Release /p:Platform=x64` pass. `dir BeeNativeSegAI\x64\Release\BeeNativeSegAI.dll` exists.
- **DoD**: DLL build, exports `SEGAI_GetVersion`. CODEX_HISTORY appended.
- **Rollback**: `git reset --hard HEAD~1`, xoá project khỏi sln.

### Task P3X.1.2 — Implement `SegFeatureExtractor::ExtractCPU` 24-D

- **Mục tiêu**: feature stack chính xác, có unit-check.
- **Preconditions**: P3X.1.1 done.
- **In-scope**: `SegFeatureCore.h`, `SegFeatureCore.cpp`.
- **Out-of-scope**: GPU path.
- **Steps**:
  1. Define `FeatureConfig` struct + class signatures.
  2. Implement `Configure` (precompute Gabor kernels, LBP LUT).
  3. Implement `ExtractCPU` per pseudo-code §8.3.
  4. Implement `PackSamples`, `PlanesToInterleaved`.
  5. Add temporary test entry trong `BeeNativeSegAI.cpp`: load 1 ảnh test, run `ExtractCPU`, dump plane[0]..plane[23] ra `.bin` để verify visual.
- **Verify**: build pass; chạy DLL bằng exe wrapper nhỏ (Task P3X.1.5 sẽ tạo). So sánh plane[0] (LBP) với ảnh expected (precompute Python script reference).
- **DoD**: `ExtractCPU` ra 24 plane đúng shape, value range hợp lý ([0,1] cho mọi plane).
- **Rollback**: `git reset --hard HEAD~1`.

### Task P3X.1.3 — Implement `SegTrainer` + RTrees integration

- **In-scope**: `SegTrainerCore.h/.cpp`, `SegAIFileFormat.h/.cpp`.
- **Steps**:
  1. Class signature SegTrainer per §8.4.
  2. `AddSample` rasterize mask, extract features, store sample.
  3. `Train` per pseudo-code (class balance + RTrees configure + train).
  4. `SaveModel` write `.segai` per format §5: header struct + RTrees blob via FileStorage MEMORY.
  5. `LoadModel` parse header, restore RTrees, verify CRC.
- **Verify**: build pass; tạm gọi từ test entry — train 1 ảnh dummy 100×100 với mask checkerboard, save, load, predict pixel (50,50). Confirm RTrees đoán đúng.
- **DoD**: train + save + load round-trip; CRC verified.

### Task P3X.1.4 — Implement `SegInferer::Predict` (CPU only)

- **In-scope**: `SegInferCore.h/.cpp`.
- **Steps**:
  1. Class signature + Predict per §8.5.
  2. Implement parallel_for_ batched predict.
  3. Connected components filter min area.
- **Verify**: build pass; chạy predict trên ảnh test trained ở P3X.1.3, mask output > 0 ở vùng defect.
- **DoD**: predict deterministic, score scalar trong [0,1].

### Task P3X.1.5 — Native exports + standalone test exe

- **In-scope**: `SegAINativeExport.cpp` (full), `BeeNativeSegAI/test/SegAITest.cpp`, `BeeNativeSegAI/test/data/` (test images folder).
- **Stop-and-ask**: kiểm `BeeNativeSegAI/test/data/sample_defect_*.png` có chưa. Nếu chưa, hỏi user cung cấp ≥ 5 cặp ảnh + mask. Fallback: agent tự generate synthetic checkerboard 256×256 với defect = 1 ô đen ở giữa ảnh trắng.
- **Steps**:
  1. Implement tất cả `SEGAI_*` exports theo signature §8.2.
  2. Tạo separate vcxproj `SegAITest.vcxproj` (Console, link BeeNativeSegAI.lib) hoặc CMake file để build CLI test.
  3. Run test: `SegAITest.exe BeeNativeSegAI/test/data/sample_defect_01.png BeeNativeSegAI/test/data/sample_mask_01.png` → train, save, infer, output `out_pred.png`.
- **Verify**: 
  - `dumpbin /EXPORTS BeeNativeSegAI.dll` show tất cả `SEGAI_*` symbols.
  - `out_pred.png` có vùng trắng overlap với `sample_mask.png` ≥ 60% IoU (acceptable ≥ 50% nếu dùng synthetic).
- **DoD**: end-to-end native pipeline pass standalone, IoU benchmark ghi vào CODEX_HISTORY (root file).

### Task P3X.1.6 — C# wrapper `NativeSegAI.cs`

- **In-scope**: `BeeCore/Func/NativeSegAI.cs`, `BeeCore/BeeCore.csproj`.
- **Steps**:
  1. Implement `NativeSegAITrainer` per §9.1.
  2. Implement `NativeSegAIInferer` mirror pattern.
  3. Add `<Compile Include>` vào csproj.
  4. Subscribe `Common.PropetyTools` rule — KHÔNG access trực tiếp.
- **Verify**: 
  - `MSBuild BeeCore/BeeCore.csproj /t:Build /p:Configuration=Release /p:Platform=x64` pass.
  - `bash tools/check_propety_tools.sh` exit 0.
  - Sample console app: instantiate `NativeSegAITrainer`, call methods on 1 image, dispose. No leak (kiểm `_handle = IntPtr.Zero` sau Dispose).
- **DoD**: full build pass; warning ≤ baseline **447** (theo [docs/architecture/baseline_build.md](docs/architecture/baseline_build.md) 2026-05-03).

### Task P3X.1.7 — `SegmentAI` POCO + `DoWork`/`Complete`

- **In-scope**: `BeeCore/Unit/SegmentAI.cs`.
- **Steps**:
  1. Implement `SegSample` + `SegmentAI` POCO theo §9.2.
  2. `SetModel`, `DoWork`, `Complete`, `Train` methods.
  3. `BuildMaskFromAnnotations` rasterize brush + polygon → byte[].
  4. `LoadSampleImage` từ `pathSamplesFolder`.
- **Verify**:
  - Build pass.
  - Unit test trong console: tạo `SegmentAI`, gán 1 SegSample, train, save model.
  - **ROI coord rule** (memory `project_roi_local_to_global`): test ROI rot=15° → CropFromOwnerCcd dùng `Matrix(Translate→Rotate→Translate)` đúng.
- **DoD**: SegmentAI clonable (`Clone()` deep-enough), serializable (BinaryFormatter round-trip).

### Task P3X.1.8 — Engine runner + enum + factory

- **In-scope**: `BeeCore/Func/Engines/SegmentAIEngineRunner.cs`, `BeeGlobal/Enums.cs`, `BeeInterface/DataTool.cs`.
- **Steps**:
  1. Implement `SegmentAIEngineRunner` per §9.3.
  2. Add `TypeTool.SegmentAI = 31` to enum.
  3. Add case vào `DataTool.New()`.
- **Verify**: full build pass; trong app, add tool mới → menu show "SegmentAI" option.
- **DoD**: tool register vào factory; create new tool không exception.

### Task P3X.1.9 — `MaskPainter` UserControl

- **In-scope**: `BeeInterface/Custom/MaskPainter.{cs,Designer.cs,resx}`, `BeeInterface/BeeInterface.csproj`.
- **Steps**:
  1. Designer.cs: declare controls + InitializeComponent + event wiring (`-=` rồi `+=`).
  2. .cs: handler bodies + `LoadBackground`, `BuildMaskBytes`, `StampBrush`, `DrawLineBresenham`.
  3. Brush throttle: MouseMove qua `Stopwatch` skip nếu < 16ms từ event trước.
- **Verify**: 
  - Build pass.
  - Open .Designer.cs trong VS Designer → render OK.
  - Test form (tạm) host `MaskPainter`, load 1 PNG, brush → mask hiện ngay, BuildMaskBytes trả byte array đúng size.
- **DoD**: brush + polygon work; undo có 1 step; clear all work; emit MaskChanged event.

### Task P3X.1.10 — `ToolSegmentAI` Form

- **In-scope**: `BeeInterface/Tool/ToolSegmentAI.{cs,Designer.cs,resx}`.
- **Steps**:
  1. Designer.cs: TabControl 4 tabs theo §10.2 với RJButton collapsible.
  2. .cs: handler bodies, `LoadPara`, `btnTrainStart_Click`, `btnTest_Click`, `Propety_PercentChange`.
  3. Add Sample button → open file dialog, load image, mở `MaskPainter` modal → save mask to `pathSamplesFolder`.
- **Verify**:
  - Full solution build pass.
  - Smoke: open app, add SegmentAI tool, vẽ 5 brush stroke defect + 3 normal trên 5 ảnh, click Train → progress chạy, mask file `.segai` tạo. Click Test → mask preview hiện.
  - Event balance: `+=` count = `-=` count trong file.
- **DoD**: end-to-end UI flow work với 5 ảnh test. Memory `feedback_ui_in_designer` & `feedback_collapsible_param_sections` respected.

### Task P3X.1.11 — GPU UMat inference path

- **In-scope**: `SegFeatureCore.cpp` (ExtractGpu), `SegInferCore.cpp` (PredictGpu).
- **Steps**:
  1. Implement `ExtractGpu` mirror `ExtractCPU` but UMat ops.
  2. `SegInferer::PredictGpu` use UMat for feature, download stack, parallel_for_ predict.
  3. Setup `cv::ocl::setUseOpenCL(true)` mutex-protected scope (copy từ Pattern2.cpp).
  4. Add `SEGAI_InferSetGpu`, `SEGAI_InferGetGpuAvailable` exports.
- **Verify**:
  - Build pass.
  - Bench: 1280×960 image with ROI 512×512 → infer time:
    - CPU only: log baseline (expect 400-600ms).
    - GPU UMat feature + CPU predict: log time (target ≤ 200ms).
  - Visual diff CPU mask vs GPU mask: ≥ 99% pixel agreement (small float diff acceptable).
- **DoD**: GPU path ≥ 1.5× faster than CPU; mask agreement ≥ 99%.

### Task P3X.1.12 — Pipeline integration

- **In-scope**: `BeeCore/Func/Camera.cs` hoặc `BeeCore/Vision.cs` (exact file TBD by inspection Week 4), call site `SegmentAIEngineRunner.Run`.
- **Steps**:
  1. Locate switch theo `TypeTool` trong scan loop.
  2. Add `case TypeTool.SegmentAI: SegmentAIEngineRunner.Run(...)` theo pattern các tool khác.
  3. Ensure `Propety.SetModel()` called sau load project (hook trong `DataTool.BuildProjectUI()`).
- **Verify**:
  - Smoke: project saved có tool SegmentAI → restart app → load lại → trigger scan → mask overlay hiện trên live image.
  - Performance: scan loop FPS với SegmentAI active không drop < 4 FPS trên 1280×960.
- **DoD**: end-to-end scan-line integration; FPS report ghi CODEX_HISTORY.

### Task P3X.1.13 — Hardening + history + tag

- **In-scope**: edge case handling + `CODEX_HISTORY.md` + git tag.
- **Steps**:
  1. Edge: 0 samples → block Train với MessageBox.
  2. Edge: class imbalance > 10:1 → warning dialog.
  3. Edge: model file missing on project load → SegmentAI.IsIni = false + log warning, không crash.
  4. Edge: ROI rot=45° crop có boundary effect → test 3 case rotation (15°, 45°, -30°).
  5. Append `CODEX_HISTORY.md` section "P3X.1 ToolSegmentAI MVP" per template §11.2.
  6. `git tag toolsegmentai-mvp-v1 -m "ToolSegmentAI MVP Phase 1 done"`.
- **Verify**: all edge cases covered, history section complete.
- **DoD**: tag exists, history committed.

---

## 14. Verification (Tổng Hợp)

### Build verification (chạy sau Task P3X.1.10 và .12)

```powershell
MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal > build.log 2>&1
Select-String -Path build.log -Pattern ': warning ' | Measure-Object
Select-String -Path build.log -Pattern ': error '   | Measure-Object
```
- Warning count ≤ **467** (baseline 447 từ [docs/architecture/baseline_build.md](docs/architecture/baseline_build.md) + tối đa 20 native new).
- 0 errors.
- Output files:
  - `BeeNativeSegAI/x64/Release/BeeNativeSegAI.dll`
  - `BeeMain/bin/x64/Release/BeeNativeSegAI.dll` (post-build copy)
  - `BeeCore/bin/x64/Release/BeeCore.dll` updated
  - `BeeInterface/bin/x64/Release/BeeInterface.dll` updated

### Convention guards

```bash
bash tools/check_propety_tools.sh   # expect exit 0
```

PowerShell event balance check trong ToolSegmentAI:
```powershell
$f = "BeeInterface/Tool/ToolSegmentAI.cs", "BeeInterface/Tool/ToolSegmentAI.Designer.cs"
$plus  = (Select-String -Path $f -Pattern ' \+=' ).Count
$minus = (Select-String -Path $f -Pattern ' -=' ).Count
"$plus == $minus ?"
```

### Performance benchmark (Task P3X.1.11)

| Metric | Target | Acceptable |
|---|---|---|
| Train 5 ảnh 1280×960 | ≤ 90s | ≤ 150s |
| Infer 1280×960 (ROI 512×512) CPU | log baseline | — |
| Infer 1280×960 (ROI 512×512) GPU UMat | ≤ 200ms | ≤ 350ms |
| Memory peak | ≤ 1.5 GB | ≤ 2 GB |
| Mask IoU vs ground truth (sample test set) | ≥ 0.6 | ≥ 0.5 |
| CPU vs GPU mask pixel agreement | ≥ 99% | ≥ 97% |

### End-to-end smoke test (final)

Test script (manual, ghi vào CODEX_HISTORY):
1. Open app EasyVision.
2. New project "test_segai".
3. Add tool SegmentAI (vai trò Tool #1).
4. Tab General → set ROI Area = (100, 100, 800, 600), rot = 0°.
5. Tab Training Data → Add Sample × 5 (load 5 ảnh defect_*.png). Trên mỗi ảnh:
   - Brush mode + Defect class → vẽ ~3000 pixel trên vùng defect.
   - Brush mode + Normal class → vẽ ~5000 pixel trên vùng nền OK.
   - Save → mask raster + meta.json lưu vào `Program\test_segai\SegAI_001\samples\`.
6. Tab Train → numTrees=100, maxDepth=12 → Start → progress 0→100% in ~80s → status "Model saved".
7. Tab Inference → threshold=0.5, minDefectArea=50, GPU=on → Test → mask overlay hiện trên picPreview, lblScore = 0.0xx.
8. File → Save Project (Ctrl+S).
9. Close app. Open app. Load "test_segai". Tool SegmentAI hiện đầy đủ 5 samples + model loaded.
10. Click Test lại → mask gen ra giống lần đầu (pixel diff < 1%).
11. Start camera scan → mask overlay hiện realtime trên frame stream, không freeze UI.

Pass tiêu chuẩn: tất cả 11 bước thành công, không exception trong log.

---

## 15. Performance & Profiling Plan

**Expected bottlenecks** (theo thứ tự nghi ngờ):
1. `RTrees::predict` (CPU only) — ~60% time trên 512×512 ROI. Mitigate: parallel_for_ + chỉ predict ROI.
2. Feature extract `cvtColor + Laplacian + Sobel + Gabor` — ~30% time CPU, < 10% GPU (UMat path).
3. `connectedComponentsWithStats` — ~5%, fixed.
4. C# ↔ C++ buffer copy (mask byte[]) — ~3-5%, scalable.

**Profiling tooling**:
- VTune Amplifier (Intel) trên `BeeNativeSegAI.dll` để confirm hotspot.
- C++ `QueryPerformanceCounter` quanh từng giai đoạn — log ra console khi predict.

**Phase 2 optimization** (nếu Phase 1 không đạt 200ms):
- Reduce feature dim từ 24 → 12 (drop Gabor 7-10 và edge density 19-21 nếu importance thấp theo RTrees feature_importance).
- LUT lookup (Phase 2 considered, expensive memory).
- Custom OpenCL kernel cho RTrees predict (research project, không phải Phase 1).

---

## 16. Risks & Mitigation

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| `RTrees::predict` không có OpenCL → infer chậm | Cao | Trung bình | parallel_for_ + chỉ predict trong ROI. Phase 2: feature dim reduction. |
| 24-D feature không đủ phân biệt defect tinh vi | Trung bình | Cao | Cho user tune brush size + thêm samples. Phase 2: DCT block 4×4, hoặc shallow CNN. |
| MaskPainter performance với ảnh > 4MP | Trung bình | Thấp | Render brush vào off-screen Bitmap, throttle MouseMove 16ms. |
| `.segai` không backward-compatible khi đổi feature config | Cao | Trung bình | Header version + featureFlags + CRC. Refuse load nếu mismatch. Phase 2 bump version=2. |
| Build thêm DLL mới làm lệch baseline warning | Thấp | Thấp | Cho phép tăng tối đa 20 warning native. Document CODEX_HISTORY. |
| **ROI local→global coords bug** (mask sai khi rot ≠ 0) | Cao | Cao | **PHẢI** dùng `Matrix(Translate→Rotate→Translate)` (memory `project_roi_local_to_global`). Test case ROI rot=15°, 45°, -30° trước commit P3X.1.10. |
| Pattern2/Pitch concurrent dev conflict | Thấp | Trung bình | Plan hoàn toàn không động `Pattern/Pattern2.*`, `Pattern/Pitch*` ([CLAUDE.md §0.2.8](CLAUDE.md)). DLL native mới hoàn toàn riêng. |
| BinaryFormatter compat khi SegSample/SegmentAI tiến hoá | Cao | Cao | Thêm field mới: declare default value để deserialize cũ vẫn work. Nếu rename field → giữ [OptionalField] + property alias. Test load 1 file `.prog` cũ trước commit. |
| OpenCL driver version không match trên máy khách | Trung bình | Trung bình | Detect qua `cv::ocl::Device::vendorName()` + version. Fallback CPU tự động nếu < OpenCL 1.2. |
| User vẽ mask không cân bằng (vd toàn defect, 0 normal) | Cao | Cao | Validate trước Train; báo lỗi rõ "Cần ≥ 100 pixel mỗi class". Cho phép class imbalance ratio ≤ 10:1 sau warning. |
| Memory leak từ C++ buffer khi Predict bị exception giữa chừng | Trung bình | Cao | Pattern `try/finally` quanh `Predict` + `SEGAI_FreeBuffer`. Native side: smart pointer ownership. |
| Thread safety RTrees::predict đồng thời với load | Thấp | Cao | Inferer.Load không cho gọi khi đang Predict. Lock `_loadLock` C# side. |

---

## 17. Out of Scope (Phase 2+)

- **Multi-class** (>2 lớp) — Phase 2.
- **OpenVINO IR export** — Phase 2 nếu chuyển sang shallow CNN.
- **Augmentation tự động** (rotate/flip/brightness, mixup) — Phase 2.
- **INT8 quantization** với NNCF — Phase 2.
- **Distributed training, GPU training thực sự** (oneDNN/SYCL) — Phase 3.
- **Anomaly detection mode** (one-class, không cần defect sample, dùng iForest/Mahalanobis) — Phase 2.
- **Active learning** (suggest pixel nên label tiếp) — Phase 3.
- **Model versioning + A/B** trong project file — Phase 2.

---

## 18. Timeline (Gantt)

```
Week 1 (Native foundation)
  P3X.1.1 ──Day1── Project skeleton + build
  P3X.1.2 ────Day2-3──── FeatureExtractor CPU
  P3X.1.3 ────Day3-4──── Trainer + RTrees + .segai save/load
  P3X.1.4 ────Day4-5──── Inferer CPU
  P3X.1.5 ────Day5──── Exports + test exe

Week 2 (C# binding)
  P3X.1.6 ──Day1-2── NativeSegAI.cs P/Invoke
  P3X.1.7 ────Day2-4──── SegmentAI POCO + DoWork
  P3X.1.8 ────Day4-5──── Engine runner + enum + factory

Week 3 (UI)
  P3X.1.9 ────Day1-3──── MaskPainter UserControl
  P3X.1.10 ──────Day3-5────── ToolSegmentAI Form

Week 4 (GPU + integration)
  P3X.1.11 ────Day1-3──── GPU UMat path + bench
  P3X.1.12 ────Day3-5──── Pipeline integration + smoke

Week 5 (Hardening)
  P3X.1.13 ──Day1-3── Edge cases + history + tag
  Buffer ────Day3-5──── Bug-fix slack, user demo
```

---

## 19. References Trong Plan

- Pattern AI tool hiện có: [BeeInterface/Tool/ToolYolo.cs](BeeInterface/Tool/ToolYolo.cs), [BeeCore/Unit/Yolo.cs](BeeCore/Unit/Yolo.cs).
- POCO/Serialize pattern: [BeeCore/Unit/OKNG.cs:30-99](BeeCore/Unit/OKNG.cs:30).
- Native binding template: [BeeCore/Func/NativeYolo.cs:10-160](BeeCore/Func/NativeYolo.cs:10).
- Native export template: [BeeNativeOnnx/YoloNativeExport.h:17-30](BeeNativeOnnx/YoloNativeExport.h:17), [BeeNativeOnnx/OpenVinoYoloHP.h:20-83](BeeNativeOnnx/OpenVinoYoloHP.h:20).
- vcxproj template: [BeeNativeOnnx/BeeNativeOnnx.vcxproj:1-148](BeeNativeOnnx/BeeNativeOnnx.vcxproj).
- Factory: [BeeInterface/DataTool.cs:297-426](BeeInterface/DataTool.cs:297).
- Engine runner template: [BeeCore/Func/Engines/CircleEngineRunner.cs](BeeCore/Func/Engines/CircleEngineRunner.cs).
- OpenCL UMat pattern: [Pattern/Pattern2.cpp:36-44](Pattern/Pattern2.cpp:36).
- ROI editor: [BeeInterface/Group/EditRectRot.cs](BeeInterface/Group/EditRectRot.cs).
- Resolver helpers: [BeeCore/Common.cs](BeeCore/Common.cs) (Common.TryGetTool / EnsureToolList / SetToolList).
- Convention rules: [CLAUDE.md §0](CLAUDE.md).
- Task Card template: [CLAUDE.md §11.1](CLAUDE.md).
- History template: [CLAUDE.md §11.2](CLAUDE.md).
- ROI coord rule: memory `project_roi_local_to_global` (Translate→Rotate→Translate matrix).
- Designer rule: memory `feedback_ui_in_designer`.
- Collapsible param rule: memory `feedback_collapsible_param_sections`.
- Clone tool pattern (nếu sau này có `Tool2SegmentAI`): memory `project_clone_tools`.
