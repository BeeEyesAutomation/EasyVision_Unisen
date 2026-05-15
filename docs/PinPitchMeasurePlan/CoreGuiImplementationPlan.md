# Core + GUI Implementation Plan - ToolPitch and ToolWidth

## Goal

Complete two pin-related workflows:

- `ToolPitch`: find pin centers `P1..P4`, measure pitch between pins, and show the pin/pitch overlay.
- `ToolWidth`: add a mode that measures from one point/pin center to one straight line `L`, then shows the perpendicular foot and distance result.

Responsibility boundary:

- Pitch owns pin centers and pitch values.
- Width owns point-to-line distance.
- BeeCpp owns heavy image processing.
- C# core owns orchestration, scoring, persistence, and drawing.
- WinForms GUI owns parameter binding and user interaction only.

## ToolPitch Core Changes

Files:

- `BeeCore/Unit/Pitch.cs`
- `BeeCore/Unit/PinPitchGdiPainter.cs` or extend `BeeCore/Unit/PitchGdiPainter.cs`
- `BeeCore/Func/Engines/PitchEngineRunner.cs` if this runner exists or should be added for UI state isolation
- `BeeGlobal/Enums.cs`

Add enum:

```csharp
public enum PitchMeasureMode
{
    PeakRoot,
    PinPitch
}
```

Add serialized fields to `Pitch`:

```csharp
public PitchMeasureMode MeasureMode = PitchMeasureMode.PeakRoot;
public int ExpectedPinCount = 4;
public LineOrientation PitchDirection = LineOrientation.Horizontal;
public float NominalPitchMm;
public float PitchToleranceMm = 0.05f;
public bool UseProjectedPitch = true;
public bool ShowPinDebugOverlay = true;
```

Add non-serialized runtime fields:

```csharp
[NonSerialized] public BeeCpp.PinPitchCli PinPitchMeasure;
[NonSerialized] public BeeCpp.PinPitchCliResult PinPitchResult;
public List<System.Drawing.Point> PinCenters = new List<System.Drawing.Point>();
public float Pitch12Mm;
public float Pitch23Mm;
public float Pitch34Mm;
public float SpanP1P4Mm;
```

Runtime behavior:

1. In `SetModel()`, initialize both current `PitchCli` and new `PinPitchCli` when needed.
2. In `DoWork()`:
   - If `MeasureMode == PeakRoot`, keep current behavior unchanged.
   - If `MeasureMode == PinPitch`, crop `rotArea`, preprocess using current `MethordEdge`/threshold/morphology settings, call `PinPitchCli.Measure()`, transform centers from crop-local to image/global coordinates, and populate `listP_Center` with P1..P4.
3. In `Complete()`:
   - If `PeakRoot`, keep current score behavior.
   - If `PinPitch`, score pitch values against `NominalPitchMm` and `PitchToleranceMm`.
   - `ScoreResult` should be the worst absolute deviation in mm or percent, but UI label must make unit clear.
4. In `DrawResult()`:
   - Keep existing peak/root drawing.
   - For `PinPitch`, draw P1..P4 labels, center markers, row centerline, adjacent pitch arrows, optional full span P1-P4.

Output contract for other tools:

- `listP_Center[0..3]` maps to `P1..P4`.
- `rectRotates` may remain empty or contain small marker rects; consumers should use `listP_Center` for point selection.
- The selected pin center is stable in image/global coordinates, not crop-local coordinates.

## ToolPitch GUI Changes

Files:

- `BeeInterface/Tool/ToolPitch.cs`
- `BeeInterface/Tool/ToolPitch.Designer.cs`
- `BeeInterface/Tool/ToolPitch.resx` only if the designer requires it

Add controls:

- Mode segmented/toggle control: `PeakRoot` / `PinPitch`.
- Pin count numeric input, default `4`.
- Pitch axis buttons: Horizontal / Vertical.
- Nominal pitch numeric input.
- Pitch tolerance numeric input, default `0.05`.
- Projected pitch toggle.
- Debug overlay toggle.

GUI behavior:

- `PeakRoot` mode shows current crest/root pitch panels.
- `PinPitch` mode hides crest/root-specific controls and shows pin pitch controls.
- Existing preprocessing controls stay shared: threshold, edge method, morphology, noise cleanup, scale.
- On mode change, update `Propety.MeasureMode`, invalidate view, and mark owner tool waiting.
- On test/calib completion, update displayed pitch values:
  - `P12`
  - `P23`
  - `P34`
  - `P1-P4`
  - pin count found
  - worst deviation

Important UI rule:

- Do not add distance-to-line controls to `ToolPitch`.
- Any distance from pin center to line `L` belongs to `ToolWidth`.

## ToolWidth Core Changes

Files:

- `BeeCore/Unit/Width.cs`
- `BeeCore/Func/Engines/WidthEngineRunner.cs`
- `BeeCore/Func/Draws.cs` only if an existing perpendicular draw helper cannot be reused
- `BeeGlobal/Enums.cs`

Add enum:

```csharp
public enum WidthMeasureMode
{
    ParallelLines,
    PointToLine
}
```

Add serialized fields to `Width`:

