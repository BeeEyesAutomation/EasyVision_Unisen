# Baseline Build

## 2026-05-03 - Release x64

Command:

```powershell
& 'C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe' EasyVision.sln /m:1 /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal '/flp:logfile=docs\architecture\baseline_build_release.log;verbosity=minimal'
```

Result: passed.

Log: `docs/architecture/baseline_build_release.log`

Summary from the saved log:

| Metric | Count |
| --- | ---: |
| Errors | 0 |
| Warnings | 447 |
| Built outputs | 15 |

Built outputs recorded in the log:

| Project | Output |
| --- | --- |
| `Pattern.vcxproj` | `x64/Release/BeeCpp.dll` |
| `BeeCV.vcxproj` | `x64/Release/BeeCV.dll` |
| `PylonCli.vcxproj` | `x64/Release/PylonCli.dll` |
| `PLC_Communication` | `PLC_Communication/bin/Release/net461/PLC_Communication.dll` |
| `BeeGlobal` | `BeeGlobal/bin/Release/BeeGlobal.dll` |
| `BeeCore` | `BeeCore/bin/Release/BeeCore.dll` |
| `BeeInterface` | `BeeInterface/bin/Release/BeeInterface.dll` |
| `BeeUi` | `BeeUi/bin/x64/Release/BeeUi.dll` |
| `BeeMain` | `bin/Release/EasyVision.exe` |
| `BeeUpdate` | `BeeUpdate/bin/Release/BeeUpdate.exe` |
| `TestCPlus` | `Update/bin/Release/Update.exe` |
| `OKNG.vcxproj` | `x64/Release/OKNG.dll` |
| `BeeNativeOnnx.vcxproj` | `x64/Release/BeeNativeOnnx.dll` |
| `BeeNativeRCNN.vcxproj` | `x64/Release/BeeNativeRCNN.dll` |
| `BeeCore.Persistence.Tests` | `tests/BeeCore.Persistence.Tests/bin/Release/BeeCore.Persistence.Tests.exe` |

Notes:

- Native version-header pre-build updates now use `tools/update_native_version.ps1` for `BeeCV` and `OKNG`.
- `BeeCore` and `BeeUi` Release builds now resolve OpenCvSharp from the local compatible output assemblies instead of missing `C:\Lib` paths.
- `BeeCore` uses the existing compatible OpenCvSharp assembly because the NuGet package assembly marks several legacy `Mat` constructors as compile-time obsolete errors.
- Warnings are still high and are treated as baseline warnings for this card, not new blockers.
