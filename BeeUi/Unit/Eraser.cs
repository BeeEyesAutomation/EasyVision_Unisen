using BeeCore;
using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi.Commons
{
    [Serializable()]
    public partial class Eraser : UserControl
    {
        public Eraser()
        {
            InitializeComponent();
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            Global.EditTool.View.listMask = new List<OpenCvSharp.Mat>();
            Global.EditTool.View.listMask = new List<OpenCvSharp.Mat>();
            Global.EditTool.View.RefreshMask();
            Global.EditTool.View.imgView.Invalidate();
          
            Global.EditTool.View.toolEdit.btnClear.PerformClick();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Global.EditTool.View.listMask = new List<OpenCvSharp.Mat>();
            Global.EditTool.View.listMask = new List<OpenCvSharp.Mat>();
            Global.EditTool.View.RefreshMask();
            Global.EditTool.View.imgView.Invalidate();
        }

        private void trackClear_Scroll(object sender, EventArgs e)
        {
         
            Global.EditTool.View.widthClear = trackClear.Value;
            Global.EditTool.View.RefreshMask();
            Global.EditTool.View.imgView.Invalidate();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            trackClear.Value -= trackClear.SmallChange;
            Global.EditTool.View.widthClear = trackClear.Value;
            Global.EditTool.View.RefreshMask();

        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            trackClear.Value += trackClear.SmallChange;
            Global.EditTool.View.widthClear = trackClear.Value;
            Global.EditTool.View.RefreshMask();

        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (Global.EditTool.View.listRedo == null) Global.EditTool.View.listRedo = new List<Mat>();
            if (Global.EditTool.View.listMask == null) Global.EditTool.View.listMask = new List<Mat>();

            if (Global.EditTool.View.listMask.Count == 0) return;
                Global.EditTool.View.listRedo.Add(Global.EditTool.View.listMask[Global.EditTool.View.listMask.Count - 1].Clone());
            Global.EditTool.View.listMask.RemoveAt(Global.EditTool.View.listMask.Count - 1);
            Global.EditTool.View.RefreshMask();
            Global.EditTool.View.imgView.Invalidate(true);
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (Global.EditTool.View.listMask.Count == 0) return;
            Global.EditTool.View.listMask.Add(Global.EditTool.View.listRedo[Global.EditTool.View.listRedo.Count - 1]);
            Global.EditTool.View.listRedo.RemoveAt(Global.EditTool.View.listRedo.Count - 1);
            Global.EditTool.View.RefreshMask();
            Global.EditTool.View.imgView.Invalidate(true);
        }

        private void rjButton5_Click(object sender, EventArgs e)
        {
            Global.EditTool.View.toolEdit.btnClear.PerformClick() ;
        }

        private void Eraser_Load(object sender, EventArgs e)
        {
            
          
        }
    }
}
