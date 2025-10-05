﻿using BeeCore;
using BeeGlobal;
using BeeInterface;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
using OpenCvSharp.ML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolVisualMatch : UserControl
    {
        
        public ToolVisualMatch( )
        {
            InitializeComponent();
           
           
        }
        

        public void LoadPara()
        {

            if (Propety.bmRaw != null)
            {
				imgTemp.Image = Propety.bmRaw;
			}
			
			Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
          

            trackScore.Min = Common.PropetyTools[Global.IndexChoose][Propety.Index].MinValue;
            trackScore.Max = Common.PropetyTools[Global.IndexChoose][Propety.Index].MaxValue;
            trackScore.Step = Common.PropetyTools[Global.IndexChoose][Propety.Index].StepValue;
            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;


            AdjColorTolerance.Value = Propety.ColorTolerance;
            AdjMaxDiffPixels.Value=Propety.MaxDiffPixels;
            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusToolChanged += ToolPattern_StatusToolChanged;
            btnArea.IsCLick = true;
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            btnElip.IsCLick = Propety.rotArea.Shape == ShapeType.Ellipse ? true : false;
            btnRect.IsCLick = Propety.rotArea.Shape == ShapeType.Rectangle ? true : false;
            btnHexagon.IsCLick = Propety.rotArea.Shape == ShapeType.Hexagon ? true : false;
            btnPolygon.IsCLick = Propety.rotArea.Shape == ShapeType.Polygon ? true : false;
            btnWhite.IsCLick = Propety.rotArea.IsWhite;
            btnBlack.IsCLick = !Propety.rotArea.IsWhite;
        }

        private void btnCropRect_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Crop;
            Propety.TypeCrop = Global.TypeCrop;
            btnElip.IsCLick = Propety.rotCrop.Shape == ShapeType.Ellipse ? true : false;
            btnRect.IsCLick = Propety.rotCrop.Shape == ShapeType.Rectangle ? true : false;
            btnHexagon.IsCLick = Propety.rotCrop.Shape == ShapeType.Hexagon ? true : false;
            btnPolygon.IsCLick = Propety.rotCrop.Shape == ShapeType.Polygon ? true : false;
            btnWhite.IsCLick = Propety.rotCrop.IsWhite;
            btnBlack.IsCLick = !Propety.rotCrop.IsWhite;

        }

        private void btnCropArea_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            btnElip.IsCLick = Propety.rotArea.Shape == ShapeType.Ellipse ? true : false;
            btnRect.IsCLick = Propety.rotArea.Shape == ShapeType.Rectangle ? true : false;
            btnHexagon.IsCLick = Propety.rotArea.Shape == ShapeType.Hexagon ? true : false;
            btnPolygon.IsCLick = Propety.rotArea.Shape == ShapeType.Polygon ? true : false;
            btnWhite.IsCLick = Propety.rotArea.IsWhite;
            btnBlack.IsCLick = !Propety.rotArea.IsWhite;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Mask;
            Propety.TypeCrop = Global.TypeCrop;
            if (Propety.rotMask == null)
            {
                Propety.rotMask = DataTool.NewRotRect(TypeCrop.Mask); ;
            }
            btnElip.IsCLick = Propety.rotMask.Shape == ShapeType.Ellipse ? true : false;
            btnRect.IsCLick = Propety.rotMask.Shape == ShapeType.Rectangle ? true : false;
            btnHexagon.IsCLick = Propety.rotMask.Shape == ShapeType.Hexagon ? true : false;
            btnPolygon.IsCLick = Propety.rotMask.Shape == ShapeType.Polygon ? true : false;
            btnWhite.IsCLick = Propety.rotArea.IsWhite;
            btnBlack.IsCLick = !Propety.rotArea.IsWhite;


        }
        private void btnNone_Click(object sender, EventArgs e)
        {
            switch (Global.TypeCrop)
            {
                //case TypeCrop.Crop:
                //    Propety.rotCrop.Shape= btnElip.IsCLick==true ? ShapeType.Ellipse: ShapeType.Rectangle;
                //    break;
                //case TypeCrop.Area:
                //    Propety.rotArea.Shape= btnElip.IsCLick==true ? ShapeType.Ellipse: ShapeType.Rectangle;
                //    break;
                case TypeCrop.Mask:
                    Propety.rotMask = null;// = btnElip.IsCLick;
                    break;

            }
            //  G.EditTool.View.imgView.Invalidate();
        }

        private void SetShapeFor(TypeCrop which, ShapeType shape)
        {

            RectRotate rr = null;
            if (which == TypeCrop.Area) { if (Propety.rotArea == null) Propety.rotArea = new RectRotate(); rr = Propety.rotArea; }
            else if (which == TypeCrop.Mask) { if (Propety.rotMask == null) Propety.rotMask = new RectRotate(); rr = Propety.rotMask; }
            else { if (Propety.rotCrop == null) Propety.rotCrop = new RectRotate(); rr = Propety.rotCrop; }

            rr.Shape = shape;
            if (shape == ShapeType.Polygon)
            {
                if (rr.PolyLocalPoints == null || rr.PolyLocalPoints.Count() == 0)
                    NewShape(shape);
                else
                {
                    rr.UpdateFromPolygon(true);
                }
            }
            if (shape == ShapeType.Hexagon)
            {
                if (rr.HexVertexOffsets == null || rr.HexVertexOffsets.Count() == 0)
                    NewShape(shape);
            }


            Global.TypeCrop = which;
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;



        }
        private void NewShape(ShapeType newShape)
        {
            // 1) Chốt shape hiện tại
            var prop = BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety;
            RectRotate rr = null;
            if (Global.TypeCrop == TypeCrop.Area) rr = prop?.rotArea;
            else if (Global.TypeCrop == TypeCrop.Mask) rr = prop?.rotMask;
            else rr = prop?.rotCrop;

            if (rr != null)
            {
                // Nếu đang drag: chấm dứt
                rr._dragAnchor = AnchorPoint.None;
                rr.ActiveVertexIndex = -1;

                // Nếu là polygon đang dựng dở
                if (rr.Shape == ShapeType.Polygon && rr.IsPolygonClosed == false)
                {
                    // CHỌN 1 TRONG 3 CHÍNH SÁCH:

                    // (A) Giữ tạm nguyên trạng (không chuẩn hoá, không xoá điểm)
                    // -> Không làm gì thêm

                    // (B) Tự đóng & chuẩn hoá (nếu muốn)
                    // nếu có >=3 điểm thì tự đóng:
                    // if (rr.PolyLocalPoints != null && rr.PolyLocalPoints.Count >= 3) {
                    //     var p0 = rr.PolyLocalPoints[0];
                    //     rr.PolyLocalPoints.Add(p0);
                    //     rr.IsPolygonClosed = true;
                    //     rr.UpdateFromPolygon(updateAngle: rr.AutoOrientPolygon);
                    // }

                    // (C) Huỷ polygon đang dựng
                    // rr.PolygonClear();
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

        ShapeType ShapeType = ShapeType.Rectangle;
        private void btnHexagon_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Hexagon;
            SetShapeFor(Global.TypeCrop, ShapeType);
        }

        private void btnPolygon_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Polygon;

            SetShapeFor(Global.TypeCrop, ShapeType);
        }

        private void btnNewShape_Click(object sender, EventArgs e)
        {
            NewShape(ShapeType);
        }
        private void btnRect_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Rectangle;
            SetShapeFor(Global.TypeCrop, ShapeType);

        }

        private void btnElip_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Ellipse;
            SetShapeFor(Global.TypeCrop, ShapeType);

        }

        private void btnWhite_Click(object sender, EventArgs e)
        {
            switch (Global.TypeCrop)
            {
                case TypeCrop.Area:
                    Propety.rotArea.IsWhite = btnWhite.IsCLick;
                    break;
                case TypeCrop.Crop:
                    Propety.rotCrop.IsWhite = btnWhite.IsCLick;
                    break;
            }

        }

        private void btnBlack_Click(object sender, EventArgs e)
        {
            switch (Global.TypeCrop)
            {
                case TypeCrop.Area:
                    Propety.rotArea.IsWhite = !btnBlack.IsCLick;
                    break;
                case TypeCrop.Crop:
                    Propety.rotCrop.IsWhite = !btnBlack.IsCLick;
                    break;
            }

        }

        private void ToolPattern_StatusToolChanged(StatusTool obj)
        {
            if (Global.IsRun) return;
            if (Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool == StatusTool.Done)
            {
                btnTest.Enabled = true;
            }
        }

        private void trackScore_ValueChanged(float obj)
        {
            Common.PropetyTools[Global.IndexChoose][Propety.Index].Score = (float)trackScore.Value;
           

        }

        public VisualMatch Propety =new VisualMatch();
        public Mat matTemp = new Mat();
        public Mat matTemp2 = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
        public void GetTemp(RectRotate rotateRect, Mat matRegister)
        {
           
                float angle = rotateRect._rectRotation;
                if (rotateRect._rectRotation < 0) angle = 360 + rotateRect._rectRotation;
                Mat matCrop =BeeCore.Common.CropRotatedRectSharp(matRegister, new RotatedRect(new Point2f(rotateRect._PosCenter.X + (rotateRect._rect.Width / 2 + rotateRect._rect.X), rotateRect._PosCenter.Y + (rotateRect._rect.Height / 2 + rotateRect._rect.Y)), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), angle));
                if (matCrop.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(matCrop, matTemp, ColorConversionCodes.BGR2GRAY);
               
        }
       
        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {

        }

      
     

        private void trackScore_MouseUp(object sender, MouseEventArgs e)
        {
           

            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

       
  
    
        private void btnCancel_Click(object sender, EventArgs e)
        {
          //  G.IsCancel = true;
            
          //  G.EditTool.RefreshGuiEdit(Step.Step3);
        }

       

        

        private void btnOK_Click(object sender, EventArgs e)
        {
          
         //   G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

      

        private void btnLearning_Click(object sender, EventArgs e)
        {

            if (Propety.rotArea != null)
                if (Propety.rotArea._rect.Width != 0 && Propety.rotArea._rect.Height != 0)
                {
                    Propety.bmRaw = Propety.LearnPattern(BeeCore.Common.listCamera[Propety.IndexThread].matRaw.Clone(), false).ToBitmap();
                    imgTemp.Image = Propety.bmRaw;
                }



        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;
            if (!Common.PropetyTools[Global.IndexChoose][Propety.Index].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Propety.Index].worker.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }
        bool IsFullSize = false;
        private void btnCropHalt_Click(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.Check;
           
        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.ParaCommon.SizeCCD.Width / 2, -Global.ParaCommon.SizeCCD.Height / 2, Global.ParaCommon.SizeCCD.Width, Global.ParaCommon.SizeCCD.Height), new PointF(Global.ParaCommon.SizeCCD.Width / 2, Global.ParaCommon.SizeCCD.Height / 2), 0, AnchorPoint.None);

            
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            Global.StatusDraw = StatusDraw.Check;
         
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

      
        private void rjButton3_Click_1(object sender, EventArgs e)
        {

        }

     

        private void rjButton5_Click(object sender, EventArgs e)
        {

        }

   

        private void btnLess_Click(object sender, EventArgs e)
        {
            Propety.Compare=Compares.Less;
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.Equal;
        }

        private void btnMore_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.More;
        }

   
        private void numLimitCounter_ValueChanged(float obj)
        {
            Propety.LimitCounter = (int)numLimitCounter.Value;
        }

        //private void btnLimitCounter_Click(object sender, EventArgs e)
        //{
        //    Propety.IsLimitCouter = btnLimitCounter.IsCLick;
        //    layLimitCouter.Enabled = btnLimitCounter.IsCLick;
        //}

    

        private void workLoadModel_DoWork(object sender, DoWorkEventArgs e)
        {
            Patterns.LoadEdge();
          
        }

   
        private void workLoadModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
          
            //    Bitmap bmTemp = Propety.bmTemp;
            //imgTemp.Image = bmTemp;

            //if (bmTemp != null)
            //{
            //    Propety.LearnPattern(OpenCvSharp.Extensions.BitmapConverter.ToMat(bmTemp));


            //}
        }

        private void trackNumObject_Load(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void trackAngle_Click(object sender, EventArgs e)
        {

        }

        private void trackMaxOverLap_ValueChanged(float obj)
        {

        }

        private void AdjMaxDiffPixels_ValueChanged(float obj)
        {
            Propety.MaxDiffPixels=(int)AdjMaxDiffPixels.Value;
        }

        private void trackNumObject_ValueChanged(float obj)
        {

        }

        private void AdjColorTolerance_ValueChanged(float obj)
        {
            Propety.ColorTolerance = (int)AdjColorTolerance.Value;
        }
    }
}
