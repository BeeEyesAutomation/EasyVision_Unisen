"""
Trial PinPitch detection: OLD vs NEW vs EDGE options on trial/*.png.

Mô phỏng đúng các bước trong Pattern/PinPitchCore.cpp::BuildMask + BuildCandidate
trên Python/OpenCV. So sánh:
  - OLD:  threshold-based mask, minSolidity=0 (behavior cũ trước fix)
  - NEW:  threshold-based mask, minSolidity=0.80 (PP-004 — chặn halo lan)
  - EDGE: Canny edge mask + edge-geometry center (PP-007 — fix runtime
          failures: bias bright spot, bóng mờ tính là pin, pin xéo)

Output: <name>_OLDvsNEW.png ghép 3 cột row1 (detect overlay) + row2 (mask) và
in bảng pin (center, angle, area, solidity, method) ra console.
"""
from __future__ import annotations
import os
from dataclasses import dataclass

import cv2
import numpy as np

TRIAL_DIR = os.path.dirname(os.path.abspath(__file__))


@dataclass
class Options:
    useOutlineCenter: bool = True
    outlineThresholdOffset: int = 14
    outlineClose: int = 7
    outlineDilate: int = 5
    minAreaPx: float = 12.0
    maxAreaRatio: float = 0.10
    minAspect: float = 0.45
    maxAspect: float = 2.20
    minFillRatio: float = 0.20
    expectedCount: int = 4
    # New
    useTopHat: bool = False
    topHatKernelPx: int = 0
    minSolidity: float = 0.0
    reduceDilateForOutline: bool = False
    # PP-007: edge-boundary mask + edge-geometry center
    useEdgeBoundary: bool = False
    edgeCannyLow: int = 20
    edgeCannyHigh: int = 60
    useEdgeGeometryCenter: bool = False
    # PP-007 trial 2: gradient refinement (CLAHE + Sobel mag) trên patch quanh seed.
    useGradientRefinement: bool = False
    gradientPatchMargin: int = 60
    gradientThreshold: int = 25
    claheClipLimit: float = 3.0
    claheTileSize: int = 8


def percentile_8u(img: np.ndarray, p: float) -> float:
    hist = cv2.calcHist([img], [0], None, [256], [0, 256]).flatten().astype(int)
    total = img.size
    target = int(round((p / 100.0) * (total - 1)))
    cum = 0
    for i, h in enumerate(hist):
        cum += h
        if cum > target:
            return float(i)
    return 255.0


def build_mask(gray: np.ndarray, opt: Options) -> np.ndarray:
    # PP-007 branch: Canny edges → close → fill enclosed contours.
    # Threshold blob bias về bright reflection + bóng mờ pin pass threshold.
    # Canny chỉ phản hồi gradient sắc -> theo biên thật pin pad, shadow tự loại.
    if opt.useEdgeBoundary:
        blur = cv2.GaussianBlur(gray, (5, 5), 0)
        if opt.useTopHat:
            k = opt.topHatKernelPx
            if k <= 0:
                dim = min(gray.shape[:2])
                k = max(81, (dim * 3) // 5)
            if k % 2 == 0:
                k += 1
            kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (k, k))
            blur = cv2.morphologyEx(blur, cv2.MORPH_TOPHAT, kernel)

        edges = cv2.Canny(blur, opt.edgeCannyLow, opt.edgeCannyHigh)
        close_k = (opt.outlineClose if opt.outlineClose > 1 else 7) | 1
        kernel = cv2.getStructuringElement(cv2.MORPH_RECT, (close_k, close_k))
        edges = cv2.morphologyEx(edges, cv2.MORPH_CLOSE, kernel)

        cnts, _ = cv2.findContours(edges, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
        filled = np.zeros_like(gray)
        cv2.drawContours(filled, cnts, -1, 255, cv2.FILLED)

        if opt.outlineDilate > 1:
            d = min(opt.outlineDilate, 3) if opt.reduceDilateForOutline else opt.outlineDilate
            d |= 1
            dk = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (d, d))
            filled = cv2.dilate(filled, dk)
        return filled

    blur_size = (5, 5) if opt.useOutlineCenter else (3, 3)
    work = cv2.GaussianBlur(gray, blur_size, 0)

    if opt.useTopHat:
        k = opt.topHatKernelPx
        if k <= 0:
            dim = min(gray.shape[:2])
            k = max(81, (dim * 3) // 5)
        if k % 2 == 0:
            k += 1
        kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (k, k))
        work = cv2.morphologyEx(work, cv2.MORPH_TOPHAT, kernel)

    if opt.useTopHat:
        _, mask = cv2.threshold(work, 0, 255, cv2.THRESH_BINARY | cv2.THRESH_OTSU)
    elif opt.useOutlineCenter:
        bg = percentile_8u(work, 10.0)
        thr = max(0.0, min(255.0, bg + opt.outlineThresholdOffset))
        _, mask = cv2.threshold(work, thr, 255, cv2.THRESH_BINARY)
    else:
        _, mask = cv2.threshold(work, 0, 255, cv2.THRESH_BINARY | cv2.THRESH_OTSU)

    if opt.outlineClose > 1:
        k = opt.outlineClose | 1
        kernel = cv2.getStructuringElement(cv2.MORPH_RECT, (k, k))
        mask = cv2.morphologyEx(mask, cv2.MORPH_CLOSE, kernel)

    if opt.useOutlineCenter and opt.outlineDilate > 1:
        d = min(opt.outlineDilate, 3) if opt.reduceDilateForOutline else opt.outlineDilate
        k = d | 1
        kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (k, k))
        mask = cv2.dilate(mask, kernel)

    return mask


