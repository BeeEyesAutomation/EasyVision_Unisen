
namespace BeeUi.Commons
{
    partial class ItemTool
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.name = new System.Windows.Forms.Label();
            this.icon = new System.Windows.Forms.PictureBox();
            this.lbNumber = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.lbCycle = new System.Windows.Forms.Label();
            this.lbScore = new System.Windows.Forms.Label();
            this.Score = new BeeUi.TrackBar2();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            this.SuspendLayout();
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.BackColor = System.Drawing.Color.Transparent;
            this.name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name.Location = new System.Drawing.Point(56, 9);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(72, 20);
            this.name.TabIndex = 3;
            this.name.Text = "OutLine";
            this.name.DoubleClick += new System.EventHandler(this.name_DoubleClick);
            // 
            // icon
            // 
            this.icon.BackColor = System.Drawing.Color.Transparent;
            this.icon.Image = global::BeeUi.Properties.Resources.OutLine;
            this.icon.Location = new System.Drawing.Point(32, 5);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(24, 24);
            this.icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.icon.TabIndex = 2;
            this.icon.TabStop = false;
            // 
            // lbNumber
            // 
            this.lbNumber.AutoSize = true;
            this.lbNumber.BackColor = System.Drawing.Color.Transparent;
            this.lbNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNumber.Location = new System.Drawing.Point(4, 9);
            this.lbNumber.Name = "lbNumber";
            this.lbNumber.Size = new System.Drawing.Size(29, 20);
            this.lbNumber.TabIndex = 5;
            this.lbNumber.Text = "01";
            // 
            // lbStatus
            // 
            this.lbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(172)))), ((int)(((byte)(73)))));
            this.lbStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.ForeColor = System.Drawing.Color.White;
            this.lbStatus.Location = new System.Drawing.Point(283, 4);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(87, 45);
            this.lbStatus.TabIndex = 7;
            this.lbStatus.Text = "OK";
            this.lbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbStatus.Click += new System.EventHandler(this.lbStatus_Click);
            // 
            // lbCycle
            // 
            this.lbCycle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCycle.BackColor = System.Drawing.Color.Gainsboro;
            this.lbCycle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbCycle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCycle.Location = new System.Drawing.Point(215, 4);
            this.lbCycle.Name = "lbCycle";
            this.lbCycle.Size = new System.Drawing.Size(65, 25);
            this.lbCycle.TabIndex = 8;
            this.lbCycle.Text = "200 ms";
            this.lbCycle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbScore
            // 
            this.lbScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbScore.BackColor = System.Drawing.Color.Transparent;
            this.lbScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbScore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(172)))), ((int)(((byte)(73)))));
            this.lbScore.Location = new System.Drawing.Point(282, 49);
            this.lbScore.Name = "lbScore";
            this.lbScore.Size = new System.Drawing.Size(87, 30);
            this.lbScore.TabIndex = 10;
            this.lbScore.Text = "100";
            this.lbScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Score
            // 
            this.Score.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Score.BackColor = System.Drawing.Color.Transparent;
            this.Score.ColorTrack = System.Drawing.Color.Gray;
            this.Score.Enabled = false;
            this.Score.Location = new System.Drawing.Point(26, 33);
            this.Score.Max = 100;
            this.Score.Min = 0;
            this.Score.Name = "Score";
            this.Score.Size = new System.Drawing.Size(254, 44);
            this.Score.Step = 1;
            this.Score.TabIndex = 11;
            this.Score.Value = 0;
            this.Score.ValueScore = 0;
            this.Score.ValueChanged +=  new System.Action<float>(this.Score_ValueChanged);
            this.Score.Load += new System.EventHandler(this.Score_Load);
            // 
            // ItemTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.Score);
            this.Controls.Add(this.lbScore);
            this.Controls.Add(this.lbCycle);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.lbNumber);
            this.Controls.Add(this.name);
            this.Controls.Add(this.icon);
            this.DoubleBuffered = true;
            this.Name = "ItemTool";
            this.Size = new System.Drawing.Size(375, 79);
            this.Load += new System.EventHandler(this.ItemTool_Load);
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label name;
        public System.Windows.Forms.PictureBox icon;
        public System.Windows.Forms.Label lbNumber;
        public System.Windows.Forms.Label lbStatus;
        public System.Windows.Forms.Label lbCycle;
        public System.Windows.Forms.Label lbScore;
        public TrackBar2 Score;
    }
}
