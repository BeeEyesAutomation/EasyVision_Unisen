
namespace BeeInterface
{
    partial class TrackBar2
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
            this.pT = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pT)).BeginInit();
            this.SuspendLayout();
            // 
            // pT
            // 
            this.pT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pT.Location = new System.Drawing.Point(0, 0);
            this.pT.Name = "pT";
            this.pT.Size = new System.Drawing.Size(213, 50);
            this.pT.TabIndex = 0;
            this.pT.TabStop = false;
            this.pT.SizeChanged += new System.EventHandler(this.pT_SizeChanged);
            this.pT.Paint += new System.Windows.Forms.PaintEventHandler(this.pT_Paint);
            this.pT.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Tick_MouseDown);
            this.pT.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Tick_MouseMove);
            this.pT.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Tick_MouseUp);
            // 
            // TrackBar2
            // 
            this.Controls.Add(this.pT);
            this.Name = "TrackBar2";
            this.Size = new System.Drawing.Size(213, 50);
            this.Load += new System.EventHandler(this.TrackBar2_Load);
            this.SizeChanged += new System.EventHandler(this.TrackBar2_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pT)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pT;
    }
}
