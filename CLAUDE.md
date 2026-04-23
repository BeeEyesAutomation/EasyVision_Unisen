# CLAUDE.md - Kế Hoạch Tối Ưu Hóa & Tái Cấu Trúc EasyVision_Unisen

> File này dành cho Claude/Codex (và các agent kế tiếp) khi làm việc trong repo. **BẮT BUỘC** đọc theo thứ tự: (1) mục 0 Hard Rules, (2) `CODEX_HISTORY.md` mục mới nhất, (3) mục 4 Roadmap để biết mình đang ở phase nào, (4) Task Card cụ thể trong mục 5. Khi hoàn tất một bước, **append** kết quả xuống `CODEX_HISTORY.md` theo template ở mục 11.

---

## 0. Hard Rules Cho Codex/Claude (PHẢI ĐỌC TRƯỚC)

### 0.1 DO — Luôn làm

1. **Đọc `CODEX_HISTORY.md` mục mới nhất** trước khi bắt đầu bất kỳ task nào — để nắm thay đổi gần đây và blocker native.
2. **Xác định phase hiện tại** (xem mục 4) → lấy đúng Task Card từ mục 5 → điền template Task Card (mục 11) trước khi gõ code.
3. **Dùng helper resolver** cho mọi truy cập tool: `Common.TryGetTool / TryGetToolList / EnsureToolList / SetToolList`. Không bao giờ dùng `Common.PropetyTools[ip][it]` trực tiếp.
4. **Event subscribe luôn cặp đôi**: `X.Event -= Handler; X.Event += Handler;` — không có ngoại lệ.
5. **Một commit = một Task Card**. Không gộp 2 task vào 1 commit. Commit message format: `[P<phase>.<task>] <summary>` (ví dụ `[P1.1] Merge 3 CustomGui.cs into BeeShared.UI`).
6. **Build verify trước khi commit**: `MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal`. Số warning ≤ baseline ghi trong `docs/architecture/baseline_build.md`.
7. **Move file giữ history**: dùng `git mv` (CLI) hoặc Solution Explorer (VS). Update `.csproj` ngay trong cùng commit.
8. **Append `CODEX_HISTORY.md`** sau khi xong: scope / files touched / verify command / blocker / notes for future agents.
9. **Giữ public API cũ** khi tách project: dùng `[assembly: TypeForwardedTo(typeof(...))]` trong shim assembly cũ, **không** đổi namespace của type đã serialize.
10. **Nếu phát hiện CLAUDE.md sai/không khả thi** → cập nhật trực tiếp CLAUDE.md (mục liên quan + Changelog mục 13) và ghi lý do trong `CODEX_HISTORY.md`. Không im lặng skip.

### 0.2 DON'T — Tuyệt đối KHÔNG

1. **KHÔNG đổi tên symbol public** (class/namespace/method) trong cùng commit với move file cấu trúc. Rename là Phase 6 riêng.
2. **KHÔNG tạo call site `Common.PropetyTools[...]` mới**. Sau commit, regex `\.PropetyTools\[` ở code mới phải = 0 (kiểm tra ngoài `Common.cs`).
3. **KHÔNG động vào file ngoài scope Task Card** đang làm. Worktree thường dirty — không format/refactor file lạ kể cả khi "thấy ngứa mắt".
4. **KHÔNG revert** file native version/build output tự generated.
5. **KHÔNG commit** build artifacts: `bin/`, `obj/`, `x64/`, `Debug/`, `Release/`, `*.pdb`, `*.user`, `.vs/`.
6. **KHÔNG skip** bước build verify chỉ vì "tôi đoán là pass". Phải có log thực.
7. **KHÔNG gộp 2 phase** vào 1 commit/PR.
8. **KHÔNG động vào `Pattern/Pattern2.*`, `Pattern/Pitch*`** trong Phase 1-2 (đang phát triển song song). Chỉ được thay build flag (như C++17, `HAS_ZXING`).
9. **KHÔNG đổi namespace** của type đã serialize vào `ClassProject.json` nếu chưa test load 1 project mẫu.
10. **KHÔNG tạo singleton/static state mới ở UI layer**. State global đi qua `BeeCore.Domain.Common.*`.
11. **KHÔNG tạo file mới ở root `BeeCore`**. File mới phải vào sub-folder đúng concern.
12. **KHÔNG xoá file Form (`.cs`/`.Designer.cs`/`.resx`)** khi chưa migrate xong sang module mới và chưa pass smoke test.

### 0.3 Stop-and-ask — Khi nào BẮT BUỘC hỏi user trước khi tiếp tục

| Tình huống | Hỏi gì |
|---|---|
| Build baseline (Phase 0) không pass | Blocker nào được phép skip / disable project nào |
| `git status` có local edit ngoài scope mình chuẩn bị đụng | Có muốn stash/commit trước không |
| Một bước CLAUDE.md nếu làm sẽ phá smoke test (ví dụ break load project cũ) | Muốn skip / sửa kế hoạch / chấp nhận break |
| Cần thêm NuGet package / native dependency mới | Có được phép thêm dependency không |
| Pilot tool effort > 2 ngày | Đổi sang tool khác / dừng pilot / mở rộng timebox |
| Phát hiện type public sẽ bị đổi tên/namespace ảnh hưởng `ClassProject.json` | Có project mẫu để test backward-compat không |
| Move/rename file tạo > 100 file diff | Có muốn chia commit nhỏ hơn không |

### 0.4 Flowchart làm việc chuẩn

```
Bắt đầu task
   ↓
[1] Đọc CODEX_HISTORY.md (3 mục cuối) + CLAUDE.md mục 0 + mục 4
   ↓
[2] Xác định phase hiện tại + Task Card ID (P<x>.<y>)
   ↓
[3] Mở mục 5.<phase> → copy Task Card template (mục 11) → điền vào docs/architecture/tasks/YYYY-MM-DD-P<x>.<y>-<slug>.md
   ↓
[4] Kiểm tra Preconditions trong Task Card
   ↓ pass                                    ↓ fail
[5] Làm theo Steps                           Hỏi user (mục 0.3)
   ↓
[6] Chạy Verify commands → so với Expected
   ↓ pass                                    ↓ fail
[7] Append CODEX_HISTORY.md (template ở 11.2)   Rollback theo Task Card → re-plan
   ↓
[8] Commit với message [P<x>.<y>] <summary>
   ↓
[9] Báo user: file đụng tới + verify result + blocker còn lại
```

### 0.5 Pre-flight checklist (trước khi gõ code)

```
[ ] Đã đọc 3 section cuối của CODEX_HISTORY.md
[ ] Đã xác định Task Card ID
[ ] Đã copy Task Card template, điền In-scope / Out-of-scope
[ ] git status: ghi nhận file dirty hiện tại (nếu có, hỏi user)
[ ] Đã chạy build baseline để có log so sánh sau
[ ] Hiểu rõ DoD của Task Card này
[ ] Biết rollback procedure nếu fail
```

---

## 1. Đánh Giá Hiện Trạng (snapshot 2026-04-22)

### 1.1 Solution layout
Solution chính: `EasyVision.sln`. 14 project, gồm cả C# (WinForms .NET Framework 4.8) lẫn C++ native. Hai solution phụ độc lập: `BeeCV/BeeCV.sln`, `PLC_Communication/PLC_Communication.sln`.

| Project | Loại | Vai trò hiện tại | Vấn đề |
|---|---|---|---|
| `BeeMain` | WinExe | Entry point | Tối giản, OK |
| `BeeUi` | WinForms | Form chính (Main, FormLoad, ScanCCD) | Có lẫn cả Unit/ logic |
| `BeeInterface` | WinForms | Tool editors, Dashboard, Steps, PLC UI | **Quá tải**: 24 ToolXxx + General/Custom/Group/Steps trộn UI và logic |
| `BeeCore` | Class Library | Vision, Camera, Algorithm, Data, Func, Items, Unit | **Quá tải**: ~130+ file gộp 8+ concern |
| `BeeGlobal` | Class Library | Tham số chung, Protocol, helper | Còn code device thấp tầng (DASK, PCI_Card) lẫn vào |
| `BeeUpdate` | WinExe | Auto-updater | OK |
| `PLC_Communication` | Class Library | PLC protocol | Đã tách solution riêng |
| `Pattern` (BeeCpp) | C++ DLL | Pitch, Pattern2, Barcode, Ransac, Color, Mono | Vừa nâng cấp (xem CODEX_HISTORY) |
| `BeeCV` | C++ DLL | OpenCV wrapper, capture, shape | Chồng chéo concern với `Pattern` |
| `BeeNativeOnnx` | C++ DLL | YOLO/OpenVINO inference | Block do thiếu `openvino/openvino.hpp` |
| `BeeNativeRCNN` | C++ DLL | Mask R-CNN | Cũng block do thiếu OpenVINO |
| `OKNG` | C++ DLL | OKNG wrapper | Nhỏ, OK |
| `PylonCli` | C++/CLI DLL | Basler camera | OK |

### 1.2 Tiến độ refactor đã có (từ CODEX_HISTORY)

`PropetyTool` đã được tách thành payload (`Propety`, `Propety2`) + `ToolState`, kèm các helper resolver:
`Common.TryGetTool`, `TryGetToolList`, `EnsureToolList`, `EnsureCurrentToolList`, `SetToolList`, `TryGetCurrentToolList`.

Nguyên tắc đã thiết lập (giữ nguyên):

- Mọi truy cập tool **không** dùng index hai cấp `Common.PropetyTools[ip][it]`. Thay bằng `TryGetTool/EnsureToolList/...`.
- Giữ `Common.PropetyTools` public để backward-compat, nhưng **không** thêm call site mới ngoài `Common.cs`.
- Subscribe event phải `-=` trước `+=` để tránh duplicate handler.
- Mỗi tool cache `OwnerTool` cục bộ thay vì lookup global mỗi lần.

`Pattern2` đã có preprocess pipeline đầy đủ (Gray/Edge/GrayPlusEdge, Sobel/Scharr/Canny/Laplacian, denoise None/Gaussian/Median/Bilateral), GPU UMat (OpenCL — không CUDA do link `opencv_world455.lib`), CPU multi-thread cho top-layer angle scan, và 5 preset (`PresetGeneralGray`, `PresetUnevenLighting`, `PresetMetalShiny`, `PresetPCBOrText`, `PresetLowContrast`). API debug C#: `PreviewPreprocessed`.

### 1.3 Vấn đề kiến trúc còn tồn đọng

