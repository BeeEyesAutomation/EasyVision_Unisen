using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public static class MatrixExtension
    {
        public static PointF TransformPoint(this Matrix @this, PointF point)
        {
            var points = new[] { point };

            @this.TransformPoints(points);

            return points[0];
        }
    }
}
