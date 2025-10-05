using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using SD = System.Drawing; // alias: dùng SD.PointF nếu cần đọc dữ liệu từ RectRotate

namespace BeeCore
{
    public static class Cropper
    {
        public static Mat CropRotatedRect(
            Mat source, RectRotate rot, RectRotate rotMask,
            bool returnMaskOnly = false)
        {
            if (source == null || source.Empty()) return new Mat();

            // ================== 1) Xác định tâm neo & kích thước theo shape ==================
            // Với Polygon: tâm neo = PosCenter + Rotate(bboxLocalCenter, rectRotation)
            //              size = kích thước bboxLocal của polygon
            // Các shape khác: tâm neo = PosCenter, size = _rect.(W,H)
            Size2f rectSize;
            SD.PointF localCenterForShape; // (0,0) cho non-Polygon; (cx,cy) cho Polygon
            GetAnchorSizeFor(rot, out SD.PointF worldAnchor, out rectSize, out localCenterForShape);

            // ================== 2) Chuẩn hoá góc/size crop (như nguyên tác) ==================
            double angleUsed = rot._rectRotation; // xoay ảnh quanh worldAnchor
            if (angleUsed < -45.0)
            {
                angleUsed += 90.0;
                float tmp = rectSize.Width; rectSize.Width = rectSize.Height; rectSize.Height = tmp;
            }

            // Góc DƯ trong hệ patch
            float angleInPatchCrop = (float)(rot._rectRotation - angleUsed);
            if (angleInPatchCrop > 180f) angleInPatchCrop -= 360f;
            if (angleInPatchCrop < -180f) angleInPatchCrop += 360f;

            Point2f anchor = new Point2f(worldAnchor.X, worldAnchor.Y);

            Mat M = null, warped = null, patch = null, cropMask = null, mask2 = null, finalMask = null, result = null, bgMat = null;

            try
            {
                // 3) Warp ảnh quanh anchor (giữ nguyên như cũ)
                M = Cv2.GetRotationMatrix2D(anchor, angleUsed, 1.0);
                warped = new Mat();
                Cv2.WarpAffine(source, warped, M, source.Size(), InterpolationFlags.Cubic, BorderTypes.Constant, new Scalar(0, 0, 0));

                // 4) Cắt patch theo size của shape (Polygon dùng size bboxLocal)
                patch = new Mat();
                Cv2.GetRectSubPix(warped, new OpenCvSharp.Size(rectSize.Width, rectSize.Height), anchor, patch);
                if (patch.Empty()) return new Mat();

                int patchH = patch.Rows;
                int patchW = patch.Cols;
                Point2f patchCenter = new Point2f(patchW * 0.5f, patchH * 0.5f);

                // 5) Mask crop 8UC1
                cropMask = new Mat(patchH, patchW, MatType.CV_8UC1, new Scalar(0));

                // >>> VẼ MASK với góc DƯ & kích thước khớp patch
                //     Với Polygon: sẽ tự trừ localCenterForShape bên trong để mask nằm giữa patch
                DrawShapeMaskIntoWithSize(rot, cropMask, patchCenter, angleInPatchCrop, 255,
                                          (int)Math.Round(rectSize.Width), (int)Math.Round(rectSize.Height),
                                          localCenterForShape);

                // 6) Mask loại trừ (nếu có)
                if (rotMask != null)
                {
                    mask2 = new Mat(patchH, patchW, MatType.CV_8UC1, new Scalar(255));

                    // ---- Tính anchor riêng cho mask (Polygon thì dùng bbox local center) ----
                    Size2f maskSize;
                    SD.PointF maskLocalCenter;
                    GetAnchorSizeFor(rotMask, out SD.PointF worldAnchorMask, out maskSize, out maskLocalCenter);

                    // Delta giữa 2 tâm neo (trong world), đưa vào hệ patch
                    Point2f deltaWorld = new Point2f(
                        (float)(worldAnchorMask.X - worldAnchor.X),
                        (float)(worldAnchorMask.Y - worldAnchor.Y)
                    );
                    Point2f deltaInPatch = RotatePoint(deltaWorld, (float)(-angleUsed));
                    Point2f maskCenterInPatch = new Point2f(patchCenter.X + deltaInPatch.X,
                                                            patchCenter.Y + deltaInPatch.Y);
                    float maskAngleInPatch = (float)(rotMask._rectRotation - angleUsed);

                    // Vẽ mask phụ đúng kích thước & tâm neo của chính nó
                    DrawShapeMaskIntoWithSize(rotMask, mask2, maskCenterInPatch, maskAngleInPatch, 0,
                                              (int)Math.Round(maskSize.Width),
                                              (int)Math.Round(maskSize.Height),
                                              maskLocalCenter);

                    finalMask = new Mat();
                    Cv2.BitwiseAnd(cropMask, mask2, finalMask);
                }
                else
                {
                    finalMask = cropMask.Clone();
                }

                if (returnMaskOnly)
                    return finalMask.Clone();

                // 7) Áp mask lên nền
                Scalar bg = rot.IsWhite ? new Scalar(255, 255, 255, 255) : new Scalar(0, 0, 0, 0);
                bgMat = new Mat(patch.Size(), patch.Type(), bg);
                result = bgMat.Clone();
                patch.CopyTo(result, finalMask);
                return result;
            }
            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Crop", ex.Message));
                return new Mat();
            }
            finally
            {
                if (M != null) M.Dispose();
                if (warped != null) warped.Dispose();
                if (patch != null) patch.Dispose();
                if (cropMask != null) cropMask.Dispose();
                if (mask2 != null) mask2.Dispose();
                if (finalMask != null && result == null) finalMask.Dispose();
                if (bgMat != null) bgMat.Dispose();
            }
        }

