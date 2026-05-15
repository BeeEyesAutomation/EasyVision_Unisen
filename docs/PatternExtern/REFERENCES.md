# References — Code snapshots

Snapshot các đoạn code critical trong codebase hiện tại (2026-05-14). Mục đích: future-agent đọc plan có context tra cứu ngay, không cần grep/read lại file lớn.

## 1. `Pattern/Pattern2.h`

### 1.1 `s_TemplData` (line 40-152) — pyramid + preprocess snapshot

Container cốt lõi cho 1 template đã học. Cần copy nguyên struct vào `s_BatchEntry::templData`.

```cpp
struct s_TemplData
{
    vector<Mat> vecPyramid;
    vector<cv::UMat> vecPyramidGpu;
    vector<Scalar> vecTemplMean;
    vector<double> vecTemplNorm;
    vector<double> vecInvArea;
    vector<bool> vecResultEqual1;
    bool bIsPatternLearned;
    int iBorderColor;
    // Template statistics for validation
    int    modelEdgeCount = 0;
    double modelEntropy = 0;
    double modelEdgeDensity = 0;
    double modelEntropyNorm = 0;
    // Auto thresholds/weights
    double autoStrongBaseScore = 0.90;
    double autoSoftEdgeIou = 0.08;
    double autoSoftEdgeRatio = 0.75;
    double autoHardEdgeIou = 0.10;
    double autoHardEdgeRatio = 0.80;
    double autoMinFused = 0.55;
    double autoWBase = 0.70;
    double autoWRaw = 0.10;
    double autoWGrad = 0.05;
    double autoWEdgeIou = 0.10;
    double autoWEdgeRatio = 0.05;
    // Preprocess snapshot — fields below là input cho PreprocessSnapshotEqual()
    bool   ppEnableBitwiseNot = false;
    bool   ppEnableIlluminationFix = false;
    int    ppIllumKernel = 0;
    bool   ppEnableCLAHE = false;
    double ppClaheClip = 2.0;
    int    ppClaheTile = 8;
    bool   ppEnableGamma = false;
    double ppGamma = 1.0;
    int    ppDenoiseMethod = 0;
    int    ppDenoiseKernel = 3;
    int    ppDomain = 0;
    int    ppEdgeMethod = 2;
    int    ppEdgeKernel = 3;
    double ppCannyLow = 0.0;
    double ppCannyHigh = 0.0;
    bool   ppEdgeKeepMagnitude = true;
    int    ppEdgeDilatePx = 1;
    bool   ppEnableEdgeLengthFilter = false;
    int    ppMinEdgeSegmentLen = 6;
    double ppFuseGrayWeight = 0.5;
    cv::Mat tplPreprocessedGray;
    cv::Mat tplEdgeMagnitude;
    cv::Mat tplEdgeBinary;
    void clear();
    void resize(int iSize);
    s_TemplData();
};
```

### 1.2 `Img` class (line 369-403) — runtime container

```cpp
class Img
{
public:
    cv::Mat     matRaw;       // source image cần match
    cv::Mat     matSample;    // template grayscale level-0
    s_TemplData m_TemplData;  // pyramid + preprocess + auto-threshold
    std::vector<s_StableScaleTemplate> stableScaleBank;
    int         m_iMinReduceArea = 256;
    bool        m_EnableGpu = false;
    std::recursive_mutex stateMutex;
    void ClearStableScaleBank();
    void BuildTemplatePyramid();
    int  GetTopLayer(...);
    void MatchTemplate(...);
    void MatchTemplateGpu(...);
    void GetRotatedROI(...);
    void CCOEFF_Denominator(...);
    cv::Size  GetBestRotationSize(...);
    Point2f   ptRotatePt2f(...);
    void FilterWithScore(...);
    void FilterWithRotatedRect(...);
    cv::Point GetNextMaxLoc(...);
    void SortPtWithCenter(...);
    bool SubPixEsimation(...);
};
```

