# Session Summary — PinPitch Detection Bugfix

**Date**: 2026-05-08
**Session goal**: Diagnose & fix 4 user-reported defects in PinPitch detection on bright square pad image.

---

## User complaints (input)

1. Sai cả 3 center, biên dạng góc xoay.
2. Không khoá được pin xéo nhiều hướng → phải tìm biên đúng + tâm đúng rồi mới tính distance.
3. Bóng mờ pin vẫn được tính là pin (background mặc định = đen).
4. Phải dựa vào toàn biên dạng pin, không phụ thuộc đốm sáng (specular highlight).

---

## Diagnosis (Bug map)

| Bug | Triggered by | Where |
|---|---|---|
| **A — Per-pin rotation bị vứt** | complaints #1, #2 | `BeeCore/Unit/Pitch.cs::ApplyPinPitchResultToPoints`, line ~320 hardcode `RectRotate(... 6×6 marker, rotation=0 ...)` mặc dù `pin.AngleDeg` đã được CLI marshal qua. |
| **B — Center bị kéo bởi halo** | #1, #3, #4 | `Pattern/PinPitchCore.cpp::BuildMask`: percentile-10 + offset 14 quá lỏng; dilate ellipse 5px phình halo; minAreaRect lấy center của (pin+halo). |
| **C — Background không thuần đen** | #3 | Plan.md đã đề xuất CLAHE/top-hat nhưng code thiếu. Halo bị giữ lại. |
| **D — Center không dùng silhouette ngoài** | #4 | Code chưa fit 4 cạnh ngoài (đã đề xuất trong Plan.md step 5). Center phụ thuộc thuần binarization. |

---

## Fixes implemented

### Native (`Pattern/PinPitchCore.{h,cpp}` + `PinPitchCli.{h,cpp}`)

Thêm 4 options vào `PinPitchOptions`:

```cpp
bool useTopHat = false;
int topHatKernelPx = 0;          // 0 = auto = max(81, min_dim * 3 / 5)
double minSolidity = 0.0;        // 0 = không filter
bool reduceDilateForOutline = false;
```

Pipeline change trong `BuildMask`:
1. Khi `useTopHat=true`: chạy `cv::MORPH_TOPHAT` ellipse kernel (auto > pin size) trước threshold; sau top-hat dùng Otsu (background đã normalize ≈ 0).
2. Khi `reduceDilateForOutline=true`: kẹp `outlineDilate ≤ 3` để không phình halo.

Filter trong `BuildCandidate`:
- Nếu `minSolidity > 0`: tính `solidity = contourArea / convexHullArea`, reject nếu `< minSolidity`. Pin vuông thật ~1.0; halo merge → solidity giảm mạnh.

CLI `SetOptions(...)` thêm 4 tham số tương ứng.

### Managed (`BeeCore/Unit/Pitch.cs`)

**Bug A fix** trong `ApplyPinPitchResultToPoints`:
```csharp
// trước:
rectRotates.Add(new RectRotate(new RectangleF(p.X - 3, p.Y - 3, 6, 6), p, 0, AnchorPoint.None));

// sau:
float wPx = (float)pin.WidthPx; if (wPx < 1f) wPx = 6f;
float hPx = (float)pin.HeightPx; if (hPx < 1f) hPx = 6f;
float rotDeg = (float)pin.AngleDeg + area._rectRotation;  // crop-local + world rotation
rectRotates.Add(new RectRotate(
    new RectangleF(-wPx * 0.5f, -hPx * 0.5f, wPx, hPx),   // local convention (-w/2,-h/2,w,h)
    p,
    rotDeg,                                                 // RectRotate dùng degrees
    AnchorPoint.None));
```

Serialized fields mới (default backward-compat):
```csharp
public bool UseTopHat = false;
public int TopHatKernelPx = 0;
public double MinSolidity = 0.80;
public bool ReduceDilateForOutline = false;
```

`RunPinPitch` truyền 4 tham số mới qua `PinPitchMeasure.SetOptions(...)`.

---

## Verification

### Build
- `Pattern.vcxproj` Release x64: pass, 0 error
- `BeeCore.csproj` Release x64: pass, 0 error
- Full `EasyVision.sln` Release x64: pass, 0 error (warnings tương đương baseline)

### Algorithmic trial (Python mirror script)

Script: [`docs/PinPitchMeasurePlan/trial/_run_trial.py`](trial/_run_trial.py) — mô phỏng đúng `BuildMask` + `BuildCandidate` qua OpenCV-Python, so sánh 3 cấu hình OLD vs NEW vs NEW+TopHat trên 6 ảnh trial.

Output: `<name>_OLDvsNEW.png` (2×3 grid: vis OLD/NEW/NEW+TH + mask tương ứng).

