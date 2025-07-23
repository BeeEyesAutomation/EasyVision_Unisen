using BeeCore;
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
            this.label1 = new System.Windows.Forms.Label();
            this.lbHeader = new System.Windows.Forms.Label();
            this.lbContent = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(130, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Error : 404";
            // 
            // lbHeader
            // 
            this.lbHeader.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHeader.ForeColor = System.Drawing.Color.Red;
            this.lbHeader.Location = new System.Drawing.Point(10, 66);
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
            this.lbContent.Location = new System.Drawing.Point(10, 120);
            this.lbContent.Name = "lbContent";
            this.lbContent.Size = new System.Drawing.Size(373, 63);
            this.lbContent.TabIndex = 2;
            this.lbContent.Text = "Turn off the Camera and reconnect it.";
            this.lbContent.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::BeeUi.Properties.Resources.Warning_1;
            this.pictureBox1.Location = new System.Drawing.Point(72, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(52, 50);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(10, 183);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(373, 63);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "OK";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ForrmAlarm
            // 
            this.ClientSize = new System.Drawing.Size(393, 251);
            this.Controls.Add(this.lbHeader);
            this.Controls.Add(this.lbContent);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ForrmAlarm";
            this.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.Load += new System.EventHandler(this.ForrmAlarm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ForrmAlarm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
          
            this.TopMost= false;
            this.Close();
        }
    }
}
