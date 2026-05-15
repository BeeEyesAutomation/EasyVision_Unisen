#include "PinPitchCore.h"
#include "VisionGeometryCore.h"
#include <algorithm>
#include <cmath>
#include <numeric>

namespace BeeCpp {

void PinPitchCore::setImage(const cv::Mat& image)
{
    if (image.empty())
    {
        _image.release();
        return;
    }

    if (image.channels() == 1)
        image.copyTo(_image);
    else if (image.channels() == 3)
        cv::cvtColor(image, _image, cv::COLOR_BGR2GRAY);
    else if (image.channels() == 4)
        cv::cvtColor(image, _image, cv::COLOR_BGRA2GRAY);
    else
        image.convertTo(_image, CV_8UC1);
}

void PinPitchCore::setOptions(const PinPitchOptions& options)
{
    _options = options;
    if (_options.expectedCount < 1) _options.expectedCount = 1;
    if (_options.manualThreshold < 0) _options.manualThreshold = 0;
    if (_options.manualThreshold > 255) _options.manualThreshold = 255;
    if (_options.outlineClose < 0) _options.outlineClose = 0;
    if (_options.outlineDilate < 0) _options.outlineDilate = 0;
    if (_options.outlinePadding < 0) _options.outlinePadding = 0;
    if (_options.maxOutlineExpand < 0) _options.maxOutlineExpand = 0;
    if (_options.mmPerPixel <= 0.0) _options.mmPerPixel = 1.0;
}

PinPitchResultCore PinPitchCore::measure() const
{
    PinPitchResultCore result;
    result.scaleMmPerPx = _options.mmPerPixel;

    if (_image.empty())
    {
        result.message = "Input image is empty.";
        return result;
    }

    cv::Mat gray;
    if (_image.type() == CV_8UC1)
        _image.copyTo(gray);
    else
        _image.convertTo(gray, CV_8UC1);

    cv::Mat mask = BuildMask(gray, _options);
    std::vector<PinCenterResult> candidates = FindCandidates(gray, mask, _options);

    // Bug 1+3+4 follow-up (PP-007 trial 2026-05-08): edge-boundary mask
    // (Canny→close→fill) cho biên đúng nhưng MISS pin contrast yếu (Canny không bắt
    // được edge mờ). Fallback sang threshold mask để đảm bảo recall, vẫn áp
    // edge-geometry-center để khử bias bright-spot trên contour threshold.
    if (_options.useEdgeBoundary && (int)candidates.size() < _options.expectedCount)
    {
        PinPitchOptions fb = _options;
        fb.useEdgeBoundary = false;
        cv::Mat fbMask = BuildMask(gray, fb);
        std::vector<PinCenterResult> fbCands = FindCandidates(gray, fbMask, _options);
        if (fbCands.size() > candidates.size())
        {
            candidates = std::move(fbCands);
            mask = std::move(fbMask);
        }
    }

    if ((int)candidates.size() < _options.expectedCount)
    {
        result.status = 1;
        result.message = "Not enough pin candidates found.";
        result.pins = candidates;
        SortAndAssignIds(result.pins, _options.arrangeMode);
        ComputePitch(result, _options.useProjectedPitch);
        result.debugBGR = DrawDebug(gray, mask, result);
        return result;
    }

    std::stable_sort(candidates.begin(), candidates.end(),
        [](const PinCenterResult& a, const PinCenterResult& b)
        {
            if (a.score == b.score) return a.areaPx > b.areaPx;
            return a.score > b.score;
        });

    candidates.resize(_options.expectedCount);
    SortAndAssignIds(candidates, _options.arrangeMode);

    // Runtime trial 2: refine box từ bright-core seed -> full pad boundary qua gradient.
    if (_options.useGradientRefinement)
    {
        for (auto& cand : candidates)
            RefineByGradient(gray, cand, _options);
    }

    result.pins = candidates;
    result.found = true;
    result.status = 0;
    result.message = "OK";
    ComputePitch(result, _options.useProjectedPitch);
    result.debugBGR = DrawDebug(gray, mask, result);
    return result;
}

cv::Mat PinPitchCore::BuildMask(const cv::Mat& gray, const PinPitchOptions& options)
{
    // Bug 1+3+4 (2026-05-08 runtime) fix: edge-boundary mask.
    // Threshold pixel sáng -> contour bị "kéo" về bright reflection + halo, bóng mờ pin
    // cũng pass threshold. Canny chỉ phản hồi gradient sắc -> theo biên thật của pin pad,
    // không kẹt với bright spot, và bóng mờ (gradient yếu) tự bị loại.
    if (options.useEdgeBoundary)
    {
        cv::Mat blur;
        cv::GaussianBlur(gray, blur, cv::Size(5, 5), 0.0);

        if (options.useTopHat)
        {
            int kernel = options.topHatKernelPx;
            if (kernel <= 0)
            {
                const int dim = std::min(gray.cols, gray.rows);
                kernel = std::max(81, (dim * 3) / 5);
            }
            kernel |= 1;
            cv::Mat th;
            cv::morphologyEx(
                blur, th, cv::MORPH_TOPHAT,
                cv::getStructuringElement(cv::MORPH_ELLIPSE, cv::Size(kernel, kernel)));
            blur = th;
        }

        cv::Mat edges;
        cv::Canny(blur, edges, options.edgeCannyLow, options.edgeCannyHigh);

        // Close 4 cạnh pin thành rectangle khép kín.
        int closeK = options.outlineClose > 1 ? options.outlineClose : 7;
        closeK |= 1;
        cv::morphologyEx(
            edges, edges, cv::MORPH_CLOSE,
            cv::getStructuringElement(cv::MORPH_RECT, cv::Size(closeK, closeK)));

        // Fill các vùng kín -> solid pin region.
        std::vector<std::vector<cv::Point>> cnts;
        cv::findContours(edges, cnts, cv::RETR_EXTERNAL, cv::CHAIN_APPROX_SIMPLE);
        cv::Mat filled = cv::Mat::zeros(gray.size(), CV_8UC1);
        cv::drawContours(filled, cnts, -1, cv::Scalar(255), cv::FILLED);

        if (options.outlineDilate > 1)
        {
            int d = options.reduceDilateForOutline
                        ? std::min(options.outlineDilate, 3)
                        : options.outlineDilate;
            d |= 1;
            cv::dilate(
                filled, filled,
                cv::getStructuringElement(cv::MORPH_ELLIPSE, cv::Size(d, d)));
        }

        return filled;
    }

    cv::Mat work;
    cv::GaussianBlur(gray, work, options.useOutlineCenter ? cv::Size(5, 5) : cv::Size(3, 3), 0.0);

    // Bug B+C fix: top-hat morphology để tách pin khỏi background biến thiên chậm
    // (halo, bóng đổ, phản chiếu nền). Sau top-hat, Otsu trên ảnh đã normalize cho threshold ổn định.
    if (options.useTopHat)
    {
        int kernel = options.topHatKernelPx;
        if (kernel <= 0)
        {
            // Auto: kernel phải > kích thước pin để top-hat không loại pin.
            // Pin row layout thường có pin lớn theo chiều ngắn của ảnh ROI.
            // Lấy 60% min(W,H), floor 81 để luôn lớn hơn pin tiêu chuẩn (<60px).
            const int dim = std::min(gray.cols, gray.rows);
            kernel = std::max(81, (dim * 3) / 5);
        }
        kernel |= 1;
        cv::Mat th;
        cv::morphologyEx(
            work, th, cv::MORPH_TOPHAT,
            cv::getStructuringElement(cv::MORPH_ELLIPSE, cv::Size(kernel, kernel)));
        work = th;
    }

    cv::Mat mask;
    if (options.useTopHat)
    {
        // Sau top-hat, background ~0; pin sáng hẳn -> Otsu chuẩn cho ngưỡng tự động.
        cv::threshold(work, mask, 0, 255, cv::THRESH_BINARY | cv::THRESH_OTSU);
    }
    else if (options.useOutlineCenter)
    {
        const double background = Percentile8U(work, 10.0);
        const double threshold = std::max(0.0, std::min(255.0, background + options.outlineThresholdOffset));
        cv::threshold(work, mask, threshold, 255, cv::THRESH_BINARY);
    }
    else if (options.useAutoThreshold)
    {
        cv::threshold(work, mask, 0, 255, cv::THRESH_BINARY | cv::THRESH_OTSU);
    }
    else
    {
        cv::threshold(work, mask, options.manualThreshold, 255, cv::THRESH_BINARY);
    }

    const int closeSize = options.useOutlineCenter ? options.outlineClose : options.morphClose;
    const int openSize = options.useOutlineCenter ? 0 : options.morphOpen;
    if (closeSize > 1)
    {
        int k = closeSize | 1;
        cv::Mat kernel = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(k, k));
        cv::morphologyEx(mask, mask, cv::MORPH_CLOSE, kernel);
    }

