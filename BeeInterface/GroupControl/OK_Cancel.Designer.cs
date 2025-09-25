using System.Windows.Forms;

namespace BeeInterface.GroupControl
{
    partial class OK_Cancel
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
            this.layout = new System.Windows.Forms.TableLayoutPanel();
            this.btnOK = new BeeInterface.RJButton();
            this.btnCancel = new BeeInterface.RJButton();
            this.layout.SuspendLayout();
            this.SuspendLayout();
            // 
            // layout
            // 
            this.layout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.layout.ColumnCount = 2;
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.layout.Controls.Add(this.btnOK, 0, 0);
            this.layout.Controls.Add(this.btnCancel, 1, 0);
            this.layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layout.Location = new System.Drawing.Point(0, 0);
            this.layout.Margin = new System.Windows.Forms.Padding(5);
            this.layout.Name = "layout";
            this.layout.RowCount = 1;
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layout.Size = new System.Drawing.Size(269, 52);
            this.layout.TabIndex = 46;
            // 
            // btnOK
            // 
            this.btnOK.AutoFont = true;
            this.btnOK.AutoFontHeightRatio = 0.75F;
            this.btnOK.AutoFontMax = 100F;
            this.btnOK.AutoFontMin = 6F;
            this.btnOK.AutoFontWidthRatio = 0.92F;
            this.btnOK.AutoImage = true;
            this.btnOK.AutoImageMaxRatio = 0.75F;
            this.btnOK.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnOK.AutoImageTint = true;
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnOK.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnOK.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnOK.BorderRadius = 8;
            this.btnOK.BorderSize = 1;
            this.btnOK.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnOK.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnOK.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnOK.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnOK.Corner = BeeGlobal.Corner.Left;
            this.btnOK.DebounceResizeMs = 16;
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.54688F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.Black;
            this.btnOK.Image = null;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.ImageDisabled = null;
            this.btnOK.ImageHover = null;
            this.btnOK.ImageNormal = null;
            this.btnOK.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnOK.ImagePressed = null;
            this.btnOK.ImageTextSpacing = 6;
            this.btnOK.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnOK.ImageTintHover = System.Drawing.Color.Empty;
            this.btnOK.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnOK.ImageTintOpacity = 0.5F;
            this.btnOK.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnOK.IsCLick = true;
            this.btnOK.IsNotChange = true;
            this.btnOK.IsRect = false;
            this.btnOK.IsUnGroup = false;
            this.btnOK.Location = new System.Drawing.Point(3, 3);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnOK.Multiline = false;
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(158, 46);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.TextColor = System.Drawing.Color.Black;
            this.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
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
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCancel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnCancel.BorderRadius = 8;
            this.btnCancel.BorderSize = 1;
            this.btnCancel.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnCancel.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnCancel.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnCancel.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCancel.Corner = BeeGlobal.Corner.Right;
            this.btnCancel.DebounceResizeMs = 16;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.54688F);
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Image = null;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.btnCancel.IsNotChange = true;
            this.btnCancel.IsRect = false;
            this.btnCancel.IsUnGroup = false;
            this.btnCancel.Location = new System.Drawing.Point(161, 3);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnCancel.Multiline = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 46);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextColor = System.Drawing.Color.Black;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // OK_Cancel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layout);
            this.DoubleBuffered = true;
            this.Name = "OK_Cancel";
            this.Size = new System.Drawing.Size(269, 52);
            this.layout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private  System.Windows.Forms.TableLayoutPanel layout;
        private RJButton btnOK;
        private RJButton btnCancel;
    }
}
