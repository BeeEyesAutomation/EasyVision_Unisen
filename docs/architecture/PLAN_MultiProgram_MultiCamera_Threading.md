# Kế Hoạch — Multi-Program / Multi-Camera / Multi-Thread Processing

> Plan riêng cho tính năng mở rộng chế độ chạy. Tuân theo Hard Rules trong `CLAUDE.md` mục 0. Mỗi task = 1 commit, build verify + smoke trước khi qua bước sau, append `CODEX_HISTORY.md` sau mỗi bước. **Lưu ý**: đây là feature work, không phải structural refactor; vẫn cấm tạo call site `Common.PropetyTools[ip][it]` mới, vẫn `-=` trước `+=`.

Ngày lập: 2026-06-04. Files đã khảo sát: `BeeGlobal/Config.cs`, `BeeGlobal/Global.cs`, `BeeCore/Checking.cs`, `BeeCore/Common.cs`, `BeeInterface/Group/View.cs`, `BeeInterface/GeneralSetting.cs`.

---

## 1. Phân Tích Hành Vi Hiện Tại

### 1.1 Mô hình dữ liệu (tất cả đều fixed-size = 4)

| Cấu trúc | Định nghĩa | Ý nghĩa |
|---|---|---|
| `Common.PropetyTools` | `List<List<PropetyTool>>` | List chương trình; mỗi phần tử = 1 program = danh sách tool. Index = "indexProg". |
| `Common.listCamera` | `List<Camera> { null, null, null, null }` | 4 slot camera cố định. |
| `Global.listParaCamera` | `List<ParaCamera> { null, null, null, null }` | Param camera tương ứng từng slot. |
| `Global.ListResult` | `List<Results> {None×4}` | Kết quả tổng mỗi program. |
| `View.Checking1..4` | 4 instance `Checking`, **chỉ `Checking1` được dùng** | `Checking2/3/4` đã khởi tạo nhưng bị comment out khắp nơi. |

### 1.2 Hai chế độ hiện có

`Config.IsMultiProg` (bool) phân biệt:

- **Single Program** (`IsMultiProg=false`): luôn `NumProgFromPLC=1`, `SelectProgram(0)`, chỉ chạy program index 0 với camera slot 0.
- **Multi Program** (`IsMultiProg=true`): PLC gửi `TriggerNum` (Trigger1..4); `Global.SelectProgramForTrigger(triggerNum)` map trigger → 1 program, và `SelectProgram(indexProg)` đặt `IndexProgChoose = indexProg` đồng thời `IndexCCCD = indexProg`.

### 1.3 Ràng buộc camera ↔ program hiện tại (1:1 cứng)

`Global.SelectProgram(int indexProg)`:
```csharp
IndexProgChoose = indexProg;
if (Config != null && Config.IsMultiProg)
    IndexCCCD = indexProg;          // camera slot = program index, hardwired
```
⇒ Multi-Program hiện tại = "**1 camera ↔ 1 program**" theo đúng index. Không có cách map nhiều program vào 1 camera, hay 1 program đọc nhiều camera. `Config.IsMultiCamera` đã tồn tại trong `Config.cs` nhưng **chưa được sử dụng** ở đâu (dead flag).

### 1.4 Vòng xử lý (state machine trong `View.cs`)

State machine toàn cục `Global.StatusProcessing`:
`None → Trigger → Read → Checking → (RunProcessing) → WaitingDone → SendResult → Drawing → Done → None`.

- `StatusProcessing.Read` (dòng ~2645): lặp `foreach (Camera camera in listCamera) camera.Read();` — đọc tất cả camera đã khởi tạo, nhưng chỉ camera tại `IndexCCCD` được dùng cho checking.
- `StatusProcessing.Checking` → `RunProcessing()` (dòng 5374): nếu PLC là `PCI` thì chạy inline; ngược lại:
  ```csharp
  Checking1.StatusProcessing = StatusProcessing.None;
  Checking1.indexThread = Global.IndexProgChoose;   // ÉP 1 checking chạy đúng 1 program
  Checking1.Start();
  ```
  Tức **mỗi trigger chỉ chạy 1 program qua đúng 1 instance `Checking1`**, tuần tự.