| Image | OLD | NEW (sol≥0.80) | NEW + topHat |
|---|---|---|---|
| `10_pinrow_raw.png` (raw input) | 4 pins, sol=1.0 | **4 pins identical centers** ✅ | 4 pins, boxes tighter |
| `13_pad_only.png` | 4 | 4 | 4 (tighter) |
| `A_bg8_off10_close9_d3.png` | 4 | 4 | 4 (tighter) |
| `B_bg10_off14_close11_d3.png` | 4 | 4 | 4 (tighter) |
| `C_bg5_off8_close13_d5.png` | 4 (gồm noise 28×13!) ❌ | 4 (vẫn lấy noise) | **3 real pins** ✅ |
| `D_bg15_off12_close9_d1.png` | 4 | 4 | 4 (tighter) |

**Findings:**
- Default solidity ≥ 0.80 không gây regression — pin thật luôn có solidity 0.85–0.99.
- Top-hat opt-in giúp box detect bám sát pin hơn (loại halo) và reject blob noise (case C). Auto kernel `max(81, min_dim * 3/5)` an toàn > pin size cho ROI tỷ lệ chuẩn.
- Top-hat OFF mặc định an toàn cho tools đã tồn tại.

---

## Files changed (uncommitted)

- `Pattern/PinPitchCore.h`
- `Pattern/PinPitchCore.cpp`
- `Pattern/PinPitchCli.h`
- `Pattern/PinPitchCli.cpp`
- `BeeCore/Unit/Pitch.cs`
- `docs/PinPitchMeasurePlan/trial/_run_trial.py` (new — trial harness)
- `docs/PinPitchMeasurePlan/trial/*_OLDvsNEW.png` (6 generated comparison images)

**Commit advice**: chỉ `git add` các file trên — KHÔNG `git add .` vì main repo đang dirty với ~80 file thay đổi khác không liên quan.

---

## Out-of-scope (chưa làm)

| Item | Lý do |
|---|---|
| **Step 5 — UI controls** ở `ToolPitch.cs/.Designer.cs` (checkbox UseTopHat, numeric TopHatKernelPx, MinSolidity, ReduceDilateForOutline) | Hiện chưa expose runtime; bind được qua deserialize. Yêu cầu confirm trước khi thêm UI. |
| **Step 3 — Edge-based 4-corner refinement** | Effort > 1 ngày. Plan trước khi commit chờ user verify Step 1+2+4 chưa đủ. Đã có anchor trong `RefineCandidateWithOutline`. |
| **`RefineWithFourEdges`** native + `PinCenterCli.CenterMethod` enum marshal | Chỉ cần khi top-hat + solidity vẫn để lệch tâm. |
| **±0.05 mm repeat capture validation** | Cần camera + sample thực tế; user sẽ chạy. |

---

## Stop-and-ask context (resolved)

| Q | Answer |
|---|---|
| `RectRotate` ctor — deg or rad? | **Degrees**. Verified in `BeeGlobal/RectRotate.cs:35,84,700`. `pin.AngleDeg + area._rectRotation` truyền trực tiếp. |
| Có ảnh background non-uniform không? | Trial folder có `10_pinrow_raw.png` — background gần đen nhưng có halo mờ quanh pin. Top-hat hữu ích. |
| Giữ percentile threshold cũ? | Yes, default vẫn dùng (`useTopHat=false` mặc định). |
| Step 3 effort > 1 ngày — gộp? | **Không**. Hoãn, làm Step 1+2+4 trước. |

---

## Verification before next session

1. Mở app, load 1 project có ToolPitch (PinPitch mode) đã serialize trước fix:
   - Confirm `MeasureMode == PinPitch` vẫn detect 4 pins (no regression).
   - Confirm `rectRotates[i]._rectRotation` ≠ 0 với pin xéo (Bug A fix).
2. Tạo project mới có pin xéo nhiều hướng:
   - Bật `UseTopHat=true` qua serialization (tạm — chờ UI).
   - Confirm box detect bám sát pin, không lan halo.
3. Test backward-compat: load `ClassProject.json` cũ — 4 fields mới được set default qua deserialize.

---

## References

- Original spec: [`Plan.md`](Plan.md)
- Implementation roadmap: [`CoreGuiImplementationPlan.md`](CoreGuiImplementationPlan.md)
- Earlier session log: [`Implementation_Summary_2026-05-08.md`](Implementation_Summary_2026-05-08.md)
- This session's plan file: `C:\Users\chitu\.claude\plans\preivew-docs-pinpitchmeasureplan-check-l-quizzical-allen.md`
- CLAUDE.md hard rule 0.3 (Stop-and-ask): worktree dirty → user xác nhận direct main-repo edit
