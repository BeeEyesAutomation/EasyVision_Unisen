# Prompt cho Codex — Bổ sung Preprocess Pipeline (Gray / Edge / GrayPlusEdge) cho Pattern2

## 0. Ngữ cảnh dự án

- Dự án: `E:\Code\EasyVision_Unisen`, module matching template nằm ở `Pattern\Pattern2.h` và `Pattern\Pattern2.cpp`.
- Ngôn ngữ: **C++/CLI** (managed wrapper `ref class Pattern2` expose sang C#), sử dụng **OpenCV 4.x**.
- Các hàm cốt lõi hiện có: `LearnPattern()`, `LearnPatternStable(Pattern2StableConfig)`, `Match(...)`, `MatchStable(Pattern2StableConfig)`.
- Validator hiện dùng fused score: `wBase·coarseNCC + wRaw·rawNCC + wGrad·gradNCC + wEdgeIou·IoU + wEdgeRatio·edgeRatio`.
- Struct có sẵn cần chú ý: `s_TemplData`, `s_StableScaleTemplate`, `s_MatchParameter`, `Pattern2StableConfig`, `PatternMatchOptions` (struct này hiện **không được dùng ở đâu**, được phép xoá).

**KHÔNG được phá vỡ API hiện có** (`LearnPattern`, `Match` legacy) — chỉ mở rộng. Tất cả thay đổi phải compile được với MSVC C++/CLI, không thêm dependency ngoài OpenCV.

---

## 1. Mục tiêu

Thêm pipeline tiền xử lý **áp dụng nhất quán cho cả template (mẫu) và ảnh test**, hỗ trợ 3 miền đặc trưng:

- `Gray`: NCC trên ảnh xám đã được tăng cường.
- `Edge`: NCC trên gradient magnitude / edge map.
- `GrayPlusEdge`: coarse chạy trên gray (nhanh, robust vị trí), validator cộng thêm NCC trên edge (chặt về shape).

Cho phép người dùng C# chỉnh qua một struct config mới `Pattern2PreprocessConfig` **được lưu kèm template khi Learn** và **replay đúng lúc Match** — không cho user đổi pipeline ở thời điểm match để tránh lệch mẫu-test.

---

## 2. Công việc cụ thể

### 2.1. Thêm enum và struct trong `Pattern2.h`

Đặt ngay trên `Pattern2StableConfig`:

```cpp
public enum class Pattern2FeatureDomain
{
    Gray         = 0,
    Edge         = 1,
    GrayPlusEdge = 2
};

public enum class Pattern2EdgeMethod
{
    SobelMag    = 0,
    ScharrMag   = 1,
    Canny       = 2,
    Laplacian   = 3
};

public enum class Pattern2DenoiseMethod
{
    None      = 0,
    Gaussian  = 1,
    Median    = 2,
    Bilateral = 3
};

public value struct Pattern2PreprocessConfig
{
    // Common
    bool   EnableBitwiseNot;

    // Illumination / contrast
    bool   EnableIlluminationFix;   // gray - blur(gray, IllumKernel) + 128
    int    IllumKernel;             // odd, 0 => auto theo min(w,h)/8
    bool   EnableCLAHE;
    double ClaheClip;               // 1.5..4.0, mặc định 2.0
    int    ClaheTile;               // mặc định 8
    bool   EnableGammaCorrection;
    double Gamma;                   // 0.5..2.2, mặc định 1.0

    // Denoise
    Pattern2DenoiseMethod DenoiseMethod;
    int    DenoiseKernel;           // 3/5/7

    // Feature domain
    Pattern2FeatureDomain Domain;

    // Edge-specific
    Pattern2EdgeMethod EdgeMethod;
    int    EdgeKernel;              // 3 hoặc 5
    double CannyLow;                // 0 => Otsu auto
    double CannyHigh;               // 0 => Otsu auto
    bool   EdgeKeepMagnitude;       // true: 0..255 mag; false: nhị phân 0/255
    int    EdgeDilatePx;            // 0..3
    bool   EnableEdgeLengthFilter;
    int    MinEdgeSegmentLen;       // px

    // Fuse (Domain = GrayPlusEdge)
    double FuseGrayWeight;          // 0..1; phần còn lại cho edge

    // Auto
    bool   AutoPickDomain;          // nếu true, chọn Domain theo modelEdgeDensity/entropy

    Pattern2PreprocessConfig(bool init)
    {
        EnableBitwiseNot = false;
        EnableIlluminationFix = false;
        IllumKernel = 0;
        EnableCLAHE = false;
        ClaheClip = 2.0;
        ClaheTile = 8;
        EnableGammaCorrection = false;
        Gamma = 1.0;
        DenoiseMethod = Pattern2DenoiseMethod::None;
        DenoiseKernel = 3;
        Domain = Pattern2FeatureDomain::Gray;
        EdgeMethod = Pattern2EdgeMethod::Canny;
        EdgeKernel = 3;
        CannyLow = 0.0;
        CannyHigh = 0.0;
        EdgeKeepMagnitude = true;
        EdgeDilatePx = 1;
        EnableEdgeLengthFilter = false;
        MinEdgeSegmentLen = 6;
        FuseGrayWeight = 0.5;
        AutoPickDomain = false;
    }
};
```

### 2.2. Tích hợp vào `Pattern2StableConfig`

Thêm field:

```cpp
Pattern2PreprocessConfig Preprocess;
```

Trong ctor `Pattern2StableConfig(bool init)`, khởi tạo:

```cpp
Preprocess = Pattern2PreprocessConfig(true);
```

**Giữ nguyên `BitwiseNot` cũ để tương thích ngược** nhưng nếu `Preprocess.EnableBitwiseNot == true` thì ưu tiên giá trị mới (OR logic, ghi rõ trong comment).

### 2.3. Xoá/Deprecate `PatternMatchOptions`

Struct `PatternMatchOptions` ở Pattern2.h đang **không được tham chiếu ở bất kỳ đâu trong Pattern2.cpp**. Xoá hoàn toàn khỏi header và bất kỳ forward declaration nào để làm sạch API.

### 2.4. Lưu preprocess kèm template

Trong `s_TemplData` thêm:

```cpp
// Preprocess snapshot (managed->native chuyển về POD)
bool   ppEnableBitwiseNot     = false;
bool   ppEnableIlluminationFix= false;
int    ppIllumKernel          = 0;
bool   ppEnableCLAHE          = false;
double ppClaheClip            = 2.0;
int    ppClaheTile            = 8;
bool   ppEnableGamma          = false;
double ppGamma                = 1.0;
int    ppDenoiseMethod        = 0;   // map từ enum
int    ppDenoiseKernel        = 3;
int    ppDomain               = 0;   // Pattern2FeatureDomain
int    ppEdgeMethod           = 2;   // Canny default
int    ppEdgeKernel           = 3;
double ppCannyLow             = 0.0;
double ppCannyHigh            = 0.0;
bool   ppEdgeKeepMagnitude    = true;
int    ppEdgeDilatePx         = 1;
bool   ppEnableEdgeLengthFilter = false;
int    ppMinEdgeSegmentLen    = 6;
double ppFuseGrayWeight       = 0.5;
```

Và một cặp cache feature của template:

```cpp
cv::Mat tplPreprocessedGray;   // sau Gray-pipeline (CLAHE, denoise, illum...)
cv::Mat tplEdgeMagnitude;      // gradient magnitude CV_8U (nếu Domain != Gray)
cv::Mat tplEdgeBinary;         // 0/255 sau Canny/dilate/length-filter
```

Đồng bộ reset các field trên trong `s_TemplData::clear()`.

Trong `s_StableScaleTemplate`, giữ `tplNorm`, `tplGrad`, `tplEdge` hiện có **không đổi** (không phá flow validator cũ), nhưng thêm thêm `tplEdgeMagnitude` (32F hoặc 8U) để validator mới dùng.

### 2.5. Helper mới trong anonymous namespace Pattern2.cpp

Thêm các hàm tĩnh (đặt cạnh `NormalizeForCompare`, `GradientMag8`, `EdgeMaskFromGrad`):

```cpp
static cv::Mat ApplyGrayPipeline(const cv::Mat& src, const Pattern2PreprocessConfig& cfg);
static cv::Mat ComputeEdgeMagnitude(const cv::Mat& grayPre, const Pattern2PreprocessConfig& cfg);
static cv::Mat ComputeEdgeBinary(const cv::Mat& edgeMag, const Pattern2PreprocessConfig& cfg);
static cv::Mat FilterEdgeByLength(const cv::Mat& edgeBin, int minLen);
static cv::Mat ApplyFullPreprocess(const cv::Mat& src,
                                   const Pattern2PreprocessConfig& cfg,
                                   cv::Mat* outGrayPre,
                                   cv::Mat* outEdgeMag,
                                   cv::Mat* outEdgeBin);
```

Yêu cầu chi tiết:

- **`ApplyGrayPipeline`** làm đúng thứ tự: `ToGray8U` → BitwiseNot (nếu bật) → IlluminationFix (subtract large-blur, offset 128) → Denoise → CLAHE → Gamma (qua LUT). `IllumKernel` auto = `(min(w,h)/8) | 1` (phải lẻ).
- **`ComputeEdgeMagnitude`** trả về CV_8U 0..255:
  - `SobelMag`/`ScharrMag`: `magnitude(gx,gy)` rồi chuẩn hoá theo `maxv`.
  - `Canny` với `CannyLow=0,High=0` → tự tính: `v = median(gray)`, low=`max(0,0.66*v)`, high=`min(255,1.33*v)`; rồi optionally `distanceTransform` đảo để có magnitude mềm — **đơn giản**: dùng Otsu trên gradient magnitude lấy ngưỡng, Canny ra binary, rồi `distanceTransform(255-bin, DIST_L2, 3)` clipped [0..255] → cho giá trị mềm (NCC ổn định hơn binary).
  - `Laplacian`: `|Laplacian(gray, CV_32F, EdgeKernel)|` → normalize 0..255.
- **`ComputeEdgeBinary`**: nếu `EdgeKeepMagnitude=false`, Otsu hoá magnitude; sau đó `dilate` theo `EdgeDilatePx`; sau đó `FilterEdgeByLength` nếu `EnableEdgeLengthFilter`.
- **`FilterEdgeByLength`**: `connectedComponentsWithStats` trên binary, bỏ thành phần có `CC_STAT_AREA < minLen`.
- **`ApplyFullPreprocess`** là entry point: gọi lần lượt ApplyGrayPipeline → ComputeEdgeMagnitude → ComputeEdgeBinary. Trả ra qua pointer out để cache lại.

### 2.6. Sửa `LearnPatternStable(Pattern2StableConfig cfg)`

Trước khi build stable scale bank:

1. Gọi `ApplyFullPreprocess(originalGray_from_matSample, cfg.Preprocess, &tplGrayPre, &tplEdgeMag, &tplEdgeBin)`.
2. **Snapshot `cfg.Preprocess` vào `img->m_TemplData.pp...` fields** để Match() replay đúng.
3. Quyết định ảnh vào pyramid theo Domain:
   - `Gray` → `tplGrayPre`.
   - `Edge` → `EdgeKeepMagnitude ? tplEdgeMag : tplEdgeBin`.
   - `GrayPlusEdge` → `tplGrayPre` (coarse); cache edge để validator dùng.
4. Nếu `cfg.Preprocess.AutoPickDomain == true`: tính `edgeDensity`, `entropy` giống code có sẵn, override `Domain` theo quy tắc:
   - `edgeDensity >= 0.15 && entropy >= 6.5` → `GrayPlusEdge`.
   - `edgeDensity < 0.05` → `Gray`.
   - còn lại → `GrayPlusEdge`.
5. Vòng lặp scale hiện tại (resize template theo `scale`) phải **tính lại edge sau khi resize** (không được resize trực tiếp edge map), để edge thickness nhất quán.
6. Lưu `tplGrayPre`, `tplEdgeMag`, `tplEdgeBin` vào `s_StableScaleTemplate` (mở rộng struct này: thêm `tplEdgeMag`, giữ `tplEdge` cũ = `tplEdgeBin` cho tương thích).

### 2.7. Sửa `Match(...)` và `MatchStable(...)`

**`Match()` (legacy)**: thêm nhánh — nếu `img->m_TemplData.ppDomain != 0` (tức template đã learn với Domain ≠ Gray), áp `ApplyFullPreprocess` lên `img->matRaw` đọc từ `m_TemplData.pp*`, rồi chọn ảnh source theo Domain như template. Nếu `ppDomain == 0`, giữ nguyên hành vi hiện tại (chỉ BitwiseNot) — zero regression.

**`MatchStable(cfg)`**:
- Áp `ApplyFullPreprocess(matRaw, cfg.Preprocess, &srcGrayPre, &srcEdgeMag, &srcEdgeBin)`.
- Ảnh dùng cho coarse `Match(...)` call (dòng ~1914 hiện tại): chọn theo Domain.
- Trong validator (dòng ~1971), nếu `Domain == GrayPlusEdge`:
  - Tính thêm `rawNccEdge = ScoreSameSizeNCC(tplEdgeMag, roiEdgeMag)` (lấy ROI từ `srcEdgeMag` bằng cùng `GetRotatedROI` với góc/size đã có).
  - `finalScore` mới:
    ```
    wGrayBlock  = wBase*coarseScore + wRaw*rawPos
    wEdgeBlock  = wGrad*gradPos + wEdgeIou*edgeIoU + wEdgeRatio*edgeRatio + wRawEdge*rawNccEdgePos
    finalScore  = Clamp01( FuseGrayWeight * wGrayBlock + (1-FuseGrayWeight) * wEdgeBlock )
    ```
  - `wRawEdge` = 0.15 default, cộng vào normalization.
- Nếu `Domain == Edge`, coarse đã chạy trên edge → không cần thêm rawNcc gray; finalScore giữ công thức cũ.
- Nếu `Domain == Gray`, behavior y như hiện tại.

### 2.8. API mới cho C#

Trong `ref class Pattern2` thêm:

```cpp
// Preview ảnh sau tiền xử lý (debug/tune GUI)
System::IntPtr PreviewPreprocessed(
    IntPtr data, int w, int h, int stride, int ch,
    Pattern2PreprocessConfig cfg,
    int outputKind,   // 0=GrayPre, 1=EdgeMag, 2=EdgeBin
    [Out] int% outW, [Out] int% outH, [Out] int% outStride, [Out] int% outChannels);
```

Buffer trả về cấp bằng `Marshal::AllocHGlobal`, user gọi `FreeBuffer` để giải phóng (pattern đã có sẵn trong `SetImgeSample`).

### 2.9. Factory preset (static methods trong ref class Pattern2 hoặc helper)

Mỗi preset là một hàm tạo `Pattern2PreprocessConfig`:

```cpp
static Pattern2PreprocessConfig PresetGeneralGray();
static Pattern2PreprocessConfig PresetUnevenLighting();
static Pattern2PreprocessConfig PresetMetalShiny();
static Pattern2PreprocessConfig PresetPCBOrText();
static Pattern2PreprocessConfig PresetLowContrast();
```

Giá trị theo khuyến nghị:

- **GeneralGray**: CLAHE on (clip 2.0, tile 8), Domain=Gray.
- **UnevenLighting**: IlluminationFix on, CLAHE on (clip 3.0), Domain=Gray.
- **MetalShiny**: CLAHE + Bilateral denoise (k=5), Domain=GrayPlusEdge, FuseGrayWeight=0.5.
- **PCBOrText**: CLAHE + Canny auto + EdgeDilatePx=1 + EdgeLengthFilter on (min=8), Domain=Edge, EdgeKeepMagnitude=true.
- **LowContrast**: Gamma=0.7 + CLAHE clip=4, Domain=GrayPlusEdge, FuseGrayWeight=0.4.

---

## 3. Ràng buộc kỹ thuật

1. **Zero-regression**: nếu người dùng không set `cfg.Preprocess` (Domain=Gray, mọi flag=false), kết quả `Match`/`MatchStable` phải bit-exact so với hiện tại trên cùng input. Thêm unit-ish test thủ công: so sánh score của ảnh bất kỳ trước/sau thay đổi khi `Preprocess=default`, sai khác tuyệt đối ≤ 1e-4.
2. **Thread safety**: `MatchStable` vẫn đang tráo `img->matSample` và `img->m_TemplData` trong loop scale. Khi thêm edge cache, **đảm bảo restore đầy đủ** ở cuối hàm (pattern try/finally tương đương — dùng RAII struct `ScopedTemplSwap`).
3. **Pyramid của edge**: tuyệt đối KHÔNG `pyrDown` trên binary edge. Nếu Domain=Edge, pyramid phải build từ `edgeMagnitude` (giá trị liên tục) và binary hoá ở layer 0 khi cần cho IoU.
4. **BitwiseNot + Edge**: khi `Domain != Gray`, bỏ qua `EnableBitwiseNot` (gradient magnitude đã bất biến với đảo cực) — log cảnh báo vào `DebugLog` nếu `cfg.DebugLog`.
5. **OpenCV API**: dùng `cv::createCLAHE`, `cv::bilateralFilter`, `cv::Canny`, `cv::Sobel`, `cv::Scharr`, `cv::Laplacian`, `cv::distanceTransform`, `cv::connectedComponentsWithStats`. Không dùng CUDA.
6. **Code style**: giữ nguyên style hiện tại (brace cùng dòng, `cv::` prefix đầy đủ, comment tiếng Việt xen tiếng Anh OK). Không đổi tên symbol có sẵn.
7. **DebugLog**: khi `cfg.DebugLog == true`, log thêm dòng `"preprocess: domain=..., illum=..., clahe=..., denoise=..., edgeMethod=..., autoPickedDomain=..."` ngay sau header `MatchStable`.

---

## 4. Tiêu chí hoàn thành (acceptance)

- Build release x64 pass, không warning mới mức ≥ C4.
- Test case 1 — ảnh PCB có chữ & via: `PresetPCBOrText` cho số false positive ≤ 50% so với Domain=Gray mặc định, với cùng `MinAcceptScore=0.7`.
- Test case 2 — ảnh vignette nặng: `PresetUnevenLighting` tăng recall so với Gray baseline (ít nhất tìm thêm được 1 vị trí đúng mà baseline miss ở score 0.75).
- Test case 3 — preset mặc định (`Preprocess(true)`) trên ảnh bất kỳ: bit-exact với hành vi cũ (đã nêu ở mục 3.1).
- `PreviewPreprocessed` trả về ảnh đúng kích thước `outW/outH/outStride/outChannels=1` cho cả 3 `outputKind`.
- Xoá `PatternMatchOptions` không gây lỗi link/compile ở bất cứ file nào trong solution (grep toàn repo xác nhận).

---

## 5. Thứ tự commit gợi ý

1. `refactor(pattern2): remove unused PatternMatchOptions struct`
2. `feat(pattern2): add Pattern2PreprocessConfig skeleton + preset factories`
3. `feat(pattern2): implement ApplyGrayPipeline + ComputeEdgeMagnitude helpers`
4. `feat(pattern2): wire preprocess into LearnPatternStable with snapshot into s_TemplData`
5. `feat(pattern2): apply preprocess in Match/MatchStable, add edge-NCC to validator for GrayPlusEdge`
6. `feat(pattern2): add PreviewPreprocessed API + AutoPickDomain`
7. `test(pattern2): smoke tests cho 3 case acceptance`

---

## 6. Câu hỏi có thể phát sinh

Nếu gặp ambiguity sau đây thì **chọn option bên phải** làm mặc định, ghi chú TODO trong code:

- Pyramid cho edge: chọn **build từ gradient magnitude** (không build từ binary).
- `CannyLow/High = 0`: dùng **median-based auto thresholds** (Otsu nếu median fail).
- Fuse formula khi `wRawEdge` chưa có weight riêng: **trừ đều từ các weight còn lại và chia lại để tổng = 1**.
- Mask xử lý sau preprocess hay trước? → **sau** (mask áp lên kết quả preprocess cuối cùng).

---

**Khi hoàn thành, trả về:**

- Diff của `Pattern2.h` và `Pattern2.cpp`.
- Danh sách file khác bị ảnh hưởng (nếu có — có khả năng C# wrapper gọi `Match`/`LearnPatternStable` cần cập nhật ctor default).
- Kết quả chạy 3 test case mục 4 (ảnh test do người dùng cung cấp — nếu không có, mock bằng ảnh `matSample` đã gộp noise + vignette tổng hợp).
