namespace BeeInterface
{
    partial class MaskPainter
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.Button btnDefect;
        private System.Windows.Forms.Button btnNormal;
        private System.Windows.Forms.Button btnEraser;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TrackBar trackBrushSize;
        private System.Windows.Forms.Label lblBrushSize;
        private System.Windows.Forms.PictureBox picCanvas;

        private void InitializeComponent()
        {
            this.panelToolbar = new System.Windows.Forms.Panel();
            this.btnDefect = new System.Windows.Forms.Button();
            this.btnNormal = new System.Windows.Forms.Button();
            this.btnEraser = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.trackBrushSize = new System.Windows.Forms.TrackBar();
            this.lblBrushSize = new System.Windows.Forms.Label();
            this.picCanvas = new System.Windows.Forms.PictureBox();
            this.panelToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBrushSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).BeginInit();
            this.SuspendLayout();
            // 
            // panelToolbar
            // 
            this.panelToolbar.Controls.Add(this.btnDefect);
            this.panelToolbar.Controls.Add(this.btnNormal);
            this.panelToolbar.Controls.Add(this.btnEraser);
            this.panelToolbar.Controls.Add(this.btnUndo);
            this.panelToolbar.Controls.Add(this.btnClear);
            this.panelToolbar.Controls.Add(this.trackBrushSize);
            this.panelToolbar.Controls.Add(this.lblBrushSize);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(0, 0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.panelToolbar.Size = new System.Drawing.Size(640, 48);
            this.panelToolbar.TabIndex = 0;
            // 
            // btnDefect
            // 
            this.btnDefect.Location = new System.Drawing.Point(8, 8);
            this.btnDefect.Name = "btnDefect";
            this.btnDefect.Size = new System.Drawing.Size(72, 32);
            this.btnDefect.TabIndex = 0;
            this.btnDefect.Text = "NG";
            this.btnDefect.UseVisualStyleBackColor = false;
            // 
            // btnNormal
            // 
            this.btnNormal.Location = new System.Drawing.Point(86, 8);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(72, 32);
            this.btnNormal.TabIndex = 1;
            this.btnNormal.Text = "OK";
            this.btnNormal.UseVisualStyleBackColor = false;
            // 
            // btnEraser
            // 
            this.btnEraser.Location = new System.Drawing.Point(164, 8);
            this.btnEraser.Name = "btnEraser";
            this.btnEraser.Size = new System.Drawing.Size(72, 32);
            this.btnEraser.TabIndex = 2;
            this.btnEraser.Text = "Erase";
            this.btnEraser.UseVisualStyleBackColor = false;
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point(242, 8);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(72, 32);
            this.btnUndo.TabIndex = 3;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(320, 8);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(72, 32);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // trackBrushSize
            // 
            this.trackBrushSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBrushSize.Location = new System.Drawing.Point(462, 5);
            this.trackBrushSize.Maximum = 96;
            this.trackBrushSize.Minimum = 2;
            this.trackBrushSize.Name = "trackBrushSize";
            this.trackBrushSize.Size = new System.Drawing.Size(170, 45);
            this.trackBrushSize.TabIndex = 6;
            this.trackBrushSize.TickFrequency = 8;
            this.trackBrushSize.Value = 24;
            // 
            // lblBrushSize
            // 
            this.lblBrushSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBrushSize.Location = new System.Drawing.Point(396, 8);
            this.lblBrushSize.Name = "lblBrushSize";
            this.lblBrushSize.Size = new System.Drawing.Size(68, 32);
            this.lblBrushSize.TabIndex = 5;
            this.lblBrushSize.Text = "Brush";
            this.lblBrushSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picCanvas
            // 
            this.picCanvas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(36)))), ((int)(((byte)(40)))));
            this.picCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picCanvas.Location = new System.Drawing.Point(0, 48);
            this.picCanvas.Name = "picCanvas";
            this.picCanvas.Size = new System.Drawing.Size(640, 432);
            this.picCanvas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCanvas.TabIndex = 1;
            this.picCanvas.TabStop = false;
            // 
            // MaskPainter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picCanvas);
            this.Controls.Add(this.panelToolbar);
            this.Name = "MaskPainter";
            this.Size = new System.Drawing.Size(640, 480);
            this.panelToolbar.ResumeLayout(false);
            this.panelToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBrushSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
