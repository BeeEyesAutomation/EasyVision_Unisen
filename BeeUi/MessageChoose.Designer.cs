
namespace BeeUi
{
    partial class MessageChoose
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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageChoose));
            this.lbContent = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNo = new BeeInterface.RJButton();
            this.btnYes = new BeeInterface.RJButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lbHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbContent
            // 
            this.lbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbContent.Location = new System.Drawing.Point(3, 60);
            this.lbContent.Name = "lbContent";
            this.lbContent.Size = new System.Drawing.Size(355, 108);
            this.lbContent.TabIndex = 2;
            this.lbContent.Text = "Copy Prog ...";
            this.lbContent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::BeeUi.Properties.Resources.Warning_1;
            this.pictureBox1.Location = new System.Drawing.Point(40, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 169F));
            this.tableLayoutPanel1.Controls.Add(this.btnNo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnYes, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 171);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(355, 54);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // btnNo
            // 
            this.btnNo.AutoFont = true;
            this.btnNo.AutoFontHeightRatio = 0.75F;
            this.btnNo.AutoFontMax = 100F;
            this.btnNo.AutoFontMin = 6F;
            this.btnNo.AutoFontWidthRatio = 0.92F;
            this.btnNo.AutoImage = true;
            this.btnNo.AutoImageMaxRatio = 0.75F;
            this.btnNo.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnNo.AutoImageTint = true;
            this.btnNo.BackColor = System.Drawing.SystemColors.Control;
            this.btnNo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnNo.BorderColor = System.Drawing.SystemColors.Control;
            this.btnNo.BorderRadius = 14;
            this.btnNo.BorderSize = 1;
            this.btnNo.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnNo.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnNo.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnNo.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnNo.Corner = BeeGlobal.Corner.Both;
            this.btnNo.DebounceResizeMs = 16;
            this.btnNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnNo.ForeColor = System.Drawing.Color.DarkGray;
            this.btnNo.Image = null;
            this.btnNo.ImageDisabled = null;
            this.btnNo.ImageHover = null;
            this.btnNo.ImageNormal = null;
            this.btnNo.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnNo.ImagePressed = null;
            this.btnNo.ImageTextSpacing = 6;
            this.btnNo.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnNo.ImageTintHover = System.Drawing.Color.Empty;
            this.btnNo.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnNo.ImageTintOpacity = 0.5F;
            this.btnNo.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnNo.IsCLick = false;
            this.btnNo.IsNotChange = false;
            this.btnNo.IsRect = false;
            this.btnNo.IsTouch = false;
            this.btnNo.IsUnGroup = false;
            this.btnNo.Location = new System.Drawing.Point(189, 3);
            this.btnNo.Multiline = false;
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(163, 48);
            this.btnNo.TabIndex = 1;
            this.btnNo.Text = "NO";
            this.btnNo.TextColor = System.Drawing.Color.DarkGray;
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnYes
            // 
            this.btnYes.AutoFont = true;
            this.btnYes.AutoFontHeightRatio = 0.75F;
            this.btnYes.AutoFontMax = 100F;
            this.btnYes.AutoFontMin = 6F;
            this.btnYes.AutoFontWidthRatio = 0.92F;
            this.btnYes.AutoImage = true;
            this.btnYes.AutoImageMaxRatio = 0.75F;
            this.btnYes.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnYes.AutoImageTint = true;
            this.btnYes.BackColor = System.Drawing.SystemColors.Control;
            this.btnYes.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnYes.BorderColor = System.Drawing.SystemColors.Control;
            this.btnYes.BorderRadius = 14;
            this.btnYes.BorderSize = 1;
            this.btnYes.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnYes.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnYes.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnYes.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnYes.Corner = BeeGlobal.Corner.Both;
            this.btnYes.DebounceResizeMs = 16;
            this.btnYes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F, System.Drawing.FontStyle.Bold);
            this.btnYes.ForeColor = System.Drawing.Color.Black;
            this.btnYes.Image = null;
            this.btnYes.ImageDisabled = null;
            this.btnYes.ImageHover = null;
            this.btnYes.ImageNormal = null;
            this.btnYes.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnYes.ImagePressed = null;
            this.btnYes.ImageTextSpacing = 6;
            this.btnYes.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnYes.ImageTintHover = System.Drawing.Color.Empty;
            this.btnYes.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnYes.ImageTintOpacity = 0.5F;
            this.btnYes.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnYes.IsCLick = false;
            this.btnYes.IsNotChange = false;
            this.btnYes.IsRect = false;
            this.btnYes.IsTouch = false;
            this.btnYes.IsUnGroup = false;
            this.btnYes.Location = new System.Drawing.Point(3, 3);
            this.btnYes.Multiline = false;
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(180, 48);
            this.btnYes.TabIndex = 0;
            this.btnYes.Text = "YES";
            this.btnYes.TextColor = System.Drawing.Color.Black;
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lbContent, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(361, 228);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.lbHeader, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(361, 60);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // lbHeader
            // 
            this.lbHeader.BackColor = System.Drawing.Color.Transparent;
            this.lbHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHeader.ForeColor = System.Drawing.Color.Black;
            this.lbHeader.Location = new System.Drawing.Point(100, 0);
            this.lbHeader.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lbHeader.Name = "lbHeader";
            this.lbHeader.Size = new System.Drawing.Size(258, 60);
            this.lbHeader.TabIndex = 4;
            this.lbHeader.Text = "Change Mode";
            this.lbHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MessageChoose
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(364, 231);
            this.Controls.Add(this.tableLayoutPanel2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MessageChoose";
            this.Padding = new System.Windows.Forms.Padding(1, 1, 2, 2);
            this.Load += new System.EventHandler(this.ForrmAlarm_Load);
            this.Shown += new System.EventHandler(this.FormChoose_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Label lbContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BeeInterface.RJButton btnNo;
        private BeeInterface.RJButton btnYes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        public System.Windows.Forms.Label lbHeader;
    }
}