    if (openSize > 1)
    {
        int k = openSize | 1;
        cv::Mat kernel = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(k, k));
        cv::morphologyEx(mask, mask, cv::MORPH_OPEN, kernel);
    }

    if (options.useOutlineCenter && options.outlineDilate > 1)
    {
        // Bug B fix: khi reduceDilateForOutline bật, kẹp dilate <= 3 để không phình halo.
        int dilate = options.reduceDilateForOutline
                         ? std::min(options.outlineDilate, 3)
                         : options.outlineDilate;
        int k = dilate | 1;
        cv::Mat kernel = cv::getStructuringElement(cv::MORPH_ELLIPSE, cv::Size(k, k));
        cv::dilate(mask, mask, kernel);
    }

    return mask;
}

std::vector<PinCenterResult> PinPitchCore::FindCandidates(const cv::Mat& gray, const cv::Mat& mask, const PinPitchOptions& options)
{
    std::vector<std::vector<cv::Point>> contours;
    cv::findContours(mask, contours, cv::RETR_EXTERNAL, cv::CHAIN_APPROX_SIMPLE);

    std::vector<PinCenterResult> candidates;
    const double maxArea = std::max(options.minAreaPx, options.maxAreaRatio * gray.cols * gray.rows);

    for (const auto& contour : contours)
    {
        if (contour.size() < 4)
            continue;

        const double area = std::abs(cv::contourArea(contour));
        if (area < options.minAreaPx || area > maxArea)
            continue;

        PinCenterResult candidate = BuildCandidate(gray, contour, options);
        if (candidate.score <= 0.0)
            continue;

        candidates.push_back(candidate);
    }

    return candidates;
}