1. **BeeCore là God-project**: Camera, EtherNetIP, Algorithm, Data, Func, Items, Unit, ShapeEditing, Parameter trong cùng một assembly.
2. **BeeInterface trộn UI và logic**: 24 file `ToolXxx.cs` vừa là Form vừa là engine.
3. **Trùng lặp class chung**: `CustomGui.cs` xuất hiện ở **3 nơi**, `Converts.cs` ở 2 nơi.
4. **Global state phân mảnh**: `BeeGlobal/Global.cs`, `BeeInterface/Global.cs`, `BeeCore/Parameter/G.cs`.
5. **Folder `Func/` mơ hồ**: gộp helper với feature thật.
6. **File "rơi" ở root BeeCore**: `Vision.cs`, `dataMat.cs`, `AddTool.cs`, `Actions.cs`, `LocalTool.cs`, `LabelItem.cs`, `KeepLargestAuto.cs`, `MatrixExtension.cs`, `BitmapExtensions.cs`, `Converts.cs`, `ValueRobot.cs`, `LibreTranslateClient.cs`, `DB.cs`.
7. **Naming không nhất quán**: `dataMat`, `Position_Adjustment`, `KEY_Send`, `Propety` (sai chính tả), `Comunication` (sai chính tả).
8. **Designer.cs nằm cùng `Tool/`**: thư mục có ~75 file (24 × 3).
9. **Dependency native lỏng**: `Pattern/BarcodeCore.cpp` thiếu `ZXing/ReadBarcode.h`; `Pattern/RansacLineCore.cpp` cần `std::clamp` (C++17); `BeeCV/Resource.rc` thiếu macro `VER_MAJOR`; OpenVINO header thiếu cho `BeeNativeOnnx`/`BeeNativeRCNN`.
10. **Trộn solution**: `BeeCV.sln` và `PLC_Communication.sln` riêng dễ build sai thứ tự.

---

## 2. Mục Tiêu & Nguyên Tắc

### 2.1 Mục tiêu

- **M1**. Giảm coupling UI ↔ Core để unit-test logic tool độc lập với Form.
- **M2**. Phân tách `BeeCore` thành assembly có trách nhiệm rõ ràng (Camera / Algorithms / Vision / IO / Comm / AI / Domain).
- **M3**. Chuẩn hóa "common vs specific": code dùng chung tách hẳn vào project Shared.
- **M4**. Mỗi `Tool` có 3 lớp tách biệt: Config (POCO), Engine (logic + native binding), View (Form + Designer).
- **M5**. Build full pass, kể cả native; rõ dependency chain.
- **M6**. Bảo toàn backward-compat ở public API đang dùng (đặc biệt `Common.PropetyTools`, `Propety/Propety2/PropetyTool/ToolState`).

### 2.2 Nguyên tắc bất biến

Đã đưa lên mục 0 (Hard Rules). Mục này giữ tóm tắt 1 dòng để tham chiếu nhanh: **không rename hàng loạt cùng commit move file; mỗi bước = move + build pass + history; không động file dirty ngoài scope; helper resolver bắt buộc; event `-=` trước `+=`.**

---

## 3. Cấu Trúc Đề Xuất

Mục tiêu cuối (target). Không đạt trong một PR — dùng làm la bàn cho từng phase.

```
EasyVision_Unisen/
├── EasyVision.sln
├── src/
│   ├── App/
│   │   ├── BeeMain/                # Entry point
│   │   └── BeeUpdate/              # Updater
│   │
│   ├── Shared/                     # Layer dùng chung, KHÔNG ref UI/Core
│   │   ├── BeeGlobal/              # Enum, Config, ParaXxx, Protocol/, TimingUtils
│   │   └── BeeShared.UI/           # CustomGui, ConvertImg, AdjustControl, RoundedPanel,
│   │                               #   GradientTabControl, LayoutPersistence,
│   │                               #   ControlStylePersistence (gộp duplicate)
│   │
│   ├── Core/
│   │   ├── BeeCore.Domain/         # Item models, Propety/Propety2/ToolState, ResultItem,
│   │   │                           #   LabelItem, ValueRobot, Common (resolver helpers)
│   │   ├── BeeCore.Algorithms/     # Algorithm/* + ImagePreprocessPipeline, Geometry2D,
│   │   │                           #   Filters, Colors, Gap, EdgePoints, RansacFitLine,
│   │   │                           #   RansacCircleFitter, FilletCornerMeasure*, MonoSegmentation,
│   │   │                           #   DetectIntersect, InsertLine, LineDetector
│   │   ├── BeeCore.Vision/         # Vision.cs, dataMat.cs, MatHelper, MatMerger, Crop,
│   │   │                           #   Draws, ImageUtils, BitmapExtensions, MatrixExtension,
│   │   │                           #   PolyOffset, Line2DTransform, RectRotateGapChecker
│   │   ├── BeeCore.Camera/         # Camera/* + HEROJE, USB, HardwareEnum, KEY_Send
│   │   ├── BeeCore.IO/             # Data/* (ClassProject, Access, LoadData, SaveData), DB.cs,
│   │   │                           #   LibreTranslateClient, BatchRename
│   │   ├── BeeCore.Comm/           # EtherNetIP/* + Comunication
│   │   └── BeeCore.AI/             # NativeYolo, NativeRCNN, Native, Func/Init, Func/Logs
│   │
│   ├── Tools/                      # Mỗi tool 1 thư mục: Engine + Form tách biệt
│   │   ├── Tool.Common/            # Base classes: ToolEngineBase, ToolFormBase, ToolBinder,
│   │   │                           #   IToolEngine, IToolConfig, ToolEngineFactory
│   │   ├── Tool.Pattern/
│   │   │   ├── PatternConfig.cs    # POCO
│   │   │   ├── PatternEngine.cs    # Logic, gọi Pattern2 native
│   │   │   ├── PatternForm.cs      # Form kế thừa ToolFormBase
│   │   │   ├── PatternForm.Designer.cs
│   │   │   ├── PatternForm.resx
│   │   │   └── Controls/           # UserControl riêng (nếu có)
│   │   ├── Tool.Pitch/
│   │   ├── Tool.Yolo/
│   │   └── ... (mỗi ToolXxx hiện tại thành 1 thư mục)
│   │
│   ├── UI/
│   │   ├── BeeUi/                  # Form chính: Main, FormLoad, ScanCCD
│   │   └── BeeInterface/           # Khung UI: Dashboard, Steps, General/, Group/, ToolPage,
│   │                               #   SettingPLC. KHÔNG còn ToolXxx.cs ở đây
│   │
│   └── Native/
│       ├── Pattern/                # BeeCpp
│       ├── BeeCV/
│       ├── BeeNativeOnnx/
│       ├── BeeNativeRCNN/
│       ├── OKNG/
│       └── PylonCli/
│
├── tests/
│   ├── BeeCore.Algorithms.Tests/
│   ├── Tool.Pattern.Tests/
│   └── ...
│
├── packages/                       # NuGet
├── docs/
│   └── architecture/
│       ├── CODEX_HISTORY.md
│       ├── baseline_build.md
│       ├── tasks/                  # Task Card đã filed
│       └── adr/                    # Architecture Decision Records
└── CLAUDE.md
```

### 3.1 Quy ước phân loại "common" vs "specific"

Một class thuộc **Shared** khi và chỉ khi:

- Không reference bất kỳ tool/feature nghiệp vụ cụ thể nào (không biết Pitch/Yolo/Pattern là gì).
- Không phụ thuộc state runtime (`Common.PropetyTools`, project hiện tại, ...).
- Có thể tái sử dụng bởi ≥ 2 project khác và không kéo theo dependency nặng.

Ví dụ Shared: `RoundedPanel`, `GradientTabControl`, `AdjustControl`, `LayoutPersistence`, `ConvertImg`, `TimingUtils`, `QpcTimer`.

Ngược lại, class thuộc **Tool.Xxx** khi chỉ phục vụ một tool. Khi class phục vụ nhiều tool nhưng không phải UI thuần → **Core.Vision** hoặc **Core.Algorithms**.

### 3.2 Tách Tool: Engine ≠ Form

Hiện tại `ToolPattern.cs` vừa Form, vừa cấu hình, vừa gọi native. Mục tiêu:

- `PatternConfig`: POCO. Field trong `Propety2` (hoặc reference từ `Propety2`).
- `PatternEngine`: nhận `PatternConfig` + ảnh → trả `PatternResult`. Test không cần Form.
- `PatternForm`: chỉ làm 2 việc: `Engine.Run(config, image)` và `Bind(config ↔ controls)`.

API ngoài không phá: pipeline vẫn vào `PropetyTool` ⇒ engine; UI mở/đóng độc lập.

---

## 4. Roadmap Tổng Quan

| Phase | Tên | Timebox | Output chính | Phụ thuộc |
|---|---|---|---|---|
| 0 | Chuẩn bị | 1-2 ngày | docs/architecture/, baseline_build.md, native gating | — |
| 1 | Dọn duplicate | 3-5 ngày | 1 bản CustomGui, 1 bản Converts; chưa đổi project | Phase 0 |
| 2 | Tách BeeCore → 7 sub-projects | 1-2 tuần | `BeeCore.Domain/.Algorithms/.Vision/.Camera/.IO/.Comm/.AI` + shim | Phase 1 |
| 3 | Tách Tool layer (chuẩn hoá GUI) | 2-3 tuần | `Tool.Common`, `Tool.Xxx` cho Nhóm A+B | Phase 2 (Domain), Phase 1 (Shared.UI) |
| 4 | Dọn UI shell | 1 tuần | BeeInterface gọn, BeeUi/Unit phân loại | Phase 3 |
| 5 | Native + test | song song với P3-P4 | header chuẩn, test xUnit cho Algorithms + 1 tool pilot | Phase 0 native gating |
| 6 | Rename | 3-5 ngày | `Propety→Property`, `Comunication→Communication`, `dataMat→DataMat` | Phase 2-4 đã ổn định |

**Mỗi phase phải build pass và CODEX_HISTORY.md được append section mới trước khi qua phase sau.**

Chi tiết Task Cards của từng phase ở mục 5.

---

## 5. Task Cards Chi Tiết Theo Phase

> Mỗi Task Card có format: **Mục tiêu / Preconditions / In-scope / Out-of-scope / Steps / Verify / DoD / Rollback / History entry**. Codex copy template ở mục 11 và điền vào `docs/architecture/tasks/YYYY-MM-DD-P<x>.<y>-<slug>.md` trước khi gõ code.

### 5.0 Phase 0 — Chuẩn bị

#### Task P0.1 — Tạo cấu trúc docs/

- **Mục tiêu**: tập trung tài liệu architecture vào 1 chỗ.
- **Preconditions**: `git status` sạch hoặc user xác nhận.
- **In-scope**: `CODEX_HISTORY.md`, `Pattern/PROMPT_CODEX_Preprocess.md`, `BeeCore/ShapeEditing/MigrationNotes.md`, tạo `docs/architecture/`.
- **Out-of-scope**: bất kỳ file `.cs`, `.cpp`, `.csproj`, `.vcxproj`.
- **Steps**:
  1. `mkdir docs && mkdir docs/architecture && mkdir docs/architecture/tasks && mkdir docs/architecture/adr`
  2. `git mv CODEX_HISTORY.md docs/architecture/CODEX_HISTORY.md`
  3. `git mv Pattern/PROMPT_CODEX_Preprocess.md docs/architecture/PROMPT_CODEX_Preprocess.md`
  4. `git mv BeeCore/ShapeEditing/MigrationNotes.md docs/architecture/ShapeEditing_MigrationNotes.md`
  5. Tạo `CODEX_HISTORY.md` ở root với 1 dòng: `> Moved to docs/architecture/CODEX_HISTORY.md`
  6. `grep -rn "CODEX_HISTORY.md" --include="*.md" --include="*.cs"` → sửa link nội bộ nếu có.
