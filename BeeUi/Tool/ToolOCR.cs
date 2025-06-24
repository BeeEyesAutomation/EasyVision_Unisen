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
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using BeeCore;
using BeeCore.Funtion;
using BeeUi.Common;
using BeeUi.Commons;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using Label = System.Windows.Forms.Label;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace BeeUi.Tool
{
    [Serializable()]
    public partial class ToolOCR : UserControl
    {
        
        public ToolOCR( )
        {
            InitializeComponent();
            
        }
        bool IsIni = false;
        public BackgroundWorker worker = new BackgroundWorker();
        Stopwatch timer = new Stopwatch();
        public void RefreshLabels()
        {
            if (Propety.listLabelResult == null)
                return;
            int index = 0;
            tabLabelResult.Height = 0;
            tabLabelResult.Controls.Clear();
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 2; col++)
                {

                    if (index >= Propety.listLabelResult.Count)
                        break;
                    if (col == 0)
                        tabLabelResult.Height += 40;
                    RJButton btn = new RJButton();
                    btn.Text = Propety.listLabelResult[index];
                    btn.Font = new Font("Arial", 11);
                    btn.IsUnGroup = true;
                    btn.ForeColor = Color.Black;
                    btn.BorderRadius = 10;
                    btn.Height = 30;
                    if (Propety.listScore[index] > Propety.Score)
                        btn.IsCLick = true;
                    else
                        btn.IsCLick = false;

                  
                    btn.Corner = Corner.Both;
                    btn.BackColor = Color.FromArgb(200, 200, 200);
                    btn.Dock = DockStyle.Fill;
                    btn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    btn.Margin = new Padding(3);

                    tabLabelResult.Controls.Add(btn, col, row);
                    index++;
                }

            }

        }
       
        public void LoadPara()
        {
           
           
            worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                timer = new Stopwatch();
                timer.Restart();
                if (!G.IsRun)
                    Propety.rotAreaAdjustment = Propety.rotArea;
                Propety.DoWork(Propety.rotAreaAdjustment);
            };

            worker.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    //  MessageBox.Show("Worker error: " + e.Error.Message);
                    return;
                }
              
                Propety.Complete();
                if(!G.IsRun)
                {
                    RefreshLabels();
                    G.EditTool.View.imgView.Invalidate();
                    btnTest.Enabled = true;
                }
                   
                timer.Stop();

              Propety. cycleTime = (int)timer.Elapsed.TotalMilliseconds;
            };
            //String nameModel = G.Project + ".pt";
            //String PathProg = "Program\\" + nameModel;
            // if(Propety.PathModel!=null)
            //if (File.Exists(Propety.PathModel))
            if (!workLoadModel.IsBusy)//&& !G.IsIniOCR
            {
               // G.IsIniOCR = true;
                workLoadModel.RunWorkerAsync();

            }
            else
                Propety.StatusTool = StatusTool.Initialed;
         
            G.TypeCrop = TypeCrop.Area;
            txtContent.Text = Propety.Matching;
           
            trackScore.Value =Propety.Score;
            numScore.Value = Propety.Score;
            btnEnLimitArea.IsCLick = Propety.IsEnLimitArea ;
            layoutLineLimit.Enabled = Propety.IsEnLimitArea;
            numCLAHE.Value = Propety.Clahe;
            numUnsharp.Value = Propety.Sigma;
            numBlur.Value = Propety.Blur;
            Propety.rotMask = null;
            if(Propety.sAllow!=null)
            txtAllow.Text = Propety.sAllow;
            switch (Propety.Compare)
            {
             
                case Compares.Less:
                    btnLessArea.IsCLick = true;
                    break;
                case Compares.More:
                    btnMoreArea.IsCLick = true;
                    break;
            }
          
     
          
        }
        private void trackScore_ValueChanged(float obj)
        {
           
            Propety.Score = (int)trackScore.Value;
            numScore.Value = Propety.Score;

        }

        public OCR Propety=new OCR();
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

        void DrawCharactersEvenly(Graphics g, string[] textArray, RectangleF box, Font font, Brush brush)
        {
            int numChars = textArray.Length;
            if (numChars == 0) return;
          //  (int)rot._rect.X, (int)rot._rect.Y
            //box.X = 0;box.Y = 0;
            // Chiều rộng mỗi ký tự
            float charBoxWidth = (float)box.Width / numChars;

            for (int i = 0; i < numChars; i++)
            {
                string character = textArray[i];

                // Tính box nhỏ cho mỗi chữ
                float charX = box.X + i * charBoxWidth;
                RectangleF charRect = new RectangleF(charX, box.Y, charBoxWidth, box.Height);

                // Đo kích thước thật của ký tự
                SizeF charSize = g.MeasureString(character, font);

                // Tính vị trí vẽ để ký tự nằm giữa box nhỏ
                float drawX = charRect.X + (charRect.Width - charSize.Width) / 2;
                float drawY = charRect.Y + (charRect.Height - charSize.Height) / 2;

                g.DrawString(character, font, brush, drawX, drawY);
            }
        }
        public Graphics ShowResult(Graphics gc, float Scale, System.Drawing.Point pScroll)
        {
            try
            {
                if (Propety.rotAreaAdjustment == null && G.IsRun) return gc;
                gc.ResetTransform();
                // gc.FillEllipse(Brushes.Black, Propety.rotArea._PosCenter.X, Propety.rotArea._PosCenter.Y, 6, 6);

              
                RectRotate rotA = Propety.rotArea;
                if (G.IsRun) rotA = Propety.rotAreaAdjustment;
                var mat = new Matrix();
                //mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
                //mat.Rotate(rotA._rectRotation);
                //gc.Transform = mat;
                ////gc.FillEllipse(Brushes.Blue, -3, -3, 6, 6);
                //gc.DrawString(indexTool + "", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, new System.Drawing.Point((int)rotA._rect.X, (int)rotA._rect.Y));

                //gc.DrawRectangle(new Pen(Color.Silver, 1), new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)rotA._rect.Width, (int)rotA._rect.Height));
                //gc.ResetTransform();
                Color cl = Color.LimeGreen;
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
                Brush brushText = Brushes.White;
                gc.ResetTransform();
                mat = new Matrix();
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
                int i = 0;
                if (Propety.listLabelResult.Count() != Propety.rectRotates.Count())
                    return gc;
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

                    mat.Translate(rot._PosCenter.X, rot._PosCenter.Y);
                    mat.Rotate(rot._rectRotation);
                    gc.Transform = mat;
                   
                    //String sDraw = "";
                    //foreach (String ss in Propety.listContent)
                    //    sDraw += ss;
                    Draws.Box2Label(gc, rot._rect, Propety.listLabelResult[i], Math.Round(Propety.listScore[i], 1) + "%", G.fontRS, cl, brushText, 50, 8, 50);

                    //gc.DrawRectangle(new Pen(cl, 4), new Rectangle((int)rot._rect.X, (int)rot._rect.Y, (int)rot._rect.Width, (int)rot._rect.Height));

                    //    int index = i + 1;
                    //    String content =   Propety.listLabel[i] + "";// + Math.Round(Propety.listScore[i], 1) + "%";
                    //        //if (Propety.IsCheckArea)
                    //        //    content = rot._rect.Height + " px";
                    //        Font font = new Font("Arial", 50, FontStyle.Bold);
                    //        //SizeF sz1 = gc.MeasureString(content, font);

                    //DrawCharactersEvenly(gc, Propety.listContent, rot._rect, font, new SolidBrush(cl));


                    //   gc.DrawString(content, font, new SolidBrush(cl), new System.Drawing.Point(0, 0));
                    i++;
                    //gc.FillEllipse(Brushes.Black, -3, -3, 6, 6);
                    gc.ResetTransform();


                }
                //String s = (int)(Propety.Index + 1) + "." + G.PropetyTools[Propety.Index].Name;
                //SizeF sz = gc.MeasureString(s, new Font("Arial", 10, FontStyle.Bold));
                //gc.FillRectangle(Brushes.White, new Rectangle((int)rotA._rect.X, (int)rotA._rect.Y, (int)sz.Width, (int)sz.Height));
                //gc.DrawString(s, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new System.Drawing.Point((int)rotA._rect.X, (int)rotA._rect.Y));
                gc.ResetTransform();
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


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return gc;
        }
        //public Graphics ShowEdit(Graphics gc, RectangleF _rect)
        //{
        //    if (matTemp == null) return gc;

        //    if (G.TypeCrop != TypeCrop.Area)
        //        try
        //        {
        //            Mat matShow = matTemp.Clone();
        //            if (Propety.TypeMode == Mode.OutLine)
        //            {
        //                Bitmap bmTemp = matShow.ToBitmap();

        //                bmTemp.MakeTransparent(Color.Black);
        //                bmTemp = ConvertImg.ChangeToColor (bmTemp, Color.FromArgb(0, 255, 0), 1f);

        //                gc.DrawImage(bmTemp, _rect);
        //            }
        //            if (matMask != null)
        //            {
        //                Bitmap myBitmap2 = matMask.ToBitmap();
        //                myBitmap2.MakeTransparent(Color.Black);
        //                myBitmap2 = ConvertImg.ChangeToColor(myBitmap2, Color.OrangeRed, 1f);

        //                gc.DrawImage(myBitmap2, _rect);
        //            }

        //        }
        //        catch (Exception ex) { }
        //    return gc;
        //}

       
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

    
        
      
   
        Bitmap bmResult ;
        private void threadProcess_DoWork(object sender, DoWorkEventArgs e)
        {
          
        }
        public int indexTool = 0;
        private void threadProcess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
          
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
            Propety.TypeTool = TypeTool.OCR;
 
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

 

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void trackNumObject_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

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
            {
                worker.RunWorkerAsync();
                btnTest.Enabled= false;
            }
               
            else
                btnTest.IsCLick = false;
        }

        private void label12_Click(object sender, EventArgs e)
        {
                    }

        private void tableLayoutPanel15_Paint(object sender, PaintEventArgs e)
        {

        }
        //public void RefreshLabels()
        //{
        //    String[] labels = txtLabel.Text.Trim().Split(',');
        //    int index = 0;
        //    List<String> listLabel = new List<String>();
        //    foreach (String label in labels)
        //    {
        //        if (label == "") continue;
        //        listLabel.Add(label);

        //    }
        //    tabLbs.Controls.Clear();
        //    for (int row = 0; row < 4; row++)
        //    {
        //        for (int col = 0; col < 4; col++)
        //        {
        //            if (index >= listLabel.Count)
        //                break;
        //            Label lbl = new Label();
        //            lbl.Text = listLabel[index++];
        //            lbl.Font = new Font("Arial", 11);
        //            lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        //            lbl.BackColor = Color.FromArgb(200, 200, 200);
        //            lbl.Dock = DockStyle.Fill;
        //            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //            lbl.Margin = new Padding(3);
        //            // lbl.BorderStyle = BorderStyle.FixedSingle;

        //            tabLbs.Controls.Add(lbl, col, row);
        //        }
        //    }
        //    Propety.listLabel = listLabel;
        //}
        private void btnSetLabel_Click(object sender, EventArgs e)
        {
         //   RefreshLabels();


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
                    Propety.Check1(Propety.rotArea);
                    tmCheckFist.Enabled = false;
                }
                    
            }
        
        }

        private void rjButton3_Click_2(object sender, EventArgs e)
        {
            Propety.CompareArea = Compares.More;
        }

        private void rjButton7_Click(object sender, EventArgs e)
        {
            Propety.CompareArea = Compares.Less;
            //   Propety.
        }

    

        private void btnCropMask_Click(object sender, EventArgs e)
        {

        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (Propety.Content.Trim() == "")
                txtContent.Text = "No Data";
            else
            {
                Propety.Matching = Propety.Content;
                txtContent.Text = Propety.Matching;
            }
         
        }

        private void txtQRCODE_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void workLoadModel_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!IsIni)
            {
                Propety.SetModel();
               
                IsIni = true;
            }
                

          

        }

        private void workLoadModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (!IsIni)
            //{
            //    IsIni = true;
            //    tmCheckFist.Enabled = false;


            //}
        }

        private void numEnhance_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void numCLAHE_Load(object sender, EventArgs e)
        {
            
        }

        private void numUnsharp_Load(object sender, EventArgs e)
        {
            
        }

        private void numBlur_Load(object sender, EventArgs e)
        {
           
        }

        private void numCLAHE_ValueChanged(object sender, EventArgs e)
        {
            Propety.Clahe = (int)numCLAHE.Value;
        }

        private void numUnsharp_ValueChanged(object sender, EventArgs e)
        {
            Propety.Sigma = (int)numUnsharp.Value;

        }

        private void numBlur_ValueChanged(object sender, EventArgs e)
        {
            Propety.Blur = (int)numBlur.Value;

        }

        private void btnEnLimitArea_Click(object sender, EventArgs e)
        {
            Propety.IsEnLimitArea = btnEnLimitArea.IsCLick;
            layoutLineLimit.Enabled = Propety.IsEnLimitArea;
        }

        private void numLimtArea_Load(object sender, EventArgs e)
        {

        }

        private void numLimtArea_ValueChanged(object sender, EventArgs e)
        {
            Propety.LimitArea = numLimtArea.Value;
        }

        private void label7_Click_1(object sender, EventArgs e)
        {

        }

        private void txtContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtContent.Text = txtContent.Text.Replace("\n", "");
                txtContent.Text = txtContent.Text.Replace(" ", "");
                Propety.Matching=txtContent.Text;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Propety.sAllow = txtAllow.Text;
        }
    }
}

