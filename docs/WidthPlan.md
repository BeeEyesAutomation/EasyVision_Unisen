# Plan: Width Tool – PointToLine Full-check + bỏ Tol mm

## Context

Trong màn hình cấu hình Width tool (PointToLine mode), user muốn:
1. **Thêm lựa chọn "Full"** trong dropdown `Point` → kiểm tất cả điểm từ Pitch tool thay vì chỉ 1 điểm (P1/P2/P3/P4).
2. **Bỏ dòng "Tol mm"** (`numPointTolerance`) khỏi UI và logic.
3. **Dùng Score (trackScore)** làm ngưỡng tolerance thay cho `PointToLineToleranceMm`.

Hiện tại `CompletePointToLine()` so `deviation <= PointToLineToleranceMm`; sau thay đổi so `deviation <= owner.Score` (mm, range 0–20, step 0.1).

---

## Files cần sửa

| File | Loại thay đổi |
|---|---|
| `BeeCore/Unit/Width.cs` | Thêm field, thêm method RunPointToLineAll, sửa Complete/Draw |
| `BeeInterface/Tool/ToolWidth.cs` | Bỏ numPointTolerance, thêm "Full" vào cbPointIndex |
| `BeeCore/Func/Engines/WidthEngineRunner.cs` | Thêm PointToLineCheckAll vào WidthViewState |

---

## Chi tiết thay đổi

### 1. `BeeCore/Unit/Width.cs`

#### 1a. Thêm field mới (sau `PointToLineToleranceMm`)
```csharp
public bool PointToLineCheckAll = false;
[NonSerialized]
public List<(PointF center, PointF foot, float dist)> AllPointResults;
```
Giữ `PointToLineToleranceMm` để backward-compat với project cũ (không xóa, không dùng nữa).

#### 1b. Sửa `RunPointToLine()` – thêm branch đầu
```csharp
private void RunPointToLine(RectRotate area)
{
    if (PointToLineCheckAll) { RunPointToLineAll(area); return; }
    // code cũ không đổi
}
```

#### 1c. Thêm method `RunPointToLineAll(RectRotate area)`
```csharp
private void RunPointToLineAll(RectRotate area)
{
    var tools = Common.EnsureToolList(IndexThread);
    if (tools == null) return;

    PropetyTool source = null;
    if (!string.IsNullOrEmpty(PointSourceToolName))
        source = tools.FirstOrDefault(t => t != null && t.Name == PointSourceToolName);
    if (source == null)
        source = tools.FirstOrDefault(t => t != null && t.TypeTool == TypeTool.Pitch);
    if (source?.Propety2 == null) return;

    var pts = source.Propety2.listP_Center as List<System.Drawing.Point>;
    if (pts == null || pts.Count == 0) return;
    if (string.IsNullOrEmpty(PointSourceToolName)) PointSourceToolName = source.Name;

    LineDirectionMode dirMode = ReferenceLineOrientation == LineOrientation.Vertical
        ? LineDirectionMode.Vertical : LineDirectionMode.Horizontal;
    ReferenceLineL = RansacLine.FindBestLine(
        matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(),
        RansacIterations, (float)RansacThreshold, 120000, Index, 1 / Scale, 0.2f,
        dirMode, ToCliScanMode(ReferenceLineScan), 0, ReferenceLineAngleRange);
    if (!ReferenceLineL.Found) return;

    AllPointResults = new List<(PointF, PointF, float)>();
    float maxDist = 0;
    PointF worstCenter = PointF.Empty, worstFoot = PointF.Empty;
    foreach (var p in pts)
    {
        PointF ptLocal = WorldToAreaLocal(new PointF(p.X, p.Y), area);
        PointF foot = ProjectPointToLine(ptLocal, ReferenceLineL);
        float dist = (float)(DistancePointToLine(ptLocal, ReferenceLineL) / Scale);
        AllPointResults.Add((ptLocal, foot, dist));
        if (dist > maxDist) { maxDist = dist; worstCenter = ptLocal; worstFoot = foot; }
    }
    PointToLineCenter = worstCenter;
    PointToLineFoot = worstFoot;
    WidthResult = maxDist;
    PointToLineFound = true;
    listP_Center = pts.ToList();
    rectRotates = new List<RectRotate>();
}
```

#### 1d. Sửa `CompletePointToLine()` – dùng `owner.Score` thay `PointToLineToleranceMm`
```csharp
private void CompletePointToLine()
{
    PropetyTool owner = Common.TryGetTool(IndexThread, Index);
    if (owner == null) return;
    if (!PointToLineFound) { owner.ScoreResult = 999; owner.Results = Results.NG; return; }
    if (IsCalibs && !Global.IsRun) { PointToLineNominalMm = WidthResult; WidthTemp = WidthResult; }

    float nominal = PointToLineNominalMm > 0 ? PointToLineNominalMm : WidthTemp;
    float tolerance = owner.Score; // Tol mm → Score

    if (PointToLineCheckAll && AllPointResults != null && AllPointResults.Count > 0)
    {
        float maxDev = 0;
        foreach (var r in AllPointResults)
        {
            float dev = nominal > 0 ? Math.Abs(r.dist - nominal) : 0;
            if (dev > maxDev) maxDev = dev;
        }
        owner.ScoreResult = maxDev;
        owner.Results = maxDev <= tolerance ? Results.OK : Results.NG;
    }
    else
    {
        float deviation = nominal > 0 ? Math.Abs(WidthResult - nominal) : 0;
        owner.ScoreResult = deviation;
        owner.Results = deviation <= tolerance ? Results.OK : Results.NG;
    }
}
```

