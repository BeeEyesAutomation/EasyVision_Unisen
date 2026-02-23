#pragma once
#include <stdint.h>

#ifdef _WIN32
#define YAPI __declspec(dllexport)
#else
#define YAPI
#endif

struct YoloBoxC
{
    float x1, y1, x2, y2;
    float score;
    int classId;
};

extern "C"
{
    YAPI void* YOLO_Create(const wchar_t* modelPath, int inputSize, int numClasses, int numThreads);
    YAPI void  YOLO_Destroy(void* handle);
    YAPI void  YOLO_Warmup(void* handle, int iters);

    YAPI int YOLO_Detect(
        void* handle,
        const uint8_t* bgr, int w, int h, int step,

        float conf, float iou,
        YoloBoxC* outBoxes,
        int maxBoxes);
}
