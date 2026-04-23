using System.Collections.Generic;
using System.Drawing;
using BeeGlobal;

namespace BeeCore.ShapeEditing
{
    public class ShapeEditOptions
    {
        public ParaShow ParaShow { get; set; }
        public Config Config { get; set; }
        public float ScaleZoom { get; set; } = 1f;
        public Point pScroll { get; set; } = Point.Empty;
        public RectRotate rotArea { get; set; }
        public RectRotate rotMask { get; set; }
        public RectRotate rotCrop { get; set; }
        public IList<RectRotate> listRotScan { get; set; } = new List<RectRotate>();
    }
}
