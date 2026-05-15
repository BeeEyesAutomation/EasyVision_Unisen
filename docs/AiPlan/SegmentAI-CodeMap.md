# SegmentAI Code Map — Reference Cheatsheet

> Mục đích: Pre-computed reference data cho agent implement `ToolSegmentAI`. Agent **KHÔNG cần** Read lại các file dưới đây để hiểu pattern — mọi snippet đã extract sẵn với file:line refs. Chỉ cần Read khi modify chính file đó.
>
> Sinh từ: Explore agent passes 2026-05-12 + targeted grep verification. Refresh khi pattern thay đổi.

---

## A. Project Constants & Locations (đã verify)

| Khái niệm | Giá trị / File | Note |
|---|---|---|
| Solution | `EasyVision.sln` | 14 projects, plus PLC_Communication.sln + BeeCV.sln separate. |
| Native toolset | `v143`, C++17 (`stdcpp17`) | [BeeNativeOnnx/BeeNativeOnnx.vcxproj:124](BeeNativeOnnx/BeeNativeOnnx.vcxproj:124) |
| OpenCV include | `C:\OpenCV4.5\opencv\build\include` | [BeeNativeOnnx/BeeNativeOnnx.vcxproj:75](BeeNativeOnnx/BeeNativeOnnx.vcxproj:75) |
| OpenCV lib | `C:\OpenCV4.5\opencv\build\x64\vc15\lib`, link `opencv_world455.lib` | [BeeNativeOnnx/BeeNativeOnnx.vcxproj:76,129](BeeNativeOnnx/BeeNativeOnnx.vcxproj:76) |
| OpenVINO (Phase 2) | `C:\Program Files (x86)\Intel\openvino_2025.4.0\runtime\` | [BeeNativeOnnx/BeeNativeOnnx.vcxproj:75-76](BeeNativeOnnx/BeeNativeOnnx.vcxproj:75) |
| C# target framework | .NET Framework 4.8, Platform x64 | Verify từ `BeeCore.csproj`. |
| Output base dir | `BeeMain/bin/x64/Release/` | DLL native copy về đây qua PostBuildEvent. |
| Working dir runtime | thư mục chứa `EasyVision.exe` | Mọi path C# relative từ đây. |
| Baseline build | 0 errors, **447 warnings** | [docs/architecture/baseline_build.md](docs/architecture/baseline_build.md) 2026-05-03. |

---

## B. Enum & Type Locations

### B.1 `TypeTool` enum
**File**: [BeeGlobal/Enums.cs:390-414](BeeGlobal/Enums.cs:390)

```csharp
public enum TypeTool
{
    Position_Adjustment=0,
    Pattern=1,
    OKNG = 16,
    Color_Area = 5,
    MatchingShape = 2,
    Crop = 17,
    Width=6,
    Circle = 15,
    Measure = 14,
    Learning = 12,
    OCR =10,
    BarCode=11,
    Corner=18,
    VisualMatch=19,
    Pitch = 20,
    EdgePixel= 21,
    Edge=22,
    CraftOCR = 23,
    Intersect = 24, Systems=25, MultiPattern = 26, AutoTrig=27, MultiLearning=28, CheckMissing=29, Edge2 = 30,
}
```

**Edit cho SegmentAI**: thêm `SegmentAI = 31` vào cuối line 411 (sau `Edge2 = 30`).

**KHÔNG đổi giá trị enum đang có** — đã serialize vào `.prog` file của khách hàng (BinaryFormatter), đổi sẽ break load.

### B.2 `TypeCrop` enum (dùng cho `DataTool.NewRotRect`)
**File**: [BeeGlobal/Enums.cs](BeeGlobal/Enums.cs) (gần TypeTool). Verify exact line khi modify.

Values cần: `Area`, `Crop`. Pattern dùng:
```csharp
rotArea = DataTool.NewRotRect(TypeCrop.Area);
rotCrop = DataTool.NewRotRect(TypeCrop.Crop);
```

---

## C. Helper Resolver API (BẮT BUỘC dùng)

**File**: [BeeCore/Common.cs:153-203](BeeCore/Common.cs:153)

| API | Signature | When to use |
|---|---|---|
| `Common.TryGetTool(indexProg, indexTool)` | `static PropetyTool TryGetTool(int, int)` — null if OOR | Lookup tool, không tạo. |
| `Common.TryGetTool(indexTool)` | `static PropetyTool TryGetTool(int)` — uses `Global.IndexProgChoose` | Lookup current program. |
| `Common.TryGetToolList(indexProg)` | `static List<PropetyTool> TryGetToolList(int)` — null if OOR | Lookup list, không tạo. |
| `Common.TryGetCurrentToolList()` | `static List<PropetyTool> TryGetCurrentToolList()` | Lookup current program list. |
| `Common.EnsureToolList(indexProg)` | `static List<PropetyTool> EnsureToolList(int)` — tạo nếu thiếu | Mutate / add tool. |
| `Common.EnsureCurrentToolList()` | `static List<PropetyTool> EnsureCurrentToolList()` | Mutate current program list. |
| `Common.SetToolList(indexProg, list)` | `static void SetToolList(int, List<PropetyTool>)` | Assign list (thay cho `PropetyTools[i] = ...`). |

**TUYỆT ĐỐI KHÔNG dùng** `Common.PropetyTools[i][j]` trực tiếp (Hard Rule CLAUDE.md §0.2.2 + CI guard [tools/check_propety_tools.sh](tools/check_propety_tools.sh)).

### Pattern OwnerTool resolution cho `ToolSegmentAI.cs`:
```csharp
private PropetyTool OwnerTool {
    get {
        // Index resolution similar to ToolYolo — verify exact indexes
        return Common.TryGetTool(Global.IndexProgChoose, ThisToolIndex);
    }
}
```

Lưu ý: các tool hiện tại cache `OwnerTool` cục bộ trong constructor / Init, KHÔNG lookup mỗi frame. Theo CODEX_HISTORY 2026-04-22 (PropetyTool index cleanup).

---

## D. Event Subscribe Convention

**Hard Rule** CLAUDE.md §0.2.4: mọi `+=` PHẢI có `-=` ngay phía trên. Verify by:
```powershell
$f = "BeeInterface/Tool/ToolSegmentAI.cs", "BeeInterface/Tool/ToolSegmentAI.Designer.cs"
(Select-String -Path $f -Pattern ' \+=').Count
(Select-String -Path $f -Pattern ' -=').Count
# expect equal
```

### Pattern (exact, copy-able):
**Source**: [BeeInterface/Tool/ToolYolo.cs:85-86](BeeInterface/Tool/ToolYolo.cs:85), [ToolYolo.cs:1735-1736](BeeInterface/Tool/ToolYolo.cs:1735)

```csharp
// EditRectRot ROI change:
EditRectRot1.RotateCurentChanged -= EditRectRot_RotateCurentChanged;
EditRectRot1.RotateCurentChanged += EditRectRot_RotateCurentChanged;