- `Checking.Start()` / `ProcessingAll()` (`Checking.cs`): trong 1 program, các tool được `RunToolAsync()` **song song** (mỗi tool async, đếm `doneCount`/`totalTools` bằng `Interlocked`). Vậy "single-thread" hiện tại là ở **mức program** (1 program/lần), còn **tool trong program đã đa luồng**.
- `Done` (dòng 2902): với Multi-Program, sau khi program cuối (`TriggerNum == TriggerNum.NumProgFromPLC`) xong mới gom `ListResult` thành `TotalOK`, rồi reset về `SelectProgram(0)`.

### 1.5 Kết luận phân tích

1. "Multi Program" hiện tại thực chất là **multi-program tuần tự, 1 camera–1 program theo index**, không phải song song.
2. `Checking2/3/4` và `IsMultiCamera` là hạ tầng bỏ dở — có sẵn để tận dụng.
3. Đa luồng đã tồn tại ở mức tool; cái thiếu là **đa luồng ở mức program/camera**.
4. Mapping camera↔program bị cứng hoá trong `SelectProgram`; cần một bảng map cấu hình được.
5. Tất cả list fixed size 4 ⇒ giới hạn cứng 4 program / 4 camera. Cần giữ giới hạn này (hoặc tăng có chủ đích) để không vỡ code đang index trực tiếp `[0..3]`.

---

## 2. Thay Đổi Cấu Hình (`Config.cs`)

Thêm enum + field mới, **giữ nguyên** `IsMultiProg` / `IsMultiCamera` để backward-compat (suy ra từ enum khi load file cũ).

```csharp
// BeeGlobal — enum mới (đặt ở Enums.cs)
public enum ProcessingMode
{
    SingleProgramSingleCamera = 0,   // = chế độ Single hiện tại
    MultiProgramSingleCamera  = 1,   // nhiều program, dùng chung 1 camera
    MultiProgramMultiCamera   = 2,   // mỗi program 1 camera (chế độ Multi hiện tại)
    // SingleProgramMultiCamera dành chỗ tương lai (1 program ghép nhiều ảnh)
}

public enum ThreadingMode
{
    Sequential = 0,   // chạy từng program lần lượt (hành vi hiện tại)
    Parallel   = 1,   // chạy các program đồng thời, mỗi program 1 Checking
}
```

Bổ sung vào `class Config`:
```csharp
public ProcessingMode ProcessingMode = ProcessingMode.SingleProgramSingleCamera;
public ThreadingMode  ThreadingMode  = ThreadingMode.Sequential;
public int NumProgram = 1;            // số program active (1..4)
public int NumCamera  = 1;            // số camera active (1..4)
public int MaxParallelPrograms = 2;   // trần luồng program chạy song song

// Bảng map: index = program, value = camera slot dùng cho program đó.
// Mặc định identity {0,1,2,3}. Cho phép nhiều program trỏ cùng 1 camera.
public int[] ProgramCameraMap = new int[] { 0, 1, 2, 3 };
```

**Suy diễn backward-compat khi load Config cũ** (file `.cfg` không có field mới → nhận default; cần map từ `IsMultiProg`):
```csharp
// Sau khi deserialize Config cũ:
if (loaded.ProcessingMode == default && loaded.IsMultiProg)
    loaded.ProcessingMode = ProcessingMode.MultiProgramMultiCamera; // giữ đúng hành vi cũ
// IsMultiProg / IsMultiCamera trở thành "computed" — đồng bộ 2 chiều (mục 7).
```

---

## 3. Thay Đổi UI — Form `General Setting`

Hiện form chỉ có 2 nút `btnSingle` / `btnMulti` (radio thủ công) + `numFlowChart` (= `NumTrig`). Thiết kế mới:

### 3.1 Khu "Processing Mode" (thay 2 nút Single/Multi)

