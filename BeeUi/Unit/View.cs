using BeeCore;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeGlobal;
using BeeInterface;
using BeeUi.Common;
using BeeUi.Commons;
using BeeUi.Unit;
using Cyotek.Windows.Forms;
using Google.Apis.Drive.v3.Data;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
using PylonCli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Xml;
using Camera = BeeCore.Camera;
using Control = System.Windows.Forms.Control;
using File = System.IO.File;
using Image = System.Drawing.Image;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Timer = System.Windows.Forms.Timer;
using UserControl = System.Windows.Forms.UserControl;
namespace BeeUi
{
    [Serializable()]
    public partial class View : UserControl
    {
        public event System.Windows.Forms.PreviewKeyDownEventHandler PreviewKeyDown;
   
        public static bool IsKeyDown(Keys Key)
        {
            return (Control.ModifierKeys & Key) == Key;
        } //test
        Keys KeysOld;
        bool IsKeyPress = false;
        Timer tmKey = new Timer();
        public Mat matResgiter = null;
        String SKey = "";
        DateTime lastTime;
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int ToUnicode(
        uint virtualKeyCode,
        uint scanCode,
        byte[] keyboardState,
        StringBuilder receivingBuffer,
        int bufferSize,
        uint flags
);
        static string GetCharsFromKeys(Keys keys, bool shift)
        {
            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];
            if (shift)
            {
                keyboardState[(int)Keys.ShiftKey] = 0xff;
            }
            ToUnicode((uint)keys, 0, keyboardState, buf, 256, 0);
            return buf.ToString();
        }

        private void KeyboardListener_s_KeyEventHandler(object sender, EventArgs e)
        {

            KeyboardListener.UniversalKeyEventArgs eventArgs = (KeyboardListener.UniversalKeyEventArgs)e;


           



                KeysOld = eventArgs.KeyCode;
              
               
              
                //if (eventArgs.KeyCode != Keys.Return)
                //{

                //    if (lastTime.Year == 1)
                //    {
                //        lastTime = DateTime.Now;
                //        SKey += GetCharsFromKeys(eventArgs.KeyCode, false);
                //    }
                //    if (lastTime > new DateTime())
                //    {
                //        if (DateTime.Now.Subtract(lastTime).Milliseconds > 30)
                //        {
                //            SKey += GetCharsFromKeys(eventArgs.KeyCode, false);
                //            lastTime = DateTime.Now;
                //        }
                       
                //    }

                   
                  
                //}
                //else 
                //{
                //SKey = "";
                //}
               

            
                
            
        }
        private LayoutPersistence _layout;
        // ===== Auto-Repeat (press & hold +/-) =====
        [Category("Behavior")] public bool AutoRepeatEnabled { get; set; } = true;
        [Category("Behavior")] public int AutoRepeatInitialDelay { get; set; } = 200; // ms
        [Category("Behavior")] public int AutoRepeatInterval { get; set; } = 50;      // ms
        [Category("Behavior")] public bool AutoRepeatAccelerate { get; set; } = true;
        [Category("Behavior")] public int AutoRepeatMinInterval { get; set; } = 1;   // ms
        [Category("Behavior")] public int AutoRepeatAccelDeltaMs { get; set; } = -6;  // mỗi tick giảm bấy nhiêu ms

        public View()
        {
            InitializeComponent();

            KeyboardListener.s_KeyEventHandler += new EventHandler(KeyboardListener_s_KeyEventHandler);
            tmKey.Tick += TmKey_Tick;
            tmKey.Interval = 50;
        }
      
        float _step = 3;
        // ===== Auto-repeat =====
        private Timer _repeatTimer;
        private int _repeatDirection; // -1 hoặc +1
        private int _repeatPhase;     // 0 = delay đầu, 1 = lặp/accelerate

        private void ApplyStep(int dir)
        {
            if (!Enabled) return;
            imgView.Zoom =(int)( imgView.Zoom + dir * _step);
        }
        private void BeginRepeat(int dir)
        {
            if (!AutoRepeatEnabled || !Enabled) return;

            _repeatDirection = (dir >= 0) ? +1 : -1;
            if (_repeatTimer == null)
            {
                _repeatTimer = new Timer();
                _repeatTimer.Tick += RepeatTimer_Tick;
            }
            _repeatPhase = 0;
            _repeatTimer.Interval = Math.Max(1, AutoRepeatInitialDelay);
            _repeatTimer.Start();
        }
        private void StopRepeat()
        {
            if (_repeatTimer != null) _repeatTimer.Stop();
        }
        private void RepeatTimer_Tick(object sender, EventArgs e)
        {
            if (!Enabled) { StopRepeat(); return; }

            ApplyStep(_repeatDirection);

            if (_repeatPhase == 0)
            {
                _repeatTimer.Interval = Math.Max(1, AutoRepeatInterval);
                _repeatPhase = 1;
            }
            else if (AutoRepeatAccelerate)
            {
                int next = _repeatTimer.Interval + AutoRepeatAccelDeltaMs; // âm => nhanh dần
                _repeatTimer.Interval = Math.Max(AutoRepeatMinInterval, next);
            }
        }
        private void TmKey_Tick(object sender, EventArgs e)
        {
            IsKeyPress = false;
            tmKey.Enabled= false;
        }

        private void imgView_Click(object sender, EventArgs e)
        {
            
        }

        private void imgView_SizeChanged(object sender, EventArgs e)
        {

        }

        private void imgView_Resize(object sender, EventArgs e)
        {

        }

        private void imgView_BindingContextChanged(object sender, EventArgs e)
        {
            imgView.Size=new System.Drawing.Size(pView.Width, pView.Height);
        }
        bool IsRect = false;
        public AreaCrop _AreaCrop=AreaCrop.Rect;

        public Point pDown;
     
        private SizeF _dragSize;
        private PointF _dragStart,_dragCenter;
        private PointF _dragStartOffset;
        private RectangleF _dragRect;
        private AnchorPoint _dragAnchor;
        private Single _dragRot;
        bool _drag;
        private void ShowCursor( AnchorPoint anchorPoint ,float angle)
        {
            if (angle < 0) angle = 360 + angle;
            if (angle == 360) angle =0;
            //Cursor curLeft= Cursors.SizeNWSE;
            //Cursor curRight= Cursors.SizeNESW;
            Cursor curLeft = Cursors.Default;
            Cursor curRight = Cursors.Default;
            //if(angle<45)
            //{
            //    curLeft = Cursors.SizeNS;
            //    curRight = Cursors.SizeWE;
            //}
            //else if (angle <90)
            //{
            //    curLeft = Cursors.SizeNWSE;
            //    curRight = Cursors.SizeNESW;
            //}
            //else if (angle < 135)
            //{
            //    curLeft = Cursors.SizeNWSE;
            //    curRight = Cursors.SizeNESW;
            //}
            //else if (angle < 180)
            //{
            //    curLeft = Cursors.SizeNS;
            //    curRight = Cursors.SizeWE;
            //}
            //else if (angle < 225)
            //{
            //    curLeft = Cursors.SizeNS;
            //    curRight = Cursors.SizeWE;
            //}
            //else if (angle < 270)
            //{
            //    curLeft = Cursors.SizeNS;
            //    curRight = Cursors.SizeWE;
            //}
            //else
            //{
            //    curLeft = Cursors.SizeNS;
            //    curRight = Cursors.SizeWE;
            //}
            switch (anchorPoint)
            {
                case AnchorPoint.TopLeft:
                    this.Cursor = curLeft;
                    break;
                case AnchorPoint.TopRight:
                    this.Cursor = curRight;
                    break;
                case AnchorPoint.BottomLeft:
                    this.Cursor = curRight;
                    break;
                case AnchorPoint.BottomRight:
                    this.Cursor = curLeft;
                    break;
                case AnchorPoint.Rotation:
                    this.Cursor = Cursors.Default;
                    break;
                case AnchorPoint.Center:
                    this.Cursor = Cursors.Hand;
                    break;
                case AnchorPoint.None:
                    this.Cursor = Cursors.Default;
                    break;
            }
         }
        public void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        HierarchyIndex[] HierarchyIndex;
       OpenCvSharp.Point[][] contourOutLine;
     
      
   
        public Mat CropRotatedRect(Mat source, RotatedRect rect )
        {
          Mat matResult  = new Mat();
            RotatedRect rot = rect;
            Point2f pCenter = new Point2f(rot.Center.X , rot.Center.Y);
            Size2f rect_size = new Size2f(rot.Size.Width , rot.Size.Height );
            RotatedRect rot2 = new RotatedRect(pCenter, rect_size, rot.Angle);
            double angle = rot.Angle;
            if (angle < -45)
            {
                angle += 90.0;

                Swap(ref rect_size.Width, ref rect_size.Height);
            }



            InputArray M =Cv2.GetRotationMatrix2D(rot2.Center, angle, 1.0);

            Mat crop1 = new Mat(); 
            try
            {
                Cv2.WarpAffine(source, crop1, M, source.Size(), InterpolationFlags.Cubic);

                Cv2.GetRectSubPix(crop1, new OpenCvSharp.Size(rect_size.Width, rect_size.Height), rot2.Center, matResult);
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
            }
          
            return matResult;
        }
        Color Renk = Color.Red;
        Point pMove;
        public Mat matClear,matMask;
        Rect rectClear = new Rect();
      public  int widthClear = 15;
        public Mat matRes = null;
        public Mat bmMask = null;
        public Mat matMaskAdd = null;
        public List<Mat> listMask ;
        public List<Mat> listRedo;
        public void Undo(dynamic Propety)
        {
            toolEdit.matTemp = Propety.Undo(toolEdit.matCrop);
         
            imgView.Invalidate();
        }
        public void ClearTemp(dynamic Propety)
        {
            if (Global.IndexToolSelected == -1) return;
            toolEdit.matTemp = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.ClearTemp();
           
            imgView.Invalidate();
        }
        public void RefreshMask()
        {
          bmMask = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows,BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1, Scalar.Black);

