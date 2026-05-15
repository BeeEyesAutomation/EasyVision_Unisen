using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using Point = System.Drawing.Point;

namespace BeeCore
{
    [Serializable]
    public class SegSample
    {
        public string ImagePath = "";
        public string MaskPath = "";
        public List<List<Point>> BrushStrokes = new List<List<Point>>();
        public List<int> BrushSizes = new List<int>();
        public List<bool> BrushIsDefect = new List<bool>();
        public List<List<Point>> Polygons = new List<List<Point>>();
        public List<bool> PolygonIsDefect = new List<bool>();
        public DateTime Created = DateTime.Now;
    }

    [Serializable]
    public class SegmentAI
    {
        public bool IsIni = false;
        public int Index = -1;
        public int IndexThread = 0;
        public int IndexCCD = 0;
        public RectRotate rotArea, rotCrop, rotMask, rotLimit;
        [NonSerialized] public RectRotate rotAreaAdjustment;
        [NonSerialized] public RectRotate rotMaskAdjustment;
        public RectRotate rotPositionAdjustment;
        public TypeCrop TypeCrop;
        public string pathModel = "";
        public string pathSamplesFolder = "";
        public List<SegSample> samples = new List<SegSample>();
        public int numTrees = 100;
        public int maxDepth = 12;
        public int minSampleCount = 10;
        public float defectThreshold = 0.5f;
        public int minDefectArea = 50;
        public bool enableGpu = true;
        [NonSerialized] public NativeSegAIInferer inferer;
        [NonSerialized] public byte[] lastMask;
        [NonSerialized] public int lastMaskW, lastMaskH;
        [NonSerialized] public float lastScore;
        [NonSerialized] public bool IsOK;
        [NonSerialized] public int Counter;
        [NonSerialized] public BackgroundWorker worker;
        public int MaxThread = 0;
        [field: NonSerialized] public event Action<int> PercentChange;
        [field: NonSerialized] public event Action ScoreChanged;
        [field: NonSerialized] public event Action StatusToolChanged;

        public object Clone()
        {
            var clone = (SegmentAI)MemberwiseClone();
            clone.samples = samples != null ? new List<SegSample>(samples) : new List<SegSample>();
            clone.inferer = null;
            clone.lastMask = null;
            return clone;
        }

        public void SetModel(bool isCopy = false)
        {
            EnsureDefaults();
            DisposeInferer();
            if (string.IsNullOrEmpty(pathModel) || !File.Exists(pathModel))
            {
                IsIni = false;
                return;
            }

            inferer = new NativeSegAIInferer();
            int rc = inferer.Load(pathModel);
            if (rc != 0)
            {
                DisposeInferer();
                IsIni = false;
                return;
            }

            inferer.SetGpu(enableGpu);
            IsIni = true;
            PropetyTool owner = Common.TryGetTool(IndexThread, Index);
            if (owner != null)
                owner.StatusTool = StatusTool.WaitCheck;
        }

        public Graphics DrawResult(Graphics gc)
        {
            if (gc == null)
                return gc;

            RectRotate rot = rotArea;
            if (Global.IsRun)
                rot = rotAreaAdjustment ?? rotArea;

            if (rot == null)
                return gc;

            Color resultColor = IsOK ? Global.ParaShow.ColorOK : Global.ParaShow.ColorNG;
            PropetyTool owner = Common.TryGetTool(IndexThread, Index);
            if (owner != null)
            {
                switch (owner.Results)
                {
                    case Results.OK:
                        resultColor = Global.ParaShow.ColorOK;
                        break;
                    case Results.NG:
                        resultColor = Global.ParaShow.ColorNG;
                        break;
                }
            }

            using (Font font = new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold))
            {
                if (Global.ParaShow.IsShowBox)
                {
                    string toolName = (Index + 1) + "." + (owner != null ? owner.Name : "SegmentAI");
                    Draws.Box1Label(gc, rot, toolName, font, Brushes.White, resultColor, Global.ParaShow.ThicknessLine);
                }
            }

            if (lastMask == null || lastMask.Length == 0 || lastMaskW <= 0 || lastMaskH <= 0)
                return gc;

            using (Mat overlay = BuildOverlayMask())
            {
                if (overlay == null || overlay.Empty())
                    return gc;

                Draws.DrawMatInRectRotate(
                    gc,
                    overlay,
                    rot,
                    Global.ScaleZoom * 100,
                    Global.pScroll,
                    resultColor,
                    Global.ParaShow.Opacity / 100.0f,
                    true);
            }

            gc.ResetTransform();
            return gc;
        }

