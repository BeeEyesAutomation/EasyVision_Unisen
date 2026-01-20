using BeeCore;
using BeeGlobal;

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
{
    public partial class FormWarning : Form
    {

        Access access = new Access();
        public FormWarning( String Err,String Content)
        {
            InitializeComponent();
          lbErr.Text = Err;
            lbContent.Text = Content;
          
        }

     
    

        private void ForrmAlarm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
        }

        private  void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();



        }

    
    }
}
