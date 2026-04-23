using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using BeeCore;
using BeeGlobal;
using Cyotek.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace BeeInterface.ShapeEditing
{
    [ToolboxItem(true)]
    public class ImageCanvasControl : ImageBox, IImageCanvas
    {
        private readonly List<IOverlayPainter> _overlays = new List<IOverlayPainter>();
        private Bitmap _currentBitmap;
        private bool _restoringNavigation;
        private int _lastAllowedZoom = 100;
        private Point _lastAllowedScrollPosition = Point.Empty;

        public ImageCanvasShapeContext ShapeContext { get; set; }
        public ShapeEditState EditState { get; } = new ShapeEditState();
        public ShapeEditOptions EditOptions { get; } = new ShapeEditOptions();
        public ImageCanvasInteractionState InteractionState { get; } = new ImageCanvasInteractionState();
        public IShapeRepository Repository { get; set; } = DefaultShapeRepository.Instance;
        public Point MouseDownPoint { get; private set; }
        public Color ColorChooe { get;  set; }
        public Point MouseMovePoint { get; private set; }
        public bool IsMouseDown { get; private set; }
        public bool IsDragging { get; private set; }
        public int DragThreshold { get; set; } = 4;
        public int MinimumZoom { get; set; } = 1;
        public float ScaleZoom { get; private set; } = 1f;
        public Point CanvasScrollPosition { get; private set; } = Point.Empty;
        public bool NavigationLocked => ShouldLockNavigation();

        public event EventHandler<CanvasViewportChangedArgs> ViewportChanged;

        public ImageCanvasControl()
        {
            BackColor = Color.White;
            GridDisplayMode = ImageBoxGridDisplayMode.None;
            AllowZoom = true;
            AllowClickZoom = false;
            _lastAllowedZoom = Zoom;
            _lastAllowedScrollPosition = AutoScrollPosition;
        }

        public void SetImage(Mat image)
        {
            var old = _currentBitmap;
            try
            {
                if (image == null || image.Empty())
                {
                    _currentBitmap = null;
                    Image = null;
                    return;
                }

                _currentBitmap = BitmapConverter.ToBitmap(image);
                Image = _currentBitmap;
            }
            finally
            {
                old?.Dispose();
            }
        }

        float IImageCanvas.Zoom => Zoom / 100f;

        public Point ScrollPosition
        {
            get
            {
                var p = AutoScrollPosition;
                return new Point(-p.X, -p.Y);
            }
        }

        public Size ViewportSize => ClientSize;

        public PointF ScreenToImage(Point clientPoint)
        {
            try
            {
                var p = PointToImage(clientPoint);
                return new PointF(p.X, p.Y);
            }
            catch
            {
                float z = Zoom <= 0 ? 1f : Zoom / 100f;
                return new PointF(
                    (clientPoint.X - AutoScrollPosition.X) / z,
                    (clientPoint.Y - AutoScrollPosition.Y) / z);
            }
        }

        public Point ImageToScreen(PointF imagePoint)
        {
            float z = Zoom <= 0 ? 1f : Zoom / 100f;
            return new Point(
                (int)(imagePoint.X * z + AutoScrollPosition.X),
                (int)(imagePoint.Y * z + AutoScrollPosition.Y));
        }

        public void AddOverlay(IOverlayPainter painter)
        {
            if (painter == null || _overlays.Contains(painter))
                return;

            _overlays.Add(painter);
            RequestRedraw();
        }

        public void RemoveOverlay(IOverlayPainter painter)
        {
            if (painter == null)
                return;

            if (_overlays.Remove(painter))
                RequestRedraw();
        }

        public void RequestRedraw()
        {
            Invalidate();
        }

        public void SetCursor(Cursor cursor)
        {
            Cursor = cursor ?? Cursors.Default;
        }

        public void SetPanEnabled(bool enabled)
        {
            PanMode = enabled && !ShouldLockNavigation() ? ImageBoxPanMode.Left : ImageBoxPanMode.None;
        }

        public void RefreshNavigationLock()
        {
            if (ShouldLockNavigation())
                RestoreNavigationViewport();
            else
            {
                AllowZoom = true;
                SetPanEnabled(ShapeContext != null && ShapeContext.PanEnabled);
                AllowClickZoom = true;
                AllowDoubleClick = true;
            }
        }

        public event EventHandler<CanvasMouseArgs> CanvasMouseDown;
        public event EventHandler<CanvasMouseArgs> CanvasMouseMove;
        public event EventHandler<CanvasMouseArgs> CanvasMouseUp;
        public event EventHandler<CanvasMouseArgs> CanvasMouseWheel;
        public event EventHandler<ShapeChangedArgs> ShapeChanged;

        private CanvasMouseArgs BuildCanvasMouseArgs(MouseEventArgs e)
        {
            return new CanvasMouseArgs(e.Location, ScreenToImage(e.Location), e.Button, e.Clicks, e.Delta);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseDownPoint = e.Location;
            MouseMovePoint = e.Location;
            IsMouseDown = true;
            IsDragging = false;
            InteractionState.MouseDownPoint = e.Location;
            InteractionState.MouseMovePoint = e.Location;
            InteractionState.IsMouseDown = true;
            InteractionState.IsDragging = false;
            OnShapeMouseDown(e);
            CanvasMouseDown?.Invoke(this, BuildCanvasMouseArgs(e));
            if (!ShouldLockNavigation())
                base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            MouseMovePoint = e.Location;
            InteractionState.MouseMovePoint = e.Location;
            if (IsMouseDown && !IsDragging)
            {
                int dx = e.X - MouseDownPoint.X;
                int dy = e.Y - MouseDownPoint.Y;
                IsDragging = Math.Abs(dx) >= DragThreshold || Math.Abs(dy) >= DragThreshold;
                InteractionState.IsDragging = IsDragging;
            }

            OnShapeMouseMove(e);
            CanvasMouseMove?.Invoke(this, BuildCanvasMouseArgs(e));
            if (!ShouldLockNavigation())
                base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            bool wasNavigationLocked = ShouldLockNavigation();
            OnShapeMouseUp(e);
            IsMouseDown = false;
            IsDragging = false;
            InteractionState.IsMouseDown = false;
            InteractionState.IsDragging = false;
            CanvasMouseUp?.Invoke(this, BuildCanvasMouseArgs(e));
            if (!wasNavigationLocked && !ShouldLockNavigation())
                base.OnMouseUp(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            CanvasMouseWheel?.Invoke(this, BuildCanvasMouseArgs(e));
            if (!ShouldLockNavigation())
                base.OnMouseWheel(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            OnShapePaint(e);

            if (_overlays.Count == 0)
                return;

            var snapshot = _overlays
                .Where(o => o != null && o.Visible)
                .OrderBy(o => o.ZOrder)
                .ToList();

            foreach (var painter in snapshot)
            {
                try
                {
                    painter.Paint(e.Graphics, this);
                }
                catch
                {
                    // Overlay paint must never break the image canvas.
                }
            }
        }

        private void OnShapeMouseDown(MouseEventArgs e)
        {
            HandleShapeMouseDown(e);
        }

        private bool HandleShapeMouseDown(MouseEventArgs e)
        {
            var ctx = ShapeContext;
            if (ctx == null)
                return false;

            if (ctx.WaitingForTool || ctx.IndexTool == -1 || ctx.Running)
                return true;

            if (!InteractionState.IsMouseDown && ctx.GettingColor)
                ctx.SetIsSetColor?.Invoke(false);

            InteractionState.MouseDownPoint = e.Location;
            InteractionState.IsMouseDown = true;
            InteractionState.IsDragging = false;
            InteractionState.MaybeCreate = false;
            InteractionState.CreatingNew = false;
            InteractionState.PreviewNew = null;

            if (ctx.Mode == StatusDraw.Scan)
            {
                ctx.SetIndexRotChoose?.Invoke(-1);
                if (InteractionState.IndexRotChoose >= 0)
                {
                    ctx.SetIndexRotChoose?.Invoke(InteractionState.IndexRotChoose);
                    return true;
                }
            }

            Invalidate();

            RectRotate rr = ctx.Current;
            if (rr == null)
                return true;

            if (ctx.Mode == StatusDraw.Check && rr._dragAnchor != AnchorPoint.None)
                ctx.SetMode?.Invoke(StatusDraw.Edit);

            if (rr.IsEmptyForCreate())
            {
                if (rr.Shape == ShapeType.Rectangle || rr.Shape == ShapeType.Ellipse || rr.Shape == ShapeType.Hexagon)
                {
                    InteractionState.IsNewShape = true;
                    InteractionState.MaybeCreate = true;
                    InteractionState.CreateStartImage = ScreenToImage(e.Location);
                }
            }

            if (rr._dragAnchor == AnchorPoint.Rotation)
                ctx.ShowAngleControl?.Invoke(e.Location);
            else if (rr._dragAnchor == AnchorPoint.Center)
                ctx.ShowCenterControl?.Invoke(e.Location);
            else
                ctx.HideAngleControl?.Invoke();

            using (var inv = GeometryHelper.BuildLocalInverseMatrixFor(rr, Zoom, AutoScrollPosition, false, PointF.Empty, 0f))
            {
                PointF pLocal = GeometryHelper.TransformPoint(inv, e.Location);

                if (rr.Shape != ShapeType.Polygon)
                    return true;

                float handle = ctx.HandleRadius;
                float closeTol = handle * 1.25f;

                if (!rr.IsPolygonClosed && (rr.PolyLocalPoints == null || rr.PolyLocalPoints.Count == 0))
                {
                    InteractionState.MaybeCreate = false;
                    InteractionState.CreatingNew = false;
                    InteractionState.PreviewNew = null;
                    InteractionState.PolygonDirtyDuringDrag = false;

                    UpdatePanZoomLock(ctx);
                    rr.ResetFrameForNewPolygonHard();
                }

                if (!rr.IsPolygonClosed)
                {
                    if (!rr.PolygonTryCloseIfNearFirst(pLocal, closeTol))
                        rr.PolygonAddPointLocal(pLocal);

                    InteractionState.PolygonDirtyDuringDrag = true;
                    InteractionState.IsDragging = false;
                    Invalidate();
                    return true;
                }

                rr.ActiveVertexIndex = -1;
                for (int i = rr.PolyLocalPoints.Count - 1; i >= 0; i--)
                {
                    PointF v = rr.PolyLocalPoints[i];
                    RectangleF handleRect = new RectangleF(v.X - handle / 2f, v.Y - handle / 2f, handle, handle);
                    if (!handleRect.Contains(pLocal))
                        continue;

                    rr.ActiveVertexIndex = i;
                    rr._dragAnchor = AnchorPoint.Vertex;
                    InteractionState.IsDragging = true;
                    InteractionState.DragRect = rr._rect;
                    InteractionState.DragCenter = rr._PosCenter;
                    InteractionState.DragStart = pLocal;
                    InteractionState.DragStartOffset = new PointF(pLocal.X - v.X, pLocal.Y - v.Y);
                    InteractionState.DragRotation = rr._rectRotation;
                    break;
                }

                Invalidate();
                return true;
            }
        }

        private void OnShapeMouseMove(MouseEventArgs e)
        {
            HandleShapeMouseMove(e);
        }

        private bool HandleShapeMouseMove(MouseEventArgs e)
        {
            var ctx = ShapeContext;
            if (ctx == null)
                return false;

            if (ctx.WaitingForTool)
                return true;

            if (ctx.IndexTool == -1 || ctx.Running)
            {
                ctx.HideAngleControl?.Invoke();
                return true;
            }

            Cursor = Cursors.Default;
            InteractionState.MouseMovePoint = e.Location;

            if (ctx.Mode == StatusDraw.Scan)
            {
                UpdateScanHitTest(ctx, e.Location);
                return true;
            }

            if (ctx.GettingColor)
            {
                Cursor = new Cursor(BeeInterface.Properties.Resources.Color_Dropper.Handle);
                AllowClickZoom = false;
                PanMode = ImageBoxPanMode.None;
                ctx.StartColorPicker?.Invoke();
                return true;
            }

            try
            {
                RectRotate rrSrc = ctx.Current;
                if (HandleCreatePreview(ctx, rrSrc, e.Location))
                    return true;

                if (rrSrc == null)
                    return true;

                if (InteractionState.IsDragging)
                    DragCurrentShape(ctx, rrSrc, e.Location);
                else
                    HitTestCurrentShape(ctx, rrSrc, e.Location);

                UpdatePanZoomLock(ctx);
                Invalidate();
            }
            catch
            {
                // Keep the image canvas responsive while the legacy editor is being migrated.
            }

            return true;
        }

        private void UpdateScanHitTest(ImageCanvasShapeContext ctx, Point screenPoint)
        {
            InteractionState.IndexRotChoose = -1;
            ctx.SetIndexRotChoose?.Invoke(-1);

            var tool = Repository?.GetTool(ctx.IndexProg, ctx.IndexTool);
            object propety2 = GetPropertyValue<object>(tool, "Propety2");
            if (tool == null || propety2 == null || ctx.Current == null)
                return;

            List<RectRotate> listScan = null;
            switch (ctx.Current.TypeCrop)
            {
                case TypeCrop.Limit:
                    listScan = GetPropertyValue<List<RectRotate>>(propety2, "listRotScan");
                    break;
                case TypeCrop.Mask:
                    listScan = GetPropertyValue<List<RectRotate>>(propety2, "ListRotMask");
                    break;
            }

            RectRotate rotArea = GetPropertyValue<RectRotate>(propety2, "rotArea");
            if (listScan == null || rotArea == null)
                return;

            float s = Zoom <= 0 ? 1f : Zoom / 100f;
            PointF pWorld = new PointF(
                (screenPoint.X - AutoScrollPosition.X) / s,
                (screenPoint.Y - AutoScrollPosition.Y) / s);
            PointF pLocalArea = rotArea.WorldToLocal(pWorld);

            for (int i = 0; i < listScan.Count; i++)
            {
                RectRotate rot = listScan[i];
                if (rot == null || !rot.ContainsPoint(pLocalArea))
                    continue;
               
                    
                InteractionState.IndexRotChoose = i;
             //   ctx.SetIndexRotChoose?.Invoke(i);
                break;
            }
        }

        private bool HandleCreatePreview(ImageCanvasShapeContext ctx, RectRotate rrSrc, Point screenPoint)
        {
            if (!InteractionState.IsDragging || !InteractionState.MaybeCreate ||
                ((rrSrc != null ? rrSrc._dragAnchor : AnchorPoint.None) != AnchorPoint.None))
                return false;

            InteractionState.CreateEndImage = ScreenToImage(screenPoint);
            if (InteractionState.CreateEndImage.X <= InteractionState.CreateStartImage.X)
            {
                InteractionState.PreviewNew = null;
                InteractionState.CreatingNew = false;
                return false;
            }

            float w = Math.Max(1f, InteractionState.CreateEndImage.X - InteractionState.CreateStartImage.X);
            float yTop = Math.Min(InteractionState.CreateStartImage.Y, InteractionState.CreateEndImage.Y);
            float yBot = Math.Max(InteractionState.CreateStartImage.Y, InteractionState.CreateEndImage.Y);
            float h = Math.Max(1f, yBot - yTop);
            var center = new PointF(InteractionState.CreateStartImage.X + w / 2f, yTop + h / 2f);

            ShapeType shape = rrSrc != null ? rrSrc.Shape : ShapeType.Rectangle;
            if (shape != ShapeType.Rectangle && shape != ShapeType.Ellipse && shape != ShapeType.Hexagon)
                shape = ShapeType.Rectangle;

            var rrNew = new RectRotate(new RectangleF(-w / 2f, -h / 2f, w, h), center, 0f, AnchorPoint.None)
            {
                TypeCrop = rrSrc != null ? rrSrc.TypeCrop : TypeCrop.Crop,
                Name = rrSrc != null ? rrSrc.Name : null,
                Shape = shape
            };

            InteractionState.PreviewNew = rrNew;
            InteractionState.CreatingNew = true;
            ctx.SetCurrent?.Invoke(rrNew);
            Invalidate();
            return true;
        }

        private void DragCurrentShape(ImageCanvasShapeContext ctx, RectRotate rrSrc, Point screenPoint)
        {
            RectRotate rotateRect = CloneRectRotate(rrSrc);
            SetCursorForAnchor(rotateRect._dragAnchor);

            using (var mat = GeometryHelper.BuildLocalInverseMatrixFor(
                       rotateRect,
                       Zoom,
                       AutoScrollPosition,
                       true,
                       InteractionState.DragCenter,
                       InteractionState.DragRotation))
            {
                PointF point = GeometryHelper.TransformPoint(mat, screenPoint);

                switch (rotateRect._dragAnchor)
                {
                    case AnchorPoint.TopLeft:
                    case AnchorPoint.TopRight:
                    case AnchorPoint.BottomLeft:
                    case AnchorPoint.BottomRight:
                        ResizeFromCorner(rotateRect, point);
                        break;
                    case AnchorPoint.Rotation:
                        RotateShape(rotateRect, point);
                        break;
                    case AnchorPoint.Center:
                        MoveShape(rotateRect, point);
                        break;
                    case AnchorPoint.V0:
                    case AnchorPoint.V1:
                    case AnchorPoint.V2:
                    case AnchorPoint.V3:
                    case AnchorPoint.V4:
                    case AnchorPoint.V5:
                        MoveHexVertex(rotateRect, point);
                        break;
                    case AnchorPoint.Vertex:
                        MovePolygonVertex(rotateRect, point);
                        break;
                }
            }

            ClampMoveToImage(ctx, rotateRect);
            ctx.SetCurrent?.Invoke(CloneRectRotate(rotateRect));
        }

        private void HitTestCurrentShape(ImageCanvasShapeContext ctx, RectRotate rrSrc, Point screenPoint)
        {
            RectRotate rotateRect = CloneRectRotate(rrSrc);
            using (var mat = GeometryHelper.BuildLocalInverseMatrixFor(
                       rotateRect,
                       Zoom,
                       AutoScrollPosition,
                       false,
                       PointF.Empty,
                       0f))
            {
                PointF point = GeometryHelper.TransformPoint(mat, screenPoint);
                RectangleF baseRect = rotateRect._rect;
                RectangleF bounds = rotateRect.Shape == ShapeType.Polygon &&
                                    rotateRect.PolyLocalPoints != null &&
                                    rotateRect.PolyLocalPoints.Count >= 3
                    ? GeometryHelper.BboxOf(rotateRect.PolyLocalPoints)
                    : baseRect;

                float r = ctx.HandleRadius;
                RectangleF rectOuter = new RectangleF(bounds.X - r / 2f, bounds.Y - r / 2f, bounds.Width + r, bounds.Height + r);
                RectangleF rectTopLeft = new RectangleF(bounds.Left - r / 2f, bounds.Top - r / 2f, r, r);
                RectangleF rectTopRight = new RectangleF(bounds.Right - r / 2f, bounds.Top - r / 2f, r, r);
                RectangleF rectBottomLeft = new RectangleF(bounds.Left - r / 2f, bounds.Bottom - r / 2f, r, r);
                RectangleF rectBottomRight = new RectangleF(bounds.Right - r / 2f, bounds.Bottom - r / 2f, r, r);
                RectangleF rectRotate = new RectangleF(-r / 2f, bounds.Top - 2f * r, 2f * r, 2f * r);

                InteractionState.DragCenter = rotateRect._PosCenter;
                bool anchored = false;

                if (rotateRect.Shape == ShapeType.Polygon)
                    anchored = HitTestPolygon(rotateRect, point, bounds, rectRotate, r);

                if (!anchored && rotateRect.Shape == ShapeType.Hexagon)
                    anchored = HitTestHexagon(rotateRect, point, baseRect, r);

                if (!anchored && rotateRect.Shape != ShapeType.Polygon)
                    HitTestBox(rotateRect, point, baseRect, rectOuter, rectTopLeft, rectTopRight, rectBottomLeft, rectBottomRight, rectRotate);
            }

            rrSrc._dragAnchor = rotateRect._dragAnchor;
            rrSrc.ActiveVertexIndex = rotateRect.ActiveVertexIndex;
        }

        private bool HitTestPolygon(RectRotate rotateRect, PointF point, RectangleF bounds, RectangleF rectRotate, float r)
        {
            if (!rotateRect.IsPolygonClosed)
            {
                rotateRect._dragAnchor = AnchorPoint.None;
                rotateRect.ActiveVertexIndex = -1;
                return true;
            }

            for (int i = 0; i < rotateRect.PolyLocalPoints.Count; i++)
            {
                var h = new RectangleF(rotateRect.PolyLocalPoints[i].X - r / 2f, rotateRect.PolyLocalPoints[i].Y - r / 2f, r, r);
                if (!h.Contains(point))
                    continue;

                BeginDrag(rotateRect, AnchorPoint.Vertex, point, bounds);
                rotateRect.ActiveVertexIndex = i;
                return true;
            }

            if (rectRotate.Contains(point))
            {
                BeginRotation(rotateRect, point, bounds);
                return true;
            }

            if (RectRotate.PointInPolygon(rotateRect.PolyLocalPoints, point))
            {
                BeginDrag(rotateRect, AnchorPoint.Center, point, RectangleF.Empty);
                InteractionState.DragStartOffset = point;
                return true;
            }

            rotateRect._dragAnchor = AnchorPoint.None;
            rotateRect.ActiveVertexIndex = -1;
            return true;
        }

        private bool HitTestHexagon(RectRotate rotateRect, PointF point, RectangleF baseRect, float r)
        {
            var verts = rotateRect.GetHexagonVerticesLocal();
            for (int i = 0; i < 6; i++)
            {
                var h = new RectangleF(verts[i].X - r / 2f, verts[i].Y - r / 2f, r, r);
                if (!h.Contains(point))
                    continue;

                BeginDrag(rotateRect, (AnchorPoint)((int)AnchorPoint.V0 + i), point, baseRect);
                return true;
            }

            return false;
        }

        private void HitTestBox(
            RectRotate rotateRect,
            PointF point,
            RectangleF baseRect,
            RectangleF rectOuter,
            RectangleF rectTopLeft,
            RectangleF rectTopRight,
            RectangleF rectBottomLeft,
            RectangleF rectBottomRight,
            RectangleF rectRotate)
        {
            if (rectTopLeft.Contains(point))
                BeginDrag(rotateRect, AnchorPoint.TopLeft, point, baseRect);
            else if (rectTopRight.Contains(point))
                BeginDrag(rotateRect, AnchorPoint.TopRight, point, baseRect);
            else if (rectBottomLeft.Contains(point))
                BeginDrag(rotateRect, AnchorPoint.BottomLeft, point, baseRect);
            else if (rectBottomRight.Contains(point))
                BeginDrag(rotateRect, AnchorPoint.BottomRight, point, baseRect);
            else if (rectRotate.Contains(point))
                BeginRotation(rotateRect, point, baseRect);
            else if (rectOuter.Contains(point))
            {
                BeginDrag(rotateRect, AnchorPoint.Center, point, baseRect);
                InteractionState.DragStartOffset = point;
            }
            else
                rotateRect._dragAnchor = AnchorPoint.None;
        }

        private void BeginDrag(RectRotate rotateRect, AnchorPoint anchor, PointF point, RectangleF dragRect)
        {
            InteractionState.DragStart = point;
            InteractionState.DragRect = dragRect;
            InteractionState.DragCenter = rotateRect._PosCenter;
            InteractionState.DragRotation = rotateRect._rectRotation;
            rotateRect._dragAnchor = anchor;
        }

        private void BeginRotation(RectRotate rotateRect, PointF point, RectangleF dragRect)
        {
            BeginDrag(rotateRect, AnchorPoint.Rotation, point, dragRect);
            InteractionState.RotateStartAngleLocal = (float)Math.Atan2(point.Y, point.X);
            InteractionState.RotateBase = rotateRect._rectRotation;
        }

        private void ResizeFromCorner(RectRotate rotateRect, PointF point)
        {
            float dx = point.X - InteractionState.DragStart.X;
            float dy = point.Y - InteractionState.DragStart.Y;
            float left = InteractionState.DragRect.Left;
            float top = InteractionState.DragRect.Top;
            float right = InteractionState.DragRect.Right;
            float bottom = InteractionState.DragRect.Bottom;

            switch (rotateRect._dragAnchor)
            {
                case AnchorPoint.TopLeft:
                    left += dx;
                    top += dy;
                    break;
                case AnchorPoint.TopRight:
                    right += dx;
                    top += dy;
                    break;
                case AnchorPoint.BottomLeft:
                    left += dx;
                    bottom += dy;
                    break;
                case AnchorPoint.BottomRight:
                    right += dx;
                    bottom += dy;
                    break;
            }

            const float minSize = 2f;
            if (right - left < minSize)
            {
                if (rotateRect._dragAnchor == AnchorPoint.TopLeft || rotateRect._dragAnchor == AnchorPoint.BottomLeft)
                    left = right - minSize;
                else
                    right = left + minSize;
            }

            if (bottom - top < minSize)
            {
                if (rotateRect._dragAnchor == AnchorPoint.TopLeft || rotateRect._dragAnchor == AnchorPoint.TopRight)
                    top = bottom - minSize;
                else
                    bottom = top + minSize;
            }

            float w = right - left;
            float h = bottom - top;
            float localCenterX = (left + right) * 0.5f;
            float localCenterY = (top + bottom) * 0.5f;

            rotateRect._rect = new RectangleF(-w / 2f, -h / 2f, w, h);
            PointF worldDelta = RectRotate.Rotate(new PointF(localCenterX, localCenterY), InteractionState.DragRotation);
            rotateRect._PosCenter = new PointF(InteractionState.DragCenter.X + worldDelta.X, InteractionState.DragCenter.Y + worldDelta.Y);
        }

        private void RotateShape(RectRotate rotateRect, PointF point)
        {
            if (rotateRect.Shape == ShapeType.Polygon)
                return;

            float angNow = (float)Math.Atan2(point.Y, point.X);
            float deltaDeg = (float)((angNow - InteractionState.RotateStartAngleLocal) * 180.0 / Math.PI);
            while (deltaDeg > 180f) deltaDeg -= 360f;
            while (deltaDeg < -180f) deltaDeg += 360f;

            rotateRect._rectRotation = InteractionState.RotateBase + deltaDeg;
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                const float snap = 15f;
                rotateRect._rectRotation = (float)Math.Round(rotateRect._rectRotation / snap) * snap;
            }
        }

        private void MoveShape(RectRotate rotateRect, PointF point)
        {
            PointF localNewCenter = new PointF(point.X - InteractionState.DragStartOffset.X, point.Y - InteractionState.DragStartOffset.Y);
            PointF worldDelta = RectRotate.Rotate(localNewCenter, InteractionState.DragRotation);
            rotateRect._PosCenter = new PointF(InteractionState.DragCenter.X + worldDelta.X, InteractionState.DragCenter.Y + worldDelta.Y);

            if (rotateRect.Shape == ShapeType.Polygon)
                rotateRect.UpdateFromPolygon(false);
        }

        private void MoveHexVertex(RectRotate rotateRect, PointF point)
        {
            if (rotateRect.Shape != ShapeType.Hexagon)
                return;

            int idx = (int)rotateRect._dragAnchor - (int)AnchorPoint.V0;
            rotateRect.SetHexVertexByLocalPoint(idx, point);
            if (rotateRect.AutoExpandBounds)
                rotateRect.RefitBoundsToHexagon();
        }

        private void MovePolygonVertex(RectRotate rotateRect, PointF point)
        {
            if (rotateRect.Shape != ShapeType.Polygon || rotateRect.ActiveVertexIndex < 0)
                return;

            int idx = rotateRect.ActiveVertexIndex;
            if (idx >= rotateRect.PolyLocalPoints.Count)
                return;

            rotateRect.PolyLocalPoints[idx] = point;
            rotateRect.UpdateFromPolygon(false);
        }

        private void ClampMoveToImage(ImageCanvasShapeContext ctx, RectRotate rotateRect)
        {
            if (rotateRect._dragAnchor != AnchorPoint.Center || rotateRect.Shape == ShapeType.Polygon)
                return;

            Mat raw = Repository?.GetRawMat(ctx.IndexCamera);
            if (raw == null || raw.Empty())
                return;

            int maxW = raw.Width;
            int maxH = raw.Height;
            float hw = rotateRect._rect.Width / 2f;
            float hh = rotateRect._rect.Height / 2f;
            var corners = new[]
            {
                new PointF(-hw, -hh),
                new PointF(hw, -hh),
                new PointF(hw, hh),
                new PointF(-hw, hh)
            };

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (PointF corner in corners)
            {
                PointF r = RectRotate.Rotate(corner, rotateRect._rectRotation);
                float wx = rotateRect._PosCenter.X + r.X;
                float wy = rotateRect._PosCenter.Y + r.Y;
                if (wx < minX) minX = wx;
                if (wy < minY) minY = wy;
                if (wx > maxX) maxX = wx;
                if (wy > maxY) maxY = wy;
            }

            float shiftX = 0f;
            float shiftY = 0f;
            if (minX < 0) shiftX += -minX;
            if (maxX > maxW) shiftX += maxW - maxX;
            if (minY < 0) shiftY += -minY;
            if (maxY > maxH) shiftY += maxH - maxY;

            rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X + shiftX, rotateRect._PosCenter.Y + shiftY);
        }

        private void UpdatePanZoomLock(ImageCanvasShapeContext ctx)
        {
            bool locked = ShouldLockNavigation();

            if (locked)
            {
                PanMode = ImageBoxPanMode.None;
                AllowZoom = false;
                AllowClickZoom = false;
                AllowDoubleClick = false;
            }
            else
            {
                AllowZoom = true;
                SetPanEnabled(ctx.PanEnabled);
                AllowClickZoom = true;
                AllowDoubleClick = true;
            }
        }

        private bool ShouldLockNavigation()
        {
            var ctx = ShapeContext;
            if (ctx == null)
                return false;

            RectRotate current = ctx.Current;
            bool editingMode = ctx.Mode == StatusDraw.Edit;
            //bool hasEditShape = editingMode && current != null &&
            //                    (current._rect.Width > 0 ||
            //                     current._rect.Height > 0 ||
            //                     current.Shape == ShapeType.Polygon ||
            //                     current.Shape == ShapeType.Hexagon);
            bool editingInteraction = editingMode &&
                                      (InteractionState.IsMouseDown ||
                                       InteractionState.IsDragging ||
                                       InteractionState.IsNewShape ||
                                       InteractionState.MaybeCreate ||
                                       InteractionState.CreatingNew ||
                                       (current != null && current._dragAnchor != AnchorPoint.None));

            return ctx.GettingColor ||
                    
                   editingInteraction;
        }

        private void RestoreNavigationViewport()
        {
            if (_restoringNavigation)
                return;

            try
            {
                _restoringNavigation = true;
                PanMode = ImageBoxPanMode.None;
                AllowZoom = false;
                AllowClickZoom = false;
                AllowDoubleClick = false;

                if (Zoom != _lastAllowedZoom)
                    Zoom = _lastAllowedZoom;

                Point current = AutoScrollPosition;
                if (current != _lastAllowedScrollPosition)
                    AutoScrollPosition = new Point(-_lastAllowedScrollPosition.X, -_lastAllowedScrollPosition.Y);
            }
            finally
            {
                _restoringNavigation = false;
            }
        }

        private void SetCursorForAnchor(AnchorPoint anchor)
        {
            switch (anchor)
            {
                case AnchorPoint.Center:
                    Cursor = Cursors.Hand;
                    break;
                case AnchorPoint.BottomLeft:
                case AnchorPoint.TopRight:
                    Cursor = Cursors.SizeNESW;
                    break;
                case AnchorPoint.TopLeft:
                case AnchorPoint.BottomRight:
                    Cursor = Cursors.SizeNWSE;
                    break;
                case AnchorPoint.Rotation:
                    Cursor = new Cursor(BeeInterface.Properties.Resources.Rotate.Handle);
                    break;
                default:
                    Cursor = Cursors.Default;
                    break;
            }
        }

        private static RectRotate CloneRectRotate(RectRotate src)
        {
            if (src == null)
                return null;

            var clone = new RectRotate(src._rect, src._PosCenter, src._rectRotation, src._dragAnchor)
            {
                Name = src.Name,
                TypeCrop = src.TypeCrop,
                Shape = src.Shape,
                IsPolygonClosed = src.IsPolygonClosed,
                ActiveVertexIndex = src.ActiveVertexIndex,
                AutoExpandBounds = src.AutoExpandBounds
            };

            if (src.HexVertexOffsets != null)
            {
                for (int i = 0; i < clone.HexVertexOffsets.Length && i < src.HexVertexOffsets.Length; i++)
                    clone.HexVertexOffsets[i] = src.HexVertexOffsets[i];
            }

            clone.PolyLocalPoints.Clear();
            if (src.PolyLocalPoints != null)
            {
                for (int i = 0; i < src.PolyLocalPoints.Count; i++)
                    clone.PolyLocalPoints.Add(src.PolyLocalPoints[i]);
            }

            return clone;
        }

        private static T GetPropertyValue<T>(object source, string name)
        {
            if (source == null)
                return default(T);

            var property = source.GetType().GetProperty(name);
            if (property != null)
            {
                object value = property.GetValue(source, null);
                if (value is T typed)
                    return typed;
            }

            var field = source.GetType().GetField(name);
            if (field != null)
            {
                object value = field.GetValue(source);
                if (value is T typed)
                    return typed;
            }

            return default(T);
        }

        private static object InvokeMethod(object source, string name, params object[] args)
        {
            if (source == null)
                return null;

            var method = source.GetType().GetMethod(name);
            return method == null ? null : method.Invoke(source, args);
        }

        private static object InvokeStaticMethod(Type type, string name, params object[] args)
        {
            if (type == null)
                return null;

            var methods = type.GetMethods()
                .Where(m => m.Name == name && m.GetParameters().Length == args.Length)
                .ToList();

            foreach (var method in methods)
            {
                try
                {
                    return method.Invoke(null, args);
                }
                catch
                {
                }
            }

            return null;
        }

        private void OnShapeMouseUp(MouseEventArgs e)
        {
            HandleShapeMouseUp(e);
        }

        private bool HandleShapeMouseUp(MouseEventArgs e)
        {
            var ctx = ShapeContext;
            if (ctx == null)
                return false;

            bool wasMouseDown = InteractionState.IsMouseDown;
            bool wasCreating = InteractionState.CreatingNew;
            bool wasPolygonDirty = InteractionState.PolygonDirtyDuringDrag;
            RectRotate current = ctx.Current;
            AnchorPoint committedAnchor = current != null ? current._dragAnchor : AnchorPoint.None;

            if (wasMouseDown && ctx.GettingColor)
            {
                ctx.SetIsSetColor?.Invoke(false);
                ctx.SetIsSetColor?.Invoke(true);
               
            }
                
               

            if (ctx.WaitingForTool)
            {
                ResetInteractionAfterMouseUp(ctx);
                return true;
            }

            if (ctx.IndexTool == -1 || ctx.Running)
            {
                ResetInteractionAfterMouseUp(ctx);
                return true;
            }

            if (wasCreating)
            {
                CommitCreatedShape(ctx);
                ResetInteractionAfterMouseUp(ctx);
                Invalidate();
                return true;
            }

            try
            {
                current = ctx.Current;
                if (current != null)
                {
                    if (current.Shape == ShapeType.Polygon && wasPolygonDirty)
                    {
                        if (current.IsPolygonClosed)
                            current.UpdateFromPolygon(updateAngle: current.AutoOrientPolygon);

                        ShapeChanged?.Invoke(
                            this,
                            new ShapeChangedArgs(
                                CloneRectRotate(current),
                                current.TypeCrop,
                                current.IsPolygonClosed ? ShapeChangeKind.PolygonClosed : ShapeChangeKind.VertexAdded));
                    }
                    else if (committedAnchor != AnchorPoint.None)
                    {
                        ShapeChanged?.Invoke(this, new ShapeChangedArgs(CloneRectRotate(current), current.TypeCrop, GetChangeKind(committedAnchor)));
                    }

                    current._dragAnchor = AnchorPoint.None;
                    current.ActiveVertexIndex = -1;
                    ctx.SetCurrent?.Invoke(current);
                }
            }
            catch
            {
                // Keep mouse-up cleanup reliable while the old editor is being retired.
            }

            ResetInteractionAfterMouseUp(ctx);
            Invalidate();
            return true;
        }

        private void CommitCreatedShape(ImageCanvasShapeContext ctx)
        {
            const float minSize = 3f;
            RectRotate rr = ctx.Current;
            if (rr != null && rr._rect.Width >= minSize && rr._rect.Height >= minSize)
            {
                rr._dragAnchor = AnchorPoint.None;
                rr.ActiveVertexIndex = -1;
                ctx.SetCurrent?.Invoke(rr);
                ShapeChanged?.Invoke(this, new ShapeChangedArgs(CloneRectRotate(rr), rr.TypeCrop, ShapeChangeKind.Created));
                return;
            }

            InteractionState.PreviewNew = null;
            InteractionState.CreatingNew = false;
        }

        private void ResetInteractionAfterMouseUp(ImageCanvasShapeContext ctx)
        {
            InteractionState.IsMouseDown = false;
            InteractionState.IsDragging = false;
            InteractionState.IsNewShape = false;
            InteractionState.MaybeCreate = false;
            InteractionState.CreatingNew = false;
            InteractionState.PolygonDirtyDuringDrag = false;
            InteractionState.PreviewNew = null;

            RectRotate current = ctx.Current;
            if (!ShouldLockNavigation() && current != null && current._dragAnchor == AnchorPoint.None)
            {
                AllowZoom = true;
                SetPanEnabled(ctx.PanEnabled);
                AllowClickZoom = true;
                AllowDoubleClick = true;
            }
            else
            {
                UpdatePanZoomLock(ctx);
            }
        }

        private static ShapeChangeKind GetChangeKind(AnchorPoint anchor)
        {
            switch (anchor)
            {
                case AnchorPoint.Center:
                    return ShapeChangeKind.Moved;
                case AnchorPoint.Rotation:
                    return ShapeChangeKind.Rotated;
                case AnchorPoint.Vertex:
                case AnchorPoint.V0:
                case AnchorPoint.V1:
                case AnchorPoint.V2:
                case AnchorPoint.V3:
                case AnchorPoint.V4:
                case AnchorPoint.V5:
                    return ShapeChangeKind.VertexMoved;
                case AnchorPoint.TopLeft:
                case AnchorPoint.TopRight:
                case AnchorPoint.BottomLeft:
                case AnchorPoint.BottomRight:
                    return ShapeChangeKind.Resized;
                default:
                    return ShapeChangeKind.Preview;
            }
        }

        private void OnShapePaint(PaintEventArgs e)
        {
            HandleShapePaint(e);
        }

        private void HandleShapePaint(PaintEventArgs e)
        {
            var ctx = ShapeContext;
            if (ctx == null)
                return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (!ctx.Live && ctx.Running)
            {
                ctx.HideAngleControl?.Invoke();
                return;
            }

            Config config = ctx.Config;
            ParaShow para = ctx.ParaShow;
            if (config == null || para == null)
                return;

            EnsureImageSize(ctx, config);
            DrawLiveBadge(ctx, g, para);
            DrawGrid(g, config);
            DrawCenterCross(g, config);
            DrawAllAreas(ctx, g, config, para);

            if (ctx.Running || ctx.IndexTool == -1 || Image == null)
                return;

            if (ctx.Mode == StatusDraw.Check || ctx.Mode == StatusDraw.Scan)
                DrawToolResult(ctx, g);
            else if (ctx.Mode == StatusDraw.Edit)
                DrawEditOverlay(ctx, g, para);
            else if (ctx.Mode == StatusDraw.None)
                ctx.HideAngleControl?.Invoke();

            DrawColorSample(ctx, g);
        }

        private void EnsureImageSize(ImageCanvasShapeContext ctx, Config config)
        {
            if (config.SizeCCD.Width != 0 || ctx.Live)
                return;

            var camera = Repository?.GetCamera(ctx.IndexCamera);
            if (camera != null)
                config.SizeCCD = camera.GetSzCCD();
        }

        private static void DrawLiveBadge(ImageCanvasShapeContext ctx, Graphics g, ParaShow para)
        {
            if (!ctx.Live)
                return;

            using (var font = new Font("Arial", para.FontSize, FontStyle.Bold))
                g.DrawString("LIVE", font, Brushes.Red, new Point(50, 50));
        }

        private static void DrawGrid(Graphics g, Config config)
        {
            if (!config.IsShowGird)
                return;

            int w = config.SizeCCD.Width;
            int h = config.SizeCCD.Height;
            if (w <= 0 || h <= 0)
                return;

            int step = Math.Min(w, h) / 15;
            if (step <= 0)
                return;

            using (var pen = new Pen(Brushes.Gray, 1))
            {
                for (int x = step; x < w; x += step)
                    g.DrawLine(pen, x, 0, x, h);
                for (int y = step; y < h; y += step)
                    g.DrawLine(pen, 0, y, w, y);
            }
        }

        private static void DrawCenterCross(Graphics g, Config config)
        {
            if (!config.IsShowCenter)
                return;

            int w = config.SizeCCD.Width;
            int h = config.SizeCCD.Height;
            if (w <= 0 || h <= 0)
                return;

            using (var pen = new Pen(Brushes.Blue, 1))
            {
                g.DrawLine(pen, w / 2, 0, w / 2, h);
                g.DrawLine(pen, 0, h / 2, w, h / 2);
            }
        }

        private void DrawAllAreas(ImageCanvasShapeContext ctx, Graphics g, Config config, ParaShow para)
        {
            if (!config.IsShowArea)
                return;

            var tools = Repository?.GetPropetyTools(ctx.IndexProg);
            if (tools == null)
                return;

            int indexTool = 0;
            foreach (var tool in tools)
            {
                object propety2 = GetPropertyValue<object>(tool, "Propety2");
                RectRotate rot = GetPropertyValue<RectRotate>(propety2, "rotArea");
                if (rot == null)
                {
                    indexTool++;
                    continue;
                }

                using (var mat = BuildWorldMatrix(rot))
                using (var pen = new Pen(para.ColorInfor, para.ThicknessLine))
                using (var font = new Font("Arial", para.FontSize, FontStyle.Bold))
                using (var fill = new SolidBrush(para.ColorInfor))
                using (var text = new SolidBrush(para.TextColor))
                {
                    g.Transform = mat;
                    RectangleF rect = rot._rect;
                    g.DrawRectangle(pen, new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));
                    string label = (indexTool + 1) + "." + GetPropertyValue<string>(tool, "Name");
                    SizeF size = g.MeasureString(label, font);
                    g.FillRectangle(fill, new Rectangle((int)rect.X, (int)rect.Y, (int)size.Width, (int)size.Height));
                    g.DrawString(label, font, text, new Point((int)rect.X, (int)rect.Y));
                    g.ResetTransform();
                }

                indexTool++;
            }
        }

        private void DrawToolResult(ImageCanvasShapeContext ctx, Graphics g)
        {
            ctx.HideAngleControl?.Invoke();
            g.ResetTransform();
            UpdateViewportState();

            var tool = Repository?.GetTool(ctx.IndexProg, ctx.IndexTool);
            object propety2 = GetPropertyValue<object>(tool, "Propety2");
            InvokeMethod(propety2, "DrawResult", g);

            double cycleTime = Convert.ToDouble(GetPropertyValue<object>(tool, "CycleTime") ?? 0d);
            Results result = GetPropertyValue<Results>(tool, "Results");
            ctx.UpdateResultInfo?.Invoke(Math.Round(cycleTime) + "ms", result.ToString(), result);
        }

        private void DrawEditOverlay(ImageCanvasShapeContext ctx, Graphics g, ParaShow para)
        {
            var tool = Repository?.GetTool(ctx.IndexProg, ctx.IndexTool);
            object propety2 = GetPropertyValue<object>(tool, "Propety2");
            RectRotate current = ctx.Current;
            if (current == null)
                return;

            try
            {
                switch (current.TypeCrop)
                {
                    case TypeCrop.Crop:
                        Draws.FillRect(g, TypeCrop.Area, GetPropertyValue<RectRotate>(propety2, "rotArea"), AutoScrollPosition, Zoom, 20);
                        Draws.FillRect(g, TypeCrop.Mask, GetPropertyValue<RectRotate>(propety2, "rotMask"), AutoScrollPosition, Zoom, 50);
                        break;
                    case TypeCrop.Area:
                        Draws.FillRect(g, TypeCrop.Crop, GetPropertyValue<RectRotate>(propety2, "rotCrop"), AutoScrollPosition, Zoom, 20);
                        Draws.FillRect(g, TypeCrop.Mask, GetPropertyValue<RectRotate>(propety2, "rotMask"), AutoScrollPosition, Zoom, 50);
                        break;
                    case TypeCrop.Mask:
                        Draws.FillRect(g, TypeCrop.Area, GetPropertyValue<RectRotate>(propety2, "rotArea"), AutoScrollPosition, Zoom, 20);
                        Draws.FillRect(g, TypeCrop.Crop, GetPropertyValue<RectRotate>(propety2, "rotCrop"), AutoScrollPosition, Zoom, 50);
                        break;
                }
            }
            catch
            {
            }

            InvokeStaticMethod(
                typeof(Draws),
                "RectEdit",
                g,
                TypeCrop.Crop,
                current,
                (int)para.RadEdit,
                AutoScrollPosition,
                Zoom,
                InteractionState.MouseMovePoint,
                4);
            DrawScanMasks(propety2, g);
            g.ResetTransform();
        }

        private void DrawScanMasks(object propety2, Graphics g)
        {
            try
            {
                RectRotate rotArea = GetPropertyValue<RectRotate>(propety2, "rotArea");
                var listScan = GetPropertyValue<List<RectRotate>>(propety2, "listRotScan");
                if (listScan != null)
                    InvokeStaticMethod(typeof(Draws), "FillListRectMask", g, Color.Gold, listScan, rotArea, AutoScrollPosition, Zoom, 30);

                var listMark = GetPropertyValue<List<RectRotate>>(propety2, "ListRotMask");
                if (listMark != null)
                    InvokeStaticMethod(typeof(Draws), "FillListRectMask", g, Color.Red, listMark, rotArea, AutoScrollPosition, Zoom, 30);
            }
            catch
            {
            }
        }

        private void DrawColorSample(ImageCanvasShapeContext ctx, Graphics g)
        {
            if (!ctx.GettingColor)
                return;

            Color color = ctx._clChoose;
            using (var pen = new Pen(color, 5))
            {
                Point p = InteractionState.MouseMovePoint;
                g.ResetTransform();
                g.DrawEllipse(pen, new Rectangle(new Point(p.X - 25, p.Y - 25), new Size(50, 50)));
            }
        }

        private Matrix BuildWorldMatrix(RectRotate rot)
        {
            var mat = new Matrix();
            mat.Translate(AutoScrollPosition.X, AutoScrollPosition.Y);
            mat.Scale(Zoom / 100f, Zoom / 100f);
            mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
            mat.Rotate(rot._rectRotation);
            return mat;
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            if (!_restoringNavigation && ShouldLockNavigation())
            {
                RestoreNavigationViewport();
                return;
            }

            base.OnScroll(se);
            UpdateViewportState();
        }

        protected override void OnZoomChanged(EventArgs e)
        {
            base.OnZoomChanged(e);
            if (!_restoringNavigation && ShouldLockNavigation())
            {
                RestoreNavigationViewport();
                return;
            }

            if (Zoom < MinimumZoom)
            {
                Zoom = MinimumZoom;
                return;
            }

            InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            UpdateViewportState();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        private void UpdateViewportState()
        {
            ScaleZoom = Zoom / 100f;
            CanvasScrollPosition = AutoScrollPosition;
            EditOptions.ScaleZoom = ScaleZoom;
            EditOptions.ScrollPosition = CanvasScrollPosition;
            if (!ShouldLockNavigation())
            {
                _lastAllowedZoom = Zoom;
                _lastAllowedScrollPosition = AutoScrollPosition;
            }
            ViewportChanged?.Invoke(this, new CanvasViewportChangedArgs(ScaleZoom, CanvasScrollPosition, Zoom));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _currentBitmap?.Dispose();
                _currentBitmap = null;
            }

            base.Dispose(disposing);
        }
    }
}
