# PlanCodex1.md — Kế Hoạch Gom & Dọn Helper C#

> **Prompt plan cho Codex/Claude agent.** Mục tiêu: gom toàn bộ class helper/util/extension nằm rải rác trong các project C# (BeeMain, BeeCore, BeeGlobal, BeeInterface, BeeUi, BeeUpdate, PLC_Communication, BeeTest) về một tập class helper **gọn, sạch, không trùng lặp**, đồng thời kéo các local/private helper method đang bị chôn trong tool-files ra đúng class. **Không động đến C++ project** (`Pattern`, `BeeCV`, `BeeNativeOnnx`, `BeeNativeRCNN`, `OKNG`, `PylonCli`, `ColorPixelsCPP`, `BeeOnnxCLi`).
>
> File này **bổ sung** cho `CLAUDE.md` (Phase 1 — Dọn duplicate). Mọi Task Card dưới đây kế thừa Hard Rules ở `CLAUDE.md` mục 0. Trước khi bắt đầu, Codex **BẮT BUỘC** đọc: (1) `CLAUDE.md` mục 0 (Hard Rules), (2) `docs/architecture/CODEX_HISTORY.md` 3 entry cuối, (3) toàn bộ file này.

---

## 0. Hiện Trạng Helper — Phát Hiện Cụ Thể

Snapshot ngày 2026-04-23, đã kiểm tra thực tế từng file:

### 0.1 Duplicate thật (phải dedup)

| # | Nhóm duplicate | File | Symbol | LOC | Ghi chú |
|---|---|---|---|---|---|
| D1 | **Converts** | `BeeCore/Converts.cs` | `class Convert2 { NumberFromString }` | 24 | Trivial. Functionality đã có trong Func/Converts.cs phiên bản đầy đủ hơn. |
| D1 | | `BeeCore/Func/Converts.cs` | `class Converts { BeforeFirstDigit, StringtoDouble, PyToRectRotates, ToCli, DrawRectRotate }` | 157 | Giữ; bổ sung `NumberFromString`. |
| D2 | **CustomGui** | `BeeCore/Func/CustomGui.cs` | `class CustomGui { BackColor, RoundRg }` | 110 | BackColor + RoundRg giống hệt ở cả 2 file. |
| D2 | | `BeeInterface/CustomGui.cs` | `class Gui { BackColor, RoundRg }` | 108 | Chỉ khác tên class (`Gui` vs `CustomGui`). |
| D3 | **GeometryHelper** | `BeeCore/ShapeEditing/GeometryHelper.cs` | 7 method (BuildLocalInverseMatrixFor, TransformPoint, RotateAround, RotateVector, BboxOf, HitTestPolygonVertex, HitTestCornerHandle) | 102 | Core-layer. |
| D3 | | `BeeInterface/ShapeEditing/GeometryHelper.cs` | 9 method (RotateAround, RotateVector, RotatePoint, TransformPoint, BuildLocalInverseMatrixFor, BboxOf, GetPolygonBoundsLocal, BoundsContainAll, HexBoundsContainAll) | 153 | UI-layer. **5/9 method trùng hệt với BeeCore.** |
| D3 | | `BeeCore/Common.cs` | `RotateAround` (line 108), `GetBoundingBox` (line 77) | (trong 573) | **Lại trùng** với 2 file trên. |

### 0.2 Naming lộn xộn / không duplicate nhưng cần chuẩn hoá

| # | Vấn đề | File | Ghi chú |
|---|---|---|---|
| N1 | `ImageUtils73` tên class có đuôi số | `BeeCore/Func/ImageUtils.cs` | Rename `ImageUtils73` → `ImageUtils`. |
| N2 | File "rơi" ở root `BeeCore/` | `BeeCore/BitmapExtensions.cs`, `BeeCore/MatrixExtension.cs`, `BeeCore/KeepLargestAuto.cs` | Move vào sub-folder (phù hợp CLAUDE.md P1.3). |
| N3 | `Global.cs` xuất hiện 3 nơi nhưng **nội dung khác nhau** | `BeeGlobal/Global.cs` (432L, state app), `BeeInterface/Global.cs` (32L, `struct G`), `BeeUi/Global.cs` (89L, `struct G`) | **KHÔNG phải duplicate** — state riêng từng tầng. Không merge ở phase này (CLAUDE.md P4.3 xử lý). |
| N4 | `Crop.cs` xuất hiện 2 nơi | `BeeCore/Func/Crop.cs` (`static class Cropper`, 1209L), `BeeCore/Unit/Crop.cs` (`class Crop` instance, 163L với `DrawResult`) | **KHÔNG duplicate** — 1 là static helper, 1 là instance unit có state. Không merge; có thể rename `Unit/Crop.cs` → `Unit/CropUnit.cs` nhưng để Phase 6 rename. |
| N5 | `ConvertImg.cs` | `BeeInterface/ConvertImg.cs` (40L, 1 method `ChangeToColor`) | Nhỏ, UI-only; đưa vào bucket UI shared. |

### 0.3 Local/private helper method lộ ra — cần kéo về helper class

Các method `private static` / `internal static` hiện đang chôn trong tool file, nhưng bản chất là utility dùng chung. Danh sách top ưu tiên (đã verify tồn tại):

**Polygon / Point-in-polygon (trong `BeeCore/Func/ResultFilter.cs` — 14 method):**
`Rotate(PointF,float)`, `PolygonArea`, `Cross`, `Inside`, `Intersect`, `PointInPolygonOrOnEdge`, `PointInPolygon`, `PointOnPolygonEdge`, `DistancePointToSegment`, `ClipEdge`, `PolygonIntersection`, `IsPolygonInside`, `GetArea(RectRotate)`, `GetWorldPoly(RectRotate)`, `IsFinite`, `MergeRectRotate`, `MergeMat`, `IsOverlapEnough`.

**Geometry point/rotation (trong `BeeCore/Func/Crop.cs` — 2 method):**
`RotatePoint(Point2f, float)`, `GetPolygonBounds(IList<PointF>, out...)`.

**Line construction (trong `BeeCore/Func/Line2DTransform.cs`):**
`LineFromTwoPoints(PointF, PointF)`.

**String parse (trong `BeeCore/Converts.cs` + `BeeCore/Func/Converts.cs`):**
`NumberFromString`, `BeforeFirstDigit`, `StringtoDouble`.

**TAR / octal parse (trong `BeeCore/Func/TarProgramHelper.cs` — 2 method):**
`ReadString(byte[], int, int)`, `ReadOctal(byte[], int, int)`.

**Image/Mat (rải rác):**
`BitmapExtensions.IsDisposed` (ext), `ItemTool.SafeCloneImage`, `MatHelper.*`, `Camera.EnsureMat` (cần xác nhận file), `Camera.CopyRows`.

---

## 1. Target Buckets — Danh Mục Helper Class Sau Consolidation

Mục tiêu cuối sau khi chạy hết PlanCodex1.md. Vị trí **TẠM** đặt ở sub-folder của `BeeCore/Func/` hoặc `BeeGlobal/Shared/` để **không tạo project mới** (CLAUDE.md Phase 2 sẽ lo tách project). Nguyên tắc: 1 concern = 1 file = 1 class.

