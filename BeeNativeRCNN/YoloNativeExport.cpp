#include "YoloNativeExport.h"
#include "OpenVinoRCNNHP.h"
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
{void* YOLO_Create(
    const wchar_t* modelPath,
    int inputW,
    int inputH,
    int numClasses,
    int numThreads)
{
    if (!modelPath) return nullptr;

    int W = inputW;
    int H = inputH;

    // auto nếu <=0
    if (W <= 0 || H <= 0)
    {
        ov::Core core;
        auto model = core.read_model(modelPath);
        auto in = model->input();
        auto shp = in.get_shape(); // [1,3,H,W]

        if (shp.size() == 4)
        {
            H = (int)shp[2];
            W = (int)shp[3];

            // NHWC
            if ((int)shp[1] != 3 && (int)shp[3] == 3)
            {
                H = (int)shp[1];
                W = (int)shp[2];
            }
        }
    }

    int nc = numClasses;
    if (nc <= 0)
        nc = InferNumClassesFromOutput(modelPath);

    return new OpenVinoRCNNHP(modelPath, W, H, nc, numThreads);
}
   
    void YOLO_Destroy(void* handle)
    {
        if (handle)
            delete (OpenVinoRCNNHP*)handle;
    }

    void YOLO_Warmup(void* handle, int iters)
    {
        if (handle)
            ((OpenVinoRCNNHP*)handle)->Warmup(iters);
    }

    int YOLO_Detect(
        void* handle,
        const uint8_t* bgr, int w, int h, int step,
        float conf, float iou,bool Is3,
        RCNNBoxC* outBoxes,
        int maxBoxes)
    {
        if (!handle) return 0;

        cv::Mat img(h, w, CV_8UC3, (void*)bgr, step);

        std::vector<RCNNBox> result;
        ((OpenVinoRCNNHP*)handle)->Detect(img, conf, iou, Is3, result);

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
