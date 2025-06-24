using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi.Tool
{
    public partial class AddTool : Form
    {
        public AddTool()
        {
            InitializeComponent();
            G.AddTool = this;
        }

        private void AddTool_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void AddTool_Load(object sender, EventArgs e)
        {
            this.Location = new Point(G.Main.Location.X + G.Main.Width / 2 - this.Width / 2, G.Main.Location.Y + G.Main.Height / 2 - this.Height / 2);
        }

        private void toolPage1_Load(object sender, EventArgs e)
        {

        }
    }
}
