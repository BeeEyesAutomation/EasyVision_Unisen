using BeeCore;
using BeeGlobal;
using BeeInterface;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
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

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolCrop : UserControl
    {

        #region OwnerTool cache (Phase 2 refactor)
        private PropetyTool _ownerTool;
        private PropetyTool OwnerTool
        {
            get
            {
                if (_ownerTool == null)
                    _ownerTool = Common.TryGetTool(Global.IndexProgChoose, Propety.Index);
                return _ownerTool;
            }
        }
        private void InvalidateOwnerToolCache() => _ownerTool = null;
        #endregion
        public ToolCrop( )
        {
            InitializeComponent();

        }


        public void LoadPara()
        {


            txtFolder.Text = Propety.PathSaveImage;

            OwnerTool.StatusTool = StatusTool.WaitCheck;

             if (OwnerTool != null)

             {

                 OwnerTool.StatusToolChanged -= ToolPattern_StatusToolChanged;

                 OwnerTool.StatusToolChanged += ToolPattern_StatusToolChanged;

             }
        }

        private void ToolPattern_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {
            if (Global.IsRun) return;
            if (OwnerTool.StatusTool == StatusTool.Done)
            {
                btnTest.Enabled = true;
                imgTemp.Image = Propety.matProcess.ToBitmap();
            }
        }



        public Crop Propety { get; set; }







        private void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;
            if (!OwnerTool.worker.IsBusy)
                OwnerTool.worker.RunWorkerAsync();
            else
                btnTest.IsCLick = false;
        }
        bool IsFullSize = false;
        public bool IsClear;
        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.Config.SizeCCD.Width / 2, -Global.Config.SizeCCD.Height / 2, Global.Config.SizeCCD.Width, Global.Config.SizeCCD.Height), new PointF(Global.Config.SizeCCD.Width / 2, Global.Config.SizeCCD.Height / 2), 0, AnchorPoint.None);


            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            Global.StatusDraw = StatusDraw.Check;

        }

        private void btnCropHalt_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.Check;
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowser.ShowDialog() == DialogResult.OK) {
                Propety.PathSaveImage = folderBrowser.SelectedPath;
                txtFolder.Text = Propety.PathSaveImage;
            }

        }
    }
}