| Control | Loại | Binding |
|---|---|---|
| `cmbProcessingMode` | ComboBox 3 mục (Single / Multi-1Cam / Multi-MultiCam) | `Config.ProcessingMode` |
| `cmbThreading` | ComboBox 2 mục (Sequential / Parallel) | `Config.ThreadingMode` |
| `numNumProgram` | NumericUpDown 1..4 | `Config.NumProgram` |
| `numNumCamera` | NumericUpDown 1..4 | `Config.NumCamera` |
| `numMaxParallel` | NumericUpDown 1..4 (enable khi Parallel) | `Config.MaxParallelPrograms` |

Giữ `btnSingle`/`btnMulti` cũ ở Designer nhưng ẩn (`Visible=false`) hoặc map thành 2 mục đầu của combo để **không phá `.resx`/`.Designer.cs`** ngay (xoá ở bước dọn riêng).

### 3.2 Khu "Camera ↔ Program Mapping" (mới)

Một `DataGridView` (hoặc grid tự vẽ kiểu hiện hành) 2 cột:
- Cột 1: Program (0..NumProgram-1, read-only).
- Cột 2: Camera slot (ComboBox 0..NumCamera-1).

Đọc/ghi `Config.ProgramCameraMap`. Hiển thị/enable theo `ProcessingMode`:
- Single: grid disabled, map cố định `{0}`.
- Multi-1Cam: cột Camera khoá về `0` cho mọi program.
- Multi-MultiCam: cho chỉnh tự do.

### 3.3 Quy tắc bind (tuân Hard Rules: `-=` trước `+=`)

Trong `GeneralSetting` constructor/load:
```csharp
cmbProcessingMode.SelectedIndexChanged -= OnProcessingModeChanged;
cmbProcessingMode.SelectedIndexChanged += OnProcessingModeChanged;
```
Handler `OnProcessingModeChanged` cập nhật enable/visible của grid + threading + đồng bộ `IsMultiProg`/`IsMultiCamera` (mục 7). Trên `btnSave_Click` đã có `SaveData.Config(Global.Config)` — chỉ cần đảm bảo field mới được serialize (cùng cơ chế `[Serializable]` hiện tại, tự động).

---

## 4. Thay Đổi Kiến Trúc Xử Lý

### 4.1 Trừu tượng hoá "đơn vị chạy" — `ProgramRunPlan`

Tạo một lớp kế hoạch để tách quyết định "chạy gì" khỏi state machine:
```csharp
// BeeCore — ProgramRunPlan.cs
public sealed class ProgramRunUnit
{
    public int ProgramIndex;   // index trong PropetyTools
    public int CameraIndex;    // slot trong listCamera (từ ProgramCameraMap)
}

public static class ProgramRunPlanner
{
    // Trả về danh sách unit cần chạy cho 1 chu kỳ trigger.
    public static List<ProgramRunUnit> Build(TriggerNum trig)
    {
        var cfg = Global.Config;
        var list = new List<ProgramRunUnit>();
        switch (cfg.ProcessingMode)
        {
            case ProcessingMode.SingleProgramSingleCamera:
                list.Add(new ProgramRunUnit { ProgramIndex = 0, CameraIndex = 0 });
                break;
            case ProcessingMode.MultiProgramSingleCamera:
                for (int p = 0; p < cfg.NumProgram; p++)
                    list.Add(new ProgramRunUnit { ProgramIndex = p, CameraIndex = 0 });
                break;
            case ProcessingMode.MultiProgramMultiCamera:
                for (int p = 0; p < cfg.NumProgram; p++)
                    list.Add(new ProgramRunUnit { ProgramIndex = p, CameraIndex = cfg.ProgramCameraMap[p] });
                break;
        }
        return list;
    }
}
```

Khi PLC vẫn điều khiển từng trigger riêng (multi-cam hiện tại), `Build` có thể trả về 1 unit theo `TriggerNum` để giữ tương thích; khi `ThreadingMode.Parallel` + "Free run", trả về toàn bộ.

### 4.2 `RunProcessing()` — đa hình theo plan

Refactor block `else` của `RunProcessing()` (dòng 5429) từ "luôn dùng Checking1" thành "chạy theo plan":

