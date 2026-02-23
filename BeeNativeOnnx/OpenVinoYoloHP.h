#pragma once

#ifdef _MANAGED
#undef interface
#endif
#include "YoloDebugLog.h"
#include <openvino/openvino.hpp>
#include <opencv2/opencv.hpp>
#include <vector>
#include <string>
#include <array>

struct YoloBox
{
    float x1, y1, x2, y2;
    float score;
    int classId;
};

class OpenVinoYoloHP
{
public:
    OpenVinoYoloHP(
        const std::wstring& xmlPath,
        int inputSize,
        int numClasses,
        int numThreads = 8);   // 👈 thêm
    
    ~OpenVinoYoloHP() = default;

    void Warmup(int iters = 10);

    // BGR uint8
    void Detect(const cv::Mat& bgr, float conf, float iou, std::vector<YoloBox>& out);

private:
    void Letterbox(const cv::Mat& src, cv::Mat& dst, float& scale, int& padw, int& padh);
    void BgrToCHWFloat01(const cv::Mat& u8, float* dstCHW); // directly write CHW float
    void DecodeDetectionOutput(
        const ov::Tensor& t,
        float conf,
        float scale, int padw, int padh,
        int imgW, int imgH,
        std::vector<YoloBox>& out);
    void DecodeAnyLayout(const ov::Tensor& outTensor,
        float conf, float scale, int padw, int padh, int srcW, int srcH,
        std::vector<YoloBox>& cand);

    void NmsPerClass(std::vector<YoloBox>& cand, float iou, std::vector<YoloBox>& out);

private:
    int S = 640;
    int C = 80;

    ov::Core core;
    std::shared_ptr<ov::Model> model;
    ov::CompiledModel compiled;
    ov::InferRequest infer;

    // ports
    ov::Output<const ov::Node> inputPort;
    ov::Output<const ov::Node> outputPort;

    // reuse buffers
    cv::Mat paddedU8;                 // SxS CV_8UC3
    std::vector<float> inputBlob;     // 1*3*S*S

    // candidates reuse
    std::vector<YoloBox> candidates;
};
