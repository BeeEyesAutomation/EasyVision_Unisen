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

namespace BeeUi
{
    public partial class QuickSetting : Form
    {
        public QuickSetting()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void QuickSetting_Load(object sender, EventArgs e)
        {
            this.Width = Global.EditTool.BtnHeaderBar.Width + 1;
          //  this.Height = Global.SizeScreen.Height - Global.EditTool.BtnHeaderBar.Height - 20;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - Global.EditTool.BtnHeaderBar.Width - 1, Global.EditTool.pTop.Height);// Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);

        }
    }
}
