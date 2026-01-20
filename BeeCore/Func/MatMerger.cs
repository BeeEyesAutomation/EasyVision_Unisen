using System;
using OpenCvSharp;
namespace BeeCore
{
    public enum MergeDirection
    {
        Horizontal,
        Vertical
    }

    public enum SizeMode
    {
        /// <summary>
        /// Ép ảnh B scale theo ảnh A để khớp chiều (rows hoặc cols) rồi concat.
        /// </summary>
        ResizeToMatch,

        /// <summary>
        /// Không scale ảnh; pad (đệm viền) ảnh nhỏ hơn để khớp chiều rồi concat.
        /// </summary>
        PadToMatch
    }

    public sealed class MatMergerOptions
    {
        public MergeDirection Direction { get; set; } = MergeDirection.Horizontal;
        public SizeMode SizeMode { get; set; } = SizeMode.ResizeToMatch;

        /// <summary>Luôn đưa ảnh về BGR 8U trước khi merge.</summary>
        public bool ForceBgr8U { get; set; } = true;

        /// <summary>Màu pad khi SizeMode=PadToMatch (BGR).</summary>
        public Scalar PadColor { get; set; } = Scalar.Black;

        /// <summary>Interpolation khi resize.</summary>
        public InterpolationFlags Interp { get; set; } = InterpolationFlags.Area;
    }

    public static class MatMerger
    {
        public static Mat Merge(Mat a, Mat b, MatMergerOptions opt = null)
        {if(opt==null)
            opt = new MatMergerOptions();

            if (a == null || b == null) throw new ArgumentNullException();
            if (a.Empty() || b.Empty()) throw new ArgumentException("Input Mat empty.");

            Mat A = opt.ForceBgr8U ? EnsureBgr8U(a) : a;
            Mat B = opt.ForceBgr8U ? EnsureBgr8U(b) : b;

            if (A.Type() != B.Type())
                throw new ArgumentException($"Type mismatch: A={A.Type()}, B={B.Type()} (set ForceBgr8U=true or convert same type).");

            // 1) Chuẩn hoá kích thước theo option
            if (opt.Direction == MergeDirection.Horizontal)
            {
                // cần cùng Rows
                if (A.Rows != B.Rows)
                {
                    if (opt.SizeMode == SizeMode.ResizeToMatch)
                    {
                        B = ResizeKeepAspect(B, targetRows: A.Rows, targetCols: null, opt.Interp);
                    }
                    else // PadToMatch
                    {
                        A = PadToRows(A, A.Rows, opt.PadColor); // (no-op)
                        B = PadToRows(B, A.Rows, opt.PadColor);
                    }
                }

                Mat outMat = new Mat();
                Cv2.HConcat(new[] { A, B }, outMat);
                return outMat;
            }
            else // Vertical
            {
                // cần cùng Cols
                if (A.Cols != B.Cols)
                {
                    if (opt.SizeMode == SizeMode.ResizeToMatch)
                    {
                        B = ResizeKeepAspect(B, targetRows: null, targetCols: A.Cols, opt.Interp);
                    }
                    else // PadToMatch
                    {
                        A = PadToCols(A, A.Cols, opt.PadColor); // (no-op)
                        B = PadToCols(B, A.Cols, opt.PadColor);
                    }
                }

                Mat outMat = new Mat();
                Cv2.VConcat(new[] { A, B }, outMat);
                return outMat;
            }
        }

        // ================= helpers =================

        private static Mat EnsureBgr8U(Mat src)
        {
            // Trả về Mat mới nếu cần convert, còn đúng type thì return src (không clone).
            if (src.Type() == MatType.CV_8UC3) return src;

            if (src.Type() == MatType.CV_8UC1)
            {
                Mat bgr = new Mat();
                Cv2.CvtColor(src, bgr, ColorConversionCodes.GRAY2BGR);
                return bgr;
            }

            if (src.Type() == MatType.CV_8UC4)
            {
                Mat bgr = new Mat();
                Cv2.CvtColor(src, bgr, ColorConversionCodes.BGRA2BGR);
                return bgr;
            }

            // Nếu 16U/32F... convert về 8U (tuỳ nhu cầu có thể normalize trước)
            Mat u8 = new Mat();
            src.ConvertTo(u8, MatType.CV_8U);

            if (u8.Channels() == 1)
            {
                Mat bgr = new Mat();
                Cv2.CvtColor(u8, bgr, ColorConversionCodes.GRAY2BGR);
                return bgr;
            }
            if (u8.Channels() == 3) return u8;

            Mat bgr2 = new Mat();
            Cv2.CvtColor(u8, bgr2, ColorConversionCodes.BGRA2BGR);
            return bgr2;
        }

        private static Mat ResizeKeepAspect(Mat src, int? targetRows, int? targetCols, InterpolationFlags interp)
        {
            if (targetRows == null && targetCols == null)
                throw new ArgumentException("targetRows/targetCols required.");

            int newW, newH;

            if (targetRows != null)
            {
                newH = targetRows.Value;
                double scale = (double)newH / src.Rows;
                newW = (int)Math.Round(src.Cols * scale);
            }
            else
            {
                newW = targetCols.Value;
                double scale = (double)newW / src.Cols;
                newH = (int)Math.Round(src.Rows * scale);
            }

            if (newW <= 0) newW = 1;
            if (newH <= 0) newH = 1;

            Mat dst = new Mat();
            Cv2.Resize(src, dst, new Size(newW, newH), 0, 0, interp);
            return dst;
        }

        private static Mat PadToRows(Mat src, int rows, Scalar padColor)
        {
            if (src.Rows == rows) return src;
            if (src.Rows > rows) return CropCenterRows(src, rows);

            int top = (rows - src.Rows) / 2;
            int bottom = rows - src.Rows - top;

            Mat dst = new Mat();
            Cv2.CopyMakeBorder(src, dst, top, bottom, 0, 0, BorderTypes.Constant, padColor);
            return dst;
        }

        private static Mat PadToCols(Mat src, int cols, Scalar padColor)
        {
            if (src.Cols == cols) return src;
            if (src.Cols > cols) return CropCenterCols(src, cols);

            int left = (cols - src.Cols) / 2;
            int right = cols - src.Cols - left;

            Mat dst = new Mat();
            Cv2.CopyMakeBorder(src, dst, 0, 0, left, right, BorderTypes.Constant, padColor);
            return dst;
        }

        private static Mat CropCenterRows(Mat src, int rows)
        {
            int y = (src.Rows - rows) / 2;
            if (y < 0) y = 0;
            if (y + rows > src.Rows) rows = src.Rows - y;
            return new Mat(src, new Rect(0, y, src.Cols, rows)).Clone();
        }

        private static Mat CropCenterCols(Mat src, int cols)
        {
            int x = (src.Cols - cols) / 2;
            if (x < 0) x = 0;
            if (x + cols > src.Cols) cols = src.Cols - x;
            return new Mat(src, new Rect(x, 0, cols, src.Rows)).Clone();
        }
    }
}