- **Verify**:
  - `ls docs/architecture/` chứa 3 file md.
  - `git diff --stat` cho thấy chỉ rename.
  - `MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` pass (doc move không ảnh hưởng build, nhưng vẫn chạy).
- **DoD**:
  - File ở root cũ là stub.
  - History entry "P0.1 Docs relocated" trong `docs/architecture/CODEX_HISTORY.md`.
- **Rollback**: `git reset --hard HEAD~1`.

#### Task P0.2 — Ghi baseline build

- **Mục tiêu**: có con số warning/error chuẩn để các phase sau so sánh.
- **Preconditions**: P0.1 done.
- **In-scope**: tạo `docs/architecture/baseline_build.md`, `docs/architecture/baseline_build.log`.
- **Out-of-scope**: sửa code build.
- **Steps**:
  1. `MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:detailed > docs/architecture/baseline_build.log 2>&1`
  2. `grep -cE ": warning " docs/architecture/baseline_build.log` → ghi vào md.
  3. `grep -cE ": error " docs/architecture/baseline_build.log` → ghi vào md.
  4. Liệt kê từng error theo project (ai block ai).
  5. Tạo `docs/architecture/baseline_build.md` với template:
     ```
     # Baseline Build — 2026-04-22
     - Command: MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64
     - Warning count: <N>
     - Error count: <M>
     - Per-project errors:
       - Pattern: <list>
       - BeeCV: <list>
       - BeeNativeOnnx: <list>
       - BeeNativeRCNN: <list>
     - Artifacts expected (Release/x64): BeeMain.exe, BeeCore.dll, BeeInterface.dll, ...
     ```
- **Verify**: `cat docs/architecture/baseline_build.md` đầy đủ section.
- **DoD**: file md committed; con số trở thành "warning count baseline" cho mọi phase sau.
- **Rollback**: `git reset --hard HEAD~1`.

#### Task P0.3 — Gate native blocker

- **Mục tiêu**: full build pass (số error = 0) bằng cách disable hoặc gate phần thiếu dependency.
- **Preconditions**: P0.2 done.
- **In-scope**: `Pattern.vcxproj`, `BeeCV/Resource.rc`, `EasyVision.sln` Configuration Manager, `Pattern/BarcodeCore.cpp` (chỉ wrap `#if`), `Pattern/RansacLineCore.cpp` (chỉ thay flag).
- **Out-of-scope**: logic native, `Pattern2.*`, `Pitch*`.
- **Steps**:

  | Blocker | Hành động | File |
  |---|---|---|
  | `Pattern/BarcodeCore.cpp` thiếu `ZXing/ReadBarcode.h` | Wrap toàn file bằng `#if defined(HAS_ZXING)` ... `#endif`; thêm `<PreprocessorDefinitions>HAS_ZXING=0;%(PreprocessorDefinitions)</PreprocessorDefinitions>` cho Release x64 trong `Pattern.vcxproj` | `Pattern/BarcodeCore.cpp`, `Pattern/Pattern.vcxproj` |
  | `Pattern/RansacLineCore.cpp` thiếu `std::clamp` | Thêm `<LanguageStandard>stdcpp17</LanguageStandard>` vào `Pattern.vcxproj` cho Release\|x64 và Debug\|x64 | `Pattern/Pattern.vcxproj` |
  | `BeeCV/Resource.rc` thiếu `VER_MAJOR` | Thêm `#define VER_MAJOR 1`, `#define VER_MINOR 0`, `#define VER_BUILD 0`, `#define VER_REVISION 0` ở đầu file | `BeeCV/Resource.rc` |
  | `BeeNativeOnnx`/`BeeNativeRCNN` thiếu OpenVINO | Mở `EasyVision.sln` trong VS → Configuration Manager → bỏ tick "Build" cho 2 project ở `Release\|x64` và `Debug\|x64`. Lưu sln. | `EasyVision.sln` |

  6. Chạy lại build full → ghi log mới `docs/architecture/baseline_build.md` thêm section "After native gating: error=0, warning=N2".

- **Verify**:
  - `grep -cE ": error " docs/architecture/baseline_build_after_gating.log` = 0.
  - `EasyVision.sln` có thể mở trong VS không lỗi.
- **DoD**: full build error = 0; warning count mới được ghi nhận làm baseline mới.
- **Rollback**: `git checkout HEAD -- Pattern/Pattern.vcxproj BeeCV/Resource.rc EasyVision.sln Pattern/BarcodeCore.cpp`.

#### Task P0.4 — Inventory file rơi ở root BeeCore

- **Mục tiêu**: bảng đầy đủ file root BeeCore + đích đề xuất + ref count để Phase 2 có thứ tự move tối ưu.
- **Preconditions**: P0.3 done.
- **In-scope**: tạo `docs/architecture/beecore_root_inventory.md`.
- **Out-of-scope**: move/sửa file BeeCore.
- **Steps**:
  1. Liệt kê file `.cs` ở **root** `BeeCore/` (không trong sub-folder, không trong `obj/`/`Properties/`):
     ```
     find BeeCore -maxdepth 1 -name "*.cs" -type f | sort > /tmp/root_files.txt
     ```
  2. Với mỗi file, đếm số ref bằng grep cross-project:
     ```
     grep -rln "<ClassName>" BeeCore BeeInterface BeeUi BeeMain BeeGlobal | wc -l
     ```
  3. Format bảng vào `beecore_root_inventory.md`:
     ```
     | File | LOC | Ref count | Đích đề xuất (theo mục 9) | Ghi chú |
     |---|---|---|---|---|
     | Vision.cs | 2100 | 340 | BeeCore.Vision | Move cuối Phase 2 |
     | dataMat.cs | 450 | 120 | BeeCore.Vision | Move cuối Phase 2; rename ở P6 |
     | Common.cs | 800 | 280 | BeeCore.Domain | Move đầu Phase 2 (P2.1) |
     | ... | | | | |
     ```
  4. Thứ tự move: **ref count thấp → cao** trong cùng nhóm Domain/Vision/Algorithm/...
- **Verify**: file md có ≥ 13 dòng (số file root hiện tại).
- **DoD**: Phase 2 dùng bảng này quyết định thứ tự move.
- **Rollback**: `git rm docs/architecture/beecore_root_inventory.md`.

#### Task P0.5 — Tag baseline + đóng phase

- **Mục tiêu**: ghim mốc git để rollback toàn bộ refactor về điểm này nếu cần.
- **Steps**:
  1. `git tag baseline-pre-refactor-2026-04-22 -m "Phase 0 done; baseline before structural refactor"`
  2. Append `CODEX_HISTORY.md` section "Phase 0 closed".
- **DoD**: `git tag --list "baseline-*"` show tag.

---

### 5.1 Phase 1 — Dọn duplicate (chưa đổi project)

#### Task P1.1 — Hợp nhất 3 `CustomGui.cs`

- **Mục tiêu**: chỉ còn 1 bản `CustomGui.cs` ở `BeeGlobal/Shared/CustomGui.cs` (placeholder cho `BeeShared.UI` sẽ tạo ở Phase 4).
- **Preconditions**: P0.5 done.
- **In-scope**: `BeeCore/CustomGui.cs`, `BeeCore/Func/CustomGui.cs`, `BeeInterface/CustomGui.cs`. Thư mục mới `BeeGlobal/Shared/`.
- **Out-of-scope**: refactor logic CustomGui; thêm method mới.
- **Steps**:
  1. Đọc hết 3 file → xác định symbol union (class, method, property). Ghi diff vào Task Card.
  2. `mkdir BeeGlobal/Shared` và tạo `BeeGlobal/Shared/CustomGui.cs` với union symbol. Namespace: `BeeGlobal.Shared`.
  3. Cập nhật `BeeGlobal.csproj` thêm `<Compile Include="Shared\CustomGui.cs" />`.
  4. Trong `BeeCore` và `BeeInterface`, thay `using <old namespace>` bằng `using BeeGlobal.Shared;`.
  5. `git rm BeeCore/CustomGui.cs BeeCore/Func/CustomGui.cs BeeInterface/CustomGui.cs`.
  6. Cập nhật `BeeCore.csproj`, `BeeInterface.csproj` xoá `<Compile Include>` tương ứng.
- **Verify**:
  - `find . -name "CustomGui.cs" -not -path "*/bin/*" -not -path "*/obj/*"` → 1 kết quả duy nhất.
  - Build pass; warning count ≤ baseline.
  - Smoke: mở app, mở 1 form bất kỳ có dùng CustomGui (ví dụ ToolEdit).
- **DoD**: 1 file CustomGui; build pass; smoke pass; history append.
- **Rollback**: `git reset --hard HEAD~1`.

#### Task P1.2 — Hợp nhất `Converts.cs`

- **Mục tiêu**: 1 bản duy nhất ở `BeeCore/Func/Converts.cs`.
- **In-scope**: `BeeCore/Converts.cs`, `BeeCore/Func/Converts.cs`.
- **Out-of-scope**: thay đổi behavior method.
- **Steps**: tương tự P1.1, đích là `BeeCore/Func/Converts.cs`. Xoá `BeeCore/Converts.cs` (root) sau khi merge.
- **Verify**: `find BeeCore -name "Converts.cs"` → 1 kết quả.
- **DoD**: build pass + smoke + history.

#### Task P1.3 — Move file root BeeCore vào sub-folder

- **Mục tiêu**: dọn sạch root `BeeCore/`, chuẩn bị Phase 2 (mỗi sub-folder = 1 sub-project tương lai).
- **In-scope**: theo bảng từ P0.4. Chỉ move trong nội bộ `BeeCore/`, **không** đổi namespace.
- **Out-of-scope**: tách project; đổi namespace.
- **Steps** (chia thành commit nhỏ — mỗi nhóm 1 commit):
  - Commit P1.3.a: `git mv BeeCore/Vision.cs BeeCore/Func/Vision.cs` (Vision.cs sẽ vào BeeCore.Vision ở P2)
  - Commit P1.3.b: `git mv BeeCore/dataMat.cs BeeCore/Func/dataMat.cs`
  - Commit P1.3.c: `git mv BeeCore/BitmapExtensions.cs BeeCore/Func/BitmapExtensions.cs`
  - Commit P1.3.d: `git mv BeeCore/MatrixExtension.cs BeeCore/Func/MatrixExtension.cs`
  - Commit P1.3.e: `git mv BeeCore/AddTool.cs BeeCore/Items/AddTool.cs`
  - Commit P1.3.f: `git mv BeeCore/LocalTool.cs BeeCore/Items/LocalTool.cs`
  - Commit P1.3.g: `git mv BeeCore/LabelItem.cs BeeCore/Items/LabelItem.cs`
  - Commit P1.3.h: `git mv BeeCore/ValueRobot.cs BeeCore/Items/ValueRobot.cs`
  - Commit P1.3.i: `git mv BeeCore/Actions.cs BeeCore/Func/Actions.cs`
  - Commit P1.3.j: `git mv BeeCore/KeepLargestAuto.cs BeeCore/Algorithm/KeepLargestAuto.cs`
  - Commit P1.3.k: `git mv BeeCore/LibreTranslateClient.cs BeeCore/Data/LibreTranslateClient.cs`
  - Commit P1.3.l: `git mv BeeCore/DB.cs BeeCore/Data/DB.cs`
  - Sau **mỗi commit**: cập nhật `BeeCore.csproj` `<Compile Include>` path mới + build verify.
