# Plan (Expanded) — Mở rộng Pattern2 hỗ trợ Multi-Template với Label

> Path correction: project native là **`Pattern/`** (output assembly `BeeCpp.dll`), không phải `BeeCpp/`. File chính: `Pattern/Pattern2.h`, `Pattern/Pattern2.cpp`.

---

## 1. Context

`Pattern/Pattern2.{h,cpp}` + C# `BeeCore/Unit/Patterns.cs` hiện single-template:
- `ref class Pattern2` (Pattern2.h:561) giữ 1 `Img*` (line 612), `Img` có 1 `matSample` + 1 `m_TemplData` + 1 `stableScaleBank` (Pattern2.h:373-375).
- `MatchStable(cfg)` (Pattern2.cpp:2694): preprocess source 1 lần → loop `stableScaleBank` (đa scale CỦA CÙNG 1 template, không phải đa template) → coarse Match + fine validator → NMS → return `List<Rotaterectangle>`.
- C# `Patterns.Pattern` (Patterns.cs:432) là 1 `BeeCpp.Pattern2` duy nhất; `DoWork()` (Patterns.cs:1623) gọi `MatchStable()` ở line 1735, kết quả là 1 list không nhãn.

User cần: 1 ToolPattern2, bật flag "Multi-Template" → cho phép add nhiều template kèm label string + expected count + score threshold riêng; 1 lần Run preprocess source + build pyramid **1 lần duy nhất**, scan tất cả template, trả về `{label, position, score, templateIndex}` toàn cục NMS, tính OK/NG dựa trên expected count per label.

