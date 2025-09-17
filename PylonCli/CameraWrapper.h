#pragma once
#include <pylon/PylonIncludes.h>

using namespace System;
using namespace System::Runtime::InteropServices; // cho [Out]

namespace PylonCli {

    // Loại output pixel
    public enum class OutputPixel : int
    {
        Auto = 0, // Tự phát hiện (mono → Mono8, color → BGR8)
        Mono8 = 1,
        BGR8 = 3
    };

    public ref class Camera
    {
    private:
        static bool s_pylonInited = false; // init Pylon 1 lần
        Pylon::CInstantCamera* _cam = nullptr;
        Pylon::CImageFormatConverter* _conv = nullptr;
        bool _opened = false;

        System::String^ _lastError;
        OutputPixel _desiredOutput = OutputPixel::Auto;
        int _activeChannels = 3; // 1 hoặc 3

    public:
        Camera();
        ~Camera();
        !Camera();

        property System::String^ LastError { System::String^ get() { return _lastError; } }
        void ClearError() { _lastError = nullptr; }

        // Scan danh sách camera
        static cli::array<String^>^ List();

        // Kết nối
        void Open(System::String^ name);
        void Close();
        void Start();
        void Stop();
        bool IsOpen();

        // Output pixel
        void        SetOutputPixel(OutputPixel fmt);
        OutputPixel GetOutputPixel();

        // ROI
        int  GetWidth();   void SetWidth(int v);
        int  GetHeight();  void SetHeight(int v);
        int  GetOffsetX(); void SetOffsetX(int v);
        int  GetOffsetY(); void SetOffsetY(int v);

        int  GetCenterX(); void SetCenterX(int cx);
        int  GetCenterY(); void SetCenterY(int cy);
        void CenterX();    void CenterY();

        // Params
        double GetExposure();   void SetExposure(double microseconds);
        double GetGain();       void SetGain(double value);
        double GetBlackLevel(); void SetBlackLevel(double value);

        // Grab ảnh → IntPtr + type info
        System::IntPtr GrabFramePtrEx([Out] int% w, [Out] int% h, [Out] int% stride, [Out] int% channels);

    private:
        // Helpers GenICam
        void   SetIntNode(GenApi::INodeMap& nm, const char* name, long long v);
        long long GetIntNode(GenApi::INodeMap& nm, const char* name);
        void   SetFloatNode(GenApi::INodeMap& nm, const char* name, double v);
        double GetFloatNode(GenApi::INodeMap& nm, const char* name);
        double GetFloatAny(GenApi::INodeMap& nm, std::initializer_list<const char*> names);
        void   SetFloatAny(GenApi::INodeMap& nm, std::initializer_list<const char*> names, double v);
        void   TrySetEnum(GenApi::INodeMap& nm, const char* enumName, const char* entry);

        // Output pixel config
        bool   DetectIsColorSensor();
        void   ConfigureConverterForOutput();
    };
}
