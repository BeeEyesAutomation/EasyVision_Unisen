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
using System.Web.UI.WebControls;
using System.Windows.Forms;
using BeeCore;
using BeeCore.Parameter;
using BeeUi.Common;
using BeeUi.Commons;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Label = System.Windows.Forms.Label;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace BeeUi.Tool
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
        public BackgroundWorker worker = new BackgroundWorker();
     
        Stopwatch timer = new Stopwatch();
        public void LoadPara()
        {
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;

            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        
            if (Propety.listModels == null) Propety.listModels = new List<string>();
            Propety.listModels = Propety.listModels.Distinct().ToList();
            if (!workLoadModel.IsBusy)
            workLoadModel.RunWorkerAsync();
            IsReload = true;
            cbListModel.DataSource = Propety.listModels;
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
            
            G.TypeCrop = TypeCrop.Area;
             CustomGui.RoundRg(tabLbs, 10, Corner.Bottom);
            //picTemp1.Image = Propety.matTemp;
            //picTemp2.Image = Propety.matTemp2;
            
          //  txtModel.Text = Propety.PathModel;
            trackScore.Value = Propety.Score;
           btnEnLineLimit.IsCLick = Propety.IsCheckArea ;
            numScore.Value = Propety.Score;
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

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //  MessageBox.Show("Worker error: " + e.Error.Message);
                return;
            }
            Propety.Complete();
            if (!G.IsRun)
                G.EditTool.View.imgView.Invalidate();
            timer.Stop();

            Propety.cycleTime = (int)timer.Elapsed.TotalMilliseconds;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (G.IsIniPython == false)
                return;
            timer.Restart();
            if (!G.IsRun)
                Propety.rotAreaAdjustment = Propety.rotArea;
            Propety.DoWork(Propety.rotAreaAdjustment);
        }

        private void trackScore_ValueChanged(float obj)
        {
           
            Propety.Score = (int)trackScore.Value;
            numScore.Value = Propety.Score;

        }

        public Yolo Propety=new Yolo();
        public Mat matTemp = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();
        public void GetTemp(RectRotate rotateRect, Mat matRegister)
        {
           
                float angle = rotateRect._rectRotation;
                if (rotateRect._rectRotation < 0) angle = 360 + rotateRect._rectRotation;
                Mat matCrop = G.EditTool.View.CropRotatedRect(matRegister, new RotatedRect(new Point2f(rotateRect._PosCenter.X + (rotateRect._rect.Width / 2 + rotateRect._rect.X), rotateRect._PosCenter.Y + (rotateRect._rect.Height / 2 + rotateRect._rect.Y)), new Size2f(rotateRect._rect.Width, rotateRect._rect.Height), angle));
                if (matCrop.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(matCrop, matTemp, ColorConversionCodes.BGR2GRAY);
                if (Propety.IsAreaWhite)
                    Cv2.BitwiseNot(matTemp, matTemp);
           
        }
      
        public Graphics ShowResult(Graphics gc, float Scale, System.Drawing.Point pScroll)
        {
            if (Propety.rotAreaAdjustment == null&& G.IsRun) return gc;
            gc.ResetTransform();
            // gc.FillEllipse(Brushes.Black, Propety.rotArea._PosCenter.X, Propety.rotArea._PosCenter.Y, 6, 6);
           
            var mat = new Matrix();
            RectRotate rotA = Propety.rotArea;
            if (G.IsRun) rotA = Propety.rotAreaAdjustment;
            if (!G.IsRun)
            {
                mat.Translate(pScroll.X, pScroll.Y);
                mat.Scale(Scale, Scale);
            }
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
            gc.Transform = mat;
            //gc.FillEllipse(Brushes.Blue, -3, -3, 6, 6);
            gc.DrawString(indexTool + "", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, new System.Drawing.Point((int)rotA._rect.X, (int)rotA._rect.Y));
           
            gc.DrawRectangle(new Pen(Color.Silver, 1), new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)rotA._rect.Width, (int)rotA._rect.Height));
            gc.ResetTransform();
            Color cl = Color.LimeGreen;
            Brush brushText = Brushes.White;
            if (!Propety.IsOK)
            {
                cl = Color.Red;
                if (G.PropetyTools[Propety.Index].UsedTool == UsedTool.Invertse &&
                    G.Config.ConditionOK == ConditionOK.Logic)
                    cl = Color.LimeGreen;


            }
            else
            {
                cl = Color.LimeGreen;
                if (G.PropetyTools[Propety.Index].UsedTool == UsedTool.Invertse &&
                    G.Config.ConditionOK == ConditionOK.Logic)
                    cl = Color.Red;
            }
            int i = 0;
            foreach (RectRotate rot in Propety.rectRotates)
            {
                mat = new Matrix();
                if (!G.IsRun)
                {
                    mat.Translate(pScroll.X, pScroll.Y);
                    mat.Scale(Scale, Scale);
                }
                mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                mat.Rotate(rotA._rectRotation);
                mat.Translate(rotA._rect.X, rotA._rect.Y);
                gc.Transform = mat;


                if (Propety.IsCheckArea)
                {
                    mat.Rotate(rot._rectRotation);
                    gc.Transform = mat;
                    gc.DrawLine(new Pen(Color.Gold, 6), new Point(0, Propety.yLine), new Point((int)rotA._rect.Width, Propety.yLine));

                    System.Drawing.Point point1 = new System.Drawing.Point((int)(rot._PosCenter.X), (int)(rot._PosCenter.Y - rot._rect.Height / 2));
                    System.Drawing.Point point2 = new System.Drawing.Point((int)(rot._PosCenter.X), (int)(rot._PosCenter.Y + rot._rect.Height / 2));
                    System.Drawing.Point point3 = new System.Drawing.Point((int)(rot._PosCenter.X - rot._rect.Width / 2), (int)(rot._PosCenter.Y - rot._rect.Height / 2));
                    System.Drawing.Point point4 = new System.Drawing.Point((int)(rot._PosCenter.X + rot._rect.Width / 2), (int)(rot._PosCenter.Y - rot._rect.Height / 2));
                    System.Drawing.Point point5 = new System.Drawing.Point((int)(rot._PosCenter.X - rot._rect.Width / 2), (int)(rot._PosCenter.Y + rot._rect.Height / 2));
                    System.Drawing.Point point6 = new System.Drawing.Point((int)(rot._PosCenter.X + rot._rect.Width / 2), (int)(rot._PosCenter.Y + rot._rect.Height / 2));
                    Color clLine = Color.Red;
                    if (Propety.listOK[i])
                        clLine = Color.Green;
                    gc.DrawLine(new Pen(clLine, 8), point1, point2);
                    gc.DrawLine(new Pen(clLine, 8), point3, point4);
                    gc.DrawLine(new Pen(clLine, 8), point5, point6);
                    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);

                    gc.Transform = mat;
                    int index = i + 1;
                    String content = "(" + Propety.listLabel[i] + ") \n" + Math.Round(Propety.listScore[i], 1) + "%";
                    if (Propety.IsCheckArea)
                        content = rot._rect.Height + " px";
                    Font font = new Font("Arial", 30, FontStyle.Bold);
                    SizeF sz1 = gc.MeasureString(content, font);
                    gc.DrawString(content, font, new SolidBrush(clLine), new System.Drawing.Point((int)(rot._rect.X + rot._rect.Width / 2), (int)(rot._rect.Y + rot._rect.Height / 2 - sz1.Height / 2)));
                    i++;
                    //gc.FillEllipse(Brushes.Black, -3, -3, 6, 6);
                    gc.ResetTransform();
                }
                else
                {
                 
                    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                    gc.Transform = mat;
                    mat.Rotate(rot._rectRotation);
                    gc.Transform = mat;

                    int index = i + 1;
                    //String content = "(" + Propety.listLabel[i] + ") \n" + Math.Round(Propety.listScore[i], 1) + "%";
                    //if (Propety.IsCheckArea)
                    //    content = rot._rect.Height + " px";
                  //  Font font = new Font("Arial", 30, FontStyle.Bold);
                  //  SizeF sz2 = gc.MeasureString(content, font);
                  if(Propety.IsEnContent)
                    Draws.Box2Label(gc, rot._rect, Propety.listLabel[i],"", G.fontRS,cl, brushText,30,3);
                 else
                        Draws.Box2Label(gc, rot._rect, Propety.listLabel[i], Math.Round(Propety.listScore[i], 1) + "%", G.fontRS, cl, brushText, 30, 3);

                    //  Draws.Box1Label(gc, rot._rect, Math.Round(Propety.listScore[i], 1) + "%", G.fontRS, brushText, Brushes.Transparent, true);
                    //  gc.DrawString(content, font, new SolidBrush(cl), new System.Drawing.Point((int)(rot._rect.X + rot._rect.Width / 2 - sz2.Width / 2), (int)(rot._rect.Y + rot._rect.Height / 2 - sz2.Height / 2)));
                    i++;
                    //gc.FillEllipse(Brushes.Black, -3, -3, 6, 6);
                    gc.ResetTransform();
                }

            }
            //if (Propety.rectRotates != null)
            //{
            //    gc.ResetTransform();
            //    var mat2 = new Matrix();
            //    if (!G.IsRun)
            //    {
            //        mat2.Translate(pScroll.X, pScroll.Y);
            //        mat2.Scale(Scale, Scale);
            //    }
            //    mat2.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            //    mat2.Rotate(rotA._rectRotation);
            //    gc.Transform = mat2;
            //    gc.DrawString("Count: " + Propety.rectRotates.Count() + "", new Font("Arial", 16, FontStyle.Bold), Brushes.White, new System.Drawing.Point((int)rotA._rect.X + 20, (int)rotA._rect.Y + 20));

            //}
            gc.ResetTransform();
            mat= new Matrix();
            if (!G.IsRun)
            {
                mat.Translate(pScroll.X, pScroll.Y);
                mat.Scale(Scale, Scale);
            }
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
            gc.Transform = mat;
            String sContent = (int)(Propety.Index + 1) + "." + G.PropetyTools[Propety.Index].Name;
            Draws.Box1Label(gc, rotA._rect, sContent, G.fontTool, brushText, cl);
          //  Draws.Box1Label(gc, rotA._rect, sContent,G.fontTool, Brushes.Black, Brushes.White);
            
            return gc;
        }
        public Graphics ShowEdit(Graphics gc, RectangleF _rect)
        {
            if (matTemp == null) return gc;

            if (G.TypeCrop != TypeCrop.Area)
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
            G.TypeCrop = BeeCore.TypeCrop.Area;
            Propety.TypeCrop = G.TypeCrop;
            
            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.imgView.Cursor = Cursors.Default;
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
               // if (G.IsRun)
               // {
               //     if (G.rotOriginAdj != null)
               //         Propety.rotAreaAdjustment = G.EditTool.View.GetPositionAdjustment(Propety.rotArea, G.rotOriginAdj);
               //     else
               //         Propety.rotAreaAdjustment = Propety.rotArea;
               //     //  Propety.Matching(G.IsRun, BeeCore.Common.matRaw.ToBitmap(), indexTool, Propety.rotAreaAdjustment);
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
            if (G.IsLoad)
                Process();
            await Task.Delay(1000);
            G.EditTool.View.imgView.Invalidate();

            G.ResultBar.lbCycleTrigger.Text = "[" + Propety.cycleTime + "ms]";
        }

        private void trackScore_ValueChanged(object sender, EventArgs e)
        {

        }

        private void trackScore_MouseUp(object sender, MouseEventArgs e)
        {
           

            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();
        }

       

        private void trackMaxOverLap_MouseUp(object sender, MouseEventArgs e)
        {
          

            if (!threadProcess.IsBusy)
                threadProcess.RunWorkerAsync();
        }
        
       
        

      
     
     

     
     
        public void Loads()
        {
            Propety.TypeTool = TypeTool.Yolo;
     
            Propety.TypeMode = Mode.Pattern;
            Propety.pathRaw = G.EditTool.View.pathRaw;
            //Propety.NumObject = 1;
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
            G.IsCancel = true;
            G.ToolSettings.Visible = true; G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void btnAreaBlack_Click(object sender, EventArgs e)
        {
            Propety.IsAreaWhite = false;
             GetTemp(Propety.rotCrop,BeeCore.Common.matRaw );
            G.EditTool.View.imgView.Invalidate();
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
            GetTemp(Propety.rotCrop, BeeCore.Common.matRaw);
            G.EditTool.View.imgView.Invalidate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            G.IsCheck = true;
            G.EditTool.RefreshGuiEdit(Step.Step3);
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
            //G.EditTool.View.imgView.Image = BeeCore.Common.matRaw.ToBitmap();
            //G.EditTool.View.imgView.Invalidate();
            //Propety.NumObject = trackNumObject.Value;
            //G.IsCheck = true;
            //BeeCore.Common.SetRaw();
            //if (!threadProcess.IsBusy)
            //    threadProcess.RunWorkerAsync();
        }

        private void cbTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
           // Propety.TypeYolo = (BeeCore.TypeYolo)cbTypes.SelectedIndex;
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
            Propety.NumObject = trackNumObject.Value;
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

        private void numScore_ValueChanged_1(object sender, EventArgs e)
        {
            Propety.Score= numScore.Value;
            trackScore.Value = Propety.Score;
        }
        bool IsFullSize;
        private void btnCropHalt_Click_1(object sender, EventArgs e)
        {
            G.TypeCrop = BeeCore.TypeCrop.Area;
            Propety.TypeCrop = G.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            G.IsCheck = false;

            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.Cursor = Cursors.Default;
            //if (IsClear)
            //    btnClear.PerformClick();
        }

        private void btnCropFull_Click_1(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-BeeCore.G.ParaCam.SizeCCD.Width / 2, -BeeCore.G.ParaCam.SizeCCD.Height / 2, BeeCore.G.ParaCam.SizeCCD.Width, BeeCore.G.ParaCam.SizeCCD.Height), new PointF(BeeCore.G.ParaCam.SizeCCD.Width / 2, BeeCore.G.ParaCam.SizeCCD.Height / 2), 0, AnchorPoint.None,false);

            G.IsCheck = false;
            G.TypeCrop = BeeCore.TypeCrop.Area;
            Propety.TypeCrop = G.TypeCrop;

            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.Cursor = Cursors.Default;
            //if (IsClear)
            //    btnClear.PerformClick();
        }

        private void btnCropRect_Click_1(object sender, EventArgs e)
        {
            G.TypeCrop = BeeCore.TypeCrop.Crop;
            Propety.TypeCrop = G.TypeCrop;


            G.EditTool.View.imgView.Invalidate();
            G.EditTool.View.imgView.Cursor = Cursors.Default;
        }

        private void btnTest_Click_1(object sender, EventArgs e)
        {
            G.IsCheck = true;
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
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
            G.IsCheck = true;
            G.EditTool.RefreshGuiEdit(Step.Step3);
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            G.IsCancel = true;

            G.EditTool.RefreshGuiEdit(Step.Step3);
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
            if (BeeCore.Camera.IsConnected&&G.PLC.IsConnected)
            {
                BeeCore.Camera.Read();
                if (BeeCore.Camera.IsConnected)
                {
                    Propety.Check();
                    tmCheckFist.Enabled = false;
                }
                    
            }
        
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
            Propety.yLine = numLine.Value;
            G.IsCheck = true;
            if (!worker.IsBusy&&!G.IsRun)
                worker.RunWorkerAsync();
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
                        pathModel = "Program\\" + G.Project + "\\" + NameModel;
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

                   
                    String[] Content = Propety.LoadNameModel(Propety.nameTool);
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
            String pathModel = "Program\\" + G.Project + "\\" + Propety.PathModel;
           

            if (File.Exists(pathModel))
            {
              
                Propety.SetModel(Propety.nameTool, pathModel, TypeYolo.YOLO);
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
                String pathModel = "Program\\" + G.Project + "\\" + Propety.PathModel;
                if (File.Exists(pathModel))
                {
                    Propety.SetModel(Propety.nameTool, pathModel, TypeYolo.YOLO);
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
                CopyDirectory(Propety.PathDataSet, "Program\\" + G.Project + "\\DataSet",true);
             
              }
        }
        public String sClass = "";
        public bool IsUpdateImgCrop = false;
        private void btnDraw_Click(object sender, EventArgs e)
        {  // Thông tin ảnh
            float imageWidth = BeeCore.Common.matRaw.Width;
            float imageHeight = BeeCore.Common.matRaw.Height;
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
             Mat matCrop=   BeeCore.Common.CropRotatedRect(BeeCore.Common.matRaw, Propety.rotCrop,Propety.rotMask);
            G.listImgTrainYolo.Add(matCrop.ToBitmap());
            G.listLabelTrainYolo.Add(cbLabels.Text);
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
                G.TypeCrop = BeeCore.TypeCrop.Crop;
                Propety.TypeCrop = G.TypeCrop;
                if (Propety.rotCrop == null)
                {
                    int with = 50, height = 50;
                    Propety.rotCrop = new BeeCore.RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(BeeCore.Common.matRaw.Width / 2, BeeCore.Common.matRaw.Height / 2), 0, BeeCore.AnchorPoint.None,false);

                }
                G.EditTool.View.imgView.Invalidate();
                G.EditTool.View.imgView.Cursor = Cursors.Default;
            }
            else
            {
                Propety.rotCrop = null;
                G.TypeCrop = BeeCore.TypeCrop.Area;
                Propety.TypeCrop = G.TypeCrop;

                G.EditTool.View.imgView.Invalidate();
                G.EditTool.View.imgView.Cursor = Cursors.Default;

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String nameFile = Path.GetFileName(G.EditTool.View.pathFileSeleted);
            String nameFileWithOut = Path.GetFileNameWithoutExtension(G.EditTool.View.pathFileSeleted);
            File.Copy(G.EditTool.View.pathFileSeleted, "Program\\" + G.Project + "\\DataSet\\images\\train\\" + nameFile, true);
            File.Copy(G.EditTool.View.pathFileSeleted, "Program\\" + G.Project + "\\DataSet\\images\\val\\" + nameFile, true);
            string outputPath = "Program\\" + G.Project + "\\DataSet\\labels\\train\\" + nameFileWithOut + ".txt";
            File.WriteAllText("Program\\" + G.Project + "\\DataSet\\labels\\train\\" + nameFileWithOut + ".txt", sClass);
            File.WriteAllText("Program\\" + G.Project + "\\DataSet\\labels\\val\\" + nameFileWithOut + ".txt", sClass);
            sClass = "";
            G.listImgTrainYolo = new List<Bitmap>();
            G.listLabelTrainYolo = new List<string>();
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
            //string outputPath2 = "Program\\" + G.Project + "\\DataSet\\labels\\val\\" + nameFileWithOut + ".txt";
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
            xImgCrop = 10;
            for (int i = G.listImgTrainYolo.Count - 1; i >= 0; i--)
            {

                int W = G.listImgTrainYolo[i].Width;
                int H = G.listImgTrainYolo[i].Height;
                double Scale = (imgCrop.Height - 20) / (H * 1.0);
                W = (int)(W * Scale);
                e.Graphics.DrawImage(G.listImgTrainYolo[i], new Rectangle(xImgCrop, 5, W, imgCrop.Height - 20));
                e.Graphics.DrawString(G.listLabelTrainYolo[i], new Font("Arial", 12), Brushes.Black, xImgCrop, imgCrop.Height - 25);
                xImgCrop += W + 20;
            }
            if(IsUpdateImgCrop)
            {
                IsUpdateImgCrop = false;
                imgCrop.Size = new Size(xImgCrop, imgCrop.Height);
            }
            
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
            Propety.Epoch = numEpoch.Value;
        }

        private void btnTraining_Click(object sender, EventArgs e)
        {
            workTrain.RunWorkerAsync();
        }

        private void workTrain_DoWork(object sender, DoWorkEventArgs e)
        {
            Propety.Training(Propety.nameTool, "Program\\NIDEC_MH_DEMO2\\DataSet\\data.yaml");

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

