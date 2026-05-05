# Task Cards — Phase 3 Lite (Tool UI Refactor)

> A trimmed task-card set targeting the goal "customizable tool UI + more tabs + class separation + DO NOT break save/load". Cards follow the template in `CLAUDE.md` section 11.1 and obey the Hard Rules in section 0. One card = one commit.
>
> **Mandatory order**: P3L.0 → P3L.1 → P3L.2 (pilot) → P3L.3 (pilot 2) → P3L.4 → P3L.5 → P3L.6 → P3L.7.
>
> Stop-and-ask after pilot P3L.2 if effort exceeds 2 days.

---

## P3L.0 — Create ToolViewBase + ToolTabRegistry

**Goal**: provide a standard tab frame that all 24 ToolXxx UCs can inherit, without introducing new singletons in the UI layer.

**Preconditions**:
- [ ] Read `CODE_PREVIEW.md` sections 2 and 8
- [ ] Read `CLAUDE.md` section 0 + section 5.3 (Phase 3) for the original CLAUDE.md frame
- [ ] `git status` is clean or user has confirmed
- [ ] Baseline build passes

**In-scope** — CREATE new:
- `BeeInterface/Tool/_Base/IToolView.cs`
- `BeeInterface/Tool/_Base/ToolViewBase.cs`
- `BeeInterface/Tool/_Base/ToolViewBase.Designer.cs`
- `BeeInterface/Tool/_Base/ToolTabRegistry.cs`
- `BeeInterface/Tool/_Base/ToolTabContext.cs`

**Out-of-scope**:
- Any existing `ToolXxx.cs` — DO NOT touch
- Any engine class in `BeeCore/Unit/*`
- Any file outside `BeeInterface/Tool/_Base/`

**Steps**:

1. Create the folder `BeeInterface/Tool/_Base/`.

2. Code `IToolView.cs`:
   ```csharp
   namespace BeeInterface.Tool._Base
   {
       public interface IToolView
       {
           // Engine payload (Circle, Patterns, ... in BeeCore.Unit) — not generic, must stay compatible with dynamic Propety
           object Propety { get; set; }
           // Resolver to the current PropetyTool wrapper
           BeeCore.PropetyTool OwnerTool { get; }
           // Hook: UI applies new values
           void LoadPara();
           // Hook: tab switched
           void OnTabChanged(string tabKey);
           // Tool kind used to look up the ToolTabRegistry
           string ToolKind { get; }
       }
   }
   ```

3. Code `ToolTabContext.cs` — args passed to a tab builder:
   ```csharp
   namespace BeeInterface.Tool._Base
   {
       public sealed class ToolTabContext
       {
           public IToolView View { get; }
           public BeeCore.PropetyTool OwnerTool { get; }
           public object Propety { get; }
           public ToolTabContext(IToolView view) { View = view; OwnerTool = view.OwnerTool; Propety = view.Propety; }
       }
   }
   ```

4. Code `ToolTabRegistry.cs` — registry pattern:
   ```csharp
   using System;
   using System.Collections.Generic;
   using System.Windows.Forms;

   namespace BeeInterface.Tool._Base
   {
       public static class ToolTabRegistry
       {
           private static readonly Dictionary<string, List<TabEntry>> _entries = new();

           public sealed class TabEntry
           {
               public string TabKey;
               public string DisplayName;
               public Func<ToolTabContext, Control> Build;
           }

           public static void Register(string toolKind, string tabKey, string displayName, Func<ToolTabContext, Control> build)
           {
               if (!_entries.TryGetValue(toolKind, out var list))
                   _entries[toolKind] = list = new List<TabEntry>();
               // De-duplicate
               list.RemoveAll(e => e.TabKey == tabKey);
               list.Add(new TabEntry { TabKey = tabKey, DisplayName = displayName, Build = build });
           }

           public static IReadOnlyList<TabEntry> Get(string toolKind)
               => _entries.TryGetValue(toolKind, out var list) ? list.AsReadOnly() : Array.Empty<TabEntry>();
       }
   }
   ```

