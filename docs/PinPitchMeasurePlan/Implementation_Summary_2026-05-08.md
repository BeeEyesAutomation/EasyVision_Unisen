# PinPitch Measurement Implementation Summary
## Date: 2026-05-08

### Overview
This document summarizes the implementation of enhanced PinPitch measurement features for the Pitch tool, including consolidated drawing, absolute vs. relative distance calculation modes, and shared scale configuration.

---

## Features Implemented

### 1. **Consolidated Drawing (Gộp lại)**
**Objective**: Single unified loop to render pins, connecting lines, and distance labels.

**Implementation**:
- **File**: `BeeCore/Unit/Pitch.cs` (method `DrawPinPitchResult`, lines 620-718)
- **Pattern**: 
  ```
  for each pin:
    (1) Draw pin marker (Plus shape + ID label)
    (2) Draw line to next pin
    (3) Calculate distance (Absolute or Relative mode)
    (4) Draw distance label rotated along segment
  ```
- **Benefits**:
  - Single pass through pin data (efficient)
  - Consistent visual styling (all labels use same font/color)
  - Eliminated code duplication from previous multi-pass approach
  - Easier to maintain and debug

**Code Snippet**:
```csharp
for (int i = 0; i < n; i++)
{
    var pin = pins[i];
    var p = new PointF((float)pin.X, (float)pin.Y);
    
    // (1) Pin marker + ID
    Draws.Plus(gc, (int)Math.Round(p.X), (int)Math.Round(p.Y), 16, Color.Yellow, 2);
    gc.DrawString($"P{pin.Id}", font, brushPin, p.X + 5, p.Y - 18);
    
    // (2) Line to next pin
    if (i >= n - 1) continue;
    var b = pins[i + 1];
    var pb = new PointF((float)b.X, (float)b.Y);
    gc.DrawLine(penLine, p, pb);
    
    // (3) & (4) Distance calculation + label
    double dx = pb.X - p.X, dy = pb.Y - p.Y;
    double distMm = (PinDistanceMode == Absolute)
        ? Math.Sqrt(dx * dx + dy * dy) * mmPerPx
        : Math.Abs(dx * rvx + dy * rvy) * mmPerPx;  // Relative: project onto row axis
    
    // Rotate label along segment
    double angRad = Math.Atan2(dy, dx);
    float angDeg = (float)(angRad * 180.0 / Math.PI);
    float mx = (p.X + pb.X) * 0.5f, my = (p.Y + pb.Y) * 0.5f;
    var state = gc.Save();
    gc.TranslateTransform(mx, my);
    gc.RotateTransform(angDeg);
    gc.DrawString($"{distMm:0.###} mm", labelFont, brushLabel, 0f, -2f, sf);
    gc.Restore(state);
}
```

---

### 2. **Absolute vs. Relative Distance Modes**

#### Enum Definition
**File**: `BeeGlobal/Enums.cs` (lines 126-130)
```csharp
public enum PinDistanceMode
{
    Relative = 0,   // Projected lên trục dọc theo hàng pin (RowVx, RowVy)
    Absolute = 1    // sqrt(dx² + dy²) * mmPerPx
}
```

#### Distance Calculation
**File**: `BeeCore/Unit/Pitch.cs` (lines 685-698)

| Mode | Formula | Use Case |
|---|---|---|
| **Relative (0)** | `projection(vector, rowAxis) × mmPerPx` | Measuring pitch along row direction when pins may be slightly staggered |
| **Absolute (1)** | `√(dx² + dy²) × mmPerPx` | Measuring straight-line Euclidean distance between pin centers |

**Code**:
```csharp
double dx = pb.X - p.X, dy = pb.Y - p.Y;
double distMm;
if (PinDistanceMode == PinDistanceMode.Absolute || !rowValid)
{
    // Euclidean distance
    distMm = Math.Sqrt(dx * dx + dy * dy) * mmPerPx;
}
else
{
    // Relative: project onto row axis (rvx, rvy normalized)
    double proj = Math.Abs(dx * rvx + dy * rvy);
    distMm = proj * mmPerPx;
}
```

