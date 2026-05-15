#include "../SegAINativeExport.h"

#include <opencv2/imgcodecs.hpp>
#include <opencv2/imgproc.hpp>

#include <filesystem>
#include <iostream>
#include <iomanip>
#include <string>
#include <vector>

namespace {

cv::Rect BenchmarkRoi()
{
    return cv::Rect(384, 224, 512, 512);
}

void MakeSynthetic(cv::Mat& image, cv::Mat& mask)
{
    image = cv::Mat(960, 1280, CV_8UC3, cv::Scalar(240, 240, 240));
    mask = cv::Mat(960, 1280, CV_8UC1, cv::Scalar(2));

    for (int y = 0; y < image.rows; ++y) {
        for (int x = 0; x < image.cols; ++x) {
            if (((x / 32) + (y / 32)) % 2 == 0) {
                image.at<cv::Vec3b>(y, x) = cv::Vec3b(225, 225, 225);
            }
        }
    }

    const cv::Rect defect(512, 352, 256, 256);
    cv::rectangle(image, defect, cv::Scalar(20, 20, 20), cv::FILLED);
    cv::rectangle(mask, defect, cv::Scalar(1), cv::FILLED);
}

bool LoadOrSynthetic(int argc, char** argv, cv::Mat& image, cv::Mat& mask)
{
    if (argc >= 3) {
        image = cv::imread(argv[1], cv::IMREAD_COLOR);
        mask = cv::imread(argv[2], cv::IMREAD_GRAYSCALE);
        if (!image.empty() && !mask.empty() && image.size() == mask.size()) {
            return true;
        }
    }

    MakeSynthetic(image, mask);
    std::filesystem::create_directories("BeeNativeSegAI/test/data");
    cv::imwrite("BeeNativeSegAI/test/data/sample_defect_01.png", image);
    cv::imwrite("BeeNativeSegAI/test/data/sample_mask_01.png", mask);
    return true;
}

cv::Mat SolidMask(const cv::Mat& image, unsigned char label)
{
    return cv::Mat(image.size(), CV_8UC1, cv::Scalar(label));
}

cv::Rect ClampRect(const cv::Rect& roi, const cv::Size& size)
{
    return roi & cv::Rect(0, 0, size.width, size.height);
}

std::vector<cv::Rect> DefaultWeldRois(const cv::Mat& image)
{
    const int y = static_cast<int>(image.rows * 0.18);
    const int h = static_cast<int>(image.rows * 0.28);
    const int w = static_cast<int>(image.cols * 0.30);
    const int leftX = static_cast<int>(image.cols * 0.08);
    const int rightX = static_cast<int>(image.cols * 0.43);

    return {
        ClampRect(cv::Rect(leftX, y, w, h), image.size()),
        ClampRect(cv::Rect(rightX, y, w, h), image.size())
    };
}

cv::Rect FindTemplateRoi(const cv::Mat& image, const cv::Mat& templ)
{
    if (image.empty() || templ.empty() || templ.cols > image.cols || templ.rows > image.rows) {
        return {};
    }

    cv::Mat result;
    cv::matchTemplate(image, templ, result, cv::TM_CCOEFF_NORMED);

    double maxValue = 0.0;
    cv::Point maxLoc;
    cv::minMaxLoc(result, nullptr, &maxValue, nullptr, &maxLoc);
    if (maxValue < 0.55) {
        return {};
    }

    const int padX = std::max(12, templ.cols / 20);
    const int padY = std::max(12, templ.rows / 20);
    return ClampRect(
        cv::Rect(maxLoc.x - padX, maxLoc.y - padY, templ.cols + padX * 2, templ.rows + padY * 2),
        image.size());
}

std::vector<cv::Rect> TemplateWeldRois(const cv::Mat& image, const cv::Mat& ng, const cv::Mat& ok)
{
    cv::Rect left = FindTemplateRoi(image, ng);
    cv::Rect right = FindTemplateRoi(image, ok);
    if (left.area() <= 0 || right.area() <= 0) {
        return DefaultWeldRois(image);
    }
    return {left, right};
}

void WriteOverlay(const cv::Mat& image, const cv::Mat& mask, const std::string& path)
{
    cv::Mat overlay = image.clone();
    cv::Mat red(image.size(), image.type(), cv::Scalar(0, 0, 255));
    red.copyTo(overlay, mask > 0);
    cv::addWeighted(overlay, 0.35, image, 0.65, 0.0, overlay);
    cv::imwrite(path, overlay);
}

int RunCropSampleMode(const char* ngPath, const char* okPath, const char* testPath)
{
    cv::Mat ng = cv::imread(ngPath, cv::IMREAD_COLOR);
    cv::Mat ok = cv::imread(okPath, cv::IMREAD_COLOR);
    cv::Mat test = cv::imread(testPath, cv::IMREAD_COLOR);
    if (ng.empty() || ok.empty() || test.empty()) {
        std::cerr << "Failed to load NG/OK/test image inputs\n";
        return 10;
    }

    const std::wstring modelPath = L"BeeNativeSegAI/test/data/out_weld_model.segai";
    void* trainer = SEGAI_TrainerCreate();
    if (!trainer) {
        std::cerr << "SEGAI_TrainerCreate failed\n";
        return 11;
    }

    cv::Mat ngMask = SolidMask(ng, 1);
    cv::Mat okMask = SolidMask(ok, 2);

    int rc = SEGAI_TrainerSetROI(trainer, 0, 0, ng.cols, ng.rows);
    if (rc == 0) {
        rc = SEGAI_TrainerAddSample(trainer, ng.data, ng.cols, ng.rows, static_cast<int>(ng.step), ngMask.data, static_cast<int>(ngMask.step));
    }
    if (rc == 0) {
        rc = SEGAI_TrainerSetROI(trainer, 0, 0, ok.cols, ok.rows);
    }
    if (rc == 0) {
        rc = SEGAI_TrainerAddSample(trainer, ok.data, ok.cols, ok.rows, static_cast<int>(ok.step), okMask.data, static_cast<int>(okMask.step));
    }

    int defectCount = 0;
    int normalCount = 0;
    if (rc == 0) {
        rc = SEGAI_TrainerSampleCount(trainer, &defectCount, &normalCount);
    }
    if (rc == 0) {
        rc = SEGAI_TrainerTrain(trainer, 40, 8, 5, nullptr, nullptr, nullptr);
    }
    if (rc == 0) {
        rc = SEGAI_TrainerSave(trainer, modelPath.c_str(), 0.5f, 40);
    }
    SEGAI_TrainerDestroy(trainer);
    if (rc != 0) {
        std::cerr << "Crop trainer pipeline failed: " << rc << "\n";
        return 12;
    }

    void* inferer = SEGAI_InferCreate();
    if (!inferer) {
        std::cerr << "SEGAI_InferCreate failed\n";
        return 13;
    }
    rc = SEGAI_InferLoad(inferer, modelPath.c_str());
    if (rc != 0) {
        SEGAI_InferDestroy(inferer);
        std::cerr << "Infer load failed: " << rc << "\n";
        return 14;
    }

    cv::Mat combinedMask = cv::Mat::zeros(test.size(), CV_8UC1);
    std::vector<cv::Rect> rois = TemplateWeldRois(test, ng, ok);
    std::vector<float> scores;

    for (size_t i = 0; i < rois.size(); ++i) {
        uint8_t* outMask = nullptr;
        int outW = 0;
        int outH = 0;
        float score = 0.0f;
        const cv::Rect roi = rois[i];
        rc = SEGAI_InferPredict(
            inferer,
            test.data,
            test.cols,
            test.rows,
            static_cast<int>(test.step),
            roi.x,
            roi.y,
            roi.width,
            roi.height,
            0.5f,
            &outMask,
            &outW,
            &outH,
            &score);
        if (rc != 0 || !outMask) {
            SEGAI_InferDestroy(inferer);
            std::cerr << "Infer predict failed for ROI " << i << ": " << rc << "\n";
            return 15;
        }

        cv::Mat pred(outH, outW, CV_8UC1, outMask);
        cv::bitwise_or(combinedMask, pred, combinedMask);
        SEGAI_FreeBuffer(outMask);
        scores.push_back(score);
    }

    SEGAI_InferDestroy(inferer);

    cv::imwrite("BeeNativeSegAI/test/data/out_weld_pred.png", combinedMask);
    WriteOverlay(test, combinedMask, "BeeNativeSegAI/test/data/out_weld_overlay.png");

    std::cout << "trained_defect_pixels=" << defectCount
              << " trained_normal_pixels=" << normalCount
              << " left_roi=" << rois[0].x << "," << rois[0].y << "," << rois[0].width << "," << rois[0].height
              << " right_roi=" << rois[1].x << "," << rois[1].y << "," << rois[1].width << "," << rois[1].height
              << " left_score=" << scores[0]
              << " right_score=" << scores[1]
              << " ratio_left_over_right=" << (scores[1] > 0.0f ? scores[0] / scores[1] : 999.0f)
              << " pred=BeeNativeSegAI/test/data/out_weld_pred.png"
              << " overlay=BeeNativeSegAI/test/data/out_weld_overlay.png"
              << "\n";

    return scores[0] > scores[1] ? 0 : 16;
}

double ComputeIou(const cv::Mat& pred, const cv::Mat& label)
{
    cv::Mat predBin = pred > 0;
    cv::Mat labelBin = label == 1;
    cv::Mat intersection;
    cv::Mat unionMask;
    cv::bitwise_and(predBin, labelBin, intersection);
    cv::bitwise_or(predBin, labelBin, unionMask);
    const int unionCount = cv::countNonZero(unionMask);
    if (unionCount == 0) {
        return 0.0;
    }
    return static_cast<double>(cv::countNonZero(intersection)) / static_cast<double>(unionCount);
}

double ComputeAgreement(const cv::Mat& left, const cv::Mat& right)
{
    if (left.empty() || right.empty() || left.size() != right.size()) {
        return 0.0;
    }

    cv::Mat diff;
    cv::compare(left, right, diff, cv::CMP_NE);
    const double total = static_cast<double>(left.total());
    if (total <= 0.0) {
        return 1.0;
    }

    return 1.0 - (static_cast<double>(cv::countNonZero(diff)) / total);
}

} // namespace

