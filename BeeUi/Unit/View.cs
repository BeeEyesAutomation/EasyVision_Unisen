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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
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
        AdjustControlMouse angleCtrl;
        AdjustMoveMouse centerCtrl;
        private void ShowCursor( AnchorPoint anchorPoint ,float angle)
        {
            if (angle < 0) angle = 360 + angle;
            if (angle == 360) angle =0;
            //Cursor curLeft= Cursors.SizeNWSE;
            //Cursor curRight= Cursors.SizeNESW;
            Cursor curLeft = Cursors.Default;
            Cursor curRight = Cursors.Default;
         
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
          bmMask = new Mat(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Rows,BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Cols, MatType.CV_8UC1, Scalar.Black);

            Mat matGroup= new Mat(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Cols, MatType.CV_8UC1, Scalar.Black);
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
        //  FORM / CONTROL: Mouse handlers 

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
            if (rr._dragAnchor == AnchorPoint.Rotation)
                ShowAngleControl(e.Location);
            else if (rr._dragAnchor == AnchorPoint.Center)
                ShowCenterControl(e.Location);
            else
                HideAngleControl();
            // ===== Polygon: thêm điểm / chọn vertex =====
            using (var inv = BuildLocalInverseMatrixFor(rr, (float)imgView.Zoom, imgView.AutoScrollPosition, false, PointF.Empty, 0f))
            {
                PointF pLocal = TransformPoint(inv, e.Location);

                if (rr.Shape == ShapeType.Polygon)
                {
                    float handle = Global.ParaShow.RadEdit;
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
            if (Global.IndexToolSelected == -1)
            {
                HideAngleControl();
                return;
            }

            if (Global.IsRun)
            {
                HideAngleControl();
                return;
            }
            imgView.Cursor = Cursors.Default;
            // Lưu vị trí chuột để OnPaint vẽ preview
            pMove = e.Location;
           
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
                var rrSrc = getCurrentRR();
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
              
                if (rrSrc == null) return;
               
              
                // ====== NHÁNH ĐANG KÉO (drag/resize/rotate/move) ======
                if (_drag)
                {




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
                    switch (rrSrc._dragAnchor)
                    {
                        case AnchorPoint.Center:
                            imgView.Cursor = Cursors.Hand;
                            break;
                        case AnchorPoint.BottomLeft:
                            imgView.Cursor = Cursors.SizeNESW;
                            break;
                        case AnchorPoint.TopLeft:
                            imgView.Cursor = Cursors.SizeNWSE;
                            break;
                        case AnchorPoint.BottomRight:
                            imgView.Cursor = Cursors.SizeNWSE;
                            break;
                        case AnchorPoint.TopRight:
                            imgView.Cursor = Cursors.SizeNESW;
                            break;
                        case AnchorPoint.Rotation:

                            imgView.Cursor = new Cursor(Properties.Resources.Rotate1.Handle);
                            break;
                        case AnchorPoint.None:
                            imgView.Cursor = Cursors.Default;
                            break;
                    }
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
                                        rotateRect.UpdateFromPolygon(false); // KHÔNG đè lại góc vừa xoay
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
                        int maxW = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Width;
                        int maxH = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Height;

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

                    float r = Global.ParaShow.RadEdit;

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

        void ShowAngleControl(Point mouseScreenPos)
        {
          
            if (angleCtrl == null)
            {
                angleCtrl = new AdjustControlMouse();
                angleCtrl.AngleDeltaChanged += delta =>
                {
                  
                    RectRotate rr = GetCurrentRR();
                    float valueCur = rr._rectRotation;
                    valueCur += delta;
                    rr._rectRotation = valueCur;
                    SetCurrentRR(rr);
                    imgView.Invalidate();
                };
                this.imgView.Controls.Add(angleCtrl);
            }

            if (centerCtrl != null)
                centerCtrl.Visible = false;
            angleCtrl.Location = new Point(mouseScreenPos.X +10, mouseScreenPos.Y +10);
            angleCtrl.Visible = true;
            angleCtrl.BringToFront();
        }
        void HideAngleControl()
        {if(angleCtrl!=null)
            angleCtrl.Visible = false;
            if (centerCtrl != null)
                centerCtrl.Visible = false;
        }
        void ShowCenterControl(Point mouseScreenPos)
        {
            if (angleCtrl != null)
                angleCtrl.Visible = false;
            if (centerCtrl == null)
            {
                centerCtrl = new AdjustMoveMouse();
                centerCtrl.CenterDeltaChanged += CenterCtrl_CenterDeltaChanged;
                this.imgView.Controls.Add(centerCtrl);
            }


            centerCtrl.Location = new Point(mouseScreenPos.X + 10, mouseScreenPos.Y + 10);
            centerCtrl.Visible = true;
            centerCtrl.BringToFront();
        }

        private void CenterCtrl_CenterDeltaChanged(float arg1, float arg2)
        {
            RectRotate rr = GetCurrentRR();
            float xCur = rr._PosCenter.X;
            float yCur = rr._PosCenter.Y;
            xCur += arg1;
            yCur += arg2;
            rr._PosCenter=new PointF(xCur, yCur);

            SetCurrentRR(rr);
            imgView.Invalidate();
        }

        void HideCenterControl()
        {
            if (angleCtrl != null)
                angleCtrl.Visible = false;
            if (centerCtrl != null)
                centerCtrl.Visible = false;
        }
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
      
        
     
     
        Graphics gc;

        StatusDraw oldStatus = StatusDraw.Edit;
       // public List<RectRotate> listChoose = new List<RectRotate>();
        private void imgView_Paint(object sender, PaintEventArgs e)
        {

            if (!Global.IsLive&&Global.IsRun)
            {
                HideAngleControl();

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
            //        Pen pen = new Pen(Global.ParaShow.ColorNone,Global.ParaShow.ThicknessLine);
            //        if(rot._dragAnchor==AnchorPoint.Center)
            //        {
            //            pen= new Pen(Global.ParaShow.ColorChoose, Global.ParaShow.ThicknessLine);
            //        }    
            //        gc.DrawPolygon(pen, rot.PolyLocalPoints.ToArray());
            //        gc.ResetTransform();
            //    }
            //    return;
            //}
            if(Global.Config.SizeCCD.Width==0)
            {
                if (!Global.IsLive)
                    if(BeeCore.Common.listCamera[Global.IndexCCCD]!=null)
                    Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexCCCD].GetSzCCD();//
            }    
            int index = 0;
            if (Global.IsLive)
                gc.DrawString("LIVE", new Font("Arial", Global.ParaShow.FontSize,FontStyle.Bold), Brushes.Red, new Point(50, 50));
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
                        gc.DrawRectangle(new Pen(Global.ParaShow.ColorInfor, Global.ParaShow.ThicknessLine), new Rectangle((int)_rect3.X, (int)_rect3.Y, (int)_rect3.Width, (int)_rect3.Height));
                        String s = (int)(indexTool + 1) + "." + BeeCore.Common.PropetyTools[Global.IndexChoose][indexTool].Name;
                        SizeF sz = gc.MeasureString(s, new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold));
                        gc.FillRectangle(new SolidBrush( Global.ParaShow.ColorInfor), new Rectangle((int)rot._rect.X, (int)rot._rect.Y, (int)sz.Width, (int)sz.Height));
                        gc.DrawString(s, new Font("Arial", Global.ParaShow.FontSize, FontStyle.Bold), new SolidBrush(Global.ParaShow.TextColor), new System.Drawing.Point((int)rot._rect.X, (int)rot._rect.Y));
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
                HideAngleControl();
                gc.ResetTransform();
              
                Global.ScaleZoom = (float)(imgView.Zoom / 100.0);
                Global.pScroll = new Point(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
                
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.DrawResult(gc);
                Global.EditTool.lbCTTool.Text = Math.Round(BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].CycleTime) + "ms";
                Global.EditTool.lbRsTool.Text = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Results.ToString();
                if (BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Results==Results.OK)
                {
                    Global.EditTool.lbRsTool.BackColor = Global.ParaShow.ColorOK;
                }
                else
                    Global.EditTool.lbRsTool.BackColor = Global.ParaShow.ColorNG;


                //return;
            }
            else if (Global.StatusDraw == StatusDraw.Edit)
            {
                switch (Global.TypeCrop)
                {
                    case TypeCrop.Crop:
                        Draws.FillRect(gc, TypeCrop.Area, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotArea, imgView.AutoScrollPosition, imgView.Zoom, 20);
                        Draws.FillRect(gc, TypeCrop.Mask, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotMask, imgView.AutoScrollPosition, imgView.Zoom, 50);
                        Draws.RectEdit(gc, TypeCrop.Crop, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotCrop,  Global.ParaShow.RadEdit, imgView.AutoScrollPosition, imgView.Zoom, pMove, 4);

                        break;
                    case TypeCrop.Area:

                        Draws.FillRect(gc, TypeCrop.Crop, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotCrop, imgView.AutoScrollPosition, imgView.Zoom, 20);
                        Draws.FillRect(gc, TypeCrop.Mask, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotMask, imgView.AutoScrollPosition, imgView.Zoom, 50);
                        Draws.RectEdit(gc, TypeCrop.Area, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotArea, Global.ParaShow.RadEdit, imgView.AutoScrollPosition, imgView.Zoom, pMove, 4);
                        break;
                    case TypeCrop.Mask:
                        Draws.FillRect(gc, TypeCrop.Area, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotArea, imgView.AutoScrollPosition, imgView.Zoom, 20);
                        Draws.FillRect(gc, TypeCrop.Crop, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotCrop, imgView.AutoScrollPosition, imgView.Zoom, 50);
                        Draws.RectEdit(gc, TypeCrop.Mask, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.rotMask, Global.ParaShow.RadEdit, imgView.AutoScrollPosition, imgView.Zoom, pMove, 4);

                        break;

                }

                gc.ResetTransform();
            }
            else if (Global.StatusDraw==StatusDraw.None)
            {
                HideAngleControl();
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
     
        private  void View_Load(object sender, EventArgs e)
        {
            
            pImg.Register("Res", () => RegisterImgs);
            pImg.Register("Sim", () =>SimImgs);
            if (G.Header == null) return;
            //  this.pBtn.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar,Global.Config.colorGui);
            Global.Config.IsShowArea = false;
            Global.Config.IsShowCenter = false;
            Global.Config.IsShowGird = false;
            Global.ParaShow.IsShowResult = true;
            Global.ParaShow.IsShowMatProcess = true;
            Global.ParaShow.IsShowNotMatching = true;
            showResultTool.Checked = Global.ParaShow.IsShowResult;
            showDetailTool.Checked = Global.ParaShow.IsShowDetail;
            showImageFilter.Checked = Global.ParaShow.IsShowMatProcess;
            showDetailWrong.Checked = Global.ParaShow.IsShowNotMatching;
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
            //BeeCore.Common.listCamera[Global.IndexCCCD].matRaw= BeeCore.Common.GetImageRaw();
            //if (BeeCore.Common.listCamera[Global.IndexCCCD].matRaw!=null)
            //    if (!BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Empty())
            //        imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
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
            _renderer = new CollageRenderer(imgView, gutter: 8, background: Color.White, autoRenderOnResize: true);
            imgView.AutoCenter = true;
          
            if (Global.Comunication.Protocol == null)
                Global.Comunication.Protocol = new ParaProtocol();
            RefreshExternal(Global.ParaCommon.IsExternal);
            G.Header.RefreshListPJ();
            Global.PLCStatusChanged += Global_PLCStatusChanged;
            Global.CameraStatusChanged += Global_CameraStatusChanged;
            Global.ChangeProg += Global_ChangeProg;
            Global.ChangeDummy += Global_ChangeDummy;
            Global.AutoShuttDown += Global_AutoShuttDown;
            Global.Comunication.Protocol.ProgressChanged += Protocol_ProgressChanged;
        }

        private void Protocol_ProgressChanged(int obj)
        {
            this.Invoke((Action)(async () =>
            {
                Global.EditTool.StepProccessBar.DoneCount = obj;
            }));
        }

        private void Global_AutoShuttDown(bool obj)
        {
           if(obj==true)
            {
                this.Invoke((Action)(async () =>
                {
                    Actions.Shuttown();
                }));
            }    
        }

        private void Global_ChangeDummy(bool obj)
        {
            G.Header.btnDummy.IsCLick = obj;
        }

        private void Global_ChangeProg(bool obj)
        {
            this.Invoke((Action)(() =>
            {
                if (obj)
                {
                    if (Global.IsPLCChangeProg == true)
                    {
                        Global.IsPLCChangeProg = false;
                        int ix = Global.ListProgNo.FindIndex(a => a.No == Global.Comunication.Protocol.NoProg);
                        if (ix > -1)
                            Global.Comunication.Protocol.ValueProg = Global.ListProgNo[ix].Name;
                        if (Global.Comunication.Protocol.ValueProg != null)
                            if (Global.Comunication.Protocol.ValueProg != Global.Project)
                            {
                                if (G.Header.listNameProg.IndexOf(Global.Comunication.Protocol.ValueProg) >= 0)
                                {
                                    Global.Project = Global.Comunication.Protocol.ValueProg.Trim();
                                    G.Header.ChangeProgram(Global.Project);
                                    //G.Header.workLoadProgram.RunWorkerAsync();
                                    G.StatusDashboard.TotalTimes = Global.Config.SumTime;
                                    G.StatusDashboard.OkCount = Global.Config.SumOK;
                                    G.StatusDashboard.NgCount = Global.Config.SumNG;
                                }
                                else
                                {
                                    Global.IsChangeProg = false;
                                    imgView.Text = "";
                                    FormWarning formWarning = new FormWarning("Change Program", "Not has Prog : " + Global.Comunication.Protocol.ValueProg);
                                    formWarning.ShowDialog();
                                    MessageChoose messageChoose = new MessageChoose("Copy Program", "Copy Program From  : " +Global.Project+" To " + Global.Comunication.Protocol.ValueProg);
                                    messageChoose.ShowDialog();
                                    if (messageChoose.IsOK)
                                    {

                                       
                                        G.StatusDashboard.TotalTimes = Global.Config.SumTime;
                                        G.StatusDashboard.OkCount = Global.Config.SumOK;
                                        G.StatusDashboard.NgCount = Global.Config.SumNG;
                                        Batch.CopyAndRename("Program\\" + Global.Project, Global.Comunication.Protocol.ValueProg);
                                        Global.Project = Global.Comunication.Protocol.ValueProg;
                                         G.Header.RefreshListPJ();
                                        Global.IsLoadProgFist = true;
                                        G.Header.ChangeProgram(Global.Project);
                                        //if (!G.Header.workLoadProgram.IsBusy)
                                        //    G.Header.workLoadProgram.RunWorkerAsync();


                                    }    
                                
                                }

                            }
                            else
                            {
                                imgView.Text = "";
                                Global.IsChangeProg = false;
                                Global.Comunication.Protocol.IO_Processing = IO_Processing.Reset;
                                return;
                            }
                       
                    }
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
                    G.StatusDashboard.TotalTimes = Global.Config.SumTime;
                    G.StatusDashboard.OkCount = Global.Config.SumOK;
                    G.StatusDashboard.NgCount = Global.Config.SumNG;
                    imgView.Text = "";
                }
            }));
        }

        private  void Global_LiveChanged(bool obj)
        {
            this.Invoke((Action)(() =>
            {
                G.Header.btnMode.Enabled = !Global.IsLive;
                if (Global.IsRun)
                {
                    if (!Global.IsLive)
                    {
                        G.StatusDashboard.StatusText = "---";
                        G.StatusDashboard.StatusBlockBackColor = Global.ColorNone;
                        imgView.Text = "Wait Trigger ..";

                        Live();
                    }

                    else
                    {

                        G.StatusDashboard.StatusText = "LIVE";
                        G.StatusDashboard.StatusBlockBackColor = Color.Red;
                        imgView.Text = "";
                        Live();
                    }
                }
                else
                {
                    if(imgView.Image!=null)
                        if (!imgView.Image.IsDisposed())

                        {
                        imgView.Text = "";
                        imgView.Image.Dispose();   // tránh leak bộ nhớ nếu là Bitmap tự tạo
                        imgView.Image = null;      // xoá ảnh khỏi control
                    }    
                   
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
                    Global.Comunication.Protocol.IO_Processing = IO_Processing.NoneErr;
                    Global.EditTool.lbCam.Image = Properties.Resources.CameraConnected;
                    Global.EditTool.lbCam.Text = "Camera Connected";
                    break;
                case CameraStatus.ErrorConnect:
                    this.Invoke((Action)(() =>
                    {
                        Global.Comunication.Protocol.IO_Processing = IO_Processing.Error;
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
                    Global.Comunication.Protocol.IsBypass = true;
                    break;
                case PLCStatus.Ready:
                    Global.Comunication.Protocol.IsBypass = false;
                    Global.EditTool.toolStripPort.Text = "PLC Ready";
                    Global.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;
                    break;
                case PLCStatus.ErrorConnect:
                    this.Invoke((Action)(() =>
                    {
                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "PLC", "PLC Error Connect"));
                        Global.EditTool.toolStripPort.Text = "PLC Error Connect";
                        Global.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;
                        Global.Comunication.Protocol.IsBypass = true;
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
                if (Global.Comunication.Protocol.IsBypass)
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
            Global.Comunication.Protocol.IO_Processing = IO_Processing.ChangeMode;
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
               if(Global.Comunication.Protocol.IsBypass)
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
            if (Global.IsDebug)
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.TRACE, "Processing", obj.ToString()));
            switch (obj)
            {
                case StatusProcessing.None:
                    break;
                case StatusProcessing.ResetImg:
                    if (Global.Config.IsResetImg)
                    {
                        this.Invoke((Action)(() =>
                        {
                            G.StatusDashboard.StatusText = "---";
                            G.StatusDashboard.StatusBlockBackColor = Global.ColorNone;
                            if (imgView.Image != null)
                            {
                                imgView.Text = "Waiting Checking...";
                                imgView.Image.Dispose();   // tránh leak bộ nhớ nếu là Bitmap tự tạo
                                imgView.Image = null;      // xoá ảnh khỏi control
                            }
                            Global.StatusProcessing = StatusProcessing.None;
                        }));
                    }
                    break;
                    
                case StatusProcessing.Trigger:

                    timer = CycleTimerSplit.Start();

                    if (Global.Config.IsResetImg && Global.TriggerNum == TriggerNum.Trigger1)
                    {
                        this.Invoke((Action)(() =>
                        {
                            if (imgView.Image != null)
                            {
                                imgView.Text = "Waiting Checking...";
                                imgView.Image.Dispose();   // tránh leak bộ nhớ nếu là Bitmap tự tạo
                                imgView.Image = null;      // xoá ảnh khỏi control
                            }
                            G.Header.txtPO.Text = Global.Config.POCurrent;
                            _renderer.ClearImages();
                            if (Global.ToolSettings.Labels.Length >= 2)
                            {
                                Global.ToolSettings.Labels[1].Results = Results.None;
                                Global.ToolSettings.Labels[1].BackColor = Global.ParaShow.ColorNone;
                            }

                            if (BeeCore.Common.PropetyTools.Count >= 2)
                                if (BeeCore.Common.PropetyTools[1] != null)
                                    foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[1])
                                    {
                                        PropetyTool.StatusTool = StatusTool.WaitCheck;
                                    }
                        }));
                    }

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
                    
                    if (Global.Config.IsAutoTrigger)
                    {
                        timer = CycleTimerSplit.Start();
                    }    


                    if(!Global.ParaCommon.IsExternal&&Global.Config.IsResetImg)
                    {
                        this.Invoke((Action)(() =>
                        {
                            if (imgView.Image != null)
                            {
                                imgView.Text = "Waiting Checking...";
                                imgView.Image.Dispose();   // tránh leak bộ nhớ nếu là Bitmap tự tạo
                                imgView.Image = null;      // xoá ảnh khỏi control
                            }
                        }));
                    }    
                  
                    timer.Split("R");
                    Global.IsAllowReadPLC = false;
                    if (Global.IsDebug)
                    {
                        G.StatusDashboard.StatusText = obj.ToString();
                        G.StatusDashboard.StatusBlockBackColor = Global.ColorNone;
                    }
                    if (!workReadCCD.IsBusy)
                        workReadCCD.RunWorkerAsync();
                 

                    break;
                case StatusProcessing.Checking:
                   
                    if (Global.IsAutoTemp)
                    {
                        switch (Global.TriggerNum)
                        {
                            case TriggerNum.Trigger1:
                                if (Global.ParaCommon.matRegister != null)
                                    Global.ParaCommon.matRegister.Dispose();
                                Global.ParaCommon.matRegister = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                                Global.listRegsImg[0].Image = Global.ParaCommon.matRegister;
                                break;
                            case TriggerNum.Trigger2:
                                if (Global.ParaCommon.matRegister2 != null)
                                    Global.ParaCommon.matRegister2.Dispose();
                                Global.ParaCommon.matRegister2 = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                                if (Global.listRegsImg.Count < 2)
                                    Global.listRegsImg.Add(new ItemRegsImg("Image 2",null));
                                Global.listRegsImg[1].Image = Global.ParaCommon.matRegister2;
                                break;
                            case TriggerNum.Trigger3:
                                if (Global.ParaCommon.matRegister3 != null)
                                    Global.ParaCommon.matRegister3.Dispose();
                                if (Global.listRegsImg.Count < 3)
                                    Global.listRegsImg.Add(new ItemRegsImg("Image 3", null));
                                Global.ParaCommon.matRegister3 = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                                Global.listRegsImg[2].Image = Global.ParaCommon.matRegister3;
                                break;
                            case TriggerNum.Trigger4:
                                if (Global.ParaCommon.matRegister4 != null)
                                    Global.ParaCommon.matRegister4.Dispose();
                                if (Global.listRegsImg.Count < 4)
                                    Global.listRegsImg.Add(new ItemRegsImg("Image 4", null));
                                Global.ParaCommon.matRegister4 = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                                Global.listRegsImg[3].Image = Global.ParaCommon.matRegister4;
                                break;
                        }  
                    }    
                    Global.Config.IsOnLight = false;
                    Global.Comunication.Protocol.IO_Processing = IO_Processing.None;
                   Global.Comunication.Protocol.IO_Processing = IO_Processing.Light;
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
                case StatusProcessing.Waiting:
                    G.StatusDashboard.StatusText = obj.ToString();
                    G.StatusDashboard.StatusBlockBackColor = Global.ColorNone;
                    
                    Global.StatusProcessing = StatusProcessing.Read;
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
                    Global.Comunication.Protocol.IsLogic1 = false;
                    Global.Comunication.Protocol.IsLogic2 = false;
                    Global.Comunication.Protocol.IsLogic3 = false;
                    Global.Comunication.Protocol.IsLogic4 = false;
                    Global.Comunication.Protocol.IsLogic5 = false;
                    Global.Comunication.Protocol.IsLogic6 = false;
                    foreach (int ix in Global.ParaCommon.indexLogic1)
                        if (ix < BeeCore.Common.PropetyTools[Global.IndexChoose].Count())
                        {
                            if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG && Global.Config.IsONNG == true)
                            {
                                Global.Comunication.Protocol.IsLogic1 = true;
                                break;
                            }
                            else if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.OK && Global.Config.IsONNG == false)
                            {
                                Global.Comunication.Protocol.IsLogic1 = true;
                                break;
                            }
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic2)
                        if (ix < BeeCore.Common.PropetyTools[Global.IndexChoose].Count())
                        {
                            if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG && Global.Config.IsONNG == true)
                            {
                                Global.Comunication.Protocol.IsLogic2 = true;
                                break;
                            }
                            else if(BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.OK && Global.Config.IsONNG == false)
                            {
                                Global.Comunication.Protocol.IsLogic2 = true;
                                break;
                            }
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic3)
                        if (ix < BeeCore.Common.PropetyTools[Global.IndexChoose].Count())
                        {
                            if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG && Global.Config.IsONNG == true)
                            {
                                Global.Comunication.Protocol.IsLogic3 = true;
                                break;
                            }
                            else if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.OK && Global.Config.IsONNG == false)
                            {
                                Global.Comunication.Protocol.IsLogic3 = true;
                                break;
                            }
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic4)
                        if (ix < BeeCore.Common.PropetyTools[Global.IndexChoose].Count())
                        {
                            if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG && Global.Config.IsONNG == true)
                            {
                                Global.Comunication.Protocol.IsLogic4 = true;
                                break;
                            }
                            else if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.OK && Global.Config.IsONNG == false)
                            {
                                Global.Comunication.Protocol.IsLogic4 = true;
                                break;
                            }
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic5)
                        if (ix < BeeCore.Common.PropetyTools[Global.IndexChoose].Count())
                        {
                            if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG && Global.Config.IsONNG == true)
                            {
                                Global.Comunication.Protocol.IsLogic5 = true;
                                break;
                            }
                            else if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.OK && Global.Config.IsONNG == false)
                            {
                                Global.Comunication.Protocol.IsLogic5 = true;
                                break;
                            }
                        }
                    foreach (int ix in Global.ParaCommon.indexLogic6)
                        if (ix < BeeCore.Common.PropetyTools[Global.IndexChoose].Count())
                        {
                            if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.NG && Global.Config.IsONNG == true)
                            {
                                Global.Comunication.Protocol.IsLogic6 = true;
                                break;
                            }
                            else if (BeeCore.Common.PropetyTools[Global.IndexChoose][ix].Results == Results.OK && Global.Config.IsONNG == false)
                            {
                                Global.Comunication.Protocol.IsLogic6 =  true;
                                break;
                            }
                        }
                    Global.Comunication.Protocol.IO_Processing = IO_Processing.Result;

                    if (Global.TotalOK==Results.OK)
                    {
                        G.StatusDashboard.StatusText = "OK";
                        G.StatusDashboard.StatusBlockBackColor = Global.ParaShow.ColorOK;
                        if(!Global.IsDummy||Global.IsAutoTemp)
                        Global.Config.SumOK++;

                        //Global.TotalOK = Results.None;
                    }
                    else if (Global.TotalOK == Results.NG)
                    {
                        G.StatusDashboard.StatusText = "NG";
                        G.StatusDashboard.StatusBlockBackColor = Global.ParaShow.ColorNG;
                         if (!Global.IsDummy || Global.IsAutoTemp)
                            Global.Config.SumNG++;
                        //Global.TotalOK = Results.None;

                    }
                    else if (Global.TotalOK == Results.Wait)
                    {
                        G.StatusDashboard.StatusText = "Wait";
                        G.StatusDashboard.StatusBlockBackColor = Global.ParaShow.ColorNone;
                        //Global.TotalOK = Results.None;

                    }
                    // G.StatusDashboard.Refresh();
                    if (Global.Comunication.Protocol.IsBypass)
                        Global.StatusProcessing = StatusProcessing.Drawing;
                    break;
                case StatusProcessing.Drawing:
                    if(!Global.ParaCommon.IsExternal)
                    this.Invoke((Action)(() =>
                    {
                        imgView.Text = "Waiting Show Picture ..";
                    }));
                    Global.IsAllowReadPLC = true;
                    timer.Split("W");
                    CTTotol = timer.StopAndFormat();
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
                        if (Global.IsAutoTemp&&Global.TotalOK!=Results.Wait)
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
                        if(Global.Config.NumTrig>Global.NumProgFromPLC)
                        {
                            Global.IndexChoose = 0;
                            Global.TriggerNum = TriggerNum.Trigger0;
                        }    
                        break;
                    }
            }
        }

        private void Global_TypeCropChanged(TypeCrop obj)
        {
            Global.StatusDraw = StatusDraw.Edit;
            imgView.Invalidate();
        }

        private void Global_StatusDrawChanged(StatusDraw obj)
        {
            this.Invoke((Action)(() =>
            {
                if (obj == StatusDraw.Color)
                {
                    imgView.PanMode = ImageBoxPanMode.None;
                    imgView.AllowClickZoom = false;
                }
                else
                {
                    if (btnPan.IsCLick)
                        imgView.PanMode = ImageBoxPanMode.Left;
                    imgView.AllowClickZoom = true;
                }

                //if (Global.StatusDraw != StatusDraw.None)
                imgView.Invalidate();
            }));
        }
   
        private void Global_IndexToolChanged(int obj)
        {
            this.Invoke((Action)(() =>
            {
                try
                {
                    if (!Global.IsEditTool) return;
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
                        toolEdit = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Control;
                          toolEdit.Enabled = false;
                         tmEnableControl.Enabled = true;
                       Global.EditTool.RegisTer(name, toolEdit);
                        //if (!Global.EditTool.pEditTool.Show(name))
                        //{
                        //    editTool.pEditTool.Register(name, () => toolEdit);
                        //    editTool.pEditTool.Show(name);
                        //}

                        //control.Size =Global.EditTool. pEditTool.Size;
                        //control.Location = new Point(0, 0);

                        // control.BringToFront();
                        // DataTool.LoadPropety(control);

                        TypeTool TypeTool = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].TypeTool;
                        Global.EditTool.iconTool.Visible = true;
                        Global.EditTool.layInforTool.Visible = true;
                        Global.EditTool.iconTool.BackgroundImage = Global.itemNews[Global.itemNews.FindIndex(a => a.TypeTool == TypeTool)].Icon;// (Image)Properties.Resources.ResourceManager.GetObject(TypeTool.ToString());
                        Global.EditTool.lbTool.Text = TypeTool.ToString();
                        Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                        BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Control.Propety = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety;
                        BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Control.LoadPara();
                        Global.EditTool.View.imgView.Invalidate();
                        Global.EditTool.View.imgView.Update();

                       // Global.EditTool.View.toolEdit = controlEdit;
                        if (!Global.IsLive)
                            Global.Config.SizeCCD = new Size(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Width, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Height);

                        ShowTool.Full(imgView, Global.Config.SizeCCD);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }));

        }
        bool IsSaving = false;
        private async void KeyboardListener_s_KeyEventHandler1(object sender, EventArgs e)
        {
            if (Global.Config.Users != Users.Admin)
                return;
            if (!Global.IsRun)
                return;
            KeyboardListener.UniversalKeyEventArgs eventArgs = (KeyboardListener.UniversalKeyEventArgs)e;
                if ( IsKeyDown(Keys.Control))
                {
                    if (eventArgs.KeyCode == Keys.S&&! IsSaving)
                {
                    IsSaving = true;
                    G.Header.pEdit.btnSave.Enabled = false;
                    using (var dlg = new SaveProgressDialog("Save Program"))
                    {
                        dlg.SetStatus("Saving Program " + Global.Project + "...", "Writing data to file...");
                        dlg.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - dlg.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - dlg.Height / 2);
                        dlg.Show(this);          // modeless
                        dlg.BringToFront();

                        try
                        {
                            await Task.Run(() =>
                            {
                                SaveData.Project(Global.Project);
                            });

                            if (dlg.CancelRequested)
                            {
                                dlg.SetStatus("Cancelled", "You have cancelled the save operation.");
                                dlg.MarkCompleted("Cancelled", "No data was written.");
                            }
                            else
                            {
                                IsSaving = false;
                                G.Header.pEdit.btnSave.Enabled = true;
                                dlg.MarkCompleted("Save completed", "Program " + Global.Project);
                            }
                        }
                        catch (Exception ex)
                        {
                            IsSaving = false;
                            dlg.SetStatus("Save error", ex.Message);
                            dlg.MarkCompleted("Error", "Please click OK to close.");
                        }

                    }
                }


            }
        }
           
        

        private void Common_FrameChanged(object sender, PropertyChangedEventArgs e)
        {
            Global.EditTool.lbFrameRate.Text = Global.Config.SizeCCD.ToString()+"-"+ BeeCore.Common.listCamera[Global.IndexCCCD].FrameRate+" img/s ";
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
        public void SaveDumy(Results Results)
        {
            String patDummy;
            String date = DateTime.Now.ToString("yyyyMMdd");
            String Hour = DateTime.Now.ToString("HHmmss");
            patDummy = "Dummy//" + date ;
           
            if (!Directory.Exists(patDummy))
                Directory.CreateDirectory(patDummy);
           
            String FileRaw = patDummy + "//DummyRaw_" + Global.Project + "_" + Global.Config.POCurrent  + "_" + Hour + "_" + Results.ToString() + ".png";
            String FileResult = patDummy + "//DummyRs_" + Global.Project + "_" + Global.Config.POCurrent + "_" + Hour + "_" + Results.ToString() + ".png";
            if (Global.listMatRaw[0] == null)
                return;
            if (Global.listMatRaw[0].IsDisposed)
                return;
            if (Global.NumProgFromPLC == 2)
            {
                if (Global.listMatRaw[1] == null)
                    return;
                if (Global.listMatRaw[1].IsDisposed)
                    return;
            }
            MatMergerOptions opt = new MatMergerOptions();
            opt.Direction = MergeDirection.Vertical;
            opt.SizeMode = SizeMode.ResizeToMatch;
           
            if (Global.NumProgFromPLC == 2)
            {
                using (Mat rs = MatMerger.Merge(Global.listMatRaw[0], Global.listMatRaw[1], opt))
                    Cv2.ImWrite(FileRaw, rs);
            }
            else
                Cv2.ImWrite(FileRaw, Global.listMatRaw[0]);
           
            if (Global.NumProgFromPLC == 2)
            {
                using (Mat rs = MatMerger.Merge(Global.listMatRs[0], Global.listMatRs[1], opt))
                    Cv2.ImWrite(FileResult, rs);
            }
            else
                Cv2.ImWrite(FileResult, Global.listMatRs[0]);
           

        }
        public void SQL_Insert(DateTime time, String model, int Qty, int Total,  Results Results)
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
               
                if (!Directory.Exists(pathRaw))
                   Directory.CreateDirectory(pathRaw);
                if (!Directory.Exists(pathRS))
                       Directory.CreateDirectory(pathRS);
                String FileRaw = pathRaw + "//" + Global.Project + "_" + Global.Config.POCurrent + "_" + 1 + "_" + Hour + "_" + Results.ToString() + ".png";
                String FileRaw2 = pathRaw + "//" + Global.Project + "_" + Global.Config.POCurrent + "_" + 2 + "_" + Hour + "_" + Results.ToString() + ".png";
                String FileResult = pathRS + "//" + Global.Project + "_" + Global.Config.POCurrent + "_" + Hour + "_" + Results.ToString() + ".png";
                if (Results == Results.OK && Global.Config.IsSaveOK || Results == Results.NG && Global.Config.IsSaveNG)
                {
                    if (Global.listMatRaw[0] == null )
                        return;
                    if (Global.listMatRaw[0].IsDisposed )
                        return;
                    if (Global.NumProgFromPLC==2)
                    {
                        if ( Global.listMatRaw[1] == null)
                            return;
                        if (Global.listMatRaw[1].IsDisposed)
                            return;
                    }
                    
                  
                    MatMergerOptions opt = new MatMergerOptions();
                    opt.Direction = MergeDirection.Vertical;
                    opt.SizeMode = SizeMode.ResizeToMatch;
                    if (Global.Config.IsSaveRaw)
                    {
                        Cv2.ImWrite(FileRaw, Global.listMatRaw[0]);
                        if (Global.NumProgFromPLC == 2)
                            Cv2.ImWrite(FileRaw2, Global.listMatRaw[1]);

                    }
                    if (Global.Config.IsSaveRS)
                    {
                        if (Global.NumProgFromPLC == 2)
                        {
                            using (Mat rs = MatMerger.Merge(Global.listMatRs[0], Global.listMatRs[1], opt))
                                Cv2.ImWrite(FileResult, rs);
                        }
                        else
                            Cv2.ImWrite(FileResult, Global.listMatRs[0]);

                    }



               
                    
                }
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = G.cnn;
                    command.CommandText = "INSERT into Report (Date,Prog,PO,Status,Raw,Result) VALUES(@Date,@Prog,@PO,@Status,@Raw,@Result)";
                    command.Prepare();
                    command.Parameters.Add("@Date", SqlDbType.DateTime).Value = DateTime.Now;             
                    command.Parameters.AddWithValue("@Prog", Global.Project);
                    command.Parameters.AddWithValue("@PO", Global.Config.POCurrent);
                    command.Parameters.AddWithValue("@Status", ResultsSave.ToString()); 
                    command.Parameters.AddWithValue("@Raw", FileRaw); 
                    command.Parameters.AddWithValue("@Result", FileResult);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    
                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "InsertData", ex.Message));
                  
                }
              
            }
            catch (Exception ex)
            {
                
                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "InsertData", ex.Message));
               
            }
       
           

        }
      
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
               
                //if(Global.Config.IsMultiProg)
                //{
                //    _renderer.ClearImages();
                //    int index = 0;
                //    foreach (Camera camera in BeeCore.Common.listCamera)
                //    {
                //        if (camera == null) continue;
                //        camera.DrawResult();
                //        if (index == 1)
                //            _renderer.AddImage(camera.bmResult, FillMode1.Contain, 0.3f);
                //        else
                //            _renderer.AddImage(camera.bmResult, FillMode1.Contain, 1);
                //        index++;
                //    }

                //}
              //  else
                {
                    switch (Global.Config.NumTrig)
                    {
                        case 1:
                            _renderer.LayoutPreset = CollageLayout.One;

                            break;
                        case 2:
                            _renderer.LayoutPreset = CollageLayout.Two;
                            _renderer.Orientation = LayoutOrientation.ForceVertical;
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
                   // Results results = Global.TotalOK==true ? Results.OK : Results.NG;
                    switch (Global.IndexChoose)
                    {
                        
                        case 0:
                            Global.ToolSettings.Labels[0].Results = Global.ListResult[Global.IndexChoose];
                            Global.ToolSettings.Labels[0].BackColor = Color.FromArgb(246, 204, 120);
                            Global.ToolSettings.Labels[1].BackColor = Color.LightGray;
                            Global.ToolSettings.Labels[2].BackColor = Color.LightGray;
                            Global.ToolSettings.Labels[3].BackColor = Color.LightGray;
                            if (_renderer.Count() < 1)
                                _renderer.AddImage(BeeCore.Common.listCamera[0].bmResult, FillMode1.Contain);
                            else
                                _renderer.ModifyImage(0, BeeCore.Common.listCamera[0].bmResult, FillMode1.Contain);
                                break;
                        case 1:
                            Global.ToolSettings.Labels[1].Results = Global.ListResult[Global.IndexChoose];
                            Global.ToolSettings.Labels[1].BackColor = Color.FromArgb(246, 204, 120);
                            Global.ToolSettings.Labels[0].BackColor = Color.LightGray;
                            Global.ToolSettings.Labels[2].BackColor = Color.LightGray;
                            Global.ToolSettings.Labels[3].BackColor = Color.LightGray;
                            if (_renderer.Count() < 2)
                                _renderer.AddImage(BeeCore.Common.listCamera[0].bmResult, FillMode1.Contain);
                            else
                                _renderer.ModifyImage(1, BeeCore.Common.listCamera[0].bmResult, FillMode1.Contain);
                            break;
                        case 2:
                            Global.ToolSettings.Labels[2].Results = Global.ListResult[Global.IndexChoose];
                            Global.ToolSettings.Labels[2].BackColor = Color.FromArgb(246, 204, 120);
                            Global.ToolSettings.Labels[1].BackColor = Color.LightGray;
                            Global.ToolSettings.Labels[0].BackColor = Color.LightGray;
                            Global.ToolSettings.Labels[3].BackColor = Color.LightGray;
                            if (_renderer.Count() < 3)
                                _renderer.AddImage(BeeCore.Common.listCamera[0].bmResult, FillMode1.Contain);
                            else
                                _renderer.ModifyImage(2, BeeCore.Common.listCamera[0].bmResult, FillMode1.Contain);
                            break;
                        case 3:
                            Global.ToolSettings.Labels[3].Results = Global.ListResult[Global.IndexChoose];
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
                ResultsSave = Global.TotalOK;
                if(Global.IsDummy&& ResultsSave!=Results.Wait)
                {
                    if (!workInsert.IsBusy)
                        workInsert.RunWorkerAsync();
                    else

                        Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Data", "Fail Save Data NG"));
                    Global.TotalOK = Results.None;
                }
                else if (!Global.IsAutoTemp)
                {
                    if (Global.TotalOK == Results.OK)
                    {
                        if (Global.Config.IsSaveOK)
                        {
                            if (!workInsert.IsBusy)
                            {
                                workInsert.RunWorkerAsync();

                            }

                            else

                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Data", "Fail Save Data OK"));
                        }

                        Global.TotalOK = Results.None;
                    }
                    else if (Global.TotalOK == Results.NG)
                    {
                        if (Global.Config.IsSaveNG)
                        {
                            if (!workInsert.IsBusy)
                                workInsert.RunWorkerAsync();
                            else

                                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Data", "Fail Save Data NG"));
                        }
                        Global.TotalOK = Results.None;
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
            if (Global.Comunication.Protocol.IsBypass)
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
            

            if (!Global.Comunication.Protocol.IsConnected&&!Global.Comunication.Protocol.IsBypass )
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
            if (Global.Comunication.Protocol.IsBypass)
            {
                switch(Global.TriggerNum)
                {
                    case TriggerNum.Trigger0:
                        Global.TriggerNum=TriggerNum.Trigger1;
                        Global.IndexChoose = 0;
                        break;
                    case TriggerNum.Trigger1:
                        Global.TriggerNum = TriggerNum.Trigger2;
                        Global.IndexChoose = 1;
                        break;
                    case TriggerNum.Trigger2:
                        Global.TriggerNum = TriggerNum.Trigger3;
                        Global.IndexChoose = 2;
                        break;
                    case TriggerNum.Trigger3:
                        Global.TriggerNum = TriggerNum.Trigger4;
                        Global.IndexChoose = 3;
                        break;
                }    
                Global.StatusProcessing = StatusProcessing.Read;
            }    
               
            else
            {
                if (Global.Config.IsMultiProg)
                {


                    switch (Global.TriggerNum)
                    {
                        case TriggerNum.Trigger0:
                            Global.TriggerNum = TriggerNum.Trigger1;
                            Global.IndexChoose = 0;
                            break;
                        case TriggerNum.Trigger1:
                            Global.TriggerNum = TriggerNum.Trigger2;
                            Global.IndexChoose = 1;
                            break;
                        case TriggerNum.Trigger2:
                            Global.TriggerNum = TriggerNum.Trigger3;
                            Global.IndexChoose = 2;
                            break;
                        case TriggerNum.Trigger3:
                            Global.TriggerNum = TriggerNum.Trigger4;
                            Global.IndexChoose = 3;
                            break;
                    }
                    Global.TriggerInternal = true;
                }
                else
                {
                    Global.IndexChoose = 0;
                    Global.TriggerInternal = true;
                }    
               
            }    
               
            //  BeeCore.Common.currentTrig++;

        }

        private async void btnRecord_Click(object sender, EventArgs e)
        {
          
            if (!Global.Comunication.Protocol.IsConnected && !Global.Comunication.Protocol.IsBypass)
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

            if (Global.Comunication.Protocol.IsConnected)
            {
                if (Global.Comunication.Protocol.IsConnected)
                {
                    tmContinuous.Enabled = btnContinuous.IsCLick;
                   
                    return;
                }
            } 
            else if(Global.Comunication.Protocol.IsBypass)
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
            if (BeeCore.Common.listCamera[Global.IndexCCCD]!= null)
                if (BeeCore.Common.listCamera[Global.IndexCCCD].matRaw!=null)
                if (!BeeCore.Common.listCamera[Global.IndexCCCD].matRaw .IsDisposed)
                    if (!BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Empty())
                    {
                        BeeCore.Common.listCamera[Global.IndexCCCD].Read();
                        imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                        Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexCCCD].GetSzCCD();
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
                //if (BeeCore.Common.listCamera[Global.IndexCCCD] != null)
                //    if (BeeCore.Common.listCamera[Global.IndexCCCD].matRaw != null)
                //        if (!BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.IsDisposed)
                //            if (!BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Empty())
                //            {
                //                BeeCore.Common.listCamera[Global.IndexCCCD].Read();
                //                imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                //                Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexCCCD].GetSzCCD();
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
               
                BeeCore.Common.listCamera[Global.IndexCCCD].GetFpsPylon();
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
                if (BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown) return;
                try
                {
                    BeeCore.Common.listCamera[Global.IndexCCCD].Read();
                }
                  
                catch(Exception)
                {

                }
            }
            else if(Global.IsRun&&Global.StatusProcessing==StatusProcessing.Read)
            {
                if (Global.Config.IsMultiProg)
                {
                    //foreach(List<PropetyTool> propetyTool in BeeCore.Common.PropetyTools)
                    //{
                    //    if (BeeCore.Common.listCamera[propetyTool[0].IndexCamera] == null) continue;
                    //    BeeCore.Common.listCamera[propetyTool[0].IndexCamera].Read();
                    //    if (BeeCore.Common.listCamera[propetyTool[0].IndexCamera].Para.TypeCamera == TypeCamera.USB)
                    //        BeeCore.Common.listCamera[propetyTool[0].IndexCamera].Read();
                    //}    
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

                if (BeeCore.Common.listCamera[Global.IndexCCCD].matRaw != null)
                    if (!BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.IsDisposed)
                      if (!BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Empty())
                        {
                        Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexCCCD].GetSzCCD();
                        // matRaw là OpenCvSharp.Mat
                        var bmp = BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw);

                        // Đẩy frame mới nhất và hủy frame cũ một cách an toàn, không cần lock
                        var old = Interlocked.Exchange(ref _sharedFrame, bmp);
                        old?.Dispose();

                        // (tuỳ chọn) báo cho display thread là có frame mới
                        _frameReady?.Set();
                        //using (Bitmap frame = BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw))
                        //{

                        //        _sharedFrame?.Dispose();
                        //        _sharedFrame = (Bitmap)frame.Clone(); // Clone để thread-safe

                        //}
                    }

                if (BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown)
                    await TimingUtils.DelayAccurateAsync(5);
                if (BeeCore.Common.listCamera[Global.IndexCCCD].IsSetPara)
                    await TimingUtils.DelayAccurateAsync(5);
                
                workReadCCD.RunWorkerAsync();
                return;
            }

			if (Global.StatusMode == StatusMode.Continuous || Global.StatusMode == StatusMode.Once)
			{
              
                Global.StatusProcessing = StatusProcessing.Checking;
                if (Global.IsByPassResult)
                    Global.Comunication.Protocol.IO_Processing = IO_Processing.ByPass;

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
            //    //  BeeCore.Common.listCamera[Global.IndexCCCD].Read(Global.Config.IsHist );
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
            using (Mat raw = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone())
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
           
              BeeCore.Common.listCamera[Global.IndexCCCD].Read();
           // BeeCore.Common.CalHist();
        }
        Results ResultsSave = Results.None;
        private void workInsert_DoWork(object sender, DoWorkEventArgs e)
            {if(Global.IsDummy)
                {
                    SaveDumy(ResultsSave);
                }
            else
            {
                if (G.cnn.State == ConnectionState.Closed)
                    G.ucReport.Connect_SQL();
                if (Global.TotalOK == Results.OK)
                    SQL_Insert(DateTime.Now, Properties.Settings.Default.programCurrent.Replace(".prog", ""), Global.Config.SumOK, Global.Config.SumOK + Global.Config.SumNG, ResultsSave);
                else
                    SQL_Insert(DateTime.Now, Properties.Settings.Default.programCurrent.Replace(".prog", ""), Global.Config.SumOK, Global.Config.SumOK + Global.Config.SumNG, ResultsSave);

            }

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
                BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = listMat[indexFile]; ;// Cv2.ImRead(Files[indexFile]);
                Native.SetImg(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
            }
        }
   
        public float PictureScale = 1.0f;
       
        private void DrawImage(Graphics gr)
        {
            gr.DrawImage(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap(),new PointF(0,0));
           
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
                if (BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.IsDisposed) return;
                BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = true;
                await TimingUtils.DelayAccurateAsync(10);
                Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexCCCD].GetSzCCD();
                if (Global.IsLive)
                    BeeCore.Common.listCamera[Global.IndexCCCD].IsMouseDown = false;

            }
            else
                Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexCCCD].GetSzCCD();
        
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
            if (BeeCore.Common.listCamera[Global.IndexCCCD].matRaw == null) return;
            saveFileDialog.Filter = " PNG|*.png";
          if (  saveFileDialog.ShowDialog()==DialogResult.OK)
            {
                Cv2.ImWrite(saveFileDialog.FileName, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw);
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
            //if (Global.Comunication.Protocol.IsConnected)
            //    Global.Comunication.IO.WriteInPut(0, true);//.  BtnWriteInPLC((RJButton)sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Global.Config.IsShowCenter = btnShowCenter.IsCLick;
            if (!Global.IsLive)
            Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexCCCD].GetSzCCD();
            imgView.Invalidate();
        }

        private void workInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ResultsSave = Results.None;
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
                if (BeeCore.Common.listCamera[Global.IndexCCCD].matRaw != null)
                    if (!BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Empty())
                        BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Release();
                Files .Add( openFile.FileName);
                BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = Cv2.ImRead(Files[indexFile]);
                listMat = new List<Mat>();
                listMat.Add(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
               Native.SetImg(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
            
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

                if (!BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Empty())
                        BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Release();
                 BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = listMat[indexFile].Clone();

                if (BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Empty()) goto X;
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
            BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = listMat[indexFile]; ;// Cv2.ImRead(Files[indexFile]);
            Native.SetImg(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
            imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
            if (Global.IsRun)
            {
                Global.StatusMode = StatusMode.SimOne;
                RunProcessing();
               
            }

        
          
           
        }

        private void btnDeleteFile_Click(object sender, EventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexCCCD].Setting();
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
                Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexCCCD].GetSzCCD();
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

            //     matCrop.CopyTo(new Mat(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw, new Rect((int)rot._PosCenter.X + (int)rot._rect.X, (int)rot._PosCenter.Y + (int)rot._rect.Y, (int)rot._rect.Width, (int)rot._rect.Height)));
            //     imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();


            //     DelayTrig = Propety.DelayTrig;
            //     tmTrig.Interval = 1;
            //     tmTrig.Enabled = true;
            //     return;
            // }
            // else if (G.StatusTrig==Trig.Trigged)
            // {

            //     BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = BeeCore.Native.GetImg();
            //     imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
            //     tmTrig.Enabled = true;
            //     tmTrig.Interval = DelayTrig;
            //     return;
            // }
            // else if (G.StatusTrig == Trig.Complete)
            // {

            //    // BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = BeeCore.Common.GetImageRaw();
            //     //imgView.ImageIpl = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw;
            //     tmTrig.Enabled = true;
            //     tmTrig.Interval = DelayTrig;
            //     return;
            // }


        }
        StatusProcessing Processing1 = StatusProcessing.None;
		StatusProcessing Processing2 = StatusProcessing.None;
		StatusProcessing Processing3 = StatusProcessing.None;
		StatusProcessing Processing4 = StatusProcessing.None;
        public async Task CheckStatus(int index)
        {
            await Task.Run(async () =>
            {
                // simple delay inside loop to avoid 100% CPU spin
                while (!_cts.Token.IsCancellationRequested)
                {
                    switch(index)
                    {
                        case 0:
                            if (Processing1 == StatusProcessing.Done)
                                return;
                            break;
                        case 1:
                            if (Processing2 == StatusProcessing.Done)
                                return;
                            break;
                        case 2:
                            if (Processing3 == StatusProcessing.Done)
                                return;
                            break;
                        case 3:
                            if (Processing4== StatusProcessing.Done)
                                return;
                            break;
                    }
                   // if (Global.Config.IsMultiProg == false)
                    {
                       
                    }
                    //else
                    //{
                    //    if (Processing1 == StatusProcessing.Done
                    //     && Processing2 == StatusProcessing.Done
                    //     && Processing3 == StatusProcessing.Done
                    //     && Processing4 == StatusProcessing.Done)
                    //    {

                    //        return;    // exit the Task.Run delegate
                    //    }
                    //}
                }
            }, _cts.Token);

            if(Global.Config.IsAutoTrigger)
            if (BeeCore.Common.PropetyTools[0][0].Results == Results.NG )
            {
                Global.StatusProcessing = StatusProcessing.Waiting;
                return;
            }

            //   timer.Stop();
            // Global.TotalOK = true;

            //if (BeeCore.Common.PropetyTools[0][0].Results == Results.NG&&Global.StatusMode==StatusMode.Continuous)
            //{
            //    Global.StatusProcessing = StatusProcessing.Waiting;
            //    return;
            //}

            //   timer.Stop();
            //     Global.TotalOK = true;
            Global.ListResult[Global.IndexChoose]= BeeCore.Common.listCamera[Global.IndexCCCD].SumResult();
          
            if (Global.IndexChoose != Global.Config.NumTrig - 1)
            {   
               if(Global.IndexChoose+1==Global.NumProgFromPLC)
                {
                    Global.TotalOK = Global.ListResult[Global.IndexChoose];
                }
                else
                    Global.TotalOK = Results.Wait;

            }    
               
            else
            {
                Global.TotalOK = Results.OK;
                foreach (Results results in Global.ListResult)
                {if(results==Results.NG)
                    {
                        Global.TotalOK = Results.NG;
                        break;
                    }    

                }
            }    
                
            //foreach ( Camera camera in BeeCore.Common.listCamera)
            //{if (camera == null)
            //        continue;
            //   camera.SumResult();
            //    if(camera.Results==Results.NG)
            //    {
            //        Global.TotalOK = false;
            //        break;
            //    }    
            //}
            if (Global.IsByPassResult)
                Global.StatusProcessing = StatusProcessing.Drawing;
            else
                Global.StatusProcessing = StatusProcessing.SendResult;
        
        }
        public async void RunProcessing()
        {
            switch(Global.IndexChoose)
            {
                case 0:
                    if (BeeCore.Common.PropetyTools[Global.IndexChoose] != null)
                    {
                        Checking1.StatusProcessing = StatusProcessing.None;
                        Checking2.indexThread = 0;
                        Checking1.Start();
                    }
                    else
                        Processing1 = StatusProcessing.Done;
                    break;
                case 1:
                    if (BeeCore.Common.PropetyTools[Global.IndexChoose] != null)
                    {
                        Checking2.StatusProcessing = StatusProcessing.None;
                        Checking2.indexThread = 1;
                        Checking2.Start();
                    }
                    else
                        Processing2 = StatusProcessing.Done;
                    break;
                case 2:
                    if (BeeCore.Common.PropetyTools[Global.IndexChoose] != null)
                    {
                        Checking3.StatusProcessing = StatusProcessing.None;
                        Checking3.indexThread = 2;
                        Checking3.Start();
                    }
                    else
                        Processing3 = StatusProcessing.Done;
                    break;
                case 3:
                    if (BeeCore.Common.PropetyTools[Global.IndexChoose] != null)
                    {
                        Checking4.StatusProcessing = StatusProcessing.None;
                        Checking4.indexThread = 3;
                        Checking4.Start();
                    }
                    else
                        Processing4 = StatusProcessing.Done;
                    break;
               
            }
            await CheckStatus(Global.IndexChoose);
            //if (BeeCore.Common.PropetyTools[Global.IndexChoose] != null)
            //{
            //    Checking1.StatusProcessing = StatusProcessing.None;
            //    Checking1.Start();
            //}
            //else
            //    Processing1 = StatusProcessing.Done;

            //if (Global.Config.IsMultiProg)
            //{
            //    if (BeeCore.Common.PropetyTools[0] != null)
            //    {
            //        Checking1.StatusProcessing = StatusProcessing.None;
            //        Checking1.Start();
            //    }
            //    else
            //        Processing1 = StatusProcessing.Done;

            //    if (Global.Config.IsMultiProg == false)
            //    {
            //        await CheckStatus();
            //        return;
            //    }
            //    if (BeeCore.Common.PropetyTools[1] != null)
            //    {
            //        Checking2.StatusProcessing = StatusProcessing.None;
            //        Checking2.Start();
            //    }
            //    else
            //        Processing2 = StatusProcessing.Done;

            //    if (BeeCore.Common.PropetyTools[2] != null)
            //    {
            //        Checking3.StatusProcessing = StatusProcessing.None;
            //        Checking3.Start();
            //    }
            //    else
            //        Processing3 = StatusProcessing.Done;
            //    if (BeeCore.Common.PropetyTools[3] != null)
            //    {
            //        Checking4.StatusProcessing = StatusProcessing.None;
            //        Checking4.Start();
            //    }
            //    else
            //        Processing4 = StatusProcessing.Done;
            //}
            //else
            //{
            //    if (BeeCore.Common.listCamera[0] != null)
            //    {
            //        Checking1.StatusProcessing = StatusProcessing.None;
            //        Checking1.Start();
            //    }
            //    else
            //        Processing1 = StatusProcessing.Done;

            //}
            //    await CheckStatus();


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
           if(Global.Comunication.Protocol.IsConnected)
                Global.EditTool.toolStripPort.Image = Properties.Resources.PortConnected;
           else
                Global.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;

        }

        private void tmEnableControl_Tick(object sender, EventArgs e)
        {
            this.Invoke((Action)(() =>
            {
                toolEdit.Enabled = true;
                tmEnableControl.Enabled = false;
            }));
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
                Global.Config.SizeCCD = BeeCore.Common.listCamera[Global.IndexCCCD].GetSzCCD();
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
            Global.ParaShow.IsShowMatProcess = showImageFilter.Checked;
            Global.StatusDraw = StatusDraw.Check;
            imgView.Invalidate();
        }

        private void showDetailTool_Click(object sender, EventArgs e)
        {
            Global.ParaShow.IsShowDetail = showDetailTool.Checked; 
            Global.StatusDraw = StatusDraw.Check;
            imgView.Invalidate();
        }

        private void showResultTool_Click(object sender, EventArgs e)
        {
            Global.ParaShow.IsShowResult = showResultTool.Checked;
            Global.StatusDraw = StatusDraw.Check;
            imgView.Invalidate();
        }

        private void showDetailWrong_Click(object sender, EventArgs e)
        {
            Global.ParaShow.IsShowNotMatching = showDetailWrong.Checked;
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
                BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = listMat[indexFile];// Cv2.ImRead(Files[indexFile]);
                imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
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
