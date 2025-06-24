using BeeCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi.Unit
{
    public partial class ItemLogic : UserControl
    {
        public ItemLogic()
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