- **Verify**:
  - `find BeeCore -maxdepth 1 -name "*.cs"` chỉ còn các file Properties (không còn root file business).
  - Mỗi commit: build pass.
- **DoD**: root `BeeCore/` chỉ còn `Properties/`, sub-folder, `BeeCore.csproj`. History append cho mỗi commit.

#### Task P1.4 — Quét + sửa event subscribe thiếu `-=`

- **Mục tiêu**: 100% subscribe có cặp `-=`/`+=`.
- **Steps**:
  1. `grep -rn "\.\(StatusToolChanged\|ScoreChanged\|PercentChange\|StatusChanged\|ItemChanged\) +=" --include="*.cs" BeeCore BeeInterface BeeUi > /tmp/subscribes.txt`
  2. Với mỗi dòng `+=`, kiểm tra trong cùng method có `-=` cùng Handler không. Nếu không có → thêm `obj.Event -= Handler;` ngay trên dòng `+=`.
  3. Đếm: `+=` count = `-=` count.
- **Verify**:
  - `grep -c "+=" /tmp/subscribes.txt` = `grep -c "-=" /tmp/refactored.txt`.
  - Build pass + smoke (mở/đóng 1 tool form 5 lần — không leak handler, kiểm tra bằng debug log nếu có).
- **DoD**: history append + cập nhật `CODEX_HISTORY.md` với event names đã chuẩn hoá.

#### Task P1.5 — CI guard cho `Common.PropetyTools[]`

- **Mục tiêu**: regex check chặn không cho call site mới.
- **Steps**:
  1. Tạo script `tools/check_propety_tools.sh`:
     ```bash
     #!/bin/bash
     count=$(grep -rn "Common\.PropetyTools\[" --include="*.cs" \
             --exclude-dir=bin --exclude-dir=obj \
             BeeCore BeeInterface BeeUi BeeMain BeeGlobal | \
             grep -v "Common.cs" | wc -l)
     if [ $count -gt 0 ]; then
       echo "ERROR: $count direct PropetyTools[] access outside Common.cs"
       grep -rn "Common\.PropetyTools\[" --include="*.cs" | grep -v "Common.cs"
       exit 1
     fi
     echo "OK: no direct PropetyTools[] access"
     ```
  2. Chạy `bash tools/check_propety_tools.sh` → expect "OK".
  3. Document trong `docs/architecture/conventions.md`: bắt buộc chạy script này trước commit Phase 1+.
- **Verify**: script return 0.
- **DoD**: script committed; convention doc.

---

### 5.2 Phase 2 — Tách BeeCore thành 7 sub-projects

> **Nguyên tắc bất biến cho Phase 2**: mỗi sub-project tách ra → BeeCore cũ trở thành **shim assembly** (chỉ có `[assembly: TypeForwardedTo(typeof(...))]` cho mọi type public bị move). Khi BeeCore không còn type của mình + không còn ai ref trực tiếp file của BeeCore → mới xoá BeeCore.

#### Task P2.0 — Setup shim infrastructure

- **Steps**:
  1. Tạo file `BeeCore/Properties/AssemblyAttributes.cs` (rỗng — sẽ thêm `TypeForwardedTo` ở mỗi sub-task).
  2. Tạo helper script `tools/gen_typeforward.ps1` nhận tham số `-Project <name>`:
     ```powershell
     param([string]$Project)
     $types = Get-ChildItem -Path $Project -Recurse -Filter "*.cs" |
              Select-String -Pattern "^\s*public (class|interface|enum|struct) (\w+)" |
              ForEach-Object { $_.Matches.Groups[2].Value }
     $types | ForEach-Object { "[assembly: TypeForwardedTo(typeof($Project.$_))]" } |
              Out-File "BeeCore/Properties/Forward_$Project.cs"
     ```

#### Task P2.1 — Tách `BeeCore.Domain`

- **Mục tiêu**: project mới `BeeCore.Domain.csproj` chứa Item models, `Propety`, `Propety2`, `PropetyTool`, `ToolState`, `Common.cs` (resolver), `ResultItem`, `LabelItem`, `ValueRobot`, `ItemTool`.
- **Preconditions**: P0-P1 done. `BeeCore.Domain` không phụ thuộc Algorithm/Camera/IO.
- **In-scope**: `BeeCore/Items/*`, `BeeCore/Common.cs`, `BeeCore/Func/ResultItem.cs`, `BeeCore/Items/LabelItem.cs`, `BeeCore/Items/ValueRobot.cs`, các file Propety nằm rải rác.
- **Out-of-scope**: type không thuần model (Algorithm, Vision, Camera).
- **Steps**:
  1. Tạo project: VS → Add → New Project → Class Library (.NET Framework 4.8) → tên `BeeCore.Domain`, location `BeeCore.Domain/`.
  2. Cấu hình target: `<TargetFrameworkVersion>v4.8</TargetFrameworkVersion>`, `<Platform>x64</Platform>`.
  3. Thêm reference: chỉ `BeeGlobal` (không OpenCvSharp, không Forms).
  4. Move file (mỗi file 1 commit nhỏ):
     - `git mv BeeCore/Common.cs BeeCore.Domain/Common.cs`
     - Tương tự cho từng file trong scope.
  5. Đổi namespace: thay `namespace BeeCore` → `namespace BeeCore.Domain` trong từng file move (sed: `sed -i 's/^namespace BeeCore$/namespace BeeCore.Domain/' <file>`).
     - **Cẩn trọng**: chỉ đổi namespace của type **mới move**, không đổi type còn ở BeeCore cũ.
     - Nếu file dùng `namespace BeeCore { ... }` block style thì sửa thủ công.
  6. Tạo shim trong `BeeCore`:
     - Cập nhật `BeeCore.csproj` thêm `<ProjectReference Include="..\BeeCore.Domain\BeeCore.Domain.csproj" />`.
     - Tạo `BeeCore/Properties/Forward_Domain.cs`:
       ```csharp
       using System.Runtime.CompilerServices;
       [assembly: TypeForwardedTo(typeof(BeeCore.Domain.Common))]
       [assembly: TypeForwardedTo(typeof(BeeCore.Domain.Propety))]
       [assembly: TypeForwardedTo(typeof(BeeCore.Domain.Propety2))]
       [assembly: TypeForwardedTo(typeof(BeeCore.Domain.PropetyTool))]
       [assembly: TypeForwardedTo(typeof(BeeCore.Domain.ToolState))]
       // ... mỗi type public
       ```
  7. Cập nhật `EasyVision.sln`: VS → Add Existing Project → `BeeCore.Domain.csproj`.
- **Verify**:
  - Build full pass.
  - `[assembly: TypeForwardedTo]` đầy đủ cho mọi public type đã move (kiểm: `grep "^public" BeeCore.Domain/*.cs | wc -l` = số dòng `TypeForwardedTo` trong `Forward_Domain.cs`).
  - Smoke: load 1 project mẫu (`ClassProject.json`) — không exception deserialize.
  - `tools/check_propety_tools.sh` pass.
- **DoD**: 1 project mới + shim hoạt động; smoke pass; history append.
- **Rollback**: revert commit, xoá project khỏi sln.

#### Task P2.2 — Tách `BeeCore.Algorithms`

- **Mục tiêu**: project mới chứa toàn bộ `BeeCore/Algorithm/*` (đã ở sub-folder, dễ tách).
- **In-scope**: `BeeCore/Algorithm/*.cs` (FilterItem, LineDetector, RansacCircleFitter, ImagePreprocessPipeline, EdgePoints, Gap, Geometry2D, DetectIntersect, RansacFitLine, FilletCornerMeasure, FilletCornerMeasure2, MonoSegmentation, InsertLine, Filters, Colors, KeepLargestAuto).
- **Out-of-scope**: file ngoài `Algorithm/`.
- **Reference**: `BeeGlobal`, `OpenCvSharp4`, `BeeCore.Domain` (cho ResultItem nếu cần).
- **Steps**: tương tự P2.1 — tạo project, move folder, đổi namespace `BeeCore.Algorithm` → `BeeCore.Algorithms`, thêm shim.
- **Verify**: build pass; smoke; check inspection 1 vòng OK.

#### Task P2.3 — Tách `BeeCore.Vision`

- **In-scope**: `BeeCore/Func/Vision.cs`, `dataMat.cs`, `MatHelper.cs`, `MatMerger.cs`, `Crop.cs`, `Draws.cs`, `ImageUtils.cs`, `BitmapExtensions.cs`, `MatrixExtension.cs`, `PolyOffset.cs`, `Line2DTransform.cs`, `RectRotateGapChecker.cs`, `FilterRect.cs`; toàn bộ `BeeCore/ShapeEditing/`.
- **Reference**: `BeeGlobal`, `OpenCvSharp4`, `BeeCore.Domain`.
- **Lưu ý**: `Vision.cs` và `dataMat.cs` ref count cao — move CUỐI cùng trong P2.3, sau khi các file nhỏ đã move xong và shim hoạt động.

#### Task P2.4 — Tách `BeeCore.Camera`

- **In-scope**: `BeeCore/Camera/*` (toàn bộ folder).
- **Reference**: `BeeGlobal`, `BeeCore.Domain`. Không OpenCvSharp.
- **Lưu ý**: `HEROJE.cs`, `USB.cs`, các CB struct — giữ namespace cũ nếu serialize qua wire (kiểm bằng grep).

#### Task P2.5 — Tách `BeeCore.IO`

- **In-scope**: `BeeCore/Data/*`, `DB.cs`, `LibreTranslateClient.cs`, các file save/load.
- **Reference**: `BeeGlobal`, `Newtonsoft.Json`, `BeeCore.Domain`.
- **CẢNH BÁO serialization**: `ClassProject.cs` là format file project khách hàng. **Bắt buộc** dùng `TypeForwardedTo` + test load 1 project mẫu trước khi commit.

#### Task P2.6 — Tách `BeeCore.Comm`

- **In-scope**: `BeeCore/EtherNetIP/*`, `Comunication.cs` (giữ tên cũ — rename Phase 6).
- **Reference**: `BeeGlobal`, `PLC_Communication`, `BeeCore.Domain`.

#### Task P2.7 — Tách `BeeCore.AI`