PinCenterResult PinPitchCore::BuildCandidate(const cv::Mat& gray, const std::vector<cv::Point>& contour, const PinPitchOptions& options)
{
    PinCenterResult out;
    out.areaPx = std::abs(cv::contourArea(contour));
    out.box = cv::minAreaRect(contour);
    out.center = out.box.center;
    out.method = PinCenterMethod::Geometry;

    const double w = std::max(1.0f, out.box.size.width);
    const double h = std::max(1.0f, out.box.size.height);
    const double aspect = std::max(w, h) / std::min(w, h);
    const double rectArea = std::max(1.0, w * h);
    out.fillRatio = out.areaPx / rectArea;

    if (aspect < options.minAspect || aspect > options.maxAspect)
    {
        out.score = 0.0;
        return out;
    }

    if (out.fillRatio < options.minFillRatio)
    {
        out.score = 0.0;
        return out;
    }

    // Bug C+D fix: solidity = contourArea / convexHullArea. Pin vuông thật ~ 0.85+;
    // halo lan / shadow merge với pin -> contour lồi lõm, solidity giảm mạnh -> reject.
    double solidity = 1.0;
    if (options.minSolidity > 0.0 && contour.size() >= 3)
    {
        std::vector<cv::Point> hull;
        cv::convexHull(contour, hull);
        const double hullArea = std::max(1.0, std::abs(cv::contourArea(hull)));
        solidity = out.areaPx / hullArea;
        if (solidity < options.minSolidity)
        {
            out.score = 0.0;
            return out;
        }
    }

    cv::Rect roi = cv::boundingRect(contour) & cv::Rect(0, 0, gray.cols, gray.rows);
    if (roi.width <= 0 || roi.height <= 0)
    {
        out.score = 0.0;
        return out;
    }

    if (contour.size() < 8)
    {
        cv::Mat componentMask = cv::Mat::zeros(roi.height, roi.width, CV_8UC1);
        std::vector<std::vector<cv::Point>> shifted(1);
        shifted[0].reserve(contour.size());
        for (const auto& p : contour)
            shifted[0].push_back(cv::Point(p.x - roi.x, p.y - roi.y));
        cv::drawContours(componentMask, shifted, 0, cv::Scalar(255), cv::FILLED);
        out.center = WeightedCentroid(gray(roi), roi, componentMask);
        out.center.x += (float)roi.x;
        out.center.y += (float)roi.y;
        out.method = PinCenterMethod::WeightedCentroidFallback;
    }

    const double aspectScore = 1.0 / aspect;
    const double fillScore = std::min(1.0, std::max(0.0, out.fillRatio));
    const double sizeScore = std::sqrt(std::max(0.0, out.areaPx));
    out.score = sizeScore * (0.65 * aspectScore + 0.35 * fillScore);
    if (out.method == PinCenterMethod::WeightedCentroidFallback)
        out.score *= 0.75;

    if (!options.useOutlineCenter)
        RefineCandidateWithOutline(gray, out, options);
    else if (options.outlinePadding > 0)
        out.box = ExpandBox(out.box, options.outlinePadding);

    // Bug 1+2 fix: thay center bằng midpoint của projection bounds trên 2 trục
    // minAreaRect -> robust với pin xéo + bright spot off-center. Áp dụng SAU khi
    // RefineCandidateWithOutline có thể đã update box, vì ta dùng axes của box hiện tại.
    if (options.useEdgeGeometryCenter && contour.size() >= 4)
    {
        out.center = ComputeEdgeGeometryCenter(contour, out.box);
        out.method = PinCenterMethod::Geometry;
    }
    return out;
}

