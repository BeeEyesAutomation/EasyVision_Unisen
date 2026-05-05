# Native interop boundary

This repository keeps the C# to native boundary in the native wrapper projects.
C# code should depend on the exported wrapper surface from these projects, not on
native implementation files directly.

## C# callable wrapper surfaces

| Project | C# callable surface | Native implementation detail |
| --- | --- | --- |
| `BeeCV` | C++/CLI wrappers such as `CCD`, `ColorMaskWrapper`, `MatchingShape`, and the `okng_wrapper` exports | OpenCV camera, matching, focus, color mask, and OK/NG helper implementation files |
| `PylonCli` | `Pylon::Camera` in `CameraWrapper.h` | Basler/Pylon SDK calls and frame acquisition implementation |
| `Pattern` | C++/CLI wrappers such as `Pattern2`, `BarcodeCoreCli`, `FilterCLi`, `MonoSegCli`, `PitchCli`, and `RansacLine` | Pattern, barcode, filter, segmentation, pitch, and line-detection native algorithms |
| `BeeNativeOnnx` | `extern "C"` exports declared in `YoloNativeExport.h` | OpenVINO YOLO runtime and debug logging implementation |
| `BeeNativeRCNN` | `extern "C"` exports declared in `YoloNativeExport.h` | OpenVINO RCNN runtime and debug logging implementation |
| `ColorPixelsCPP` | Native project consumed through its declared project output | Color pixel native implementation |
| `OKNG` | Native project consumed through its declared project output | OK/NG native resources and implementation |

## Boundary rules

- Keep C# calls routed through the wrapper headers and exported functions above.
- Do not change existing C++/CLI public classes, exported function names, calling
  conventions, or P/Invoke-compatible signatures without coordinating the C#
  caller changes in the same task.
- Keep unmanaged ownership explicit at the wrapper boundary. Any buffer allocated
  for C# callers must have a documented owner and release path.
- Camera wrappers must preserve the SDK lifecycle expected by C# callers:
  Start, then Read, then Stop.