            Mat matGroup= new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1, Scalar.Black);
            foreach (Mat mat in listMask)
            {
               
                Cv2.BitwiseOr(mat, matGroup, bmMask);
               
            }
           
          imgView.Invalidate();
            imgView.Update();
        }
        //public PointF RotatePoint(float angle, PointF pt)
        //{
        //    var a = angle * System.Math.PI / 180.0;
        //    float cosa =(float) Math.Cos(a), sina =(float) Math.Sin(a);
        //    return new PointF((float)pt.X * cosa - pt.Y * sina, (float)pt.X * sina + pt.Y * cosa);
        //}
        RectRotate rotateDraw = new RectRotate();
        bool IsDone = false;
        PointF ptRotatePt2f(PointF ptInput, PointF ptOrg, double dAngle)
        {
            double dWidth = ptOrg.X * 2;
            double dHeight = ptOrg.Y * 2;
            double dY1 = dHeight - ptInput.Y, dY2 = dHeight - ptOrg.Y;

            double dX = (ptInput.X - ptOrg.X) * Math.Cos(dAngle) - (dY1 - ptOrg.Y) * Math.Sin(dAngle) + ptOrg.X;
            double dY = (ptInput.X - ptOrg.X) * Math.Sin(dAngle) + (dY1 - ptOrg.Y) * Math.Cos(dAngle) + dY2;

            dY = -dY + dHeight;
            return new PointF((float)dX, (float)dY);
        }
        double rtTop, rtLeft, rtRight, rtBotton;
        public void BoundRotate( RectRotate rot)
        {
            float Fi = rot._rectRotation;
            double w = rot._rect.Width;
            double h = rot._rect.Height;

            double  H = w * Math.Abs(Math.Sin(Fi)) + h * Math.Abs(Math.Cos(Fi));
            double W = w * Math.Abs(Math.Cos(Fi)) + h * Math.Abs(Math.Sin(Fi));

            double   AS = Math.Abs(Math.Sin(Fi));
            double cs = Math.Abs(Math.Cos(Fi));
             h = (H * cs - W * AS) / ( cs*cs - AS*AS);
             w = -(H * AS - W * cs) / (cs * cs - AS * AS);
            rtTop = w * cs;
            rtRight = h * cs;
            rtBotton = h * AS ;
            rtLeft = w * AS;
         
        }
        // ===== Helpers: paste trong cùng class form (hoặc lớp chứa sự kiện) =====
        private float ZoomFactor => (float)(imgView.Zoom / 100.0);

        // Thử lấy viewport chuẩn của Cyotek.ImageBox; nếu không có, fallback đơn giản
        private Rectangle GetImageViewPortSafe()
        {
            // Cyotek.ImageBox có method GetImageViewPort()
            var mi = imgView.GetType().GetMethod("GetImageViewPort", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (mi != null)
            {
                try
                {
                    return (Rectangle)mi.Invoke(imgView, null);
                }
                catch { /* ignore */ }
            }

            // Fallback: ước lượng từ AutoScroll + ClientSize (đủ dùng cho phần lớn layout)
            return new Rectangle(
                imgView.AutoScrollPosition.X,
                imgView.AutoScrollPosition.Y,
                imgView.ClientSize.Width,
                imgView.ClientSize.Height
            );
        }

        // Chuyển toạ độ Control (pixel trên imgView) → toạ độ Ảnh (pixel gốc ảnh)
        private PointF ControlToImage(Point p)
        {
            // Nếu có PointToImage (Cyotek), ưu tiên dùng:
            var m = imgView.GetType().GetMethod("PointToImage", new[] { typeof(Point) });
            if (m != null)
            {
                try { return (PointF)m.Invoke(imgView, new object[] { p }); }
                catch { /* ignore */ }
            }

            // Tự tính: (p - viewport.TopLeft) / Zoom
            var vp = GetImageViewPortSafe();
            return new PointF((p.X - vp.X) / ZoomFactor, (p.Y - vp.Y) / ZoomFactor);
        }

        // Quay một điểm quanh tâm theo góc deg
        private static PointF RotateAround(PointF pt, PointF center, float deg)
        {
            float rad = deg * (float)Math.PI / 180f;
            float cos = (float)Math.Cos(rad), sin = (float)Math.Sin(rad);
            float x = pt.X - center.X, y = pt.Y - center.Y;
            return new PointF(center.X + x * cos - y * sin, center.Y + x * sin + y * cos);
        }

        // Quay một vector (delta) theo góc deg (không cộng tâm)
        private static PointF RotateVector(PointF v, float deg)
        {
            float rad = deg * (float)Math.PI / 180f;
            float cos = (float)Math.Cos(rad), sin = (float)Math.Sin(rad);
            return new PointF(v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
        }

        Color clChoose;

       

        static PointF RotatePoint(float angleDeg, PointF p)
        {
            return RectRotate.Rotate(p, angleDeg);
        }
     
        private bool _maybeCreate = false;
        private bool _creatingNew = false;
        private PointF _createStartImg;
        private PointF _createEndImg;
        private RectRotate _previewNew;

        // ====== Bạn đã có imgView, Global, BeeCore.* ======
        // imgView: control hiển thị ảnh (có AutoScrollPosition, Zoom, Pan, ...)

        // ====== Helpers ======
        private static PointF TransformPoint(Matrix m, PointF p)
        {
            var pts = new[] { p };
            m.TransformPoints(pts);
            return pts[0];
        }

        private static Matrix BuildLocalInverseMatrixFor(RectRotate rr, float zoomPercent, Point scroll,
                                                        bool useDragCenter, PointF dragCenter, float angleWhenDrag)
        {
            var m = new Matrix();
            m.Translate(scroll.X, scroll.Y);
            float s = zoomPercent / 100f;
            m.Scale(s, s);
            if (useDragCenter) m.Translate(dragCenter.X, dragCenter.Y);
            else m.Translate(rr._PosCenter.X, rr._PosCenter.Y);
            if (useDragCenter) m.Rotate(angleWhenDrag);
            else m.Rotate(rr._rectRotation);
            m.Invert();
            return m;
        }

        private static RectangleF GetPolygonBoundsLocal(RectRotate rr)
        {
            var pts = rr == null ? null : rr.PolyLocalPoints;
            if (pts == null || pts.Count == 0)
                return rr != null ? rr._rect : RectangleF.Empty;

            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;
            for (int i = 0; i < pts.Count; i++)
            {
                var p = pts[i];
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }
            return new RectangleF(minX, minY, Math.Max(0, maxX - minX), Math.Max(0, maxY - minY));
        }

        private PointF ScreenToImage(PointF pScreen)
        {
            using (var m = new Matrix())
            {
                m.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
                float s = (float)(imgView.Zoom / 100.0);
                m.Scale(s, s);
                m.Invert();
                var pts = new[] { pScreen };
                m.TransformPoints(pts);
                return pts[0];
            }
        }

        // Lấy/đặt rr theo TypeCrop (giống nguyên tác)
        private RectRotate GetCurrentRR()
        {
            var tool = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected];
            if (tool == null || tool.Propety == null) return null;
            if (Global.TypeCrop == TypeCrop.Area) return tool.Propety.rotArea;
            else if (Global.TypeCrop == TypeCrop.Mask) return tool.Propety.rotMask;
            else return tool.Propety.rotCrop;
        }
        private void SetCurrentRR(RectRotate rr)
        {
            var tool = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected];
            if (tool == null) return;
            if (Global.TypeCrop == TypeCrop.Area) tool.Propety.rotArea = rr;
            else if (Global.TypeCrop == TypeCrop.Mask) tool.Propety.rotMask = rr;
            else tool.Propety.rotCrop = rr;
            if(tool.TypeTool==TypeTool.Position_Adjustment|| tool.TypeTool == TypeTool.Pattern)
            {
                tool.Propety.ReSetAngle();
            }    
        }

  
        private static bool BoundsContainAll(RectangleF r, IList<PointF> pts)
        {
            if (pts == null || pts.Count == 0) return true;
            for (int i = 0; i < pts.Count; i++)
                if (!r.Contains(pts[i])) return false;
            return true;
        }

        private static bool HexBoundsContainAll(RectRotate rr)
        {
            var r = rr._rect;
            var verts = rr.GetHexagonVerticesLocal(); // 6 đỉnh local (đã gồm offsets)
            for (int i = 0; i < verts.Length; i++)
                if (!r.Contains(verts[i])) return false;
            return true;
        }
        // ====== MouseDown ======
        // ===================== FORM / CONTROL: Mouse handlers =====================

        // ====== MouseDown ======
        // ⬇️ CỜ MỚI: Polygon bẩn trong lúc kéo (hoãn update center/bounds/angle)
        private bool _polyDirtyDuringDrag = false;
        bool  IsNewShape = false;

        // ====== MouseDown ======
        private void imgView_MouseDown(object sender, MouseEventArgs e)
        {
            if (Global.IndexToolSelected == -1) return;
                if (toolEdit == null) return;
            if (Global.IsRun) return;
         
            //if (Global.StatusDraw == StatusDraw.Scan && e.Button == MouseButtons.Left)
            //    Global.StatusDraw = StatusDraw.Choose;
            pDown = e.Location;
            _drag = true;

            if (Global.StatusDraw == StatusDraw.Color)
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.AddColor();


            if (Global.StatusDraw == StatusDraw.Scan)
            {
                int i = 0;
                if (IndexRotChoose >= 0)
                {
                   
                    if (BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.ModeCheck == ModeCheck.Single)
                    {
                        BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.IndexChoose = IndexRotChoose;

                        foreach (RectRotate rot in BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.listRotScan)
                        {
                            if (i == IndexRotChoose)
                            {
                                rot._dragAnchor = AnchorPoint.Center;
                                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.SetTemp(rot);

                            }

                            else
                                rot._dragAnchor = AnchorPoint.None;
                            i++;

                        }

                    }
                    else
                    {
                        RectRotate rot = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.listRotScan[IndexRotChoose];
                        if (rot._dragAnchor == AnchorPoint.Center)
                            rot._dragAnchor = AnchorPoint.None;
                        else
                            rot._dragAnchor = AnchorPoint.Center;
                        BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.SetMulTemp();
                    }
                    imgView.Invalidate();
                    return;
                }
            }
                imgView.Invalidate();
            
            RectRotate rr = GetCurrentRR();
            if (rr == null) return;
            if (Global.StatusDraw == StatusDraw.Check && rr._dragAnchor != AnchorPoint.None)
                Global.StatusDraw = StatusDraw.Edit;
            // Reset tạo mới
            _maybeCreate = false;
            _creatingNew = false;
            _previewNew = null;

            if (rr.IsEmptyForCreate())
            {
                // cho phép tạo mới: Rect/Ellipse/Hexagon (thêm Hexagon)
                if (rr.Shape == ShapeType.Rectangle || rr.Shape == ShapeType.Ellipse || rr.Shape == ShapeType.Hexagon)
                {
                    IsNewShape = true;
                    _maybeCreate = true;
                    _createStartImg = ScreenToImage(e.Location);
                }
            }

            // ===== Polygon: thêm điểm / chọn vertex =====
            using (var inv = BuildLocalInverseMatrixFor(rr, (float)imgView.Zoom, imgView.AutoScrollPosition, false, PointF.Empty, 0f))
            {
                PointF pLocal = TransformPoint(inv, e.Location);

                if (rr.Shape == ShapeType.Polygon)
                {
                    float handle = Global.Config.RadEdit;
                    float closeTol = handle * 1.25f;

                    // Nếu polygon đang rỗng -> reset sạch khung + xoá điểm cũ
                    if (!rr.IsPolygonClosed && (rr.PolyLocalPoints == null || rr.PolyLocalPoints.Count == 0))
                    {  // 2) Reset cờ thao tác/UI
                       
                        _maybeCreate = false;
                        _creatingNew = false;
                        _previewNew = null;
                        _polyDirtyDuringDrag = false;

                        imgView.PanMode = btnPan.IsCLick ? ImageBoxPanMode.Left : ImageBoxPanMode.None;
                        imgView.AllowClickZoom = true;
                        imgView.AllowDoubleClick = true;
                        rr.ResetFrameForNewPolygonHard();
                    }

                    if (!rr.IsPolygonClosed)
                    {
                        if (!rr.PolygonTryCloseIfNearFirst(pLocal, closeTol))
                            rr.PolygonAddPointLocal(pLocal);

                        _polyDirtyDuringDrag = true; // hoãn chuẩn hoá
                        _drag = false;
                        imgView.Invalidate();
                        return;
                    }
                    else
                    {
                        rr.ActiveVertexIndex = -1;
                        for (int i = rr.PolyLocalPoints.Count - 1; i >= 0; i--)
                        {
                            PointF v = rr.PolyLocalPoints[i];
                            RectangleF handleRect = new RectangleF(v.X - handle / 2f, v.Y - handle / 2f, handle, handle);
                            if (handleRect.Contains(pLocal))
                            {
                                rr.ActiveVertexIndex = i;
                                rr._dragAnchor = AnchorPoint.Vertex;
                                _drag = true;

                                _dragRect = rr._rect;
                                _dragCenter = rr._PosCenter;
                                _dragStart = pLocal;
                                _dragStartOffset = new PointF(_dragStart.X - v.X, _dragStart.Y - v.Y);
                                _dragRot = rr._rectRotation;
                                break;
                            }
                        }
                        imgView.Invalidate();
                        return;
                    }
                }
            }
            // các shape khác: hit-test ở MouseMove
        }
  

        // CHÍNH: đưa polygon về tâm local (0,0), cập nhật _PosCenter, _rect & (tuỳ chọn) _rectRotation
    

        // ====== MouseMove ======
        private float _rotStartAngleLocal = 0f; // góc local lúc bắt đầu xoay (radian)
        private float _rotBase = 0f;            // rotation ban đầu (degree) để cộng delta
        public int IndexRotChoose = -1;
        private void imgView_MouseMove(object sender, MouseEventArgs e)
        {
            if (Global.IndexToolSelected == -1) return;

            if (Global.IsRun) return;

            // Lưu vị trí chuột để OnPaint vẽ preview
            pMove = e.Location;
            //if (Global.StatusDraw == StatusDraw.Scan )
            //    Global.StatusDraw = StatusDraw.Choose;
            if (Global.StatusDraw == StatusDraw.Scan)
            { int j = 0;
                foreach (RectRotate rot in BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.listRotScan)
                {
                    var rrSrc = rot;
                    if (rrSrc == null) continue;

                    RectRotate rotateRect = new RectRotate(rrSrc._rect, rrSrc._PosCenter, rrSrc._rectRotation, rrSrc._dragAnchor);
                    rotateRect.Shape = rrSrc.Shape;
                    if (rrSrc.HexVertexOffsets != null)
                        for (int i = 0; i < 6; i++) rotateRect.HexVertexOffsets[i] = rrSrc.HexVertexOffsets[i];
                    rotateRect.PolyLocalPoints.Clear();
                    if (rrSrc.PolyLocalPoints != null)
                        for (int i = 0; i < rrSrc.PolyLocalPoints.Count; i++) rotateRect.PolyLocalPoints.Add(rrSrc.PolyLocalPoints[i]);
                    rotateRect.IsPolygonClosed = rrSrc.IsPolygonClosed;
                    rotateRect.ActiveVertexIndex = rrSrc.ActiveVertexIndex;
                    rotateRect.AutoExpandBounds = rrSrc.AutoExpandBounds;

                    var mat = new Matrix();
                    mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
                    float s = (float)(imgView.Zoom / 100.0);
                    mat.Scale(s, s);
                    mat.Translate(rotateRect._PosCenter.X, rotateRect._PosCenter.Y);
                    mat.Rotate(rotateRect._rectRotation);
                    mat.Invert();

                    var point = TransformPoint(mat, new PointF(e.X, e.Y)); // local

                
               

                  
                    bool anchored = false;

                    if (rotateRect.Shape == ShapeType.Polygon)
                    {
                        
                            if (RectRotate.PointInPolygon(rotateRect.PolyLocalPoints, point))
                        {
                            IndexRotChoose = j;
                            break;
                        }
                        else
                        {
                           
                           }
                        
                    }


                    j++;
                }
             
                return;
            }

        

            // ===== Color picker =====
            if (Global.IndexToolSelected >= 0)
            {
                var tool = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected];
                if (tool.TypeTool == TypeTool.Color_Area)
                {
                    if (tool.Propety.IsGetColor)
                    {
                        imgView.Cursor = new Cursor(Properties.Resources.Color_Dropper.Handle);
                        imgView.AllowClickZoom = false;
                        imgView.PanMode = ImageBoxPanMode.None;
                        if (!workGetColor.IsBusy) workGetColor.RunWorkerAsync();
                        return;
                    }
                    else imgView.Cursor = Cursors.Default;
                }
            }

            //if (Global.StatusDraw != StatusDraw.Edit) return;

            try
            {
                Func<RectRotate> getCurrentRR = GetCurrentRR;
                Action<RectRotate> setCurrentRR = SetCurrentRR;

                // ====== NHÁNH TẠO MỚI (sau Clear) ======
                if (_drag && _maybeCreate &&
                    ((getCurrentRR() != null ? getCurrentRR()._dragAnchor : AnchorPoint.None) == AnchorPoint.None))
                {
                    _createEndImg = ScreenToImage(e.Location);

                    // Chỉ tạo khi kéo TRÁI -> PHẢI
                    if (_createEndImg.X > _createStartImg.X)
                    {
                        float w = Math.Max(1f, _createEndImg.X - _createStartImg.X);
                        float yTop = Math.Min(_createStartImg.Y, _createEndImg.Y);
                        float yBot = Math.Max(_createStartImg.Y, _createEndImg.Y);
                        float hReal = Math.Max(1f, yBot - yTop);
                        var center = new PointF(_createStartImg.X + w / 2f, yTop + hReal / 2f);

                        var rrSrc = getCurrentRR();
                        ShapeType shape = rrSrc != null ? rrSrc.Shape : ShapeType.Rectangle;
                        if (shape != ShapeType.Rectangle && shape != ShapeType.Ellipse && shape != ShapeType.Hexagon)
                            shape = ShapeType.Rectangle;

                        var rrNew = new RectRotate(
                            new RectangleF(-w / 2f, -hReal / 2f, w, hReal),
                            center,
                            0f,
                            AnchorPoint.None
                        );
                        rrNew.Shape = shape;

                        _previewNew = rrNew;
                        _creatingNew = true;
                        setCurrentRR(rrNew);
                        imgView.Invalidate();
                        return;
                    }
                    else
                    {
                        _previewNew = null;
                        _creatingNew = false;
                        // không return: cho phép rơi xuống hit-test/drag hiện hữu nếu có
                    }
                }

                // ====== NHÁNH ĐANG KÉO (drag/resize/rotate/move) ======
                if (_drag)
                {
                    
                    var rrSrc = getCurrentRR();
                    if (rrSrc == null) return;
                  
                    // clone rrSrc
                    RectRotate rotateRect = new RectRotate(rrSrc._rect, rrSrc._PosCenter, rrSrc._rectRotation, rrSrc._dragAnchor);
                    rotateRect.Shape = rrSrc.Shape;
                    if (rrSrc.HexVertexOffsets != null)
                        for (int i = 0; i < 6; i++) rotateRect.HexVertexOffsets[i] = rrSrc.HexVertexOffsets[i];
                    rotateRect.PolyLocalPoints.Clear();
                    if (rrSrc.PolyLocalPoints != null)
                        for (int i = 0; i < rrSrc.PolyLocalPoints.Count; i++) rotateRect.PolyLocalPoints.Add(rrSrc.PolyLocalPoints[i]);
                    rotateRect.IsPolygonClosed = rrSrc.IsPolygonClosed;
                    rotateRect.ActiveVertexIndex = rrSrc.ActiveVertexIndex;
                    rotateRect.AutoExpandBounds = rrSrc.AutoExpandBounds;

                    // screen->local dùng tâm & GÓC CỐ ĐỊNH lúc bắt đầu kéo (_dragCenter, _dragRot)
                    var mat = new Matrix();
                    mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
                    float s = (float)(imgView.Zoom / 100.0);
                    mat.Scale(s, s);
                    mat.Translate(_dragCenter.X, _dragCenter.Y);
                    mat.Rotate(_dragRot); // ❗ dùng _dragRot cố định cho phiên kéo
                    mat.Invert();

                    var point = TransformPoint(mat, new PointF(e.X, e.Y)); // local-space (frame cố định)

                    SizeF deltaSize = SizeF.Empty;
                    float deltaX = 0f, deltaY = 0f;

                    // Không resize bbox cho Polygon
                    bool isPolygonBBoxResize = false;

                    if (!isPolygonBBoxResize)
                    {
                        switch (rotateRect._dragAnchor)
                        {
                            case AnchorPoint.TopLeft:
                                {
                                    var clamped = new PointF(Math.Min(0f, point.X), Math.Min(0f, point.Y));
                                    deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                                    rotateRect._rect = new RectangleF(
                                        _dragRect.Left + deltaSize.Width / 2f,
                                        _dragRect.Top + deltaSize.Height / 2f,
                                        _dragRect.Width - deltaSize.Width,
                                        _dragRect.Height - deltaSize.Height);
                                    deltaX = deltaSize.Width / 2f; deltaY = deltaSize.Height / 2f;
                                    break;
                                }
                            case AnchorPoint.TopRight:
                                {
                                    var clamped = new PointF(Math.Max(0f, point.X), Math.Min(0f, point.Y));
                                    deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                                    rotateRect._rect = new RectangleF(
                                        _dragRect.Left - deltaSize.Width / 2f,
                                        _dragRect.Top + deltaSize.Height / 2f,
                                        _dragRect.Width + deltaSize.Width,
                                        _dragRect.Height - deltaSize.Height);
                                    deltaX = deltaSize.Width / 2f; deltaY = deltaSize.Height / 2f;
                                    break;
                                }
                            case AnchorPoint.BottomLeft:
                                {
                                    var clamped = new PointF(Math.Min(0f, point.X), Math.Max(0f, point.Y));
                                    deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                                    rotateRect._rect = new RectangleF(
                                        _dragRect.Left + deltaSize.Width / 2f,
                                        _dragRect.Top - deltaSize.Height / 2f,
                                        _dragRect.Width - deltaSize.Width,
                                        _dragRect.Height + deltaSize.Height);
                                    deltaX = deltaSize.Width / 2f; deltaY = deltaSize.Height / 2f;
                                    break;
                                }
                            case AnchorPoint.BottomRight:
                                {
                                    var clamped = new PointF(Math.Max(0f, point.X), Math.Max(0f, point.Y));
                                    deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                                    rotateRect._rect = new RectangleF(
                                        _dragRect.Left - deltaSize.Width / 2f,
                                        _dragRect.Top - deltaSize.Height / 2f,
                                        _dragRect.Width + deltaSize.Width,
                                        _dragRect.Height + deltaSize.Height);
                                    deltaX = deltaSize.Width / 2f; deltaY = deltaSize.Height / 2f;
                                    break;
                                }

                            case AnchorPoint.Rotation:
                                {
                                    if (rotateRect.Shape == ShapeType.Polygon)
                                        return;
                                        float angNow = (float)Math.Atan2(point.Y, point.X);
                                    float deltaDeg = (float)((angNow - _rotStartAngleLocal) * 180.0 / Math.PI);
                                    while (deltaDeg > 180f) deltaDeg -= 360f;
                                    while (deltaDeg < -180f) deltaDeg += 360f;

                                    rotateRect._rectRotation = _rotBase + deltaDeg;
                                    if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                                    {
                                        float snap = 15f;
                                        rotateRect._rectRotation = (float)Math.Round(rotateRect._rectRotation / snap) * snap;
                                    }

                                  
                                    if (rotateRect.Shape == ShapeType.Polygon)
                                        rotateRect.UpdateFromPolygon(  false); // KHÔNG đè lại góc vừa xoay
                                    break;
                                    //// === XOAY MƯỢT VỚI ATAN2 & DELTA ANGLE ===
                                    //float angNow = (float)Math.Atan2(point.Y, point.X);

                                    //float deltaDeg = (float)((angNow - _rotStartAngleLocal) * 180.0 / Math.PI);

                                    //// chuẩn hoá về [-180, 180] để tránh "quay vòng"
                                    //while (deltaDeg > 180f) deltaDeg -= 360f;
                                    //while (deltaDeg < -180f) deltaDeg += 360f;

                                    //rotateRect._rectRotation = _rotBase + deltaDeg;

                                    //// (tuỳ chọn) snap khi giữ Shift
                                    //if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                                    //{
                                    //    float snap = 15f;
                                    //    rotateRect._rectRotation = (float)Math.Round(rotateRect._rectRotation / snap) * snap;
                                    //}
                                 
                                }

                            case AnchorPoint.Center:
                                {
                                    var localNewCenter = new PointF(point.X - _dragStartOffset.X, point.Y - _dragStartOffset.Y);
                                    var worldDelta = RectRotate.Rotate(localNewCenter, _dragRot);
                                    rotateRect._PosCenter = new PointF(_dragCenter.X + worldDelta.X, _dragCenter.Y + worldDelta.Y);

                                    if (rotateRect.Shape == ShapeType.Polygon)
                                        rotateRect.UpdateFromPolygon(false); // chỉ để sync _rect/handles
                                    break;
                                    //if (rotateRect.Shape == ShapeType.Polygon)
                                    //{
                                    //    float dx = point.X - _dragStart.X;
                                    //    float dy = point.Y - _dragStart.Y;
                                    //    rotateRect.TranslatePolygonLocal(dx, dy);
                                    //    _dragStart = point;
                                    //}
                                    //else
                                    //{
                                    //    // local → world với góc cố định _dragRot
                                    //    var localNewCenter = new PointF(point.X - _dragStartOffset.X, point.Y - _dragStartOffset.Y);
                                    //    var worldDelta = RectRotate.Rotate(localNewCenter, _dragRot);
                                    //    rotateRect._PosCenter = new PointF(_dragCenter.X + worldDelta.X, _dragCenter.Y + worldDelta.Y);
                                    //}
                                    break;
                                }

                            case AnchorPoint.V0:
                            case AnchorPoint.V1:
                            case AnchorPoint.V2:
                            case AnchorPoint.V3:
                            case AnchorPoint.V4:
                            case AnchorPoint.V5:
                                {
                                    if (rotateRect.Shape == ShapeType.Hexagon)
                                    {
                                        int idx = (int)rotateRect._dragAnchor - (int)AnchorPoint.V0;
                                        var pLocal = new PointF(point.X, point.Y);
                                        rotateRect.SetHexVertexByLocalPoint(idx, pLocal);
                                        if (rotateRect.AutoExpandBounds)
                                            rotateRect.RefitBoundsToHexagon();
                                    }
                                    break;
                                }

                            case AnchorPoint.Vertex:
                                {
                                    if (rotateRect.Shape == ShapeType.Polygon && rotateRect.ActiveVertexIndex >= 0)
                                    {
                                        int idx = rotateRect.ActiveVertexIndex;
                                        var pLocal = new PointF(point.X, point.Y);

                                        if (idx >= 0 && idx < rotateRect.PolyLocalPoints.Count)
                                        {
                                            rotateRect.PolyLocalPoints[idx] = pLocal;

                                            //if (rotateRect.IsPolygonClosed && rotateRect.PolyLocalPoints.Count >= 2)
                                            //{
                                            //    if (idx == 0)
                                            //        rotateRect.PolyLocalPoints[rotateRect.PolyLocalPoints.Count - 1] = pLocal;
                                            //    //else if (idx == rotateRect.PolyLocalPoints.Count - 1)
                                            //    //    rotateRect.PolyLocalPoints[0] = pLocal;
                                            //}

                                            // >>> NEW: chuẩn hoá lại frame polygon
                                          rotateRect.UpdateFromPolygon(false);
                                        }
                                    }
                                    //if (rotateRect.Shape == ShapeType.Polygon && rotateRect.ActiveVertexIndex >= 0)
                                    //if (rotateRect.Shape == ShapeType.Polygon && rotateRect.ActiveVertexIndex >= 0)
                                    //{
                                    //    int idx = rotateRect.ActiveVertexIndex;
                                    //    var pLocal = new PointF(point.X, point.Y);

                                    //    if (idx >= 0 && idx < rotateRect.PolyLocalPoints.Count)
                                    //    {
                                    //        rotateRect.PolyLocalPoints[idx] = pLocal;

                                    //        if (rotateRect.IsPolygonClosed && rotateRect.PolyLocalPoints.Count >= 2)
                                    //        {
                                    //            if (idx == 0)
                                    //                rotateRect.PolyLocalPoints[rotateRect.PolyLocalPoints.Count - 1] = pLocal;
                                    //            else if (idx == rotateRect.PolyLocalPoints.Count - 1)
                                    //                rotateRect.PolyLocalPoints[0] = pLocal;
                                    //        }
                                    //    }
                                    //}
                                    break;
                                }
                        }
                    }

                    // Sau resize 4 góc: cập nhật tâm theo delta đã xoay
                    if (rotateRect._dragAnchor != AnchorPoint.None &&
                        rotateRect._dragAnchor != AnchorPoint.Center &&
                        rotateRect._dragAnchor != AnchorPoint.Rotation &&
                        rotateRect._dragAnchor < AnchorPoint.V0)
                    {
                        if (deltaX != 0f || deltaY != 0f)
                        {
                            var pDelta = RectRotate.Rotate(new PointF(deltaX, deltaY), _dragRot); // dùng _dragRot
                            rotateRect._PosCenter = new PointF(_dragCenter.X + pDelta.X, _dragCenter.Y + pDelta.Y);
                            IsDone = false;
                        }
                    }

                    // Clamp theo ảnh cho Area (trừ polygon)
                    if (Global.TypeCrop == TypeCrop.Area && rotateRect.Shape != ShapeType.Polygon)
                    {
                        float x = rotateRect._PosCenter.X - rotateRect._rect.Width / 2f;
                        float y = rotateRect._PosCenter.Y - rotateRect._rect.Height / 2f;
                        float w = rotateRect._rect.Width, h = rotateRect._rect.Height;
                        int maxW = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Width;
                        int maxH = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Height;

                        if (x < 0f) rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X - x, rotateRect._PosCenter.Y);
                        else if (x + w > maxW) rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X - (x + w - maxW), rotateRect._PosCenter.Y);
                        if (y < 0f) rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y - y);
                        else if (y + h > maxH) rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y - (y + h - maxH));
                    }

                    // Ghi về Propety
                    var rrNew = new RectRotate(
                        new RectangleF(rotateRect._rect.X, rotateRect._rect.Y, rotateRect._rect.Width, rotateRect._rect.Height),
                        new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y),
                        rotateRect._rectRotation,
                        rotateRect._dragAnchor
                    );
                    rrNew.Shape = rotateRect.Shape;
                    for (int i = 0; i < 6; i++) rrNew.HexVertexOffsets[i] = rotateRect.HexVertexOffsets[i];
                    rrNew.PolyLocalPoints.Clear();
                    for (int i = 0; i < rotateRect.PolyLocalPoints.Count; i++) rrNew.PolyLocalPoints.Add(rotateRect.PolyLocalPoints[i]);
                    rrNew.IsPolygonClosed = rotateRect.IsPolygonClosed;
                    rrNew.ActiveVertexIndex = rotateRect.ActiveVertexIndex;
                    rrNew.AutoExpandBounds = rotateRect.AutoExpandBounds;

                    setCurrentRR(rrNew);
                }
                // ====== NHÁNH HIT-TEST (không kéo) ======
                else
                {
                    var rrSrc = getCurrentRR();
                    if (rrSrc == null) return;

                    RectRotate rotateRect = new RectRotate(rrSrc._rect, rrSrc._PosCenter, rrSrc._rectRotation, rrSrc._dragAnchor);
                    rotateRect.Shape = rrSrc.Shape;
                    if (rrSrc.HexVertexOffsets != null)
                        for (int i = 0; i < 6; i++) rotateRect.HexVertexOffsets[i] = rrSrc.HexVertexOffsets[i];
                    rotateRect.PolyLocalPoints.Clear();
                    if (rrSrc.PolyLocalPoints != null)
                        for (int i = 0; i < rrSrc.PolyLocalPoints.Count; i++) rotateRect.PolyLocalPoints.Add(rrSrc.PolyLocalPoints[i]);
                    rotateRect.IsPolygonClosed = rrSrc.IsPolygonClosed;
                    rotateRect.ActiveVertexIndex = rrSrc.ActiveVertexIndex;
                    rotateRect.AutoExpandBounds = rrSrc.AutoExpandBounds;

                    var mat = new Matrix();
                    mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
                    float s = (float)(imgView.Zoom / 100.0);
                    mat.Scale(s, s);
                    mat.Translate(rotateRect._PosCenter.X, rotateRect._PosCenter.Y);
                    mat.Rotate(rotateRect._rectRotation);
                    mat.Invert();

                    var point = TransformPoint(mat, new PointF(e.X, e.Y)); // local

                    RectangleF baseRect = rotateRect._rect;
                    RectangleF polyBounds = (rotateRect.Shape == ShapeType.Polygon && rotateRect.PolyLocalPoints != null && rotateRect.PolyLocalPoints.Count >= 3)
                        ? BboxOf(rotateRect.PolyLocalPoints)
                        : baseRect;

                    float r = Global.Config.RadEdit;

                    RectangleF rectOuter = new RectangleF(polyBounds.X - r / 2f, polyBounds.Y - r / 2f, polyBounds.Width + r, polyBounds.Height + r);
                    RectangleF rectTopLeft = new RectangleF(polyBounds.Left - r / 2f, polyBounds.Top - r / 2f, r, r);
                    RectangleF rectTopRight = new RectangleF(polyBounds.Right - r / 2f, polyBounds.Top - r / 2f, r, r);
                    RectangleF rectBottomLeft = new RectangleF(polyBounds.Left - r / 2f, polyBounds.Bottom - r / 2f, r, r);
                    RectangleF rectBottomRight = new RectangleF(polyBounds.Right - r / 2f, polyBounds.Bottom - r / 2f, r, r);
                    RectangleF rectRotate = new RectangleF(-r / 2f, polyBounds.Top - 2f * r, 2f * r, 2f * r);

                    _dragCenter = new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y);

                    bool anchored = false;

                    // 1) Polygon
                    if (rotateRect.Shape == ShapeType.Polygon)
                    {
                        if (!rotateRect.IsPolygonClosed)
                        {
                            rotateRect._dragAnchor = AnchorPoint.None;
                            rotateRect.ActiveVertexIndex = -1;
                        }
                        else
                        {
                            
                            for (int i = 0; i < rotateRect.PolyLocalPoints.Count; i++)
                            {
                                RectangleF h = new RectangleF(rotateRect.PolyLocalPoints[i].X - r / 2f,
                                                              rotateRect.PolyLocalPoints[i].Y - r / 2f, r, r);
                                if (h.Contains(point))
                                {
                                    _dragStart = new PointF(point.X, point.Y);
                                    rotateRect._dragAnchor = AnchorPoint.Vertex;
                                    rotateRect.ActiveVertexIndex = i;
                                    _dragRect = polyBounds;
                                    anchored = true;
                                    break;
                                }
                            }
                            if (!anchored)
                            {
                                if (rectRotate.Contains(point))
                                {
                                    _dragStart = new PointF(point.X, point.Y);
                                    rotateRect._dragAnchor = AnchorPoint.Rotation;
                                    _dragRect = polyBounds;
                                    _dragRot = rotateRect._rectRotation;          // cố định góc phiên kéo
                                    _rotStartAngleLocal = (float)Math.Atan2(_dragStart.Y, _dragStart.X);
                                    _rotBase = rotateRect._rectRotation;
                                    anchored = true;
                                }
                                else if (RectRotate.PointInPolygon(rotateRect.PolyLocalPoints, point))
                                {
                                    _dragStart = new PointF(point.X, point.Y);
                                    rotateRect._dragAnchor = AnchorPoint.Center;
                                    _dragRect = RectangleF.Empty;
                                    _dragStartOffset = _dragStart;                // local offset
                                    _dragRot = rotateRect._rectRotation;          // cố định góc phiên kéo
                                    anchored = true;
                                }
                            }
                        }
                    }

                    // 2) Hexagon: ưu tiên 6 đỉnh
                    if (!anchored && rotateRect.Shape == ShapeType.Hexagon)
                    {
                        var verts = rotateRect.GetHexagonVerticesLocal();
                        for (int i = 0; i < 6; i++)
                        {
                            var h = new RectangleF(verts[i].X - r / 2f, verts[i].Y - r / 2f, r, r);
                            if (h.Contains(point))
                            {
                                _dragStart = new PointF(point.X, point.Y);
                                rotateRect._dragAnchor = (AnchorPoint)((int)AnchorPoint.V0 + i);
                                _dragRect = baseRect;
                                _dragRot = rotateRect._rectRotation;
                                anchored = true;
                                break;
                            }
                        }
                    }

                    // 3) Rectangle/Ellipse (hoặc Hexagon không trúng đỉnh)
                    if (!anchored && rotateRect.Shape != ShapeType.Polygon)
                    {
                        if (rectTopLeft.Contains(point))
                        { _dragStart = new PointF(point.X, point.Y); rotateRect._dragAnchor = AnchorPoint.TopLeft; _dragRect = baseRect; _dragRot = rotateRect._rectRotation; }
                        else if (rectTopRight.Contains(point))
                        { _dragStart = new PointF(point.X, point.Y); rotateRect._dragAnchor = AnchorPoint.TopRight; _dragRect = baseRect; _dragRot = rotateRect._rectRotation; }
                        else if (rectBottomLeft.Contains(point))
                        { _dragStart = new PointF(point.X, point.Y); rotateRect._dragAnchor = AnchorPoint.BottomLeft; _dragRect = baseRect; _dragRot = rotateRect._rectRotation; }
                        else if (rectBottomRight.Contains(point))
                        { _dragStart = new PointF(point.X, point.Y); rotateRect._dragAnchor = AnchorPoint.BottomRight; _dragRect = baseRect; _dragRot = rotateRect._rectRotation; }
                        else if (rectRotate.Contains(point))
                        {
                           
                            _dragStart = new PointF(point.X, point.Y);
                            rotateRect._dragAnchor = AnchorPoint.Rotation;
                            _dragRect = baseRect;
                            _dragRot = rotateRect._rectRotation;               // cố định góc phiên kéo
                            _rotStartAngleLocal = (float)Math.Atan2(_dragStart.Y, _dragStart.X);
                            _rotBase = rotateRect._rectRotation;
                        }
                        else if (rectOuter.Contains(point))
                        {
                            
                            _dragStart = new PointF(point.X, point.Y);
                            rotateRect._dragAnchor = AnchorPoint.Center;
                            _dragRect = baseRect;
                            _dragStartOffset = _dragStart;                       // local offset
                            _dragRot = rotateRect._rectRotation;                 // cố định góc
                        }
                        else
                        {
                            rotateRect._dragAnchor = AnchorPoint.None;
                        }
                    }

                    // Ghi lại anchor & active index về rrSrc
                    var rrSet = getCurrentRR();
                    if (rrSet != null)
                    {
                        rrSet._dragAnchor = rotateRect._dragAnchor;
                        rrSet.ActiveVertexIndex = rotateRect.ActiveVertexIndex;
                    }
                }

                // ===== Khoá pan/zoom khi có anchor =====
                var cur = GetCurrentRR();
                if (cur != null && cur._dragAnchor != AnchorPoint.None|| IsNewShape||cur._rect.Width==0)
                {
                    //if (Global.StatusDraw != StatusDraw.Color) Global.StatusDraw = StatusDraw.Edit;
                    imgView.PanMode = ImageBoxPanMode.None;
                    imgView.AllowClickZoom = false;
                    imgView.AllowDoubleClick = false;
                }
                else
                {
                    if (btnPan.IsCLick) imgView.PanMode = ImageBoxPanMode.Left;
                    imgView.AllowClickZoom = true;
                    imgView.AllowDoubleClick = true;
                }

                imgView.Invalidate();
            }
            catch
            {
                // log nếu cần
            }
        }

        //private void imgView_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (Global.IndexToolSelected == -1) return;
        //    if (Global.StatusDraw == StatusDraw.Check) Global.StatusDraw = StatusDraw.Edit;

        //    pMove = e.Location;
        //    if (Global.IsRun) return;

        //    // ===== Color picker =====
        //    if (Global.IndexToolSelected >= 0)
        //    {
        //        var tool = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected];
        //        if (tool.TypeTool == TypeTool.Color_Area)
        //        {
        //            if (tool.Propety.IsGetColor)
        //            {
        //                imgView.Cursor = new Cursor(Properties.Resources.Color_Dropper.Handle);
        //                imgView.AllowClickZoom = false;
        //                imgView.PanMode = ImageBoxPanMode.None;
        //                if (!workGetColor.IsBusy) workGetColor.RunWorkerAsync();
        //                return;
        //            }
        //            else imgView.Cursor = Cursors.Default;
        //        }
        //    }

        //    if (Global.StatusDraw != StatusDraw.Edit) return;

        //    try
        //    {
        //        Func<RectRotate> getCurrentRR = GetCurrentRR;
        //        Action<RectRotate> setCurrentRR = SetCurrentRR;

        //        // ====== NHÁNH TẠO MỚI (sau Clear) ======
        //        if (_drag && _maybeCreate &&
        //            ((getCurrentRR() != null ? getCurrentRR()._dragAnchor : AnchorPoint.None) == AnchorPoint.None))
        //        {
        //            _createEndImg = ScreenToImage(e.Location);

        //            // chỉ tạo khi kéo trái -> phải
        //            if (_createEndImg.X > _createStartImg.X)
        //            {
        //                float w = Math.Max(1f, _createEndImg.X - _createStartImg.X);
        //                float yTop = Math.Min(_createStartImg.Y, _createEndImg.Y);
        //                float yBot = Math.Max(_createStartImg.Y, _createEndImg.Y);
        //                float hReal = Math.Max(1f, yBot - yTop);
        //                var center = new PointF(_createStartImg.X + w / 2f, yTop + hReal / 2f);

        //                var rrSrc = getCurrentRR();
        //                ShapeType shape = rrSrc != null ? rrSrc.Shape : ShapeType.Rectangle;
        //                if (shape != ShapeType.Rectangle && shape != ShapeType.Ellipse && shape != ShapeType.Hexagon)
        //                    shape = ShapeType.Rectangle;

        //                var rrNew = new RectRotate(
        //                    new RectangleF(-w / 2f, -hReal / 2f, w, hReal),
        //                    center,
        //                    0f,
        //                    AnchorPoint.None
        //                );
        //                rrNew.Shape = shape; // có thể là Hexagon

        //                _previewNew = rrNew;
        //                _creatingNew = true;
        //                setCurrentRR(rrNew);
        //                imgView.Invalidate();
        //                return;
        //            }
        //            else
        //            {
        //                _previewNew = null;
        //                _creatingNew = false;
        //            }
        //        }

        //        // ====== NHÁNH ĐANG KÉO ======
        //        if (_drag)
        //        {
        //            var rrSrc = getCurrentRR();
        //            if (rrSrc == null) return;

        //            // clone rrSrc
        //            RectRotate rotateRect = new RectRotate(rrSrc._rect, rrSrc._PosCenter, rrSrc._rectRotation, rrSrc._dragAnchor);
        //            rotateRect.Shape = rrSrc.Shape;
        //            if (rrSrc.HexVertexOffsets != null)
        //                for (int i = 0; i < 6; i++) rotateRect.HexVertexOffsets[i] = rrSrc.HexVertexOffsets[i];
        //            rotateRect.PolyLocalPoints.Clear();
        //            if (rrSrc.PolyLocalPoints != null)
        //                for (int i = 0; i < rrSrc.PolyLocalPoints.Count; i++) rotateRect.PolyLocalPoints.Add(rrSrc.PolyLocalPoints[i]);
        //            rotateRect.IsPolygonClosed = rrSrc.IsPolygonClosed;
        //            rotateRect.ActiveVertexIndex = rrSrc.ActiveVertexIndex;
        //            rotateRect.AutoExpandBounds = rrSrc.AutoExpandBounds;
        //            rotateRect.AutoOrientPolygon = rrSrc.AutoOrientPolygon;

        //            // screen->local dùng tâm/góc lúc bắt đầu kéo
        //            var mat = new Matrix();
        //            mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
        //            float s = (float)(imgView.Zoom / 100.0);
        //            mat.Scale(s, s);
        //            mat.Translate(_dragCenter.X, _dragCenter.Y);
        //            mat.Rotate(rotateRect._rectRotation);
        //            mat.Invert();

        //            var point = TransformPoint(mat, new PointF(e.X, e.Y)); // local-space

        //            SizeF deltaSize = SizeF.Empty;
        //            float deltaX = 0f, deltaY = 0f;

        //            // Không resize bbox cho Polygon
        //            bool isPolygonBBoxResize = false;

        //            if (!isPolygonBBoxResize)
        //            {
        //                switch (rotateRect._dragAnchor)
        //                {
        //                    case AnchorPoint.TopLeft:
        //                        {
        //                            var clamped = new PointF(Math.Min(0f, point.X), Math.Min(0f, point.Y));
        //                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
        //                            rotateRect._rect = new RectangleF(
        //                                _dragRect.Left + deltaSize.Width / 2f,
        //                                _dragRect.Top + deltaSize.Height / 2f,
        //                                _dragRect.Width - deltaSize.Width,
        //                                _dragRect.Height - deltaSize.Height);
        //                            deltaX = deltaSize.Width / 2f; deltaY = deltaSize.Height / 2f;
        //                            break;
        //                        }
        //                    case AnchorPoint.TopRight:
        //                        {
        //                            var clamped = new PointF(Math.Max(0f, point.X), Math.Min(0f, point.Y));
        //                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
        //                            rotateRect._rect = new RectangleF(
        //                                _dragRect.Left - deltaSize.Width / 2f,
        //                                _dragRect.Top + deltaSize.Height / 2f,
        //                                _dragRect.Width + deltaSize.Width,
        //                                _dragRect.Height - deltaSize.Height);
        //                            deltaX = deltaSize.Width / 2f; deltaY = deltaSize.Height / 2f;
        //                            break;
        //                        }
        //                    case AnchorPoint.BottomLeft:
        //                        {
        //                            var clamped = new PointF(Math.Min(0f, point.X), Math.Max(0f, point.Y));
        //                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
        //                            rotateRect._rect = new RectangleF(
        //                                _dragRect.Left + deltaSize.Width / 2f,
        //                                _dragRect.Top - deltaSize.Height / 2f,
        //                                _dragRect.Width - deltaSize.Width,
        //                                _dragRect.Height + deltaSize.Height);
        //                            deltaX = deltaSize.Width / 2f; deltaY = deltaSize.Height / 2f;
        //                            break;
        //                        }
        //                    case AnchorPoint.BottomRight:
        //                        {
        //                            var clamped = new PointF(Math.Max(0f, point.X), Math.Max(0f, point.Y));
        //                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
        //                            rotateRect._rect = new RectangleF(
        //                                _dragRect.Left - deltaSize.Width / 2f,
        //                                _dragRect.Top - deltaSize.Height / 2f,
        //                                _dragRect.Width + deltaSize.Width,
        //                                _dragRect.Height + deltaSize.Height);
        //                            deltaX = deltaSize.Width / 2f; deltaY = deltaSize.Height / 2f;
        //                            break;
        //                        }
        //                    case AnchorPoint.Rotation:
        //                        {
        //                            float vx = point.X, vy = -point.Y;
        //                            double len = Math.Sqrt(vx * vx + vy * vy);
        //                            if (len > 1e-6)
        //                            {
        //                                double nx = vx / len, ny = vy / len;
        //                                double dot = Math.Max(-1.0, Math.Min(1.0, ny));
        //                                double ang = Math.Acos(dot);
        //                                if (point.X < 0) ang = -ang;
        //                                float old = rotateRect._rectRotation;
        //                                float deg = (float)(ang * 180.0 / Math.PI);
        //                                if (!float.IsNaN(deg) && Math.Abs(deg) > float.Epsilon) rotateRect._rectRotation += deg;
        //                                if (float.IsNaN(rotateRect._rectRotation)) rotateRect._rectRotation = old;
        //                            }
        //                            break;
        //                        }
        //                    case AnchorPoint.Center:
        //                        {
        //                            if (rotateRect.Shape == ShapeType.Polygon)
        //                            {
        //                                float dx = point.X - _dragStart.X;
        //                                float dy = point.Y - _dragStart.Y;
        //                                rotateRect.TranslatePolygonLocal(dx, dy);

        //                                _polyDirtyDuringDrag = true;   // hoãn chuẩn hoá
        //                                _dragStart = point;            // kéo mượt
        //                            }
        //                            else
        //                            {
        //                                rotateRect._PosCenter = new PointF(point.X - _dragStartOffset.X, point.Y - _dragStartOffset.Y);
        //                            }
        //                            break;
        //                        }
        //                    case AnchorPoint.V0:
        //                    case AnchorPoint.V1:
        //                    case AnchorPoint.V2:
        //                    case AnchorPoint.V3:
        //                    case AnchorPoint.V4:
        //                    case AnchorPoint.V5:
        //                        {
        //                            if (rotateRect.Shape == ShapeType.Hexagon)
        //                            {
        //                                int idx = (int)rotateRect._dragAnchor - (int)AnchorPoint.V0;
        //                                var pLocal = new PointF(point.X, point.Y);
        //                                rotateRect.SetHexVertexByLocalPoint(idx, pLocal);

        //                                if (rotateRect.AutoExpandBounds)
        //                                    rotateRect.RefitBoundsToHexagon();
        //                                else
        //                                    rotateRect._rect = GetPolygonBoundsLocal(rotateRect); // nếu có
        //                            }
        //                            break;
        //                        }
        //                    case AnchorPoint.Vertex:
        //                        {
        //                            if (rotateRect.Shape == ShapeType.Polygon && rotateRect.ActiveVertexIndex >= 0)
        //                            {
        //                                int idx = rotateRect.ActiveVertexIndex;
        //                                var pLocal = new PointF(point.X, point.Y);

        //                                if (idx >= 0 && idx < rotateRect.PolyLocalPoints.Count)
        //                                {
        //                                    rotateRect.PolyLocalPoints[idx] = pLocal;

        //                                    if (rotateRect.IsPolygonClosed && rotateRect.PolyLocalPoints.Count >= 2)
        //                                    {
        //                                        if (idx == 0)
        //                                            rotateRect.PolyLocalPoints[rotateRect.PolyLocalPoints.Count - 1] = pLocal;
        //                                        else if (idx == rotateRect.PolyLocalPoints.Count - 1)
        //                                            rotateRect.PolyLocalPoints[0] = pLocal;
        //                                    }
        //                                }

        //                                _polyDirtyDuringDrag = true;   // hoãn chuẩn hoá
        //                            }
        //                            break;
        //                        }
        //                }
        //            }

        //            // cập nhật _PosCenter theo delta cho resize góc (không áp cho center/rotation/vertex/hex-vertex)
        //            if (rotateRect._dragAnchor != AnchorPoint.None &&
        //                rotateRect._dragAnchor != AnchorPoint.Center &&
        //                rotateRect._dragAnchor != AnchorPoint.Rotation &&
        //                rotateRect._dragAnchor < AnchorPoint.V0)
        //            {
        //                if (deltaX != 0f || deltaY != 0f)
        //                {
        //                    PointF pDelta = RectRotate.Rotate(new PointF(deltaX, deltaY), rotateRect._rectRotation);
        //                    rotateRect._PosCenter = new PointF(_dragCenter.X + pDelta.X, _dragCenter.Y + pDelta.Y);
        //                    IsDone = false;
        //                }
        //            }

        //            // Clamp theo ảnh — BỎ CHO POLYGON
        //            if (Global.TypeCrop == TypeCrop.Area && rotateRect.Shape != ShapeType.Polygon)
        //            {
        //                float x = rotateRect._PosCenter.X - rotateRect._rect.Width / 2f;
        //                float y = rotateRect._PosCenter.Y - rotateRect._rect.Height / 2f;
        //                float w = rotateRect._rect.Width, h = rotateRect._rect.Height;
        //                int maxW = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Width;
        //                int maxH = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Height;

        //                if (x < 0f) rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X - x, rotateRect._PosCenter.Y);
        //                else if (x + w > maxW) rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X - (x + w - maxW), rotateRect._PosCenter.Y);
        //                if (y < 0f) rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y - y);
        //                else if (y + h > maxH) rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y - (y + h - maxH));
        //            }

        //            // Ghi về Propety
        //            var rrNew = new RectRotate(
        //                new RectangleF(rotateRect._rect.X, rotateRect._rect.Y, rotateRect._rect.Width, rotateRect._rect.Height),
        //                new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y),
        //                rotateRect._rectRotation,
        //                rotateRect._dragAnchor
        //            );
        //            rrNew.Shape = rotateRect.Shape;
        //            for (int i = 0; i < 6; i++) rrNew.HexVertexOffsets[i] = rotateRect.HexVertexOffsets[i];
        //            rrNew.PolyLocalPoints.Clear();
        //            for (int i = 0; i < rotateRect.PolyLocalPoints.Count; i++) rrNew.PolyLocalPoints.Add(rotateRect.PolyLocalPoints[i]);
        //            rrNew.IsPolygonClosed = rotateRect.IsPolygonClosed;
        //            rrNew.ActiveVertexIndex = rotateRect.ActiveVertexIndex;
        //            rrNew.AutoExpandBounds = rotateRect.AutoExpandBounds;
        //            rrNew.AutoOrientPolygon = rotateRect.AutoOrientPolygon;

        //            setCurrentRR(rrNew);
        //        }
        //        // ====== NHÁNH HIT-TEST (không kéo) ======
        //        else
        //        {
        //            var rrSrc = getCurrentRR();
        //            if (rrSrc == null) return;

        //            RectRotate rotateRect = new RectRotate(rrSrc._rect, rrSrc._PosCenter, rrSrc._rectRotation, rrSrc._dragAnchor);
        //            rotateRect.Shape = rrSrc.Shape;
        //            if (rrSrc.HexVertexOffsets != null)
        //                for (int i = 0; i < 6; i++) rotateRect.HexVertexOffsets[i] = rrSrc.HexVertexOffsets[i];
        //            rotateRect.PolyLocalPoints.Clear();
        //            if (rrSrc.PolyLocalPoints != null)
        //                for (int i = 0; i < rrSrc.PolyLocalPoints.Count; i++) rotateRect.PolyLocalPoints.Add(rrSrc.PolyLocalPoints[i]);
        //            rotateRect.IsPolygonClosed = rrSrc.IsPolygonClosed;
        //            rotateRect.ActiveVertexIndex = rrSrc.ActiveVertexIndex;
        //            rotateRect.AutoExpandBounds = rrSrc.AutoExpandBounds;
        //            rotateRect.AutoOrientPolygon = rrSrc.AutoOrientPolygon;

        //            var mat = new Matrix();
        //            mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
        //            float s = (float)(imgView.Zoom / 100.0);
        //            mat.Scale(s, s);
        //            mat.Translate(rotateRect._PosCenter.X, rotateRect._PosCenter.Y);
        //            mat.Rotate(rotateRect._rectRotation);
        //            mat.Invert();

        //            var point = TransformPoint(mat, new PointF(e.X, e.Y)); // local

        //            RectangleF baseRect = rotateRect._rect;
        //            RectangleF polyBounds = baseRect; // chỉ để đặt handle xoay

        //            // tính bbox tạm từ polygon (không ghi vào rr)
        //            if (rotateRect.Shape == ShapeType.Polygon && rotateRect.PolyLocalPoints != null && rotateRect.PolyLocalPoints.Count >= 3)
        //                polyBounds = BboxOf(rotateRect.PolyLocalPoints);

        //            float r = Global.Config.RadEdit;

        //            RectangleF rectOuter = new RectangleF(polyBounds.X - r / 2f, polyBounds.Y - r / 2f, polyBounds.Width + r, polyBounds.Height + r);
        //            RectangleF rectTopLeft = new RectangleF(polyBounds.Left - r / 2f, polyBounds.Top - r / 2f, r, r);
        //            RectangleF rectTopRight = new RectangleF(polyBounds.Right - r / 2f, polyBounds.Top - r / 2f, r, r);
        //            RectangleF rectBottomLeft = new RectangleF(polyBounds.Left - r / 2f, polyBounds.Bottom - r / 2f, r, r);
        //            RectangleF rectBottomRight = new RectangleF(polyBounds.Right - r / 2f, polyBounds.Bottom - r / 2f, r, r);
        //            RectangleF rectRotate = new RectangleF(-r / 2f, polyBounds.Top - 2f * r, 2f * r, 2f * r);

        //            _dragCenter = new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y);

        //            bool anchored = false;

        //            // 1) Polygon
        //            if (rotateRect.Shape == ShapeType.Polygon)
        //            {
        //                // ⬇️ polygon CHƯA ĐÓNG: KHÔNG cho bắt anchor nào (chỉ click thêm điểm ở MouseDown)
        //                if (!rotateRect.IsPolygonClosed)
        //                {
        //                    rotateRect._dragAnchor = AnchorPoint.None;
        //                    rotateRect.ActiveVertexIndex = -1;
        //                    // không set anchored -> để các shape khác cũng không bắt
        //                }
        //                else
        //                {
        //                    // ===== polygon đã đóng: bắt đỉnh / xoay / move =====
        //                    for (int i = 0; i < rotateRect.PolyLocalPoints.Count; i++)
        //                    {
        //                        RectangleF h = new RectangleF(rotateRect.PolyLocalPoints[i].X - r / 2f,
        //                                                      rotateRect.PolyLocalPoints[i].Y - r / 2f, r, r);
        //                        if (h.Contains(point))
        //                        {
        //                            _dragStart = new PointF(point.X, point.Y);
        //                            rotateRect._dragAnchor = AnchorPoint.Vertex;
        //                            rotateRect.ActiveVertexIndex = i;
        //                            _dragRect = polyBounds;
        //                            anchored = true;
        //                            break;
        //                        }
        //                    }
        //                    if (!anchored)
        //                    {
        //                        if (rectRotate.Contains(point))
        //                        {
        //                            _dragStart = new PointF(point.X, point.Y);
        //                            rotateRect._dragAnchor = AnchorPoint.Rotation;
        //                            _dragRect = polyBounds;
        //                            _dragRot = rotateRect._rectRotation;
        //                            anchored = true;
        //                        }
        //                        else if (RectRotate.PointInPolygon(rotateRect.PolyLocalPoints, point))
        //                        {
        //                            _dragStart = new PointF(point.X, point.Y);
        //                            rotateRect._dragAnchor = AnchorPoint.Center;
        //                            _dragRect = RectangleF.Empty;
        //                            _dragStartOffset = new PointF(0, 0);
        //                            anchored = true;
        //                        }
        //                    }
        //                }
        //            }

        //            // 2) Hexagon: ưu tiên 6 đỉnh
        //            if (!anchored && rotateRect.Shape == ShapeType.Hexagon)
        //            {
        //                var verts = rotateRect.GetHexagonVerticesLocal();
        //                for (int i = 0; i < 6; i++)
        //                {
        //                    var h = new RectangleF(verts[i].X - r / 2f, verts[i].Y - r / 2f, r, r);
        //                    if (h.Contains(point))
        //                    {
        //                        _dragStart = new PointF(point.X, point.Y);
        //                        rotateRect._dragAnchor = (AnchorPoint)((int)AnchorPoint.V0 + i);
        //                        _dragRect = baseRect; // reference
        //                        anchored = true;
        //                        break;
        //                    }
        //                }
        //            }

        //            // 3) Rectangle/Ellipse (hoặc Hexagon không trúng đỉnh)
        //            if (!anchored && rotateRect.Shape != ShapeType.Polygon)
        //            {
        //                if (rectTopLeft.Contains(point))
        //                { _dragStart = new PointF(point.X, point.Y); rotateRect._dragAnchor = AnchorPoint.TopLeft; _dragRect = baseRect; }
        //                else if (rectTopRight.Contains(point))
        //                { _dragStart = new PointF(point.X, point.Y); rotateRect._dragAnchor = AnchorPoint.TopRight; _dragRect = baseRect; }
        //                else if (rectBottomLeft.Contains(point))
        //                { _dragStart = new PointF(point.X, point.Y); rotateRect._dragAnchor = AnchorPoint.BottomLeft; _dragRect = baseRect; }
        //                else if (rectBottomRight.Contains(point))
        //                { _dragStart = new PointF(point.X, point.Y); rotateRect._dragAnchor = AnchorPoint.BottomRight; _dragRect = baseRect; }
        //                else if (rectRotate.Contains(point))
        //                { _dragStart = new PointF(point.X, point.Y); rotateRect._dragAnchor = AnchorPoint.Rotation; _dragRect = baseRect; _dragRot = rotateRect._rectRotation; }
        //                else if (rectOuter.Contains(point))
        //                { _dragStart = new PointF(point.X, point.Y); rotateRect._dragAnchor = AnchorPoint.Center; _dragRect = baseRect; _dragStartOffset = new PointF(_dragStart.X - rotateRect._PosCenter.X, _dragStart.Y - rotateRect._PosCenter.Y); }
        //                else
        //                {
        //                    rotateRect._dragAnchor = AnchorPoint.None;
        //                }
        //            }

        //            // Ghi lại anchor & active index về rrSrc (không đụng dữ liệu khác)
        //            var rrSet = getCurrentRR();
        //            if (rrSet != null)
        //            {
        //                rrSet._dragAnchor = rotateRect._dragAnchor;
        //                rrSet.ActiveVertexIndex = rotateRect.ActiveVertexIndex;
        //            }
        //        }

        //        // ===== Khoá pan/zoom khi có anchor =====
        //        var cur = GetCurrentRR();
        //        if (cur != null && cur._dragAnchor != AnchorPoint.None)
        //        {
        //            if (Global.StatusDraw != StatusDraw.Color) Global.StatusDraw = StatusDraw.Edit;
        //            imgView.PanMode = ImageBoxPanMode.None;
        //            imgView.AllowClickZoom = false;
        //            imgView.AllowDoubleClick = false;
        //        }
        //        else
        //        {
        //            if (btnPan.IsCLick) imgView.PanMode = ImageBoxPanMode.Left;
        //            imgView.AllowClickZoom = true;
        //            imgView.AllowDoubleClick = true;
        //        }

        //        imgView.Invalidate();
        //    }
        //    catch
        //    {
        //        // log nếu cần
        //    }
        //}

        // ====== MouseUp ======
        private void imgView_MouseUp(object sender, MouseEventArgs e)
        {
            if (Global.IndexToolSelected == -1) return;
            if (Global.IsRun) return;
            // Chốt nhánh tạo mới
            if (_creatingNew)
            {
                float minSize = 3f;
                var rr = GetCurrentRR();
                if (rr != null && rr._rect.Width >= minSize && rr._rect.Height >= minSize)
                {
                    rr._dragAnchor = AnchorPoint.None;
                    rr.ActiveVertexIndex = -1;
                    SetCurrentRR(rr);
                }
                else
                {
                    _previewNew = null;
                    _creatingNew = false;
                }
                IsNewShape = false;
                _maybeCreate = false;
                _creatingNew = false;
                _previewNew = null;
                _drag = false;

                imgView.Invalidate();
                return;
            }

            _drag = false;

            if (BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotCrop != null)
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotCrop._dragAnchor = AnchorPoint.None;

            ToolMouseUp();

            try
            {
                var prop = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety;
                if (prop == null) return;

                RectRotate rr = null;
                if (Global.TypeCrop == TypeCrop.Area) rr = prop.rotArea;
                else if (Global.TypeCrop == TypeCrop.Mask) rr = prop.rotMask;
                else rr = prop.rotCrop;

                if (rr != null)
                {
                    // CHỈ chuẩn hoá khi polygon ĐÃ ĐÓNG
                    if (rr.Shape == ShapeType.Polygon && _polyDirtyDuringDrag)
                    {
                        if (rr.IsPolygonClosed)
                            rr.UpdateFromPolygon(updateAngle: rr.AutoOrientPolygon);

                        _polyDirtyDuringDrag = false;
                    }

                    rr._dragAnchor = AnchorPoint.None;
                    rr.ActiveVertexIndex = -1;
                }

                _drag = false;
                imgView.Invalidate();
            }
            catch { }

            imgView.Invalidate();
        }

        // ===== Helper bbox tạm cho polygon (không ghi vào rr._rect) =====
        private static RectangleF BboxOf(System.Collections.Generic.IList<PointF> pts)
        {
            if (pts == null || pts.Count == 0) return RectangleF.Empty;
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;
            int n = pts.Count;
            int m = (n >= 2 && pts[0].Equals(pts[n - 1])) ? n - 1 : n;
            for (int i = 0; i < m; i++)
            {
                var p = pts[i];
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }
            return new RectangleF(minX, minY, Math.Max(1f, maxX - minX), Math.Max(1f, maxY - minY));
        }


        // ===== MAIN =====


        //private void imgView_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (Global.IndexToolSelected == -1) return;
        //    if (Global.StatusDraw == StatusDraw.Check) Global.StatusDraw = StatusDraw.Edit;

        //    // lưu vị trí chuột để OnPaint vẽ preview (điểm chuột + đường từ điểm cuối polygon)
        //    pMove = e.Location;

        //    if (Global.IsRun) return;

        //    // Color picker
        //    if (Global.IndexToolSelected >= 0)
        //    {
        //        if (BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].TypeTool == TypeTool.Color_Area)
        //        {
        //            if (BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.IsGetColor)
        //            {
        //                imgView.Cursor = new Cursor(Properties.Resources.Color_Dropper.Handle);
        //                imgView.AllowClickZoom = false;
        //                imgView.PanMode = ImageBoxPanMode.None;
        //                if (!workGetColor.IsBusy) workGetColor.RunWorkerAsync();
        //                return;
        //            }
        //            else imgView.Cursor = Cursors.Default;
        //        }
        //    }

        //    if (Global.StatusDraw != StatusDraw.Edit) return;

        //    try
        //    {
        //        RectRotate rotateRect = new RectRotate();

        //        // lấy RectRotate hiện hành theo TypeCrop
        //        Func<RectRotate> getCurrentRR = () =>
        //        {
        //            var prop = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety;
        //            if (Global.TypeCrop == TypeCrop.Area) return prop.rotArea;
        //            else if (Global.TypeCrop == TypeCrop.Mask) return prop.rotMask;
        //            else return prop.rotCrop;
        //        };
        //        Action<RectRotate> setCurrentRR = (rrNew) =>
        //        {
        //            var prop = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety;
        //            if (Global.TypeCrop == TypeCrop.Area) prop.rotArea = rrNew;
        //            else if (Global.TypeCrop == TypeCrop.Mask) prop.rotMask = rrNew;
        //            else prop.rotCrop = rrNew;
        //        };

        //        // ======== _drag == true: đang kéo ========
        //        if (_drag)
        //        {
        //            var rrSrc = getCurrentRR();
        //            if (rrSrc == null) return;

        //            // clone rrSrc
        //            rotateRect = new RectRotate(rrSrc._rect, rrSrc._PosCenter, rrSrc._rectRotation, rrSrc._dragAnchor);
        //            rotateRect.Shape = rrSrc.Shape;
        //            if(rrSrc.HexVertexOffsets!=null)
        //            for (int i = 0; i < 6; i++) rotateRect.HexVertexOffsets[i] = rrSrc.HexVertexOffsets[i];
        //            rotateRect.PolyLocalPoints.Clear();
        //            if(rrSrc.PolyLocalPoints!=null)
        //            for (int i = 0; i < rrSrc.PolyLocalPoints.Count; i++) rotateRect.PolyLocalPoints.Add(rrSrc.PolyLocalPoints[i]);
        //            rotateRect.IsPolygonClosed = rrSrc.IsPolygonClosed;
        //            rotateRect.ActiveVertexIndex = rrSrc.ActiveVertexIndex;

        //            // screen->local dùng tâm/góc tại lúc bắt đầu kéo
        //            var mat = new Matrix();
        //            mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
        //            float s = (float)(imgView.Zoom / 100.0);
        //            mat.Scale(s, s);
        //            mat.Translate(_dragCenter.X, _dragCenter.Y);
        //            mat.Rotate(rotateRect._rectRotation);
        //            mat.Invert();

        //            var point = TransformPoint(mat, new PointF(e.X, e.Y)); // local-space

        //            SizeF deltaSize = SizeF.Empty;
        //            float deltaX = 0f, deltaY = 0f;

        //            // nếu đang resize Polygon qua bounding-rect corners, _dragRect là bounding cũ
        //            bool isPolygonBBoxResize =
        //                (rotateRect.Shape == ShapeType.Polygon) &&
        //                (rotateRect._dragAnchor == AnchorPoint.TopLeft ||
        //                 rotateRect._dragAnchor == AnchorPoint.TopRight ||
        //                 rotateRect._dragAnchor == AnchorPoint.BottomLeft ||
        //                 rotateRect._dragAnchor == AnchorPoint.BottomRight);

        //            if (isPolygonBBoxResize)
        //            {
        //                // Scale toàn bộ PolyLocalPoints theo góc đối diện
        //                RectangleF oldB = _dragRect; // đã set khi hit-test
        //                if (oldB.Width < 1e-6f || oldB.Height < 1e-6f) goto APPLY_BACK;

        //                // Xác định clamp theo anchor
        //                PointF clamped = point;
        //                if (rotateRect._dragAnchor == AnchorPoint.TopLeft)
        //                    clamped = new PointF(Math.Min(oldB.Right, point.X), Math.Min(oldB.Bottom, point.Y));
        //                else if (rotateRect._dragAnchor == AnchorPoint.TopRight)
        //                    clamped = new PointF(Math.Max(oldB.Left, point.X), Math.Min(oldB.Bottom, point.Y));
        //                else if (rotateRect._dragAnchor == AnchorPoint.BottomLeft)
        //                    clamped = new PointF(Math.Min(oldB.Right, point.X), Math.Max(oldB.Top, point.Y));
        //                else if (rotateRect._dragAnchor == AnchorPoint.BottomRight)
        //                    clamped = new PointF(Math.Max(oldB.Left, point.X), Math.Max(oldB.Top, point.Y));

        //                // góc cố định (đối diện) và new bounds
        //                PointF fixedOpp, newTL, newBR;
        //                if (rotateRect._dragAnchor == AnchorPoint.TopLeft)
        //                { fixedOpp = new PointF(oldB.Right, oldB.Bottom); newTL = clamped; newBR = new PointF(oldB.Right, oldB.Bottom); }
        //                else if (rotateRect._dragAnchor == AnchorPoint.TopRight)
        //                { fixedOpp = new PointF(oldB.Left, oldB.Bottom); newTL = new PointF(oldB.Left, clamped.Y); newBR = new PointF(clamped.X, oldB.Bottom); }
        //                else if (rotateRect._dragAnchor == AnchorPoint.BottomLeft)
        //                { fixedOpp = new PointF(oldB.Right, oldB.Top); newTL = new PointF(clamped.X, oldB.Top); newBR = new PointF(oldB.Right, clamped.Y); }
        //                else // BottomRight
        //                { fixedOpp = new PointF(oldB.Left, oldB.Top); newTL = new PointF(oldB.Left, oldB.Top); newBR = clamped; }

        //                float newW = Math.Max(1f, Math.Abs(newBR.X - newTL.X));
        //                float newH = Math.Max(1f, Math.Abs(newBR.Y - newTL.Y));

        //                // scale theo fixedOpp
        //                float oldW = oldB.Width;
        //                float oldH = oldB.Height;
        //                float sx = (newW / oldW);
        //                float sy = (newH / oldH);
        //                if (sx < 1e-6f) sx = 1e-6f;
        //                if (sy < 1e-6f) sy = 1e-6f;

        //                // hướng vector: khi cố định fixedOpp, vector p-oldOpp scale rồi cộng lại
        //                // lưu ý: oldB.Left/Top có thể > fixedOpp.X/Y tuỳ anchor, nhưng công thức vector vẫn đúng.
        //                for (int i = 0; i < rotateRect.PolyLocalPoints.Count; i++)
        //                {
        //                    var p = rotateRect.PolyLocalPoints[i];
        //                    var v = new PointF(p.X - fixedOpp.X, p.Y - fixedOpp.Y);
        //                    var vScaled = new PointF(v.X * sx, v.Y * sy);
        //                    rotateRect.PolyLocalPoints[i] = new PointF(fixedOpp.X + vScaled.X, fixedOpp.Y + vScaled.Y);
        //                }

        //                // cập nhật _rect = new bounding (để hiện handles)
        //                rotateRect._rect = new RectangleF(
        //                    Math.Min(newTL.X, newBR.X),
        //                    Math.Min(newTL.Y, newBR.Y),
        //                    newW, newH
        //                );

        //                // center di chuyển tương tự nhánh rectangle: lấy deltaX/Y để đẩy tâm
        //                // deltaX/Y lấy theo half thay đổi (như code cũ)
        //                deltaX = ((clamped.X - _dragStart.X)) / 2f;
        //                deltaY = ((clamped.Y - _dragStart.Y)) / 2f;
        //            }
        //            else
        //            {
        //                // ===== các case bình thường: rectangle/ellipse/hexagon/center/rotation/vertex =====
        //                switch (rotateRect._dragAnchor)
        //                {
        //                    case AnchorPoint.TopLeft:
        //                        {
        //                            var clamped = new PointF(Math.Min(0f, point.X), Math.Min(0f, point.Y));
        //                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
        //                            rotateRect._rect = new RectangleF(
        //                                _dragRect.Left + deltaSize.Width / 2f,
        //                                _dragRect.Top + deltaSize.Height / 2f,
        //                                _dragRect.Width - deltaSize.Width,
        //                                _dragRect.Height - deltaSize.Height);
        //                            deltaX = deltaSize.Width / 2f; deltaY = deltaSize.Height / 2f;
        //                            break;
        //                        }
        //                    case AnchorPoint.TopRight:
        //                        {
        //                            var clamped = new PointF(Math.Max(0f, point.X), Math.Min(0f, point.Y));
        //                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
        //                            rotateRect._rect = new RectangleF(
        //                                _dragRect.Left - deltaSize.Width / 2f,
        //                                _dragRect.Top + deltaSize.Height / 2f,
        //                                _dragRect.Width + deltaSize.Width,
        //                                _dragRect.Height - deltaSize.Height);
        //                            deltaX = deltaSize.Width / 2f; deltaY = deltaSize.Height / 2f;
        //                            break;
        //                        }
        //                    case AnchorPoint.BottomLeft:
        //                        {
        //                            var clamped = new PointF(Math.Min(0f, point.X), Math.Max(0f, point.Y));
        //                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
        //                            rotateRect._rect = new RectangleF(
        //                                _dragRect.Left + deltaSize.Width / 2f,
        //                                _dragRect.Top - deltaSize.Height / 2f,
        //                                _dragRect.Width - deltaSize.Width,
        //                                _dragRect.Height + deltaSize.Height);
        //                            deltaX = deltaSize.Width / 2f; deltaY = deltaSize.Height / 2f;
        //                            break;
        //                        }
        //                    case AnchorPoint.BottomRight:
        //                        {
        //                            var clamped = new PointF(Math.Max(0f, point.X), Math.Max(0f, point.Y));
        //                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
        //                            rotateRect._rect = new RectangleF(
        //                                _dragRect.Left - deltaSize.Width / 2f,
        //                                _dragRect.Top - deltaSize.Height / 2f,
        //                                _dragRect.Width + deltaSize.Width,
        //                                _dragRect.Height + deltaSize.Height);
        //                            deltaX = deltaSize.Width / 2f; deltaY = deltaSize.Height / 2f;
        //                            break;
        //                        }
        //                    case AnchorPoint.Rotation:
        //                        {
        //                            float vx = point.X, vy = -point.Y;
        //                            double len = Math.Sqrt(vx * vx + vy * vy);
        //                            if (len > 1e-6)
        //                            {
        //                                double nx = vx / len, ny = vy / len;
        //                                double dot = Math.Max(-1.0, Math.Min(1.0, ny)); // dot with Up(0,1)
        //                                double ang = Math.Acos(dot);
        //                                if (point.X < 0) ang = -ang;
        //                                float old = rotateRect._rectRotation;
        //                                float deg = (float)(ang * 180.0 / Math.PI);
        //                                if (!float.IsNaN(deg) && Math.Abs(deg) > float.Epsilon) rotateRect._rectRotation += deg;
        //                                if (float.IsNaN(rotateRect._rectRotation)) rotateRect._rectRotation = old;
        //                            }
        //                            break;
        //                        }
        //                    case AnchorPoint.Center:
        //                        {
        //                            rotateRect._PosCenter = new PointF(point.X - _dragStartOffset.X, point.Y - _dragStartOffset.Y);
        //                            break;
        //                        }
        //                    case AnchorPoint.V0:
        //                    case AnchorPoint.V1:
        //                    case AnchorPoint.V2:
        //                    case AnchorPoint.V3:
        //                    case AnchorPoint.V4:
        //                    case AnchorPoint.V5:
        //                        {
        //                            if (rotateRect.Shape == ShapeType.Hexagon)
        //                            {
        //                                int idx = (int)rotateRect._dragAnchor - (int)AnchorPoint.V0;
        //                                float halfW = _dragRect.Width / 2f, halfH = _dragRect.Height / 2f;
        //                                var pLocal = new PointF(
        //                                    Math.Max(-halfW, Math.Min(halfW, point.X)),
        //                                    Math.Max(-halfH, Math.Min(halfH, point.Y))
        //                                );
        //                                rotateRect.SetHexVertexByLocalPoint(idx, pLocal);
        //                            }
        //                            break;
        //                        }
        //                    case AnchorPoint.Vertex:
        //                        {
        //                            if (rotateRect.Shape == ShapeType.Polygon && rotateRect.ActiveVertexIndex >= 0)
        //                            {
        //                                int idx = rotateRect.ActiveVertexIndex;
        //                                float halfW = _dragRect.Width / 2f, halfH = _dragRect.Height / 2f;
        //                                var pLocal = new PointF(
        //                                    Math.Max(-halfW, Math.Min(halfW, point.X)),
        //                                    Math.Max(-halfH, Math.Min(halfH, point.Y))
        //                                );
        //                                if (idx >= 0 && idx < rotateRect.PolyLocalPoints.Count)
        //                                {
        //                                    rotateRect.PolyLocalPoints[idx] = pLocal;
        //                                    if (rotateRect.IsPolygonClosed && rotateRect.PolyLocalPoints.Count >= 2)
        //                                    {
        //                                        if (idx == 0) rotateRect.PolyLocalPoints[rotateRect.PolyLocalPoints.Count - 1] = pLocal;
        //                                        else if (idx == rotateRect.PolyLocalPoints.Count - 1) rotateRect.PolyLocalPoints[0] = pLocal;
        //                                    }
        //                                }
        //                                // cập nhật _rect = bounds mới để vẽ handles đúng
        //                                rotateRect._rect = GetPolygonBoundsLocal(rotateRect);
        //                            }
        //                            break;
        //                        }
        //                }
        //            }

        //            // sau resize 4 góc: cập nhật PosCenter y như code cũ (dịch theo delta, có quay)
        //            if (rotateRect._dragAnchor != AnchorPoint.None &&
        //                rotateRect._dragAnchor != AnchorPoint.Center &&
        //                rotateRect._dragAnchor != AnchorPoint.Rotation &&
        //                rotateRect._dragAnchor < AnchorPoint.V0)
        //            {
        //                if (deltaX != 0f || deltaY != 0f)
        //                {
        //                    PointF pDelta = RectRotate.Rotate(new PointF(deltaX, deltaY), rotateRect._rectRotation);
        //                    rotateRect._PosCenter = new PointF(_dragCenter.X + pDelta.X, _dragCenter.Y + pDelta.Y);
        //                    IsDone = false;
        //                }
        //            }

        //        APPLY_BACK:
        //            // chặn biên (Area)
        //            if (Global.TypeCrop == TypeCrop.Area)
        //            {
        //                float x = rotateRect._PosCenter.X - rotateRect._rect.Width / 2f;
        //                float y = rotateRect._PosCenter.Y - rotateRect._rect.Height / 2f;
        //                float w = rotateRect._rect.Width, h = rotateRect._rect.Height;
        //                int maxW = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Width;
        //                int maxH = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Height;

        //                if (x < 0f) rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X - x, rotateRect._PosCenter.Y);
        //                else if (x + w > maxW) rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X - (x + w - maxW), rotateRect._PosCenter.Y);
        //                if (y < 0f) rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y - y);
        //                else if (y + h > maxH) rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y - (y + h - maxH));
        //            }

        //            // push về Propety
        //            var rrNew = new RectRotate(
        //                new RectangleF(rotateRect._rect.X, rotateRect._rect.Y, rotateRect._rect.Width, rotateRect._rect.Height),
        //                new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y),
        //                rotateRect._rectRotation,
        //                rotateRect._dragAnchor
        //            );
        //            rrNew.Shape = rotateRect.Shape;
        //            for (int i = 0; i < 6; i++) rrNew.HexVertexOffsets[i] = rotateRect.HexVertexOffsets[i];
        //            rrNew.PolyLocalPoints.Clear();
        //            for (int i = 0; i < rotateRect.PolyLocalPoints.Count; i++) rrNew.PolyLocalPoints.Add(rotateRect.PolyLocalPoints[i]);
        //            rrNew.IsPolygonClosed = rotateRect.IsPolygonClosed;
        //            rrNew.ActiveVertexIndex = rotateRect.ActiveVertexIndex;

        //            setCurrentRR(rrNew);
        //        }
        //        // ======== _drag == false: hit-test ========
        //        else
        //        {
        //            var rrSrc = getCurrentRR();
        //            if (rrSrc == null) return;

        //            rotateRect = new RectRotate(rrSrc._rect, rrSrc._PosCenter, rrSrc._rectRotation, rrSrc._dragAnchor);
        //            rotateRect.Shape = rrSrc.Shape;
        //            if (rrSrc.HexVertexOffsets!=null)
        //            for (int i = 0; i < 6; i++) rotateRect.HexVertexOffsets[i] = rrSrc.HexVertexOffsets[i];
        //            rotateRect.PolyLocalPoints.Clear();
        //            if(rrSrc.PolyLocalPoints!=null)
        //            for (int i = 0; i < rrSrc.PolyLocalPoints.Count; i++) rotateRect.PolyLocalPoints.Add(rrSrc.PolyLocalPoints[i]);
        //            rotateRect.IsPolygonClosed = rrSrc.IsPolygonClosed;
        //            rotateRect.ActiveVertexIndex = rrSrc.ActiveVertexIndex;

        //            var mat = new Matrix();
        //            mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
        //            float s = (float)(imgView.Zoom / 100.0);
        //            mat.Scale(s, s);
        //            mat.Translate(rotateRect._PosCenter.X, rotateRect._PosCenter.Y);
        //            mat.Rotate(rotateRect._rectRotation);
        //            mat.Invert();

        //            var point = TransformPoint(mat, new PointF(e.X, e.Y)); // local

        //            RectangleF baseRect = rotateRect._rect;
        //            // nếu Polygon thì bounding-rect theo điểm (để có handles)
        //            RectangleF polyBounds = (rotateRect.Shape == ShapeType.Polygon) ? GetPolygonBoundsLocal(rotateRect) : baseRect;

        //            float r = Global.Config.RadEdit;

        //            RectangleF rectOuter = new RectangleF(polyBounds.X - r / 2f, polyBounds.Y - r / 2f, polyBounds.Width + r, polyBounds.Height + r);
        //            RectangleF rectTopLeft = new RectangleF(polyBounds.Left - r / 2f, polyBounds.Top - r / 2f, r, r);
        //            RectangleF rectTopRight = new RectangleF(polyBounds.Right - r / 2f, polyBounds.Top - r / 2f, r, r);
        //            RectangleF rectBottomLeft = new RectangleF(polyBounds.Left - r / 2f, polyBounds.Bottom - r / 2f, r, r);
        //            RectangleF rectBottomRight = new RectangleF(polyBounds.Right - r / 2f, polyBounds.Bottom - r / 2f, r, r);
        //            RectangleF rectRotate = new RectangleF(-r / 2f, polyBounds.Top - 2f * r, 2f * r, 2f * r);

        //            _dragCenter = new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y);

        //            bool anchored = false;

        //            // 1) Polygon: ưu tiên chọn đỉnh
        //            if (rotateRect.Shape == ShapeType.Polygon)
        //            {
        //                for (int i = 0; i < rotateRect.PolyLocalPoints.Count; i++)
        //                {
        //                    RectangleF h = new RectangleF(rotateRect.PolyLocalPoints[i].X - r / 2f,
        //                                                  rotateRect.PolyLocalPoints[i].Y - r / 2f, r, r);
        //                    if (h.Contains(point))
        //                    {
        //                        _dragStart = new PointF(point.X, point.Y);
        //                        rotateRect._dragAnchor = AnchorPoint.Vertex;
        //                        rotateRect.ActiveVertexIndex = i;
        //                        _dragRect = polyBounds; // để clamp khi kéo
        //                        anchored = true;
        //                        break;
        //                    }
        //                }
        //                // 2) Nếu không vào đỉnh: cho phép resize bằng 4 góc của bounding-rect
        //                if (!anchored)
        //                {
        //                    if (rectTopLeft.Contains(point))
        //                    {
        //                        _dragStart = new PointF(point.X, point.Y);
        //                        rotateRect._dragAnchor = AnchorPoint.TopLeft;
        //                        _dragRect = polyBounds;
        //                        anchored = true;
        //                    }
        //                    else if (rectTopRight.Contains(point))
        //                    {
        //                        _dragStart = new PointF(point.X, point.Y);
        //                        rotateRect._dragAnchor = AnchorPoint.TopRight;
        //                        _dragRect = polyBounds;
        //                        anchored = true;
        //                    }
        //                    else if (rectBottomLeft.Contains(point))
        //                    {
        //                        _dragStart = new PointF(point.X, point.Y);
        //                        rotateRect._dragAnchor = AnchorPoint.BottomLeft;
        //                        _dragRect = polyBounds;
        //                        anchored = true;
        //                    }
        //                    else if (rectBottomRight.Contains(point))
        //                    {
        //                        _dragStart = new PointF(point.X, point.Y);
        //                        rotateRect._dragAnchor = AnchorPoint.BottomRight;
        //                        _dragRect = polyBounds;
        //                        anchored = true;
        //                    }
        //                }
        //                // 3) Center/Rotation
        //                if (!anchored)
        //                {
        //                    if (rectRotate.Contains(point))
        //                    {
        //                        _dragStart = new PointF(point.X, point.Y);
        //                        rotateRect._dragAnchor = AnchorPoint.Rotation;
        //                        _dragRect = polyBounds;
        //                        _dragRot = rotateRect._rectRotation;
        //                        anchored = true;
        //                    }
        //                    else if (rectOuter.Contains(point))
        //                    {
        //                        _dragStart = new PointF(point.X, point.Y);
        //                        rotateRect._dragAnchor = AnchorPoint.Center;
        //                        _dragRect = polyBounds;
        //                        _dragStartOffset = new PointF(_dragStart.X - rotateRect._PosCenter.X, _dragStart.Y - rotateRect._PosCenter.Y);
        //                        anchored = true;
        //                    }
        //                }
        //            }

        //            // 4) Hexagon: chọn 6 đỉnh trước rồi tới 4 góc/rotation
        //            if (!anchored && rotateRect.Shape == ShapeType.Hexagon)
        //            {
        //                var verts = rotateRect.GetHexagonVerticesLocal();
        //                for (int i = 0; i < 6; i++)
        //                {
        //                    var h = new RectangleF(verts[i].X - r / 2f, verts[i].Y - r / 2f, r, r);
        //                    if (h.Contains(point))
        //                    {
        //                        _dragStart = new PointF(point.X, point.Y);
        //                        rotateRect._dragAnchor = (AnchorPoint)((int)AnchorPoint.V0 + i);
        //                        _dragRect = baseRect;
        //                        anchored = true;
        //                        break;
        //                    }
        //                }
        //            }

        //            // 5) Rectangle/Ellipse (hoặc Hexagon không trúng đỉnh): 4 góc/rotation/center
        //            if (!anchored)
        //            {
        //                if (rectTopLeft.Contains(point))
        //                {
        //                    _dragStart = new PointF(point.X, point.Y);
        //                    rotateRect._dragAnchor = AnchorPoint.TopLeft;
        //                    _dragRect = baseRect;
        //                }
        //                else if (rectTopRight.Contains(point))
        //                {
        //                    _dragStart = new PointF(point.X, point.Y);
        //                    rotateRect._dragAnchor = AnchorPoint.TopRight;
        //                    _dragRect = baseRect;
        //                }
        //                else if (rectBottomLeft.Contains(point))
        //                {
        //                    _dragStart = new PointF(point.X, point.Y);
        //                    rotateRect._dragAnchor = AnchorPoint.BottomLeft;
        //                    _dragRect = baseRect;
        //                }
        //                else if (rectBottomRight.Contains(point))
        //                {
        //                    _dragStart = new PointF(point.X, point.Y);
        //                    rotateRect._dragAnchor = AnchorPoint.BottomRight;
        //                    _dragRect = baseRect;
        //                }
        //                else if (rectRotate.Contains(point))
        //                {
        //                    _dragStart = new PointF(point.X, point.Y);
        //                    rotateRect._dragAnchor = AnchorPoint.Rotation;
        //                    _dragRect = baseRect;
        //                    _dragRot = rotateRect._rectRotation;
        //                }
        //                else if (rectOuter.Contains(point))
        //                {
        //                    _dragStart = new PointF(point.X, point.Y);
        //                    rotateRect._dragAnchor = AnchorPoint.Center;
        //                    _dragRect = baseRect;
        //                    _dragStartOffset = new PointF(_dragStart.X - rotateRect._PosCenter.X, _dragStart.Y - rotateRect._PosCenter.Y);
        //                }
        //                else
        //                {
        //                    rotateRect._dragAnchor = AnchorPoint.None;
        //                }
        //            }

        //            // Lưu anchor & active index trở lại rrSrc
        //            var rrSet = getCurrentRR();
        //            if (rrSet != null)
        //            {
        //                rrSet._dragAnchor = rotateRect._dragAnchor;
        //                rrSet.ActiveVertexIndex = rotateRect.ActiveVertexIndex;
        //            }
        //        }

        //        // khóa pan/zoom khi đang có anchor
        //        if ((getCurrentRR()?._dragAnchor ?? AnchorPoint.None) != AnchorPoint.None)
        //        {
        //            if (Global.StatusDraw != StatusDraw.Color) Global.StatusDraw = StatusDraw.Edit;
        //            imgView.PanMode = ImageBoxPanMode.None;
        //            imgView.AllowClickZoom = false;
        //            imgView.AllowDoubleClick = false;
        //        }
        //        else
        //        {
        //            if (btnPan.IsCLick) imgView.PanMode = ImageBoxPanMode.Left;
        //            imgView.AllowClickZoom = true;
        //            imgView.AllowDoubleClick = true;
        //        }

        //        // BẮT BUỘC gọi Invalidate để:
        //        // - OnPaint vẽ điểm chuột hiện tại (marker)
        //        // - Vẽ đường từ điểm Polygon cuối -> chuột (khi chưa đóng)
        //        imgView.Invalidate();
        //    }
        //    catch (Exception)
        //    {
        //        // log nếu cần
        //    }
        //}

        bool _isPaint;
        float _thiness = 2;
      public   bool IsLoad = false;
        Graphics g;
        bool Durum;
        int x = 5;

     


        private TypeCrop IsTypeArea()
        {
            return Global.TypeCrop;
              
        }
      
        
        //public RectRotate GetPositionAdjustment(RectRotate rotOrigin, RectRotate rotTemp)
        //{
        //    if (G.Y_Adjustment == 0) return rotOrigin;
        //    System.Drawing.Size sz = BeeCore.Common.SizeCCD();
        //    RectRotate rot = new RectRotate();

        //    rot._rect = rotOrigin._rect;

        //    PointF pPos = new PointF(rotTemp._PosCenter.X + G.X_Adjustment, rotTemp._PosCenter.Y + G.Y_Adjustment);
        //    double DeltaX = rotOrigin._PosCenter.X - rotTemp._PosCenter.X;
        //    double DeltaY = rotOrigin._PosCenter.Y - rotTemp._PosCenter.Y;
        //    double angle1 = Math.Atan(G.Y_Adjustment / G.X_Adjustment) * 180 / Math.PI;

        //    double angle2 = 90 - Math.Abs(angle1);
        //    rot._rectRotation = rotOrigin._rectRotation +(float) angle2+G.angle_Adjustment;
        //    int dauX = 1; int dauY = 1;
        //    if (DeltaX != 0)
        //        dauX = DeltaX > 0 ? 1 : -1;
        //    if (DeltaY != 0)
        //        dauY = DeltaY > 0 ? 1 : -1;
        //       double cos1 = Math.Cos((angle2) * Math.PI / 180);
        //       double sin1 = Math.Sin((angle2) * Math.PI / 180);

        //    double R = GetDistance(rotOrigin._PosCenter.X, rotOrigin._PosCenter.Y, rotTemp._PosCenter.X, rotTemp._PosCenter.Y);
        //    double DelX =R* cos1;
        //    double DelY = R * sin1;



        //    rot._PosCenter = new PointF(pPos.X - (float)DelX , pPos.Y -(float)DelY * dauY);
        //    double R2 = GetDistance(rot._PosCenter.X, rot._PosCenter.Y, pPos.X, pPos.Y);

        //    return rot;
        //}

     
        Graphics gc;

        StatusDraw oldStatus = StatusDraw.Edit;
       // public List<RectRotate> listChoose = new List<RectRotate>();
        private void imgView_Paint(object sender, PaintEventArgs e)
        {

            if (!Global.IsLive&&Global.IsRun)
            {

                // Vẽ ảnh 2 cũng fit và canh giữa (ví dụ overlay trong suốt)
                //  DrawImageFit(e.Graphics, bmp2, targetRect);

                //  gcResult = gc;

                return;
            }




            gc = e.Graphics;
            gc.SmoothingMode= SmoothingMode.AntiAlias;
             var mat = new Matrix();

            mat = new Matrix();
           
            //if (Global.StatusDraw == StatusDraw.Choose)
            //{
            //    foreach (RectRotate rot in listChoose)
            //    {
            //        mat = new Matrix();
            //        mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
            //        mat.Scale((float)(imgView.Zoom / 100.0), (float)(imgView.Zoom / 100.0));
            //        gc.Transform = mat;
            //        mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
            //        mat.Rotate(rot._rectRotation);
            //        gc.Transform = mat;
            //        Pen pen = new Pen(Global.Config.ColorNone,Global.Config.ThicknessLine);
            //        if(rot._dragAnchor==AnchorPoint.Center)
            //        {
            //            pen= new Pen(Global.Config.ColorChoose, Global.Config.ThicknessLine);
            //        }    
            //        gc.DrawPolygon(pen, rot.PolyLocalPoints.ToArray());
            //        gc.ResetTransform();
            //    }
            //    return;
            //}
            if(Global.Config.SizeCCD.Width==0)
            {
                if (!Global.IsLive)
                    if(BeeCore.Common.listCamera[Global.IndexChoose]!=null)
                    Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexChoose].GetSzCCD();//
            }    
            int index = 0;
            if (Global.IsLive)
                gc.DrawString("LIVE", new Font("Arial", Global.Config.FontSize,FontStyle.Bold), Brushes.Red, new Point(50, 50));
            if (Global.Config.IsShowGird)
            {
                int W = Global.Config.SizeCCD.Width, H = Global.Config.SizeCCD.Height;
                int step = Math.Min(W, H) / 15;
                for (int x = step; x < W; x += step)
                    gc.DrawLine(new Pen(Brushes.Gray, 1), x, 0, x, H);
                for (int y = step; y < H; y += step)
                    gc.DrawLine(new Pen(Brushes.Gray, 1), 0, y, W, y);
            }
            if (Global.Config.IsShowCenter)
                {
                    gc.DrawLine(new Pen(Brushes.Blue, 1), Global.Config.SizeCCD.Width / 2, 0, Global.Config.SizeCCD.Width / 2, Global.Config.SizeCCD.Height);
                    gc.DrawLine(new Pen(Brushes.Blue, 1), 0, Global.Config.SizeCCD.Height / 2, Global.Config.SizeCCD.Width, Global.Config.SizeCCD.Height / 2);
                }
               
            gc.ResetTransform();
            if (Global.Config.IsShowArea)
                {
                    int indexTool = 0;
                    foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[Global.IndexChoose])
                    {
                        RectRotate rot = PropetyTool.Control.Propety.rotArea;
                    if (rot == null) continue;
                        mat = new Matrix();
                        mat.Scale((float)(imgView.Zoom / 100.0), (float)(imgView.Zoom / 100.0));
                        mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                        mat.Rotate(rot._rectRotation);
                        RectangleF _rect3 = rot._rect;
                        gc.Transform = mat;
                        gc.DrawRectangle(new Pen(Color.Blue, 1), new Rectangle((int)_rect3.X, (int)_rect3.Y, (int)_rect3.Width, (int)_rect3.Height));
                        String s = (int)(indexTool + 1) + "." + BeeCore.Common.PropetyTools[Global.IndexChoose][indexTool].Name;
                        SizeF sz = gc.MeasureString(s, new Font("Arial", 10, FontStyle.Bold));
                        gc.FillRectangle(Brushes.Red, new Rectangle((int)rot._rect.X, (int)rot._rect.Y, (int)sz.Width, (int)sz.Height));
                        gc.DrawString(s, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new System.Drawing.Point((int)rot._rect.X, (int)rot._rect.Y));
                        indexTool++;
                        gc.ResetTransform();
                   
                   

                }

            }
            if (Global.IsRun)
            {

                // Vẽ ảnh 2 cũng fit và canh giữa (ví dụ overlay trong suốt)
                //  DrawImageFit(e.Graphics, bmp2, targetRect);

                //  gcResult = gc;

                return;
            }
            if ( Global.IndexToolSelected == -1)
            {
                //
              
                //  gcResult = gc;

                return;
            }
          //  if (Global.StatusDraw == StatusDraw.None)
                if (toolEdit != null)
                    foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[Global.IndexChoose])
                    {
                        if (index != BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.Index )
                        {
                            RectRotate rot = PropetyTool.Control.Propety.rotArea;
                        if (rot != null)
                        {
                            mat = new Matrix();
                            mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
                            mat.Scale((float)(imgView.Zoom / 100.0), (float)(imgView.Zoom / 100.0));

                            mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                            mat.Rotate(rot._rectRotation);
                            RectangleF _rect3 = rot._rect;
                            gc.Transform = mat;
                            gc.DrawRectangle(new Pen(Color.Cornsilk, 4), new Rectangle((int)_rect3.X, (int)_rect3.Y, (int)_rect3.Width, (int)_rect3.Height));
                            gc.ResetTransform();
                        }
                        }
                        index++;
                    }

               
                if (toolEdit == null)
                    return;
                if (imgView.Image == null)
                    return;

              
                Pen penRect = new Pen(Color.Orange, 2);

        
            if (Global.StatusDraw == StatusDraw.Check||Global.StatusDraw==StatusDraw.Scan)
            {
           
                gc.ResetTransform();
              
                Global.ScaleZoom = (float)(imgView.Zoom / 100.0);
                Global.pScroll = new Point(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
                
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.DrawResult(gc);
                Global.EditTool.lbCTTool.Text = Math.Round(BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].CycleTime) + "ms";
                Global.EditTool.lbRsTool.Text = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Results.ToString();
                if (BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Results==Results.OK)
                {
                    Global.EditTool.lbRsTool.BackColor = Global.Config.ColorOK;
                }
                else
                    Global.EditTool.lbRsTool.BackColor = Global.Config.ColorNG;


                //return;
            }
            else if (Global.StatusDraw == StatusDraw.Edit)
            {
                switch (Global.TypeCrop)
                {
                    case TypeCrop.Crop:
                        Draws.FillRect(gc, TypeCrop.Area, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotArea, imgView.AutoScrollPosition, imgView.Zoom, 20);
                        Draws.FillRect(gc, TypeCrop.Mask, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotMask, imgView.AutoScrollPosition, imgView.Zoom, 50);
                        Draws.RectEdit(gc, TypeCrop.Crop, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotCrop, Properties.Resources.Rotate2, Global.Config.RadEdit, imgView.AutoScrollPosition, imgView.Zoom, pMove, 4);

                        break;
                    case TypeCrop.Area:

                        Draws.FillRect(gc, TypeCrop.Crop, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotCrop, imgView.AutoScrollPosition, imgView.Zoom, 20);
                        Draws.FillRect(gc, TypeCrop.Mask, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotMask, imgView.AutoScrollPosition, imgView.Zoom, 50);
                        Draws.RectEdit(gc, TypeCrop.Area, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotArea, Properties.Resources.Rotate2, Global.Config.RadEdit, imgView.AutoScrollPosition, imgView.Zoom, pMove, 4);
                        break;
                    case TypeCrop.Mask:
                        Draws.FillRect(gc, TypeCrop.Area, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotArea, imgView.AutoScrollPosition, imgView.Zoom, 20);
                        Draws.FillRect(gc, TypeCrop.Crop, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotCrop, imgView.AutoScrollPosition, imgView.Zoom, 50);
                        Draws.RectEdit(gc, TypeCrop.Mask, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotMask, Properties.Resources.Rotate2, Global.Config.RadEdit, imgView.AutoScrollPosition, imgView.Zoom, pMove, 4);

                        break;

                }

                gc.ResetTransform();
            }
      

                try
                {
                   
                
               
                    if (BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.IsGetColor)
                    {
                    gc.ResetTransform();

                    gc.DrawEllipse(new Pen(clChoose, 5), new Rectangle(new Point(pMove.X - 25, pMove.Y - 25), new Size(50, 50)));
                   
                    }
                }
                catch (Exception)
                {

                }
         
        }
        public void tool_MouseMove(object sender, MouseEventArgs e)
        {

         //   G.IsCheck = false;
            imgView.Invalidate();
        }
     
        private void View_Load(object sender, EventArgs e)
        {

            pImg.Register("Res", () => RegisterImgs);
            pImg.Register("Sim", () =>SimImgs);
            if (G.Header == null) return;
            //  this.pBtn.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar,Global.Config.colorGui);
            Global.Config.IsShowArea = false;
            Global.Config.IsShowCenter = false;
            Global.Config.IsShowGird = false;
            Global.Config.IsShowResult = true;
            Global.Config.IsShowMatProcess = true;
            Global.Config.IsShowNotMatching = true;
            showResultTool.Checked = Global.Config.IsShowResult;
            showDetailTool.Checked = Global.Config.IsShowDetail;
            showImageFilter.Checked = Global.Config.IsShowMatProcess;
            showDetailWrong.Checked = Global.Config.IsShowNotMatching;
            //   pBtn.Height = (int)(pBtn.Height * Global.PerScaleHeight);
            //  

            // BeeCore.Common.Scan();
            KeyboardListener.s_KeyEventHandler += KeyboardListener_s_KeyEventHandler1;
            Global.IndexToolChanged += Global_IndexToolChanged;
            Global.StatusDrawChanged += Global_StatusDrawChanged;
            Global.TypeCropChanged += Global_TypeCropChanged;
              Global.StatusProcessingChanged += Global_StatusProcessingChanged;
            Global.LiveChanged += Global_LiveChanged;
            // tmProcessing.Enabled = true;

            // toolEdit.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tool_MouseMove);
            //BeeCore.Common.listCamera[Global.IndexChoose].matRaw= BeeCore.Common.GetImageRaw();
            //if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw!=null)
            //    if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty())
            //        imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
            btnMenu.PerformClick();
            Global.ScaleZoom = (float)(imgView.Zoom / 100.0);
            Global.pScroll = new Point(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
            Checking1.StatusProcessingChanged += Checking1_StatusProcessingChanged;
            Checking2.StatusProcessingChanged += Checking2_StatusProcessingChanged;
            Checking3.StatusProcessingChanged += Checking3_StatusProcessingChanged;
            Checking4.StatusProcessingChanged += Checking4_StatusProcessingChanged;
            Global.ParaCommon.ExternalChange += ParaCommon_ExternalChange;
            Global.StatusProcessing=StatusProcessing.None;
            //time
            _renderer = new CollageRenderer(imgView, gutter: 8, background: Color.White, autoRerenderOnResize: true);
            imgView.AutoCenter = true;
          
            if (Global.ParaCommon.Comunication.Protocol == null)
                Global.ParaCommon.Comunication.Protocol = new ParaProtocol();
            RefreshExternal(Global.ParaCommon.IsExternal);
            Global.PLCStatusChanged += Global_PLCStatusChanged;
            Global.CameraStatusChanged += Global_CameraStatusChanged;
            Global.ChangeProg += Global_ChangeProg;
        }

        private void Global_ChangeProg(bool obj)
        {
            this.Invoke((Action)(() =>
            {
                if (obj)
                {
                    G.StatusDashboard.StatusText = "---";
                    G.StatusDashboard.StatusBlockBackColor = Global.ColorNone;
                    if (imgView.Image != null)
                    {
                        imgView.Image.Dispose();   // tránh leak bộ nhớ nếu là Bitmap tự tạo
                        imgView.Image = null;      // xoá ảnh khỏi control
                    }
                    imgView.Text = "Wait Change Program ...";
                }

                else
                {

                    imgView.Text = "";
                }
            }));
        }

        private  void Global_LiveChanged(bool obj)
        {
            this.Invoke((Action)(() =>
            {
                G.Header.btnMode.Enabled = !Global.IsLive;
               if(!Global.IsLive)
                {
                    G.StatusDashboard.StatusText = "---";
                    G.StatusDashboard.StatusBlockBackColor = Global.ColorNone;
                    imgView.Text = "Wait Trigger ..";
                    
                    Live();
                }    
                    
                else
                {
                   
                    G.StatusDashboard.StatusText ="LIVE";
                    G.StatusDashboard.StatusBlockBackColor = Color.Red;
                    imgView.Text = "";
                    Live();
                }  
               
            }));
        }

        private void Global_CameraStatusChanged(CameraStatus obj)
        {
            switch (obj)
            {
                case CameraStatus.NotConnect:
                   
                    Global.EditTool.lbCam.Image = Properties.Resources.CameraNotConnect;
                    Global.EditTool.lbCam.Text = "Camera Not Connect";
                    break;
                case CameraStatus.Ready:
                    Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.NoneErr;
                    Global.EditTool.lbCam.Image = Properties.Resources.CameraConnected;
                    Global.EditTool.lbCam.Text = "Camera Connected";
                    break;
                case CameraStatus.ErrorConnect:
                    this.Invoke((Action)(() =>
                    {
                        Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.Error;
                        Global.EditTool.lbCam.Image = Properties.Resources.CameraNotConnect;
                        Global.EditTool.lbCam.Text = "Camera Error Connect";
                        ForrmAlarm forrmAlarm = new ForrmAlarm();
                        forrmAlarm.lbHeader.Text = "Camera Error Connect !!";
                        forrmAlarm.lbContent.Text = "Checking Connect Camera";
                        forrmAlarm.lbCode.Text = "0x001";
                        forrmAlarm.btnCancel.Text = "Retry";
                        forrmAlarm.BringToFront();
                        forrmAlarm.TopMost = true;
                        forrmAlarm.ShowDialog();
                       
                    }));
                    break;

            }
        }

        private void Global_PLCStatusChanged(PLCStatus obj)
        {
          switch(obj)
            {
                case PLCStatus.NotConnect:
                    Global.ParaCommon.Comunication.Protocol.IsBypass = true;
                    break;
                case PLCStatus.Ready:
                    Global.ParaCommon.Comunication.Protocol.IsBypass = false;
                    Global.EditTool.toolStripPort.Text = "PLC Ready";
                    Global.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;
                    break;
                case PLCStatus.ErrorConnect:
                    this.Invoke((Action)(() =>
                    {
                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "PLC", "PLC Error Connect"));
                        Global.EditTool.toolStripPort.Text = "PLC Error Connect";
                        Global.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;
                        Global.ParaCommon.Comunication.Protocol.IsBypass = true;
                      //  G.Main.Hide();
                        ForrmAlarm forrmAlarm = new ForrmAlarm();
                        forrmAlarm.lbHeader.Text = "PLC not Alive !!";
                        forrmAlarm.lbContent.Text = "Checking Mode RUN of PLC";
                        forrmAlarm.BringToFront();
                        forrmAlarm.lbCode.Text = "0x002";
                        forrmAlarm.btnCancel.Text = "Retry";
                        forrmAlarm.TopMost = true;

                        forrmAlarm.ShowDialog();
                      //  G.Main.Show();
                    }));
                  
                   
                    break;

            }    
        }

        public void RefreshExternal(bool obj)

        {
            btnTypeTrig.Enabled = Global.IsRun;
            if (!obj)
            {
                //btnTypeTrig.Enabled = false;
                btnTypeTrig.Text = "Trig Internal";
                if (Global.IsRun)
                {
                    btnCap.Enabled = true;
                    btnContinuous.Enabled = true;
                  
                }
                else
                {
                   
                    btnCap.Enabled = false;
                    btnContinuous.Enabled = false;
                 
                }    
            }
            else
            {
                // btnTypeTrig.Enabled = true;
                if (Global.ParaCommon.Comunication.Protocol.IsBypass)
                    btnTypeTrig.Text = "ByPass I/O";
                else
                    btnTypeTrig.Text = "Trig External";
                btnCap.Enabled = false;
                btnContinuous.Enabled = false;
             
            }
        }


        private void ParaCommon_ExternalChange(bool obj)
        {
            Global.StatusProcessing = StatusProcessing.None;
            Global.StatusIO = StatusIO.None;
            Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.ChangeMode;
            if (!obj)
            {
                //btnTypeTrig.Enabled = false;
                btnTypeTrig.Text = "Trig Internal";
                btnCap.Enabled = true;
                btnContinuous.Enabled = true;
             
              
            }
            else
            {
               // btnTypeTrig.Enabled = true;
               if(Global.ParaCommon.Comunication.Protocol.IsBypass)
                    btnTypeTrig.Text = "ByPass I/O";
                else
                    btnTypeTrig.Text = "Trig External";
                btnCap.Enabled = false;
                btnContinuous.Enabled = false;
              
              
                
            }
        }

        private void Checking4_StatusProcessingChanged(StatusProcessing obj)
        {
			Processing4 = obj;
		}

        private void Checking3_StatusProcessingChanged(StatusProcessing obj)
        {
			Processing3 = obj;
		}

        private void Checking2_StatusProcessingChanged(StatusProcessing obj)
        {
			Processing2= obj;
		}

        private void Checking1_StatusProcessingChanged(StatusProcessing obj)
        {
            Processing1 = obj;
        }
        String CTTotol = "";
        private void Global_StatusProcessingChanged(StatusProcessing obj)
        {

            //   Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "Processing", obj.ToString()));
            switch (obj)
            {
                case StatusProcessing.None:
                    break;
                case StatusProcessing.Trigger:

                    timer = CycleTimerSplit.Start();
                    this.Invoke((Action)(() =>
                    {
                        if (imgView.Image != null)
                        {
                            imgView.Text = "Waiting Checking...";
                            imgView.Image.Dispose();   // tránh leak bộ nhớ nếu là Bitmap tự tạo
                            imgView.Image = null;      // xoá ảnh khỏi control
                        }
                    }));
                    Global.IsAllowReadPLC = false;
                    if (Global.IsDebug)
                    {
                        G.StatusDashboard.StatusText = obj.ToString();
                        G.StatusDashboard.StatusBlockBackColor = Global.ColorNone;
                    }
                    else
                    {
                        G.StatusDashboard.StatusText = "---";
                        G.StatusDashboard.StatusBlockBackColor = Global.ColorNone;
                    }    
                    break;
                case StatusProcessing.Read:
                   timer.Split("R");
                    Global.IsAllowReadPLC = false;
                    if (Global.IsDebug)
                    {
                        G.StatusDashboard.StatusText = obj.ToString();
                        G.StatusDashboard.StatusBlockBackColor = Global.ColorNone;
                    }
                    if (!workReadCCD.IsBusy)
                        workReadCCD.RunWorkerAsync();
                    //if (!Global.IsRun)
                    //{

                    //    BeeCore.Common.listCamera[Global.IndexChoose].Read();
                    //    if (BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera==TypeCamera.USB)
                    //        BeeCore.Common.listCamera[Global.IndexChoose].Read();

                    //}
                    //else
                    //{
                    //if (Global.Config.IsMultiCamera)
                    //{
                    //    Parallel.ForEach(BeeCore.Common.listCamera, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, camera =>
                    //    {
                    //        if (camera != null)
                    //        {
                    //            camera.Read();
                    //            if (camera.Para.TypeCamera == TypeCamera.USB)
                    //               camera.Read();
                    //        }

                    //    });
                    //}
                    //else
                    //{
                    //  BeeCore.Common.listCamera[0].Read();
                    //    if (BeeCore.Common.listCamera[0].Para.TypeCamera == TypeCamera.USB)
                    //       BeeCore.Common.listCamera[0].Read();
                    //    //switch (Global.TriggerNum)
                    //    //{
                    //    //    case TriggerNum.Trigger1:
                    //    //        BeeCore.Common.listCamera[0].Read();
                    //    //        if (BeeCore.Common.listCamera[0].Para.TypeCamera == TypeCamera.USB)
                    //    //            BeeCore.Common.listCamera[0].Read();
                    //    //        break;
                    //    //    case TriggerNum.Trigger2:
                    //    //        BeeCore.Common.listCamera[1].Read();
                    //    //        break;
                    //    //    case TriggerNum.Trigger3:
                    //    //        BeeCore.Common.listCamera[2].Read();
                    //    //        break;
                    //    //    case TriggerNum.Trigger4:
                    //    //        BeeCore.Common.listCamera[3].Read();
                    //    //        break;


                    //    //}
                    //}
                    ////}
                    
                    //if (Global.StatusMode == StatusMode.Continuous || Global.StatusMode == StatusMode.Once)
                    //{

                    //    Global.StatusProcessing = StatusProcessing.Checking;
                    //    if (Global.IsByPassResult)
                    //        Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.ByPass;

                    //}

                    break;
                case StatusProcessing.Checking:
                    if (timer == null) timer = CycleTimerSplit.Start();
                    timer.Split("C");
                    Global.IsAllowReadPLC = false;
                    if (Global.IsDebug)
                    {
                        G.StatusDashboard.StatusText = obj.ToString();
                        G.StatusDashboard.StatusBlockBackColor = Global.ColorNone;
                    }
                        // G.StatusDashboard.Refresh();
                        RunProcessing();
                        Global.StatusProcessing = StatusProcessing.WaitingDone;
                    
                    break;
                case StatusProcessing.SendResult:
                    this.Invoke((Action)(() =>
                    {
                        imgView.Text = "Waiting Show Picture ..";
                    }));
                    timer.Split("P");
                    Global.IsAllowReadPLC = false;
                    if (Global.IsDebug)
                    {
                       
                        G.StatusDashboard.StatusText = obj.ToString();
                        G.StatusDashboard.StatusBlockBackColor = Global.ColorNone;
                    }
                    Global.ParaCommon.Comunication.Protocol.IsLogic1 = false;
                    Global.ParaCommon.Comunication.Protocol.IsLogic2 = false;
                    Global.ParaCommon.Comunication.Protocol.IsLogic3 = false;
                    Global.ParaCommon.Comunication.Protocol.IsLogic4 = false;
                    Global.ParaCommon.Comunication.Protocol.IsLogic5 = false;
                    Global.ParaCommon.Comunication.Protocol.IsLogic6 = false;
                    foreach (int ix in Global.ParaCommon.indexLogic1)
                        if (ix < BeeCore.Common.PropetyTools[Global.IndexChoose].Count())
                        {
                            if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG && Global.Config.IsONNG == true)
                            {
                                Global.ParaCommon.Comunication.Protocol.IsLogic1 = true;
                                break;
                            }
                            else if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.OK && Global.Config.IsONNG == false)
                            {
                                Global.ParaCommon.Comunication.Protocol.IsLogic1 = true;
                                break;
                            }
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic2)
                        if (ix < BeeCore.Common.PropetyTools[Global.IndexChoose].Count())
                        {
                            if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG && Global.Config.IsONNG == true)
                            {
                                Global.ParaCommon.Comunication.Protocol.IsLogic2 = true;
                                break;
                            }
                            else if(BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.OK && Global.Config.IsONNG == false)
                            {
                                Global.ParaCommon.Comunication.Protocol.IsLogic2 = true;
                                break;
                            }
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic3)
                        if (ix < BeeCore.Common.PropetyTools[Global.IndexChoose].Count())
                        {
                            if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG && Global.Config.IsONNG == true)
                            {
                                Global.ParaCommon.Comunication.Protocol.IsLogic3 = true;
                                break;
                            }
                            else if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.OK && Global.Config.IsONNG == false)
                            {
                                Global.ParaCommon.Comunication.Protocol.IsLogic3 = true;
                                break;
                            }
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic4)
                        if (ix < BeeCore.Common.PropetyTools[Global.IndexChoose].Count())
                        {
                            if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG && Global.Config.IsONNG == true)
                            {
                                Global.ParaCommon.Comunication.Protocol.IsLogic4 = true;
                                break;
                            }
                            else if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.OK && Global.Config.IsONNG == false)
                            {
                                Global.ParaCommon.Comunication.Protocol.IsLogic4 = true;
                                break;
                            }
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic5)
                        if (ix < BeeCore.Common.PropetyTools[Global.IndexChoose].Count())
                        {
                            if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG && Global.Config.IsONNG == true)
                            {
                                Global.ParaCommon.Comunication.Protocol.IsLogic5 = true;
                                break;
                            }
                            else if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.OK && Global.Config.IsONNG == false)
                            {
                                Global.ParaCommon.Comunication.Protocol.IsLogic5 = true;
                                break;
                            }
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic6)
                        if (ix < BeeCore.Common.PropetyTools[Global.IndexChoose].Count())
                        {
                            if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG && Global.Config.IsONNG == true)
                            {
                                Global.ParaCommon.Comunication.Protocol.IsLogic6 = true;
                                break;
                            }
                            else if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.OK && Global.Config.IsONNG == false)
                            {
                                Global.ParaCommon.Comunication.Protocol.IsLogic6 =  true;
                                break;
                            }
                        }
                    Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.Result;

                    if (Global.TotalOK)
                    {
                        G.StatusDashboard.StatusText = "OK";
                        G.StatusDashboard.StatusBlockBackColor = Color.FromArgb(255, 27, 186, 98);
                        Global.Config.SumOK++;


                    }
                    else
                    {
                        G.StatusDashboard.StatusText = "NG";
                        G.StatusDashboard.StatusBlockBackColor = Color.DarkRed;
                        Global.Config.SumNG++;


                    }

                    // G.StatusDashboard.Refresh();
                    if (Global.ParaCommon.Comunication.Protocol.IsBypass)
                        Global.StatusProcessing = StatusProcessing.Drawing;
                    break;
                case StatusProcessing.Drawing:
                   
                    Global.IsAllowReadPLC = true;

                    timer.Split("W");
                   
                    CTTotol = timer.StopAndFormat();
                    BeeCore.Common.CycleCamera = timer.seg[timer.seg.FindIndex(a => a.Label == "C")].Ms;
                 
                    if (Global.IsDebug)
                    {

                        G.StatusDashboard.StatusText = obj.ToString();
                        G.StatusDashboard.StatusBlockBackColor = Global.ColorNone;
                    }

                 

                    this.Invoke((Action)(() =>
                    {
                      
                        ShowResultTotal();
                       
                      
                        CheckStatusMode();
                    }));
                    Global.StatusProcessing = StatusProcessing.Done;

                  

                    break;
                case StatusProcessing.Done:
                    {
                        this.Invoke((Action)(() =>
                        {
                            imgView.Text = "";
                        }));
                        if (Global.IsAutoTemp)
                        {
                            Global.IsAutoTemp = false;
                            G.Header.btnTraining.IsCLick = false;
                        }

                       
                     
                       
                        Global.Config.SumTime = Global.Config.SumOK + Global.Config.SumNG;
                        G.StatusDashboard.CycleTime = (int)(timer.TT + Cyclyle1);
                        G.StatusDashboard.CamTime = (int)BeeCore.Common.CycleCamera;
                        G.StatusDashboard.TotalTimes = Global.Config.SumTime;
                        G.StatusDashboard.OkCount = Global.Config.SumOK; 
                        G.StatusDashboard.NgCount = Global.Config.SumNG;
                        Global.Config.TotalTime += Convert.ToSingle(G.StatusDashboard.CycleTime / (60000.0));
                        Global.Config.Percent = Convert.ToSingle(((Global.Config.SumOK * 1.0) / (Global.Config.SumOK + Global.Config.SumNG)) * 100.0);
                        switch (Global.TriggerNum)
                        {
                            case TriggerNum.Trigger1:
                                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "Result1", Global.TotalOK.ToString()));
                                break;
                            case TriggerNum.Trigger2:
                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "Result2", Global.TotalOK.ToString()));
                                break;
                            case TriggerNum.Trigger3:
                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "Result3", Global.TotalOK.ToString()));
                                break;
                            case TriggerNum.Trigger4:
                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "Result4", Global.TotalOK.ToString()));
                                break;
                        }
                       
                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.INFO, "CT", CTTotol));

                        Global.StatusProcessing = StatusProcessing.None;
                        Checking1.StatusProcessing = StatusProcessing.None;
                        Checking2.StatusProcessing = StatusProcessing.None;
                        Checking3.StatusProcessing = StatusProcessing.None;
                        Checking4.StatusProcessing = StatusProcessing.None;
                        switch (Global.Config.NumTrig)
                        {
                            case 1:
                                if (Global.TriggerNum == TriggerNum.Trigger1)
                                    Global.TriggerNum = TriggerNum.Trigger0;
                                break;
                            case 2:
                                if (Global.TriggerNum == TriggerNum.Trigger2)
                                    Global.TriggerNum = TriggerNum.Trigger0;
                                break;
                            case 3:
                                if (Global.TriggerNum == TriggerNum.Trigger3)
                                    Global.TriggerNum = TriggerNum.Trigger0;
                                break;
                            case 4:
                                if (Global.TriggerNum == TriggerNum.Trigger4)
                                    Global.TriggerNum = TriggerNum.Trigger0;
                                break;
                        }
                        break;
                    }
            }
        }

        private void Global_TypeCropChanged(TypeCrop obj)
        {
            imgView.Invalidate();
        }

        private void Global_StatusDrawChanged(StatusDraw obj)
        {
            if(obj==StatusDraw.Color)
            {
                imgView.PanMode = ImageBoxPanMode.None;
                imgView.AllowClickZoom = false;
            }
            else
            {
                if(btnPan.IsCLick)
                imgView.PanMode = ImageBoxPanMode.Left;
                imgView.AllowClickZoom = true;
            }    

                //if (Global.StatusDraw != StatusDraw.None)
                imgView.Invalidate();
        }
        Control controlEdit;
        private void Global_IndexToolChanged(int obj)
        {if (!Global.IsEditTool) return;
            if (Global.IndexToolSelected == -1) return;
            Global.IsEditTool = false;
            if (Global.StatusDraw == StatusDraw.Edit)
            {
                Global.StepEdit.Enabled = false;
                  btnChangeImg.Visible = true;
                Global.OldPropetyTool = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.Clone();
                String name = "Tools" + BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Name;
                //   if (Score.Enabled||Global.IsRun) return;
                Global.TypeCrop = TypeCrop.Area;
                Global.EditTool.pEditTool.Controls.Clear();
                controlEdit = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Control;
                controlEdit.Enabled = false;
                tmEnableControl.Enabled = true;
                  EditTool editTool = Global.EditTool as EditTool;
                if (!Global.EditTool.pEditTool.Show(name))
                {
                    editTool.pEditTool.Register(name, () => controlEdit);
                    editTool.pEditTool.Show(name);
                }    
                  
                //control.Size =Global.EditTool. pEditTool.Size;
                //control.Location = new Point(0, 0);

                // control.BringToFront();
                // DataTool.LoadPropety(control);
              
                TypeTool TypeTool = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].TypeTool;
                Global.EditTool.iconTool.Visible = true;
                Global.EditTool.layInforTool.Visible = true;
                Global.EditTool.iconTool.BackgroundImage = Global.itemNews[Global.itemNews.FindIndex(a => a.TypeTool == TypeTool)].Icon;// (Image)Properties.Resources.ResourceManager.GetObject(TypeTool.ToString());
                Global.EditTool.lbTool.Text = TypeTool.ToString();
                Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Control.Propety = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety;
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Control.LoadPara();
                Global.EditTool.View.imgView.Invalidate();
                Global.EditTool.View.imgView.Update();
              
                Global.EditTool.View.toolEdit = controlEdit;
                if (!Global.IsLive)
                    Global.Config.SizeCCD = new Size(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Width, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Height);

                ShowTool.Full(imgView,Global.Config.SizeCCD);
            }

        }

        private void KeyboardListener_s_KeyEventHandler1(object sender, EventArgs e)
        {
            KeyboardListener.UniversalKeyEventArgs eventArgs = (KeyboardListener.UniversalKeyEventArgs)e;
                if ( IsKeyDown(Keys.Control))
                {
                    if (eventArgs.KeyCode == Keys.S)
                    {
                    SaveData.Project(Global.Project);
                    
                    }
                }
            
        }

        private void Common_FrameChanged(object sender, PropertyChangedEventArgs e)
        {
            Global.EditTool.lbFrameRate.Text = Global.Config.SizeCCD.ToString()+"-"+ BeeCore.Common.listCamera[Global.IndexChoose].FrameRate+" img/s ";
        }

        public dynamic toolEdit;
      
        public void CurrentTool()
        {
            
          
        }
        public void ToolMouseUp()
        {
            if (toolEdit == null) return;
       
          if(Global.IndexToolSelected>=0)
            switch (BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].TypeTool)
            {
             
                case TypeTool.Pattern:
                   
                case TypeTool.Position_Adjustment:
                    //if (BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotCrop != null)
                    //    if (BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotCrop._rect.Width != 0 && BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotCrop._rect.Height != 0)
                    //    {
                    //        BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.LearnPattern(toolEdit.indexTool, toolEdit.matTemp);

                    //    }
                    break;
                case TypeTool.Color_Area:
                  
                    toolEdit.GetTemp();
                        //Global.StatusDraw = StatusDraw.Check;
                    break;
            }
          
        }
        private void cbView_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }
       public  String pathRaw;
       public bool IsProcess;

     

        public String[] listPath;
        public int indexTool = 0; int indexImg = 0;
 

     
    
      
        int indexToolPosition = -1;
        bool IsAutoTrig;
    //    OutLine ParaPosition;
      
        bool IsCompleteAll = false;
        private void workPlay_DoWork(object sender, DoWorkEventArgs e)
        {

    
        }
   //     [DllImport("KERNEL32.DLL", EntryPoint =
   //"SetProcessWorkingSetSize", SetLastError = true,
   //CallingConvention = CallingConvention.StdCall)]
   //     internal static extern bool SetProcessWorkingSetSize32Bit
   //(IntPtr pProcess, int dwMinimumWorkingSetSize,
   //int dwMaximumWorkingSetSize);

   //     [DllImport("KERNEL32.DLL", EntryPoint =
   //        "SetProcessWorkingSetSize", SetLastError = true,
   //        CallingConvention = CallingConvention.StdCall)]
   //     internal static extern bool SetProcessWorkingSetSize64Bit
   //        (IntPtr pProcess, long dwMinimumWorkingSetSize,
   //        long dwMaximumWorkingSetSize);
        bool IsProcessing=false;
        Stopwatch stopWatch = new Stopwatch();

        int DelayTrig;
        Graphics graphicsOld;
        public void SQL_Insert(DateTime time, String model, int Qty, int Total, String Status)
        {

            try
            {  
                
                if (G.cnn.State == ConnectionState.Closed)
                    G.cnn.Open();
                String pathRaw, pathRS;
                String date = DateTime.Now.ToString("yyyyMMdd");
                String Hour = DateTime.Now.ToString("HHmmss");
                pathRaw = "Report//" + date + "//Raw";
                pathRS = "Report//" + date + "//Result";
               
            
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = G.cnn;
                  
                 
                    command.CommandText = "INSERT into Report (Date,Model,Qty,Total,Status,Raw,Result) VALUES(@Date,@Model,@Qty,@Total,@Status,@Raw,@Result)";
                    command.Prepare();
                    command.Parameters.Add("@Date", SqlDbType.DateTime).Value = DateTime.Now;             // 1
                    command.Parameters.AddWithValue("@Model", model);                                             // 2                                                                                                      //  command.Parameters.Add("@Time", MySqlDbType.DateTime).Value = DateTime.Now;             // 3
                    command.Parameters.Add("@Qty", SqlDbType.Int).Value = Qty;                                             // 4
                    command.Parameters.Add("@Total", SqlDbType.Int).Value = Total;                                        // 5
                    command.Parameters.AddWithValue("@Status", Status);                                           // 6
                    command.Parameters.AddWithValue("@Raw", pathRaw + "//" + Global.Project + "_" + Hour + ".png");// imageToByteArray(raw);         // 7
                    command.Parameters.AddWithValue("@Result", pathRS + "//" + Global.Project + "_" + Hour + ".png"); ;   // 8
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    
                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "InsertData", ex.Message));
                    // MessageBox.Show(ex.Message);
                }
              
            }
            catch (Exception ex)
            {
                
                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "InsertData", ex.Message));
                // MessageBox.Show(ex.Message);
            }
       
           

        }
        //public static Bitmap GetBmResultSnapshot()
        //{
        //    lock (BeeCore.Common.BmLock)
        //    {
        //        var src = BeeCore.Common.bmResult;
        //        if (src == null) return null;
        //        // Clone theo pixel format hiện tại
        //        return src.Clone(new Rectangle(0, 0, src.Width, src.Height), src.PixelFormat);
        //    }
        //}
        int indexNG = 0;
        int numToolOK = 0;
        public static Bitmap ConvertToNonIndexed(Bitmap original)
        {
            Bitmap newBmp = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(newBmp))
            {
                g.DrawImage(original, 0, 0);
            }
            return newBmp;
        }

        //public  void SetBmResultAndShow(Bitmap newFrame)
        //{
        //    if (newFrame == null) return;
        //    if (newFrame.Width == 0 || newFrame.Height == 0) { newFrame.Dispose(); return; }

        //    // Nếu ai đó truyền nhầm indexed Bitmap, convert sang 24bppRgb
        //    if ((newFrame.PixelFormat & PixelFormat.Indexed) != 0)
        //    {
        //        var nonIdx = new Bitmap(newFrame.Width, newFrame.Height, PixelFormat.Format24bppRgb);
        //        using (var g = Graphics.FromImage(nonIdx))
        //            g.DrawImageUnscaled(newFrame, 0, 0);
        //        newFrame.Dispose();
        //        newFrame = nonIdx;
        //    }

        //    if (imgView.IsHandleCreated && !imgView.IsDisposed)
        //    {
        //        var frameToSet = newFrame;   // chuyển quyền sở hữu sang UI
        //        newFrame = null;

        //        imgView.BeginInvoke(new Action(() =>
        //        {
        //            Bitmap oldImg = null;

        //            lock (BeeCore.Common.BmLock)
        //            {
        //                oldImg = imgView.Image as Bitmap;
                       
        //                BeeCore.Common.bmResult.Add(frameToSet);
        //                //BeeCore.Common.bmResult = frameToSet;     // GIỮ LẠI cho nơi khác dùng
        //                imgView.Image = frameToSet;
        //            }

        //            // Dọn ảnh cũ (trên UI thread)
        //          //  if (!ReferenceEquals(oldImg, BeeCore.Common.bmResult))
        //                oldImg?.Dispose();

                   
        //        }));
        //    }
        //    else
        //    {
        //        // UI không nhận → dọn
        //        newFrame?.Dispose();
        //    }
        //}

        // Locks
        private readonly object _bmLock = new object();   // bảo vệ bmResult
        private readonly object _camLock = new object();   // bảo vệ nguồn camera (nếu cần)
        private readonly object _swapLock = new object();   // bảo vệ A/B & _displayMat

        // Double-buffer Mat (KHÔNG readonly để có thể thay thế khi bị Dispose)
        private Mat _bufA = new Mat();
        private Mat _bufB = new Mat();
        private Mat _displayMat; // trỏ tới buffer đang hiển thị (A hoặc B)

        private bool _disposed;

        // --- helper: đảm bảo có buffer làm việc hợp lệ, đúng size/type; nếu bị Dispose -> tạo mới và gán lại field
        private Mat EnsureWorkingBuffer(Mat src)
        {
            // Nếu đang hiển thị A thì vẽ vào B, ngược lại
            bool useB = ReferenceEquals(_displayMat, _bufA);
            Mat target = useB ? _bufB : _bufA;

            if (target == null || target.IsDisposed)
            {
                target = new Mat();                     // tạo mới nếu đã Dispose
                if (useB) _bufB = target; else _bufA = target;
            }

            // target.Create sẽ cấp phát đúng kích thước/kiểu; không cần Release trước
            target.Create(src.Rows, src.Cols, src.Type());
            return target;
        }

        // --- chuẩn hoá về 8UC3 để ToBitmap nhanh; trả alias nếu đã 8UC3, ngược lại tạo bản tạm (caller sẽ Dispose nếu createdTemp = true)
        private static Mat EnsureBgr8Uc3AliasOrConvert(Mat working, out bool createdTemp)
        {
            createdTemp = false;
            if (working.Type() == MatType.CV_8UC3) return working;

            var dst = new Mat();
            createdTemp = true;

            if (working.Channels() == 1)
            {
                if (working.Depth() == MatType.CV_8U)
                    Cv2.CvtColor(working, dst, ColorConversionCodes.GRAY2BGR);
                else
                {
                    var tmp8 = new Mat();
                    try
                    {
                        Cv2.Normalize(working, tmp8, 0, 255, NormTypes.MinMax);
                        tmp8.ConvertTo(tmp8, MatType.CV_8U);
                        Cv2.CvtColor(tmp8, dst, ColorConversionCodes.GRAY2BGR);
                    }
                    finally { tmp8.Dispose(); }
                }
            }
            else if (working.Channels() == 4 && working.Depth() == MatType.CV_8U)
            {
                Cv2.CvtColor(working, dst, ColorConversionCodes.BGRA2BGR);
            }
            else
            {
                var tmp8 = new Mat();
                try
                {
                    if (working.Channels() == 3)
                    {
                        working.ConvertTo(tmp8, MatType.CV_8UC3);
                        tmp8.CopyTo(dst);
                    }
                    else
                    {
                        Cv2.Normalize(working, tmp8, 0, 255, NormTypes.MinMax);
                        tmp8.ConvertTo(tmp8, MatType.CV_8U);
                        Cv2.CvtColor(tmp8, dst, ColorConversionCodes.GRAY2BGR);
                    }
                }
                finally { tmp8.Dispose(); }
            }
            return dst;
        }
        //public void RenderAndDisplay()
        //{
        //    if (_disposed) return;

        //    // 1) Lấy frame nguồn
        //    Mat src;
        //    lock (_camLock)
        //    {
        //        src = BeeCore.Common.listCamera[Global.IndexChoose].matRaw?.Clone();
        //    }
        //    if (src == null || src.Empty() || src.Width <= 0 || src.Height <= 0)
        //    {
        //        src?.Dispose();
        //        return;
        //    }

        //    // 2) Chuẩn bị buffer
        //    Mat working;
        //    lock (_swapLock)
        //    {
        //        working = EnsureWorkingBuffer(src);
        //        src.CopyTo(working);
        //    }
        //    src.Dispose();

        //    // 3) Convert -> Bitmap & vẽ overlay
        //    using (Mat bgr = EnsureBgr8Uc3AliasOrConvert(working, out bool createdTemp))
        //    {
        //        Bitmap canvas = null;
        //        try
        //        {
        //            canvas = BitmapConverter.ToBitmap(bgr);

        //            using (var g = Graphics.FromImage(canvas))
        //            using (var xf = new Matrix())
        //            {
        //                g.SmoothingMode = SmoothingMode.None;
        //                g.InterpolationMode = InterpolationMode.NearestNeighbor;
        //                g.CompositingQuality = CompositingQuality.HighSpeed;
        //                g.PixelOffsetMode = PixelOffsetMode.Half;

        //                xf.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
        //                float s = 1.0f;
        //                try
        //                {
        //                    var pi = imgView.GetType().GetProperty("Zoom");
        //                    if (pi != null) s = Convert.ToSingle(pi.GetValue(imgView)) / 100f;
        //                }
        //                catch { }
        //                xf.Scale(s, s);
        //                g.Transform = xf;

        //                var tools = BeeCore.Common.PropetyTools[Global.IndexChoose];
        //                foreach (var tool in tools)
        //                    if (tool.UsedTool != UsedTool.NotUsed)
        //                        tool.Propety.DrawResult(g);

        //                //String Content = "OK Date:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        //                //     if (!Global.TotalOK)
        //                //    Content = "NG Date:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        //                //g.DrawString(Content, new Font("Arial", 12, FontStyle.Regular),new SolidBrush(Color.WhiteSmoke),new Point(10,10));
        //            }

        //            // 4) Tạo bmResult bằng copy pixel data trực tiếp từ canvas
        //            Bitmap storeCopy = new Bitmap(canvas.Width, canvas.Height, canvas.PixelFormat);
        //            using (var gCopy = Graphics.FromImage(storeCopy))
        //            {
        //                gCopy.DrawImageUnscaled(canvas, 0, 0);
        //            }

        //            lock (_bmLock)
        //            {
        //                BeeCore.Common.bmResult?.Dispose();
        //                BeeCore.Common.bmResult = storeCopy;
        //            }

        //            // 5) Dùng chính canvas cho UI (không clone lại)
        //            if (imgView.IsHandleCreated && !imgView.IsDisposed)
        //            {
        //                if (imgView.InvokeRequired)
        //                {
        //                    var uiBmp = canvas; // giữ canvas cho UI
        //                    canvas = null; // tránh dispose ở finally
        //                    imgView.BeginInvoke(new Action(() =>
        //                    {
        //                        if (imgView.IsDisposed) { uiBmp.Dispose(); return; }
        //                        var oldUi = imgView.Image;
        //                        imgView.Image = uiBmp;
        //                        oldUi?.Dispose();
        //                    }));
        //                }
        //                else
        //                {
        //                    var oldUi = imgView.Image;
        //                    imgView.Image = canvas;
        //                    oldUi?.Dispose();
        //                    canvas = null; // tránh dispose ở finally
        //                }
        //            }

        //            // 6) Xác nhận buffer hiển thị
        //            lock (_swapLock)
        //            {
        //                _displayMat = working;
        //            }
        //        }
        //        finally
        //        {
        //            canvas?.Dispose();
        //        }
        //    }
        //}

        //public void RenderAndDisplay()
        //{
        //    if (_disposed) return;

        //    // 1) Lấy frame nguồn
        //    Mat src;
        //    lock (_camLock)
        //    {
        //        src = BeeCore.Common.listCamera[Global.IndexChoose].matRaw?.Clone();
        //    }
        //    if (src == null || src.Empty() || src.Width <= 0 || src.Height <= 0)
        //    {
        //        src?.Dispose();
        //        return;
        //    }

        //    // 2) Chọn & chuẩn bị buffer làm việc (AN TOÀN với Dispose)
        //    Mat working;
        //    lock (_swapLock)
        //    {
        //        working = EnsureWorkingBuffer(src); // đảm bảo target còn sống & đúng size/type
        //        src.CopyTo(working);               // KHÔNG cần Release trước; Create() đã cấp phát
        //    }
        //    src.Dispose();

        //    // 3) Convert -> Bitmap & vẽ overlay
        //    using (Mat bgr = EnsureBgr8Uc3AliasOrConvert(working, out bool createdTemp))
        //    {
        //        Bitmap canvas = null;
        //        try
        //        {
        //            canvas = BitmapConverter.ToBitmap(bgr);

        //            using (var g = Graphics.FromImage(canvas))
        //            using (var xf = new Matrix())
        //            {
        //                g.SmoothingMode = SmoothingMode.None;
        //                g.InterpolationMode = InterpolationMode.NearestNeighbor;
        //                g.CompositingQuality = CompositingQuality.HighSpeed;
        //                g.PixelOffsetMode = PixelOffsetMode.Half;

        //                xf.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
        //                float s = 1.0f;
        //                try
        //                {
        //                    var pi = imgView.GetType().GetProperty("Zoom");
        //                    if (pi != null) s = Convert.ToSingle(pi.GetValue(imgView)) / 100f;
        //                }
        //                catch { }
        //                xf.Scale(s, s);
        //                g.Transform = xf;

        //                var tools = BeeCore.Common.PropetyTools[Global.IndexChoose];
        //                foreach (var tool in tools)
        //                    if (tool.UsedTool != UsedTool.NotUsed)
        //                        tool.Propety.DrawResult(g);
        //            }

        //            // 4) Swap bmResult cho nơi khác lưu ảnh
        //            lock (_bmLock)
        //            {
        //                BeeCore.Common.bmResult?.Dispose();
        //                BeeCore.Common.bmResult = (Bitmap)canvas.Clone();
        //            }
        //            // 5) Hiển thị ngay trên imgView
        //            if (imgView.InvokeRequired)
        //            {
        //                imgView.BeginInvoke(new Action(() =>
        //                {
        //                    var oldImg = imgView.Image; // hoặc Image nếu bạn muốn
        //                    imgView.Image = BeeCore.Common.bmResult;
        //                    oldImg?.Dispose();
        //                }));
        //            }
        //            else
        //            {
        //                var oldImg = imgView.Image; // hoặc Image nếu bạn muốn
        //                imgView.Image = BeeCore.Common.bmResult;
        //                oldImg?.Dispose();
        //            }
        //            // 5) Xác nhận buffer đang hiển thị
        //         lock (_swapLock)
        //            {
        //                _displayMat = working;
        //            }

        //          //  imgView.Invalidate();
        //        }
        //        finally
        //        {
        //            canvas?.Dispose();
        //            // bgr là using -> nếu bgr là alias của working (8UC3 sẵn) thì Dispose() cũng OK (Mat là header, data share),
        //            // còn nếu là bản tạm, nó sẽ giải phóng đúng bản tạm.
        //        }
        //    }
        //}

        /// <summary>
        /// Giải phóng tài nguyên khi kết thúc vòng đời renderer/app.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            // 1) Dispose buffer Mat (chỉ khi shutdown)
            _bufA.Dispose();
            _bufB.Dispose();
            _displayMat = null;

            // 2) KHÔNG dispose bmResult ở đây nếu app còn dùng nó bên ngoài.
            // Tuỳ nhu cầu, bạn có thể chủ động huỷ:
            // lock(_bmLock) { BeeCore.Common.bmResult?.Dispose(); BeeCore.Common.bmResult = null; }
        }


        //public void RenderAndDisplay2()
        //{

        //    Mat src = BeeCore.Common.listCamera[Global.IndexChoose].matRaw;
        //    using (Mat rs = src?.Clone())
        //    {
        //        if (rs == null || rs.Empty() || rs.Width == 0 || rs.Height == 0) return;

     
        //        using (Mat rs24 = EnsureBgr24(rs))
        //        {

        //            Bitmap canvas = null;
        //            try
        //            {
        //                canvas = BitmapConverter.ToBitmap(rs24); // -> 24bppRgb

        //                // 4) Vẽ overlay
        //                using (var g = Graphics.FromImage(canvas))
        //                {
        //                    // tốc độ cao; nếu cần mịn hơn thì chỉnh lại
        //                    g.SmoothingMode = SmoothingMode.None;
        //                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
        //                    g.CompositingQuality = CompositingQuality.HighSpeed;
        //                    g.PixelOffsetMode = PixelOffsetMode.Half;

        //                    using (var xf = new Matrix())
        //                    {  // các thuộc tính này phụ thuộc custom control của bạn
        //                        xf.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
        //                        float s = 1.0f;




        //                        try
        //                        {
        //                            // nếu imgView có property Zoom (custom), dùng nó
        //                            var pi = imgView.GetType().GetProperty("Zoom");
        //                            if (pi != null)
        //                            {
        //                                var zoomVal = Convert.ToSingle(pi.GetValue(imgView));
        //                                s = zoomVal / 100.0f;
        //                            }
        //                        }
        //                        catch { /* bỏ qua nếu không có Zoom */ }
        //                        xf.Scale(s, s);
        //                        g.Transform = xf;

        //                        // Vẽ kết quả từ các tool
        //                        var tools = BeeCore.Common.PropetyTools[Global.IndexChoose];
        //                        foreach (var tool in tools)
        //                        {
        //                            if (tool.UsedTool != UsedTool.NotUsed)
        //                                tool.Propety.DrawResult(g);
        //                        }
        //                    }
        //                }

        //                // 5) Đẩy lên UI & giữ lại bmResult
        //                SetBmResultAndShow(canvas);
        //                canvas = null; // đã chuyển quyền sở hữu sang UI/bmResult
        //            }
        //            finally
        //            {
        //                // nếu có lỗi và chưa chuyển giao cho UI
        //                canvas?.Dispose();
        //            }
        //        }
        //    }
        //}

    //    // Gọi trên worker thread (không phải UI)
    //    void RenderAndDisplay()
    //    {
    //        var src = BeeCore.Common.listCamera[Global.IndexChoose].matRaw;
    //        using (Mat rs = src.Clone())
    //        {
    //            if (rs.Empty() || rs.Width == 0 || rs.Height == 0) return;

    //            using (Mat rs24 = EnsureBgr24(rs))
    //            {     
    //                var canvas = BitmapConverter.ToBitmap(rs24);  // -> 24bppRgb

    //            // Vẽ overlay
    //            using (var g = Graphics.FromImage(canvas))
    //            {
    //                g.SmoothingMode = SmoothingMode.None;
    //                g.InterpolationMode = InterpolationMode.NearestNeighbor;
    //                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;

    //                    using (var xf = new Matrix())
    //                    {
    //                        xf.Translate(imgView.AutoScrollPosition.X,
    //                                   imgView.AutoScrollPosition.Y);
    //                        float s = (float)(imgView.Zoom / 100.0);
    //                        xf.Scale(s, s);
    //                        g.Transform = xf;
    //                    }    
                          

    //                foreach (var tool in BeeCore.Common.PropetyTools[Global.IndexChoose])
    //                    if (tool.UsedTool != UsedTool.NotUsed)
    //                        tool.Propety.DrawResult(g);
    //            }

    //            // Đẩy lên UI và giữ lại bmResult
    //            bool posted = false;
    //            try
    //            {
    //                if (imgView.IsHandleCreated && !imgView.IsDisposed)
    //                {

    //                    imgView.BeginInvoke(new Action(() =>
    //                    {
    //                        Bitmap oldImg = null;
    //                        Bitmap oldBm = null;

    //                        // Swap bmResult & imgView.Image atomically dưới 1 lock ngắn
    //                        lock (BeeCore.Common.BmLock)
    //                        {
    //                            oldImg = imgView.Image as Bitmap;
    //                            oldBm = BeeCore.Common.bmResult;
    //                            BeeCore.Common.bmResult = canvas;  // GIỮ LẠI cho chỗ khác dùng
    //                            imgView.Image = BeeCore.Common.bmResult;
    //                            canvas = null; // chuyển quyền sở hữu sang bmResult/UI
    //                        }

    //                        // Giải phóng ảnh cũ (nếu khác ref)
    //                        if (!ReferenceEquals(oldImg, BeeCore.Common.bmResult))
    //                            oldImg?.Dispose();

    //                        if (oldBm != null &&
    //                            !ReferenceEquals(oldBm, oldImg) &&
    //                            !ReferenceEquals(oldBm, BeeCore.Common.bmResult))
    //                            oldBm.Dispose();
    //                    }));
    //                    posted = true; // UI sẽ sở hữu/giải phóng
    //                }
    //            }
    //            finally
    //            {
    //                // Nếu chưa post được lên UI, tự dọn
    //                if (!posted)
    //                    canvas?.Dispose();
    //            }
    //        } }
    //}
    // Ép Mat về 8UC3 BGR để ToBitmap ra 24bppRgb
 
      Mat ConvertToNew( Mat src, MatType type)
    {
        var m = new Mat(); src.ConvertTo(m, type); return m;
    }


        int numSetImg;
        private void DrawImageFit(Graphics g, Bitmap bmp, Rectangle targetRect)
        {
            float ratioX = (float)targetRect.Width / bmp.Width;
            float ratioY = (float)targetRect.Height / bmp.Height;
            float ratio = Math.Min(ratioX, ratioY); // scale nhỏ hơn để vừa khung

            int newWidth = (int)(bmp.Width * ratio);
            int newHeight = (int)(bmp.Height * ratio);

            // canh giữa
            int posX = targetRect.X + (targetRect.Width - newWidth) / 2;
            int posY = targetRect.Y + (targetRect.Height - newHeight) / 2;

            g.DrawImage(bmp, new Rectangle(posX, posY, newWidth, newHeight));
        }
        private CollageRenderer _renderer;
     
        public  void ShowResultTotal()
        {
            try
            {




               

           


            numToolOK = 0;
			
            Global.ScaleZoom = (float)(imgView.Zoom / 100.0);
            Global.pScroll = new Point(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
               
                if(Global.Config.IsMultiCamera)
                {
                    _renderer.ClearImages();
                    int index = 0;
                    foreach (Camera camera in BeeCore.Common.listCamera)
                    {
                        if (camera == null) continue;
                        camera.DrawResult();
                        if (index == 1)
                            _renderer.AddImage(camera.bmResult, FillMode1.Contain, 0.3f);
                        else
                            _renderer.AddImage(camera.bmResult, FillMode1.Contain, 1);
                        index++;
                    }

                }
                else
                {
                    switch (Global.Config.NumTrig)
                    {
                        case 1:
                            _renderer.LayoutPreset = CollageLayout.One;

                            break;
                        case 2:
                            _renderer.LayoutPreset = CollageLayout.Two;
                            break;
                        case 3:
                            _renderer.LayoutPreset = CollageLayout.ThreeRow;
                            break;
                        case 4:
                            _renderer.LayoutPreset = CollageLayout.FourGrid;
                            break;
                    }
                   

                   
                        if (BeeCore.Common.listCamera[0] == null) return;
                        BeeCore.Common.listCamera[0].DrawResult();
                    Results results = Global.TotalOK==true ? Results.OK : Results.NG;
                    switch (Global.TriggerNum)
                    {
                        
                        case TriggerNum.Trigger1:
                            Global.ToolSettings.Labels[0].Results = results;
                            Global.ToolSettings.Labels[0].BackColor = Color.FromArgb(246, 204, 120);
                            Global.ToolSettings.Labels[1].BackColor = Color.LightGray;
                            Global.ToolSettings.Labels[2].BackColor = Color.LightGray;
                            Global.ToolSettings.Labels[3].BackColor = Color.LightGray;
                            if (_renderer.Count() < 1)
                                _renderer.AddImage(BeeCore.Common.listCamera[0].bmResult, FillMode1.Contain);
                            else
                                _renderer.ModifyImage(0, BeeCore.Common.listCamera[0].bmResult, FillMode1.Contain);
                                break;
                        case TriggerNum.Trigger2:
                            Global.ToolSettings.Labels[1].Results = results;
                            Global.ToolSettings.Labels[1].BackColor = Color.FromArgb(246, 204, 120);
                            Global.ToolSettings.Labels[0].BackColor = Color.LightGray;
                            Global.ToolSettings.Labels[2].BackColor = Color.LightGray;
                            Global.ToolSettings.Labels[3].BackColor = Color.LightGray;
                            if (_renderer.Count() < 2)
                                _renderer.AddImage(BeeCore.Common.listCamera[0].bmResult, FillMode1.Contain);
                            else
                                _renderer.ModifyImage(1, BeeCore.Common.listCamera[0].bmResult, FillMode1.Contain);
                            break;
                        case TriggerNum.Trigger3:
                            Global.ToolSettings.Labels[2].Results = results;
                            Global.ToolSettings.Labels[2].BackColor = Color.FromArgb(246, 204, 120);
                            Global.ToolSettings.Labels[1].BackColor = Color.LightGray;
                            Global.ToolSettings.Labels[0].BackColor = Color.LightGray;
                            Global.ToolSettings.Labels[3].BackColor = Color.LightGray;
                            if (_renderer.Count() < 3)
                                _renderer.AddImage(BeeCore.Common.listCamera[0].bmResult, FillMode1.Contain);
                            else
                                _renderer.ModifyImage(2, BeeCore.Common.listCamera[0].bmResult, FillMode1.Contain);
                            break;
                        case TriggerNum.Trigger4:
                            Global.ToolSettings.Labels[3].Results = results;
                            Global.ToolSettings.Labels[3].BackColor = Color.FromArgb(246, 204, 120);
                            Global.ToolSettings.Labels[1].BackColor = Color.LightGray;
                            Global.ToolSettings.Labels[2].BackColor = Color.LightGray;
                            Global.ToolSettings.Labels[0].BackColor = Color.LightGray;
                            if (_renderer.Count() < 4)
                                _renderer.AddImage(BeeCore.Common.listCamera[0].bmResult, FillMode1.Contain);
                            else
                                _renderer.ModifyImage(3, BeeCore.Common.listCamera[0].bmResult, FillMode1.Contain);
                            break;
                    }    
                      
                }

              
                   
                        Global.Config.SizeCCD = _renderer.szImage;
                        ShowTool.Full(imgView, Global.Config.SizeCCD);
                

                //_renderer.Render();
                // RenderAndDisplay();
                if (Global.TotalOK)
                    {
                        if (Global.Config.IsSaveOK)
                        {
                            if (!workInsert.IsBusy)
                        {
                            workInsert.RunWorkerAsync();
                           
                        }    
                               
                            else
                            
                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now,LeveLLog.ERROR, "Data", "Fail Save Data OK"));
                    }
                    
                      
                        }
                    else
                    {
                        if (Global.Config.IsSaveNG)
                        {
                            if (!workInsert.IsBusy)
                                workInsert.RunWorkerAsync();
                            else
                             
                            Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Data", "Fail Save Data NG"));
                    }
                   
                    }    
                    
            
            }
            catch (Exception ex)
            {
                
                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Drawing", ex.Message));
            }
            // btnCap.Enabled = true;
        }
        public float SumCycle = 0;
        public void CheckStatusMode()
        {
          
            switch (Global.StatusMode)
            {
                case StatusMode.SimContinuous:
                 
                    tmSimulation.Enabled = true;
                    break;
                case StatusMode.SimOne:
                    indexFile++;
                   
                    Global.StatusMode = StatusMode.None;
                    break;
                case StatusMode.Once:
                    tmPress.Enabled = true;
                    btnCap.IsCLick = false;
                    Global.StatusMode = StatusMode.None;
                    break;
                case StatusMode.Continuous:
                    tmContinuous.Enabled = true;
                  
                   // btnCap.IsCLick = false;
                    break;
                case StatusMode.None:

               
                    break;
            }
           

        }
     
        Graphics gcResult;
        public  void Continuous()
            {
             Global.StatusMode = StatusMode.Continuous;
            if (Global.ParaCommon.Comunication.Protocol.IsBypass)
            {
                switch (Global.TriggerNum)
                {
                    case TriggerNum.Trigger0:
                        Global.TriggerNum = TriggerNum.Trigger1;
                        break;
                    case TriggerNum.Trigger1:
                        Global.TriggerNum = TriggerNum.Trigger2;
                        break;
                    case TriggerNum.Trigger2:
                        Global.TriggerNum = TriggerNum.Trigger3;
                        break;
                    case TriggerNum.Trigger3:
                        Global.TriggerNum = TriggerNum.Trigger4;
                        break;
                }
                timer = CycleTimerSplit.Start();
                Global.StatusProcessing = StatusProcessing.Read;
            }

            else
            {
                timer = CycleTimerSplit.Start();
                Global.TriggerInternal = true;
            }
          
        }
        public float Cyclyle1 = 0;
       
        public bool  IsBTNCap=false;
        private  void btnCap_Click(object sender, EventArgs e)
        {
            

            if (!Global.ParaCommon.Comunication.Protocol.IsConnected&&!Global.ParaCommon.Comunication.Protocol.IsBypass )
            {
                btnCap.IsCLick = false;
                return;
            }
         
            if (btnContinuous.IsCLick)
            {
               
                btnCap.IsCLick = false;
               // MessageBox.Show("Please stop Mode Continue");
                return;
            }
            if (btnLive.IsCLick)
            {
              
                btnCap.IsCLick = false;
               // MessageBox.Show("Please stop Mode Live");
                return;
            }
            if (btnLive.IsCLick)
            {
                btnCap.IsCLick = false;
                return;
            }
            Global.StatusMode = StatusMode.Once;
            timer= CycleTimerSplit.Start();
            btnCap.Enabled = false;
            if (Global.ParaCommon.Comunication.Protocol.IsBypass)
            {
                switch(Global.TriggerNum)
                {
                    case TriggerNum.Trigger0:
                        Global.TriggerNum=TriggerNum.Trigger1;
                        break;
                    case TriggerNum.Trigger1:
                        Global.TriggerNum = TriggerNum.Trigger2;
                        break;
                    case TriggerNum.Trigger2:
                        Global.TriggerNum = TriggerNum.Trigger3;
                        break;
                    case TriggerNum.Trigger3:
                        Global.TriggerNum = TriggerNum.Trigger4;
                        break;
                }    
                Global.StatusProcessing = StatusProcessing.Read;
            }    
               
            else
            {
             
                Global.TriggerInternal = true;
            }    
               
            //  BeeCore.Common.currentTrig++;

        }

        private async void btnRecord_Click(object sender, EventArgs e)
        {
          
            if (!Global.ParaCommon.Comunication.Protocol.IsConnected && !Global.ParaCommon.Comunication.Protocol.IsBypass)
            {
                btnContinuous.IsCLick = false;
                return;
            }
            if (btnLive.IsCLick)
            {
                btnContinuous.IsCLick = false;
                return;
            }
            btnCap.Enabled = !btnContinuous.IsCLick;
            Global.StatusMode = btnContinuous.IsCLick ? StatusMode.Continuous : StatusMode.None;

            if (Global.ParaCommon.Comunication.Protocol.IsConnected)
            {
                if (Global.ParaCommon.Comunication.Protocol.IsConnected)
                {
                    tmContinuous.Enabled = btnContinuous.IsCLick;
                   
                    return;
                }
            } 
            else if(Global.ParaCommon.Comunication.Protocol.IsBypass)
            {
                tmContinuous.Enabled = btnContinuous.IsCLick;
            }    
            else
            {
                btnContinuous.IsCLick = false;
                tmContinuous.Enabled = false;
                return;
            }
            if (!btnContinuous.IsCLick) btnCap.Enabled = true;
          
           

        }

        private void tmPlay_Tick(object sender, EventArgs e)
        {
           
            tmPlay.Enabled = false;
        }
        private Thread _displayThread;
        private readonly AutoResetEvent _frameReady = new AutoResetEvent(false);
        private Bitmap _sharedFrame;
        private int _uiPending; // 0: idle, 1: đang đẩy frame lên UI
        void PublishFrame(Bitmap src)
        {
            if (!Global.IsLive) { src.Dispose(); return; }
            // Clone 1 lần ở producer, không clone trong display thread
            var clone = (Bitmap)src.Clone();
            var old = Interlocked.Exchange(ref _sharedFrame, clone); // giữ frame mới nhất, drop cũ
            old?.Dispose();
            _frameReady.Set();
        }

        void StartLive()
        {
           
            _displayThread = new Thread(DisplayLoop) { IsBackground = true, Name = "DisplayLoop" };
            _displayThread.Start();
        }

        void StopLive()
        {
           
            _frameReady.Set();
            _displayThread?.Join();
            _displayThread = null;

            // Clear ảnh trên UI
            if (IsHandleCreated && !IsDisposed)
                BeginInvoke(new Action(() =>
                {
                    var old = imgView.Image;
                    imgView.Image = null;
                    old?.Dispose();
                }));

            // Dọn rác còn sót
            var leftover = Interlocked.Exchange(ref _sharedFrame, null);
            leftover?.Dispose();
            if (BeeCore.Common.listCamera[Global.IndexChoose]!= null)
                if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw!=null)
                if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw .IsDisposed)
                    if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty())
                    {
                        BeeCore.Common.listCamera[Global.IndexChoose].Read();
                        imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                        Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexChoose].GetSzCCD();
                        ShowTool.Full(imgView, Global.Config.SizeCCD);
                    }    
                        
        }

        void DisplayLoop()
        {
            while (Global.IsLive)
            {
                _frameReady.WaitOne(50);        // chờ tín hiệu có frame (hoặc timeout để thoát nhanh)
                if (!Global.IsLive) break;

                // Lấy quyền sở hữu frame mới nhất và làm rỗng buffer chung
                var frame = Interlocked.Exchange(ref _sharedFrame, null);
                if (frame == null) continue;

                // Chỉ cho phép 1 cập nhật UI pending; nếu UI chưa kịp xử lý → drop frame
                if (Interlocked.Exchange(ref _uiPending, 1) == 1)
                {
                    frame.Dispose();
                    continue;
                }

                try
                {
                    if (IsHandleCreated && !IsDisposed)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                var old = imgView.Image;
                                imgView.Image = frame;   // chuyển quyền sở hữu cho PictureBox
                                old?.Dispose();          // hủy ảnh cũ sau khi gán
                            }
                            finally
                            {
                                Interlocked.Exchange(ref _uiPending, 0);
                            }
                        }));
                    }
                    else
                    {
                        frame.Dispose();
                        Interlocked.Exchange(ref _uiPending, 0);
                    }
                }
                catch
                {
                    frame.Dispose();
                    Interlocked.Exchange(ref _uiPending, 0);
                }
            }
        }
        public void Live()
        {
            if (Global.IsLive)
            {
                //if (BeeCore.Common.listCamera[Global.IndexChoose] != null)
                //    if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw != null)
                //        if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.IsDisposed)
                //            if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty())
                //            {
                //                BeeCore.Common.listCamera[Global.IndexChoose].Read();
                //                imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                //                Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexChoose].GetSzCCD();
                //                ShowTool.Full(imgView, Global.Config.SizeCCD);
                //            }
                if (!workReadCCD.IsBusy)
                    workReadCCD.RunWorkerAsync();
                StartLive();
            }

            else
            {
                
                StopLive();
             

            }
        }
        private async void btnSer_Click(object sender, EventArgs e)
        {
           
            //btnFull.Enabled = !btnLive.IsCLick;
            //btnZoomIn.Enabled = !btnLive.IsCLick;
            //btnZoomOut.Enabled = !btnLive.IsCLick;

            Global.IsLive = btnLive.IsCLick;
         
          
                numLive = 0;
                tmOut.Enabled = false;
           // pMenu.Visible = btnLive.IsCLick;
          
            await Task.Delay(300);
          //  tmLive.Enabled = btnLive.IsCLick;
            if (btnLive.IsCLick)
            {
                if (G.SettingPLC != null)
                    G.SettingPLC.Enabled = false;
              
                btnCap.Enabled = false;
                btnContinuous.Enabled = false;
            }
            else
            {
            
                if (G.SettingPLC != null)
                    G.SettingPLC.Enabled = true;
            
            }
            gc = imgView.CreateGraphics();

            Live();




        }
