# Tool Measure Angle Line/Point Map

## ID Rules

- Prefix: `TM`
- Format: `TM-xxx`
- Next ID: `TM-005`
- Reuse existing IDs.
- Do not duplicate IDs.

## Status values: `open` | `wip` | `partial` | `done` | `blocked`
## Risk values: `low` | `medium` | `high`

## Entries

| ID | Status | Risk | Area | Files | Depends on | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| TM-001 | done | low | Measure line input model | `BeeGlobal/Enums.cs`, `BeeCore/Unit/Measure.cs` | - | Added `MeasureLineInputMode` plus serialized `Line1InputMode`, `Line2InputMode`, and `listLineChoose`. `EnsureSelectionLists()` initializes missing fields so old saved Measure tools default to Point mode. |
| TM-002 | done | medium | Runtime line resolver | `BeeCore/Unit/Measure.cs`, `BeeCore/Func/Engines/MeasureEngineRunner.cs` | TM-001 | Measure now resolves each angle line through a shared helper. Point mode uses two selected point outputs. Line mode accepts only `TypeTool.Edge`/`TypeTool.Edge2` and copies their detected line endpoints, preferring absolute `listP_Center` endpoints with `Line2DCli` fallback. Removed the previous name-based Edge inference path. |
| TM-003 | done | medium | Tool Measure UI mode controls | `BeeInterface/Tool/ToolMeasure.cs` | TM-001 | Added runtime UI mode combos in the Line 1/Line 2 headers. Point mode shows the existing point-pair combo workflow. Line mode filters source combo items to Edge/Edge2 tools and hides/disables the second point row for that line. |
| TM-004 | done | low | UI binding cleanup and verification | `BeeInterface/Tool/ToolMeasure.cs`, `BeeCore/Unit/Measure.cs` | TM-002, TM-003 | Restores selected modes/sources in `LoadPara`, refreshes preview endpoints on line source selection, fixes `cbMethord_SelectedIndexChanged` to use `cbMethord.SelectedItem`, and verifies Release x64 build pass with 429 warnings. |
