using System;
using System.Drawing;
using System.Runtime.InteropServices;
using OpenCvSharp;

namespace BeeCore
{
    public static class OKNGAPI
    {
        private const string DllName = "OKNG";

        // Version
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_GetVersion();

        // Lifecycle
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr OKNG_Create();
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_Destroy(IntPtr h);

        // Parameters
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_SetCanny(IntPtr h, int t1, int t2);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_SetMatchThreshold(IntPtr h, float thr);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_SetUseOMP(IntPtr h, int flag);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_SetEdgeSpeckleMinArea(IntPtr h, int pixels);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_EnableMultiScaleCanny(IntPtr h, int enable);

        // Match modes & configs
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_SetMatchMode(IntPtr h, int mode); // 0=edge,1=intensity,2=orb
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_SetIntensityMultiScale(IntPtr h, int enable, float minScale, float maxScale, float stepScale);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_SetIntensityRotSearch(IntPtr h, int enable, float maxDeg, float stepDeg);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_SetORBParams(IntPtr h, int nFeatures, float scaleFactor, int nLevels);

        // Working resize
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_SetWorkingResize(IntPtr h, int enable, float scale);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_GetWorkingResize(IntPtr h, out int enable, out float scale);

        // OpenMP control & info
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_SetOMPThreadCount(IntPtr h, int threads);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_SetOMPDynamic(IntPtr h, int enable);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OKNG_GetOMPInfo(IntPtr h, out int enabled, out int maxThreads, out int numProcs);

        // Learn
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_LearnAutoFromFile(IntPtr h, string imagePath, int label);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_LearnAutoFromMemory(
            IntPtr h, IntPtr data, int width, int height, int step, int channels, int label);

        // Save/Load
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_SaveModels(IntPtr h, string yamlPath);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_LoadModels(IntPtr h, string yamlPath);

        // Remove
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_RemoveModel(IntPtr h, int modelId);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_RemoveLastOKModel(IntPtr h);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_RemoveLastNGModel(IntPtr h);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_RemoveAllByLabel(IntPtr h, int label);

        // Detect (NG→OK; threshold)
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_DetectPriorityFromFile(
            IntPtr h, string imagePath,
            out int outLabel, out float outScore, out int outModelId,
            out int outX, out int outY, out int outW, out int outH);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_DetectPriorityFromMemory(
            IntPtr h, IntPtr data, int width, int height, int step, int channels,
            out int outLabel, out float outScore, out int outModelId,
            out int outX, out int outY, out int outW, out int outH);

        // Nearest / Similarity
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_ClosestAnyFromFile(
            IntPtr h, string imagePath,
            out int outLabel, out float outScore, out int outModelId,
            out int outX, out int outY, out int outW, out int outH);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_ClosestAnyFromMemory(
            IntPtr h, IntPtr data, int width, int height, int step, int channels,
            out int outLabel, out float outScore, out int outModelId,
            out int outX, out int outY, out int outW, out int outH);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_BestPerLabelFromFile(
            IntPtr h, string imagePath,
            out int bestOKId, out float bestOKScore, out int bestNGId, out float bestNGScore);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_BestPerLabelFromMemory(
            IntPtr h, IntPtr data, int width, int height, int step, int channels,
            out int bestOKId, out float bestOKScore, out int bestNGId, out float bestNGScore);

        // Profile Detect
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_ProfileDetectFromFile(
            IntPtr h, string imagePath,
            out int outLabel, out float outScore, out int outModelId,
            out int outX, out int outY, out int outW, out int outH,
            out double outMillis, out int outThreadsUsed,
            out int outTriedNG, out int outPassedNG, out int outTriedOK, out int outPassedOK);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OKNG_ProfileDetectFromMemory(
            IntPtr h,
            IntPtr data, int width, int height, int step, int channels,
            out int outLabel, out float outScore, out int outModelId,
            out int outX, out int outY, out int outW, out int outH,
            out double outMillis, out int outThreadsUsed,
            out int outTriedNG, out int outPassedNG, out int outTriedOK, out int outPassedOK);

        // ===== Helpers: OpenCvSharp.Mat =====
        public static int LearnFromMat(IntPtr h, Mat mat, int label)
        {
            if (h == IntPtr.Zero) throw new ArgumentNullException(nameof(h));
            if (mat == null || mat.Empty()) throw new ArgumentException("Mat is empty");
            return OKNG_LearnAutoFromMemory(h, mat.Data, mat.Width, mat.Height, (int)mat.Step(), mat.Channels(), label);
        }

        public static bool DetectFromMat(IntPtr h, Mat mat,
            out int label, out float score, out int modelId,
            out int x, out int y, out int w, out int hgt)
        {
            if (h == IntPtr.Zero) throw new ArgumentNullException(nameof(h));
            if (mat == null || mat.Empty()) throw new ArgumentException("Mat is empty");
            int ok = OKNG_DetectPriorityFromMemory(
                h, mat.Data, mat.Width, mat.Height, (int)mat.Step(), mat.Channels(),
                out label, out score, out modelId, out x, out y, out w, out hgt);
            return ok != 0;
        }

        public static bool ClosestAnyFromMat(IntPtr h, Mat mat,
            out int label, out float score, out int modelId,
            out int x, out int y, out int w, out int hgt)
        {
            if (h == IntPtr.Zero) throw new ArgumentNullException(nameof(h));
            if (mat == null || mat.Empty()) throw new ArgumentException("Mat is empty");
            int ok = OKNG_ClosestAnyFromMemory(
                h, mat.Data, mat.Width, mat.Height, (int)mat.Step(), mat.Channels(),
                out label, out score, out modelId, out x, out y, out w, out hgt);
            return ok != 0;
        }

