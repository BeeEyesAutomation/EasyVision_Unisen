using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeeCore;

using OpenCvSharp;
using OpenCvSharp.Extensions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Label = System.Windows.Forms.Label;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using BeeGlobal;

namespace BeeInterface
{
 public   enum StepSetModel
    {
        SetModel, SetLabels, Retrain
    }
    [Serializable()]
    public partial class ToolYolo : UserControl
    {
        
        public ToolYolo( )
        {
            InitializeComponent();
            
        }
        bool IsIni = false;
      
        public StepSetModel StepEdit = StepSetModel.SetModel;
      
        public void LoadPara()
        {
          
            if (Propety.listModels == null) Propety.listModels = new List<string>();
            Propety.listModels = Propety.listModels.Distinct().ToList();
            Propety.pathFullModel = "Program\\" + Global.Project + "\\" + Propety.PathModel;
            //if (!workLoadModel.IsBusy)
            //workLoadModel.RunWorkerAsync();
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
           
            if(Propety.PathModel!="")
            cbListModel.Text =Propety.PathModel;
            txtMatching.Text = Propety.Matching;
            btnEnbleContent.IsCLick = Propety.IsEnContent;
            //if (Propety.PathModel!=null)
            //   if (File.Exists(Propety.PathModel))
            //       Propety.SetModel(this.Name, Propety.PathModel, TypeYolo.YOLO);
            IsIni = true;
            String slabel = "";

         //   txtLabel.Text = Propety.PathLabels; ;
                RefreshLabels();
            
           Global.TypeCrop= TypeCrop.Area;
             CustomGui.RoundRg(tabLbs, 10, Corner.Bottom);
            //picTemp1.Image = Propety.matTemp;
            //picTemp2.Image = Propety.matTemp2;

            trackScore.Min = Common.PropetyTools[Global.IndexChoose][Propety.Index].MinValue;
            trackScore.Max = Common.PropetyTools[Global.IndexChoose][Propety.Index].MaxValue;
            trackScore.Step = Common.PropetyTools[Global.IndexChoose][Propety.Index].StepValue;
            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;

            btnEnLineLimit.IsCLick = Propety.IsCheckArea ;
             trackNumObject.Value= Propety.NumObject;
            numLine.Value = Propety.yLine;

            SetLabels();
            switch (Propety.CompareLine)
            {
              
                case Compares.Less:
                    btnLessArea.IsCLick = true;
                    break;
                case Compares.More:
                    btnMoreArea.IsCLick = true;
                    break;
            }
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

            //Propety.TypeMode = Propety.TypeMode;

        }

      
        private void trackScore_ValueChanged(float obj)
        {

            Common.PropetyTools[Global.IndexChoose][Propety.Index].Score = (int)trackScore.Value;
          
        }

