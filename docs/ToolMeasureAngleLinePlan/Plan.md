# Plan - Tool Measure Angle Line/Point input mode

## Goal

Tool Measure dang do `Angle` bang cach lay 4 toa do point roi ghep thanh 2 line. Can them option de moi line co the chon:

- `Point`: hanh vi cu, moi line gom 2 point lay tu cac tool Pattern/Circle/Width/Edge...
- `Line`: chon truc tiep 1 line tu tool `TypeTool.Edge` hoac `TypeTool.Edge2`, combo chi list cac tool line hop le.

Muc tieu la nguoi dung co the do goc giua 2 duong Edge/Edge2 ma khong can chon 2 endpoint nhu 2 point doc lap, dong thoi van giu duoc workflow point hien tai.

## Current Code Reading

- `BeeCore/Unit/Measure.cs`
  - `listPointChoose` luu 4 tuple `(toolName, pointIndex)` cho Line 1 Point 1/Point 2 va Line 2 Point 1/Point 2.
  - `DoWork()` lay `Propety2.listP_Center[index]` cho 4 diem.
  - Rieng Point 4 co logic `PropetyTool4.Name.Contains("Edge")` de tinh perpendicular foot, day la inference bang ten tool va khong phu hop khi nguoi dung muon chon ca line.
  - `Complete()` tinh angle bang `Cal.GetAngleBetweenSegments(p1,p2,p3,p4)`.
- `BeeInterface/Tool/ToolMeasure.cs`
  - UI hien co `cb1/cb2`, `cb3/cb4`, `cb5/cb6`, `cb7/cb8`.
  - Cac combo tool dang load tu `GetToolNames()` va loc type trong event.
  - Selection handlers ghi truc tiep vao `Propety.listLine1Point`, `Propety.listLine2Point`, `listRot`, `listPointChoose`.
  - `cbMethord_SelectedIndexChanged` hien gan nham tu `cbDirect.SelectedItem`, nen nen sua chung trong cung batch vi dung scope Measure UI.
- `BeeInterface/Tool/ToolMeasure.Designer.cs`
  - UI hien co 2 row point cho moi line. Can them mode combo/button nho cho `Line 1` va `Line 2`, hoac doi label row de khong lam vo layout.
- `BeeCore/Unit/Edge.cs` va `BeeCore/Unit/Edge2.cs`
  - Ca hai co `Line2DCli` endpoint `X1/Y1/X2/Y2` sau khi detect.
  - `Edge2` gan `Line2DCli = ParallelLines.CenterLine` khi tim duoc cap song song, phu hop lam line do angle.

## Proposed Model

Them enum serializable trong `BeeGlobal/Enums.cs`:

```csharp
public enum MeasureLineInputMode
{
    Point,
    Line
}
```

Them vao `Measure`:

```csharp
public MeasureLineInputMode Line1InputMode = MeasureLineInputMode.Point;
public MeasureLineInputMode Line2InputMode = MeasureLineInputMode.Point;
public List<Tuple<string, int>> listLineChoose = new List<Tuple<string, int>>();
```

`listLineChoose[0]` va `[1]` luu tool line cho Line 1/Line 2 khi mode `Line`; `Item2` de `-1` vi Edge/Edge2 hien chi expose line chinh qua `Line2DCli`. Old save khong co field moi se default `Point`.

## Proposed Runtime

Tach helper trong `Measure.cs`:

```csharp
private bool TryLoadMeasureLine(
    IReadOnlyList<PropetyTool> tools,
    MeasureLineInputMode mode,
    Tuple<string, int> first,
    Tuple<string, int> second,
    out Point p1,
    out Point p2,
    out RectRotate rot1,
    out RectRotate rot2)
{
    if (mode == MeasureLineInputMode.Line)
        return TryLoadEdgeLine(tools, first, out p1, out p2, out rot1, out rot2);

    return TryLoadPointPair(tools, first, second, out p1, out p2, out rot1, out rot2);
}
```

Line mode lay endpoint:

```csharp
private static bool TryGetLine2D(PropetyTool tool, out Line2DCli line)
{
    line = null;
    if (tool == null || tool.Results != Results.OK)
        return false;

    if (tool.TypeTool == TypeTool.Edge)
        line = ((Edge)tool.Propety).Line2DCli;
    else if (tool.TypeTool == TypeTool.Edge2)
        line = ((Edge2)tool.Propety).Line2DCli;

    return line != null && line.Found;
}
```

`Complete()` khong can doi cong thuc; chi can dam bao `listLine1Point[0..1]` va `listLine2Point[0..1]` da la endpoint dung theo mode.

## Proposed UI

Them hai combo mode:

- `cbLine1Mode`: `Point`, `Line`
- `cbLine2Mode`: `Point`, `Line`

Behavior:

- Khi mode `Point`:
  - Giu UI cu: Point 1 + Point 2 rows.
  - Tool combo list cac tool co point center hop le: Pattern, Circle, Width, Edge, Edge2 neu `Propety2.listP_Center` co endpoint.
- Khi mode `Line`:
  - Row dau tien label thanh `Line`; combo tool chi list `TypeTool.Edge` va `TypeTool.Edge2`.
  - Row thu hai cua line disable/hidden de tranh hieu nham.
  - Khi chon tool line, UI cap nhat `listLineChoose[lineIndex]` va preview endpoint tu `Line2DCli`.

Nen gom logic combo vao helper de tranh lap 4 event:

```csharp
private IEnumerable<string> GetMeasureSourceNames(MeasureLineInputMode mode)
{
    return GetToolNames().Cast<string>()
        .Where(name =>
        {
            var tool = GetTool(FindToolIndexByName(name));
            if (tool == null)
                return false;

            if (mode == MeasureLineInputMode.Line)
                return tool.TypeTool == TypeTool.Edge || tool.TypeTool == TypeTool.Edge2;

            return tool.TypeTool == TypeTool.Pattern
                || tool.TypeTool == TypeTool.Circle
                || tool.TypeTool == TypeTool.Width
                || tool.TypeTool == TypeTool.Edge
                || tool.TypeTool == TypeTool.Edge2;
        });
}
```

## Implementation Order

1. Add enum and serialized fields with migration/default initialization.
2. Refactor Measure runtime to resolve two lines through helper: point pair or Edge/Edge2 `Line2DCli`.
3. Add UI mode controls and combo filtering.
4. Save/load binding: restore selected modes and source tools.
5. Fix `cbMethord_SelectedIndexChanged` to use `cbMethord.SelectedItem`.
6. Build Release x64 once after the code batch.

## Risks

- Designer churn: keep control additions minimal and avoid broad layout rewrite.
- Old project files: must keep `listPointChoose` behavior intact.
- Edge/Edge2 timing: selected Edge tool must be done before Measure resolves line; if not found or not OK, Measure returns NG.
- Tool type filtering: do not rely on `Name.Contains("Edge")`.

