using BeeCpp;
using BeeGlobal;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ShapeType = BeeGlobal.ShapeType;

namespace BeeCore.Func
{
    public class Converts
    {

public static List<RectRotate> PyToRectRotates(PyObject payloads)
    {
            var list = new List<RectRotate>();
            using (var pyList = new PyList(payloads))
            {
                foreach (PyObject t in pyList)
                    using (t)
                    {
                        float cx = (float)t.GetItem(0).As<double>();
                        float cy = (float)t.GetItem(1).As<double>();
                        float ang = (float)t.GetItem(2).As<double>(); // 0.0
                        float w = (float)t.GetItem(3).As<double>();
                        float h = (float)t.GetItem(4).As<double>();

                        var rr = new RectRotate(
                            new RectangleF(-w / 2f, -h / 2f, w, h),
                            new PointF(cx, cy),
                            ang,
                            AnchorPoint.None
                        );
                        rr.Shape = ShapeType.Polygon;

                        // tạo list MỚI cho mỗi polygon, không reuse
                        using (var pts = new PyList(t.GetItem(5)))
                        {
                            var local = new List<PointF>((int)pts.Length());
                            foreach (PyObject p in pts) using (p)
                                {
                                    float x = (float)p.GetItem(0).As<double>();
                                    float y = (float)p.GetItem(1).As<double>();
                                    local.Add(new PointF(x, y));
                                }
                            rr.PolyLocalPoints = new List<PointF>(local);
                            rr.IsPolygonClosed = true;
                        }

                        // ⚠️ Nếu muốn GIỮ Y NGUYÊN polygon như pred["polys"], KHÔNG auto-fit:
                        // rr.UpdateFromPolygon(false);   // hoặc bỏ hẳn dòng này
                        list.Add(rr);
                    }
            }
            return list;
        }
static void DrawRectRotate(Graphics g, RectRotate rr, float zoom, PointF scroll,
                           Color edge, float penWidth = 2f)
        {
            using (var m = new System.Drawing.Drawing2D.Matrix())
            {
                m.Translate(scroll.X, scroll.Y);
                m.Scale(zoom, zoom);
                m.Translate(rr._PosCenter.X, rr._PosCenter.Y);
                m.Rotate(rr._rectRotation);

                var old = g.Transform; g.Transform = m;
                var pts = rr.PolyLocalPoints;
                if (pts != null && pts.Count >= 2)
                {
                    using (var pen = new Pen(edge, penWidth / zoom))
                        g.DrawPolygon(pen, pts.ToArray()); // <- VẼ POLYGON, KHÔNG phải rr._rect
                }
                g.Transform = old;
            }
        }

    public static string BeforeFirstDigit(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            int i = 0;
            while (i < s.Length && !char.IsDigit(s[i])) i++;
            return s.Substring(0, i);
        }

        public static double? StringtoDouble(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;

            // Match: [+/-]? digits (opt thousands sep) + optional decimal with . or ,
            var m = Regex.Match(s, @"[-+]?\d{1,3}([., ]?\d{3})*([.,]\d+)?");
            if (!m.Success) return null;

            string t = m.Value.Trim();

            // Nếu có cả '.' và ',' -> đoán dấu thập phân là ký tự xuất hiện SAU CÙNG
            int lastDot = t.LastIndexOf('.');
            int lastComma = t.LastIndexOf(',');
            char decimalSep = lastDot > lastComma ? '.' : ',';

            // Bỏ các dấu phân tách nghìn: mọi ký tự ., , hoặc space KHÔNG phải là dấu thập phân cuối cùng
            var sb = new System.Text.StringBuilder(t.Length);
            for (int i = 0; i < t.Length; i++)
            {
                char c = t[i];
                if (char.IsDigit(c) || c == '+' || c == '-')
                {
                    sb.Append(c);
                }
                else if ((c == '.' || c == ',') && i == (decimalSep == '.' ? lastDot : lastComma))
                {
                    sb.Append('.'); // chuẩn hoá về '.'
                }
                // else: bỏ (nghìn sep)
            }

            return double.TryParse(sb.ToString(), System.Globalization.NumberStyles.Float,
                                   System.Globalization.CultureInfo.InvariantCulture, out double v)
                   ? v : (double?)null;
        }

        public static BeeCpp.RectRotateCli ToCli(RectRotate r)
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
