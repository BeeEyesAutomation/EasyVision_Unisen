using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeInterface
{
    using BeeGlobal;
    using OpenCvSharp;
    using OpenCvSharp.Extensions;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public sealed class FrameRenderer : IDisposable
    {
        private readonly Cyotek.Windows.Forms.ImageBox _imgView;

        // Locks
        private readonly object _bmLock = new object(); // bảo vệ bmResult
        private readonly object _camLock = new object(); // bảo vệ nguồn camera (nếu cần)
        private readonly object _swapLock = new object(); // bảo vệ Mat/Bitmap A/B

        // Double-buffer Mat (sống suốt vòng đời; KHÔNG dispose giữa chừng)
        private Mat _bufA = new Mat();
        private Mat _bufB = new Mat();
        private Mat _displayMat; // tham chiếu tới buffer đang hiển thị (A hoặc B)

        // Double-buffer Bitmap (tái sử dụng, không tạo mới liên tục)
        private Bitmap _bmpA;
        private Bitmap _bmpB;
        private Bitmap _displayBmp; // tham chiếu tới bitmap đang hiển thị (A hoặc B)

        private bool _disposed;

        public FrameRenderer(Cyotek.Windows.Forms.ImageBox imageView)
        {
            _imgView = imageView ?? throw new ArgumentNullException(nameof(imageView));
            EnableDoubleBuffer(_imgView);
        }

        // Bật double-buffer cho viewer để vẽ mượt hơn
        private static void EnableDoubleBuffer(Control c)
        {
            c.GetType().GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(c, true, null);
        }

        /// ========== API chính ==========
        public void RenderAndDisplay(Mat raw)
        {
            if (_disposed) return;

            // 1) Clone frame nguồn an toàn
            Mat src;
            lock (_camLock)
            {
                src = raw?.Clone();
            }
            if (src == null || src.Empty() || src.Width <= 0 || src.Height <= 0)
            {
                src?.Dispose();
                return;
            }

            // 2) Chọn working Mat và đảm bảo còn sống/đúng kích thước
            Mat working;
            lock (_swapLock)
            {
                bool useB = ReferenceEquals(_displayMat, _bufA);
                working = useB ? _bufB : _bufA;

                // đảm bảo size/type (Create sẽ cấp phát lại nếu cần)
                working.Create(src.Rows, src.Cols, src.Type());
                src.CopyTo(working);
            }
            src.Dispose();

            // 3) Đảm bảo 8UC3 (BGR) để copy vào Bitmap 24bppRgb
            using (Mat bgr = EnsureBgr8Uc3(working))
            {
                // 4) Copy dữ liệu Mat -> Bitmap back-buffer (không tạo bitmap mới)
                Bitmap backBmp;
                lock (_swapLock)
                {
                    bool useB = ReferenceEquals(_displayBmp, _bmpA);
                    backBmp = useB ? EnsureBitmap(ref _bmpB, bgr.Width, bgr.Height)    // dùng B nếu A đang hiển thị
                                   : EnsureBitmap(ref _bmpA, bgr.Width, bgr.Height);   // dùng A nếu B đang hiển thị
                }

                CopyMatToBitmap24(bgr, backBmp);

                // 5) Vẽ overlay trực tiếp lên backBmp (không tạo Bitmap mới)
                using (var g = Graphics.FromImage(backBmp))
                using (var xf = new Matrix())
                {
                    g.SmoothingMode = SmoothingMode.None;
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.CompositingQuality = CompositingQuality.HighSpeed;
                    g.PixelOffsetMode = PixelOffsetMode.Half;

                    xf.Translate(_imgView.AutoScrollPosition.X, _imgView.AutoScrollPosition.Y);
                    float s = 1.0f;
                    try
                    {
                        var pi = _imgView.GetType().GetProperty("Zoom");
                        if (pi != null) s = Convert.ToSingle(pi.GetValue(_imgView)) / 100f;
                    }
                    catch { }
                    xf.Scale(s, s);
                    g.Transform = xf;

                    var tools = BeeCore.Common.PropetyTools[Global.IndexChoose];
                    foreach (var tool in tools)
                        if (tool.UsedTool != UsedTool.NotUsed)
                            tool.Propety.DrawResult(g);
                }

                // 6) Swap: cập nhật bmResult (clone một lần, ảnh cũ dispose) + hiển thị lên imgView
                //    (không tạo ảnh mới cho viewer; dùng chính backBmp tái sử dụng)
                lock (_bmLock)
                {
                    BeeCore.Common.bmResult?.Dispose();
                    BeeCore.Common.bmResult = (Bitmap)backBmp.Clone(); // giữ lại cho các hàm lưu ảnh
                }

                // swap con trỏ hiển thị (Mat & Bitmap)
                lock (_swapLock)
                {
                    _displayMat = working;
                    _displayBmp = backBmp;
                }

                // 7) Gán lên imgView (không rò rỉ: giải phóng ảnh cũ của control)
                Action assign = () =>
                {
                    // Tùy bạn: dùng Image hay BackgroundImage
                    var pb = _imgView as Cyotek.Windows.Forms.ImageBox;
                    if (pb != null)
                    {
                        var old = pb.Image;
                        pb.Image = _displayBmp;  // dùng back buffer trực tiếp
                        old?.Dispose();          // giải phóng ảnh cũ mà control giữ
                    }
                    else
                    {
                        var old = _imgView.BackgroundImage;
                        _imgView.BackgroundImage = _displayBmp;
                        old?.Dispose();
                    }

                    // Nếu control tự vẽ từ bmResult, chỉ cần Invalidate()
                    _imgView.Invalidate();
                };

                if (_imgView.IsHandleCreated && _imgView.InvokeRequired) _imgView.BeginInvoke(assign);
                else assign();
            }
        }

        /// Lưu ảnh hiện tại (không block render)
   

        /// ========== Helper tối ưu ==========
        // Đảm bảo Mat 8UC3 (BGR). Trả về NEW Mat nếu cần, còn nếu working đã 8UC3 thì trả working.Clone() để tránh share data.
        private static Mat EnsureBgr8Uc3(Mat working)
        {
            if (working.Type() == MatType.CV_8UC3)
                return working.Clone(); // clone tách bộ nhớ, tránh writer/reader đụng nhau

            var dst = new Mat();
            if (working.Channels() == 1)
            {
                if (working.Depth() == MatType.CV_8U)
                    Cv2.CvtColor(working, dst, ColorConversionCodes.GRAY2BGR);
                else
                {
                    using (var tmp8 = new Mat())
                    {
                        Cv2.Normalize(working, tmp8, 0, 255, NormTypes.MinMax);
                        tmp8.ConvertTo(dst, MatType.CV_8UC1);
                        Cv2.CvtColor(dst, dst, ColorConversionCodes.GRAY2BGR);
                    }    
                       
                }
            }
            else if (working.Channels() == 4 && working.Depth() == MatType.CV_8U)
            {
                Cv2.CvtColor(working, dst, ColorConversionCodes.BGRA2BGR);
            }
            else
            {
                // Fallback cho 16U/32F x 3 channels
                working.ConvertTo(dst, MatType.CV_8UC3);
            }
            return dst;
        }

        // Tạo/giữ Bitmap 24bppRgb đúng kích thước để tái sử dụng
        private static Bitmap EnsureBitmap(ref Bitmap bmp, int w, int h)
        {
            if (bmp == null || bmp.Width != w || bmp.Height != h || bmp.PixelFormat != PixelFormat.Format24bppRgb)
            {
                bmp?.Dispose();
                bmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            }
            return bmp;
        }
        // P/Invoke copy unmanaged->unmanaged, KHÔNG cần /unsafe
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, UIntPtr count);
        // Copy dữ liệu từ Mat BGR 8UC3 -> Bitmap 24bppRgb (không alloc mới)
        private static void CopyMatToBitmap24(Mat srcBgr, Bitmap dstBmp)
        {
            // y/c: srcBgr: CV_8UC3, dstBmp: 24bppRgb, cùng Width/Height
            var rect = new Rectangle(0, 0, dstBmp.Width, dstBmp.Height);
            BitmapData data = null;
            try
            {
                data = dstBmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

                int width = srcBgr.Cols;
                int height = srcBgr.Rows;
                int bytesPerPixel = 3;
                int srcStride = (int)srcBgr.Step();
                int dstStride = data.Stride;
                int rowBytes = width * bytesPerPixel;

                IntPtr pSrc = srcBgr.Data;
                IntPtr pDst = data.Scan0;

                // copy từng dòng để xử lý stride khác nhau
                for (int y = 0; y < height; y++)
                {
                    IntPtr srcRow = IntPtr.Add(pSrc, y * srcStride);
                    IntPtr dstRow = IntPtr.Add(pDst, y * dstStride);
                    CopyMemory(dstRow, srcRow, (UIntPtr)rowBytes);
                }
            }
            finally
            {
                if (data != null) dstBmp.UnlockBits(data);
            }
        }
        /// ========== Shutdown ==========
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            lock (_swapLock)
            {
                _bufA?.Dispose();
                _bufB?.Dispose();
                _displayMat = null;

                // Không dispose _displayBmp vì control có thể đang giữ; chủ động clear control trước khi shutdown app.
                _bmpA?.Dispose();
                _bmpB?.Dispose();
                _bmpA = _bmpB = _displayBmp = null;
            }

            // Tuỳ nhu cầu có huỷ bmResult:
            // lock (_bmLock) { BeeCore.Common.bmResult?.Dispose(); BeeCore.Common.bmResult = null; }
        }
    }

}