- **In-scope**: `BeeCore/Func/NativeYolo.cs`, `NativeRCNN.cs`, `Native.cs`, `Init.cs`, `Logs.cs`.
- **Reference**: `BeeGlobal`, `BeeCore.Domain`, native DLL (`BeeNativeOnnx`, `BeeNativeRCNN` — note: 2 native này đã exclude khỏi build ở P0.3).

#### Task P2.8 — Đóng phase: shim BeeCore còn lại

- **Mục tiêu**: BeeCore chỉ còn `Properties/Forward_*.cs` + `BeeCore.csproj` reference 7 sub-project.
- **Steps**:
  1. `find BeeCore -name "*.cs" -not -path "*/Properties/*" -not -path "*/bin/*" -not -path "*/obj/*"` → expect 0 file.
  2. Cập nhật `BeeCore.csproj`: chỉ còn `<ProjectReference>` cho 7 sub-project + `Forward_*.cs` files.
  3. Build full → mọi consumer cũ vẫn ref `BeeCore.dll` mà bên trong là forward.
- **Verify**: full build pass; smoke; ai consume `BeeCore.dll` vẫn chạy.
- **DoD**: BeeCore là pure shim. Phase 4 sẽ quyết định xoá luôn hay không.

---

### 5.3 Phase 3 — Tách Tool layer (chuẩn hoá GUI)

> Chi tiết khung GUI + danh sách tool ở **mục 10**. Phase 3 task cards ở đây chỉ là cấu trúc lịch.

#### Task P3.0 — Tạo `Tool.Common` (base classes)

- **In-scope**: tạo project mới `Tool.Common/` chứa `IToolEngine.cs`, `IToolConfig.cs`, `ToolEngineBase.cs`, `ToolFormBase.cs`, `ToolBinder.cs`, `ToolEngineFactory.cs`, `ToolCustomLayoutAttribute.cs`.
- **Reference**: `BeeCore.Domain`, `BeeCore.Vision`, `System.Windows.Forms`.
- **Code template**: xem mục 10.10.
- **Verify**: project build pass standalone.

#### Task P3.0.1 — Tạo UserControl chung trong `BeeShared.UI/Controls/`

- **In-scope**: 5 control bắt buộc cho pilot Nhóm A: `RoiSelectorControl`, `ThresholdRangeControl`, `ResultTable`, `ImagePreviewControl`, `ScoreBar`.
- **Out-of-scope**: control của Nhóm B (`PresetCombo`, `AngleControl`, `ColorPickerControl`) — làm khi tới Nhóm B.

#### Task P3.1 — Pilot Tool.Circle

- **Mục tiêu**: chứng minh khung chuẩn fit. Step-by-step ở **mục 10.12**.
- **Exit criteria**: smoke pass + 1 unit test cho `CircleEngine` xanh + Designer mở được trong VS.
- **Stop-and-ask** nếu effort > 2 ngày.

#### Task P3.2 — Pilot Tool.Width

- Lặp lại P3.1 cho `ToolWidth`. So sánh thời gian với P3.1 — nếu lệch > 1.5x, review khung.

#### Task P3.3 — Bulk Nhóm A (10 tool còn lại)

- 10 tool: `Measure`, `Edge`, `EdgePixel`, `Corner`, `Crop`, `Intersect`, `ColorArea`, `Counter`, `CheckMissing`, `PositionAdjustment`.
- Mỗi tool 1 commit, theo checklist 10.7.
- Có thể song song 2-3 tool nếu effort thấp.

#### Task P3.4 — Nhóm B (7 tool)

- Bổ sung UserControl còn thiếu trước (`PresetCombo`, `AngleControl`, `ColorPickerControl`, `ReferenceCombo`).
- Pilot: `ToolBarcode` (đơn giản nhất Nhóm B).
- Sau pilot, làm song song theo độ phức tạp tăng dần: `OCR` → `CraftOCR` → `Pattern` → `MultiPattern` → `MatchingShape` → `Pitch`.

#### Task P3.5 — Review Nhóm C

- 5 tool: `Yolo`, `MultiOnnx`, `OKNG`, `AutoTrig`, `VisualMatch`.
- Mỗi tool: hỏi user 3 lựa chọn — giữ nguyên / refactor riêng / deprecate.
- Tool quyết định "giữ nguyên": thêm `[ToolCustomLayout("reason")]` lên class.
- Tool quyết định "deprecate": thêm `[Obsolete("Will be removed in vX. Reason: ...")]`.
- KHÔNG ép vào khung.

---

### 5.4 Phase 4 — Dọn UI shell

#### Task P4.1 — Move Custom controls vào `BeeShared.UI`

- **In-scope**: `BeeInterface/CustomGui.cs` (đã merge ở P1.1), `ConvertImg.cs`, `LayoutPersistence.cs`, `ControlStylePersistence.cs`, `Custom/RoundedPanel.cs`, `Custom/GradientTabControl.cs`, `Custom/DbTableLayoutPanel.cs`.
- **Steps**: move + đổi namespace + shim ở `BeeInterface` nếu cần.

#### Task P4.2 — Xử lý `BeeUi/Unit/`

- Đọc từng file → quyết định: vào `BeeCore.Vision` (image utility) / `Tool.<x>` (specific) / `BeeShared.UI` (UI control).
- Move theo quyết định.

#### Task P4.3 — Hợp nhất Global state

- 3 nguồn `Global.cs` hiện tại → 3 nơi rõ ràng:
  - `BeeShared.UI/UiState.cs`: state UI (current form, selected tool ID).
  - `BeeCore.Domain/RuntimeState.cs`: state runtime (current program, current threading).
  - `BeeGlobal/Config.cs`: parameter cấu hình (đã có).
- Mỗi static field di chuyển theo phân loại trên.

---

### 5.5 Phase 5 — Native + test (song song với P3-P4)

#### Task P5.1 — Pattern/BeeCV de-duplicate

- Đọc danh sách function exported của `Pattern.dll` và `BeeCV.dll`.
- Identify chồng chéo (capture, shape utility).
- Move sang 1 bên — bên còn lại re-export forward (`extern "C" __declspec(dllexport)` proxy).

#### Task P5.2 — Bật C++17 cho Pattern + BeeCV

- Đã làm cho Pattern ở P0.3. Lặp lại cho BeeCV.

#### Task P5.3 — Header chuẩn cho mỗi DLL

- 1 file header export chính: `Pattern/BeeCpp.h`, `BeeCV/BeeCV.h`, `BeeNativeOnnx/BeeNativeOnnx.h`.
- C# P/Invoke chỉ qua header này.

#### Task P5.4 — Tạo test infrastructure

- Tạo `tests/` solution folder.
- 3 project xUnit ban đầu:
  - `BeeCore.Algorithms.Tests/` — test pure algorithm functions.
  - `Tool.Circle.Tests/` — test pilot tool engine.
  - `BeeCore.Domain.Tests/` — test resolver helpers (`Common.TryGetTool`, ...).
- Mỗi project tối thiểu 5 test case.

---

### 5.6 Phase 6 — Rename (chỉ làm sau P2-P5 ổn định)

> Mỗi rename là **1 PR riêng**. Có git tag mốc trước khi bắt đầu.

#### Task P6.1 — Rename `Propety` family

- `Propety` → `Property`, `Propety2` → `Property2`, `PropetyTool` → `PropertyTool`.
- `Common.PropetyTools` → `Common.PropertyTools`. Helper `TryGetTool`/... giữ tên (đã đúng).
- **Cách làm**: VS IDE Rename refactor (F2) — tự update mọi reference, designer, XML doc.
- **Sau rename**: chạy `tools/check_propety_tools.sh` đổi tên thành `check_property_tools.sh`. Update CI guard.
- **Backward-compat**: thêm shim type alias trong `BeeCore.Domain`:
  ```csharp
  [Obsolete("Use Property")] public class Propety : Property { }
  ```

#### Task P6.2 — Rename `Comunication` → `Communication`

- File: `BeeCore.Comm/Comunication.cs` → `Communication.cs`.
- Class: cùng tên file.

#### Task P6.3 — Rename `dataMat` → `DataMat`

- File + class.

#### Task P6.4 — Rename file underscore

- `Position_Adjustment` → `PositionAdjustment`.
- `KEY_Send` → `KeySend`.

---

## 6. Rủi Ro & Biện Pháp

| Rủi ro | Khả năng | Tác động | Giảm thiểu |
|---|---|---|---|
| Designer.cs mất reference khi move file | Cao | Form không mở được trong VS | Move bằng VS giữ `.resx`/`.Designer.cs`/`.cs` cùng nhau. Test mở từng Form sau move. |
| `[assembly: TypeForwardedTo]` không hoạt động cho type internal | Trung bình | Plugin/call site cũ break | Phase 2 chỉ forward type `public`. Type `internal` move trực tiếp. |
| Native build phụ thuộc OpenVINO/ZXing chưa có | Cao | Full build vẫn fail | P0.3 đã gate. Khi user cung cấp header → revert gate. |
| Project hiện tại có nhiều "local edit" của user | Cao | Mất công việc đang dở | Trước mỗi Task Card hỏi user `git status`. Không revert/format file ngoài scope. |
| Đổi namespace làm vỡ serialization (`ClassProject`) | Trung bình | File project khách hàng không load | Giữ namespace cũ qua `[assembly: TypeForwardedTo]` hoặc `JsonConverter` mapping. P2.5 bắt buộc test load project mẫu. |
| Refactor song song Pattern2 (đang phát triển) | Trung bình | Conflict | KHÔNG động `Pattern/Pattern2.*`, `Pattern/Pitch*` trong Phase 1-2. |
| Pilot tool effort vượt timebox | Trung bình | Block toàn bộ Phase 3 | Stop-and-ask sau 2 ngày. Có thể downgrade tool xuống Nhóm C. |
| Type forward lan rộng làm BeeCore.dll quá phụ thuộc | Thấp | Build chậm | Cuối Phase 4 đánh giá xoá BeeCore shim hay giữ. |

---

## 7. Tiêu Chí Chấp Nhận (Definition of Done) Cho Mỗi Task Card

Mọi Task Card phải pass đủ 5 criteria:

1. **Build**: `MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64` pass; warning ≤ baseline trong `docs/architecture/baseline_build.md`.
2. **Smoke**: mở app → mở 1 project mẫu → chạy 1 vòng inspection trên ảnh tĩnh → không exception.
3. **Convention**: 
   - `bash tools/check_propety_tools.sh` return 0.
   - Event subscribe `+=` count = `-=` count cho event đã chuẩn hoá.
   - Code mới: namespace đúng theo mục 3, không `using BeeCore` directly nếu type đã move sang sub-project.
4. **History**: `docs/architecture/CODEX_HISTORY.md` có section mới với template ở mục 11.2 (Scope / Files touched / Verify / Notes / Blockers).
5. **No unrelated changes**: `git diff --name-only HEAD~1` chỉ chứa file trong "In-scope" của Task Card.

---

## 8. Quy Tắc Làm Việc Cho Agent (Tóm Tắt)