def edge_geometry_center(contour: np.ndarray, rect) -> tuple:
    """PP-007: midpoint của projection bounds trên 2 trục minAreaRect.
    Robust với pin xéo + bright spot off-center vs blob centroid."""
    (cx, cy), (rw, rh), ang = rect
    if len(contour) < 4 or rw < 2 or rh < 2:
        return cx, cy
    a = np.deg2rad(ang)
    ax = np.array([np.cos(a), np.sin(a)], dtype=np.float32)
    ay = np.array([-np.sin(a), np.cos(a)], dtype=np.float32)
    pts = contour.reshape(-1, 2).astype(np.float32) - np.array([cx, cy], dtype=np.float32)
    u = pts @ ax
    v = pts @ ay
    uc = float(u.min() + u.max()) * 0.5
    vc = float(v.min() + v.max()) * 0.5
    return (cx + uc * ax[0] + vc * ay[0],
            cy + uc * ax[1] + vc * ay[1])


def refine_by_gradient(gray: np.ndarray, cand: dict, opt: Options) -> bool:
    """Mirror C++ RefineByGradient: từ seed, tìm full pad boundary qua CLAHE+Sobel."""
    rect = cand["rect"]
    box_pts = cv2.boxPoints(rect)
    bx, by, bw, bh = cv2.boundingRect(box_pts.astype(np.intp))
    m = max(20, opt.gradientPatchMargin)
    h, w = gray.shape[:2]
    x0 = max(0, bx - m); y0 = max(0, by - m)
    x1 = min(w, bx + bw + m); y1 = min(h, by + bh + m)
    if x1 - x0 < 8 or y1 - y0 < 8:
        return False
    roi = gray[y0:y1, x0:x1]
    blur = cv2.GaussianBlur(roi, (5, 5), 0)
    clahe = cv2.createCLAHE(clipLimit=opt.claheClipLimit,
                            tileGridSize=(opt.claheTileSize, opt.claheTileSize))
    enhanced = clahe.apply(blur)
    gx = cv2.Sobel(enhanced, cv2.CV_32F, 1, 0, ksize=3)
    gy = cv2.Sobel(enhanced, cv2.CV_32F, 0, 1, ksize=3)
    mag = cv2.magnitude(gx, gy)
    magU8 = cv2.normalize(mag, None, 0, 255, cv2.NORM_MINMAX, cv2.CV_8U)
    _, edges = cv2.threshold(magU8, opt.gradientThreshold, 255, cv2.THRESH_BINARY)
    closeK = max(11, opt.outlineClose) | 1
    edges = cv2.morphologyEx(edges, cv2.MORPH_CLOSE,
                             cv2.getStructuringElement(cv2.MORPH_RECT, (closeK, closeK)))
    cnts, _ = cv2.findContours(edges, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    if not cnts:
        return False
    seed_local = (cand["rect"][0][0] - x0, cand["rect"][0][1] - y0)
    best = None; best_area = 0.0
    for c in cnts:
        if len(c) < 4:
            continue
        area = abs(cv2.contourArea(c))
        if area < cand["area"] * 0.8:
            continue
        if cv2.pointPolygonTest(c, seed_local, False) < 0:
            continue
        if area > best_area:
            best = c; best_area = area
    if best is None:
        return False
    max_area = max(opt.minAreaPx, opt.maxAreaRatio * w * h)
    if best_area > max_area:
        return False
    shifted = best.reshape(-1, 2) + np.array([x0, y0])
    shifted = shifted.reshape(-1, 1, 2).astype(np.int32)
    rr = cv2.minAreaRect(shifted)
    (rcx, rcy), (rw_, rh_), rang = rr
    aspect = max(rw_, rh_) / max(1.0, min(rw_, rh_))
    if aspect < opt.minAspect or aspect > opt.maxAspect:
        return False
    fill = best_area / max(1.0, rw_ * rh_)
    if fill < opt.minFillRatio:
        return False
    if opt.useEdgeGeometryCenter:
        tcx, tcy = edge_geometry_center(shifted, rr)
        rr = ((float(tcx), float(tcy)), (rw_, rh_), rang)
    cand["rect"] = rr
    cand["area"] = best_area
    cand["fill"] = fill
    cand["contour"] = shifted
    return True


def find_candidates(gray: np.ndarray, mask: np.ndarray, opt: Options):
    contours, _ = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    h, w = gray.shape[:2]
    max_area = max(opt.minAreaPx, opt.maxAreaRatio * w * h)
    cands = []
    for c in contours:
        if len(c) < 4:
            continue
        area = abs(cv2.contourArea(c))
        if area < opt.minAreaPx or area > max_area:
            continue

        rect = cv2.minAreaRect(c)  # ((cx,cy),(w,h),angle)
        (cx, cy), (rw, rh), ang = rect
        if rw < 1 or rh < 1:
            continue
        aspect = max(rw, rh) / max(1.0, min(rw, rh))
        if aspect < opt.minAspect or aspect > opt.maxAspect:
            continue
        rect_area = max(1.0, rw * rh)
        fill = area / rect_area
        if fill < opt.minFillRatio:
            continue

        solidity = 1.0
        if opt.minSolidity > 0 and len(c) >= 3:
            hull = cv2.convexHull(c)
            hull_area = max(1.0, abs(cv2.contourArea(hull)))
            solidity = area / hull_area
            if solidity < opt.minSolidity:
                continue

        size_score = np.sqrt(max(0.0, area))
        aspect_score = 1.0 / aspect
        fill_score = max(0.0, min(1.0, fill))
        score = size_score * (0.65 * aspect_score + 0.35 * fill_score)

        if opt.useEdgeGeometryCenter:
            tcx, tcy = edge_geometry_center(c, rect)
            rect = ((float(tcx), float(tcy)), (rw, rh), ang)
        cands.append({
            "rect": rect,
            "area": area,
            "fill": fill,
            "solidity": solidity,
            "aspect": aspect,
            "score": score,
            "contour": c,
        })

    cands.sort(key=lambda c: (c["score"], c["area"]), reverse=True)
    cands = cands[: opt.expectedCount]
    if opt.useGradientRefinement:
        for c in cands:
            refine_by_gradient(gray, c, opt)
    return cands


def draw_pins(gray_bgr: np.ndarray, pins, color):
    out = gray_bgr.copy()
    for i, p in enumerate(sorted(pins, key=lambda c: c["rect"][0][0])):
        rect = p["rect"]
        box = cv2.boxPoints(rect).astype(np.intp)
        cv2.polylines(out, [box], True, color, 2, cv2.LINE_AA)
        cx, cy = int(rect[0][0]), int(rect[0][1])
        cv2.drawMarker(out, (cx, cy), (0, 0, 255), cv2.MARKER_CROSS, 14, 2)
        cv2.putText(out, f"P{i+1} a={rect[2]:.0f} sol={p['solidity']:.2f}",
                    (cx + 6, cy - 6), cv2.FONT_HERSHEY_SIMPLEX, 0.45,
                    (255, 255, 255), 1, cv2.LINE_AA)
    return out


def annotate(img: np.ndarray, label: str) -> np.ndarray:
    out = img.copy()
    cv2.rectangle(out, (0, 0), (out.shape[1], 28), (0, 0, 0), -1)
    cv2.putText(out, label, (8, 20), cv2.FONT_HERSHEY_SIMPLEX, 0.6,
                (255, 255, 255), 1, cv2.LINE_AA)
    return out


def run_one(name: str):
    path = os.path.join(TRIAL_DIR, name)
    img = cv2.imread(path, cv2.IMREAD_UNCHANGED)
    if img is None:
        print(f"[skip] cannot read {name}")
        return
    if img.ndim == 3:
        gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    else:
        gray = img
    bgr = cv2.cvtColor(gray, cv2.COLOR_GRAY2BGR)

    # OLD  = behavior cũ trước fix
    # NEW  = threshold + minSolidity=0.80
    # EDGE = PP-007: Canny edges + edge-geometry center (default mới của Pitch.cs)
    opt_old = Options(useTopHat=False, minSolidity=0.0, reduceDilateForOutline=False)
    opt_new = Options(useTopHat=False, minSolidity=0.80, reduceDilateForOutline=False)
    opt_edge = Options(useEdgeBoundary=True, useEdgeGeometryCenter=True,
                       useGradientRefinement=True,
                       gradientPatchMargin=60, gradientThreshold=25,
                       claheClipLimit=3.0, claheTileSize=8,
                       minSolidity=0.80, edgeCannyLow=10, edgeCannyHigh=40,
                       outlineClose=21, outlineDilate=3,
                       reduceDilateForOutline=True)

    mask_old = build_mask(gray, opt_old)
    mask_new = build_mask(gray, opt_new)
    mask_edge = build_mask(gray, opt_edge)

    cands_old = find_candidates(gray, mask_old, opt_old)
    cands_new = find_candidates(gray, mask_new, opt_new)
    cands_edge = find_candidates(gray, mask_edge, opt_edge)

    print(f"\n=== {name} ===  shape={gray.shape}")
    for label, cands in (("OLD", cands_old), ("NEW", cands_new), ("EDGE", cands_edge)):
        print(f"  {label}: pins={len(cands)}")
        for i, c in enumerate(sorted(cands, key=lambda x: x['rect'][0][0])):
            (cx, cy), (rw, rh), ang = c["rect"]
            print(f"    P{i+1} center=({cx:7.2f},{cy:7.2f}) "
                  f"size=({rw:5.1f}x{rh:5.1f}) angle={ang:6.2f} "
                  f"area={c['area']:7.1f} fill={c['fill']:.2f} sol={c['solidity']:.2f}")

    vis_old = annotate(draw_pins(bgr, cands_old, (0, 255, 0)),
                       f"OLD: pins={len(cands_old)}")
    vis_new = annotate(draw_pins(bgr, cands_new, (255, 0, 255)),
                       f"NEW (sol>=.80): pins={len(cands_new)}")
    vis_edge = annotate(draw_pins(bgr, cands_edge, (0, 200, 255)),
                        f"EDGE+geomCenter: pins={len(cands_edge)}")

    def m(x, label):
        return annotate(cv2.cvtColor(x, cv2.COLOR_GRAY2BGR), label)

    row1 = cv2.hconcat([vis_old, vis_new, vis_edge])
    row2 = cv2.hconcat([m(mask_old, "mask OLD"),
                        m(mask_new, "mask NEW"),
                        m(mask_edge, "mask EDGE")])
    grid = cv2.vconcat([row1, row2])
    out_name = os.path.splitext(name)[0] + "_OLDvsNEW.png"
    cv2.imwrite(os.path.join(TRIAL_DIR, out_name), grid)


if __name__ == "__main__":
    for n in sorted(os.listdir(TRIAL_DIR)):
        if not n.lower().endswith(".png"):
            continue
        if "_OLDvsNEW" in n or n.startswith("_"):
            continue
        # Bỏ các file đã là so sánh sẵn
        if any(k in n for k in ("compare",)):
            continue
        run_one(n)