        // ====================== Helpers cho neo/size theo shape ======================
        private static void GetAnchorSizeFor(RectRotate rr,
            out SD.PointF worldAnchor, out Size2f size, out SD.PointF localCenterForShape)
        {
            if (rr.Shape == ShapeType.Polygon && rr.PolyLocalPoints != null && rr.PolyLocalPoints.Count >= 3)
            {
                GetPolygonBounds(rr.PolyLocalPoints, out float minX, out float minY, out float maxX, out float maxY);
                float w = Math.Max(1f, maxX - minX);
                float h = Math.Max(1f, maxY - minY);
                float cx = (minX + maxX) * 0.5f;
                float cy = (minY + maxY) * 0.5f;

                // Tâm neo world = PosCenter + Rotate((cx,cy), rectRotation)
                SD.PointF delta = RectRotate.Rotate(new SD.PointF(cx, cy), rr._rectRotation);
                worldAnchor = new SD.PointF(rr._PosCenter.X + delta.X, rr._PosCenter.Y + delta.Y);

                size = new Size2f(w, h);
                localCenterForShape = new SD.PointF(cx, cy); // để trừ ra khi vẽ polygon vào patch
            }
            else
            {
                // Non-Polygon: dùng _rect và _PosCenter như cũ
                worldAnchor = rr._PosCenter;
                size = new Size2f(rr._rect.Width, rr._rect.Height);
                localCenterForShape = new SD.PointF(0, 0);
            }
        }

        private static void GetPolygonBounds(IList<SD.PointF> pts, out float minX, out float minY, out float maxX, out float maxY)
        {
            minX = float.MaxValue; minY = float.MaxValue;
            maxX = float.MinValue; maxY = float.MinValue;
            int n = pts.Count;
            int max = n;
            if (n >= 2 && pts[0].Equals(pts[n - 1])) max = n - 1; // nếu đã đóng, bỏ điểm cuối

            for (int i = 0; i < max; i++)
            {
                var p = pts[i];
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }
            if (max == 0) { minX = minY = 0; maxX = maxY = 1; }
        }

