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
            this.pView = new System.Windows.Forms.Panel();
            this.imgHis = new Cyotek.Windows.Forms.ImageBox();
            this.tmShowHis = new System.Windows.Forms.Timer(this.components);
            this.pView.SuspendLayout();
            this.SuspendLayout();
            // 
            // pView
            // 
            this.pView.AutoScroll = true;
            this.pView.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pView.Controls.Add(this.imgHis);
            this.pView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pView.Location = new System.Drawing.Point(0, 0);
            this.pView.Name = "pView";
            this.pView.Size = new System.Drawing.Size(384, 746);
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
            this.imgHis.Location = new System.Drawing.Point(3, 3);
            this.imgHis.Name = "imgHis";
            this.imgHis.PanMode = Cyotek.Windows.Forms.ImageBoxPanMode.Middle;
            this.imgHis.ShortcutsEnabled = false;
            this.imgHis.Size = new System.Drawing.Size(378, 740);
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
            // InforBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pView);
            this.DoubleBuffered = true;
            this.Name = "InforBar";
            this.Size = new System.Drawing.Size(384, 746);
            this.pView.ResumeLayout(false);
            this.pView.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Timer tmShowHis;
        public System.Windows.Forms.Panel pView;
        public Cyotek.Windows.Forms.ImageBox imgHis;
    }
}
