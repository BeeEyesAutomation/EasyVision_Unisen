using BeeCore;
using BeeCore.Algorithm;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeGlobal;
using BeeInterface;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
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
using static System.Net.Mime.MediaTypeNames;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolBarcode : UserControl
    {
        
        public ToolBarcode( )
        {
            InitializeComponent();
        
        }
        
        public void LoadPara()
        {

          
            try
            {
                EditRectRot1.Rot = new List<RectRotate> { Propety.rotArea, Propety.rotCrop };
                EditRectRot1.Refresh();
                EditRectRot1.RotateCurentChanged -= EditRectRot1_RotateCurentChanged;
                EditRectRot1.RotateCurentChanged += EditRectRot1_RotateCurentChanged;
                EditRectRot1.IsHide = false;
                this.VisibleChanged += ToolBarcode_VisibleChanged;
                btnOnSendResult.IsCLick = Propety.IsSendResult;
                btnOffSendResult.IsCLick =! Propety.IsSendResult;
                txtAdd.Text = Propety.AddPLC;
               
                trackScore.Min = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].MinValue;
                trackScore.Max = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].MaxValue;
                trackScore.Step = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StepValue;
                trackScore.Value = Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Score;

                Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StatusTool = StatusTool.WaitCheck;
                Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StatusToolChanged += ToolWidth_StatusToolChanged;
                Common.PropetyTools[Global.IndexProgChoose][Propety.Index].ScoreChanged += ToolWidth_ScoreChanged;
              
                
               
                
                Global.TypeCrop = TypeCrop.Crop;
                Propety.TypeCrop = Global.TypeCrop;
                imgTemp.Image = Propety.bmRaw;
             
                btnModeSingle.IsCLick=Propety.ModeCheck==ModeCheck.Single ? true : false;
                btnModeMulti.IsCLick = Propety.ModeCheck == ModeCheck.Multi ? true : false;
                AdjIndexProgChoose.Value = Propety.IndexProgChoose + 1;
                AdjOffSetArea.IsInital = true;
                AdjOffSetArea.Value = Propety.OffSetArea;
                AdjIndexProgChoose.Enabled= Propety.ModeCheck == ModeCheck.Single ? true : false;
            }
            catch (Exception ex)
            {
                String s = ex.Message;
            }
        }

        private void ToolBarcode_VisibleChanged(object sender, EventArgs e)
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

        private void ToolWidth_ScoreChanged(float obj)
        {
           trackScore.Value = obj;
        }
        float ScaleW = 0;
        private void ToolWidth_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {if (Global.IsRun) return;
            btnScan.Enabled = true;
            if (Common.PropetyTools[Global.IndexProgChoose][Propety.Index].StatusTool == StatusTool.Done)
                {if(Propety.IsScan)
                    {
                        AdjIndexProgChoose.Value = Propety.IndexProgChoose+1;
                        btnChoose.IsCLick = true;
                        Propety.TypeCrop = TypeCrop.Crop;
                        imgTemp.Image = Propety.bmRaw;
                    int withBox = imgTemp.Width;
                    int heightBox = imgTemp.Height;
                     ScaleW = (float)(withBox * 1.0 / Propety.bmRaw.Width);
                    imgTemp.Zoom =(int)( ScaleW * 100);
                    imgTemp.AutoScrollPosition = new System.Drawing.Point(0, 0);
                    layTemp.Height = Propety.bmRaw.Height *( imgTemp.Zoom/100)+150;
                }    
               
               // Global.EditTool.View.listChoose = Propety.listRotScan;
               
             
                btnTest.Enabled = true;
                
               
            }
           
        }

        private void trackScore_ValueChanged(float obj)
        {
            Common.PropetyTools[Global.IndexProgChoose][Propety.Index].Score=trackScore.Value;
         }
        public bool IsClear = false;
        public Barcode Propety { get; set; }




        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {

        }

      
       
        private void btnCannyMin_Click(object sender, EventArgs e)
        {
        

        }

        private void btnCannyMedium_Click(object sender, EventArgs e)
        {
         
        }

        private void btnCannyMax_Click(object sender, EventArgs e)
        {
        

        }

    
        
      
    
   
        private void btnTest_Click(object sender, EventArgs e)
        {
            btnChoose.IsCLick = false;
            
            Global.StatusDraw = StatusDraw.Edit;
            Propety.TypeCrop = TypeCrop.Area;
            Global.TypeCrop= TypeCrop.Area;

            btnTest.Enabled = false;
            Common.PropetyTools[Global.IndexProgChoose][Global.IndexToolSelected].RunToolAsync();

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
            Propety.rotArea = new RectRotate(new RectangleF(-Global.Config.SizeCCD.Width / 2, -Global.Config.SizeCCD.Height / 2, Global.Config.SizeCCD.Width, Global.Config.SizeCCD.Height), new PointF(Global.Config.SizeCCD.Width / 2, Global.Config.SizeCCD.Height / 2), 0, AnchorPoint.None);

            
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            Global.StatusDraw = StatusDraw.Check;
           
        }

     
        

    

       

      
   
        private void btnScanOCR_Click(object sender, EventArgs e)
        {
            btnChoose.IsCLick = false;
            Propety.IsScan = true;
         Propety.Scan();
           
        }

        private void rjButton2_Click(object sender, EventArgs e)
        {if(btnChoose.IsCLick)
            {
                Global.TypeCrop = TypeCrop.Crop;
                Global.StatusDraw = StatusDraw.Scan;
            }
            else
            {
                Global.StatusDraw = StatusDraw.Edit;
                Global.TypeCrop = TypeCrop.Area;
            }    
               
        }

        private void AdjOffSetArea_ValueChanged(float obj)
        {
            Propety.OffSetArea =(int) AdjOffSetArea.Value;
            Propety.UpdateOffSet();
        }

        private void AdjIndexProgChoose_ValueChanged(float obj)
        {
            Propety.IndexProgChoose =(int) AdjIndexProgChoose.Value - 1;
        }

        private void rjButton2_Click_1(object sender, EventArgs e)
        {
            Propety.ModeCheck = ModeCheck.Multi;
            AdjIndexProgChoose.Enabled = Propety.ModeCheck == ModeCheck.Single ? true : false;
        }

        private void btnModeSingle_Click(object sender, EventArgs e)
        {
            Propety.ModeCheck= ModeCheck.Single;
            AdjIndexProgChoose.Enabled = Propety.ModeCheck == ModeCheck.Single ? true : false;
        }

        private void txtAdd_TextChanged(object sender, EventArgs e)
        {
            Propety.AddPLC= txtAdd.Text.Trim();
        }

        private void btnOnSendResult_Click(object sender, EventArgs e)
        {
            Propety.IsSendResult = true;
        }

        private void btnOffSendResult_Click(object sender, EventArgs e)
        {
            Propety.IsSendResult = false;
        }
    }
}