| Target | Path | Nội dung (sau merge) | Nguồn |
|---|---|---|---|
| `StringHelper` | `BeeCore/Func/StringHelper.cs` | `BeforeFirstDigit`, `StringtoDouble`, `NumberFromString` | `BeeCore/Converts.cs` (Convert2), `BeeCore/Func/Converts.cs` (Converts — 2 method string) |
| `Converts` | `BeeCore/Func/Converts.cs` | Giữ lại **chỉ** convert domain: `PyToRectRotates`, `ToCli`, `DrawRectRotate` (BeeCpp↔managed). String ops MOVE đi StringHelper. | Shrink lại file cũ |
| `PolygonHelper` | `BeeCore/Func/PolygonHelper.cs` | Toàn bộ private polygon/point-in-polygon math từ `ResultFilter.cs`, `Crop.RotatePoint`, `Crop.GetPolygonBounds`, `Line2DTransform.LineFromTwoPoints` | 14 method extract + 2 + 1 |
| `Geometry2D` | `BeeCore/Algorithm/Geometry2D.cs` | **Không động** — đã là helper tốt. Bổ sung (nếu cần) cross-ref từ PolygonHelper. | Giữ |
| `GeometryHelper` | `BeeCore/ShapeEditing/GeometryHelper.cs` | **Nguồn duy nhất** cho: `RotateAround`, `RotateVector`, `RotatePoint`, `TransformPoint`, `BuildLocalInverseMatrixFor`, `BboxOf`, `GetPolygonBoundsLocal`, `HitTestPolygonVertex`, `HitTestCornerHandle`, `BoundsContainAll`, `HexBoundsContainAll` | Merge 2 file GeometryHelper + 2 method từ Common.cs |
| `Shared.CustomGui` | `BeeGlobal/Shared/CustomGui.cs` | 1 bản `CustomGui { BackColor, RoundRg }` (hoặc giữ tên `Gui` — quyết định ở Step, xem Task S2) | Merge từ `BeeCore/Func/CustomGui.cs` + `BeeInterface/CustomGui.cs` |
| `Shared.ConvertImg` | `BeeGlobal/Shared/ConvertImg.cs` | `ChangeToColor` (+ chỗ chứa các color-blit future) | `BeeInterface/ConvertImg.cs` |
| `BitmapHelper` | `BeeCore/Func/BitmapHelper.cs` | `IsDisposed` (ext), `SafeCloneImage`, bitmap-safe utility | `BeeCore/BitmapExtensions.cs`, `ItemTool.SafeCloneImage` (chỉ khi an toàn) |
| `MatrixHelper` | `BeeCore/Func/MatrixHelper.cs` | Transform PointF qua System.Drawing.Matrix | Move `BeeCore/MatrixExtension.cs` |
| `MatHelper` | `BeeCore/Func/MatHelper.cs` | **Không động**. Có thể bổ sung `EnsureMat`, `CopyRows` nếu Camera file đồng ý. | Giữ |
| `TimingUtils` | `BeeGlobal/TimingUtils.cs` | **Không động.** Đã tốt. | Giữ |
| `HsvConvert` | `BeeCore/Core/HsvConvert.cs` | **Không đổi tên trong phase này** (rename là Phase 6 — CLAUDE.md). Có thể move nếu gây lẫn, nhưng khuyến nghị giữ. | Giữ |

**Không vào phase này (defer sang CLAUDE.md Phase 2+):**
- Tách project con `BeeCore.*`.
- Đổi tên symbol (`dataMat`, `Propety`, `Comunication`, `Position_Adjustment`).
- Gộp 3 `Global.cs`.
- Tool migrate sang `Tool.Xxx/`.
- Native helper (C++).
- PLC_Communication helper (vendor-specific, không gom — CLAUDE.md khuyến nghị skip).

---

## 2. Hard Rules Riêng Cho PlanCodex1.md

Kế thừa CLAUDE.md mục 0. Bổ sung:

1. **Không tách project mới**. Chỉ move file trong cùng assembly, hoặc `BeeCore` ↔ `BeeGlobal` (vì `BeeGlobal` đã tồn tại và đã được BeeCore reference). **Không** ref ngược `BeeGlobal` → `BeeCore`.
2. **Không đổi namespace public hiện tại của type còn sống** trong cùng commit move. Dùng `[assembly: TypeForwardedTo]` nếu có risk (CLAUDE.md mục 0.1.9).
3. **Không xoá `Convert2` ngay** — tạo stub `[Obsolete] class Convert2 { public static int NumberFromString(string s) => StringHelper.ParseFirstIntegerSequence(s); }` giữ backward-compat.
4. **Không thay đổi public API của class được giữ** (ví dụ: sau khi extract polygon từ ResultFilter, public method của `ResultFilter` phải không đổi signature).
5. **Không đổi thứ tự serialization**. Nếu class có `[Serializable]` hoặc đang được JSON deserialize qua `ClassProject.json`, test load 1 project mẫu trước khi commit.
6. **Mỗi Task Card = 1 commit**. Commit message: `[PlanCodex1.S<n>] <summary>`.
7. **Đọc CODEX_HISTORY.md trước mọi task**. Append entry sau khi xong.
8. **Verify script** `bash tools/check_propety_tools.sh` phải pass trước mỗi commit.
9. **Event subscribe**: phát hiện bất kỳ `+=` nào mới tạo ra trong quá trình merge → đảm bảo có `-=` trước đó.
10. **Smoke test bắt buộc** sau mỗi task: build + mở app + load 1 project mẫu + chạy 1 inspection.

---

## 3. Lộ Trình (Suggested Order)

Thứ tự đề xuất — risk thấp đến cao, mỗi task độc lập, có thể làm riêng lẻ:

| Thứ tự | Task | Risk | Timebox | Phụ thuộc |
|---|---|---|---|---|
| 1 | **S1** Gộp `Converts` (Convert2 → Func/Converts + tạo StringHelper) | Thấp | 2h | — |
| 2 | **S2** Gộp 2 file `CustomGui` vào `BeeGlobal/Shared/CustomGui.cs` | Thấp | 2h | S1 |
| 3 | **S3** Move `BeeInterface/ConvertImg.cs` → `BeeGlobal/Shared/ConvertImg.cs` | Thấp | 1h | S2 |
| 4 | **S4** Move `BeeCore/BitmapExtensions.cs` → `BeeCore/Func/BitmapHelper.cs` + rename class | Thấp | 1.5h | — |
| 5 | **S5** Move `BeeCore/MatrixExtension.cs` → `BeeCore/Func/MatrixHelper.cs` | Thấp | 1h | — |
| 6 | **S6** Rename `ImageUtils73` → `ImageUtils` (class only, file giữ tên) | Thấp | 0.5h | — |
| 7 | **S7** Move `BeeCore/KeepLargestAuto.cs` → `BeeCore/Algorithm/KeepLargestAuto.cs` | Thấp | 0.5h | — |
| 8 | **S8** Extract private polygon/point math từ `ResultFilter.cs` → `BeeCore/Func/PolygonHelper.cs` | **Trung bình** | 4h | — |
| 9 | **S9** Dedup GeometryHelper: BeeCore giữ canonical + BeeInterface delegate + Common.cs loại 2 method trùng | **Trung bình** | 3h | S8 (nếu dùng chung) |
| 10 | **S10** Kéo các local method còn lại (`Line2DTransform.LineFromTwoPoints`, `Crop.RotatePoint`, `Crop.GetPolygonBounds`) vào PolygonHelper/Geometry2D | Thấp | 2h | S8 |
| 11 | **S11** Regex guard + smoke tổng + append CODEX_HISTORY | Thấp | 1h | Tất cả trên |