Reference: `BeeCore/Unit/MultiPattern.cs:1675-1696` (loop per-template ở C#) — plan **không** copy approach này; thay vào đó **mở rộng native** để chia sẻ preprocess + source pyramid (chính là 80% chi phí của 1 lần match khi template nhỏ).

Outcome cụ thể:
- Backward-compat 100% với project cũ (single mode mặc định OFF).
- Native API mới `MatchBatchStable` + 3 helper.
- C# expose `IsMultiTemplate`, `MultiTemplates : List<Pattern2TemplateEntry>` serialize được vào `ClassProject.json`.
- UI: section collapsible mới trong ToolPattern2 với DataGridView quản lý template list.

---

## 2. Approach & Trade-offs

**Đã chọn: Native batch + Shared config + Per-label required-count.**

Lý do native batch (đã hỏi user xong, ghi lại để future-agent hiểu):
- Build source pyramid là phần đắt nhất khi multi-template (template nhỏ, source lớn).
- Preprocess (CLAHE / Sobel / Canny) chạy 1 lần cho source thay vì N lần.
- Trade-off: chấp nhận tăng kích thước `Img` struct (vector entries) + thêm code path trong Pattern2.cpp. Single-template path không bị ảnh hưởng (struct cũ vẫn dùng).

Per-label required-count:
- Mỗi template entry có `ExpectedCount` (mặc định 1).
- OK ↔ với mọi label trong list: số match có score ≥ threshold == ExpectedCount (chính xác hoặc ≥ tuỳ chọn — plan chọn ≥ để dễ dùng; nếu cần == thì tương lai thêm flag).

---

## 3. Native — Pattern/Pattern2.h

### 3.1 Thêm struct quản lý 1 template entry (internal, native side, sau dòng 376)

Tại `Pattern/Pattern2.h` sau khai báo class `Img`:

```cpp
struct s_BatchEntry
{
    cv::Mat                              matSample;     // template grayscale, level-0
    s_TemplData                          templData;     // pyramid + auto-threshold + preprocess snapshot
    std::vector<s_StableScaleTemplate>   scaleBank;     // multi-scale variants for this template
    std::string                          label;         // UTF-8 label
    double                               minAcceptScore;// per-template threshold (0..1)
    int                                  expectedCount; // ≥0; 0 = optional (anything goes)
    int                                  maxPerTemplate;// upper cap per template; 0 => use cfg.MaxPos
};
```

Mở rộng class `Img` (Pattern2.h:369-403), thêm field sau `stableScaleBank`:

```cpp
std::vector<s_BatchEntry> batchEntries;
bool                      batchActive = false;   // true sau LearnPatternBatchEnd, false sau khi single-API được dùng
```

### 3.2 Thêm CLI value struct cho batch I/O (sau Pattern2StableConfig, trước ref class)

```cpp
public value struct Pattern2BatchTemplateConfig
{
    System::String^ Label;
    double          MinAcceptScore;  // 0..1; <=0 => fallback cfg.MinAcceptScore
    int             ExpectedCount;   // ≥0
    int             MaxPerTemplate;  // 0 => cfg.MaxPos
    Pattern2BatchTemplateConfig(System::String^ label, double score, int expected)
    {
        Label = label;
        MinAcceptScore = score;
        ExpectedCount = expected;
        MaxPerTemplate = 0;
    }
};

public value struct Pattern2BatchResult
{
    int              TemplateIndex;  // index trong batchEntries
    System::String^  Label;
    double           Cx, Cy, AngleDeg, Width, Height, Score; // Score in 0..100 (×100 như Rotaterectangle)
};
```

### 3.3 Thêm 4 method trên `ref class Pattern2` (sau dòng 608)

```cpp
void LearnPatternBatchBegin();
int  AddBatchTemplate(
        System::IntPtr data, int w, int h, int stride, int ch,
        Pattern2StableConfig learnCfg,         // dùng preprocess + scale params; mỗi template học riêng
        Pattern2BatchTemplateConfig tplCfg);   // label + threshold + expected
void LearnPatternBatchEnd();
List<Pattern2BatchResult>^ MatchBatchStable(Pattern2StableConfig cfg);
```

### 3.4 Backward-compat

- Không sửa signature method cũ.
- Single-template caller cứ dùng `SetImgeSample/SetImgeSampleNoCrop` + `LearnPatternStable` + `MatchStable`. Khi `batchActive == false`, single path không thấy `batchEntries`.
- Khi caller gọi `LearnPatternBatchBegin()` thì set `batchActive = true` và **clear** single fields (`matSample.release()`, `stableScaleBank.clear()`, `m_TemplData = {}`). Khi gọi single API trở lại thì `batchActive = false` + clear `batchEntries`. Switch mode rõ ràng để tránh state lai.

---

## 4. Native — Pattern/Pattern2.cpp

### 4.1 Refactor `MatchStable` — tách helper

Tại Pattern2.cpp:2715-2787 (preprocess source + build srcFeature + debug logging) là phần dùng chung. Tách thành helper static-file-scope:

```cpp
struct SourceFeatures {
    cv::Mat feature;       // dùng cho coarse matchTemplate
    cv::Mat grayPre;       // dùng validator
    cv::Mat edgeMag;
    cv::Mat edgeBin;
    Pattern2PreprocessConfig learned;
};

// Đọc preprocess snapshot từ template, áp lên source. Trả empty feature nếu fail.
static bool BuildSourceFeatures(
    const cv::Mat& matRaw,
    const s_TemplData& templ,            // để LoadPreprocess
    SourceFeatures& out,
    StringBuilder^ debugSb /*nullable*/);
```

**Quan trọng**: hiện tại `MatchStable` đang `LoadPreprocess(img->m_TemplData)` (line 2715). Trong batch, từng template có thể học preprocess khác nhau (Auto-threshold). Quyết định:

- **Phương án A (đơn giản)**: dùng preprocess snapshot của **template đầu tiên** trong batch áp cho source. Tất cả template phải học cùng preset (UI đã ép shared config nên đảm bảo). Trade-off: nếu user lỡ học khác preset thì kết quả sai — nhưng UI sẽ chặn (chỉ có 1 preset chung).
- **Phương án B (đầy đủ)**: nếu các template có preprocess khác nhau → group entries theo preprocess hash, preprocess source mỗi group 1 lần. Phức tạp.

**Plan dùng A**. Ép contract: batch mode = shared preprocess. Validate ở `LearnPatternBatchEnd()`: nếu thấy `templData.preprocessSnapshot` của entry i khác entry 0 → throw `System::InvalidOperationException("All batch templates must share the same preprocess config")`. Compare bằng memcmp/field-by-field của `s_TemplData.preprocessSnapshot` (Pattern2.h:70-89).

### 4.2 Implement `LearnPatternBatchBegin/End` (sau implementation của `LearnPatternStable`, Pattern2.cpp ~line 2130)

```cpp
void Pattern2::LearnPatternBatchBegin()
{
    std::lock_guard<std::recursive_mutex> g(img->stateMutex);
    img->batchEntries.clear();
    img->batchActive = true;
    // Clear single-template state
    img->matSample.release();
    img->stableScaleBank.clear();
    img->m_TemplData = s_TemplData();
}

int Pattern2::AddBatchTemplate(IntPtr data, int w, int h, int stride, int ch,
                                Pattern2StableConfig learnCfg,
                                Pattern2BatchTemplateConfig tplCfg)
{
    std::lock_guard<std::recursive_mutex> g(img->stateMutex);
    if (!img->batchActive) throw gcnew InvalidOperationException("Call LearnPatternBatchBegin first");

    // 1) Wrap input ptr -> cv::Mat (giống SetImgeSampleNoCrop, Pattern2.cpp ~line 648)
    cv::Mat src(h, w, MakeMatType(ch), (void*)data.ToPointer(), stride);
    cv::Mat gray;
    if (ch == 1) gray = src.clone();
    else cv::cvtColor(src, gray, ch == 4 ? cv::COLOR_BGRA2GRAY : cv::COLOR_BGR2GRAY);

    // 2) Tạm thời swap vào img->matSample + chạy LearnPatternStable logic
    //    Hiện tại LearnPatternStable() viết trên img->matSample → img->m_TemplData + img->stableScaleBank.
    //    Cách tái sử dụng: copy gray -> img->matSample, gọi LearnPatternStable(learnCfg), sau đó
    //    move ra batchEntries.
    img->matSample = gray;
    img->stableScaleBank.clear();
    img->m_TemplData = s_TemplData();
    LearnPatternStable(learnCfg);   // hàm này đã có, line 1991

    // 3) Push entry
    s_BatchEntry e;
    e.matSample = std::move(img->matSample);
    e.templData = std::move(img->m_TemplData);
    e.scaleBank = std::move(img->stableScaleBank);
    e.label = SystemStringToUtf8(tplCfg.Label);
    e.minAcceptScore = (tplCfg.MinAcceptScore > 0.0) ? tplCfg.MinAcceptScore : 0.0;
    e.expectedCount = std::max(0, tplCfg.ExpectedCount);
    e.maxPerTemplate = std::max(0, tplCfg.MaxPerTemplate);
    img->batchEntries.push_back(std::move(e));

    // Reset single fields (đã move ra)
    img->matSample.release();
    img->stableScaleBank.clear();
    img->m_TemplData = s_TemplData();
    return (int)img->batchEntries.size() - 1;
}

void Pattern2::LearnPatternBatchEnd()
{
    std::lock_guard<std::recursive_mutex> g(img->stateMutex);
    if (img->batchEntries.empty())
        throw gcnew InvalidOperationException("Batch is empty");

    // Validate shared preprocess (Phương án A)
    const auto& ref = img->batchEntries[0].templData;
    for (size_t i = 1; i < img->batchEntries.size(); ++i)
    {
        if (!PreprocessSnapshotEqual(img->batchEntries[i].templData, ref))
            throw gcnew InvalidOperationException(
                "Batch template " + i + " has different preprocess snapshot than template 0");
    }
}
```

Trong đó cần thêm 2 helper file-scope:
- `std::string SystemStringToUtf8(System::String^ s)` — dùng `Marshal::StringToHGlobalAnsi` hoặc UTF-8 nếu label có Unicode.
- `bool PreprocessSnapshotEqual(const s_TemplData& a, const s_TemplData& b)` — so sánh các field `ppEnable*`, `ppDomain`, `ppEdgeMethod`, `ppCannyLow/High`, `ppFuseGrayWeight`, `ppDenoiseMethod`, etc. (xem Pattern2.h:70-89).

### 4.3 Implement `MatchBatchStable`

```cpp
List<Pattern2BatchResult>^ Pattern2::MatchBatchStable(Pattern2StableConfig cfg)
{
    std::lock_guard<std::recursive_mutex> g(img->stateMutex);
    auto results = gcnew List<Pattern2BatchResult>();
    if (img == nullptr || img->matRaw.empty()) return results;
    if (!img->batchActive || img->batchEntries.empty()) return results;

    const int maxPosGlobal = std::max(1, std::min(cfg.MaxPos, 256));
    const double maxOverlap = std::max(0.0, std::min(cfg.MaxOverlap, 0.90));
    img->m_EnableGpu = cfg.EnableGpu && CanUseGpu(true);

    // 1) Preprocess source 1 lần (dùng preprocess snapshot của entry 0)
    SourceFeatures sf;
    if (!BuildSourceFeatures(img->matRaw, img->batchEntries[0].templData, sf, /*debug*/nullptr))
        return results;

    // 2) Loop từng entry; reuse phần coarse+fine của MatchStable hiện tại.
    //    Cách đơn giản nhất: swap entry vào single fields, gọi 1 helper InternalMatchStableOnPreprocessed
    //    (refactor từ MatchStable lines 2789-end thành function nhận sẵn srcFeature/srcGrayPre/...).
    struct LabeledMatch {
        int templateIndex;
        std::string label;
        Rotaterectangle rect;
    };
    std::vector<LabeledMatch> all;
    all.reserve(maxPosGlobal * img->batchEntries.size());

    for (int idx = 0; idx < (int)img->batchEntries.size(); ++idx)
    {
        auto& e = img->batchEntries[idx];
        // Swap entry vào single fields tạm thời (giữ pattern ScopedTemplSwap đã có ở line 2709)
        ScopedTemplSwap restore(img);
        img->matSample = e.matSample;
        img->m_TemplData = e.templData;
        img->stableScaleBank = e.scaleBank;

        Pattern2StableConfig perCfg = cfg;
        perCfg.MinAcceptScore = (e.minAcceptScore > 0.0) ? e.minAcceptScore : cfg.MinAcceptScore;
        perCfg.MaxPos = (e.maxPerTemplate > 0) ? e.maxPerTemplate : maxPosGlobal;

        // Gọi helper đã refactor (nhận sẵn srcFeature)
        auto perResults = MatchStableOnPreprocessedSource(perCfg, sf);
        for each (Rotaterectangle r in perResults)
        {
            LabeledMatch lm;
            lm.templateIndex = idx;
            lm.label = e.label;
            lm.rect = r;
            all.push_back(lm);
        }
    }

    // 3) NMS toàn cục cross-template theo rotated-rect IoU + global maxPos
    //    Sort descending by Score, greedy reject if overlap > maxOverlap với candidate đã chọn.
    std::sort(all.begin(), all.end(), [](const LabeledMatch& a, const LabeledMatch& b) {
        return a.rect.Score > b.rect.Score;
    });
    std::vector<LabeledMatch> kept;
    for (auto& cand : all) {
        bool reject = false;
        for (auto& k : kept) {
            if (RotatedIoU(cand.rect, k.rect) > maxOverlap) { reject = true; break; }
        }
        if (!reject) kept.push_back(cand);
        if ((int)kept.size() >= maxPosGlobal * (int)img->batchEntries.size()) break;
    }

    // 4) Đóng gói managed
    for (auto& k : kept) {
        Pattern2BatchResult r;
        r.TemplateIndex = k.templateIndex;
        r.Label = gcnew System::String(k.label.c_str());
        r.Cx = k.rect.Cx; r.Cy = k.rect.Cy;
        r.AngleDeg = k.rect.AngleDeg;
        r.Width = k.rect.Width; r.Height = k.rect.Height;
        r.Score = k.rect.Score;
        results->Add(r);
    }
    return results;
}
```

**Helper cần extract từ `MatchStable` (Pattern2.cpp:2694-end)**:
- Tách phần `2789-end` (loop stableScaleBank + acceptedAll + filter + return) thành function `MatchStableOnPreprocessedSource(cfg, sf)` trả `List<Rotaterectangle>^`.
- `MatchStable` cũ giữ nguyên signature, gọi `BuildSourceFeatures` → `MatchStableOnPreprocessedSource(...)`.
- Cẩn thận: `ScopedTemplSwap` (line 2709) restore `matRaw/matSample/m_TemplData` cuối scope — trong batch ta swap nhiều lần nên dùng cùng helper là OK, mỗi iteration tự restore.
- `RotatedIoU` — đã có ở Pattern2.cpp? Nếu chưa, dùng `cv::rotatedRectangleIntersection` + `cv::contourArea`. Grep sẵn trước khi implement.

### 4.4 Đảm bảo single-template path không vỡ

Sau refactor `MatchStable`:
- Single mode: `batchActive == false`, `batchEntries.empty()`, `matSample`/`m_TemplData`/`stableScaleBank` đầy đủ từ `LearnPatternStable`.
- `MatchStable(cfg)` cũ gọi `BuildSourceFeatures(matRaw, m_TemplData, sf)` → `MatchStableOnPreprocessedSource(cfg, sf)`. Output **phải** bit-identical so với trước refactor.
- Test regression: chạy 1 project mẫu trước/sau refactor, so kết quả `Rotaterectangle` (Cx, Cy, Score) ≤ ε.

### 4.5 File khác cần đụng

- `Pattern/Pattern.vcxproj` — không cần đổi.
- Header forward declarations bên trong cpp — thêm `static` helpers ở đầu file.

---

## 5. C# Engine — BeeCore/Unit/Patterns.cs

### 5.1 Thêm class entry (mới, đặt cuối file hoặc trong Items/)

```csharp
[Serializable]
public class Pattern2TemplateEntry
{
    public string Label { get; set; } = "";
    public float ScoreThreshold { get; set; } = 70f;  // 0..100, UI scale
    public int ExpectedCount { get; set; } = 1;
    public int MaxPerTemplate { get; set; } = 0;       // 0 = follow global MaxObject

    // Lưu template bằng byte[] PNG để JSON-serialize được. Bitmap không serialize JSON.
    public byte[] TemplatePng { get; set; }

    [NonSerialized]
    public Bitmap _cachedBitmap;  // lazy decode khi UI cần

    public Bitmap GetBitmap()
    {
        if (_cachedBitmap != null) return _cachedBitmap;
        if (TemplatePng == null || TemplatePng.Length == 0) return null;
        using (var ms = new System.IO.MemoryStream(TemplatePng))
            _cachedBitmap = new Bitmap(ms);
        return _cachedBitmap;
    }
}
```

### 5.2 Thêm field vào `Patterns` class (Patterns.cs ~line 444 hoặc gần Pattern field)

```csharp
public bool IsMultiTemplate { get; set; } = false;
public List<Pattern2TemplateEntry> MultiTemplates { get; set; } = new List<Pattern2TemplateEntry>();
```

### 5.3 Sửa `LearnPattern` (Patterns.cs:458)

Hiện tại học từ `raw` đơn lẻ. Thêm branch:

```csharp
public Mat LearnPattern(Mat raw, bool IsNoCrop)
{
    lock (RuntimeLock)
    {
        if (IsMultiTemplate)
        {
            LearnPatternsBatch();   // method mới, học từ MultiTemplates list
            return new Mat();        // bmRaw không ý nghĩa trong multi mode
        }
        // ... code cũ giữ nguyên
    }
}

private void LearnPatternsBatch()
{
    if (MultiTemplates == null || MultiTemplates.Count == 0) return;

    var learnCfg = BuildStableConfig();  // extract phần build Pattern2StableConfig hiện ở DoWork:1707-1733
    Pattern.LearnPatternBatchBegin();
    for (int i = 0; i < MultiTemplates.Count; i++)
    {
        var e = MultiTemplates[i];
        var bmp = e.GetBitmap();
        if (bmp == null) continue;
        using (Mat m = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmp))
        {
            Mat gray = m;
            if (m.Channels() != 1) {
                gray = new Mat();
                Cv2.CvtColor(m, gray, m.Channels() == 4 ? ColorConversionCodes.BGRA2GRAY
                                                         : ColorConversionCodes.BGR2GRAY);
            }
            var tplCfg = new Pattern2BatchTemplateConfig(
                e.Label,
                e.ScoreThreshold / 100.0,
                e.ExpectedCount);
            tplCfg.MaxPerTemplate = e.MaxPerTemplate;
            Pattern.AddBatchTemplate(gray.Data, gray.Width, gray.Height,
                                     (int)gray.Step(), gray.Channels(),
                                     learnCfg, tplCfg);
            if (gray != m) gray.Dispose();
        }
    }
    Pattern.LearnPatternBatchEnd();
}

private Pattern2StableConfig BuildStableConfig()
{
    var cfg = new Pattern2StableConfig(true);
    cfg.AngleStartDeg = AngleLower;
    cfg.AngleEndDeg = AngleUper;
    cfg.AngleStepDeg = StepAngle;
    cfg.MaxOverlap = OverLap;
    cfg.BitwiseNot = ckBitwiseNot;
    cfg.SubPixel = ckSubPixel;
    cfg.Difficulty = (Pattern2DifficultyLevel)(int)DifficultyPattern;
    cfg.EnableValidator = EnableValidator;
    cfg.EnableKeepFilter = EnableKeepFilter;
    cfg.EnableNms = EnableNms;
    cfg.EnableAutoThreshold = true;
    cfg.EnableScaleSearch = EnableScaleSearch;
    cfg.ScaleMin = (100 - ScalePattern) / 100.0;
    cfg.ScaleMax = (100 + ScalePattern) / 100.0;
    cfg.ScaleStep = ScaleStep / 100.0;
    cfg.EnableGpu = UseGpu;
    cfg.EnableCpuMultiThread = EnableMultiThread;
    cfg.CpuThreads = NumThreads;
    return cfg;
}
```

Sau đó refactor `DoWork` để dùng `BuildStableConfig()` cùng — bỏ duplicate code ở 1707-1733.

### 5.4 Sửa `DoWork` (Patterns.cs:1623) — branch multi

Tại Pattern2.cpp:1707 thay thế bằng:

```csharp
Pattern2StableConfig cfg = BuildStableConfig();
cfg.MinAcceptScore = Common.TryGetTool(IndexThread, Index).Score / 100.0;
cfg.MaxPos = MaxObject;

if (IsMultiTemplate)
{
    var listRS = Pattern.MatchBatchStable(cfg);
    ProcessBatchResults(listRS, rotArea);
}
else
{
    var listRS = Pattern.MatchStable(cfg);
    // ... code cũ từ line 1738-1815 giữ nguyên
}
```

`ProcessBatchResults`:

```csharp
private void ProcessBatchResults(List<Pattern2BatchResult> listRS, RectRotate rotArea)
{
    var globalThreshold = Common.TryGetTool(IndexThread, Index).Score; // 0..100
    var perLabelCount = new Dictionary<string, int>(StringComparer.Ordinal);
    var perLabelMaxScore = new Dictionary<string, float>(StringComparer.Ordinal);
    foreach (var e in MultiTemplates)
    {
        perLabelCount[e.Label] = 0;
        perLabelMaxScore[e.Label] = 0f;
    }

    float scoreSum = 0f;
    int kept = 0;
    if (listRS != null)
    {
        foreach (var r in listRS)
        {
            if (r.AngleDeg < AngleLower || r.AngleDeg > AngleUper) continue;
            // Per-template threshold override (đã apply native rồi, ở đây check thêm global)
            if (r.Score < globalThreshold) continue;

            float w = (float)r.Width, h = (float)r.Height;
            var pCenter = new PointF((float)r.Cx, (float)r.Cy);
            float angle = (float)r.AngleDeg;
            float score = (float)r.Score;
            scoreSum += score;
            kept++;

            var localRect = new RectRotate(
                new RectangleF(-w / 2f, -h / 2f, w, h),
                pCenter, angle, AnchorPoint.None);
            var item = AddMatchResult(rotArea, localRect, score);
            if (item != null)
            {
                // Tận dụng ResultItem.Name làm label (đỡ phải sửa ResultItem struct)
                item.Name = r.Label ?? "";
            }
            if (!string.IsNullOrEmpty(r.Label))
            {
                if (perLabelCount.TryGetValue(r.Label, out int c)) perLabelCount[r.Label] = c + 1;
                if (perLabelMaxScore.TryGetValue(r.Label, out float s) && score > s)
                    perLabelMaxScore[r.Label] = score;
            }
        }
    }

    // Judge OK/NG theo expected count
    bool overallOk = true;
    foreach (var e in MultiTemplates)
    {
        if (e.ExpectedCount <= 0) continue;  // 0 = optional
        if (perLabelCount[e.Label] < e.ExpectedCount) { overallOk = false; break; }
    }

    var owner = Common.TryGetTool(Global.IndexProgChoose, Index);
    owner.Results = overallOk ? Results.OK : Results.NG;
    owner.ScoreResult = (kept > 0) ? (int)Math.Round(scoreSum / kept, 1) : 0;
}
```

### 5.5 ResultItem — không cần đổi

Quyết định: tận dụng `ResultItem.Name` (Patterns.cs đã có ở ResultItem.cs:16) làm label string thay vì thêm field mới. Đỡ rủi ro serialize backward-compat. Comment 1 dòng tại call site `item.Name = r.Label` giải thích quy ước.

### 5.6 Save/Load

`Patterns` là `[Serializable]` (kiểm tra). `MultiTemplates` là `List<Pattern2TemplateEntry>` với `[Serializable]` + property setter public → Newtonsoft.Json tự handle. Field `TemplatePng` là `byte[]` → JSON base64. Field `_cachedBitmap` có `[NonSerialized]`.

Backward-compat: project cũ không có `MultiTemplates` trong JSON → deserialize default = `new List<>()`. `IsMultiTemplate` default = false. Bool field thiếu trong JSON → default false. **Không phá**.

---

## 6. UI — BeeInterface/Tool/ToolPattern.{cs,Designer.cs}

> Áp dụng 2 memory:
> - `feedback_ui_in_designer`: layout/control declaration + event wiring ở `.Designer.cs`; body handler ở `.cs`.
> - `feedback_collapsible_param_sections`: section gom theo `RJButton` header (IsTouch=true) toggle Visible của TableLayoutPanel con.

### 6.1 Designer.cs — thêm section "Multi-Template"

Trong `InitializeComponent()`, sau section preprocess hiện có:

```csharp
// Header button
this.btnSecMulti = new ReaLTaiizor.Controls.RJButton();
this.btnSecMulti.Text = "▶ Multi-Template";
this.btnSecMulti.IsTouch = true;
this.btnSecMulti.Dock = DockStyle.Top;
this.btnSecMulti.Height = 30;
this.btnSecMulti.Click -= OnSecMultiClicked;
this.btnSecMulti.Click += OnSecMultiClicked;

// Content panel
this.tlpMulti = new TableLayoutPanel();
this.tlpMulti.Dock = DockStyle.Top;
this.tlpMulti.AutoSize = true;
this.tlpMulti.Visible = false;            // mặc định collapsed
this.tlpMulti.ColumnCount = 1;
this.tlpMulti.RowCount = 3;

this.chkMultiTemplate = new CheckBox();
this.chkMultiTemplate.Text = "Enable multi-template mode";
this.chkMultiTemplate.AutoSize = true;
this.chkMultiTemplate.CheckedChanged -= OnChkMultiTemplateChanged;
this.chkMultiTemplate.CheckedChanged += OnChkMultiTemplateChanged;

this.dgvTemplates = new DataGridView();
this.dgvTemplates.Dock = DockStyle.Fill;
this.dgvTemplates.Height = 220;
this.dgvTemplates.AllowUserToAddRows = false;
this.dgvTemplates.AutoGenerateColumns = false;
this.dgvTemplates.RowTemplate.Height = 64;
this.dgvTemplates.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLabel", HeaderText = "Label", Width = 100 });
this.dgvTemplates.Columns.Add(new DataGridViewImageColumn { Name = "colThumb", HeaderText = "Thumb", Width = 80, ImageLayout = DataGridViewImageCellLayout.Zoom });
this.dgvTemplates.Columns.Add(new DataGridViewTextBoxColumn { Name = "colScore", HeaderText = "Score≥", Width = 60 });
this.dgvTemplates.Columns.Add(new DataGridViewTextBoxColumn { Name = "colExpected", HeaderText = "Expect", Width = 60 });
this.dgvTemplates.CellValueChanged -= OnDgvCellValueChanged;
this.dgvTemplates.CellValueChanged += OnDgvCellValueChanged;

// Button row
var pnlBtns = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true };
this.btnAddTpl = new Button { Text = "Add (current ROI)" };
this.btnAddTpl.Click -= OnAddTplClicked; this.btnAddTpl.Click += OnAddTplClicked;
this.btnRemoveTpl = new Button { Text = "Remove" };
this.btnRemoveTpl.Click -= OnRemoveTplClicked; this.btnRemoveTpl.Click += OnRemoveTplClicked;
this.btnTplUp = new Button { Text = "↑" };
this.btnTplUp.Click -= OnTplUpClicked; this.btnTplUp.Click += OnTplUpClicked;
this.btnTplDown = new Button { Text = "↓" };
this.btnTplDown.Click -= OnTplDownClicked; this.btnTplDown.Click += OnTplDownClicked;
pnlBtns.Controls.AddRange(new Control[] { btnAddTpl, btnRemoveTpl, btnTplUp, btnTplDown });

this.tlpMulti.Controls.Add(this.chkMultiTemplate, 0, 0);
this.tlpMulti.Controls.Add(pnlBtns, 0, 1);
this.tlpMulti.Controls.Add(this.dgvTemplates, 0, 2);

// Add vào params area
this.pnlParams.Controls.Add(this.tlpMulti);
this.pnlParams.Controls.Add(this.btnSecMulti);
```

Declarations field tại đầu Designer.cs:
```csharp
private ReaLTaiizor.Controls.RJButton btnSecMulti;
private TableLayoutPanel tlpMulti;
private CheckBox chkMultiTemplate;
private DataGridView dgvTemplates;
private Button btnAddTpl, btnRemoveTpl, btnTplUp, btnTplDown;
```

### 6.2 ToolPattern.cs — handler body

```csharp
private void OnSecMultiClicked(object s, EventArgs e)
{
    tlpMulti.Visible = !tlpMulti.Visible;
    btnSecMulti.Text = (tlpMulti.Visible ? "▼ " : "▶ ") + "Multi-Template";
}

private void OnChkMultiTemplateChanged(object s, EventArgs e)
{
    var p = GetCurrentPatterns();
    if (p == null) return;
    p.IsMultiTemplate = chkMultiTemplate.Checked;
    UpdateSingleControlsEnabled(!p.IsMultiTemplate);  // disable bmRaw preview/Learn cũ khi multi mode
    dgvTemplates.Enabled = p.IsMultiTemplate;
}

private void OnAddTplClicked(object s, EventArgs e)
{
    var p = GetCurrentPatterns();
    if (p == null) return;
    var bmp = CaptureCurrentRoiBitmap();   // dùng same path như Learn cũ: crop từ camera + ROI hiện tại
    if (bmp == null) return;
    var entry = new Pattern2TemplateEntry {
        Label = $"Tpl{p.MultiTemplates.Count + 1}",
        ScoreThreshold = 70f,
        ExpectedCount = 1,
        TemplatePng = BitmapToPng(bmp)
    };
    p.MultiTemplates.Add(entry);
    RefreshTemplatesGrid(p);
}

private void OnRemoveTplClicked(object s, EventArgs e)
{
    var p = GetCurrentPatterns();
    if (p == null) return;
    int idx = dgvTemplates.CurrentCell?.RowIndex ?? -1;
    if (idx < 0 || idx >= p.MultiTemplates.Count) return;
    p.MultiTemplates.RemoveAt(idx);
    RefreshTemplatesGrid(p);
}

private void OnTplUpClicked(object s, EventArgs e)   { MoveTpl(-1); }
private void OnTplDownClicked(object s, EventArgs e) { MoveTpl(+1); }
private void MoveTpl(int delta) { /* swap in list + RefreshTemplatesGrid */ }

private void OnDgvCellValueChanged(object s, DataGridViewCellEventArgs e)
{
    var p = GetCurrentPatterns();
    if (p == null || e.RowIndex < 0 || e.RowIndex >= p.MultiTemplates.Count) return;
    var entry = p.MultiTemplates[e.RowIndex];
    var row = dgvTemplates.Rows[e.RowIndex];
    if (e.ColumnIndex == 0) entry.Label = row.Cells["colLabel"].Value?.ToString() ?? "";
    else if (e.ColumnIndex == 2) entry.ScoreThreshold = ParseFloat(row.Cells["colScore"].Value, 70f);
    else if (e.ColumnIndex == 3) entry.ExpectedCount  = ParseInt(row.Cells["colExpected"].Value, 1);
}

private void RefreshTemplatesGrid(Patterns p)
{
    dgvTemplates.Rows.Clear();
    foreach (var entry in p.MultiTemplates)
    {
        int r = dgvTemplates.Rows.Add();
        var row = dgvTemplates.Rows[r];
        row.Cells["colLabel"].Value = entry.Label;
        row.Cells["colThumb"].Value = entry.GetBitmap();   // nullable
        row.Cells["colScore"].Value = entry.ScoreThreshold;
        row.Cells["colExpected"].Value = entry.ExpectedCount;
    }
}
```

### 6.3 Bind khi mở form

Trong handler `Bind()` (hoặc method tương đương khi load tool state — tìm trong `ToolPattern.cs`):
```csharp
chkMultiTemplate.Checked = p.IsMultiTemplate;
RefreshTemplatesGrid(p);
dgvTemplates.Enabled = p.IsMultiTemplate;
```

### 6.4 Hard rules tuân thủ

- ✅ Mọi `+=` đi kèm `-=` ở Designer (CLAUDE.md 0.1.4).
- ✅ Dùng `Common.TryGetTool` (không index `Common.PropetyTools[...]`).
- ✅ Layout + wire ở Designer.cs (memory `feedback_ui_in_designer`).
- ✅ Collapsible section dùng RJButton + TableLayoutPanel (memory `feedback_collapsible_param_sections`).
- ✅ Không tạo control mới ở `BeeShared.UI` (mới 1 tool dùng).

---

## 7. Implementation Order (3 commit)

### Commit 1: Native — `[feat] Pattern2: add MatchBatchStable for multi-template`
1. Sửa `Pattern2.h`: thêm `s_BatchEntry`, mở rộng `Img`, thêm 2 value struct CLI, declare 4 method ref class.
2. Refactor `Pattern2.cpp::MatchStable`: extract `BuildSourceFeatures` + `MatchStableOnPreprocessedSource`.
3. Implement `LearnPatternBatchBegin/AddBatchTemplate/LearnPatternBatchEnd/MatchBatchStable` + helpers (`SystemStringToUtf8`, `PreprocessSnapshotEqual`, `RotatedIoU` nếu chưa có).
4. **Verify**:
   - Build native: `MSBuild Pattern/Pattern.vcxproj /p:Configuration=Release /p:Platform=x64`.
   - Build sln full: pass.
   - Regression single-template: chạy project mẫu cũ → kết quả `Rotaterectangle` không đổi (so ≤ ε=0.01 cho Cx/Cy/Score).
5. Commit standalone — C# chưa gọi API mới, nhưng phải build pass full sln (chỉ cần `BeeCpp.dll` có symbol mới, chưa ai dùng).

### Commit 2: C# engine — `[feat] Patterns: add IsMultiTemplate batch path`
1. Thêm `Pattern2TemplateEntry` (mới class trong Patterns.cs hoặc Items/).
2. Thêm field `IsMultiTemplate`, `MultiTemplates` vào `Patterns`.
3. Extract `BuildStableConfig()` từ `DoWork` (refactor không đổi behavior).
4. Thêm `LearnPatternsBatch()`, `ProcessBatchResults()`.
5. Branch trong `LearnPattern` + `DoWork`.
6. **Verify**:
   - Build sln pass; warning ≤ baseline.
   - `bash tools/check_propety_tools.sh` exit 0.
   - Load `ClassProject.json` cũ → `IsMultiTemplate=false`, `MultiTemplates.Count==0`, không crash.
   - Single-template smoke chạy đúng (behavior cũ).
   - Test manual: chỉnh `IsMultiTemplate=true` qua debugger, thêm 2 entry vào `MultiTemplates` → match ra label đúng.

### Commit 3: UI — `[feat] ToolPattern: multi-template UI section`
1. Sửa `ToolPattern.Designer.cs` — thêm section collapsible + DataGridView + 4 button + 1 checkbox.
2. Sửa `ToolPattern.cs` — handler body.
3. Bind trong load handler.
4. **Verify**:
   - Build pass.
   - Mở Form trong VS Designer không lỗi.
   - Smoke: bật checkbox → grid enabled; Add 3 template (label A/B/C, expected 2/1/1), Save project, Reopen → 3 entry còn nguyên với label/threshold/expected.
   - Run inspection trên ảnh có A×2 B×1 C×1 → ResultTable hiện 4 entry với label đúng, overall OK.
   - Run trên ảnh thiếu B → overall NG.
   - Mở/đóng form 5 lần → không leak handler.

---

## 8. Verification Matrix

| Test | Commit | Method | Expected |
|---|---|---|---|
| Full build | 1,2,3 | `MSBuild EasyVision.sln /p:Configuration=Release /p:Platform=x64` | pass, warning ≤ baseline |
| Regression single-template | 1 | Load project cũ, chạy → diff `Cx,Cy,Score` với baseline | |delta| ≤ 0.01 |
| Native batch unit | 1 | C++ standalone test gọi 3 template trên ảnh tổng hợp | Trả đúng 3 match khác label |
| Backward-compat JSON | 2 | Load `ClassProject.json` từ git tag cũ | No exception, single mode |
| Multi-mode smoke | 3 | Project mới, 3 template A/B/C expect 2/2/1 trên ảnh "ABCAB" | 5 match, label đúng, OK |
| Multi-mode NG | 3 | Ảnh thiếu C | overall NG |
| Performance | 3 | 8 template × 200×200 ROI trên 2048×2048 | < 2× thời gian 1 template |
| Save/Load multi | 3 | Add 3 entry → save → reopen | 3 entry còn nguyên |
| Convention | 2,3 | `bash tools/check_propety_tools.sh` | exit 0 |
| Event leak | 3 | Mở/đóng form 5 lần | Handler count không tăng |

---

## 9. Critical Files Map

| File | Action | Phạm vi sửa |
|---|---|---|
| `Pattern/Pattern2.h` | Thêm | sau line 376 (Img extend), sau 491 (CLI structs), sau 608 (ref class methods) |
| `Pattern/Pattern2.cpp` | Refactor + thêm | Refactor `MatchStable` 2694-end thành `BuildSourceFeatures` + `MatchStableOnPreprocessedSource`; thêm 4 method batch sau impl `LearnPatternStable` (~line 2130) |
| `BeeCore/Unit/Patterns.cs` | Thêm + branch | Field at ~444; `Pattern2TemplateEntry` class cuối file; `LearnPatternsBatch`, `BuildStableConfig`, `ProcessBatchResults` mới; branch `LearnPattern` line 458, `DoWork` line 1707 |
| `BeeInterface/Tool/ToolPattern.Designer.cs` | Thêm | Section collapsible mới trong `InitializeComponent()` |
| `BeeInterface/Tool/ToolPattern.cs` | Thêm | 7 handler + 2 helper (`CaptureCurrentRoiBitmap`, `BitmapToPng`) |
| `BeeCore/Func/ResultItem.cs` | KHÔNG sửa | Dùng `Name` field sẵn có làm label |

---

## 10. Out of Scope

- `BeeCore/Unit/MultiPattern.cs` (tool v1) — giữ nguyên.
- `Pattern/Pattern.cpp` (v1 native) — giữ nguyên.
- Preprocess pipeline / 5 preset trong Pattern2 — chỉ reuse, không thay đổi semantics.
- Single-template public API của `Pattern2` ref class — 100% backward-compat.
- Per-template preprocess khác nhau — Phương án A (shared snapshot, validate ở `LearnPatternBatchEnd`).
- Refactor structural theo CLAUDE.md Phase 1-6.
- Thêm field `Label` vào `ResultItem` — dùng `Name` thay.

---

## 11. Risks & Mitigations

| Risk | Mitigation |
|---|---|
| Refactor `MatchStable` làm vỡ output single-template | Regression test diff trước/sau (ε=0.01); revert ngay nếu diff |
| `ScopedTemplSwap` không restore đúng trong loop batch | Mỗi iteration tạo `ScopedTemplSwap` riêng (RAII), test 100 lần Match liên tiếp không leak/state drift |
| RotatedIoU chưa có | Grep trước; nếu thiếu, dùng `cv::rotatedRectangleIntersection` + `cv::contourArea` (chuẩn OpenCV) |
| Per-template preprocess khác snapshot | Validate ở `LearnPatternBatchEnd` → throw rõ ràng; UI ép shared preset |
| `Pattern2TemplateEntry.TemplatePng` lớn → JSON nặng | Compress PNG; cảnh báo nếu >50 entry |
| Bitmap thumb chiếm RAM | Lazy decode trong `GetBitmap()`; Dispose khi remove |
| Event handler leak ở DataGridView | `-=` trước `+=` (đã follow); Form Closed dispose grid |
| `bm.Data` invalid sau Dispose | `GC.KeepAlive(gray)` trước native call |

---

## 12. Rollback

- **Commit 1 fail**: `git revert <sha>`. Single API vẫn hoạt động (chưa ai gọi batch).
- **Commit 2 fail**: `git revert <sha>` C#. Native batch API standalone vô hại.
- **Commit 3 fail**: `git revert <sha>` UI. Engine batch vẫn dùng được qua code (`p.IsMultiTemplate = true`).
- **Regression single mode**: nếu phát hiện sau merge, `git revert` cả 3 commit theo thứ tự ngược.
