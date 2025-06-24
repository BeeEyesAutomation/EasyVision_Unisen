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
    public partial class FormReport : Form
    {
        public FormReport()
        {
            InitializeComponent();
            G.ucReport = new Tool.ucReport();
            G.ucReport.Parent = this;
          
            //this.Size = new Size(G.ucReport.Width + 20, G.ucReport.Height + 20);

        }

        private void FormReport_Load(object sender, EventArgs e)
        {
            G.ucReport.Dock = DockStyle.Fill;
            this.Size = new Size( G.ucReport.Width+30, G.ucReport.Height +30);

        }

        private void FormReport_FormClosing(object sender, FormClosingEventArgs e)
        {
         //   if(G.cnn.State==ConnectionState.Open)
          //  G.cnn.Close();
        }
    }
}
