using System;
using System.Drawing;

namespace BeeInterface.ShapeEditing
{
    public sealed class CanvasViewportChangedArgs : EventArgs
    {
        public CanvasViewportChangedArgs(float scaleZoom, Point scrollPosition, int zoomPercent)
        {
            ScaleZoom = scaleZoom;
            ScrollPosition = scrollPosition;
            ZoomPercent = zoomPercent;
        }

        public float ScaleZoom { get; }
        public Point ScrollPosition { get; }
        public int ZoomPercent { get; }
    }
}
