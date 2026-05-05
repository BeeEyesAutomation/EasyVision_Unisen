# EasyVision_Unisen — Code Preview & Refactor Map

> Companion to `code_map.json`. JSON is for fast Codex parsing; this file is the narrative for humans and Codex when context is needed. Read together with `CLAUDE.md` section 0 (Hard Rules) before editing code.

Scan scope: **C# UI + Core layer + Tool layer + save mechanism**. Native C++ (`Pattern`, `BeeCV`, `BeeNativeOnnx`, `BeeNativeRCNN`, `OKNG`, `PylonCli`) is out of scope.

---

## 0. TL;DR (30-second read)

EasyVision_Unisen is a WinForms .NET 4.8 / x64 vision-inspection app with 24 inspection tools (24 `ToolXxx.cs` UserControls mapped to engine classes in `BeeCore/Unit/`; the Unit folder also holds a few helpers without dedicated UI: `OKNGAPI.cs`, `PaperEnhance.cs`, `PitchGdiPainter.cs`, `Positions.cs`). The actual architecture deviates significantly from what `CLAUDE.md` section 3 proposes:

- **`BeeCore` is a god-project** holding tool engines, algorithms, camera HAL, IO, EtherNetIP, AI, parameters — 50,567 LOC across 130+ files.
- **`BeeInterface` owns all tool UI** as 24 `UserControl`s (`ToolXxx.cs` + `.Designer.cs` ~83,000 LOC), NOT modal Forms as CLAUDE.md assumes. Each tool already embeds a `TabControl` with 2-4 tabs.
- **Save mechanism** uses `BinaryFormatter` + Base64 atomic write, persisting `List<List<PropetyTool>>` to `.prog` files. This is a **frozen format** — renaming any class/namespace/field in `BeeCore.Unit.*` or `PropetyTool` breaks loading of legacy files.
- **Custom controls already exist**: `RJButton`, `AdjustBarEx`, `CustomNumericEx`, `RoundedPanel`, `GradientTab`, `AutoFontLabel`, `StepProgressBar`, `TextBoxAuto`, `DbTableLayoutPanel`, `AdjustNumberPad` — reusable across every tool.
- **Resolver helpers are stable** (legacy Phase 1-2 work): `Common.TryGetTool`, `EnsureToolList`, etc. New code MUST NOT use `Common.PropetyTools[ip][it]`.

Your goals ("scientific reorganization, customizable tool UI, more tabs, class separation, no save/load breakage") are achievable **without** changing the save mechanism, **without** renaming namespaces — by (a) extracting logic out of UserControls, (b) standardizing tab layout, and (c) factoring repeated UI fragments into shared UserControls.

---

## 1. Project map

```
                 ┌──────────────────────┐
                 │      BeeMain         │  WinExe, ns=BeeIV2, asm=EasyVision
                 │  (Program.cs only)   │
                 └──────┬────────┬──────┘
                        ▼        ▼
              ┌──────────────┐  ┌──────────────────────────────┐
              │    BeeUi     │  │        BeeInterface          │
              │ Main forms   │  │ Tool UCs + UI shell + Forms  │
              └──────┬───────┘  └────────┬─────────────────────┘
                     │                   │
                     └────────┬──────────┘
                              ▼
                       ┌─────────────┐
                       │   BeeCore   │  God-project, 50k LOC
                       └──────┬──────┘
                              ▼
                       ┌─────────────┐         ┌─────────────────────┐
                       │  BeeGlobal  │ ─────▶  │ PLC_Communication   │
                       └─────────────┘         └─────────────────────┘

  Native (C++/CLI, out of scope for this document):
    Pattern (BeeCpp), BeeCV, BeeNativeOnnx, BeeNativeRCNN, OKNG, PylonCli
```

