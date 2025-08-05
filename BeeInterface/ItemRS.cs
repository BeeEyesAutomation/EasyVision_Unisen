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

namespace BeeInterface
{
    public partial class ItemRS : UserControl
    {
        public ItemRS()
        {
            InitializeComponent();
        }
       public PropetyTool propetyTool;
        private void ckUnused_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.UsedTool = UsedTool.NotUsed;
        }

        private void ckUsed_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.UsedTool = UsedTool.Used;
        }

        private void ckInverse_CheckedChanged(object sender, EventArgs e)
        {
            propetyTool.UsedTool = UsedTool.Invertse;
        }
    }
}
