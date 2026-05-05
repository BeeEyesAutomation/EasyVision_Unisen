# Converts Usage Map

P3L.10 documentation-only map for the two `Converts.cs` files. They are not direct duplicates: they expose different type names and serve different domains.

## Files

| File | Namespace/type | Project | Role |
| --- | --- | --- | --- |
| `BeeCore/Converts.cs` | `BeeCore.Convert2` | `BeeCore` | Small string helper for extracting digits as an integer. |
| `BeeCore/Func/Converts.cs` | `BeeCore.Func.Converts` | `BeeCore` | Engine/native conversion and OCR/PLC string parsing helper. |

## Public API

| Type | Method | Current callers | Notes |
| --- | --- | --- | --- |
| `BeeCore.Convert2` | `NumberFromString(string input)` | `BeeCore/Unit/Yolo.cs` | Extracts digits only; returns `0` on blank/no digits. Does not parse signs or decimals. |
| `BeeCore.Func.Converts` | `BeforeFirstDigit(string s)` | Pattern/check/multi-pattern/position PLC code | Returns prefix before the first digit. |
| `BeeCore.Func.Converts` | `StringtoDouble(string s)` | Pattern/check/multi-pattern/position PLC code | Parses signed decimal-like values with `.` or `,`, returns nullable `double`. |
| `BeeCore.Func.Converts` | `ToCli(RectRotate r)` | Native engine call paths | Converts `BeeGlobal.RectRotate` to `BeeCpp.RectRotateCli`, including polygon and hexagon points. |
| `BeeCore.Func.Converts` | `PyToRectRotates(PyObject payloads)` | `BeeCore/Unit/CraftOCR.cs` | Converts Python payloads into `RectRotate` polygons. Requires `Python.Runtime`. |

`BeeCore.Func.Converts.DrawRectRotate(...)` is a private helper in the file and has no external callers.

## Active Call Sites

| Caller | Methods | Purpose |
| --- | --- | --- |
| `BeeCore/Unit/Yolo.cs` | `Convert2.NumberFromString` | Split PLC address text into numeric and prefix parts. |
| `BeeCore/Unit/Barcode.cs` | `Converts.ToCli` | Pass crop/area/mask shapes to native barcode code. |
| `BeeCore/Unit/CheckMissing.cs` | `Converts.StringtoDouble`, `BeforeFirstDigit`, `ToCli` | PLC position formatting and native shape conversion. |
| `BeeCore/Unit/ColorArea.cs` | `Converts.ToCli` | Native color-area shape conversion. |
| `BeeCore/Unit/CraftOCR.cs` | `Converts.PyToRectRotates`, `ToCli` | Python OCR payload conversion and native crop conversion. |
| `BeeCore/Unit/MultiPattern.cs` | `Converts.StringtoDouble`, `BeforeFirstDigit`, `ToCli` | PLC formatting and native pattern shape conversion. |
| `BeeCore/Unit/Patterns.cs` | `Converts.StringtoDouble`, `BeforeFirstDigit`, `ToCli` | PLC formatting and native pattern shape conversion. |
| `BeeCore/Unit/PositionAdj.cs` | `Converts.StringtoDouble`, `BeforeFirstDigit`, `ToCli` | PLC formatting and native pattern/position shape conversion. |
| `BeeCore/Unit/VisualMatch.cs` | `Converts.ToCli` | Native visual-match shape conversion. |

Comment-only references exist in `BeeCore/Unit/Edge.cs` and `BeeCore/Unit/MultiOnnx.cs`; these should not block cleanup by themselves.

## Recommended Target

Do not merge these files by filename alone. The safer split is by responsibility:

| Responsibility | Suggested future home | Rationale |
| --- | --- | --- |
| PLC/string parsing (`NumberFromString`, `BeforeFirstDigit`, `StringtoDouble`) | `BeeCore.Func.TextConvert` or future `BeeCore.Domain` utility | Pure string helpers, no native or Python dependency required. |
| Native shape conversion (`ToCli`) | `BeeCore.Func.NativeConvert` or future `BeeCore.Vision.NativeInterop` | Depends on `BeeCpp`, `BeeGlobal.RectRotate`, polygon/hex shape semantics. |
| Python OCR payload conversion (`PyToRectRotates`) | OCR/CraftOCR runner or future `BeeCore.AI` | Depends on `Python.Runtime`; should not be loaded by unrelated tools. |

Suggested migration order:

1. Add new clearly named helper classes while keeping both old type names.
2. Move `NumberFromString`, `BeforeFirstDigit`, and `StringtoDouble` first; these are pure and easy to test.
3. Move `ToCli` only after native Debug and Release builds are green, because it affects Barcode, Pattern, ColorArea, VisualMatch, and OCR paths.
4. Move `PyToRectRotates` last and keep it near CraftOCR dependencies so `Python.Runtime` does not spread into pure conversion utilities.
5. Convert old `Convert2` and `Converts` methods into forwarding wrappers for one compatibility pass.
6. Delete the old wrappers only after a full solution build plus persistence test pass.

## Risk Notes

- `Converts.StringtoDouble(...)` returns `double?`, but several current call sites cast directly to `int`. Existing behavior can throw if parsing returns `null`; do not change semantics during dedup.
- `Convert2.NumberFromString(...)` strips all non-digits, so `"D10.5"` becomes `105`, not `10`. Do not replace it with `StringtoDouble(...)` without checking caller intent.
- `Converts.ToCli(...)` preserves polygon and hexagon shape metadata. Any replacement must keep `PolyLocalPoints`, `HexVertexOffsets`, `Shape`, `IsWhite`, center, size, and rotation behavior.
- `PyToRectRotates(...)` uses Python object lifetime management. It needs a focused test before relocation.

## Verification Commands Used

```powershell
Get-ChildItem -Path . -Recurse -Include *.cs -File |
  Where-Object { $_.FullName -notmatch '\\(bin|obj|\.vs|BackupOld)\\' } |
  Select-String -Pattern '\bConvert2\b','\bConverts\b','using BeeCore\.Func'
```

```powershell
Select-String -Path BeeCore\Unit\*.cs -Pattern 'Converts\.|Convert2\.' -Context 1,1
```