**Tổng**: ~18-22 giờ làm việc nếu làm tuần tự. Có thể song song S4/S5/S6/S7 (độc lập).

---

## 4. Task Cards Chi Tiết

### 🔹 Task S1 — Gộp Converts (Convert2 → Func/Converts + tạo StringHelper)

**Mục tiêu**: loại duplicate `BeeCore/Converts.cs` ↔ `BeeCore/Func/Converts.cs`; tách string parsing ra class riêng.

**Preconditions**:
- [ ] Đọc `docs/architecture/CODEX_HISTORY.md` 3 entry cuối.
- [ ] `git status` sạch hoặc user confirm.
- [ ] Build baseline (`docs/architecture/baseline_build.md`) đã có.
- [ ] Không có file nào trong "In-scope" đang bị user sửa dở.

**In-scope**:
- `BeeCore/Converts.cs` (xoá sau khi forward)
- `BeeCore/Func/Converts.cs` (chỉnh — chỉ giữ method domain-specific)
- `BeeCore/Func/StringHelper.cs` (tạo mới)
- `BeeCore.csproj` (update `<Compile Include>`)
- Mọi call site gọi `Convert2.NumberFromString`, `Converts.BeforeFirstDigit`, `Converts.StringtoDouble` trong code `.cs`.

**Out-of-scope**:
- `Pattern/*`, `BeeCV/*`, bất kỳ C++ file nào.
- `Pattern2.*`, `Pitch*`.
- `ClassProject.cs` (serialization).
- Rename class cũ (Convert2 giữ nguyên dưới dạng `[Obsolete]`).

**Steps**:
1. Đọc `BeeCore/Converts.cs` (24 dòng) + `BeeCore/Func/Converts.cs` (157 dòng). Ghi nhận 3 method target:
   - `Convert2.NumberFromString(string)` → sẽ thành `StringHelper.ParseFirstIntegerSequence(string)`.
   - `Converts.BeforeFirstDigit(string)` → `StringHelper.BeforeFirstDigit(string)`.
   - `Converts.StringtoDouble(string)` → `StringHelper.ParseFlexibleDouble(string)`.
2. Tạo file `BeeCore/Func/StringHelper.cs`:
   ```csharp
   using System.Globalization;
   using System.Text;
   using System.Text.RegularExpressions;
   namespace BeeCore.Func
   {
       public static class StringHelper
       {
           public static int ParseFirstIntegerSequence(string input) { /* từ Convert2.NumberFromString */ }
           public static string BeforeFirstDigit(string s) { /* từ Converts.BeforeFirstDigit */ }
           public static double? ParseFlexibleDouble(string s) { /* từ Converts.StringtoDouble */ }
       }
   }
   ```
3. Thêm vào `BeeCore.csproj`:
   ```xml
   <Compile Include="Func\StringHelper.cs" />
   ```
4. Trong `BeeCore/Func/Converts.cs`: **xoá** `BeforeFirstDigit`, `StringtoDouble`. **Giữ lại** `PyToRectRotates`, `ToCli`, `DrawRectRotate` (domain).
5. Trong `BeeCore/Converts.cs`: thay bằng shim:
   ```csharp
   using System;
   namespace BeeCore
   {
       [Obsolete("Use BeeCore.Func.StringHelper.ParseFirstIntegerSequence")]
       public class Convert2
       {
           public static int NumberFromString(string input)
               => BeeCore.Func.StringHelper.ParseFirstIntegerSequence(input);
       }
   }
   ```
   (Giữ file — không xoá — để backward-compat. Nếu quyết định xoá hẳn: xem rollback.)
6. Tìm call site và thay thế:
   ```
   grep -rn "Convert2\.NumberFromString\|Converts\.BeforeFirstDigit\|Converts\.StringtoDouble" \
        --include="*.cs" --exclude-dir=bin --exclude-dir=obj \
        BeeCore BeeInterface BeeUi BeeMain.csproj BeeGlobal BeeUpdate
   ```
   - Với mỗi hit: thay bằng `StringHelper.<newName>` + thêm `using BeeCore.Func;` nếu cần.
7. Build verify.

**Verify**:
| Command | Expected |
|---|---|
| `MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal` | pass, warning ≤ baseline |
| `bash tools/check_propety_tools.sh` | exit 0 |
| `grep -rn "Convert2\.NumberFromString" --include="*.cs" --exclude-dir=bin --exclude-dir=obj .` | 0 hit (trừ trong file `Convert2` shim) |
| `grep -rn "Converts\.BeforeFirstDigit\|Converts\.StringtoDouble" --include="*.cs" . ` | 0 hit |
| Smoke | Mở app → load project mẫu → chạy 1 vòng inspection → không exception |

**Rollback**: `git reset --hard HEAD~1`.

**DoD**:
- [ ] File `BeeCore/Func/StringHelper.cs` tồn tại với 3 method.
- [ ] `BeeCore/Func/Converts.cs` không còn 2 method string.
- [ ] `BeeCore/Converts.cs` chỉ còn shim `Convert2` delegate.
- [ ] 0 call site trực tiếp method cũ (ngoài shim).
- [ ] Build pass, smoke pass.
- [ ] History entry `[PlanCodex1.S1]` trong `docs/architecture/CODEX_HISTORY.md`.
- [ ] Commit message: `[PlanCodex1.S1] Extract StringHelper; shim Convert2/Converts`.

---

### 🔹 Task S2 — Gộp 2 bản CustomGui vào BeeGlobal/Shared

**Mục tiêu**: 1 file duy nhất `BeeGlobal/Shared/CustomGui.cs`, xoá 2 bản trùng.

**Preconditions**:
- [ ] S1 done (không bắt buộc nhưng tránh conflict merge).
- [ ] `git status` sạch.
- [ ] Verify 2 file hiện tại có `BackColor` và `RoundRg` identical (Codex **bắt buộc** diff 2 file trước khi merge).

**In-scope**:
- `BeeCore/Func/CustomGui.cs` (xoá sau khi forward)
- `BeeInterface/CustomGui.cs` (xoá sau khi forward)
- `BeeGlobal/Shared/CustomGui.cs` (tạo mới)
- `BeeGlobal.csproj`, `BeeCore.csproj`, `BeeInterface.csproj` (update `<Compile Include>`)
- Call site `CustomGui.BackColor`, `CustomGui.RoundRg`, `Gui.BackColor`, `Gui.RoundRg`.

**Out-of-scope**:
- Thêm/sửa method mới trong CustomGui.
- Động `BeeShared.UI/` (chưa tồn tại — đó là Phase 4 CLAUDE.md).
- Đổi namespace của BeeCore types không liên quan.

**Steps**:
1. Diff 2 file bằng:
   ```
   diff <(grep -v '^using\|^namespace\|^\s*$' BeeCore/Func/CustomGui.cs) \
        <(grep -v '^using\|^namespace\|^\s*$' BeeInterface/CustomGui.cs)
   ```
   Nếu body method giống hệt → merge trực tiếp. Nếu khác → **dừng, hỏi user** chọn bản nào làm chuẩn.
