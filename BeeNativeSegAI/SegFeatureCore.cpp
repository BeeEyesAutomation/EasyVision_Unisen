#include "pch.h"

#include "SegFeatureCore.h"

#include <opencv2/imgproc.hpp>

#include <algorithm>
#include <cmath>
#include <stdexcept>

namespace BeeSegAI {
namespace {

int MakeOddAtLeast(int value, int minimum)
{
    value = std::max(value, minimum);
    return (value % 2 == 0) ? value + 1 : value;
}

cv::Rect EffectiveRoi(const cv::Rect& roi, const cv::Size& size)
{
    const cv::Rect imageRect(0, 0, size.width, size.height);
    if (roi.width <= 0 || roi.height <= 0) {
        return imageRect;
    }

    const cv::Rect clipped = roi & imageRect;
    return clipped.area() > 0 ? clipped : imageRect;
}

int CountCircularTransitions(int code)
{
    int transitions = 0;
    int previous = (code >> 7) & 1;
    for (int i = 0; i < 8; ++i) {
        const int current = (code >> i) & 1;
        if (current != previous) {
            ++transitions;
        }
        previous = current;
    }
    return transitions;
}

cv::Mat BuildUniformLbpLookup()
{
    cv::Mat lookup(1, 256, CV_8U);
    int uniformIndex = 0;
    for (int code = 0; code < 256; ++code) {
        if (CountCircularTransitions(code) <= 2) {
            lookup.at<unsigned char>(0, code) = static_cast<unsigned char>(uniformIndex++);
        } else {
            lookup.at<unsigned char>(0, code) = 59;
        }
    }
    return lookup;
}

void Clamp01(cv::Mat& mat)
{
    cv::max(mat, 0.0f, mat);
    cv::min(mat, 1.0f, mat);
}

void Clamp01(cv::UMat& mat)
{
    cv::max(mat, 0.0f, mat);
    cv::min(mat, 1.0f, mat);
}

void BoxMeanStd(const cv::Mat& srcU8, int window, cv::Mat& mean, cv::Mat& variance)
{
    cv::Mat src32;
    srcU8.convertTo(src32, CV_32F);

    cv::boxFilter(src32, mean, CV_32F, cv::Size(window, window), cv::Point(-1, -1), true, cv::BORDER_REPLICATE);

    cv::Mat squared;
    cv::multiply(src32, src32, squared);

    cv::Mat meanSquared;
    cv::boxFilter(squared, meanSquared, CV_32F, cv::Size(window, window), cv::Point(-1, -1), true, cv::BORDER_REPLICATE);

    cv::Mat meanTimesMean;
    cv::multiply(mean, mean, meanTimesMean);
    variance = meanSquared - meanTimesMean;
    cv::max(variance, 0.0f, variance);
}

void BoxMeanStd(const cv::UMat& srcU8, int window, cv::UMat& mean, cv::UMat& variance)
{
    cv::UMat src32;
    srcU8.convertTo(src32, CV_32F);

    cv::boxFilter(src32, mean, CV_32F, cv::Size(window, window), cv::Point(-1, -1), true, cv::BORDER_REPLICATE);

    cv::UMat squared;
    cv::multiply(src32, src32, squared);

    cv::UMat meanSquared;
    cv::boxFilter(squared, meanSquared, CV_32F, cv::Size(window, window), cv::Point(-1, -1), true, cv::BORDER_REPLICATE);

    cv::UMat meanTimesMean;
    cv::multiply(mean, mean, meanTimesMean);
    cv::subtract(meanSquared, meanTimesMean, variance);
    cv::max(variance, 0.0f, variance);
}

void ComputeLBP(const cv::Mat& gray, const cv::Mat& lookup, cv::Mat& outPlane)
{
    outPlane = cv::Mat::zeros(gray.size(), CV_32F);
    if (gray.rows < 3 || gray.cols < 3) {
        return;
    }

    for (int y = 1; y < gray.rows - 1; ++y) {
        const unsigned char* prev = gray.ptr<unsigned char>(y - 1);
        const unsigned char* cur = gray.ptr<unsigned char>(y);
        const unsigned char* next = gray.ptr<unsigned char>(y + 1);
        float* dst = outPlane.ptr<float>(y);

        for (int x = 1; x < gray.cols - 1; ++x) {
            const unsigned char center = cur[x];
            int code = 0;
            code |= (prev[x - 1] >= center) << 0;
            code |= (prev[x] >= center) << 1;
            code |= (prev[x + 1] >= center) << 2;
            code |= (cur[x + 1] >= center) << 3;
            code |= (next[x + 1] >= center) << 4;
            code |= (next[x] >= center) << 5;
            code |= (next[x - 1] >= center) << 6;
            code |= (cur[x - 1] >= center) << 7;
            dst[x] = static_cast<float>(lookup.at<unsigned char>(0, code)) / 59.0f;
        }
    }
}

void ComputeLBPGpuCompatible(const cv::UMat& grayU8, const cv::Mat& lookup, cv::UMat& outPlane)
{
    cv::Mat gray = grayU8.getMat(cv::ACCESS_READ);
    cv::Mat cpuPlane;
    ComputeLBP(gray, lookup, cpuPlane);
    cpuPlane.copyTo(outPlane);
}

void FillPositionPlanes(cv::Mat& xPlane, cv::Mat& yPlane, const cv::Rect& roi, const cv::Size& size)
{
    xPlane.create(size, CV_32F);
    yPlane.create(size, CV_32F);

    const cv::Rect r = EffectiveRoi(roi, size);
    const float invWidth = r.width > 1 ? 1.0f / static_cast<float>(r.width - 1) : 0.0f;
    const float invHeight = r.height > 1 ? 1.0f / static_cast<float>(r.height - 1) : 0.0f;

    for (int y = 0; y < size.height; ++y) {
        float* xDst = xPlane.ptr<float>(y);
        float* yDst = yPlane.ptr<float>(y);
        const float yn = std::clamp((static_cast<float>(y - r.y)) * invHeight, 0.0f, 1.0f);

        for (int x = 0; x < size.width; ++x) {
            xDst[x] = std::clamp((static_cast<float>(x - r.x)) * invWidth, 0.0f, 1.0f);
            yDst[x] = yn;
        }
    }
}

void FillPositionPlanes(cv::UMat& xPlane, cv::UMat& yPlane, const cv::Rect& roi, const cv::Size& size)
{
    cv::Mat xCpu;
    cv::Mat yCpu;
    FillPositionPlanes(xCpu, yCpu, roi, size);
    xCpu.copyTo(xPlane);
    yCpu.copyTo(yPlane);
}

void FillDistanceToRoi(cv::Mat& outPlane, const cv::Rect& roi, const cv::Size& size)
{
    outPlane = cv::Mat::zeros(size, CV_32F);

    const cv::Rect r = EffectiveRoi(roi, size);
    const float denom = std::max(1.0f, static_cast<float>(std::max(r.width, r.height)) * 0.5f);

    for (int y = r.y; y < r.y + r.height; ++y) {
        float* dst = outPlane.ptr<float>(y);
        const int top = y - r.y;
        const int bottom = r.y + r.height - 1 - y;

        for (int x = r.x; x < r.x + r.width; ++x) {
            const int left = x - r.x;
            const int right = r.x + r.width - 1 - x;
            const int edgeDistance = std::min(std::min(left, right), std::min(top, bottom));
            dst[x] = std::clamp(static_cast<float>(edgeDistance) / denom, 0.0f, 1.0f);
        }
    }
}

void FillDistanceToRoi(cv::UMat& outPlane, const cv::Rect& roi, const cv::Size& size)
{
    cv::Mat cpuPlane;
    FillDistanceToRoi(cpuPlane, roi, size);
    cpuPlane.copyTo(outPlane);
}

void EnsurePlaneSet(const std::vector<cv::Mat>& planes)
{
    if (planes.size() != SegFeatureExtractor::kNumFeatures) {
        throw std::invalid_argument("SegmentAI feature plane count must be 24.");
    }

    if (planes.empty() || planes[0].empty()) {
        throw std::invalid_argument("SegmentAI feature planes are empty.");
    }

    const cv::Size size = planes[0].size();
    for (const cv::Mat& plane : planes) {
        if (plane.empty() || plane.type() != CV_32F || plane.size() != size) {
            throw std::invalid_argument("SegmentAI feature planes must be CV_32F with identical size.");
        }
    }
}

} // namespace

SegFeatureExtractor::SegFeatureExtractor()
{
    Configure(FeatureConfig());
}

void SegFeatureExtractor::Configure(const FeatureConfig& cfg)
{
    cfg_ = cfg;
    cfg_.lbpRadius = 1;
    cfg_.hsvWindow = MakeOddAtLeast(cfg_.hsvWindow, 3);
    cfg_.gaborSize = MakeOddAtLeast(cfg_.gaborSize, 3);
    cfg_.gaborSigma = std::max(cfg_.gaborSigma, 0.1f);
    cfg_.gaborLambda = std::max(cfg_.gaborLambda, 0.1f);

    const double angles[] = {0.0, CV_PI / 4.0, CV_PI / 2.0, 3.0 * CV_PI / 4.0};
    for (int i = 0; i < 4; ++i) {
        gaborKernels_[i] = cv::getGaborKernel(
            cv::Size(cfg_.gaborSize, cfg_.gaborSize),
            cfg_.gaborSigma,
            angles[i],
            cfg_.gaborLambda,
            0.5,
            0.0,
            CV_32F);
    }

    lbpLookup_ = BuildUniformLbpLookup();
}

void SegFeatureExtractor::ExtractCPU(const cv::Mat& srcBgr, std::vector<cv::Mat>& outPlanes) const
{
    if (srcBgr.empty()) {
        throw std::invalid_argument("SegmentAI source image is empty.");
    }
    if (srcBgr.type() != CV_8UC3) {
        throw std::invalid_argument("SegmentAI ExtractCPU expects CV_8UC3 BGR input.");
    }

    cv::Mat gray;
    cv::Mat hsv;
    cv::cvtColor(srcBgr, gray, cv::COLOR_BGR2GRAY);
    cv::cvtColor(srcBgr, hsv, cv::COLOR_BGR2HSV);

    outPlanes.assign(kNumFeatures, cv::Mat());

    ComputeLBP(gray, lbpLookup_, outPlanes[0]);

    std::vector<cv::Mat> hsvCh;
    cv::split(hsv, hsvCh);
    for (int c = 0; c < 3; ++c) {
        cv::Mat mean;
        cv::Mat variance;
        BoxMeanStd(hsvCh[c], cfg_.hsvWindow, mean, variance);

        const double scale = (c == 0) ? 1.0 / 179.0 : 1.0 / 255.0;
        mean.convertTo(outPlanes[1 + c], CV_32F, scale);
        Clamp01(outPlanes[1 + c]);

        cv::sqrt(variance, variance);
        variance.convertTo(outPlanes[4 + c], CV_32F, scale);
        Clamp01(outPlanes[4 + c]);
    }

    for (int i = 0; i < 4; ++i) {
        cv::Mat filtered;
        cv::filter2D(gray, filtered, CV_32F, gaborKernels_[i], cv::Point(-1, -1), 0.0, cv::BORDER_REPLICATE);
        cv::absdiff(filtered, cv::Scalar::all(0), filtered);
        filtered.convertTo(outPlanes[7 + i], CV_32F, 1.0 / 255.0);
        Clamp01(outPlanes[7 + i]);
    }

    cv::Mat sx;
    cv::Mat sy;
    cv::Sobel(gray, sx, CV_32F, 1, 0, 3, 1.0, 0.0, cv::BORDER_REPLICATE);
    cv::Sobel(gray, sy, CV_32F, 0, 1, 3, 1.0, 0.0, cv::BORDER_REPLICATE);

    cv::Mat magnitude;
    cv::magnitude(sx, sy, magnitude);
    magnitude.convertTo(outPlanes[11], CV_32F, 1.0 / 255.0);
    Clamp01(outPlanes[11]);

    cv::Mat angle;
    cv::phase(sx, sy, angle, false);
    angle.convertTo(outPlanes[12], CV_32F, 1.0 / (2.0 * CV_PI));
    Clamp01(outPlanes[12]);

    cv::Mat laplacian;
    cv::Laplacian(gray, laplacian, CV_32F, 3, 1.0, 0.0, cv::BORDER_REPLICATE);
    cv::absdiff(laplacian, cv::Scalar::all(0), laplacian);
    laplacian.convertTo(outPlanes[13], CV_32F, 1.0 / 255.0);
    Clamp01(outPlanes[13]);

    FillPositionPlanes(outPlanes[14], outPlanes[15], cfg_.roi, gray.size());

    const int blurSizes[] = {3, 7, 15};
    for (int i = 0; i < 3; ++i) {
        cv::Mat blurred;
        cv::boxFilter(gray, blurred, CV_32F, cv::Size(blurSizes[i], blurSizes[i]), cv::Point(-1, -1), true, cv::BORDER_REPLICATE);
        blurred.convertTo(outPlanes[16 + i], CV_32F, 1.0 / 255.0);
        Clamp01(outPlanes[16 + i]);
    }

    cv::Mat edges;
    cv::Canny(gray, edges, 50, 150);
    edges.convertTo(edges, CV_32F, 1.0 / 255.0);

    const int edgeSizes[] = {5, 11, 21};
    for (int i = 0; i < 3; ++i) {
        cv::boxFilter(edges, outPlanes[19 + i], CV_32F, cv::Size(edgeSizes[i], edgeSizes[i]), cv::Point(-1, -1), true, cv::BORDER_REPLICATE);
        Clamp01(outPlanes[19 + i]);
    }

    cv::Mat dilated;
    cv::Mat eroded;
    const cv::Mat contrastKernel = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(5, 5));
    cv::dilate(hsvCh[2], dilated, contrastKernel, cv::Point(-1, -1), 1, cv::BORDER_REPLICATE);
    cv::erode(hsvCh[2], eroded, contrastKernel, cv::Point(-1, -1), 1, cv::BORDER_REPLICATE);

