using BeeCore;
using BeeCore.Funtion;
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
            //if (File.Exists("Program\\" + G.Project + ".para"))
            //    File.Delete("Program\\" + G.Project + ".para");
            //Access.SaveParaCam("Program\\" + G.Project + ".para", BeeCore.G.ParaCam);

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
              BeeCore.Common.listCamera[G.indexChoose].matRaw = BeeCore.Common.LoadImage(G.EditTool.View.pathRaw, ImreadModes.AnyColor);
               
              //  Cv2.CvtColor(BeeCore.Common.listCamera[G.indexChoose].matRaw, BeeCore.Common.listCamera[G.indexChoose].matRaw, ColorConversionCodes.GRAY2BGR);
                BeeCore.G.ParaCam.matRegister = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[G.indexChoose].matRaw);
                BeeCore.G.ParaCam.SizeCCD =new System.Drawing.Size( BeeCore.Common.listCamera[G.indexChoose].matRaw.Size().Width, BeeCore.Common.listCamera[G.indexChoose].matRaw.Size().Height);
                G.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[G.indexChoose].matRaw.Rows, BeeCore.Common.listCamera[G.indexChoose].matRaw.Cols, MatType.CV_8UC1);
                G.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[G.indexChoose].matRaw.Rows, BeeCore.Common.listCamera[G.indexChoose].matRaw.Cols, MatType.CV_8UC1);
                G.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[G.indexChoose].matRaw.Rows, BeeCore.Common.listCamera[G.indexChoose].matRaw.Cols, MatType.CV_8UC1);
                G.EditTool.View.matResgiter = BeeCore.Common.listCamera[G.indexChoose].matRaw.Clone();
                btnNextStep.Enabled = true;
                G.EditTool.View.imgView.Image = BeeCore.Common.listCamera[G.indexChoose].matRaw.ToBitmap();
                //  btnNextStep.BackgroundImage = Properties.Resources.btnChoose1;
                Shows.Full(G.EditTool.View.imgView, BeeCore.Common.listCamera[G.indexChoose].matRaw.Size());

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
            BeeCore.Common.listCamera[G.indexChoose].Read();
         


        }

        private void workRead_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
          //  BeeCore.Common.listCamera[G.indexChoose].matRaw = BeeCore.Native.GetImg();
            if (BeeCore.Common.listCamera[G.indexChoose].matRaw != null)
                if (!BeeCore.Common.listCamera[G.indexChoose].matRaw.Empty())
                    BeeCore.G.ParaCam.matRegister = BeeCore.Common.listCamera[G.indexChoose].matRaw.Clone().ToBitmap(); ;
            if (BeeCore.G.ParaCam.matRegister == null)
                return;
            G.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[G.indexChoose].matRaw.Rows, BeeCore.Common.listCamera[G.indexChoose].matRaw.Cols, MatType.CV_8UC1);
            G.EditTool.View.bmMask = new Mat(BeeCore.Common.listCamera[G.indexChoose].matRaw.Rows, BeeCore.Common.listCamera[G.indexChoose].matRaw.Cols, MatType.CV_8UC1);
            G.EditTool.View.matMaskAdd = new Mat(BeeCore.Common.listCamera[G.indexChoose].matRaw.Rows, BeeCore.Common.listCamera[G.indexChoose].matRaw.Cols, MatType.CV_8UC1);
            G.EditTool.View.matResgiter = BeeCore.Common.listCamera[G.indexChoose].matRaw.Clone();
            btnNextStep.Enabled = true;
            G.EditTool.View.imgView.Image = BeeCore.G.ParaCam.matRegister;
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