Đã chi tiết ở mục 0. Tóm tắt 1 dòng:

1. Đọc `CODEX_HISTORY.md` + CLAUDE.md mục 0 trước.
2. 1 Task Card = 1 commit.
3. Namespace mới theo mục 3; không đổi namespace file cũ ngoài scope phase tương ứng.
4. State global qua `BeeCore.Domain.Common.*`. Cấm singleton mới ở UI.
5. Event `-=` trước `+=`.
6. `git mv` để giữ history. `.csproj` update cùng commit.
7. Helper Shared: UI → `BeeShared.UI`; non-UI → `BeeGlobal`. Không thêm vào `BeeCore`.
8. Xong append `CODEX_HISTORY.md`.
9. CLAUDE.md sai → sửa CLAUDE.md + ghi lý do `CODEX_HISTORY.md`.

---

## 9. Phụ Lục — Bảng Ánh Xạ File ⇒ Đích

| File hiện tại | Đích đề xuất | Phase |
|---|---|---|
| `BeeCore/Vision.cs` | `BeeCore.Vision/Vision.cs` | P2.3 |
| `BeeCore/dataMat.cs` | `BeeCore.Vision/DataMat.cs` (rename P6) | P2.3 |
| `BeeCore/Common.cs` | `BeeCore.Domain/Common.cs` | P2.1 |
| `BeeCore/Items/*` | `BeeCore.Domain/Items/*` | P2.1 |
| `BeeCore/Func/Cal.cs`, `MatHelper.cs`, `ImageUtils.cs`, `Crop.cs`, `Draws.cs`, `MatMerger.cs`, `PolyOffset.cs`, `Line2DTransform.cs`, `RectRotateGapChecker.cs`, `FilterRect.cs` | `BeeCore.Vision/` | P2.3 |
| `BeeCore/Func/NativeYolo.cs`, `NativeRCNN.cs`, `Native.cs` | `BeeCore.AI/` | P2.7 |
| `BeeCore/Func/Init.cs`, `Logs.cs`, `General.cs`, `TarProgramHelper.cs`, `ComboBoxExtensions.cs`, `CodeSymbologyCliExtensions.cs`, `ResultItemHelper.cs`, `ResultMulti.cs`, `ResultFilter.cs`, `Converts.cs` | Phân phối: helper UI → `BeeShared.UI`; logic kết quả → `BeeCore.Domain`; init → `BeeCore.AI`/`BeeCore.IO` tuỳ nội dung | P2.x |
| `BeeCore/Algorithm/*` | `BeeCore.Algorithms/` | P2.2 |
| `BeeCore/Camera/*`, `HEROJE.cs` | `BeeCore.Camera/` | P2.4 |
| `BeeCore/Data/*`, `DB.cs`, `LibreTranslateClient.cs` | `BeeCore.IO/` | P2.5 |
| `BeeCore/EtherNetIP/*`, `Comunication.cs` | `BeeCore.Comm/` | P2.6 |
| `BeeCore/Unit/*` | Phần lớn → `Tool.Pattern/Tool.Pitch/...` tuỳ ngữ cảnh; còn lại → `BeeCore.Vision` | P3 |
| `BeeCore/ShapeEditing/*` | `BeeCore.Vision/ShapeEditing/` | P2.3 |
| `BeeCore/Parameter/G.cs` | `BeeGlobal/G.cs` (gộp với Para khác) | P4.3 |
| `BeeCore/CustomGui.cs`, `BeeCore/Func/CustomGui.cs`, `BeeInterface/CustomGui.cs` | `BeeShared.UI/CustomGui.cs` (1 bản) | P1.1 → P4.1 |
| `BeeCore/AddTool.cs`, `LocalTool.cs`, `Actions.cs`, `KeepLargestAuto.cs`, `LabelItem.cs`, `ValueRobot.cs`, `BitmapExtensions.cs`, `MatrixExtension.cs`, `Converts.cs` | Phân loại theo ngữ cảnh; mặc định Vision/Domain | P1.3, P2.x |
| `BeeInterface/Tool/ToolXxx.{cs,Designer.cs,resx}` | `Tools/Tool.Xxx/` | P3 |
| `BeeInterface/CustomGui.cs`, `ConvertImg.cs`, `LayoutPersistence.cs`, `ControlStylePersistence.cs`, `Custom/*` | `BeeShared.UI/` | P4.1 |

---

## 10. Chuẩn Hoá GUI Tool Theo Module

> Mục này mở rộng Phase 3. Mục tiêu: gom 24 `ToolXxx` về một khung GUI chung; tool không fit ⇒ tạm bỏ qua có ghi chú.

### 10.1 Vấn đề hiện tại

24 `ToolXxx.cs` ở `BeeInterface/Tool/` đều là Form riêng, mỗi form tự design layout, tự bind config → không thống nhất. Các khối lặp lại (chọn ROI, slider ngưỡng, bảng kết quả, preview, nút Test/Save) — mỗi Form tự code thô. Engine bị nhồi vào Form ⇒ không thể unit-test.

### 10.2 Mục tiêu

- Mỗi tool = **module độc lập** gồm 3 file: `XxxConfig.cs`, `XxxEngine.cs`, `XxxForm.{cs,Designer.cs,resx}`.
- Form kế thừa **`ToolFormBase`** với 5 vùng cố định.
- UserControl dùng chung ở `BeeShared.UI/Controls/`.
- Tool không fit ⇒ tạm skip có lý do (Nhóm C).

### 10.3 Khung chuẩn `ToolFormBase` (layout cố định)

```
┌───────────────────────────────────────────┐
│ Header (60px)                             │  Tool name, Enable, IndexProg/IndexTool
├───────────────────────────────────────────┤
│ ROI / Reference (120px, collapsible)      │  RoiSelectorControl + ReferenceCombo
├───────────────────────────────────────────┤
│ Params (Fill)                             │  Vùng nghiệp vụ — tool-specific
├───────────────────────────────────────────┤
│ Preview (280px)                           │  ImageBox + overlay + ResultTable
├───────────────────────────────────────────┤
│ Actions (50px)                            │  Test | Apply | Reset | Close
└───────────────────────────────────────────┘
```

### 10.4 Thư viện UserControl dùng chung (`BeeShared.UI/Controls/`)

| Control | Dùng cho tool | Thay thế trong |
|---|---|---|
| `RoiSelectorControl` | Rect / rotated / poly ROI | Mọi tool có `Mask`, `ShapeEditing` |
| `ReferenceCombo` | Chọn tool reference align | PositionAdjustment, Pattern, Pitch |
| `ThresholdRangeControl` | Min/Max slider + entry | Edge, EdgePixel, ColorArea, CheckMissing |
| `ScoreBar` | Thanh score + threshold line | Pattern, MatchingShape, Yolo |
| `ResultTable` | Grid kết quả (Name/Value/OK) | Mọi tool có kết quả số |
| `PresetCombo` | Dropdown preset + Save/Delete | Pattern2, OCR, Barcode |
| `ImagePreviewControl` | ImageBox + overlay, zoom đồng bộ | Thay `Cyotek.ImageBox` thô |
| `AngleControl` | Trackbar góc + numeric ± | Pattern, Pitch, MatchingShape |
| `ColorPickerControl` | HSV range picker | ColorArea, RGB |

Quy tắc: control vào library khi **≥ 3 tool** dùng. Control 1-2 tool dùng giữ trong `Tool.Xxx/Controls/`.

### 10.5 Phân loại tool theo độ sẵn sàng

**Nhóm A — Fit khung chuẩn ngay** (12 tool):
`ToolCircle`, `ToolWidth`, `ToolMeasure`, `ToolEdge`, `ToolEdgePixel`, `ToolCorner`, `ToolCrop`, `ToolIntersect`, `ToolColorArea`, `ToolCounter`, `ToolCheckMissing`, `ToolPosition_Adjustment`.

**Nhóm B — Fit nhưng cần thêm UserControl** (7 tool):

| Tool | Thiếu gì |
|---|---|
| `ToolPattern` | `PresetCombo` (5 preset), `AngleControl`, `ScoreBar` |
| `ToolMultiPattern` | Như Pattern + DataGrid pattern con |
| `ToolMatchingShape` | `ShapeEditing` + `ScoreBar` |
| `ToolPitch` | `PitchGdiPainter` thành UserControl |
| `ToolBarcode` | Symbology combo |
| `ToolOCR` | Preset + dictionary picker |
| `ToolCraftOCR` | Preset + model path |

**Nhóm C — Tạm bỏ qua** (5 tool):

| Tool | Lý do tạm skip | Khi nào xử lý lại |
|---|---|---|
| `ToolYolo` | Label editor + training UI gắn liền | Sau khi tách label editor ra UserControl riêng |
| `ToolMultiOnnx` | Multi-model list | Cùng đợt Yolo |
| `ToolOKNG` | Native OKNG API đóng gói chặt | Sau khi có `OKNG.h` chuẩn hoá |
| `ToolAutoTrig` | Trigger logic + timer, không có preview ảnh | Tách thành "Trigger module" riêng |
| `ToolVisualMatch` | Legacy, chưa rõ còn dùng | Confirm với user; nếu deprecated → `[Obsolete]` |

### 10.6 Thứ tự triển khai

1. **10.6.1** Base + UserControl (Task P3.0, P3.0.1) — 1 tuần.
2. **10.6.2** Pilot Tool.Circle (P3.1) — 2 ngày.
3. **10.6.3** Pilot Tool.Width (P3.2) — 1.5 ngày (đã có template từ Circle).
4. **10.6.4** Bulk Nhóm A (P3.3) — 1.5 tuần (10 tool, ~1 ngày/tool).
5. **10.6.5** Nhóm B (P3.4) — 2 tuần.
6. **10.6.6** Review Nhóm C (P3.5) — 2 ngày (review only).

### 10.7 Checklist cho mỗi tool migrate

```
[ ] Đọc ToolXxx.cs hiện tại, list field config + logic Run/Save/Load
[ ] Tạo Tool.Xxx/XxxConfig.cs - POCO, copy field
[ ] Tạo Tool.Xxx/XxxEngine.cs - logic, KHÔNG ref System.Windows.Forms
[ ] Tạo Tool.Xxx/XxxForm.cs kế thừa ToolFormBase, override BuildParamsPanel()
[ ] Move Designer.cs + .resx vào Tool.Xxx/ qua VS Solution Explorer
[ ] Đổi namespace ToolXxx → Tool.Xxx (sed)
[ ] Update Common.TryGetTool ở mọi call site (không truy cập PropetyTools[..][..] trực tiếp)
[ ] Event -= trước += cho mọi subscribe
[ ] Tạo ToolEngineFactory entry: factory.Register("Circle", () => new CircleEngine());
[ ] Build pass + warning ≤ baseline
[ ] Mở Form trong VS Designer không lỗi
[ ] Chạy 1 inspection trên ảnh tĩnh không exception
[ ] Viết ≥ 1 unit test cho XxxEngine ở tests/Tool.Xxx.Tests/
[ ] Append CODEX_HISTORY.md (template 11.2)
[ ] Commit message: [P3.<n>] Migrate ToolXxx to Tool.Xxx module
```

