using BeeCore;
using BeeGlobal;
using BeeUi.Commons;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi
{
    public partial class FormChoose : Form
    {

        Access access = new Access();
        public FormChoose()
        {
            InitializeComponent();
          
          
        }

     
    

        private void ForrmAlarm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
        }

     

        private void btnYes_Click(object sender, EventArgs e)
        {
            Global.Step = Step.Step1;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormChoose_Shown(object sender, EventArgs e)
        {
            btnYes.Focus();
        }
    }
}