| Project | Type | C# LOC | Role |
|---|---|---|---|
| `BeeMain` | WinExe | small | Entry point, AssemblyName=EasyVision |
| `BeeUi` | Library | medium | Top-level Forms (`Main`, `FormLoad`, `ScanCCD`, `FormActive`, `ShowEraser`, `ForrmCCD`) + `EditTool` UC + `PCITriggerEngine` |
| `BeeInterface` | Library | very large | 24 ToolXxx UCs (`Tool/`) + UI shell (`Group/`, `DashBoard/`, `General/`, `PLC/`, `Steps/`, `ShapeEditing/`, `Custom/`) + ancillary Forms (Account, FormFlowChart, ForrmAlarm, FormWarning, GeneralSetting, FormChoose, FormReport, ...) |
| `BeeCore` | Library | ~50,567 | Tool engines (`Unit/`), Algorithm, Camera HAL, IO, EtherNetIP, AI native wrappers, ShapeEditing, ItemTool, **PropetyTool/ToolState/Common** (3 core files) |
| `BeeGlobal` | Library | medium | POCO config, enums, parameters, RectRotate, ProgNo, Comunication, Labels, Logs |
| `PLC_Communication` | Library | medium | PLC protocol drivers |
| `BeeUpdate` | WinExe | small | Auto-updater |
| `BeeTest` | Library | small | Test fixtures |

---

## 2. Tool layer — actual pattern (critical)

This is the crux of your question. Each tool has **3 entities** tightly coupled:

```
                              ┌────────────────────────────────────┐
                              │  BeeCore.PropetyTool   (wrapper)   │
                              │  [Serializable, ISerializable]     │
                              │  + dynamic Propety   (engine)      │
                              │  + dynamic Propety2  (optional)    │
                              │  + ToolState State                 │
                              └─────────────┬──────────────────────┘
                                            │
                          ┌─────────────────┴─────────────────┐
                          ▼                                   ▼
       ┌──────────────────────────┐           ┌──────────────────────────────┐
       │ BeeCore.Unit.Circle      │           │ BeeInterface.ToolCircle      │
       │ [Serializable]           │  ────▶    │ partial class : UserControl  │
       │ payload — fields stored  │           │ public Circle Propety {get;set}│
       │ in .prog                 │           │ private PropetyTool          │
       │ Mat matProcess,          │           │   _ownerTool (cache)         │
       │ RectRotate rotArea/...   │           │ LoadPara() reads OwnerTool   │
       └──────────────────────────┘           └──────────────────────────────┘
```

**Key observations:**

1. `ToolXxx` is a `UserControl` (NOT `Form`). It is hosted inside `ToolPage` / `MultiDockHost` in BeeInterface, never opened as a dialog.
2. Each `ToolXxx` is also `[Serializable]` — but during serialization only the `Propety` field (engine class) gets persisted into `.prog`. The UC itself is transient.
3. The cached `_ownerTool` is resolved via `Common.TryGetTool(Global.IndexProgChoose, Propety.Index)` — the mandatory resolver helper.
4. Each tool subscribes to `OwnerTool.StatusToolChanged` and `OwnerTool.ScoreChanged` using the `-=` then `+=` pair. Breaking that pattern leaks handlers.
5. **24/24 tools already use `TabControl`** with 2-4 `TabPage`s. The current tab structure is inconsistent across tools — this is the standardization target.

### 2.1 Grouping the 24 tools (aligned with `CLAUDE.md` section 10.5)

**Group A — fits the standard frame (12 tools):**
Circle, Width, Measure, Edge, EdgePixel, Corner (engine `MeasureCorner`), Crop, Intersect, ColorArea, Counter, CheckMissing, Position_Adjustment.

**Group B — fits but needs extra UserControls (7 tools):**
Pattern, MultiPattern, MatchingShape, Pitch, Barcode, OCR, CraftOCR.

**Group C — custom layout, defer (5 tools):**
Yolo, MultiOnnx, OKNG, AutoTrig, VisualMatch.

### 2.2 ToolXxx ⇒ engine ⇒ size mapping

