# Pattern2 Multi-Template Extension

Plan & reference materials cho việc mở rộng `Pattern/Pattern2.{h,cpp}` + `BeeCore/Unit/Patterns.cs` + `BeeInterface/Tool/ToolPattern.{cs,Designer.cs}` để hỗ trợ check nhiều mẫu (template) khác label trên cùng 1 ảnh, trong 1 lần Run.

## Files trong thư mục

| File | Nội dung |
|---|---|
| [PLAN.md](PLAN.md) | Plan chi tiết 12 section: context, approach, code skeleton native + C# + UI, implementation order 3 commit, verification matrix, risks, rollback. **Đọc đầu tiên.** |
| [REFERENCES.md](REFERENCES.md) | Snapshot các đoạn code critical trong codebase hiện tại (Pattern2.h structs, helper signatures, Patterns.cs call sites) để future-agent tra cứu nhanh không cần grep lại. |
| [DESIGN_DECISIONS.md](DESIGN_DECISIONS.md) | 4 quyết định thiết kế đã chốt với user (tích hợp / config sharing / native batch / OK-NG logic) — nguồn cho các trade-off trong PLAN. |

## Quick links

- Tracking issue / branch: TBD khi bắt đầu implement
- Liên quan: `docs/Pattern2FalseMatchPlan/` (false-match tuning cho single-template Pattern2 — context bổ sung)
- Memory dependencies: `feedback_ui_in_designer`, `feedback_collapsible_param_sections` trong `~/.claude/projects/.../memory/`

## Status

- [x] Phase 1: Exploration (3 Explore agents — Pattern2.cpp / Patterns.cs / MultiPattern reference)
- [x] Phase 2: Design decisions (AskUserQuestion ×4)
- [x] Phase 3: Plan written + expanded
- [ ] Phase 4: Commit 1 — Native batch API
- [ ] Phase 5: Commit 2 — C# engine branch
- [ ] Phase 6: Commit 3 — UI section
