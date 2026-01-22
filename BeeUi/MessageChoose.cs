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
    public partial class MessageChoose : Form
    {

        Access access = new Access();
        public MessageChoose(String Header, String Content)
        {
            InitializeComponent();
            lbHeader.Text = Header;
            lbContent.Text = Content;

        }




        private void ForrmAlarm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
        }

         public  bool IsOK = false;

        private void btnYes_Click(object sender, EventArgs e)
        {
            IsOK = true;
            this.Close();
          
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            IsOK = false;
            this.Close();
        }

        private void FormChoose_Shown(object sender, EventArgs e)
        {
            btnYes.Focus();
        }
    }
}