        public void DoWork(RectRotate roiArea, RectRotate roiMask)
        {
            ResetResult();
            if (inferer == null || !inferer.IsOpened)
                SetModel();
            if (inferer == null || !inferer.IsOpened)
                return;
            if (Common.listCamera == null || IndexCCD < 0 || IndexCCD >= Common.listCamera.Count)
                return;

            Mat raw = Common.listCamera[IndexCCD].matRaw;
            if (raw == null || raw.Empty())
                return;

            using (Mat crop = Cropper.CropRotatedRect(raw, roiArea, roiMask))
            {
                Predict(crop, new OpenCvSharp.Rect(0, 0, crop.Width, crop.Height));
            }
        }

        public void DoWork(Mat bgr, OpenCvSharp.Rect roi)
        {
            if (inferer == null || !inferer.IsOpened)
                SetModel();
            Predict(bgr, roi);
        }

        public void Complete()
        {
            PropetyTool owner = Common.TryGetTool(IndexThread, Index);
            if (owner != null)
            {
                owner.ScoreResult = lastScore * 100.0f;
                owner.Results = IsOK ? Results.OK : Results.NG;
            }
            StatusToolChanged?.Invoke();
        }

        public bool Train(Action<int> onProgress, CancellationToken cancellationToken)
        {
            EnsureDefaults();
            if (samples == null || samples.Count == 0)
                return false;

            using (var trainer = new NativeSegAITrainer())
            {
                foreach (SegSample sample in samples)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return false;

                    using (Mat image = LoadSampleImage(sample))
                    using (Mat bgr = EnsureBgr(image))
                    using (Mat mask = LoadOrBuildMask(sample, bgr.Size()))
                    {
                        trainer.SetRoi(0, 0, bgr.Width, bgr.Height);
                        if (trainer.AddSample(bgr, mask) != 0)
                            return false;
                    }
                }

                int rc = trainer.Train(numTrees, maxDepth, minSampleCount, p =>
                {
                    PercentChange?.Invoke(p);
                    if (onProgress != null)
                        onProgress(p);
                });
                if (rc != 0)
                    return false;

                string folder = Path.GetDirectoryName(pathModel);
                if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                if (trainer.Save(pathModel, defectThreshold, (uint)Math.Max(1, minDefectArea)) != 0)
                    return false;
            }

