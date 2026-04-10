using BeeCore;
using BeeGlobal;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
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
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolOCR : UserControl
    {

        public ToolOCR(bool IsNew = false)
        {
            InitializeComponent();
            this.IsNew = IsNew;
            if (Propety == null)
                Propety = new OCR();

        }
        bool IsNew = false;
        bool IsIni = false;
        
        Stopwatch timer = new Stopwatch();
        public void RefreshLabels()
        {
            //if (Propety.listLabelResult == null)
            //    return;
            //int index = 0;
            //tabLabelResult.Height = 0;
            //tabLabelResult.Controls.Clear();
            //for (int row = 0; row < 4; row++)
            //{
            //    for (int col = 0; col < 2; col++)
            //    {

            //        if (index >= Propety.listLabelResult.Count)
            //            break;
            //        if (col == 0)
            //            tabLabelResult.Height += 40;
            //        RJButton btn = new RJButton();
            //        btn.Text = Propety.listLabelResult[index];
            //        btn.Font = new Font("Arial", 11);
            //        btn.IsUnGroup = true;
            //        btn.ForeColor = Color.Black;
            //        btn.BorderRadius = 10;
            //        btn.Height = 30;
            //        if (Propety.listScore[index] >Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Score)
            //            btn.IsCLick = true;
            //        else
            //            btn.IsCLick = false;


            //        btn.Corner = Corner.Both;
            //        btn.BackColor = Color.FromArgb(200, 200, 200);
            //        btn.Dock = DockStyle.Fill;
            //        btn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //        btn.Margin = new Padding(3);

            //        tabLabelResult.Controls.Add(btn, col, row);
            //        index++;
            //    }

            //}

        }

        public void LoadPara()
        {
            EditRectRot1.Rot = new List<RectRotate> { Propety.rotArea, Propety.rotMask };
            EditRectRot1.IsHide = false;
            EditRectRot1.Refresh();
            EditRectRot1.RotateCurentChanged -= EditRectRot1_RotateCurentChanged;
            EditRectRot1.RotateCurentChanged += EditRectRot1_RotateCurentChanged;
        
            this.VisibleChanged -= ToolOCR_VisibleChanged;
            this.VisibleChanged += ToolOCR_VisibleChanged;
          
            Global.TypeCrop = TypeCrop.Area;
            txtContent.Text = Propety.Matching;

            trackScore.Min = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].MinValue;
            trackScore.Max = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].MaxValue;
            trackScore.Step = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StepValue;
            trackScore.Value = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Score;

            Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
            Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StatusToolChanged += ToolOCR_StatusToolChanged;
            Common.PropetyTools[Global.IndexProgChoose][Propety.Index].ScoreChanged += ToolOCR_ScoreChanged;
            AdjLimitArea.Value = Propety.LimitArea;
        
            numCLAHE.Value = Propety.Clahe;
            numUnsharp.Value = Propety.Sigma;
            numBlur.Value = Propety.Blur;
            Propety.rotMask = null;
          
            if (Propety.sAllow != null)
                txtAllow.Text = Propety.sAllow;
            btn8.IsCLick =! Propety.IsCompareNoFixed;
           lay8.Visible = !btn8.IsCLick;
            txtAddressPLC.Text = Propety.AddPLC;
            
            if (IsNew)
            {
                Common.PropetyTools[Global.IndexProgChoose][Propety.Index].ItemTool.Enabled = false;
                IsNew = false;
                if (!workLoadModel.IsBusy)
                    workLoadModel.RunWorkerAsync();
            }

            lay4.Enabled = !Propety.IsCompareNoFixed;

        }

        private void ToolOCR_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                EditRectRot1.IsHide = true;
                EditRectRot1.RotateCurentChanged -= EditRectRot1_RotateCurentChanged;
            }
        }

        private void EditRectRot1_RotateCurentChanged(RectRotate obj)
        {
            switch (obj.TypeCrop)
            {
                case TypeCrop.Area:
                    Propety.rotArea = obj; break;
                case TypeCrop.Crop:
                    Propety.rotCrop = obj; break;
                case TypeCrop.Mask:
                    Propety.rotMask = obj; break;

            }
        }

        private void ToolOCR_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {
            if (Global.IsRun) return;
            if (Propety.Index >= Common.PropetyTools[Global.IndexProgChoose].Count)
                return;
            if (Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StatusTool == StatusTool.Done)
            {
               
                btnTest.Enabled = true;
            }
        }

        private void ToolOCR_ScoreChanged(float obj)
        {
            trackScore.Value = obj;
        }

        private void trackScore_ValueChanged(float obj)
        {

            Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Score = (int)trackScore.Value;

        }

        public OCR Propety { get; set; }
        public Mat matTemp = new Mat();
        Mat matClear = new Mat(); Mat matMask = new Mat();

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
    

        private void rjButton3_Click(object sender, EventArgs e)
        {


            //  cv3.Pattern();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {

        }

        private void btnCropArea_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;


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
            //    Propety.IsAreaWhite = false;
            //   GetTemp(Propety.rotCrop,BeeCore.Common.listCamera[Global. IndexProgChoose].matRaw );

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
            //   Propety.IsAreaWhite = true;
            //  GetTemp(Propety.rotCrop, BeeCore.Common.listCamera[Global. IndexProgChoose].matRaw);

        }

        private void btnOK_Click(object sender, EventArgs e)
        {


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



        private void numScore_ValueChanged(object sender, EventArgs e)
        {

        }

        private void rjButton5_Click(object sender, EventArgs e)
        {

        }

        private void btnLearning_Click(object sender, EventArgs e)
        {

        }

        private void rjButton3_Click_1(object sender, EventArgs e)
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
            Global.StatusDraw = StatusDraw.Check;
        }

        private void btnCropFull_Click_1(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.Config.SizeCCD.Width / 2, -Global.Config.SizeCCD.Height / 2, Global.Config.SizeCCD.Width, Global.Config.SizeCCD.Height), new PointF(Global.Config.SizeCCD.Width / 2, Global.Config.SizeCCD.Height / 2), 0, AnchorPoint.None);


            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            Global.StatusDraw = StatusDraw.Check;
        }

        private void btnCropRect_Click_1(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Crop;
            Propety.TypeCrop = Global.TypeCrop;



        }

        private void btnTest_Click_1(object sender, EventArgs e)
        {

            btnTest.Enabled = false;
            Common.PropetyTools[Global.IndexProgChoose][Global.IndexToolSelected].RunToolAsync();

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
            //if (BeeCore.Common.listCamera[Global.IndexProgChoose].IsConnected)
            //{
            //    BeeCore.Common.listCamera[Global.IndexProgChoose].Read();
            //    if (BeeCore.Common.listCamera[Global.IndexProgChoose].IsConnected)
            //    {
            //        //Propety.Check1(Propety.rotArea);
            //        tmCheckFist.Enabled = false;
            //    }

            //}

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
                Propety.SetModelOCR();
                
                IsIni = true;
            }




        }

        private void workLoadModel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Common.PropetyTools[Global.IndexProgChoose][Propety.Index].ItemTool.Enabled = true;
          //  Common.PropetyTools[Global.IndexProgChoose][Propety.Index].ItemTool.Name = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Name;
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

        private void numCLAHE_ValueChanged(float obj)
        {
            Propety.Clahe = (int)numCLAHE.Value;
        }

        private void numUnsharp_ValueChanged(float obj)
        {
            Propety.Sigma = (int)numUnsharp.Value;

        }

        private void numBlur_ValueChanged(float obj)
        {
            Propety.Blur = (int)numBlur.Value;

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
                Propety.Matching = txtContent.Text;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Propety.sAllow = txtAllow.Text;
        }

        

        private void btn3_Click(object sender, EventArgs e)
        {
            lay3.Visible = !btn3.IsCLick;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            lay4.Visible = !btn4.IsCLick;
            lay4.Enabled = !Propety.IsCompareNoFixed;
         //   Propety.IsCompareNoFixed = !btn4.IsCLick;
        }

    

    
    

     

     

        private void AdjLimitArea_ValueChanged(float obj)
        {
            Propety.LimitArea =(int) AdjLimitArea.Value;
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            if (Global.Comunication.Protocol.IsConnected)
            {
                lay8.Visible = !btn8.IsCLick;
                Propety.IsCompareNoFixed = !btn8.IsCLick;
                lay4.Enabled = !Propety.IsCompareNoFixed;
            }    
         
        }

        private void AddressPLC_TextChanged(object sender, EventArgs e)
        {
            Propety.AddPLC=txtAddressPLC.Text;
        }

        private void btnValuePLC_Click(object sender, EventArgs e)
        {
            if (Global.Comunication.Protocol.IsConnected&& Propety.AddPLC!="")
            {
                Propety.Matching = Global.Comunication.Protocol.PlcClient.ReadStringAsciiKey(Propety.AddPLC,16).Trim().ToString();
                txtContent.Text=Propety.Matching;
                btnValuePLC.Text=Propety.Matching;
            }    
           

        }

        private void btn6_Click(object sender, EventArgs e)
        {
            AdjLimitArea.Visible = !btn6.IsCLick;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            EditRectRot1.Visible = !btn2.IsCLick;
        }
    }
}