- **Sequential**: lặp plan, mỗi unit set `IndexProgChoose/IndexCCCD` rồi chạy 1 `Checking`, chờ `Done` trước khi sang unit kế (giữ đúng hành vi tuần tự hiện tại, nhưng đã tổng quát hoá cho 1-cam-nhiều-program).
- **Parallel**: cấp 1 `Checking` instance cho mỗi unit (dùng `Checking1..4` theo `ProgramIndex`), chạy đồng thời, dùng một bộ đếm `Interlocked` mức-program để biết khi nào tất cả xong → mới gom kết quả.

### 4.3 `Checking.cs` — bỏ phụ thuộc state toàn cục

`Checking` hiện đọc nhiều `Global.*` (`IndexToolAuto`, `IsAutoTemp`, `IsRun`...) và viết `Global.StatusProcessing` gián tiếp. Để chạy song song nhiều `Checking`:

- `indexThread` đã là per-instance — tốt; mọi truy cập tool đã qua `EnsureToolList(indexThread)` / `TryGetTool(indexThread, ...)` — **đã đúng helper**, an toàn cho song song về dữ liệu tool.
- Vấn đề: `Global.IndexToolAuto`, `Global.IsDoneTrig` là **biến toàn cục dùng chung** — nếu 2 Checking chạy song song sẽ đua. Cần chuyển các biến "per-program" này thành field của `Checking` (hoặc một mảng index theo `indexThread`).
- Kết quả mỗi program phải ghi vào `ListResult[programIndex]` (đã đúng index), tránh ghi đè.

---

## 5. Thiết Kế Mô Hình Luồng (Threading Model)

### 5.1 Ba cấp song song

| Cấp | Hiện trạng | Sau refactor |
|---|---|---|
| Tool trong 1 program | Đã song song (`RunToolAsync` + `Interlocked`) | Giữ nguyên |
| Program | Tuần tự (1 `Checking1`) | Chọn được Sequential / Parallel |
| Camera đọc ảnh | `foreach camera.Read()` tuần tự | Đọc song song (Task.WhenAll) khi Multi-Camera |

### 5.2 Parallel program — sơ đồ

```
Trigger → Read (đọc song song các camera trong plan)
        → Build plan = [unit0(prog0,cam0), unit1(prog1,cam1), ...]
        → with SemaphoreSlim(MaxParallelPrograms):
             foreach unit: launch Checking[unit.ProgramIndex].Run(unit)   // async
        → chờ tất cả Checking.Done  (CountdownEvent / Interlocked counter)
        → gom ListResult → TotalOK → SendResult → Done
```

### 5.3 Nguyên tắc an toàn luồng

1. **Tách state per-program**: `IndexToolAuto`, `IsDoneTrig`, `indexToolPosition` → field của `Checking` (đã là field với `indexToolPosition`; còn `IndexToolAuto`/`IsDoneTrig` đang ở `Global`).
2. **Camera đọc-only khi checking**: mỗi `Checking` chỉ đọc `listCamera[cameraIndex].matRaw`; nếu 2 program dùng chung 1 camera (Multi-1Cam) thì **phải snapshot** ảnh (clone `Mat`) trước khi chạy song song để tránh release giữa chừng.
3. **`Global.StatusProcessing` là biến tổng** — chỉ unit điều phối (View) được set, các `Checking` chỉ raise event `StatusProcessingChanged` của riêng nó; View tổng hợp.
4. **Đếm hoàn tất**: dùng `CountdownEvent(plan.Count)` hoặc `Interlocked.Decrement`; tránh dùng lại bộ đếm `doneCount` nội bộ của từng `Checking` cho mức program.
5. **Giới hạn luồng**: `SemaphoreSlim(MaxParallelPrograms)` để không nổ thread khi 4 program × N tool.
6. **UI thread**: mọi cập nhật control vẫn qua `this.Invoke(...)` như hiện tại.

### 5.4 Backward-compat threading

`ThreadingMode.Sequential` phải tái lập **chính xác** vòng hiện tại (1 `Checking1`, `IndexProgChoose` ép tuần tự). Đây là đường default cho mọi project cũ.

---

## 6. Thiết Kế Mapping Camera ↔ Program

