# ToolWidth Edge-Based Parallel Gap Plan

## Request Summary

Replace the current `ToolWidth` parallel gap implementation:

```csharp
GapResult = ParallelGapDetector.MeasureParallelGap(matCrop, matProcess, MaximumLine, GapExtremum, LineOrientation, SegmentStatType, MinInliers);
```

with an edge-based measurement that finds exactly two edge lines through the same RANSAC path used by `ToolEdge`:

```csharp
RansacLine.FindBestLine(...)
```

The new behavior keeps the current `GapResult`/`WidthResult` output contract, draws the selected two lines and measurement ticks as before, and also draws the scan boxes used for detection.

## Confirmed Requirements

- Replace `ParallelGapDetector.MeasureParallelGap(...)` completely for `WidthMeasureMode.ParallelLines`.
- Use `RansacLine.FindBestLine(...)`, matching the ToolEdge detection method.
- Use the filtered image for detection:
  - crop the main rotated ROI from raw image,
  - apply the existing `ToolWidth` filters into `matProcess`,
  - crop scan boxes from `matProcess`,
  - run RANSAC on each scan-box crop.
- Add scan direction option:
  - `OutToInside = 1`: two scan boxes, one from each outside edge toward the center.
  - `InsideToOut = 2`: one central scan box, then detect two edges from the same scan crop with opposite scan priorities.
- Add `LengthScan` integer setting:
  - for `LineOrientation.Vertical`, `LengthScan` is scan-box width on X.
  - for `LineOrientation.Horizontal`, `LengthScan` is scan-box height on Y.
- Draw scan boxes:
  - red-style boxes for `LineOrientation.Vertical`.
  - orange-style boxes for `LineOrientation.Horizontal`.
  - one or two boxes depending on scan direction.
- Preserve current `GapResult`, `WidthResult`, calibration, OK/NG scoring, PLC result, and overlay contract.

## Current Code Preview

### Core Measurement

`BeeCore/Unit/Width.cs`

- `DoWork(...)` builds `matProcess` from the full rotated area:
  - `Cropper.CropRotatedRect(raw, rotArea, rotMask)`
  - grayscale/equalize
  - `Filters.ApplyEdgeMethod(...)`
  - optional morphology/noise cleanup
- `WidthMeasureMode.PointToLine` already uses `RansacLine.FindBestLine(...)`.
- `WidthMeasureMode.ParallelLines` currently calls `ParallelGapDetector.MeasureParallelGap(...)`.
- `Complete()` reads `GapResult.GapMedium`, `GapResult.GapMin`, or `GapResult.GapMax` based on `SegmentStatType`.
- `DrawResult(...)` expects:
  - `GapResult.line2Ds`
  - `GapResult.LineA`
  - `GapResult.LineB`
  - `GapResult.lineMid`

### ToolEdge Reference

`BeeCore/Unit/Edge.cs`

`ToolEdge` detects a line with:

```csharp
Line2DCli = RansacLine.FindBestLine(
    matProcess.Data, matProcess.Width, matProcess.Height, (int)matProcess.Step(),
    iterations: RansacIterations,
    threshold: (float)RansacThreshold,
    maxPoints: 120000,
    seed: Index,
    mmPerPixel: 1 / Scale,
    AspectLen,
    (LineDirectionMode)((int)LineOrientation),
    (BeeCpp.LineScanMode)((int)LineDirScan),
    0,
    AngleRange
);
```

`ToolWidth` should reuse this call style instead of the older managed `ParallelGapDetector` line extraction.

### UI and Runner State

`BeeInterface/Tool/ToolWidth.cs`

- Loads `WidthViewState` from `WidthEngineRunner.ReadFromOwner(...)`.
- Already binds:
  - `LineOrientation`
  - `SegmentStatType`
  - `GapExtremum`
  - `MaximumLine`
  - `MinInliers`
  - RANSAC threshold/iterations
  - filter parameters

`BeeCore/Func/Engines/WidthEngineRunner.cs`

- Mirrors settings into `WidthViewState`.
- `WidthRunResult.From(...)` currently treats non-PointToLine success as `propety.GapResult.line2Ds != null`.

## Proposed Data Model

Add a dedicated enum in `BeeGlobal/Enums.cs`:

```csharp
public enum WidthScanDirection
{
    OutToInside = 1,
    InsideToOut = 2
}
```

Add fields to `BeeCore/Unit/Width.cs`:

```csharp
public WidthScanDirection ScanDirection = WidthScanDirection.OutToInside;
public int LengthScan = 20;

[NonSerialized]
public List<RectangleF> ScanBoxes = new List<RectangleF>();

[NonSerialized]
public Line2DCli EdgeLineA = new Line2DCli();

[NonSerialized]
public Line2DCli EdgeLineB = new Line2DCli();
```

Notes:

- Use `LengthScan` for code spelling. The UI label can still display `LenghtScan` if the operator naming must match existing wording.
- Use `[NonSerialized]` for runtime-only detected boxes/lines.
- Consider `[OptionalField]` if old serialized projects need tolerant deserialization for the new persisted settings.

