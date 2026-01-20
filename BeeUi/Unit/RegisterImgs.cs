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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace BeeUi.Unit
{
    public partial class RegisterImgs : UserControl
    {
        public RegisterImgs()
        {
            InitializeComponent();
        }
        public void LoadData()
        {
            registerImg.LoadAllItem(Global.listRegsImg, BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].IndexImgRegis);

        }
        private void RegisterImgs_Load(object sender, EventArgs e)
        {
          
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
         
          
            Global.EditTool.View.btnChangeImg.Enabled = true;
         
        }

        private void registerImg_SelectedItemChanged(object sender, BeeInterface.RegisterImgSelectionChangedEventArgs e)
        {
            using (Mat clone = e.Image?.Clone())
            {
                // phần Global của bạn — giữ nguyên
                BeeCore.Common.listCamera[Global.IndexCCCD].matRaw = clone.Clone();
                //switch(Global.IndexChoose)
                //{
                //    case 0:
                //        Global.ParaCommon.matRegister = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                //        break;
                //    case 1:
                //        Global.ParaCommon.matRegister2 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                //        break;
                //    case 2:
                //        Global.ParaCommon.matRegister3 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                //        break;
                //    case 3:
                //        Global.ParaCommon.matRegister4 = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone());
                //        break;
                //}    
               
                Global.Config.SizeCCD = new System.Drawing.Size(BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size().Width, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size().Height);
               // Global.EditTool.View.matResgiter = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Clone();
                Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.ToBitmap();
                ShowTool.Full(Global.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexCCCD].matRaw.Size());
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].IndexImgRegis = registerImg.IndexSelected;
            }

        }

        private void RegisterImgs_VisibleChanged(object sender, EventArgs e)
        {
        }
    }
}
