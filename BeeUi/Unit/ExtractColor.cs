using BeeCore;
using BeeGlobal;
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
    public partial class ExtractColor : UserControl
    {
        ColorArea ColorArea;
        public ExtractColor(ColorArea colorArea)
        {
            InitializeComponent();
            this.ColorArea = colorArea;
        }
        private void btnHide_Click(object sender, EventArgs e)
        {
           
            G.EditTool.View.imgView.Invalidate();

            G.EditTool.View.toolEdit.btnGetColor.PerformClick();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            G.EditTool.View.toolEdit.Propety.ClearTemp(Global.IsRun);
            G.EditTool.View.imgView.Invalidate();
        }

        private void trackClear_Scroll(object sender, EventArgs e)
        {
            ColorArea.AreaPixel = trackClear.Value;
            G.EditTool.View.toolEdit.GetTemp(G.EditTool.View.toolEdit.Propety.rotArea);


            G.EditTool.View.imgView.Invalidate();
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            trackClear.Value -= trackClear.SmallChange;
            ColorArea.AreaPixel = trackClear.Value;
            G.EditTool.View.imgView.Invalidate();
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            trackClear.Value += trackClear.SmallChange;
            ColorArea.AreaPixel = trackClear.Value;
            G.EditTool.View.imgView.Invalidate();
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
          G.EditTool.View.toolEdit.Propety.Undo(BeeCore.Common.listCamera[Global.IndexChoose].matRaw);
            G.EditTool.View.RefreshMask();
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            
            G.EditTool.View.imgView.Invalidate(true);
        }

        private void rjButton5_Click(object sender, EventArgs e)
        {
            G.EditTool.View.toolEdit.btnGetColor.PerformClick();
        }

        private void ExtractColor_Load(object sender, EventArgs e)
        {

        }
    }
}