**Plan thêm sau dòng 375 (sau `stableScaleBank`)**:
```cpp
std::vector<s_BatchEntry> batchEntries;
bool                      batchActive = false;
```

### 1.3 Ref class `Pattern2` (line 561-613) — CLI API

Public method hiện có (giữ 100% backward-compat):
```cpp
public ref class Pattern2
{
public:
    Pattern2(); ~Pattern2(); !Pattern2();
    void SetRawNoCrop(IntPtr data, int w, int h, int stride, int ch);
    void SetImgeRaw(IntPtr, ..., RectRotateCli, Nullable<RectRotateCli>);
    void SetImgeSampleNoCrop(IntPtr, int, int, int, int);
    IntPtr SetImgeSample(...);
    void LearnPattern();
    void LearnPatternStable();
    void LearnPatternStable(Pattern2StableConfig cfg);
    void FreeBuffer(IntPtr p);
    void SetGpuEnabled(bool);
    static bool IsGpuAvailable();
    IntPtr PreviewPreprocessed(...);
    static Pattern2PreprocessConfig PresetGeneralGray();
    static Pattern2PreprocessConfig PresetUnevenLighting();
    static Pattern2PreprocessConfig PresetMetalShiny();
    static Pattern2PreprocessConfig PresetPCBOrText();
    static Pattern2PreprocessConfig PresetLowContrast();
    List<Rotaterectangle>^ Match(...);
    List<Rotaterectangle>^ MatchStable(Pattern2StableConfig cfg);
};
```

**Plan thêm 4 method mới sau dòng 608**:
```cpp
void LearnPatternBatchBegin();
int  AddBatchTemplate(IntPtr, int, int, int, int,
                       Pattern2StableConfig, Pattern2BatchTemplateConfig);
void LearnPatternBatchEnd();
List<Pattern2BatchResult>^ MatchBatchStable(Pattern2StableConfig cfg);
```

## 2. `Pattern/Pattern2.cpp` — helper functions đã có

Grep map (đường dẫn — line):
| Symbol | Line | Vai trò |
|---|---|---|
| `ApplyFullPreprocess` | 1083 | Áp preprocess pipeline lên 1 ảnh (gray/edge/denoise/CLAHE/gamma) |
| `SnapshotPreprocess` | 1103 | Lưu `Pattern2PreprocessConfig` vào `s_TemplData.pp*` fields |
| `LoadPreprocess` | 1127 | Đọc `pp*` fields ra `Pattern2PreprocessConfig` |
| `DisablePreprocessReplay` | 1164 | Set `pp*` fields về "no-op" để Match() không re-preprocess source |
| `ScopedTemplSwap` | 1276 | RAII guard: save+restore `img->matRaw/matSample/m_TemplData/m_EnableGpu` |
| `LearnPatternStable(cfg)` | 1991 | Học template + build pyramid + snapshot preprocess |
| `Match()` | 2273 | Coarse hierarchical match (legacy single-stage) |
| `MatchStable()` | 2694 | Multi-scale + validator + NMS (plan refactor) |

**Anchor cho refactor `MatchStable`**:
- Lines 2696-2718: setup + preprocess source — extract thành `BuildSourceFeatures()`.
- Lines 2719-2787: debug logging — giữ trong cả 2 path (single + batch).
- Lines 2789-end: scale bank loop + filter + return — extract thành `MatchStableOnPreprocessedSource(cfg, SourceFeatures&)`.

`RotatedIoU` chưa tồn tại trong Pattern2.cpp (grep không thấy). Plan: thêm helper file-scope dùng `cv::rotatedRectangleIntersection` + `cv::contourArea`:

