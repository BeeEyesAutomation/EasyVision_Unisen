using BeeCore;
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
using System.Runtime.CompilerServices;
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
                switch (Global.IndexChoose)
                {
                    case 0:
                        Global.Config.SizeCCD = Global.ParaCommon.matRegister.Size;// = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                        break;
                    case 1:
                        Global.Config.SizeCCD = Global.ParaCommon.matRegister2.Size;// Global.ParaCommon.matRegister2 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                        break;
                    case 2:
                        Global.Config.SizeCCD = Global.ParaCommon.matRegister3.Size;//  Global.ParaCommon.matRegister3 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                        break;
                    case 3:
                        Global.Config.SizeCCD = Global.ParaCommon.matRegister4.Size;//   Global.ParaCommon.matRegister4 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                        break;
                }
                //if (!Global.ParaCommon.matRegister.IsDisposed())
                //    Global.Config.SizeCCD = Global.ParaCommon.matRegister.Size;

                Global.Step = Step.Step3;
            }
            else
            {
                Global.StepEdit.btnStep2.IsCLick = true;
            }    
               
        }
        bool IsLoad = false;
        public void LoadImg()
        {
            IsLoad = true;
            if (Global.listRegsImg == null)
            {
                Global.listRegsImg = new List<ItemRegsImg>();
                if (Global.ParaCommon.matRegister != null)
                    Global.listRegsImg.Add(new ItemRegsImg("IMAGE", Global.ParaCommon.matRegister));
            }
            if (Global.listRegsImg.Count == 0)
            {
                Global.listRegsImg = new List<ItemRegsImg>();
                if (Global.ParaCommon.matRegister != null)
                    Global.listRegsImg.Add(new ItemRegsImg("IMAGE", Global.ParaCommon.matRegister));
            }
            RegisterImg.LoadAllItem(Global.listRegsImg);

        }
        private void SettingStep2_Load(object sender, EventArgs e)
        {
           // LoadImg();

           // RegisterImg.IndexSelected = 1;
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
              BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = BeeCore.Common.LoadImage(Global.EditTool.View.pathRaw, ImreadModes.AnyColor);
                Cv2.CvtColor(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw, ColorConversionCodes.GRAY2BGR);
                if (!BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.IsDisposed)
                {
                    if (!BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Empty())
                    {

                        switch (Global.IndexChoose)
                        {
                            case 0:
                                Global.ParaCommon.matRegister = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                                break;
                            case 1:
                                Global.ParaCommon.matRegister2 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                                break;
                            case 2:
                                Global.ParaCommon.matRegister3 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                                break;
                            case 3:
                                Global.ParaCommon.matRegister4 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                                break;
                        }
                    }
                    else
                        return;
                }
                else
                    return;

                 Global.Config.SizeCCD =new System.Drawing.Size( BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size().Width, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size().Height);
              
                btnNextStep.Enabled = true;
                Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                //  btnNextStep.BackgroundImage = Properties.Resources.btnChoose1;
                ShowTool.Full(Global.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size());

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
          
            BeeCore.Common.listCamera[Global.IndexCCCD].Read();
          
            if (BeeCore.Common.listCamera[Global.IndexCCCD].matRaw != null)
            {


                if (!BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.IsDisposed)
                {
                    if (!BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Empty())
                    {
                        switch (Global.IndexChoose)
                        {
                            case 0:
                                Global.ParaCommon.matRegister = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                                break;
                            case 1:
                                Global.ParaCommon.matRegister2 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                                break;
                            case 2:
                                Global.ParaCommon.matRegister3 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                                break;
                            case 3:
                                Global.ParaCommon.matRegister4 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                                break;
                        }
                    }
                    else
                        return;
                }
                else
                    
                    return;
            }
            else
                { return; }

            
                btnNextStep.Enabled = true;
            Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
            Global.Config.SizeCCD =new System.Drawing.Size( BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size().Width, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size().Height);
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
            if (IsLoad)
            {
                IsLoad = false;
                return;
            }    
              
            using (Mat clone = e.Image?.Clone())
            {if(clone==null)
                {
                    BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = new Mat();
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
                BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = clone.Clone();
                switch (Global.IndexChoose)
                {
                    case 0:
                        Global.ParaCommon.matRegister = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                        break;
                    case 1:
                        Global.ParaCommon.matRegister2 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                        break;
                    case 2:
                        Global.ParaCommon.matRegister3 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                        break;
                    case 3:
                        Global.ParaCommon.matRegister4 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                        break;
                }
             //   Global.ParaCommon.matRegister = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                Global.Config.SizeCCD = new System.Drawing.Size(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size().Width, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size().Height);
             //   Global.EditTool.View.matResgiter = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone();
                Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                ShowTool.Full(Global.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size());
                btnNextStep.Enabled = true;
            }

           // Text = "Đang chọn: " + e.Name;
        }

        private void RegisterImg_ItemsChanged(object sender, EventArgs e)
        {

        }
    }
}