| Tool UC | Engine class (BeeCore.Unit) | Tabs | `.cs` LOC | Designer LOC | Group |
|---|---|---:|---:|---:|:---:|
| ToolCircle | Circle | 3 | ~800 | 3354 | A |
| ToolWidth | Width | – | ~600 | 3202 | A |
| ToolMeasure | Measure | – | ~500 | – | A |
| ToolEdge | Edge | – | ~600 | 2733 | A |
| ToolEdgePixel | EdgePixel | – | – | 2538 | A |
| ToolCorner | MeasureCorner | – | – | 2215 | A |
| ToolCrop | Crop | – | – | – | A |
| ToolIntersect | Intersect | – | – | 3300 | A |
| ToolColorArea | ColorArea | – | – | 1855 | A |
| ToolCounter | Counter | – | 1359 | 3037 | A |
| ToolCheckMissing | CheckMissing | – | – | 3356 | A |
| ToolPosition_Adjustment | PositionAdj | 4 | – | 4323 | A |
| ToolPattern | Patterns | 4 | 1241 | 4920 | B |
| ToolMultiPattern | MultiPattern | – | – | 4093 | B |
| ToolMatchingShape | MatchingShape | – | – | – | B |
| ToolPitch | Pitch | 3 | – | 3646 | B |
| ToolBarcode | Barcode | 2 | – | 1206 | B |
| ToolOCR | OCR | – | – | 1360 | B |
| ToolCraftOCR | CraftOCR | – | – | 1991 | B |
| ToolYolo | Yolo | 4 | 2713 | 4869 | C |
| ToolMultiOnnx | MultiOnnx | – | – | 3608 | C |
| ToolOKNG | OKNG | – | – | 1369 | C |
| ToolAutoTrig | AutoTrig | – | – | 2666 | C |
| ToolVisualMatch | VisualMatch | – | – | 1982 | C |

Total Designer files: ~83,000 LOC. This is the largest reduction target via shared UserControls.

---

## 3. BeeCore — details (god-project)

### 3.1 Three pillars (compatibility MUST be preserved)

The three files below are the core; any refactor must respect their public API:

#### `Common.cs` (~22.7 KB)

Static service hub, holds:
- Tool registry: `static List<List<PropetyTool>> PropetyTools` (indexed `[indexProg][indexTool]`).
- Resolver helpers (already standardized): `TryGetTool`, `TryGetToolList`, `TryGetCurrentToolList`, `EnsureToolList`, `EnsureCurrentToolList`, `SetToolList`.
- Vision utilities: `TransformToolRect`, `GetPositionAdjustment`, `AutoCanny`, `CannyWithMorph`, `GetCrop`, `CropRotate`.
- Camera/runtime: `static List<Camera> listCamera` (4 slots), `IsLive`, `IsRun`, `IsDebug`, `FrameRate`, events `FrameChanged` / `PropertyChanged`.
- Python: `IniPython` / `ClosePython`.
- P/Invoke: `GetImage`, `GetImageCrop`, `GetImageResult`, `SetSrc`.
- Singletons: `Comunication Comunication`, `HSVCli HSVSample`, `RGBCli RGBSample`.

Touching this file: **only add helpers, never remove any public symbol.**

#### `PropetyTool.cs`

Wrapper persisted into `.prog`. Implements `ISerializable` manually with `GetObjectData` and a deserialization ctor. The **string keys** in serialization form a frozen contract:

```
"_disposed", "Propety", "<Propety2>k__BackingField",
"IndexCamera", "Name", "TypeTool", "IndexLogics", "UsedTool",
"IsSendResult", "IndexImgRegis", "_Score", "_Percent",
"Location", "CycleTime", "ScoreResult", "MinValue", "MaxValue",
"StepValue", "_StatusTool", "Results"
```

Renaming any field or any of these string keys breaks loading of legacy `.prog` files.

#### `ToolState.cs`

`[Serializable]`, holds runtime state plus events. Notes:
- `[NonSerialized] PropetyTool Owner` — back-pointer reattached after deserialize via `AttachStateOwner()`.
- Events: `ScoreChanged(float)`, `PercentChange(int)`, `StatusToolChanged(PropetyTool, StatusTool)`, `ToolDoneChanged(PropetyTool, StatusTool)`.
- `BackgroundWorker worker` runs the tool asynchronously (`DoWork`, `RunToolAsync`, `Complete` on `PropetyTool`).

### 3.2 BeeCore folder layout

