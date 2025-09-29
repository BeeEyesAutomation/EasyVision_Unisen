using BeeCore;
using BeeCpp;
using BeeGlobal;
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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolColorArea : UserControl
    {
     
        public TypeTool TypeTool;
        public ToolColorArea( )
        {
            InitializeComponent();
        }
        public int indexTool;
        
        public BeeCore. ColorArea Propety=new BeeCore.ColorArea();
      
       
      
        public bool IsClear;
        public bool IsIni = false;
        public void LoadPara( )
        {
         
            if (Propety.listCLShow==null)
                Propety.listCLShow = new List<Color>();
            trackScore.Min = Common.PropetyTools[Global.IndexChoose][Propety.Index].MinValue;
            trackScore.Max = Common.PropetyTools[Global.IndexChoose][Propety.Index].MaxValue;
            trackScore.Step = Common.PropetyTools[Global.IndexChoose][Propety.Index].StepValue;
            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
            AdjValueTemp.Value = Propety.PxTemp;

            AdjClose.Value = Propety.SizeClose;
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
            AdjClose.Enabled = Propety.IsClose;
            Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusToolChanged += ToolColorArea_StatusToolChanged;
            Common.PropetyTools[Global.IndexChoose][Propety.Index].ScoreChanged += ToolColorArea_ScoreChanged;
        
            trackPixel.Value = (int)Propety.Extraction;
            switch(Propety.TypeColor)
            {
                case ColorGp.RGB:
                    btnRGB.IsCLick = true;
                    break;
                case ColorGp.HSV:
                    btnHSV.IsCLick = true;
                    break;
            }
          
              
            btnGetColor.IsCLick = Propety.IsGetColor;

            trackScore.Value = Common.PropetyTools[Global.IndexChoose][Propety.Index].Score;
           
        }

        private void ToolColorArea_ScoreChanged(float obj)
        {
           trackScore.Value =(float)obj;
        }

        private void ToolColorArea_StatusToolChanged(StatusTool obj)
        {

            if (Global.IsRun) return;
            if (Common.PropetyTools[Global.IndexChoose][Propety.Index].StatusTool == StatusTool.Done)
            {
                if (Propety.IsCalib)
                {
                    btnCalib.IsCLick = false;
                    btnCalib.Enabled = true;
                    Propety.IsCalib = false;
                    Propety.PxTemp = Propety.pxRS;
                    AdjValueTemp.Value = Propety.PxTemp;
                }
            }

            btnInspect.Enabled = true;
        }

        public Mat matCrop=new Mat();
        public void GetTemp()
        {
          
        
            
            picColor.Invalidate();
            Propety.SetColor();
           
        }
        

        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {
           
           
            btnGetColor.IsCLick = false;
            Propety.IsGetColor = btnGetColor.IsCLick;
            Global.StatusDraw = StatusDraw.Check;
        }
        
      
     
      
       
    
       

      

     

      
      
    
       
       

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnCropRect_Click_1(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            if (Propety.rotAreaTemp != null)
                Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.Edit;


        }

        private void rjButton1_Click(object sender, EventArgs e)
        {

        }

        bool IsFullSize;
        private void btnCropArea_Click_1(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Width / 2, -BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Height / 2, BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Width, BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Height), new PointF(BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Width / 2, BeeCore.Common.listCamera[Global. IndexChoose].matRaw.Height / 2), 0, AnchorPoint.None, false);

           
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            Global.StatusDraw = StatusDraw.Edit;

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
          //  G.IsCancel = true;
           
          //  G.EditTool.RefreshGuiEdit(Step.Step3);
            btnGetColor.IsCLick = false;
            Propety.IsGetColor = btnGetColor.IsCLick;
        }

      
        private void btnClBlack_Click(object sender, EventArgs e)
        {
           
            btnDeleteAll.PerformClick();
        }

        private void trackScore_ValueChanged(float obj)
        {
            Common.PropetyTools[Global.IndexChoose][Propety.Index].Score = (int)trackScore.Value;
            
          

        }
       
      

        private void picColor_Paint(object sender, PaintEventArgs e)
        {
            int x = 0;int h = picColor.Height;int w = h;
            foreach (Color cl in Propety.listCLShow)
            {

                e.Graphics.FillRectangle(new SolidBrush( cl), new RectangleF(x, 0, w, h));
                e.Graphics.DrawRectangle(new Pen(Color.Black,1), new Rectangle(x, 0, w, h));
                x += w ;
            }
        }

        private void trackPixel_Validating(object sender, CancelEventArgs e)
        {

        }

        private void trackPixel_ValueChanged(float obj)
        {
            Propety.Extraction = (int)trackPixel.Value;

            if(!IsIni)
            {
                IsIni = true;
                return;
            }    
             Propety.SetColor();
           
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {if (Propety.listCLShow.Count == 0) return;
            Propety.listCLShow.RemoveAt(Propety.listCLShow.Count - 1);
            Propety.Undo();
            picColor.Invalidate();
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {

            
           
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            Propety.listCLShow = new List<Color>();
            Propety.ClearTemp();
          //  G.EditTool.View.ClearTemp(Propety);
            picColor.Invalidate();
        }

        private void pMode_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnMask_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Mask;
            Propety.TypeCrop = Global.TypeCrop;
            if (Propety.rotMask == null)
            {
                Propety.rotMask = DataTool.NewRotRect(TypeCrop.Mask);
            }
            btnElip.IsCLick = Propety.rotMask.IsElip;
            btnRect.IsCLick = !Propety.rotMask.IsElip;
        }

        private void btnCropHalt_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.Edit;

            if (IsClear)
                btnMask.PerformClick();
        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.ParaCommon.SizeCCD.Width / 2, -Global.ParaCommon.SizeCCD.Height / 2, Global.ParaCommon.SizeCCD.Width, Global.ParaCommon.SizeCCD.Height), new PointF(Global.ParaCommon.SizeCCD.Width / 2, Global.ParaCommon.SizeCCD.Height / 2), 0, AnchorPoint.None, false);


            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            if (IsClear)
                btnMask.PerformClick();
            Global.StatusDraw = StatusDraw.Edit;
        }

        private void btnCropRect_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Crop;
            Propety.TypeCrop = Global.TypeCrop;
            btnElip.IsCLick = Propety.rotCrop.IsElip;
            btnRect.IsCLick = !Propety.rotCrop.IsElip;

        }

        private void btnCropArea_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            btnElip.IsCLick = Propety.rotArea.IsElip;
            btnRect.IsCLick = !Propety.rotArea.IsElip;
        }

        private void btnRect_Click(object sender, EventArgs e)
        {
            if (Propety.rotMask == null)
            {
                Propety.rotMask = DataTool.NewRotRect(TypeCrop.Mask); ;
            }
            switch (Global.TypeCrop)
            {
                case TypeCrop.Crop:
                    Propety.rotCrop.IsElip = btnElip.IsCLick;
                    break;
                case TypeCrop.Area:
                    Propety.rotArea.IsElip = btnElip.IsCLick;
                    break;
                case TypeCrop.Mask:
                    Propety.rotMask.IsElip = btnElip.IsCLick;
                    break;

            }
        }

        private void btnElip_Click(object sender, EventArgs e)
        {
            if (Propety.rotMask == null)
            {
                Propety.rotMask = DataTool.NewRotRect(TypeCrop.Mask); ;
            }

            switch (Global.TypeCrop)
            {
                case TypeCrop.Crop:
                    Propety.rotCrop.IsElip = btnElip.IsCLick;
                    break;
                case TypeCrop.Area:
                    Propety.rotArea.IsElip = btnElip.IsCLick;
                    break;
                case TypeCrop.Mask:
                    Propety.rotMask.IsElip = btnElip.IsCLick;
                    break;

            }
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            switch (Global.TypeCrop)
            {
                //case TypeCrop.Crop:
                //    Propety.rotCrop.IsElip = btnElip.IsCLick;
                //    break;
                //case TypeCrop.Area:
                //    Propety.rotArea.IsElip = btnElip.IsCLick;
                //    break;
                case TypeCrop.Mask:
                    Propety.rotMask = null;// = btnElip.IsCLick;
                    break;

            }
        }

        private void btnHSV_Click(object sender, EventArgs e)
        {
            Propety.TypeColor = ColorGp.HSV;
            btnDeleteAll.PerformClick();
        }

      

        private void btnInspect_Click(object sender, EventArgs e)
        {
            if (!Common.PropetyTools[Global.IndexChoose][Propety.Index].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Propety.Index].worker.RunWorkerAsync();
            else
                btnInspect.IsCLick = false;
        }

        private void btnRGB_Click(object sender, EventArgs e)
        {
         Propety .TypeColor = ColorGp.RGB;

            btnDeleteAll.PerformClick();
        }

        private void btnGetColor_Click(object sender, EventArgs e)
        {
            Propety.IsGetColor = btnGetColor.IsCLick;
            if (Propety.IsGetColor)
            {

              
                Global.StatusDraw = StatusDraw.Color;
               
            }
            else
                Global.StatusDraw = StatusDraw.Edit;
        }

      
        private void AdjClearNoise_ValueChanged(float obj)
        {
            Propety.SizeClearsmall = (int)AdjClearNoise.Value;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Propety.IsClose = btnClose.IsCLick;
            AdjClose.Enabled = Propety.IsClose;
        }

        private void btnEnableNoise_Click(object sender, EventArgs e)
        {

            Propety.IsClearNoiseSmall = btnIsClearSmall.IsCLick;
            AdjClearNoise.Enabled = Propety.IsClearNoiseSmall;
        }

        private void AdjClose_ValueChanged(float obj)
        {

            Propety.SizeClose = (int)AdjClose.Value;
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

        private void btnCalib_Click(object sender, EventArgs e)
        {
            btnCalib.Enabled = false;
            Propety.IsCalib = true;
            if (!Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.IsBusy)
                Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].worker.RunWorkerAsync();
            else
                btnCalib.IsCLick = false;
        }

        private void AdjValueTemp_ValueChanged(float obj)
        {
            Propety.PxTemp = (int)AdjValueTemp.Value;
        }
    }
}
