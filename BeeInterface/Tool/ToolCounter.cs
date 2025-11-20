using BeeCore;
using BeeGlobal;
using Cyotek.Windows.Forms;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Label = System.Windows.Forms.Label;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace BeeInterface
{

    [Serializable()]
    public partial class ToolCounter : UserControl
    {
        
        public ToolCounter( )
        {
            InitializeComponent();
           // tabLbs.SizeChanged += TabLbs_SizeChanged;
            
        }

        private void TabLbs_SizeChanged(object sender, EventArgs e)
        {
          //  CustomGui.RoundRg(tabLbs, 10, Corner.Bottom);
        }

        bool IsIni = false;
      
        public StepSetModel StepEdit = StepSetModel.SetModel;
      
        public void LoadPara()
        {
            try
            {

                if (Propety.listModels == null) Propety.listModels = new List<string>();
                Propety.listModels = Propety.listModels.Distinct().ToList();
                Propety.pathFullModel = "Program\\" + Global.Project + "\\" + Propety.PathModel;
                if(!File.Exists(Propety.pathFullModel))
                {
                    Propety.listModels.Remove(Propety.PathModel);
                    if(Propety.listModels.Count>0)
                    {
                        Propety.PathModel = Propety.listModels[Propety.listModels.Count - 1];
                        Propety.pathFullModel = "Program\\" + Global.Project + "\\" + Propety.PathModel;

                    }
                    else
                    {
                        Propety.PathModel = "";
                        Propety.pathFullModel ="";
                    }    
                        
                }    
                 
              IsReload = true;
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
                //txtMatching.Text = Propety.Matching;
                //btnEnbleContent.IsCLick = Propety.IsEnContent;
                //if (Propety.PathModel!=null)
                //   if (File.Exists(Propety.PathModel))
                //       Propety.SetModel(this.Name, Propety.PathModel, TypeYolo.YOLO);
                IsIni = true;
                String slabel = "";

                //   txtLabel.Text = Propety.PathLabels; ;
                RefreshLabels();

                Global.TypeCrop = TypeCrop.Area;
              //  CustomGui.RoundRg(tabLbs, 10, Corner.Bottom);
                //picTemp1.Image = Propety.matTemp;
                //picTemp2.Image = Propety.matTemp2;

                trackScore.Min = Common.PropetyTools[Global.IndexChoose][Propety.Index].MinValue;
                trackScore.Max = Common.PropetyTools[Global.IndexChoose][Propety.Index].MaxValue;
                trackScore.Step = Common.PropetyTools[Global.IndexChoose][Propety.Index].StepValue;
                trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
                numEpoch.Value = Propety.Epoch;
               
                switch (Propety.Compare)
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
                btnArrangeBox.IsCLick = Propety.IsArrangeBox;
                switch (Propety.ArrangeBox)
                {
                    case ArrangeBox.X_Left_Rigth:
                        btnX_L_R.IsCLick = true;
                        break;
                    case ArrangeBox.X_Right_Left:
                        btnX_L_R.IsCLick = true;
                        break;
                    case ArrangeBox.Y_Left_Rigth:
                        btnX_L_R.IsCLick = true;
                        break;
                    case ArrangeBox.Y_Right_Left:
                        btnX_L_R.IsCLick = true;
                        break;

                }
                Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusToolChanged += ToolYolo_StatusToolChanged;
                
            }
            catch (Exception ex)
            {
                Er = ex.Message;
            }
            //Propety.TypeMode = Propety.TypeMode;

        }
        String Er = "";

        private void ToolYolo_StatusToolChanged(StatusTool obj)
        {
            if (Global.IsRun) return;
            if (Propety.Index >= Common.PropetyTools[Global.IndexChoose].Count)
                return;
            if (Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool == StatusTool.Done)
            {
                Propety.rectTrain = Propety.rectRotates;//note
                btnTest.Enabled = true;
            }
         }

        private void trackScore_ValueChanged(float obj)
        {

            Common.PropetyTools[Global.IndexChoose][Propety.Index].Score = (int)trackScore.Value;
          
        }

        public Yolo Propety=new Yolo();
  
  
      
     
     

       
        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {

        }

        private void btnCropRect_Click(object sender, EventArgs e)
        {
          
        }

        private void btnCropArea_Click(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
  
        }

       
        private void btnCannyMin_Click(object sender, EventArgs e)
        {
            //Propety.threshMin = 180;
            //Propety.threshMax = 255;
            //Propety.LearnPattern(indexTool, matTemp);

        }

        private void btnCannyMedium_Click(object sender, EventArgs e)
        {
            //    Propety.threshMin = 100;
            //    Propety.threshMax = 255;
            //    Propety.LearnPattern(indexTool, matTemp);
            //}
        }

        private void btnCannyMax_Click(object sender, EventArgs e)
        {

        }

    
        
      
  
        Bitmap bmResult ;
        private void threadProcess_DoWork(object sender, DoWorkEventArgs e)
        {
          
        }
        public int indexTool = 0;
        private async void threadProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
        }

        private void trackScore_ValueChanged(object sender, EventArgs e)
        {

        }

        private void trackScore_MouseUp(object sender, MouseEventArgs e)
        {
           

          
        }

       

        private void trackMaxOverLap_MouseUp(object sender, MouseEventArgs e)
        {
          

        }
        
       
        

      
     
     

     
     
        public void Loads()
        {
            Propety.TypeTool = TypeTool.Learning;
     
            Propety.TypeMode = Mode.Pattern;
           
        }
        private void ToolOutLine_Load(object sender, EventArgs e)
        {
            Loads();


        }

        private void ToolOutLine_VisibleChanged(object sender, EventArgs e)
        {

        }
       public bool IsClear = false;
        private void btnClear_Click(object sender, EventArgs e)
        {
            //btnClear.IsCLick = !btnClear.IsCLick;
            //IsClear = btnClear.IsCLick;
            //G.EditTool.View.Cursor = new Cursor(Properties.Resources.Erase1.Handle);



            //G.EditTool.View.imgView.Invalidate();



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           
        }

        private void btnAreaBlack_Click(object sender, EventArgs e)
        {
        
         
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
           // Propety.IsHighSpeed = false;
        }

        private void btnHighSpeed_Click(object sender, EventArgs e)
        {
            

        }
        private void btnAreaWhite_Click(object sender, EventArgs e)
        {
          
           // G.EditTool.View.imgView.Invalidate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            
           // G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }
        Mat matTemp2;
      

     
        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void pCany_Paint(object sender, PaintEventArgs e)
        {

        }

        private void trackScore_Load(object sender, EventArgs e)
        {

        }

        private void trackNumObject_ValueChanged_1(int obj)
        {
            //G.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global. IndexChoose].matRaw.ToBitmap();
            //G.EditTool.View.imgView.Invalidate();
            //Propety.NumObject = trackNumObject.Value;
            //G.IsCheck = true;
            //SetRaw();
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

        private void cbTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
           // Propety.TypeYolo = (TypeYolo)cbTypes.SelectedIndex;
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void trackNumObject_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void txtLabel_TextChanged(object sender, EventArgs e)
        {
           
        //  Propety.Labels =txtLabel.Text.Trim().Split(',').ToList();
        }

        private void numScore_ValueChanged(object sender, EventArgs e)
        {

        }

        private void rjButton5_Click(object sender, EventArgs e)
        {

        }

        private void btnTest_Click(object sender, EventArgs e)
        {

        }

        private void btnLearning_Click(object sender, EventArgs e)
        {

        }

        private void rjButton3_Click_1(object sender, EventArgs e)
        {

        }

        private void trackNumObject_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {

        }

        private void btnCropHalt_Click(object sender, EventArgs e)
        {

        }

        private void ckBitwiseNot_Click(object sender, EventArgs e)
        {

        }

        private void ckSubPixel_Click(object sender, EventArgs e)
        {

        }

        private void ckSIMD_Click(object sender, EventArgs e)
        {

        }

        private void trackMaxOverLap_ValueChanged(float obj)
        {

        }

        private void numAngle_ValueChanged(object sender, EventArgs e)
        {

        }

        private void trackAngle_ValueChanged(float obj)
        {

        }

      
        bool IsFullSize;
        private void btnCropHalt_Click_1(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.Edit;

            // G.EditTool.View.imgView.Invalidate();
            // G.EditTool.View.Cursor = Cursors.Default;
            //if (IsClear)
            //    btnClear.PerformClick();
        }

        private void btnCropFull_Click_1(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.ParaCommon.SizeCCD.Width / 2, -Global.ParaCommon.SizeCCD.Height / 2, Global.ParaCommon.SizeCCD.Width, Global.ParaCommon.SizeCCD.Height), new PointF(Global.ParaCommon.SizeCCD.Width / 2, Global.ParaCommon.SizeCCD.Height / 2), 0, AnchorPoint.None);
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            Global.StatusDraw = StatusDraw.Edit;
            // G.EditTool.View.imgView.Invalidate();
            //G.EditTool.View.Cursor = Cursors.Default;
            //if (IsClear)
            //    btnClear.PerformClick();
        }

        private void btnCropRect_Click_1(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Crop;
            Propety.TypeCrop = Global.TypeCrop;

        }

        private void btnTest_Click_1(object sender, EventArgs e)
        {
           btnTest.Enabled = false;
            if (!Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }

        private void label12_Click(object sender, EventArgs e)
        {
                    }

        private void tableLayoutPanel15_Paint(object sender, PaintEventArgs e)
        {

        }
        public void RefreshLabels()
        {
            if (Propety.labelItems == null)
                return;
            dashboardLabel.Items.Clear();
            foreach (LabelItem labelItem in Propety.labelItems)
                dashboardLabel.Items.Add(labelItem);
            
           
        


        }

        private void Btn_Click(object sender, EventArgs e)
        {
            RJButton btn = sender as RJButton;
          int index=  Propety.listLabelCompare.FindIndex(a => a.label == btn.Text);
            Propety.listLabelCompare[index].IsEn = btn.IsCLick;
        }

        private void btnSetLabel_Click(object sender, EventArgs e)
        {
          
            OpenFileDialog OpenFileDialog = new OpenFileDialog();

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                Propety.PathLabels = OpenFileDialog.FileName;
                //txtLabel.Text = Propety.PathLabels;
                String[] Content = File.ReadAllLines(Propety.PathLabels);
                if (Content != null && Content.Length > 0)
                {
                    Propety.listLabelCompare = new List<Labels>();
                    foreach (String label in Content)
                    {
                        if (label == "") continue;
                        Propety.listLabelCompare.Add(new Labels(label, true));

                    }
                    RefreshLabels();
                }
                else
                {
                    MessageBox.Show("Check File Class Again", "Error");
                }
            }
           
          


        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
          
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
          
        }

        private void rjButton6_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.Equal;
        }

        private void trackScore_Load_1(object sender, EventArgs e)
        {

        }

        private void tmCheckFist_Tick(object sender, EventArgs e)
        {
            //if (BeeCore.Common.listCamera[Global. IndexChoose].IsConnected)
            //{
            //    BeeCore.Common.listCamera[Global. IndexChoose].Read();
            //    if (BeeCore.Common.listCamera[Global. IndexChoose].IsConnected)
            //    {
            //       // Propety.Check();
            //        tmCheckFist.Enabled = false;
            //    }
                    
            //}
        
        }

        private void rjButton3_Click_2(object sender, EventArgs e)
        {
            Propety.Compare = Compares.More;
        }

        private void rjButton7_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.Less;
            //   Propety.
        }

      
        

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
                    //cbListModel.SelectedIndex = Propety.listModels.Count-1;


                }
            }
            switch (StepEdit)
            {
                case StepSetModel.SetModel:
                   

                    break;
                case StepSetModel.SetLabels:

                   
                    
                    break;
            }    
           
        }
        bool IsReload = false;
        private void cbListModel_SelectedValueChanged(object sender, EventArgs e)
        {
         
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
            //switch (StepEdit)
            //{
            //    case StepSetModel.SetModel:

                   

            //            break;
            //    case StepSetModel.SetLabels:
            //        OpenFileDialog OpenFileDialog = new OpenFileDialog();

            //        if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            //        {
            //            Propety.PathLabels = OpenFileDialog.FileName;

            //            String[] Content = File.ReadAllLines(Propety.PathLabels);
            //            if (Content != null && Content.Length > 0)
            //            {
            //                Propety.labelItems = new List<LabelItem>();
            //                foreach (String label in Content)
            //                {
            //                    if (label == "") continue;
            //                    Propety.labelItems.Add(new LabelItem(label));

            //                }
            //                RefreshLabels();
            //            }
            //            else
            //            {
            //                MessageBox.Show("Check File Class Again", "Error");
            //            }
            //        }

            //        break;
            //}
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
     
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }
        static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Lấy thông tin thư mục nguồn
            var dir = new DirectoryInfo(sourceDir);

            // Kiểm tra thư mục có tồn tại không
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Nếu thư mục đích chưa tồn tại, tạo nó
            Directory.CreateDirectory(destinationDir);

            // Copy tất cả file
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath, true); // Ghi đè nếu đã tồn tại
            }

            // Nếu recursive là true, copy các thư mục con
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
        private void btnPathDataSet_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                Propety.PathDataSet = folderDialog.SelectedPath;
                string destPath = $"Program\\{Global.Project}\\DataSet\\train";
                tbDataSet.Text = destPath;
                CopyDirectory(Propety.PathDataSet, destPath, true);
            }

        }
        public String sClass = "";
        public bool IsUpdateImgCrop = false;
        private void btnDraw_Click(object sender, EventArgs e)
        {
         
            if (cbLabels.Text == "")
            {
                MessageBox.Show("Please select label");
                return;
            }

            float imageWidth = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Width;
            float imageHeight = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Height;

            RotatedRect rrect = new RotatedRect(
                new Point2f(Propety.rotCrop._PosCenter.X, Propety.rotCrop._PosCenter.Y),
                new Size2f(Propety.rotCrop._rect.Width, Propety.rotCrop._rect.Height),
                Propety.rotCrop._rectRotation
            );

            Mat matCrop = BeeCore.Cropper.CropRotatedRect(
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw,
                Propety.rotCrop, null
            );

            Point2f[] points = rrect.Points();

            for (int i = 0; i < points.Length; i++)
            {
                points[i].X /= imageWidth;
                points[i].Y /= imageHeight;
            }

            int classId = cbLabels.SelectedIndex;

            string segLabel = classId.ToString();
            foreach (var p in points)
            {
                segLabel += $" {p.X:0.######} {p.Y:0.######}";
            }

            listImgTrainYolo.Add(matCrop.ToBitmap());
            listLabelTrainYolo.Add(segLabel);

            IsUpdateImgCrop = true;
            imgCrop.Invalidate();

            sClass = segLabel + "\n";

        }
        List<Image> listImgTrainYolo = new List<Image>();
        List<String> listLabelTrainYolo = new List<String>();
        private void CreateYamlFromLabels(string outputPath)
        {
            try
            {
                var labels = new List<string>();
                foreach (var item in cbLabels.Items)
                    labels.Add(item.ToString());

                int nc = labels.Count;

                // ✅ Lấy full path đến thư mục train/val
                string trainPath = Path.GetFullPath(Path.Combine("Program", Global.Project, "DataSet", "train", "images"));
                string valPath = trainPath; // nếu chưa có val riêng

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"train: {trainPath.Replace("\\", "/")}");
                sb.AppendLine($"val: {valPath.Replace("\\", "/")}");
                sb.AppendLine();
                sb.AppendLine($"nc: {nc}");
                sb.AppendLine("names:");

                foreach (var label in labels)
                {
                    sb.AppendLine($"  - {label}");
                }

                File.WriteAllText(outputPath, sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo YAML: " + ex.Message);
            }
        }
        string SaveYoloRects(List<RectRotate> rects, int imgW, int imgH, List<string> labels, List<LabelItem> allLabels)
        {
            var sb = new StringBuilder();

            int count = Math.Min(rects.Count, labels.Count);

            for (int i = 0; i < count; i++)
            {
                var r = rects[i];
                string lblName = labels[i];
                int classId = allLabels.FindIndex(x =>
                 x.Name.Trim().Equals(lblName.Trim(), StringComparison.OrdinalIgnoreCase));

                if (classId < 0) continue;
                 RotatedRect rrect = new RotatedRect(
                new Point2f(Propety.rotArea._PosCenter.X- Propety.rotArea._rect.Width/2+ rects[i]._PosCenter.X, Propety.rotArea._PosCenter.Y - Propety.rotArea._rect.Height / 2 + rects[i]._PosCenter.Y),
               new Size2f(rects[i]._rect.Width, rects[i]._rect.Height),
              rects[i]._rectRotation
           );

              

                Point2f[] points = rrect.Points();

                for (int j = 0; j < points.Length; j++)
                {
                    points[j].X /= imgW;
                    points[j].Y /= imgH;
                }

             
                string segLabel = classId.ToString();
                foreach (var p in points)
                {
                    segLabel += $" {p.X:0.######} {p.Y:0.######}";
                }
                //float xCenter = r._PosCenter.X / imgW;
                //float yCenter = r._PosCenter.Y / imgH;
                //float width = r._rect.Width / imgW;
                //float height = r._rect.Height / imgH;

                sb.AppendLine(segLabel);// $"{classId} {xCenter:F6} {yCenter:F6} {width:F6} {height:F6}");
            }

            return sb.ToString();
        }

        private void SaveImageAndLabel()//Hau
        {
            
             //   strImgName = $"img_{DateTime.Now:yyyyMMdd_HHmmssfff}";
                try
                {
                    if (Propety.rectTrain == null)
                    {
                        string imageName = strImgName + ".bmp";
                        string labelName = strImgName + ".txt";

                        string pathImage = Path.Combine("Program", Global.Project, "DataSet", "train", "images");
                        string pathLabel = Path.Combine("Program", Global.Project, "DataSet", "train", "labels");
                        Directory.CreateDirectory(pathImage);
                        Directory.CreateDirectory(pathLabel);

                        string fullImagePath = Path.Combine(pathImage, imageName);
                        BeeCore.Common.listCamera[Global.IndexChoose].matRaw.SaveImage(fullImagePath);

                        string fullLabelPath = Path.Combine(pathLabel, labelName);
                        File.WriteAllLines(fullLabelPath, listLabelTrainYolo);
                    }
                    else
                    {

                        string lb = SaveYoloRects(
                            Propety.rectTrain,
                            BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Width,
                            BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Height, Propety.listLabel,
                            Propety.labelItems
                                    );

                        string imageName = strImgName + ".jpg";
                        string labelName = strImgName + ".txt";

                        string pathImage = Path.Combine("Program", Global.Project, "DataSet", "train", "images");
                        string pathLabel = Path.Combine("Program", Global.Project, "DataSet", "train", "labels");

                        Directory.CreateDirectory(pathImage);
                        Directory.CreateDirectory(pathLabel);

                        string fullImagePath = Path.Combine(pathImage, imageName);
                        BeeCore.Common.listCamera[Global.IndexChoose].matRaw.SaveImage(fullImagePath);

                        string fullLabelPath = Path.Combine(pathLabel, labelName);
                        File.WriteAllText(fullLabelPath, lb + string.Join("", listLabelTrainYolo));
                    }
                }
                catch (Exception ex)
                {
                }
           
        }
        private void tabYolo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabYolo.SelectedIndex ==2)
            {
                cbLabels.DataSource = new List<string>();
              
                if (Propety.labelItems != null)
                {
                    cbLabels.DataSource = null;  
                    cbLabels.Items.Clear();      

                    foreach (LabelItem item in Propety.labelItems)
                    {
                        cbLabels.Items.Add(item.Name); 
                    }
                    RefreshLabels();
                }
                else
                {
                    MessageBox.Show("Please select models fist");
                }
               Global.TypeCrop= TypeCrop.Crop;
                Propety.TypeCrop = Global.TypeCrop;
                if (Propety.rotCrop == null)
                {
                    int with = 50, height = 50;
                    Propety.rotCrop = new RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Width / 2, BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Height / 2), 0, AnchorPoint.None);

                }
             
            }
            else
            {
                Propety.rotCrop = null;
               Global.TypeCrop= TypeCrop.Area;
                Propety.TypeCrop = Global.TypeCrop;

            }
        }

        private string strImgName = "";//Hau
        private void ClearImgBox()
        {
            try
            {
                if (imgCrop.Image != null)
                {
                    imgCrop.Image.Dispose();
                    imgCrop.Image = null;
                }
            }
            catch (Exception ex)
            {

            }
            listImgTrainYolo.Clear();
            listLabelTrainYolo.Clear();
            if(Propety.rectTrain != null)
            Propety.rectTrain.Clear();
            else if(Propety.listLabel != null)
            Propety.listLabel.Clear();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
           //if(imgCrop.Image == null)
           // {
           //     MessageBox.Show("Please crop images");
           //     return;
           // }
            imgCrop.Invalidate();
            SaveImageAndLabel();
            string pathYaml = Path.Combine("Program", Global.Project, "DataSet", "data.yaml");//note
            CreateYamlFromLabels(pathYaml);
            MessageBox.Show("Complete");
            
        }
        int xImgCrop = 10;
        private void imgCrop_Paint(object sender, PaintEventArgs e)
        {
            xImgCrop = 10;
            for (int i = listImgTrainYolo.Count - 1; i >= 0; i--)
            {

                int W = listImgTrainYolo[i].Width;
                int H = listImgTrainYolo[i].Height;
                double Scale = (imgCrop.Height - 20) / (H * 1.0);
                W = (int)(W * Scale);
                e.Graphics.DrawImage(listImgTrainYolo[i], new Rectangle(xImgCrop, 5, W, imgCrop.Height - 20));
                e.Graphics.DrawString(listLabelTrainYolo[i], new Font("Arial", 14), Brushes.Black, xImgCrop, imgCrop.Height - 25);
                xImgCrop += W + 20;
            }
            if (IsUpdateImgCrop)
            {
                IsUpdateImgCrop = false;
                imgCrop.Size = new Size(xImgCrop, imgCrop.Height);
            }
        }

        private void workLoadModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cbListModel.Enabled = true;
           
          
        }

        private void btnTraining_Click(object sender, EventArgs e)
        {
            if(!workTrain.IsBusy)
            {
                btnTraining.Enabled = false;
                workTrain.RunWorkerAsync();
            }
         
          
        }

        private void workTrain_DoWork(object sender, DoWorkEventArgs e)
        {
            string pathYaml = Path.Combine("Program", Global.Project, "DataSet", "data.yaml");

           // boxLog.Items.Add($"{DateTime.Now:HH:mm:ss} - Start training: {Propety.PathModel}");

            Propety.PercentChange += Yolo_PercentChange;

            try
            {
                Propety.Training(Common.PropetyTools[Global.IndexChoose][Propety.Index].Name, Propety.pathFullModel, pathYaml);

            }
            catch (Exception ex)
            {
             //   boxLog.Items.Add($"{DateTime.Now:HH:mm:ss} - Error train: {ex.Message}");
            }
            finally
            {
              //  if (boxLog.Items.Count > 0)
                //    boxLog.TopIndex = boxLog.Items.Count - 1;
            }


        }
        private void Yolo_PercentChange(int percent)//note
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    progressBar1.Value = percent;
                    txtPercent.Text = percent.ToString() + "%";

                  //  boxLog.Items.Add($"{DateTime.Now:HH:mm:ss} - Training" + " " + percent.ToString() + "%");
                  
                }));
            }
            else
            {

                progressBar1.Value = percent;
                txtPercent.Text = percent.ToString() + "%";

               // boxLog.Items.Add($"{DateTime.Now:HH:mm:ss} - Training" +" "+ percent.ToString()+"%");
                
            }
        }
        private void workTrain_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void workTrain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = 100;
           btnTraining.Enabled = true;
        }

      

        private void btnX_L_R_Click(object sender, EventArgs e)
        {
            Propety.ArrangeBox = ArrangeBox.X_Left_Rigth;
        }

        private void btnArrangeBox_Click(object sender, EventArgs e)
        {
            Propety.IsArrangeBox = btnArrangeBox.IsCLick;
        }

        private void btnX_R_L_Click(object sender, EventArgs e)
        {
            Propety.ArrangeBox = ArrangeBox.X_Right_Left;
        }

        private void btnY_L_R_Click(object sender, EventArgs e)
        {
            Propety.ArrangeBox = ArrangeBox.Y_Left_Rigth;
        }

        private void btnY_R_L_Click(object sender, EventArgs e)
        {
            Propety.ArrangeBox = ArrangeBox.Y_Right_Left;
        }

        private void tableLayoutPanel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel15_Paint_1(object sender, PaintEventArgs e)
        {

        }

     
        private void btnLessArea_Click_1(object sender, EventArgs e)
        {
            Propety.CompareArea =  Compares.Less;
        }

        private void btnMoreArea_Click_1(object sender, EventArgs e)
        {
            Propety.CompareArea = Compares.More;
        }

        private void numArea_Load(object sender, EventArgs e)
        {
           
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

        private void trackNumObject_ValueChanged(float obj)
        {
            Propety.NumObject = (int)trackNumObject.Value;
        }

        private void numEpoch_ValueChanged(float obj)
        {
            Propety.Epoch = (int)numEpoch.Value;
        }

        private void btnLoadImg_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw != null)
                    if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty())
                        BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Release();
               
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = Cv2.ImRead(openFile.FileName);
              
              Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();


                strImgName =Path.GetFileNameWithoutExtension( openFile.FileName);
            }

            ClearImgBox();
        }
        public List<PointF[]> listBoxCorners = new List<PointF[]>();
        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (!Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.IsBusy)
            {
                Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
            }
                
            else
                btnCheck.IsCLick = false;
            
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
         
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].IsSendResult = btnEnable.IsCLick;

        }

        private void txtAddPLC_TextChanged(object sender, EventArgs e)
        {
            Propety.AddPLC =txtAddPLC.Text.Trim();
        }

        private void btnBits_Click(object sender, EventArgs e)
        {
           // Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].TypeSendPLC = TypeSendPLC.Bits;
        }

        private void btnString_Click(object sender, EventArgs e)
        {
           // Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].TypeSendPLC = TypeSendPLC.String;
        }
    }
}

