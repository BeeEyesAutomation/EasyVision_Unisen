#include "Global.h"
#include <cmath> // đảm bảo đã include

using namespace cv;
using namespace BeeCpp;
using namespace std;
/* ================= Raw buffer utils ================= */
int CommonPlus::CvTypeFromChannels(int ch)
{
    switch (ch)
    {
    case 1: return CV_8UC1;
    case 3: return CV_8UC3;
    case 4: return CV_8UC4;
    default: throw gcnew System::ArgumentOutOfRangeException("ch", "Only 1/3/4 channels supported");
    }
}
size_t CommonPlus::SafeStep(int w, int ch, int stride)
{
    if (stride > 0) return static_cast<size_t>(stride);
    return static_cast<size_t>(w) * static_cast<size_t>(ch); // 8-bit/channel giả định
}

/* ================= API cho C# ================= */
IntPtr CommonPlus::CropRotatedRect(
    IntPtr srcMatCvPtr, RectRotateCli rr, Nullable<RectRotateCli> rrMask, bool returnMaskOnly)
{
    try
    {
        auto* src = reinterpret_cast<cv::Mat*>(srcMatCvPtr.ToPointer());
        if (!src || src->empty()) return IntPtr::Zero;

        cv::Mat result;
        if (rrMask.HasValue) { RectRotateCli m = rrMask.Value; RunCrop(*src, rr, &m, returnMaskOnly, result); }
        else { RunCrop(*src, rr, nullptr, returnMaskOnly, result); }

        return IntPtr(new cv::Mat(result)); // caller FreeMat
    }
    catch (const std::exception& e)
    {
        throw gcnew System::InvalidOperationException(gcnew System::String(e.what()));
    }
}

void CommonPlus::CropTo(
    IntPtr srcMatCvPtr, RectRotateCli rr, Nullable<RectRotateCli> rrMask, bool returnMaskOnly, IntPtr dstMatCvPtr)
{
    try
    {
        auto* src = reinterpret_cast<cv::Mat*>(srcMatCvPtr.ToPointer());
        auto* dst = reinterpret_cast<cv::Mat*>(dstMatCvPtr.ToPointer());
        if (!src || src->empty() || !dst) return;

        cv::Mat result;
        if (rrMask.HasValue) { RectRotateCli m = rrMask.Value; RunCrop(*src, rr, &m, returnMaskOnly, result); }
        else { RunCrop(*src, rr, nullptr, returnMaskOnly, result); }

        *dst = result;
    }
    catch (const std::exception& e)
    {
        throw gcnew System::InvalidOperationException(gcnew System::String(e.what()));
    }
}
CropPlus::CropPlus() {
   com = new CommonPlus();
}
void CropPlus::FreeBuffer(System::IntPtr p)
{
    if (p != System::IntPtr::Zero)
        System::Runtime::InteropServices::Marshal::FreeHGlobal(p);
}

CropPlus::~CropPlus() { this->!CropPlus(); }
CropPlus::!CropPlus() { if (com) { delete com; com = nullptr; } }
IntPtr CropPlus::CropRotatedInt(
    IntPtr data, int w, int h, int stride, int ch,
    RectRotateCli rr, Nullable<RectRotateCli> rrMask, 
     int% outW,
     int% outH,
     int% outStride,
     int% outChannels)
{
    if (data == IntPtr::Zero || w <= 0 || h <= 0) return IntPtr::Zero;
    try
    {
        const int type =com-> CvTypeFromChannels(ch);
        const size_t step = com->SafeStep(w, ch, stride);
        cv::Mat src(h, w, type, data.ToPointer(), step); // header non-owning

        cv::Mat result;
        if (rrMask.HasValue) { RectRotateCli m = rrMask.Value; com->RunCrop(src, rr, &m, false, result); }
        else { com->RunCrop(src, rr, nullptr, false, result); }
      
        outW = result.cols;
        outH = result.rows;
        outChannels  = result.channels();    // =1
        outStride = static_cast<int>(result.step);

        return IntPtr(new cv::Mat(result));
    }
    catch (const std::exception& e)
    {
        throw gcnew System::InvalidOperationException(gcnew System::String(e.what()));
    }
}