// Progress callback from POCO Propety:
Propety.PercentChange -= Propety_PercentChange;
Propety.PercentChange += Propety_PercentChange;
```

### Events available on `PropetyTool`:
**File**: [BeeCore/PropetyTool.cs:192-210](BeeCore/PropetyTool.cs:192)

| Event | Signature | Purpose |
|---|---|---|
| `ScoreChanged` | `event Action<float>` | Score result updated. |
| `PercentChange` | `event Action<int>` | Long-running progress 0-100. |
| `StatusToolChanged` | `event Action<PropetyTool, StatusTool>` | Tool state machine change (Ready/Run/Done/Error). |

### Events available on `EditRectRot`:
**File**: [BeeInterface/Group/EditRectRot.cs](BeeInterface/Group/EditRectRot.cs)

| Event | Signature | Line |
|---|---|---|
| `RotateCurentChanged` | `event Action<RectRotate>` | [:49](BeeInterface/Group/EditRectRot.cs:49) |
| `ChooseScanChange` | `event Action<int>` | [:471](BeeInterface/Group/EditRectRot.cs:471) |
| `ChooseEditBegin` | `event Action<bool>` | [:486](BeeInterface/Group/EditRectRot.cs:486) |
| `ChooseEditEnd` | `event Action<int>` | [:487](BeeInterface/Group/EditRectRot.cs:487) |
| `AddRotEvent` | `event Action<RectRotate>` | [:517](BeeInterface/Group/EditRectRot.cs:517) |
| `UnoRotEvent` | `event Action<bool>` | [:518](BeeInterface/Group/EditRectRot.cs:518) |
| `DeleteEvent` | `event Action<bool>` | [:519](BeeInterface/Group/EditRectRot.cs:519) |
| `ClearAllEvent` | `event Action<bool>` | [:520](BeeInterface/Group/EditRectRot.cs:520) |

### Set ROI list (4 RectRotate):
```csharp
EditRectRot1.Rot = new List<RectRotate> { Propety.rotArea, Propety.rotCrop, Propety.rotMask, Propety.rotLimit };
```

---

## E. POCO Unit Pattern (`SegmentAI.cs` skeleton dựa trên `Yolo.cs`)

**Source reference**: [BeeCore/Unit/Yolo.cs:39-150](BeeCore/Unit/Yolo.cs:39)

### Bắt buộc có (theo Yolo.cs structure):

```csharp
[Serializable]
public class SegmentAI
{
    // === Standard pattern (mọi Unit POCO đều có) ===
    public bool IsIni = false;                               // init flag
    public int Index = -1;                                   // tool index
    public int IndexCCD = 0;                                 // camera index
    public RectRotate rotArea, rotCrop, rotMask, rotLimit;   // ROIs (Yolo.cs:345)

    [NonSerialized] public RectRotate rotAreaAdjustment;
    [NonSerialized] public RectRotate rotMaskAdjustment;
    public RectRotate rotPositionAdjustment;
    public TypeCrop TypeCrop;

    // === Progress event (Yolo.cs:48) ===
    [field: NonSerialized] public event Action<int> PercentChange;