### 10.8 Tiêu chí dừng / rollback

- Sau 9.6.2 + 9.6.3 (2 pilot) effort > 2 ngày/tool ⇒ dừng, review khung.
- Nhóm A có > 30% tool "không fit" ⇒ giả thiết sai, redesign `ToolFormBase`.
- Nhóm C sau review > 5 tool ⇒ chấp nhận 2 lớp tool song song (chuẩn + custom).

### 10.9 Tool không migrate được — quy ước

- KHÔNG xoá Form cũ.
- Thêm attribute `[ToolCustomLayout("reason")]` lên class form (định nghĩa ở `Tool.Common/ToolCustomLayoutAttribute.cs`).
- Cập nhật bảng 10.5 cột "Khi nào xử lý lại" mỗi review.
- Vẫn phải tuân nguyên tắc non-GUI: `Common.TryGetTool`, event `-=` trước `+=`.

### 10.10 Code template — `Tool.Common`

**File `Tool.Common/IToolConfig.cs`**:
```csharp
namespace Tool.Common
{
    public interface IToolConfig
    {
        string ToolKind { get; }
        bool Enabled { get; set; }
        IToolConfig Clone();
    }
}
```

**File `Tool.Common/IToolEngine.cs`**:
```csharp
using OpenCvSharp;

namespace Tool.Common
{
    public interface IToolEngine
    {
        string ToolKind { get; }
        IToolResult Run(IToolConfig config, Mat image);
        void Save(IToolConfig config);
        void Load(IToolConfig config);
    }

    public interface IToolResult
    {
        bool IsOk { get; }
        double Score { get; }
        Mat Overlay { get; }    // có thể null
    }
}
```

**File `Tool.Common/ToolEngineBase.cs`**:
```csharp
using OpenCvSharp;
using BeeCore.Domain;

namespace Tool.Common
{
    public abstract class ToolEngineBase : IToolEngine
    {
        public abstract string ToolKind { get; }

        public abstract IToolResult Run(IToolConfig config, Mat image);

        public virtual void Save(IToolConfig config)
        {
            // Default: ghi config về PropetyTool tương ứng qua Common.TryGetTool
            if (Common.TryGetTool(out var tool) && tool != null)
            {
                tool.Config = config;
            }
        }

        public virtual void Load(IToolConfig config)
        {
            if (Common.TryGetTool(out var tool) && tool?.Config is IToolConfig src)
            {
                config = src.Clone();
            }
        }
    }
}
```

**File `Tool.Common/ToolFormBase.cs`** (skeleton):
```csharp
using System.Windows.Forms;
using BeeShared.UI.Controls;

namespace Tool.Common
{
    public partial class ToolFormBase : Form
    {
        private TableLayoutPanel _root;
        protected Panel HeaderPanel { get; private set; }
        protected Panel RoiPanel { get; private set; }
        protected Panel ParamsPanel { get; private set; }
        protected Panel PreviewPanel { get; private set; }
        protected Panel ActionsPanel { get; private set; }

        protected ImagePreviewControl Preview { get; private set; }
        protected ResultTable Results { get; private set; }
        protected RoiSelectorControl RoiSelector { get; private set; }

        protected abstract IToolEngine Engine { get; }
        protected IToolConfig Config { get; set; }

        public ToolFormBase()
        {
            InitializeBaseLayout();
        }

        private void InitializeBaseLayout()
        {
            _root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 5, ColumnCount = 1 };
            _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));   // Header
            _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));  // ROI
            _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));   // Params
            _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 280));  // Preview
            _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));   // Actions

            HeaderPanel = BuildHeader();
            RoiPanel = BuildRoi();
            ParamsPanel = (Panel)BuildParamsPanel();
            PreviewPanel = BuildPreview();
            ActionsPanel = BuildActions();

            _root.Controls.Add(HeaderPanel, 0, 0);
            _root.Controls.Add(RoiPanel, 0, 1);
            _root.Controls.Add(ParamsPanel, 0, 2);
            _root.Controls.Add(PreviewPanel, 0, 3);
            _root.Controls.Add(ActionsPanel, 0, 4);
            this.Controls.Add(_root);
        }

        protected abstract Control BuildParamsPanel();
        protected abstract void BindToConfig(IToolConfig config);

        protected virtual Panel BuildHeader() { /* tool name, enable, indexProg/Tool */ return new Panel(); }
        protected virtual Panel BuildRoi()
        {
            var p = new Panel { Dock = DockStyle.Fill };
            RoiSelector = new RoiSelectorControl { Dock = DockStyle.Fill };
            p.Controls.Add(RoiSelector);
            return p;
        }
        protected virtual Panel BuildPreview()
        {
            var p = new Panel { Dock = DockStyle.Fill };
            Preview = new ImagePreviewControl { Dock = DockStyle.Left, Width = 400 };
            Results = new ResultTable { Dock = DockStyle.Fill };
            p.Controls.Add(Results);
            p.Controls.Add(Preview);
            return p;
        }
        protected virtual Panel BuildActions()
        {
            var p = new Panel { Dock = DockStyle.Fill };
            var btnTest = new Button { Text = "Test", Dock = DockStyle.Right };
            btnTest.Click -= OnTestClicked; btnTest.Click += OnTestClicked;
            var btnApply = new Button { Text = "Apply", Dock = DockStyle.Right };
            btnApply.Click -= OnApplyClicked; btnApply.Click += OnApplyClicked;
            p.Controls.Add(btnApply);
            p.Controls.Add(btnTest);
            return p;
        }

        protected virtual void OnTestClicked(object sender, System.EventArgs e)
        {
            var image = Preview.CurrentImage;
            if (image == null) return;
            var result = Engine.Run(Config, image);
            Preview.SetOverlay(result.Overlay);
            Results.SetScore(result.Score, result.IsOk);
        }

        protected virtual void OnApplyClicked(object sender, System.EventArgs e)
        {
            Engine.Save(Config);
            this.Close();
        }
    }
}
```

**File `Tool.Common/ToolEngineFactory.cs`**:
```csharp
using System;
using System.Collections.Generic;

namespace Tool.Common
{
    public static class ToolEngineFactory
    {
        private static readonly Dictionary<string, Func<IToolEngine>> _registry = new();

        public static void Register(string toolKind, Func<IToolEngine> factory)
        {
            _registry[toolKind] = factory;
        }

        public static IToolEngine Create(string toolKind)
        {
            if (_registry.TryGetValue(toolKind, out var f)) return f();
            throw new InvalidOperationException($"No engine registered for {toolKind}");
        }

        public static bool IsRegistered(string toolKind) => _registry.ContainsKey(toolKind);
    }
}
```

**File `Tool.Common/ToolCustomLayoutAttribute.cs`**:
```csharp
using System;

namespace Tool.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ToolCustomLayoutAttribute : Attribute
    {
        public string Reason { get; }
        public ToolCustomLayoutAttribute(string reason) { Reason = reason; }
    }
}
```

### 10.11 Code template — Tool module (ví dụ Tool.Circle)

**File `Tool.Circle/CircleConfig.cs`**:
```csharp
using OpenCvSharp;
using Tool.Common;

namespace Tool.Circle
{
    public sealed class CircleConfig : IToolConfig
    {
        public string ToolKind => "Circle";
        public bool Enabled { get; set; } = true;

        public Rect Roi { get; set; }
        public int CannyLow { get; set; } = 50;
        public int CannyHigh { get; set; } = 150;
        public double MinRadius { get; set; } = 10;
        public double MaxRadius { get; set; } = 200;
        public double ScoreThreshold { get; set; } = 0.7;

        public IToolConfig Clone() => (CircleConfig)this.MemberwiseClone();
    }
}
```

**File `Tool.Circle/CircleEngine.cs`**:
```csharp
using OpenCvSharp;
using Tool.Common;

namespace Tool.Circle
{
    public sealed class CircleEngine : ToolEngineBase
    {
        public override string ToolKind => "Circle";

        public override IToolResult Run(IToolConfig config, Mat image)
        {
            var c = (CircleConfig)config;
            var roi = new Mat(image, c.Roi);
            var gray = new Mat();
            Cv2.CvtColor(roi, gray, ColorConversionCodes.BGR2GRAY);

            var circles = Cv2.HoughCircles(gray, HoughModes.Gradient, 1.0, 20,
                                           c.CannyHigh, c.CannyHigh / 2,
                                           (int)c.MinRadius, (int)c.MaxRadius);

            var overlay = roi.Clone();
            foreach (var ci in circles)
                Cv2.Circle(overlay, (int)ci.Center.X, (int)ci.Center.Y, (int)ci.Radius, Scalar.Lime, 2);

            var score = circles.Length > 0 ? 1.0 : 0.0;
            return new CircleResult
            {
                IsOk = score >= c.ScoreThreshold,
                Score = score,
                Overlay = overlay,
            };
        }
    }

    public sealed class CircleResult : IToolResult
    {
        public bool IsOk { get; set; }
        public double Score { get; set; }
        public Mat Overlay { get; set; }
    }
}
```

**File `Tool.Circle/CircleForm.cs`**:
```csharp
using System.Windows.Forms;
using Tool.Common;

namespace Tool.Circle
{
    public partial class CircleForm : ToolFormBase
    {
        private readonly CircleEngine _engine = new CircleEngine();
        private CircleParamsControl _params;

        protected override IToolEngine Engine => _engine;

        public CircleForm()
        {
            // ToolFormBase ctor đã build layout
        }

        protected override Control BuildParamsPanel()
        {
            _params = new CircleParamsControl { Dock = DockStyle.Fill };
            return _params;
        }

        protected override void BindToConfig(IToolConfig config)
        {
            Config = config;
            _params.BindTo((CircleConfig)config);
        }
    }
}
```

**File `Tool.Circle/Controls/CircleParamsControl.cs`** (UserControl riêng, simple):
```csharp
using System.Windows.Forms;
using BeeShared.UI.Controls;

namespace Tool.Circle
{
    public partial class CircleParamsControl : UserControl
    {
        private ThresholdRangeControl _canny;
        private ThresholdRangeControl _radius;
        private NumericUpDown _scoreThreshold;
        private CircleConfig _config;

        public CircleParamsControl()
        {
            InitializeComponent();
            // Hoặc build động:
            _canny = new ThresholdRangeControl { Caption = "Canny Low/High", Min = 0, Max = 255 };
            _radius = new ThresholdRangeControl { Caption = "Radius Min/Max", Min = 1, Max = 500 };
            _scoreThreshold = new NumericUpDown { Minimum = 0, Maximum = 1, DecimalPlaces = 2, Increment = 0.05M };

            _canny.ValueChanged -= OnCannyChanged; _canny.ValueChanged += OnCannyChanged;
            _radius.ValueChanged -= OnRadiusChanged; _radius.ValueChanged += OnRadiusChanged;
            _scoreThreshold.ValueChanged -= OnScoreChanged; _scoreThreshold.ValueChanged += OnScoreChanged;
        }

        public void BindTo(CircleConfig c)
        {
            _config = c;
            _canny.LowValue = c.CannyLow;
            _canny.HighValue = c.CannyHigh;
            _radius.LowValue = (int)c.MinRadius;
            _radius.HighValue = (int)c.MaxRadius;
            _scoreThreshold.Value = (decimal)c.ScoreThreshold;
        }

        private void OnCannyChanged(object s, System.EventArgs e)
        {
            if (_config == null) return;
            _config.CannyLow = _canny.LowValue;
            _config.CannyHigh = _canny.HighValue;
        }
        private void OnRadiusChanged(object s, System.EventArgs e)
        {
            if (_config == null) return;
            _config.MinRadius = _radius.LowValue;
            _config.MaxRadius = _radius.HighValue;
        }
        private void OnScoreChanged(object s, System.EventArgs e)
        {
            if (_config == null) return;
            _config.ScoreThreshold = (double)_scoreThreshold.Value;
        }
    }
}
```