        public static bool BestPerLabelFromMat(IntPtr h, Mat mat,
            out int bestOKId, out float bestOKScore, out int bestNGId, out float bestNGScore)
        {
            if (h == IntPtr.Zero) throw new ArgumentNullException(nameof(h));
            if (mat == null || mat.Empty()) throw new ArgumentException("Mat is empty");
            int ok = OKNG_BestPerLabelFromMemory(
                h, mat.Data, mat.Width, mat.Height, (int)mat.Step(), mat.Channels(),
                out bestOKId, out bestOKScore, out bestNGId, out bestNGScore);
            return ok != 0;
        }

        public static bool ProfileDetectFromMat(IntPtr h, Mat mat,
            out int label, out float score, out int modelId,
            out int x, out int y, out int w, out int hgt,
            out double ms, out int threadsUsed,
            out int triedNG, out int passedNG, out int triedOK, out int passedOK)
        {
            if (h == IntPtr.Zero) throw new ArgumentNullException(nameof(h));
            if (mat == null || mat.Empty()) throw new ArgumentException("Mat is empty");
            int ok = OKNG_ProfileDetectFromMemory(
                h, mat.Data, mat.Width, mat.Height, (int)mat.Step(), mat.Channels(),
                out label, out score, out modelId, out x, out y, out w, out hgt,
                out ms, out threadsUsed, out triedNG, out passedNG, out triedOK, out passedOK);
            return ok != 0;
        }

        // ===== Helpers: System.Drawing.Bitmap (tuỳ chọn) =====
        public static int LearnFromBitmap(IntPtr h, Bitmap bmp, int label)
        {
            if (h == IntPtr.Zero) throw new ArgumentNullException(nameof(h));
            if (bmp == null) throw new ArgumentNullException(nameof(bmp));
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var data = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
            try
            {
                int channels = Image.GetPixelFormatSize(bmp.PixelFormat) / 8; // 1/3/4
                return OKNG_LearnAutoFromMemory(h, data.Scan0, bmp.Width, bmp.Height, data.Stride, channels, label);
            }
            finally { bmp.UnlockBits(data); }
        }

        public static bool DetectFromBitmap(IntPtr h, Bitmap bmp,
            out int label, out float score, out int modelId,
            out int x, out int y, out int w, out int hgt)
        {
            if (h == IntPtr.Zero) throw new ArgumentNullException(nameof(h));
            if (bmp == null) throw new ArgumentNullException(nameof(bmp));
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var data = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
            try
            {
                int channels = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;
                int ok = OKNG_DetectPriorityFromMemory(
                    h, data.Scan0, bmp.Width, bmp.Height, data.Stride, channels,
                    out label, out score, out modelId, out x, out y, out w, out hgt);
                return ok != 0;
            }
            finally { bmp.UnlockBits(data); }
        }
    }

    public sealed class OKNGHandle : IDisposable
    {
        public IntPtr Handle { get; private set; }
        public OKNGHandle()
        {
            Handle = OKNGAPI.OKNG_Create();
            if (Handle == IntPtr.Zero) throw new InvalidOperationException("OKNG_Create failed");
        }
        public void Dispose()
        {
            if (Handle != IntPtr.Zero)
            {
                OKNGAPI.OKNG_Destroy(Handle);
                Handle = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }
        ~OKNGHandle() => Dispose();

        // Shorthand config
        public void SetCanny(int t1, int t2) => OKNGAPI.OKNG_SetCanny(Handle, t1, t2);
        public void SetThreshold(float thr) => OKNGAPI.OKNG_SetMatchThreshold(Handle, thr);
        public void EnableOMP(bool on) => OKNGAPI.OKNG_SetUseOMP(Handle, on ? 1 : 0);
        public void SetSpeckleMinArea(int px) => OKNGAPI.OKNG_SetEdgeSpeckleMinArea(Handle, px);
        public void EnableMultiScaleEdge(bool on) => OKNGAPI.OKNG_EnableMultiScaleCanny(Handle, on ? 1 : 0);

        public void SetMatchMode(int mode /*0,1,2*/) => OKNGAPI.OKNG_SetMatchMode(Handle, mode);
        public void SetIntensityMultiScale(bool enable, float min, float max, float step)
            => OKNGAPI.OKNG_SetIntensityMultiScale(Handle, enable ? 1 : 0, min, max, step);
        public void SetIntensityRotation(bool enable, float maxDeg, float stepDeg)
            => OKNGAPI.OKNG_SetIntensityRotSearch(Handle, enable ? 1 : 0, maxDeg, stepDeg);
        public void SetORBParams(int nFeatures, float scaleFactor, int nLevels)
            => OKNGAPI.OKNG_SetORBParams(Handle, nFeatures, scaleFactor, nLevels);

        public void SetWorkingResize(bool enable, float scale)
            => OKNGAPI.OKNG_SetWorkingResize(Handle, enable ? 1 : 0, scale);
        public (bool enable, float scale) GetWorkingResize()
        {
            OKNGAPI.OKNG_GetWorkingResize(Handle, out var en, out var s);
            return (en != 0, s);
        }
    }
}
