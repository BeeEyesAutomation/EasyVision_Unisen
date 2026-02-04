using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BeeGlobal
{
    [Serializable()]
    public class RectRotate
    {
        public  bool ContainsPoint(PointF pWorld, float eps = 1e-4f)
        {
            // 1) World -> Local (local gốc tại rr._PosCenter, trục local quay theo rr._rectRotation)
            float dx = pWorld.X - this._PosCenter.X;
            float dy = pWorld.Y - this._PosCenter.Y;

            double rad = -this._rectRotation * Math.PI / 180.0; // quay ngược để đưa về local
            float c = (float)Math.Cos(rad);
            float s = (float)Math.Sin(rad);

            float xL = dx * c - dy * s;
            float yL = dx * s + dy * c;

            // 2) Check theo shape
            var r = this._rect; // expected: (-w/2,-h/2,w,h) in local

            switch (this.Shape)
            {
                case BeeGlobal.ShapeType.Rectangle:
                    return (xL >= r.Left - eps && xL <= r.Right + eps &&
                            yL >= r.Top - eps && yL <= r.Bottom + eps);

                case BeeGlobal.ShapeType.Ellipse:
                    {
                        float a = r.Width * 0.5f;
                        float b = r.Height * 0.5f;
                        if (a <= eps || b <= eps) return false;
                        float u = xL / a;
                        float v = yL / b;
                        return (u * u + v * v) <= 1.0f + eps;
                    }

                case BeeGlobal.ShapeType.Polygon:
                    {
                        // PolyLocalPoints là điểm local -> dùng thẳng
                        var poly = this.PolyLocalPoints;
                        if (poly == null || poly.Count < 3) return false;
                        return BeeGlobal.RectRotate.PointInPolygon(poly, new PointF(xL, yL));
                    }

                default:
                    // nếu shape khác, fallback rectangle
                    return (xL >= r.Left - eps && xL <= r.Right + eps &&
                            yL >= r.Top - eps && yL <= r.Bottom + eps);
            }
        }
        public int TypeValue;
        public bool IsOK = false;
        public float Score = 0;
        public float Value = 0;
        public String Name = "";
        public RectangleF _rect { get; set; }                 // ALWAYS: (-w/2,-h/2,w,h)
        public PointF _PosCenter { get; set; }                // world center
        private float _angle = 0f;
        public float _rectRotation { get { return _angle; } set { _angle = value; } }
        public AnchorPoint _dragAnchor { get; set; }
        public ShapeType Shape { get; set; } = ShapeType.Rectangle;
        public bool IsWhite = false;
        public PointF[] HexVertexOffsets { get;  set; }   // offsets from default hex (local)
        public List<PointF> PolyLocalPoints { get;  set; } // polygon points (local)
        public bool IsPolygonClosed { get; set; }
        public int ActiveVertexIndex { get; set; }

        public bool AutoExpandBounds { get; set; } = true;
        public bool AutoOrientPolygon { get; set; } = true; // auto xoay theo PCA khi chuẩn hoá polygon (MouseUp)

        public RectRotate(RectangleF rect, PointF posCenter, float rectRotation, AnchorPoint dragAnchor)
        {
            _rect = rect;
            _PosCenter = posCenter;
            _rectRotation = rectRotation;
            _dragAnchor = dragAnchor;

            HexVertexOffsets = new PointF[6];
            for (int i = 0; i < 6; i++) HexVertexOffsets[i] = PointF.Empty;

            PolyLocalPoints = new List<PointF>(16);
            IsPolygonClosed = false;
            ActiveVertexIndex = -1;
        }

        protected RectRotate(RectRotate clone)
        {
            _rect = clone._rect;
            _PosCenter = clone._PosCenter;
            _rectRotation = clone._rectRotation;
            _dragAnchor = clone._dragAnchor;
            Shape = clone.Shape;
            if(clone.HexVertexOffsets==null)
            {
                clone.HexVertexOffsets = new PointF[6];
                
            }    
            HexVertexOffsets = new PointF[6];
            for (int i = 0; i < 6; i++) HexVertexOffsets[i] = clone.HexVertexOffsets[i];
            if (clone.PolyLocalPoints == null)
            {
                clone.PolyLocalPoints = new List<PointF>();

            }
            PolyLocalPoints = new List<PointF>(clone.PolyLocalPoints);
            IsPolygonClosed = clone.IsPolygonClosed;
            ActiveVertexIndex = clone.ActiveVertexIndex;
            AutoExpandBounds = clone.AutoExpandBounds;
            AutoOrientPolygon = clone.AutoOrientPolygon;
            Name=clone.Name;
        }

        public RectRotate()
        {
            _rect = RectangleF.Empty;
            _PosCenter = PointF.Empty;
            _rectRotation = 0f;
            _dragAnchor = AnchorPoint.None;

            HexVertexOffsets = new PointF[6];
            for (int i = 0; i < 6; i++) HexVertexOffsets[i] = PointF.Empty;

            PolyLocalPoints = new List<PointF>(16);
            IsPolygonClosed = false;
            ActiveVertexIndex = -1;
        }
        public void ExpandPixels(float expandX, float expandY, bool scalePolygonPoints = false)
        {
            if (expandX < 0) expandX = 0;
            if (expandY < 0) expandY = 0;

            // _rect luôn theo quy ước local: (-w/2, -h/2, w, h)
            float w0 = Math.Max(1f, _rect.Width);
            float h0 = Math.Max(1f, _rect.Height);

            float w1 = Math.Max(1f, w0 + 2f * expandX);
            float h1 = Math.Max(1f, h0 + 2f * expandY);

            if (Shape == ShapeType.Polygon && scalePolygonPoints && PolyLocalPoints != null && PolyLocalPoints.Count > 0)
            {
                float sx = w1 / w0;
                float sy = h1 / h0;

                int end = PolyLocalPoints.Count;
                // nếu closed thì giữ điểm cuối = điểm đầu
                bool closed = (end >= 2 && PolyLocalPoints[0].Equals(PolyLocalPoints[end - 1]));

                for (int i = 0; i < end; i++)
                {
                    var p = PolyLocalPoints[i];
                    PolyLocalPoints[i] = new PointF(p.X * sx, p.Y * sy);
                }

                if (closed) PolyLocalPoints[end - 1] = PolyLocalPoints[0];
            }

            _rect = new RectangleF(-w1 / 2f, -h1 / 2f, w1, h1);
        }
        public void OffsetPixels(float dx, float dy, bool scalePolygonPoints = false)
        {
            // dx, dy > 0 : expand
            // dx, dy < 0 : shrink
            float w0 = Math.Max(1f, _rect.Width);
            float h0 = Math.Max(1f, _rect.Height);

            float w1 = Math.Max(1f, w0 + 2f * dx);
            float h1 = Math.Max(1f, h0 + 2f * dy);

            if (Shape == ShapeType.Polygon &&
                scalePolygonPoints &&
                PolyLocalPoints != null &&
                PolyLocalPoints.Count > 0)
            {
                float sx = w1 / w0;
                float sy = h1 / h0;

                int end = PolyLocalPoints.Count;
                bool closed =
                    end >= 2 &&
                    PolyLocalPoints[0].Equals(PolyLocalPoints[end - 1]);

                for (int i = 0; i < end; i++)
                {
                    var p = PolyLocalPoints[i];
                    PolyLocalPoints[i] = new PointF(p.X * sx, p.Y * sy);
                }

                if (closed)
                    PolyLocalPoints[end - 1] = PolyLocalPoints[0];
            }

            _rect = new RectangleF(-w1 / 2f, -h1 / 2f, w1, h1);
        }
        public RectRotate Clone() => new RectRotate(this);

        // ====== NEW: reset sạch trước khi bắt đầu polygon mới ======
        public void ResetFrameForNewPolygonHard()
        {
            _PosCenter = PointF.Empty;
            _rectRotation = 0f;
            _rect = RectangleF.Empty;
            _dragAnchor = AnchorPoint.None;
            ActiveVertexIndex = -1;

            if (PolyLocalPoints == null) PolyLocalPoints = new List<PointF>(16);
            PolyLocalPoints.Clear();
            IsPolygonClosed = false;
        }

        //  HEXAGON 
        public PointF[] GetHexagonVerticesLocal()
        {
            const float SQ3_2 = 0.8660254037844386f;
            float halfW = _rect.Width / 2f;
            float halfH = _rect.Height / 2f;

            PointF[] baseVerts = new PointF[6]
            {
                new PointF(+1f, 0f),
                new PointF(+0.5f, +SQ3_2),
                new PointF(-0.5f, +SQ3_2),
                new PointF(-1f, 0f),
                new PointF(-0.5f, -SQ3_2),
                new PointF(+0.5f, -SQ3_2)
            };

            var verts = new PointF[6];
            if (HexVertexOffsets == null)
            {
                HexVertexOffsets = new PointF[6];
                for (int i = 0; i < 6; i++) HexVertexOffsets[i] = PointF.Empty;
            }

            for (int i = 0; i < 6; i++)
            {
                var mapped = new PointF(baseVerts[i].X * halfW, baseVerts[i].Y * halfH);
                verts[i] = new PointF(mapped.X + HexVertexOffsets[i].X, mapped.Y + HexVertexOffsets[i].Y);
            }
            return verts;
        }

        private PointF[] GetHexagonVerticesLocalWithoutOffsets()
        {
            const float SQ3_2 = 0.8660254037844386f;
            float halfW = _rect.Width / 2f;
            float halfH = _rect.Height / 2f;
            return new PointF[6]
            {
                new PointF(+1f*halfW, 0f),
                new PointF(+0.5f*halfW, +SQ3_2*halfH),
                new PointF(-0.5f*halfW, +SQ3_2*halfH),
                new PointF(-1f*halfW, 0f),
                new PointF(-0.5f*halfW, -SQ3_2*halfH),
                new PointF(+0.5f*halfW, -SQ3_2*halfH)
            };
        }

        public void SetHexVertexByLocalPoint(int i, PointF localPoint)
        {
            var def = GetHexagonVerticesLocalWithoutOffsets();
            HexVertexOffsets[i] = new PointF(localPoint.X - def[i].X, localPoint.Y - def[i].Y);
        }

        public PointF[] GetHexagonVerticesWorld()
        {
            var local = GetHexagonVerticesLocal();
            var world = new PointF[local.Length];
            for (int i = 0; i < local.Length; i++)
                world[i] = Add(_PosCenter, Rotate(local[i], _rectRotation));
            return world;
        }

        public void SetHexagonFromWorldVertices(PointF[] worldVerts)
        {
            if (worldVerts == null || worldVerts.Length != 6) return;

            double rad = -_rectRotation * Math.PI / 180.0;
            double c = Math.Cos(rad), s = Math.Sin(rad);

            if (HexVertexOffsets == null || HexVertexOffsets.Length != 6)
                HexVertexOffsets = new PointF[6];

            var defLocal = GetHexagonVerticesLocalWithoutOffsets();

            for (int i = 0; i < 6; i++)
            {
                var dw = new PointF(worldVerts[i].X - _PosCenter.X, worldVerts[i].Y - _PosCenter.Y);
                var loc = new PointF((float)(dw.X * c - dw.Y * s), (float)(dw.X * s + dw.Y * c));
                HexVertexOffsets[i] = new PointF(loc.X - defLocal[i].X, loc.Y - defLocal[i].Y);
            }
        }

        public void PreserveHexagonWorldVerticesWhile(Action changeRectCenterRotation)
        {
            if (Shape != ShapeType.Hexagon || changeRectCenterRotation == null)
            {
                changeRectCenterRotation?.Invoke();
                return;
            }
            var world = GetHexagonVerticesWorld();
            changeRectCenterRotation();
            SetHexagonFromWorldVertices(world);
        }

        public void LetHexagonFollowTransform(Action changeRectCenterRotation)
        {
            changeRectCenterRotation?.Invoke();
        }

        //  POLYGON 
        public void PolygonClear()
        {
            if (PolyLocalPoints == null) PolyLocalPoints = new List<PointF>(16);
            PolyLocalPoints.Clear();
            IsPolygonClosed = false;
            ActiveVertexIndex = -1;
        }

        public void PolygonAddPointLocal(PointF pLocal)
        {
            if (IsPolygonClosed) return;
            PolyLocalPoints.Add(pLocal);
        }

        public bool PolygonTryCloseIfNearFirst(PointF pLocal, float closeTolerance)
        {
            if (IsPolygonClosed) return true;
            if (PolyLocalPoints.Count < 3) return false;

            PointF p0 = PolyLocalPoints[0];
            if (DistanceSquared(pLocal, p0) <= closeTolerance * closeTolerance)
            {
                //PolyLocalPoints.Add(p0);
                IsPolygonClosed = true;
             //   PolyLocalPoints = UniquePolygonPoints(PolyLocalPoints.ToArray(), true).ToList();
                return true;
            }
            return false;
        }

        public PointF[] GetPolygonVerticesLocal() => PolyLocalPoints.ToArray();

        public PointF[] GetPolygonVerticesWorld()
        {
            var local = GetPolygonVerticesLocal();
            var world = new PointF[local.Length];
            for (int i = 0; i < local.Length; i++)
                world[i] = Add(_PosCenter, Rotate(local[i], _rectRotation));
            return world;
        }

        public void TranslatePolygonLocal(float dx, float dy)
        {
            if (PolyLocalPoints == null || PolyLocalPoints.Count == 0) return;

            bool closed = (PolyLocalPoints.Count >= 2 &&
                           PolyLocalPoints[0].Equals(PolyLocalPoints[PolyLocalPoints.Count - 1]));

            for (int i = 0; i < PolyLocalPoints.Count; i++)
            {
                var p = PolyLocalPoints[i];
                PolyLocalPoints[i] = new PointF(p.X + dx, p.Y + dy);
            }

            if (closed) PolyLocalPoints[PolyLocalPoints.Count - 1] = PolyLocalPoints[0];
        }

        private static PointF RotateLocal(PointF p, float deg)
        {
            double r = deg * Math.PI / 180.0;
            double c = Math.Cos(r), s = Math.Sin(r);
            return new PointF((float)(p.X * c - p.Y * s), (float)(p.X * s + p.Y * c));
        }

        private void RotatePolygonLocal(float deg)
        {
            if (PolyLocalPoints == null || PolyLocalPoints.Count == 0) return;
            for (int i = 0; i < PolyLocalPoints.Count; i++)
                PolyLocalPoints[i] = RotateLocal(PolyLocalPoints[i], deg);
        }
        private static PointF[] UniquePolygonPoints(PointF[] poly, bool isClosed)
        {
            if (poly == null) return null;
            int n = poly.Length;
            if (n == 0) return poly;

            // Nếu polygon đóng và phần tử cuối trùng hệt phần tử đầu → bỏ phần tử cuối
            bool hasDupClose = isClosed && n >= 2 && poly[n - 1].Equals(poly[0]);
            if (!hasDupClose) return poly;

            var pts = new PointF[n - 1];
            Array.Copy(poly, pts, n - 1);
            return pts;
        }
        /// <summary>
        /// Cập nhật _PosCenter, _rect theo polygon hiện tại.
        /// Nếu updateAngle = true: ước lượng hướng chính (PCA thô) rồi cộng vào _rectRotation
        /// và quay các điểm polygon về lại trục để bbox local vẫn axis-aligned.
        /// </summary>
        public void UpdateFromPolygon(bool updateAngle)
        {
            if (Shape != ShapeType.Polygon || PolyLocalPoints == null || PolyLocalPoints.Count < 1)
                return;
            PolyLocalPoints= UniquePolygonPoints(PolyLocalPoints.ToArray(), true).ToList();
            // 1) bbox local hiện tại
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;

            int n = PolyLocalPoints.Count;
            int max = n;
            if (n >= 2 && PolyLocalPoints[0].Equals(PolyLocalPoints[n - 1])) max = n - 1;

            for (int i = 0; i < max; i++)
            {
                var p = PolyLocalPoints[i];
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }

            float w = Math.Max(1f, maxX - minX);
            float h = Math.Max(1f, maxY - minY);
            float cx = (minX + maxX) * 0.5f;
            float cy = (minY + maxY) * 0.5f;

            // 2) PCA (optional)
            float thetaDeg = 0f;
            if (updateAngle && max >= 2)
            {
                double sxx = 0, syy = 0, sxy = 0;
                int count = max;
                for (int i = 0; i < count; i++)
                {
                    double dx = PolyLocalPoints[i].X - cx;
                    double dy = PolyLocalPoints[i].Y - cy;
                    sxx += dx * dx;
                    syy += dy * dy;
                    sxy += dx * dy;
                }
                double denom = (sxx - syy);
                double num = 2.0 * sxy;
                if (Math.Abs(denom) + Math.Abs(num) > 1e-12)
                {
                    double thetaRadHalf = 0.5 * Math.Atan2(num, denom);
                    thetaDeg = (float)(thetaRadHalf * 2.0 * 180.0 / Math.PI);
                }
            }

            // 3) đưa polygon về quanh gốc
            for (int i = 0; i < PolyLocalPoints.Count; i++)
                PolyLocalPoints[i] = new PointF(PolyLocalPoints[i].X - cx, PolyLocalPoints[i].Y - cy);

            // 4) nếu auto-orient: quay điểm về trục, cộng góc vào _rectRotation
            if (updateAngle && Math.Abs(thetaDeg) > 1e-6f)
            {
                RotatePolygonLocal(-thetaDeg);
                _rectRotation += thetaDeg;
            }

            // 5) dịch center thế giới theo (cx,cy) đã xoay
            var deltaWorld = Rotate(new PointF(cx, cy), _rectRotation);
            _PosCenter = new PointF(_PosCenter.X + deltaWorld.X, _PosCenter.Y + deltaWorld.Y);

            // 6) nếu vừa quay, tính lại w/h
            if (updateAngle && Math.Abs(thetaDeg) > 1e-6f)
            {
                float min2X = float.MaxValue, min2Y = float.MaxValue;
                float max2X = float.MinValue, max2Y = float.MinValue;
                max = PolyLocalPoints.Count;
                if (max >= 2 && PolyLocalPoints[0].Equals(PolyLocalPoints[max - 1])) max -= 1;
                for (int i = 0; i < max; i++)
                {
                    var p = PolyLocalPoints[i];
                    if (p.X < min2X) min2X = p.X;
                    if (p.Y < min2Y) min2Y = p.Y;
                    if (p.X > max2X) max2X = p.X;
                    if (p.Y > max2Y) max2Y = p.Y;
                }
                w = Math.Max(1f, max2X - min2X);
                h = Math.Max(1f, max2Y - min2Y);
            }

            // 7) set _rect theo quy ước local
            _rect = new RectangleF(-w / 2f, -h / 2f, w, h);
        }

        //  Refit (khi cần) 
        public void RefitBoundsToPolygon()
        {
            if (PolyLocalPoints == null || PolyLocalPoints.Count == 0) return;

            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;
            for (int i = 0; i < PolyLocalPoints.Count; i++)
            {
                var p = PolyLocalPoints[i];
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }

            float w = Math.Max(1f, maxX - minX);
            float h = Math.Max(1f, maxY - minY);
            float cx = (minX + maxX) * 0.5f;
            float cy = (minY + maxY) * 0.5f;

            for (int i = 0; i < PolyLocalPoints.Count; i++)
                PolyLocalPoints[i] = new PointF(PolyLocalPoints[i].X - cx, PolyLocalPoints[i].Y - cy);

            _rect = new RectangleF(-w / 2f, -h / 2f, w, h);
        }

        public void RefitBoundsToHexagon()
        {
            var vertsAbs = GetHexagonVerticesLocal();

            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;
            for (int i = 0; i < 6; i++)
            {
                var p = vertsAbs[i];
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }

            float w = Math.Max(1f, maxX - minX);
            float h = Math.Max(1f, maxY - minY);
            float cx = (minX + maxX) * 0.5f;
            float cy = (minY + maxY) * 0.5f;

            _rect = new RectangleF(-w / 2f, -h / 2f, w, h);

            var defLocal = GetHexagonVerticesLocalWithoutOffsets(); // theo size mới, tâm 0
            if (HexVertexOffsets == null || HexVertexOffsets.Length != 6)
                HexVertexOffsets = new PointF[6];

            for (int i = 0; i < 6; i++)
            {
                var pRecentered = new PointF(vertsAbs[i].X - cx, vertsAbs[i].Y - cy);
                HexVertexOffsets[i] = new PointF(
                    pRecentered.X - defLocal[i].X,
                    pRecentered.Y - defLocal[i].Y
                );
            }
        }

        //  Utils & Maintenance 
        public void Clear()
        {
            if (Shape == ShapeType.Polygon)
            {
                PolygonClear(); // chỉ clear polygon
            }
            else
            {
                _rect = RectangleF.Empty;
                _PosCenter = PointF.Empty;
                _rectRotation = 0f;
                _dragAnchor = AnchorPoint.None;

                if (HexVertexOffsets == null) HexVertexOffsets = new PointF[6];
                for (int i = 0; i < HexVertexOffsets.Length; i++) HexVertexOffsets[i] = PointF.Empty;

                if (PolyLocalPoints == null) PolyLocalPoints = new List<PointF>(16);
                PolyLocalPoints.Clear();
                IsPolygonClosed = false;
                ActiveVertexIndex = -1;
            }
        }

        public bool IsEmptyForCreate()
        {
            if (Shape == ShapeType.Polygon)
                return PolyLocalPoints == null || PolyLocalPoints.Count == 0;
            return (_rect.Width <= 0f || _rect.Height <= 0f);
        }

        public static float DistanceSquared(PointF a, PointF b)
        {
            float dx = a.X - b.X, dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }

        public static PointF Rotate(PointF p, float deg)
        {
            double r = deg * Math.PI / 180.0;
            double c = Math.Cos(r), s = Math.Sin(r);
            return new PointF((float)(p.X * c - p.Y * s), (float)(p.X * s + p.Y * c));
        }

        public static PointF Add(PointF a, PointF b) => new PointF(a.X + b.X, a.Y + b.Y);

        //  Shape conversions 
        public void ConvertToPolygonAutoBounds(int ellipseSegments = 32, bool close = true, bool autoOrient = false)
        {
            List<PointF> pts = new List<PointF>(64);

            switch (Shape)
            {
                case ShapeType.Polygon:
                    UpdateFromPolygon(autoOrient);
                    return;

                case ShapeType.Rectangle:
                    {
                        float hx = _rect.Width * 0.5f;
                        float hy = _rect.Height * 0.5f;
                        pts.Add(new PointF(-hx, -hy));
                        pts.Add(new PointF(+hx, -hy));
                        pts.Add(new PointF(+hx, +hy));
                        pts.Add(new PointF(-hx, +hy));
                        break;
                    }

                case ShapeType.Ellipse:
                    {
                        int n = Math.Max(4, ellipseSegments);
                        float rx = _rect.Width * 0.5f;
                        float ry = _rect.Height * 0.5f;
                        for (int i = 0; i < n; i++)
                        {
                            double t = 2.0 * Math.PI * i / n;
                            pts.Add(new PointF((float)(rx * Math.Cos(t)), (float)(ry * Math.Sin(t))));
                        }
                        break;
                    }

                case ShapeType.Hexagon:
                    {
                        var hex = GetHexagonVerticesLocal();
                        pts.AddRange(hex);
                        break;
                    }

                default:
                    {
                        float hx = _rect.Width * 0.5f;
                        float hy = _rect.Height * 0.5f;
                        pts.Add(new PointF(-hx, -hy));
                        pts.Add(new PointF(+hx, -hy));
                        pts.Add(new PointF(+hx, +hy));
                        pts.Add(new PointF(-hx, +hy));
                        break;
                    }
            }

            Shape = ShapeType.Polygon;
            if (PolyLocalPoints == null) PolyLocalPoints = new List<PointF>(pts.Count + 1);
            PolyLocalPoints.Clear();
            PolyLocalPoints.AddRange(pts);
            IsPolygonClosed = close;
            if (close && PolyLocalPoints.Count >= 1)
                PolyLocalPoints.Add(PolyLocalPoints[0]);
            ActiveVertexIndex = -1;

            UpdateFromPolygon(autoOrient);
        }

        public void ConvertHexToPolygon(bool closed = true, bool autoOrient = false)
        {
            if (Shape != ShapeType.Hexagon) return;
            ConvertToPolygonAutoBounds(ellipseSegments: 32, close: closed, autoOrient: autoOrient);
        }
        private  RectangleF BboxOfNoClose(IList<PointF> pts)
        {
            int n = pts?.Count ?? 0;
            if (n == 0) return RectangleF.Empty;

            // Bỏ điểm cuối nếu đã close
            int end = IsPolygonClosed ? n - 1 : n;

            float minx = float.PositiveInfinity, miny = float.PositiveInfinity;
            float maxx = float.NegativeInfinity, maxy = float.NegativeInfinity;
            for (int i = 0; i < end; i++)
            {
                var p = pts[i];
                if (p.X < minx) minx = p.X;
                if (p.Y < miny) miny = p.Y;
                if (p.X > maxx) maxx = p.X;
                if (p.Y > maxy) maxy = p.Y;
            }
            if (float.IsNaN(minx) || float.IsInfinity(minx)) return RectangleF.Empty;
            return RectangleF.FromLTRB(minx, miny, maxx, maxy);
        }

        // Tính PCA angle (độ) từ điểm local (bạn đã có bản cho C++).
        private  float PCAAngleDeg(IList<PointF> pts)
        {
            // Bản tối giản, dùng cov xy; nếu bạn đã có phiên bản chính xác hơn thì dùng lại bản đó.
            int n = pts?.Count ?? 0;
            if (n < 2) return 0f;
            int end = IsPolygonClosed ? n - 1 : n;

            double mx = 0, my = 0;
            for (int i = 0; i < end; i++) { mx += pts[i].X; my += pts[i].Y; }
            mx /= end; my /= end;

            double sxx = 0, sxy = 0, syy = 0;
            for (int i = 0; i < end; i++)
            {
                double dx = pts[i].X - mx, dy = pts[i].Y - my;
                sxx += dx * dx; sxy += dx * dy; syy += dy * dy;
            }
            // góc trục chính
            double ang = 0.5 * Math.Atan2(2 * sxy, (sxx - syy)); // rad
            return (float)(ang * 180.0 / Math.PI);
        }
        //public  void NormalizePolygonFrame( bool recomputeAngle)
        //{
        //    if ( this.PolyLocalPoints == null || this.PolyLocalPoints.Count < 3) return;
        //   PolyLocalPoints = UniquePolygonPoints(PolyLocalPoints.ToArray(), true).ToList();
        //    if (recomputeAngle)
        //    {
        //        float a = PCAAngleDeg(this.PolyLocalPoints); // độ
        //        this._rectRotation = a;
        //    }

        //    // Bbox theo local
        //    var bb = BboxOfNoClose(this.PolyLocalPoints);
        //    if (bb.IsEmpty) return;

        //    // Tâm local của polygon
        //    var cLocal = new PointF(bb.Left + bb.Width * 0.5f, bb.Top + bb.Height * 0.5f);

        //    // Dời tâm thế giới theo cLocal đã xoay theo _rectRotation
        //    if (Math.Abs(cLocal.X) > 1e-6f || Math.Abs(cLocal.Y) > 1e-6f)
        //    {
        //        var dcWorld = RectRotate.Rotate(cLocal,this._rectRotation);
        //       this._PosCenter = new PointF(this._PosCenter.X + dcWorld.X,this._PosCenter.Y + dcWorld.Y);

        //        // Kéo toàn bộ điểm về tâm (0,0)
        //        int end = IsPolygonClosed ?this.PolyLocalPoints.Count - 1 :this.PolyLocalPoints.Count;
        //        //for (int i = 0; i < end; i++)
        //        //{
        //        //    var p =this.PolyLocalPoints[i];
        //        //   this.PolyLocalPoints[i] = new PointF(p.X - cLocal.X, p.Y - cLocal.Y);
        //        //}
        //        // Nếu closed, đồng bộ điểm cuối = điểm đầu
        //        //if (IsPolygonClosed)
        //        //   this.PolyLocalPoints[this.PolyLocalPoints.Count - 1] =this.PolyLocalPoints[0];

        //        // Bbox mới quanh (0,0)
        //        bb = BboxOfNoClose(this.PolyLocalPoints);
        //    }

        //    // _rect luôn tâm tại (0,0) trong local
        //   this._rect = new RectangleF(-bb.Width * 0.5f, -bb.Height * 0.5f, bb.Width, bb.Height);
        //}
        public static bool PointInPolygon(IList<PointF> poly, PointF p)
        {
            if (poly == null || poly.Count < 3) return false;
            bool inside = false;
            int n = poly.Count;

            int max = n;
            if (n >= 2 && poly[0].Equals(poly[n - 1])) max = n - 1;

            for (int i = 0, j = max - 1; i < max; j = i++)
            {
                var pi = poly[i];
                var pj = poly[j];

                bool intersect = ((pi.Y > p.Y) != (pj.Y > p.Y)) &&
                                 (p.X < (pj.X - pi.X) * (p.Y - pi.Y) / ((pj.Y - pi.Y) == 0 ? 1e-6f : (pj.Y - pi.Y)) + pi.X);
                if (intersect) inside = !inside;
            }
            return inside;
        }
    }

}