        public Yolo Propety=new Yolo();
        public Mat matTemp = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
        public void GetTemp(RectRotate rotateRect, Mat matRegister)
        {
           
                float angle = rotateRect._rectRotation;
                if (rotateRect._rectRotation < 0) angle = 360 + rotateRect._rectRotation;
                Mat matCrop =BeeCore.Common.CropRotatedRectSharp(matRegister, new RotatedRect(new Point2f(rotateRect._PosCenter.X + (rotateRect._rect.Width / 2 + rotateRect._rect.X), rotateRect._PosCenter.Y + (rotateRect._rect.Height / 2 + rotateRect._rect.Y)), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), angle));
                if (matCrop.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(matCrop, matTemp, ColorConversionCodes.BGR2GRAY);
                if (Propety.IsAreaWhite)
                    Cv2.BitwiseNot(matTemp, matTemp);
           
        }
      
     
        public Graphics ShowEdit(Graphics gc, RectangleF _rect)
        {
            if (matTemp == null) return gc;

            if (Global.TypeCrop != TypeCrop.Area)
                try
                {
                    Mat matShow = matTemp.Clone();
                    if (Propety.TypeMode == Mode.OutLine)
                    {
                        Bitmap bmTemp = matShow.ToBitmap();

                        bmTemp.MakeTransparent(Color.Black);
                        bmTemp = ConvertImg.ChangeToColor (bmTemp, Color.FromArgb(0, 255, 0), 1f);

                        gc.DrawImage(bmTemp, _rect);
                    }
                    if (matMask != null)
                    {
                        Bitmap myBitmap2 = matMask.ToBitmap();
                        myBitmap2.MakeTransparent(Color.Black);
                        myBitmap2 = ConvertImg.ChangeToColor(myBitmap2, Color.OrangeRed, 1f);

                        gc.DrawImage(myBitmap2, _rect);
                    }

                }
                catch (Exception ex) { }
            return gc;
        }

       
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

    
        
      
        public void Process()
        {
            try
            {
               // Propety.rectRotates = new List<RectRotate>();
               // if (Global.IsRun)
               // {
               //     if (G.rotOriginAdj != null)
               //         Propety.rotAreaAdjustment = G.EditTool.View.GetPositionAdjustment(Propety.rotArea, G.rotOriginAdj);
               //     else
               //         Propety.rotAreaAdjustment = Propety.rotArea;
               //     //  Propety.Matching(Global.IsRun, BeeCore.Common.listCamera[Global. IndexChoose].matRaw.ToBitmap(), indexTool, Propety.rotAreaAdjustment);
               // //    Propety.Check(this.Name, Propety.rotAreaAdjustment);
               // }
               //// else
               //   //  Propety.Check(this.Name, Propety.rotArea);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
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
            Propety.IsAreaWhite = false;
             GetTemp(Propety.rotCrop,BeeCore.Common.listCamera[Global. IndexChoose].matRaw );
         
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
            Propety.IsAreaWhite = true;
            GetTemp(Propety.rotCrop, BeeCore.Common.listCamera[Global. IndexChoose].matRaw);
           // G.EditTool.View.imgView.Invalidate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            
           // G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void trackBar21_Load(object sender, EventArgs e)
        {

        }

        private void trackNumObject_ValueChanged(float obj)
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

        private void btnPathModel_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog=new OpenFileDialog();

            if (OpenFileDialog.ShowDialog()==DialogResult.OK)
            {
                Propety.PathModel = OpenFileDialog.FileName;
                //txtModel.Text = Propety.PathModel;
                LoadPara();
            }
          
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
            Propety.NumObject = (int)trackNumObject.Value;
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
            Propety.rotArea = new RectRotate(new RectangleF(-Global.ParaCommon.SizeCCD.Width / 2, -Global.ParaCommon.SizeCCD.Height / 2, Global.ParaCommon.SizeCCD.Width, Global.ParaCommon.SizeCCD.Height), new PointF(Global.ParaCommon.SizeCCD.Width / 2, Global.ParaCommon.SizeCCD.Height / 2), 0, AnchorPoint.None,false);
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


            //G.EditTool.View.imgView.Invalidate();
           // G.EditTool.View.imgView.Cursor = Cursors.Default;
        }

        private void btnTest_Click_1(object sender, EventArgs e)
        {

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
            if (Propety.listLabelCompare == null)
                return;
            int index = 0;
            tabLbs.Height = 0;
            tabLbs.Controls.Clear();
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                   
                    if (index >= Propety.listLabelCompare.Count)
                        break;
                    if (col == 0)
                        tabLbs.Height += 40;
                    RJButton btn = new RJButton();
                    btn.Text = Propety.listLabelCompare[index].label;
                    btn.Font = new Font("Arial", 11);
                    btn.IsUnGroup = true;
                    btn.ForeColor = Color.Black;
                    btn.BorderRadius = 10; 
                    btn.Height = 30;
                    btn.IsCLick = Propety.listLabelCompare[index].IsEn;
                    btn.Click += Btn_Click;
                    btn.Corner = Corner.Both;
                    btn.BackColor = Color.FromArgb(200, 200, 200);
                    btn.Dock = DockStyle.Fill;
                    btn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    btn.Margin = new Padding(3);
                 
                    tabLbs.Controls.Add(btn, col, row);
                    index++;
                }
              
            }
          
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

        private void btnCheckArea_Click(object sender, EventArgs e)
        {
            Propety.IsCheckArea = btnEnLineLimit.IsCLick;
            layoutLineLimit.Enabled = btnEnLineLimit.IsCLick;
        }

        private void rjButton6_Click_1(object sender, EventArgs e)
        {

        }

        private void btnMoreArea_Click(object sender, EventArgs e)
        {
            Propety.CompareLine=Compares.More;
        }

        private void btnLessArea_Click(object sender, EventArgs e)
        {
            Propety.CompareLine = Compares.Less;
        }

        private void numLine_ValueChanged(object sender, EventArgs e)
        {
            Propety.yLine = (int)numLine.Value;

            if (!Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }

        private void btnAddModel_Click(object sender, EventArgs e)
        {
            switch(StepEdit)
            {
                case StepSetModel.SetModel:
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
                            Propety.listModels= Propety.listModels.Distinct().ToList();
                            cbListModel.DataSource = null;
                            Propety.PathModel =Path.GetFileName( pathModel);
                            IsReload = true;
                            if(!workLoadModel.IsBusy)
                            workLoadModel.RunWorkerAsync();
                            cbListModel.DataSource = Propety.listModels.ToArray();

                            //cbListModel.SelectedIndex = Propety.listModels.Count-1;


                        }
                    }

                    break;
                case StepSetModel.SetLabels:

                   
                    String[] Content = Propety.LoadNameModel(Common.PropetyTools[Global.IndexChoose][Propety.Index].Name);
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

                    break;
            }    
           
        }
        bool IsReload = false;
        private void cbListModel_SelectedValueChanged(object sender, EventArgs e)
        {
            if (IsReload)
            {
                IsReload = false;
                return;
            }    
               
            if (cbListModel.SelectedIndex == -1) return;
            Propety.PathModel = cbListModel.Text;
           Propety.pathFullModel = "Program\\" + Global.Project + "\\" + Propety.PathModel;
           

            if (File.Exists(Propety.pathFullModel))
            {
              
                Propety.SetModel();
              //  Propety.listLabelCompare = new List<Labels>();
                //RefreshLabels();

            }
        }

