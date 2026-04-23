using System;
using System.Drawing;
using OpenCvSharp;
using Point = System.Drawing.Point;

namespace BeeCore.ShapeEditing
{
    public interface IImageCanvas
    {
        void SetImage(Mat m);
        PointF ScreenToImage(Point p);
        Point ImageToScreen(PointF p);
        float Zoom { get; }
        void AddOverlay(IOverlayPainter p);
        void RemoveOverlay(IOverlayPainter p);
        void RequestRedraw();
        void SetCursor(CanvasCursorKind cursor);
        event EventHandler<CanvasMouseArgs> CanvasMouseDown;
        event EventHandler<CanvasMouseArgs> CanvasMouseMove;
        event EventHandler<CanvasMouseArgs> CanvasMouseUp;
    }
}