2. Tạo folder `BeeGlobal/Shared/` nếu chưa có: `mkdir BeeGlobal/Shared`.
3. Tạo `BeeGlobal/Shared/CustomGui.cs`:
   ```csharp
   using System.Drawing;
   // không using BeeCore để tránh circular ref
   namespace BeeGlobal.Shared
   {
       public static class CustomGui
       {
           public static Color BackColor(TypeCtr typeCtr, Color color) { /* body */ }
           public static void RoundRg(dynamic contr, int roundRad, Corner _Corner = Corner.Both) { /* body */ }
       }
   }
   ```
   - Đưa `TypeCtr`, `Corner` enum (nếu chưa ở BeeGlobal) vào `BeeGlobal/Shared/` cùng file hoặc file riêng. Kiểm tra namespace hiện tại của 2 enum này trước.
4. Thêm `<Compile Include="Shared\CustomGui.cs" />` vào `BeeGlobal.csproj`.
5. Trong `BeeCore/Func/CustomGui.cs`: thay bằng shim alias (giữ file để backward-compat):
   ```csharp
   using System.Drawing;
   namespace BeeCore.Func
   {
       [System.Obsolete("Use BeeGlobal.Shared.CustomGui")]
       public static class CustomGui
       {
           public static Color BackColor(TypeCtr t, Color c) => BeeGlobal.Shared.CustomGui.BackColor(t, c);
           public static void RoundRg(dynamic contr, int r, Corner c = Corner.Both) => BeeGlobal.Shared.CustomGui.RoundRg(contr, r, c);
       }
   }
   ```
6. Tương tự cho `BeeInterface/CustomGui.cs` với class `Gui`:
   ```csharp
   namespace BeeInterface
   {
       [System.Obsolete("Use BeeGlobal.Shared.CustomGui")]
       public static class Gui
       {
           public static Color BackColor(TypeCtr t, Color c) => BeeGlobal.Shared.CustomGui.BackColor(t, c);
           public static void RoundRg(dynamic contr, int r, Corner c = Corner.Both) => BeeGlobal.Shared.CustomGui.RoundRg(contr, r, c);
       }
   }
   ```
7. Build verify lần 1.
8. Tìm call site:
   ```
   grep -rn "CustomGui\.BackColor\|CustomGui\.RoundRg\|\bGui\.BackColor\|\bGui\.RoundRg" \
        --include="*.cs" --exclude-dir=bin --exclude-dir=obj .
   ```
   - Mỗi hit: thêm `using BeeGlobal.Shared;` + thay thành `CustomGui.Xxx`.
9. Build verify lần 2.

**Verify**:
| Command | Expected |
|---|---|
| `find . -name "CustomGui.cs" -not -path "*/bin/*" -not -path "*/obj/*" -not -path "*/BackupOld/*"` | 3 kết quả (new + 2 shim). Chưa xoá vì backward-compat. |
| MSBuild Release x64 | pass, warning ≤ baseline |
| Smoke: mở Form có RoundRg (ví dụ ToolEdit) | render đúng |
| Smoke: mở Dashboard | không exception |

**Rollback**: `git reset --hard HEAD~1`.

**DoD**: 1 bản canonical + 2 shim với `[Obsolete]`; build pass; smoke pass; history entry `[PlanCodex1.S2]`.

**Stop-and-ask**: nếu body method 2 file khác nhau (logic, không chỉ whitespace) → hỏi user chọn bản nào hoặc có muốn parameterize.

---

### 🔹 Task S3 — Move BeeInterface/ConvertImg.cs vào Shared

**Mục tiêu**: ConvertImg là UI-only utility thuần, không ref BeeCore → chuyển vào `BeeGlobal/Shared/`.

**In-scope**: `BeeInterface/ConvertImg.cs`, `BeeGlobal/Shared/ConvertImg.cs`, 2 csproj, call site.

**Out-of-scope**: tách `ChangeToColor` ra method mới.

**Steps**:
1. `git mv BeeInterface/ConvertImg.cs BeeGlobal/Shared/ConvertImg.cs`.
2. Đổi namespace: `sed -i 's|^namespace BeeInterface$|namespace BeeGlobal.Shared|' BeeGlobal/Shared/ConvertImg.cs`.
3. Update `BeeInterface.csproj`: xoá `<Compile Include="ConvertImg.cs" />`.
4. Update `BeeGlobal.csproj`: thêm `<Compile Include="Shared\ConvertImg.cs" />`.
5. Tạo shim ở `BeeInterface/ConvertImg.shim.cs`:
   ```csharp
   using System.Drawing;
   namespace BeeInterface
   {
       [System.Obsolete("Use BeeGlobal.Shared.ConvertImg")]
       public static class ConvertImg
       {
           public static Bitmap ChangeToColor(Bitmap bmp, Color c, float op)
               => BeeGlobal.Shared.ConvertImg.ChangeToColor(bmp, c, op);
       }
   }
   ```
6. Grep call site `ConvertImg.ChangeToColor` → thêm `using BeeGlobal.Shared;` ở file caller.

**Verify**: build pass; smoke pass; `find . -name "ConvertImg.cs" -not -path "*/bin/*"` có 2 kết quả (moved + shim).

**DoD**: tương tự S2.

---

### 🔹 Task S4 — BitmapExtensions → BitmapHelper (move vào Func)

**Mục tiêu**: file `BeeCore/BitmapExtensions.cs` ở root BeeCore → sub-folder `Func`; class rename `BitmapExtensions` → `BitmapHelper` (nhưng **giữ old alias** để không break call site).

**In-scope**: `BeeCore/BitmapExtensions.cs`, `BeeCore/Func/BitmapHelper.cs`, `BeeCore.csproj`, call site `.IsDisposed()`.

**Out-of-scope**: gộp thêm method mới (SafeCloneImage, v.v.).

**Steps**:
1. `git mv BeeCore/BitmapExtensions.cs BeeCore/Func/BitmapHelper.cs`.
2. Trong `BeeCore/Func/BitmapHelper.cs`:
   - Đổi `namespace BeeCore` → `namespace BeeCore.Func`.
   - Đổi `public static class BitmapExtensions` → `public static class BitmapHelper`.
   - Giữ signature extension method `public static bool IsDisposed(this Image image)` nguyên.
