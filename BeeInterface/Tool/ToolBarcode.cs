using BeeCore;
using BeeCore.Algorithm;
using BeeCore.Func;
using BeeCore.Funtion;
using BeeCore.Funtion.Engines;
using BeeGlobal;
using BeeInterface;
using BeeInterface.Tool._Base;
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
    public partial class ToolBarcode : ToolViewBase
    {
        private Barcode _propety;

        public new Barcode Propety
        {
            get { return _propety; }
            set
            {
                _propety = value;
                base.Propety = value;
                InvalidateOwnerToolCache();
            }
        }

        public ToolBarcode( )
        {
            InitializeComponent();
            HostLegacyControlsInBaseTabs();
            if (Propety == null)
                Propety = new Barcode();
            RequestDynamicTabsForKind("Barcode");
        }
        
        public override void LoadPara()
        {

          
            try
            {
                BarcodeViewState viewState = BarcodeEngineRunner.ReadFromOwner(OwnerTool, Propety);
                EditRectRot1.Rot = new List<RectRotate> { Propety.rotArea, Propety.rotCrop };
                EditRectRot1.Refresh();
                EditRectRot1.RotateCurentChanged -= EditRectRot1_RotateCurentChanged;
                EditRectRot1.RotateCurentChanged += EditRectRot1_RotateCurentChanged;
                EditRectRot1.IsHide = false;
                this.VisibleChanged -= ToolBarcode_VisibleChanged;
                this.VisibleChanged += ToolBarcode_VisibleChanged;
                btnOnSendResult.IsCLick = viewState.IsSendResult;
                btnOffSendResult.IsCLick = !viewState.IsSendResult;
                txtAdd.Text = viewState.AddPLC;
               
                trackScore.Min = viewState.ScoreMin;
                trackScore.Max = viewState.ScoreMax;
                trackScore.Step = viewState.ScoreStep;
                trackScore.Value = viewState.Score;

                BarcodeEngineRunner.MarkOwnerWaiting(OwnerTool);
                 if (OwnerTool != null)
                 {
                     OwnerTool.StatusToolChanged -= ToolWidth_StatusToolChanged;
                     OwnerTool.StatusToolChanged += ToolWidth_StatusToolChanged;
                 }
                 if (OwnerTool != null)
                 {
                     OwnerTool.ScoreChanged -= ToolWidth_ScoreChanged;
                     OwnerTool.ScoreChanged += ToolWidth_ScoreChanged;
                 }
              
                
               
                
                Global.TypeCrop = TypeCrop.Crop;
                Propety.TypeCrop = Global.TypeCrop;
                imgTemp.Image = Propety.bmRaw;
             
                btnModeSingle.IsCLick = viewState.ModeCheck == ModeCheck.Single ? true : false;
                btnModeMulti.IsCLick = viewState.ModeCheck == ModeCheck.Multi ? true : false;
                AdjIndexProgChoose.Value = viewState.IndexProgChoose + 1;
                AdjOffSetArea.IsInital = true;
                AdjOffSetArea.Value = viewState.OffSetArea;
                AdjIndexProgChoose.Enabled = viewState.ModeCheck == ModeCheck.Single ? true : false;
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
            if (OwnerTool.StatusTool == StatusTool.Done)
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
            BarcodeEngineRunner.ApplyScoreToOwner(OwnerTool, trackScore.Value);
         }
        public bool IsClear = false;




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
            BarcodeEngineRunner.TryRunSelectedTool();

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
            BarcodeEngineRunner.Scan(Propety);
           
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

        private void HostLegacyControlsInBaseTabs()
        {
            if (tabRoot == null)
                return;

            if (tabControl2 != null)
            {
                tabRoot.Font = tabControl2.Font;
                Controls.Remove(tabControl2);
            }

            Controls.Remove(pInspect);
            Controls.Remove(oK_Cancel1);

            AddControlsToBaseTab(tabRoi, label3, EditRectRot1, tableLayoutPanel7);
            AddControlsToBaseTab(tabParams, label5, tableLayoutPanel8, tableLayoutPanel9, label2, AdjOffSetArea);
            AddControlsToBaseTab(tabResult, label8, trackScore, label9, layTemp);
            AddControlsToBaseTab(tabGeneral, tableLayoutPanel10, pInspect, oK_Cancel1);
        }

        private static void AddControlsToBaseTab(TabPage tabPage, params Control[] controls)
        {
            TableLayoutPanel panel = new TableLayoutPanel
            {
                AutoScroll = true,
                BackColor = SystemColors.Control,
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(1, 0, 1, 0),
                RowCount = controls.Length
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            for (int i = 0; i < controls.Length; i++)
            {
                Control control = controls[i];
                if (control == null)
                    continue;

                control.Dock = DockStyle.Fill;
                panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                panel.Controls.Add(control, 0, i);
            }

            tabPage.Controls.Clear();
            tabPage.Controls.Add(panel);
        }
    }
}
