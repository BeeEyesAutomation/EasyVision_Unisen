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
    public partial class ToolMultiPattern : UserControl
    {

        public ToolMultiPattern()
        {
            InitializeComponent();


        }


        public void LoadPara()
        {
            btnEnBet.IsCLick = Propety.IsAdjPostion;
            AdjScoreNG.Value = Propety.ScoreYolo;
            ckBitwiseNot.IsCLick = Propety.ckBitwiseNot;
            ckSIMD.IsCLick = Propety.ckSIMD;
            ckSubPixel.IsCLick = Propety.ckSubPixel;
            if (Propety.IsHighSpeed)
                btnHighSpeed.IsCLick = true;
            else
                btnNormal.IsCLick = true;
            btnOffBlackDot.IsCLick = !Propety.IsBlackDot;
            btnOnBlackDot.IsCLick = Propety.IsBlackDot;
            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
            trackAngle.Value = (int)Propety.Angle;


            if (Propety.Angle > 360) Propety.Angle = 360;

            if (Propety.Angle == 0)
            {
                Propety.Angle = 1;
            }
            float angle = (Propety.rotCrop._rectRotation) - (Propety.rotArea._rectRotation);
            Propety.AngleLower = angle - Propety.Angle;
            Propety.AngleUper = angle + Propety.Angle;

           
            trackScore.Min = Common.PropetyTools[Global.IndexChoose][Propety.Index].MinValue;
            trackScore.Max = Common.PropetyTools[Global.IndexChoose][Propety.Index].MaxValue;
            trackScore.Step = Common.PropetyTools[Global.IndexChoose][Propety.Index].StepValue;
            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
            if (Propety.MaxObject == 0) Propety.MaxObject = 1;
           
          
          
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

          
            AdjScoreLearning.Value = Propety.ScoreYolo;

            AdjScale.Value = Propety.Scale;
            adjOffsetX.Value = Propety.ExpandX;//
            adjOffsetY.Value = Propety.ExpandY;//
          
            AdjLimitX.Value = Propety.LimitX;//
            AdjLimitY.Value = Propety.LimitY;//

            AdjPage.Value = Propety.ExpandPage;
            AdjSample.Value = Propety.ExpandPattern;
            AdjWidthDetect.Value = Propety.WidthDetectBox;
            if (cbListModel.InvokeRequired)
            {
                cbListModel.Invoke(new Action(() =>
                {
                    cbListModel.DataSource = Propety.listModels;
                }));
            }
            else
            {
                cbListModel.DataSource = Propety.listModels;
            }

            if (Propety.PathModel != "")
                cbListModel.Text = Propety.PathModel;
           
            RefreshLabels();
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

        public MultiPattern Propety = new MultiPattern();



     



     







     

        private void btnTest_Click(object sender, EventArgs e)
        {

         //   btnTest.Enabled = false;
            if (!Common.PropetyTools[Global.IndexChoose][Propety.Index].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Propety.Index].worker.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }
        bool IsFullSize = false;
      



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
     

      

        private void workLoadModel_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Propety.PathModel != null)
            {
                Propety.pathFullModel = "Program\\" + Global.Project + "\\" + Propety.PathModel;

                if (File.Exists(Propety.pathFullModel))
                {
                    Propety.SetModel();
                }
            }

        }

    
        private void trackNumObject_Load(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }


      
    


       
        private void btn4_Click(object sender, EventArgs e)
        {
            trackAngle.Visible = !btn4.IsCLick;
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
            Common.PropetyTools[Global.IndexChoose][Propety.Index].Propety.IsCalibs = true;
     //       btnTest.Enabled = false;
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



        private void adjOffsetX_ValueChanged(float obj)
        {
            Propety.ExpandX = adjOffsetX.Value;
        }

        private void adjOffsetY_ValueChanged(float obj)
        {
            Propety.ExpandY = adjOffsetY.Value;
        }

     







   

        private void AdjLimitX_ValueChanged(float obj)
        {
            Propety.LimitX = AdjLimitX.Value;
        }

        private void AdjLimitY_ValueChanged(float obj)
        {
            Propety.LimitY = AdjLimitY.Value;
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            AdjLimitY.Visible = !btn9.IsCLick;
        }

     
    
      

        private void cbListModel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Propety.PathModel = cbListModel.SelectedValue.ToString();//.Text;
            Propety.pathFullModel = "Program\\" + Global.Project + "\\" + Propety.PathModel;
            if (!File.Exists(Propety.pathFullModel))
            {
                Propety.listModels.Remove(Propety.PathModel);
                if (Propety.listModels.Count > 0)
                {
                    Propety.PathModel = Propety.listModels[Propety.listModels.Count - 1];
                    Propety.pathFullModel = "Program\\" + Global.Project + "\\" + Propety.PathModel;

                }
                else
                {
                    Propety.PathModel = "";
                    Propety.pathFullModel = "";
                }
                cbListModel.DataSource = null;
                cbListModel.DataSource = Propety.listModels;
                cbListModel.Refresh();
                cbListModel.Text = Propety.PathModel;
            }

            if (File.Exists(Propety.pathFullModel))
            {
                cbListModel.Enabled = false;

                workLoadModel.RunWorkerAsync();
                //  Propety.listLabelCompare = new List<Labels>();
                //RefreshLabels();

            }
        }
        public void RefreshLabels()
        {
            if (Propety.labelItems == null)
                return;
            DashLabels.Items.Clear();
            foreach (LabelItem labelItem in Propety.labelItems)
                DashLabels.Items.Add(labelItem);

        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure", "Reload All Para of Label", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                String[] Content = Propety.LoadNameModel(Common.PropetyTools[Global.IndexChoose][Propety.Index].Name);
                if (Content != null && Content.Length > 0)
                {
                    Propety.labelItems = new List<LabelItem>();
                    foreach (String label in Content)
                    {
                        if (label == "") continue;
                        Propety.labelItems.Add(new LabelItem(label));
                    }
                    RefreshLabels();
                }
                else
                {
                    MessageBox.Show("Check File Class Again", "Error");
                }
            }
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

        private void btnRemoveModel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure", "Delete Model", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Propety.PathModel = Propety.listModels[Propety.listModels.Count - 1];
                String pathModel = "Program\\" + Global.Project + "\\" + Propety.PathModel;
                if (File.Exists(pathModel))
                {
                    File.Delete(pathModel);
                }
                Propety.listModels.Remove(Propety.PathModel);
                Propety.listModels = Propety.listModels.Distinct().ToList();
                cbListModel.DataSource = null;
                cbListModel.DataSource = Propety.listModels.ToArray();
                if (Propety.listModels.Count > 0)
                {
                    Propety.PathModel = Propety.listModels[Propety.listModels.Count - 1];
                    cbListModel.Text = Propety.PathModel;
                    if (!workLoadModel.IsBusy)
                        workLoadModel.RunWorkerAsync();
                }
                else
                    cbListModel.Text = "";
            }
        }
        bool IsReload;

        private void btnAddModel_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                String pathModel = OpenFileDialog.FileName;

                String NameModel = Path.GetFileName(pathModel);
                pathModel = "Program\\" + Global.Project + "\\" + NameModel;
                if (Propety.listModels == null) Propety.listModels = new List<string>();
                if (File.Exists(OpenFileDialog.FileName))
                {
                    File.Copy(OpenFileDialog.FileName, pathModel, true);
                    Propety.listModels.Add(NameModel);
                    Propety.listModels = Propety.listModels.Distinct().ToList();
                    cbListModel.DataSource = null;
                    Propety.PathModel = Path.GetFileName(pathModel);
                    IsReload = true;
                    if (!workLoadModel.IsBusy)
                        workLoadModel.RunWorkerAsync();
                    cbListModel.DataSource = Propety.listModels.ToArray();
                    cbListModel.Text = Propety.PathModel;
                  
                }
            }
            //switch (StepEdit)
            //{
            //    case StepSetModel.SetModel:


            //        break;
            //    case StepSetModel.SetLabels:



            //        break;
            //}
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
            Propety.ExpandPage = (int)AdjPage.Value;
        }

        private void AdjSample_ValueChanged(float obj)
        {
            Propety.ExpandPattern = (int)AdjSample.Value;
        }

        private void AdjWidthDetect_ValueChanged(float obj)
        {
            Propety.WidthDetectBox= (int)AdjWidthDetect.Value;
        }
        private void ckSIMD_Click(object sender, EventArgs e)
        {
            Propety.ckSIMD = !Propety.ckSIMD;
            if (Propety.ckSIMD)
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
        private void btnNormal_Click(object sender, EventArgs e)
        {
            Propety.IsHighSpeed = false;
        }

        private void btnHighSpeed_Click(object sender, EventArgs e)
        {
            Propety.IsHighSpeed = true;

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
             String   NameModel = new DirectoryInfo(
                          Path.GetDirectoryName(OpenFileDialog.FileName)
                      ).Name;
                //pathModel =Path.GetPathRoot(OpenFileDialog.FileName);
                //NameModel = Path.GetDirectoryName(OpenFileDialog.FileName);// Path.GetFileNameWithoutExtension(OpenFileDialog.FileName);
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

        private void btnEnBet_Click(object sender, EventArgs e)
        {
            Propety.IsAdjPostion =btnEnBet.IsCLick;
        }
    }
}