    // === SegmentAI specific ===
    public string pathModel = "";                            // relative path
    public string pathSamplesFolder = "";
    public List<SegSample> samples = new List<SegSample>();

    public int numTrees = 100;
    public int maxDepth = 12;
    public int minSampleCount = 10;
    public float defectThreshold = 0.5f;
    public int minDefectArea = 50;
    public bool enableGpu = true;

    // === Runtime state (NonSerialized — re-init after deserialize) ===
    [NonSerialized] public NativeSegAIInferer inferer;       // Yolo.cs:81 pattern
    [NonSerialized] public byte[] lastMask;
    [NonSerialized] public int lastMaskW, lastMaskH;
    [NonSerialized] public float lastScore;
    [NonSerialized] public bool IsOK;

    // === Init null-safe (Yolo.cs:108 pattern) ===
    public void SetModel() {
        if (rotArea == null) rotArea = DataTool.NewRotRect(TypeCrop.Area);
        if (rotCrop == null) rotCrop = DataTool.NewRotRect(TypeCrop.Crop);
        // ... etc
        // Then attempt to load model from pathModel (relative path)
    }

    public object Clone() => MemberwiseClone();
}
```

### Path pattern (relative from working dir):
**Source**: [BeeCore/Data/SaveData.cs:65](BeeCore/Data/SaveData.cs:65)
```csharp
String path = "Program\\" + Global.Project;
if (!Directory.Exists(path)) Directory.CreateDirectory(path);
// File at: path + "\\" + filename
```

Per-tool subfolder convention:
```csharp
string folder = $"Program\\{Global.Project}\\SegAI_{indexTool:D3}";
string modelPath = Path.Combine(folder, "model.segai");
```

**KHÔNG dùng** `Global.PathRoot` — không tồn tại trong codebase.

---

## F. Native DllImport Wrapper Pattern

**Source**: [BeeCore/Func/NativeYolo.cs:1-160](BeeCore/Func/NativeYolo.cs:1) — 1:1 template.

### Skeleton điểm chính (copy + rename `YOLO_` → `SEGAI_`):

```csharp
using System;
using System.Runtime.InteropServices;

namespace BeeCore
{
    public class NativeSegAITrainer : IDisposable
    {
        const string DLL = "BeeNativeSegAI.dll";

        // ===== Native struct (StructLayout.Sequential — exact match C++ struct order) =====
        // (Phase 1 không có struct trả về, chỉ ptr+w+h+score)

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern IntPtr SEGAI_TrainerCreate();

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SEGAI_TrainerDestroy(IntPtr handle);

        // ... etc per signatures in plan §8.2

        private IntPtr _handle;
        public bool IsOpened => _handle != IntPtr.Zero;

        public NativeSegAITrainer() {
            _handle = SEGAI_TrainerCreate();
            if (_handle == IntPtr.Zero) throw new Exception("SEGAI_TrainerCreate failed");
        }

