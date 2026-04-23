using System;
using System.Drawing;

namespace BeeCore.ShapeEditing
{
    public sealed class CanvasMouseArgs : EventArgs
    {
        public CanvasMouseArgs(Point screen, PointF image, CanvasMouseButton button)
        {
            Screen = screen;
            Image = image;
            Button = button;
        }

        public Point Screen { get; private set; }
        public PointF Image { get; private set; }
        public CanvasMouseButton Button { get; private set; }
    }
}
