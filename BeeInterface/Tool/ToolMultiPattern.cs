using BeeCore;
using BeeGlobal;
using BeeInterface;
using Cyotek.Windows.Forms;
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
    public partial class ToolMultiPattern : UserControl
    {
        
        public ToolMultiPattern( )
        {
            InitializeComponent();
           
           
        }
        

        public void LoadPara()
        {


            //if (!workLoadModel.IsBusy)
            //    workLoadModel.RunWorkerAsync();
            if (Propety.IsColorPixel)
                btnVisualMatch.IsCLick = true;
            else
                btnVisualMatch.IsCLick = false;
            if (Propety.bmRaw != null)
            {
                imgTemp.Image = Propety.bmRaw;
            }
            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
            trackAngle.Value =(int) Propety.Angle;
           

            if (Propety.Angle > 360) Propety.Angle = 360;

            if (Propety.Angle == 0)
            {
                Propety.Angle = 1;
            }
            float angle = (Propety.rotCrop._rectRotation) - (Propety.rotArea._rectRotation);
            Propety.AngleLower = angle - Propety.Angle;
            Propety.AngleUper = angle + Propety.Angle;

            btnWhite.IsCLick = Propety.rotArea.IsWhite;
            btnBlack.IsCLick =! Propety.rotArea.IsWhite;
            trackScore.Min = Common.PropetyTools[Global.IndexChoose][Propety.Index].MinValue;
            trackScore.Max = Common.PropetyTools[Global.IndexChoose][Propety.Index].MaxValue;
            trackScore.Step = Common.PropetyTools[Global.IndexChoose][Propety.Index].StepValue;
            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
            if (Propety.MaxObject == 0) Propety.MaxObject = 1;
            AdjMaximumObj.Value = Propety.MaxObject;
            AdjStepAngle.Value = Propety.StepAngle;
            AdjLimitCounter.Value= Propety.LimitCounter;
            trackMaxOverLap.Value = (int)(Propety.OverLap * 100);
           
            ckBitwiseNot.IsCLick = Propety.ckBitwiseNot;
            ckSIMD.IsCLick = Propety.ckSIMD;
            ckSubPixel.IsCLick = Propety.ckSubPixel;
          switch(Propety.Compare)
            {
                case Compares.Equal:
                    btnEqual.IsCLick = true;
                    break;
                case Compares.Less:
                    btnLess.IsCLick = true;
                    break;
                case Compares.More:
                    btnMore.IsCLick = true;
                    break;
            }    
            if (Propety.IsHighSpeed)
                btnHighSpeed.IsCLick = true;
            else
                btnNormal.IsCLick = true;
            AdjMorphology.Value = Propety.SizeClose;
            AdjOpen.Value = Propety.SizeOpen;
            AdjClearNoise.Value = Propety.SizeClearsmall;
            AdjClearBig.Value = Propety.SizeClearBig;
          
            btnClose.IsCLick = Propety.IsClose;
            btnOpen.IsCLick = Propety.IsOpen;
            btnIsClearSmall.IsCLick = Propety.IsClearNoiseSmall;
            btnIsClearBig.IsCLick = Propety.IsClearNoiseBig;
            AdjClearNoise.Enabled = Propety.IsClearNoiseSmall;
            AdjClearBig.Enabled = Propety.IsClearNoiseBig;
            AdjOpen.Enabled = Propety.IsOpen;
            AdjMorphology.Enabled = Propety.IsClose;
            btnArea.IsCLick = true;
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            btnElip.IsCLick = Propety.rotArea.Shape == ShapeType.Ellipse ? true : false;
            btnRect.IsCLick = Propety.rotArea.Shape == ShapeType.Rectangle ? true : false;
            btnHexagon.IsCLick = Propety.rotArea.Shape == ShapeType.Hexagon ? true : false;
            btnPolygon.IsCLick = Propety.rotArea.Shape == ShapeType.Polygon ? true : false;
            btnWhite.IsCLick = Propety.rotArea.IsWhite;
            btnBlack.IsCLick = !Propety.rotArea.IsWhite;
          
            adjScale.Value = Propety.Scale;
            adjOffsetX.Value = Propety.ExpandX;//
            adjOffsetY.Value = Propety.ExpandY;//
            adjScalePixel.Value = Propety.ScalePixel;//
            adjColorPixel.Value = Propety.PxTemp;//
            btnZero0.IsCLick=Propety.ZeroPos==ZeroPos.Zero?true:false;
            btnZeroAdj.IsCLick = Propety.ZeroPos == ZeroPos.ZeroADJ ? true : false;

            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusToolChanged += ToolMultiPattern_StatusToolChanged;
        }

        private void ToolMultiPattern_StatusToolChanged(StatusTool obj)
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

        public MultiPattern Propety=new MultiPattern();
 
     
   
        private void ckSIMD_Click(object sender, EventArgs e)
        {
            Propety.ckSIMD = !Propety.ckSIMD;
              if(Propety.ckSIMD)
                {
                ckSIMD.BackColor = Color.Goldenrod;
                ckSIMD.BorderColor = Color.DarkGoldenrod;
                }
                else
                { 
                ckSIMD.BackColor = Color.WhiteSmoke;
                ckSIMD.BorderColor = Color.Silver;
                ckSIMD.TextColor = Color.Black;
                }
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

        private void ckBitwiseNot_Click(object sender, EventArgs e)
        {
            Propety.ckBitwiseNot = !Propety.ckBitwiseNot;
            if (Propety.ckBitwiseNot)
            {
                ckBitwiseNot.BackColor = Color.Goldenrod;
                ckBitwiseNot.BorderColor = Color.DarkGoldenrod;
            }
            else
            {
                ckBitwiseNot.BackColor = Color.WhiteSmoke;
                ckBitwiseNot.BorderColor = Color.Silver;
                ckBitwiseNot.TextColor = Color.Black;
            }
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

        private void ckSubPixel_Click(object sender, EventArgs e)
        {
            Propety.ckSubPixel = !Propety.ckSubPixel;
            if (Propety.ckSubPixel)
            {
                ckSubPixel.BackColor = Color.Goldenrod;
                ckSubPixel.BorderColor = Color.DarkGoldenrod;
            }
            else
            {
                ckSubPixel.BackColor = Color.WhiteSmoke;
                ckSubPixel.BorderColor = Color.Silver;
                ckSubPixel.TextColor = Color.Black;
            }
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }
     
      
     

        private void btnClear_Click(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Mask;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
          //  G.IsCancel = true;
            
          //  G.EditTool.RefreshGuiEdit(Step.Step3);
        }

    

        private void btnNormal_Click(object sender, EventArgs e)
        {
            Propety.IsHighSpeed = false;
        }

        private void btnHighSpeed_Click(object sender, EventArgs e)
        {
            Propety.IsHighSpeed = true;

        }
       

       

   
        private void btnLearning_Click(object sender, EventArgs e)
        {

            if (Propety.rotCrop != null)
                if (Propety.rotCrop._rect.Width != 0 && Propety.rotCrop._rect.Height != 0)
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
            Global.IsRun = true;//note
        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.Config.SizeCCD.Width / 2, -Global.Config.SizeCCD.Height / 2, Global.Config.SizeCCD.Width, Global.Config.SizeCCD.Height), new PointF(Global.Config.SizeCCD.Width / 2, Global.Config.SizeCCD.Height / 2), 0, AnchorPoint.None);

            
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            Global.StatusDraw = StatusDraw.Check;

        }

     

        private void trackAngle_ValueChanged(float obj)
        {
            Propety.Angle = trackAngle.Value;
          
            if (Propety.Angle > 360) Propety.Angle = 360;
            if (Propety.Angle == 0)
            {
                Propety.Angle = 1;
            }
            float angle = (Propety.rotCrop._rectRotation) - (Propety.rotArea._rectRotation);
            Propety.AngleLower = angle - Propety.Angle;
            Propety.AngleUper = angle + Propety.Angle;

        }

  
        private void trackMaxOverLap_ValueChanged(float obj)
        {

           Propety.OverLap= trackMaxOverLap.Value/100.0 ;
          
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
            Propety.LimitCounter = (int)AdjLimitCounter.Value;
        }
   


        private void SetShapeFor(TypeCrop which, ShapeType shape)
        {
            
            RectRotate rr = null;
            if (which == TypeCrop.Area) { if (Propety.rotArea == null) Propety.rotArea = new RectRotate(); rr = Propety.rotArea; }
            else if (which == TypeCrop.Mask) { if (Propety.rotMask == null) Propety.rotMask = new RectRotate(); rr = Propety.rotMask; }
            else { if (Propety.rotCrop == null) Propety.rotCrop = new RectRotate(); rr = Propety.rotCrop; }

            rr.Shape = shape;
            if(shape==ShapeType.Polygon)
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

       
        private void btnModeEdge_Click(object sender, EventArgs e)
        {
            Propety.TypeMode = Mode.Edge;
        }
     

        private void btnModePattern_Click(object sender, EventArgs e)
        {
            Propety.TypeMode = Mode.Pattern;
        }
        private void AdjThreshod_ValueChanged(float obj)
        {
            Propety.ThresholdBinary = (int)AdjThreshod.Value;
        }

        private void btnInvert_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.InvertBinary;
            layThreshod.Enabled = true;
        }

        private void btnStrongEdge_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.StrongEdges;
            layThreshod.Enabled = false;
        }

        private void btnCloseEdge_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.CloseEdges;
            layThreshod.Enabled = false;
        }
        private void btnBinary_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.Binary;
            layThreshod.Enabled = true;
        }

        private void workLoadModel_DoWork(object sender, DoWorkEventArgs e)
        {
          //  Patterns.LoadEdge();
          
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            switch (Global. TypeCrop)
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

        private void workLoadModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (Propety.TypeMode == Mode.Edge)
            //    if(!G.IniEdge)
            //    {
            //        workLoadModel.RunWorkerAsync();
            //        return;

            //    }    
          
          
        }

        private void trackNumObject_Load(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

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
            switch(Global.TypeCrop)
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
                    Propety.rotArea.IsWhite =! btnBlack.IsCLick;
                    break;
                case TypeCrop.Crop:
                    Propety.rotCrop.IsWhite =! btnBlack.IsCLick;
                    break;
            }
          
        }
        private void AdjClearNoise_ValueChanged(float obj)
        {
            Propety.SizeClearsmall = (int)AdjClearNoise.Value;
        }

        private void btnEnMorphology_Click(object sender, EventArgs e)
        {
            Propety.IsClose = btnClose.IsCLick;
            AdjMorphology.Enabled = Propety.IsClose;
        }

        private void btnEnableNoise_Click(object sender, EventArgs e)
        {

            Propety.IsClearNoiseSmall = btnIsClearSmall.IsCLick;
            AdjClearNoise.Enabled = Propety.IsClearNoiseSmall;
        }

        private void AdjMorphology_ValueChanged(float obj)
        {

            Propety.SizeClose = (int)AdjMorphology.Value;
        }
        private void AdjOpen_ValueChanged(float obj)
        {
            Propety.SizeOpen = (int)AdjOpen.Value;
        }

        private void AdjClearBig_ValueChanged(float obj)
        {
            Propety.SizeClearBig = (int)AdjClearBig.Value;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Propety.IsOpen = btnOpen.IsCLick;
            AdjOpen.Enabled = Propety.IsOpen;
        }

        private void btnIsClearBig_Click(object sender, EventArgs e)
        {
            Propety.IsClearNoiseBig = btnIsClearBig.IsCLick;
            AdjClearBig.Enabled = Propety.IsClearNoiseBig;

        }

        private void AdjMaximumObj_ValueChanged(float obj)
        {
            Propety.MaxObject = (int)AdjMaximumObj.Value;
        }

        private void AdjStepAngle_ValueChanged(float obj)
        {

            Propety.StepAngle = (int)AdjStepAngle.Value;
        }

       
        
        private void btn1_Click(object sender, EventArgs e)
        {
            lay1.Visible =! btn1.IsCLick;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            lay2.Visible = !btn2.IsCLick;
            lb2.Visible = !btn2.IsCLick;
            lay21.Visible=!btn2.IsCLick;
            lay22.Visible=!btn2.IsCLick;
            lay23.Visible = !btn2.IsCLick;
            
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            lay3.Visible = !btn3.IsCLick;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            trackAngle.Visible = !btn4.IsCLick;
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            lay5.Visible =! btn5.IsCLick;
        }

      

        private void btn6_Click(object sender, EventArgs e)
        {
            trackScore.Visible = !btn6.IsCLick;
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            layLimitCouter.Visible = !btn8.IsCLick;
            AdjLimitCounter.Visible = !btn8.IsCLick;
        }

        private void btnZeroAdj_Click(object sender, EventArgs e)
        {
            Propety.ZeroPos = ZeroPos.ZeroADJ;
        }

        private void adjScale_ValueChanged(float obj)
        {
            Propety.Scale =(float)adjScale.Value;
        }

        private void btn7_Click_1(object sender, EventArgs e)
        {
            lay71.Visible = !btn7.IsCLick;
            lay72.Visible = !btn7.IsCLick;
        }

        private void btnZero0_Click(object sender, EventArgs e)
        {
            Propety.ZeroPos = ZeroPos.Zero;
        }

        private void btnCalib_Click(object sender, EventArgs e)
        {
            Propety.IsCalibs = true;
            btnTest.Enabled = false;
            if (!Common.PropetyTools[Global.IndexChoose][Propety.Index].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Propety.Index].worker.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }

        private void rjButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void adjScalePixel_ValueChanged(float obj)
        {
            Propety.ScalePixel = adjScalePixel.Value;
        }

        private void adjOffsetX_ValueChanged(float obj)
        {
            Propety.ExpandX = adjOffsetX.Value;
        }

        private void adjOffsetY_ValueChanged(float obj)
        {
            Propety.ExpandY = adjOffsetY.Value;
        }

        private void btnVisualMatch_Click(object sender, EventArgs e)
        {
            if (btnVisualMatch.IsCLick)
            {
                Propety.IsColorPixel =true;
            }
            else
            {
                Propety.IsColorPixel =false;
            }
        }

        private void adjColorPixel_ValueChanged_1(float obj)
        {

            Propety.PxTemp = adjColorPixel.Value;
        }
    }
}