5. Code `ToolViewBase.cs` — abstract UC, partial for the Designer:
   ```csharp
   using System.ComponentModel;
   using System.Windows.Forms;

   namespace BeeInterface.Tool._Base
   {
       [Serializable]
       public partial class ToolViewBase : UserControl, IToolView
       {
           // Cache OwnerTool (matches the existing pattern in ToolCircle)
           private BeeCore.PropetyTool _ownerTool;
           public BeeCore.PropetyTool OwnerTool
           {
               get
               {
                   if (_ownerTool == null && Propety != null)
                   {
                       int index = (int)((dynamic)Propety).Index;
                       _ownerTool = BeeCore.Common.TryGetTool(BeeGlobal.Global.IndexProgChoose, index);
                   }
                   return _ownerTool;
               }
           }
           protected void InvalidateOwnerToolCache() => _ownerTool = null;

           // Engine — concrete tool assigns in its ctor
           public virtual object Propety { get; set; }

           // ToolKind — concrete tool overrides to match the registry. Default = engine class name.
           public virtual string ToolKind => Propety?.GetType().Name ?? GetType().Name;

           // Hooks — concrete override
           public virtual void LoadPara() { }
           public virtual void OnTabChanged(string tabKey) { }

           // Fixed 4-tab frame + dynamic tabs — Designer.cs only builds the skeleton
           protected TabControl tabRoot;
           protected TabPage tabGeneral;
           protected TabPage tabRoi;
           protected TabPage tabParams;
           protected TabPage tabResult;

           public ToolViewBase()
           {
               InitializeComponent();
               // Wire dynamic tabs (lazy)
               this.HandleCreated -= OnHandleCreated_BuildDynamicTabs;
               this.HandleCreated += OnHandleCreated_BuildDynamicTabs;
               if (tabRoot != null)
               {
                   tabRoot.SelectedIndexChanged -= OnTabRootSelectedChanged;
                   tabRoot.SelectedIndexChanged += OnTabRootSelectedChanged;
               }
           }

           private void OnHandleCreated_BuildDynamicTabs(object sender, System.EventArgs e)
           {
               if (Propety == null || tabRoot == null) return;
               var ctx = new ToolTabContext(this);
               foreach (var entry in ToolTabRegistry.Get(ToolKind))
               {
                   var page = new TabPage(entry.DisplayName) { Tag = entry.TabKey };
                   var content = entry.Build(ctx);
                   if (content != null) { content.Dock = DockStyle.Fill; page.Controls.Add(content); }
                   tabRoot.TabPages.Add(page);
               }
           }

           private void OnTabRootSelectedChanged(object sender, System.EventArgs e)
           {
               var tab = tabRoot.SelectedTab;
               if (tab == null) return;
               OnTabChanged(tab.Tag?.ToString() ?? tab.Name);
           }
       }
   }
   ```

6. Code `ToolViewBase.Designer.cs` — only build `tabRoot` with 4 empty TabPages (Dock=Fill). (Long file; do this in VS Designer.)

7. Update `BeeInterface/BeeInterface.csproj`: add the 5 files to `<Compile Include>`.

**Verify**:
| Command | Expected |
|---|---|
| `MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` | warnings ≤ baseline |
| `bash tools/check_propety_tools.sh` | exit 0 |
| Open `ToolViewBase.cs` in VS Designer | shows 4 empty tabs |
| Smoke: app builds and runs | no exceptions |

**Rollback**: `git reset --hard HEAD~1`.

**DoD**:
- [ ] 5 new files committed
- [ ] Build passes
- [ ] Designer renders OK
- [ ] `CODEX_HISTORY.md` appended with section P3L.0
- [ ] Commit message: `[P3L.0] Add ToolViewBase + ToolTabRegistry framework`

---

## P3L.1 — Extract 3 shared UCs from existing Designers

**Goal**: extract 3 control clusters that repeat across multiple `ToolXxx.Designer.cs` into reusable UCs and shrink the Designer footprint.

**Preconditions**:
- [ ] P3L.0 done
- [ ] Baseline build passes