void CommonPlus::CropRotToMat(
    IntPtr data, int w, int h, int stride, int ch,
    RectRotateCli rr, Nullable<RectRotateCli> rrMask, bool returnMaskOnly, IntPtr dstMatCvPtr)
{
    if (data == IntPtr::Zero || w <= 0 || h <= 0 || dstMatCvPtr == IntPtr::Zero) return;
    try
    {
        const int type = CvTypeFromChannels(ch);
        const size_t step = SafeStep(w, ch, stride);
        cv::Mat src(h, w, type, data.ToPointer(), step);

        cv::Mat result;
        if (rrMask.HasValue) { RectRotateCli m = rrMask.Value; RunCrop(src, rr, &m, returnMaskOnly, result); }
        else { RunCrop(src, rr, nullptr, returnMaskOnly, result); }

        auto* dst = reinterpret_cast<cv::Mat*>(dstMatCvPtr.ToPointer());
        *dst = result;
    }
    catch (const std::exception& e)
    {
        throw gcnew System::InvalidOperationException(gcnew System::String(e.what()));
    }
}

void CommonPlus::FreeMat(IntPtr p)
{
    auto* m = reinterpret_cast<cv::Mat*>(p.ToPointer());
    delete m;
}

/* ================= Helpers lõi ================= */

cv::Mat CommonPlus::RotateMat(const cv::Mat& raw, const cv::RotatedRect& rot)
{
    cv::Mat matRs, matR = cv::getRotationMatrix2D(rot.center, rot.angle, 1);
    float tx = (rot.size.width - 1) / 2.0f - rot.center.x;
    float ty = (rot.size.height - 1) / 2.0f - rot.center.y;
    matR.at<double>(0, 2) += tx;
    matR.at<double>(1, 2) += ty;
    cv::warpAffine(raw, matRs, matR, rot.size, cv::INTER_LINEAR, cv::BORDER_CONSTANT);
    return matRs;
}

cv::Point2f CommonPlus::RotatePoint(const cv::Point2f& p, float degree)
{
    const double rad = degree * CV_PI / 180.0;
    const double c = std::cos(rad), s = std::sin(rad);
    return cv::Point2f(
        (float)(p.x * c - p.y * s),
        (float)(p.x * s + p.y * c)
    );
}

void CommonPlus::GetPolygonBounds(const std::vector<cv::Point2f>& pts,
    float& minX, float& minY, float& maxX, float& maxY)
{
    minX = std::numeric_limits<float>::max();
    minY = std::numeric_limits<float>::max();
    maxX = -std::numeric_limits<float>::max();
    maxY = -std::numeric_limits<float>::max();

    if (pts.empty()) { minX = minY = 0.f; maxX = maxY = 1.f; return; }

    size_t N = pts.size();
    size_t M = (N >= 2 && pts.front() == pts.back()) ? N - 1 : N;

    for (size_t i = 0; i < M; ++i)
    {
        const auto& p = pts[i];
        minX = std::fmin(minX, p.x);
        minY = std::fmin(minY, p.y);
        maxX =std:: fmax(maxX, p.x);
        maxY = std::fmax(maxY, p.y);
    }
}

std::vector<cv::Point2f> CommonPlus::BuildHexLocalVertices(
    float w, float h, cli::array<PointF32>^ hexOffsets)
{
    const float halfW = w * 0.5f;
    const float halfH = h * 0.5f;
    const float SQ3_2 = 0.8660254037844386f;

    cv::Point2f base[6] = {
        { +halfW,         0.0f          },
        { +halfW * 0.5f,   +SQ3_2 * halfH   },
        { -halfW * 0.5f,   +SQ3_2 * halfH   },
        { -halfW,         0.0f          },
        { -halfW * 0.5f,   -SQ3_2 * halfH   },
        { +halfW * 0.5f,   -SQ3_2 * halfH   }
    };

    std::vector<cv::Point2f> v(6);
    for (int i = 0; i < 6; ++i) v[i] = base[i];

    if (hexOffsets != nullptr && hexOffsets->Length == 6) {
        for (int i = 0; i < 6; ++i) {
            PointF32 off = hexOffsets[i];
            v[i].x += off.X;
            v[i].y += off.Y;
        }
    }
    return v;
}

