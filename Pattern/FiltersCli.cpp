#include "FiltersCli.h"
#include "FiltersCore.hpp"
#include <opencv2/opencv.hpp>

using namespace System;

namespace BeeCpp
{
    void FilterCLi::RunEdgePipeline(
        IntPtr srcData, int width, int height, int type, size_t srcStep,
        EdgePipelineOptionsCli opts,
        IntPtr dstData, int dstType, size_t dstStep)
    {
        using namespace BeeCpp; // Core

        unsigned char* s = reinterpret_cast<unsigned char*>(srcData.ToPointer());
        unsigned char* d = reinterpret_cast<unsigned char*>(dstData.ToPointer());

        cv::Mat src(height, width, type, s, srcStep);
        cv::Mat dst(height, width, dstType, d, dstStep);

        EdgePipelineOptions o;
        o.method = static_cast<MethordEdge>(opts.Method);
        o.strongPercentile = opts.StrongPercentile;
        o.thresholdBinary = opts.ThresholdBinary;
        o.clearNoiseSmallArea = opts.ClearNoiseSmallArea;
        o.sizeClose = opts.SizeClose;
        o.sizeOpen = opts.SizeOpen;
        o.clearNoiseBigArea = opts.ClearNoiseBigArea;

        cv::Mat out;
        BeeCpp::RunEdgePipeline(src, out, o);

        if (out.type() != dstType || out.size() != dst.size())
            throw gcnew System::InvalidOperationException("Dst buffer mismatch (type/size).");

        out.copyTo(dst);
    }

    void FilterCLi::GetStrongEdgesOnly(
        IntPtr srcData, int width, int height, int type, size_t srcStep,
        double percentile,
        IntPtr dstData, int dstType, size_t dstStep)
    {
        unsigned char* s = reinterpret_cast<unsigned char*>(srcData.ToPointer());
        unsigned char* d = reinterpret_cast<unsigned char*>(dstData.ToPointer());

        cv::Mat src(height, width, type, s, srcStep);
        cv::Mat dst(height, width, dstType, d, dstStep);
        cv::Mat out;

        BeeCpp::GetStrongEdgesOnly(src, out, percentile);

        if (out.type() != dstType || out.size() != dst.size())
            throw gcnew System::InvalidOperationException("Dst buffer mismatch (type/size).");

        out.copyTo(dst);
    }

    void FilterCLi::Edge(
        IntPtr srcData, int width, int height, int type, size_t srcStep,
        IntPtr dstData, int dstType, size_t dstStep)
    {
        unsigned char* s = reinterpret_cast<unsigned char*>(srcData.ToPointer());
        unsigned char* d = reinterpret_cast<unsigned char*>(dstData.ToPointer());

        cv::Mat src(height, width, type, s, srcStep);
        cv::Mat dst(height, width, dstType, d, dstStep);
        cv::Mat out;

        BeeCpp::Edge(src, out);

        if (out.type() != dstType || out.size() != dst.size())
            throw gcnew System::InvalidOperationException("Dst buffer mismatch (type/size).");

        out.copyTo(dst);
    }

    void FilterCLi::ThresholdToEdges(
        IntPtr srcData, int width, int height, int type, size_t srcStep,
        int thresh, int thresholdType,
        IntPtr dstData, int dstType, size_t dstStep)
    {
        unsigned char* s = reinterpret_cast<unsigned char*>(srcData.ToPointer());
        unsigned char* d = reinterpret_cast<unsigned char*>(dstData.ToPointer());

        cv::Mat src(height, width, type, s, srcStep);
        cv::Mat dst(height, width, dstType, d, dstStep);
        cv::Mat out;

        BeeCpp::ThresholdToEdges(src, thresh, out, thresholdType);

        if (out.type() != dstType || out.size() != dst.size())
            throw gcnew System::InvalidOperationException("Dst buffer mismatch (type/size).");

        out.copyTo(dst);
    }

    void FilterCLi::Morphology(
        IntPtr srcData, int width, int height, int type, size_t srcStep,
        int morphType, int kW, int kH, int iterations,
        IntPtr dstData, int dstType, size_t dstStep)
    {
        unsigned char* s = reinterpret_cast<unsigned char*>(srcData.ToPointer());
        unsigned char* d = reinterpret_cast<unsigned char*>(dstData.ToPointer());

        cv::Mat src(height, width, type, s, srcStep);
        cv::Mat dst(height, width, dstType, d, dstStep);
        cv::Mat out;

        BeeCpp::Morphology(src, out, morphType, cv::Size(kW, kH), iterations);

        if (out.type() != dstType || out.size() != dst.size())
            throw gcnew System::InvalidOperationException("Dst buffer mismatch (type/size).");

        out.copyTo(dst);
    }

    void FilterCLi::ClearNoise(
        IntPtr edgesData, int width, int height, int type, size_t step,
        int minCompArea,
        IntPtr cleanData, int cleanType, size_t cleanStep)
    {
        unsigned char* e = reinterpret_cast<unsigned char*>(edgesData.ToPointer());
        unsigned char* c = reinterpret_cast<unsigned char*>(cleanData.ToPointer());

        cv::Mat edges(height, width, type, e, step);
        cv::Mat clean(height, width, cleanType, c, cleanStep);
        cv::Mat out;

        BeeCpp::ClearNoise(edges, out, minCompArea);

        if (out.type() != cleanType || out.size() != clean.size())
            throw gcnew System::InvalidOperationException("Dst buffer mismatch (type/size).");

        out.copyTo(clean);
    }

    void FilterCLi::AutoCannyThresholdFromHistogram(
        IntPtr grayData, int width, int height, int type, size_t step,
        int% lower, int% upper)
    {
        unsigned char* g = reinterpret_cast<unsigned char*>(grayData.ToPointer());
        cv::Mat gray(height, width, type, g, step);

        if (gray.type() != CV_8UC1)
            throw gcnew System::ArgumentException("gray must be CV_8UC1");

        auto th = BeeCpp::AutoCannyThresholdFromHistogram(gray);
        lower = th.lower;
        upper = th.upper;
    }
} // namespace BeeCppCli