## Scan Box Geometry

All scan boxes are local to the already-cropped `rotArea` image coordinates:

```text
local ROI origin = (0, 0)
local ROI size   = (matProcess.Width, matProcess.Height)
```

### Vertical LineOrientation

Goal: find two mostly vertical edges. `LengthScan` controls box width.

`OutToInside`:

- left box: `x = 0`, `width = LengthScan`, full height.
- right box: `x = matProcess.Width - LengthScan`, `width = LengthScan`, full height.
- left scan priority: `LeftRight`.
- right scan priority: `RightLeft`.

`InsideToOut`:

- center box: centered on ROI X, `width = LengthScan`, full height.
- detect left edge with `RightLeft`.
- detect right edge with `LeftRight`.

### Horizontal LineOrientation

Goal: find two mostly horizontal edges. `LengthScan` controls box height.

`OutToInside`:

- top box: `y = 0`, `height = LengthScan`, full width.
- bottom box: `y = matProcess.Height - LengthScan`, `height = LengthScan`, full width.
- top scan priority: `TopBot`.
- bottom scan priority: `BotTop`.

`InsideToOut`:

- center box: centered on ROI Y, `height = LengthScan`, full width.
- detect top edge with `BotTop`.
- detect bottom edge with `TopBot`.

### Clamp Rules

- Clamp `LengthScan` to at least `1`.
- For vertical mode, clamp to `matProcess.Width`.
- For horizontal mode, clamp to `matProcess.Height`.
- If the effective scan crop is empty, return an empty `GapResult`.

## Detection Algorithm

Add a `RunParallelGapByEdges(RectRotate area)` method in `BeeCore/Unit/Width.cs`.

High-level flow:

1. Clear runtime state:
   - `GapResult = new GapResult();`
   - `ScanBoxes.Clear();`
   - `EdgeLineA = new Line2DCli();`
   - `EdgeLineB = new Line2DCli();`
2. Build scan boxes from `matProcess` dimensions, `LineOrientation`, `ScanDirection`, and `LengthScan`.
3. For each scan box:
   - create a `Mat` view or clone from `matProcess` using the scan rectangle.
   - call `RansacLine.FindBestLine(...)`.
   - pass `LineDirectionMode.Vertical` or `LineDirectionMode.Horizontal`.
   - pass the scan priority mapped to `BeeCpp.LineScanMode`.
4. Convert each `Line2DCli` from scan-box-local coordinates back to full ROI-local coordinates by adding the scan box offset to:
   - `X1`, `X2`, `X0`
   - `Y1`, `Y2`, `Y0`
5. If two lines are not found, keep `GapResult` empty and let `Complete()` mark NG using the existing `line2Ds == null` contract.
6. Convert the two full ROI-local `Line2DCli` objects into `Line2D` objects for `GapResult`.
7. Compute:
   - `GapMin`
   - `GapMedium`
   - `GapMax`
   - `lineMid`
   using the same geometry rules already used by `ParallelGapDetector.MeasureParallelGap(...)`.
8. Set:

```csharp
GapResult = new GapResult
{
    line2Ds = new List<Line2D> { lineA, lineB },
    LineA = lineA,
    LineB = lineB,
    lineMid = lineMids,
    GapMin = shortPx,
    GapMedium = mediumPx,
    GapMax = longPx,
    Inlier = Math.Min(EdgeLineA.Inliers, EdgeLineB.Inliers)
};
```

## Helper Methods

Add small private helpers in `Width.cs` to keep the diff controlled:

- `BuildWidthScanBoxes()`
- `FindLineInScanBox(RectangleF box, BeeCpp.LineScanMode scanMode)`
- `OffsetLine(Line2DCli line, float dx, float dy)`
- `ToLine2D(Line2DCli line)`
- `BuildGapResultFromEdgeLines(Line2DCli lineA, Line2DCli lineB)`
- `SolveX(...)` / `SolveY(...)` or reuse the existing local formulas from `Gap.cs`

The helper should not change C++/CLI signatures, native ownership, or `RansacLineCore`.

## Width.DoWork Change

Replace only the parallel-lines branch:

```csharp
if (MeasureMode == WidthMeasureMode.PointToLine)
{
    RunPointToLine(rotArea);
    return;
}

RunParallelGapByEdges(rotArea);
```

Do not keep `ParallelGapDetector.MeasureParallelGap(...)` as a fallback, because the requirement is full replacement.

`ParallelGapDetector` can remain as an allocated member for backward compatibility unless a later cleanup entry removes it safely.

## Complete and Scoring

Keep `Complete()` behavior stable:

- `SegmentStatType.Average` maps to `GapResult.GapMedium / Scale`.
- `SegmentStatType.Shortest` maps to `GapResult.GapMin / Scale`.
- `SegmentStatType.Longest` maps to `GapResult.GapMax / Scale`.
- Calibration updates:
  - `MinInliers`
  - `MinLen`
  - `MaxLen`
  - `WidthTemp`