        // ====================== Vẽ mask vào patch (có hỗ trợ tâm local riêng cho polygon) ======================
        private static void DrawShapeMaskIntoWithSize(
            RectRotate rr, Mat mask, Point2f centerInMask, float angleInPatch, byte fillValue,
            int W, int H, SD.PointF localCenterForShape)
        {
            if (W <= 0 || H <= 0) return;

            switch (rr.Shape)
            {
                case ShapeType.Rectangle:
                    if (Math.Abs(angleInPatch) < 1e-6f)
                    {
                        int tlx = (int)Math.Round(centerInMask.X - W * 0.5f);
                        int tly = (int)Math.Round(centerInMask.Y - H * 0.5f);
                        Cv2.Rectangle(mask, new Rect(tlx, tly, W, H), new Scalar(fillValue), -1);
                    }
                    else
                    {
                        var ptsRect = GetAxisAlignedRectCorners(centerInMask, W, H);
                        for (int i = 0; i < ptsRect.Length; i++)
                            ptsRect[i] = RotateAround(ptsRect[i], centerInMask, angleInPatch);
                        Cv2.FillPoly(mask, new[] { ptsRect }, new Scalar(fillValue));
                    }
                    break;

                case ShapeType.Ellipse:
                    {
                        var rrBox = new RotatedRect(centerInMask, new Size2f(W, H), angleInPatch);
                        Cv2.Ellipse(mask, rrBox, new Scalar(fillValue), -1);
                        break;
                    }

                case ShapeType.Hexagon:
                    {
                        // Hex đã nằm quanh gốc (0,0) theo _rect, không cần trừ localCenter
                        var mi = rr.GetType().GetMethod("GetHexagonVerticesLocal");
                        SD.PointF[] pf = mi != null ? (SD.PointF[])mi.Invoke(rr, null) : null;
                        if (pf != null && pf.Length >= 3)
                        {
                            var list = new List<OpenCvSharp.Point>(pf.Length);
                            for (int i = 0; i < pf.Length; i++)
                            {
                                var p = new Point2f(pf[i].X, pf[i].Y);
                                if (Math.Abs(angleInPatch) > 1e-6f)
                                    p = RotatePoint(p, angleInPatch);
                                p = new Point2f(p.X + centerInMask.X, p.Y + centerInMask.Y);
                                list.Add(new OpenCvSharp.Point((int)Math.Round(p.X), (int)Math.Round(p.Y)));
                            }
                            Cv2.FillPoly(mask, new[] { list.ToArray() }, new Scalar(fillValue));
                        }
                        break;
                    }

                case ShapeType.Polygon:
                    {
                        var listF = rr.PolyLocalPoints;
                        if (listF != null && listF.Count >= 3)
                        {
                            // Trừ tâm local của polygon để polygon nằm giữa patch
                            float cx = localCenterForShape.X;
                            float cy = localCenterForShape.Y;

                            int n = listF.Count;
                            // Nếu polygon đã đóng, có thể giữ nguyên – FillPoly chấp nhận lặp điểm đầu/cuối
                            var arr = new OpenCvSharp.Point[n];
                            for (int i = 0; i < n; i++)
                            {
                                var p = new Point2f(listF[i].X - cx, listF[i].Y - cy);
                                if (Math.Abs(angleInPatch) > 1e-6f)
                                    p = RotatePoint(p, angleInPatch);
                                p = new Point2f(p.X + centerInMask.X, p.Y + centerInMask.Y);
                                arr[i] = new OpenCvSharp.Point((int)Math.Round(p.X), (int)Math.Round(p.Y));
                            }
                            Cv2.FillPoly(mask, new[] { arr }, new Scalar(fillValue));
                        }
                        break;
                    }

                default:
                    {
                        int tlx = (int)Math.Round(centerInMask.X - W * 0.5f);
                        int tly = (int)Math.Round(centerInMask.Y - H * 0.5f);
                        Cv2.Rectangle(mask, new Rect(tlx, tly, W, H), new Scalar(fillValue), -1);
                        break;
                    }
            }
        }

        // ===== Helpers gốc =====

        private static Point2f RotatePoint(Point2f p, float degree)
        {
            double rad = degree * Math.PI / 180.0;
            double c = Math.Cos(rad), s = Math.Sin(rad);
            return new Point2f(
                (float)(p.X * c - p.Y * s),
                (float)(p.X * s + p.Y * c)
            );
        }

        private static OpenCvSharp.Point RotateAround(OpenCvSharp.Point p, Point2f center, float degree)
        {
            Point2f pf = new Point2f(p.X - center.X, p.Y - center.Y);
            pf = RotatePoint(pf, degree);
            return new OpenCvSharp.Point(
                (int)Math.Round(pf.X + center.X),
                (int)Math.Round(pf.Y + center.Y)
            );
        }

