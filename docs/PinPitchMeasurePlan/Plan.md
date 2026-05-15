# Plan - Pin Pitch Measurement

## Goal

Add a pin pitch measurement workflow for the image style in `C:\Users\chitu\Downloads\PIN20260507091052018.bmp`:

- Detect four pin centers `P1..P4`.
- Measure pitch between adjacent pins and optionally full span `P1 -> P4`.
- Detect a reference straight line `L` and measure perpendicular distance from a selected pin center to `L` through an extended `ToolWidth` mode.
- Reuse `RansacLine.FindBestLine` for line `L`.
- Keep center detection accurate and repeatable enough for `+-0.05mm`, subject to camera calibration and image resolution.

## Implementation Status

> Updated 2026-05-08. See `pin-pitch-map.md` for full tracker.

| PP-ID | Status | Area |
|---|---|---|
| PP-001 | ✅ done | PinPitchCore/Cli native, VisionGeometryCore, Pattern.vcxproj |
| PP-002 | ✅ done | Width PointToLine mode (core + basic UI) |
| PP-003 | ✅ done | Width reuses RansacLine.FindBestLine for line L |
| PP-004 | 🔶 partial | Pitch PinPitch scoring, listP_Center, arrange mode |
| PP-005 | 🔶 partial | ToolPitch/ToolWidth UI panels (needs layout polish) |
| PP-006 | 🔲 open | Validation dataset, repeatability check |

Additional features implemented (2026-05-08):

- `DrawPinPitchResult()` consolidated single-loop drawing ✅
- `PinDistanceMode` enum (Relative / Absolute) ✅
- `UseSharedScale` / `EffectiveScale` ✅
- Unified scoring (worst deviation across adjacent pitches) ✅
- Edge-boundary mask + edge-geometry center (PP-007, fix runtime failures) ✅

## Observed Failures (2026-05-08 runtime)

Sample run on 4-pin row (4 metallic square pads, dark background, bright off-center reflections):

| # | Symptom | Root cause |
|---|---|---|
| 1 | All 4 centers wrong; rotated box angle wrong | `cv::minAreaRect` chạy trên contour của blob threshold → contour chỉ phủ vùng phản chiếu sáng + halo lệch một bên → bounding rectangle xoay theo halo, không theo pin. |
| 2 | Pins skewed in different directions không lock được | Hệ quả của (1): box angle sai → consumers (Width PointToLine) lấy sai trục. |
| 3 | Bóng mờ pin (halo / shadow) bị tính là pin | Background không thuần đen; threshold pixel sáng bao luôn cả halo soft-edge. `minSolidity=0.80` chỉ chặn halo lan, không chặn shadow gọn. |
| 4 | Bias về vùng phản chiếu sáng | Threshold dựa trên **intensity** → contour bị "kéo" theo bright spot. Phải dựa **gradient ở biên pin pad**, không phải pixel sáng. |

### Fix (PP-007)

Thêm 2 cờ runtime vào `PinPitchOptions`:

| Option | Mặc định | Tác dụng |
|---|---|---|
| `useEdgeBoundary` | `true` | Mask = Canny edges → close kernel → fill enclosed contours. Gradient sắc theo biên pin pad; bóng mờ có gradient yếu nên Canny tự loại. |
| `useEdgeGeometryCenter` | `true` | Center = midpoint của projection bounds (uMin+uMax)/2, (vMin+vMax)/2 trên 2 trục `minAreaRect`. Robust với pin xéo và bright spot off-center. |
| `edgeCannyLow`, `edgeCannyHigh` | 20, 60 | Ngưỡng Canny. Tăng nếu nhiễu nhỏ tạo edge giả; giảm nếu pin contrast thấp. |

Pipeline mới khi `useEdgeBoundary = true`:

1. GaussianBlur 5×5
2. Optional top-hat (tách background xám)
3. Canny(low, high) → edge image
4. MorphClose(`outlineClose`) → khép 4 cạnh pin
5. findContours RETR_EXTERNAL → fill drawContours → solid pin region
6. Optional dilate (kẹp ≤3 nếu `reduceDilateForOutline`)

Sau filter (area / aspect / fillRatio / solidity), `useEdgeGeometryCenter` recompute center từ projection bounds — không lệ thuộc vào blob centroid.

