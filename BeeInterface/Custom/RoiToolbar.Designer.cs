namespace BeeInterface
{
    partial class RoiToolbar
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel layoutRoot;
        private RJButton btnRect;
        private RJButton btnEllipse;
        private RJButton btnPolygon;
        private RJButton btnHexagon;
        private RJButton btnNewShape;
        private RJButton btnArea;
        private RJButton btnCrop;
        private RJButton btnMask;
        private RJButton btnInsideOut;
        private RJButton btnOutsideIn;
        private RJButton btnCropFull;
        private RJButton btnCropHalt;
        private RJButton btnBlack;
        private RJButton btnWhite;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.layoutRoot = new System.Windows.Forms.TableLayoutPanel();
            this.btnRect = new BeeInterface.RJButton();
            this.btnEllipse = new BeeInterface.RJButton();
            this.btnPolygon = new BeeInterface.RJButton();
            this.btnHexagon = new BeeInterface.RJButton();
            this.btnNewShape = new BeeInterface.RJButton();
            this.btnArea = new BeeInterface.RJButton();
            this.btnCrop = new BeeInterface.RJButton();
            this.btnMask = new BeeInterface.RJButton();
            this.btnInsideOut = new BeeInterface.RJButton();
            this.btnOutsideIn = new BeeInterface.RJButton();
            this.btnCropFull = new BeeInterface.RJButton();
            this.btnCropHalt = new BeeInterface.RJButton();
            this.btnBlack = new BeeInterface.RJButton();
            this.btnWhite = new BeeInterface.RJButton();
            this.layoutRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutRoot
            // 
            this.layoutRoot.ColumnCount = 7;
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.layoutRoot.Controls.Add(this.btnRect, 0, 0);
            this.layoutRoot.Controls.Add(this.btnEllipse, 1, 0);
            this.layoutRoot.Controls.Add(this.btnPolygon, 2, 0);
            this.layoutRoot.Controls.Add(this.btnHexagon, 3, 0);
            this.layoutRoot.Controls.Add(this.btnNewShape, 4, 0);
            this.layoutRoot.Controls.Add(this.btnArea, 5, 0);
            this.layoutRoot.Controls.Add(this.btnCrop, 6, 0);
            this.layoutRoot.Controls.Add(this.btnMask, 0, 1);
            this.layoutRoot.Controls.Add(this.btnInsideOut, 1, 1);
            this.layoutRoot.Controls.Add(this.btnOutsideIn, 2, 1);
            this.layoutRoot.Controls.Add(this.btnCropFull, 3, 1);
            this.layoutRoot.Controls.Add(this.btnCropHalt, 4, 1);
            this.layoutRoot.Controls.Add(this.btnBlack, 5, 1);
            this.layoutRoot.Controls.Add(this.btnWhite, 6, 1);
            this.layoutRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRoot.Location = new System.Drawing.Point(0, 0);
            this.layoutRoot.Name = "layoutRoot";
            this.layoutRoot.RowCount = 2;
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutRoot.Size = new System.Drawing.Size(700, 104);
            this.layoutRoot.TabIndex = 0;
            // 
            // buttons
            // 
            ConfigureButton(this.btnRect, "Rect");
            ConfigureButton(this.btnEllipse, "Ellipse");
            ConfigureButton(this.btnPolygon, "Polygon");
            ConfigureButton(this.btnHexagon, "Hexagon");
            ConfigureButton(this.btnNewShape, "New");
            ConfigureButton(this.btnArea, "Area");
            ConfigureButton(this.btnCrop, "Crop");
            ConfigureButton(this.btnMask, "Mask");
            ConfigureButton(this.btnInsideOut, "Inside");
            ConfigureButton(this.btnOutsideIn, "Outside");
            ConfigureButton(this.btnCropFull, "Full");
            ConfigureButton(this.btnCropHalt, "Half");
            ConfigureButton(this.btnBlack, "Black");
            ConfigureButton(this.btnWhite, "White");
            // 
            // RoiToolbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutRoot);
            this.Name = "RoiToolbar";
            this.Size = new System.Drawing.Size(700, 104);
            this.layoutRoot.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private static void ConfigureButton(RJButton button, string text)
        {
            button.BackColor = System.Drawing.Color.White;
            button.BackgroundColor = System.Drawing.Color.White;
            button.BorderColor = System.Drawing.Color.FromArgb(180, 180, 180);
            button.BorderRadius = 8;
            button.BorderSize = 1;
            button.Dock = System.Windows.Forms.DockStyle.Fill;
            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            button.ForeColor = System.Drawing.Color.Black;
            button.Margin = new System.Windows.Forms.Padding(3);
            button.Name = "btn" + text;
            button.Text = text;
            button.TextColor = System.Drawing.Color.Black;
            button.UseVisualStyleBackColor = false;
        }
    }
}
