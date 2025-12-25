#pragma once
#include <opencv2/opencv.hpp>
#include <algorithm>
#include <vector>
#include <cmath>
#include <cstring>
#include <stdexcept>

namespace BeeCpp {
    static inline int Clamp(int v, int lo, int hi) { return (v < lo) ? lo : (v > hi) ? hi : v; }
    // ==== Edge method enum ====
    enum class MethordEdge {
        CloseEdges = 0,  // auto-canny + close + dilate/erode
        StrongEdges = 1,  // percentile on gradient magnitude
        Binary = 2,  // threshold -> canny(0..255)
        InvertBinary = 3   // threshold_inv -> canny(0..255)
    };

    // ==== Pipeline options ====
     struct EdgePipelineOptions {
        MethordEdge method = MethordEdge::CloseEdges;

        // StrongEdges
        double strongPercentile = 0.98;   // 0..1

        // Binary/InvertBinary
        int thresholdBinary = 128;        // 0..255

        // post-processing chain (0 = skip)
        int clearNoiseSmallArea = 0;
        int sizeClose = 0;      // kernel WxH = sizeClose x sizeClose
        int sizeOpen = 0;
        int clearNoiseBigArea = 0;
    };

    // ==== Helpers used by pipeline ====
    struct CannyThresh { int lower; int upper; };

    inline CannyThresh AutoCannyThresholdFromHistogram(const cv::Mat& gray, double k1 = 0.66, double k2 = 1.33) {
        CV_Assert(gray.type() == CV_8UC1);
        int histSize = 256;
        float range[] = { 0, 256 };
        const float* ranges[] = { range };
        cv::Mat hist;
        int channels[] = { 0 };
        cv::calcHist(&gray, 1, channels, cv::Mat(), hist, 1, &histSize, ranges);

        double minVal, maxVal;
        cv::Point minLoc, maxLoc;
        cv::minMaxLoc(hist, &minVal, &maxVal, &minLoc, &maxLoc);

        int peak = (maxLoc.y != 0) ? maxLoc.y : maxLoc.x;
        int lower = (int)std::max(0.0, peak * k1);
        int upper = (int)std::min(255.0, peak * k2);
        return { lower, upper };
    }

    inline void GetStrongEdgesOnly(const cv::Mat& raw, cv::Mat& out, double percentile = 0.98)
    {
        CV_Assert(!raw.empty());
        cv::Mat gray;
        if (raw.channels() == 1) gray = raw;
        else cv::cvtColor(raw, gray, cv::COLOR_BGR2GRAY);

        cv::Mat blur; cv::GaussianBlur(gray, blur, cv::Size(3, 3), 1.0);

        cv::Mat gradX, gradY;
        cv::Sobel(blur, gradX, CV_32F, 1, 0, 3);
        cv::Sobel(blur, gradY, CV_32F, 0, 1, 3);

        cv::Mat magnitude; cv::magnitude(gradX, gradY, magnitude);

        // percentile threshold
        std::vector<float> buf;
        buf.resize((size_t)magnitude.rows * (size_t)magnitude.cols);
        std::memcpy(buf.data(), magnitude.ptr<float>(0), buf.size() * sizeof(float));
        std::sort(buf.begin(), buf.end());
        const int len = (int)buf.size();
        int idx =Clamp((int)std::floor(len * percentile), 0, len - 1);
        float thr = buf[(size_t)idx];
        cv::imwrite("gray.png", magnitude);
        cv::Mat bin; cv::threshold(magnitude, bin, thr, 255, cv::THRESH_BINARY);
        bin.convertTo(out, CV_8U);
       
    }

    inline void Edge(const cv::Mat& raw, cv::Mat& edgesOut)
    {
        CV_Assert(!raw.empty());
        cv::Mat gray;
        if (raw.type() == CV_8UC3) cv::cvtColor(raw, gray, cv::COLOR_BGR2GRAY);
        else gray = raw;

        // trunc highlight
        cv::threshold(gray, gray, 245, 245, cv::THRESH_TRUNC);
        // normalize
        cv::normalize(gray, gray, 0, 255, cv::NORM_MINMAX);
        // smooth
        cv::Mat smooth; cv::GaussianBlur(gray, smooth, cv::Size(5, 5), 1.0);

        auto th = AutoCannyThresholdFromHistogram(smooth, 0.66, 1.33);
        cv::Canny(smooth, edgesOut, th.lower, th.upper);

        cv::Mat kernelClose = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(3, 3));
        cv::morphologyEx(edgesOut, edgesOut, cv::MORPH_CLOSE, kernelClose);