```csharp
public WidthMeasureMode MeasureMode = WidthMeasureMode.ParallelLines;
public string PointSourceToolName;
public int PointSourceIndex;
public float PointToLineNominalMm;
public float PointToLineToleranceMm = 0.05f;
public LineOrientation ReferenceLineOrientation = LineOrientation.Horizontal;
public int ReferenceLineAngleRange = 10;
public LineDirScan ReferenceLineScan = LineDirScan.TopBot;
```

Add non-serialized/runtime fields:

```csharp
[NonSerialized] public BeeCpp.RansacLine RansacLine;
[NonSerialized] public BeeCpp.Line2DCli ReferenceLineL;
public System.Drawing.PointF PointToLineCenter;
public System.Drawing.PointF PointToLineFoot;
public bool PointToLineFound;
```

Runtime behavior:

1. In `SetModel()`, initialize `ParallelGapDetector` for current mode and `RansacLine` for `PointToLine`.
2. In `DoWork()`:
   - If `ParallelLines`, keep current behavior unchanged.
   - If `PointToLine`:
     - Resolve the source point from `PointSourceToolName` and `PointSourceIndex`.
     - Crop/process line ROI using the same preprocessing controls.
     - Call `RansacLine.FindBestLine` to detect `L`.
     - Transform `L` from crop-local to image/global coordinates when needed.
     - Compute foot point from selected point to `L`.
     - Set `WidthResult = distanceMm`.
3. In `Complete()`:
   - If `ParallelLines`, keep current scoring.
   - If `PointToLine`, score absolute deviation from `PointToLineNominalMm`.
   - Calibration should set `PointToLineNominalMm = WidthResult` when `IsCalibs` is true and not running.
4. In `DrawResult()`:
   - If `ParallelLines`, keep existing drawing.
   - If `PointToLine`, draw selected point, line `L`, foot point, perpendicular segment, and distance label.

Point resolution:

- Primary source: `TypeTool.Pitch` in `PinPitch` mode, using `listP_Center[0..3]` as P1..P4.
- Compatible fallback: any tool exposing `listP_Center` with enough points.
- If point source is missing, not found, or index out of range, mark NG and log a clear message.

Line `L` detection:

- Use existing `RansacLine.FindBestLine`.
- Do not duplicate RANSAC line fitting in Width.
- Keep `RansacLineCore` unchanged unless a missing generic helper is discovered.

## ToolWidth GUI Changes

Files:

- `BeeInterface/Tool/ToolWidth.cs`
- `BeeInterface/Tool/ToolWidth.Designer.cs`
- `BeeInterface/Tool/ToolWidth.resx` only if the designer requires it

Add controls:

- Mode segmented/toggle control: `ParallelLines` / `PointToLine`.
- Point source combo: list compatible tools.
- Point index combo: `P1`, `P2`, `P3`, `P4` for PinPitch; generic numeric labels for other tools.
- Nominal distance numeric input.
- Distance tolerance numeric input, default `0.05`.
- Reference line orientation buttons.
- Reference line scan direction buttons.
- Reference line angle range numeric input.

GUI behavior:

- `ParallelLines` mode shows existing width controls: max lines, gap extremum, segment stat, min/max length.
- `PointToLine` mode hides parallel-line-only controls and shows point/line controls.
- Shared preprocessing controls remain visible in both modes.
- Combo source should refresh on visible/load and after selected source changes.
- When source is a Pitch PinPitch tool, point index labels should be `P1..P4`.

## BeeCpp Changes

Files:

- `Pattern/VisionGeometryCore.h`
- `Pattern/VisionGeometryCore.cpp`
- `Pattern/PinPitchCore.h`
- `Pattern/PinPitchCore.cpp`
- `Pattern/PinPitchCli.h`
- `Pattern/PinPitchCli.cpp`
- `Pattern/Pattern.vcxproj`

Rules:

- `PinPitchCore` does not inherit from `PitchCore`.
- `PinPitchCli` is a thin wrapper and must not own algorithm logic.
- `RansacLineCore` remains the owner of line fitting.
- `VisionGeometryCore` holds reusable math only when at least two native modules need it.

Minimum helper functions:

```cpp
double DistancePointToLine(const cv::Point2f& p, const cv::Vec4f& line);
cv::Point2f ProjectPointToLine(const cv::Point2f& p, const cv::Vec4f& line);
std::vector<cv::Point2f> SortAlongAxis(std::vector<cv::Point2f> points, bool horizontal);
```

## Implementation Order

1. Add enums and state fields for Pitch/Width, preserving old default behavior.
2. Add `PinPitchCore/Cli` and project entries.
3. Wire Pitch `PinPitch` mode in core only; build.
4. Add Pitch GUI mode controls and overlay; build.
5. Add Width `PointToLine` mode in core only; build.
6. Add Width GUI mode controls and overlay; build.
7. Run repeated-image validation and tune thresholds.

## Verification

Build after each code batch:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal
```

Functional checks:

- Old Pitch `PeakRoot` mode still produces the same result on saved tools.
- Old Width `ParallelLines` mode still produces the same result on saved tools.
- New Pitch `PinPitch` finds exactly 4 pins on the sample image and labels them P1..P4 in order.
- New Width `PointToLine` can select P1..P4 from Pitch and measure to line L.
- Repeated captures meet the required repeatability before documenting `+-0.05mm` as achieved.
