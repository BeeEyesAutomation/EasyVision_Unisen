#include "YoloNativeExport.h"
#include "OpenVinoYoloHP.h"
#include <opencv2/opencv.hpp>
#include <vector>
static int InferInputSizeFromModel(const wchar_t* xmlPath)
{
    ov::Core core;
    auto model = core.read_model(xmlPath);
    auto in = model->input();
    auto shp = in.get_shape(); // usually [1,3,H,W] or [1,H,W,3]

    if (shp.size() == 4)
    {
        // try NCHW
        int h = (int)shp[2];
        int w = (int)shp[3];

        // if looks like NHWC
        if ((int)shp[1] != 3 && (int)shp[3] == 3)
        {
            h = (int)shp[1];
            w = (int)shp[2];
        }

        // prefer square
        int s = std::min(h, w);
        if (s > 0) return s;
    }
    return 640; // fallback
}

static int InferNumClassesFromOutput(const wchar_t* xmlPath)
{
    ov::Core core;
    auto model = core.read_model(xmlPath);
    auto out = model->output();
    auto shp = out.get_shape();

    // Case: [1,300,6] => already post-processed => cannot know total classes reliably
    if (shp.size() == 3 && shp[2] == 6) return -1;

    // Common raw-yolo guesses:
    // [1, N, 5+nc]
    if (shp.size() == 3 && shp[2] > 6 && shp[2] < 2000) return (int)shp[2] - 5;

    // [1, 5+nc, N]
    if (shp.size() == 3 && shp[1] > 6 && shp[1] < 2000) return (int)shp[1] - 5;

    return -1;
}
extern "C"
{
    void* YOLO_Create(const wchar_t* modelPath, int inputSize, int numClasses, int numThreads)
    {
        if (!modelPath) return nullptr;

        // 1) auto input size if needed
        int S = inputSize;
        if (S <= 0)
            S = InferInputSizeFromModel(modelPath);

        // 2) auto numClasses if needed (may be -1)
        int nc = numClasses;
        if (nc <= 0)
            nc = InferNumClassesFromOutput(modelPath);

        // 3) create engine
        // NOTE: OpenVinoYoloHP should accept nc=-1 too if your output is [1,300,6].
        return new OpenVinoYoloHP(modelPath, S, nc, numThreads);
       // return new OpenVinoYoloHP(modelPath, inputSize, numClasses, numThreads);
    }

    void YOLO_Destroy(void* handle)
    {
        if (handle)
            delete (OpenVinoYoloHP*)handle;
    }

    void YOLO_Warmup(void* handle, int iters)
    {
        if (handle)
            ((OpenVinoYoloHP*)handle)->Warmup(iters);
    }

    int YOLO_Detect(
        void* handle,
        const uint8_t* bgr, int w, int h, int step,
        float conf, float iou,
        YoloBoxC* outBoxes,
        int maxBoxes)
    {
        if (!handle) return 0;

        cv::Mat img(h, w, CV_8UC3, (void*)bgr, step);

        std::vector<YoloBox> result;
        ((OpenVinoYoloHP*)handle)->Detect(img, conf, iou, result);

        int n = (int)result.size();
        if (n > maxBoxes) n = maxBoxes;

        for (int i = 0; i < n; i++)
        {
            outBoxes[i] = {
                result[i].x1, result[i].y1,
                result[i].x2, result[i].y2,
                result[i].score,
                result[i].classId
            };
        }
        return n;
    }
}
