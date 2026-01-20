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
    public partial class ForrmAlarm : Form
    {

        Access access = new Access();
        public ForrmAlarm()
        {
            InitializeComponent();
          
          
        }

     
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ForrmAlarm));
            this.lbCode = new System.Windows.Forms.Label();
            this.lbHeader = new System.Windows.Forms.Label();
            this.lbContent = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbCode
            // 
            this.lbCode.AutoSize = true;
            this.lbCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCode.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lbCode.Location = new System.Drawing.Point(141, 22);
            this.lbCode.Name = "lbCode";
            this.lbCode.Size = new System.Drawing.Size(136, 25);
            this.lbCode.TabIndex = 0;
            this.lbCode.Text = "Error : 0x002";
            // 
            // lbHeader
            // 
            this.lbHeader.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHeader.ForeColor = System.Drawing.Color.Red;
            this.lbHeader.Location = new System.Drawing.Point(10, 78);
            this.lbHeader.Name = "lbHeader";
            this.lbHeader.Size = new System.Drawing.Size(373, 54);
            this.lbHeader.TabIndex = 1;
            this.lbHeader.Text = "Camera Disconnect !!";
            this.lbHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbContent
            // 
            this.lbContent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbContent.Location = new System.Drawing.Point(10, 132);
            this.lbContent.Name = "lbContent";
            this.lbContent.Size = new System.Drawing.Size(373, 63);
            this.lbContent.TabIndex = 2;
            this.lbContent.Text = "Turn off the Camera and reconnect it.";
            this.lbContent.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = Properties.Resources.Warning_1;
            this.pictureBox1.Location = new System.Drawing.Point(83, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(52, 50);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(257, 51);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "OK";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(10, 195);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(373, 51);
            this.panel1.TabIndex = 5;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(257, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(116, 51);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ForrmAlarm
            // 
            this.ClientSize = new System.Drawing.Size(393, 251);
            this.Controls.Add(this.lbHeader);
            this.Controls.Add(this.lbContent);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbCode);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ForrmAlarm";
            this.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.Load += new System.EventHandler(this.ForrmAlarm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ForrmAlarm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
        }

        private async void btnCancel_Click(object sender, EventArgs e)
        {
            if(lbCode.Text.Contains("0x002"))
            {
                Global.PLCStatus = PLCStatus.NotConnect;
                await Global.Comunication.Protocol.Connect();
                if (Global.Comunication.Protocol.IsConnected)
                {
                    Global.PLCStatus = PLCStatus.Ready;
                    this.TopMost = false;
                    this.Close();

                    Global.Comunication.Protocol.IsBypass = false;

                }
                else
                {
                    Global.PLCStatus = PLCStatus.ErrorConnect;
                  
                    Global.Comunication.Protocol.IsBypass = true;
                  //  MessageBox.Show("Fail Connect to Module I/O");

                }
            }
            else if (lbCode.Text.Contains("0x001"))
            {
                Global.CameraStatus = CameraStatus.Reconnect;
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Retry", "Reconnect CCD"));
                if ( BeeCore.Common.listCamera[Global.IndexCCCD].ReConnect2())
                {
                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Retry", "Success"));

                    Global.CameraStatus = CameraStatus.Ready;
                    this.TopMost = false;
                    this.Close();

                }
                else
                {
                    Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Retry", "Fail"));
                    this.TopMost = false;
                    this.Close();
                    Global.CameraStatus = CameraStatus.ErrorConnect;
                 
                }
            }
            else
            {
                this.TopMost = false;
                this.Close();
            }

               
           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
