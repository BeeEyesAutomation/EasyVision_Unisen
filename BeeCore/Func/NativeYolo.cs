using BeeGlobal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
namespace BeeCore
{
    public class NativeYolo : IDisposable
    {
        const string DLL = "BeeNativeOnnx.dll";

        // =============================
        // Native struct
        // =============================
        [StructLayout(LayoutKind.Sequential)]
        public struct YoloBox
        {
            public float x1, y1, x2, y2;
            public float score;
            public int classId;
        }
        public static RectRotate YoloBoxToRectRotate(YoloBox b)
        {
            float w = b.x2 - b.x1;
            float h = b.y2 - b.y1;

            float cx = b.x1 + w * 0.5f;
            float cy = b.y1 + h * 0.5f;
            
            // RectRotate thường lưu center + width/height + rotation(deg)
            return new RectRotate(
             new RectangleF(-w/2f,-h/2f,w,h),
               new PointF(cx, cy),
               0f,AnchorPoint.None
            );
        }
        // =============================
        // DllImport (PHẢI static)
        // =============================
        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern IntPtr YOLO_Create(
            string modelPath, int inputSize, int numClasses, int numThreads);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void YOLO_Destroy(IntPtr handle);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void YOLO_Warmup(IntPtr handle, int iters);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int YOLO_Detect(
            IntPtr handle,
            IntPtr bgr, int w, int h, int step,
            float conf, float iou,
            [Out] YoloBox[] boxes,
            int maxBoxes);

        // =============================
        // Instance
        // =============================
        private IntPtr _handle;

        public bool IsOpened => _handle != IntPtr.Zero;

        // =============================
        // Constructor
        // =============================
        public NativeYolo(string modelPath, int inputSize = 0, int numClasses = 0, int threads = 8)
        {
            _handle = YOLO_Create(modelPath, inputSize, numClasses, threads);

            if (_handle == IntPtr.Zero)
                throw new Exception("YOLO_Create failed");
        }

        // =============================
        // Warmup
        // =============================
        public void Warmup(int iters = 10)
        {
            if (_handle != IntPtr.Zero)
                YOLO_Warmup(_handle, iters);
        }

        // =============================
        // Detect
        // =============================
        public int Detect(
            IntPtr bgr, int w, int h, int step,
            float conf, float iou,
            YoloBox[] boxes)
        {
            if (_handle == IntPtr.Zero) return 0;

            return YOLO_Detect(_handle, bgr, w, h, step, conf, iou, boxes, boxes.Length);
        }
        public  Dictionary<int, string> LoadNames(string yamlPath)
        {
            var lines = File.ReadAllLines(yamlPath);
            var dict = new Dictionary<int, string>();

            bool inNames = false;
            foreach (var raw in lines)
            {
                var line = raw.Trim();
                if (line.Length == 0 || line.StartsWith("#")) continue;

                if (line.StartsWith("names:"))
                {
                    inNames = true;

                    // names: [a, b, c]
                    var mList = Regex.Match(line, @"^names:\s*\[(.*)\]\s*$");
                    if (mList.Success)
                    {
                        var parts = mList.Groups[1].Value.Split(',');
                        for (int i = 0; i < parts.Length; i++)
                            dict[i] = parts[i].Trim().Trim('\'', '"');
                        return dict;
                    }
                    continue;
                }

                if (!inNames) continue;

                // hết block names khi gặp key khác (không thụt dòng)
                if (!raw.StartsWith(" ") && !raw.StartsWith("\t")) break;

                // "0: screw"
                var m = Regex.Match(line, @"^(\d+)\s*:\s*(.+)\s*$");
                if (m.Success)
                {
                    int id = int.Parse(m.Groups[1].Value);
                    string name = m.Groups[2].Value.Trim().Trim('\'', '"');
                    dict[id] = name;
                }
            }

            return dict;
        }
        // =============================
        // Destroy
        // =============================
        public void Dispose()
        {
            if (_handle != IntPtr.Zero)
            {
                YOLO_Destroy(_handle);
                _handle = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }

        ~NativeYolo()
        {
            Dispose();
        }
    }

}