**In-scope** — CREATE new:
- `BeeInterface/Custom/RoiToolbar.cs` (+ Designer + resx)
- `BeeInterface/Custom/ScoreThresholdBar.cs` (+ Designer + resx)
- `BeeInterface/Custom/ResultMiniGrid.cs` (+ Designer + resx)

**Out-of-scope**:
- Editing existing ToolXxx files — only create new UCs; they are consumed starting at P3L.2
- Existing custom controls (`RJButton`, `AdjustBarEx`, ...) — do not modify

**Steps**:

1. **`RoiToolbar`**: copy the cluster `btnRect/btnElip/btnPolygon/btnHexagon/btnNewShape/btnArea/btnCrop/btnMask/btnInsideOut/btnOutsideIn/btnCropFull/btnCropHalt/btnBlack/btnWhite` from `ToolCircle.Designer.cs` into a standalone UC. Public API:
   ```csharp
   public class RoiToolbar : UserControl
   {
       public event EventHandler<RoiToolEventArgs> RoiActionClicked;
       public bool ShowMaskGroup { get; set; } = true;
       public bool ShowPolygon { get; set; } = true;
       public bool ShowSampling { get; set; } = true;
       // ... visibility flags so a tool can hide unused buttons
   }
   public enum RoiAction { Rect, Ellipse, Polygon, Hexagon, NewShape, Area, Crop, Mask, InsideOut, OutsideIn, CropFull, CropHalt, Black, White }
   public class RoiToolEventArgs : EventArgs { public RoiAction Action { get; set; } }
   ```

2. **`ScoreThresholdBar`**: cluster `trackScore + label "Score"` + threshold lines.
   ```csharp
   public class ScoreThresholdBar : UserControl
   {
       public float Min { get => bar.Min; set => bar.Min = value; }
       public float Max { get => bar.Max; set => bar.Max = value; }
       public float Step { get => bar.Step; set => bar.Step = value; }
       public float Value { get => bar.Value; set => bar.Value = value; }
       public string Caption { get; set; } = "Score";
       public event EventHandler ValueChanged;
       // internal: AdjustBarEx bar (already exists)
   }
   ```

3. **`ResultMiniGrid`**: 2-column result table (Name/Value) + an OK/NG column.
   ```csharp
   public class ResultMiniGrid : UserControl
   {
       public void SetRow(string name, object value, bool? isOk = null);
       public void Clear();
       public Color OkColor { get; set; } = Color.LimeGreen;
       public Color NgColor { get; set; } = Color.OrangeRed;
   }
   ```

4. Update `BeeInterface.csproj` to include 9 new files (3 UCs × 3 files each).

**Verify**:
- Build passes
- Open the 3 UCs in Designer with no errors
- Drag-drop them onto an empty Form

**DoD**:
- [ ] 3 UCs + Designer + resx committed (9 files)
- [ ] Build passes
- [ ] `CODEX_HISTORY.md` appended with P3L.1
- [ ] Commit: `[P3L.1] Extract RoiToolbar + ScoreThresholdBar + ResultMiniGrid`

---

## P3L.2 — Pilot ToolCircle: extract logic into Engine Runner

**Goal**: prove that Run/Save/Load logic can be extracted from `ToolCircle.cs` WITHOUT touching `BeeCore.Unit.Circle` (the persisted `[Serializable]` engine payload).

**Preconditions**:
- [ ] P3L.0, P3L.1 done
- [ ] A sample `.prog` file is available for save/load smoke testing
- [ ] Baseline build passes

**In-scope**:
- CREATE new: `BeeCore/Func/Engines/CircleEngineRunner.cs`
- EDIT: `BeeInterface/Tool/ToolCircle.cs` (delegation only; do NOT edit Designer.cs)

**Out-of-scope**:
- `BeeCore/Unit/Circle.cs` — DO NOT touch (preserves persisted format)
- `BeeInterface/Tool/ToolCircle.Designer.cs` — DO NOT touch
- The other 23 ToolXxx files

**Steps**:

1. Read `BeeInterface/Tool/ToolCircle.cs` (~800 LOC). Identify:
   - Which method runs the algorithm (HoughCircles + draw + score)
   - Where Save/Load Para reads/writes Circle fields via OwnerTool