### Trial validation (PP-007 trial — `_run_trial.py`)

Chạy trên 6 ảnh trial (`10_pinrow_raw`, `13_pad_only`, `A_bg8`, `B_bg10`, `C_bg5`, `D_bg15`) so sánh OLD vs NEW vs EDGE:

| Quan sát | Kết quả |
|---|---|
| Y-coordinate sau projection | Đã verified; bug `cy=cx` chỉ tồn tại trong Python (đã fix). C++ `c.y + uC*ax.y + vC*ay.y` đúng. |
| Center shift về geom (4 ảnh tốt) | EDGE shift 1–3 px so với threshold-blob centroid → đúng hướng khử bias. |
| EDGE rejects noise | Trên `C_bg5`, OLD/NEW chấp nhận blob 256px (28×13) làm "P4"; EDGE loại đúng (Canny không có gradient mạnh ở blob này). |
| EDGE miss low-contrast pin | `10_pinrow_raw` (raw không preprocess) → EDGE bắt được 3/4 pin (pin trái nhất contrast yếu). Đây là lý do C++ cần **fallback**. |

### Fallback strategy (PinPitchCore.cpp, `measure()`)

Pseudocode:
```
mask     = BuildMask(gray, opts)              // edge-boundary if useEdgeBoundary
candidates = FindCandidates(gray, mask)
if (useEdgeBoundary && candidates.size < expectedCount) {
    // Retry với threshold mask để giữ recall
    fb_opts = opts; fb_opts.useEdgeBoundary = false
    fb_mask = BuildMask(gray, fb_opts)
    fb_cands = FindCandidates(gray, fb_mask, opts)   // vẫn áp useEdgeGeometryCenter
    if (fb_cands.size > candidates.size) {
        mask = fb_mask; candidates = fb_cands
    }
}
```

Khi fallback kích hoạt, edge-geometry-center vẫn chạy trên contour threshold → giảm bias bright-spot mặc dù mask đến từ threshold path. Trade-off: shadow rejection chỉ hoạt động trên primary path; nếu fallback dùng, shadow phải được lọc bằng `minSolidity`.

### Tuning từ trial

| Param | C++ default | Python trial best | Khuyến nghị |
|---|---|---|---|
| `edgeCannyLow`  | 20 | 10 | Để 20 default; giảm xuống 10–15 khi pin contrast yếu |
| `edgeCannyHigh` | 60 | 40 | Để 60 default; giảm xuống 40 cùng lúc với low |
| `outlineClose`  | 7 (cũ, dùng cả cho threshold) | 21 | Edge path tự dùng `max(7, outlineClose)`; tăng outlineClose lên ~13–21 nếu pin lớn |
| `outlineDilate` + `reduceDilateForOutline` | 5 + false | 3 + true | Khi `useEdgeBoundary` bật nên kẹp dilate ≤ 3 |

## Observed Failure Round 2 (2026-05-08 runtime)

Sau trial 1 (PP-007 v1: `useEdgeBoundary` + `useEdgeGeometryCenter`), screenshot 2 cho
thấy **green box quá nhỏ — chỉ phủ vùng phản chiếu sáng, không phủ full pin pad**. Pad
có contrast yếu với background (pad ~25 vs bg ~10) nên Canny global chỉ bắt được biên
sắc bên trong (bright-spot edges) thay vì biên ngoài của pad. Edge-geometry-center áp
trên contour bright-core → vẫn cho center của bright-core, không phải pad center.

### Fix v2 — Gradient refinement per-candidate (PP-007 v2)

Sau khi có candidates ban đầu (từ edge hoặc threshold path), với mỗi pin chạy
`RefineByGradient(gray, candidate, options)`:

1. Crop patch quanh seed box, mở rộng `gradientPatchMargin = 60` px
2. **CLAHE** (clipLimit=3.0, tile=8×8) → boost local contrast cho weak edges
3. **Sobel magnitude** (CV_32F sx², sy², `magnitude`) → normalize 0..255
4. Threshold magnitude tại `gradientThreshold = 25` → solid edge mask
5. Morph close kernel = `max(11, outlineClose)` → khép biên pad
6. `findContours RETR_EXTERNAL` → lấy contour **bao quanh seed** (`pointPolygonTest >= 0`) và area lớn nhất
7. Validate: area ≥ seed_area × 0.8 (refinement chỉ mở rộng), aspect/fillRatio pass filter, area ≤ maxAreaRatio
8. Update `box`, `areaPx`, `fillRatio`, `center` (qua `useEdgeGeometryCenter` nếu bật)

