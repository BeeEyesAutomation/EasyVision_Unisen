
namespace BeeInterface
{
    partial class ItemRS
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
            this.lbNumber = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.Label();
            this.ckUnused = new System.Windows.Forms.RadioButton();
            this.ckUsed = new System.Windows.Forms.RadioButton();
            this.ckInverse = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // lbNumber
            // 
            this.lbNumber.BackColor = System.Drawing.Color.Transparent;
            this.lbNumber.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNumber.Location = new System.Drawing.Point(0, 0);
            this.lbNumber.Name = "lbNumber";
            this.lbNumber.Size = new System.Drawing.Size(32, 40);
            this.lbNumber.TabIndex = 8;
            this.lbNumber.Text = "01";
            this.lbNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // name
            // 
            this.name.BackColor = System.Drawing.Color.Transparent;
            this.name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.name.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name.Location = new System.Drawing.Point(32, 0);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(136, 40);
            this.name.TabIndex = 7;
            this.name.Text = "OutLine";
            this.name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ckUnused
            // 
            this.ckUnused.AutoSize = true;
            this.ckUnused.Dock = System.Windows.Forms.DockStyle.Right;
            this.ckUnused.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckUnused.Location = new System.Drawing.Point(168, 0);
            this.ckUnused.Name = "ckUnused";
            this.ckUnused.Size = new System.Drawing.Size(94, 40);
            this.ckUnused.TabIndex = 12;
            this.ckUnused.TabStop = true;
            this.ckUnused.Text = "Unused";
            this.ckUnused.UseVisualStyleBackColor = true;
            this.ckUnused.CheckedChanged += new System.EventHandler(this.ckUnused_CheckedChanged);
            // 
            // ckUsed
            // 
            this.ckUsed.AutoSize = true;
            this.ckUsed.Dock = System.Windows.Forms.DockStyle.Right;
            this.ckUsed.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckUsed.Location = new System.Drawing.Point(262, 0);
            this.ckUsed.Name = "ckUsed";
            this.ckUsed.Size = new System.Drawing.Size(72, 40);
            this.ckUsed.TabIndex = 13;
            this.ckUsed.TabStop = true;
            this.ckUsed.Text = "Used";
            this.ckUsed.UseVisualStyleBackColor = true;
            this.ckUsed.CheckedChanged += new System.EventHandler(this.ckUsed_CheckedChanged);
            // 
            // ckInverse
            // 
            this.ckInverse.AutoSize = true;
            this.ckInverse.Dock = System.Windows.Forms.DockStyle.Right;
            this.ckInverse.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckInverse.Location = new System.Drawing.Point(334, 0);
            this.ckInverse.Name = "ckInverse";
            this.ckInverse.Size = new System.Drawing.Size(89, 40);
            this.ckInverse.TabIndex = 14;
            this.ckInverse.TabStop = true;
            this.ckInverse.Text = "Inverse";
            this.ckInverse.UseVisualStyleBackColor = true;
            this.ckInverse.CheckedChanged += new System.EventHandler(this.ckInverse_CheckedChanged);
            // 
            // ItemRS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.name);
            this.Controls.Add(this.ckUnused);
            this.Controls.Add(this.ckUsed);
            this.Controls.Add(this.ckInverse);
            this.Controls.Add(this.lbNumber);
            this.Name = "ItemRS";
            this.Size = new System.Drawing.Size(423, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lbNumber;
        public System.Windows.Forms.Label name;
        public System.Windows.Forms.RadioButton ckUnused;
        public System.Windows.Forms.RadioButton ckUsed;
        public System.Windows.Forms.RadioButton ckInverse;
    }
}
