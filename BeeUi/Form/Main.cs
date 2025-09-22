using BeeCore;
using BeeGlobal;
using BeeUi.Commons;
using BeeUi.Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi
{
    public partial class Main : Form
    {
        public Main()
        {
            //SetStyle(ControlStyles.AllPaintingInWmPaint |
            //     ControlStyles.UserPaint |
            //     ControlStyles.OptimizedDoubleBuffer, true);
            //typeof(Form).GetProperty("DoubleBuffered",
            //    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            //    ?.SetValue(this, true, null);
            InitializeComponent();
            G.Main = this;
            Global.EditTool = this.editTool1;
            //G.Header = editTool1.header1;
            this.MinimumSize = new Size(1190, 780);
        }
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        var cp = base.CreateParams;
        //        // WS_EX_COMPOSITED: vẽ từ dưới lên, giảm flicker mạnh cho Form nhiều child
        //        cp.ExStyle |= 0x02000000;
        //        return cp;
        //    }
        //}
        //protected override void OnHandleCreated(EventArgs e)
        //{
        //    base.OnHandleCreated(e);
        //    EnableDoubleBufferRecursive(this);
        //}
        //private static void EnableDoubleBufferRecursive(Control root)
        //{
        //    foreach (Control c in root.Controls)
        //    {
        //        var pi = c.GetType().GetProperty("DoubleBuffered",
        //            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        //        pi?.SetValue(c, true, null);
        //        EnableDoubleBufferRecursive(c);
        //    }
        //}
        //protected override async void OnShown(EventArgs e)
        //{
        //    base.OnShown(e);
        //    // Cho form hiển thị xong đã
        //    await System.Threading.Tasks.Task.Yield();
          
        //}
        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.ParaCommon.Comunication.Protocol.IO_Processing = IO_Processing.Reset;
            Task.Delay(1000);
            //if(G.DeviceConnectForm!=null)
            //G.DeviceConnectForm.FormClosingDo(sender, e);
            editTool1.DesTroy();
            if (G.AddTool!=null)
            G.AddTool.Close();
            G.Load.Close();
            Global.ScanCCD.Close();
            if(G.FormActive!=null)
            G.FormActive.Close();
            if (G.IsShutDown)
                Process.Start("shutdown", "/s /t 3"); // t=0 để tắt ngay sau khi gọi lệnh
            Application.Exit();
            Process.GetCurrentProcess().Kill(); // Tắt tiến trình hiện tại (hơi "mạnh tay")
          
            if (G.IsReConnectCCD)
                Application.Restart();
        }
      
        private void Main_Load(object sender, EventArgs e)
        {
            //this.editTool1 = new BeeUi.EditTool();
            //// editTool1
            //// 
            //this.editTool1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.editTool1.Location = new System.Drawing.Point(0, 0);
            //this.editTool1.Name = "editTool1";
            //this.editTool1.Size = new System.Drawing.Size(1058, 764);
            //this.editTool1.TabIndex = 0;
            // this.Controls.Add(this.editTool1);
            if (Properties.Settings.Default.szForm !=null)
                if (Properties.Settings.Default.szForm.Width>720)
       this.Size = Properties.Settings.Default.szForm ;
        
        }

        private void button2_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                this.editTool1.Focus();
            }    
        }

        private void button2_Leave(object sender, EventArgs e)
        {

        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {

        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            //{Width = 1190 Height = 777}
          
                Properties.Settings.Default.szForm = this.Size;
                Properties.Settings.Default.Save();
          
        }
        String barcode;
        DateTime lastTime;bool f1;
        private void Main_KeyPress(object sender, KeyPressEventArgs e)
        {
            barcode = string.Empty;

            try
            {
                //barcode += e.KeyChar;

                //if (lastTime > new DateTime())
                //{
                //    if (DateTime.Now.Subtract(lastTime).Milliseconds > 30)
                //    {
                //        f1 = false;
                //    }
                //    else
                //    {
                //        f1 = true;
                //    }
                //}

                //lastTime = DateTime.Now;
                //if(e.KeyChar=='\n')
                //Global.EditTool.View.txtQRcode.Text = barcode;
                /*

                Test your Barcode, and if it matches your criteria then change your TextBox text

                TextBox1.Text = barcode;

                */

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong");
            }
        }
    }
}