Nếu refinement fail validate → giữ box cũ (an toàn).

| Option mới | Default | Tác dụng |
|---|---|---|
| `useGradientRefinement` | `true` | Bật/tắt bước refinement |
| `gradientPatchMargin` | 60 | px mở rộng quanh seed box |
| `gradientThreshold` | 25 | ngưỡng Sobel mag (0..255) |
| `claheClipLimit` | 3.0 | CLAHE clip — tăng nếu pad rất tối |
| `claheTileSize` | 8 | CLAHE tile size (px) |

## Current Code Reading

- `BeeGlobal/Enums.cs` already has `TypeTool.Pitch = 20`.
- `BeeCore/Unit/Pitch.cs` and `BeeInterface/Tool/ToolPitch.cs` already implement a Pitch tool backed by `BeeCppCli.PitchCli`.
- `Pattern/PitchCore.*` currently detects crest/root peaks from a profile. That is not the right model for the four bright square pin pads in the sample image.
- `BeeCore/Unit/Width.cs` currently measures width as the gap between two detected parallel lines.
- `Pattern/RansacLineCore.*` and `Pattern/RansacLineCli.*` already expose `RansacLine.FindBestLine`, which should be reused for bottom line `L`.

Recommendation:

- Extend the existing Pitch tool with a new `PinPitch` mode for finding P1..P4 and measuring pitch between pins.
- Extend the existing Width tool with a new `PointToLine` mode for measuring from one pin center/point to one detected line `L`.
- Do not put point-to-line distance inside Pitch. It belongs in Width because the result is a distance/width value, not a pitch series.

## Proposed Runtime Model

Add a mode enum in managed code:

```csharp
public enum PitchMeasureMode
{
    PeakRoot,
    PinPitch
}
```

Add fields to `BeeCore/Unit/Pitch.cs`:

```csharp
public PitchMeasureMode MeasureMode = PitchMeasureMode.PeakRoot;
public int ExpectedPinCount = 4;
public LineOrientation PitchDirection = LineOrientation.Horizontal;
public float NominalPitchMm;
public float PitchToleranceMm = 0.05f;
public PinPitchCliResult PinPitchResult;
```

`PeakRoot` keeps current behavior. `PinPitch` uses the new detector and stores P1..P4 so other tools, especially Width `PointToLine`, can consume a selected pin center.

## Width Point-To-Line Model

Add a Width measurement mode:

```csharp
public enum WidthMeasureMode
{
    ParallelLines,
    PointToLine
}
```

Add fields to `BeeCore/Unit/Width.cs`:

```csharp
public WidthMeasureMode MeasureMode = WidthMeasureMode.ParallelLines;
public string PointSourceToolName;
public int PointSourceIndex; // P1..P4 or any supported point index
public Line2DCli ReferenceLineL;
public PointF PointToLineCenter;
public PointF PointToLineFoot;
public float PointToLineNominalMm;
public float PointToLineToleranceMm = 0.05f;
```

Behavior:

- `ParallelLines`: current Width behavior, measuring the gap between two detected lines.
- `PointToLine`: resolve one selected point, detect one line `L`, project the point onto `L`, and store the perpendicular distance in `WidthResult`.
- The visual result should draw the point, line `L`, foot point on `L`, and the perpendicular measurement segment. This is the one-point-to-one-line measurement workflow along the center axis.

## Native API Shape

Add BeeCpp native/core files:

- `Pattern/VisionGeometryCore.h`
- `Pattern/VisionGeometryCore.cpp`
- `Pattern/PinPitchCore.h`
- `Pattern/PinPitchCore.cpp`
- `Pattern/PinPitchCli.h`
- `Pattern/PinPitchCli.cpp`

Native structure:

- `RansacLineCore.*`: existing line fitting; keep it focused and reuse it from Width `PointToLine`.
- `PitchCore.*`: existing crest/root profile pitch; leave it unchanged for the current Pitch behavior.
- `PinPitchCore.*`: new detector for bright square pin pads; it owns only P1..P4 center detection and pin pitch calculations.
- `VisionGeometryCore.*`: optional shared math helpers when the helper code becomes reusable across PinPitch/Width/RANSAC.
- `PinPitchCli.*`: C++/CLI wrapper only; do not put detection logic in the wrapper.

