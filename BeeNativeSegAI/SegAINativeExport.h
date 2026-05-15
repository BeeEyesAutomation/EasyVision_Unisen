#pragma once

#include <stdint.h>

#ifdef BEESEGAI_EXPORTS
#define SAPI __declspec(dllexport)
#elif defined(_WIN32)
#define SAPI __declspec(dllimport)
#else
#define SAPI
#endif

extern "C"
{
    SAPI void* SEGAI_TrainerCreate();
    SAPI void SEGAI_TrainerDestroy(void* handle);
    SAPI int32_t SEGAI_TrainerSetROI(void* handle, int32_t x, int32_t y, int32_t w, int32_t h);
    SAPI int32_t SEGAI_TrainerAddSample(void* handle,
                                        const uint8_t* bgr,
                                        int32_t w,
                                        int32_t h,
                                        int32_t step,
                                        const uint8_t* mask,
                                        int32_t maskStep);
    SAPI void SEGAI_TrainerClearSamples(void* handle);
    SAPI int32_t SEGAI_TrainerTrain(void* handle,
                                    int32_t numTrees,
                                    int32_t maxDepth,
                                    int32_t minSampleCount,
                                    void(*progressCb)(int32_t, void*),
                                    void* userData,
                                    const volatile int32_t* cancelFlag);
    SAPI int32_t SEGAI_TrainerSave(void* handle,
                                   const wchar_t* path,
                                   float defectThreshold,
                                   uint32_t minDefectArea);
    SAPI int32_t SEGAI_TrainerSampleCount(void* handle, int32_t* outDefectPixels, int32_t* outNormalPixels);

    SAPI void* SEGAI_InferCreate();
    SAPI void SEGAI_InferDestroy(void* handle);
    SAPI int32_t SEGAI_InferLoad(void* handle, const wchar_t* path);
    SAPI int32_t SEGAI_InferSetGpu(void* handle, int32_t enable);
    SAPI int32_t SEGAI_InferGetGpuAvailable();
    SAPI int32_t SEGAI_InferPredict(void* handle,
                                    const uint8_t* bgr,
                                    int32_t w,
                                    int32_t h,
                                    int32_t step,
                                    int32_t roiX,
                                    int32_t roiY,
                                    int32_t roiW,
                                    int32_t roiH,
                                    float threshold,
                                    uint8_t** outMaskPtr,
                                    int32_t* outMaskW,
                                    int32_t* outMaskH,
                                    float* outScore);

    SAPI void SEGAI_FreeBuffer(void* ptr);

    SAPI int32_t SEGAI_GetVersion();
    SAPI const char* SEGAI_GetBuildInfo();
    SAPI int32_t SEGAI_RunFeatureSelfTest();
}
