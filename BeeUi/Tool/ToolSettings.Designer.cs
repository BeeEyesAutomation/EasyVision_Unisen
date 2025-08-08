
using BeeCore;
using BeeGlobal;
using BeeInterface;

namespace BeeUi.Tool
{
    partial class ToolSettings
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
            this.pAllTool = new System.Windows.Forms.Panel();
            this.pBtn = new System.Windows.Forms.TableLayoutPanel();
            this.btnCopy = new RJButton();
            this.btnEnEdit = new RJButton();
            this.btnAdd = new RJButton();
            this.btnDelect = new RJButton();
            this.pBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // pAllTool
            // 
            this.pAllTool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pAllTool.AutoScroll = true;
            this.pAllTool.BackColor = System.Drawing.Color.White;
            this.pAllTool.Location = new System.Drawing.Point(2, 70);
            this.pAllTool.Name = "pAllTool";
            this.pAllTool.Size = new System.Drawing.Size(396, 453);
            this.pAllTool.TabIndex = 9;
            // 
            // pBtn
            // 
            this.pBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.pBtn.ColumnCount = 4;
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pBtn.Controls.Add(this.btnCopy, 2, 0);
            this.pBtn.Controls.Add(this.btnEnEdit, 1, 0);
            this.pBtn.Controls.Add(this.btnAdd, 0, 0);
            this.pBtn.Controls.Add(this.btnDelect, 3, 0);
            this.pBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.pBtn.Location = new System.Drawing.Point(2, 2);
            this.pBtn.Name = "pBtn";
            this.pBtn.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.pBtn.RowCount = 1;
            this.pBtn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pBtn.Size = new System.Drawing.Size(396, 68);
            this.pBtn.TabIndex = 10;
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCopy.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCopy.BorderColor = System.Drawing.Color.Transparent;
            this.btnCopy.BorderRadius = 10;
            this.btnCopy.BorderSize = 1;
            this.btnCopy.Image = null;
            this.btnCopy.Corner =Corner.Both;
            this.btnCopy.Enabled = false;
            this.btnCopy.FlatAppearance.BorderSize = 0;
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopy.ForeColor = System.Drawing.Color.Black;
            this.btnCopy.Image = global::BeeUi.Properties.Resources.BID_ICON_COPY_D_32BIT;
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCopy.IsCLick = false;
            this.btnCopy.IsNotChange = true;
            this.btnCopy.IsRect = false;
            this.btnCopy.IsUnGroup = true;
            this.btnCopy.Location = new System.Drawing.Point(205, 7);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(5);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(89, 54);
            this.btnCopy.TabIndex = 9;
            this.btnCopy.Text = "Copy";
            this.btnCopy.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCopy.TextColor = System.Drawing.Color.Black;
            this.btnCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnEnEdit
            // 
            this.btnEnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnEnEdit.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnEnEdit.BorderColor = System.Drawing.Color.Transparent;
            this.btnEnEdit.BorderRadius = 10;
            this.btnEnEdit.BorderSize = 1;
            this.btnEnEdit.Image = null;
            this.btnEnEdit.Corner =Corner.Both;
            this.btnEnEdit.FlatAppearance.BorderSize = 0;
            this.btnEnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnEdit.ForeColor = System.Drawing.Color.Black;
            this.btnEnEdit.Image = global::BeeUi.Properties.Resources.BID_ICON_TOOL_EDIT_E_32BIT;
            this.btnEnEdit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEnEdit.IsCLick = false;
            this.btnEnEdit.IsNotChange = false;
            this.btnEnEdit.IsRect = false;
            this.btnEnEdit.IsUnGroup = true;
            this.btnEnEdit.Location = new System.Drawing.Point(108, 7);
            this.btnEnEdit.Margin = new System.Windows.Forms.Padding(5);
            this.btnEnEdit.Name = "btnEnEdit";
            this.btnEnEdit.Size = new System.Drawing.Size(87, 54);
            this.btnEnEdit.TabIndex = 8;
            this.btnEnEdit.Text = "Edit";
            this.btnEnEdit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEnEdit.TextColor = System.Drawing.Color.Black;
            this.btnEnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEnEdit.UseVisualStyleBackColor = false;
            this.btnEnEdit.Click += new System.EventHandler(this.btnEnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnAdd.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnAdd.BorderColor = System.Drawing.Color.Transparent;
            this.btnAdd.BorderRadius = 10;
            this.btnAdd.BorderSize = 1;
            this.btnAdd.Image = null;
            this.btnAdd.Corner =Corner.Both;
            this.btnAdd.Enabled = false;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Image = global::BeeUi.Properties.Resources.Add;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAdd.IsCLick = false;
            this.btnAdd.IsNotChange = true;
            this.btnAdd.IsRect = false;
            this.btnAdd.IsUnGroup = true;
            this.btnAdd.Location = new System.Drawing.Point(5, 7);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(93, 54);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAdd.TextColor = System.Drawing.Color.Black;
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelect
            // 
            this.btnDelect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnDelect.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnDelect.BorderColor = System.Drawing.Color.Transparent;
            this.btnDelect.BorderRadius = 10;
            this.btnDelect.BorderSize = 1;
            this.btnDelect.Image = null;
            this.btnDelect.Corner =Corner.Both;
            this.btnDelect.Enabled = false;
            this.btnDelect.FlatAppearance.BorderSize = 0;
            this.btnDelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelect.ForeColor = System.Drawing.Color.Black;
            this.btnDelect.Image = global::BeeUi.Properties.Resources.BID_ICON_DELETE_E_32BIT;
            this.btnDelect.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDelect.IsCLick = false;
            this.btnDelect.IsNotChange = true;
            this.btnDelect.IsRect = false;
            this.btnDelect.IsUnGroup = true;
            this.btnDelect.Location = new System.Drawing.Point(304, 7);
            this.btnDelect.Margin = new System.Windows.Forms.Padding(5);
            this.btnDelect.Name = "btnDelect";
            this.btnDelect.Size = new System.Drawing.Size(87, 54);
            this.btnDelect.TabIndex = 10;
            this.btnDelect.Text = "Delete";
            this.btnDelect.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDelect.TextColor = System.Drawing.Color.Black;
            this.btnDelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDelect.UseVisualStyleBackColor = false;
            this.btnDelect.Click += new System.EventHandler(this.btnDelect_Click);
            // 
            // ToolSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.pAllTool);
            this.Controls.Add(this.pBtn);
            this.Name = "ToolSettings";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(400, 525);
            this.Load += new System.EventHandler(this.ToolSettings_Load);
            this.SizeChanged += new System.EventHandler(this.ToolSettings_SizeChanged);
            this.pBtn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Panel pAllTool;
        public RJButton btnAdd;
        public RJButton btnDelect;
        public RJButton btnCopy;
        public RJButton btnEnEdit;
        public System.Windows.Forms.TableLayoutPanel pBtn;
    }
}
