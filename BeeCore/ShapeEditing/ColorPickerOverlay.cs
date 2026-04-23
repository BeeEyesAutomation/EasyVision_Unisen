using System;
using System.Drawing;
using BeeGlobal;

namespace BeeCore.ShapeEditing
{
    public sealed class ColorPickerOverlay : IOverlayPainter, IDisposable
    {
        private readonly IImageCanvas _canvas;
        private bool _active;

        public ColorPickerOverlay(IImageCanvas canvas)
        {
            _canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
            _canvas.AddOverlay(this);
        }

        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active == value)
                    return;

                _active = value;
                _canvas.SetCursor(_active ? CanvasCursorKind.Cross : CanvasCursorKind.Default);
                _canvas.RequestRedraw();
            }
        }

        public event EventHandler<ColorPickedArgs> ColorPicked;

        public void Paint(Graphics g, IImageCanvas canvas)
        {
            // Reserved for a future loupe/crosshair overlay. Color sampling stays out of View.cs.
        }

        public void NotifyColorPicked(Color color, ColorGp colorMode)
        {
            ColorPicked?.Invoke(this, new ColorPickedArgs(color, colorMode));
        }

        public void Dispose()
        {
            _canvas.RemoveOverlay(this);
            if (_active)
                _canvas.SetCursor(CanvasCursorKind.Default);
        }
    }
}