### 6.1 Nguồn chân lý: `Config.ProgramCameraMap` (int[4])

Thay thế logic cứng trong `SelectProgram`. Mới:
```csharp
public static void SelectProgram(int indexProg)
{
    IndexProgChoose = indexProg;
    var cfg = Config;
    if (cfg == null) { IndexCCCD = 0; return; }
    switch (cfg.ProcessingMode)
    {
        case ProcessingMode.SingleProgramSingleCamera:
            IndexCCCD = 0; break;
        case ProcessingMode.MultiProgramSingleCamera:
            IndexCCCD = 0; break;                       // mọi program → camera 0
        case ProcessingMode.MultiProgramMultiCamera:
            IndexCCCD = (indexProg >= 0 && indexProg < cfg.ProgramCameraMap.Length)
                        ? cfg.ProgramCameraMap[indexProg] : indexProg;  // fallback = hành vi cũ
            break;
    }
}
```
Khi `ProgramCameraMap = {0,1,2,3}` + `MultiProgramMultiCamera` ⇒ **kết quả y hệt code cũ** (`IndexCCCD = indexProg`). Đây là điểm mấu chốt đảm bảo không hồi quy.

### 6.2 Các kịch bản map

| Kịch bản | NumProgram | NumCamera | ProgramCameraMap |
|---|---|---|---|
| 1 cam – 1 prog (cũ) | 1 | 1 | `{0,...}` |
| 1 cam – nhiều prog | 4 | 1 | `{0,0,0,0}` |
| nhiều cam – nhiều prog (cũ) | 4 | 4 | `{0,1,2,3}` |
| nhiều cam – nhiều prog (custom) | 3 | 2 | ví dụ `{0,1,0,_}` |

### 6.3 Khởi tạo camera theo map

Hiện camera được tạo lazy tại `listCamera[IndexCCCD]`. Khi load project, cần đảm bảo mọi slot camera **xuất hiện trong `ProgramCameraMap[0..NumProgram-1]`** đều được khởi tạo từ `Global.listParaCamera[slot]`. Thêm bước "EnsureCamerasForPlan()" ở chỗ connect camera.

---

## 7. Cân Nhắc Tương Thích Ngược

1. **File Config cũ** (`.cfg`): không có field mới → default `SingleProgramSingleCamera` + `Sequential`. Phải thêm bước migrate: nếu `IsMultiProg==true` và `ProcessingMode==default` ⇒ set `MultiProgramMultiCamera`, `NumProgram = NumTrig`, `ProgramCameraMap={0,1,2,3}`. Thực hiện ngay sau deserialize trong `LoadData.Config` (hoặc nơi load Config).
2. **`IsMultiProg` / `IsMultiCamera`**: giữ field, biến thành **đồng bộ 2 chiều**. Đặt computed-sync trong setter của `ProcessingMode` (hoặc hàm `SyncLegacyFlags()`):
   - `IsMultiProg = ProcessingMode != SingleProgramSingleCamera`
   - `IsMultiCamera = ProcessingMode == MultiProgramMultiCamera`
   ⇒ mọi call site cũ đọc `Config.IsMultiProg` (Header.cs, DataTool.cs, View.cs) vẫn đúng.
3. **`NumTrig` vs `NumProgram`**: hiện `NumTrig` lái vòng PLC. Giữ `NumTrig`; đặt `NumProgram` đồng bộ với `NumTrig` cho chế độ PLC-driven; chỉ tách khi free-run/parallel.
4. **Fixed size 4**: tiếp tục giữ `listCamera`/`PropetyTools`/`ListResult` size 4. `ProgramCameraMap` length 4. **Không** đổi sang size động trong phase này (sẽ vỡ nhiều index trực tiếp).
5. **`SelectProgram` fallback**: nhánh default trả về hành vi cũ (`IndexCCCD = indexProg`) để bất kỳ code path chưa migrate vẫn chạy.
6. **PLC protocol** (`ParaProtocol.cs`) đang map `Trigger→SelectProgram`: giữ nguyên cho Sequential/PLC-driven; Parallel free-run là đường mới, không đụng path PLC cũ.
7. **`Checking2/3/4`**: đã tồn tại nhưng comment — kích hoạt có kiểm soát, không xoá.