2. Create `BeeCore/Func/Engines/CircleEngineRunner.cs`:
   ```csharp
   using BeeCore;
   using OpenCvSharp;
   namespace BeeCore.Funtion.Engines
   {
       public static class CircleEngineRunner
       {
           // Pure function — no UI dependency
           public static CircleRunResult Run(Circle propety, Mat input)
           {
               // copy logic from ToolCircle.cs
               // ...
               return new CircleRunResult { /*...*/ };
           }

           public static void ApplyToOwner(PropetyTool owner, Circle propety) { /* copy save logic */ }
           public static void ReadFromOwner(PropetyTool owner, Circle propety) { /* copy load logic */ }
       }

       public class CircleRunResult
       {
           public bool IsOk;
           public float Score;
           public Mat Overlay;
           // ... fields the UI needs to render
       }
   }
   ```

3. Edit `BeeInterface/Tool/ToolCircle.cs` — replace logic body with calls:
   ```csharp
   public void RunCheck(Mat input)
   {
       var result = BeeCore.Funtion.Engines.CircleEngineRunner.Run(Propety, input);
       // UI work only: draw overlay, set track score, update ResultMiniGrid
       trackScore.Value = result.Score;
       // ...
   }
   ```
   - Event subscription: keep the `-=` / `+=` pair for `OwnerTool.StatusToolChanged` and `OwnerTool.ScoreChanged`.

4. Register a dynamic tab (if Circle needs an extra tab):
   ```csharp
   // In BeeMain/Program.cs or app startup
   ToolTabRegistry.Register("Circle", "logs", "Logs", ctx => new ResultMiniGrid());
   ```

**Verify**:
| Command | Expected |
|---|---|
| Full build | passes, warnings ≤ baseline |
| `bash tools/check_propety_tools.sh` | exit 0 |
| Smoke: load a legacy `.prog` | loads OK, ToolCircle renders as before |
| Smoke: run one ToolCircle inspection | numeric result matches pre-refactor (static image test) |
| Smoke: save → close app → reopen → load | round-trip OK |

**DoD**:
- [ ] `ToolCircle.cs` LOC drops by ≥ 30% (logic extracted)
- [ ] `BeeCore.Unit.Circle` has no diff
- [ ] Save/load round-trip OK
- [ ] `CODEX_HISTORY.md` appended with P3L.2 and actual effort
- [ ] Commit: `[P3L.2] Pilot — extract CircleEngineRunner from ToolCircle`

**Stop-and-ask**: if effort > 2 days → halt and review the framework before doing P3L.3.

---

## P3L.3 — Pilot ToolWidth (replicate P3L.2)

**Goal**: confirm the P3L.2 template works for a second tool.

**Steps**: clone P3L.2, swap Circle → Width.

**Exit criteria**:
- [ ] Actual effort ≤ 1.5x P3L.2 → framework is OK, proceed to P3L.4
- [ ] Effort > 2x → review the template

---

## P3L.4 — Bulk Group A (10 remaining tools)

**Goal**: apply the P3L.2 pattern to `Measure`, `Edge`, `EdgePixel`, `Corner`, `Crop`, `Intersect`, `ColorArea`, `Counter`, `CheckMissing`, `Position_Adjustment`.

**Preconditions**: P3L.2 + P3L.3 have passed DoD; smoke for 2 tools is OK.

**Steps**: one commit per tool. Use the P3L.2 checklist. Up to 2-3 tools may run in parallel if effort is low.

**DoD per tool**: round-trip save/load OK + ToolXxx LOC reduced.

---

## P3L.5 — Group B: dynamic tabs + extra UCs

**Goal**: 7 Group B tools (Pattern, MultiPattern, MatchingShape, Pitch, Barcode, OCR, CraftOCR) gain a `Preset` tab and (when needed) a `Preprocess` tab.

**Per-tool extra steps**:
1. Create `XxxEngineRunner` as in P3L.2.
2. Create extra UCs as needed (`PresetCombo`, `SymbologyCombo`, `PitchPainterControl`, ...).
3. Register dynamic tabs via `ToolTabRegistry.Register(toolKind, "preset", "Preset", ...)`.