**Đăng ký factory** (gọi 1 lần ở app startup, ví dụ `BeeMain/Program.cs`):
```csharp
ToolEngineFactory.Register("Circle", () => new CircleEngine());
```

### 10.12 Pilot walkthrough chi tiết — Tool.Circle (P3.1)

**Step 1**: Đọc `BeeInterface/Tool/ToolCircle.cs` + `.Designer.cs`. List:
- Field config: ROI, Canny low/high, Radius min/max, score threshold.
- Logic Run: gọi `Cv2.HoughCircles`, vẽ overlay, set kết quả.
- Logic Save/Load: đọc/ghi `Propety2` field.

**Step 2**: Tạo project `Tool.Circle`:
- VS → Add → New Project → Class Library (.NET Framework 4.8) → name `Tool.Circle`, location `Tools/Tool.Circle/`.
- Add reference: `BeeCore.Domain`, `BeeCore.Vision`, `BeeShared.UI`, `Tool.Common`, `OpenCvSharp4`, `System.Windows.Forms`, `System.Drawing`.

**Step 3**: Tạo 3 file theo template 10.11. Copy field từ `ToolCircle.cs` cũ vào `CircleConfig.cs`. Copy logic vào `CircleEngine.cs`.

**Step 4**: Move Designer + .resx (nếu giữ Designer):
- VS Solution Explorer → drag `BeeInterface/Tool/ToolCircle.Designer.cs` + `.resx` sang `Tool.Circle/CircleForm.Designer.cs` + `.resx`.
- Sửa `partial class ToolCircle` → `partial class CircleForm` trong Designer.cs.

**Step 5**: Update factory registration:
- Mở `BeeMain/Program.cs` (hoặc `BeeUi/Form/FormLoad.cs` — wherever app init).
- Thêm `ToolEngineFactory.Register("Circle", () => new CircleEngine());`.

**Step 6**: Update pipeline call site:
- Tìm chỗ `new ToolCircle()` trong codebase: `grep -rn "new ToolCircle" --include="*.cs"`.
- Thay bằng `var engine = ToolEngineFactory.Create("Circle");` hoặc giữ `new ToolCircle()` nhưng `ToolCircle` giờ là alias `[Obsolete] class ToolCircle : CircleForm { }` ở `BeeInterface/Tool/`.

**Step 7**: Build verify:
```
MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal
```
- Pass + warning ≤ baseline.

**Step 8**: Smoke test:
- Mở app → load 1 project mẫu có ToolCircle.
- Mở form Circle → confirm UI hiện đầy đủ 5 vùng.
- Click Test → confirm overlay hiện, score đúng.
- Click Apply → confirm config save xuống `Propety2`.

**Step 9**: Unit test:
- Tạo `tests/Tool.Circle.Tests/Tool.Circle.Tests.csproj` (xUnit).
- Test case:
  ```csharp
  [Fact]
  public void Circle_Detects_Single_Circle()
  {
      var img = new Mat("testdata/single_circle.png");
      var cfg = new CircleConfig { Roi = new Rect(0,0,img.Width,img.Height), CannyLow=50, CannyHigh=150, MinRadius=10, MaxRadius=100, ScoreThreshold=0.5 };
      var engine = new CircleEngine();
      var result = engine.Run(cfg, img);
      Assert.True(result.IsOk);
      Assert.True(result.Score > 0.5);
  }
  ```
- Chạy: `dotnet test tests/Tool.Circle.Tests/`.

**Step 10**: Commit:
```
git add Tools/Tool.Circle/ tests/Tool.Circle.Tests/ EasyVision.sln
git commit -m "[P3.1] Migrate ToolCircle to Tool.Circle module"
```

**Step 11**: Append `CODEX_HISTORY.md` với template 11.2.

**Step 12**: Báo user — pilot done, log effort actual để dự đoán cho 11 tool còn lại Nhóm A.

---

## 11. Task Card Template & History Entry Template

### 11.1 Task Card Template (file `docs/architecture/tasks/YYYY-MM-DD-P<x>.<y>-<slug>.md`)

```markdown
# Task P<x>.<y> — <slug>

## Liên kết
- Phase: <số phase> mục 5.<phase>
- CLAUDE.md mục: <số mục>

## Mục tiêu
<1-2 câu>

## Preconditions
- [ ] Đã đọc CODEX_HISTORY.md mục mới nhất (3 entry cuối)
- [ ] Đã đọc CLAUDE.md mục 0 (Hard Rules)
- [ ] git status sạch hoặc user đã confirm
- [ ] Baseline build pass (P0.2 done)
- [ ] <điều kiện cụ thể khác cho task>

## In-scope (file được phép sửa)
- file1.cs
- file2.cs
- ...

## Out-of-scope (file KHÔNG được đụng)
- folder/ — lý do
- Pattern/Pattern2.* — đang phát triển song song
- ...

## Steps
1. ...
2. ...

## Verify commands
| Command | Expected |
|---|---|
| `MSBuild ...` | warning ≤ <N> |
| `bash tools/check_propety_tools.sh` | exit 0 |
| `dotnet test tests/...` | all pass |
| Smoke: open app → load proj → run inspection | no exception |

## Rollback
- Nếu build fail: `git reset --hard HEAD` (chưa push) hoặc `git revert <sha>`.
- Nếu smoke fail nhưng build pass: <step cụ thể để revert một phần>.

## Exit criteria (DoD)
- [ ] Build pass với warning count ghi trong verify
- [ ] Smoke test pass (mô tả cụ thể)
- [ ] `git diff --name-only HEAD~1` chỉ chứa file In-scope
- [ ] CODEX_HISTORY.md có section mới (template 11.2)
- [ ] Commit message: `[P<x>.<y>] <summary>`

## Notes / Stop-and-ask
- <ghi chú trong quá trình làm>
- <câu hỏi cần user trả lời nếu có>
```

### 11.2 History Entry Template (append vào `docs/architecture/CODEX_HISTORY.md`)

```markdown
## YYYY-MM-DD — P<x>.<y> <summary>

Scope:
- Module / project: ...
- Mục tiêu: ...

Files touched:
- path/file1.cs (move from X to Y)
- path/file2.cs (modify N lines)
- ...

Build verification:
- Command: MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64
- Result: pass, warnings = <N> (baseline = <M>)
- Smoke: <mô tả>

Notes for future agents:
- <pattern phát hiện được>
- <quirk / edge case đã xử lý>

Blockers / left dirty:
- <file dirty không liên quan task>
- <blocker không xử lý nổi>
```

---

## 12. Git & Commit Workflow

### 12.1 Branch naming

- Phase 0-1: trực tiếp trên `main` (low-risk).
- Phase 2 trở đi: branch `refactor/p<x>-<y>-<slug>` (ví dụ `refactor/p2-1-domain`).
- Long-running phase (P3): branch parent `refactor/p3-tools` + sub-branch `refactor/p3-1-circle`, merge sub-branch vào parent.

### 12.2 Commit message format

```
[P<x>.<y>] <summary <= 60 chars>

<body — optional, bullet các thay đổi>

Refs: docs/architecture/tasks/YYYY-MM-DD-P<x>.<y>-<slug>.md
```

Ví dụ:
```
[P1.1] Merge 3 CustomGui.cs into BeeGlobal/Shared

- Combined symbols from BeeCore/, BeeCore/Func/, BeeInterface/
- Updated 47 using statements
- Deleted 2 redundant files

Refs: docs/architecture/tasks/2026-04-23-P1.1-merge-customgui.md
```

### 12.3 Khi nào push, khi nào PR

- Phase 0-1: push main sau mỗi task pass DoD.
- Phase 2-6: làm trên branch, push sub-branch sau mỗi task, mở PR khi hoàn thành nhóm task (ví dụ tất cả P2.1-P2.7 → 1 PR vào main, hoặc 1 PR mỗi sub-task).
- Yêu cầu PR: link Task Card md, link History entry, screenshot smoke test (nếu UI).

### 12.4 Xử lý dirty worktree

- **Trước khi bắt đầu Task Card**: `git status` → nếu có file dirty:
  - File trong scope task: review với user trước khi bắt đầu.
  - File ngoài scope: hỏi user — `stash`, `commit`, hay `để nguyên`.
- **Trong khi làm**: chỉ `git add` file thuộc In-scope. KHÔNG `git add .`.
- **Sau commit**: nếu vẫn còn dirty (ngoài scope), KHÔNG động.

### 12.5 Tag mốc

- Cuối mỗi Phase: `git tag phase-<n>-done -m "Phase <n> complete"`.
- Khi rollback toàn phase: `git reset --hard phase-<n-1>-done`.

---

## 13. Cập Nhật Kế Hoạch

Khi user yêu cầu điều chỉnh, hoặc khi agent phát hiện vấn đề mới: **sửa trực tiếp file này** + ghi 1 dòng changelog ở dưới + ghi lý do vào `CODEX_HISTORY.md`.

### Changelog
- 2026-04-22: Tạo file. Snapshot ban đầu dựa trên `CODEX_HISTORY.md` 2 mục (Pattern2 preprocess/GPU/CPU và PropetyTool cleanup) và quét cấu trúc 14 project.
- 2026-04-22: Thêm mục 9 - Chuẩn hoá GUI tool theo module (khung `ToolFormBase`, library UserControl chung, phân loại 24 tool theo 3 nhóm A/B/C, tool Nhóm C tạm bỏ qua).
- 2026-04-22: **Tái cấu trúc lớn** — thêm mục 0 (Hard Rules với DO/DON'T/Stop-and-ask/Flowchart), mở rộng Roadmap (mục 4 → mục 5 Task Cards chi tiết với Preconditions/In-scope/Out-of-scope/Steps/Verify/DoD/Rollback cho từng task), thêm code template đầy đủ cho `Tool.Common` + `Tool.Circle` (mục 10.10-10.12), thêm Task Card Template + History Entry Template (mục 11), thêm Git Workflow (mục 12). Mục đích: ngăn Codex/Claude đi lệch khi nhận task lẻ.