void CommonPlus::GetAnchorSizeFor(
    const RectRotateCli% rr,
    cv::Point2f& worldAnchor,
    cv::Size2f& size,
    cv::Point2f& localCenterForShape)
{
    if (rr.Shape == ShapeType::Polygon)
    {
        const bool hasPoly = (rr.PolyLocalPoints != nullptr && rr.PolyLocalPoints->Length >= 3);
        if (hasPoly) {
            auto loc = ToVec(rr.PolyLocalPoints);
            if (loc.size() >= 2 && loc.front() == loc.back()) loc.pop_back();

            float minX, minY, maxX, maxY;
            GetPolygonBounds(loc, minX, minY, maxX, maxY);

            const float w = std::fmax(2.0f, maxX - minX);
            const float h = std::fmax(2.0f, maxY - minY);
            const float cx = 0.5f * (minX + maxX);
            const float cy = 0.5f * (minY + maxY);

            cv::Point2f delta(cx, cy);
            delta = RotatePoint(delta, (float)rr.RectRotationDeg);
            worldAnchor = { rr.PosCenter.X + delta.x, rr.PosCenter.Y + delta.y };

            size = { w, h };
            localCenterForShape = { cx, cy };
            return;
        }
        // Nếu Polygon rỗng → rơi xuống fallback RectWH
    }
    else if (rr.Shape == ShapeType::Hexagon)
    {
        // Luôn lấy bbox từ hex “thật” (RectWH + offsets)
        const float w = (float)rr.RectWH.Width;
        const float h = (float)rr.RectWH.Height;
        auto hexLocal = BuildHexLocalVertices(w, h, rr.HexVertexOffsets);

        float minX, minY, maxX, maxY;
        GetPolygonBounds(hexLocal, minX, minY, maxX, maxY);

        const float W = std::fmax(2.0f, maxX - minX);
        const float H = std::fmax(2.0f, maxY - minY);
        const float cx = 0.5f * (minX + maxX);
        const float cy = 0.5f * (minY + maxY);

        cv::Point2f delta(cx, cy);
        delta = RotatePoint(delta, (float)rr.RectRotationDeg);
        worldAnchor = { rr.PosCenter.X + delta.x, rr.PosCenter.Y + delta.y };

        size = { W, H };
        localCenterForShape = { cx, cy };
        return;
    }

    // Fallback: Rectangle, Ellipse, hoặc Polygon rỗng
    worldAnchor = { rr.PosCenter.X, rr.PosCenter.Y };
    size ={ 
      std::fmax(2.0f, (float)rr.RectWH.Width),
             std::fmax(2.0f, (float)rr.RectWH.Height) };
    localCenterForShape = { 0.f, 0.f };
}

std::vector<cv::Point> CommonPlus::AxisAlignedRectCorners(const cv::Point2f& c, int W, int H)
{
    const float hx = 0.5f * W, hy = 0.5f * H;
    std::vector<cv::Point> pts(4);
    pts[0] = cv::Point((int)std::round(c.x - hx), (int)std::round(c.y - hy));
    pts[1] = cv::Point((int)std::round(c.x + hx), (int)std::round(c.y - hy));
    pts[2] = cv::Point((int)std::round(c.x + hx), (int)std::round(c.y + hy));
    pts[3] = cv::Point((int)std::round(c.x - hx), (int)std::round(c.y + hy));
    return pts;
}

