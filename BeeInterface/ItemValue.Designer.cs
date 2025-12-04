namespace BeeInterface
{
    partial class ItemValue
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TypeBit = new System.Windows.Forms.ComboBox();
            this.textBoxAuto1 = new BeeInterface.TextBoxAuto();
            this.textBoxAuto2 = new BeeInterface.TextBoxAuto();
            this.autoFontLabel1 = new BeeInterface.AutoFontLabel();
            this.autoFontLabel2 = new BeeInterface.AutoFontLabel();
            this.autoFontLabel3 = new BeeInterface.AutoFontLabel();
            this.autoFontLabel4 = new BeeInterface.AutoFontLabel();
            this.autoFontLabel5 = new BeeInterface.AutoFontLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.autoFontLabel5, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.autoFontLabel4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.autoFontLabel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.autoFontLabel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxAuto2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.TypeBit, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxAuto1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.autoFontLabel1, 3, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(420, 38);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // TypeBit
            // 
            this.TypeBit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TypeBit.ForeColor = System.Drawing.Color.Blue;
            this.TypeBit.FormattingEnabled = true;
            this.TypeBit.Items.AddRange(new object[] {
            "Bit",
            "Int",
            "Float",
            "String"});
            this.TypeBit.Location = new System.Drawing.Point(108, 15);
            this.TypeBit.Name = "TypeBit";
            this.TypeBit.Size = new System.Drawing.Size(99, 21);
            this.TypeBit.TabIndex = 3;
            this.TypeBit.Text = "Bit";
            // 
            // textBoxAuto1
            // 
            this.textBoxAuto1.AutoFont = true;
            this.textBoxAuto1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAuto1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.507813F);
            this.textBoxAuto1.Location = new System.Drawing.Point(3, 15);
            this.textBoxAuto1.Name = "textBoxAuto1";
            this.textBoxAuto1.Size = new System.Drawing.Size(99, 20);
            this.textBoxAuto1.TabIndex = 0;
            // 
            // textBoxAuto2
            // 
            this.textBoxAuto2.AutoFont = true;
            this.textBoxAuto2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAuto2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.507813F);
            this.textBoxAuto2.Location = new System.Drawing.Point(213, 15);
            this.textBoxAuto2.Name = "textBoxAuto2";
            this.textBoxAuto2.Size = new System.Drawing.Size(99, 20);
            this.textBoxAuto2.TabIndex = 4;
            // 
            // autoFontLabel1
            // 
            this.autoFontLabel1.AutoFont = true;
            this.autoFontLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.47559F);
            this.autoFontLabel1.Location = new System.Drawing.Point(318, 12);
            this.autoFontLabel1.Name = "autoFontLabel1";
            this.autoFontLabel1.Size = new System.Drawing.Size(99, 26);
            this.autoFontLabel1.TabIndex = 5;
            this.autoFontLabel1.Text = "00";
            this.autoFontLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoFontLabel2
            // 
            this.autoFontLabel2.AutoFont = true;
            this.autoFontLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.547852F);
            this.autoFontLabel2.Location = new System.Drawing.Point(3, 0);
            this.autoFontLabel2.Name = "autoFontLabel2";
            this.autoFontLabel2.Size = new System.Drawing.Size(99, 12);
            this.autoFontLabel2.TabIndex = 6;
            this.autoFontLabel2.Text = "Name";
            this.autoFontLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoFontLabel3
            // 
            this.autoFontLabel3.AutoFont = true;
            this.autoFontLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.547852F);
            this.autoFontLabel3.Location = new System.Drawing.Point(108, 0);
            this.autoFontLabel3.Name = "autoFontLabel3";
            this.autoFontLabel3.Size = new System.Drawing.Size(99, 12);
            this.autoFontLabel3.TabIndex = 7;
            this.autoFontLabel3.Text = "Type";
            this.autoFontLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoFontLabel4
            // 
            this.autoFontLabel4.AutoFont = true;
            this.autoFontLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.547852F);
            this.autoFontLabel4.Location = new System.Drawing.Point(213, 0);
            this.autoFontLabel4.Name = "autoFontLabel4";
            this.autoFontLabel4.Size = new System.Drawing.Size(99, 12);
            this.autoFontLabel4.TabIndex = 8;
            this.autoFontLabel4.Text = "Add";
            this.autoFontLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoFontLabel5
            // 
            this.autoFontLabel5.AutoFont = true;
            this.autoFontLabel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.547852F);
            this.autoFontLabel5.Location = new System.Drawing.Point(318, 0);
            this.autoFontLabel5.Name = "autoFontLabel5";
            this.autoFontLabel5.Size = new System.Drawing.Size(99, 12);
            this.autoFontLabel5.TabIndex = 9;
            this.autoFontLabel5.Text = "Value";
            this.autoFontLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ItemValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ItemValue";
            this.Size = new System.Drawing.Size(420, 38);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox TypeBit;
        private TextBoxAuto textBoxAuto2;
        private TextBoxAuto textBoxAuto1;
        private AutoFontLabel autoFontLabel1;
        private AutoFontLabel autoFontLabel4;
        private AutoFontLabel autoFontLabel3;
        private AutoFontLabel autoFontLabel2;
        private AutoFontLabel autoFontLabel5;
    }
}
