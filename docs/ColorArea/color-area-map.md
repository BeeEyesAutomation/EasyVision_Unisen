# ColorArea Multi List Map

Next ID: CA-014

| ID | Status | Phase | Risk | Depends on | Files | Summary |
| --- | --- | --- | --- | --- | --- | --- |
| CA-001 | done | vision | low | none | `BeeCore/Unit/ColorArea.cs`, `BeeCore/Func/Engines/ColorAreaEngineRunner.cs` | Add serializable mode, scan direction, multi-list config, and multi-region result models while preserving existing Single mode fields. |
| CA-002 | done | vision | medium | CA-001 | `BeeCore/Unit/ColorArea.cs` | Implement Multi mode execution by reusing the existing single-list native `Check()` path per enabled color list, then split masks into ordered region results. |
| CA-003 | done | ui | medium | CA-001 | `BeeInterface/Tool/ToolColorArea.cs`, `BeeInterface/Tool/ToolColorArea.Designer.cs` | Add Single/Multi mode controls, X/Y scan direction selector, and multi-list editing UI. |
| CA-004 | done | ui | medium | CA-002 | `BeeCore/Unit/ColorArea.cs`, `BeeInterface/Tool/ToolColorArea.cs` | Draw preview labels for ordered regions and show each region's pixel deviation in scan order. |
| CA-005 | done | vision | low | CA-002 | `BeeCore/Unit/ColorArea.cs`, `BeeCore/Func/Engines/ColorAreaEngineRunner.cs` | Add compatibility and result aggregation behavior so legacy owner score/result fields remain meaningful in Multi mode. |
| CA-006 | done | vision | medium | CA-003, CA-005 | `BeeCore/Unit/ColorArea.cs`, `BeeInterface/Tool/ToolColorArea.cs` | Validate old recipe loading, Single mode parity, Multi mode ordering, and buffer cleanup. |
| CA-007 | done | ui | low | CA-003 | `BeeInterface/Tool/ToolColorArea.cs` | Unify Tol/Temp/ColorType controls to shared AdjustBarEx; remove duplicated NumericUpDown in `layMultiColor`; show ListColor only in Multi mode. |
| CA-008 | done | ui | low | CA-007 | `BeeInterface/Tool/ToolColorArea.cs` | Toggle HSV/RGB without clearing colors; refresh params (Tol/Temp/ColorType/palette) on Single↔Multi mode switch. |
| CA-009 | done | vision | medium | CA-002 | `BeeCore/Unit/ColorArea.cs`, `BeeInterface/Tool/ToolColorArea.cs` | Reset native `ColorAreaPP` instance on mode switch and at start of Multi inspect to avoid cross-mode pollution. Add `PrimeNativeColors()` to load per-list samples on load/switch. |
| CA-010 | done | ui | low | CA-005 | `BeeInterface/Tool/ToolColorArea.cs`, `BeeCore/Unit/ColorArea.cs` | Set Sample (calibration) splits per mode: Single sets `PxTemp`, Multi sets `PixelTemplate` for each list (engine `CompleteMulti` already supported). Bottom corner shows total deviation instead of total detected pixels. |
| CA-011 | done | ui | medium | CA-007 | `BeeInterface/Tool/ToolColorArea.cs` | Fix `LoadPara()` triggering `AdjValueTemp_ValueChanged` / `trackPixel_ValueChanged` while syncing legacy values, which overwrote selected list's `PixelTemplate`/`Extraction` with legacy `PxTemp`/`Extraction`. Guard handlers with `_isSyncingColorAreaMultiUi`. |
| CA-012 | done | vision | medium | CA-002 | `BeeCore/Unit/ColorArea.cs` | Replace slot-based scan with per-list direct counting (`AddPerListResults`). Each list's filtered mask gives `PixelCount` via `Cv2.CountNonZero`, centroid via `Moments`. Avoids cross-color sticking from connected components on combined mask. Cache `_listMasksCache` for per-region overlay. ScoreResult = sum of all deviations. |
| CA-013 | done | vision | low | CA-012 | `BeeCore/Unit/ColorArea.cs` | Per-region OK/NG evaluation with overlay tinted by region status. Add `Label`, `OrderTemplate`, `CenterTemplate*` fields. Order mismatch vs `OrderTemplate` forces NG. Empty masks fall back to `CenterTemplate` (or index-based if not yet calibrated). |

## Implementation Guardrails

- Do not change existing `BeeCpp.ColorArea.Check()` or `SetTempHSV/SetTempRGB` signatures in the first implementation pass.
- Keep Single mode behavior byte-for-byte compatible wherever practical.
- Preserve unmanaged buffer ownership: every pointer returned from `Check()` must be released through `FreeBuffer()` in a `finally` block.
- Add native multi-list APIs only after the managed per-list loop is proven too slow.
- Keep ColorArea-specific edits inside the listed files unless a build error proves a shared type must be added elsewhere.
- Per-list scan rule (CA-012): each color list is evaluated independently — never merge masks for evaluation. Combined mask only kept for legacy `matProcess` field.

## Resume Point

- **Next ID**: CA-014 — open for new requirements.
- **Blocker**: none.
- **Notes**: Per-list scan + order check + position fallback all wired. UI controls unified to AdjustBarEx. Native state reset on every mode change and at Multi inspect start. PixelTemplate/OrderTemplate/CenterTemplate persisted via [Serializable] fields on `ColorAreaColorList`.