bool PinPitchCore::RefineByGradient(const cv::Mat& gray, PinCenterResult& cand, const PinPitchOptions& options)
{
    // Runtime trial 2 fix: từ seed candidate (thường chỉ là bright core), tìm full pad
    // boundary trong patch quanh seed bằng CLAHE + Sobel magnitude (permissive hơn Canny
    // global, bắt được weak gradient ở mép pad-vs-background).
    cv::Rect bb = cand.box.boundingRect();
    const int margin = std::max(20, options.gradientPatchMargin);
    cv::Rect patch = cv::Rect(bb.x - margin, bb.y - margin,
                              bb.width + 2 * margin, bb.height + 2 * margin)
                     & cv::Rect(0, 0, gray.cols, gray.rows);
    if (patch.width < 8 || patch.height < 8)
        return false;

    cv::Mat roi = gray(patch);
    cv::Mat blur;
    cv::GaussianBlur(roi, blur, cv::Size(5, 5), 0.0);

    cv::Mat enhanced;
    const double clip = options.claheClipLimit > 0.0 ? options.claheClipLimit : 3.0;
    const int tile = options.claheTileSize > 1 ? options.claheTileSize : 8;
    cv::Ptr<cv::CLAHE> clahe = cv::createCLAHE(clip, cv::Size(tile, tile));
    clahe->apply(blur, enhanced);

    cv::Mat gx, gy, mag;
    cv::Sobel(enhanced, gx, CV_32F, 1, 0, 3);
    cv::Sobel(enhanced, gy, CV_32F, 0, 1, 3);
    cv::magnitude(gx, gy, mag);
    cv::Mat magU8;
    cv::normalize(mag, magU8, 0, 255, cv::NORM_MINMAX, CV_8U);

    cv::Mat edges;
    const int gThr = options.gradientThreshold > 0 ? options.gradientThreshold : 25;
    cv::threshold(magU8, edges, gThr, 255, cv::THRESH_BINARY);

    int closeK = options.outlineClose > 1 ? std::max(options.outlineClose, 11) : 11;
    closeK |= 1;
    cv::morphologyEx(
        edges, edges, cv::MORPH_CLOSE,
        cv::getStructuringElement(cv::MORPH_RECT, cv::Size(closeK, closeK)));

    std::vector<std::vector<cv::Point>> cnts;
    cv::findContours(edges, cnts, cv::RETR_EXTERNAL, cv::CHAIN_APPROX_SIMPLE);
    if (cnts.empty())
        return false;

    const cv::Point2f seed(cand.center.x - patch.x, cand.center.y - patch.y);
    int bestIdx = -1;
    double bestArea = 0.0;
    for (int i = 0; i < (int)cnts.size(); ++i)
    {
        if (cnts[i].size() < 4)
            continue;
        const double area = std::abs(cv::contourArea(cnts[i]));
        if (area < cand.areaPx * 0.8)
            continue;  // không nhỏ hơn seed (refinement phải mở rộng, không co lại)
        const double inside = cv::pointPolygonTest(cnts[i], seed, false);
        if (inside < 0)
            continue;  // seed phải nằm trong contour mới chấp nhận
        if (area > bestArea)
        {
            bestArea = area;
            bestIdx = i;
        }
    }
    if (bestIdx < 0)
        return false;

    // Sanity: contour mới không được lớn quá maxAreaRatio
    const double maxArea = std::max(options.minAreaPx,
                                    options.maxAreaRatio * gray.cols * gray.rows);
    if (bestArea > maxArea)
        return false;

    std::vector<cv::Point> shifted;
    shifted.reserve(cnts[bestIdx].size());
    for (const auto& p : cnts[bestIdx])
        shifted.push_back(cv::Point(p.x + patch.x, p.y + patch.y));

    // Aspect/fill check trên contour refine để không bị lan ra shadow lớn
    cv::RotatedRect rr = cv::minAreaRect(shifted);
    const double rw = std::max(1.0f, rr.size.width);
    const double rh = std::max(1.0f, rr.size.height);
    const double aspect = std::max(rw, rh) / std::min(rw, rh);
    if (aspect < options.minAspect || aspect > options.maxAspect)
        return false;
    const double rectArea = std::max(1.0, rw * rh);
    const double fill = bestArea / rectArea;
    if (fill < options.minFillRatio)
        return false;

    cand.box = rr;
    cand.areaPx = bestArea;
    cand.fillRatio = fill;
    cand.center = options.useEdgeGeometryCenter
                      ? ComputeEdgeGeometryCenter(shifted, rr)
                      : rr.center;
    return true;
}

