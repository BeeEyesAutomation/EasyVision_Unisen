using BeeCore;
using BeeGlobal;
using BeeInterface;
using Cyotek.Windows.Forms;
using Newtonsoft.Json.Linq;
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
    public partial class ToolMultiOnnx : UserControl
    {

        public ToolMultiOnnx()
        {
            InitializeComponent();
        }
        public void LoadPara()
        {
         
            Propety = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Propety;
            txtModel.Text = Propety.pathChipOnnx;
            AdjLimitLeft.Value = Propety.LimitXSub;
            AdjOffSetBox.Value = Propety.OffSetBoxLine;
            AdjOffSetBR.Value = Propety.OffSetBR;
            AdjAspectBox.Value = Propety.AspectBox;
            AdjBinary.Value = Propety.ThresholdBinary;
            AdjBinary.Visible= Propety.MethordEdge == MethordEdge.Binary ? true : false;
            btnLineBot.IsCLick = Propety.CornerAdj == CornerAdj.Bottom ? true : false;
            btnLineRight.IsCLick = Propety.CornerAdj == CornerAdj.Right ? true : false;
            btnLineMid.IsCLick = Propety.CornerAdj == CornerAdj.MidBotRight ? true : false;
            AdThreshStrong.Visible = Propety.MethordEdge == MethordEdge.StrongEdges ? true : false;
            AdThreshStrong.Value = Propety.ThreshStrongRight;
            btnEdgeNormal.IsCLick = Propety.MethordEdge == MethordEdge.CloseEdges ?true: false;
            btnEdgeStrong.IsCLick = Propety.MethordEdge == MethordEdge.StrongEdges ? true : false;
            btnBinary.IsCLick = Propety.MethordEdge == MethordEdge.Binary ? true : false;
          
            AdjScoreNG.Value = Propety.ScoreYolo;
          

            btnOffBlackDot.IsCLick = !Propety.IsBlackDot;
            btnOnBlackDot.IsCLick = Propety.IsBlackDot;
            Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
         

         

           
            trackScore.Min = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].MinValue;
            trackScore.Max = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].MaxValue;
            trackScore.Step = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StepValue;
            trackScore.Value = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Score;
         
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

          
            AdjScoreLearning.Value = Propety.ScoreYolo;

            AdjScale.Value = Propety.Scale;
         
            AdjLimitXRight.Value = Propety.LimitX;//
            AdjLimitY.Value = Propety.LimitY;//

            AdjWidthBoxBR.Value = Propety.WidthDetectBoxBR;
          
            AdjWidthDetect.Value = Propety.WidthDetectBox;
            AdjArea.Value = Propety.minArea;
            Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StatusToolChanged += ToolMultiPattern_StatusToolChanged;
    
        }

        private void ToolMultiPattern_StatusToolChanged(StatusTool obj)
        {
            if (Global.IsRun) return;
            if (Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StatusTool == StatusTool.Done)
            {
                btnTest.Enabled = true;
            }
        }

        private void trackScore_ValueChanged(float obj)
        {
            Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Score = (float)trackScore.Value;


        }

        public MultiOnnx Propety = new MultiOnnx();

        private void btnTest_Click(object sender, EventArgs e)
        {

         //   btnTest.Enabled = false;
            if (!Common.PropetyTools[Global.IndexProgChoose][Propety.Index].worker.IsBusy)
                Common.PropetyTools[Global.IndexProgChoose][Propety.Index].worker.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }
        bool IsFullSize = false;

        private void btnLess_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.Less;
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
            Propety.LimitCounter = (int)AdjLimitY.Value;
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
            var prop = BeeCore.Common.PropetyTools[Global.IndexProgChoose][Global.IndexToolSelected].Propety;
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


      

        private void workLoadModel_DoWork(object sender, DoWorkEventArgs e)
        {
            Propety.SetModel();
            //if (Propety.PathModel != null)
            //{
            //    Propety.pathFullModel = "Program\\" + Global.Project + "\\" + Propety.pathChipOnnx;

            //    if (File.Exists(Propety.pathFullModel))
            //    {
            //        Propety.SetModel();
            //    }
            //}

        }

        private void btn6_Click(object sender, EventArgs e)
        {
            trackScore.Visible = !btn6.IsCLick;
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            //layLimitCouter.Visible = !btn9.IsCLick;
            AdjLimitY.Visible = !btn9.IsCLick;
        }

        private void btnZeroAdj_Click(object sender, EventArgs e)
        {
            Propety.ZeroPos = ZeroPos.ZeroADJ;
        }

     
        private void btn7_Click_1(object sender, EventArgs e)
        {
            AdjScale.Visible = !btn7.IsCLick;
            AdjScale.Visible = !btn7.IsCLick;
        }

        private void btnZero0_Click(object sender, EventArgs e)
        {
            Propety.ZeroPos = ZeroPos.Zero;
        }

        private void btnCalib_Click(object sender, EventArgs e)
        {
            Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Propety.IsCalibs = true;
     //       btnTest.Enabled = false;
            if (!Common.PropetyTools[Global.IndexProgChoose][Propety.Index].worker.IsBusy)
                Common.PropetyTools[Global.IndexProgChoose][Propety.Index].worker.RunWorkerAsync();
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



   
     







   

        private void AdjLimitX_ValueChanged(float obj)
        {
            Propety.LimitX = AdjLimitXRight.Value;
        }

        private void AdjLimitY_ValueChanged(float obj)
        {
            Propety.LimitY = AdjLimitY.Value;
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            AdjLimitY.Visible = !btn9.IsCLick;
        }

     
    
      

      
      

      
        private void btnMergeBox_Click(object sender, EventArgs e)
        {
            Propety.FilterBox = BeeGlobal.FilterBox.Merge;
            AdjOverLap.Enabled = true;
        }

        private void btnRemoveBox_Click(object sender, EventArgs e)
        {
            Propety.FilterBox = BeeGlobal.FilterBox.Remove;
            AdjOverLap.Enabled = true;
        }

       
        private void btnNoneBox_Click(object sender, EventArgs e)
        {
            Propety.FilterBox = BeeGlobal.FilterBox.None;
            AdjOverLap.Enabled = false;
        }

        private void AdjOverLap_ValueChanged(float obj)
        {
            Propety.ThreshOverlap = AdjOverLap.Value;
        }

       
        bool IsReload;

        private void btnAddModel_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                String NameModel = new DirectoryInfo(
                            Path.GetDirectoryName(OpenFileDialog.FileName)
                        ).Name;
                //pathModel =Path.GetPathRoot(OpenFileDialog.FileName);
                //NameModel = Path.GetDirectoryName(OpenFileDialog.FileName);// Path.GetFileNameWithoutExtension(OpenFileDialog.FileName);
                String pathModel = "Program\\" + Global.Project + "\\" + NameModel;
                Batch.CopyAndRename(Path.GetDirectoryName(OpenFileDialog.FileName), pathModel, false);
                Propety.pathChipOnnx = pathModel;
                //String pathModel = "Program\\" + Global.Project + "\\" + NameModel;
               
                //Batch.CopyAndRename(Path.GetDirectoryName(OpenFileDialog.FileName), pathModel, false);

       
               
                //Propety.pathChipOnnx = Path.GetFileName(pathModel);
                IsReload = true;
                if (!workLoadModel.IsBusy)
                    workLoadModel.RunWorkerAsync();

                txtModel.Text = Propety.pathChipOnnx;
               

            }
       
        }

        private void AdjScoreLearning_ValueChanged(float obj)
        {
            Propety.ScoreYolo=(int)AdjScoreLearning.Value;
        }

       

        private void AdjScale_ValueChanged(float obj)
        {
            Propety.Scale = (float)AdjScale.Value;

        }

        private void AdjPage_ValueChanged(float obj)
        {
           Propety.WidthDetectBoxBR = (int)AdjWidthBoxBR.Value;
        }

   
        private void AdjWidthDetect_ValueChanged(float obj)
        {
            Propety.WidthDetectBox= (int)AdjWidthDetect.Value;
        }
       

    
    
     
        private void btnOnBlackDot_Click(object sender, EventArgs e)
        {
            Propety.IsBlackDot = btnOnBlackDot.IsCLick;
        }

        private void btnOffBlackDot_Click(object sender, EventArgs e)
        {
            Propety.IsBlackDot =! btnOffBlackDot.IsCLick;
        }

        private void rjButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                String pathModel = OpenFileDialog.FileName;
                String NameModel = new DirectoryInfo(
                          Path.GetDirectoryName(OpenFileDialog.FileName)
                      ).Name;
                pathModel = "Program\\" + Global.Project + "\\" + NameModel;
                Batch.CopyAndRename(Path.GetDirectoryName(OpenFileDialog.FileName), pathModel, false);
                Propety.pathBlackDot = pathModel;
                IsReload = true;
                if (!workLoadModel.IsBusy)
                    workLoadModel.RunWorkerAsync();
                MessageBox.Show(Propety.pathBlackDot);
            }
        }

        private void AdjScoreNG_ValueChanged(float obj)
        {
            Propety.ScoreYolo =(int) AdjScoreNG.Value;
        }

      

        private void btnEdgeNormal_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.CloseEdges;
            AdjBinary.Visible = Propety.MethordEdge == MethordEdge.Binary ? true : false;
            AdThreshStrong.Visible = Propety.MethordEdge == MethordEdge.StrongEdges ? true : false;
        }

        private void btnEdgeStrong_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.StrongEdges;
            AdjBinary.Visible = Propety.MethordEdge == MethordEdge.Binary ? true : false;
            AdThreshStrong.Visible = Propety.MethordEdge == MethordEdge.StrongEdges ? true : false;
        }

        private void AdThreshStrong_ValueChanged(float obj)
        {
            Propety.ThreshStrongRight = AdThreshStrong.Value;
        }

        private void btnBinary_Click(object sender, EventArgs e)
        {
            Propety.MethordEdge = MethordEdge.Binary;
            AdjBinary.Visible = Propety.MethordEdge == MethordEdge.Binary ? true : false;
            AdThreshStrong.Visible = Propety.MethordEdge == MethordEdge.StrongEdges ? true : false;
        }

        private void AdjBinary_ValueChanged(float obj)
        {
            Propety.ThresholdBinary = (int)AdjBinary.Value;
        }

        private void AdjAspect_ValueChanged(float obj)
        {
            Propety.AspectBox=AdjAspectBox.Value;
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLineBot_Click(object sender, EventArgs e)
        {
            Propety.CornerAdj = CornerAdj.Bottom;
        }

        private void btnLineRight_Click(object sender, EventArgs e)
        {
            Propety.CornerAdj = CornerAdj.Right;
        }

        private void btnLineMid_Click(object sender, EventArgs e)
        {
            Propety.CornerAdj = CornerAdj.MidBotRight;
        }

        private void AdjOffSetBR_ValueChanged(float obj)
        {
            Propety.OffSetBR = (int)AdjOffSetBR.Value;
        }

        private void AdjArea_ValueChanged(float obj)
        {
            Propety.minArea = (int)AdjArea.Value;
        }

        private void AdjOffSetBox_ValueChanged(float obj)
        {
            Propety.OffSetBoxLine= (int)AdjOffSetBox.Value;
        }

        private void AdjLimitLeft_ValueChanged(float obj)
        {
            Propety.LimitXSub = (int)AdjLimitLeft.Value;
        }
    }
}