            SetModel();
            return IsIni;
        }

        public string EnsureStoragePaths(int indexTool)
        {
            string folder = Path.Combine("Program", Global.Project ?? "", "SegAI_" + indexTool.ToString("D3"));
            pathSamplesFolder = Path.Combine(folder, "samples");
            pathModel = Path.Combine(folder, "model.segai");
            Directory.CreateDirectory(pathSamplesFolder);
            return folder;
        }

        public Mat BuildMaskFromAnnotations(SegSample sample, int width, int height)
        {
            var mask = new Mat(height, width, MatType.CV_8UC1, Scalar.All(0));
            if (sample == null)
                return mask;

            DrawBrushStrokes(mask, sample);
            DrawPolygons(mask, sample);
            return mask;
        }

        private void Predict(Mat input, OpenCvSharp.Rect roi)
        {
            if (inferer == null || !inferer.IsOpened || input == null || input.Empty())
                return;

            using (Mat bgr = EnsureBgr(input))
            {
                NativeSegAIInferer.PredictResult result = inferer.Predict(bgr, roi, defectThreshold);
                if (result == null)
                    return;

                lastMask = result.Mask;
                lastMaskW = result.Width;
                lastMaskH = result.Height;
                lastScore = result.Score;
                Counter = CountMaskPixels(lastMask);
                IsOK = lastScore < defectThreshold;
                ScoreChanged?.Invoke();
            }
        }

        private void ResetResult()
        {
            lastMask = null;
            lastMaskW = 0;
            lastMaskH = 0;
            lastScore = 0;
            Counter = 0;
            IsOK = false;
        }

        private void EnsureDefaults()
        {
            if (rotArea == null) rotArea = NewRect("Area Check", TypeCrop.Area);
            if (rotCrop == null) rotCrop = NewRect("Area Crop", TypeCrop.Crop);
            if (rotMask == null) rotMask = NewRect("Area Mask", TypeCrop.Mask);
            if (rotLimit == null) rotLimit = NewRect("Area Limit", TypeCrop.Limit);
            if (samples == null) samples = new List<SegSample>();
            if (string.IsNullOrEmpty(pathModel)) EnsureStoragePaths(Math.Max(0, Index));
        }

        private static RectRotate NewRect(string name, TypeCrop typeCrop)
        {
            var rect = new RectRotate();
            rect.Name = name;
            rect.TypeCrop = typeCrop;
            return rect;
        }

        private static Mat LoadSampleImage(SegSample sample)
        {
            if (sample == null || string.IsNullOrEmpty(sample.ImagePath) || !File.Exists(sample.ImagePath))
                throw new FileNotFoundException("SegmentAI sample image not found.", sample != null ? sample.ImagePath : "");

            Mat image = Cv2.ImRead(sample.ImagePath, ImreadModes.Color);
            if (image.Empty())
                throw new InvalidDataException("SegmentAI sample image cannot be decoded: " + sample.ImagePath);
            return image;
        }

        private Mat LoadOrBuildMask(SegSample sample, OpenCvSharp.Size size)
        {
            if (sample != null && !string.IsNullOrEmpty(sample.MaskPath) && File.Exists(sample.MaskPath))
            {
                Mat mask = Cv2.ImRead(sample.MaskPath, ImreadModes.Grayscale);
                if (!mask.Empty() && mask.Size() == size)
                    return NormalizeMask(mask);
                mask.Dispose();
            }

            return BuildMaskFromAnnotations(sample, size.Width, size.Height);
        }

        private static void DrawBrushStrokes(Mat mask, SegSample sample)
        {
            if (sample.BrushStrokes == null)
                return;

            for (int i = 0; i < sample.BrushStrokes.Count; ++i)
            {
                List<Point> stroke = sample.BrushStrokes[i];
                if (stroke == null || stroke.Count == 0)
                    continue;

                int brushSize = sample.BrushSizes != null && i < sample.BrushSizes.Count ? Math.Max(1, sample.BrushSizes[i]) : 8;
                byte label = sample.BrushIsDefect != null && i < sample.BrushIsDefect.Count && !sample.BrushIsDefect[i] ? (byte)2 : (byte)1;
                for (int p = 0; p < stroke.Count; ++p)
                {
                    var pt = new OpenCvSharp.Point(stroke[p].X, stroke[p].Y);
                    Cv2.Circle(mask, pt, brushSize, Scalar.All(label), -1);
                    if (p > 0)
                    {
                        var prev = new OpenCvSharp.Point(stroke[p - 1].X, stroke[p - 1].Y);
                        Cv2.Line(mask, prev, pt, Scalar.All(label), brushSize * 2);
                    }
                }
            }
        }

        private static void DrawPolygons(Mat mask, SegSample sample)
        {
            if (sample.Polygons == null)
                return;

            for (int i = 0; i < sample.Polygons.Count; ++i)
            {
                List<Point> polygon = sample.Polygons[i];
                if (polygon == null || polygon.Count < 3)
                    continue;

                byte label = sample.PolygonIsDefect != null && i < sample.PolygonIsDefect.Count && !sample.PolygonIsDefect[i] ? (byte)2 : (byte)1;
                var pts = new OpenCvSharp.Point[polygon.Count];
                for (int p = 0; p < polygon.Count; ++p)
                    pts[p] = new OpenCvSharp.Point(polygon[p].X, polygon[p].Y);
                Cv2.FillPoly(mask, new[] { pts }, Scalar.All(label));
            }
        }

        private static Mat NormalizeMask(Mat mask)
        {
            var normalized = new Mat(mask.Size(), MatType.CV_8UC1, Scalar.All(0));
            for (int y = 0; y < mask.Rows; ++y)
            {
                for (int x = 0; x < mask.Cols; ++x)
                {
                    byte value = mask.At<byte>(y, x);
                    if (value == 1 || value >= 200)
                        normalized.Set<byte>(y, x, 1);
                    else if (value == 2 || (value >= 100 && value < 200))
                        normalized.Set<byte>(y, x, 2);
                }
            }
            mask.Dispose();
            return normalized;
        }

        private static Mat EnsureBgr(Mat input)
        {
            if (input.Type() == MatType.CV_8UC3)
                return input.Clone();

            var bgr = new Mat();
            if (input.Channels() == 1)
                Cv2.CvtColor(input, bgr, ColorConversionCodes.GRAY2BGR);
            else if (input.Channels() == 4)
                Cv2.CvtColor(input, bgr, ColorConversionCodes.BGRA2BGR);
            else
                input.ConvertTo(bgr, MatType.CV_8UC3);
            return bgr;
        }

        private Mat BuildOverlayMask()
        {
            if (lastMask == null || lastMask.Length == 0 || lastMaskW <= 0 || lastMaskH <= 0)
                return null;

            int expected = lastMaskW * lastMaskH;
            if (lastMask.Length < expected)
                return null;

            Mat overlay = new Mat(lastMaskH, lastMaskW, MatType.CV_8UC1, Scalar.All(0));
            for (int y = 0; y < lastMaskH; ++y)
            {
                for (int x = 0; x < lastMaskW; ++x)
                {
                    if (lastMask[(y * lastMaskW) + x] != 0)
                        overlay.Set<byte>(y, x, 255);
                }
            }

            return overlay;
        }

        private static int CountMaskPixels(byte[] mask)
        {
            if (mask == null) return 0;
            int count = 0;
            for (int i = 0; i < mask.Length; ++i)
                if (mask[i] != 0) ++count;
            return count;
        }

        private void DisposeInferer()
        {
            if (inferer != null)
            {
                inferer.Dispose();
                inferer = null;
            }
        }
    }
}