cv::Point2f PinPitchCore::ComputeEdgeGeometryCenter(const std::vector<cv::Point>& contour, const cv::RotatedRect& box)
{
    if (contour.size() < 4 || box.size.width < 2.f || box.size.height < 2.f)
        return box.center;

    const float angRad = box.angle * (float)CV_PI / 180.0f;
    const cv::Point2f ax(std::cos(angRad), std::sin(angRad));
    const cv::Point2f ay(-std::sin(angRad), std::cos(angRad));
    const cv::Point2f c = box.center;

    float uMin = 1e9f, uMax = -1e9f;
    float vMin = 1e9f, vMax = -1e9f;
    for (const auto& p : contour)
    {
        const float rx = (float)p.x - c.x;
        const float ry = (float)p.y - c.y;
        const float u = rx * ax.x + ry * ax.y;
        const float v = rx * ay.x + ry * ay.y;
        if (u < uMin) uMin = u;
        if (u > uMax) uMax = u;
        if (v < vMin) vMin = v;
        if (v > vMax) vMax = v;
    }

    const float uC = (uMin + uMax) * 0.5f;
    const float vC = (vMin + vMax) * 0.5f;
    return cv::Point2f(c.x + uC * ax.x + vC * ay.x,
                       c.y + uC * ax.y + vC * ay.y);
}

