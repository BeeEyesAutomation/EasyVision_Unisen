using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BeeInterface
{
    [DefaultEvent(nameof(MaskChanged))]
    public partial class MaskPainter : UserControl
    {
        private enum PaintMode
        {
            Defect,
            Normal,
            Erase
        }

        private const int MaxUndoDepth = 16;
        private readonly Stack<byte[]> undoStack = new Stack<byte[]>();
        private Bitmap background;
        private Bitmap overlay;
        private byte[] maskBytes;
        private bool overlayDirty = true;
        private bool isPainting;
        private PaintMode mode = PaintMode.Defect;
        private Point lastImagePoint;

        public MaskPainter()
        {
            InitializeComponent();
            WireEvents();
            UpdateModeButtons();
            UpdateBrushLabel();
        }

        public event EventHandler MaskChanged;

        [Browsable(false)]
        public int MaskWidth
        {
            get { return background == null ? 0 : background.Width; }
        }

        [Browsable(false)]
        public int MaskHeight
        {
            get { return background == null ? 0 : background.Height; }
        }

        [Browsable(false)]
        public bool HasMask
        {
            get { return maskBytes != null && maskBytes.Length > 0; }
        }

        public void LoadBackground(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                throw new ArgumentException("Image path is required.", nameof(imagePath));
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("Image file was not found.", imagePath);

            using (var image = Image.FromFile(imagePath))
                LoadBackground(image);
        }

        public void LoadBackground(Image image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            DisposeBitmap(ref background);
            DisposeBitmap(ref overlay);
            background = new Bitmap(image);
            maskBytes = new byte[background.Width * background.Height];
            overlayDirty = true;
            undoStack.Clear();

            picCanvas.Image = background;
            picCanvas.Invalidate();
            OnMaskChanged();
        }

        public void LoadMaskBytes(byte[] labels, int width, int height)
        {
            if (background == null)
                throw new InvalidOperationException("Load a background image before loading a mask.");
            if (width != background.Width || height != background.Height)
                throw new ArgumentException("Mask dimensions must match the background image.");
            if (labels == null || labels.Length != width * height)
                throw new ArgumentException("Mask byte array length is invalid.", nameof(labels));

            PushUndo();
            maskBytes = (byte[])labels.Clone();
            NormalizeLabels(maskBytes);
            overlayDirty = true;
            picCanvas.Invalidate();
            OnMaskChanged();
        }

        public byte[] BuildMaskBytes()
        {
            return maskBytes == null ? new byte[0] : (byte[])maskBytes.Clone();
        }

        public void ClearMask()
        {
            if (maskBytes == null)
                return;

            PushUndo();
            Array.Clear(maskBytes, 0, maskBytes.Length);
            overlayDirty = true;
            picCanvas.Invalidate();
            OnMaskChanged();
        }

        private void WireEvents()
        {
            picCanvas.Paint -= PicCanvas_Paint;
            picCanvas.Paint += PicCanvas_Paint;
            picCanvas.MouseDown -= PicCanvas_MouseDown;
            picCanvas.MouseDown += PicCanvas_MouseDown;
            picCanvas.MouseMove -= PicCanvas_MouseMove;
            picCanvas.MouseMove += PicCanvas_MouseMove;
            picCanvas.MouseUp -= PicCanvas_MouseUp;
            picCanvas.MouseUp += PicCanvas_MouseUp;
            picCanvas.MouseLeave -= PicCanvas_MouseUp;
            picCanvas.MouseLeave += PicCanvas_MouseUp;

            btnDefect.Click -= BtnDefect_Click;
            btnDefect.Click += BtnDefect_Click;
            btnNormal.Click -= BtnNormal_Click;
            btnNormal.Click += BtnNormal_Click;
            btnEraser.Click -= BtnEraser_Click;
            btnEraser.Click += BtnEraser_Click;
            btnUndo.Click -= BtnUndo_Click;
            btnUndo.Click += BtnUndo_Click;
            btnClear.Click -= BtnClear_Click;
            btnClear.Click += BtnClear_Click;
            trackBrushSize.ValueChanged -= TrackBrushSize_ValueChanged;
            trackBrushSize.ValueChanged += TrackBrushSize_ValueChanged;
        }

        private void BtnDefect_Click(object sender, EventArgs e)
        {
            mode = PaintMode.Defect;
            UpdateModeButtons();
        }

        private void BtnNormal_Click(object sender, EventArgs e)
        {
            mode = PaintMode.Normal;
            UpdateModeButtons();
        }

        private void BtnEraser_Click(object sender, EventArgs e)
        {
            mode = PaintMode.Erase;
            UpdateModeButtons();
        }

        private void BtnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count == 0 || maskBytes == null)
                return;

            maskBytes = undoStack.Pop();
            overlayDirty = true;
            picCanvas.Invalidate();
            OnMaskChanged();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearMask();
        }

        private void TrackBrushSize_ValueChanged(object sender, EventArgs e)
        {
            UpdateBrushLabel();
        }

        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || background == null || maskBytes == null)
                return;
            if (!TryMapToImagePoint(e.Location, out lastImagePoint))
                return;

            PushUndo();
            isPainting = true;
            Stamp(lastImagePoint);
            picCanvas.Invalidate();
            OnMaskChanged();
        }

        private void PicCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isPainting || background == null || maskBytes == null)
                return;
            if (!TryMapToImagePoint(e.Location, out var current))
                return;

            DrawLine(lastImagePoint, current);
            lastImagePoint = current;
            picCanvas.Invalidate();
            OnMaskChanged();
        }

        private void PicCanvas_MouseUp(object sender, EventArgs e)
        {
            isPainting = false;
        }

        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (background == null || maskBytes == null)
                return;

            EnsureOverlay();
            var bounds = GetImageBounds();
            if (!bounds.IsEmpty && overlay != null)
            {
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                e.Graphics.DrawImage(overlay, bounds);
            }
        }

        private void DrawLine(Point from, Point to)
        {
            var dx = to.X - from.X;
            var dy = to.Y - from.Y;
            var steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
            var stride = Math.Max(1, trackBrushSize.Value / 3);
            steps = Math.Max(1, steps / stride);

            for (var i = 0; i <= steps; i++)
            {
                var t = steps == 0 ? 0 : i / (float)steps;
                var x = (int)Math.Round(from.X + dx * t);
                var y = (int)Math.Round(from.Y + dy * t);
                Stamp(new Point(x, y));
            }
        }

        private void Stamp(Point center)
        {
            var label = GetCurrentLabel();
            var radius = Math.Max(1, trackBrushSize.Value / 2);
            var r2 = radius * radius;
            var minX = Math.Max(0, center.X - radius);
            var maxX = Math.Min(background.Width - 1, center.X + radius);
            var minY = Math.Max(0, center.Y - radius);
            var maxY = Math.Min(background.Height - 1, center.Y + radius);

            for (var y = minY; y <= maxY; y++)
            {
                var dy = y - center.Y;
                var row = y * background.Width;
                for (var x = minX; x <= maxX; x++)
                {
                    var dx = x - center.X;
                    if (dx * dx + dy * dy <= r2)
                        maskBytes[row + x] = label;
                }
            }

            overlayDirty = true;
        }

        private byte GetCurrentLabel()
        {
            if (mode == PaintMode.Defect)
                return 1;
            if (mode == PaintMode.Normal)
                return 2;
            return 0;
        }

        private bool TryMapToImagePoint(Point controlPoint, out Point imagePoint)
        {
            imagePoint = Point.Empty;
            var bounds = GetImageBounds();
            if (bounds.IsEmpty || !bounds.Contains(controlPoint))
                return false;

            var x = (int)Math.Floor((controlPoint.X - bounds.X) * background.Width / (float)bounds.Width);
            var y = (int)Math.Floor((controlPoint.Y - bounds.Y) * background.Height / (float)bounds.Height);
            x = Math.Max(0, Math.Min(background.Width - 1, x));
            y = Math.Max(0, Math.Min(background.Height - 1, y));
            imagePoint = new Point(x, y);
            return true;
        }

        private Rectangle GetImageBounds()
        {
            if (background == null || picCanvas.ClientSize.Width <= 0 || picCanvas.ClientSize.Height <= 0)
                return Rectangle.Empty;

            var imageAspect = background.Width / (float)background.Height;
            var clientAspect = picCanvas.ClientSize.Width / (float)picCanvas.ClientSize.Height;
            if (clientAspect > imageAspect)
            {
                var height = picCanvas.ClientSize.Height;
                var width = (int)Math.Round(height * imageAspect);
                return new Rectangle((picCanvas.ClientSize.Width - width) / 2, 0, width, height);
            }
            else
            {
                var width = picCanvas.ClientSize.Width;
                var height = (int)Math.Round(width / imageAspect);
                return new Rectangle(0, (picCanvas.ClientSize.Height - height) / 2, width, height);
            }
        }

        private void EnsureOverlay()
        {
            if (!overlayDirty && overlay != null)
                return;

            DisposeBitmap(ref overlay);
            overlay = new Bitmap(background.Width, background.Height, PixelFormat.Format32bppArgb);
            var rect = new Rectangle(0, 0, overlay.Width, overlay.Height);
            var data = overlay.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            try
            {
                var pixels = new byte[Math.Abs(data.Stride) * overlay.Height];
                for (var y = 0; y < overlay.Height; y++)
                {
                    var srcRow = y * overlay.Width;
                    var dstRow = y * data.Stride;
                    for (var x = 0; x < overlay.Width; x++)
                    {
                        var label = maskBytes[srcRow + x];
                        if (label == 0)
                            continue;

                        var offset = dstRow + x * 4;
                        if (label == 1)
                        {
                            pixels[offset + 0] = 64;
                            pixels[offset + 1] = 48;
                            pixels[offset + 2] = 230;
                            pixels[offset + 3] = 120;
                        }
                        else
                        {
                            pixels[offset + 0] = 64;
                            pixels[offset + 1] = 220;
                            pixels[offset + 2] = 90;
                            pixels[offset + 3] = 100;
                        }
                    }
                }

                Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
            }
            finally
            {
                overlay.UnlockBits(data);
            }

            overlayDirty = false;
        }

        private void PushUndo()
        {
            if (maskBytes == null)
                return;

            undoStack.Push((byte[])maskBytes.Clone());
            while (undoStack.Count > MaxUndoDepth)
            {
                var items = undoStack.ToArray();
                undoStack.Clear();
                for (var i = items.Length - 2; i >= 0; i--)
                    undoStack.Push(items[i]);
            }
        }

        private void NormalizeLabels(byte[] labels)
        {
            for (var i = 0; i < labels.Length; i++)
            {
                if (labels[i] != 1 && labels[i] != 2)
                    labels[i] = 0;
            }
        }

        private void UpdateBrushLabel()
        {
            lblBrushSize.Text = "Brush " + trackBrushSize.Value + " px";
        }

        private void UpdateModeButtons()
        {
            btnDefect.BackColor = mode == PaintMode.Defect ? Color.FromArgb(230, 68, 68) : SystemColors.Control;
            btnDefect.ForeColor = mode == PaintMode.Defect ? Color.White : SystemColors.ControlText;
            btnNormal.BackColor = mode == PaintMode.Normal ? Color.FromArgb(56, 168, 92) : SystemColors.Control;
            btnNormal.ForeColor = mode == PaintMode.Normal ? Color.White : SystemColors.ControlText;
            btnEraser.BackColor = mode == PaintMode.Erase ? Color.FromArgb(80, 96, 112) : SystemColors.Control;
            btnEraser.ForeColor = mode == PaintMode.Erase ? Color.White : SystemColors.ControlText;
        }

        private void OnMaskChanged()
        {
            var handler = MaskChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private static void DisposeBitmap(ref Bitmap bitmap)
        {
            if (bitmap == null)
                return;

            bitmap.Dispose();
            bitmap = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                DisposeBitmap(ref background);
                DisposeBitmap(ref overlay);
            }

            base.Dispose(disposing);
        }
    }
}
