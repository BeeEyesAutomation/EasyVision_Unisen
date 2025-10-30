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

namespace BeeInterface
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
            if (Global.ParaCommon.matRegister != null)
            {
                if (!Global.ParaCommon.matRegister.IsDisposed())
                    Global.ParaCommon.SizeCCD = Global.ParaCommon.matRegister.Size;

                Global.Step = Step.Step3;
            }
            else
            {
                Global.StepEdit.btnStep2.IsCLick = true;
            }    
               
        }

        private void SettingStep2_Load(object sender, EventArgs e)
        {
            RegisterImg.LoadAllItem();

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
                Global.EditTool.View. pathRaw = fileDialog.FileName;
              BeeCore.Common.listCamera[Global.IndexChoose].matRaw = BeeCore.Common.LoadImage(Global.EditTool.View.pathRaw, ImreadModes.AnyColor);
               
              //  Cv2.CvtColor(BeeCore.Common.listCamera[Global.IndexChoose].matRaw, BeeCore.Common.listCamera[Global.IndexChoose].matRaw, ColorConversionCodes.GRAY2BGR);
                Global.ParaCommon.matRegister = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
                Global.ParaCommon.SizeCCD =new System.Drawing.Size( BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size().Width, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size().Height);
                Global.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
                Global.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
                Global.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
                Global.EditTool.View.matResgiter = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone();
                btnNextStep.Enabled = true;
                Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                //  btnNextStep.BackgroundImage = Properties.Resources.btnChoose1;
                ShowTool.Full(Global.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());

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
           
         


        }

        private void workRead_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BeeCore.Common.listCamera[Global.IndexChoose].Read();
            if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw != null)
                if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.IsDisposed)
                    if (!BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty())
                        Global.ParaCommon.matRegister = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone().ToBitmap(); ;
            if (Global.ParaCommon.matRegister == null)
                return;
            //Global.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
            //Global.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
            //Global.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Rows, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Cols, MatType.CV_8UC1);
            //Global.EditTool.View.matResgiter = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone();
            btnNextStep.Enabled = true;
            Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
            Global.ParaCommon.SizeCCD =new System.Drawing.Size( BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size().Width, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size().Height);
          if (Global.ParaCommon.matRegister.IsDisposed())
            { MessageBox.Show("Fail"); }    

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Global.EditTool.RefreshGuiEdit(Step.Step1);
        }

        private void tmNotPress_Tick(object sender, EventArgs e)
        {
           // btnCapCamera.IsCLick =!btnCapCamera.IsCLick;
           // btnNextStep.Enabled = true;
            tmNotPress.Enabled = false;
        }

        private void RegisterImg_SelectedItemChanged(object sender, RegisterImgSelectionChangedEventArgs e)
        {
            using (Mat clone = e.Image?.Clone())
            {if(clone==null)
                {
                    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = new Mat();
                    Global.ParaCommon.matRegister = null;
                  //  Global.EditTool.View.imgView.Image.Dispose();
                    BeginInvoke(new Action(() =>
                    {
                        var old = Global.EditTool.View.imgView.Image;
                        Global.EditTool.View.imgView.Image = null;
                        old?.Dispose();
                    }));
                    return;
                }    
                // phần Global của bạn — giữ nguyên
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = clone.Clone();
                Global.ParaCommon.matRegister = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
                Global.ParaCommon.SizeCCD = new System.Drawing.Size(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size().Width, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size().Height);
                Global.EditTool.View.matResgiter = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone();
                Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                ShowTool.Full(Global.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
                btnNextStep.Enabled = true;
            }

           // Text = "Đang chọn: " + e.Name;
        }

        private void RegisterImg_ItemsChanged(object sender, EventArgs e)
        {

        }
    }
}