cv::RotatedRect PinPitchCore::ExpandBox(const cv::RotatedRect& box, double padding)
{
    cv::RotatedRect expanded = box;
    expanded.size.width = (float)std::max(1.0, expanded.size.width + padding * 2.0);
    expanded.size.height = (float)std::max(1.0, expanded.size.height + padding * 2.0);
    return expanded;
}

cv::RotatedRect PinPitchCore::BuildAxisAlignedBox(const std::vector<cv::Point>& contour, double padding)
{
    cv::Rect bounds = cv::boundingRect(contour);
    bounds.x -= (int)std::round(padding);
    bounds.y -= (int)std::round(padding);
    bounds.width += (int)std::round(padding * 2.0);
    bounds.height += (int)std::round(padding * 2.0);
    return cv::RotatedRect(
        cv::Point2f(bounds.x + bounds.width * 0.5f, bounds.y + bounds.height * 0.5f),
        cv::Size2f((float)std::max(1, bounds.width), (float)std::max(1, bounds.height)),
        0.0f);
}

double PinPitchCore::Percentile8U(const cv::Mat& image, double percentile)
{
    int hist[256] = {};
    for (int y = 0; y < image.rows; ++y)
    {
        const uchar* row = image.ptr<uchar>(y);
        for (int x = 0; x < image.cols; ++x)
            hist[row[x]]++;
    }

    const int total = image.rows * image.cols;
    if (total <= 0)
        return 0.0;

    const int target = std::max(0, std::min(total - 1, (int)std::round((percentile / 100.0) * (total - 1))));
    int cumulative = 0;
    for (int i = 0; i < 256; ++i)
    {
        cumulative += hist[i];
        if (cumulative > target)
            return i;
    }
    return 255.0;
}

void PinPitchCore::RefineCandidateWithOutline(const cv::Mat& gray, PinCenterResult& candidate, const PinPitchOptions& options)
{
    if (!options.useOutlineCenter)
    {
        if (options.outlinePadding > 0)
            candidate.box = ExpandBox(candidate.box, options.outlinePadding);
        return;
    }

    const double baseW = std::max(1.0f, candidate.box.size.width);
    const double baseH = std::max(1.0f, candidate.box.size.height);
    const double radius = std::max(baseW, baseH);
    const int margin = std::max(24, options.maxOutlineExpand);
    const cv::Point2f center = candidate.box.center;
    const int x0 = std::max(0, (int)std::floor(center.x - radius * 0.5 - margin));
    const int y0 = std::max(0, (int)std::floor(center.y - radius * 0.5 - margin));
    const int x1 = std::min(gray.cols, (int)std::ceil(center.x + radius * 0.5 + margin));
    const int y1 = std::min(gray.rows, (int)std::ceil(center.y + radius * 0.5 + margin));
    if (x1 <= x0 || y1 <= y0)
    {
        candidate.box = ExpandBox(candidate.box, options.outlinePadding);
        candidate.center = candidate.box.center;
        return;
    }

    cv::Mat patch = gray(cv::Rect(x0, y0, x1 - x0, y1 - y0));
    cv::Mat work;
    cv::GaussianBlur(patch, work, cv::Size(3, 3), 0.0);

    const double background = Percentile8U(work, 20.0);
    const double threshold = std::max(0.0, std::min(255.0, background + options.outlineThresholdOffset));
    cv::Mat outlineMask;
    cv::threshold(work, outlineMask, threshold, 255, cv::THRESH_BINARY);

    if (options.outlineClose > 1)
    {
        int k = options.outlineClose | 1;
        cv::Mat kernel = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(k, k));
        cv::morphologyEx(outlineMask, outlineMask, cv::MORPH_CLOSE, kernel);
    }
    if (options.outlineDilate > 1)
    {
        int k = options.outlineDilate | 1;
        cv::Mat kernel = cv::getStructuringElement(cv::MORPH_ELLIPSE, cv::Size(k, k));
        cv::dilate(outlineMask, outlineMask, kernel);
    }

    std::vector<std::vector<cv::Point>> contours;
    cv::findContours(outlineMask, contours, cv::RETR_EXTERNAL, cv::CHAIN_APPROX_SIMPLE);

    const cv::Point2f localCenter(center.x - x0, center.y - y0);
    int bestIndex = -1;
    double bestScore = -1e18;
    for (int i = 0; i < (int)contours.size(); ++i)
    {
        const double area = std::abs(cv::contourArea(contours[i]));
        if (area < std::max(20.0, candidate.areaPx * 0.35))
            continue;

        const cv::Rect bounds = cv::boundingRect(contours[i]);
        if (bounds.width > baseW + options.maxOutlineExpand * 2.0 ||
            bounds.height > baseH + options.maxOutlineExpand * 2.0)
            continue;

        const double signedDistance = cv::pointPolygonTest(contours[i], localCenter, true);
        const double outsideDistance = signedDistance >= 0 ? 0.0 : std::abs(signedDistance);
        const double score = area - outsideDistance * 180.0;
        if (score > bestScore)
        {
            bestScore = score;
            bestIndex = i;
        }
    }

    if (bestIndex < 0)
    {
        candidate.box = ExpandBox(candidate.box, std::max(0, options.outlinePadding + 8));
        candidate.center = candidate.box.center;
        return;
    }

    std::vector<cv::Point> shifted;
    shifted.reserve(contours[bestIndex].size());
    for (const auto& p : contours[bestIndex])
        shifted.push_back(cv::Point(p.x + x0, p.y + y0));

    cv::RotatedRect outlineBox = BuildAxisAlignedBox(shifted, options.outlinePadding);

    const double outlineW = std::max(1.0f, outlineBox.size.width);
    const double outlineH = std::max(1.0f, outlineBox.size.height);
    if (outlineW > baseW + options.maxOutlineExpand * 2.0 ||
        outlineH > baseH + options.maxOutlineExpand * 2.0)
    {
        candidate.box = ExpandBox(candidate.box, std::max(0, options.outlinePadding + 8));
        candidate.center = candidate.box.center;
        return;
    }

    candidate.box = outlineBox;
    candidate.center = outlineBox.center;
}

