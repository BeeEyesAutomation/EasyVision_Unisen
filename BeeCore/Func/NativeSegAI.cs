using OpenCvSharp;
using System;
using System.Runtime.InteropServices;

namespace BeeCore
{
    public sealed class NativeSegAITrainer : IDisposable
    {
        private const string DLL = "BeeNativeSegAI.dll";

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ProgressCallback(int progress, IntPtr userData);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SEGAI_TrainerCreate();

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SEGAI_TrainerDestroy(IntPtr handle);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SEGAI_TrainerSetROI(IntPtr handle, int x, int y, int w, int h);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SEGAI_TrainerAddSample(
            IntPtr handle,
            IntPtr bgr,
            int w,
            int h,
            int step,
            IntPtr mask,
            int maskStep);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SEGAI_TrainerClearSamples(IntPtr handle);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SEGAI_TrainerTrain(
            IntPtr handle,
            int numTrees,
            int maxDepth,
            int minSampleCount,
            ProgressCallback progressCb,
            IntPtr userData,
            IntPtr cancelFlag);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern int SEGAI_TrainerSave(
            IntPtr handle,
            string path,
            float defectThreshold,
            uint minDefectArea);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SEGAI_TrainerSampleCount(IntPtr handle, out int outDefectPixels, out int outNormalPixels);

        private IntPtr _handle;

        public bool IsOpened => _handle != IntPtr.Zero;

        public NativeSegAITrainer()
        {
            _handle = SEGAI_TrainerCreate();
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("SEGAI_TrainerCreate failed.");
        }

        public int SetRoi(int x, int y, int width, int height)
        {
            if (_handle == IntPtr.Zero) return -1;
            return SEGAI_TrainerSetROI(_handle, x, y, width, height);
        }

        public int AddSample(IntPtr bgr, int width, int height, int step, IntPtr mask, int maskStep)
        {
            if (_handle == IntPtr.Zero) return -1;
            return SEGAI_TrainerAddSample(_handle, bgr, width, height, step, mask, maskStep);
        }

        public int AddSample(Mat bgr, Mat mask)
        {
            if (!IsValidBgr(bgr) || !IsValidMask(mask) || bgr.Size() != mask.Size())
                return -2;

            return AddSample(
                bgr.Data,
                bgr.Width,
                bgr.Height,
                Step32(bgr),
                mask.Data,
                Step32(mask));
        }

        public void ClearSamples()
        {
            if (_handle != IntPtr.Zero)
                SEGAI_TrainerClearSamples(_handle);
        }

        public int Train(int numTrees, int maxDepth, int minSampleCount, Action<int> progress = null, IntPtr cancelFlag = default(IntPtr))
        {
            if (_handle == IntPtr.Zero) return -1;

            ProgressCallback callback = null;
            if (progress != null)
                callback = (p, _) => progress(p);

            return SEGAI_TrainerTrain(_handle, numTrees, maxDepth, minSampleCount, callback, IntPtr.Zero, cancelFlag);
        }

        public int Save(string path, float defectThreshold, uint minDefectArea)
        {
            if (_handle == IntPtr.Zero) return -1;
            if (string.IsNullOrEmpty(path)) return -2;
            return SEGAI_TrainerSave(_handle, path, defectThreshold, minDefectArea);
        }

        public int GetSampleCount(out int defectPixels, out int normalPixels)
        {
            defectPixels = 0;
            normalPixels = 0;
            if (_handle == IntPtr.Zero) return -1;
            return SEGAI_TrainerSampleCount(_handle, out defectPixels, out normalPixels);
        }

        public void Dispose()
        {
            if (_handle != IntPtr.Zero)
            {
                SEGAI_TrainerDestroy(_handle);
                _handle = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }

        ~NativeSegAITrainer()
        {
            Dispose();
        }

        private static bool IsValidBgr(Mat mat)
        {
            return mat != null && !mat.Empty() && mat.Channels() == 3;
        }

        private static bool IsValidMask(Mat mat)
        {
            return mat != null && !mat.Empty() && mat.Channels() == 1;
        }

        private static int Step32(Mat mat)
        {
            return checked((int)mat.Step());
        }
    }

