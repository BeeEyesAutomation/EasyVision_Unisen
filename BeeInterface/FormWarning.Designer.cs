
namespace BeeInterface
{
    partial class FormWarning
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWarning));
            this.lbCode = new System.Windows.Forms.Label();
            this.lbErr = new System.Windows.Forms.Label();
            this.lbContent = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbCode
            // 
            this.lbCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCode.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lbCode.Location = new System.Drawing.Point(10, 5);
            this.lbCode.Name = "lbCode";
            this.lbCode.Size = new System.Drawing.Size(373, 36);
            this.lbCode.TabIndex = 0;
            this.lbCode.Text = "Message Box";
            this.lbCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbErr
            // 
            this.lbErr.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbErr.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbErr.ForeColor = System.Drawing.Color.Red;
            this.lbErr.Location = new System.Drawing.Point(10, 54);
            this.lbErr.Name = "lbErr";
            this.lbErr.Size = new System.Drawing.Size(373, 54);
            this.lbErr.TabIndex = 1;
            this.lbErr.Text = "Image Master";
            this.lbErr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbContent
            // 
            this.lbContent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbContent.Location = new System.Drawing.Point(10, 108);
            this.lbContent.Name = "lbContent";
            this.lbContent.Size = new System.Drawing.Size(373, 63);
            this.lbContent.TabIndex = 2;
            this.lbContent.Text = "Emtry!!";
            this.lbContent.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(373, 51);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "OK";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(10, 171);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(373, 51);
            this.panel1.TabIndex = 5;
            // 
            // FormWarning
            // 
            this.ClientSize = new System.Drawing.Size(393, 227);
            this.Controls.Add(this.lbErr);
            this.Controls.Add(this.lbContent);
            this.Controls.Add(this.lbCode);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormWarning";
            this.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.Load += new System.EventHandler(this.ForrmAlarm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        public System.Windows.Forms.Label lbErr;
        public System.Windows.Forms.Label lbContent;
        public System.Windows.Forms.Label lbCode;
        public System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
    }
}