cv::Point2f PinPitchCore::WeightedCentroid(const cv::Mat& grayRoi, const cv::Rect&, const cv::Mat& componentMask)
{
    double sumW = 0.0;
    double sumX = 0.0;
    double sumY = 0.0;

    for (int y = 0; y < grayRoi.rows; ++y)
    {
        const uchar* g = grayRoi.ptr<uchar>(y);
        const uchar* m = componentMask.ptr<uchar>(y);
        for (int x = 0; x < grayRoi.cols; ++x)
        {
            if (m[x] == 0)
                continue;

            const double w = std::min(240, (int)g[x]) + 1.0;
            sumW += w;
            sumX += x * w;
            sumY += y * w;
        }
    }

    if (sumW <= 1e-9)
        return cv::Point2f(grayRoi.cols * 0.5f, grayRoi.rows * 0.5f);

    return cv::Point2f((float)(sumX / sumW), (float)(sumY / sumW));
}

void PinPitchCore::SortAndAssignIds(std::vector<PinCenterResult>& pins, PinArrangeMode arrangeMode)
{
    if (arrangeMode == PinArrangeMode::RowProjection)
    {
        std::vector<cv::Point2f> centers;
        centers.reserve(pins.size());
        for (const auto& pin : pins)
            centers.push_back(pin.center);

        const cv::Vec4f line = VisionGeometry::FitLineOrDefault(centers);
        const cv::Point2f axis(line[0], line[1]);
        const cv::Point2f origin(line[2], line[3]);
        std::stable_sort(pins.begin(), pins.end(),
            [axis, origin](const PinCenterResult& a, const PinCenterResult& b)
            {
                const double ta = (a.center.x - origin.x) * axis.x + (a.center.y - origin.y) * axis.y;
                const double tb = (b.center.x - origin.x) * axis.x + (b.center.y - origin.y) * axis.y;
                if (std::abs(ta - tb) <= 1e-6)
                {
                    if (a.center.x == b.center.x) return a.center.y < b.center.y;
                    return a.center.x < b.center.x;
                }
                return ta < tb;
            });
    }
    else if (arrangeMode == PinArrangeMode::Y)
    {
        std::stable_sort(pins.begin(), pins.end(),
            [](const PinCenterResult& a, const PinCenterResult& b)
            {
                if (a.center.y == b.center.y) return a.center.x < b.center.x;
                return a.center.y < b.center.y;
            });
    }
    else
    {
        std::stable_sort(pins.begin(), pins.end(),
            [](const PinCenterResult& a, const PinCenterResult& b)
            {
                if (a.center.x == b.center.x) return a.center.y < b.center.y;
                return a.center.x < b.center.x;
            });
    }

    for (int i = 0; i < (int)pins.size(); ++i)
        pins[i].id = i + 1;
}

