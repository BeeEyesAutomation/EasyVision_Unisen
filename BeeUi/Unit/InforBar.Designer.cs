using BeeGlobal;
using BeeInterface;

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
            this.pView = new System.Windows.Forms.Panel();
            this.imgHis = new Cyotek.Windows.Forms.ImageBox();
            this.tmShowHis = new System.Windows.Forms.Timer(this.components);
            this.btnHide = new BeeInterface.RJButton();
            this.tableLayoutPanel1 = new DbTableLayoutPanel();
            this.rjButton1 = new BeeInterface.RJButton();
            this.pView.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
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
            this.pView.Size = new System.Drawing.Size(320, 696);
            this.pView.TabIndex = 7;
            // 
            // imgHis
            // 
            this.imgHis.AllowFreePan = false;
            this.imgHis.AlwaysShowHScroll = true;
            this.imgHis.AlwaysShowVScroll = true;
            this.imgHis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imgHis.AutoPan = false;
            this.imgHis.AutoSize = true;
            this.imgHis.GridColor = System.Drawing.Color.Transparent;
            this.imgHis.GridScale = Cyotek.Windows.Forms.ImageBoxGridScale.None;
            this.imgHis.Location = new System.Drawing.Point(6, 249);
            this.imgHis.Name = "imgHis";
            this.imgHis.PanMode = Cyotek.Windows.Forms.ImageBoxPanMode.Middle;
            this.imgHis.ShortcutsEnabled = false;
            this.imgHis.Size = new System.Drawing.Size(191, 109);
            this.imgHis.TabIndex = 1;
            this.imgHis.TextBackColor = System.Drawing.Color.White;
            this.imgHis.TextDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.None;
            this.imgHis.Paint += new System.Windows.Forms.PaintEventHandler(this.imgHis_Paint);
            // 
            // tmShowHis
            // 
            this.tmShowHis.Enabled = true;
            this.tmShowHis.Tick += new System.EventHandler(this.tmShowHis_Tick);
            // 
            // btnHide
            // 
            this.btnHide.BackColor = System.Drawing.Color.White;
            this.btnHide.BackgroundColor = System.Drawing.Color.White;
            this.btnHide.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHide.BackgroundImage")));
            this.btnHide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHide.BorderColor = System.Drawing.Color.Silver;
            this.btnHide.BorderRadius = 5;
            this.btnHide.BorderSize = 0;
            this.btnHide.Corner = BeeGlobal.Corner.Both;
            this.btnHide.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.btnHide.Location = new System.Drawing.Point(0, 0);
            this.btnHide.Margin = new System.Windows.Forms.Padding(0);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(42, 32);
            this.btnHide.TabIndex = 27;
            this.btnHide.TextColor = System.Drawing.Color.Black;
            this.btnHide.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnHide.UseVisualStyleBackColor = false;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.rjButton1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnHide, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(342, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.539007F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.46099F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(42, 746);
            this.tableLayoutPanel1.TabIndex = 28;
            // 
            // rjButton1
            // 
            this.rjButton1.BackColor = System.Drawing.Color.White;
            this.rjButton1.BackgroundColor = System.Drawing.Color.White;
            this.rjButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton1.BackgroundImage")));
            this.rjButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rjButton1.BorderColor = System.Drawing.Color.Silver;
            this.rjButton1.BorderRadius = 5;
            this.rjButton1.BorderSize = 0;
            this.rjButton1.Corner = BeeGlobal.Corner.Both;
            this.rjButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton1.ForeColor = System.Drawing.Color.Black;
            this.rjButton1.Image = global::BeeUi.Properties.Resources.Down;
            this.rjButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton1.IsCLick = false;
            this.rjButton1.IsNotChange = false;
            this.rjButton1.IsRect = false;
            this.rjButton1.IsUnGroup = true;
            this.rjButton1.Location = new System.Drawing.Point(0, 32);
            this.rjButton1.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(42, 673);
            this.rjButton1.TabIndex = 28;
            this.rjButton1.Text = "Image";
            this.rjButton1.TextColor = System.Drawing.Color.Black;
            this.rjButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rjButton1.UseVisualStyleBackColor = false;
            // 
            // InforBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pView);
            this.Name = "InforBar";
            this.Size = new System.Drawing.Size(384, 746);
            this.pView.ResumeLayout(false);
            this.pView.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public RJButton btnHide;
        public System.Windows.Forms.Timer tmShowHis;
        public System.Windows.Forms.Panel pView;
        public Cyotek.Windows.Forms.ImageBox imgHis;
        private DbTableLayoutPanel tableLayoutPanel1;
        public RJButton rjButton1;
    }
}
