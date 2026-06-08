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
            SyncSelectedCamera();
            Bitmap registerBitmap = GetSelectedRegisterBitmap();
            if (registerBitmap != null)
            {
                Global.Config.SizeCCD = registerBitmap.Size;
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
            SyncSelectedCamera();
            IsLoad = true;
            if (Global.listRegsImg == null)
            {
                Global.listRegsImg = new List<ItemRegsImg>();
                Bitmap registerBitmap = GetSelectedRegisterBitmap();
                if (registerBitmap != null)
                    Global.listRegsImg.Add(new ItemRegsImg("IMAGE", registerBitmap));
            }
            if (Global.listRegsImg.Count == 0)
            {
                Global.listRegsImg = new List<ItemRegsImg>();
                Bitmap registerBitmap = GetSelectedRegisterBitmap();
                if (registerBitmap != null)
                    Global.listRegsImg.Add(new ItemRegsImg("IMAGE", registerBitmap));
            }
            RegisterImg.LoadAllItem(Global.listRegsImg, Global.ParaCommon.ListIndexMatReg[Global.IndexProgChoose]);
          

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
            SyncSelectedCamera();
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

                        SetSelectedRegisterBitmap(BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone()));
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
            SyncSelectedCamera();
            Global.Comunication.Protocol.IsOnLight = true;
            Global.Comunication.Protocol.IO_Processing = IO_Processing.None;
            Global.Comunication.Protocol.IO_Processing = IO_Processing.Light;

            if (!workRead.IsBusy)
               workRead.RunWorkerAsync();
            tmNotPress.Enabled = true;


        }

        private void workRead_DoWork(object sender, DoWorkEventArgs e)
        {
           
         


        }

        private void workRead_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SyncSelectedCamera();
            Camera camera = GetConnectedCamera("ReadMaster");
            if (camera == null)
                return;

            try
            {
                camera.Read();
            }
            catch (Exception ex)
            {
                Global.LogError("ReadMaster", "Read camera failed. Program=" + Global.IndexProgChoose + ", Camera=" + Global.IndexCCCD, ex);
                return;
            }
          
            if (camera.matRaw != null)
            {


                if (!camera.matRaw.IsDisposed)
                {
                    if (!camera.matRaw.Empty())
                    {
                        SetSelectedRegisterBitmap(BitmapConverter.ToBitmap(camera.matRaw.Clone()));
                    }
                    else
                    {
                        Global.LogError("ReadMaster", "Read image is empty. Program=" + Global.IndexProgChoose + ", Camera=" + Global.IndexCCCD);
                        return;
                    }
                }
                else
                {
                    Global.LogError("ReadMaster", "Read image is disposed. Program=" + Global.IndexProgChoose + ", Camera=" + Global.IndexCCCD);
                    return;
                }
            }
            else
            {
                Global.LogError("ReadMaster", "Read image is null. Program=" + Global.IndexProgChoose + ", Camera=" + Global.IndexCCCD);
                return;
            }

            
            btnNextStep.Enabled = true;
            Global.EditTool.View.imgView.Image = camera.matRaw.ToBitmap();
            Global.Config.SizeCCD =new System.Drawing.Size( camera.matRaw.Size().Width, camera.matRaw.Size().Height);
          Bitmap registerBitmap = GetSelectedRegisterBitmap();
          if (registerBitmap == null || registerBitmap.IsDisposed())
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
            SyncSelectedCamera();
            if (IsLoad)
            {
                IsLoad = false;
                return;
            }
            Global.ParaCommon.ListIndexMatReg[Global.IndexProgChoose]=e.IndexChange;
            using (Mat clone = e.Image?.Clone())
            {if(clone==null)
                {
                    BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = new Mat();
                    SetSelectedRegisterBitmap(null);
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
                SetSelectedRegisterBitmap(BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone()));
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

        private void RegisterImg_Load(object sender, EventArgs e)
        {

        }

        private static void SyncSelectedCamera()
        {
            if (Global.Config != null && Global.Config.IsMultiProg)
                Global.SelectProgram(Global.IndexProgChoose);

            EnsureCameraSlot(Global.IndexCCCD);
        }

        private static void EnsureCameraSlot(int index)
        {
            if (Global.listParaCamera == null)
                Global.listParaCamera = new List<ParaCamera>();

            while (Global.listParaCamera.Count <= index)
                Global.listParaCamera.Add(null);

            if (BeeCore.Common.listCamera == null)
                BeeCore.Common.listCamera = new List<Camera>();

            while (BeeCore.Common.listCamera.Count <= index)
                BeeCore.Common.listCamera.Add(null);

            if (Global.listParaCamera[index] == null)
                Global.listParaCamera[index] = new ParaCamera();

            if (BeeCore.Common.listCamera[index] == null)
                BeeCore.Common.listCamera[index] = new Camera(Global.listParaCamera[index], index);
        }

        private static Camera GetConnectedCamera(string source)
        {
            if (BeeCore.Common.listCamera == null || Global.IndexCCCD < 0 || Global.IndexCCCD >= BeeCore.Common.listCamera.Count)
            {
                Global.LogError(source, "Camera slot is missing. Program=" + Global.IndexProgChoose + ", Camera=" + Global.IndexCCCD);
                return null;
            }

            Camera camera = BeeCore.Common.listCamera[Global.IndexCCCD];
            if (camera == null)
            {
                Global.LogError(source, "Camera is null. Program=" + Global.IndexProgChoose + ", Camera=" + Global.IndexCCCD);
                return null;
            }

            if (!camera.IsConnected)
            {
                Global.LogError(source, "Camera is not connected. Program=" + Global.IndexProgChoose + ", Camera=" + Global.IndexCCCD);
                return null;
            }

            return camera;
        }

        private static Bitmap GetSelectedRegisterBitmap()
        {
            switch (Global.IndexProgChoose)
            {
                case 1:
                    return Global.ParaCommon.matRegister2;
                case 2:
                    return Global.ParaCommon.matRegister3;
                case 3:
                    return Global.ParaCommon.matRegister4;
                default:
                    return Global.ParaCommon.matRegister;
            }
        }

        private static void SetSelectedRegisterBitmap(Bitmap bitmap)
        {
            switch (Global.IndexProgChoose)
            {
                case 1:
                    Global.ParaCommon.matRegister2 = bitmap;
                    break;
                case 2:
                    Global.ParaCommon.matRegister3 = bitmap;
                    break;
                case 3:
                    Global.ParaCommon.matRegister4 = bitmap;
                    break;
                default:
                    Global.ParaCommon.matRegister = bitmap;
                    break;
            }
        }
    }
}