---

## 8. Các Bước Triển Khai (Task Cards)

Mỗi task = 1 commit, build + smoke + append `CODEX_HISTORY.md`.

| Task | Mục tiêu | In-scope (file) | Verify |
|---|---|---|---|
| **T1** | Thêm enum `ProcessingMode`, `ThreadingMode` | `BeeGlobal/Enums.cs` | Build pass |
| **T2** | Thêm field mới vào `Config` + giữ legacy flags | `BeeGlobal/Config.cs` | Build pass; load Config cũ không lỗi |
| **T3** | Migrate khi load Config (`IsMultiProg`→`ProcessingMode`) + `SyncLegacyFlags()` | nơi load Config (`LoadData`/`Access`) | Smoke: load project Multi cũ → vẫn Multi |
| **T4** | Refactor `Global.SelectProgram` dùng `ProgramCameraMap`, giữ fallback | `BeeGlobal/Global.cs` | Smoke: Multi-cam cũ chạy y hệt |
| **T5** | `ProgramRunPlanner` + `ProgramRunUnit` | `BeeCore/ProgramRunPlan.cs` (file mới, đúng sub-folder) | Unit test planner cho 3 mode |
| **T6** | Tách state per-program ra khỏi `Global` (`IndexToolAuto`, `IsDoneTrig`) vào `Checking` | `BeeCore/Checking.cs` | Build; smoke single-program |
| **T7** | `RunProcessing()` Sequential theo plan (1-cam-nhiều-prog) | `BeeInterface/Group/View.cs` | Smoke: 1 cam, 2 program tuần tự |
| **T8** | Đọc camera song song khi Multi-Camera + `EnsureCamerasForPlan()` | `View.cs` (state Read) | Smoke: 2 camera đọc song song |
| **T9** | `RunProcessing()` Parallel + `SemaphoreSlim` + Countdown gom kết quả | `View.cs`, `Checking.cs` | Smoke: 2 program song song, kết quả đúng |
| **T10** | UI General Setting: combo mode/threading + numeric | `GeneralSetting.cs/.Designer.cs/.resx` | Form mở trong Designer, bind đúng |
| **T11** | UI mapping grid camera↔program | `GeneralSetting.*` | Chỉnh map → lưu → reload đúng |
| **T12** | End-to-end + tài liệu + append history | `docs/architecture/CODEX_HISTORY.md` | Cả 6 tổ hợp chạy |

Thứ tự bắt buộc: T1→T2→T3→T4 (nền tảng config + compat) trước; T5→T6 (kế hoạch + tách state); T7→T8→T9 (xử lý); T10→T11 (UI) cuối; T12 đóng.

---

## 9. Rủi Ro & Edge Case

| Rủi ro / Edge case | Tác động | Giảm thiểu |
|---|---|---|
| Race trên `Global.IndexToolAuto`/`IsDoneTrig` khi parallel | Kết quả sai/đè | T6 tách thành per-Checking trước khi bật parallel (T9) |
| 2 program dùng chung 1 camera (Multi-1Cam) release `Mat` giữa chừng | Crash/ảnh hỏng | Snapshot (clone) `matRaw` cho mỗi program trước khi chạy |
| Config cũ load ra Single thay vì Multi | Hồi quy nghiêm trọng | T3 migrate + smoke bắt buộc trên project Multi thật |
| `ProgramCameraMap` trỏ slot camera chưa khởi tạo/null | NullRef tại `listCamera[idx]` | `EnsureCamerasForPlan()` + guard null như code hiện tại |
| `NumProgram > PropetyTools.Count` | Index out of range | Clamp `NumProgram` theo `PropetyTools.Count`; `EnsureToolList` đã tự pad |
| PLC vẫn driver tuần tự nhưng user chọn Parallel | Lệch protocol | Parallel chỉ cho free-run/SimCam giai đoạn đầu; PLC-driven giữ Sequential |
| Vượt 4 program/camera | Vỡ list fixed-4 | Validate 1..4 ở UI; chặn ngoài range |
| Đếm hoàn tất sai khi 1 program rỗng (0 tool) | Deadlock chờ Done | `Checking` đã xử lý `Count==0 → Done`; planner bỏ qua program rỗng |
| `SemaphoreSlim`/CountdownEvent leak khi exception | Treo vòng | `try/finally` release; timeout `TimerOutChecking` |
| Thread pool đói khi 4×N tool async | Trễ/giật | `MaxParallelPrograms` + giữ `MaxConcurrentTools` hiện có |
| UI Invoke từ nhiều Checking song song | Cross-thread/giật UI | Gom cập nhật UI về 1 điểm trong View, không Invoke từ Checking |

