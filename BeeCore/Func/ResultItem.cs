using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
   
    public class ResultItem
    {
        public ResultItem(String Name) { this.Name = Name; }
        public string Name { get; set; }
        public RectRotate rot { get; set; }
        public float Score { get; set; }
        public float Percent { get; set; }
        public float Area { get; set; }
        public int ValueColor { get; set; }
        public Rectangle ColorMarkRect { get; set; } = Rectangle.Empty;
        public OpenCvSharp.Point[] ColorMarkContour { get; set; }
        public int IndexScanBox { get; set; }

        public float Distance { get; set; }
        public PointF point { get; set; }
        [NonSerialized]
        public bool IsArea = false;

        public bool IsOK { get; set; }
        public Mat matProcess = null;
        public float PercentColor = 0;

        // Overlay debug cho color check: base crop + viền mask vàng + pixel NG đỏ.
        [NonSerialized]
        public Mat ColorDebugOverlay = null;

    }

}
