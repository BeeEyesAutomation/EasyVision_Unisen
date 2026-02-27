using BeeGlobal;
using BeeInterface;

namespace BeeUi.Unit
{
    partial class StatusBar
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
            this.rjButton1 = new BeeInterface.RJButton();
            this.lbdisPLC = new System.Windows.Forms.Label();
            this.lbBypass = new System.Windows.Forms.Label();
            this.rjButton2 = new BeeInterface.RJButton();
            this.rjButton3 = new BeeInterface.RJButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.rjButton3, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.rjButton2, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbdisPLC, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbBypass, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.rjButton1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(577, 28);
            this.tableLayoutPanel1.TabIndex = 36;
            // 
            // rjButton1
            // 
            this.rjButton1.AutoFont = true;
            this.rjButton1.AutoFontHeightRatio = 1F;
            this.rjButton1.AutoFontMax = 100F;
            this.rjButton1.AutoFontMin = 8F;
            this.rjButton1.AutoFontWidthRatio = 0.92F;
            this.rjButton1.AutoImage = true;
            this.rjButton1.AutoImageMaxRatio = 1F;
            this.rjButton1.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton1.AutoImageTint = true;
            this.rjButton1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rjButton1.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton1.BorderRadius = 5;
            this.rjButton1.BorderSize = 1;
            this.rjButton1.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton1.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton1.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.rjButton1.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton1.Corner = BeeGlobal.Corner.Both;
            this.rjButton1.DebounceResizeMs = 16;
            this.rjButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton1.Enabled = false;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rjButton1.ForeColor = System.Drawing.Color.Black;
            this.rjButton1.Image = global::BeeUi.Properties.Resources.Show;
            this.rjButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton1.ImageDisabled = null;
            this.rjButton1.ImageHover = null;
            this.rjButton1.ImageNormal = null;
            this.rjButton1.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton1.ImagePressed = null;
            this.rjButton1.ImageTextSpacing = 0;
            this.rjButton1.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton1.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton1.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton1.ImageTintOpacity = 0.5F;
            this.rjButton1.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton1.IsCLick = false;
            this.rjButton1.IsNotChange = false;
            this.rjButton1.IsRect = false;
            this.rjButton1.IsTouch = false;
            this.rjButton1.IsUnGroup = true;
            this.rjButton1.Location = new System.Drawing.Point(2, 2);
            this.rjButton1.Margin = new System.Windows.Forms.Padding(2);
            this.rjButton1.Multiline = false;
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(111, 24);
            this.rjButton1.TabIndex = 46;
            this.rjButton1.Text = "Status Bar";
            this.rjButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton1.TextColor = System.Drawing.Color.Black;
            this.rjButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton1.UseVisualStyleBackColor = false;
            // 
            // lbdisPLC
            // 
            this.lbdisPLC.BackColor = System.Drawing.Color.Brown;
            this.lbdisPLC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbdisPLC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbdisPLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbdisPLC.ForeColor = System.Drawing.Color.White;
            this.lbdisPLC.Location = new System.Drawing.Point(118, 0);
            this.lbdisPLC.Name = "lbdisPLC";
            this.lbdisPLC.Size = new System.Drawing.Size(109, 28);
            this.lbdisPLC.TabIndex = 48;
            this.lbdisPLC.Text = "Dis PLC";
            this.lbdisPLC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbdisPLC.Visible = false;
            // 
            // lbBypass
            // 
            this.lbBypass.BackColor = System.Drawing.Color.LightGreen;
            this.lbBypass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBypass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbBypass.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBypass.ForeColor = System.Drawing.Color.White;
            this.lbBypass.Location = new System.Drawing.Point(233, 0);
            this.lbBypass.Name = "lbBypass";
            this.lbBypass.Size = new System.Drawing.Size(109, 28);
            this.lbBypass.TabIndex = 47;
            this.lbBypass.Text = "ByPass";
            this.lbBypass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBypass.Visible = false;
            // 
            // rjButton2
            // 
            this.rjButton2.AutoFont = true;
            this.rjButton2.AutoFontHeightRatio = 1F;
            this.rjButton2.AutoFontMax = 100F;
            this.rjButton2.AutoFontMin = 8F;
            this.rjButton2.AutoFontWidthRatio = 0.92F;
            this.rjButton2.AutoImage = true;
            this.rjButton2.AutoImageMaxRatio = 1F;
            this.rjButton2.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton2.AutoImageTint = true;
            this.rjButton2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton2.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rjButton2.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton2.BorderRadius = 5;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton2.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton2.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.rjButton2.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton2.Corner = BeeGlobal.Corner.Both;
            this.rjButton2.DebounceResizeMs = 16;
            this.rjButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton2.Enabled = false;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rjButton2.ForeColor = System.Drawing.Color.Black;
            this.rjButton2.Image = global::BeeUi.Properties.Resources.Show;
            this.rjButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton2.ImageDisabled = null;
            this.rjButton2.ImageHover = null;
            this.rjButton2.ImageNormal = null;
            this.rjButton2.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton2.ImagePressed = null;
            this.rjButton2.ImageTextSpacing = 0;
            this.rjButton2.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton2.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton2.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton2.ImageTintOpacity = 0.5F;
            this.rjButton2.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton2.IsCLick = false;
            this.rjButton2.IsNotChange = false;
            this.rjButton2.IsRect = false;
            this.rjButton2.IsTouch = false;
            this.rjButton2.IsUnGroup = true;
            this.rjButton2.Location = new System.Drawing.Point(347, 2);
            this.rjButton2.Margin = new System.Windows.Forms.Padding(2);
            this.rjButton2.Multiline = false;
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(111, 24);
            this.rjButton2.TabIndex = 49;
            this.rjButton2.Text = "PLC";
            this.rjButton2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton2.TextColor = System.Drawing.Color.Black;
            this.rjButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton2.UseVisualStyleBackColor = false;
            // 
            // rjButton3
            // 
            this.rjButton3.AutoFont = true;
            this.rjButton3.AutoFontHeightRatio = 1F;
            this.rjButton3.AutoFontMax = 100F;
            this.rjButton3.AutoFontMin = 8F;
            this.rjButton3.AutoFontWidthRatio = 0.92F;
            this.rjButton3.AutoImage = true;
            this.rjButton3.AutoImageMaxRatio = 1F;
            this.rjButton3.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton3.AutoImageTint = true;
            this.rjButton3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton3.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rjButton3.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton3.BorderRadius = 5;
            this.rjButton3.BorderSize = 1;
            this.rjButton3.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton3.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton3.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.rjButton3.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton3.Corner = BeeGlobal.Corner.Both;
            this.rjButton3.DebounceResizeMs = 16;
            this.rjButton3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton3.Enabled = false;
            this.rjButton3.FlatAppearance.BorderSize = 0;
            this.rjButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rjButton3.ForeColor = System.Drawing.Color.Black;
            this.rjButton3.Image = global::BeeUi.Properties.Resources.Show;
            this.rjButton3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton3.ImageDisabled = null;
            this.rjButton3.ImageHover = null;
            this.rjButton3.ImageNormal = null;
            this.rjButton3.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton3.ImagePressed = null;
            this.rjButton3.ImageTextSpacing = 0;
            this.rjButton3.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton3.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton3.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton3.ImageTintOpacity = 0.5F;
            this.rjButton3.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton3.IsCLick = false;
            this.rjButton3.IsNotChange = false;
            this.rjButton3.IsRect = false;
            this.rjButton3.IsTouch = false;
            this.rjButton3.IsUnGroup = true;
            this.rjButton3.Location = new System.Drawing.Point(462, 2);
            this.rjButton3.Margin = new System.Windows.Forms.Padding(2);
            this.rjButton3.Multiline = false;
            this.rjButton3.Name = "rjButton3";
            this.rjButton3.Size = new System.Drawing.Size(113, 24);
            this.rjButton3.TabIndex = 50;
            this.rjButton3.Text = "Camera";
            this.rjButton3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton3.TextColor = System.Drawing.Color.Black;
            this.rjButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton3.UseVisualStyleBackColor = false;
            // 
            // StatusBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "StatusBar";
            this.Size = new System.Drawing.Size(577, 28);
            this.Load += new System.EventHandler(this.HideBar_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public RJButton rjButton1;
        public System.Windows.Forms.Label lbdisPLC;
        public System.Windows.Forms.Label lbBypass;
        public RJButton rjButton3;
        public RJButton rjButton2;
    }
}