Do not make `PinPitchCore` inherit from `PitchCore`. The current `PitchCore` is a profile peak/root algorithm, while pin pads need contour/edge geometry. Composition through shared helpers is lower risk than inheritance here.

### PinPitchCore config parameters (confirmed from trial)

```cpp
struct PinPitchOptions {
    bool   useOutlineCenter        = true;
    int    outlineThresholdOffset  = 14;    // bg + offset → threshold
    int    outlineClose            = 9;     // morphological close kernel
    int    outlineDilate           = 3;     // morphological dilate kernel
    float  minAreaPx               = 12.0f;
    float  maxAreaRatio            = 0.10f; // fraction of image area
    float  minAspect               = 0.45f;
    float  maxAspect               = 2.20f;
    float  minFillRatio            = 0.20f;
    int    expectedCount           = 4;
    // Top-hat option (for uneven/grey background)
    bool   useTopHat               = false;
    int    topHatKernelPx          = 0;     // 0 = auto (≥81, ~3/5 of min dim)
    // Solidity filter (key fix for halo/ring rejection)
    float  minSolidity             = 0.80f; // contourArea / convexHullArea
    bool   reduceDilateForOutline  = false; // limit dilate to min(d,3) when useTopHat
};
```

Suggested CLI result:

```cpp
public ref class PinCenterCli sealed {
public:
    int Id;
    double X;
    double Y;
    double Score;
    double AreaPx;
    double WidthPx;
    double HeightPx;
};

public ref class PinPitchCliResult sealed {
public:
    bool Found;
    String^ Message;
    array<PinCenterCli^>^ Pins;
    array<double>^ AdjacentPitchMm;
    double SpanP1P4Mm;
};
```

Keep ownership simple: C# passes `matProcess.Data` and receives managed arrays. If debug images are added later, follow the existing `PitchCliResult.DebugPtr` + `FreeBuffer` pattern.

## Pin Center Algorithm

### BuildMask — two threshold branches

**Branch 1 — `useOutlineCenter=True` (default, for dark background with bright pins):**

1. Gaussian blur (5×5).
2. Compute `bg = percentile_10(gray)` — estimates the dark background level.
3. `threshold = clamp(bg + outlineThresholdOffset, 0, 255)` — binary threshold above background.
4. Morphological close with `outlineClose` kernel (default 9) — fills small holes inside pins.
5. Dilate with `outlineDilate` kernel (default 3) — expands contour slightly for robust edge extraction.

**Branch 2 — `useTopHat=True` (for uneven / grey background):**

1. Gaussian blur (5×5).
2. Top-hat morphology with large elliptical kernel (`topHatKernelPx`, default auto ≥81) — removes slowly-varying background, leaves only bright structures.
3. Otsu threshold on the top-hat result.
4. Morphological close with `outlineClose` kernel.
5. Dilate with `min(outlineDilate, 3)` if `reduceDilateForOutline=True` — prevents over-inflation when top-hat already isolates the pins.

### BuildCandidate — filtering and ranking

For each external contour from the binary mask:

1. **Area filter**: reject if `area < minAreaPx` or `area > maxAreaRatio × W × H`.
2. **Aspect ratio filter**: fit `minAreaRect`; compute `aspect = max(w,h) / min(w,h)`; reject if outside `[minAspect, maxAspect]` = `[0.45, 2.20]`.
3. **Fill ratio filter**: `fill = contourArea / rectArea`; reject if `fill < minFillRatio` (0.20) — removes thin elongated streaks.
4. **Solidity filter** (`minSolidity > 0`): compute `solidity = contourArea / convexHullArea`; reject if `solidity < minSolidity` (default 0.80). This is the primary fix for halo/ring artifacts caused by over-dilation or specular reflections — OLD (minSolidity=0) let these through; NEW (minSolidity=0.80) rejects them.
5. **Score and rank**: keep top `expectedCount` candidates by descending score:

```
size_score   = sqrt(area)
aspect_score = 1.0 / aspect
fill_score   = clamp(fill, 0, 1)
score        = size_score × (0.65 × aspect_score + 0.35 × fill_score)
```

