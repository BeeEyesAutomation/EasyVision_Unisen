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

        private void RegisterImgs_Load(object sender, EventArgs e)
        {
            this.Location = new Point( Global.EditTool.View.Width - this.Width,Global.EditTool.View.pBtn.Height + 1);

            registerImg.LoadAllItem((BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].IndexImgRegis));
          
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
          
            Global.EditTool.View.btnChangeImg.Enabled = true;
         
        }

        private void registerImg_SelectedItemChanged(object sender, BeeInterface.RegisterImgSelectionChangedEventArgs e)
        {
            using (Mat clone = e.Image?.Clone())
            {
                // phần Global của bạn — giữ nguyên
                BeeCore.Common.listCamera[Global.IndexChoose].matRaw = clone.Clone();
                Global.ParaCommon.matRegister = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());
                Global.ParaCommon.SizeCCD = new System.Drawing.Size(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size().Width, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size().Height);
                Global.EditTool.View.matResgiter = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone();
                Global.EditTool.View.imgView.Image = BeeCore.Common.listCamera[Global.IndexChoose].matRaw.ToBitmap();
                ShowTool.Full(Global.EditTool.View.imgView, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Size());
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].IndexImgRegis = registerImg.IndexSelected;
            }

        }

        private void RegisterImgs_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible)
            {
                this.Location = new Point(Global.EditTool.View.Width - this.Width, Global.EditTool.View.pBtn.Height + 1);

                registerImg.LoadAllItem((BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].IndexImgRegis));

            }
        }
    }
}
