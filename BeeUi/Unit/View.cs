using BeeCore;

using BeeUi.Common;
using BeeUi.Commons;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Timer = System.Windows.Forms.Timer;
using BeeCore.Funtion;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Timers;
using System.Windows.Markup;
using BeeUi.Data;
using System.Web;
using BeeGlobal;
using BeeInterface;
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
              public View()
        {
            InitializeComponent();
            this.BackColor = Color.Transparent;
            KeyboardListener.s_KeyEventHandler += new EventHandler(KeyboardListener_s_KeyEventHandler);
            tmKey.Tick += TmKey_Tick;
            tmKey.Interval = 50;
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

            toolEdit.matTemp = toolEdit.Propety.ClearTemp();
           
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
        public PointF RotatePoint(float angle, PointF pt)
        {
            var a = angle * System.Math.PI / 180.0;
            float cosa =(float) Math.Cos(a), sina =(float) Math.Sin(a);
            return new PointF((float)pt.X * cosa - pt.Y * sina, (float)pt.X * sina + pt.Y * cosa);
        }
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
        Color clChoose;
        private void imgView_MouseMove(object sender, MouseEventArgs e)
        {
            pMove = e.Location;
                if (Global.IsRun) return;
            if (toolEdit != null)
                if(Global.IndexToolSelected>=0)
                if (BeeCore. Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].TypeTool == TypeTool.Color_Area)
            {
              
                    if (toolEdit.Propety.IsGetColor)
                        imgView.Cursor = new Cursor(Properties.Resources.Color_Dropper.Handle);
                    else
                        imgView.Cursor = Cursors.Default;
                    if (toolEdit.Propety.IsGetColor)
                    {
                        if(!workGetColor.IsBusy)
                        workGetColor.RunWorkerAsync();
                        return;
                    }
                }

            try
            {
                if (toolEdit == null) return;

                RectRotate rotateRect = new RectRotate();
                if (toolEdit.IsClear)
                {
                    if (_drag)
                    {

                        rectClear = new Rect(e.Location.X - widthClear/2, e.Location.Y + widthClear / 4, widthClear, widthClear);
                      Mat  bmp = new Mat(rectClear.Width, rectClear.Height, MatType.CV_8UC1, Scalar.White);
                        bmp.CopyTo(new Mat(matMaskAdd, rectClear));
                        bmp.CopyTo(new Mat(bmMask, rectClear));
                      
                      
                    }

                }

         
                pDown = e.Location;
                
                //this.Cursor = Cursors.Default;
            
                    if (_drag)
                    {
                        if (Global.TypeCrop == TypeCrop.Area)
                        {

                            // if(toolEdit.Propety.rotCrop._rectRotation.Contains(toolEdit.Propety.rotArea._rectRotation))
                            if (toolEdit.Propety.rotArea == null)
                                return;
                            rotateRect = new RectRotate(toolEdit.Propety.rotArea._rect, toolEdit.Propety.rotArea._PosCenter, toolEdit.Propety.rotArea._rectRotation, toolEdit.Propety.rotArea._dragAnchor, toolEdit.Propety.rotArea.IsElip);
                        }
                        else if (Global.TypeCrop == TypeCrop.Mask)
                        {

                            // if(toolEdit.Propety.rotCrop._rectRotation.Contains(toolEdit.Propety.rotArea._rectRotation))
                            if (toolEdit.Propety.rotMask == null)
                                return;
                            rotateRect = new RectRotate(toolEdit.Propety.rotMask._rect, toolEdit.Propety.rotMask._PosCenter, toolEdit.Propety.rotMask._rectRotation, toolEdit.Propety.rotMask._dragAnchor, toolEdit.Propety.rotMask.IsElip);
                        }
                        else
                        {
                            if (toolEdit.Propety.rotCrop == null)
                                return;
                            rotateRect = new RectRotate(toolEdit.Propety.rotCrop._rect, toolEdit.Propety.rotCrop._PosCenter, toolEdit.Propety.rotCrop._rectRotation, toolEdit.Propety.rotCrop._dragAnchor,toolEdit.Propety.rotCrop.IsElip);
                        }
                        var mat = new Matrix();
                   mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
                        mat.Scale((float)(imgView.Zoom / 100.0), (float)(imgView.Zoom / 100.0));

                    mat.Translate(_dragCenter.X, _dragCenter.Y);
                        mat.Rotate(rotateRect._rectRotation);
                        mat.Invert();

                        var point = mat.TransformPoint(new PointF(e.X, e.Y));

                        SizeF deltaSize=new SizeF();
                        PointF clamped;
                        float deltaX = 0, deltaY = 0;
                        switch (rotateRect._dragAnchor)
                        {
                            case AnchorPoint.TopLeft:
                                
                                IsDone = true;
                                clamped = new PointF(Math.Min(0, point.X), Math.Min(0, point.Y));
                                deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                               
                                rotateRect._rect = new RectangleF(
                                       _dragRect.Left + deltaSize.Width/2,
                                       _dragRect.Top +deltaSize.Height/2,
                                       _dragRect.Width - deltaSize.Width,
                                       _dragRect.Height - deltaSize.Height);
                                deltaX = deltaSize.Width/2; deltaY = deltaSize.Height/2;

                                break;

                            case AnchorPoint.TopRight:

                                clamped = new PointF(Math.Max(0, point.X), Math.Min(0, point.Y));
                                deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                              //  if (deltaSize.Width < 2 && deltaSize.Height < 2) return;
                                rotateRect._rect = new RectangleF(
                                    _dragRect.Left- deltaSize.Width/2,
                                    _dragRect.Top + deltaSize.Height/2,
                                    _dragRect.Width + deltaSize.Width,
                                    _dragRect.Height - deltaSize.Height);
                                deltaX = deltaSize.Width/2; deltaY = deltaSize.Height/2;
                                break;

                            case AnchorPoint.BottomLeft:

                                clamped = new PointF(Math.Min(0, point.X), Math.Max(0, point.Y));
                                deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                                rotateRect._rect = new RectangleF(
                                    _dragRect.Left + deltaSize.Width/2,
                                    _dragRect.Top - deltaSize.Height/2,
                                    _dragRect.Width - deltaSize.Width,
                                    _dragRect.Height + deltaSize.Height);
                                deltaX = deltaSize.Width/2; deltaY = deltaSize.Height/2;
                             
                                break;

                            case AnchorPoint.BottomRight:

                                clamped = new PointF(Math.Max(0, point.X), Math.Max(0, point.Y));
                                deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                                rotateRect._rect = new RectangleF(
                                    _dragRect.Left - deltaSize.Width/2,
                                    _dragRect.Top - deltaSize.Height/2,
                                    _dragRect.Width + deltaSize.Width,
                                    _dragRect.Height + deltaSize.Height);
                                deltaX = deltaSize.Width/2; deltaY = deltaSize.Height/2;
                                break;

                            case AnchorPoint.Rotation:
                               
                                var vecX = point.X;
                                var vecY = -point.Y;

                                var len = Math.Sqrt(vecX * vecX + vecY * vecY);

                                var normX = vecX / len;
                                var normY = vecY / len;

                                //In rectangles's space, 
                                //compute dot product between, 
                                //Up and mouse-position vector
                                var dotProduct = (0 * normX + 1 * normY);
                                var angle = Math.Acos(dotProduct);

                                if (point.X < 0)
                                    angle = -angle;
                            float oldAngle = rotateRect._rectRotation;
                                // Add (delta-radians) to rotation as degrees
                                float fAngle = (float)((180 / Math.PI) * angle);
                                if (!float.IsNaN(fAngle )&& fAngle!=0)
                                    rotateRect._rectRotation += fAngle;
                            if (float.IsNaN(rotateRect._rectRotation))
                                rotateRect._rectRotation = oldAngle;
                                break;

                            case AnchorPoint.Center:

                                //move this in screen-space
                                rotateRect._PosCenter = new PointF(e.X - _dragStartOffset.X, e.Y - _dragStartOffset.Y);
                                break;
                        }
                       
                        if (rotateRect._dragAnchor != AnchorPoint.None)
                        {
                          
                            if(rotateRect._dragAnchor != AnchorPoint.Center)
                            if (deltaX != 0 || deltaY != 0)
                            {
                                PointF pDelta = RotatePoint(rotateRect._rectRotation, new PointF(deltaX, deltaY));

                                rotateRect._PosCenter = new PointF(_dragCenter.X + pDelta.X, _dragCenter.Y + pDelta.Y);
                               
                                IsDone = false;
                            }
                            if (Global.TypeCrop == TypeCrop.Area)
                            {
                                float x = rotateRect._PosCenter.X - rotateRect._rect.Width/2;
                                float y = rotateRect._PosCenter.Y - rotateRect._rect.Height/2;
                                float w = rotateRect._rect.Width; float h = rotateRect._rect.Height;
                                if (x<0)
                                {

                                    rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X - x, rotateRect._PosCenter.Y);
                                }
                                else if (x + w > BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Width)
                                {

                                    rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X - (x + w - BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Width), rotateRect._PosCenter.Y);
                                }
                                  if (y < 0)
                                {

                                    rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y- y);
                                }
                               
                                else if (y + h > BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Height)
                                {

                                    rotateRect._PosCenter = new PointF(rotateRect._PosCenter.X , rotateRect._PosCenter.Y -(y+h - BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Height));
                                }
                                if (toolEdit.Propety.rotCrop != null)
                                {
                                    RectRotate rectRotate1 = toolEdit.Propety.rotCrop;
                                    RotatedRect rot = new RotatedRect(new Point2f(rectRotate1._PosCenter.X, rectRotate1._PosCenter.Y), new Size2f(rectRotate1._rect.Width, rectRotate1._rect.Height), rectRotate1._rectRotation);
                                    Rect bound = rot.BoundingRect();
                                    RectRotate rectRotate2 = rotateRect;
                                    RectangleF rectF = new RectangleF(rectRotate2._PosCenter.X + rectRotate2._rect.X, rectRotate2._PosCenter.Y + rectRotate2._rect.Y, rectRotate2._rect.Width, rectRotate2._rect.Height);

                                    if (!rectF.Contains(new PointF(bound.X, bound.Y)) ||
                                       !rectF.Contains(new PointF(bound.X + bound.Width, bound.Y)) ||
                                       !rectF.Contains(new PointF(bound.X, bound.Y + bound.Height)) ||
                                       !rectF.Contains(new PointF(bound.X + bound.Width, bound.Y + bound.Height)))
                                        return;
                                     }
                            if (rotateRect == null) return;
                            toolEdit.Propety.rotArea = new RectRotate(new RectangleF(rotateRect._rect.X, rotateRect._rect.Y, rotateRect._rect.Width, rotateRect._rect.Height), new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y), rotateRect._rectRotation, rotateRect._dragAnchor,false);

                        }
                        else if (Global.TypeCrop == TypeCrop.Crop)
                            {
                                RotatedRect rot = new RotatedRect(new Point2f(rotateRect._PosCenter.X, rotateRect._PosCenter.Y), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), rotateRect._rectRotation);
                               Rect bound= rot.BoundingRect();
                               // RectRotate rectRotate2 = toolEdit.Propety.rotArea;
                               // RectangleF rectF = new RectangleF(rectRotate2._PosCenter.X + rectRotate2._rect.X, rectRotate2._PosCenter.Y + rectRotate2._rect.Y, rectRotate2._rect.Width, rectRotate2._rect.Height);
                             
                             //if(!rectF.Contains(new PointF( bound.X,bound.Y))||
                             //   !rectF.Contains(new PointF(bound.X+bound.Width, bound.Y)) ||
                             //   !rectF.Contains(new PointF(bound.X, bound.Y+bound.Height))||
                             //   !rectF.Contains(new PointF(bound.X + bound.Width, bound.Y + bound.Height)) )
                             //       return;

                                if (rotateRect == null) return;
                                toolEdit.Propety.rotCrop = new RectRotate(new RectangleF( rotateRect._rect.X, rotateRect._rect.Y, rotateRect._rect.Width, rotateRect._rect.Height),new PointF( rotateRect._PosCenter.X, rotateRect._PosCenter.Y), rotateRect._rectRotation, rotateRect._dragAnchor, rotateRect.IsElip);
                            }

                             else
                            {
                                RotatedRect rot = new RotatedRect(new Point2f(rotateRect._PosCenter.X, rotateRect._PosCenter.Y), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), rotateRect._rectRotation);
                                Rect bound = rot.BoundingRect();
                                RectRotate rectRotate2 = toolEdit.Propety.rotArea;
                                RectangleF rectF = new RectangleF(rectRotate2._PosCenter.X + rectRotate2._rect.X, rectRotate2._PosCenter.Y + rectRotate2._rect.Y, rectRotate2._rect.Width, rectRotate2._rect.Height);

                                if (!rectF.Contains(new PointF(bound.X, bound.Y)) ||
                                   !rectF.Contains(new PointF(bound.X + bound.Width, bound.Y)) ||
                                   !rectF.Contains(new PointF(bound.X, bound.Y + bound.Height)) ||
                                   !rectF.Contains(new PointF(bound.X + bound.Width, bound.Y + bound.Height)))
                                    return;

                                if (rotateRect == null) return;
                                toolEdit.Propety.rotMask = new RectRotate(new RectangleF(rotateRect._rect.X, rotateRect._rect.Y, rotateRect._rect.Width, rotateRect._rect.Height), new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y), rotateRect._rectRotation, rotateRect._dragAnchor, rotateRect.IsElip);
                            }
                        }
                        if (Global.TypeCrop == TypeCrop.Crop||toolEdit.Propety.rotCrop==null)
                        {

                        if (BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].TypeTool != TypeTool.Color_Area)
                            if (rotateRect != null)
                            {
                              //  toolEdit.matTemp = toolEdit.Propety.Processing(BeeCore.Common.listCamera[Global.IndexChoose].matRaw);

                               // toolEdit.matTemp = toolEdit.Propety.GetTemp(rotateRect, BeeCore.Common.listCamera[Global.IndexChoose].matRaw, bmMask);
                            }
                        //GetTemp(RectRotate rotateRect, Mat BeeCore.Common.listCamera[Global.IndexChoose].matRaw, Mat bmMask,  Mat matClear, ref Mat matMask, ref Mat matTemp)

                    }



                }
                    else
                    {
                        int IsCheck = 0 ;
                        if (!toolEdit.IsClear)
                        {
                          //  Global.TypeCrop = TypeCrop.Crop;
                          
                            //X:
                            //if (toolEdit.Propety.rotCrop == null)
                            //    Global.TypeCrop = TypeCrop.Area;
                            if (Global.TypeCrop == TypeCrop.Area)
                            {

                                if (toolEdit.Propety.rotArea == null) return;
                                rotateRect = new RectRotate(toolEdit.Propety.rotArea._rect, toolEdit.Propety.rotArea._PosCenter, toolEdit.Propety.rotArea._rectRotation, toolEdit.Propety.rotArea._dragAnchor,toolEdit.Propety.rotArea.IsElip);
                            }
                            else if (Global.TypeCrop == TypeCrop.Mask)
                            {

                                if (toolEdit.Propety.rotMask == null) return;
                                rotateRect = new RectRotate(toolEdit.Propety.rotMask._rect, toolEdit.Propety.rotMask._PosCenter, toolEdit.Propety.rotMask._rectRotation, toolEdit.Propety.rotMask._dragAnchor, toolEdit.Propety.rotMask.IsElip);
                            }
                            else
                            {
                                if (toolEdit.Propety.rotCrop == null) return;
                                rotateRect = new RectRotate(toolEdit.Propety.rotCrop._rect, toolEdit.Propety.rotCrop._PosCenter, toolEdit.Propety.rotCrop._rectRotation, toolEdit.Propety.rotCrop._dragAnchor, toolEdit.Propety.rotCrop.IsElip);
                            }
                            var mat = new Matrix();
                        mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
                        mat.Scale((float)(imgView.Zoom / 100.0), (float)(imgView.Zoom / 100.0));

                        mat.Translate(rotateRect._PosCenter.X, rotateRect._PosCenter.Y);
                            mat.Rotate(rotateRect._rectRotation);
                            mat.Invert();
                            // Mouse point in Rectangle's space. 
                            var point = mat.TransformPoint(new PointF(e.X, e.Y));

                            RectangleF _rect = rotateRect._rect;
                           
                            var rect = new RectangleF(rotateRect._rect.X - WidthPoint/2, rotateRect._rect.Y - WidthPoint / 2, rotateRect._rect.Width+ WidthPoint , rotateRect._rect.Height + WidthPoint );
                            if (Global.TypeCrop == TypeCrop.Area)
                                rect = new RectangleF(rotateRect._rect.X - 20, rotateRect._rect.Y - 20, rotateRect._rect.Width +40, rotateRect._rect.Height+40);

                            var rectTopLeft = new RectangleF(_rect.Left - WidthPoint/2, _rect.Top - WidthPoint / 2, WidthPoint, WidthPoint);
                            var rectTopRight = new RectangleF(_rect.Left + _rect.Width - WidthPoint / 2, _rect.Top - WidthPoint / 2, WidthPoint, WidthPoint);
                            var rectBottomLeft = new RectangleF(_rect.Left - WidthPoint / 2, _rect.Top + _rect.Height - WidthPoint / 2, WidthPoint, WidthPoint);
                            var rectBottomRight = new RectangleF(_rect.Left + _rect.Width - WidthPoint / 2, _rect.Top + _rect.Height - WidthPoint / 2, WidthPoint, WidthPoint);
                            var rectRotate = new RectangleF(-WidthPoint / 2, _rect.Top - WidthPoint*3, WidthPoint*2, WidthPoint*2);
                            _dragCenter = new PointF(rotateRect._PosCenter.X, rotateRect._PosCenter.Y);
                            if (rectTopLeft.Contains(point))
                            {
                                _dragStart = new PointF(point.X, point.Y);
                                rotateRect._dragAnchor = AnchorPoint.TopLeft;
                                _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);

                            }
                            else if (rectTopRight.Contains(point))
                            {

                                _dragStart = new PointF(point.X, point.Y);
                                rotateRect._dragAnchor = AnchorPoint.TopRight;
                                _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                            }
                            else if (rectBottomLeft.Contains(point))
                            {

                                _dragStart = new PointF(point.X, point.Y);
                                rotateRect._dragAnchor = AnchorPoint.BottomLeft;
                                _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                            }
                            else if (rectBottomRight.Contains(point))
                            {

                                _dragStart = new PointF(point.X, point.Y);
                                rotateRect._dragAnchor = AnchorPoint.BottomRight;
                                _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                            }
                            else if (rectRotate.Contains(point) && toolEdit.Propety.TypeTool != TypeTool.Position_Adjustment || rectRotate.Contains(point) && Global.TypeCrop != TypeCrop.Area )
                            {

                                _dragStart = new PointF(point.X, point.Y);
                                rotateRect._dragAnchor = AnchorPoint.Rotation;
                                _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                                _dragRot = rotateRect._rectRotation;
                            }
                            else if (rect.Contains(point))
                            {
                               
                                _dragStart = new PointF(point.X, point.Y);
                                rotateRect._dragAnchor = AnchorPoint.Center;
                                _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                                _dragStartOffset = new PointF(e.X - rotateRect._PosCenter.X, e.Y - rotateRect._PosCenter.Y);
                            }

                            else
                            {

                                rotateRect._dragAnchor = AnchorPoint.None;
                                var rectNone = new RectangleF(rotateRect._rect.X + rotateRect._rect.Width / 4, rotateRect._rect.Y + rotateRect._rect.Height / 4, rotateRect._rect.Width + rotateRect._rect.Width / 2, rotateRect._rect.Height + rotateRect._rect.Height / 2);

                                //if (IsCheck == 0)
                                //{
                                  
                                  
                                //    if (!rectNone.Contains(point))
                                //    {
                                //         if (toolEdit.Propety.rotMask == null|| toolEdit.Propety.rotCrop == null)
                                //        {
                                //            IsCheck+=2;
                                //            G.IsCheck = false;
                                //          //  Global.TypeCrop = TypeCrop.Area;
                                //            goto X;
                                //        }
                                //        else
                                //        {
                                //            IsCheck++; G.IsCheck = false;
                                //         //   Global.TypeCrop = TypeCrop.Mask;
                                //            goto X;
                                //        }
                                //    }
                                //    else
                                //    {

                                //    }

                                    
                                //}
                                //else   if (IsCheck == 1)
                                //{

                                //    if (!rectNone.Contains(point))
                                //    {
                                //        IsCheck++; G.IsCheck = false;
                                //     //   Global.TypeCrop = TypeCrop.Area;
                                //        goto X;
                                //    }
                                //}
                                //else
                                //{

                                //    ////G.IsCheck = true;
                                //    //if (!toolEdit.threadProcess.IsBusy)
                                //    //    toolEdit.threadProcess.RunWorkerAsync();
                                //    //if (toolEdit.Propety.rotCrop == null)
                                //    //    Global.TypeCrop = TypeCrop.Area;
                                //    //else
                                //    //Global.TypeCrop = TypeCrop.Crop;
                                //}
                                
                            }

                            if (Global.TypeCrop == TypeCrop.Area)
                                toolEdit.Propety.rotArea._dragAnchor = rotateRect._dragAnchor;
                            else if (Global.TypeCrop == TypeCrop.Mask)
                                toolEdit.Propety.rotMask._dragAnchor = rotateRect._dragAnchor;
                            else
                                toolEdit.Propety.rotCrop._dragAnchor = rotateRect._dragAnchor;
                        }
                      
                    }
                if (rotateRect._dragAnchor != AnchorPoint.None)
                    Global.StatusDraw = StatusDraw.None;
              //  rotateDraw = rotateRect.Clone();
                //  ShowCursor(rotateRect._dragAnchor, rotateRect._rectRotation);
                imgView.Invalidate();
            }
            catch(Exception ex)
            {

            }
        }


      
        private void imgView_MouseDown(object sender, MouseEventArgs e)
        {
            if (toolEdit == null) return;
            pDown = e.Location;
            _drag = true;
            if (toolEdit.IsClear)
                return;
            //if (Global.TypeCrop == TypeCrop.Area && toolEdit.Propety.rotArea._dragAnchor == AnchorPoint.None ||
            //    Global.TypeCrop == TypeCrop.Crop && toolEdit.Propety.rotCrop._dragAnchor == AnchorPoint.None)
            //{
            //    G.IsCheck = true;
            //}
            //else
            //{
            //    G.IsCheck = false;
              
              if(Global.IndexToolSelected>=0)
                if (BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].TypeTool == TypeTool.Color_Area)
                {
              
                    if (toolEdit.Propety.IsGetColor)
                        toolEdit.Propety.AddColor();
                
             

                }
           // }
         
              
            imgView.Invalidate();
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
        int WidthPoint = 10;
        
        private void imgView_Paint(object sender, PaintEventArgs e)
        {
            

                if (Global.IsRun)
                {
               //

                    //  gcResult = gc;

                    return;
                }


          
            gc = e.Graphics;
            gc.SmoothingMode= SmoothingMode.AntiAlias;
             var mat = new Matrix();

            mat = new Matrix();
            mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
            mat.Scale((float)(imgView.Zoom / 100.0), (float)(imgView.Zoom / 100.0));
            gc.Transform = mat;

            int index = 0;
                if (G.Config.IsShowCenter)
                {
                    e.Graphics.DrawLine(new Pen(Brushes.Blue, 1), Global.ParaCommon.SizeCCD.Width / 2, 0, Global.ParaCommon.SizeCCD.Width / 2, Global.ParaCommon.SizeCCD.Height);
                    e.Graphics.DrawLine(new Pen(Brushes.Blue, 1), 0, Global.ParaCommon.SizeCCD.Height / 2, Global.ParaCommon.SizeCCD.Width, Global.ParaCommon.SizeCCD.Height / 2);
                }
                if (G.Config.IsShowGird)
                {
                    int W =Global.ParaCommon.SizeCCD.Width, H = Global.ParaCommon.SizeCCD.Height;
                    int step = Math.Min(W, H) / 15;
                    for (int x = step; x < W; x += step)
                        e.Graphics.DrawLine(new Pen(Brushes.Gray, 1), x, 0, x, H);
                    for (int y = step; y < H; y += step)
                        e.Graphics.DrawLine(new Pen(Brushes.Gray, 1), 0, y, W, y);
                }
            gc.ResetTransform();
            if (G.Config.IsShowArea)
                {
                    int indexTool = 0;
                    foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[Global.IndexChoose])
                    {
                        RectRotate rot = PropetyTool.Control.Propety.rotArea;
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
            if(Global.StatusDraw == StatusDraw.None)
                if (toolEdit != null)
                    foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[Global.IndexChoose])
                    {
                        if (index != toolEdit.Propety.Index )
                        {
                            RectRotate rot = PropetyTool.Control.Propety.rotArea;
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
                        index++;
                    }

               
                if (toolEdit == null)
                    return;
                if (imgView.Image == null)
                    return;

                RectangleF _rect = new RectangleF(); ;
                PointF _rectPos = new PointF(); ;
                Single _rectRotation = 0;
                AnchorPoint _dragAnchor = AnchorPoint.None;
                Pen penRect = new Pen(Color.Orange, 2);

            //switch (IsTypeArea())
            //{
            //    case TypeCrop.Area:
            //        if (toolEdit.Propety.rotArea != null)
            //        {
            //            _rect = toolEdit.Propety.rotArea._rect;
            //            _rectPos = toolEdit.Propety.rotArea._PosCenter;
            //            _rectRotation = toolEdit.Propety.rotArea._rectRotation;
            //            _dragAnchor = toolEdit.Propety.rotArea._dragAnchor;
            //            penRect = new Pen(Color.DeepSkyBlue, 2);
            //        }
            //        else return;
            //        break;
            //    case TypeCrop.Crop:
            //        {
            //            if (toolEdit.Propety.rotCrop != null)
            //            {
            //                _rect = toolEdit.Propety.rotCrop._rect;
            //                _rectPos = toolEdit.Propety.rotCrop._PosCenter;
            //                _rectRotation = toolEdit.Propety.rotCrop._rectRotation;
            //                _dragAnchor = toolEdit.Propety.rotCrop._dragAnchor;
            //                penRect = new Pen(Color.Orange, 2);
            //            }

            //        }
            //        break;
            //    case TypeCrop.Mask:
            //        {
            //            if (toolEdit.Propety.rotMask != null)
            //            {
            //                _rect = toolEdit.Propety.rotMask._rect;
            //                _rectPos = toolEdit.Propety.rotMask._PosCenter;
            //                _rectRotation = toolEdit.Propety.rotMask._rectRotation;
            //                _dragAnchor = toolEdit.Propety.rotMask._dragAnchor;
            //                penRect = new Pen(Color.FromArgb(100, 111, 211, 213), 2);
            //            }
            //            else return;
            //        }
            //        break;
            //}
            //  if (_rectRotation == float.NaN) return;
            if (Global.StatusDraw == StatusDraw.Check)
            {
                gc.ResetTransform();
                //mat = new Matrix();
                //mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
                //mat.Scale((float)(imgView.Zoom / 100.0), (float)(imgView.Zoom / 100.0));
                //gc.Transform = mat;
                Global.ScaleZoom = (float)(imgView.Zoom / 100.0);
                Global.pScroll = new Point(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);

                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.DrawResult(gc);
                Global.StatusDraw = StatusDraw.None;
            
                return;
            }
            else
            {
                switch (Global.TypeCrop)
                {
                    case TypeCrop.Crop:
                        Draws.FillRect(gc, TypeCrop.Area, toolEdit.Propety.rotArea, imgView.AutoScrollPosition, imgView.Zoom, 20);
                        Draws.FillRect(gc, TypeCrop.Mask, toolEdit.Propety.rotMask, imgView.AutoScrollPosition, imgView.Zoom, 50);
                        Draws.RectEdit(gc, TypeCrop.Crop, toolEdit.Propety.rotCrop, Properties.Resources.Rotate, WidthPoint, imgView.AutoScrollPosition, imgView.Zoom, 4);

                        break;
                    case TypeCrop.Area:

                        Draws.FillRect(gc, TypeCrop.Crop, toolEdit.Propety.rotCrop, imgView.AutoScrollPosition, imgView.Zoom, 20);
                        Draws.FillRect(gc, TypeCrop.Mask, toolEdit.Propety.rotMask, imgView.AutoScrollPosition, imgView.Zoom, 50);
                        Draws.RectEdit(gc, TypeCrop.Area, toolEdit.Propety.rotArea, Properties.Resources.Rotate, WidthPoint, imgView.AutoScrollPosition, imgView.Zoom, 4);
                        break;
                    case TypeCrop.Mask:
                        Draws.FillRect(gc, TypeCrop.Area, toolEdit.Propety.rotArea, imgView.AutoScrollPosition, imgView.Zoom, 20);
                        Draws.FillRect(gc, TypeCrop.Crop, toolEdit.Propety.rotCrop, imgView.AutoScrollPosition, imgView.Zoom, 50);
                        Draws.RectEdit(gc, TypeCrop.Mask, toolEdit.Propety.rotMask, Properties.Resources.Rotate, WidthPoint, imgView.AutoScrollPosition, imgView.Zoom, 4);

                        break;

                }

                gc.ResetTransform();
            }
            //if (Global.TypeCrop == TypeCrop.Area)
            //    {
            //        //Crop
            //        if (toolEdit.Propety.rotCrop != null)
            //        {
            //            mat = new Matrix();
            //        mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
            //        mat.Scale((float)(imgView.Zoom / 100.0), (float)(imgView.Zoom / 100.0));

            //        mat.Translate(toolEdit.Propety.rotCrop._PosCenter.X, toolEdit.Propety.rotCrop._PosCenter.Y);
            //            mat.Rotate(toolEdit.Propety.rotCrop._rectRotation);
            //            RectangleF _rect2 = toolEdit.Propety.rotCrop._rect;
            //            gc.Transform = mat;
            //            gc.DrawRectangle(new Pen(Color.Silver, 2), new Rectangle((int)_rect2.X, (int)_rect2.Y, (int)_rect2.Width, (int)_rect2.Height));
            //            gc.ResetTransform();
            //        }
            //        //Mask
            //        if (toolEdit.Propety.rotMask != null)
            //        {
            //            mat = new Matrix();
            //        mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
            //        mat.Scale((float)(imgView.Zoom / 100.0), (float)(imgView.Zoom / 100.0));

            //        mat.Translate(toolEdit.Propety.rotMask._PosCenter.X, toolEdit.Propety.rotMask._PosCenter.Y);
            //            mat.Rotate(toolEdit.Propety.rotMask._rectRotation);
            //            RectangleF _rect2 = toolEdit.Propety.rotMask._rect;
            //            gc.Transform = mat;
            //            gc.FillRectangle(new SolidBrush(Color.FromArgb(70, 111, 211, 213)), new Rectangle((int)_rect2.X, (int)_rect2.Y, (int)_rect2.Width, (int)_rect2.Height));
            //            gc.ResetTransform();
            //        }

            //    }
            //    else if (Global.TypeCrop == TypeCrop.Crop)
            //    {
            //        //Area
            //        mat = new Matrix();
            //    mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
            //    mat.Scale((float)(imgView.Zoom / 100.0), (float)(imgView.Zoom / 100.0));


            //    mat.Translate(toolEdit.Propety.rotArea._PosCenter.X, toolEdit.Propety.rotArea._PosCenter.Y);
            //        mat.Rotate(toolEdit.Propety.rotArea._rectRotation);
            //        RectangleF _rect2 = toolEdit.Propety.rotArea._rect;
            //        gc.Transform = mat;
            //        gc.DrawRectangle(new Pen(Color.LightGray, 2), new Rectangle((int)_rect2.X, (int)_rect2.Y, (int)_rect2.Width, (int)_rect2.Height));
            //        gc.ResetTransform();
            //        //Mask
            //        if (toolEdit.Propety.rotMask != null)
            //        {
            //            mat = new Matrix();
            //        mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
            //        mat.Scale((float)(imgView.Zoom / 100.0), (float)(imgView.Zoom / 100.0));

            //        mat.Translate(toolEdit.Propety.rotMask._PosCenter.X, toolEdit.Propety.rotMask._PosCenter.Y);
            //            mat.Rotate(toolEdit.Propety.rotMask._rectRotation);
            //            _rect2 = toolEdit.Propety.rotMask._rect;
            //            gc.Transform = mat;
            //            gc.FillRectangle(new SolidBrush(Color.FromArgb(70, 111, 211, 213)), new Rectangle((int)_rect2.X, (int)_rect2.Y, (int)_rect2.Width, (int)_rect2.Height));
            //            gc.ResetTransform();
            //        }
            //    }
            //    else
            //    { //Crop
            //        if (toolEdit.Propety.rotCrop != null)
            //        {
            //            mat = new Matrix();
            //            mat.Scale((float)(imgView.Zoom / 100.0), (float)(imgView.Zoom / 100.0));
            //            mat.Translate(toolEdit.Propety.rotCrop._PosCenter.X, toolEdit.Propety.rotCrop._PosCenter.Y);
            //            mat.Rotate(toolEdit.Propety.rotCrop._rectRotation);
            //            RectangleF _rect3 = toolEdit.Propety.rotCrop._rect;
            //            gc.Transform = mat;
            //            gc.DrawRectangle(new Pen(Color.Silver, 2), new Rectangle((int)_rect3.X, (int)_rect3.Y, (int)_rect3.Width, (int)_rect3.Height));
            //            gc.ResetTransform();
            //        }
            //        //Area
            //        mat = new Matrix();
            //    mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
            //    mat.Scale((float)(imgView.Zoom / 100.0), (float)(imgView.Zoom / 100.0));

            //    mat.Translate(toolEdit.Propety.rotArea._PosCenter.X, toolEdit.Propety.rotArea._PosCenter.Y);
            //        mat.Rotate(toolEdit.Propety.rotArea._rectRotation);
            //        RectangleF _rect2 = toolEdit.Propety.rotArea._rect;
            //        gc.Transform = mat;
            //        gc.DrawRectangle(new Pen(Color.Red, 2), new Rectangle((int)_rect2.X, (int)_rect2.Y, (int)_rect2.Width, (int)_rect2.Height));
            //        gc.ResetTransform();


            //    }
            //    var backNone = new SolidBrush(Color.FromArgb(10, 220, 220, 220));
            //    mat = new Matrix();


                try
                {
                   
                
                    //var rectTopLeft = new RectangleF(_rect.Left - WidthPoint/2, _rect.Top - WidthPoint / 2, WidthPoint, WidthPoint);
                    //var rectTopRight = new RectangleF(_rect.Left + _rect.Width - WidthPoint / 2, _rect.Top - WidthPoint / 2, WidthPoint, WidthPoint);
                    //var rectBottomLeft = new RectangleF(_rect.Left - WidthPoint / 2, _rect.Top + _rect.Height - WidthPoint / 2, WidthPoint, WidthPoint);
                    //var rectBottomRight = new RectangleF(_rect.Left + _rect.Width - WidthPoint / 2, _rect.Top + _rect.Height - WidthPoint / 2, WidthPoint, WidthPoint);
                    //var rectRotate = new RectangleF(-WidthPoint / 2, _rect.Top + -WidthPoint * 3, WidthPoint*2, WidthPoint*2);
                    //var rectCenter = new RectangleF(-WidthPoint / 2, -WidthPoint / 2, WidthPoint, WidthPoint);

                    //var backNG = new SolidBrush(Color.FromArgb(0, 0, 0, 255));
                    ////var backChoose = new SolidBrush(Color.FromArgb(60, 255, 205, 35));
                    ////var cornerNone = new SolidBrush(Color.OrangeRed);
                    ////var cornerChoose = new SolidBrush(Color.Blue);
                    //var _clX = new Pen(Color.LightGray, 1);
                    //var _clY = new Pen(Color.Gray, 1);
                    //if (!G.IsCheck)
                    //{
                    //    //if (Global.TypeCrop == TypeCrop.Crop || toolEdit.Propety.rotCrop == null)
                    //    //    toolEdit.ShowEdit(gc, _rect);

                    //    //else if (Global.TypeCrop == TypeCrop.Mask)
                    //    //    gc.FillRectangle(new SolidBrush(Color.FromArgb(90, 111, 211, 213)), new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
      
                    //      //  BeeCore.Draws.RectEdit(gc, Global.TypeCrop , Global.TypeCrop == TypeCrop.Crop ? toolEdit.Propety.rotCrop : Global.TypeCrop == TypeCrop.Area ? toolEdit.Propety.rotArea :toolEdit.Propety.rotMask, Properties.Resources.Rotate, WidthPoint,imgView.AutoScrollPosition,imgView.Zoom,2);

                        
                    //     //switch (_dragAnchor)
                    //    //{
                    //    //    case AnchorPoint.None:
                    //    //        gc.DrawRectangle(penRect, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
                    //    //        gc.FillEllipse(cornerNone, rectTopLeft);
                    //    //        gc.FillEllipse(cornerNone, rectTopRight);
                    //    //        gc.FillEllipse(cornerNone, rectBottomLeft);
                    //    //        gc.FillEllipse(cornerNone, rectBottomRight);
                    //    //        gc.DrawImage(Properties.Resources.Rotate, rectRotate);
                    //    //        //gc.FillRectangle(cornerNone, rectCenter);
                    //    //        break;

                    //    //    case AnchorPoint.TopLeft:
                    //    //        rectTopLeft.Width += rectTopLeft.Width; rectTopLeft.Height += rectTopLeft.Height;
                    //    //        rectTopLeft.X -= rectTopLeft.Width/4; rectTopLeft.Y -= rectTopLeft.Height/4;
                    //    //    gc.DrawRectangle(penRect, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
                    //    //        gc.FillRectangle(cornerChoose, rectTopLeft);
                    //    //        gc.FillEllipse(cornerNone, rectTopRight);
                    //    //        gc.FillEllipse(cornerNone, rectBottomLeft);
                    //    //        gc.FillEllipse(cornerNone, rectBottomRight);
                    //    //        gc.DrawImage(Properties.Resources.Rotate, rectRotate);
                    //    //        // gc.FillRectangle(cornerNone, rectCenter);
                    //    //        break;
                    //    //    case AnchorPoint.TopRight:
                    //    //        //  gc.FillRectangle(backNone, _rect);
                    //    //        gc.DrawRectangle(penRect, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
                    //    //        gc.FillEllipse(cornerNone, rectTopLeft);
                    //    //        gc.FillEllipse(cornerChoose, rectTopRight);
                    //    //        gc.FillEllipse(cornerNone, rectBottomLeft);
                    //    //        gc.FillEllipse(cornerNone, rectBottomRight);
                    //    //        gc.DrawImage(Properties.Resources.Rotate, rectRotate);
                    //    //        break;
                    //    //    case AnchorPoint.BottomLeft:
                    //    //        //  gc.FillRectangle(backNone, _rect);
                    //    //        gc.DrawRectangle(penRect, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
                    //    //        gc.FillEllipse(cornerNone, rectTopLeft);
                    //    //        gc.FillEllipse(cornerNone, rectTopRight);
                    //    //        gc.FillEllipse(cornerChoose, rectBottomLeft);
                    //    //        gc.FillEllipse(cornerNone, rectBottomRight);
                    //    //        gc.DrawImage(Properties.Resources.Rotate, rectRotate);
                    //    //        break;
                    //    //    case AnchorPoint.BottomRight:
                    //    //        //gc.FillRectangle(backNone, _rect);
                    //    //        gc.DrawRectangle(penRect, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
                    //    //        gc.FillEllipse(cornerNone, rectTopLeft);
                    //    //        gc.FillEllipse(cornerNone, rectTopRight);
                    //    //        gc.FillEllipse(cornerNone, rectBottomLeft);
                    //    //        gc.FillEllipse(cornerChoose, rectBottomRight);
                    //    //        gc.DrawImage(Properties.Resources.Rotate, rectRotate);
                    //    //        break;
                    //    //    case AnchorPoint.Center:
                    //    //        // gc.FillRectangle(backChoose, _rect);
                    //    //        gc.DrawRectangle(penRect, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
                    //    //        gc.FillEllipse(cornerNone, rectTopLeft);
                    //    //        gc.FillEllipse(cornerNone, rectTopRight);
                    //    //        gc.FillEllipse(cornerNone, rectBottomLeft);
                    //    //        gc.FillEllipse(cornerNone, rectBottomRight);
                    //    //        gc.DrawImage(Properties.Resources.Rotate, rectRotate);
                    //    //        break;
                    //    //    case AnchorPoint.Rotation:
                    //    //        // gc.FillRectangle(backNone, _rect);
                    //    //        gc.DrawRectangle(penRect, new Rectangle((int)_rect.X, (int)_rect.Y, (int)_rect.Width, (int)_rect.Height));
                    //    //        gc.FillEllipse(cornerNone, rectTopLeft);
                    //    //        gc.FillEllipse(cornerNone, rectTopRight);
                    //    //        gc.FillEllipse(cornerNone, rectBottomLeft);
                    //    //        gc.FillEllipse(cornerNone, rectBottomRight);
                    //    //        gc.DrawImage(Properties.Resources.Rotate2, rectRotate);
                    //    //        break;


                    //    //}
                    //    if (toolEdit.IsClear && _drag)
                    //    {
                    //        gc.ResetTransform();
                    //        gc.FillRectangle(new SolidBrush(Color.FromArgb(100, 255, 255, 255)), new Rectangle(rectClear.X, rectClear.Y, widthClear, widthClear));
                    //        gc.DrawRectangle(new Pen(Color.Black, 2), new Rectangle(rectClear.X, rectClear.Y, widthClear, widthClear));

                    //    }

                    //}
                      
                
                        

                    if (toolEdit.Propety.IsGetColor)
                    {
                        e.Graphics.DrawEllipse(new Pen(clChoose, 5), new Rectangle(new Point(pMove.X - 25, pMove.Y - 25), new Size(50, 50)));
                    }
                }
                catch (Exception)
                {

                }
           // GC.Collect();
           // GC.WaitForPendingFinalizers();

        }
        public void tool_MouseMove(object sender, MouseEventArgs e)
        {

         //   G.IsCheck = false;
            imgView.Invalidate();
        }
        private void View_Load(object sender, EventArgs e)
        {
            if (G.Header == null) return;
            this.pBtn.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);
          
            this.pHeader.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);
          
            if (!G.Config.IsExternal)
            {
                btnTypeTrig.Enabled= false;
                btnTypeTrig.Text = "Trig Internal";
            }
            else
            {
                btnTypeTrig.Enabled = true;
                btnTypeTrig.Text = "Trig External";
            }
            //  

            // BeeCore.Common.Scan();
            KeyboardListener.s_KeyEventHandler += KeyboardListener_s_KeyEventHandler1;
            Global.IndexToolChanged += Global_IndexToolChanged;
            Global.StatusDrawChanged += Global_StatusDrawChanged;
            Global.TypeCropChanged += Global_TypeCropChanged;
            // toolEdit.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tool_MouseMove);
            //BeeCore.Common.listCamera[Global.IndexChoose].matRaw= BeeCore.Common.GetImageRaw();
            //if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw!=null)
            //    if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty())
            //        imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
            btnMenu.PerformClick();
            Global.ScaleZoom = (float)(imgView.Zoom / 100.0);
            Global.pScroll = new Point(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);

            //BeeCore.Common.listCamera[Global.IndexChoose].FrameChanged += Common_FrameChanged;


        }

        private void Global_TypeCropChanged(TypeCrop obj)
        {
            imgView.Invalidate();
        }

        private void Global_StatusDrawChanged(StatusDraw obj)
        {
            if (Global.StatusDraw != StatusDraw.None)
            imgView.Invalidate();
        }

        private void Global_IndexToolChanged(int obj)
        {
            if (Global.IndexToolSelected == -1) return;
            if (Global.StatusDraw == StatusDraw.Edit)
            {
                Global.OldPropetyTool = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.Clone();
             
                //   if (Score.Enabled||Global.IsRun) return;
                Global.TypeCrop = TypeCrop.Area;
              //  G.EditTool.pEditTool.Controls.Clear();
                Control control = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Control;
                control.Parent = G.EditTool.pEditTool;
                control.Size =G.EditTool. pEditTool.Size;
                control.Location = new Point(0, 0);
                control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                control.BringToFront();
               // DataTool.LoadPropety(control);
                TypeTool TypeTool = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].TypeTool;

                G.EditTool.iconTool.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject(TypeTool.ToString());
                G.EditTool.lbTool.Text = TypeTool.ToString();
                G.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Control.LoadPara();
                G.EditTool.View.imgView.Invalidate();
                G.EditTool.View.imgView.Update();
                G.EditTool.View.toolEdit = control;
             
              
                Shows.Full(imgView,Global.ParaCommon.SizeCCD);
            }

        }

        private void KeyboardListener_s_KeyEventHandler1(object sender, EventArgs e)
        {
            KeyboardListener.UniversalKeyEventArgs eventArgs = (KeyboardListener.UniversalKeyEventArgs)e;
                if ( IsKeyDown(Keys.Alt))
                {
                    if (eventArgs.KeyCode == Keys.Enter)
                    {
                    }
                }
            
        }

        private void Common_FrameChanged(object sender, PropertyChangedEventArgs e)
        {
            G.EditTool.lbFrameRate.Text = Global.ParaCommon.SizeCCD.ToString()+"-"+ BeeCore.Common.listCamera[Global.IndexChoose].FrameRate+" img/s ";
        }

        public dynamic toolEdit;
      
        public void CurrentTool()
        {
            
            switch(toolEdit.Propety.TypeTool)
             {
                case TypeTool.OutLine:
                
                    
                    toolEdit.OutLinepathRaw = pathRaw;
                    toolEdit.OutLineIsProcess = IsProcess;
                    break;
                case  TypeTool.Pattern:


                    toolEdit.OutLinepathRaw = pathRaw;
                    toolEdit.OutLineIsProcess = IsProcess;
                    break;
            }
        }
        public void ToolMouseUp()
        {
            if (toolEdit == null) return;
            if(toolEdit.IsClear)
            {
                if (listMask == null) listMask = new List<Mat>();
                listMask.Add(matMaskAdd.Clone());
                matMaskAdd = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1, Scalar.Black);
               
            }
          if(Global.IndexToolSelected>=0)
            switch (BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].TypeTool)
            {
                case TypeTool.OutLine:
                case TypeTool.Pattern:
                   
                case TypeTool.Position_Adjustment:
                    //if (toolEdit.Propety.rotCrop != null)
                    //    if (toolEdit.Propety.rotCrop._rect.Width != 0 && toolEdit.Propety.rotCrop._rect.Height != 0)
                    //    {
                    //        toolEdit.Propety.LearnPattern(toolEdit.indexTool, toolEdit.matTemp);

                    //    }
                    break;
                case TypeTool.Color_Area:
                  
                    toolEdit.matTemp = toolEdit.GetTemp( toolEdit.Propety.rotArea);
                    break;
            }
            //if (G.IsCheck)
            //    if (!toolEdit.threadProcess.IsBusy)
            //        toolEdit.threadProcess.RunWorkerAsync();
        }
        private void cbView_SelectedIndexChanged(object sender, EventArgs e)
        {
          //BeeCore.G.WindowView= (WindowView)Enum.Parse(typeof(WindowView), cbView.Text.Trim(), true);

          //  IsProcess = Convert.ToBoolean( (int)BeeCore.G.WindowView);
          //  CurrentTool();
        }
       public  String pathRaw;
       public bool IsProcess;

        private void workUndo_DoWork(object sender, DoWorkEventArgs e)
        {
           
        }

        private void workUndo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
           
        }

        public String[] listPath;
        public int indexTool = 0; int indexImg = 0;
 

        private void tmTool_Tick(object sender, EventArgs e)
        {
            
        }
       
    
      
        int indexToolPosition = -1;
        bool IsAutoTrig;
        OutLine ParaPosition;
        public void Checking()
        {

        }
        bool IsCompleteAll = false;
        private void workPlay_DoWork(object sender, DoWorkEventArgs e)
        {

           
            //int index = 0;
            //        foreach (Tools tool in G.listAlltool)
            //        {
            //            if (tool.PropetyTool.TypeTool == TypeTool.Yolo)
            //                continue;
            //            if (tool.PropetyTool.TypeTool == TypeTool.OCR)
            //                continue;
            //            if (BeeCore.Common.PropetyTools[index].UsedTool != UsedTool.NotUsed)
            //            {
            //                tool.tool.Process();
            //            }
            //            index++;
            //        }
                    return;
                    
 
                
                     

                //if (G.StatusTrig == Trig.None)
                //{
                //    indexToolPosition = BeeCore.Common.PropetyTools.FindIndex(a => a.TypeTool == TypeTool.Position_Adjustment);
                //    if (indexToolPosition > -1 && BeeCore.Common.PropetyTools[indexToolPosition].Propety.IsAutoTrig)
                //    {
                //        ParaPosition = (OutLine)G.listAlltool[indexToolPosition].tool.Propety;
                //        IsAutoTrig = ParaPosition.IsAutoTrig;
                //        G.StatusTrig = Trig.Processing;
                //    }
                //    else
                //    {
                //        foreach (Tools tool in G.listAlltool)
                //        {

                //            tool.tool.Process();

                //        }
                //        return;
                //    }
                //}
                //else if (G.StatusTrig == Trig.Processing || G.StatusTrig == Trig.NotTrig)
                //{
                //    G.listAlltool[indexToolPosition].tool.Process();
                //    if (ParaPosition.IsOK && G.StatusTrig == Trig.Processing)
                //    {
                //        ParaPosition.numTempOK++;
                //        if (ParaPosition.numTempOK >= ParaPosition.NumOK)
                //        {
                //            ParaPosition.numTempOK = 0;
                //            G.StatusTrig = Trig.Trigged;


                //        }
                //    }
                //    else if (!ParaPosition.IsOK && G.StatusTrig == Trig.Processing)
                //    {
                //        ParaPosition.numTempOK = 0;
                //    }


                //    if (!ParaPosition.IsOK && G.StatusTrig == Trig.NotTrig)
                //    {
                //        ParaPosition.numTempOK++;
                //        if (ParaPosition.numTempOK >= ParaPosition.NumOK)
                //        {
                //            ParaPosition.numTempOK = 0;
                //            G.StatusTrig = Trig.Complete;


                //        }
                //    }
                //    else if (ParaPosition.IsOK && G.StatusTrig == Trig.NotTrig)
                //    {
                //        ParaPosition.numTempOK = 0;
                //    }
                //    return;

                //}
                ////    else if (G.StatusTrig == Trig.Continue)
                //{
                   
                //    foreach (Tools tool in G.listAlltool)
                //    {
                //        if (tool.PropetyTool.TypeTool != TypeTool.Position_Adjustment)
                //            tool.tool.Process();

                //    }
                //}
            

         
            
             
                 
              
              
            
        }
        [DllImport("KERNEL32.DLL", EntryPoint =
   "SetProcessWorkingSetSize", SetLastError = true,
   CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize32Bit
   (IntPtr pProcess, int dwMinimumWorkingSetSize,
   int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint =
           "SetProcessWorkingSetSize", SetLastError = true,
           CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize64Bit
           (IntPtr pProcess, long dwMinimumWorkingSetSize,
           long dwMaximumWorkingSetSize);
        bool IsProcessing=false;
        Stopwatch stopWatch = new Stopwatch();

        int DelayTrig;
        Graphics graphicsOld;
        public void SQL_Insert(DateTime time, String model, int Qty, int Total, String Status, Mat raw,Bitmap rs)
        {
            if (!G.Config.IsSaveOK)
            {
                if(Status.Contains("OK"))
                {
                    return;
                }    
            }
            if (!G.Config.IsSaveNG)
            {
                if (Status.Contains("NG"))
                {
                    return;
                }
            }
            if (G.cnn.State == ConnectionState.Closed)
                G.cnn.Open();
            String pathRaw, pathRS;
            String date = DateTime.Now.ToString("yyyyMMdd");
            String Hour= DateTime.Now.ToString("HHmmss");
            pathRaw = "Report//" + date + "//Raw";
            pathRS = "Report//" + date + "//Result";
            if (!Directory.Exists(pathRaw))
            {
              
                Directory.CreateDirectory(pathRaw);
            }
            if (!Directory.Exists(pathRS))
            {
                Directory.CreateDirectory(pathRS);
            }
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = G.cnn;
                String Model = Properties.Settings.Default.programCurrent.Replace(".prog", "");
                String Sql = "";
                command.CommandText = "INSERT into Report (Date,Model,Qty,Total,Status,Raw,Result) VALUES(@Date,@Model,@Qty,@Total,@Status,@Raw,@Result)";
                command.Prepare();
                command.Parameters.Add("@Date", SqlDbType.DateTime).Value = DateTime.Now;             // 1
                command.Parameters.AddWithValue("@Model", model);                                             // 2                                                                                                      //  command.Parameters.Add("@Time", MySqlDbType.DateTime).Value = DateTime.Now;             // 3
                command.Parameters.Add("@Qty", SqlDbType.Int).Value = Qty;                                             // 4
                command.Parameters.Add("@Total", SqlDbType.Int).Value = Total;                                        // 5
                command.Parameters.AddWithValue("@Status", Status);                                           // 6
                command.Parameters.AddWithValue("@Raw", pathRaw + "//" + Model + "_" + Hour + ".png");// imageToByteArray(raw);         // 7
                command.Parameters.AddWithValue("@Result", pathRS + "//" + Model + "_" + Hour + ".png"); ;   // 8
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
            Mat matRs = rs.ToMat();
            switch(G.Config.TypeSave)
            {
                case 1: 
                    Cv2.PyrDown(matRs, matRs);
                    Cv2.PyrDown(matRs, matRs);
                    Cv2.PyrDown(raw, raw);
                    Cv2.PyrDown(raw, raw);
                    break;
                case 2: 
                    Cv2.PyrDown(matRs, matRs);
                    Cv2.PyrDown(raw, raw);
                    break;
            }    
            if(G.Config.IsSaveRaw)
            Cv2.ImWrite( pathRaw + "//" + model + "_"+ Hour + ".png", raw);
            if (G.Config.IsSaveRS)
                Cv2.ImWrite(pathRS + "//" + model + "_" + Hour + ".png", matRs);


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
        public void DrawTotalResult(int IndexThread )
        {

        
            if (BeeCore.Common.bmResult != null)
            {
               
                BeeCore.Common.bmResult.Dispose();
            }


            // BeeCore.Common.listCamera[Global.IndexChoose].matRaw= BeeCore.Common.GetImageRaw();
            //imgView.ImageIpl = BeeCore.Common.listCamera[Global.IndexChoose].matRaw;
            // Cv2.ImShow("A", BeeCore.Common.listCamera[Global.IndexChoose].matRaw);
            // Set the scale.
            //imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y

            //gc.Clear(Color.White);
            Mat rs = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone();
            if (rs.Empty()) return;
            if (rs.Width==0|| rs.Height == 0) return;
            BeeCore.Common.bmResult = rs.ToBitmap();

            // Convert nếu cần
            if ((BeeCore.Common.bmResult.PixelFormat & PixelFormat.Indexed) != 0)
            {
                BeeCore.Common.bmResult = ConvertToNonIndexed(BeeCore.Common.bmResult);
            }

                using (Graphics gc = Graphics.FromImage(BeeCore.Common.bmResult))
            {
              
             //   BeeCore.Common.bmResult = new Bitmap(bm, (int)bm.Width, (int)bm.Height);
                // Convert nếu cần
               
                // BeeCore.Common.bmResult = new Bitmap(bm,(int)bm.Width, (int)bm.Height);
                //Graphics gc = Graphics.FromImage(BeeCore.Common.bmResult);
                gc.ResetTransform();
                var mat = new Matrix();
                mat.Translate(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
                mat.Scale((float)(imgView.Zoom / 100.0), (float)(imgView.Zoom / 100.0));
                //gc.Transform=mat;
                
                foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[Global.IndexChoose])

                {
                    // tool.tool.Propety.ScoreRs = 80;
                    //tool.tool.Propety.IsOK = true;
                    if (PropetyTool.UsedTool == UsedTool.NotUsed)
                    {
                        //tool.ItemTool.lbStatus.Text = "NC";
                        //tool.ItemTool.Score.ColorTrack = Color.Gray;
                        //tool.ItemTool.lbScore.ForeColor = Color.Gray;
                        //tool.ItemTool.lbStatus.BackColor = Color.Gray;

                      
                        continue;
                    }
                    // BeeCore.Common.PropetyTools[indexTool]
                    //  SumCycle += tool.tool.Propety.cycleTime;
                    PropetyTool.Propety.DrawResult(gc);
                    //PropetyTool.ItemTool.CT = PropetyTool.Propety.cycleTime;
                    //PropetyTool.ItemTool.Score = PropetyTool.Propety.ScoreRs+"";
                    //PropetyTool.ItemTool.ValueScore =PropetyTool.Propety.ScoreRs;
                    if (PropetyTool.Results==Results.OK)
                    {
                        numToolOK++;
                        PropetyTool.ItemTool.Status = "OK";
                        PropetyTool.ItemTool.ColorTrack = Color.FromArgb(0, 172, 73);
                        PropetyTool.ItemTool.ClScore = Color.FromArgb(0, 172, 73);
                        PropetyTool.ItemTool.ClStatus = Color.FromArgb(0, 172, 73);
                        //switch (G.Config.ConditionOK)
                        //{
                        //    case ConditionOK.TotalOK:
                        //        numToolOK++;
                        //        break;
                        //    case ConditionOK.AnyOK:
                        //        numToolOK++;
                        //        break;
                        //    case ConditionOK.Logic:
                        //        if (BeeCore.Common.PropetyTools[IndexThread][indexTool].UsedTool == UsedTool.Invertse)
                        //        {
                        //            G.TotalOK = false;
                        //        }
                        //        break;
                        //}
                        //if (G.Config.ConditionOK == ConditionOK.Logic)
                        //{
                        //    if (BeeCore.Common.PropetyTools[IndexThread][indexTool].UsedTool == UsedTool.Used)
                        //    {
                        //        tool.ItemTool.lbStatus.Text = "OK";
                        //        tool.ItemTool.Score.ColorTrack = Color.FromArgb(0, 172, 73);
                        //        tool.ItemTool.lbScore.ForeColor = Color.FromArgb(0, 172, 73);
                        //        tool.ItemTool.lbStatus.BackColor = Color.FromArgb(0, 172, 73);
                        //    }
                        //    else
                        //    {
                        //        tool.ItemTool.Score.ColorTrack = Color.DarkRed;
                        //        tool.ItemTool.lbStatus.Text = "NG";
                        //        tool.ItemTool.lbScore.ForeColor = Color.DarkRed;
                        //        tool.ItemTool.lbStatus.BackColor = Color.DarkRed;
                        //    }
                        //}
                        //else
                        //{
                        //    tool.ItemTool.lbStatus.Text = "OK";
                        //    tool.ItemTool.Score.ColorTrack = Color.FromArgb(0, 172, 73);
                        //    tool.ItemTool.lbScore.ForeColor = Color.FromArgb(0, 172, 73);
                        //    tool.ItemTool.lbStatus.BackColor = Color.FromArgb(0, 172, 73);
                        //}
                    }

                    else
                    {
                        G.TotalOK = false;
                        //switch (G.Config.ConditionOK)
                        //{


                        //    case ConditionOK.Logic:
                        //        if (BeeCore.Common.PropetyTools[IndexThread][indexTool].UsedTool == UsedTool.Used)
                        //        {
                        //            G.TotalOK = false;
                        //        }
                        //        break;
                        //}
                        //if (G.Config.ConditionOK == ConditionOK.Logic)
                        //{
                        //    if (BeeCore.Common.PropetyTools[IndexThread][indexTool].UsedTool != UsedTool.Used)
                        //    {
                        //        tool.ItemTool.lbStatus.Text = "OK";
                        //        tool.ItemTool.Score.ColorTrack = Color.FromArgb(0, 172, 73);
                        //        tool.ItemTool.lbScore.ForeColor = Color.FromArgb(0, 172, 73);
                        //        tool.ItemTool.lbStatus.BackColor = Color.FromArgb(0, 172, 73);
                        //    }
                        //    else
                        //    {
                        //        tool.ItemTool.Score.ColorTrack = Color.DarkRed;
                        //        tool.ItemTool.lbStatus.Text = "NG";
                        //        tool.ItemTool.lbScore.ForeColor = Color.DarkRed;
                        //        tool.ItemTool.lbStatus.BackColor = Color.DarkRed;
                        //    }
                        //}
                        //else
                        //{
                        PropetyTool.ItemTool.ColorTrack = Color.DarkRed;
                        PropetyTool.ItemTool.Status= "NG";
                        PropetyTool.ItemTool.ClScore = Color.DarkRed;
                        PropetyTool.ItemTool.ClStatus= Color.DarkRed;
                        //}


                    }
                    PropetyTool.ItemTool.Refresh();

                    indexTool++;
                } 
                if (imgView.Image != null)
                {
                    imgView.Image.Dispose(); // Giải phóng ảnh cũ
                }   
                imgView.Image = BeeCore.Common.bmResult; ;
                rs.Dispose();
                //if (BeeCore.Common.bmResult == null) return;
                //if (BeeCore.Common.bmResult.Width == 0) return;
                //if (BeeCore.Common.bmResult.Height == 0) return;
            }
            //if(G.listHis.Count >20){
            //    G.listHis[0].bm.Dispose();
            //    G.listHis.RemoveAt(0);
            //}
            // G.listHis.Add(new HistoryCheck( (Bitmap) BeeCore.Common.bmResult.Clone(),G.TotalOK,DateTime.Now));
            //  Shows.RefreshImg(imgView,TypeImg.Result);
            //imgView.Invoke((Action)(() =>
            //{
            //    if (imgView.Image != null)
            //    {
            //        imgView.Image.Dispose(); // Giải phóng ảnh cũ
            //    }
            //    Bitmap copy = null;
            //    try
            //    {
            //         //copy = new Bitmap(BeeCore.Common.bmResult);  // tạo bản sao mới
            //        imgView.Image = BeeCore.Common.bmResult;
            //        //using (MemoryStream ms = new MemoryStream())
            //        //{
            //        //    BeeCore.Common.bmResult.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //        //    ms.Seek(0, SeekOrigin.Begin); // Đặt lại vị trí đầu stream

            //        //    // Load hình ảnh từ MemoryStream vào PictureBox
            //        //    imgView.Image = Image.FromStream(ms);
            //        //}
            //    }
            //    catch (Exception ex)
            //    {
            //        if (ex.Message.Contains("GDI"))
            //        {
            //            imgView = new Cyotek.Windows.Forms.ImageBox();

            //            imgView.Size = new System.Drawing.Size(pView.Width, pView.Height);

            //            imgView.Parent = pView;
            //            imgView.Location = new Point(pView.Width / 2 - 1280 / 2, Height / 2 - 720 / 2);

            //        }

            //    }
            //    finally
            //    {
            //      //  copy.Dispose();
            //      //  BeeCore.Common.bmResult.Dispose();
            //    }

            //}));
            //imgView.Refresh();
            //imgView.Update();
            //if (!IsInvert)
            //{
            //    imgView.BringToFront();// = true;
            //    //imgView2.Visible = false;
            //    imgView.Invoke((Action)(() =>
            //    {
            //      if (imgView.Image != null)
            //        {
            //            imgView.Image.Dispose(); // Giải phóng ảnh cũ
            //        }
            //      imgView.Image = new Bitmap(newImage); // Đặt ảnh mới
            //    }));
            //}
            //else
            //{
            //    //imgView.Visible = false;
            //    imgView2.BringToFront();// = true;
            //    imgView2.Invoke((Action)(() =>
            //    {
            //        if (imgView2.Image != null)
            //        {
            //            imgView2.Image.Dispose(); // Giải phóng ảnh cũ
            //        }
            //        imgView2.Image = new Bitmap(newImage); // Đặt ảnh mới
            //    }));
            //}
            //  IsInvert = !IsInvert;
            // Giải phóng ảnh từ file
            numSetImg++;
            //if (numSetImg > 1000)
            //{
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            //    {

            //        SetProcessWorkingSetSize32Bit(System.Diagnostics
            //           .Process.GetCurrentProcess().Handle, -1, -1);
            //    }
            //    numSetImg = 0;
            //}
          

        }
        int numSetImg;
        public  void ShowResultTotal()
        {
            int indexTool = 0;

            numToolOK = 0;
            G.TotalOK = true;
            Global.ScaleZoom = (float)(imgView.Zoom / 100.0);
            Global.pScroll = new Point(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);

            foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[0])
            {
                if (PropetyTool.UsedTool == UsedTool.NotUsed)
                {

                    indexTool++;
                    continue;
                }
                if (PropetyTool.Results==Results.NG)
                {
                    G.TotalOK = false;
                    break;
                }
                    
            }
            G.IsSendRS = true;
            DrawTotalResult(0);

           
            //switch (G.Config.ConditionOK)
            //{
            //    case ConditionOK.TotalOK:
            //        if (numToolOK < G.listAlltool[0].Count)
            //            G.TotalOK = false;
            //        else
            //            G.TotalOK = true;
            //        break;
            //    case ConditionOK.AnyOK:
            //        if (numToolOK >= Global.ParaCommon.numToolOK)
            //            G.TotalOK = true;
            //        else
            //            G.TotalOK = false;
            //        break;

            //}
            if (G.TotalOK)
            {
                 G.StatusDashboard.StatusText = "OK";
                 G.StatusDashboard.StatusBlockBackColor= Color.FromArgb(255, 27, 186, 98);
                if (!G.IsModeTest)
                     G.Config.SumOK++;
               
            
            }
            else
            {
                 G.StatusDashboard.StatusText = "NG";
                 G.StatusDashboard.StatusBlockBackColor = Color.DarkRed;
                if (!G.IsModeTest)
                    G.Config.SumNG++;

              
            }
           
            if (!G.IsModeTest)
            if (G.Config.IsSaveOK || G.Config.IsSaveNG)
            {
                if (!workInsert.IsBusy)
                {
                    workInsert.RunWorkerAsync();
                }
            }
            G.Config.SumTime =  G.Config.SumOK + G.Config.SumNG;
           
             G.StatusDashboard.TotalTimes= G.Config.SumTime;
             G.StatusDashboard.OkCount=  G.Config.SumOK;  G.StatusDashboard.NgCount= G.Config.SumNG ;
            G.Config.TotalTime += Convert.ToSingle(SumCycle / (60000.0));
            G.Config.Percent=Convert.ToSingle((( G.Config.SumOK*1.0)/( G.Config.SumOK+G.Config.SumNG)) * 100.0);
            // G.StatusDashboard.lbTotalTime.Text =Math.Round(BeeCore.Common.Cycle,1)+ " ms";
            
             G.StatusDashboard.CycleTime= (int)SumCycle;
            SumCycle = 0;
         
           // btnCap.Enabled = true;
        }
        public float SumCycle = 0;
        public void CheckStatusMode()
        {
          
            switch (Global.StatusMode)
            {
                case StatusMode.SimContinuous:
                    btnFolder.Enabled = false;
                    btnRunSim.Enabled = true;
                    btnPlayStep.Enabled = true;
                    tmSimulation.Enabled = true;
                    break;
                case StatusMode.SimOne:
                    indexFile++;
                    btnFile.Enabled = true;
                    btnPlayStep.Enabled = true;
                    Global.StatusMode = StatusMode.None;
                    break;
                case StatusMode.Once:
                    tmPress.Enabled = true;
                    btnCap.IsCLick = false;
                    Global.StatusMode = StatusMode.None;
                    break;
                case StatusMode.Continuous:
                    tmContinuous.Enabled = true;
                    btnCap.Enabled = false;
                   // btnCap.IsCLick = false;
                    break;
                case StatusMode.None:

                    //btnRunSim.Enabled = false;
                    btnPlayStep.Enabled = false;
                    btnFile.Enabled = true;
                    btnFolder.Enabled = true;
                    break;
            }
           

        }
     
        Graphics gcResult;
        public  void Continuous()
            {
             Global.StatusMode = StatusMode.Continuous;
            if(!workReadCCD.IsBusy)
            {
                workReadCCD.RunWorkerAsync();
            }    
            
            }
        public float Cyclyle1 = 0;
        public void Cap(bool IsTest)
        {

            if (btnLive.IsCLick)
            {
                btnCap.IsCLick = false;
                return;
            }    

                btnCap.Enabled = false;

           Global.StatusMode =  StatusMode.Once;

            
            timer.Restart();
            if (!workReadCCD.IsBusy) workReadCCD.RunWorkerAsync();
        }
        public bool  IsBTNCap=false;
        private  void btnCap_Click(object sender, EventArgs e)
        {

            if (!Global.Comunication.IO.IsConnected&&!Global.Comunication.IO.IsBypass )
            {
                btnCap.IsCLick = false;
                return;
            }
         
            if (btnRecord.IsCLick)
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
          //  BeeCore.Common.currentTrig++;
            Cap(false);
        }

        private async void btnRecord_Click(object sender, EventArgs e)
        {
           
            if (!Global.Comunication.IO.IsConnected && !Global.Comunication.IO.IsBypass)
            {
                btnRecord.IsCLick = false;
                return;
            }
            if (btnLive.IsCLick)
            {
                btnRecord.IsCLick = false;
                return;
            }
            Global.StatusMode = btnRecord.IsCLick ? StatusMode.Continuous : StatusMode.None;

            if (Global.Comunication.IO.IsConnected)
            {
                if (Global.Comunication.IO.IsConnected)
                {
                    tmContinuous.Enabled = btnRecord.IsCLick;
                    //X: G.Header.tmReadPLC.Enabled = false;
                    //    if (G.Header.workPLC.IsBusy)
                    //    {
                    //        await Task.Delay(10);
                    //        goto X;
                    //    }
                    //    await Task.Run(() => Global.Comunication.IO.WriteInPut(0, true));
                    //    G.Header.tmReadPLC.Enabled = true;
                    //    Global.Comunication.IO.WriteInPut(0, true);
                    //}
                    //if (!workReadCCD.IsBusy)
                    //    workReadCCD.RunWorkerAsync();
                    return;
                }
            } 
            else if(Global.Comunication.IO.IsBypass)
            {
                tmContinuous.Enabled = btnRecord.IsCLick;
            }    
            else
            {
                btnRecord.IsCLick = false;
                tmContinuous.Enabled = false;
                return;
            }
            if (!btnRecord.IsCLick) btnCap.Enabled = true;
          
           

        }

        private void tmPlay_Tick(object sender, EventArgs e)
        {
           
            tmPlay.Enabled = false;
        }
        private Thread displayThread;
        public void Live()
        {
            if (Global.Comunication.IO.IsConnected)
                G.Header.tmReadPLC.Enabled = !btnLive.IsCLick;
            tmLive.Enabled = btnLive.IsCLick;
            if (btnLive.IsCLick)
            {
                if (G.SettingPLC != null)
                    G.SettingPLC.Enabled = false;
                btnShowArea.Enabled = true;
                btnShowCenter.Enabled = true;
                btnGird.Enabled = true;
                btnCap.Enabled = false;
                btnRecord.Enabled = false;
            }
            else
            {
                btnShowArea.Enabled = false;
                btnShowCenter.Enabled = false;
                btnGird.Enabled = false;
                if (G.SettingPLC != null)
                    G.SettingPLC.Enabled = true;
                //btnCap.Enabled = true;
                //btnRecord.Enabled = true;
            }    
            gc = imgView.CreateGraphics();
            if (!workReadCCD.IsBusy)
                workReadCCD.RunWorkerAsync();
            if (btnLive.IsCLick)
            {


                displayThread = new Thread(() =>
            {
                while (btnLive.IsCLick)
                {
                   
                    Bitmap frameToDisplay;
                    lock (frameLock)
                    {
                        frameToDisplay = sharedFrame?.Clone() as Bitmap;
                    }

                    var old = imgView.Image;
                    if (frameToDisplay != null)
                    {
                        try
                        {
                            this.Invoke(new Action(() =>
                            {

                                imgView.Image = frameToDisplay;
                            }));
                        }
                        catch
                        {
                            frameToDisplay.Dispose(); // UI đã dispose rồi
                        }
                    }

                    old?.Dispose(); // Dispose ảnh cũ sau khi gán xong

                   
                }
            });
                displayThread.IsBackground = true;

                displayThread.Start();
            }

        }
        private void btnSer_Click(object sender, EventArgs e)
        {
          
            if (btnCap.Enabled == false&&Global.IsRun)
            {
                btnLive.IsCLick = false;
                return;
            }
            if (btnRecord.IsCLick)
            {

                btnLive.IsCLick = false;
             
                MessageBox.Show("Please stop Mode Continue");
                return;
            }
            
          
                numLive = 0;
                tmOut.Enabled = false;
                Live();
            
          
            //else if (G.ConfiGlobal.TypeCamera  == TypeCamera.TinyIV)
            //{
            //    BeeCore.Common.listCamera[Global.IndexChoose].Read();
            //    BeeCore.Common.IsLive = btnLive.IsCLick;
            //    if (btnLive.IsCLick)
            //        BeeCore.Common.PropertyChanged += Common_PropertyChanged;
            //    else
            //        BeeCore.Common.PropertyChanged -= Common_PropertyChanged;
            //}
            //if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw != null)
            //    if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty())
            //        //G.EditTool.View.imgView.Location = new Point(G.EditTool.View.pView.Width / 2 - BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Width / 2, G.EditTool.View.pView.Height / 2 - BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Height / 2);
            if (btnLive.IsCLick)
            {
              //  tmRefresh.Enabled = true;

                //if (G.Header.SerialPort1.IsOpen)
                //    G.Header.SerialPort1.WriteLine("Live");
               // btnFull.PerformClick();
            }

            else
            {
                tmRefresh.Enabled = false;
                //if (G.Header.SerialPort1.IsOpen)
                //    G.Header.SerialPort1.WriteLine("Capture");

            }
           
        }

        public void Common_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
            //imgView.Image = BeeCore.Common.matLive.ToBitmap();
           // GC.Collect();
           // GC.WaitForPendingFinalizers();

        }
      
    
        int numLive = 500;
        Stopwatch timer = new Stopwatch();
    
        private Bitmap sharedFrame = null;
        private object frameLock = new object();
        private void workReadCCD_DoWork(object sender, DoWorkEventArgs e)
        {
            if(!Global.IsRun)
            {
                BeeCore.Common.listCamera[Global.IndexChoose].Read();

            }
            else
                
                Parallel.ForEach(BeeCore.Common.listCamera, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, camera =>
                {
                    if (camera != null)
                        camera.Read();
                });



        }
        private void  workReadCCD_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

         
            if (btnLive.IsCLick)
            {
                if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw != null && !BeeCore.Common.listCamera[Global.IndexChoose].matRaw.IsDisposed)
                    using (Bitmap frame = BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexChoose].matRaw))
                {
                    lock (frameLock)
                    {
                        sharedFrame?.Dispose();
                        sharedFrame = (Bitmap)frame.Clone(); // Clone để thread-safe
                    }
                }
                  
           
                workReadCCD.RunWorkerAsync();
                return;
            }
            else if (Global.StatusMode==StatusMode.Continuous|| Global.StatusMode == StatusMode.Once)
            {
                RunProcessing();
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
            //    //  BeeCore.Common.listCamera[Global.IndexChoose].Read(G.Config.IsHist );
            //    tmTrig.Enabled = false;
            //    if (!workPlay.IsBusy)
            //        workPlay.RunWorkerAsync();
            //}
          
           
         
        }

        private void workShow_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
       
        }

        private void btnShowSetting_Click(object sender, EventArgs e)
        {
            if (Global.TypeCamera  == TypeCamera.USB)
                BeeCore.Common.listCamera[Global.IndexChoose].Setting();
            else if (Global.TypeCamera  == TypeCamera.TinyIV)
            {
                SettingDevice settingDevice = new SettingDevice();
            
                settingDevice.Show();
               
              
            }
           
        }

        private void workGetColor_DoWork(object sender, DoWorkEventArgs e)
        {
            using (Mat raw = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone())
            {
                int X = Convert.ToInt32((pMove.X - 10) / (G.Config.imgZoom / 100.0)) + G.Config.imgOffSetX;
                int Y = Convert.ToInt32((pMove.Y + 10) / (G.Config.imgZoom / 100.0)) + G.Config.imgOffSetY;
                clChoose = toolEdit.Propety.GetColor(raw, X, Y);
            }    
           
                      
                 
        }

        private void workGetColor_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            imgView.Invalidate();
        }

        private void btnResetQty_Click(object sender, EventArgs e)
        {
           
        }

        private void ckProcess_CheckedChanged(object sender, EventArgs e)
        {
            BeeCore.Common.IsDebug=ckProcess.Checked;
          
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
            if (G.TotalOK)
                SQL_Insert(DateTime.Now, Properties.Settings.Default.programCurrent.Replace(".prog", ""),  G.Config.SumOK,  G.Config.SumOK + G.Config.SumNG, "OK", BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone(), BeeCore.Common.bmResult);
            else
                SQL_Insert(DateTime.Now, Properties.Settings.Default.programCurrent.Replace(".prog", ""),  G.Config.SumOK,  G.Config.SumOK + G.Config.SumNG, "NG", BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone(), BeeCore.Common.bmResult);

        }
        int numErrPort = 0;
        private void tmCheckPort_Tick(object sender, EventArgs e)
        {
           
            if (!G.Header.SerialPort1.IsOpen)
            {
                try
                {
                    G.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;
                    G.EditTool.toolStripPort.Text = "Port Not Connect";
                    G.Header.SerialPort1.Close();
                    G.Header.SerialPort1.Open();
                }
                catch (Exception ex)
                {
                    numErrPort++;
                }
                if(numErrPort>5)
                {
                   // G.EditTool.toolStripPort.Image = Properties.Resources.PortNotConnect;
                   // G.EditTool.toolStripPort.Text = "Port Not Connect";
                    // MessageBox.Show("Error connect Port " + G.Header.SerialPort.PortName);

                }
            }
        }

       
        FormCalib formCalib;

    

        private void tmCheckCCD_Tick(object sender, EventArgs e)
        {
            //if (G.ConfiGlobal.TypeCamera  == TypeCamera.USB)
            //{
            //    if (!btnCap.IsCLick && !btnRecord.IsCLick && !workPlay.IsBusy && !workReadCCD.IsBusy)
            //        BeeCore.Common.listCamera[Global.IndexChoose].Read();
            //    if (BeeCore.Common.listCamera[Global.IndexChoose].Status())
            //    {

            //        if (G.ScanCCD.ScanIDCCD().FindIndex(a => a.Contains(G.Config.IDCamera)) > -1)
            //        {
            //            if (!BeeCore.Common.ConnectCCD(G.ScanCCD.indexCCD, G.Config.Resolution))
            //            {
            //                G.EditTool.lbCam.Image = Properties.Resources.CameraNotConnect;
            //                G.EditTool.lbCam.Text = "Camera Not Connect";

            //                btnCap.Enabled = false;
            //                btnRecord.Enabled = false;
                           
            //                tmCheckCCD.Interval = 3000;
            //            }
            //            else
            //            {
            //                tmCheckCCD.Interval = 1000;
            //                if (Global.IsRun)
            //                {
            //                    btnCap.Enabled = true;
            //                    if (G.Config.nameUser == "Admin")
            //                    {

            //                        btnRecord.Enabled = true;
                                  
            //                    }
            //                }
            //                G.EditTool.lbCam.Image = Properties.Resources.CameraConnected;
            //                G.EditTool.lbCam.Text = G.Config.IDCamera.Split('$')[0] + " Connected";
            //            }
            //        }
            //        else
            //        {
            //            tmCheckCCD.Interval = 3000;
            //            btnCap.Enabled = false;
            //            btnRecord.Enabled = false;
                      
            //            G.EditTool.lbCam.Image = Properties.Resources.CameraNotConnect;
            //            G.EditTool.lbCam.Text = "Camera Not Connect";

            //        }
            //    }
            //    else
            //    {
            //        tmCheckCCD.Interval = 1000;
            //        if (Global.IsRun)
            //        {
            //            btnCap.Enabled = true;
            //            if (G.Config.nameUser == "Admin")
            //            {
            //                btnRecord.Enabled = true;
                           
            //            }
            //        }
            //        G.EditTool.lbCam.Image = Properties.Resources.CameraConnected;
            //        G.EditTool.lbCam.Text =  G.Config.IDCamera.Split('$')[0] + " Connected";
            //    }
            //}
               
        }
      List<  String> Files=new List<string>();
        List<Mat> listMat = new List<Mat>();
        int indexFile = 0;
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
                    btnRunSim.Enabled = true; btnPlayStep.Enabled = true;
                }
               
            }    
          if(!Global.IsRun)
            {
                indexFile = 0;
                pathFileSeleted = Files[indexFile];
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = BeeCore.Common.listCamera[Global.IndexChoose].matRaw = listMat[indexFile]; ;// Cv2.ImRead(Files[indexFile]);
                BeeCore.Native.SetImg(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
                imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
            }
        }
        // The Bitmap we display.
        private Bitmap Bm = null;

        // The dimensions of the drawing area in world coordinates.
       

        // The scale.
        public float PictureScale = 1.0f;
       
        private void DrawImage(Graphics gr)
        {
            gr.DrawImage(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap(),new PointF(0,0));
           
        }
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
           // PictureScale += 0.1f;
           // imgView.Invalidate();
            imgView.Zoom += 5;
          //  imgView2.Invalidate();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            imgView.Zoom -= 5;
          //  imgView2.Invalidate();
            //PictureScale -= 0.1f;
            //if (PictureScale == 0) PictureScale = 0.1f;
            //imgView.Invalidate();
        }

        private void btnFull_Click(object sender, EventArgs e)
        {
            if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw.IsDisposed) return;
            Size sz = new Size(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Width, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Height);
            Shows.Full(imgView,sz);
            G.Config.imgZoom = imgView.Zoom;
            G.Config.imgOffSetX = imgView.AutoScrollPosition.X;
            G.Config.imgOffSetY= imgView.AutoScrollPosition.Y;
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
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {

                    SetProcessWorkingSetSize32Bit(System.Diagnostics
                       .Process.GetCurrentProcess().Handle, -1, -1);

                }
                Thread.Sleep(50);

                btnLive.IsCLick = true;
                if(!workReadCCD.IsBusy)
                workReadCCD.RunWorkerAsync();
            }    
        }

        private void imgView_ZoomChanged(object sender, EventArgs e)
        {
            Global.ScaleZoom = (float)(imgView.Zoom / 100.0);
            Global.pScroll = new Point(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);

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
            if (G.Header == null) return;
             BeeCore.CustomGui.RoundRg(this.pBtn, G.Config.RoundRad);

        }

        private void lbSumOK_Click(object sender, EventArgs e)
        {

        }

        private void pInforTotal_SizeChanged(object sender, EventArgs e)
        {
            //if (G.Header == null) return;
            //BeeCore.CustomGui.RoundRg( G.StatusDashboard, G.Config.RoundRad);

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
            BeeCore.CustomGui.RoundRg(pHeader, G.Config.RoundRad);

        }

        private void btnGird_Click(object sender, EventArgs e)
        {
        
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            G.Config.IsShowArea = !G.Config.IsShowArea;
         imgView.Invalidate();
        }
       
        private  void tmContinuous_Tick(object sender, EventArgs e)
        {
            if(!BeeCore.Common.listCamera[Global.IndexChoose].IsConnected)
            {
                G.Header.ShowErr();
                tmContinuous.Enabled = false;
                btnRecord.IsCLick = false;
                return;
            }
            if (!btnCap.Enabled&&!Global.Comunication.IO.IsBypass)
                return;
            Continuous();
            tmContinuous.Enabled = false;
        }

        private void workTrig_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Global.Comunication.IO.IsConnected)
                Global.Comunication.IO.WriteInPut(0, true);//.  BtnWriteInPLC((RJButton)sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            G.Config.IsShowCenter = !G.Config.IsShowCenter;
            imgView.Invalidate();
        }

        private void workInsert_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
        }

        private void tmPress_Tick(object sender, EventArgs e)
        {
            tmPress.Enabled = false;
            if(Global.IsRun&&!G.Config.IsExternal)
            btnCap.Enabled = true;
            else
            {
                btnCap.Enabled = false;
            }
            G.EditTool.View.btnTypeTrig.IsCLick = false;
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
                BeeCore.Native.SetImg(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
                imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                 btnFile.Enabled = false;
                Global.StatusMode = StatusMode.SimOne;
                RunProcessing();
                btnRunSim.Enabled = true;
            }

        }

        private void tmSimulation_Tick(object sender, EventArgs e)
        {
            Global.StatusMode = btnRunSim.IsCLick ? StatusMode.SimContinuous : StatusMode.None;

            tmSimulation.Enabled = false;

        X: indexFile++;
                if (indexFile < Files.Count())
                {
                   
                    if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty())
                        BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Release();
                    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Cv2.ImRead(Files[indexFile]);
                G.EditTool.lbNamefile.Text = indexFile + "." + Path.GetFileNameWithoutExtension(Files[indexFile]);
                if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty()) goto X;
                BeeCore.Native.SetImg(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
                    imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                RunProcessing();
            }
                else
                {
                   
                Global.StatusMode = StatusMode.None;
             
                }

         
        }

        private void btnTypeTrig_Click(object sender, EventArgs e)
        {

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
            BeeCore.Native.SetImg(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
            imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
            if (Global.IsRun)
            {
                Global.StatusMode = StatusMode.SimOne;
                RunProcessing();
                btnPlayStep.Enabled = false;
            }

            G.EditTool.lbNamefile.Text = indexFile + "." + Path.GetFileNameWithoutExtension(Files[indexFile]);
          
           
        }

        private void btnDeleteFile_Click(object sender, EventArgs e)
        {
            File.Delete(Files[indexFile - 1]);
            Files.RemoveAt(indexFile - 1);
            listMat.RemoveAt(indexFile - 1);

            indexFile--;
            if (indexFile < 0)
                indexFile = 0;
            btnPlayStep.PerformClick();
        }

        private void btnGird_Click_1(object sender, EventArgs e)
        {
            G.Config.IsShowGird = !G.Config.IsShowGird;
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
        StatusProcessing Processing1 = StatusProcessing.None;
        StatusProcessing Processing2 = StatusProcessing.None;
        StatusProcessing Processing3 = StatusProcessing.None;
        StatusProcessing Processing4 = StatusProcessing.None;
        ClassProject classProcessing1 = new ClassProject();
        ClassProject classProcessing2 = new ClassProject();
        ClassProject classProcessing3 = new ClassProject();
        ClassProject classProcessing4 = new ClassProject();

        private void workCheck1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            Processing1= classProcessing1.ProcessingAll( Processing1,0);
            if (Processing1 != StatusProcessing.Done) workCheck1.RunWorkerAsync();
            else
                foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[0])
                {

                    propetyTool.StatusTool =StatusTool.WaitCheck;
                }

        }

        private void workCheck2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Processing2 = classProcessing2.ProcessingAll(Processing2,1);
            if (Processing2 != StatusProcessing.Done) workCheck2.RunWorkerAsync();
            else
                foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[1])
                {

                    propetyTool.StatusTool =StatusTool.WaitCheck;
                }
        }

        private void workCheck3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Processing3 = classProcessing3.ProcessingAll(Processing3,2);
            if (Processing3 != StatusProcessing.Done) workCheck3.RunWorkerAsync();
            else
                foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[2])
                {

                    propetyTool.StatusTool =StatusTool.WaitCheck;
                }
        }
        private void workPlay_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // ClassProject.  ProcessingAll();

            if (Processing1 == StatusProcessing.Done && Processing2 == StatusProcessing.Done && Processing3 == StatusProcessing.Done && Processing4 == StatusProcessing.Done)
            {



                timer.Stop();

                ShowResultTotal();


                SumCycle = (int)timer.Elapsed.TotalMilliseconds + Cyclyle1;
                if (G.Header.IsWaitingRead)
                {
                    G.Header.IsWaitingRead = false;
                    G.Header.tmReadPLC.Enabled = true;
                }

                CheckStatusMode();
                IsCompleteAll = false;
                
                Processing1 = StatusProcessing.None;
                Processing2 = StatusProcessing.None;
                Processing3 = StatusProcessing.None;
                Processing4 = StatusProcessing.None;
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
            //     Mat matCrop = G.EditTool.View.CropRotatedRect(matCCD, new RotatedRect(new Point2f(rot._PosCenter.X + (rot._rect.Width / 2 + rot._rect.X), rot._PosCenter.Y + (rot._rect.Height / 2 + rot._rect.Y)), new Size2f(rot._rect.Width, rot._rect.Height), angle));

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

        private void workCheck4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Processing4 = classProcessing4.ProcessingAll(Processing4,3);
            if (Processing4 != StatusProcessing.Done) workCheck4.RunWorkerAsync();
            else
                foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[3])
                {

                    propetyTool.StatusTool = StatusTool.WaitCheck;
                }
        }
        public void RunProcessing()
        {
            if (BeeCore.Common.listCamera[0] != null)
            {
                if (!workCheck1.IsBusy)
                    workCheck1.RunWorkerAsync();
                else
                    Processing1 = StatusProcessing.Done;
            }
            else
                Processing1 = StatusProcessing.Done;
            if (BeeCore.Common.listCamera[1] != null)
            {
                if (!workCheck2.IsBusy)
                    workCheck2.RunWorkerAsync();
                else
                    Processing2 = StatusProcessing.Done;
            }
            else
                Processing2 = StatusProcessing.Done;

            if (BeeCore.Common.listCamera[2] != null)
            {
                if (!workCheck3.IsBusy)
                    workCheck3.RunWorkerAsync();
                else
                    Processing3 = StatusProcessing.Done;
            }
            else
                Processing3 = StatusProcessing.Done;

            if (BeeCore.Common.listCamera[3] != null)
            {
                if (!workCheck4.IsBusy)
                    workCheck4.RunWorkerAsync();
                else
                    Processing4 = StatusProcessing.Done;
            }
            else
                Processing4 = StatusProcessing.Done;
            if (!workPlay.IsBusy)
                    workPlay.RunWorkerAsync();
        }

        private void imgView_Scroll(object sender, ScrollEventArgs e)
        {
            Global.ScaleZoom = (float)(imgView.Zoom / 100.0);
            Global.pScroll = new Point(imgView.AutoScrollPosition.X, imgView.AutoScrollPosition.Y);
        }

        private void btnRunSim_Click_1(object sender, EventArgs e)
        {
            if (Files == null) return;
            if (Files.Count == 0) return;
            
            Global.StatusMode = btnRunSim.IsCLick  ? StatusMode.SimContinuous : StatusMode.None;
            if (btnRunSim.IsCLick)
            {
                
                btnRunSim.Image = Properties.Resources.Stop;

                btnFolder.Enabled = false;
                if (indexFile >= listMat.Count) indexFile = 0;
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = listMat[indexFile];// Cv2.ImRead(Files[indexFile]);
                imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                RunProcessing();
            }
            else
            {
                btnRunSim.Image = Properties.Resources.Play_2;
                btnFolder.Enabled = true; Global.StatusMode=StatusMode.SimContinuous;
                
            }
            if(indexFile >= Files.Count)
            indexFile = 0;
            G.EditTool.lbNamefile.Text = indexFile+"."+ Path.GetFileNameWithoutExtension(Files[indexFile]);

        }

        private void pView_MouseLeave(object sender, EventArgs e)
        {
            //G.IsCheck = true;
           // imgView.Invalidate();
        }

        private void pView_MouseMove(object sender, MouseEventArgs e)
        {

          //  G.IsCheck = false;
          //  imgView.Invalidate();
        }

        private void View_MouseHover(object sender, EventArgs e)
        {

        }

       

        private void imgView_MouseUp(object sender, MouseEventArgs e)
        {
            
            if (toolEdit == null) return;
            _drag = false;
            if(toolEdit.Propety.rotCrop!=null)
              toolEdit.Propety.rotCrop._dragAnchor = AnchorPoint.None;
          
                ToolMouseUp();
            imgView.Invalidate();
            
        }
    }
}
