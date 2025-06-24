namespace BeeUi.Unit
{
    partial class InforBar
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InforBar));
            this.tab = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pView = new System.Windows.Forms.Panel();
            this.imgHis = new Cyotek.Windows.Forms.ImageBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tmShowHis = new System.Windows.Forms.Timer(this.components);
            this.btnHide = new BeeUi.Common.RJButton();
            this.tab.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.pView.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab
            // 
            this.tab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tab.Controls.Add(this.tabPage3);
            this.tab.Controls.Add(this.tabPage4);
            this.tab.Controls.Add(this.tabPage1);
            this.tab.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tab.Location = new System.Drawing.Point(0, 0);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(960, 252);
            this.tab.TabIndex = 29;
            // 
            // tabPage3
            // 
            this.tabPage3.AllowDrop = true;
            this.tabPage3.AutoScroll = true;
            this.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPage3.Controls.Add(this.pView);
            this.tabPage3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(173)))), ((int)(((byte)(245)))));
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(952, 214);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Image";
            // 
            // pView
            // 
            this.pView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pView.AutoScroll = true;
            this.pView.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pView.Controls.Add(this.imgHis);
            this.pView.Location = new System.Drawing.Point(3, 3);
            this.pView.Name = "pView";
            this.pView.Size = new System.Drawing.Size(944, 206);
            this.pView.TabIndex = 7;
            // 
            // imgHis
            // 
            this.imgHis.AllowFreePan = false;
            this.imgHis.AlwaysShowHScroll = true;
            this.imgHis.AlwaysShowVScroll = true;
            this.imgHis.AutoPan = false;
            this.imgHis.AutoSize = true;
            this.imgHis.GridColor = System.Drawing.Color.Transparent;
            this.imgHis.GridScale = Cyotek.Windows.Forms.ImageBoxGridScale.None;
            this.imgHis.Location = new System.Drawing.Point(0, 0);
            this.imgHis.Name = "imgHis";
            this.imgHis.PanMode = Cyotek.Windows.Forms.ImageBoxPanMode.Middle;
            this.imgHis.ShortcutsEnabled = false;
            this.imgHis.Size = new System.Drawing.Size(944, 203);
            this.imgHis.TabIndex = 1;
            this.imgHis.TextBackColor = System.Drawing.Color.White;
            this.imgHis.TextDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.None;
            this.imgHis.Paint += new System.Windows.Forms.PaintEventHandler(this.imgHis_Paint);
            // 
            // tabPage4
            // 
            this.tabPage4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(952, 214);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Output";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(952, 214);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Logs";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tmShowHis
            // 
            this.tmShowHis.Enabled = true;
            this.tmShowHis.Tick += new System.EventHandler(this.tmShowHis_Tick);
            // 
            // btnHide
            // 
            this.btnHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHide.BackColor = System.Drawing.Color.White;
            this.btnHide.BackgroundColor = System.Drawing.Color.White;
            this.btnHide.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHide.BackgroundImage")));
            this.btnHide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHide.BorderColor = System.Drawing.Color.Silver;
            this.btnHide.BorderRadius = 5;
            this.btnHide.BorderSize = 0;
            this.btnHide.ButtonImage = null;
            this.btnHide.Corner = BeeCore.Corner.Both;
            this.btnHide.FlatAppearance.BorderSize = 0;
            this.btnHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHide.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHide.ForeColor = System.Drawing.Color.Black;
            this.btnHide.Image = global::BeeUi.Properties.Resources.Down;
            this.btnHide.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHide.IsCLick = false;
            this.btnHide.IsNotChange = false;
            this.btnHide.IsRect = false;
            this.btnHide.IsUnGroup = true;
            this.btnHide.Location = new System.Drawing.Point(919, 0);
            this.btnHide.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(40, 33);
            this.btnHide.TabIndex = 27;
            this.btnHide.TextColor = System.Drawing.Color.Black;
            this.btnHide.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnHide.UseVisualStyleBackColor = false;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // InforBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnHide);
            this.Controls.Add(this.tab);
            this.Name = "InforBar";
            this.Size = new System.Drawing.Size(960, 252);
            this.tab.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.pView.ResumeLayout(false);
            this.pView.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public Common.RJButton btnHide;
        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.Timer tmShowHis;
        public System.Windows.Forms.Panel pView;
        public Cyotek.Windows.Forms.ImageBox imgHis;
    }
}
