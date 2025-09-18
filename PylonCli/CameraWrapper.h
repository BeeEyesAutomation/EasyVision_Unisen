﻿#pragma once
#include <pylon/PylonIncludes.h>

using namespace System;

namespace PylonCli {

    // Định dạng ảnh đầu ra
    public enum class OutputPixel : int { Auto = 0, Mono8 = 1, BGR8 = 3 };
    // Chế độ grab
    public enum class GrabMode : int { InternalLoop = 0, UserLoop = 1 };

    // Event zero-copy: trả con trỏ uchar* qua IntPtr
    public delegate void FrameReadyHandler(System::IntPtr buffer, int width, int height, int stride, int channels);

    public ref class Camera
    {
    private:
        static bool s_pylonInited;

        Pylon::CInstantCamera* _cam = nullptr;
        Pylon::CImageFormatConverter* _conv = nullptr;

        // Double-buffer (để con trỏ luôn hợp lệ đến frame kế tiếp)
        Pylon::CPylonImage* _bufA = nullptr;
        Pylon::CPylonImage* _bufB = nullptr;
        int _bufIndex = 0;

        // ImageEventHandler native (quản lý thủ công)
        void* _imgHandlerPtr = nullptr;

        bool _opened = false;
        GrabMode _mode = GrabMode::InternalLoop;

        OutputPixel _desiredOutput = OutputPixel::Auto;
        int _activeChannels = 3;           // 1 (Mono8) hoặc 3 (BGR8)
        System::String^ _lastError;

        // backing delegate cho custom event
        FrameReadyHandler^ _frameReadyHandlers = nullptr;

    public:
        Camera();
        ~Camera();
        !Camera();

        property System::String^ LastError { System::String^ get() { return _lastError; } }

        // Scan danh sách camera
        static cli::array<String^>^ List();

        // Lifecycle
        void Open(System::String^ name);
        void Close();
        void Start(GrabMode mode);
        void Start(); // mặc định InternalLoop
        void Stop();
        bool IsOpen();

        // Output pixel
        void        SetOutputPixel(OutputPixel fmt);
        OutputPixel GetOutputPixel();

        // ===== ROI =====
      // ROI
        int  SetWidth(int v);
        int  SetHeight(int v);
        int  SetOffsetX(int v);
        int  SetOffsetY(int v);

        // Params
        double SetExposure(double microseconds);
        double SetGain(double value);
        double SetBlackLevel(double value);
        void CenterX();    void CenterY();

        // ===== Params =====
        // ===== Query with min/max/step =====
        void GetExposure([System::Runtime::InteropServices::Out] float% min,
            [System::Runtime::InteropServices::Out] float% max,
            [System::Runtime::InteropServices::Out] float% step,
            [System::Runtime::InteropServices::Out] float% current);

        void GetGain([System::Runtime::InteropServices::Out] float% min,
            [System::Runtime::InteropServices::Out] float% max,
            [System::Runtime::InteropServices::Out] float% step,
            [System::Runtime::InteropServices::Out] float% current);

        void GetWidth([System::Runtime::InteropServices::Out] int% min,
            [System::Runtime::InteropServices::Out] int% max,
            [System::Runtime::InteropServices::Out] int% step,
            [System::Runtime::InteropServices::Out] int% current);

        void GetHeight([System::Runtime::InteropServices::Out] float% min,
            [System::Runtime::InteropServices::Out] float% max,
            [System::Runtime::InteropServices::Out] float% step,
            [System::Runtime::InteropServices::Out] float% current);
        void GetOffsetX([System::Runtime::InteropServices::Out] float% min,
            [System::Runtime::InteropServices::Out] float% max,
            [System::Runtime::InteropServices::Out] float% step,
            [System::Runtime::InteropServices::Out] float% current);

        void GetOffsetY([System::Runtime::InteropServices::Out] float% min,
            [System::Runtime::InteropServices::Out] float% max,
            [System::Runtime::InteropServices::Out] float% step,
            [System::Runtime::InteropServices::Out] float% current);
      
        void GetBlackLevel([System::Runtime::InteropServices::Out] float% min,
            [System::Runtime::InteropServices::Out] float% max,
            [System::Runtime::InteropServices::Out] float% step,
            [System::Runtime::InteropServices::Out] float% current);


        // ===== Event (uchar* qua IntPtr) =====
        event FrameReadyHandler^ FrameReady {
            void add(FrameReadyHandler^ d) { _frameReadyHandlers += d; }
            void remove(FrameReadyHandler^ d) { _frameReadyHandlers -= d; }
    protected:
        void raise(System::IntPtr buffer, int w, int h, int stride, int ch) {
            FrameReadyHandler^ tmp = _frameReadyHandlers;
            if (tmp != nullptr) tmp(buffer, w, h, stride, ch);
        }
        }

        // ===== UserLoop API (uchar* qua IntPtr) =====
        System::IntPtr GrabOneUcharPtr(int timeoutMs,
            [System::Runtime::InteropServices::Out] int% w,
            [System::Runtime::InteropServices::Out] int% h,
            [System::Runtime::InteropServices::Out] int% stride,
            [System::Runtime::InteropServices::Out] int% channels);

        System::IntPtr GrabLatestUcharPtr([System::Runtime::InteropServices::Out] int% w,
            [System::Runtime::InteropServices::Out] int% h,
            [System::Runtime::InteropServices::Out] int% stride,
            [System::Runtime::InteropServices::Out] int% channels);

        // Bridge cho handler nội bộ (InternalLoop)
        void ProcessGrabbed(const Pylon::CGrabResultPtr& ptr);

    private:
        // GenICam helpers
        long long GetIntNode(GenApi::INodeMap& nm, const char* name);
        void      SetIntNode(GenApi::INodeMap& nm, const char* name, long long v);
        double    GetFloatNode(GenApi::INodeMap& nm, const char* name);
        void      SetFloatNode(GenApi::INodeMap& nm, const char* name, double v);
        double    GetFloatAny(GenApi::INodeMap& nm, std::initializer_list<const char*> names);
        void      SetFloatAny(GenApi::INodeMap& nm, std::initializer_list<const char*> names, double v);
        void      TrySetEnum(GenApi::INodeMap& nm, const char* enumName, const char* entry);

        bool DetectIsColorSensor();
        void ConfigureConverterForOutput();

        // Double-buffer utils
        Pylon::CPylonImage* NextBuffer();
        Pylon::CPylonImage* CurrentBuffer();
    };
}
