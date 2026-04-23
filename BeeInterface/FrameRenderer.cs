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
        private readonly object _bmLock = new object(); // b?o v? bmResult
        private readonly object _camLock = new object(); // b?o v? ngu?n camera (n?u c?n)
        private readonly object _swapLock = new object(); // b?o v? Mat/Bitmap A/B

        // Double-buffer Mat (s?ng su?t vòng d?i; KHÔNG dispose gi?a ch?ng)
        private Mat _bufA = new Mat();
        private Mat _bufB = new Mat();
        private Mat _displayMat; // tham chi?u t?i buffer dang hi?n th? (A ho?c B)

        // Double-buffer Bitmap (tái s? d?ng, không t?o m?i liên t?c)
        private Bitmap _bmpA;
        private Bitmap _bmpB;
        private Bitmap _displayBmp; // tham chi?u t?i bitmap dang hi?n th? (A ho?c B)

        private bool _disposed;

        public FrameRenderer(Cyotek.Windows.Forms.ImageBox imageView)
        {
            _imgView = imageView ?? throw new ArgumentNullException(nameof(imageView));
            EnableDoubleBuffer(_imgView);
        }

        // B?t double-buffer cho viewer d? v? mu?t hon
        private static void EnableDoubleBuffer(Control c)
        {
            c.GetType().GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(c, true, null);
        }

        /// === API chính ===
        public void RenderAndDisplay(Mat raw)
        {
            if (_disposed) return;

            // 1) Clone frame ngu?n an toàn
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

            // 2) Ch?n working Mat và d?m b?o còn s?ng/dúng kích thu?c
            Mat working;
            lock (_swapLock)
            {
                bool useB = ReferenceEquals(_displayMat, _bufA);
                working = useB ? _bufB : _bufA;

                // d?m b?o size/type (Create s? c?p phát l?i n?u c?n)
                working.Create(src.Rows, src.Cols, src.Type());
                src.CopyTo(working);
            }
            src.Dispose();

            // 3) Ð?m b?o 8UC3 (BGR) d? copy vào Bitmap 24bppRgb
            using (Mat bgr = EnsureBgr8Uc3(working))
            {
                // 4) Copy d? li?u Mat -> Bitmap back-buffer (không t?o bitmap m?i)
                Bitmap backBmp;
                lock (_swapLock)
                {
                    bool useB = ReferenceEquals(_displayBmp, _bmpA);
                    backBmp = useB ? EnsureBitmap(ref _bmpB, bgr.Width, bgr.Height)    // dùng B n?u A dang hi?n th?
                                   : EnsureBitmap(ref _bmpA, bgr.Width, bgr.Height);   // dùng A n?u B dang hi?n th?
                }

                CopyMatToBitmap24(bgr, backBmp);

                // 5) V? overlay tr?c ti?p lên backBmp (không t?o Bitmap m?i)
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

                    var tools = BeeCore.Common.EnsureToolList(Global.IndexProgChoose);
                    foreach (var tool in tools)
                        if (tool.UsedTool != UsedTool.NotUsed)
                            tool.Propety2.DrawResult(g);
                }

                // 6) Swap: c?p nh?t bmResult (clone m?t l?n, ?nh cu dispose) + hi?n th? lên imgView
                //    (không t?o ?nh m?i cho viewer; dùng chính backBmp tái s? d?ng)
                //lock (_bmLock)
                //{
                //    BeeCore.Common.bmResult?.Dispose();
                //    BeeCore.Common.bmResult = (Bitmap)backBmp.Clone(); // gi? l?i cho các hàm luu ?nh
                //}

                // swap con tr? hi?n th? (Mat & Bitmap)
                lock (_swapLock)
                {
                    _displayMat = working;
                    _displayBmp = backBmp;
                }

                // 7) Gán lên imgView (không rò r?: gi?i phóng ?nh cu c?a control)
                Action assign = () =>
                {
                    // Tùy b?n: dùng Image hay BackgroundImage
                    var pb = _imgView as Cyotek.Windows.Forms.ImageBox;
                    if (pb != null)
                    {
                        var old = pb.Image;
                        pb.Image = _displayBmp;  // dùng back buffer tr?c ti?p
                        old?.Dispose();          // gi?i phóng ?nh cu mà control gi?
                    }
                    else
                    {
                        var old = _imgView.BackgroundImage;
                        _imgView.BackgroundImage = _displayBmp;
                        old?.Dispose();
                    }

                    // N?u control t? v? t? bmResult, ch? c?n Invalidate()
                    _imgView.Invalidate();
                };

                if (_imgView.IsHandleCreated && _imgView.InvokeRequired) _imgView.BeginInvoke(assign);
                else assign();
            }
        }

        /// Luu ?nh hi?n t?i (không block render)
   

        /// === Helper t?i uu ===
        // Ð?m b?o Mat 8UC3 (BGR). Tr? v? NEW Mat n?u c?n, còn n?u working dã 8UC3 thì tr? working.Clone() d? tránh share data.
        private static Mat EnsureBgr8Uc3(Mat working)
        {
            if (working.Type() == MatType.CV_8UC3)
                return working.Clone(); // clone tách b? nh?, tránh writer/reader d?ng nhau

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

        // T?o/gi? Bitmap 24bppRgb dúng kích thu?c d? tái s? d?ng
        private static Bitmap EnsureBitmap(ref Bitmap bmp, int w, int h)
        {
            if (bmp == null || bmp.Width != w || bmp.Height != h || bmp.PixelFormat != PixelFormat.Format24bppRgb)
            {
                bmp?.Dispose();
                bmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            }
            return bmp;
        }
        // P/Invoke copy unmanaged->unmanaged, KHÔNG c?n /unsafe
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, UIntPtr count);
        // Copy d? li?u t? Mat BGR 8UC3 -> Bitmap 24bppRgb (không alloc m?i)
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

                // copy t?ng dòng d? x? lý stride khác nhau
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
        /// === Shutdown ===
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            lock (_swapLock)
            {
                _bufA?.Dispose();
                _bufB?.Dispose();
                _displayMat = null;

                // Không dispose _displayBmp vì control có th? dang gi?; ch? d?ng clear control tru?c khi shutdown app.
                _bmpA?.Dispose();
                _bmpB?.Dispose();
                _bmpA = _bmpB = _displayBmp = null;
            }

            // Tu? nhu c?u có hu? bmResult:
            // lock (_bmLock) { BeeCore.Common.bmResult?.Dispose(); BeeCore.Common.bmResult = null; }
        }
    }

}