#### 1e. Sửa `DrawPointToLine()` – vẽ tất cả điểm khi Full mode
Khi `PointToLineCheckAll && AllPointResults != null`, loop qua `AllPointResults` vẽ từng cặp (center, foot). Worst point dùng màu NG nếu fail, còn lại màu OK. Cấu trúc giữ nguyên, chỉ thêm vòng loop.

---

### 2. `BeeInterface/Tool/ToolWidth.cs`

#### 2a. Xóa field `numPointTolerance` (dòng 51) và khởi tạo ở `EnsurePointToLinePanel()`

#### 2b. Sửa `EnsurePointToLinePanel()`
- Đổi `panel.RowCount = 4` (từ 5), bỏ row "Tol mm".
- Bỏ tạo `numPointTolerance`, bỏ `AddPanelRow(panel, "Tol mm", numPointTolerance, 4)`.
- Sửa `cbPointIndex.SelectedIndexChanged`:
```csharp
cbPointIndex.SelectedIndexChanged += (s, e) =>
{
    if (Propety == null || cbPointIndex.SelectedIndex < 0) return;
    if (cbPointIndex.SelectedIndex == 0)   // "Full"
    {
        Propety.PointToLineCheckAll = true;
    }
    else
    {
        Propety.PointToLineCheckAll = false;
        Propety.PointSourceIndex = cbPointIndex.SelectedIndex - 1; // offset -1 vì "Full" ở vị trí 0
    }
};
```

#### 2c. Sửa `PopulatePointIndexCombo()`
```csharp
private void PopulatePointIndexCombo()
{
    cbPointIndex.Items.Clear();
    cbPointIndex.Items.Add("Full");   // ← thêm mới, index 0
    string sourceName = cbPointSource.SelectedItem as string;
    int count = 0;
    var tool = Common.EnsureToolList(Global.IndexProgChoose)
                     .FirstOrDefault(t => t != null && t.Name == sourceName);
    if (tool != null)
    {
        try { var pts = tool.Propety2.listP_Center as List<System.Drawing.Point>;
              count = pts != null ? pts.Count : 0; }
        catch { count = 0; }
    }
    for (int i = 0; i < count; i++)
        cbPointIndex.Items.Add(tool != null && tool.TypeTool == TypeTool.Pitch
            ? $"P{i + 1}" : $"Point {i + 1}");
    // Restore selection
    if (Propety.PointToLineCheckAll)
        cbPointIndex.SelectedIndex = 0;
    else if (cbPointIndex.Items.Count > 1)
        cbPointIndex.SelectedIndex = Math.Min(Math.Max(1, Propety.PointSourceIndex + 1),
                                              cbPointIndex.Items.Count - 1);
}
```

#### 2d. Sửa `LoadPointToLinePanel()`
- Bỏ dòng `numPointTolerance.Value = ...`
- Khi restore `cbPointIndex`: nếu `state.PointToLineCheckAll` → `SelectedIndex = 0`, còn lại `SelectedIndex = state.PointSourceIndex + 1`.

---

### 3. `BeeCore/Func/Engines/WidthEngineRunner.cs`

#### 3a. Thêm `PointToLineCheckAll` vào `WidthViewState`
```csharp
public bool PointToLineCheckAll { get; set; }
```

#### 3b. Cập nhật `ReadFromOwner()` – thêm field mapping
```csharp
PointToLineCheckAll = propety.PointToLineCheckAll,
```
Bỏ `PointToLineToleranceMm` khỏi `WidthViewState` (không cần nữa).

---

## Lưu ý backward-compat

- Field `PointToLineToleranceMm` trong `Width.cs` **giữ nguyên** (không xóa) để project cũ load không lỗi JSON. Chỉ ngừng sử dụng trong logic.
- Khi load project cũ có `PointToLineCheckAll = false` (default) → behavior không đổi, chỉ `tolerance` nguồn đổi từ `PointToLineToleranceMm` → `owner.Score`.

---

## Verification

1. Build: `MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal`
2. Smoke: Mở Width tool → Mode = PointToLine → xác nhận UI chỉ có 4 dòng (Mode / Point tool / Point / Nominal mm), không còn "Tol mm".
3. Chọn Point = "Full" → chạy Test → xác nhận tất cả Pitch points được vẽ trên preview, result OK/NG theo Score.
4. Chọn Point = P2 → chạy Test → behavior như cũ nhưng tolerance = Score (không phải Tol mm).
5. Load project mẫu cũ → không exception.

---

*Plan sẽ được copy vào `docs/WidthPlan.md` khi bắt đầu implement.*
