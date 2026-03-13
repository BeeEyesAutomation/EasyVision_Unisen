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
    public partial class AddTool : Form
    {
        public AddTool()
        {
            InitializeComponent();
            
        }

        private void AddTool_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void AddTool_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Global.SizeScreen.Width / 2 - this.Width / 2, 0 + Global.SizeScreen.Height / 2 - this.Height / 2);
        }

        private void toolPage1_Load(object sender, EventArgs e)
        {

        }
    }
}