### Center estimation — two tiers

**Tier 1 (current implementation):** use `minAreaRect.center` directly from the winning contour. Trial validation showed this is sufficient when `minSolidity=0.80` already rejects off-center halo blobs. No additional edge fitting is needed for the standard 4-pin case.

**Tier 2 (future enhancement, higher precision):** if edge confidence is needed:
- Build a local ROI around the candidate.
- Extract edge points on the outer pin boundary.
- Fit four local edges using robust line fitting or use `minAreaRect` as starting estimate.
- Center = intersection of the two midlines between opposite edges.
- Use intensity-weighted centroid only as last fallback; lower confidence because specular glare can bias it.
- Report which method was used (`EdgeGeometry` vs `MinAreaRect` vs `WeightedCentroidFallback`).

### Sort and output

- **Horizontal mode**: sort by X → P1 (leftmost) .. P4 (rightmost).
- **Vertical mode**: sort by Y → P1 (top) .. P4 (bottom).
- **RowProjection mode**: fit row direction from the four candidates, project onto axis, sort by projection.
- Return `P1..P4` in sorted order as `listP_Center[0..3]` for consumption by Width `PointToLine`.

### Trial parameter ranges (from `trial/` folder)

Images in the trial folder are named `<case>_bg<N>_off<off>_close<close>_d<d>.png` where `bg<N>` is the measured percentile-10 background pixel value, and the other fields are the PinPitchOptions values used.

| Trial case | outlineThresholdOffset | outlineClose | outlineDilate | Result |
|---|---|---|---|---|
| A (`bg8_off10_close9_d3`) | 10 | 9 | 3 | Conservative; works when bg is very dark |
| B (`bg10_off14_close11_d3`) | 14 | 11 | 3 | Standard — recommended default |
| C (`bg5_off8_close13_d5`) | 8 | 13 | 5 | Aggressive close; wide dilate; risk of merging adjacent pins |
| D (`bg15_off12_close9_d1`) | 12 | 9 | 1 | Minimal dilate; clean bright pins |

The trial compared **OLD** (minSolidity=0), **NEW** (minSolidity=0.80, no top-hat), and **NEW+TH** (tophat+solidity+reduceDilate) on each image. Key finding: minSolidity was the single most impactful change, eliminating false positives on all tested cases.

## Reference Line L In Width

Line `L` should be detected by Width `PointToLine`, not by Pitch:

1. Create a bottom-line ROI or use the lower part of `rotArea`.
2. Run the same edge preprocessing pipeline as `Edge`.
3. Call `RansacLine.FindBestLine` with horizontal or angle-range filtering, `mmPerPixel: 1 / Scale`, and the configured scan mode.

Transform the returned local line back into image coordinates using the same matrix pattern used in `Edge` / `Edge2`.

## Measurements

For pin centers `P[i]` in Pitch:

- Adjacent pitch:
  - `Pitch12 = distance(P1, P2) * mmPerPixel`
  - `Pitch23 = distance(P2, P3) * mmPerPixel`
  - `Pitch34 = distance(P3, P4) * mmPerPixel`
- Full span:
  - `SpanP1P4 = distance(P1, P4) * mmPerPixel`

Distance mode is controlled by `PinDistanceMode`:
- **Relative**: project `P[i+1] - P[i]` onto the fitted row direction vector. More stable when pins are slightly staggered vertically.
- **Absolute**: Euclidean `sqrt(dx² + dy²)`. Simpler; correct when pins are perfectly aligned.

For Width `PointToLine`:

- Resolve selected point, for example `P1`, `P2`, `P3`, or `P4` from PinPitch.
- Detect line `L`.
- Compute foot point on `L`.
- `WidthResult = abs(a*x + b*y + c) / sqrt(a*a + b*b) * mmPerPixel`.
- If a signed result is useful later, keep it as an additional field, but PLC/output should use the absolute distance by default.

## Accuracy Requirements

`+-0.05mm` is realistic only if these are true:

- Calibration `Scale` is accurate for the current lens/working distance.
- Image resolution is high enough. For example, at `0.02mm/px`, `0.05mm` equals `2.5px`; at `0.05mm/px`, it equals only `1px`.
- The ROI is stable, not motion blurred, and exposure does not saturate the pin boundary.
- Repeatability is verified on multiple frames, not only one image.

