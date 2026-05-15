# ColorArea Multi Color List Scan Plan

## Requirement Summary

Add a mode option to the ColorArea preview/check workflow:

- **Single mode** keeps the current behavior unchanged.
- **Multi mode** allows multiple independent color lists. Each list can contain its own Color Sample values and Extern Color values.
- Multi mode lets the user choose the scan direction: **X** or **Y**.
- The run result must return the pixel deviation for each scan slot, ordered by the selected scan direction.

The target area is the existing ColorArea flow that currently stores one `HSVs` list or one `RGBs` list, sends one merged range set into `BeeCpp.ColorArea`, builds one mask, counts non-zero pixels, and returns one `PixelResult`.

## Current Observations

- `BeeCore/Unit/ColorArea.cs` stores one active color template through `HSVs`, `RGBs`, `TypeColor`, and `Extraction`.
- `SetColor()` sends one merged HSV or RGB range set to `BeeCpp.ColorArea.SetTempHSV/SetTempRGB`.
- `CheckColor()` returns one pixel count through `pxRS` and keeps one processed mask in `matProcess`.
- `ColorAreaEngineRunner.ColorAreaRunResult` exposes one `PixelResult`.
- `Pattern/ColorArea.cpp` merges all configured color ranges into one output mask with `bitwise_or`.
- `ToolColorArea` is the main WinForms control that loads score, pixel threshold, color type, morphology, and sample-pick state.

## Key Questions To Resolve Before Implementation

- Pixel deviation is defined as `abs(PixelCount - PixelTemplate)` for each detected region. It does not include center/position offset along the scan axis.
- Define how to reject noise slots that are not real color profiles.
- Define the conflict rule when one scan slot contains pixels matching more than one color list.
- Define how Extern Color is selected per list and whether it must snapshot the external color at run time or at configuration time.
- Define the maximum practical number of lists for UI usability and runtime cost.

## Proposed Product Behavior

### Modes

`ColorAreaCheckMode`:

- `Single`: current model and result path. Existing recipes must load into this mode by default.
- `MultiList`: new model with a collection of color groups.

### Multi List Model

Each list item should hold:

- Stable `Id` and user-facing `Name`.
- `TypeColor` or a decision to inherit the tool-level `TypeColor`.
- Internal Color Sample values: HSV list or RGB list.
- Extern Color values or references.
- `Extraction` tolerance. Prefer per-list tolerance if operators need different color flexibility; otherwise inherit from tool-level tolerance for a smaller first version.
- Optional enable/disable flag.
- Optional expected pixel template value for calibration and deviation.

### Scan Direction

Add `ColorAreaScanDirection`:

- `X`: sort detected regions left-to-right by region center X, then by Y as a tie-breaker.
- `Y`: sort detected regions top-to-bottom by region center Y, then by X as a tie-breaker.

Sorting should be deterministic and based on connected-component bounding boxes or centroids after morphology, not on raw contour enumeration order.

### Result Model

Add a multi-result model rather than overloading `PixelResult`:

- `ListId`
- `ListIndex`
- `RegionIndex`
- `BoundingBox`
- `Center`
- `PixelCount`
- `TemplatePixel`
- `PixelDeviation = abs(PixelCount - PixelTemplate)`
- `Order`
- `Result`

For Single mode, keep `pxRS`, `PxTemp`, `ScoreResult`, and `PixelResult` compatible. Multi mode can populate a new list while still setting aggregate owner result for legacy displays.

## Proposed Architecture

### C# Core

Add serializable models in or near `BeeCore/Unit/ColorArea.cs`:

- `ColorAreaCheckMode`
- `ColorAreaScanDirection`
- `ColorAreaColorList`
- `ColorAreaRegionResult`
- `ColorAreaMultiRunResult`

Keep old `HSVs`, `RGBs`, `PxTemp`, and `Extraction` fields for backward compatibility. On load, if the new list is null or empty, initialize one default list from the old fields when entering Multi mode.

### Native BeeCpp Layer

Prefer adding new APIs instead of changing existing signatures:

- Keep `SetTempHSV`, `SetTempRGB`, and `Check()` unchanged for Single mode.
- Add a multi-list native path only if profiling shows the C# loop is too slow.

First implementation option:

- For each enabled list, call existing `SetTempHSV/SetTempRGB` with that list's samples.
- Call existing `Check()` to get a per-list mask.
- Run morphology and connected-component analysis in C#.
- Merge each list's ordered region results into a single ordered result set.

This preserves native interop risk and keeps the current unmanaged buffer ownership pattern unchanged.

Second implementation option:

- Add a native `CheckMulti` API returning per-list masks or a labeled image.
- Use this only if the first option is too slow or memory-heavy.

### Region Detection

After the mask is generated per list:

1. Apply the current morphology pipeline.
2. Convert to single-channel mask if needed.
3. Use connected components or contours to split regions.
4. Filter noise using existing clear-small/clear-big rules or a new per-region minimum area.
5. Sort by scan direction.
6. Compute `PixelDeviation = abs(PixelCount - PixelTemplate)` for each region.

### UI

`ToolColorArea` should add:

- Mode selector: Single / Multi.
- Scan direction selector: X / Y, visible or enabled only in Multi mode.
- Multi list editor with add, remove, duplicate, rename, enable, and select current list.
- Per-list Color Sample display.
- Per-list Extern Color display or binding.
- Preview map that overlays ordered region labels and per-region pixel deviation.

Keep Single mode visually close to the current UI to reduce operator retraining.

### Serialization And Compatibility

- Existing saved tools with no new fields must load as Single mode.
- Existing `HSVs`, `RGBs`, `PxTemp`, and `Extraction` must remain valid.
- Multi list data must be serializable with the existing recipe/tool persistence mechanism.
- If a Multi recipe is opened by older code, old fields may not represent the full setup; document that backward compatibility is one-way unless older versions are required.

## Risk Notes

- The term "Extern Color" may mean a live external configuration value rather than a sampled color. The plan must preserve that behavior once confirmed.
- Returning only pixel-count deviation can hide positional failures, but center/position offset is intentionally out of scope for this feature.
- Multiple lists may produce overlapping masks. The result needs a deterministic priority rule.
- UI complexity can grow quickly if every list owns every setting. Start with the minimum per-list settings needed by operators.
- Repeated native calls per list may be acceptable for small list counts but should be benchmarked with production image sizes.

## Verification Strategy

- Build only after code changes, not for this plan-only step.
- Add unit-level tests or small harness coverage for X/Y sorting with synthetic masks.
- Verify Single mode produces the same pixel count and result as before.
- Verify Multi mode with two lists returns stable scan-slot order and separate deviations.
- Verify old recipes load without null-reference errors and default to Single mode.
- Verify unmanaged buffers returned by `Check()` are still released through `FreeBuffer()` in `finally`.

## Open Decisions

- Whether Extern Color is copied into the list or referenced dynamically.
- Maximum list count and expected image size for performance validation.

## Decisions Applied

- Multi mode uses per-list `TypeColor` and `Extraction`.
- Multi mode builds physical scan slots from the combined enabled-list mask, sorts those slots by the selected scan direction, and compares `ColorLists[i]` only inside scan slot `i`.
- This catches swapped order: if the first physical slot contains white but the first expected list is blue, the first result uses the blue mask inside slot 1 and becomes low or zero instead of moving the white slot to another list.
- Owner aggregate result uses the maximum region pixel deviation divided by 10 as `ScoreResult`.
- Pixel deviation follows the active NG direction rule per region: `NG More` ignores under-template regions, `NG Less` ignores over-template regions, and no NG direction uses absolute deviation.