std::vector<cv::Point> CommonPlus::RegularHexFallback(
    const cv::Point2f& centerInMask, float angleInPatch, int W, int H)
{
    std::vector<cv::Point> pts;
    pts.reserve(6);
    for (int i = 0; i < 6; ++i)
    {
        double theta = (CV_PI / 3.0) * i;
        cv::Point2f p(
            (float)((W * 0.5) * std::cos(theta)),
            (float)((H * 0.5) * std::sin(theta))
        );
        if (std::abs(angleInPatch) > 1e-6f)
            p = RotatePoint(p, angleInPatch);
        p += centerInMask;
        pts.emplace_back((int)std::round(p.x), (int)std::round(p.y));
    }
    return pts;
}

std::vector<cv::Point> CommonPlus::PolyFromLocalPoints(
    const std::vector<cv::Point2f>& localPts,
    const cv::Point2f& centerInMask,
    float angleInPatch,
    const cv::Point2f& localCenter)
{
    std::vector<cv::Point> out;
    if (localPts.size() < 3) return out;

    size_t N = localPts.size();
    size_t M = (N >= 2 && localPts.front() == localPts.back()) ? N - 1 : N;
    out.reserve(M);

    for (size_t i = 0; i < M; ++i)
    {
        cv::Point2f p = localPts[i];
        p -= localCenter;
        if (std::abs(angleInPatch) > 1e-6f)
            p = RotatePoint(p, angleInPatch);
        p += centerInMask;
        if (!isFinite(p)) continue;
        out.emplace_back((int)std::round(p.x), (int)std::round(p.y));
    }

    if (out.size() >= 2) {
        std::vector<cv::Point> cleaned;
        cleaned.reserve(out.size());
        cleaned.push_back(out[0]);
        for (size_t i = 1; i < out.size(); ++i) {
            if (out[i].x != cleaned.back().x || out[i].y != cleaned.back().y)
                cleaned.push_back(out[i]);
        }
        out.swap(cleaned);
    }

    if (out.size() < 3) out.clear();
    return out;
}

void CommonPlus::DrawShapeMaskIntoWithSize(
    const RectRotateCli% rr,
    cv::Mat& mask,
    const cv::Point2f& centerInMask,
    float angleInPatch,
    uchar fillValue,
    int W, int H,
    const cv::Point2f& localCenterForShape)
{
    if (W <= 1 || H <= 1) return;

    switch (rr.Shape)
    {
    case ShapeType::Hexagon:
    {
        auto hexLocal = BuildHexLocalVertices((float)rr.RectWH.Width, (float)rr.RectWH.Height,
            rr.HexVertexOffsets);
        std::vector<cv::Point> poly;
        poly.reserve(6);
        for (auto p : hexLocal) {
            p -= localCenterForShape;                   // đưa về giữa patch
            if (std::abs(angleInPatch) > 1e-6f) p = RotatePoint(p, angleInPatch);
            p += centerInMask;
            if (!isFinite(p)) continue;
            poly.emplace_back((int)std::round(p.x), (int)std::round(p.y));
        }
        if (poly.size() >= 3) {
            const std::vector<std::vector<cv::Point>> pp{ poly };
            cv::fillPoly(mask, pp, cv::Scalar(fillValue));
        }
        break;
    }

    case ShapeType::Polygon:
    {
        std::vector<cv::Point> poly;
        if (rr.PolyLocalPoints != nullptr && rr.PolyLocalPoints->Length >= 3)
        {
            auto loc = ToVec(rr.PolyLocalPoints);
            poly = PolyFromLocalPoints(loc, centerInMask, angleInPatch, localCenterForShape);
        }
        if (poly.size() >= 3) {
            const std::vector<std::vector<cv::Point>> pp{ poly };
            cv::fillPoly(mask, pp, cv::Scalar(fillValue));
        }
        break;
    }

    case ShapeType::Ellipse:
    {
        cv::RotatedRect r(centerInMask, cv::Size2f((float)W, (float)H), angleInPatch);
        cv::ellipse(mask, r, cv::Scalar(fillValue), cv::FILLED);
        break;
    }

    case ShapeType::Rectangle:
    default:
    {
        int tlx = (int)std::round(centerInMask.x - W * 0.5f);
        int tly = (int)std::round(centerInMask.y - H * 0.5f);
        cv::rectangle(mask, cv::Rect(tlx, tly, W, H), cv::Scalar(fillValue), cv::FILLED);
        break;
    }
    }
}

