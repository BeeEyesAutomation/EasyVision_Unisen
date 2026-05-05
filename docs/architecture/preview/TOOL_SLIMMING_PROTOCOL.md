# Tool Slimming Protocol

P3L.12 defines the first enforceable budget for migrated `ToolXxx` views. The goal is not to hide complexity; it is to make each file name describe one responsibility so future changes can land without reading a large mixed UI/engine file.

## Budget

| File kind | Target | Hard ceiling | Notes |
| --- | --- | --- | --- |
| Main `ToolXxx.cs` | 150-220 physical lines | 250 physical lines | Constructor, owner resolution, `LoadPara`, tab hosting, and high-level lifecycle only. |
| `ToolXxx.Parameters.cs` | 80-220 physical lines | 300 physical lines | Control value handlers and score/status callbacks. |
| `ToolXxx.Roi.cs` | 80-260 physical lines | 350 physical lines | ROI/shape selection and crop/mask UI behavior. |
| `ToolXxx.*Runner.cs` | No fixed ceiling | Review by cohesion | Engine-facing actions, owner writes, and non-UI orchestration. |
| Designer files | No slimming target | Designer-owned | Do not manually rewrite unless the task explicitly scopes it. |

If a tool cannot fit the hard ceiling without making event flow harder to trace, document a per-tool exception in `CODEX_HISTORY.md` and keep the split by responsibility.

## Allowed Moves

1. Keep persisted payload classes in `BeeCore/Unit/*` unchanged.
2. Move engine-facing or owner-facing behavior into `BeeCore/Func/Engines/*Runner.cs` when it has no direct WinForms dependency.
3. Move UI-only event handlers into partial files named by responsibility:
   - `ToolXxx.Parameters.cs`
   - `ToolXxx.Roi.cs`
   - `ToolXxx.Layout.cs`
   - `ToolXxx.Results.cs`
4. Keep Designer event handler method names stable. Designer code may call private methods from any partial class.
5. Keep `event -= Handler;` immediately above `event += Handler;`.
6. Keep `Common.PropetyTools[...]` out of UI code; use `Common.TryGetTool(...)` helpers.

## Disallowed Moves

1. Do not rename or remove fields from `[Serializable]` payload classes.
2. Do not change namespaces of persisted `BeeCore/Unit/*` types.
3. Do not edit `*.Designer.cs` only to reduce line count.
4. Do not replace parsing/conversion helpers during a slimming card unless that card explicitly scopes the semantic change.
5. Do not delete old duplicate-looking helpers until a usage map and clean build confirm the replacement.

## Verification Per Tool

Run these checks after each slimming card:

```powershell
(Get-Content BeeInterface\Tool\ToolXxx.cs | Measure-Object -Line).Lines
```

```powershell
Get-ChildItem -Path BeeCore,BeeInterface -Recurse -Include *.cs -File |
  Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' } |
  Select-String -Pattern 'Common\.PropetyTools\['
```

```powershell
MSBuild BeeInterface\BeeInterface.csproj /t:Build /p:Configuration=Debug /p:Platform=x64 /v:minimal
```

When the persistence harness exists, also run:

```powershell
MSBuild tests\BeeCore.Persistence.Tests\BeeCore.Persistence.Tests.csproj /t:Build /p:Configuration=Debug /p:Platform=x64 /v:minimal
tests\BeeCore.Persistence.Tests\bin\x64\Debug\BeeCore.Persistence.Tests.exe
```

## Circle Pilot Result

P3L.12 applies this protocol first to `ToolCircle`:

| File | Responsibility |
| --- | --- |
| `BeeInterface/Tool/ToolCircle.cs` | Constructor, owner cache, `LoadPara`, run/test shell, empty designer stubs. |
| `BeeInterface/Tool/ToolCircle.Roi.cs` | Crop/area/mask switching, shape creation, color polarity, full-area toggle. |
| `BeeInterface/Tool/ToolCircle.Parameters.cs` | Score/status callbacks, edge/threshold/radius/morphology controls, section visibility toggles. |

The pilot intentionally does not change `BeeCore/Unit/Circle.cs` or `ToolCircle.Designer.cs`.
