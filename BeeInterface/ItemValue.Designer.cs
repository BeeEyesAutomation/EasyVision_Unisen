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
            this.cbTool = new System.Windows.Forms.ComboBox();
            this.lbValue = new BeeInterface.AutoFontLabel();
            this.autoFontLabel4 = new BeeInterface.AutoFontLabel();
            this.autoFontLabel3 = new BeeInterface.AutoFontLabel();
            this.autoFontLabel2 = new BeeInterface.AutoFontLabel();
            this.autoFontLabel1 = new BeeInterface.AutoFontLabel();
            this.cbTypeResults = new System.Windows.Forms.ComboBox();
            this.txtAdd = new BeeInterface.TextBoxAuto();
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
            this.tableLayoutPanel1.Controls.Add(this.cbTool, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbValue, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.autoFontLabel4, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.autoFontLabel3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.autoFontLabel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.autoFontLabel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbTypeResults, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtAdd, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(420, 45);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // cbTool
            // 
            this.cbTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbTool.ForeColor = System.Drawing.Color.Blue;
            this.cbTool.FormattingEnabled = true;
            this.cbTool.Location = new System.Drawing.Point(108, 19);
            this.cbTool.Name = "cbTool";
            this.cbTool.Size = new System.Drawing.Size(99, 21);
            this.cbTool.TabIndex = 20;
            // 
            // lbValue
            // 
            this.lbValue.AutoFont = false;
            this.lbValue.AutoSize = true;
            this.lbValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.47559F);
            this.lbValue.Location = new System.Drawing.Point(318, 16);
            this.lbValue.Name = "lbValue";
            this.lbValue.Size = new System.Drawing.Size(99, 29);
            this.lbValue.TabIndex = 18;
            this.lbValue.Text = "---";
            this.lbValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoFontLabel4
            // 
            this.autoFontLabel4.AutoFont = true;
            this.autoFontLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoFontLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.467773F);
            this.autoFontLabel4.Location = new System.Drawing.Point(318, 0);
            this.autoFontLabel4.Name = "autoFontLabel4";
            this.autoFontLabel4.Size = new System.Drawing.Size(99, 16);
            this.autoFontLabel4.TabIndex = 17;
            this.autoFontLabel4.Text = "Value";
            this.autoFontLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoFontLabel3
            // 
            this.autoFontLabel3.AutoFont = true;
            this.autoFontLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoFontLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.467773F);
            this.autoFontLabel3.Location = new System.Drawing.Point(213, 0);
            this.autoFontLabel3.Name = "autoFontLabel3";
            this.autoFontLabel3.Size = new System.Drawing.Size(99, 16);
            this.autoFontLabel3.TabIndex = 16;
            this.autoFontLabel3.Text = "Address";
            this.autoFontLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoFontLabel2
            // 
            this.autoFontLabel2.AutoFont = true;
            this.autoFontLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoFontLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.467773F);
            this.autoFontLabel2.Location = new System.Drawing.Point(108, 0);
            this.autoFontLabel2.Name = "autoFontLabel2";
            this.autoFontLabel2.Size = new System.Drawing.Size(99, 16);
            this.autoFontLabel2.TabIndex = 15;
            this.autoFontLabel2.Text = "Tools";
            this.autoFontLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoFontLabel1
            // 
            this.autoFontLabel1.AutoFont = true;
            this.autoFontLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoFontLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.467773F);
            this.autoFontLabel1.Location = new System.Drawing.Point(3, 0);
            this.autoFontLabel1.Name = "autoFontLabel1";
            this.autoFontLabel1.Size = new System.Drawing.Size(99, 16);
            this.autoFontLabel1.TabIndex = 14;
            this.autoFontLabel1.Text = "Type";
            this.autoFontLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbTypeResults
            // 
            this.cbTypeResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbTypeResults.ForeColor = System.Drawing.Color.Blue;
            this.cbTypeResults.FormattingEnabled = true;
            this.cbTypeResults.Location = new System.Drawing.Point(3, 19);
            this.cbTypeResults.Name = "cbTypeResults";
            this.cbTypeResults.Size = new System.Drawing.Size(99, 21);
            this.cbTypeResults.TabIndex = 13;
            // 
            // txtAdd
            // 
            this.txtAdd.AutoFont = true;
            this.txtAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.507813F);
            this.txtAdd.Location = new System.Drawing.Point(213, 19);
            this.txtAdd.Name = "txtAdd";
            this.txtAdd.Size = new System.Drawing.Size(99, 23);
            this.txtAdd.TabIndex = 19;
            // 
            // ItemValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ItemValue";
            this.Size = new System.Drawing.Size(420, 45);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AutoFontLabel autoFontLabel2;
        private AutoFontLabel autoFontLabel1;
        private AutoFontLabel autoFontLabel4;
        private AutoFontLabel autoFontLabel3;
        public System.Windows.Forms.ComboBox cbTypeResults;
        public AutoFontLabel lbValue;
        public System.Windows.Forms.ComboBox cbTool;
        public TextBoxAuto txtAdd;
    }
}
