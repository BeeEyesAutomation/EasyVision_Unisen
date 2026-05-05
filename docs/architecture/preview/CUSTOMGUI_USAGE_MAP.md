# CustomGui Usage Map

P3L.10 documentation-only map for the duplicate-looking GUI helpers. This file does not authorize deletion by itself; it records the current public types, active call sites, and a safe migration recipe for the later dedup card.

## Files

| File | Namespace/type | Project | Role |
| --- | --- | --- | --- |
| `BeeCore/Func/CustomGui.cs` | `BeeCore.CustomGui` | `BeeCore` | Active WinForms helper for translucent colors and rounded control regions. |
| `BeeInterface/CustomGui.cs` | `BeeInterface.Gui` | `BeeInterface` | Same helper implementation under a different type name. No active call sites found in current source. |

Both files expose the same public methods:

| Method | Signature | Dependencies | Notes |
| --- | --- | --- | --- |
| `BackColor` | `static Color BackColor(TypeCtr type, Color color)` | `BeeGlobal.TypeCtr`, `System.Drawing.Color` | Computes an ARGB color by fixed alpha per control kind. |
| `RoundRg` | `static void RoundRg(dynamic contr, int roundRad, Corner corner = Corner.Both)` | `BeeGlobal.Corner`, `System.Drawing.Drawing2D.GraphicsPath`, WinForms `Region` | Mutates a WinForms control's `Region`. This is UI-only behavior despite living in `BeeCore`. |

## Active Call Sites

Active, non-comment call sites found outside the helper files:

| Caller | Lines | Current API | Purpose |
| --- | --- | --- | --- |
| `BeeInterface/Group/ToolSettings.cs` | 135, 153 | `CustomGui.RoundRg(...)` resolved via `using BeeCore;` | Rounds `pBtn` on load and resize. |
| `BeeUi/Unit/EditTool.cs` | 706, 936 | `BeeCore.CustomGui.RoundRg(...)` | Rounds `pInfor` on setup and resize. |

No active `BeeInterface.Gui.*` call sites were found. `BeeInterface.Gui` appears to be a duplicate copy kept in the UI project.

Many commented-out references remain in `BeeInterface/Group/*`, `BeeInterface/Tool/*`, `BeeInterface/StepEdit.cs`, and `BeeUi/Unit/EditTool.cs`. They should not drive API retention decisions, but they are useful when checking whether old theme code is being revived.

## Recommended Target

Move the helper to a UI-owned location because both methods depend on WinForms drawing/control mutation:

| Target | Keep? | Reason |
| --- | --- | --- |
| `BeeInterface.Gui` or a future shared UI helper | Yes | Correct layer for `Region`, `GraphicsPath`, and control styling. |
| `BeeCore.CustomGui` | Temporary compatibility shim only | `BeeCore` should not own WinForms UI helpers long term. |

Short-term, the least risky migration is:

1. Change the two active callers to use `BeeInterface.Gui.RoundRg(...)`.
2. Keep `BeeCore.CustomGui` as a wrapper that forwards to `BeeInterface.Gui` only if project references allow it. If the reference would be circular, leave `BeeCore.CustomGui` in place until a `BeeShared.UI` project exists.
3. Once `BeeShared.UI` exists, move the implementation there and let both old type names forward to the shared helper for one release.
4. Delete `BeeInterface/CustomGui.cs` only after all active callers and project references are confirmed in a clean Debug and Release build.

Do not rename `Corner` or `TypeCtr` during this migration; both are `BeeGlobal` enums and are used broadly outside this helper.

## ShapeEditing Notes

The earlier P3L.6 card also called out `ShapeEditing` overlap. Current source contains two separate namespaces, not duplicate files in one namespace:

| Folder | Namespace | Role |
| --- | --- | --- |
| `BeeCore/ShapeEditing/` | `BeeCore.ShapeEditing` | Contracts, state, geometry, repository, and overlay abstractions. |
| `BeeInterface/ShapeEditing/` | `BeeInterface.ShapeEditing` | UI canvas control, UI interaction state/context, plus parallel copies of several contracts/state types. |

Current active consumers found outside the folders:

| Caller | API | Purpose |
| --- | --- | --- |
| `BeeInterface/Group/View.cs` | `using BeeInterface.ShapeEditing` | Hosts the current image canvas/editing UI. |
| `BeeInterface/Group/View.Designer.cs` | `BeeInterface.ShapeEditing.ImageCanvasControl` | Designer-owned canvas field. |

Safe migration direction:

1. Keep UI controls in `BeeInterface.ShapeEditing` until a shared UI project exists.
2. Prefer `BeeCore.ShapeEditing` for pure contracts/state that must be used by engine or non-UI code.
3. Do not delete any `BeeInterface.ShapeEditing` type while `View.Designer.cs` still instantiates `ImageCanvasControl`.
4. Before merging duplicate contract names, build a per-type adapter plan because namespaces differ and several classes are referenced through designer-generated code.

## Verification Commands Used

```powershell
Get-ChildItem -Path . -Recurse -Include *.cs -File |
  Where-Object { $_.FullName -notmatch '\\(bin|obj|\.vs|BackupOld)\\' } |
  Select-String -Pattern '\bCustomGui\b','\bGui\.','BeeCore\.ShapeEditing','BeeInterface\.ShapeEditing'
```

```powershell
Select-String -Path BeeCore\BeeCore.csproj,BeeInterface\BeeInterface.csproj `
  -Pattern 'CustomGui.cs|ShapeEditing'
```
