
namespace BeeUi.Commons
{
    partial class ExtractColor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractColor));
            this.btnRedo = new BeeUi.Common.RJButton();
            this.btnHide = new BeeUi.Common.RJButton();
            this.rjButton5 = new BeeUi.Common.RJButton();
            this.btnUndo = new BeeUi.Common.RJButton();
            this.btnClear = new BeeUi.Common.RJButton();
            this.btnPlus = new System.Windows.Forms.Button();
            this.btnSub = new System.Windows.Forms.Button();
            this.trackClear = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackClear)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRedo
            // 
            this.btnRedo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnRedo.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnRedo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRedo.BackgroundImage")));
            this.btnRedo.BorderColor = System.Drawing.Color.Silver;
            this.btnRedo.BorderRadius = 3;
            this.btnRedo.BorderSize = 1;
            this.btnRedo.FlatAppearance.BorderSize = 0;
            this.btnRedo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRedo.ForeColor = System.Drawing.Color.Black;
            this.btnRedo.Image = global::BeeUi.Properties.Resources.Redo;
            this.btnRedo.IsCLick = false;
            this.btnRedo.IsNotChange = true;
            this.btnRedo.IsRect = false;
            this.btnRedo.IsUnGroup = true;
            this.btnRedo.Location = new System.Drawing.Point(128, 8);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(85, 40);
            this.btnRedo.TabIndex = 31;
            this.btnRedo.Text = "Redo";
            this.btnRedo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRedo.TextColor = System.Drawing.Color.Black;
            this.btnRedo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRedo.UseVisualStyleBackColor = false;
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // btnHide
            // 
            this.btnHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHide.BackColor = System.Drawing.Color.Transparent;
            this.btnHide.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnHide.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHide.BackgroundImage")));
            this.btnHide.BorderColor = System.Drawing.Color.Transparent;
            this.btnHide.BorderRadius = 5;
            this.btnHide.BorderSize = 1;
            this.btnHide.FlatAppearance.BorderSize = 0;
            this.btnHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHide.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHide.ForeColor = System.Drawing.Color.Black;
            this.btnHide.IsCLick = false;
            this.btnHide.IsNotChange = true;
            this.btnHide.IsRect = false;
            this.btnHide.IsUnGroup = false;
            this.btnHide.Location = new System.Drawing.Point(190, 120);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(148, 40);
            this.btnHide.TabIndex = 30;
            this.btnHide.Text = "Cancel";
            this.btnHide.TextColor = System.Drawing.Color.Black;
            this.btnHide.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHide.UseVisualStyleBackColor = false;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // rjButton5
            // 
            this.rjButton5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rjButton5.BackColor = System.Drawing.Color.Transparent;
            this.rjButton5.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rjButton5.BackgroundImage")));
            this.rjButton5.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton5.BorderRadius = 5;
            this.rjButton5.BorderSize = 1;
            this.rjButton5.FlatAppearance.BorderSize = 0;
            this.rjButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton5.ForeColor = System.Drawing.Color.Black;
            this.rjButton5.IsCLick = true;
            this.rjButton5.IsNotChange = true;
            this.rjButton5.IsRect = false;
            this.rjButton5.IsUnGroup = false;
            this.rjButton5.Location = new System.Drawing.Point(16, 120);
            this.rjButton5.Name = "rjButton5";
            this.rjButton5.Size = new System.Drawing.Size(148, 40);
            this.rjButton5.TabIndex = 29;
            this.rjButton5.Text = "OK";
            this.rjButton5.TextColor = System.Drawing.Color.Black;
            this.rjButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton5.UseVisualStyleBackColor = false;
            this.rjButton5.Click += new System.EventHandler(this.rjButton5_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnUndo.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnUndo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUndo.BackgroundImage")));
            this.btnUndo.BorderColor = System.Drawing.Color.Silver;
            this.btnUndo.BorderRadius = 3;
            this.btnUndo.BorderSize = 1;
            this.btnUndo.FlatAppearance.BorderSize = 0;
            this.btnUndo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUndo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUndo.ForeColor = System.Drawing.Color.Black;
            this.btnUndo.Image = global::BeeUi.Properties.Resources.Undo_3;
            this.btnUndo.IsCLick = false;
            this.btnUndo.IsNotChange = true;
            this.btnUndo.IsRect = false;
            this.btnUndo.IsUnGroup = true;
            this.btnUndo.Location = new System.Drawing.Point(16, 8);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(85, 40);
            this.btnUndo.TabIndex = 28;
            this.btnUndo.Text = "Undo";
            this.btnUndo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUndo.TextColor = System.Drawing.Color.Black;
            this.btnUndo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUndo.UseVisualStyleBackColor = false;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnClear.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnClear.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClear.BackgroundImage")));
            this.btnClear.BorderColor = System.Drawing.Color.Silver;
            this.btnClear.BorderRadius = 3;
            this.btnClear.BorderSize = 1;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Image = global::BeeUi.Properties.Resources.BID_ICON_DELETE_E_32BIT;
            this.btnClear.IsCLick = false;
            this.btnClear.IsNotChange = false;
            this.btnClear.IsRect = false;
            this.btnClear.IsUnGroup = true;
            this.btnClear.Location = new System.Drawing.Point(240, 8);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(97, 40);
            this.btnClear.TabIndex = 27;
            this.btnClear.Text = "Clear All";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClear.TextColor = System.Drawing.Color.Black;
            this.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlus.Location = new System.Drawing.Point(309, 69);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(29, 27);
            this.btnPlus.TabIndex = 26;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // btnSub
            // 
            this.btnSub.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSub.Location = new System.Drawing.Point(35, 69);
            this.btnSub.Name = "btnSub";
            this.btnSub.Size = new System.Drawing.Size(29, 27);
            this.btnSub.TabIndex = 25;
            this.btnSub.Text = "-";
            this.btnSub.UseVisualStyleBackColor = true;
            this.btnSub.Click += new System.EventHandler(this.btnSub_Click);
            // 
            // trackClear
            // 
            this.trackClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(213)))), ((int)(((byte)(216)))));
            this.trackClear.Location = new System.Drawing.Point(70, 69);
            this.trackClear.Maximum = 50;
            this.trackClear.Name = "trackClear";
            this.trackClear.Size = new System.Drawing.Size(233, 45);
            this.trackClear.SmallChange = 5;
            this.trackClear.TabIndex = 24;
            this.trackClear.TickFrequency = 5;
            this.trackClear.Value = 15;
            this.trackClear.Scroll += new System.EventHandler(this.trackClear_Scroll);
            // 
            // ExtractColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
           // this.BackgroundImage = global::BeeUi.Properties.Resources.btnUnChoose;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.btnRedo);
            this.Controls.Add(this.btnHide);
            this.Controls.Add(this.rjButton5);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnPlus);
            this.Controls.Add(this.btnSub);
            this.Controls.Add(this.trackClear);
            this.DoubleBuffered = true;
            this.Name = "ExtractColor";
            this.Size = new System.Drawing.Size(355, 180);
            this.Load += new System.EventHandler(this.ExtractColor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackClear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Common.RJButton btnRedo;
        private Common.RJButton btnHide;
        private Common.RJButton rjButton5;
        private Common.RJButton btnUndo;
        private Common.RJButton btnClear;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Button btnSub;
        private System.Windows.Forms.TrackBar trackClear;
    }
}