    cv::Mat contrast;
    cv::subtract(dilated, eroded, contrast);
    contrast.convertTo(outPlanes[22], CV_32F, 1.0 / 255.0);
    Clamp01(outPlanes[22]);

    FillDistanceToRoi(outPlanes[23], cfg_.roi, gray.size());
}

void SegFeatureExtractor::ExtractGpu(const cv::UMat& srcBgr, std::vector<cv::UMat>& outPlanes) const
{
    if (srcBgr.empty()) {
        throw std::invalid_argument("SegmentAI source image is empty.");
    }
    if (srcBgr.type() != CV_8UC3) {
        throw std::invalid_argument("SegmentAI ExtractGpu expects CV_8UC3 BGR input.");
    }

    cv::UMat gray;
    cv::UMat hsv;
    cv::cvtColor(srcBgr, gray, cv::COLOR_BGR2GRAY);
    cv::cvtColor(srcBgr, hsv, cv::COLOR_BGR2HSV);

    outPlanes.assign(kNumFeatures, cv::UMat());

    ComputeLBPGpuCompatible(gray, lbpLookup_, outPlanes[0]);

    std::vector<cv::UMat> hsvCh;
    cv::split(hsv, hsvCh);
    for (int c = 0; c < 3; ++c) {
        cv::UMat mean;
        cv::UMat variance;
        BoxMeanStd(hsvCh[c], cfg_.hsvWindow, mean, variance);

        const double scale = (c == 0) ? 1.0 / 179.0 : 1.0 / 255.0;
        mean.convertTo(outPlanes[1 + c], CV_32F, scale);
        Clamp01(outPlanes[1 + c]);

        cv::sqrt(variance, variance);
        variance.convertTo(outPlanes[4 + c], CV_32F, scale);
        Clamp01(outPlanes[4 + c]);
    }

    for (int i = 0; i < 4; ++i) {
        cv::UMat filtered;
        cv::filter2D(gray, filtered, CV_32F, gaborKernels_[i], cv::Point(-1, -1), 0.0, cv::BORDER_REPLICATE);
        cv::absdiff(filtered, cv::Scalar::all(0), filtered);
        filtered.convertTo(outPlanes[7 + i], CV_32F, 1.0 / 255.0);
        Clamp01(outPlanes[7 + i]);
    }

    cv::UMat sx;
    cv::UMat sy;
    cv::Sobel(gray, sx, CV_32F, 1, 0, 3, 1.0, 0.0, cv::BORDER_REPLICATE);
    cv::Sobel(gray, sy, CV_32F, 0, 1, 3, 1.0, 0.0, cv::BORDER_REPLICATE);

    cv::UMat magnitude;
    cv::magnitude(sx, sy, magnitude);
    magnitude.convertTo(outPlanes[11], CV_32F, 1.0 / 255.0);
    Clamp01(outPlanes[11]);

    cv::UMat angle;
    cv::phase(sx, sy, angle, false);
    angle.convertTo(outPlanes[12], CV_32F, 1.0 / (2.0 * CV_PI));
    Clamp01(outPlanes[12]);

    cv::UMat laplacian;
    cv::Laplacian(gray, laplacian, CV_32F, 3, 1.0, 0.0, cv::BORDER_REPLICATE);
    cv::absdiff(laplacian, cv::Scalar::all(0), laplacian);
    laplacian.convertTo(outPlanes[13], CV_32F, 1.0 / 255.0);
    Clamp01(outPlanes[13]);

    FillPositionPlanes(outPlanes[14], outPlanes[15], cfg_.roi, gray.size());

    const int blurSizes[] = {3, 7, 15};
    for (int i = 0; i < 3; ++i) {
        cv::UMat blurred;
        cv::boxFilter(gray, blurred, CV_32F, cv::Size(blurSizes[i], blurSizes[i]), cv::Point(-1, -1), true, cv::BORDER_REPLICATE);
        blurred.convertTo(outPlanes[16 + i], CV_32F, 1.0 / 255.0);
        Clamp01(outPlanes[16 + i]);
    }

    cv::UMat edges;
    cv::Canny(gray, edges, 50, 150);
    edges.convertTo(edges, CV_32F, 1.0 / 255.0);

    const int edgeSizes[] = {5, 11, 21};
    for (int i = 0; i < 3; ++i) {
        cv::boxFilter(edges, outPlanes[19 + i], CV_32F, cv::Size(edgeSizes[i], edgeSizes[i]), cv::Point(-1, -1), true, cv::BORDER_REPLICATE);
        Clamp01(outPlanes[19 + i]);
    }

    cv::UMat dilated;
    cv::UMat eroded;
    const cv::Mat contrastKernel = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(5, 5));
    cv::dilate(hsvCh[2], dilated, contrastKernel, cv::Point(-1, -1), 1, cv::BORDER_REPLICATE);
    cv::erode(hsvCh[2], eroded, contrastKernel, cv::Point(-1, -1), 1, cv::BORDER_REPLICATE);

    cv::UMat contrast;
    cv::subtract(dilated, eroded, contrast);
    contrast.convertTo(outPlanes[22], CV_32F, 1.0 / 255.0);
    Clamp01(outPlanes[22]);

    FillDistanceToRoi(outPlanes[23], cfg_.roi, gray.size());
}

