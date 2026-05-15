# AGENTS.md - Pattern2 False Match Plan

## Scope

Folder nay dung de lap ke hoach va theo doi rieng cho loi Tool Pattern bat sai mau trong `Pattern/Pattern2.cpp`.

Khong sua code trong folder ke hoach nay. Khi trien khai code, chi sua trong scope da ghi o `pattern2-map.md`.

## Working Rules

1. Truoc khi sua code, doc `Plan.md` va `pattern2-map.md`.
2. Chon entry `Status=open`, `Risk` thap nhat truoc, tru khi entry khac dang chan loi false positive ro rang hon.
3. Khong doi P/Invoke hoac C++/CLI public API neu chua co entry rieng va test goi tu C#.
4. Khong thay doi ownership unmanaged memory: buffer cap phat qua `AllocHGlobal` hoac native allocation phai co duong `FreeBuffer`/free tuong ung.
5. Khong lam nang UI thread. Cac do benchmark/debug anh phai chay ngoai UI path hoac co flag debug.
6. Moi thay doi thuat toan phai co log/anh doi chung:
   - true positive: mau trang vien xanh/blue trong anh san pham.
   - false positive: vung canh cheo/o chu nhat xanh duoc khoanh do trong anh loi.
7. Build theo root `AGENTS.md`. Voi `.md` only: khong build.

## Verification

Minimum verification for code changes:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal
```

Additional vision verification before closing algorithm entries:

- Save `Pattern2StableConfig.DebugLog=true` output for at least one OK and one NG/false-positive image.
- Compare `coarse`, `raw`, `grad`, `edgeIoU`, `edgeRatio`, `final`, and `reason` lines.
- Confirm false-positive candidate is rejected by `shape_gate`, `below_threshold`, or geometry validator while the real sample still passes.

