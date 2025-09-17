using BeeCore;
using BeeCore.Funtion;
using BeeGlobal;
using BeeInterface;
using BeeUi.Tool;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace BeeUi.Common
{
    [Serializable()]
    public partial class StepEdit : UserControl
    {

        public StepEdit()
        {
            InitializeComponent();
            //this.DoubleBuffered = true;
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.UserPaint, true);
            this.AutoScaleMode = AutoScaleMode.Dpi; // hoặc AutoScaleMode.Font
            // this.Visible = false;
            G.StepEdit = this;


        }
        public void ButtonRadius(Control control, int radius)
        {
        }
        public void Load1()
        {
          
           
        }
        
        private void StepEdit_Load(object sender, EventArgs e)
        {
            //if (this.Width< 859)
            //{
            //    btnStep1.Width=
            //}
            //  if(G.Header!=null)
            // this.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar,Global.Config.colorGui);

            //   G.StepEdit = this;
        }
        public SettingStep2 SettingStep2 = new SettingStep2();
        public SettingStep1 SettingStep1=new SettingStep1();
        private void btnStep2_Click(object sender, EventArgs e)
        {
         
            Global.EditTool.RefreshGuiEdit(Step.Step2);
           
        }

   

        
        bool IsLoaded = false;
        private void btnStep3_Click(object sender, EventArgs e)
        {
          
            if(!Global.ParaCommon.matRegister.IsDisposed())
            Global.ParaCommon.SizeCCD = Global.ParaCommon.matRegister.Size;
           
            Global.EditTool.RefreshGuiEdit(Step.Step3);
          
        }

        private void btnStep1_Click(object sender, EventArgs e)
        {
            Global.EditTool.View.btnLive.Enabled = true;
            Global.EditTool.RefreshGuiEdit(Step.Step1);
            //SettingStep1.btnNextStep.Enabled = true;
            //if (SettingStep1 == null)
            //    SettingStep1 = new SettingStep1();


            //Global.EditTool.pEditTool.Controls.Clear();
            //SettingStep1 = new SettingStep1();
            //SettingStep1.Parent = Global.EditTool.pEditTool;
            //SettingStep1.Dock = DockStyle.Fill;
            //Global.EditTool.lbNumStep.Text = "Step 1";
            //Global.EditTool.lbNameStep.Text = "Image Optimization";
            //Global.EditTool.iconTool.BackgroundImage = Properties.Resources._1;
            //Global.EditTool.lbTool.Text = "Setup Camera";
            //if (!workConnect.IsBusy)
            //    workConnect.RunWorkerAsync();
        }

      

    
        private void StepEdit_VisibleChanged(object sender, EventArgs e)
        { 
        //{if (G.Header == null) return;
        //    if(this.Visible==true&&!G.IsRun)
        //    {
        //        btnStep1.PerformClick();
        //    }
        //    else
        //    {
        //        if (G.Header != null)
        //            G.Header.workLoadProgram.RunWorkerAsync();
        //    }
        }
      public  SettingStep4 SettingStep4=new SettingStep4();
        private void btnStep4_Click(object sender, EventArgs e)
        {
            
            Global.EditTool.RefreshGuiEdit(Step.Step4);
         

        }

        private void btnSaveProgram_Click(object sender, EventArgs e)
        {
            SaveData.Project(Global.Project);
            G.Header.Mode();
        }

        private void StepEdit_SizeChanged(object sender, EventArgs e)
        {
          //BeeCore.CustomGui.RoundRg(this,Global.Config.RoundRad);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           // SaveData.Project(Global.Project);
            Global.EditTool.RefreshGuiEdit(Step.Run);
            Global.ParaCommon.Comunication.IO.IO_Processing = IO_Processing.ChangeMode;

        }
    }
}
