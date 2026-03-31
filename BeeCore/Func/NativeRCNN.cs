using BeeGlobal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace BeeCore
{
    public class NativeRCNN : IDisposable
    {
        const string DLL = "BeeNativeRCNN.dll";

        // =============================
        // Native struct (phải match C++)
        // =============================
        [StructLayout(LayoutKind.Sequential)]
        public struct RCNNBox
        {
            public float x1, y1, x2, y2;
            public float score;
            public int classId;
        }

        // =============================
        // Convert sang RectRotate
        // =============================
        public static RectRotate RCNNBoxToRectRotate(RCNNBox b)
        {
            float w = b.x2 - b.x1;
            float h = b.y2 - b.y1;

            float cx = b.x1 + w * 0.5f;
            float cy = b.y1 + h * 0.5f;

            return new RectRotate(
                new RectangleF(-w / 2f, -h / 2f, w, h),
                new PointF(cx, cy),
                0f,
                AnchorPoint.None
            );
        }

        // =============================
        // DLL IMPORT (UPDATED)
        // =============================
        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern IntPtr YOLO_Create(
            string modelPath,
            int inputW,
            int inputH,
            int numClasses,
            int numThreads);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void YOLO_Destroy(IntPtr handle);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void YOLO_Warmup(IntPtr handle, int iters);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int YOLO_Detect(
            IntPtr handle,
            IntPtr bgr, int w, int h, int step,
            float conf, float iou, bool Is3,
            [Out] RCNNBox[] boxes,
            int maxBoxes);

        // =============================
        // Instance
        // =============================
        private IntPtr _handle;

        public bool IsOpened => _handle != IntPtr.Zero;

        // =============================
        // Constructor (UPDATED)
        // =============================
        public NativeRCNN(
            string modelPath,
            int inputW = 0,
            int inputH = 0,
            int numClasses = 0,
            int threads = 8)
        {
            _handle = YOLO_Create(modelPath, inputW, inputH, numClasses, threads);

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
        // Detect (low level - pointer)
        // =============================
        public int Detect(
            IntPtr bgr, int w, int h, int step,
            float conf, float iou, bool Is3,
            RCNNBox[] boxes)
        {
            if (_handle == IntPtr.Zero) return 0;

            return YOLO_Detect(_handle, bgr, w, h, step, conf, iou, Is3, boxes, boxes.Length);
        }

        // =============================
        // Detect (Bitmap tiện dụng)
        // =============================
        public List<RectRotate> Detect(Bitmap bmp, float conf = 0.25f, float iou = 0.45f, bool Is3 = false)
        {
            var rects = new List<RectRotate>();

            if (_handle == IntPtr.Zero || bmp == null)
                return rects;

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            var data = bmp.LockBits(rect,
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            try
            {
                int maxBoxes = 1000;
                RCNNBox[] boxes = new RCNNBox[maxBoxes];

                int n = YOLO_Detect(
                    _handle,
                    data.Scan0,
                    bmp.Width,
                    bmp.Height,
                    data.Stride,
                    conf,
                    iou,
                    Is3,
                    boxes,
                    maxBoxes);

                for (int i = 0; i < n; i++)
                {
                    rects.Add(RCNNBoxToRectRotate(boxes[i]));
                }
            }
            finally
            {
                bmp.UnlockBits(data);
            }

            return rects;
        }

        // =============================
        // Load class names từ YAML
        // =============================
        public Dictionary<int, string> LoadNames(string yamlPath)
        {
            var lines = File.ReadAllLines(yamlPath);
            var dict = new Dictionary<int, string>();

            bool inNames = false;

            foreach (var raw in lines)
            {
                var line = raw.Trim();

                if (line.Length == 0 || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("names:"))
                {
                    inNames = true;

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

                if (!raw.StartsWith(" ") && !raw.StartsWith("\t"))
                    break;

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
        // Dispose
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

        ~NativeRCNN()
        {
            Dispose();
        }
    }
}