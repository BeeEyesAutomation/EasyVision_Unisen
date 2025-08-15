#pragma once
#include <stdint.h>

#ifdef _WIN32
#ifdef OKNG_BUILD
#define OKNG_API extern "C" __declspec(dllexport)
#else
#define OKNG_API extern "C" __declspec(dllimport)
#endif
#else
#define OKNG_API extern "C"
#endif

typedef void* OKNGHandle;

// ===== Version =====
OKNG_API int OKNG_GetVersion(); // ví dụ 240 = v2.40

// ===== Lifecycle =====
OKNG_API OKNGHandle OKNG_Create();
OKNG_API void       OKNG_Destroy(OKNGHandle h);

// ===== Parameters =====
OKNG_API void OKNG_SetCanny(OKNGHandle h, int t1, int t2);      // EDGE: t1/t2<=0 => auto
OKNG_API void OKNG_SetMatchThreshold(OKNGHandle h, float thr);  // 0..1 (lọc cứng trong detect)
OKNG_API void OKNG_SetUseOMP(OKNGHandle h, int flag);           // 0/1

// Edge advanced
OKNG_API void OKNG_SetEdgeSpeckleMinArea(OKNGHandle h, int pixels); // 0=tắt
OKNG_API void OKNG_EnableMultiScaleCanny(OKNGHandle h, int enable); // 0/1

// Match mode: 0=EDGE, 1=INTENSITY, 2=ORB
OKNG_API void OKNG_SetMatchMode(OKNGHandle h, int mode);

// INTENSITY multi-scale + rotation search
OKNG_API void OKNG_SetIntensityMultiScale(OKNGHandle h, int enable, float minScale, float maxScale, float stepScale);
OKNG_API void OKNG_SetIntensityRotSearch(OKNGHandle h, int enable, float maxDeg, float stepDeg);

// ORB params
OKNG_API void OKNG_SetORBParams(OKNGHandle h, int nFeatures, float scaleFactor, int nLevels);

// ===== Working resize (tăng tốc) =====
OKNG_API void OKNG_SetWorkingResize(OKNGHandle h, int enable, float scale); // (0,1], ví dụ 0.5
OKNG_API void OKNG_GetWorkingResize(OKNGHandle h, int* outEnable, float* outScale);

// ===== OpenMP control & info =====
OKNG_API void OKNG_SetOMPThreadCount(OKNGHandle h, int threads); // 0=reset runtime default
OKNG_API void OKNG_SetOMPDynamic(OKNGHandle h, int enable);      // 0=off (khuyên)
OKNG_API void OKNG_GetOMPInfo(OKNGHandle h, int* outEnabled, int* outMaxThreads, int* outNumProcs);

// ===== Learn (auto id; label: +1=OK, -1=NG) =====
OKNG_API int OKNG_LearnAutoFromFile(OKNGHandle h, const char* imagePath, int label);
OKNG_API int OKNG_LearnAutoFromMemory(OKNGHandle h,
    const uint8_t* data, int width, int height, int step, int channels, int label);

// ===== Save / Load =====
OKNG_API int OKNG_SaveModels(OKNGHandle h, const char* yamlPath);
OKNG_API int OKNG_LoadModels(OKNGHandle h, const char* yamlPath);

// ===== Remove models =====
OKNG_API int OKNG_RemoveModel(OKNGHandle h, int modelId);
OKNG_API int OKNG_RemoveLastOKModel(OKNGHandle h);
OKNG_API int OKNG_RemoveLastNGModel(OKNGHandle h);
OKNG_API int OKNG_RemoveAllByLabel(OKNGHandle h, int label); // +1 OK, -1 NG; return count

// ===== Detect (ưu tiên NG -> OK; lọc theo threshold) =====
OKNG_API int OKNG_DetectPriorityFromFile(OKNGHandle h, const char* imagePath,
    int* outLabel, float* outScore, int* outModelId,
    int* outX, int* outY, int* outW, int* outH);

OKNG_API int OKNG_DetectPriorityFromMemory(OKNGHandle h,
    const uint8_t* data, int width, int height, int step, int channels,
    int* outLabel, float* outScore, int* outModelId,
    int* outX, int* outY, int* outW, int* outH);

// ===== Nearest / Similarity (KHÔNG dùng threshold) =====
OKNG_API int OKNG_ClosestAnyFromFile(OKNGHandle h, const char* imagePath,
    int* outLabel, float* outScore, int* outModelId,
    int* outX, int* outY, int* outW, int* outH);
OKNG_API int OKNG_ClosestAnyFromMemory(OKNGHandle h,
    const uint8_t* data, int width, int height, int step, int channels,
    int* outLabel, float* outScore, int* outModelId,
    int* outX, int* outY, int* outW, int* outH);

OKNG_API int OKNG_BestPerLabelFromFile(OKNGHandle h, const char* imagePath,
    int* outBestOKId, float* outBestOKScore, int* outBestNGId, float* outBestNGScore);
OKNG_API int OKNG_BestPerLabelFromMemory(OKNGHandle h,
    const uint8_t* data, int width, int height, int step, int channels,
    int* outBestOKId, float* outBestOKScore, int* outBestNGId, float* outBestNGScore);

// ===== Profile Detect (đo thời gian, luồng, tried/passed) =====
OKNG_API int OKNG_ProfileDetectFromFile(
    OKNGHandle h, const char* imagePath,
    int* outLabel, float* outScore, int* outModelId,
    int* outX, int* outY, int* outW, int* outH,
    double* outMillis, int* outThreadsUsed,
    int* outTriedNG, int* outPassedNG, int* outTriedOK, int* outPassedOK);

OKNG_API int OKNG_ProfileDetectFromMemory(
    OKNGHandle h,
    const uint8_t* data, int width, int height, int step, int channels,
    int* outLabel, float* outScore, int* outModelId,
    int* outX, int* outY, int* outW, int* outH,
    double* outMillis, int* outThreadsUsed,
    int* outTriedNG, int* outPassedNG, int* outTriedOK, int* outPassedOK);