    public sealed class NativeSegAIInferer : IDisposable
    {
        private const string DLL = "BeeNativeSegAI.dll";

        public sealed class PredictResult
        {
            public byte[] Mask { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public float Score { get; set; }
        }

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SEGAI_InferCreate();

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SEGAI_InferDestroy(IntPtr handle);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern int SEGAI_InferLoad(IntPtr handle, string path);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SEGAI_InferSetGpu(IntPtr handle, int enable);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SEGAI_InferGetGpuAvailable();

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SEGAI_InferPredict(
            IntPtr handle,
            IntPtr bgr,
            int w,
            int h,
            int step,
            int roiX,
            int roiY,
            int roiW,
            int roiH,
            float threshold,
            out IntPtr outMaskPtr,
            out int outMaskW,
            out int outMaskH,
            out float outScore);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SEGAI_FreeBuffer(IntPtr ptr);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SEGAI_RunFeatureSelfTest();

        private IntPtr _handle;

        public bool IsOpened => _handle != IntPtr.Zero;

        public static bool IsGpuAvailable => SEGAI_InferGetGpuAvailable() != 0;

        public NativeSegAIInferer()
        {
            _handle = SEGAI_InferCreate();
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("SEGAI_InferCreate failed.");
        }

        public NativeSegAIInferer(string modelPath)
            : this()
        {
            int rc = Load(modelPath);
            if (rc != 0)
                throw new InvalidOperationException("SEGAI_InferLoad failed: " + rc);
        }

        public int Load(string path)
        {
            if (_handle == IntPtr.Zero) return -1;
            if (string.IsNullOrEmpty(path)) return -2;
            return SEGAI_InferLoad(_handle, path);
        }

        public int SetGpu(bool enable)
        {
            if (_handle == IntPtr.Zero) return -1;
            return SEGAI_InferSetGpu(_handle, enable ? 1 : 0);
        }

        public PredictResult Predict(IntPtr bgr, int width, int height, int step, Rect roi, float threshold)
        {
            if (_handle == IntPtr.Zero) return null;

            IntPtr maskPtr = IntPtr.Zero;
            int maskW = 0;
            int maskH = 0;
            float score = 0;

            int rc = SEGAI_InferPredict(
                _handle,
                bgr,
                width,
                height,
                step,
                roi.X,
                roi.Y,
                roi.Width,
                roi.Height,
                threshold,
                out maskPtr,
                out maskW,
                out maskH,
                out score);

            if (rc != 0 || maskPtr == IntPtr.Zero || maskW <= 0 || maskH <= 0)
                return null;

            try
            {
                var mask = new byte[checked(maskW * maskH)];
                Marshal.Copy(maskPtr, mask, 0, mask.Length);
                return new PredictResult
                {
                    Mask = mask,
                    Width = maskW,
                    Height = maskH,
                    Score = score
                };
            }
            finally
            {
                SEGAI_FreeBuffer(maskPtr);
            }
        }

        public PredictResult Predict(Mat bgr, Rect roi, float threshold)
        {
            if (!IsValidBgr(bgr))
                return null;

            return Predict(bgr.Data, bgr.Width, bgr.Height, Step32(bgr), roi, threshold);
        }

        public void Dispose()
        {
            if (_handle != IntPtr.Zero)
            {
                SEGAI_InferDestroy(_handle);
                _handle = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }

        ~NativeSegAIInferer()
        {
            Dispose();
        }

        private static bool IsValidBgr(Mat mat)
        {
            return mat != null && !mat.Empty() && mat.Channels() == 3;
        }

        private static int Step32(Mat mat)
        {
            return checked((int)mat.Step());
        }
    }
}