void SegFeatureExtractor::PackSamples(const std::vector<cv::Mat>& planes,
                                      const std::vector<cv::Point>& pixels,
                                      cv::Mat& outSamples) const
{
    EnsurePlaneSet(planes);

    outSamples.create(static_cast<int>(pixels.size()), kNumFeatures, CV_32F);
    const cv::Rect bounds(0, 0, planes[0].cols, planes[0].rows);

    for (int row = 0; row < static_cast<int>(pixels.size()); ++row) {
        if (!bounds.contains(pixels[row])) {
            throw std::out_of_range("SegmentAI sample pixel is outside feature planes.");
        }

        float* dst = outSamples.ptr<float>(row);
        for (int feature = 0; feature < kNumFeatures; ++feature) {
            dst[feature] = planes[feature].at<float>(pixels[row]);
        }
    }
}

void SegFeatureExtractor::PlanesToInterleaved(const std::vector<cv::Mat>& planes,
                                              const cv::Rect& roi,
                                              cv::Mat& outRows) const
{
    EnsurePlaneSet(planes);

    const cv::Rect bounds(0, 0, planes[0].cols, planes[0].rows);
    const cv::Rect r = roi & bounds;
    if (r.area() <= 0) {
        outRows.release();
        return;
    }

    outRows.create(r.area(), kNumFeatures, CV_32F);

    int row = 0;
    for (int y = r.y; y < r.y + r.height; ++y) {
        for (int x = r.x; x < r.x + r.width; ++x) {
            float* dst = outRows.ptr<float>(row++);
            for (int feature = 0; feature < kNumFeatures; ++feature) {
                dst[feature] = planes[feature].at<float>(y, x);
            }
        }
    }
}

} // namespace BeeSegAI