| Folder | Role | `.cs` files |
|---|---|---:|
| (root) | Common, PropetyTool, ToolState, Checking, Vision, dataMat, Action, KeepLargestAuto, ValueRobot, LabelItem, AddTool, LocalTool, BitmapExtensions, MatrixExtension, Converts (dup), DB, LibreTranslateClient, CustomGui (dup) | 17 |
| `Algorithm/` | 15 pure-algorithm files: RansacCircleFitter (1101), Filters (1383), DetectIntersect (928), FilletCornerMeasure(2), Geometry2D, EdgePoints, Gap, ImagePreprocessPipeline, MonoSegmentation, LineDetector, RansacFitLine, InsertLine, Colors, FilterItem | 15 |
| `Camera/` | HEROJE (2215), ModuleSetting (2984), DeviceFindAndCom (894), USB, KEY_Send, ToolCfg, HardwareEnum, HJ_CRC32, About, various CB structs | 18 |
| `Core/` | HSV/RGB/HsvConvert/HelpMouseView | 4 |
| `Data/` | **Access (BinaryFormatter+Base64), LoadData, SaveData**, ClassProject (empty) | 4 |
| `EtherNetIP/` | EIPAdapter, EIPScanner, EDS, AssemblyHelper | 4 |
| `Func/` (namespace `BeeCore.Funtion` — typo) | 28 files: Camera (2451), Crop (1209), Draws (1681), MatHelper, MatMerger, ImageUtils, Native, NativeYolo, NativeRCNN, ResultItem, ResultMulti, ResultFilter, ResultItemHelper, Cal, Init, Logs, General, PolyOffset, Line2DTransform, RectRotateGapChecker, FilterRect, Converts (dup), CustomGui (dup), CodeSymbologyCliExtensions, ComboBoxExtensions, TarProgramHelper, CameraIOFast | 28 |
| `Items/` | ItemTool UC (840 LOC) | 2 |
| `Parameter/` | G struct (dup) | 1 |
| `ShapeEditing/` | Interfaces + state (DUPLICATE with BeeInterface/ShapeEditing) | 16 |
| `Unit/` | **24 engine classes** + a few helpers | 28 |

### 3.3 Duplicates to be aware of

| Symbol | Duplicate locations | Proposed action |
|---|---|---|
| `CustomGui.cs` | `BeeCore/CustomGui.cs`, `BeeCore/Func/CustomGui.cs`, `BeeInterface/CustomGui.cs` | CLAUDE.md P1.1 — merge into one |
| `Converts.cs` | `BeeCore/Converts.cs`, `BeeCore/Func/Converts.cs` | CLAUDE.md P1.2 — merge into one |
| `ShapeEditing/*` | `BeeCore/ShapeEditing/`, `BeeInterface/ShapeEditing/` | UI side stays in BeeInterface; abstractions belong in BeeCore.Vision |
| Global state | `BeeGlobal/Global.cs`, `BeeInterface/Global.cs`, `BeeCore/Parameter/G.cs`, `BeeUi/Global.cs` | CLAUDE.md P4.3 — split into 3 sources by concern |

---

## 4. BeeInterface — UI shell + tool UCs

### 4.1 Custom controls (existing) — REUSE for refactor

Inside `BeeInterface/Custom/`:

| Control | Base type | Purpose |
|---|---|---|
| `RJButton` | `Button` | Image+text rounded button with `ImageFitMode` (None/Contain/Cover/Fill/FitWidth/FitHeight). **Used by every tool.** |
| `AdjustBarEx` | `UserControl` | Linear slider with Min/Max/Step/Value display. **Used by every tool** (trackScore, trackThreshold, trackMinInlier, AdjScale, ...). |
| `AdjustNumberPad` | `UserControl` | Popup numeric keypad. |
| `AutoFontLabel` | `ScrollableControl` | Auto-fit-font label. |
| `CustomNumericEx` | `UserControl` | Custom NumericUpDown. |
| `DbTableLayoutPanel` (`TableLayoutPanel2`) | `TableLayoutPanel` | Designer-friendly layout. |
| `GradientTab` | `partial class` | Gradient tab control. |
| `RoundedPanel` | `Panel` | Rounded-corner panel. |
| `StepProgressBar` | `UserControl` | Step-based progress bar. |
| `TextBoxAuto` | `TextBox` | Autocomplete textbox. |
| `CollageRenderer` | helper | Collage layout (`enum CollageLayout`). |

These controls are existing **assets** for the refactor — no need to invent complex `RoiSelectorControl` / `ScoreBar` / `ResultTable` controls as CLAUDE.md proposes; you can extract them gradually from existing tool Designers.

### 4.2 UI shell (`Group/`, `General/`, `DashBoard/`, `Steps/`, `PLC/`, `GroupControl/`)

This is the outer chrome (header, sidebar, status, dashboard, wizard) — **not tools**:

- `Group/`: Header, EditBar, EditProg, EditRectRot, AddFilters, AddTool (dialog), BarRight (`Cameras` UC — class name differs from file), HideBar, InforBar, RegisterImgs, SimImgs, StatusBar, ToolSettings, View, ViewHost, BtnHeaderBar, ucReport.
- `General/`: ToolPage (host UC for tool), Account form, FormCheckUpdate, FormChoose, FormReport, InforGroup, ItemLogic, ItemRS, MessageChoose, QuickSetting.
- `DashBoard/`: DashboardImages, DashboardListCompact, FlipClockControl, RegisterImgDashboard, ReportDashBoard, SaveProgressDialog, StatusDashboard.
- `Steps/`: SettingStep1, SettingStep2, SettingStep4 (note: Step3 missing).
- `PLC/`: ProtocolPLC, ucBitInput, ucBitOutput, ucValueOutput.
- `GroupControl/`: OK_Cancel.
- `Comunications/` (typo): WriteValuePLCSystems.

### 4.3 BeeInterface root files

`AppRestart`, `ControlStylePersistence`, `ConvertImg`, `CustomGui` (dup), `DataTool`, `FileName`, `FormFlowChart`, `FormWarning`, `ForrmAlarm` (typo), `FrameRenderer`, `GeneralSetting`, `Global`, `GlobalIconManager`, `ItemValue`, `LayoutPersistence`, `Load`, `MultiDockHost` (Panel + DockSide enum), `ShowTool`, `StepEdit`, `Tools`.

---

## 5. Save mechanism (DO NOT BREAK)

### 5.1 Pipeline

```
SaveData.Program(string Project, List<List<PropetyTool>> Prog, bool IsBk=false)
    ↓
Access.SaveProg(path, list)
    ↓
SerializeToBase64<List<List<PropetyTool>>>(list)
    ↓ BinaryFormatter.Serialize → MemoryStream → Convert.ToBase64String
AtomicWriteAllText(path, base64, UTF8)
    ↓ write .tmp → File.Replace(.tmp, path, .bak) → delete .bak
```

```
LoadData.Project(string Project)
    ↓
Access.LoadProg(path)
    ↓
File.ReadAllText(path, UTF8) → Convert.FromBase64String → BinaryFormatter.Deserialize<List<List<PropetyTool>>>
```

The `.prog` path is selected by the `Global.Config.IsSaveProg` flag:
- `true`: `Common\Common.prog` (shared across all projects)
- `false`: `Program\<Project>\<Project>.prog`

### 5.2 Other persisted files (same Base64 binary mechanism)

| Extension | C# type | Save / load helpers |
|---|---|---|
| `.prog` | `List<List<PropetyTool>>` | `SaveData.Program` / `LoadData.Project` |
| `.para` | `ParaCommon` | `SaveData.ParaPJ` / `LoadData.Para` |
| `.cam` | `List<ParaCamera>` | `SaveData.Camera` / `LoadData.ParaCamera` |
| `.com` | `Comunication` | `SaveData.Comunication` / `LoadData.Comunication` |
| `.gc` | `ParaShow` | `SaveData.ParaShow` / `LoadData.ParaShow` |
| `.img` | `List<ItemRegsImg>` register | `SaveData.ListImgRegister` / `LoadData.listImgRegister` |
| `.sim` | `List<ItemRegsImg>` simulation | `SaveData.ListImgSim` / `LoadData.listImgSim` |
| `.config` | `Config` | `SaveData.Config` / `LoadData.Config` |
| `.no` | `List<ProgNo>` | `SaveData.ProgNo` / `LoadData.ProgNo` |

### 5.3 Invariants required to keep loading legacy files