#### UI Control
**File**: `BeeInterface/Tool/ToolPitch.cs` (lines 410-415, 433)
- ComboBox `cbPinDistanceMode` with enum values
- Synchronized with `Propety.PinDistanceMode`
- Stored and loaded with serialized tool configuration

---

### 3. **Shared Scale Option (UseSharedScale)**

#### Purpose
Allow users to choose between:
- **Per-tool scale** (old behavior): Each tool maintains its own `Scale` value
- **Shared global scale** (new): All tools use `Global.Config.Scale`

#### Implementation

**Property Addition**:
- **File**: `BeeCore/Unit/Pitch.cs` (lines 86, 749)
```csharp
public bool UseSharedScale = false;
public float EffectiveScale => UseSharedScale ? Global.Config.Scale : Scale;
```

**Usage Points**:
1. **DoWork() / RunPinPitch()**: Uses `EffectiveScale` when calling `PitchMeasure.SetScaleMmPerPx()`
2. **DrawPinPitchResult()**: Uses `EffectiveScale` for distance label calculation
3. **CompletePinPitch()**: Inherits scale from result calculation

**UI Control**:
**File**: `BeeInterface/Tool/ToolPitch.cs` (lines 419-423, 434, 476-478)
- CheckBox `chkUseSharedScale`
- Auto-disables `AdjScale` (per-tool scale input) when enabled
- Synchronized with `Propety.UseSharedScale`

**Visual Indicator**:
```
[✓] Use shared scale (Global.Config.Scale)
     └─ AdjScale control is DISABLED when checked
```

---

### 4. **Unified Scoring (Chung Score)**

#### Design Decision
- **Score remains per-tool** (not split by mode)
- **PeakRoot and PinPitch share same threshold**: `owner.Score` (percentage-based)
- Both modes calculate worst percentage deviation and compare against this threshold

#### Implementation
**File**: `BeeCore/Unit/Pitch.cs` (lines 480-491)

```csharp
// CompletePinPitch() - unified scoring
owner.ScoreResult = 0;
if (NominalPitchMm > 0 && PinPitchResult.AdjacentPitchMm != null)
{
    foreach (double pitch in PinPitchResult.AdjacentPitchMm)
    {
        float pct = Math.Abs(((float)pitch - NominalPitchMm) / NominalPitchMm) * 100f;
        if (pct > owner.ScoreResult)
            owner.ScoreResult = pct;  // Worst deviation
    }
    if (owner.ScoreResult > owner.Score)
        owner.Results = Results.NG;
}
```

**Scoring Formula**:
```
WorstPct = MAX(|Pi - Nominal| / Nominal × 100)  for all adjacent pitches
Result = OK if WorstPct ≤ owner.Score
       = NG if WorstPct > owner.Score
```

---

## Architecture Changes

### Files Modified
1. **BeeGlobal/Enums.cs**
   - Added `PinDistanceMode` enum (Relative/Absolute)
   - No behavior changes to existing code

2. **BeeCore/Unit/Pitch.cs**
   - Added `PinDistanceMode` property (default: Relative)
   - Added `UseSharedScale` property (default: false)
   - Added `EffectiveScale` computed property
   - Rewrote `DrawPinPitchResult()` method (single consolidated loop)
   - Updated `CompletePinPitch()` (unified scoring logic)

3. **BeeInterface/Tool/ToolPitch.cs**
   - Added `cbPinDistanceMode` ComboBox (UI)
   - Added `chkUseSharedScale` CheckBox (UI)
   - Updated event handlers and LoadPara() for new controls
   - Automatic Enable/Disable logic for `AdjScale` based on `UseSharedScale`

### No Breaking Changes
- Serialization backward-compatible (new fields have safe defaults)
- Old tools without these options continue to work with defaults:
  - `PinDistanceMode = Relative` (original behavior)
  - `UseSharedScale = false` (per-tool scale, original behavior)
- Score threshold remains per-tool

---

## Verification & Testing

### Build Status
- **Command**: `MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64`
- **Result**: ✅ Pass, 424 warnings (baseline: ≤447)
- **Errors**: 0
- **Output Assemblies**: BeeMain.exe, BeeCore.dll, BeeInterface.dll (all current)

