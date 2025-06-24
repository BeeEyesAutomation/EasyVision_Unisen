
namespace BeeUi.Commons
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
            this.btnCancel = new BeeUi.Common.RJButton();
            this.btnOk = new BeeUi.Common.RJButton();
            this.tabTool = new System.Windows.Forms.TabControl();
            this.Basic_Tool = new System.Windows.Forms.TabPage();
            this.Extra_Tool_1 = new System.Windows.Forms.TabPage();
            this.Extra_Tool_2 = new System.Windows.Forms.TabPage();
            this.pContent = new System.Windows.Forms.Panel();
            this.lbContent = new System.Windows.Forms.Label();
            this.img = new System.Windows.Forms.PictureBox();
            this.lbName = new System.Windows.Forms.Label();
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
            this.label1.Location = new System.Drawing.Point(128, 14);
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
            this.panel1.Size = new System.Drawing.Size(666, 53);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 468);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(666, 59);
            this.panel2.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
            this.btnCancel.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.BorderRadius = 5;
            this.btnCancel.BorderSize = 1;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.DimGray;
            this.btnCancel.IsCLick = false;
            this.btnCancel.IsNotChange = false;
            this.btnCancel.IsRect = false;
            this.btnCancel.IsUnGroup = false;
            this.btnCancel.Location = new System.Drawing.Point(508, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 50);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextColor = System.Drawing.Color.DimGray;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOk.BackgroundImage")));
            this.btnOk.BorderColor = System.Drawing.Color.Transparent;
            this.btnOk.BorderRadius = 5;
            this.btnOk.BorderSize = 1;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.Black;
            this.btnOk.IsCLick = false;
            this.btnOk.IsNotChange = false;
            this.btnOk.IsRect = false;
            this.btnOk.IsUnGroup = false;
            this.btnOk.Location = new System.Drawing.Point(337, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(150, 50);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.TextColor = System.Drawing.Color.Black;
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tabTool
            // 
            this.tabTool.Controls.Add(this.Basic_Tool);
            this.tabTool.Controls.Add(this.Extra_Tool_1);
            this.tabTool.Controls.Add(this.Extra_Tool_2);
            this.tabTool.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabTool.Location = new System.Drawing.Point(334, 53);
            this.tabTool.Name = "tabTool";
            this.tabTool.SelectedIndex = 0;
            this.tabTool.Size = new System.Drawing.Size(332, 415);
            this.tabTool.TabIndex = 5;
            this.tabTool.SelectedIndexChanged += new System.EventHandler(this.tabTool_SelectedIndexChanged);
            // 
            // Basic_Tool
            // 
            this.Basic_Tool.Location = new System.Drawing.Point(4, 29);
            this.Basic_Tool.Name = "Basic_Tool";
            this.Basic_Tool.Padding = new System.Windows.Forms.Padding(3);
            this.Basic_Tool.Size = new System.Drawing.Size(324, 382);
            this.Basic_Tool.TabIndex = 0;
            this.Basic_Tool.Text = "Basic Tool";
            this.Basic_Tool.UseVisualStyleBackColor = true;
            // 
            // Extra_Tool_1
            // 
            this.Extra_Tool_1.Location = new System.Drawing.Point(4, 29);
            this.Extra_Tool_1.Name = "Extra_Tool_1";
            this.Extra_Tool_1.Padding = new System.Windows.Forms.Padding(3);
            this.Extra_Tool_1.Size = new System.Drawing.Size(324, 382);
            this.Extra_Tool_1.TabIndex = 1;
            this.Extra_Tool_1.Text = "Extra Tool 1";
            this.Extra_Tool_1.UseVisualStyleBackColor = true;
            // 
            // Extra_Tool_2
            // 
            this.Extra_Tool_2.Location = new System.Drawing.Point(4, 29);
            this.Extra_Tool_2.Name = "Extra_Tool_2";
            this.Extra_Tool_2.Padding = new System.Windows.Forms.Padding(3);
            this.Extra_Tool_2.Size = new System.Drawing.Size(324, 382);
            this.Extra_Tool_2.TabIndex = 2;
            this.Extra_Tool_2.Text = "Extra tool 2";
            this.Extra_Tool_2.UseVisualStyleBackColor = true;
            // 
            // pContent
            // 
            this.pContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.pContent.Controls.Add(this.lbContent);
            this.pContent.Controls.Add(this.img);
            this.pContent.Controls.Add(this.lbName);
            this.pContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pContent.Location = new System.Drawing.Point(0, 53);
            this.pContent.Name = "pContent";
            this.pContent.Size = new System.Drawing.Size(334, 415);
            this.pContent.TabIndex = 6;
            // 
            // lbContent
            // 
            this.lbContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.lbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbContent.ForeColor = System.Drawing.Color.Black;
            this.lbContent.Location = new System.Drawing.Point(0, 236);
            this.lbContent.Name = "lbContent";
            this.lbContent.Padding = new System.Windows.Forms.Padding(10);
            this.lbContent.Size = new System.Drawing.Size(334, 179);
            this.lbContent.TabIndex = 3;
            // 
            // img
            // 
            this.img.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.img.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.img.Dock = System.Windows.Forms.DockStyle.Top;
            this.img.Image = global::BeeUi.Properties.Resources.contentOutLine;
            this.img.Location = new System.Drawing.Point(0, 38);
            this.img.Name = "img";
            this.img.Size = new System.Drawing.Size(334, 198);
            this.img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.img.TabIndex = 1;
            this.img.TabStop = false;
            // 
            // lbName
            // 
            this.lbName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.ForeColor = System.Drawing.Color.White;
            this.lbName.Location = new System.Drawing.Point(0, 0);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(334, 38);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "Tool";
            this.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ToolPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pContent);
            this.Controls.Add(this.tabTool);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ToolPage";
            this.Size = new System.Drawing.Size(666, 527);
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
        private Common.RJButton btnCancel;
        private Common.RJButton btnOk;
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
