#include "OpenVinoYoloCli.h"
#include <msclr/marshal_cppstd.h>

using namespace msclr::interop;

//
// Link with the import library produced by BeeVision.NativeAI (DLL project).
// Make sure BeeVision.NativeAI.lib is in Linker -> Input for BeeCpp
//
#pragma comment(lib, "BeeNativeOnnx.lib")

// ---- Native C-API imports (from BeeVision.NativeAI.dll) ----
#ifdef _WIN32
#define YAPI_IMPORT __declspec(dllimport)
#else
#define YAPI_IMPORT
#endif

extern "C"
{
    struct YoloBoxC
    {
        float x1, y1, x2, y2, score;
        int classId;
    };

    YAPI_IMPORT void* OV_Create(const wchar_t* xmlPath, int inputSize, int numClasses, int numThreads);
    YAPI_IMPORT void  OV_Destroy(void* handle);
    YAPI_IMPORT void  OV_Warmup(void* handle, int iters);

    YAPI_IMPORT int   OV_Detect(
        void* handle,
        const unsigned char* bgr, int w, int h, int step,
        float conf, float iou,
        YoloBoxC* outBoxes, int maxBoxes);
}

namespace BeeCpp
{
    OpenVinoYoloCli::OpenVinoYoloCli(System::String^ xmlPath, int inputSize, int numClasses, int numThreads)
        : _handle(System::IntPtr::Zero), _disposed(false)
    {
        std::wstring p = marshal_as<std::wstring>(xmlPath);
        void* h = OV_Create(p.c_str(), inputSize, numClasses, numThreads);
        _handle = System::IntPtr(h);
    }

    OpenVinoYoloCli::~OpenVinoYoloCli()
    {
        ReleaseNative();
        _disposed = true;
        System::GC::SuppressFinalize(this);
    }

    OpenVinoYoloCli::!OpenVinoYoloCli()
    {
        ReleaseNative();
    }

    void OpenVinoYoloCli::ReleaseNative()
    {
        if (_handle != System::IntPtr::Zero)
        {
            OV_Destroy(_handle.ToPointer());
            _handle = System::IntPtr::Zero;
        }
    }

    void OpenVinoYoloCli::Warmup(int iters)
    {
        if (_disposed) throw gcnew System::ObjectDisposedException("OpenVinoYoloCli");
        if (_handle == System::IntPtr::Zero) return;

        OV_Warmup(_handle.ToPointer(), iters);
    }

    array<YoloBoxCli>^ OpenVinoYoloCli::Detect(
        System::IntPtr bgrPtr, int w, int h, int step,
        float conf, float iou)
    {
        if (_disposed) throw gcnew System::ObjectDisposedException("OpenVinoYoloCli");
        if (_handle == System::IntPtr::Zero) return gcnew array<YoloBoxCli>(0);

        if (bgrPtr == System::IntPtr::Zero || w <= 0 || h <= 0 || step <= 0)
            return gcnew array<YoloBoxCli>(0);

        // Stack buffer (no heap). Increase if your scenes have many detections.
        const int MAX = 1024;
        YoloBoxC buf[MAX];

        int n = OV_Detect(
            _handle.ToPointer(),
            (const unsigned char*)bgrPtr.ToPointer(),
            w, h, step,
            conf, iou,
            buf, MAX);

        if (n <= 0) return gcnew array<YoloBoxCli>(0);
        if (n > MAX) n = MAX;

        auto arr = gcnew array<YoloBoxCli>(n);
        for (int i = 0; i < n; i++)
        {
            arr[i].X1 = buf[i].x1;
            arr[i].Y1 = buf[i].y1;
            arr[i].X2 = buf[i].x2;
            arr[i].Y2 = buf[i].y2;
            arr[i].Score = buf[i].score;
            arr[i].ClassId = buf[i].classId;
        }
        return arr;
    }
}