3. Tạo alias **trong sub-folder** (CLAUDE.md mục 0.2 #11: cấm file mới ở root BeeCore) — file `BeeCore/Func/BitmapExtensionsAlias.cs` giữ namespace cũ `BeeCore` để callsite không đổi:
   ```csharp
   using System;
   using System.Drawing;
   namespace BeeCore
   {
       [Obsolete("Use BeeCore.Func.BitmapHelper")]
       public static class BitmapExtensions
       {
           public static bool IsDisposed(this Image image) => BeeCore.Func.BitmapHelper.IsDisposed(image);
       }
   }
   ```
   (Namespace trong file không cần trùng path folder — C# cho phép.)
4. Update `BeeCore.csproj`: `<Compile Include="Func\BitmapHelper.cs" />` + `<Compile Include="Func\BitmapExtensionsAlias.cs" />`; xoá `<Compile Include="BitmapExtensions.cs" />`.
5. Grep call site `IsDisposed(` — vì là extension method, không thay; chỉ cần caller import đúng namespace (`using BeeCore.Func;`). Tìm caller file:
   ```
   grep -rln "\.IsDisposed()" --include="*.cs" --exclude-dir=bin --exclude-dir=obj .
   ```
   - Trong mỗi file: nếu chưa có `using BeeCore;` và chưa có `using BeeCore.Func;` → thêm `using BeeCore.Func;`. Nếu đã `using BeeCore;` → có thể giữ (shim hoạt động).

**Verify**: build pass; `.IsDisposed()` call site hoạt động.

**DoD**: tương tự S2.

---

### 🔹 Task S5 — MatrixExtension → MatrixHelper

**Mục tiêu**: tương tự S4 cho `BeeCore/MatrixExtension.cs`.

**In-scope**: `BeeCore/MatrixExtension.cs` → `BeeCore/Func/MatrixHelper.cs`. Alias giữ ở root.

**Steps**:
1. `git mv BeeCore/MatrixExtension.cs BeeCore/Func/MatrixHelper.cs`.
2. Sửa namespace + class name (`MatrixExtension` → `MatrixHelper`, namespace `BeeCore.Func`).
3. Alias **trong sub-folder** `BeeCore/Func/MatrixExtensionAlias.cs` (giữ namespace `BeeCore` — không để file ở root):
   ```csharp
   using System.Drawing;
   using System.Drawing.Drawing2D;
   namespace BeeCore
   {
       [System.Obsolete("Use BeeCore.Func.MatrixHelper")]
       public static class MatrixExtension
       {
           public static PointF TransformPoint(this Matrix m, PointF p)
               => BeeCore.Func.MatrixHelper.TransformPoint(m, p);
       }
   }
   ```
4. Update csproj: thêm `<Compile Include="Func\MatrixHelper.cs" />` + `<Compile Include="Func\MatrixExtensionAlias.cs" />`; xoá entry root cũ. Grep call site `.TransformPoint(` — caller chỉ cần import namespace.

**Verify**: build pass.

**DoD**: tương tự.

---

### 🔹 Task S6 — Rename ImageUtils73 → ImageUtils

**Mục tiêu**: tên class `ImageUtils73` trong file `BeeCore/Func/ImageUtils.cs` rõ ràng hơn.

**In-scope**: `BeeCore/Func/ImageUtils.cs` (đổi class name), call site.

**Out-of-scope**: rename file (file đã tên đúng); đổi namespace.

**Steps**:
1. Đọc `BeeCore/Func/ImageUtils.cs`. Xác nhận class name là `ImageUtils73`.
2. Trong VS: F2 rename `ImageUtils73` → `ImageUtils` (auto update mọi call site).
   - Hoặc CLI: `grep -rln "ImageUtils73" --include="*.cs" . | xargs sed -i 's/\bImageUtils73\b/ImageUtils/g'`.
3. Thêm alias obsolete (chỉ nếu có call site ngoài BeeCore):
   ```csharp
   namespace BeeCore.Func
   {
       [System.Obsolete("Use ImageUtils")]
       public static class ImageUtils73
       {
           public static Bitmap StitchVerticalWithCaptions(/* … */)
               => ImageUtils.StitchVerticalWithCaptions(/* … */);
       }
   }
   ```
   Chỉ tạo shim nếu grep cho thấy có caller ngoài file hiện tại.

**Verify**: build pass; `grep -rn "ImageUtils73" --include="*.cs" .` = 0 (hoặc chỉ trong shim).

**DoD**: tương tự.

---

### 🔹 Task S7 — Move KeepLargestAuto.cs vào Algorithm/

**Mục tiêu**: `BeeCore/KeepLargestAuto.cs` nằm ở root; đích đúng là `BeeCore/Algorithm/` (đã có trong CLAUDE.md P1.3.j nhưng có thể chưa làm).

**Steps**:
1. Kiểm tra file có ở root chưa: `ls BeeCore/KeepLargestAuto.cs`.
   - Nếu đã ở `BeeCore/Algorithm/` rồi → skip task.
2. `git mv BeeCore/KeepLargestAuto.cs BeeCore/Algorithm/KeepLargestAuto.cs`.
3. **KHÔNG** đổi namespace (giữ `BeeCore` để không break).
4. Update csproj.

**Verify**: build pass. `find BeeCore -maxdepth 1 -name "KeepLargestAuto.cs"` = 0.

**DoD**: tương tự.

---

### 🔹 Task S8 — Extract Polygon/Point math từ ResultFilter → PolygonHelper

**Mục tiêu**: kéo 14 method `private static` polygon math từ `ResultFilter.cs` ra thành `BeeCore/Func/PolygonHelper.cs` **public static**, `ResultFilter` chỉ gọi.

**Preconditions**:
- [ ] S1-S7 done (không bắt buộc, nhưng nên).
- [ ] `git status` sạch.

**In-scope**:
- `BeeCore/Func/ResultFilter.cs` (chỉnh: xoá 14 private, thêm `using BeeCore.Func;` nếu cần, đổi call `Rotate(...)` → `PolygonHelper.Rotate(...)`).
- `BeeCore/Func/PolygonHelper.cs` (tạo mới).
- `BeeCore.csproj`.

**Out-of-scope**:
- **Không đổi** public method signature của `ResultFilter`.
- Không merge thêm polygon method từ `Crop.cs` ở task này (sẽ làm ở S10).
- Không đụng Algorithm/Geometry2D.cs.

**Steps**:
1. Đọc `BeeCore/Func/ResultFilter.cs` (~542 dòng). List 14 method cần extract:
   - `Rotate(PointF, float)`
   - `PolygonArea(IList<PointF>)`
   - `Cross(PointF, PointF, PointF)`
   - `Inside(PointF, PointF, PointF)`
   - `Intersect(PointF, PointF, PointF, PointF)`
   - `PointInPolygonOrOnEdge(PointF, List<PointF>)`
   - `PointInPolygon(PointF, List<PointF>)`
   - `PointOnPolygonEdge(PointF, List<PointF>, float)`
   - `DistancePointToSegment(PointF, PointF, PointF)`
   - `ClipEdge(List<PointF>, PointF, PointF)`
   - `PolygonIntersection(IList<PointF>, IList<PointF>)`
   - `IsPolygonInside(List<PointF>, List<PointF>)`
   - `IsFinite(float)` (move nếu thuần math)
   - `GetArea(RectRotate)`, `GetWorldPoly(RectRotate)` (chỉ nếu không reference runtime state)
2. Tạo `BeeCore/Func/PolygonHelper.cs`:
   ```csharp
   using System.Collections.Generic;
   using System.Drawing;
   namespace BeeCore.Func
   {
       public static class PolygonHelper
       {
           public static PointF Rotate(PointF p, float deg) { /* copy body */ }
           public static float PolygonArea(IList<PointF> poly) { /* */ }
           // ... 14 method total, đổi từ private → public static
           public static bool IsFinite(float x) => !float.IsNaN(x) && !float.IsInfinity(x);
       }
   }
   ```
3. Trong `ResultFilter.cs`:
   - Thêm `using BeeCore.Func;` đầu file (nếu chưa có).
   - Với mỗi call site cũ trong file (ví dụ `Rotate(p, 45f)`) → thay thành `PolygonHelper.Rotate(p, 45f)`.
   - **Xoá 14 method private** (sau khi đã move).
   - **Không đổi** public method signature của `ResultFilter`.
4. Update `BeeCore.csproj`: `<Compile Include="Func\PolygonHelper.cs" />`.
5. Build verify.
6. Grep ngoài ResultFilter xem có ai gọi những method này qua reflection không (rất hiếm):
   ```
   grep -rn "\"PolygonArea\"\|\"Intersect\"\|\"PointInPolygon\"" --include="*.cs" .
   ```

**Verify**:
| Command | Expected |
|---|---|
| Build | pass, warning ≤ baseline |
| `grep -c "private static" BeeCore/Func/ResultFilter.cs` | giảm đi 14 (approx) |
| `wc -l BeeCore/Func/PolygonHelper.cs` | ≥ 200 (14 method) |
| Smoke: chạy project có ResultFilter dùng (multi-pattern / counter) | kết quả identical |

**Rollback**: `git reset --hard HEAD~1`. Hoặc `git revert <sha>` nếu đã push.

**DoD**:
- [ ] 14 method ở `PolygonHelper` là `public static`.
- [ ] `ResultFilter` không còn 14 method đó ở dạng `private static`.
- [ ] Public API của `ResultFilter` không đổi.
- [ ] Build pass, smoke pass.
- [ ] History entry `[PlanCodex1.S8]`.

**Stop-and-ask**: nếu method nào trong 14 kia có reference runtime state (ví dụ `Common.PropetyTools`), **không** extract — để lại `ResultFilter.cs` là private, ghi chú lý do vào History.

---

### 🔹 Task S9 — Dedup GeometryHelper

**Mục tiêu**: 1 canonical GeometryHelper ở `BeeCore/ShapeEditing/GeometryHelper.cs` chứa tất cả 9 method union; `BeeInterface/ShapeEditing/GeometryHelper.cs` → shim delegate; `Common.cs` xoá 2 method trùng (`RotateAround`, `GetBoundingBox`).

**Preconditions**:
- [ ] S8 done (để tránh conflict khi merge polygon-related method).
- [ ] Đọc kỹ 2 file + đoạn method trong `Common.cs`.
- [ ] Baseline build ghi lại.

**In-scope**:
- `BeeCore/ShapeEditing/GeometryHelper.cs` (mở rộng).
- `BeeInterface/ShapeEditing/GeometryHelper.cs` (shim).
- `BeeCore/Common.cs` (xoá 2 method + thêm `using BeeCore.ShapeEditing;`).
- Call site `Common.RotateAround`, `Common.GetBoundingBox`, `GeometryHelper.*`.

**Out-of-scope**:
- Không động field/static state trong Common (listCamera, PropetyTools, HSVSample, v.v.).
- Không đổi public API khác của Common.

**Steps**:
1. Diff 2 GeometryHelper:
   ```
   diff <(grep -E "public static" BeeCore/ShapeEditing/GeometryHelper.cs) \
        <(grep -E "public static" BeeInterface/ShapeEditing/GeometryHelper.cs)
   ```
   - Method trùng (signature giống): `RotateAround`, `RotateVector`, `TransformPoint`, `BuildLocalInverseMatrixFor`, `BboxOf`.
   - Chỉ ở BeeCore: `HitTestPolygonVertex`, `HitTestCornerHandle`.
   - Chỉ ở BeeInterface: `RotatePoint(float, PointF)`, `GetPolygonBoundsLocal`, `BoundsContainAll`, `HexBoundsContainAll`.
2. Compare body của 5 method trùng. Nếu identical → giữ bản BeeCore. Nếu khác → **stop-and-ask** user.
3. Vào `BeeCore/ShapeEditing/GeometryHelper.cs`, thêm 4 method còn thiếu (`RotatePoint`, `GetPolygonBoundsLocal`, `BoundsContainAll`, `HexBoundsContainAll`) — copy body từ BeeInterface sang.
4. Trong `BeeInterface/ShapeEditing/GeometryHelper.cs`: thay **toàn bộ body** bằng shim:
   ```csharp
   using System.Collections.Generic;
   using System.Drawing;
   using System.Drawing.Drawing2D;
   using BeeCoreGH = BeeCore.ShapeEditing.GeometryHelper;
   namespace BeeInterface.ShapeEditing
   {
       [System.Obsolete("Use BeeCore.ShapeEditing.GeometryHelper")]
       public static class GeometryHelper
       {
           public static PointF RotateAround(PointF p, PointF c, float d) => BeeCoreGH.RotateAround(p, c, d);
           public static PointF RotateVector(PointF v, float d) => BeeCoreGH.RotateVector(v, d);
           public static PointF RotatePoint(float d, PointF p) => BeeCoreGH.RotatePoint(d, p);
           public static PointF TransformPoint(Matrix m, PointF p) => BeeCoreGH.TransformPoint(m, p);
           public static Matrix BuildLocalInverseMatrixFor(RectRotate rr, float z, Point s, bool u, PointF dc, float aw)
               => BeeCoreGH.BuildLocalInverseMatrixFor(rr, z, s, u, dc, aw);
           public static RectangleF BboxOf(IList<PointF> pts) => BeeCoreGH.BboxOf(pts);
           public static RectangleF GetPolygonBoundsLocal(RectRotate rr) => BeeCoreGH.GetPolygonBoundsLocal(rr);
           public static bool BoundsContainAll(RectangleF r, IList<PointF> pts) => BeeCoreGH.BoundsContainAll(r, pts);
           public static bool HexBoundsContainAll(RectRotate rr) => BeeCoreGH.HexBoundsContainAll(rr);
       }
   }
   ```
5. Trong `BeeCore/Common.cs`:
   - **Xoá** method `RotateAround` (line ~108) và `GetBoundingBox` (line ~77).
   - Thêm trên đầu file `using BeeCore.ShapeEditing;`.
   - Tìm call site trong cùng Common.cs (nếu có ai gọi `RotateAround`/`GetBoundingBox` nội bộ) → thay `GeometryHelper.RotateAround(...)`, `GeometryHelper.BboxOf(...)` (hoặc tên tương đương).
6. Grep toàn repo:
   ```
   grep -rn "Common\.RotateAround\|Common\.GetBoundingBox" --include="*.cs" --exclude-dir=bin --exclude-dir=obj .
   ```
   - Thay thành `GeometryHelper.RotateAround(...)` / `GeometryHelper.BboxOf(...)` (note: `GetBoundingBox` tên khác → map đúng).
   - **CẨN THẬN**: `GetBoundingBox(RectRotate)` ở Common có signature khác `BboxOf(IList<PointF>)` ở GeometryHelper → có thể cần giữ lại ở Common dưới dạng method delegate hoặc bổ sung overload vào GeometryHelper. Nếu không rõ → **stop-and-ask**.
7. Build verify lần 1 (sau step 5).
8. Build verify lần 2 (sau step 6).

**Verify**:
| Command | Expected |
|---|---|
| Build Release x64 | pass, warning ≤ baseline |
| `grep -c "public static" BeeCore/ShapeEditing/GeometryHelper.cs` | 9-11 method |
| `grep -c "public static" BeeInterface/ShapeEditing/GeometryHelper.cs` | 9 method (shim) |
| `grep -n "RotateAround\|GetBoundingBox" BeeCore/Common.cs` | 0 hit (đã xoá) |
| `bash tools/check_propety_tools.sh` | exit 0 |
| Smoke: mở tool có ShapeEditing (MatchingShape) — vẽ polygon, rotate, drag handle | không exception, visual OK |

**Rollback**: `git reset --hard HEAD~1`. Nếu đã merge → `git revert`.

**DoD**:
- [ ] GeometryHelper canonical chứa union 9-11 method.
- [ ] BeeInterface version là shim + `[Obsolete]`.
- [ ] Common.cs xoá `RotateAround` + `GetBoundingBox`.
- [ ] Build pass, smoke ShapeEditing pass.
- [ ] `[PlanCodex1.S9]` history entry.

**Stop-and-ask**: bất kỳ body method nào khác nhau giữa 2 file GeometryHelper → hỏi user.

---

### 🔹 Task S10 — Kéo local helper còn lại vào PolygonHelper/Geometry2D

**Mục tiêu**: 3 local method còn sót ở Crop.cs / Line2DTransform.cs → đúng nhà.

**In-scope**:
- `BeeCore/Func/Crop.cs` (xoá `RotatePoint`, `GetPolygonBounds`).
- `BeeCore/Func/Line2DTransform.cs` (xoá/expose `LineFromTwoPoints`).
- `BeeCore/Func/PolygonHelper.cs` (thêm overload nếu cần).
- `BeeCore/Algorithm/Geometry2D.cs` (thêm `LineFromTwoPoints` nếu chưa có).

**Steps**:
1. `Crop.cs` dòng ~1063 có `private static Point2f RotatePoint(Point2f p, float degree)`. Thêm vào `PolygonHelper`:
   ```csharp
   public static OpenCvSharp.Point2f RotatePoint(OpenCvSharp.Point2f p, float degree) { /* */ }
   ```
   (Lưu ý: signature khác với `GeometryHelper.RotatePoint` vì dùng `Point2f` từ OpenCvSharp, không phải `PointF` từ `System.Drawing`. Giữ ở PolygonHelper vì liên quan shape. Nếu PolygonHelper không ref OpenCvSharp → cân nhắc đặt vào `BitmapHelper` hoặc extend `MatHelper`. Trước khi chọn, check `using` của 2 file hiện tại.)
2. `Crop.cs` có `private static void GetPolygonBounds(IList<PointF>, out float, out float, out float, out float)`. Thêm vào `PolygonHelper`:
   ```csharp
   public static void GetPolygonBounds(IList<PointF> pts, out float minX, out float minY, out float maxX, out float maxY) { /* */ }
   ```
3. Trong `Crop.cs`: thay call nội bộ `RotatePoint(...)` → `PolygonHelper.RotatePoint(...)`; `GetPolygonBounds(...)` → `PolygonHelper.GetPolygonBounds(...)`. **Xoá** định nghĩa private.
4. `Line2DTransform.cs`: nếu `LineFromTwoPoints(PointF, PointF)` là `private static` + logic thuần toán → đổi thành `public static` (expose) hoặc move vào `Geometry2D.cs`. Quyết định: nếu `Line2D` type ở Algorithm/ → move; nếu ở Func/ → expose tại chỗ.
5. Update csproj nếu có file mới (không có — chỉ edit).

**Verify**: build pass; smoke pass (Crop-related tool).

**DoD**: tương tự S8.

**Stop-and-ask**: nếu `RotatePoint(Point2f)` xung đột namespace với `GeometryHelper.RotatePoint(PointF)` → hỏi user cách đặt tên (ví dụ `RotatePointF`, `RotatePoint2f`).

---

### 🔹 Task S11 — Regex guard + Smoke tổng + History tổng

**Mục tiêu**: xác nhận toàn bộ helper đã gom; không tạo regression.

**Steps**:
1. Chạy toàn bộ guard:
   ```
   bash tools/check_propety_tools.sh
   ```
2. Regex phát hiện duplicate còn sót:
   ```
   # 2 file cùng tên class helper
   grep -rln "public static class CustomGui\b" --include="*.cs" --exclude-dir=bin --exclude-dir=obj .
   grep -rln "public static class GeometryHelper\b" --include="*.cs" --exclude-dir=bin --exclude-dir=obj .
   grep -rln "class Convert2\b\|class Converts\b" --include="*.cs" --exclude-dir=bin --exclude-dir=obj .
   ```
   - `CustomGui`: 1 canonical + 2 shim (OK, flag với `[Obsolete]`).
   - `GeometryHelper`: 1 canonical + 1 shim.
   - `Converts`/`Convert2`: 1 shim + 1 canonical chỉ domain method.
3. Đếm private static helper "ít nghi ngờ" còn lại (báo cáo, không ép):
   ```
   grep -rnE "private static [A-Za-z<>, \[\]]+ (Rotate|Parse|Is|Has|To|From|Get|Convert|Clamp)[A-Z]" \
       --include="*.cs" --exclude-dir=bin --exclude-dir=obj \
       BeeCore BeeInterface BeeUi BeeGlobal | wc -l
   ```
   - Ghi con số vào history.
4. Full build Release x64. Warning count ghi vào history, so baseline.
5. Smoke test tổng:
   - Mở app → chờ init.
   - Load 1 project mẫu (Codex hỏi user path nếu không có sẵn trong `Resources/`).
   - Chạy 1 vòng inspection trên ảnh tĩnh.
   - Mở 3 form: ToolCircle, ToolPattern, ToolEdit (dùng ShapeEditing).
   - Click RoundRg-styled button (kiểm CustomGui path).
   - Chạy 1 result có ResultFilter (multi-pattern / counter).
   - Không có exception.
6. Append `docs/architecture/CODEX_HISTORY.md`:
   ```markdown
   ## YYYY-MM-DD — PlanCodex1 Phase A-B closed

   Scope:
   - Consolidated helper classes across C# projects.
   - Tasks done: S1, S2, S3, S4, S5, S6, S7, S8, S9, S10.

   Files touched: <list>
   Files deleted (replaced by shim): <list>
   Files created: <list>

   Verify:
   - Build Release x64: pass, warnings = <N> (baseline = <M>)
   - check_propety_tools.sh: exit 0
   - Smoke: loaded project mẫu + 3 forms + 1 inspection — pass

   Notes for future agents:
   - Obsolete shim ở: BeeCore/Converts.cs (Convert2), BeeCore/Func/CustomGui.cs, BeeInterface/CustomGui.cs (Gui), BeeInterface/ConvertImg.cs, BeeCore/BitmapExtensionsAlias.cs, BeeCore/MatrixExtensionAlias.cs, BeeInterface/ShapeEditing/GeometryHelper.cs.
   - Các shim này nên bị xoá trong Phase 2 CLAUDE.md (khi tách BeeCore sub-projects) hoặc Phase 6 (rename).
   - PolygonHelper + GeometryHelper giờ là canonical; mọi polygon/point math mới PHẢI vào đây.

   Blockers: <nếu có>
   ```

**DoD**: all check pass; history ghi nhận.

---

## 5. Pre-flight Checklist Tổng Hợp

Trước khi chạy BẤT KỲ task S1-S10 nào:

```
[ ] Đã đọc CLAUDE.md mục 0 (Hard Rules)
[ ] Đã đọc docs/architecture/CODEX_HISTORY.md 3 entry cuối
[ ] Đã đọc PlanCodex1.md mục 0 (hiện trạng) + mục 1 (target) + mục 2 (rules)
[ ] Đã xác định Task Card ID (S<n>)
[ ] Đã copy Task Card vào docs/architecture/tasks/YYYY-MM-DD-PlanCodex1.S<n>-<slug>.md
[ ] git status: không có file dirty ngoài scope task (hoặc user đã confirm)
[ ] Build baseline (docs/architecture/baseline_build.md) đã tồn tại
[ ] Hiểu rõ In-scope / Out-of-scope / Verify / DoD của task
[ ] Biết rollback procedure
```

---

## 6. Anti-Pattern — Điều KHÔNG Được Làm

1. **KHÔNG** xoá file cũ ngay khi move — luôn tạo shim `[Obsolete]` delegate đến canonical. Shim sẽ bị xoá ở Phase 2 CLAUDE.md khi tách project.
2. **KHÔNG** đổi namespace của public type đã serialize (kiểm `ClassProject.json` có reference class `Converts`, `CustomGui`, `GeometryHelper`, `BitmapExtensions`, `MatrixExtension` không). Nếu có → giữ namespace cũ, dùng `[TypeForwardedTo]`.
3. **KHÔNG** gộp 2 task vào 1 commit.
4. **KHÔNG** đụng `Pattern2.*`, `Pitch*`, native C++.
5. **KHÔNG** thêm dependency ref mới `BeeGlobal → BeeCore` (sẽ gây circular).
6. **KHÔNG** thêm `using BeeCore;` vào file ở `BeeGlobal/` hoặc `PLC_Communication/`.
7. **KHÔNG** tạo file helper ở root của project (mọi file mới vào sub-folder đúng concern).
8. **KHÔNG** format/reorder code trong class mà task không yêu cầu (tránh diff noise).
9. **KHÔNG** rename public method trong task này — rename là Phase 6 CLAUDE.md.
10. **KHÔNG** tạo call site `Common.PropetyTools[ip][it]` mới — dùng `Common.TryGetTool/EnsureToolList`.

---

## 7. Bảng Tóm Tắt File Affected

| File | Thao tác | Task | Kết quả cuối |
|---|---|---|---|
| `BeeCore/Converts.cs` | Replace by shim `[Obsolete] Convert2` | S1 | Shim, xoá ở Phase 2 CLAUDE.md |
| `BeeCore/Func/Converts.cs` | Shrink: xoá `BeforeFirstDigit`, `StringtoDouble`; giữ domain method | S1 | Domain-only |
| `BeeCore/Func/StringHelper.cs` | **New** | S1 | Canonical string helper |
| `BeeCore/Func/CustomGui.cs` | Replace by shim delegate | S2 | Shim |
| `BeeInterface/CustomGui.cs` | Replace by shim delegate | S2 | Shim |
| `BeeGlobal/Shared/CustomGui.cs` | **New** | S2 | Canonical |
| `BeeInterface/ConvertImg.cs` | Replace by shim | S3 | Shim |
| `BeeGlobal/Shared/ConvertImg.cs` | **New** (moved) | S3 | Canonical |
| `BeeCore/BitmapExtensions.cs` | Delete (replace by alias in sub-folder) | S4 | Deleted |
| `BeeCore/Func/BitmapHelper.cs` | **New** (moved + renamed) | S4 | Canonical |
| `BeeCore/Func/BitmapExtensionsAlias.cs` | **New** (shim, namespace BeeCore) | S4 | Shim alias |
| `BeeCore/MatrixExtension.cs` | Delete (replace by alias in sub-folder) | S5 | Deleted |
| `BeeCore/Func/MatrixHelper.cs` | **New** (moved + renamed) | S5 | Canonical |
| `BeeCore/Func/MatrixExtensionAlias.cs` | **New** (shim, namespace BeeCore) | S5 | Shim alias |
| `BeeCore/Func/ImageUtils.cs` | Rename class `ImageUtils73` → `ImageUtils` | S6 | Giữ file |
| `BeeCore/KeepLargestAuto.cs` | `git mv` → Algorithm/ | S7 | Moved |
| `BeeCore/Func/ResultFilter.cs` | Xoá 14 private polygon method, dùng PolygonHelper | S8 | Slim hơn |
| `BeeCore/Func/PolygonHelper.cs` | **New** | S8 | Canonical |
| `BeeCore/ShapeEditing/GeometryHelper.cs` | Expand: bổ sung 4 method từ BeeInterface | S9 | Canonical |
| `BeeInterface/ShapeEditing/GeometryHelper.cs` | Replace by shim delegate | S9 | Shim |
| `BeeCore/Common.cs` | Xoá `RotateAround`, `GetBoundingBox` | S9 | Slim hơn |
| `BeeCore/Func/Crop.cs` | Xoá `RotatePoint`, `GetPolygonBounds` | S10 | Slim hơn |
| `BeeCore/Func/Line2DTransform.cs` | Expose/move `LineFromTwoPoints` | S10 | — |

**Estimate diff**: ~15-20 file sửa + ~6 file mới + ~6 shim. Mỗi Task Card commit riêng → 10 commit.

---

## 8. History Entry Template Riêng (dùng sau mỗi S<n>)

```markdown
## YYYY-MM-DD — PlanCodex1.S<n> <summary>

Scope:
- Target bucket: <tên class canonical>
- Source files merged/moved: <list>

Files touched:
- <file 1>: <what changed>
- <file 2>: <what changed>

Shim created: <list hoặc "none">

Build verification:
- Command: MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64
- Result: pass, warnings = <N> (baseline = <M>)

Smoke test:
- <mô tả cụ thể>
- Result: <pass/fail>

Notes for future agents:
- <nếu có pattern/edge case>

Call sites updated: <N> (grep output)
Blockers / left dirty: <nếu có>
```

---

## 9. FAQ / Stop-and-Ask Scenarios

| Tình huống | Hành động |
|---|---|
| Body method ở 2 file duplicate KHÁC NHAU (không chỉ whitespace) | **Stop-and-ask** — user chọn bản nào hoặc parameterize |
| Class được JSON deserialize (xuất hiện trong ClassProject.json) | **Stop-and-ask** — cần test backward-compat |
| Tìm thấy `Common.PropetyTools[ip][it]` trong file mình cần sửa | Thay bằng `Common.TryGetTool(ip, it, out var t)` **trong cùng commit nếu task S yêu cầu đụng file đó**; không thì **ghi chú** và skip |
| Method private có reference đến `Common.PropetyTools` / runtime state | Không extract thành public helper — để lại tại chỗ, ghi history |
| File dirty (user đang sửa) rơi vào In-scope | **Stop-and-ask** — stash/commit/bỏ qua |
| Build baseline chưa tồn tại | Chạy CLAUDE.md P0.2 trước |
| Smoke test fail sau commit | Rollback ngay (`git reset --hard HEAD~1` hoặc `git revert`), đừng fix-on-main |
| `ClassProject.json` mẫu không có trong repo | **Stop-and-ask** — xin user path đến 1 project .bee mẫu để test load |

---

## 10. Kết

PlanCodex1.md dừng ở "gom helper C# hiện có". Không đụng phase 2+ CLAUDE.md (tách project, rename, tool migration). Khi làm xong toàn bộ S1-S11, Phase 1 CLAUDE.md (dọn duplicate) coi như complete hơn dự kiến — các shim `[Obsolete]` sẽ được phase 2 xoá sạch khi tách sub-project.

**Thứ tự khuyến nghị bắt đầu: S1 → S2 → S3 → S4 → S5 → S6 → S7 → S8 → S9 → S10 → S11.**

Codex/Claude kế tiếp: đọc `CLAUDE.md` mục 0 + PlanCodex1.md mục 0-4 → chọn task theo thứ tự → điền Task Card template (CLAUDE.md mục 11.1) vào `docs/architecture/tasks/` → làm → verify → commit → history.

**Changelog PlanCodex1.md**:
- 2026-04-23: Tạo file, snapshot helper C# dựa trên grep thực tế. 11 task S1-S11 chi tiết, bucket target 10 class, respect hoàn toàn CLAUDE.md Hard Rules.
