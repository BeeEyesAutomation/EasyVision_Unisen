#pragma once



namespace BeeCpp
{
    public value struct YoloBoxCli
    {
        float X1, Y1, X2, Y2;
        float Score;
        int ClassId;
    };

    /// <summary>
    /// C++/CLI wrapper for Native OpenVINO backend (BeeVision.NativeAI.dll)
    /// - Native side exports C-API: OV_Create / OV_Destroy / OV_Warmup / OV_Detect
    /// - This class holds an opaque native handle (void*)
    /// </summary>
    public ref class OpenVinoYoloCli sealed
    {
    public:
        OpenVinoYoloCli(System:: String^ xmlPath, int inputSize, int numClasses, int numThreads);
        ~OpenVinoYoloCli();        // IDisposable.Dispose
        !OpenVinoYoloCli();        // Finalizer (backup)

        void Warmup(int iters);

        // Returns new array each call (easy). If you want zero-GC 24/7, tell me -> DetectIntoBuffer version.
        array<YoloBoxCli>^ Detect(
            System::IntPtr bgrPtr, int w, int h, int step,
            float conf, float iou);

    private:
        void ReleaseNative();

    private:
        System::IntPtr _handle;  // void* from native
        bool _disposed;
    };
}
