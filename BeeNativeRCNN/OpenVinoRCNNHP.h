#pragma once

#ifdef _MANAGED
#undef interface
#endif

#include "YoloDebugLog.h"
#include <openvino/openvino.hpp>
#include <opencv2/opencv.hpp>
#include <vector>
#include <string>

struct RCNNBox
{
    float x1, y1, x2, y2;
    float score;
    int classId;
};

class OpenVinoRCNNHP
{
public:
    enum class ModelKind
    {
        Auto = 0,
        YoloRaw,
        DetectionOutput,   // [1,N,6] or [N,6]
        RCNN_MultiOutput   // boxes + labels + scores
    };

    enum class PreprocessKind
    {
        Auto = 0,
        LetterboxYolo,
        ResizePlain
    };

public:
public:
    OpenVinoRCNNHP(
        const std::wstring& xmlPath,
        int inputW,
        int inputH,
        int numClasses,
        int numThreads = 8);

    ~OpenVinoRCNNHP() = default;

    void Warmup(int iters = 10);

    void Detect(const cv::Mat& bgr, float conf, float iou, bool Is3, std::vector<RCNNBox>& out);

private:
    // detect model type
    void AnalyzeModel();

    // preprocess
    void Letterbox(const cv::Mat& src, cv::Mat& dst, float& scale, int& padw, int& padh);
    void ResizePlain(const cv::Mat& src, cv::Mat& dst, float& scaleX, float& scaleY);

    // input convert
    void BgrToCHWFloat01(const cv::Mat& u8, float* dstCHW);

    // decode
    void DecodeYoloAuto(
        const ov::Tensor& t,
        float conf,
        float scale, int padw, int padh,
        int imgW, int imgH,
        std::vector<RCNNBox>& out);

    void DecodeDetectionOutput(
        const ov::Tensor& t,
        float conf,
        float scaleX, float scaleY,
        int padw, int padh,
        int imgW, int imgH,
        std::vector<RCNNBox>& out);

    void DecodeRCNNMultiOutput(
        const ov::Tensor& boxesT,
        const ov::Tensor& labelsT,
        const ov::Tensor& scoresT,
        float conf,
        float scaleX, float scaleY,
        int padw, int padh,
        int imgW, int imgH,
        std::vector<RCNNBox>& out);

    void NmsPerClass(std::vector<RCNNBox>& cand, float iou, std::vector<RCNNBox>& out);

private:
    int W = 640;
    int H = 640;
    int C = 80;

    ModelKind modelKind = ModelKind::Auto;
    PreprocessKind preprocessKind = PreprocessKind::Auto;

    ov::Core core;
    std::shared_ptr<ov::Model> model;
    ov::CompiledModel compiled;
    ov::InferRequest infer;

    ov::Output<const ov::Node> inputPort;
    std::vector<ov::Output<const ov::Node>> outputPorts;

    cv::Mat paddedU8;
    std::vector<float> inputBlob;
    std::vector<RCNNBox> candidates;
};