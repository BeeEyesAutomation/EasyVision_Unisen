#include "pch.h"

#include "SegAINativeExport.h"

#include "SegFeatureCore.h"
#include "SegInferCore.h"
#include "SegTrainerCore.h"

#include <opencv2/core/ocl.hpp>
#include <opencv2/core/version.hpp>
#include <opencv2/imgproc.hpp>

#include <filesystem>

namespace
{
volatile int32_t kSegAIVersion = 1;

bool IsPlaneValid(const cv::Mat& plane, const cv::Size& expectedSize)
{
    if (plane.empty() || plane.type() != CV_32F || plane.size() != expectedSize) {
        return false;
    }

    double minValue = 0.0;
    double maxValue = 0.0;
    cv::minMaxLoc(plane, &minValue, &maxValue);
    return minValue >= -1.0e-6 && maxValue <= 1.0 + 1.0e-6;
}
}

SAPI int32_t SEGAI_GetVersion()
{
    return kSegAIVersion;
}

SAPI const char* SEGAI_GetBuildInfo()
{
    return "BeeNativeSegAI 1.0 / OpenCV " CV_VERSION;
}

SAPI int32_t SEGAI_RunFeatureSelfTest()
{
    try {
        cv::Mat image(32, 48, CV_8UC3);
        for (int y = 0; y < image.rows; ++y) {
            for (int x = 0; x < image.cols; ++x) {
                image.at<cv::Vec3b>(y, x) = cv::Vec3b(
                    static_cast<unsigned char>((x * 5) % 256),
                    static_cast<unsigned char>((y * 7) % 256),
                    static_cast<unsigned char>(((x + y) * 3) % 256));
            }
        }
        cv::rectangle(image, cv::Rect(14, 10, 12, 8), cv::Scalar(255, 255, 255), cv::FILLED);

        BeeSegAI::FeatureConfig config;
        config.roi = cv::Rect(4, 3, 30, 20);

        BeeSegAI::SegFeatureExtractor extractor;
        extractor.Configure(config);

        std::vector<cv::Mat> planes;
        extractor.ExtractCPU(image, planes);
        if (planes.size() != BeeSegAI::SegFeatureExtractor::kNumFeatures) {
            return -1;
        }

        for (const cv::Mat& plane : planes) {
            if (!IsPlaneValid(plane, image.size())) {
                return -2;
            }
        }

        if (cv::ocl::haveOpenCL()) {
            cv::UMat imageGpu;
            image.copyTo(imageGpu);
            std::vector<cv::UMat> gpuPlanes;
            extractor.ExtractGpu(imageGpu, gpuPlanes);
            if (gpuPlanes.size() != BeeSegAI::SegFeatureExtractor::kNumFeatures) {
                return -15;
            }

            for (size_t i = 0; i < gpuPlanes.size(); ++i) {
                cv::Mat gpuPlane;
                gpuPlanes[i].copyTo(gpuPlane);
                if (!IsPlaneValid(gpuPlane, image.size())) {
                    return -16;
                }
            }
        }

        const std::vector<cv::Point> pixels = {cv::Point(1, 1), cv::Point(20, 15), cv::Point(47, 31)};
        cv::Mat samples;
        extractor.PackSamples(planes, pixels, samples);
        if (samples.rows != static_cast<int>(pixels.size()) ||
            samples.cols != BeeSegAI::SegFeatureExtractor::kNumFeatures ||
            samples.type() != CV_32F) {
            return -3;
        }

        cv::Mat interleaved;
        extractor.PlanesToInterleaved(planes, config.roi, interleaved);
        if (interleaved.rows != config.roi.area() ||
            interleaved.cols != BeeSegAI::SegFeatureExtractor::kNumFeatures ||
            interleaved.type() != CV_32F) {
            return -4;
        }

        cv::Mat mask(image.size(), CV_8UC1, cv::Scalar(2));
        cv::rectangle(mask, cv::Rect(14, 10, 12, 8), cv::Scalar(1), cv::FILLED);

        BeeSegAI::SegTrainer trainer;
        config.roi = cv::Rect(0, 0, image.cols, image.rows);
        trainer.Configure(config);
        if (trainer.AddSample(image, mask, config.roi) != 0) {
            return -5;
        }
        if (trainer.Train(8, 5, 2) != 0) {
            return -6;
        }

        const std::filesystem::path modelPath = std::filesystem::temp_directory_path() / L"BeeNativeSegAI_selftest.segai";
        if (!trainer.SaveModel(modelPath.wstring(), 0.5f, 50)) {
            return -7;
        }

        float threshold = 0.0f;
        uint32_t minArea = 0;
        BeeSegAI::SegTrainer loadedTrainer;
        if (!loadedTrainer.LoadModel(modelPath.wstring(), threshold, minArea)) {
            std::filesystem::remove(modelPath);
            return -8;
        }

        if (threshold < 0.49f || threshold > 0.51f || minArea != 50) {
            std::filesystem::remove(modelPath);
            return -9;
        }

        const float defectPrediction = loadedTrainer.PredictPixel(image, cv::Point(18, 14));
        const float normalPrediction = loadedTrainer.PredictPixel(image, cv::Point(2, 2));
        if (defectPrediction < 0.5f || normalPrediction > 0.5f) {
            std::filesystem::remove(modelPath);
            return -10;
        }

        BeeSegAI::SegInferer inferer;
        if (!inferer.LoadModel(modelPath.wstring())) {
            std::filesystem::remove(modelPath);
            return -11;
        }

        cv::Mat predMask;
        float score = 0.0f;
        if (inferer.Predict(image, config.roi, 0.5f, predMask, score) != 0) {
            std::filesystem::remove(modelPath);
            return -12;
        }
        std::filesystem::remove(modelPath);

        if (predMask.empty() || predMask.size() != image.size() || score <= 0.0f || score > 1.0f) {
            return -13;
        }
        if (cv::countNonZero(predMask(cv::Rect(14, 10, 12, 8))) <= 0) {
            return -14;
        }

        if (cv::ocl::haveOpenCL()) {
            inferer.SetGpuEnabled(false);
            cv::Mat cpuMask;
            float cpuScore = 0.0f;
            if (inferer.Predict(image, config.roi, 0.5f, cpuMask, cpuScore) != 0) {
                return -17;
            }

            inferer.SetGpuEnabled(true);
            cv::Mat gpuMask;
            float gpuScore = 0.0f;
            if (inferer.Predict(image, config.roi, 0.5f, gpuMask, gpuScore) != 0) {
                return -18;
            }

            cv::Mat diff;
            cv::compare(cpuMask, gpuMask, diff, cv::CMP_NE);
            const double totalPixels = static_cast<double>(cpuMask.total());
            const double agreement = totalPixels > 0.0 ? 1.0 - (static_cast<double>(cv::countNonZero(diff)) / totalPixels) : 1.0;
            if (agreement < 0.99) {
                return -19;
            }
        }

        return 0;
    } catch (...) {
        return -100;
    }
}