- OK/NG remains based on `ScoreResult <= owner.Score`.

Potential adjustment:

- `MinLen`/`MaxLen` currently use gap values, not line length. Keep that behavior for compatibility.
- `MaximumLine` becomes unused for edge-based Width. Leave it in place for serialized compatibility unless UI cleanup is explicitly requested.

## Overlay Plan

Update `DrawResult(...)` in `BeeCore/Unit/Width.cs` after the transform into ROI-local coordinates:

1. Draw scan boxes before drawing lines:

```csharp
using (Pen scanPen = new Pen(LineOrientation == LineOrientation.Vertical ? Color.Red : Color.Orange, 2))
{
    foreach (RectangleF box in ScanBoxes)
        gc.DrawRectangle(scanPen, box.X, box.Y, box.Width, box.Height);
}
```

2. Keep existing line drawing:
   - draw optional grey background lines if needed.
   - draw `GapResult.LineA`.
   - draw `GapResult.LineB`.
   - draw ticks, measurement line, and width text.

3. `matProcess` display remains full ROI, so the operator can see all filtered edges while scan boxes show which region was used by RANSAC.

## UI Plan

Update `ToolWidth` UI with two new controls:

- scan direction selector:
  - `OutToInside`
  - `InsideToOut`
- `LengthScan` integer control.

Recommended placement:

- Add controls to the existing `layPointToLine`/mode panel area or add a small `layWidthScan` block under the existing orientation/gap controls in `ToolWidth.Designer.cs`.
- Bind through `WidthEngineRunner.ReadFromOwner(...)` and `WidthViewState`.
- Add event handlers in `ToolWidth.cs`:

```csharp
private void cbWidthScanDirection_SelectedIndexChanged(object sender, EventArgs e)
{
    if (Propety == null || cbWidthScanDirection.SelectedItem == null) return;
    Propety.ScanDirection = (WidthScanDirection)Enum.Parse(
        typeof(WidthScanDirection),
        cbWidthScanDirection.SelectedItem.ToString());
    WidthEngineRunner.MarkOwnerWaiting(OwnerTool);
}

private void numLengthScan_ValueChanged(object sender, EventArgs e)
{
    if (Propety == null) return;
    Propety.LengthScan = (int)numLengthScan.Value;
    WidthEngineRunner.MarkOwnerWaiting(OwnerTool);
}
```

If the designer is too noisy, create controls programmatically in `ToolWidth.cs` as a first pass, following the existing PointToLine panel pattern.

## Files To Change During Implementation

| File | Change |
| --- | --- |
| `BeeGlobal/Enums.cs` | Add `WidthScanDirection` enum. |
| `BeeCore/Unit/Width.cs` | Add settings/runtime fields, scan-box construction, edge-based two-line detection, line conversion, `GapResult` construction, scan-box overlay. |
| `BeeCore/Func/Engines/WidthEngineRunner.cs` | Add `ScanDirection` and `LengthScan` to `WidthViewState`. |
| `BeeInterface/Tool/ToolWidth.cs` | Bind/load new controls and mark owner waiting on change. |
| `BeeInterface/Tool/ToolWidth.Designer.cs` | Add scan direction and length controls unless implemented programmatically. |

No changes should be needed in:

- `Pattern/RansacLineCore.*`
- `Pattern/RansacLineCli.*`
- `BeeCore/Algorithm/Gap.cs`

## Verification Plan

Run the full solution build once after the implementation batch, because the implementation touches C# logic and UI designer code:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal
```

Manual verification:

1. `LineOrientation.Vertical` + `OutToInside`
   - two red scan boxes on left/right edges.
   - two vertical lines found.
   - width result matches distance between detected lines.
2. `LineOrientation.Vertical` + `InsideToOut`
   - one red center scan box.
   - two vertical edges found from center outward.
3. `LineOrientation.Horizontal` + `OutToInside`
   - two orange scan boxes on top/bottom edges.
   - two horizontal lines found.
4. `LineOrientation.Horizontal` + `InsideToOut`
   - one orange center scan box.
   - two horizontal edges found from center outward.
5. Confirm `matProcess` display still shows the filtered full ROI.
6. Confirm calibration still updates `WidthTemp`, min/max limits, and owner result.

## Risks And Notes

- `FindBestLine` returns one best line from a scan crop. `InsideToOut` with one center box must call it twice with opposite scan priority; if both calls return the same edge, the implementation may need to mask/remove the first line band before the second call. Start without masking, then add a narrow exclusion band only if manual verification shows duplicate line selection.
- `MaximumLine` becomes obsolete for this mode but should stay serialized for compatibility.
- `GapExtremum` also becomes less meaningful when scan direction explicitly chooses two lines. Keep it persisted but ignore it for edge-based Width unless a later requirement wants nearest/farthest behavior inside each scan crop.
- Keep all C++/CLI API boundaries unchanged.
- Do not add a fallback to `ParallelGapDetector.MeasureParallelGap(...)`; the requested behavior is full replacement.