        private static void DrawShapeMaskInto(
            RectRotate rr, Mat mask, Point2f centerInMask, float angleInPatch, byte fillValue)
        {
            int W = (int)Math.Round(rr._rect.Width);
            int H = (int)Math.Round(rr._rect.Height);
            if (W <= 0 || H <= 0) return;

            switch (rr.Shape)
            {
                case ShapeType.Rectangle:
                    if (Math.Abs(angleInPatch) < 1e-6f)
                    {
                        int tlx = (int)Math.Round(centerInMask.X - W * 0.5f);
                        int tly = (int)Math.Round(centerInMask.Y - H * 0.5f);
                        Rect r = new Rect(tlx, tly, W, H);
                        Cv2.Rectangle(mask, r, new Scalar(fillValue), thickness: -1);
                    }
                    else
                    {
                        OpenCvSharp.Point[] ptsRect = GetAxisAlignedRectCorners(centerInMask, W, H);
                        for (int i = 0; i < ptsRect.Length; i++)
                            ptsRect[i] = RotateAround(ptsRect[i], centerInMask, angleInPatch);
                        Cv2.FillPoly(mask, new[] { ptsRect }, new Scalar(fillValue));
                    }
                    break;

                case ShapeType.Ellipse:
                    {
                        var rrBox = new RotatedRect(centerInMask, new Size2f(W, H), angleInPatch);
                        Cv2.Ellipse(mask, rrBox, new Scalar(fillValue), -1);
                        break;
                    }

                case ShapeType.Hexagon:
                    {
                        OpenCvSharp.Point[] poly = TryGetPolygonFromHex(rr, centerInMask, angleInPatch, W, H);
                        if (poly != null && poly.Length >= 3)
                            Cv2.FillPoly(mask, new[] { poly }, new Scalar(fillValue));
                        break;
                    }

                case ShapeType.Polygon:
                    {
                        IList<SD.PointF> list = rr.PolyLocalPoints;
                        if (list != null && list.Count >= 3)
                        {
                            OpenCvSharp.Point[] arr = new OpenCvSharp.Point[list.Count];
                            for (int i = 0; i < list.Count; i++)
                            {
                                Point2f p = new Point2f(list[i].X, list[i].Y);
                                if (Math.Abs(angleInPatch) > 1e-6f)
                                    p = RotatePoint(p, angleInPatch);
                                p = new Point2f(p.X + centerInMask.X, p.Y + centerInMask.Y);
                                arr[i] = new OpenCvSharp.Point((int)Math.Round(p.X), (int)Math.Round(p.Y));
                            }
                            Cv2.FillPoly(mask, new[] { arr }, new Scalar(fillValue));
                        }
                        break;
                    }

                default:
                    {
                        int tlx = (int)Math.Round(centerInMask.X - W * 0.5f);
                        int tly = (int)Math.Round(centerInMask.Y - H * 0.5f);
                        Rect r = new Rect(tlx, tly, W, H);
                        Cv2.Rectangle(mask, r, new Scalar(fillValue), thickness: -1);
                        break;
                    }
            }
        }

        private static OpenCvSharp.Point[] GetAxisAlignedRectCorners(Point2f center, int W, int H)
        {
            float hx = W * 0.5f, hy = H * 0.5f;
            return new[]
            {
                new OpenCvSharp.Point((int)Math.Round(center.X - hx), (int)Math.Round(center.Y - hy)),
                new OpenCvSharp.Point((int)Math.Round(center.X + hx), (int)Math.Round(center.Y - hy)),
                new OpenCvSharp.Point((int)Math.Round(center.X + hx), (int)Math.Round(center.Y + hy)),
                new OpenCvSharp.Point((int)Math.Round(center.X - hx), (int)Math.Round(center.Y + hy)),
            };
        }

        private static OpenCvSharp.Point[] TryGetPolygonFromHex(RectRotate rr, Point2f centerInMask, float angleInPatch, int W, int H)
        {
            try
            {
                var mi = rr.GetType().GetMethod("GetHexagonVerticesLocal");
                if (mi != null)
                {
                    object ptsObj = mi.Invoke(rr, null);
                    var pf = ptsObj as SD.PointF[];
                    if (pf != null && pf.Length >= 3)
                    {
                        var list = new List<OpenCvSharp.Point>(pf.Length);
                        for (int i = 0; i < pf.Length; i++)
                        {
                            Point2f p = new Point2f(pf[i].X, pf[i].Y);
                            if (Math.Abs(angleInPatch) > 1e-6f)
                                p = RotatePoint(p, angleInPatch);
                            p = new Point2f(p.X + centerInMask.X, p.Y + centerInMask.Y);
                            list.Add(new OpenCvSharp.Point((int)Math.Round(p.X), (int)Math.Round(p.Y)));
                        }
                        return list.ToArray();
                    }
                }
            }
            catch { }

            // hex đều nội tiếp vào W×H (fallback)
            var list2 = new List<OpenCvSharp.Point>(6);
            for (int i = 0; i < 6; i++)
            {
                double theta = (Math.PI / 3.0) * i; // 0,60,120,...
                Point2f p = new Point2f(
                    (float)((W * 0.5) * Math.Cos(theta)),
                    (float)((H * 0.5) * Math.Sin(theta))
                );
                if (Math.Abs(angleInPatch) > 1e-6f)
                    p = RotatePoint(p, angleInPatch);
                p = new Point2f(p.X + centerInMask.X, p.Y + centerInMask.Y);
                list2.Add(new OpenCvSharp.Point((int)Math.Round(p.X), (int)Math.Round(p.Y)));
            }
            return list2.ToArray();
        }
    }
}
