using BeeGlobal;
using BeeInterface;
using System.Windows.Forms;

namespace BeeUi.Unit
{
    partial class Cameras
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Cameras));
            this.Lay = new System.Windows.Forms.TableLayoutPanel();
            this.btnFlowChart = new BeeInterface.RJButton();
            this.btnHardware = new BeeInterface.RJButton();
            this.btnLog = new BeeInterface.RJButton();
            this.btnHistory = new BeeInterface.RJButton();
            this.Lay.SuspendLayout();
            this.SuspendLayout();
            // 
            // Lay
            // 
            this.Lay.AutoScroll = true;
            this.Lay.AutoSize = true;
            this.Lay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Lay.ColumnCount = 4;
            this.Lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.Lay.Controls.Add(this.btnFlowChart, 0, 0);
            this.Lay.Controls.Add(this.btnHistory, 1, 0);
            this.Lay.Controls.Add(this.btnLog, 3, 0);
            this.Lay.Controls.Add(this.btnHardware, 2, 0);
            this.Lay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Lay.Location = new System.Drawing.Point(0, 0);
            this.Lay.Margin = new System.Windows.Forms.Padding(1);
            this.Lay.Name = "Lay";
            this.Lay.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.Lay.RowCount = 1;
            this.Lay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Lay.Size = new System.Drawing.Size(451, 63);
            this.Lay.TabIndex = 31;
            // 
            // btnFlowChart
            // 
            this.btnFlowChart.AutoFont = true;
            this.btnFlowChart.AutoFontHeightRatio = 0.8F;
            this.btnFlowChart.AutoFontMax = 100F;
            this.btnFlowChart.AutoFontMin = 8F;
            this.btnFlowChart.AutoFontWidthRatio = 0.92F;
            this.btnFlowChart.AutoImage = true;
            this.btnFlowChart.AutoImageMaxRatio = 0.7F;
            this.btnFlowChart.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnFlowChart.AutoImageTint = true;
            this.btnFlowChart.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnFlowChart.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnFlowChart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFlowChart.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnFlowChart.BorderRadius = 6;
            this.btnFlowChart.BorderSize = 0;
            this.btnFlowChart.ClickBotColor = System.Drawing.Color.WhiteSmoke;
            this.btnFlowChart.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnFlowChart.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnFlowChart.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnFlowChart.Corner = BeeGlobal.Corner.None;
            this.btnFlowChart.DebounceResizeMs = 16;
            this.btnFlowChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFlowChart.FlatAppearance.BorderSize = 0;
            this.btnFlowChart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFlowChart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnFlowChart.ForeColor = System.Drawing.Color.Black;
            this.btnFlowChart.Image = global::BeeUi.Properties.Resources.Flow;
            this.btnFlowChart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFlowChart.ImageDisabled = null;
            this.btnFlowChart.ImageHover = null;
            this.btnFlowChart.ImageNormal = null;
            this.btnFlowChart.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnFlowChart.ImagePressed = null;
            this.btnFlowChart.ImageTextSpacing = 0;
            this.btnFlowChart.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnFlowChart.ImageTintHover = System.Drawing.Color.Empty;
            this.btnFlowChart.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnFlowChart.ImageTintOpacity = 0.5F;
            this.btnFlowChart.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnFlowChart.IsCLick = true;
            this.btnFlowChart.IsNotChange = false;
            this.btnFlowChart.IsRect = true;
            this.btnFlowChart.IsUnGroup = false;
            this.btnFlowChart.Location = new System.Drawing.Point(0, 5);
            this.btnFlowChart.Margin = new System.Windows.Forms.Padding(0);
            this.btnFlowChart.Multiline = false;
            this.btnFlowChart.Name = "btnFlowChart";
            this.btnFlowChart.Size = new System.Drawing.Size(112, 58);
            this.btnFlowChart.TabIndex = 32;
            this.btnFlowChart.Text = "Chart";
            this.btnFlowChart.TextColor = System.Drawing.Color.Black;
            this.btnFlowChart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnFlowChart.UseVisualStyleBackColor = false;
            this.btnFlowChart.Click += new System.EventHandler(this.btnCamera1_Click);
            // 
            // btnHardware
            // 
            this.btnHardware.AutoFont = true;
            this.btnHardware.AutoFontHeightRatio = 0.8F;
            this.btnHardware.AutoFontMax = 100F;
            this.btnHardware.AutoFontMin = 8F;
            this.btnHardware.AutoFontWidthRatio = 1F;
            this.btnHardware.AutoImage = true;
            this.btnHardware.AutoImageMaxRatio = 0.7F;
            this.btnHardware.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnHardware.AutoImageTint = true;
            this.btnHardware.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnHardware.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnHardware.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHardware.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnHardware.BorderRadius = 0;
            this.btnHardware.BorderSize = 0;
            this.btnHardware.ClickBotColor = System.Drawing.Color.WhiteSmoke;
            this.btnHardware.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnHardware.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnHardware.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnHardware.Corner = BeeGlobal.Corner.None;
            this.btnHardware.DebounceResizeMs = 16;
            this.btnHardware.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHardware.FlatAppearance.BorderSize = 0;
            this.btnHardware.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHardware.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnHardware.ForeColor = System.Drawing.Color.Black;
            this.btnHardware.Image = ((System.Drawing.Image)(resources.GetObject("btnHardware.Image")));
            this.btnHardware.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnHardware.ImageDisabled = null;
            this.btnHardware.ImageHover = null;
            this.btnHardware.ImageNormal = null;
            this.btnHardware.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnHardware.ImagePressed = null;
            this.btnHardware.ImageTextSpacing = 2;
            this.btnHardware.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnHardware.ImageTintHover = System.Drawing.Color.Empty;
            this.btnHardware.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnHardware.ImageTintOpacity = 0.5F;
            this.btnHardware.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnHardware.IsCLick = false;
            this.btnHardware.IsNotChange = false;
            this.btnHardware.IsRect = false;
            this.btnHardware.IsUnGroup = false;
            this.btnHardware.Location = new System.Drawing.Point(224, 5);
            this.btnHardware.Margin = new System.Windows.Forms.Padding(0);
            this.btnHardware.Multiline = false;
            this.btnHardware.Name = "btnHardware";
            this.btnHardware.Size = new System.Drawing.Size(112, 58);
            this.btnHardware.TabIndex = 38;
            this.btnHardware.Text = "Hardware";
            this.btnHardware.TextColor = System.Drawing.Color.Black;
            this.btnHardware.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnHardware.UseVisualStyleBackColor = false;
            this.btnHardware.Click += new System.EventHandler(this.btnSettingPLC_Click);
            // 
            // btnLog
            // 
            this.btnLog.AutoFont = true;
            this.btnLog.AutoFontHeightRatio = 0.8F;
            this.btnLog.AutoFontMax = 100F;
            this.btnLog.AutoFontMin = 8F;
            this.btnLog.AutoFontWidthRatio = 0.92F;
            this.btnLog.AutoImage = true;
            this.btnLog.AutoImageMaxRatio = 0.7F;
            this.btnLog.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLog.AutoImageTint = true;
            this.btnLog.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLog.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnLog.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnLog.BorderRadius = 10;
            this.btnLog.BorderSize = 0;
            this.btnLog.ClickBotColor = System.Drawing.Color.WhiteSmoke;
            this.btnLog.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnLog.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnLog.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLog.Corner = BeeGlobal.Corner.None;
            this.btnLog.DebounceResizeMs = 16;
            this.btnLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLog.FlatAppearance.BorderSize = 0;
            this.btnLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnLog.ForeColor = System.Drawing.Color.Black;
            this.btnLog.Image = global::BeeUi.Properties.Resources.LOG;
            this.btnLog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLog.ImageDisabled = null;
            this.btnLog.ImageHover = null;
            this.btnLog.ImageNormal = null;
            this.btnLog.ImagePadding = new System.Windows.Forms.Padding(3);
            this.btnLog.ImagePressed = null;
            this.btnLog.ImageTextSpacing = 3;
            this.btnLog.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLog.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLog.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLog.ImageTintOpacity = 1F;
            this.btnLog.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLog.IsCLick = false;
            this.btnLog.IsNotChange = false;
            this.btnLog.IsRect = true;
            this.btnLog.IsUnGroup = false;
            this.btnLog.Location = new System.Drawing.Point(336, 5);
            this.btnLog.Margin = new System.Windows.Forms.Padding(0);
            this.btnLog.Multiline = false;
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(115, 58);
            this.btnLog.TabIndex = 36;
            this.btnLog.Text = "Logs";
            this.btnLog.TextColor = System.Drawing.Color.Black;
            this.btnLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnLog.UseVisualStyleBackColor = false;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.AutoFont = true;
            this.btnHistory.AutoFontHeightRatio = 0.8F;
            this.btnHistory.AutoFontMax = 100F;
            this.btnHistory.AutoFontMin = 8F;
            this.btnHistory.AutoFontWidthRatio = 0.92F;
            this.btnHistory.AutoImage = true;
            this.btnHistory.AutoImageMaxRatio = 0.7F;
            this.btnHistory.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnHistory.AutoImageTint = true;
            this.btnHistory.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnHistory.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnHistory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHistory.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnHistory.BorderRadius = 0;
            this.btnHistory.BorderSize = 0;
            this.btnHistory.ClickBotColor = System.Drawing.Color.WhiteSmoke;
            this.btnHistory.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnHistory.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnHistory.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnHistory.Corner = BeeGlobal.Corner.None;
            this.btnHistory.DebounceResizeMs = 16;
            this.btnHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHistory.FlatAppearance.BorderSize = 0;
            this.btnHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnHistory.ForeColor = System.Drawing.Color.Black;
            this.btnHistory.Image = global::BeeUi.Properties.Resources.Image;
            this.btnHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHistory.ImageDisabled = null;
            this.btnHistory.ImageHover = null;
            this.btnHistory.ImageNormal = null;
            this.btnHistory.ImagePadding = new System.Windows.Forms.Padding(3);
            this.btnHistory.ImagePressed = null;
            this.btnHistory.ImageTextSpacing = 3;
            this.btnHistory.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnHistory.ImageTintHover = System.Drawing.Color.Empty;
            this.btnHistory.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnHistory.ImageTintOpacity = 0.5F;
            this.btnHistory.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnHistory.IsCLick = false;
            this.btnHistory.IsNotChange = false;
            this.btnHistory.IsRect = true;
            this.btnHistory.IsUnGroup = false;
            this.btnHistory.Location = new System.Drawing.Point(112, 5);
            this.btnHistory.Margin = new System.Windows.Forms.Padding(0);
            this.btnHistory.Multiline = false;
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(112, 58);
            this.btnHistory.TabIndex = 35;
            this.btnHistory.Text = "History";
            this.btnHistory.TextColor = System.Drawing.Color.Black;
            this.btnHistory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnHistory.UseVisualStyleBackColor = false;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // Cameras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Controls.Add(this.Lay);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Cameras";
            this.Size = new System.Drawing.Size(451, 63);
            this.Lay.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private  System.Windows.Forms.TableLayoutPanel Lay;
        public RJButton btnFlowChart;
        public RJButton btnHistory;
        public RJButton btnLog;
        public RJButton btnHardware;
    }
}