private void PylonCam_FrameReady(IntPtr buffer, int width, int height, int stride, int channels)
        {
            if (buffer == IntPtr.Zero) return ; // timeout hoặc fail
            int matType = (channels == 1) ? OpenCvSharp.MatType.CV_8UC1 : OpenCvSharp.MatType.CV_8UC3;

            using (var m = new Mat(height, width, matType, buffer, stride))// new OpenCvSharp.Mat(h, w, type, p, s))
            {
               
                BeeCore.Common.listCamera[Global.IndexChoose].GetFpsPylon();
                try
                {
                    var bmp = BitmapConverter.ToBitmap(m);

                    // Đẩy frame mới nhất và hủy frame cũ một cách an toàn, không cần lock
                    var old = Interlocked.Exchange(ref _sharedFrame, bmp);
                    old?.Dispose();

                    // (tuỳ chọn) báo cho display thread là có frame mới
                    _frameReady?.Set();
                }
                catch(Exception ex)
                {

                }
            }
          
        }

        public void Common_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
            //imgView.Image = BeeCore.Common.matLive.ToBitmap();
           // GC.Collect();
           // GC.WaitForPendingFinalizers();

        }
      
    
        int numLive = 500;
      public CycleTimerSplit timer ;

       
        private void workReadCCD_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!Global.IsLive && Global.IsRun && Global.StatusProcessing != StatusProcessing.Read) return;
                if (Global.IsLive||!Global.IsRun)
            {
                if (BeeCore.Common.listCamera[Global.IndexChoose].IsMouseDown) return;
                try
                {
                    BeeCore.Common.listCamera[Global.IndexChoose].Read();
                }
                  
                catch(Exception)
                {

                }
            }
            else if(Global.IsRun&&Global.StatusProcessing==StatusProcessing.Read)
            {
                if (Global.Config.IsMultiCamera)
                {
                    foreach(Camera camera in BeeCore.Common.listCamera)
                    {
                        if (camera == null) continue;
                        camera.Read();

                        if (camera.Para.TypeCamera == TypeCamera.USB)
                            camera.Read();
                    }    
                    //Parallel.ForEach(BeeCore.Common.listCamera, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, camera =>
                    //{
                    //    if (camera != null)
                    //    {
                          
                          
                    //    }

                    //});
                }
                else
                {
                    BeeCore.Common.listCamera[0].Read();
                    if (BeeCore.Common.listCamera[0].Para.TypeCamera == TypeCamera.USB)
                       BeeCore.Common.listCamera[0].Read();
                }
             }    
               



        }
        private async void  workReadCCD_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (IsErrCCD)
            //{
            //    await TimingUtils.DelayAccurateAsync(5);

            //    if(!workReadCCD.IsBusy)
            //    {
            //        workReadCCD.RunWorkerAsync();
            //        return;

            //    }    

            //}    

            if (Global.IsLive )
            {

                if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw != null)
                    if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.IsDisposed)
                      if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty())
                        {
                        Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexChoose].GetSzCCD();
                        // matRaw là OpenCvSharp.Mat
                        var bmp = BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexChoose].matRaw);

                        // Đẩy frame mới nhất và hủy frame cũ một cách an toàn, không cần lock
                        var old = Interlocked.Exchange(ref _sharedFrame, bmp);
                        old?.Dispose();

                        // (tuỳ chọn) báo cho display thread là có frame mới
                        _frameReady?.Set();
                        //using (Bitmap frame = BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexChoose].matRaw))
                        //{

                        //        _sharedFrame?.Dispose();
                        //        _sharedFrame = (Bitmap)frame.Clone(); // Clone để thread-safe

                        //}
                    }

                if (BeeCore.Common.listCamera[Global.IndexChoose].IsMouseDown)
                    await TimingUtils.DelayAccurateAsync(5);
                if (BeeCore.Common.listCamera[Global.IndexChoose].IsSetPara)
                    await TimingUtils.DelayAccurateAsync(5);
                
                workReadCCD.RunWorkerAsync();
                return;
            }

			if (Global.StatusMode == StatusMode.Continuous || Global.StatusMode == StatusMode.Once)
			{
              
                Global.StatusProcessing = StatusProcessing.Checking;
                if (Global.IsByPassResult)
                    Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.ByPass;

            }
		}
        private void workShow_DoWork(object sender, DoWorkEventArgs e)
        {
        }
        private void tmTrig_Tick(object sender, EventArgs e)
        {
            //if (!btnRecord.IsCLick)
            //{
            //    tmTrig.Enabled = false;
            //    return;
            //}
            //if (G.StatusTrig==Trig.Trigged)
            //{
            //    G.StatusTrig = Trig.Continue;
            //    tmTrig.Enabled = false;
            //  //  tmCycle = DateTime.Now;
            //    G.listAlltool[indexToolPosition].tool.ShowResult(gc);
            //    tmTrig.Enabled = false;
            //    if (!workReadCCD.IsBusy)
            //        workReadCCD.RunWorkerAsync();
            //}
            //else if (G.StatusTrig == Trig.Complete)
            //{

            //    G.StatusTrig = Trig.Processing;
            //    tmTrig.Enabled = false;
            //   // tmCycle = DateTime.Now;
            //    G.listAlltool[indexToolPosition].tool.ShowResult(gc);
            //    tmTrig.Enabled = false;
            //    if (!workPlay.IsBusy)
            //        workPlay.RunWorkerAsync();
            //}
          
            //else
            //{

            //    foreach (Tools tool in G.listAlltool)
            //    {
                  
                  
            //          toolEdit.ShowResult(gc,(float)(imgView.Zoom/100.0),new Point(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y));

            //    }
            //   // G.listAlltool[indexToolPosition].tool.ShowResult(gc);
            //    //  BeeCore.Common.listCamera[Global.IndexChoose].Read(Global.Config.IsHist );
            //    tmTrig.Enabled = false;
            //    if (!workPlay.IsBusy)
            //        workPlay.RunWorkerAsync();
            //}
          
           
         
        }

        private void workShow_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
       
        }

      

        private void workGetColor_DoWork(object sender, DoWorkEventArgs e)
        {
            using (Mat raw = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone())
            {
                int X = Convert.ToInt32((pMove.X - 10) / (Global.ScaleZoom)) + Global.pScroll.X;
                int Y = Convert.ToInt32((pMove.Y + 10) / (Global.ScaleZoom )) + Global.pScroll.Y;
                clChoose = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.GetColor(raw, X, Y);
            }    
           
                      
                 
        }

        private void workGetColor_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            imgView.Invalidate();
        }

        private void btnResetQty_Click(object sender, EventArgs e)
        {
           
        }

    

        private void rjButton1_Click(object sender, EventArgs e)
        {
           
              BeeCore.Common.listCamera[Global.IndexChoose].Read();
           // BeeCore.Common.CalHist();
        }

        private void workInsert_DoWork(object sender, DoWorkEventArgs e)
        {
            if (G.cnn.State == ConnectionState.Closed)
                G.ucReport.Connect_SQL();
            if (Global.TotalOK)
                SQL_Insert(DateTime.Now, Properties.Settings.Default.programCurrent.Replace(".prog", ""), Global.Config.SumOK, Global.Config.SumOK + Global.Config.SumNG, "OK");
            else
                SQL_Insert(DateTime.Now, Properties.Settings.Default.programCurrent.Replace(".prog", ""), Global.Config.SumOK, Global.Config.SumOK + Global.Config.SumNG, "NG");

        }
        int numErrPort = 0;
       

    
       

    

     
 public      List<  String> Files=new List<string>();
        public List<Mat> listMat = new List<Mat>();
        public int indexFile = 0;
        private Native Native = new Native();
        private void btnImg_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog()==DialogResult.OK)
            {
                indexFile = 0;
                Files = new List<string>();
                Files = Directory.GetFiles(folderBrowserDialog1.SelectedPath).ToList(); ;
               
                if (Files.Count > 0)
                {
                    listMat = new List<Mat>();
                    foreach (string file in Files)
                    {
                        listMat.Add(new Mat(file));
                    }
                  
                }
               
            }    
          if(!Global.IsRun)
            {
                indexFile = 0;
                pathFileSeleted = Files[indexFile];
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = BeeCore.Common.listCamera[Global.IndexChoose].matRaw = listMat[indexFile]; ;// Cv2.ImRead(Files[indexFile]);
                Native.SetImg(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
                imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
            }
        }
   
        public float PictureScale = 1.0f;
       
        private void DrawImage(Graphics gr)
        {
            gr.DrawImage(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap(),new PointF(0,0));
           
        }
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
          
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
         
            
            
           
          //  imgView2.Invalidate();
            //PictureScale -= 0.1f;
            //if (PictureScale == 0) PictureScale = 0.1f;
            //imgView.Invalidate();
        }
        
        private async void btnFull_Click(object sender, EventArgs e)
        {if (Global.IsRun)
            {
                if (_renderer.Count() > 0)
                {
                    Global.Config.SizeCCD = _renderer.szImage;
                    ShowTool.Full(imgView, Global.Config.SizeCCD);
                    return;
                }
            }
           
            if(Global.IsLive)
            {
                if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw.IsDisposed) return;
                BeeCore.Common.listCamera[Global.IndexChoose].IsMouseDown = true;
                await TimingUtils.DelayAccurateAsync(10);
                Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexChoose].GetSzCCD();
                if (Global.IsLive)
                    BeeCore.Common.listCamera[Global.IndexChoose].IsMouseDown = false;

            }
            else
                Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexChoose].GetSzCCD();
        
            ShowTool.Full(imgView, Global.Config.SizeCCD);
          
            Global.Config.imgZoom = imgView.Zoom;
           Global.Config.imgOffSetX = imgView.AutoScrollPosition.X;
           Global.Config.imgOffSetY= imgView.AutoScrollPosition.Y;
           
        }

        private void imgView_Click_1(object sender, EventArgs e)
        {

        }

        private void tmRefresh_Tick(object sender, EventArgs e)
        {
            if(btnLive.IsCLick)
            {
                if (btnLive.Enabled == false)
                    btnLive.Enabled = true;
               btnLive.IsCLick = false;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                //{

                //    SetProcessWorkingSetSize32Bit(System.Diagnostics
                //       .Process.GetCurrentProcess().Handle, -1, -1);

                //}
                Thread.Sleep(50);

                btnLive.IsCLick = true;
                if(!workReadCCD.IsBusy)
                workReadCCD.RunWorkerAsync();
            }    
        }

        private void imgView_ZoomChanged(object sender, EventArgs e)
        {
            if (imgView.Zoom < Global.ZoomMinimum)
                imgView.Zoom =(int) Global.ZoomMinimum;
            Global.ScaleZoom = (float)(imgView.Zoom / 100.0);
            Global.pScroll = new Point(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
            imgView.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            //if(Global.IsRun)
            // {
            //     DrawTotalResult();
            // }
        }

       
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw == null) return;
            saveFileDialog.Filter = " PNG|*.png";
          if (  saveFileDialog.ShowDialog()==DialogResult.OK)
            {
                Cv2.ImWrite(saveFileDialog.FileName, BeeCore.Common.listCamera[Global.IndexChoose].matRaw);
            }
        }

        private void tmOut_Tick(object sender, EventArgs e)
        {
           
            if(!workReadCCD.IsBusy)
            {
                tmOut.Enabled = false;
                workReadCCD.RunWorkerAsync();
            }
           
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
           
        }

        private void imgView_SizeChanged_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnMenu_Click_1(object sender, EventArgs e)
        {
         //   pMenu.BackColor = System.Drawing.Color.FromArgb((int)(1 * 255), pMenu.BackColor);
           pMenu.Visible = !pMenu.Visible;
        }

        private void tmShowHis_Tick(object sender, EventArgs e)
        {
           
        }

        private void pInfor_SizeChanged(object sender, EventArgs e)
        {
            if (G.Header == null) return;
             G.StatusDashboard.Region = System.Drawing.Region.FromHrgn(Draws.CreateRoundRectRgn(0, 0,  G.StatusDashboard.Width,  G.StatusDashboard.Height, 10, 10));

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pBtn_SizeChanged(object sender, EventArgs e)
        {
            if (this.Parent == null) return;
            if (this.Width > this.Parent.Width) this.Width = this.Parent.Width;
          
            // BeeCore.CustomGui.RoundRg(this.pBtn,Global.Config.RoundRad);

        }

        private void lbSumOK_Click(object sender, EventArgs e)
        {

        }

        private void pInforTotal_SizeChanged(object sender, EventArgs e)
        {
            //if (G.Header == null) return;
            //BeeCore.CustomGui.RoundRg( G.StatusDashboard,Global.Config.RoundRad);

        }

        private void pBtn_Paint(object sender, PaintEventArgs e)
        {

        }

        private void resultBar1_Load(object sender, EventArgs e)
        {

        }

        private void tmClear_Tick(object sender, EventArgs e)
        {
            //Funtion.SaveModel("\\Program\\Model.prog", BeeCore.G.Model);
           // BeeCore.G.Model = Funtion.LoadModel("\\Program\\Model.prog");
        }

        private void pHeader_SizeChanged(object sender, EventArgs e)
        {
            //BeeCore.CustomGui.RoundRg(pHeader,Global.Config.RoundRad);

        }

        private void btnGird_Click(object sender, EventArgs e)
        {
        
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
           Global.Config.IsShowArea = !Global.Config.IsShowArea;
         imgView.Invalidate();
        }
       
        private  void tmContinuous_Tick(object sender, EventArgs e)
        {
          
            Continuous();
            tmContinuous.Enabled = false;
        }

        private void workTrig_DoWork(object sender, DoWorkEventArgs e)
        {
            //if (Global.ParaCommon.Comunication.Protocol.IsConnected)
            //    Global.ParaCommon.Comunication.IO.WriteInPut(0, true);//.  BtnWriteInPLC((RJButton)sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowCenter = btnShowCenter.IsCLick;
            if (!Global.IsLive)
            Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexChoose].GetSzCCD();
            imgView.Invalidate();
        }

        private void workInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "InsertData", " Save Complete"));
        }

        private void tmPress_Tick(object sender, EventArgs e)
        {
            tmPress.Enabled = false;
            if(Global.IsRun&&! Global.ParaCommon.IsExternal)
            btnCap.Enabled = true;
            else
            {
                btnCap.Enabled = false;
            }
            Global.EditTool.View.btnTypeTrig.IsCLick = false;
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                Files = new List<string>();
              indexFile = 0;
                if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw != null)
                    if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty())
                        BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Release();
                Files .Add( openFile.FileName);
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Cv2.ImRead(Files[indexFile]);
                listMat = new List<Mat>();
                listMat.Add(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
               Native.SetImg(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
                imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
            
                Global.StatusMode = StatusMode.SimOne;
                timer= CycleTimerSplit.Start();
                Global.StatusProcessing = StatusProcessing.Checking;

              
            }

        }

        private void tmSimulation_Tick(object sender, EventArgs e)
        {
            Global.StatusMode = Global.IsSim ? StatusMode.SimContinuous : StatusMode.None;
         
            tmSimulation.Enabled = false;

        X: indexFile++;
                if (indexFile < listMat.Count())
            {
                Global.TriggerNum = TriggerNum.Trigger1;

                if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty())
                        BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Release();
                 BeeCore.Common.listCamera[Global.IndexChoose].matRaw = listMat[indexFile].Clone();

                if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty()) goto X;
                timer = CycleTimerSplit.Start();
                Global.TriggerNum = TriggerNum.Trigger1;
                Global.StatusProcessing = StatusProcessing.Checking;
            }
                else
                {

                indexFile = 0;
                goto X;


                }

         
        }

        private void btnTypeTrig_Click(object sender, EventArgs e)
        {
          
                Global.ParaCommon.IsExternal = !Global.ParaCommon.IsExternal;
        }

        private void btnRunSim_Click(object sender, EventArgs e)
        {
           
        }
        public String pathFileSeleted = "";
        private void btnPlayStep_Click(object sender, EventArgs e)
        {
            if(!Global.IsRun)
            {
                indexFile++;
            }
            if(indexFile>=Files.Count())
            {
                indexFile = 0;

            }
            pathFileSeleted=Files[indexFile];
            BeeCore.Common.listCamera[Global.IndexChoose].matRaw = BeeCore.Common.listCamera[Global.IndexChoose].matRaw = listMat[indexFile]; ;// Cv2.ImRead(Files[indexFile]);
            Native.SetImg(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
            imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
            if (Global.IsRun)
            {
                Global.StatusMode = StatusMode.SimOne;
                RunProcessing();
               
            }

        
          
           
        }

        private void btnDeleteFile_Click(object sender, EventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Setting();
            //File.Delete(Files[indexFile - 1]);
            //Files.RemoveAt(indexFile - 1);
            //listMat.RemoveAt(indexFile - 1);

            //indexFile--;
            //if (indexFile < 0)
            //    indexFile = 0;
            //btnPlayStep.PerformClick();
        }

        private void btnGird_Click_1(object sender, EventArgs e)
        {
           Global.Config.IsShowGird = btnGird.IsCLick;
            if (!Global.IsLive)
                Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexChoose].GetSzCCD();
            imgView.Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void tmLive_Tick(object sender, EventArgs e)
        {
           
        }

        private void workCheck1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        bool wIsWork1 = false;
        bool wIsWork2= false;
        bool wIsWork3 = false;
        bool wIsWork4 = false;

		Checking Checking1 = new Checking(0);
		Checking Checking2 = new Checking(1);
		Checking Checking3 = new Checking(2);
		Checking Checking4 = new Checking(3);
		

         private void workPlay_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // ClassProject.  ProcessingAll();

            if (Processing1 == StatusProcessing.Done && Processing2 == StatusProcessing.Done && Processing3 == StatusProcessing.Done && Processing4 == StatusProcessing.Done)
            {



                //timer.Stop();

                ShowResultTotal();


               // SumCycle = (int)timer.TT + Cyclyle1;
             
                CheckStatusMode();
                IsCompleteAll = false;
                
              
            }
            else
                workPlay.RunWorkerAsync();
            // if (IsAutoTrig==false)
            // {
            //     ShowResultTotal();
            //       indexTool = 0; G.IsCap = false;


            //     if (btnRecord.IsCLick)
            //     {


            //         G.IsCap = true;


            //         if (IsAutoTrig)
            //             workPlay.RunWorkerAsync();
            //         else
            //             workReadCCD.RunWorkerAsync();
            //     }
            //     else
            //     {
            //         G.IsCap = false;
            //     }
            //     //if (G.Header.SerialPort1.IsOpen)
            //     //{
            //     //    Thread.Sleep(100);
            //     //    G.Header.SerialPort1.WriteLine("OffTrig");
            //     //  //  G.Header.SerialPort.WriteLine("OffTrig");
            //     //}
            //     return;
            // }

            //     if (G.StatusTrig==Trig.Continue)
            // {
            //     G.StatusTrig = Trig.NotTrig;
            //     ShowResultTotal();


            //     indexTool = 0; G.IsCap = false;

            //     if (btnRecord.IsCLick)
            //     {


            //         G.IsCap = true;

            //         if (IsAutoTrig)

            //             workPlay.RunWorkerAsync();
            //         else
            //                 if (!workReadCCD.IsBusy)
            //                 workReadCCD.RunWorkerAsync();
            //     }
            //     else
            //     {
            //         G.IsCap = false;
            //     }



            // }

            //else if (G.StatusTrig == Trig.NotTrig|| G.StatusTrig==Trig.Processing)
            // {
            //    Mat matCCD = BeeCore.Native.GetImg();

            //     Tools tool = G.listAlltool[indexToolPosition];

            //     OutLine Para = (OutLine)G.listAlltool[indexToolPosition].tool.Propety;
            //     RectRotate rot = Propety.rotArea;
            //     float angle = rot._rectRotation;
            //     if (rot._rectRotation < 0) angle = 360 + rot._rectRotation;
            //     Mat matCrop = Global.EditTool.View.CropRotatedRect(matCCD, new RotatedRect(new Point2f(rot._PosCenter.X + (rot._rect.Width / 2 + rot._rect.X), rot._PosCenter.Y + (rot._rect.Height / 2 + rot._rect.Y)), new Size2f(rot._rect.Width, rot._rect.Height), angle));

            //     matCrop.CopyTo(new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw, new Rect((int)rot._PosCenter.X + (int)rot._rect.X, (int)rot._PosCenter.Y + (int)rot._rect.Y, (int)rot._rect.Width, (int)rot._rect.Height)));
            //     imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();


            //     DelayTrig = Propety.DelayTrig;
            //     tmTrig.Interval = 1;
            //     tmTrig.Enabled = true;
            //     return;
            // }
            // else if (G.StatusTrig==Trig.Trigged)
            // {

            //     BeeCore.Common.listCamera[Global.IndexChoose].matRaw = BeeCore.Native.GetImg();
            //     imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
            //     tmTrig.Enabled = true;
            //     tmTrig.Interval = DelayTrig;
            //     return;
            // }
            // else if (G.StatusTrig == Trig.Complete)
            // {

            //    // BeeCore.Common.listCamera[Global.IndexChoose].matRaw = BeeCore.Common.GetImageRaw();
            //     //imgView.ImageIpl = BeeCore.Common.listCamera[Global.IndexChoose].matRaw;
            //     tmTrig.Enabled = true;
            //     tmTrig.Interval = DelayTrig;
            //     return;
            // }


        }
        StatusProcessing Processing1 = StatusProcessing.None;
		StatusProcessing Processing2 = StatusProcessing.None;
		StatusProcessing Processing3 = StatusProcessing.None;
		StatusProcessing Processing4 = StatusProcessing.None;
        public async Task CheckStatus()
        {
            await Task.Run(async () =>
            {
                // simple delay inside loop to avoid 100% CPU spin
                while (!_cts.Token.IsCancellationRequested)
                {
                    if (Global.Config.IsMultiCamera == false)
                    {
                        if (Processing1 == StatusProcessing.Done)
                            return;
                    }
                    else
                    {
                        if (Processing1 == StatusProcessing.Done
                         && Processing2 == StatusProcessing.Done
                         && Processing3 == StatusProcessing.Done
                         && Processing4 == StatusProcessing.Done)
                        {

                            return;    // exit the Task.Run delegate
                        }
                    }
                }
            }, _cts.Token);
         //   timer.Stop();
            Global.TotalOK = true;
            foreach ( Camera camera in BeeCore.Common.listCamera)
            {if (camera == null)
                    continue;
               camera. SumResult();
                if(camera.Results==Results.NG)
                {
                    Global.TotalOK = false;
                    break;
                }    
            }
            if (Global.IsByPassResult)
                Global.StatusProcessing = StatusProcessing.Drawing;
            else
                Global.StatusProcessing = StatusProcessing.SendResult;
        
        }
        public async void RunProcessing()
        {
           
            
            if (Global.Config.IsMultiCamera)
            {
                if (BeeCore.Common.listCamera[0] != null)
                {
                    Checking1.StatusProcessing = StatusProcessing.None;
                    Checking1.Start();
                }
                else
                    Processing1 = StatusProcessing.Done;
                if (Global.Config.IsMultiCamera == false)
                {
                    await CheckStatus();
                    return;
                }
                if (BeeCore.Common.listCamera[1] != null)
                {
                    Checking2.StatusProcessing = StatusProcessing.None;
                    Checking2.Start();
                }
                else
                    Processing2 = StatusProcessing.Done;

                if (BeeCore.Common.listCamera[2] != null)
                {
                    Checking3.StatusProcessing = StatusProcessing.None;
                    Checking3.Start();
                }
                else
                    Processing3 = StatusProcessing.Done;
                if (BeeCore.Common.listCamera[3] != null)
                {
                    Checking4.StatusProcessing = StatusProcessing.None;
                    Checking4.Start();
                }
                else
                    Processing4 = StatusProcessing.Done;
            }
            else
            {
                if (BeeCore.Common.listCamera[0] != null)
                {
                    Checking1.StatusProcessing = StatusProcessing.None;
                    Checking1.Start();
                }
                else
                    Processing1 = StatusProcessing.Done;
               
            }
                await CheckStatus();


        }
		private readonly CancellationTokenSource _cts = new CancellationTokenSource
		  ();
		private void imgView_Scroll(object sender, ScrollEventArgs e)
        {
            Global.ScaleZoom = (float)(imgView.Zoom / 100.0);
            Global.pScroll = new Point(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
        }

        private async void tmFist_Tick(object sender, EventArgs e)
        {
           
           
        }

        private void tmShow_Tick(object sender, EventArgs e)
        {
           if(Global.ParaCommon.Comunication.Protocol.IsConnected)
                Global.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;
           else
                Global.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;

        }

        private void tmEnableControl_Tick(object sender, EventArgs e)
        {
            controlEdit.Enabled = true;
            tmEnableControl.Enabled = false;
        }

        private void pView_SizeChanged(object sender, EventArgs e)
        {
            if(imgView!=null)
            imgView.Size = pView.Size;
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnShowArea_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowArea = btnShowArea.IsCLick;
            if (!Global.IsLive)
                Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexChoose].GetSzCCD();
            imgView.Invalidate();
        }

        private void pBtn_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void btnZoomIn_MouseUp(object sender, MouseEventArgs e)
        {
            if ((Control.MouseButtons & MouseButtons.Left) == 0)
                StopRepeat();
        }

        private void btnZoomIn_MouseLeave(object sender, EventArgs e)
        {
            if ((Control.MouseButtons & MouseButtons.Left) == 0)
                StopRepeat();
        }

        private void btnZoomOut_MouseUp(object sender, MouseEventArgs e)
        {
            if ((Control.MouseButtons & MouseButtons.Left) == 0)
                StopRepeat();
        }

        private void btnZoomOut_MouseLeave(object sender, EventArgs e)
        {
            if ((Control.MouseButtons & MouseButtons.Left) == 0)
                StopRepeat();
        }

        private void btnZoomIn_MouseDown(object sender, MouseEventArgs e)
        {
            Focus();
            ApplyStep(+1);
            BeginRepeat(+1);
        }

        private void showImageFilter_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowMatProcess = showImageFilter.Checked;
            Global.StatusDraw = StatusDraw.Check;
            imgView.Invalidate();
        }

        private void showDetailTool_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowDetail = showDetailTool.Checked; 
            Global.StatusDraw = StatusDraw.Check;
            imgView.Invalidate();
        }

        private void showResultTool_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowResult = showResultTool.Checked;
            Global.StatusDraw = StatusDraw.Check;
            imgView.Invalidate();
        }

        private void showDetailWrong_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowNotMatching = showDetailWrong.Checked;
            Global.StatusDraw = StatusDraw.Check;
            imgView.Invalidate();

        }

        private void newShapeTool_Click(object sender, EventArgs e)
        {
            NewShape();
            Global.StatusDraw = StatusDraw.Edit;
            imgView.Invalidate();
        }

        private void btnZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
            Focus();
            ApplyStep(-1);
            BeginRepeat(-1);
        }

        private void btnClick_Click(object sender, EventArgs e)
        {
            if (btnClick.IsCLick)
            {   btnPan.IsCLick = false;
                imgView.Cursor = Cursors.Hand;
                imgView.AllowClickZoom = true;
                imgView.AllowDoubleClick = true;
                imgView.PanMode = ImageBoxPanMode.None;

                imgView.InvertMouse = false;
            }
            else
            {
                btnPan.IsCLick = true;
                imgView.AllowClickZoom = false;
                imgView.AllowDoubleClick = false;

                imgView.Cursor = Cursors.Default;
                imgView.PanMode = ImageBoxPanMode.Left;


            }
        }

        private void btnMouseRight_Click(object sender, EventArgs e)
        {
            if (btnMouseRight.IsCLick)
            {
                btnClick.IsCLick = false;
                btnClick.Enabled = false;
                btnPan.IsCLick = true;
                imgView.AllowClickZoom = false;
                imgView.AllowDoubleClick = false;
                imgView.AllowZoom = false;
                imgView.ContextMenuStrip = contextMenu;
                imgView.Cursor = Cursors.Default;
                imgView.PanMode = ImageBoxPanMode.Left;
              
            }
            else
            {
             
                btnClick.Enabled = true;
                imgView.AllowZoom = true;
                imgView.ContextMenuStrip = null;
            }    
               
        }
         public    RegisterImgs RegisterImgs = new RegisterImgs();
        public SimImgs SimImgs = new SimImgs();
        private void btnChangeImg_Click(object sender, EventArgs e)
        {
         
           
            
            
            if (!Global.IsRun)
            {
                RegisterImgs.Dock = DockStyle.Fill;
               
                pImg.Show("Res");
                RegisterImgs.LoadData();

            }
            else
            {
                SimImgs.Dock = DockStyle.Fill;
                pImg.Show("Sim");


            }
            pImg.Visible = btnChangeImg.IsCLick;
            spImgs.Visible = btnChangeImg.IsCLick;
            if (btnChangeImg.IsCLick)
            {
                pImg.BringToFront();
                spImgs.BringToFront();
                pView.BringToFront();

            }
        }

        private void chooseAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global._TypeCrop = TypeCrop.Area;
            imgView.Invalidate();
        }

        private void chooseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global._TypeCrop = TypeCrop.Crop;
            imgView.Invalidate();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Global._TypeCrop = TypeCrop.Mask;
            imgView.Invalidate();
        }

        private void NewShape()
        {
            // 1) Chốt shape hiện tại
            var prop = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety;
            RectRotate rr = null; 
            
            if (Global.TypeCrop == TypeCrop.Area)
                rr = prop?.rotArea;
            else if (Global.TypeCrop == TypeCrop.Mask)
                rr = prop?.rotMask;
            else
                rr = prop?.rotCrop;
            ShapeType newShape = rr.Shape;
            if (rr != null)
            {
                // Nếu đang drag: chấm dứt
                rr._dragAnchor = AnchorPoint.None;
                rr.ActiveVertexIndex = -1;

                // Nếu là polygon đang dựng dở
                if (rr.Shape == ShapeType.Polygon && rr.IsPolygonClosed == false)
                {
                    
                    // (C) Huỷ polygon đang dựng
                    rr.PolygonClear();
                }
            }



            // 3) Gán shape mới & chuẩn bị khung
            if (rr == null)
            {
                // tuỳ code lưu trữ của bạn mà tạo mới:
                rr = new RectRotate();
                if (Global.TypeCrop == TypeCrop.Area) prop.rotArea = rr;
                else if (Global.TypeCrop == TypeCrop.Mask) prop.rotMask = rr;
                else prop.rotCrop = rr;
            }
           
            rr.Shape = newShape;

            switch (newShape)
            {
                case ShapeType.Polygon:
                    // Local sạch, xoá điểm cũ: chờ click điểm đầu tiên
                    rr.ResetFrameForNewPolygonHard();
                    rr.AutoOrientPolygon = false; // thường tắt lúc dựng, bạn có thể để true nếu quen
                    break;

                case ShapeType.Rectangle:
                case ShapeType.Ellipse:
                case ShapeType.Hexagon:
                    // Không cần xoá toàn bộ; chỉ đảm bảo không kéo theo trạng thái cũ
                    rr._dragAnchor = AnchorPoint.None;
                    rr.ActiveVertexIndex = -1;

                    // Option: reset rotation cho phiên mới (tuỳ UX)
                    // rr._rectRotation = 0f;

                    // Để trống _rect: user kéo trái→phải để tạo mới theo logic MouseDown/Move của bạn
                    rr._rect = RectangleF.Empty;

                    // Hexagon: offsets về 0
                    if (newShape == ShapeType.Hexagon)
                    {
                        if (rr.HexVertexOffsets == null || rr.HexVertexOffsets.Length != 6)
                            rr.HexVertexOffsets = new PointF[6];
                        for (int i = 0; i < 6; i++) rr.HexVertexOffsets[i] = PointF.Empty;
                    }

                    break;
            }

            // Cập nhật về prop
            if (Global.TypeCrop == TypeCrop.Area) prop.rotArea = rr;
            else if (Global.TypeCrop == TypeCrop.Mask) prop.rotMask = rr;
            else prop.rotCrop = rr;


        }

        private void btnPan_Click(object sender, EventArgs e)
        {
           if(btnPan.IsCLick)
            {
                btnClick.IsCLick = false;
                imgView.AllowClickZoom = false;
                imgView.AllowDoubleClick = false;
                imgView.Cursor = Cursors.Hand;
             
                imgView.PanMode = ImageBoxPanMode.Left;
                
                imgView.InvertMouse = false;
            }
            else
            {
                btnClick.IsCLick = true;
                imgView.AllowClickZoom = true;
                imgView.AllowDoubleClick = true;
                imgView.Cursor = Cursors.Default;
                imgView.PanMode = ImageBoxPanMode.None;
               
               
            }
             // true = pan follows mouse, false = opposite
        }

        private void tmCheckPort_Tick(object sender, EventArgs e)
        {

        }

        private void workLiveWebcam_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void btnRunSim_Click_1(object sender, EventArgs e)
        {
            if (Files == null) return;
            if (Files.Count == 0) return;
            
            Global.StatusMode = Global.EditTool.IsRunSim ? StatusMode.SimContinuous : StatusMode.None;
            if (Global.EditTool.IsRunSim)
            {
                
               
                if (indexFile >= listMat.Count) indexFile = 0;
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = listMat[indexFile];// Cv2.ImRead(Files[indexFile]);
                imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                Global.StatusProcessing = StatusProcessing.Checking;
            }
           
            if(indexFile >= Files.Count)
            indexFile = 0;
     

        }

 
       

        //private void imgView_MouseUp(object sender, MouseEventArgs e)
        //{

        //    if (Global.IndexToolSelected == -1) return;
        //    _drag = false;
        //    if(BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotCrop!=null)
        //        BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotCrop._dragAnchor = AnchorPoint.None;
          
        //        ToolMouseUp();
         
        //    try
        //    {
        //        var prop = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected]?.Propety;
        //        if (prop == null) return;

        //        RectRotate rr = null;
        //        if (Global.TypeCrop == TypeCrop.Area) rr = prop.rotArea;
        //        else if (Global.TypeCrop == TypeCrop.Mask) rr = prop.rotMask;
        //        else rr = prop.rotCrop;

        //        if (rr != null)
        //        {
        //            // Kết thúc kéo: bỏ anchor, bỏ active vertex
        //            rr._dragAnchor = AnchorPoint.None;
        //            rr.ActiveVertexIndex = -1;
        //        }

        //        // Reset cờ kéo
        //        _drag = false;

        //        //// Trả quyền pan/zoom
        //        //if (btnPan.IsCLick)
        //        //    imgView.PanMode = ImageBoxPanMode.Left;
        //        //imgView.AllowClickZoom = true;
        //        //imgView.AllowDoubleClick = true;

        //        // Vẽ lại (để mất highlight kéo)
        //        imgView.Invalidate();
        //    }
        //    catch
        //    {
        //        // log nếu cần
        //    }
        //    imgView.Invalidate();
        //}
    }
}