Current center method (Tier 1: `minAreaRect.center`) is adequate for repeatability validation. Edge-geometry fitting (Tier 2) should be considered if Tier 1 does not reach the ±0.05mm target on real hardware.

Implementation should report confidence per pin:

- edge inlier count / contour area
- fitted rectangle size consistency
- center method used: `MinAreaRect`, `EdgeGeometry`, or `WeightedCentroidFallback`
- row residual after fitting P1..P4
- solidity value (useful for diagnosing halo rejection)

If confidence is low, return NG rather than outputting a precise-looking wrong value.

## UI Plan

In `ToolPitch` add a mode selector:

- `PeakRoot`: current UI and behavior.
- `PinPitch`: show only controls relevant to pin measurement.

Pitch PinPitch controls:

- expected pin count: default `4`
- pitch axis: horizontal / vertical / row-projection
- threshold mode: `useOutlineCenter` (default) / `useTopHat`
- `outlineThresholdOffset` (int, default 14) — only visible when `useOutlineCenter=True`
- `topHatKernelPx` (int, 0=auto, default 0) — only visible when `useTopHat=True`
- `outlineClose` (int, default 9)
- `outlineDilate` (int, default 3)
- `reduceDilateForOutline` (checkbox, default false) — enable when `useTopHat=True` to avoid over-dilation
- `minSolidity` (float 0.0–1.0, default 0.80) — solidity threshold for halo rejection
- nominal pitch and tolerance
- `PinDistanceMode`: Relative / Absolute
- `UseSharedScale` checkbox
- debug overlay toggles: centers, pin boxes, row line, pitch arrows

Pitch overlay:

- circle/cross at `P1..P4`
- labels `P1..P4`
- arrows for `P1-P2`, `P2-P3`, `P3-P4`, and optional `P1-P4`
- row centerline through P1..P4

In `ToolWidth`, add a measurement mode selector:

- `ParallelLines`: current two-line width mode.
- `PointToLine`: new one-point-to-one-line mode.

Width PointToLine controls:

- source point tool: PinPitch/Pitch first, then any compatible tool exposing `listP_Center`
- source point index: P1, P2, P3, P4
- reference line ROI/scan direction
- RANSAC threshold/iterations/angle range for L
- nominal distance and tolerance
- debug overlay toggles: point, foot, line L, perpendicular segment

Width PointToLine overlay:

- selected pin center
- line `L` in a separate color
- foot point projected onto `L`
- perpendicular segment from pin center to `L`
- distance label from `WidthResult`

## Implementation Order

1. ✅ Implement `PinPitchCore` and `PinPitchCli` with a standalone unit-testable detector. (PP-001)
2. ✅ Add files to `Pattern.vcxproj` and build Release x64. (PP-001)
3. ✅ Extend `BeeCore/Unit/Pitch.cs` with `PitchMeasureMode.PinPitch`, scoring, listP_Center output. (PP-004 partial)
4. ✅ Extend `BeeCore/Unit/Width.cs` with `WidthMeasureMode.PointToLine`. (PP-002)
5. ✅ In Width `PointToLine`, use `RansacLine.FindBestLine` for line `L`. (PP-003)
6. ✅ Add `DrawPinPitchResult()` consolidated loop, `PinDistanceMode`, `UseSharedScale`. (PP-004 / 2026-05-08)
7. 🔶 Add `ToolPitch` and `ToolWidth` UI mode binding and layout polish. (PP-005 partial)
8. 🔲 Validate against at least 10 repeated captures and log center/pitch/distance repeatability. (PP-006)
9. 🔲 If Tier 1 (minAreaRect) does not reach ±0.05mm, implement Tier 2 edge-geometry fitting.

## Stop-And-Ask Points

Stop before implementation if any of these are unclear:

- Whether PinPitch should be a new toolbox item or a mode inside existing Pitch.
- Whether `P1-P4` means adjacent pitches only, full span only, or both.
- Whether Width `PointToLine` should measure only one selected pin per Width tool, or support batch P1..P4 distances in one tool later.
- The exact `Scale` / mm-per-pixel calibration for the camera setup.
- Whether `minSolidity=0.80` is appropriate for the target hardware (adjust down if valid pins have concave contours due to focus blur).
