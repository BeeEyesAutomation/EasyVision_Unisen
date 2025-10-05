using BeeCpp;
using BeeGlobal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapeType = BeeGlobal.ShapeType;

namespace BeeCore.Func
{
    public class Converts
    {
   public     static BeeCpp.RectRotateCli ToCli(RectRotate r)
        {
            var cli = new BeeCpp.RectRotateCli
            {
                Shape = (BeeCpp.ShapeType)(int)r.Shape,
                PosCenter = new BeeCpp.PointF32(r._PosCenter.X, r._PosCenter.Y),
                RectWH = new BeeCpp.RectF32(0, 0, r._rect.Width, r._rect.Height),
                RectRotationDeg = r._rectRotation,
                IsWhite = r.IsWhite,
                PolyLocalPoints = null,
                HexVertexOffsets = null
            };

            if (r.Shape == ShapeType.Polygon && r.PolyLocalPoints != null && r.PolyLocalPoints.Count > 0)
            {
                var arr = new BeeCpp.PointF32[r.PolyLocalPoints.Count];
                for (int i = 0; i < arr.Length; i++)
                    arr[i] = new BeeCpp.PointF32(r.PolyLocalPoints[i].X, r.PolyLocalPoints[i].Y);
                cli.PolyLocalPoints = arr;
            }
            if (r.Shape == ShapeType.Hexagon && r.HexVertexOffsets != null && r.HexVertexOffsets.Length == 6)
            {
                var arr = new BeeCpp.PointF32[6];
                for (int i = 0; i < 6; i++)
                    arr[i] = new BeeCpp.PointF32(r.HexVertexOffsets[i].X, r.HexVertexOffsets[i].Y);
                cli.HexVertexOffsets = arr;
            }
            return cli;
        }
     
    }
}