---

## 10. Kế Hoạch Kiểm Thử

### 10.1 Ma trận tổ hợp (6 cấu hình bắt buộc)

| # | ProcessingMode | Threading | NumProg | NumCam | Kỳ vọng |
|---|---|---|---|---|---|
| 1 | Single | Sequential | 1 | 1 | = hành vi cũ Single |
| 2 | Multi-MultiCam | Sequential | 4 | 4 | = hành vi cũ Multi (regression) |
| 3 | Multi-1Cam | Sequential | 2 | 1 | 2 program lần lượt, cùng 1 ảnh |
| 4 | Multi-1Cam | Parallel | 2 | 1 | 2 program song song, cùng ảnh (snapshot) |
| 5 | Multi-MultiCam | Sequential | 2 | 2 | mỗi prog 1 cam, tuần tự |
| 6 | Multi-MultiCam | Parallel | 4 | 4 | 4 prog song song, kết quả gom đúng |

### 10.2 Unit test
- `ProgramRunPlanner.Build` cho cả 3 `ProcessingMode` × map mặc định/custom.
- `SelectProgram` mapping: assert `IndexCCCD` đúng theo `ProgramCameraMap`.
- Config migration: deserialize Config cũ (`IsMultiProg=true`) ⇒ `ProcessingMode==MultiProgramMultiCamera`.

### 10.3 Smoke / integration (SimCam, không cần PLC)
- Mỗi cấu hình ở 10.1: load project, chạy ≥ 20 chu kỳ liên tục, kiểm tra: không exception, `ListResult`/`TotalOK` đúng, không leak handler (mở/đóng 5 lần), CycleTime hợp lý.
- So sánh #1 và #2 với baseline (ghi lại OK/NG/CycleTime trước refactor) để xác nhận no-regression.

### 10.4 Stress / an toàn luồng
- Cấu hình #6 chạy 1000 chu kỳ; theo dõi memory (Mat leak), thread count, và tính nhất quán kết quả (so với chạy Sequential cùng ảnh — kết quả tool phải trùng).
- Bật `IsAutoTrigger` + `Position_Adjustment` trong ≥1 program để cover nhánh phức tạp của `Checking`.

### 10.5 Verify chung mỗi commit
`MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64`; warning ≤ baseline; `bash tools/check_propety_tools.sh` = 0; `+=`/`-=` cân bằng.

---

## Phụ Lục — Bản Đồ Code Tham Chiếu

| Hạng mục | Vị trí |
|---|---|
| Flags chế độ | `BeeGlobal/Config.cs:27-30` (`IsMultiProg`, `IsMultiCamera`, `NumTrig`) |
| Map prog→cam | `BeeGlobal/Global.cs:441-472` (`SelectProgram`, `SelectProgramForTrigger`) |
| State chung | `BeeGlobal/Global.cs:432,438,440` (`NumProgFromPLC`, `IndexCCCD`, `IndexProgChoose`) |
| Lists fixed-4 | `BeeCore/Common.cs:145,151`; `BeeGlobal/Global.cs:162,513` |
| Vòng xử lý | `BeeInterface/Group/View.cs:2517-3053` (state machine), `5374-5497` (`RunProcessing`/callback) |
| Checking instances | `BeeInterface/Group/View.cs:5244-5247` |
| Engine 1 program | `BeeCore/Checking.cs` (toàn bộ) |
| UI mode | `BeeInterface/GeneralSetting.cs:78-80,206-223` |