        private void btnSetModel_Click(object sender, EventArgs e)
        {
            StepEdit = StepSetModel.SetModel;
            tabLbs.Enabled = false;
            btnAddModel.Text = "Add";
            btnRemoveModel.Text = "Remove";
            btnRemoveModel.Visible = true;
            cbListModel.Enabled = true;
        }
        public void SetLabels()
        {
            cbListModel.SelectionStart = cbListModel.Text.Length;
            cbListModel.SelectionLength = 0;
            StepEdit = StepSetModel.SetLabels;
            tabLbs.Enabled = true;
            cbListModel.Enabled = false;
            btnAddModel.Text = "Re.Load";
            btnRemoveModel.Visible = true;
            btnRemoveModel.Text = "Import";


        }
        private void btnSetLabels_Click(object sender, EventArgs e)
        {
            SetLabels();
        }

        private void btnRemoveModel_Click(object sender, EventArgs e)
        {
            switch (StepEdit)
            {
                case StepSetModel.SetModel:
                    break;
                case StepSetModel.SetLabels:
                    OpenFileDialog OpenFileDialog = new OpenFileDialog();

                    if (OpenFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        Propety.PathLabels = OpenFileDialog.FileName;

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

                    break;
            }
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
            FolderBrowserDialog OpenFileDialog = new FolderBrowserDialog();

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                Propety.PathDataSet = OpenFileDialog.SelectedPath;
                CopyDirectory(Propety.PathDataSet, "Program\\" + Global.Project + "\\DataSet",true);
             
              }
        }
        public String sClass = "";
        public bool IsUpdateImgCrop = false;
        private void btnDraw_Click(object sender, EventArgs e)
        {  // Thông tin ảnh
            float imageWidth = BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Width;
            float imageHeight = BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Height;
            RotatedRect rrect = new RotatedRect(
           new Point2f( Propety.rotCrop._PosCenter.X, Propety.rotCrop._PosCenter.Y),    // center
            new Size2f(Propety.rotCrop._rect.Width, Propety.rotCrop._rect.Height),       // width, height
            Propety.rotCrop._rectRotation                        // angle in degrees
           );
            // Tính 4 điểm
            Point2f[] points = rrect.Points();
            // Normalize
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X /= imageWidth;
                points[i].Y /= imageHeight;
            }
            int classId = cbLabels.SelectedIndex;
           
