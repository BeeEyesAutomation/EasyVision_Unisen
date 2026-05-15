# AGENTS.md - Tool Measure Angle Line/Point Plan

## Scope

Folder nay dung de lap ke hoach va theo doi rieng cho thay doi Tool Measure: do Angle giua 2 duong thang, moi duong co the lay tu 2 point nhu cu hoac lay truc tiep 1 line tu tool Edge/Edge2.

Khong sua code trong folder ke hoach nay. Khi trien khai code, chi sua trong scope da ghi o `tool-measure-angle-map.md`.

## Working Rules

1. Truoc khi sua code, doc `Plan.md` va `tool-measure-angle-map.md`.
2. Chon entry `Status=open`, `Risk` thap nhat truoc, tru khi entry khac dang chan luong do Angle ro rang hon.
3. Bao toan file save cu: tool Measure cu khong co mode moi phai load nhu Point mode.
4. Khong doi P/Invoke, C++/CLI public API, hoac unmanaged memory.
5. Khong block UI thread. UI chi binding combo/mode, khong goi SDK hay xu ly anh nang.
6. Khong sua Designer ngoai cac control can cho mode Line/Point.
7. Build theo root `AGENTS.md`. Voi `.md` only: khong build.

## Verification

Minimum verification for code changes:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal
```

Manual UI verification before closing:

- `Type Measure = Angle`, line mode `Point`: four existing point combos still work.
- `Type Measure = Angle`, line mode `Line`: combo lists only `TypeTool.Edge` and `TypeTool.Edge2`; endpoint combo is hidden/disabled.
- Mixed mode works: Line 1 = Edge/Edge2 line, Line 2 = two points, and reverse.
- Old saved Measure tools load without losing `listPointChoose`.
- Edge2 uses its selected center line (`Line2DCli`) as the angle line, while Edge uses its own `Line2DCli`.

