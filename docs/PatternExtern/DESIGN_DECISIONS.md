# Design Decisions — Pattern2 Multi-Template

Ghi lại 4 quyết định thiết kế đã chốt với user (qua AskUserQuestion) cùng lý do, để future-agent hiểu trade-off và không re-litigate.

## Q1 — Tích hợp multi-template vào đâu?

**Chọn: Extend ToolPattern2 (thêm checkbox Multi-Template)**

Lý do:
- Backward-compat dễ: project cũ load vẫn dùng single-mode mặc định.
- 1 tool duy nhất: tránh nhân đôi UI/save-load code.
- "Multi-Template" là option phụ, không phải tool riêng biệt.

Loại bỏ: Tạo `ToolMultiPattern2` riêng — sẽ duplicate Designer/Form/handler ~80%.

## Q2 — Per-template config hay shared?

**Chọn: Shared (chia sẻ angle range / preset preprocess / GPU / CPU multi-thread)**

Mỗi template chỉ giữ riêng:
- `Bitmap` (template image)
- `Label` (string)
- `ScoreThreshold` (0..100)
- `ExpectedCount` (≥0)
- `MaxPerTemplate` (0 = follow global)

Lý do:
- Khớp với pattern hiện tại của `MultiPattern.cs` (v1).
- Cho phép native preprocess source **1 lần** — không cần group entries theo preprocess hash (chính là speedup chính của approach).
- UI đơn giản: 1 DataGridView 4 cột thay vì panel per-entry với 15 control.

Loại bỏ: Per-template preset — phức tạp, đa số use case không cần.

## Q3 — Native batch hay C# loop?

**Chọn: Native batch (extend Pattern2.h thêm `MatchBatchStable`)**

Lý do:
- Preprocess source (CLAHE / Sobel / Canny) + build source pyramid là phần đắt nhất khi template nhỏ. Native batch chia sẻ phần này, tiết kiệm ~70-80% thời gian với 8 template.
- Refactor `MatchStable` thành `BuildSourceFeatures` + `MatchStableOnPreprocessedSource` cũng improve code quality của single-template path.
- User confirm chấp nhận chi phí thay đổi native side.

Loại bỏ: C# loop kiểu MultiPattern v1 — đơn giản hơn nhưng N× preprocess cost. Plan ghi nhận đây là fallback nếu native quá rủi ro.

## Q4 — OK/NG logic?

**Chọn: Per-label required-count**

Mỗi template entry có `ExpectedCount`:
- `ExpectedCount = 0`: optional, không xét OK/NG.
- `ExpectedCount ≥ 1`: số match có score ≥ threshold cho label đó phải ≥ ExpectedCount.

Overall OK ↔ mọi label trong list đều đạt ExpectedCount.

Lý do:
- Linh hoạt cover được cả 3 use case:
  - "Phải có A và B": 2 entry, ExpectedCount=1 cho cả 2.
  - "Đếm số lượng": ExpectedCount=N.
  - "Phân loại A/B/C": ExpectedCount=0 cho tất cả, chỉ cần biết label nào match (overall OK = true).
- Đối xứng với cách `MultiPattern.cs` v1 dùng `ValueCompare` (MultiPattern.cs:1846).

Loại bỏ:
- "Tất cả label phải xuất hiện ≥1 lần": subset của approach trên (ExpectedCount=1 cho tất cả).
- "Chỉ cần ≥1 label match": cần thêm flag riêng, ít dùng — defer.