        cv::Mat kernel = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(3, 3));
        cv::dilate(edgesOut, edgesOut, kernel, cv::Point(-1, -1), 1);
        cv::erode(edgesOut, edgesOut, kernel, cv::Point(-1, -1), 1);
    }

    inline void ThresholdToEdges(const cv::Mat& raw, int thresh, cv::Mat& edgesOut, int thresholdType = cv::THRESH_BINARY)
    {
        CV_Assert(!raw.empty());
        cv::Mat gray;
        if (raw.type() == CV_8UC3) cv::cvtColor(raw, gray, cv::COLOR_BGR2GRAY);
        else gray = raw.clone();

        cv::threshold(gray, gray, thresh, 255, thresholdType);
        cv::Canny(gray, edgesOut, 0, 255);

        cv::Mat kernelClose = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(3, 3));
        cv::morphologyEx(edgesOut, edgesOut, cv::MORPH_CLOSE, kernelClose);

        cv::Mat kernel = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(3, 3));
        cv::dilate(edgesOut, edgesOut, kernel, cv::Point(-1, -1), 1);
        cv::erode(edgesOut, edgesOut, kernel, cv::Point(-1, -1), 1);
    }

    inline void Morphology(const cv::Mat& src, cv::Mat& dst, int morphType, cv::Size ksize, int iterations = 1)
    {
        cv::Mat kernel = cv::getStructuringElement(cv::MORPH_ELLIPSE, ksize);
        cv::morphologyEx(src, dst, morphType, kernel, cv::Point(-1, -1), iterations);
    }

    inline void ClearNoise(const cv::Mat& edges, cv::Mat& clean, int minCompArea = 1000)
    {
        CV_Assert(edges.type() == CV_8U);
        cv::Mat labels, stats, centroids;
        int num = cv::connectedComponentsWithStats(edges, labels, stats, centroids, 8, CV_32S);
        clean = cv::Mat::zeros(edges.size(), CV_8U);

        for (int i = 1; i < num; ++i) {
            int area = stats.at<int>(i, cv::CC_STAT_AREA);
            if (area >= minCompArea) {
                cv::Mat mask; cv::inRange(labels, i, i, mask);
                clean.setTo(255, mask);
            }
        }
    }

    // === Extra filters (same as your C#) ===
    inline void Sharpen(const cv::Mat& src, cv::Mat& dst, double amount = 1.0) {
        cv::Mat blurred; cv::GaussianBlur(src, blurred, cv::Size(0, 0), 3);
        cv::addWeighted(src, 1 + amount, blurred, -amount, 0, dst);
    }
    inline void Clahe(const cv::Mat& src, cv::Mat& dst, double clipLimit, cv::Size tile) {
        cv::Ptr<cv::CLAHE> clahe = cv::createCLAHE(clipLimit, tile);
        clahe->apply(src, dst);
    }
    inline void HistEq(const cv::Mat& src, cv::Mat& dst) {
        cv::equalizeHist(src, dst);
    }
    inline void Gamma(const cv::Mat& src, cv::Mat& dst, double gamma) {
        CV_Assert(src.type() == CV_8UC1);
        cv::Mat lut(1, 256, CV_8UC1);
        for (int i = 0; i < 256; i++) lut.at<uchar>(0, i) = (uchar)(std::pow(i / 255.0, 1.0 / gamma) * 255.0 + 0.5);
        cv::LUT(src, lut, dst);
    }

    // ==== ONE-PASS PIPELINE ====
    inline void RunEdgePipeline(const cv::Mat& srcBgrOrGray, cv::Mat& dstEdges8u, const EdgePipelineOptions& opt)
    {
        CV_Assert(!srcBgrOrGray.empty());
        cv::Mat edge; // 8UC1
        
        BeeCpp::GetStrongEdgesOnly(srcBgrOrGray, edge, 1);
        /*switch (opt.method) {
        case MethordEdge::CloseEdges: {
            BeeCpp::Edge(srcBgrOrGray, edge);
        } break;
        case MethordEdge::StrongEdges: {
            BeeCpp::GetStrongEdgesOnly(srcBgrOrGray, edge, 1);
        } break;
        case MethordEdge::Binary: {
            BeeCpp::ThresholdToEdges(srcBgrOrGray, opt.thresholdBinary, edge, cv::THRESH_BINARY);
        } break;
        case MethordEdge::InvertBinary: {
            BeeCpp::ThresholdToEdges(srcBgrOrGray, opt.thresholdBinary, edge, cv::THRESH_BINARY_INV);
        } break;
        default:
            BeeCpp::Edge(srcBgrOrGray, edge);
            break;
        }*/

        cv::Mat cur = edge;

        if (opt.clearNoiseSmallArea > 0) {
            cv::Mat tmp; BeeCpp::ClearNoise(cur, tmp, opt.clearNoiseSmallArea);
            cur = tmp;
        }

        if (opt.sizeClose > 0) {
            int k = std::max(1, opt.sizeClose);
            cv::Mat tmp; BeeCpp::Morphology(cur, tmp, cv::MORPH_CLOSE, cv::Size(k, k), 1);
            cur = tmp;
        }

        if (opt.sizeOpen > 0) {
            int k = std::max(1, opt.sizeOpen);
            cv::Mat tmp; BeeCpp::Morphology(cur, tmp, cv::MORPH_OPEN, cv::Size(k, k), 1);
            cur = tmp;
        }

        if (opt.clearNoiseBigArea > 0) {
            cv::Mat tmp; BeeCpp::ClearNoise(cur, tmp, opt.clearNoiseBigArea);
            cur = tmp;
        }

        cur.copyTo(dstEdges8u); // ensure 8UC1
    }

} // namespace BeeCpp
