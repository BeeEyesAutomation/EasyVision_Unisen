using System.Drawing;
using BeeGlobal;

namespace BeeInterface.ShapeEditing
{
    public sealed class ImageCanvasInteractionState
    {
        public Point MouseDownPoint { get; set; }
        public Point MouseMovePoint { get; set; }
        public bool IsMouseDown { get; set; }
        public bool IsDragging { get; set; }
        public bool MaybeCreate { get; set; }
        public bool CreatingNew { get; set; }
        public bool IsNewShape { get; set; }
        public bool PolygonDirtyDuringDrag { get; set; }
        public PointF CreateStartImage { get; set; }
        public PointF CreateEndImage { get; set; }
        public RectRotate PreviewNew { get; set; }
        public RectangleF DragRect { get; set; }
        public PointF DragStart { get; set; }
        public PointF DragCenter { get; set; }
        public PointF DragStartOffset { get; set; }
        public float DragRotation { get; set; }
        public float RotateStartAngleLocal { get; set; }
        public float RotateBase { get; set; }
        public int IndexRotChoose { get; set; } = -1;
    }
}
