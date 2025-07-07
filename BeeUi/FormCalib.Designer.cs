
using BeeInterface;

namespace BeeUi
{
    partial class FormCalib
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCalib));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbStatus = new System.Windows.Forms.Label();
            this.btnSetSample = new RJButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCheck = new RJButton();
            this.lbScoreError = new System.Windows.Forms.Label();
            this.trackScoreErr = new TrackBar2();
            this.workerCalib = new System.ComponentModel.BackgroundWorker();
            this.btnLive = new RJButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnLive);
            this.panel1.Controls.Add(this.lbStatus);
            this.panel1.Controls.Add(this.btnSetSample);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnCheck);
            this.panel1.Controls.Add(this.lbScoreError);
            this.panel1.Controls.Add(this.trackScoreErr);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(474, 257);
            this.panel1.TabIndex = 46;
            // 
            // lbStatus
            // 
            this.lbStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(186)))), ((int)(((byte)(98)))));
            this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.ForeColor = System.Drawing.Color.White;
            this.lbStatus.Location = new System.Drawing.Point(118, 7);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(349, 90);
            this.lbStatus.TabIndex = 45;
            this.lbStatus.Text = "---";
            this.lbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSetSample
            // 
            this.btnSetSample.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSetSample.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnSetSample.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSetSample.BackgroundImage")));
            this.btnSetSample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSetSample.BorderColor = System.Drawing.Color.Silver;
            this.btnSetSample.BorderRadius = 5;
            this.btnSetSample.BorderSize = 1;
            this.btnSetSample.FlatAppearance.BorderSize = 0;
            this.btnSetSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetSample.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetSample.ForeColor = System.Drawing.Color.Black;
            this.btnSetSample.Image = global::BeeUi.Properties.Resources.Example;
            this.btnSetSample.IsCLick = false;
            this.btnSetSample.IsNotChange = true;
            this.btnSetSample.IsRect = false;
            this.btnSetSample.IsUnGroup = true;
            this.btnSetSample.Location = new System.Drawing.Point(146, 112);
            this.btnSetSample.Name = "btnSetSample";
            this.btnSetSample.Size = new System.Drawing.Size(182, 59);
            this.btnSetSample.TabIndex = 34;
            this.btnSetSample.Text = "Set Sample";
            this.btnSetSample.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSetSample.TextColor = System.Drawing.Color.Black;
            this.btnSetSample.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSetSample.UseVisualStyleBackColor = false;
            this.btnSetSample.Click += new System.EventHandler(this.btnSetSample_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 33);
            this.label1.TabIndex = 43;
            this.label1.Text = "Status";
            // 
            // btnCheck
            // 
            this.btnCheck.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCheck.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnCheck.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCheck.BackgroundImage")));
            this.btnCheck.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCheck.BorderColor = System.Drawing.Color.Silver;
            this.btnCheck.BorderRadius = 5;
            this.btnCheck.BorderSize = 1;
            this.btnCheck.FlatAppearance.BorderSize = 0;
            this.btnCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheck.ForeColor = System.Drawing.Color.Black;
            this.btnCheck.Image = global::BeeUi.Properties.Resources.Natural_User_Interface_2;
            this.btnCheck.IsCLick = false;
            this.btnCheck.IsNotChange = false;
            this.btnCheck.IsRect = false;
            this.btnCheck.IsUnGroup = true;
            this.btnCheck.Location = new System.Drawing.Point(348, 112);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(119, 59);
            this.btnCheck.TabIndex = 40;
            this.btnCheck.Text = "Check";
            this.btnCheck.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCheck.TextColor = System.Drawing.Color.Black;
            this.btnCheck.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCheck.UseVisualStyleBackColor = false;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // lbScoreError
            // 
            this.lbScoreError.AutoSize = true;
            this.lbScoreError.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbScoreError.Location = new System.Drawing.Point(6, 200);
            this.lbScoreError.Name = "lbScoreError";
            this.lbScoreError.Size = new System.Drawing.Size(119, 20);
            this.lbScoreError.TabIndex = 42;
            this.lbScoreError.Text = "Error Bightness";
            // 
            // trackScoreErr
            // 
            this.trackScoreErr.ColorTrack = System.Drawing.Color.Gray;
            this.trackScoreErr.Location = new System.Drawing.Point(133, 190);
            this.trackScoreErr.Max = 50;
            this.trackScoreErr.Min = 1;
            this.trackScoreErr.Name = "trackScoreErr";
            this.trackScoreErr.Size = new System.Drawing.Size(334, 58);
            this.trackScoreErr.Step = 1;
            this.trackScoreErr.TabIndex = 41;
            this.trackScoreErr.Value = 1;
            this.trackScoreErr.ValueScore = 1;
            this.trackScoreErr.ValueChanged += new System.Action<float>(this.trackScoreErr_ValueChanged);
            // 
            // workerCalib
            // 
            this.workerCalib.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerCalib_DoWork);
            this.workerCalib.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerCalib_RunWorkerCompleted);
            // 
            // btnLive
            // 
            this.btnLive.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLive.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnLive.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLive.BackgroundImage")));
            this.btnLive.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLive.BorderColor = System.Drawing.Color.Silver;
            this.btnLive.BorderRadius = 5;
            this.btnLive.BorderSize = 1;
            this.btnLive.FlatAppearance.BorderSize = 0;
            this.btnLive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLive.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLive.ForeColor = System.Drawing.Color.Black;
            this.btnLive.Image = global::BeeUi.Properties.Resources.Record_2;
            this.btnLive.IsCLick = false;
            this.btnLive.IsNotChange = false;
            this.btnLive.IsRect = false;
            this.btnLive.IsUnGroup = true;
            this.btnLive.Location = new System.Drawing.Point(10, 112);
            this.btnLive.Name = "btnLive";
            this.btnLive.Size = new System.Drawing.Size(119, 59);
            this.btnLive.TabIndex = 46;
            this.btnLive.Text = "LIVE";
            this.btnLive.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLive.TextColor = System.Drawing.Color.Black;
            this.btnLive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLive.UseVisualStyleBackColor = false;
            this.btnLive.Click += new System.EventHandler(this.btnLive_Click);
            // 
            // FormCalib
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 257);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCalib";
            this.Text = "FormCalib";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCalib_FormClosing);
            this.Load += new System.EventHandler(this.FormCalib_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public RJButton btnSetSample;
        private System.Windows.Forms.Label label1;
        public RJButton btnCheck;
        private System.Windows.Forms.Label lbScoreError;
        private TrackBar2 trackScoreErr;
        private System.ComponentModel.BackgroundWorker workerCalib;
        public System.Windows.Forms.Label lbStatus;
        public RJButton btnLive;
    }
}