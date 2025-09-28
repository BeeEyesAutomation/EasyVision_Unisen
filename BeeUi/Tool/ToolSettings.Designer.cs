
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
            this.split1 = new System.Windows.Forms.Splitter();
            this.pBtn = new System.Windows.Forms.TableLayoutPanel();
            this.btnRename = new BeeInterface.RJButton();
            this.btnCopy = new BeeInterface.RJButton();
            this.btnEnEdit = new BeeInterface.RJButton();
            this.btnAdd = new BeeInterface.RJButton();
            this.btnDelect = new BeeInterface.RJButton();
            this.pAllTool = new System.Windows.Forms.TableLayoutPanel();
            this.pBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // split1
            // 
            this.split1.Dock = System.Windows.Forms.DockStyle.Top;
            this.split1.Location = new System.Drawing.Point(2, 56);
            this.split1.Name = "split1";
            this.split1.Size = new System.Drawing.Size(396, 3);
            this.split1.TabIndex = 11;
            this.split1.TabStop = false;
            // 
            // pBtn
            // 
            this.pBtn.BackColor = System.Drawing.Color.Silver;
            this.pBtn.ColumnCount = 5;
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pBtn.Controls.Add(this.btnRename, 2, 0);
            this.pBtn.Controls.Add(this.btnCopy, 3, 0);
            this.pBtn.Controls.Add(this.btnEnEdit, 1, 0);
            this.pBtn.Controls.Add(this.btnAdd, 0, 0);
            this.pBtn.Controls.Add(this.btnDelect, 4, 0);
            this.pBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.pBtn.Location = new System.Drawing.Point(2, 2);
            this.pBtn.Name = "pBtn";
            this.pBtn.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.pBtn.RowCount = 1;
            this.pBtn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pBtn.Size = new System.Drawing.Size(396, 54);
            this.pBtn.TabIndex = 10;
            this.pBtn.SizeChanged += new System.EventHandler(this.pBtn_SizeChanged);
            // 
            // btnRename
            // 
            this.btnRename.AutoFont = true;
            this.btnRename.AutoFontHeightRatio = 0.6F;
            this.btnRename.AutoFontMax = 100F;
            this.btnRename.AutoFontMin = 8F;
            this.btnRename.AutoFontWidthRatio = 0.92F;
            this.btnRename.AutoImage = true;
            this.btnRename.AutoImageMaxRatio = 0.65F;
            this.btnRename.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnRename.AutoImageTint = true;
            this.btnRename.BackColor = System.Drawing.Color.Silver;
            this.btnRename.BackgroundColor = System.Drawing.Color.Silver;
            this.btnRename.BorderColor = System.Drawing.Color.Silver;
            this.btnRename.BorderRadius = 10;
            this.btnRename.BorderSize = 1;
            this.btnRename.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnRename.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnRename.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnRename.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnRename.Corner = BeeGlobal.Corner.Both;
            this.btnRename.DebounceResizeMs = 16;
            this.btnRename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRename.Enabled = false;
            this.btnRename.FlatAppearance.BorderSize = 0;
            this.btnRename.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRename.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnRename.ForeColor = System.Drawing.Color.Black;
            this.btnRename.Image = global::BeeUi.Properties.Resources.Rename;
            this.btnRename.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRename.ImageDisabled = null;
            this.btnRename.ImageHover = null;
            this.btnRename.ImageNormal = null;
            this.btnRename.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnRename.ImagePressed = null;
            this.btnRename.ImageTextSpacing = 2;
            this.btnRename.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnRename.ImageTintHover = System.Drawing.Color.Empty;
            this.btnRename.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnRename.ImageTintOpacity = 0.5F;
            this.btnRename.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnRename.IsCLick = false;
            this.btnRename.IsNotChange = false;
            this.btnRename.IsRect = false;
            this.btnRename.IsUnGroup = true;
            this.btnRename.Location = new System.Drawing.Point(161, 5);
            this.btnRename.Multiline = false;
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(73, 44);
            this.btnRename.TabIndex = 11;
            this.btnRename.Text = "Rename";
            this.btnRename.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRename.TextColor = System.Drawing.Color.Black;
            this.btnRename.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRename.UseVisualStyleBackColor = false;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.AutoFont = true;
            this.btnCopy.AutoFontHeightRatio = 0.6F;
            this.btnCopy.AutoFontMax = 100F;
            this.btnCopy.AutoFontMin = 8F;
            this.btnCopy.AutoFontWidthRatio = 0.92F;
            this.btnCopy.AutoImage = true;
            this.btnCopy.AutoImageMaxRatio = 0.65F;
            this.btnCopy.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCopy.AutoImageTint = true;
            this.btnCopy.BackColor = System.Drawing.Color.Silver;
            this.btnCopy.BackgroundColor = System.Drawing.Color.Silver;
            this.btnCopy.BorderColor = System.Drawing.Color.Silver;
            this.btnCopy.BorderRadius = 10;
            this.btnCopy.BorderSize = 1;
            this.btnCopy.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnCopy.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnCopy.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnCopy.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCopy.Corner = BeeGlobal.Corner.Both;
            this.btnCopy.DebounceResizeMs = 16;
            this.btnCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCopy.Enabled = false;
            this.btnCopy.FlatAppearance.BorderSize = 0;
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnCopy.ForeColor = System.Drawing.Color.Black;
            this.btnCopy.Image = global::BeeUi.Properties.Resources.BID_ICON_COPY_D_32BIT;
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCopy.ImageDisabled = null;
            this.btnCopy.ImageHover = null;
            this.btnCopy.ImageNormal = null;
            this.btnCopy.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCopy.ImagePressed = null;
            this.btnCopy.ImageTextSpacing = 2;
            this.btnCopy.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCopy.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCopy.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCopy.ImageTintOpacity = 0.5F;
            this.btnCopy.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCopy.IsCLick = false;
            this.btnCopy.IsNotChange = true;
            this.btnCopy.IsRect = false;
            this.btnCopy.IsUnGroup = true;
            this.btnCopy.Location = new System.Drawing.Point(240, 5);
            this.btnCopy.Multiline = false;
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(73, 44);
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
            this.btnEnEdit.AutoFont = true;
            this.btnEnEdit.AutoFontHeightRatio = 0.6F;
            this.btnEnEdit.AutoFontMax = 100F;
            this.btnEnEdit.AutoFontMin = 8F;
            this.btnEnEdit.AutoFontWidthRatio = 0.92F;
            this.btnEnEdit.AutoImage = true;
            this.btnEnEdit.AutoImageMaxRatio = 0.65F;
            this.btnEnEdit.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEnEdit.AutoImageTint = true;
            this.btnEnEdit.BackColor = System.Drawing.Color.Silver;
            this.btnEnEdit.BackgroundColor = System.Drawing.Color.Silver;
            this.btnEnEdit.BorderColor = System.Drawing.Color.Silver;
            this.btnEnEdit.BorderRadius = 10;
            this.btnEnEdit.BorderSize = 1;
            this.btnEnEdit.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnEnEdit.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnEnEdit.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnEnEdit.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEnEdit.Corner = BeeGlobal.Corner.Both;
            this.btnEnEdit.DebounceResizeMs = 16;
            this.btnEnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnEdit.FlatAppearance.BorderSize = 0;
            this.btnEnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnEnEdit.ForeColor = System.Drawing.Color.Black;
            this.btnEnEdit.Image = global::BeeUi.Properties.Resources.BID_ICON_TOOL_EDIT_E_32BIT;
            this.btnEnEdit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEnEdit.ImageDisabled = null;
            this.btnEnEdit.ImageHover = null;
            this.btnEnEdit.ImageNormal = null;
            this.btnEnEdit.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEnEdit.ImagePressed = null;
            this.btnEnEdit.ImageTextSpacing = 2;
            this.btnEnEdit.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEnEdit.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEnEdit.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEnEdit.ImageTintOpacity = 0.5F;
            this.btnEnEdit.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEnEdit.IsCLick = false;
            this.btnEnEdit.IsNotChange = false;
            this.btnEnEdit.IsRect = false;
            this.btnEnEdit.IsUnGroup = true;
            this.btnEnEdit.Location = new System.Drawing.Point(82, 5);
            this.btnEnEdit.Multiline = false;
            this.btnEnEdit.Name = "btnEnEdit";
            this.btnEnEdit.Size = new System.Drawing.Size(73, 44);
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
            this.btnAdd.AutoFont = true;
            this.btnAdd.AutoFontHeightRatio = 0.6F;
            this.btnAdd.AutoFontMax = 100F;
            this.btnAdd.AutoFontMin = 8F;
            this.btnAdd.AutoFontWidthRatio = 0.92F;
            this.btnAdd.AutoImage = true;
            this.btnAdd.AutoImageMaxRatio = 0.65F;
            this.btnAdd.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnAdd.AutoImageTint = true;
            this.btnAdd.BackColor = System.Drawing.Color.Silver;
            this.btnAdd.BackgroundColor = System.Drawing.Color.Silver;
            this.btnAdd.BorderColor = System.Drawing.Color.Silver;
            this.btnAdd.BorderRadius = 10;
            this.btnAdd.BorderSize = 1;
            this.btnAdd.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnAdd.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnAdd.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnAdd.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnAdd.Corner = BeeGlobal.Corner.Both;
            this.btnAdd.DebounceResizeMs = 16;
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.Enabled = false;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnAdd.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Image = global::BeeUi.Properties.Resources.Add;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAdd.ImageDisabled = null;
            this.btnAdd.ImageHover = null;
            this.btnAdd.ImageNormal = null;
            this.btnAdd.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnAdd.ImagePressed = null;
            this.btnAdd.ImageTextSpacing = 2;
            this.btnAdd.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnAdd.ImageTintHover = System.Drawing.Color.Empty;
            this.btnAdd.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnAdd.ImageTintOpacity = 0.5F;
            this.btnAdd.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnAdd.IsCLick = false;
            this.btnAdd.IsNotChange = true;
            this.btnAdd.IsRect = false;
            this.btnAdd.IsUnGroup = true;
            this.btnAdd.Location = new System.Drawing.Point(3, 5);
            this.btnAdd.Multiline = false;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(73, 44);
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
            this.btnDelect.AutoFont = true;
            this.btnDelect.AutoFontHeightRatio = 0.75F;
            this.btnDelect.AutoFontMax = 100F;
            this.btnDelect.AutoFontMin = 8F;
            this.btnDelect.AutoFontWidthRatio = 0.92F;
            this.btnDelect.AutoImage = true;
            this.btnDelect.AutoImageMaxRatio = 0.65F;
            this.btnDelect.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnDelect.AutoImageTint = true;
            this.btnDelect.BackColor = System.Drawing.Color.Silver;
            this.btnDelect.BackgroundColor = System.Drawing.Color.Silver;
            this.btnDelect.BorderColor = System.Drawing.Color.Silver;
            this.btnDelect.BorderRadius = 10;
            this.btnDelect.BorderSize = 1;
            this.btnDelect.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnDelect.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnDelect.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnDelect.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnDelect.Corner = BeeGlobal.Corner.Both;
            this.btnDelect.DebounceResizeMs = 16;
            this.btnDelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelect.Enabled = false;
            this.btnDelect.FlatAppearance.BorderSize = 0;
            this.btnDelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnDelect.ForeColor = System.Drawing.Color.Black;
            this.btnDelect.Image = global::BeeUi.Properties.Resources.BID_ICON_DELETE_E_32BIT;
            this.btnDelect.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDelect.ImageDisabled = null;
            this.btnDelect.ImageHover = null;
            this.btnDelect.ImageNormal = null;
            this.btnDelect.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnDelect.ImagePressed = null;
            this.btnDelect.ImageTextSpacing = 2;
            this.btnDelect.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnDelect.ImageTintHover = System.Drawing.Color.Empty;
            this.btnDelect.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnDelect.ImageTintOpacity = 0.5F;
            this.btnDelect.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnDelect.IsCLick = false;
            this.btnDelect.IsNotChange = true;
            this.btnDelect.IsRect = false;
            this.btnDelect.IsUnGroup = true;
            this.btnDelect.Location = new System.Drawing.Point(319, 5);
            this.btnDelect.Multiline = false;
            this.btnDelect.Name = "btnDelect";
            this.btnDelect.Size = new System.Drawing.Size(74, 44);
            this.btnDelect.TabIndex = 10;
            this.btnDelect.Text = "Delete";
            this.btnDelect.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDelect.TextColor = System.Drawing.Color.Black;
            this.btnDelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDelect.UseVisualStyleBackColor = false;
            this.btnDelect.Click += new System.EventHandler(this.btnDelect_Click);
            // 
            // pAllTool
            // 
            this.pAllTool.AutoScroll = true;
            this.pAllTool.ColumnCount = 1;
            this.pAllTool.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pAllTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pAllTool.Location = new System.Drawing.Point(2, 59);
            this.pAllTool.Name = "pAllTool";
            this.pAllTool.RowCount = 2;
            this.pAllTool.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pAllTool.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pAllTool.Size = new System.Drawing.Size(396, 464);
            this.pAllTool.TabIndex = 12;
            // 
            // ToolSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.pAllTool);
            this.Controls.Add(this.split1);
            this.Controls.Add(this.pBtn);
            this.DoubleBuffered = true;
            this.Name = "ToolSettings";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(400, 525);
            this.Load += new System.EventHandler(this.ToolSettings_Load);
            this.SizeChanged += new System.EventHandler(this.ToolSettings_SizeChanged);
            this.pBtn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public RJButton btnAdd;
        public RJButton btnDelect;
        public RJButton btnCopy;
        public RJButton btnEnEdit;
        public  System.Windows.Forms.TableLayoutPanel pBtn;
        public RJButton btnRename;
        public System.Windows.Forms.TableLayoutPanel pAllTool;
        public System.Windows.Forms.Splitter split1;
    }
}
