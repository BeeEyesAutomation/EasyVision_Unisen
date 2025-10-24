namespace BeeInterface.Tool
{
    partial class CounterInfor
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.autoFontLabel1 = new BeeInterface.AutoFontLabel();
            this.AdjScale = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.AdjScale, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.autoFontLabel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.40909F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.59091F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 563F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(528, 677);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // autoFontLabel1
            // 
            this.autoFontLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoFontLabel1.AutoFont = true;
            this.autoFontLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 38.89844F);
            this.autoFontLabel1.Location = new System.Drawing.Point(3, 0);
            this.autoFontLabel1.Name = "autoFontLabel1";
            this.autoFontLabel1.Size = new System.Drawing.Size(522, 60);
            this.autoFontLabel1.TabIndex = 0;
            this.autoFontLabel1.Text = "autoFontLabel1";
            // 
            // AdjScale
            // 
            this.AdjScale.AutoRepeatAccelDeltaMs = -5;
            this.AdjScale.AutoRepeatAccelerate = true;
            this.AdjScale.AutoRepeatEnabled = true;
            this.AdjScale.AutoRepeatInitialDelay = 400;
            this.AdjScale.AutoRepeatInterval = 60;
            this.AdjScale.AutoRepeatMinInterval = 20;
            this.AdjScale.AutoShowTextbox = true;
            this.AdjScale.AutoSizeTextbox = true;
            this.AdjScale.BackColor = System.Drawing.Color.White;
            this.AdjScale.BarLeftGap = 20;
            this.AdjScale.BarRightGap = 6;
            this.AdjScale.ChromeGap = 1;
            this.AdjScale.ChromeWidthRatio = 0.14F;
            this.AdjScale.ColorBorder = System.Drawing.Color.LightGray;
            this.AdjScale.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjScale.ColorScale = System.Drawing.Color.LightGray;
            this.AdjScale.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjScale.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjScale.ColorTrack = System.Drawing.Color.LightGray;
            this.AdjScale.Decimals = 1;
            this.AdjScale.DisabledDesaturateMix = 0.3F;
            this.AdjScale.DisabledDimFactor = 0.9F;
            this.AdjScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjScale.EdgePadding = 2;
            this.AdjScale.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjScale.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjScale.KeyboardStep = 1F;
            this.AdjScale.Location = new System.Drawing.Point(5, 60);
            this.AdjScale.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.AdjScale.MatchTextboxFontToThumb = true;
            this.AdjScale.Max = 1000F;
            this.AdjScale.MaxTextboxWidth = 0;
            this.AdjScale.MaxThumb = 1000;
            this.AdjScale.MaxTrackHeight = 1000;
            this.AdjScale.Min = 1F;
            this.AdjScale.MinChromeWidth = 64;
            this.AdjScale.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjScale.MinTextboxWidth = 32;
            this.AdjScale.MinThumb = 30;
            this.AdjScale.MinTrackHeight = 8;
            this.AdjScale.Name = "AdjScale";
            this.AdjScale.Radius = 8;
            this.AdjScale.ShowValueOnThumb = true;
            this.AdjScale.Size = new System.Drawing.Size(518, 53);
            this.AdjScale.SnapToStep = true;
            this.AdjScale.StartWithTextboxHidden = true;
            this.AdjScale.Step = 1F;
            this.AdjScale.TabIndex = 84;
            this.AdjScale.TextboxFontSize = 22F;
            this.AdjScale.TextboxSidePadding = 10;
            this.AdjScale.TextboxWidth = 600;
            this.AdjScale.ThumbDiameterRatio = 2F;
            this.AdjScale.ThumbValueBold = true;
            this.AdjScale.ThumbValueFontScale = 1.5F;
            this.AdjScale.ThumbValuePadding = 0;
            this.AdjScale.TightEdges = true;
            this.AdjScale.TrackHeightRatio = 0.45F;
            this.AdjScale.TrackWidthRatio = 1F;
            this.AdjScale.UnitText = "";
            this.AdjScale.Value = 1F;
            this.AdjScale.WheelStep = 1F;
            // 
            // CounterInfor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CounterInfor";
            this.Size = new System.Drawing.Size(528, 677);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AutoFontLabel autoFontLabel1;
        private AdjustBarEx AdjScale;
    }
}