int main(int argc, char** argv)
{
    if (argc >= 4) {
        return RunCropSampleMode(argv[1], argv[2], argv[3]);
    }

    cv::Mat image;
    cv::Mat mask;
    LoadOrSynthetic(argc, argv, image, mask);

    const std::wstring modelPath = L"BeeNativeSegAI/test/data/out_model.segai";
    const std::string predPath = "BeeNativeSegAI/test/data/out_pred.png";

    void* trainer = SEGAI_TrainerCreate();
    if (!trainer) {
        std::cerr << "SEGAI_TrainerCreate failed\n";
        return 1;
    }

    const cv::Rect roi = BenchmarkRoi();

    int rc = SEGAI_TrainerSetROI(trainer, roi.x, roi.y, roi.width, roi.height);
    if (rc == 0) {
        rc = SEGAI_TrainerAddSample(trainer, image.data, image.cols, image.rows, static_cast<int>(image.step), mask.data, static_cast<int>(mask.step));
    }
    if (rc == 0) {
        rc = SEGAI_TrainerTrain(trainer, 12, 6, 2, nullptr, nullptr, nullptr);
    }
    if (rc == 0) {
        rc = SEGAI_TrainerSave(trainer, modelPath.c_str(), 0.5f, 20);
    }
    SEGAI_TrainerDestroy(trainer);

    if (rc != 0) {
        std::cerr << "Trainer pipeline failed: " << rc << "\n";
        return 2;
    }

    void* inferer = SEGAI_InferCreate();
    if (!inferer) {
        std::cerr << "SEGAI_InferCreate failed\n";
        return 3;
    }

    uint8_t* outMask = nullptr;
    int outW = 0;
    int outH = 0;
    float score = 0.0f;

    rc = SEGAI_InferLoad(inferer, modelPath.c_str());
    if (rc != 0) {
        SEGAI_InferDestroy(inferer);
        std::cerr << "Infer pipeline failed: " << rc << "\n";
        return 4;
    }

    cv::Mat predCopy;
    double cpuMs = 0.0;
    double gpuMs = 0.0;
    double agreement = 1.0;
    float gpuScore = 0.0f;

    {
        cv::TickMeter meter;
        meter.start();
        SEGAI_InferSetGpu(inferer, 0);
        rc = SEGAI_InferPredict(
            inferer,
            image.data,
            image.cols,
            image.rows,
            static_cast<int>(image.step),
            roi.x,
            roi.y,
            roi.width,
            roi.height,
            0.5f,
            &outMask,
            &outW,
            &outH,
            &score);
        meter.stop();
        cpuMs = meter.getTimeMilli();
    }

    if (rc != 0 || !outMask) {
        SEGAI_InferDestroy(inferer);
        std::cerr << "CPU infer pipeline failed: " << rc << "\n";
        return 4;
    }

    cv::Mat pred(outH, outW, CV_8UC1, outMask);
    predCopy = pred.clone();
    SEGAI_FreeBuffer(outMask);
    outMask = nullptr;

    if (SEGAI_InferGetGpuAvailable() != 0) {
        cv::TickMeter meter;
        meter.start();
        SEGAI_InferSetGpu(inferer, 1);
        rc = SEGAI_InferPredict(
            inferer,
            image.data,
            image.cols,
            image.rows,
            static_cast<int>(image.step),
            roi.x,
            roi.y,
            roi.width,
            roi.height,
            0.5f,
            &outMask,
            &outW,
            &outH,
            &gpuScore);
        meter.stop();
        gpuMs = meter.getTimeMilli();

        if (rc != 0 || !outMask) {
            SEGAI_InferDestroy(inferer);
            std::cerr << "GPU infer pipeline failed: " << rc << "\n";
            return 6;
        }

        cv::Mat gpuPred(outH, outW, CV_8UC1, outMask);
        agreement = ComputeAgreement(predCopy, gpuPred);
        SEGAI_FreeBuffer(outMask);
        outMask = nullptr;
    }

    SEGAI_InferDestroy(inferer);

    cv::imwrite(predPath, predCopy);

    const double iou = ComputeIou(predCopy(roi), mask(roi));
    std::cout << std::fixed << std::setprecision(6)
              << "score=" << score
              << " iou=" << iou
              << " cpu_ms=" << cpuMs;
    if (gpuMs > 0.0) {
        std::cout << " gpu_ms=" << gpuMs
                  << " gpu_speedup=" << (cpuMs / gpuMs)
                  << " gpu_agreement=" << agreement;
    } else {
        std::cout << " gpu_ms=unavailable gpu_speedup=unavailable gpu_agreement=unavailable";
    }
    std::cout << " pred=" << predPath << "\n";
    return iou >= 0.50 && (gpuMs <= 0.0 || agreement >= 0.99) ? 0 : 5;
}
