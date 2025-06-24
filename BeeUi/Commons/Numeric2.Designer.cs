
namespace BeeUi.Commons
{
    partial class Numeric2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Numeric2));
            this.txt = new System.Windows.Forms.TextBox();
            this.btnSub = new BeeUi.Common.RJButton();
            this.btnPlus = new BeeUi.Common.RJButton();
            this.SuspendLayout();
            // 
            // txt
            // 
            this.txt.BackColor = System.Drawing.Color.White;
            this.txt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt.Location = new System.Drawing.Point(60, 0);
            this.txt.Multiline = true;
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(68, 39);
            this.txt.TabIndex = 13;
            this.txt.Text = "0";
            this.txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // btnSub
            // 
            this.btnSub.BackColor = System.Drawing.Color.Gray;
            this.btnSub.BackgroundColor = System.Drawing.Color.Gray;
            this.btnSub.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSub.BackgroundImage")));
            this.btnSub.BorderColor = System.Drawing.Color.Transparent;
            this.btnSub.BorderRadius = 1;
            this.btnSub.BorderSize = 0;
            this.btnSub.ButtonImage = null;
            this.btnSub.Corner = BeeCore.Corner.Both;
            this.btnSub.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSub.FlatAppearance.BorderSize = 0;
            this.btnSub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSub.ForeColor = System.Drawing.Color.White;
            this.btnSub.Image = ((System.Drawing.Image)(resources.GetObject("btnSub.Image")));
            this.btnSub.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSub.IsCLick = false;
            this.btnSub.IsNotChange = false;
            this.btnSub.IsRect = false;
            this.btnSub.IsUnGroup = false;
            this.btnSub.Location = new System.Drawing.Point(5, 0);
            this.btnSub.Name = "btnSub";
            this.btnSub.Size = new System.Drawing.Size(55, 39);
            this.btnSub.TabIndex = 11;
            this.btnSub.TextColor = System.Drawing.Color.White;
            this.btnSub.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSub.UseVisualStyleBackColor = false;
            this.btnSub.Click += new System.EventHandler(this.btnSub_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.BackColor = System.Drawing.Color.Transparent;
            this.btnPlus.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnPlus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPlus.BackgroundImage")));
            this.btnPlus.BorderColor = System.Drawing.Color.Transparent;
            this.btnPlus.BorderRadius = 1;
            this.btnPlus.BorderSize = 0;
            this.btnPlus.ButtonImage = null;
            this.btnPlus.Corner = BeeCore.Corner.Both;
            this.btnPlus.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPlus.FlatAppearance.BorderSize = 0;
            this.btnPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlus.ForeColor = System.Drawing.Color.Black;
            this.btnPlus.Image = ((System.Drawing.Image)(resources.GetObject("btnPlus.Image")));
            this.btnPlus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPlus.IsCLick = false;
            this.btnPlus.IsNotChange = false;
            this.btnPlus.IsRect = false;
            this.btnPlus.IsUnGroup = false;
            this.btnPlus.Location = new System.Drawing.Point(128, 0);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Padding = new System.Windows.Forms.Padding(10);
            this.btnPlus.Size = new System.Drawing.Size(63, 39);
            this.btnPlus.TabIndex = 12;
            this.btnPlus.TextColor = System.Drawing.Color.Black;
            this.btnPlus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPlus.UseVisualStyleBackColor = false;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // Numeric2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txt);
            this.Controls.Add(this.btnSub);
            this.Controls.Add(this.btnPlus);
            this.Name = "Numeric2";
            this.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Size = new System.Drawing.Size(196, 39);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt;
        private Common.RJButton btnSub;
        private Common.RJButton btnPlus;
    }
}
