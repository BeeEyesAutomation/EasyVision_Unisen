namespace BeeUi.Unit
{
    partial class RegisterImgs
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
            this.registerImgDashboard1 = new BeeInterface.RegisterImgDashboard();
            this.registerImg = new BeeInterface.RegisterImgDashboard();
            this.SuspendLayout();
            // 
            // registerImgDashboard1
            // 
            this.registerImgDashboard1.AutoNameDigits = 3;
            this.registerImgDashboard1.AutoNamePrefix = "Img";
            this.registerImgDashboard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registerImgDashboard1.HideTopBar = true;
            this.registerImgDashboard1.Location = new System.Drawing.Point(0, 0);
            this.registerImgDashboard1.Name = "registerImgDashboard1";
            this.registerImgDashboard1.ShowCameraButton = false;
            this.registerImgDashboard1.Size = new System.Drawing.Size(350, 497);
            this.registerImgDashboard1.TabIndex = 1;
            // 
            // registerImg
            // 
            this.registerImg.AutoNameDigits = 3;
            this.registerImg.AutoNamePrefix = "Img";
            this.registerImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registerImg.HideTopBar = true;
            this.registerImg.Location = new System.Drawing.Point(0, 0);
            this.registerImg.Name = "registerImg";
            this.registerImg.ShowCameraButton = false;
            this.registerImg.Size = new System.Drawing.Size(350, 497);
            this.registerImg.TabIndex = 2;
            this.registerImg.SelectedItemChanged += new System.EventHandler<BeeInterface.RegisterImgSelectionChangedEventArgs>(this.registerImg_SelectedItemChanged);
            // 
            // RegisterImgs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.registerImg);
            this.Controls.Add(this.registerImgDashboard1);
            this.DoubleBuffered = true;
            this.Name = "RegisterImgs";
            this.Size = new System.Drawing.Size(350, 497);
            this.Load += new System.EventHandler(this.RegisterImgs_Load);
            this.VisibleChanged += new System.EventHandler(this.RegisterImgs_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private BeeInterface.RegisterImgDashboard registerImgDashboard1;
        private BeeInterface.RegisterImgDashboard registerImg;
    }
}