void CommonPlus::RunCrop(
    const cv::Mat& src,
    const RectRotateCli% rr,
    const RectRotateCli* rrMask,
    bool returnMaskOnly,
    cv::Mat& out)
{
    // 1) Anchor & size theo shape
    cv::Point2f worldAnchor, localCenter;
    cv::Size2f  rectSize;
    GetAnchorSizeFor(rr, worldAnchor, rectSize, localCenter);

    double angleUsed = rr.RectRotationDeg;
    if (angleUsed < -45.0) {
        angleUsed += 90.0;
        float tmp = rectSize.width; rectSize.width = rectSize.height; rectSize.height = tmp;
    }
    float angleInPatchCrop = (float)(rr.RectRotationDeg - angleUsed);
    if (angleInPatchCrop > 180.f)  angleInPatchCrop -= 360.f;
    if (angleInPatchCrop < -180.f) angleInPatchCrop += 360.f;

    const cv::Point2f anchor(worldAnchor.x, worldAnchor.y);

    // 2) Warp quanh anchor
    cv::Mat M = cv::getRotationMatrix2D(anchor, angleUsed, 1.0);
    cv::Mat warped;
    cv::warpAffine(src, warped, M, src.size(),
        cv::INTER_CUBIC, cv::BORDER_CONSTANT, cv::Scalar(0, 0, 0));

    // 3) Cắt patch
    cv::Mat patch;
    cv::getRectSubPix(warped,
        cv::Size((int)std::round(rectSize.width), (int)std::round(rectSize.height)),
        anchor, patch);
    if (patch.empty()) { out.release(); return; }

    const int patchH = patch.rows;
    const int patchW = patch.cols;
    const cv::Point2f patchCenter((float)patchW * 0.5f, (float)patchH * 0.5f);

    // 4) Mask theo shape
    cv::Mat cropMask(patchH, patchW, CV_8UC1, cv::Scalar(0));
    DrawShapeMaskIntoWithSize(rr, cropMask, patchCenter, angleInPatchCrop, 255,
        (int)std::round(rectSize.width),
        (int)std::round(rectSize.height),
        localCenter);

    // 5) Mask loại trừ (nếu có)
    cv::Mat finalMask;
    if (rrMask)
    {
        cv::Mat mask2(patchH, patchW, CV_8UC1, cv::Scalar(255));

        cv::Point2f worldAnchorMask, maskLocalCenter;
        cv::Size2f  maskSize;
        GetAnchorSizeFor(*rrMask, worldAnchorMask, maskSize, maskLocalCenter);

        cv::Point2f deltaWorld(worldAnchorMask.x - worldAnchor.x, worldAnchorMask.y - worldAnchor.y);
        cv::Point2f deltaInPatch = RotatePoint(deltaWorld, (float)(-angleUsed));
        cv::Point2f maskCenterInPatch(patchCenter.x + deltaInPatch.x, patchCenter.y + deltaInPatch.y);
        float maskAngleInPatch = (float)(rrMask->RectRotationDeg - angleUsed);

        DrawShapeMaskIntoWithSize(*rrMask, mask2, maskCenterInPatch, maskAngleInPatch, 0,
            (int)std::round(maskSize.width),
            (int)std::round(maskSize.height),
            maskLocalCenter);

        cv::bitwise_and(cropMask, mask2, finalMask);
    }
    else {
        finalMask = cropMask.clone();
    }

    if (returnMaskOnly) {
        out = finalMask.clone();
        return;
    }

    // 6) Áp mask lên nền
    cv::Scalar bg = rr.IsWhite ? cv::Scalar(255, 255, 255, 255) : cv::Scalar(0, 0, 0, 0);
    cv::Mat bgMat(patch.size(), patch.type(), bg);
    out = bgMat.clone();
    patch.copyTo(out, finalMask);
}