**Notes**:
- Pattern2 is being developed in parallel (CLAUDE.md section 1.2). Do NOT touch the `Patterns.cs` engine — only add UI tabs.
- `PitchGdiPainter.cs` may be wrapped into a UC — but wrap it in the Tool layer (BeeInterface), do NOT relocate the original file.

---

## P3L.6 — Merge duplicates

Same as CLAUDE.md P1.1, P1.2 but performed after the UI has stabilized:

**In-scope**:
- Merge the 3 `CustomGui.cs` → `BeeInterface/Custom/CustomGuiMerged.cs` (placed in the UI layer because the helpers are mostly UI Color/RoundRg).
- Merge the 2 `Converts.cs` → one copy in `BeeCore/Func/Converts.cs`.
- Merge `ShapeEditing/`: keep the abstractions (interfaces, args, enums) in `BeeCore/ShapeEditing/`; move the UI implementations (`ImageCanvasControl`, `ImageCanvasInteractionState`, `ImageCanvasShapeContext`) into `BeeInterface/ShapeEditing/` (already present — just remove the BeeCore duplicates).

**Verify**:
- `find . -name "CustomGui.cs"` → 1 result
- `find . -name "Converts.cs" -not -path "*/bin/*" -not -path "*/obj/*"` → 1 result
- Build passes + smoke

---

## P3L.7 (optional) — Split UI namespaces for the 12 Group A tools

**Goal**: tighten the `BeeInterface` namespace by moving tools into `BeeInterface.Tool.Xxx`. APPLIES ONLY to the UC layer — engines remain untouched.

**Preconditions**: P3L.4 done, build clean.

**Steps**:
- Change `namespace BeeInterface` → `namespace BeeInterface.Tool.Circle.View` in `ToolCircle.cs` + Designer + resx.
- Move files into `BeeInterface/Tool/Circle/`.
- Update `using` statements at every consumer: `BeeUi/`, `BeeMain/`, `BeeInterface/Group/AddTool.cs`, `BeeInterface/General/ToolPage.cs`, ...
- Engine `BeeCore.Circle` — DO NOT touch.

**Verify**:
- Build passes
- Smoke: legacy `.prog` files still load — engine lookup keeps using `BeeCore.Circle`, which is unaffected because the UI namespace is not in the persisted stream

**DoD**:
- [ ] 12 Group A tools have new namespaces
- [ ] Legacy `.prog` files still load OK
- [ ] One commit per tool: `[P3L.7.<n>] Move ToolXxx to BeeInterface.Tool.<Xxx>.View`

---

## Effort summary

| ID | Name | Estimated effort | Risk |
|---|---|---|---|
| P3L.0 | Base + Registry | 1 day | Low |
| P3L.1 | 3 shared UCs | 1 day | Low |
| P3L.2 | Pilot Circle | 1.5 days | **Med** (gate) |
| P3L.3 | Pilot Width | 1 day | Low (if P3L.2 OK) |
| P3L.4 | Bulk Group A | 1.5 weeks | Med |
| P3L.5 | Group B | 1.5 weeks | Med (Pattern/Pitch) |
| P3L.6 | Dedup | 0.5 week | Low |
| P3L.7 | UI namespaces | 0.5 week | Low |

**Total**: about 5 weeks for a single engineer. Drops to ~3 weeks if you skip P3L.5 (defer Group B) and P3L.7 (optional).

---

## Stop conditions

Halt and ask the user when:
- P3L.2 effort exceeds 2 days
- Loading a legacy `.prog` smoke fails at any point
- Concurrent Pattern2 development conflicts with P3L.5 for `Patterns` / `MultiPattern`
- Designer fails to open in VS after edits

## Hard rules recap (short)

1. DO NOT rename or change the namespace of any class in `BeeCore/Unit/*`.
2. DO NOT remove fields from `[Serializable]` classes persisted to `.prog`.
3. DO NOT use `Common.PropetyTools[i][j]` — use `Common.TryGetTool(i, j)` instead.
4. Every `event +=` must be paired with `event -=` on the same handler immediately above.
5. One commit = one task card. Build + smoke + history append before committing.
