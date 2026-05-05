## 2026-04-23

- Moved `ToolPattern` color configuration into `Basic` tab, inserted after sample section and before score section.
- Added collapsible `6.Color` section hosted inside `ToolPattern` basic layout.
- Removed standalone `Color` tab from `ToolPattern`.
- Updated `LoadPara()` in `ToolPattern` to reload color UI state and compute options.
- Added compute options in `Extension`: `CPU`, `GPU`, `MultiThread`, and thread count.
- Extended `Patterns` model with color-check configuration and compute/thread settings.
- Kept rule for pattern color NG: if any detected object has `ValueColor > ScoreNG`, the tool result becomes `NG`.
- Fixed `ToolPattern` object drawing alignment by rendering detection boxes in local ROI transform space and only converting to absolute for NG color mask overlay.
- Fixed `6.Color` layout clipping by enabling auto-size for runtime color panel layout and increasing the designer host panel minimum height.
- Added `Color Mark` flow for `ToolPattern`: after color split on each detected object, the system extracts the largest connected color component as mark region, then calculates `ValueColor`/NG from that mark region.
- Added visual mark rectangle overlay for detected `Color Mark` region in `Pattern` draw result to make inspection/debug easier.
- Applied `Filter` pipeline before `ToolPattern` color result: `Clear Noise Small -> Close -> Open -> Clear Noise Big`, then compute Color Mark and NG from filtered mask.
- Fixed compile error in `ToolPattern` Color Mark extraction by replacing `labels == bestLabel` with `Cv2.Compare(labels, bestLabel, ..., CmpType.EQ)` for OpenCvSharp compatibility.
- Aligned `ToolPattern` color NG UX with `ToolColorArea`: the preview color strip itself is the NG color list, with `Get Color`, `Undo`, and `Clear` operating directly on that list; removed the extra text list UI.
- Split `ToolPattern` section `6.Color` into separate concerns: `Color Mask` for mask/extraction tuning and `List Color Set` for `Get Color`, preview-strip list, `Undo`, and `Clear`.
- Refactored `ToolPattern` into two fully independent color lists:
  `List Color Mask` for building the mark mask, and `List Color NG` for checking NG colors inside the detected mark.
- Added separate `Get / Undo / Clear / ExternColor` controls for both `List Color Mask` and `List Color NG`.
- Updated `Patterns.ApplyColorCheck()` pipeline to run: object crop -> mask color extraction -> largest mark region -> NG color extraction inside mark region.
