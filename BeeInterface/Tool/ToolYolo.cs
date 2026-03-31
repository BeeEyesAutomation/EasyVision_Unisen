using BeeCore;
using BeeCpp;
using BeeGlobal;
using BeeInterface.Group;
using Cyotek.Windows.Forms;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
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
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Label = System.Windows.Forms.Label;
using Point = System.Drawing.Point;
using ShapeType = BeeGlobal.ShapeType;
using Size = System.Drawing.Size;

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
            {if(Propety.rotCrop!=null)
                Propety.rotCrop.IsVisible =! Propety.IsLine;
                EditRectRot1.Rot = new List<RectRotate> { Propety.rotArea , Propety.rotCrop, Propety.rotMask, Propety.rotLimit };
                EditRectRot1.RotateCurentChanged -= EditRectRot_RotateCurentChanged;
                EditRectRot1.RotateCurentChanged += EditRectRot_RotateCurentChanged;
                EditRectRot1.ChooseEditBegin += EditRectRot1_ChooseEditBegin;
                EditRectRot1.ChooseEditEnd += EditRectRot1_ChooseEditEnd;
                EditRectRot1.ClearAllEvent += EditRectRot1_ClearAllEvent;
                EditRectRot1.UnoRotEvent += EditRectRot1_UnoRotEvent;
                EditRectRot1.AddRotEvent += EditRectRot1_AddRotEvent;
                EditRectRot1.IsHide = false;
                this.VisibleChanged += ToolYolo_VisibleChanged;
                btnCLAll.IsCLick = Propety.IsColorAllObjLabel;
                btnClOne.IsCLick=!Propety.IsColorAllObjLabel;
                EditRectRot1.Refresh();
                btnNone.IsCLick = Global.Dir == Dir.None ? true : false;
                btnLeft.IsCLick = Global.Dir == Dir.Left ? true : false;
                btnRight.IsCLick = Global.Dir == Dir.Right ? true : false;
                Global.SetColorChange -= Global_SetColorChange;
                Global.SetColorChange += Global_SetColorChange;
                btnNoCropMask.IsCLick = !Propety.IsCropSingle;
                btnCropMask.IsCLick = Propety.IsCropSingle;
                if (Propety.listModels == null) Propety.listModels = new List<string>();
                Propety.listModels = Propety.listModels.Distinct().ToList();
                Propety.pathFullModel = "Program\\" + Global.Project + "\\" + Propety.PathModel;
                lbLen.Text = Propety.LenTemp.ToString();
                laySetLine.Visible = Propety.IsLine;
                laySetLine2.Visible = Propety.IsLine;
                laySetLine3.Visible = Propety.IsLine;
                AdjThreshLine.Value = Propety.ThresholdLine;
                AdjTolerance.Value = Propety.ToleranceLine;
                trackNumObject.Value = Propety.NumObject;
                btnTypeYolo.IsCLick = Propety.TypeYolo == TypeYolo.YOLO ?true:false ;
                btnTypeOnnx.IsCLick = Propety.TypeYolo == TypeYolo.Onnx ? true : false;
                btnTypeRCNN.IsCLick = Propety.TypeYolo == TypeYolo.RCNN ? true : false;
                switch(Propety.TypeYolo)
                {
                    case TypeYolo.YOLO:
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

                        }
                        break;
                    case TypeYolo.Onnx:
                        if (!Directory.Exists(Propety.pathFullModel))
                        {
                            Propety.listModelOnnx.Remove(Propety.PathModel);
                            if (Propety.listModelOnnx.Count > 0)
                            {
                                Propety.PathModel = Propety.listModelOnnx[Propety.listModelOnnx.Count - 1];
                                Propety.pathFullModel = "Program\\" + Global.Project + "\\" + Propety.PathModel;

                            }
                            else
                            {
                                Propety.PathModel = "";
                                Propety.pathFullModel = "";
                            }

                        }
                        break;

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

              //  Global.TypeCrop = TypeCrop.Area;
              //  CustomGui.RoundRg(tabLbs, 10, Corner.Bottom);
                //picTemp1.Image = Propety.matTemp;
                //picTemp2.Image = Propety.matTemp2;

                trackScore.Min = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].MinValue;
                trackScore.Max = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].MaxValue;
                trackScore.Step = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StepValue;
                trackScore.Value = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Score;
                numEpoch.Value = Propety.Epoch;
               btnMergeBox.IsCLick=Propety.FilterBox==FilterBox.Merge?true:false;
                btnRemoveBox.IsCLick = Propety.FilterBox == FilterBox.Remove ? true : false;
                btnNoneBox.IsCLick = Propety.FilterBox == FilterBox.None ? true : false;
                AdjOverLap.Enabled= Propety.FilterBox == FilterBox.None ? false : true;
                AdjOverLap.Value = Propety.ThreshOverlap;
                btnOnline.IsCLick = Propety.IsLine;
                btnOffLine.IsCLick=!Propety.IsLine;
                //btnCrop.Enabled=Propety.IsLine;
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
                Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StatusToolChanged += ToolYolo_StatusToolChanged;
                
            }
            catch (Exception ex)
            {
                Er = ex.Message;
            }
            //Propety.TypeMode = Propety.TypeMode;

        }

        private void EditRectRot1_AddRotEvent(RectRotate obj)
        {  
            Propety.listRotScan.Add(obj);
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;
        }

        private void EditRectRot1_UnoRotEvent(bool obj)
        {
            if (Propety.listRotScan.Count > 0)
            {
                Propety.listRotScan.RemoveAt(Propety.listRotScan.Count - 1);
                if(_currentLabel.ListInsideBox.Count>0)
                _currentLabel.ListInsideBox.RemoveAt(_currentLabel.ListInsideBox.Count - 1);

            }    
            
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;
          
        }

        private void EditRectRot1_ClearAllEvent(bool obj)
        {
            if (Propety.listRotScan.Count > 0)
                Propety.listRotScan.Clear();
         
            _currentLabel.ListInsideBox.Clear();
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;
           
        }

        private void EditRectRot1_ChooseEditEnd(int obj)
        {if (obj == -1) return;
            if (Propety.rotLimit != null)
            {
                Dir Dir= Propety.listRotScan[obj].Dir;
                Propety.rotLimit.Dir = Dir;
                RectRotate rot = Propety.rotLimit.Clone();

                PointF pCenter = Propety.rotArea.WorldToLocal(rot._PosCenter);
                rot._PosCenter = pCenter;
                if(obj < Propety.listRotScan.Count())
                {
                    Propety.listRotScan[obj] = rot;
                    foreach (RectRotate rot1 in Propety.listRotScan)
                    {
                        rot1.Name = "";
                    }
                        foreach (LabelItem labelItem in Propety.labelItems)
                    {
                        if (labelItem.ListInsideBox != null)
                            labelItem.ListInsideBox.Clear();
                    }    
                      
                 
                }    
              
                

            }

        }

        private void EditRectRot1_ChooseEditBegin(bool obj)
        {
            Propety.ModeCheck = ModeCheck.Single;
        
            foreach (RectRotate rectRotate in Propety.listRotScan)
                rectRotate._dragAnchor = AnchorPoint.None;
        }

        private void ToolYolo_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                EditRectRot1.IsHide = true;
                EditRectRot1.ChooseEditBegin -= EditRectRot1_ChooseEditBegin;
                EditRectRot1.ChooseEditEnd -= EditRectRot1_ChooseEditEnd;
                EditRectRot1.RotateCurentChanged -= EditRectRot_RotateCurentChanged;
            } 
                
           
        }

        private void EditRectRot_RotateCurentChanged(RectRotate obj)
        {if (obj == null) return;
           switch(obj.TypeCrop)
            {
                case TypeCrop.Area:
                    Propety.rotArea = obj; break;
                case TypeCrop.Crop:
                    Propety.rotCrop = obj; break;
                case TypeCrop.Mask:
                    Propety.rotMask = obj; break;
                case TypeCrop.Limit:
                    Propety.rotLimit = obj; break;
            }    
        }

        private void Global_SetColorChange(bool obj)
        {   try
            {
              
                Invoke(new Action(() =>
                {
                    if (Global.ColorSample == null)
                        return;
                    if (BeeCore.Common.HSVSample == null)
                        return;

                    HSVCli = BeeCore.Common.HSVSample;
                    //  tableLayoutModel.BackColor = Global.ColorSample;
                    _currentColorItem.SampleColor = Global.ColorSample;
                    _currentColorItem.HSV = new BeeCore.Core.HSV(BeeCore.Common.HSVSample.H, BeeCore.Common.HSVSample.S, BeeCore.Common.HSVSample.V);
                    dashboardLabel.Invalidate();
                }));
            }
            catch(Exception ex)
            {

            }
           
        }

        String Er = "";

        private void ToolYolo_StatusToolChanged(StatusTool obj)
        {
            if (Global.IsRun) return;
            if (Propety.Index >= Common.PropetyTools[Global.IndexProgChoose].Count)
                return;
            if (Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StatusTool == StatusTool.Done)
            {
                Propety.rectTrain = Propety.rectRotates;//note
                btnTest.Enabled = true;
            }
         }

        private void trackScore_ValueChanged(float obj)
        {

            Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Score = (int)trackScore.Value;
          
        }

        public Yolo Propety=new Yolo();
  
  
      
     
     

       
        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {

        }

        private void btnCropArea_Click(object sender, EventArgs e)
        {
           //Global.TypeCrop= TypeCrop.Area;
           // Propety.TypeCrop = Global.TypeCrop;
  
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
            //G.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global. IndexProgChoose].matRaw.ToBitmap();
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
            //Global.TypeCrop = TypeCrop.Area;
            //Propety.TypeCrop = Global.TypeCrop;
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
            Propety.rotArea = new RectRotate(new RectangleF(-Global.Config.SizeCCD.Width / 2, -Global.Config.SizeCCD.Height / 2, Global.Config.SizeCCD.Width, Global.Config.SizeCCD.Height), new PointF(Global.Config.SizeCCD.Width / 2, Global.Config.SizeCCD.Height / 2), 0, AnchorPoint.None);
            //Global.TypeCrop = TypeCrop.Area;
            //Propety.TypeCrop = Global.TypeCrop;
            Global.StatusDraw = StatusDraw.Edit;
            // G.EditTool.View.imgView.Invalidate();
            //G.EditTool.View.Cursor = Cursors.Default;
            //if (IsClear)
            //    btnClear.PerformClick();
        }

        private void btnCropRect_Click_1(object sender, EventArgs e)
        {
            //Global.TypeCrop = TypeCrop.Crop;
            //Propety.TypeCrop = Global.TypeCrop;

        }

        private void btnTest_Click_1(object sender, EventArgs e)
        {
           btnTest.Enabled = false;
            if (!Common.PropetyTools[Global.IndexProgChoose][Global.IndexToolSelected].worker.IsBusy)
                Common.PropetyTools[Global.IndexProgChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
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
         
        }

        private void trackScore_Load_1(object sender, EventArgs e)
        {

        }

        private void tmCheckFist_Tick(object sender, EventArgs e)
        {
            //if (BeeCore.Common.listCamera[Global. IndexProgChoose].IsConnected)
            //{
            //    BeeCore.Common.listCamera[Global. IndexProgChoose].Read();
            //    if (BeeCore.Common.listCamera[Global. IndexProgChoose].IsConnected)
            //    {
            //       // Propety.Check();
            //        tmCheckFist.Enabled = false;
            //    }
                    
            //}
        
        }

        private void rjButton3_Click_2(object sender, EventArgs e)
        {
           
        }

        private void rjButton7_Click(object sender, EventArgs e)
        {
           
        }

      
        

        private void btnAddModel_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();

            switch(Propety.TypeYolo)
            {
                case TypeYolo.YOLO:
                    OpenFileDialog.Filter = "Model|*.pt";
                    break;
                case TypeYolo.Onnx:
                    OpenFileDialog.Filter = "Onnx|*.xml";
                    break;
                case TypeYolo.RCNN:
                    OpenFileDialog.Filter = "RCNN|*.pth";
                    break;
        }
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                
                String pathModel = OpenFileDialog.FileName;


                String NameModel = Path.GetFileName(pathModel);
                pathModel = "Program\\" + Global.Project + "\\" + NameModel;
                switch (Propety.TypeYolo)
                {
                    case TypeYolo.YOLO:
                        if (Propety.listModels == null) Propety.listModels = new List<string>();
                       
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
                           
                        
                        break;
                    case TypeYolo.Onnx:
                        NameModel = new DirectoryInfo(
                            Path.GetDirectoryName(OpenFileDialog.FileName)
                        ).Name;
                        //pathModel =Path.GetPathRoot(OpenFileDialog.FileName);
                        //NameModel = Path.GetDirectoryName(OpenFileDialog.FileName);// Path.GetFileNameWithoutExtension(OpenFileDialog.FileName);
                        pathModel = "Program\\" + Global.Project + "\\" + NameModel;
                        if (Propety.listModelOnnx == null) Propety.listModelOnnx = new List<string>();
                            Batch.CopyAndRename(Path.GetDirectoryName(OpenFileDialog.FileName), pathModel, false);

                            Propety.listModelOnnx.Add(NameModel);
                            Propety.listModelOnnx = Propety.listModelOnnx.Distinct().ToList();
                            cbListModel.DataSource = null;
                            Propety.PathModel = Path.GetFileName(pathModel);
                            IsReload = true;
                        if (!workLoadModel.IsBusy)
                            workLoadModel.RunWorkerAsync();
                        cbListModel.DataSource = Propety.listModelOnnx.ToArray();
                        cbListModel.Text = Propety.PathModel;
                        break;
                    case TypeYolo.RCNN:
                        break;
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


                switch(Propety.TypeYolo)
                {
                    case TypeYolo.YOLO:
                        if (File.Exists(Propety.pathFullModel))
                        {
                            Propety.SetModel();
                        }
                        break;
                    case TypeYolo.Onnx:
                        if (Directory.Exists(Propety.pathFullModel))
                        {
                            Propety.SetModel();
                        }
                        break;
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

            float imageWidth = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Width;
            float imageHeight = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Height;

            RotatedRect rrect = new RotatedRect(
                new Point2f(Propety.rotCrop._PosCenter.X, Propety.rotCrop._PosCenter.Y),
                new Size2f(Propety.rotCrop._rect.Width, Propety.rotCrop._rect.Height),
                Propety.rotCrop._rectRotation
            );

            Mat matCrop = BeeCore.Cropper.CropRotatedRect(
                BeeCore.Common.listCamera[Global.IndexCCCD].matRaw,
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
                        BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.SaveImage(fullImagePath);

                        string fullLabelPath = Path.Combine(pathLabel, labelName);
                        File.WriteAllLines(fullLabelPath, listLabelTrainYolo);
                    }
                    else
                    {

                        string lb = SaveYoloRects(
                            Propety.rectTrain,
                            BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Width,
                            BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Height, new List<string>(),
                            Propety.labelItems
                                    );

                        string imageName = strImgName + ".jpg";
                        string labelName = strImgName + ".txt";

                        string pathImage = Path.Combine("Program", Global.Project, "DataSet", "train", "images");
                        string pathLabel = Path.Combine("Program", Global.Project, "DataSet", "train", "labels");

                        Directory.CreateDirectory(pathImage);
                        Directory.CreateDirectory(pathLabel);

                        string fullImagePath = Path.Combine(pathImage, imageName);
                        BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.SaveImage(fullImagePath);

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
            if(t.SelectedIndex ==2)
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
               //Global.TypeCrop= TypeCrop.Crop;
               // Propety.TypeCrop = Global.TypeCrop;
                if (Propety.rotCrop == null)
                {
                    int with = 50, height = 50;
                    Propety.rotCrop = new RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(BeeCore.Common.listCamera[Global. IndexProgChoose].matRaw.Width / 2, BeeCore.Common.listCamera[Global. IndexProgChoose].matRaw.Height / 2), 0, AnchorPoint.None);

                }
             
            }
            else
            {
                Propety.rotCrop = null;
               //Global.TypeCrop= TypeCrop.Area;
               // Propety.TypeCrop = Global.TypeCrop;

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
            //else if(Propety.listLabel != null)
            //Propety.listLabel.Clear();
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
                Propety.Training(Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Name, Propety.pathFullModel, pathYaml);

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
            switch(Propety.TypeYolo)
            {
                case TypeYolo.YOLO:
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
                    break;
                case TypeYolo.Onnx:
                    if (!Directory.Exists(Propety.pathFullModel))
                    {
                        Propety.listModelOnnx.Remove(Propety.PathModel);
                        if (Propety.listModelOnnx.Count > 0)
                        {
                            Propety.PathModel = Propety.listModelOnnx[Propety.listModelOnnx.Count - 1];
                            Propety.pathFullModel = "Program\\" + Global.Project + "\\" + Propety.PathModel;

                        }
                        else
                        {
                            Propety.PathModel = "";
                            Propety.pathFullModel = "";
                        }
                        cbListModel.DataSource = null;
                        cbListModel.DataSource = Propety.listModelOnnx;
                        cbListModel.Refresh();
                        cbListModel.Text = Propety.PathModel;
                    }

                    if (Directory.Exists(Propety.pathFullModel))
                    {
                        cbListModel.Enabled = false;

                        workLoadModel.RunWorkerAsync();
                        //  Propety.listLabelCompare = new List<Labels>();
                        //RefreshLabels();

                    }
                    break;
            }    
           
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure", "Reload All Para of Label", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
               
                String[] Content = Propety.LoadNameModel(Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Name);
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
                if (BeeCore.Common.listCamera[Global.IndexCCCD].matRaw != null)
                    if (!BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Empty())
                        BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Release();
               
                BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = Cv2.ImRead(openFile.FileName);
              
              Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();


                strImgName =Path.GetFileNameWithoutExtension( openFile.FileName);
            }

            ClearImgBox();
        }
        public List<PointF[]> listBoxCorners = new List<PointF[]>();
        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (!Common.PropetyTools[Global.IndexProgChoose][Global.IndexToolSelected].worker.IsBusy)
            {
                Common.PropetyTools[Global.IndexProgChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
            }
                
            else
                btnCheck.IsCLick = false;
            
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
         
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            Common.PropetyTools[Global.IndexProgChoose][Global.IndexToolSelected].IsSendResult = btnEnable.IsCLick;

        }

        private void txtAddPLC_TextChanged(object sender, EventArgs e)
        {
            Propety.AddPLC =txtAddPLC.Text.Trim();
        }

        private void btnBits_Click(object sender, EventArgs e)
        {
           // Common.PropetyTools[Global.IndexProgChoose][Global.IndexToolSelected].TypeSendPLC = TypeSendPLC.Bits;
        }

        private void btnString_Click(object sender, EventArgs e)
        {
           // Common.PropetyTools[Global.IndexProgChoose][Global.IndexToolSelected].TypeSendPLC = TypeSendPLC.String;
        }

        private void btnMergeBox_Click(object sender, EventArgs e)
        {
            Propety.FilterBox = FilterBox.Merge;
            AdjOverLap.Enabled = true;
        }

        private void btnRemoveBox_Click(object sender, EventArgs e)
        {
            Propety.FilterBox = FilterBox.Remove;
            AdjOverLap.Enabled = true;
        }

        private void dashboardLabel_Click(object sender, EventArgs e)
        {

        }

        private void btnNoneBox_Click(object sender, EventArgs e)
        {
            Propety.FilterBox = FilterBox.None;
            AdjOverLap.Enabled = false;
        }

        private void AdjOverLap_ValueChanged(float obj)
        {
            Propety.ThreshOverlap = AdjOverLap.Value;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            EditRectRot1.Visible = !btn2.IsCLick;
        //    lay21.Visible = !btn2.IsCLick;
        //    lay22.Visible = !btn2.IsCLick;
        //    lay23.Visible = !btn2.IsCLick;
        //    lay24.Visible = !btn2.IsCLick;
           
        }

        //private void btnArea_Click(object sender, EventArgs e)
        //{
          
        //    bool IsCLick = btnArea.IsCLick ;
        //    if(IsCLick)
        //    Global.RotateCurentChanged += Global_RotateCurentChanged;
        //    else
        //    Global.RotateCurentChanged -= Global_RotateCurentChanged;
        //    btnElip.IsCLick = Propety.rotArea.Shape == ShapeType.Ellipse ? true : false;
        //    btnRect.IsCLick = Propety.rotArea.Shape == ShapeType.Rectangle ? true : false;
        //    btnHexagon.IsCLick = Propety.rotArea.Shape == ShapeType.Hexagon ? true : false;
        //    btnPolygon.IsCLick = Propety.rotArea.Shape == ShapeType.Polygon ? true : false;
        //    btnWhite.IsCLick = Propety.rotArea.IsWhite;
        //    btnBlack.IsCLick = !Propety.rotArea.IsWhite;
        //    layListScan.Visible = false;
        //}

        private void Global_RotateCurentChanged(RectRotate obj)
        {
           //if(btnArea.IsCLick)
           //Propety.rotArea = obj;
           // if (btnArea.IsCLick)
           //     Propety.rotArea = obj;
        }

        private void btnMask_Click(object sender, EventArgs e)
        {
            Global.StatusDraw = StatusDraw.Edit;
           // lay2Mask.Visible = true;
            //Global.TypeCrop = TypeCrop.Mask;
            //Propety.TypeCrop = Global.TypeCrop;
           
            //if (Propety.rotMask == null)
            //{
            //    Propety.rotMask = new RectRotate();
            //}
            //btnElip.IsCLick = Propety.rotMask.Shape == ShapeType.Ellipse ? true : false;
            //btnRect.IsCLick = Propety.rotMask.Shape == ShapeType.Rectangle ? true : false;
            //btnHexagon.IsCLick = Propety.rotMask.Shape == ShapeType.Hexagon ? true : false;
            //btnPolygon.IsCLick = Propety.rotMask.Shape == ShapeType.Polygon ? true : false;
            //btnWhite.IsCLick = Propety.rotMask.IsWhite;
            //btnBlack.IsCLick = !Propety.rotMask.IsWhite;
            //layListScan.Visible =true;
        }
        ShapeType ShapeType = ShapeType.Rectangle;
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


           // Global.TypeCrop = which;
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;



        }
        private void NewShape(ShapeType newShape,int W=0,int H=0)
        {
            // 1) Chốt shape hiện tại
            var prop = BeeCore.Common.PropetyTools[Global.IndexProgChoose][Global.IndexToolSelected].Propety2;
            RectRotate rr = null;
            //if (Global.TypeCrop == TypeCrop.Area) rr = prop?.rotArea;
            //else if (Global.TypeCrop == TypeCrop.Mask) rr = prop?.rotMask;
            //else rr = prop?.rotCrop;

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
            if (W != 0 && H != 0)
            {rr._PosCenter = new PointF(Global.Config.SizeCCD.Width/2, Global.Config.SizeCCD.Height / 2);
                rr._rect = new RectangleF(-W / 2, -H / 2, W, H);
            }
            // Cập nhật về prop
            if (Global.TypeCrop == TypeCrop.Area) prop.rotArea = rr;
            else if (Global.TypeCrop == TypeCrop.Mask) prop.rotMask = rr;
            else prop.rotCrop = rr;


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

        private void rjButton3_Click_3(object sender, EventArgs e)
        {
          
        }

        //private void btnNone_Click(object sender, EventArgs e)
        //{
        //    switch (Global.TypeCrop)
        //    {
        //        case TypeCrop.Crop:

        //            Propety.rotCrop.Shape = btnElip.IsCLick == true ? ShapeType.Ellipse : ShapeType.Rectangle;
        //            break;
        //        //case TypeCrop.Area:
        //        //    Propety.rotArea.Shape= btnElip.IsCLick==true ? ShapeType.Ellipse: ShapeType.Rectangle;
        //        //    break;
        //        case TypeCrop.Mask:
        //            Propety.rotMask = null;// = btnElip.IsCLick;
        //            break;

        //    }
        //}

        //private void btnNewShape_Click(object sender, EventArgs e)
        //{
        //    NewShape(ShapeType,(int)numW.Value, (int)numH.Value);
        //    Global.StatusDraw = StatusDraw.Edit;
        //}

        //private void btnWhite_Click(object sender, EventArgs e)
        //{
        //    switch (Global.TypeCrop)
        //    {
        //        case TypeCrop.Area:
        //            Propety.rotArea.IsWhite = btnWhite.IsCLick;
        //            break;
        //        case TypeCrop.Crop:
        //            Propety.rotCrop.IsWhite = btnWhite.IsCLick;
        //            break;
        //        case TypeCrop.Mask:
        //            Propety.rotMask.IsWhite = btnWhite.IsCLick;
        //            break;
        //    }
        //}

        //private void btnBlack_Click(object sender, EventArgs e)
        //{
        //    switch (Global.TypeCrop)
        //    {
        //        case TypeCrop.Area:
        //            Propety.rotArea.IsWhite = !btnBlack.IsCLick;
        //            break;
        //        case TypeCrop.Crop:
        //            Propety.rotCrop.IsWhite = !btnBlack.IsCLick;
        //            break;
        //        case TypeCrop.Mask:
        //            Propety.rotMask.IsWhite = !btnBlack.IsCLick;
        //            break;
        //    }
        //}

        //private void btnCrop_Click(object sender, EventArgs e)
        //{ 
        //   Propety.IsLine = true;
        //    if (Propety.rotCrop == null) Propety.rotCrop = new RectRotate();
        //    Global.StatusDraw = StatusDraw.Edit;
        //    Global.TypeCrop = TypeCrop.Crop;
        //    Propety.TypeCrop = Global.TypeCrop;
        //    btnElip.IsCLick = Propety.rotCrop.Shape == ShapeType.Ellipse ? true : false;
        //    btnRect.IsCLick = Propety.rotCrop.Shape == ShapeType.Rectangle ? true : false;
        //    btnHexagon.IsCLick = Propety.rotCrop.Shape == ShapeType.Hexagon ? true : false;
        //    btnPolygon.IsCLick = Propety.rotCrop.Shape == ShapeType.Polygon ? true : false;
        //    btnWhite.IsCLick = Propety.rotCrop.IsWhite;
        //    btnBlack.IsCLick = !Propety.rotCrop.IsWhite;
        //    layListScan.Visible = false;
        //}

        private void btnOnline_Click(object sender, EventArgs e)
        {
            Propety.IsLine = btnOnline.IsCLick;
         //   btnCrop.Enabled = Propety.IsLine;
            laySetLine.Visible = Propety.IsLine;
            laySetLine2.Visible = Propety.IsLine;
            laySetLine3.Visible = Propety.IsLine;
            Propety.rotCrop.IsVisible =! Propety.IsLine;
            if (EditRectRot1.Rot.Count>0)
            EditRectRot1.Rot[1].IsVisible = Propety.rotCrop.IsVisible;
            EditRectRot1.Refresh(true);
        }

        private void tnOffLine_Click(object sender, EventArgs e)
        {
            Propety.IsLine = !btnOffLine.IsCLick;
          //  btnCrop.Enabled = Propety.IsLine;
            laySetLine.Visible = Propety.IsLine;
            laySetLine2.Visible = Propety.IsLine;
            laySetLine3.Visible = Propety.IsLine;
            Propety.rotCrop.IsVisible = !Propety.IsLine;
            if (EditRectRot1.Rot.Count > 0)
                EditRectRot1.Rot[1].IsVisible = Propety.rotCrop.IsVisible;
            EditRectRot1.Refresh(true);
        }

        private void btnSetThreshLine_Click(object sender, EventArgs e)
        {
            Propety.LenTemp = Propety.LenRS;
            lbLen.Text = Propety.LenTemp.ToString();
        }

        private void AdjThreshLine_ValueChanged(float obj)
        {
            Propety.ThresholdLine = AdjThreshLine.Value;
        }

        private void AdjTolerance_ValueChanged(float obj)
        {
            Propety.ToleranceLine = AdjTolerance.Value;
        }

        private void btnTypeYolo_Click(object sender, EventArgs e)
        {
            Propety.TypeYolo = TypeYolo.YOLO;
        }

        private void btnTypeOnnx_Click(object sender, EventArgs e)
        {
            Propety.TypeYolo = TypeYolo.Onnx;
            if (Propety.listModelOnnx != null)
            {
                Propety.listModelOnnx = Propety.listModelOnnx.Distinct().ToList();
                cbListModel.DataSource = null;

                IsReload = true;

                cbListModel.DataSource = Propety.listModelOnnx.ToArray();
                cbListModel.Text = Propety.PathModel;
            }

        }

        private void btnTypeRCNN_Click(object sender, EventArgs e)
        {
            Propety.TypeYolo=TypeYolo.RCNN;
        }

        private void btnAddToList_Click(object sender, EventArgs e)
        {
            RectRotate rot = Propety.rotMask.Clone();
            PointF pCenter = Propety.rotArea.WorldToLocal(rot._PosCenter);

            rot._PosCenter = pCenter;
            Propety.ListRotMask.Add(rot);
            Global.EditTool.View.imgView.Invalidate();
        }

        private void btnUnoMask_Click(object sender, EventArgs e)
        {
            if (Propety.ListRotMask.Count() > 0)
                Propety.ListRotMask.RemoveAt(Propety.ListRotMask.Count() - 1);
            Global.EditTool.View.imgView.Invalidate();
        }

        private void btnClearAllMask_Click(object sender, EventArgs e)
        {
            if (Propety.ListRotMask.Count() > 0)
                Propety.ListRotMask.Clear();
            Global.EditTool.View.imgView.Invalidate();
        }

        private void dashboardLabel_ChooseAreaRequest(int arg1, LabelItem arg2)
        {
            // ví dụ lấy box từ vision engine

        }

        private void dashboardLabel_ChooseAreaBegin(int arg1, LabelItem arg2)
        {
            this.Invoke((Action)(async () =>
            {
                _currentLabel = arg2;
             
               
                Global.ChooseRotChage += Global_ChooseRotChage;
                Propety.ModeCheck = ModeCheck.Multi;
                Propety.NameChoose = arg2.Name;
                if (_currentLabel.ListInsideBox!=null)
                
                foreach (RectRotate rot in Propety.listRotScan)
                {
                    if(_currentLabel.ListInsideBox.FindIndex(a=>a==rot) < 0)
                            rot._dragAnchor = AnchorPoint.None;
                    else
                        rot._dragAnchor = AnchorPoint.Center;

                }
              
                Global.StatusDraw = StatusDraw.Scan;
            

                Global.EditTool.View.imgView.Invalidate();
            }));
        }

        private void Global_ChooseRotChage(int obj)
        {if (obj ==-1) return;
        if (obj>=Propety.listRotScan.Count()) return;
            RectRotate rot= Propety.listRotScan[Global.IndexRotChoose];
     
            if (_currentLabel.ListInsideBox == null)
                _currentLabel.ListInsideBox = new List<RectRotate>();
            if (_currentLabel.ListInsideBox.FindIndex(a => a == rot) == -1)
            {
                rot._dragAnchor = AnchorPoint.Center;
                _currentLabel.ListInsideBox.Add(rot);
            }    
              
            else
            {
                rot.Name = "";
                rot._dragAnchor = AnchorPoint.None;
                _currentLabel.ListInsideBox.Remove(rot);
            }    
              
        }
        LabelItem _currentLabel;
        private void dashboardLabel_ChooseAreaEnd(int arg1, LabelItem arg2)
        {
            this.Invoke((Action)(async () =>
            {
                Global.ChooseRotChage -= Global_ChooseRotChage;
                Global.StatusDraw = StatusDraw.Edit;
                Global.EditTool.View.imgView.Invalidate();
                Propety.IsScan = false;
                Propety.NameChoose = "";
               
              
            }));
        }

        private void btn1M_Click(object sender, EventArgs e)
        {
            layTypeYolo.Visible = !btn1M.IsCLick;
            layoutSetLearning.Visible=!btn1M.IsCLick;
        }

        private void btnNoCropMask_Click(object sender, EventArgs e)
        {
            Propety.IsCropSingle = !btnNoCropMask.IsCLick;
        }

        private void btnCropMask_Click(object sender, EventArgs e)
        {
            Propety.IsCropSingle = btnCropMask.IsCLick;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Propety.ModeCheck = ModeCheck.Single;
            Propety.listRotScan = Propety.ListRotMask;
            //Propety.IsScan = btnEdit.IsCLick; ;

            //if (btnEdit.IsCLick)
            //{
            //    btnEdit.Text = "OK"; Global.StatusDraw = StatusDraw.Scan;
            //    //RectRotate rot = Propety.ListRotMask[(int)numIndexArea.Value];
            //    //PointF pCenter = Propety.rotArea.LocalToWorld(rot._PosCenter);

            //    //rot._PosCenter = pCenter;
            //    //Propety.rotMask = rot;
            //}
            //else
            //{
            //    if (Propety.rotMask != null)
            //    { 
            //        btnEdit.Text = "Edit";
            //        RectRotate rot = Propety.rotMask.Clone();
            //        PointF pCenter = Propety.rotArea.WorldToLocal(rot._PosCenter);
            //        rot._PosCenter = pCenter;
            //        Propety.rotMask = new RectRotate();
                    
            //        Propety.ListRotMask[Propety.IndexProgChoose] = rot; 
            //        Global.StatusDraw = StatusDraw.Edit;
            //    }

            //}

         
        }
        private LabelItem _currentColorItem;
        private void dashboardLabel_ChooseColorBegin(int arg1, LabelItem arg2)
        {
            _currentColorItem = arg2;
           // arg2.SampleColor = Global.ColorSample;
            // bật chế độ pick màu
            Global.IsGetColor = true;
          

            Global.ColorGp = ColorGp.HSV;

            Global.StatusDraw = StatusDraw.Color;
           

        }
        private HSVCli HSVCli;
      

        private void dashboardLabel_ChooseColorEnd(int arg1, LabelItem arg2)
        {
          //  arg2.SampleColor = Global.ColorSample;
            Global.IsGetColor = false;
            Global.StatusDraw = StatusDraw.Edit;
            Propety.SetListTemp();

        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.Equal;
        }

        private void btnMore_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.More;
        }

        private void btnLess_Click(object sender, EventArgs e)
        {
            Propety.Compare = Compares.Less;
            //   Propety.
        }

        private void rjButton4_Click(object sender, EventArgs e)
        {
            lay1.Visible =! btn1.IsCLick;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            trackScore.Visible = !btn3.IsCLick;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            layComparison.Visible = !btn4.IsCLick;
        }

        private void btnAreaLimit_Click(object sender, EventArgs e)
        {
           // Global.rotAreaAdj
        }

        private void btn3_Click_1(object sender, EventArgs e)
        {
            trackScore.Visible = !btn3.IsCLick;
        }

        private void btn22_Click(object sender, EventArgs e)
        {
            dashboardLabel.Visible = !btn22.IsCLick;
            layDir.Visible = !btn22.IsCLick;
        }

        private void btn23_Click(object sender, EventArgs e)
        {
            Lay2.Visible = !btn23.IsCLick;
            lay32.Visible = !btn23.IsCLick;
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            Global.Dir = Dir.Left;
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            Global.Dir = Dir.Right;
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            Global.Dir=Dir.None;
        }

        private void btnCLAll_Click(object sender, EventArgs e)
        {
            Propety.IsColorAllObjLabel = true;
        }

        private void lbClOne_Click(object sender, EventArgs e)
        {
            Propety.IsColorAllObjLabel = false;
        }
    }
}

