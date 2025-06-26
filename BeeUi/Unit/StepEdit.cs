using BeeCore;
using BeeCore.Funtion;
using BeeUi.Commons;
using BeeUi.Data;
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
          //  if(G.Header!=null)
           // this.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);

            //   G.StepEdit = this;
        }
        public SettingStep2 SettingStep2 ;
        public SettingStep1 SettingStep1;
        private void btnStep2_Click(object sender, EventArgs e)
        {
            if (G.EditTool.View.btnLive.IsCLick)
            {
                G.EditTool.View.btnLive.PerformClick();
            }
            G.EditTool.View.btnLive.Enabled = false;
            G.EditTool.RefreshGuiEdit(Step.Step2);
           
        }

   

        
        bool IsLoaded = false;
        private void btnStep3_Click(object sender, EventArgs e)
        {
            if(G.EditTool.View.btnLive.IsCLick)
            {
                G.EditTool.View.btnLive.PerformClick();
            }    
            G.EditTool.View.btnLive.Enabled = false;
            G.EditTool.RefreshGuiEdit(Step.Step3);
          
        }

        private void btnStep1_Click(object sender, EventArgs e)
        {
            G.EditTool.View.btnLive.Enabled = true;
            G.EditTool.RefreshGuiEdit(Step.Step1);
            //SettingStep1.btnNextStep.Enabled = true;
            //if (SettingStep1 == null)
            //    SettingStep1 = new SettingStep1();


            //G.EditTool.pEditTool.Controls.Clear();
            //SettingStep1 = new SettingStep1();
            //SettingStep1.Parent = G.EditTool.pEditTool;
            //SettingStep1.Dock = DockStyle.Fill;
            //G.EditTool.lbNumStep.Text = "Step 1";
            //G.EditTool.lbNameStep.Text = "Image Optimization";
            //G.EditTool.iconTool.BackgroundImage = Properties.Resources._1;
            //G.EditTool.lbTool.Text = "Setup Camera";
            //if (!workConnect.IsBusy)
            //    workConnect.RunWorkerAsync();
        }

        private void workConnect_DoWork(object sender, DoWorkEventArgs e)
        {
           if( G.IsCCD ==true)
                if(BeeCore.Common.listCamera[G.indexChoose].matRaw==null)
                {
                    BeeCore.Common.listCamera[G.indexChoose].Read();
                   
                }
               
          
        }

        private void workConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (G.IsCCD)
            {

                Shows.RefreshImg(G.EditTool.View.imgView, BeeCore.Common.listCamera[G.indexChoose].matRaw);

               
            }
        
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
      public  SettingStep4 SettingStep4;
        private void btnStep4_Click(object sender, EventArgs e)
        {
            if (G.EditTool.View.btnLive.IsCLick)
            {
                G.EditTool.View.btnLive.PerformClick();
            }
            G.EditTool.View.btnLive.Enabled = false;
            G.EditTool.RefreshGuiEdit(Step.Step4);
         

        }

        private void btnSaveProgram_Click(object sender, EventArgs e)
        {
            SaveData.Project(G.Project,BeeCore.G.ParaCam);
            G.Header.Mode();
        }

        private void StepEdit_SizeChanged(object sender, EventArgs e)
        {
          BeeCore.CustomGui.RoundRg(this, G.Config.RoundRad);

        }
    }
}