        public void Dispose() {
            if (_handle != IntPtr.Zero) {
                SEGAI_TrainerDestroy(_handle);
                _handle = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }

        ~NativeSegAITrainer() { Dispose(); }
    }
}
```

### Quy ước:
- `CallingConvention.Cdecl` luôn dùng (match C++ `extern "C"`).
- `CharSet = CharSet.Unicode` cho function nhận `wchar_t*` (path file).
- `[Out] T[]` cho output array (Yolo dùng cho `YoloBox[]`).
- `IntPtr` cho buffer raw (Mat data ptr).
- Pattern `IDisposable` + finalizer fallback BẮT BUỘC.

---

## G. Native vcxproj Template (BeeNativeSegAI)

**Source**: [BeeNativeOnnx/BeeNativeOnnx.vcxproj:1-148](BeeNativeOnnx/BeeNativeOnnx.vcxproj:1).

### Khác biệt cần áp dụng:
1. **ProjectGuid** mới — auto-gen khi VS Add New Project, hoặc:
   ```powershell
   [guid]::NewGuid().ToString().ToUpper()
   ```
2. **RootNamespace** → `BeeNativeSegAI`.
3. **Debug|x64**: đổi `<ConfigurationType>Application</ConfigurationType>` → `<ConfigurationType>DynamicLibrary</ConfigurationType>` (line 44 trong template — template Debug đang là Application, lỗi! Fix khi copy).
4. **PreprocessorDefinitions** Release|x64 (line 122): thêm `BEESEGAI_EXPORTS;_USRDLL;` đầu list.
5. **IncludePath** (line 75): **chỉ** OpenCV, **bỏ** OpenVINO:
   ```
   C:\OpenCV4.5\opencv\build\include;$(IncludePath)
   ```
6. **LibraryPath** (line 76): chỉ OpenCV.
7. **AdditionalDependencies** (line 129): **chỉ** `opencv_world455.lib;` (Phase 1 không OpenVINO).
   - Debug|x64 dùng `opencv_world455d.lib;` (lib debug nếu có; nếu không, vẫn link `opencv_world455.lib` — verify trên dev machine).
8. **ItemGroup** (line 135-143): liệt kê 5 `.cpp` + 5 `.h` của SegAI module.
9. **PostBuildEvent** (thêm mới):
   ```xml
   <ItemDefinitionGroup Condition="'$(Configuration)|$(Platform)'=='Release|x64'">
     <PostBuildEvent>
       <Command>copy /Y "$(OutDir)BeeNativeSegAI.dll" "$(SolutionDir)BeeMain\bin\x64\Release\"</Command>
     </PostBuildEvent>
   </ItemDefinitionGroup>
   ```
   (Tham khảo Pattern.vcxproj cho cú pháp exact.)

### Tránh nhầm khi copy vcxproj:
- Trong template, Debug|x64 line 43-48 dùng `ConfigurationType>Application`. Đó là **lỗi** của BeeNativeOnnx (nó là DLL nhưng debug config khai application). Khi tạo BeeNativeSegAI, fix luôn cả Debug.
- `<UseDebugLibraries>` đúng cho từng config.

---

## H. Native Export Header Template

**Source**: [BeeNativeOnnx/YoloNativeExport.h:1-31](BeeNativeOnnx/YoloNativeExport.h:1) — 31 LOC reference.

### Pattern exact:
```cpp
#pragma once
#include <stdint.h>

#ifdef _WIN32
#define SAPI __declspec(dllexport)   // Compile DLL: export
#else
#define SAPI
#endif

extern "C"
{
    SAPI void* SEGAI_TrainerCreate();
    SAPI void  SEGAI_TrainerDestroy(void* handle);
    // ... etc per plan §8.2
}
```

**Pattern: define cùng macro cho BOTH compile (export) và client (import)**. Đây là quirk của BeeNativeOnnx — `__declspec(dllexport)` luôn, không `dllimport`. Compile DLL với `BEESEGAI_EXPORTS` defined như preprocessor để kích hoạt (xem §G.4). Alternative an toàn hơn (Phase 2 cân nhắc):
```cpp
#ifdef BEESEGAI_EXPORTS
#define SAPI __declspec(dllexport)
#else
#define SAPI __declspec(dllimport)
#endif
```

Phase 1 follow Yolo pattern (chỉ `dllexport`) để consistent.

---

## I. C++ Class Pattern (OpenVinoYoloHP.h reference)

**Source**: [BeeNativeOnnx/OpenVinoYoloHP.h:1-83](BeeNativeOnnx/OpenVinoYoloHP.h:1).

Pattern điểm chính:
- Class với private state members.
- Reuse buffers (`cv::Mat paddedU8`, `std::vector<float> inputBlob`) để tránh re-alloc.
- `#ifdef _MANAGED #undef interface #endif` — guard vì C++/CLI workaround.
- Member function signatures kết thúc bằng `std::vector<...>& out` (output by ref).

Apply cho SegAI:
- `SegFeatureExtractor` reuse kernels precomputed sau `Configure()`.
- `SegInferer` reuse `samples` Mat, `predRaw` Mat.

---

## J. Factory Registration

**File**: [BeeInterface/DataTool.cs:297-426](BeeInterface/DataTool.cs:297).

### Insertion point (sau case `MultiLearning`):
**Verify trước commit** — agent grep `case TypeTool.MultiLearning` ra exact line, insert ngay sau closing `break;`.

```csharp
case TypeTool.MultiLearning:
    control = new ToolMultiOnnx();
    if (IsNew)
        control.Propety = new MultiOnnx();
    break;
case TypeTool.SegmentAI:        // <-- thêm
    control = new ToolSegmentAI();
    if (IsNew)
        control.Propety = new SegmentAI();
    break;
case TypeTool.CheckMissing:
    // ...
```

Method signature: `public static dynamic New(TypeTool typeTool, bool IsNew=false)`.

### `DataTool.NewRotRect(TypeCrop)`
**File**: [BeeInterface/DataTool.cs:428](BeeInterface/DataTool.cs:428). Trả về `RectRotate` mặc định, sử dụng `Global.Config.SizeCCD`.

---

## K. BinaryFormatter Serialization Convention

**File**: [BeeCore/Data/Access.cs:20-110](BeeCore/Data/Access.cs:20).

### Save flow:
1. `SaveData.Prog()` ([BeeCore/Data/SaveData.cs:154](BeeCore/Data/SaveData.cs:154)) gọi `Access.SaveProg(path, Common.PropetyTools)`.
2. `Access.SaveProg` (line 107) dùng `new BinaryFormatter()` → `Serialize(stream, list)`.
3. SegmentAI POCO `[Serializable]` → tự động serialize cùng `PropetyTool.Propety2`.

### Load flow:
1. `LoadData.Project()` line 22-30:
   ```csharp
   listPropetyTool = Access.LoadProg("Program\\" + Project + "\\" + Project + ".prog");
   ```
2. Sau load, mỗi `SegmentAI` POCO cần re-init `[NonSerialized]` fields qua `SetModel()` hook ở `DataTool.BuildProjectUI()`.

### Forward-compat rule:
- Thêm field mới: declare default value, BinaryFormatter handle missing field gracefully nếu là default.
- Đổi tên / xoá field: dùng `[OptionalField]` + property alias OR bump version manually.
- **Test load 1 `.prog` cũ** trước khi commit field mới (Risk row §16 plan).

---

## L. Engine Runner Pattern

**Source**: [BeeCore/Func/Engines/CircleEngineRunner.cs](BeeCore/Func/Engines/CircleEngineRunner.cs) — pattern reference.

```csharp
namespace BeeCore.Func.Engines
{
    public static class SegmentAIEngineRunner
    {
        public sealed class SegmentAIRunResult
        {
            public bool IsOK;
            public float Score;
            public byte[] Mask;
            public int MaskW, MaskH;
            public int DefectPixelCount;
        }

        public static SegmentAIRunResult Run(SegmentAI propety, RectRotate roiArea, RectRotate roiMask, bool complete) {
            propety.DoWork(roiArea, roiMask);
            if (complete) propety.Complete();
            return new SegmentAIRunResult {
                IsOK = propety.IsOK,
                Score = propety.lastScore,
                Mask = propety.lastMask,
                MaskW = propety.lastMaskW,
                MaskH = propety.lastMaskH,
                DefectPixelCount = (int)(propety.lastScore * propety.lastMaskW * propety.lastMaskH)
            };
        }
    }
}
```

Other engine runners cùng pattern: `WidthEngineRunner.cs`, `MeasureEngineRunner.cs`, `ColorAreaEngineRunner.cs` — đều dùng `static class` + `Run(propety, roiArea, roiMask, complete)`.

---

## M. ROI Coordinate Rule (CRITICAL)

**Memory**: `project_roi_local_to_global`.

Khi convert toạ độ pixel từ local crop sang global frame:
- **BẮT BUỘC** dùng `Matrix(Translate→Rotate→Translate)`.
- Phép tịnh tiến đơn giản CHỈ đúng khi `RectRotate.Rotation == 0`.

### Pattern reference:
Cần grep Edge/Edge2/Circle code để tìm utility hiện có. Có lẽ có helper trong `BeeCore/Func/` (Crop, Vision) — nếu không, viết inline:

```csharp
using System.Drawing.Drawing2D;

// roi: RectRotate có Center (PointF), Rotation (deg)
// localPt: Point trong toạ độ crop (origin = top-left của crop)
// Returns: Point trong toạ độ full frame
public static PointF LocalToGlobal(RectRotate roi, PointF localPt, Size cropSize) {
    using (var m = new Matrix()) {
        // Move local origin to crop center
        m.Translate(-cropSize.Width / 2f, -cropSize.Height / 2f);
        // Rotate by ROI rotation
        m.Rotate(roi.Rotation, MatrixOrder.Append);
        // Translate to ROI center
        m.Translate(roi.Center.X, roi.Center.Y, MatrixOrder.Append);
        var pts = new[] { localPt };
        m.TransformPoints(pts);
        return pts[0];
    }
}
```

**Test case bắt buộc** (Task P3X.1.7 verify):
- Rot = 0° (sanity)
- Rot = 15° (small tilt)
- Rot = 45° (diagonal)
- Rot = -30° (negative sign)

So với ground truth: crop center luôn map về `roi.Center`.

---

## N. UI Designer Convention

**Memory**: `feedback_ui_in_designer`.

### Quy ước file split:
| File | Chứa | Không chứa |
|---|---|---|
| `ToolSegmentAI.cs` | Handler bodies (`btnTrainStart_Click`, etc), `LoadPara`, `Bind*`, `Refresh*` methods | Control declarations, layout code |
| `ToolSegmentAI.Designer.cs` | `partial class` declaration, `InitializeComponent`, control fields, **all event subscriptions** (`-=` rồi `+=`), layout properties | Handler bodies |
| `ToolSegmentAI.resx` | Embedded resources (icons, strings) | — |

### Quy ước collapsible section
**Memory**: `feedback_collapsible_param_sections`.

```csharp
// In Designer.cs:
private BeeInterface.Custom.RJButton btnHeaderRoi;        // header
private TableLayoutPanel tablePanelRoi;                   // collapsible content

// InitializeComponent:
btnHeaderRoi.IsTouch = true;
btnHeaderRoi.Click -= btnHeaderRoi_Click;
btnHeaderRoi.Click += btnHeaderRoi_Click;

// In .cs:
private void btnHeaderRoi_Click(object sender, EventArgs e) {
    tablePanelRoi.Visible = !tablePanelRoi.Visible;
}
```

---

## O. Threading & Async Pattern

**File**: [BeeCore/PropetyTool.cs:222](BeeCore/PropetyTool.cs:222).

```csharp
public BackgroundWorker worker { get => EnsureState().worker; set => EnsureState().worker = value; }
```

### Pattern dispatch long-running op:
```csharp
private void btnTrainStart_Click(object sender, EventArgs e) {
    // Validate state on UI thread first
    if (Propety.samples.Count == 0) { MessageBox.Show("..."); return; }

    OwnerTool.worker.DoWork -= TrainDoWork;
    OwnerTool.worker.DoWork += TrainDoWork;
    OwnerTool.worker.ProgressChanged -= TrainProgressChanged;
    OwnerTool.worker.ProgressChanged += TrainProgressChanged;
    OwnerTool.worker.RunWorkerCompleted -= TrainCompleted;
    OwnerTool.worker.RunWorkerCompleted += TrainCompleted;

    OwnerTool.worker.RunWorkerAsync();
}

private void TrainDoWork(object sender, DoWorkEventArgs e) {
    // Off UI thread
    Propety.Train(p => OwnerTool.worker.ReportProgress(p), CancellationToken.None);
}
```

### UI marshalling (per memory `project_roi_local_to_global` không liên quan, but UI thread safety):
```csharp
private void Propety_PercentChange(int p) {
    if (InvokeRequired) BeginInvoke((Action)(() => progressTrain.Value = p));
    else progressTrain.Value = p;
}
```

---

## P. Project File Updates (.csproj XML snippets)

### P.1 `BeeCore/BeeCore.csproj` — thêm vào `<ItemGroup>` chứa `<Compile Include>`:

```xml
<Compile Include="Func\NativeSegAI.cs" />
<Compile Include="Unit\SegmentAI.cs" />
<Compile Include="Func\Engines\SegmentAIEngineRunner.cs" />
```

### P.2 `BeeInterface/BeeInterface.csproj`:

```xml
<Compile Include="Custom\MaskPainter.cs">
  <SubType>UserControl</SubType>
</Compile>
<Compile Include="Custom\MaskPainter.Designer.cs">
  <DependentUpon>MaskPainter.cs</DependentUpon>
</Compile>
<EmbeddedResource Include="Custom\MaskPainter.resx">
  <DependentUpon>MaskPainter.cs</DependentUpon>
</EmbeddedResource>

<Compile Include="Tool\ToolSegmentAI.cs">
  <SubType>UserControl</SubType>
</Compile>
<Compile Include="Tool\ToolSegmentAI.Designer.cs">
  <DependentUpon>ToolSegmentAI.cs</DependentUpon>
</Compile>
<EmbeddedResource Include="Tool\ToolSegmentAI.resx">
  <DependentUpon>ToolSegmentAI.cs</DependentUpon>
</EmbeddedResource>
```

### P.3 `EasyVision.sln`:
- Add Existing Project → `BeeNativeSegAI\BeeNativeSegAI.vcxproj`.
- Configuration Manager: tick Build cho Release|x64 + Debug|x64.
- Project Dependencies: `BeeCore` depends on `BeeNativeSegAI`.

Sln entry format (manual edit nếu không có VS):
```
Project("{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}") = "BeeNativeSegAI", "BeeNativeSegAI\BeeNativeSegAI.vcxproj", "{<NEW-GUID>}"
EndProject
```
Trong `GlobalSection(ProjectConfigurationPlatforms)`:
```
{<NEW-GUID>}.Debug|x64.ActiveCfg = Debug|x64
{<NEW-GUID>}.Debug|x64.Build.0 = Debug|x64
{<NEW-GUID>}.Release|x64.ActiveCfg = Release|x64
{<NEW-GUID>}.Release|x64.Build.0 = Release|x64
```

---

## Q. Build Verify Commands (PowerShell)

```powershell
# Full solution build
& 'C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe' `
    EasyVision.sln /m:1 /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal `
    /flp:logfile=build.log";verbosity=minimal"

# Warning count
(Select-String -Path build.log -Pattern ': warning ').Count
# Error count
(Select-String -Path build.log -Pattern ': error ').Count

# Convention guard
bash tools/check_propety_tools.sh   # expect "OK: no direct..."

# Native standalone (after P3X.1.5)
.\BeeNativeSegAI\x64\Release\SegAITest.exe `
    .\BeeNativeSegAI\test\data\sample_defect_01.png `
    .\BeeNativeSegAI\test\data\sample_mask_01.png

# Native exports check
dumpbin /EXPORTS .\BeeNativeSegAI\x64\Release\BeeNativeSegAI.dll | Select-String SEGAI_

# Event balance for ToolSegmentAI
$f = "BeeInterface/Tool/ToolSegmentAI.cs", "BeeInterface/Tool/ToolSegmentAI.Designer.cs"
$plus = (Select-String -Path $f -Pattern ' \+=').Count
$minus = (Select-String -Path $f -Pattern ' -=').Count
"plus=$plus minus=$minus equal=$($plus -eq $minus)"
```

---

## R. Anti-Patterns / Known Traps

| Anti-pattern | Detection | Why bad |
|---|---|---|
| `Common.PropetyTools[i][j]` direct access | [tools/check_propety_tools.sh](tools/check_propety_tools.sh) | OOR crash, no null safety. Use `Common.TryGetTool`. |
| `event += Handler` without `-=` first | Manual review / event balance script | Duplicate handler subscription → double fire. |
| Refresh / repaint trong handler chạy trên non-UI thread | Crash `InvalidOperationException` | WinForms requires UI thread. Wrap `if (InvokeRequired) BeginInvoke(...)`. |
| `Global.PathRoot` reference | Compile error (symbol không tồn tại) | Paths luôn relative từ working dir. |
| Modify `TypeTool` enum value của tool đã có | BinaryFormatter load crash khi project file cũ | Đổi giá trị break serialized data. Chỉ thêm cuối. |
| Touch `Pattern/Pattern2.*`, `Pattern/Pitch*` | git merge conflict | Parallel dev — Hard Rule §0.2.8. |
| Tịnh tiến đơn giản ROI local→global | Mask position lệch khi rot ≠ 0 | Phải dùng Matrix Translate→Rotate→Translate (memory `project_roi_local_to_global`). |
| Control declaration trong `.cs` (không Designer.cs) | Designer crash khi mở trong VS | All UI in `.Designer.cs` (memory `feedback_ui_in_designer`). |
| `string path = Path.Combine(Global.PathRoot, …)` | Compile error | Dùng relative path từ working dir. |
| Drop `BeeNativeSegAI.dll` không vào `BeeMain/bin/x64/Release/` | Runtime `DllNotFoundException` | Cần PostBuildEvent copy. |
| Train trên 0 samples | Native crash hoặc empty model | Validate trước call native. |
| BinaryFormatter deserialize sau khi thêm `[Serializable]` field mà không default value | Field = default reference (`null`) — NPE | Init trong field declaration: `public List<X> foo = new List<X>();`. |
| `cv::ml::RTrees::predict` không UMat (chỉ Mat) | Compile fine, GPU không có effect | Download UMat sang Mat trước khi predict. |
| Multi-thread call `Predict` không lock | RTrees::predict thread-unsafe trong v4.5? | Verify với simple stress test; nếu unsafe, lock per-instance. |

---

## S. File Index — Master List

### Sẽ tạo mới (15 files)
| File | LOC ước tính | Task Card |
|---|---|---|
| `BeeNativeSegAI/BeeNativeSegAI.vcxproj` | 150 (XML) | P3X.1.1 |
| `BeeNativeSegAI/pch.h` | 10 | P3X.1.1 |
| `BeeNativeSegAI/framework.h` | 5 | P3X.1.1 |
| `BeeNativeSegAI/dllmain.cpp` | 25 | P3X.1.1 |
| `BeeNativeSegAI/SegAINativeExport.h` | 70 | P3X.1.5 |
| `BeeNativeSegAI/SegAINativeExport.cpp` | 250 | P3X.1.5 |
| `BeeNativeSegAI/SegFeatureCore.h` | 50 | P3X.1.2 |
| `BeeNativeSegAI/SegFeatureCore.cpp` | 400 | P3X.1.2 |
| `BeeNativeSegAI/SegTrainerCore.h` | 40 | P3X.1.3 |
| `BeeNativeSegAI/SegTrainerCore.cpp` | 200 | P3X.1.3 |
| `BeeNativeSegAI/SegInferCore.h` | 40 | P3X.1.4 |
| `BeeNativeSegAI/SegInferCore.cpp` | 200 | P3X.1.4, P3X.1.11 |
| `BeeNativeSegAI/SegAIFileFormat.h` | 30 | P3X.1.3 |
| `BeeNativeSegAI/SegAIFileFormat.cpp` | 120 | P3X.1.3 |
| `BeeNativeSegAI/test/SegAITest.cpp` | 100 | P3X.1.5 |
| `BeeCore/Func/NativeSegAI.cs` | 350 | P3X.1.6 |
| `BeeCore/Unit/SegmentAI.cs` | 400 | P3X.1.7 |
| `BeeCore/Func/Engines/SegmentAIEngineRunner.cs` | 50 | P3X.1.8 |
| `BeeInterface/Custom/MaskPainter.cs` | 250 | P3X.1.9 |
| `BeeInterface/Custom/MaskPainter.Designer.cs` | 200 | P3X.1.9 |
| `BeeInterface/Custom/MaskPainter.resx` | 50 (XML) | P3X.1.9 |
| `BeeInterface/Tool/ToolSegmentAI.cs` | 400 | P3X.1.10 |
| `BeeInterface/Tool/ToolSegmentAI.Designer.cs` | 600 | P3X.1.10 |
| `BeeInterface/Tool/ToolSegmentAI.resx` | 50 (XML) | P3X.1.10 |

### Sẽ sửa (5 files)
| File | Sửa gì | Task Card |
|---|---|---|
| [BeeGlobal/Enums.cs:411](BeeGlobal/Enums.cs:411) | Thêm `SegmentAI = 31` | P3X.1.8 |
| [BeeInterface/DataTool.cs:412](BeeInterface/DataTool.cs:412) | Thêm case `TypeTool.SegmentAI` | P3X.1.8 |
| `BeeCore/BeeCore.csproj` | 3 `<Compile Include>` (xem §P.1) | P3X.1.6, .7, .8 |
| `BeeInterface/BeeInterface.csproj` | Compile + EmbeddedResource (xem §P.2) | P3X.1.9, .10 |
| `EasyVision.sln` | Add project + dependencies | P3X.1.1 |
| `BeeCore/Func/Camera.cs` hoặc `BeeCore/Vision.cs` | Switch case scan loop | P3X.1.12 |
| `CODEX_HISTORY.md` (root) | Append section per task | mọi Task Card |

### Sẽ Read làm reference (KHÔNG sửa)
| File | Mục đích |
|---|---|
| [BeeNativeOnnx/BeeNativeOnnx.vcxproj](BeeNativeOnnx/BeeNativeOnnx.vcxproj) | Template vcxproj |
| [BeeNativeOnnx/YoloNativeExport.h](BeeNativeOnnx/YoloNativeExport.h) | Export header pattern |
| [BeeNativeOnnx/OpenVinoYoloHP.h](BeeNativeOnnx/OpenVinoYoloHP.h) | C++ class pattern |
| [BeeCore/Func/NativeYolo.cs](BeeCore/Func/NativeYolo.cs) | DllImport pattern |
| [BeeCore/Unit/Yolo.cs](BeeCore/Unit/Yolo.cs) | POCO pattern |
| [BeeCore/Unit/OKNG.cs](BeeCore/Unit/OKNG.cs) | Simpler POCO pattern |
| [BeeInterface/Tool/ToolYolo.cs](BeeInterface/Tool/ToolYolo.cs) | Form pattern (event sub) |
| [BeeInterface/Group/EditRectRot.cs](BeeInterface/Group/EditRectRot.cs) | ROI editor API |
| [BeeCore/Func/Engines/CircleEngineRunner.cs](BeeCore/Func/Engines/CircleEngineRunner.cs) | Runner pattern |
| [BeeCore/Common.cs](BeeCore/Common.cs) | Helper resolvers |
| [BeeCore/PropetyTool.cs](BeeCore/PropetyTool.cs) | Events + worker |
| [BeeCore/Data/Access.cs](BeeCore/Data/Access.cs) | BinaryFormatter ops |
| [BeeCore/Data/SaveData.cs](BeeCore/Data/SaveData.cs) | Path convention |
| [Pattern/Pattern2.cpp](Pattern/Pattern2.cpp) | OpenCL UMat setup ([line 36](Pattern/Pattern2.cpp:36)) |

---

## T. Quick Lookup

| Need | Where |
|---|---|
| TypeTool enum value | [BeeGlobal/Enums.cs:390](BeeGlobal/Enums.cs:390) |
| TypeCrop enum value | Search `BeeGlobal/Enums.cs` for `TypeCrop` |
| `Common.TryGetTool` | [BeeCore/Common.cs:154](BeeCore/Common.cs:154) |
| `PropetyTool.PercentChange` | [BeeCore/PropetyTool.cs:198](BeeCore/PropetyTool.cs:198) |
| `PropetyTool.worker` | [BeeCore/PropetyTool.cs:222](BeeCore/PropetyTool.cs:222) |
| `EditRectRot.RotateCurentChanged` | [BeeInterface/Group/EditRectRot.cs:49](BeeInterface/Group/EditRectRot.cs:49) |
| `DataTool.New` factory | [BeeInterface/DataTool.cs:297](BeeInterface/DataTool.cs:297) |
| `DataTool.NewRotRect` | [BeeInterface/DataTool.cs:428](BeeInterface/DataTool.cs:428) |
| `Yolo` POCO sample | [BeeCore/Unit/Yolo.cs:39](BeeCore/Unit/Yolo.cs:39) |
| `NativeYolo` DllImport sample | [BeeCore/Func/NativeYolo.cs:10](BeeCore/Func/NativeYolo.cs:10) |
| OpenCV path | `C:\OpenCV4.5\opencv\build\` |
| OpenCV lib | `opencv_world455.lib` (release), debug version `opencv_world455d.lib` if available |
| Working dir runtime | `BeeMain/bin/x64/Release/` |
| Project file (.prog) location | `Program\<Global.Project>\<Project>.prog` |
| Model file (.segai) location | `Program\<Global.Project>\SegAI_<NNN>\model.segai` |
| OpenCL enable | `cv::ocl::setUseOpenCL(true)` ([Pattern/Pattern2.cpp:36](Pattern/Pattern2.cpp:36)) |

---

## U. Reference Patterns NOT Yet In This Map

Khi gặp 1 trong các need dưới đây, agent có thể Read trực tiếp file source (đỡ tốn token):

- Cropper rotate ROI: `BeeCore/Func/Crop.cs` (cần check exact file)
- Mat ↔ Bitmap conversion: `BeeCore/Func/Converts.cs`
- Color overlay vẽ trên image: `BeeCore/Func/Draws.cs`
- ImageBox custom render: `BeeInterface/Group/View.cs`
- Bitmap save with metadata: `BeeCore/Data/Access.cs` related methods

---

## V. Maintenance

Refresh map này khi:
- Có thay đổi cấu trúc TypeTool enum (thêm/xoá value).
- Có thay đổi signature `Common.*` helpers.
- Pattern POCO (`[Serializable]` + events) thay đổi convention.
- File `[Pattern2.cpp](Pattern/Pattern2.cpp)` line 36 (OpenCL init) thay đổi.
- Baseline build warning count đổi (cập nhật §A).

Append note vào `CODEX_HISTORY.md` khi refresh.
