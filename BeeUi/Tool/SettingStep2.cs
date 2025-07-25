﻿using BeeCore;
using BeeCore.Funtion;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi.Tool
{
    [Serializable()]
    public partial class SettingStep2 : UserControl
    {
        public SettingStep2()
        {
            InitializeComponent();
        }

        private void btnNextStep_Click(object sender, EventArgs e)
        {
            //if (File.Exists("Program\\" + Global.Project + ".para"))
            //    File.Delete("Program\\" + Global.Project + ".para");
            //Access.SaveParaCam("Program\\" + Global.Project + ".para", Global.ParaCommon);

            G.StepEdit.SettingStep2.Parent.Controls.Remove(G.StepEdit.SettingStep2);
            G.StepEdit.btnStep3.PerformClick();
        }

        private void SettingStep2_Load(object sender, EventArgs e)
        {
          

        }
        public void SaveParaPJ()
        {
          
        }
       
        private void btnLoadImge_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                G.EditTool.View. pathRaw = fileDialog.FileName;
              BeeCore.Common.listCamera[Global.IndexChoose].matRaw = BeeCore.Common.LoadImage(G.EditTool.View.pathRaw, ImreadModes.AnyColor);
               
              //  Cv2.CvtColor(BeeCore.Common.listCamera[Global.IndexChoose].matRaw, BeeCore.Common.listCamera[Global.IndexChoose].matRaw, ColorConversionCodes.GRAY2BGR);
                Global.ParaCommon.matRegister = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexChoose].matRaw);
                Global.ParaCommon.SizeCCD =new System.Drawing.Size( BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size().Width, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size().Height);
                G.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
                G.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
                G.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
                G.EditTool.View.matResgiter = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone();
                btnNextStep.Enabled = true;
                G.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                //  btnNextStep.BackgroundImage = Properties.Resources.btnChoose1;
                Shows.Full(G.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());

                SaveParaPJ();

            }
        }

        private void btnCapCamera_Click(object sender, EventArgs e)
        {
            if (!workRead.IsBusy)
               workRead.RunWorkerAsync();
            tmNotPress.Enabled = true;


        }

        private void workRead_DoWork(object sender, DoWorkEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Read();
         


        }

        private void workRead_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
          //  BeeCore.Common.listCamera[Global.IndexChoose].matRaw = BeeCore.Native.GetImg();
            if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw != null)
                if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty())
                    Global.ParaCommon.matRegister = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone().ToBitmap(); ;
            if (Global.ParaCommon.matRegister == null)
                return;
            G.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
            G.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
            G.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
            G.EditTool.View.matResgiter = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone();
            btnNextStep.Enabled = true;
            G.EditTool.View.imgView.Image = Global.ParaCommon.matRegister;
            Global.ParaCommon.SizeCCD = Global.ParaCommon.matRegister.Size;
          //  btnNextStep.BackgroundImage = Properties.Resources.btnChoose1;

            //if (!G.Header.workSaveProject.IsBusy)
            //    G.Header.workSaveProject.RunWorkerAsync();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            G.Header.btnMode.PerformClick();
        }

        private void tmNotPress_Tick(object sender, EventArgs e)
        {
            btnCapCamera.IsCLick =!btnCapCamera.IsCLick;
           // btnNextStep.Enabled = true;
            tmNotPress.Enabled = false;
        }
    }
}
