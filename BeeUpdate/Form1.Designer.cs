namespace BeeUpdate
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnfolder = new System.Windows.Forms.Button();
            this.lbInfor = new System.Windows.Forms.Label();
            this.btnMove = new System.Windows.Forms.Button();
            this.folder = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(217, 29);
            this.textBox1.TabIndex = 0;
            // 
            // btnfolder
            // 
            this.btnfolder.Location = new System.Drawing.Point(235, 12);
            this.btnfolder.Name = "btnfolder";
            this.btnfolder.Size = new System.Drawing.Size(75, 29);
            this.btnfolder.TabIndex = 1;
            this.btnfolder.Text = "Folder";
            this.btnfolder.UseVisualStyleBackColor = true;
            this.btnfolder.Click += new System.EventHandler(this.btnfolder_Click);
            // 
            // lbInfor
            // 
            this.lbInfor.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.lbInfor.Location = new System.Drawing.Point(12, 48);
            this.lbInfor.Name = "lbInfor";
            this.lbInfor.Size = new System.Drawing.Size(293, 196);
            this.lbInfor.TabIndex = 2;
            this.lbInfor.Text = ".";
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(230, 247);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(75, 29);
            this.btnMove.TabIndex = 3;
            this.btnMove.Text = "Move";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 247);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 29);
            this.button1.TabIndex = 4;
            this.button1.Text = "Read";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 356);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.lbInfor);
            this.Controls.Add(this.btnfolder);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnfolder;
        private System.Windows.Forms.Label lbInfor;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.FolderBrowserDialog folder;
        private System.Windows.Forms.Button button1;
    }
}