1. **Do NOT rename classes** in `BeeCore.Unit.*` (Circle, Patterns, Yolo, Edge, ...). Types are looked up by **assembly-qualified name** in the BinaryFormatter stream.
2. **Do NOT change the `BeeCore` namespace** for any persisted `[Serializable]` class (Unit/* + PropetyTool + ToolState + RectRotate in BeeGlobal, ...). If you absolutely must, install a custom `SerializationBinder` mapping old types to new types.
3. **Do NOT remove or rename existing fields** in those `[Serializable]` classes — `BinaryFormatter` persists field names by default. You MAY **add** new fields (they get default values when loading legacy files because BF tolerates missing members when annotated — safest is `[OptionalField]`).
4. **Do NOT change the 20 string keys** in `PropetyTool.GetObjectData()` (see section 3.1).
5. When splitting projects, you MUST add `[assembly: TypeForwardedTo(typeof(...))]` to keep the legacy assembly-qualified names valid.

---

## 6. Current state vs your goals

You stated 5 goals:

| Goal | Status | Shortest path |
|---|---|---|
| (a) Scientifically reorganize code | CLAUDE.md proposes 7 sub-projects. May be excessive — extracting logic out of UserControls is enough so each tool = 3 files (Engine + UC + Designer) instead of 2 huge UC files | Phase 3 section 5.3 with a lighter variant (no project split, just a `Tool.Xxx/` folder inside `BeeInterface`) |
| (b) More customizable tool UI | 24/24 tools already use TabControl. Custom controls already exist (RJButton, AdjustBarEx, ...). Standardizing tabs and extracting shared UCs is sufficient | Add `IToolView` + `ToolViewBase : UserControl` with the standard tab frame |
| (c) Add tabs / features per tool | Today tabs are hard-coded in Designer. Need a way for a tool to **declare** tabs dynamically | Add `ToolTabRegistry` so tools can extend tabs at runtime |
| (d) Refine tab structure per tool | Currently inconsistent (2/3/4 tabs depending on tool) | Adopt the standard 4-area tab layout: **General / ROI / Params / Result** + optional tabs |
| (e) Cleaner class boundaries | Heavy interleaving in `BeeCore` root + `Func/` + duplicates. CLAUDE.md already has a roadmap | Phase 1.3 + Phase 2 |
| (f) **Do NOT change persisted class layout** | Core persisted types: `BeeCore.Unit.*` + `PropetyTool` + `ToolState` + `RectRotate` | Follow section 5.3 above |

Conclusion: you do **not** need to walk through all 6 phases of CLAUDE.md to reach your goals. A trimmed phase (call it Phase 3-Lite) focused on tool UI + logic extraction + further customization is sufficient.

---

## 7. Proposed task cards (P3-Lite)

See `docs/architecture/preview/TASK_CARDS_P3_LITE.md` (sibling file). Overview:

| ID | Name | Effort | Output |
|---|---|---|---|
| P3L.0 | Create `IToolView` + `ToolViewBase` + `ToolTabRegistry` in `BeeInterface/Tool/_Base/` | 1 day | 4 base files |
| P3L.1 | Extract 3 shared UCs from existing Designers: `RoiToolbar`, `ScoreThresholdBar`, `ResultMiniGrid` | 1 day | 3 new UCs in `BeeInterface/Custom/` |
| P3L.2 | Pilot ToolCircle: extract `CircleEngineRunner` (Run/Save/Load logic) out of `ToolCircle.cs`. Engine `Circle` (BeeCore.Unit) is UNTOUCHED | 1.5 days | 1 new file `BeeCore/Func/Engines/CircleEngineRunner.cs`; ToolCircle slimmed |
| P3L.3 | Pilot ToolWidth: replicate P3L.2 pattern | 1 day | same shape |
| P3L.4 | Standardize tabs for Group A (10 remaining tools): apply the fixed 4-tab frame (General/ROI/Params/Result) + optional dynamic tabs | 1.5 weeks | 10 commits |
| P3L.5 | Group B: add preset/preprocess tabs (Pattern/MultiPattern/Pitch/Barcode/OCR/CraftOCR/MatchingShape) | 1.5 weeks | 7 commits |
| P3L.6 | Merge `CustomGui` + `Converts` + `ShapeEditing` duplicates (same as CLAUDE.md P1.1, P1.2) | 0.5 week | cleanup |
| P3L.7 | (Optional) Move 12 Group A tools into `BeeInterface.Tool.<Xxx>` namespaces — engine `BeeCore.Unit.*` is UNTOUCHED | 0.5 week | tighter namespaces |

P3L.7 is the **only** step that changes namespaces, and it only touches the UC layer (which is not persisted to `.prog`), so it is safe.

---

## 8. Standard tab layout for Tool UCs

Looking at `ToolCircle.Designer.cs` (3 tabs), `ToolPattern.Designer.cs` (4 tabs), `ToolPitch.Designer.cs` (3 tabs), the proposed layout is a fixed 4-tab frame plus dynamic tabs:

```
┌──────────────────────────────────────────────────────────────────┐
│  ┌───────┬───────┬────────┬────────┬─────────┬──────┬─────────┐  │
│  │General│ ROI   │ Params │Result  │ Preset  │ Logs │ Custom… │  │
│  └───────┴───────┴────────┴────────┴─────────┴──────┴─────────┘  │
│                                                                   │
│  ┌──────────────────────────────────────────────────────────┐    │
│  │              Active tab content                          │    │
│  │   (TableLayoutPanel2 → RJButton/AdjustBarEx/labels)      │    │
│  └──────────────────────────────────────────────────────────┘    │
│                                                                   │
│  ┌──────────────────────────────────────────────────────────┐    │
│  │  Footer: Score bar + Status + Apply/Test/Reset           │    │
│  └──────────────────────────────────────────────────────────┘    │
└──────────────────────────────────────────────────────────────────┘
```

| Tab | Mandatory? | Content |
|---|---|---|
| **General** | Yes | Tool name, Enable, IndexCCD, IndexImgRegis, MinValue/MaxValue/StepValue, IsSendResult, IndexLogics |
| **ROI** | Yes | Draw rotArea / rotMask / rotCheck / rotCrop; buttons btnRect/Elip/Polygon/Hexagon/Mask/Crop/Area; PositionAdjustment combo |
| **Params** | Yes | Tool-specific parameters (Threshold, Iterations, MinInliers, ...) using the standard `AdjustBarEx` |
| **Result** | Yes | Result table + Score bar (Min/Max threshold) |
| **Preset** | Optional | Group B tools only (Pattern/OCR/Pitch/Barcode) |
| **Logs** | Optional | For multi-step tools (Pattern/Pitch/Yolo) |
| **Custom...** | Optional | Tool registers extra tabs via `ToolTabRegistry.Register(toolKind, tabBuilder)` |

Convention: the first 4 tabs are **constants** in `ToolViewBase`. Subsequent tabs come from `ToolTabRegistry` keyed by `Propety.GetType().Name`. The tool's Designer only builds the Params tab via override `BuildParamsTab()`.

---

## 9. 'Do not break save/load' checklist

When editing tool-layer code:

```
[ ] DO NOT rename classes in BeeCore.Unit.* (Circle, Patterns, Yolo, Edge, EdgePixel, Width, Measure, MeasureCorner, Crop, Intersect, ColorArea, Counter, CheckMissing, PositionAdj, Pitch, MultiPattern, MatchingShape, Barcode, OCR, CraftOCR, MultiOnnx, OKNG, AutoTrig, VisualMatch, PaperEnhance, Positions)
[ ] DO NOT change the 'BeeCore' namespace of those classes
[ ] DO NOT delete existing fields in those classes — only add new fields with [OptionalField]
[ ] DO NOT rename / delete fields in PropetyTool, ToolState, RectRotate, ParaCommon, ParaCamera, Comunication, ParaShow, ItemRegsImg, ProgNo, Config
[ ] DO NOT change the 20 string keys in PropetyTool.GetObjectData (section 5.3)
[ ] DO NOT change how Access.cs serializes (BinaryFormatter+Base64)
[ ] When splitting a project: add [assembly: TypeForwardedTo(typeof(...))]
[ ] After every commit: load a legacy .prog file and run the smoke test
```

---

## 10. Safe defaults for Codex on ad-hoc tasks

Placed at the end so Codex always reads the last 5 lines when uncertain:

1. If the file is in `BeeCore/Unit/*` → only add fields; never rename or change types of existing fields.
2. If the file is `BeeInterface/Tool/ToolXxx.cs` → extract logic into `BeeCore.Funtion.Engines.XxxEngineRunner`; do NOT touch the engine class in Unit.
3. If you see `Common.PropetyTools[i][j]` → replace with `Common.TryGetTool(i, j)`.
4. If subscribing `event +=` → it must have a paired `event -=` with the same handler immediately above.
5. If you must change a UI namespace (`BeeInterface.ToolCircle` → `BeeInterface.Tool.Circle.View`) → OK; engine namespace (`BeeCore.Circle`) → never touch.
