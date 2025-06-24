using BeeCore;
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
            G.EditTool.View.listMask = new List<OpenCvSharp.Mat>();
            G.EditTool.View.listMask = new List<OpenCvSharp.Mat>();
            G.EditTool.View.RefreshMask();
            G.EditTool.View.imgView.Invalidate();
          
            G.EditTool.View.toolEdit.btnClear.PerformClick();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            G.EditTool.View.listMask = new List<OpenCvSharp.Mat>();
            G.EditTool.View.listMask = new List<OpenCvSharp.Mat>();
            G.EditTool.View.RefreshMask();
            G.EditTool.View.imgView.Invalidate();
        }

        private void trackClear_Scroll(object sender, EventArgs e)
        {
         
            G.EditTool.View.widthClear = trackClear.Value;
            G.EditTool.View.RefreshMask();
            G.EditTool.View.imgView.Invalidate();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            trackClear.Value -= trackClear.SmallChange;
            G.EditTool.View.widthClear = trackClear.Value;
            G.EditTool.View.RefreshMask();

        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            trackClear.Value += trackClear.SmallChange;
            G.EditTool.View.widthClear = trackClear.Value;
            G.EditTool.View.RefreshMask();

        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (G.EditTool.View.listRedo == null) G.EditTool.View.listRedo = new List<Mat>();
            if (G.EditTool.View.listMask == null) G.EditTool.View.listMask = new List<Mat>();

            if (G.EditTool.View.listMask.Count == 0) return;
                G.EditTool.View.listRedo.Add(G.EditTool.View.listMask[G.EditTool.View.listMask.Count - 1].Clone());
            G.EditTool.View.listMask.RemoveAt(G.EditTool.View.listMask.Count - 1);
            G.EditTool.View.RefreshMask();
            G.EditTool.View.imgView.Invalidate(true);
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (G.EditTool.View.listMask.Count == 0) return;
            G.EditTool.View.listMask.Add(G.EditTool.View.listRedo[G.EditTool.View.listRedo.Count - 1]);
            G.EditTool.View.listRedo.RemoveAt(G.EditTool.View.listRedo.Count - 1);
            G.EditTool.View.RefreshMask();
            G.EditTool.View.imgView.Invalidate(true);
        }

        private void rjButton5_Click(object sender, EventArgs e)
        {
            G.EditTool.View.toolEdit.btnClear.PerformClick() ;
        }

        private void Eraser_Load(object sender, EventArgs e)
        {
            
          
        }
    }
}