            // Tạo nội dung dòng annotation
            sClass += classId.ToString();
            foreach (var p in points)
            {
                sClass += $" {p.X.ToString("0.######", CultureInfo.InvariantCulture)} {p.Y.ToString("0.######", CultureInfo.InvariantCulture)}";
            }
            sClass += "\n";
            // Mat matCrop=   CropRotatedRect(BeeCore.Common.listCamera[Global. IndexChoose].matRaw, Propety.rotCrop,Propety.rotMask);
           // G.listImgTrainYolo.Add(matCrop.ToBitmap());
           // G.listLabelTrainYolo.Add(cbLabels.Text);
            IsUpdateImgCrop = true;
            imgCrop.Invalidate();

        }

        private void tabYolo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabYolo.SelectedIndex ==2)
            {
                cbLabels.DataSource = new List<string>();
                List<string> list = new List<string>();
                foreach (Labels labels in Propety.listLabelCompare)
                    list.Add(labels.label);
                cbLabels.DataSource = list.ToArray();
               Global.TypeCrop= TypeCrop.Crop;
                Propety.TypeCrop = Global.TypeCrop;
                if (Propety.rotCrop == null)
                {
                    int with = 50, height = 50;
                    Propety.rotCrop = new RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Width / 2, BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Height / 2), 0, AnchorPoint.None,false);

                }
               // G.EditTool.View.imgView.Invalidate();
               // G.EditTool.View.imgView.Cursor = Cursors.Default;
            }
            else
            {
                Propety.rotCrop = null;
               Global.TypeCrop= TypeCrop.Area;
                Propety.TypeCrop = Global.TypeCrop;

               // G.EditTool.View.imgView.Invalidate();
              //  G.EditTool.View.imgView.Cursor = Cursors.Default;

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //String nameFile = Path.GetFileName(G.EditTool.View.pathFileSeleted);
            //String nameFileWithOut = Path.GetFileNameWithoutExtension(G.EditTool.View.pathFileSeleted);
            //File.Copy(G.EditTool.View.pathFileSeleted, "Program\\" + Global.Project + "\\DataSet\\images\\train\\" + nameFile, true);
            //File.Copy(G.EditTool.View.pathFileSeleted, "Program\\" + Global.Project + "\\DataSet\\images\\val\\" + nameFile, true);
            //string outputPath = "Program\\" + Global.Project + "\\DataSet\\labels\\train\\" + nameFileWithOut + ".txt";
            //File.WriteAllText("Program\\" + Global.Project + "\\DataSet\\labels\\train\\" + nameFileWithOut + ".txt", sClass);
            //File.WriteAllText("Program\\" + Global.Project + "\\DataSet\\labels\\val\\" + nameFileWithOut + ".txt", sClass);
            //sClass = "";
            //G.listImgTrainYolo = new List<Bitmap>();
            //G.listLabelTrainYolo = new List<string>();
            imgCrop.Invalidate();
            MessageBox.Show("Complete");
            //using (StreamWriter sw = new StreamWriter(outputPath, false, Encoding.UTF8))
            //{
            //    sw.Write($"{classId}");
            //    foreach (var p in points)
            //    {
            //        sw.Write($" {p.X.ToString("0.######", CultureInfo.InvariantCulture)} {p.Y.ToString("0.######", CultureInfo.InvariantCulture)}");
            //    }
            //    sw.WriteLine();
            //}
            //string outputPath2 = "Program\\" + Global.Project + "\\DataSet\\labels\\val\\" + nameFileWithOut + ".txt";
            //using (StreamWriter sw = new StreamWriter(outputPath2, false, Encoding.UTF8))
            //{
            //    sw.Write($"{classId}");
            //    foreach (var p in points)
            //    {
            //        sw.Write($" {p.X.ToString("0.######", CultureInfo.InvariantCulture)} {p.Y.ToString("0.######", CultureInfo.InvariantCulture)}");
            //    }
            //    sw.WriteLine();
            //}
        }
        int xImgCrop = 10;
        private void imgCrop_Paint(object sender, PaintEventArgs e)
        {
            //xImgCrop = 10;
            //for (int i = G.listImgTrainYolo.Count - 1; i >= 0; i--)
            //{

            //    int W = G.listImgTrainYolo[i].Width;
            //    int H = G.listImgTrainYolo[i].Height;
            //    double Scale = (imgCrop.Height - 20) / (H * 1.0);
            //    W = (int)(W * Scale);
            //    e.Graphics.DrawImage(G.listImgTrainYolo[i], new Rectangle(xImgCrop, 5, W, imgCrop.Height - 20));
            //    e.Graphics.DrawString(G.listLabelTrainYolo[i], new Font("Arial", 12), Brushes.Black, xImgCrop, imgCrop.Height - 25);
            //    xImgCrop += W + 20;
            //}
            //if(IsUpdateImgCrop)
            //{
            //    IsUpdateImgCrop = false;
            //    imgCrop.Size = new Size(xImgCrop, imgCrop.Height);
            //}
            
        }

        private void workLoadModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!IsIni)
            {
                IsIni = true;
             //   tmCheckFist.Enabled = true;


            }
        }

        private void numTourch_ValueChanged(object sender, EventArgs e)
        {
            Propety.Epoch = (int)numEpoch.Value;
        }

        private void btnTraining_Click(object sender, EventArgs e)
        {
            workTrain.RunWorkerAsync();
        }

        private void workTrain_DoWork(object sender, DoWorkEventArgs e)
        {
            Propety.Training(Common.PropetyTools[Global.IndexChoose][Propety.Index].Name, "Program\\NIDEC_MH_DEMO2\\DataSet\\data.yaml");

            workTrain.ReportProgress(Propety.Percent);
        }

        private void workTrain_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;


        }

        private void workTrain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void btnEnbleContent_Click(object sender, EventArgs e)
        {
            Propety.IsEnContent = btnEnbleContent.IsCLick;
            layContent.Enabled = btnEnbleContent.IsCLick;
        }

        private void txtMatching_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtMatching.Text = txtMatching.Text.Trim().Replace("\n", "") ;
                Propety.Matching = txtMatching.Text;
            }
        }

        private void btnSetContent_Click(object sender, EventArgs e)
        {
            Propety.Matching = Propety.Content;
            txtMatching.Text = Propety.Matching;
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
    }
}