void PinPitchCore::ComputePitch(PinPitchResultCore& result, bool useProjectedPitch)
{
    std::vector<cv::Point2f> centers;
    centers.reserve(result.pins.size());
    for (const auto& pin : result.pins)
        centers.push_back(pin.center);

    result.rowLine = VisionGeometry::FitLineOrDefault(centers);

    cv::Point2f axis(result.rowLine[0], result.rowLine[1]);
    result.adjacentPitchMm.clear();
    for (int i = 0; i + 1 < (int)centers.size(); ++i)
    {
        double dPx = useProjectedPitch
            ? VisionGeometry::ProjectedDistance(centers[i], centers[i + 1], axis)
            : VisionGeometry::PointDistance(centers[i], centers[i + 1]);
        result.adjacentPitchMm.push_back(dPx * result.scaleMmPerPx);
    }

    if (centers.size() >= 2)
    {
        double spanPx = useProjectedPitch
            ? VisionGeometry::ProjectedDistance(centers.front(), centers.back(), axis)
            : VisionGeometry::PointDistance(centers.front(), centers.back());
        result.spanP1P4Mm = spanPx * result.scaleMmPerPx;
    }

    double residual = 0.0;
    for (const auto& p : centers)
        residual += VisionGeometry::DistancePointToLine(p, result.rowLine);
    result.rowResidualPx = centers.empty() ? 0.0 : residual / centers.size();
}

cv::Mat PinPitchCore::DrawDebug(const cv::Mat& gray, const cv::Mat& mask, const PinPitchResultCore& result)
{
    cv::Mat dbg;
    cv::cvtColor(gray, dbg, cv::COLOR_GRAY2BGR);

    cv::Mat maskColor;
    cv::cvtColor(mask, maskColor, cv::COLOR_GRAY2BGR);
    cv::addWeighted(dbg, 0.80, maskColor, 0.20, 0.0, dbg);

    for (const auto& pin : result.pins)
    {
        cv::Point2f pts[4];
        pin.box.points(pts);
        const cv::Scalar color = pin.method == PinCenterMethod::Geometry
            ? cv::Scalar(0, 255, 0)
            : cv::Scalar(0, 180, 255);

        for (int i = 0; i < 4; ++i)
            cv::line(dbg, pts[i], pts[(i + 1) % 4], color, 1, cv::LINE_AA);

        cv::drawMarker(dbg, pin.center, cv::Scalar(0, 0, 255), cv::MARKER_CROSS, 14, 2, cv::LINE_AA);
        cv::putText(dbg, "P" + std::to_string(pin.id),
            cv::Point((int)std::round(pin.center.x) + 5, (int)std::round(pin.center.y) - 5),
            cv::FONT_HERSHEY_SIMPLEX, 0.45, cv::Scalar(255, 255, 255), 1, cv::LINE_AA);
    }

    if (result.pins.size() >= 2)
    {
        for (int i = 0; i + 1 < (int)result.pins.size(); ++i)
            cv::line(dbg, result.pins[i].center, result.pins[i + 1].center, cv::Scalar(255, 0, 255), 1, cv::LINE_AA);
    }

    return dbg;
}

} // namespace BeeCpp
