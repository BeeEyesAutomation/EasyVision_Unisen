
using BeeInterface;

namespace BeeInterface
{
    partial class ToolPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolPage));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabTool = new System.Windows.Forms.TabControl();
            this.Basic_Tool = new System.Windows.Forms.TabPage();
            this.Extra_Tool_1 = new System.Windows.Forms.TabPage();
            this.Extra_Tool_2 = new System.Windows.Forms.TabPage();
            this.pContent = new System.Windows.Forms.Panel();
            this.img = new System.Windows.Forms.PictureBox();
            this.lbContent = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.btnOk = new BeeInterface.RJButton();
            this.btnCancel = new BeeInterface.RJButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabTool.SuspendLayout();
            this.pContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.img)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label1.Location = new System.Drawing.Point(193, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(373, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hãy chọn Tool phù hợp cho từng mục đích";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(796, 53);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 557);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(796, 59);
            this.panel2.TabIndex = 4;
            // 
            // tabTool
            // 
            this.tabTool.Controls.Add(this.Basic_Tool);
            this.tabTool.Controls.Add(this.Extra_Tool_1);
            this.tabTool.Controls.Add(this.Extra_Tool_2);
            this.tabTool.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabTool.Location = new System.Drawing.Point(464, 53);
            this.tabTool.Name = "tabTool";
            this.tabTool.SelectedIndex = 0;
            this.tabTool.Size = new System.Drawing.Size(332, 504);
            this.tabTool.TabIndex = 5;
            this.tabTool.SelectedIndexChanged += new System.EventHandler(this.tabTool_SelectedIndexChanged);
            // 
            // Basic_Tool
            // 
            this.Basic_Tool.Location = new System.Drawing.Point(4, 29);
            this.Basic_Tool.Name = "Basic_Tool";
            this.Basic_Tool.Padding = new System.Windows.Forms.Padding(3);
            this.Basic_Tool.Size = new System.Drawing.Size(324, 471);
            this.Basic_Tool.TabIndex = 0;
            this.Basic_Tool.Text = "Basic Tool";
            this.Basic_Tool.UseVisualStyleBackColor = true;
            // 
            // Extra_Tool_1
            // 
            this.Extra_Tool_1.Location = new System.Drawing.Point(4, 29);
            this.Extra_Tool_1.Name = "Extra_Tool_1";
            this.Extra_Tool_1.Padding = new System.Windows.Forms.Padding(3);
            this.Extra_Tool_1.Size = new System.Drawing.Size(324, 471);
            this.Extra_Tool_1.TabIndex = 1;
            this.Extra_Tool_1.Text = "Extra Tool 1";
            this.Extra_Tool_1.UseVisualStyleBackColor = true;
            // 
            // Extra_Tool_2
            // 
            this.Extra_Tool_2.Location = new System.Drawing.Point(4, 29);
            this.Extra_Tool_2.Name = "Extra_Tool_2";
            this.Extra_Tool_2.Padding = new System.Windows.Forms.Padding(3);
            this.Extra_Tool_2.Size = new System.Drawing.Size(324, 471);
            this.Extra_Tool_2.TabIndex = 2;
            this.Extra_Tool_2.Text = "Extra tool 2";
            this.Extra_Tool_2.UseVisualStyleBackColor = true;
            // 
            // pContent
            // 
            this.pContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.pContent.Controls.Add(this.img);
            this.pContent.Controls.Add(this.lbContent);
            this.pContent.Controls.Add(this.lbName);
            this.pContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pContent.Location = new System.Drawing.Point(0, 53);
            this.pContent.Name = "pContent";
            this.pContent.Size = new System.Drawing.Size(464, 504);
            this.pContent.TabIndex = 6;
            // 
            // img
            // 
            this.img.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.img.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.img.Dock = System.Windows.Forms.DockStyle.Fill;
            this.img.Image = global::BeeInterface.Properties.Resources.contentPosition_Adjustment;
            this.img.Location = new System.Drawing.Point(0, 38);
            this.img.Name = "img";
            this.img.Size = new System.Drawing.Size(464, 319);
            this.img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.img.TabIndex = 1;
            this.img.TabStop = false;
            // 
            // lbContent
            // 
            this.lbContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.lbContent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbContent.ForeColor = System.Drawing.Color.Black;
            this.lbContent.Location = new System.Drawing.Point(0, 357);
            this.lbContent.Name = "lbContent";
            this.lbContent.Padding = new System.Windows.Forms.Padding(10);
            this.lbContent.Size = new System.Drawing.Size(464, 147);
            this.lbContent.TabIndex = 3;
            // 
            // lbName
            // 
            this.lbName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.ForeColor = System.Drawing.Color.White;
            this.lbName.Location = new System.Drawing.Point(0, 0);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(464, 38);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "Tool";
            this.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOk
            // 
            this.btnOk.AutoFont = true;
            this.btnOk.AutoFontHeightRatio = 0.75F;
            this.btnOk.AutoFontMax = 100F;
            this.btnOk.AutoFontMin = 6F;
            this.btnOk.AutoFontWidthRatio = 0.92F;
            this.btnOk.AutoImage = true;
            this.btnOk.AutoImageMaxRatio = 0.75F;
            this.btnOk.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnOk.AutoImageTint = true;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.btnOk.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.btnOk.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOk.BackgroundImage")));
            this.btnOk.BorderColor = System.Drawing.Color.Transparent;
            this.btnOk.BorderRadius = 5;
            this.btnOk.BorderSize = 1;
            this.btnOk.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnOk.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnOk.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnOk.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnOk.Corner = BeeGlobal.Corner.Both;
          
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.42188F, System.Drawing.FontStyle.Bold);
            this.btnOk.ForeColor = System.Drawing.Color.Black;
            this.btnOk.Image = null;
            this.btnOk.ImageDisabled = null;
            this.btnOk.ImageHover = null;
            this.btnOk.ImageNormal = null;
            this.btnOk.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnOk.ImagePressed = null;
            this.btnOk.ImageTextSpacing = 6;
            this.btnOk.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnOk.ImageTintHover = System.Drawing.Color.Empty;
            this.btnOk.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnOk.ImageTintOpacity = 0.5F;
            this.btnOk.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnOk.IsCLick = false;
            this.btnOk.IsNotChange = false;
            this.btnOk.IsRect = false;
            this.btnOk.IsUnGroup = false;
            this.btnOk.Location = new System.Drawing.Point(464, 0);
            this.btnOk.Multiline = false;
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(182, 59);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.TextColor = System.Drawing.Color.Black;
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoFont = true;
            this.btnCancel.AutoFontHeightRatio = 0.75F;
            this.btnCancel.AutoFontMax = 100F;
            this.btnCancel.AutoFontMin = 6F;
            this.btnCancel.AutoFontWidthRatio = 0.92F;
            this.btnCancel.AutoImage = true;
            this.btnCancel.AutoImageMaxRatio = 0.75F;
            this.btnCancel.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCancel.AutoImageTint = true;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.btnCancel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.BorderRadius = 5;
            this.btnCancel.BorderSize = 1;
            this.btnCancel.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnCancel.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnCancel.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnCancel.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCancel.Corner = BeeGlobal.Corner.Both;
            this.btnCancel.DebounceResizeMs = 16;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.42188F);
            this.btnCancel.ForeColor = System.Drawing.Color.DimGray;
            this.btnCancel.Image = null;
            this.btnCancel.ImageDisabled = null;
            this.btnCancel.ImageHover = null;
            this.btnCancel.ImageNormal = null;
            this.btnCancel.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCancel.ImagePressed = null;
            this.btnCancel.ImageTextSpacing = 6;
            this.btnCancel.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCancel.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCancel.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCancel.ImageTintOpacity = 0.5F;
            this.btnCancel.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCancel.IsCLick = false;
            this.btnCancel.IsNotChange = false;
            this.btnCancel.IsRect = false;
            this.btnCancel.IsUnGroup = false;
            this.btnCancel.Location = new System.Drawing.Point(646, 0);
            this.btnCancel.Multiline = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 59);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextColor = System.Drawing.Color.DimGray;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ToolPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pContent);
            this.Controls.Add(this.tabTool);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "ToolPage";
            this.Size = new System.Drawing.Size(796, 616);
            this.Load += new System.EventHandler(this.ToolPage_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabTool.ResumeLayout(false);
            this.pContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.img)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private RJButton btnCancel;
        private RJButton btnOk;
        private System.Windows.Forms.TabControl tabTool;
        private System.Windows.Forms.TabPage Basic_Tool;
        private System.Windows.Forms.TabPage Extra_Tool_1;
        private System.Windows.Forms.TabPage Extra_Tool_2;
        private System.Windows.Forms.Panel pContent;
        private System.Windows.Forms.Label lbContent;
        private System.Windows.Forms.PictureBox img;
        private System.Windows.Forms.Label lbName;
    }
}