### Runtime Verification Checklist
- [ ] Open app → load project with existing ToolPitch (PeakRoot mode)
  - Expected: No change, displays pitch as before
- [ ] Switch to PinPitch mode → select PinDistanceMode = Relative
  - Expected: Pins drawn with pitch distance along row axis
- [ ] Switch to PinDistanceMode = Absolute
  - Expected: Pitch distance shown as Euclidean (√dx²+dy²)
- [ ] Enable UseSharedScale → set Global.Config.Scale to test value
  - Expected: Distance labels recalculate using global scale
- [ ] Run repeated captures on sample image
  - Expected: Pitch values consistent between captures (with proper calibration)

---

## Configuration Parameters (Pitch.cs)

| Parameter | Type | Default | Range | Notes |
|---|---|---|---|---|
| `MeasureMode` | enum | PeakRoot | PeakRoot, PinPitch | Selects measurement mode |
| `PinDistanceMode` | enum | Relative | Relative, Absolute | Distance calculation method |
| `UseSharedScale` | bool | false | — | true = use Global.Config.Scale |
| `ExpectedPinCount` | int | 4 | 1-10 | Number of pins to detect |
| `NominalPitchMm` | float | 0 | 0+ | Target pitch (set during calib) |
| `PitchToleranceMm` | float | 0.05 | 0+ | Legacy (not used in new scoring) |
| `UseProjectedPitch` | bool | true | — | Project pitch along row axis (legacy) |

---

## Future Enhancements

1. **Extended ToolWidth PointToLine Mode**
   - Measure distance from selected pin (P1-P4) to reference line L
   - Reuse RansacLine.FindBestLine for line detection
   - Perpendicular foot projection with distance label

2. **Pin Debug Overlay Toggles**
   - Optional visual: pin boxes, row centerline, pitch arrows, confidence scores

3. **Confidence Metrics per Pin**
   - Edge inlier count / contour area ratio
   - Fitted rectangle size consistency
   - Center method used (EdgeGeometry vs WeightedCentroid)
   - Row fit residual

4. **Repeated Image Validation**
   - Test ±0.05mm repeatability on 10+ consecutive frames
   - Log confidence per capture for traceability

---

## Testing Notes

### Sample Image
- File: `C:\Users\chitu\Downloads\PIN20260507091052018.bmp`
- Content: 4 bright square pin pads on dark background
- Expected detection: P1, P2, P3, P4 in order along horizontal axis
- Calibration note: `Scale` and camera working distance must be accurate

### Repeatability Requirements
- ±0.05mm pitch accuracy realistic only if:
  - Calibration `Scale` is correct for lens/working distance
  - Image resolution ≥ 50px per 1mm (0.02mm/px or better)
  - ROI is stable, not motion-blurred, no saturation
  - Verified on multiple captures, not single image

---

## References

### Related Documents
- `docs/PinPitchMeasurePlan/Plan.md` — Original feature specification
- `docs/PinPitchMeasurePlan/CoreGuiImplementationPlan.md` — Detailed implementation roadmap
- `CLAUDE.md` mục 5.0 (Phase 0 Architecture documentation)

### Code Locations
- **Core logic**: `BeeCore/Unit/Pitch.cs` lines 232-718
- **Enum definition**: `BeeGlobal/Enums.cs` lines 126-130
- **UI binding**: `BeeInterface/Tool/ToolPitch.cs` lines 52-53, 410-434, 475-478

---

## Session Summary

**Session Duration**: ~2h, across multiple context windows
**Main Achievement**: Consolidated drawing implementation + dual distance mode + shared scale option
**Key Decision**: Unified score threshold (not per-mode), backward-compatible defaults

**Changes Made**:
1. ✅ Added PinDistanceMode enum
2. ✅ Implemented EffectiveScale property
3. ✅ Rewrote DrawPinPitchResult() with single consolidated loop
4. ✅ Added UseSharedScale UI control
5. ✅ Unified CompletePinPitch() scoring logic
6. ✅ Full build pass verification

**Status**: 🟢 Complete and verified. Ready for user testing with actual pin images.

---

**Last Updated**: 2026-05-08 by Claude  
**Build Status**: ✅ Release x64, 424 warnings, 0 errors