```cpp
static double RotatedIoU(const Rotaterectangle& a, const Rotaterectangle& b)
{
    cv::RotatedRect ra(cv::Point2f((float)a.Cx, (float)a.Cy),
                       cv::Size2f((float)a.Width, (float)a.Height),
                       (float)a.AngleDeg);
    cv::RotatedRect rb(cv::Point2f((float)b.Cx, (float)b.Cy),
                       cv::Size2f((float)b.Width, (float)b.Height),
                       (float)b.AngleDeg);
    std::vector<cv::Point2f> inter;
    int code = cv::rotatedRectangleIntersection(ra, rb, inter);
    if (inter.empty()) return 0.0;
    double interArea = cv::contourArea(inter);
    double unionArea = ra.size.area() + rb.size.area() - interArea;
    return (unionArea > 1e-9) ? interArea / unionArea : 0.0;
}
```

## 3. `BeeCore/Unit/Patterns.cs` — call sites

| Symbol | Line | Vai trò |
|---|---|---|
| `Pattern` field | 432 | `BeeCpp.Pattern2` instance (NonSerialized) |
| `bmRaw` | 241 | Bitmap template hiện tại (single-mode) |
| `pathRaw` | 245 | Path template file |
| `ResultItems` | 444 | List kết quả (Plan ghi label vào `.Name`) |
| `LearnPattern()` | 458 | Plan branch `IsMultiTemplate` → `LearnPatternsBatch()` |
| `ClearResultItems()` | 737 | Reset |
| `AddMatchResult(area, local, score)` | 891 | Tạo ResultItem + push vào ResultItems |
| `DifficultyPattern` ... `ScalePattern` fields | 1612-1621 | Per-tool config |
| `DoWork(rotArea, rotMask)` | 1623 | Entry point match — Plan branch `IsMultiTemplate` |
| Pattern2StableConfig build | 1707-1733 | Plan extract thành `BuildStableConfig()` |
| `MatchStable(cfg)` call | 1735 | Plan branch: nếu multi → `MatchBatchStable(cfg)` |

## 4. `BeeCore/Func/ResultItem.cs` — toàn file (line 1-41)

```csharp
public class ResultItem
{
    public ResultItem(String Name) { this.Name = Name; }
    public string Name { get; set; }       // ← Plan dùng làm label
    public RectRotate rot { get; set; }
    public float Score { get; set; }
    public float Percent { get; set; }
    public float Area { get; set; }
    public int ValueColor { get; set; }
    public Rectangle ColorMarkRect { get; set; } = Rectangle.Empty;
    public OpenCvSharp.Point[] ColorMarkContour { get; set; }
    public int IndexScanBox { get; set; }
    public float Distance { get; set; }
    public PointF point { get; set; }
    [NonSerialized] public bool IsArea = false;
    public bool IsOK { get; set; }
    public Mat matProcess = null;
    public float PercentColor = 0;
    [NonSerialized] public Mat ColorDebugOverlay = null;
}
```

**Quyết định**: KHÔNG thêm field `Label` mới. Dùng `Name` (đã có) để tránh đụng serialization backward-compat.

## 5. `BeeCore/Unit/MultiPattern.cs` — reference design

Snippet match loop hiện có (line 1675-1696) — Plan KHÔNG copy approach này nhưng giữ làm fallback:

```csharp
list_Patterns[i].SetRawNoCrop(ptr, w0, h0, step, ch);
var rot = list_Patterns[i].Match(
    IsHighSpeed, 0, AngleLower, AngleUper,
    Common.TryGetTool(IndexThread, Index).Score / 100.0,
    ckSIMD, ckBitwiseNot, ckSubPixel,
    1, OverLap, false, -1);
```

- `list_Patterns` (line 337): `List<BeeCpp.Pattern2>` — 1 instance/template.
- `ResultMulti` (line 337): per-template result struct.
- `labelItems` (line 461): từ YOLO model — KHÔNG dùng cho Pattern2 plan.
- `Parallel.For` rebuild trên load (line 506-642): tham khảo cho lazy bitmap decode.
- `ValueCompare` judge (line 1846): tham khảo cho ProcessBatchResults logic.
