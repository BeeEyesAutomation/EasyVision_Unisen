
namespace BeeUi.Tool
{
    partial class ucReport
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbNG = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbProcess = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.process = new System.Windows.Forms.ProgressBar();
            this.btnReport = new System.Windows.Forms.Button();
            this.lbOK = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckResult = new System.Windows.Forms.CheckBox();
            this.ckRaw = new System.Windows.Forms.CheckBox();
            this.ckDetail = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ckOK = new System.Windows.Forms.CheckBox();
            this.ckNG = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.dateTime = new System.Windows.Forms.DateTimePicker();
            this.btnDate = new System.Windows.Forms.Button();
            this.btnIsModel = new System.Windows.Forms.Button();
            this.cbModel = new System.Windows.Forms.ComboBox();
            this.wExport = new System.ComponentModel.BackgroundWorker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataView = new System.Windows.Forms.DataGridView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.lbNG);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.lbProcess);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.process);
            this.panel1.Controls.Add(this.btnReport);
            this.panel1.Controls.Add(this.lbOK);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 526);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(983, 91);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // lbNG
            // 
            this.lbNG.BackColor = System.Drawing.SystemColors.GrayText;
            this.lbNG.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNG.ForeColor = System.Drawing.Color.LightCoral;
            this.lbNG.Location = new System.Drawing.Point(194, 27);
            this.lbNG.Name = "lbNG";
            this.lbNG.Size = new System.Drawing.Size(86, 34);
            this.lbNG.TabIndex = 19;
            this.lbNG.Text = "00000";
            this.lbNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(248, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 20);
            this.label3.TabIndex = 18;
            this.label3.Text = "pcs";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(211, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 20);
            this.label6.TabIndex = 17;
            this.label6.Text = "NG";
            // 
            // lbProcess
            // 
            this.lbProcess.AutoSize = true;
            this.lbProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbProcess.ForeColor = System.Drawing.Color.White;
            this.lbProcess.Location = new System.Drawing.Point(1133, 114);
            this.lbProcess.Name = "lbProcess";
            this.lbProcess.Size = new System.Drawing.Size(38, 25);
            this.lbProcess.TabIndex = 15;
            this.lbProcess.Text = "00";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(1169, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "%";
            // 
            // process
            // 
            this.process.Location = new System.Drawing.Point(7, 68);
            this.process.Name = "process";
            this.process.Size = new System.Drawing.Size(973, 20);
            this.process.TabIndex = 16;
            // 
            // btnReport
            // 
            this.btnReport.BackColor = System.Drawing.SystemColors.Control;
            this.btnReport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReport.Image = global::BeeUi.Properties.Resources.Export;
            this.btnReport.Location = new System.Drawing.Point(715, 7);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(135, 56);
            this.btnReport.TabIndex = 4;
            this.btnReport.Text = "Xuất dữ liệu";
            this.btnReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReport.UseVisualStyleBackColor = false;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // lbOK
            // 
            this.lbOK.BackColor = System.Drawing.SystemColors.GrayText;
            this.lbOK.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOK.ForeColor = System.Drawing.Color.White;
            this.lbOK.Location = new System.Drawing.Point(7, 27);
            this.lbOK.Name = "lbOK";
            this.lbOK.Size = new System.Drawing.Size(165, 34);
            this.lbOK.TabIndex = 3;
            this.lbOK.Text = "00000";
            this.lbOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbOK.Click += new System.EventHandler(this.label4_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(135, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "pcs";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(72, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "OK";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckResult);
            this.groupBox1.Controls.Add(this.ckRaw);
            this.groupBox1.Controls.Add(this.ckDetail);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(306, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(402, 56);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Cần xuất";
            // 
            // ckResult
            // 
            this.ckResult.Checked = true;
            this.ckResult.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckResult.ForeColor = System.Drawing.Color.Lime;
            this.ckResult.Location = new System.Drawing.Point(133, 21);
            this.ckResult.Name = "ckResult";
            this.ckResult.Size = new System.Drawing.Size(144, 24);
            this.ckResult.TabIndex = 13;
            this.ckResult.Text = "File Hình Kết quả";
            this.ckResult.UseVisualStyleBackColor = true;
            // 
            // ckRaw
            // 
            this.ckRaw.ForeColor = System.Drawing.Color.Blue;
            this.ckRaw.Location = new System.Drawing.Point(283, 21);
            this.ckRaw.Name = "ckRaw";
            this.ckRaw.Size = new System.Drawing.Size(120, 24);
            this.ckRaw.TabIndex = 12;
            this.ckRaw.Text = "File Hình Gốc";
            this.ckRaw.UseVisualStyleBackColor = true;
            // 
            // ckDetail
            // 
            this.ckDetail.Checked = true;
            this.ckDetail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckDetail.Location = new System.Drawing.Point(15, 21);
            this.ckDetail.Name = "ckDetail";
            this.ckDetail.Size = new System.Drawing.Size(109, 24);
            this.ckDetail.TabIndex = 10;
            this.ckDetail.Text = "File Chi tiết";
            this.ckDetail.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Gray;
            this.panel3.Controls.Add(this.ckOK);
            this.panel3.Controls.Add(this.ckNG);
            this.panel3.Controls.Add(this.btnRefresh);
            this.panel3.Controls.Add(this.btnDown);
            this.panel3.Controls.Add(this.dateTime);
            this.panel3.Controls.Add(this.btnDate);
            this.panel3.Controls.Add(this.btnIsModel);
            this.panel3.Controls.Add(this.cbModel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(983, 58);
            this.panel3.TabIndex = 2;
            // 
            // ckOK
            // 
            this.ckOK.AutoSize = true;
            this.ckOK.Checked = true;
            this.ckOK.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckOK.ForeColor = System.Drawing.Color.SpringGreen;
            this.ckOK.Location = new System.Drawing.Point(792, 3);
            this.ckOK.Name = "ckOK";
            this.ckOK.Size = new System.Drawing.Size(58, 28);
            this.ckOK.TabIndex = 3;
            this.ckOK.Text = "OK";
            this.ckOK.UseVisualStyleBackColor = true;
            this.ckOK.CheckedChanged += new System.EventHandler(this.ckOK_CheckedChanged);
            // 
            // ckNG
            // 
            this.ckNG.AutoSize = true;
            this.ckNG.Checked = true;
            this.ckNG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckNG.ForeColor = System.Drawing.Color.OrangeRed;
            this.ckNG.Location = new System.Drawing.Point(791, 27);
            this.ckNG.Name = "ckNG";
            this.ckNG.Size = new System.Drawing.Size(59, 28);
            this.ckNG.TabIndex = 3;
            this.ckNG.Text = "NG";
            this.ckNG.UseVisualStyleBackColor = true;
            this.ckNG.CheckedChanged += new System.EventHandler(this.ckNG_CheckedChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.White;
            this.btnRefresh.BackgroundImage = global::BeeUi.Properties.Resources.Refresh;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRefresh.Location = new System.Drawing.Point(895, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(70, 47);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDown
            // 
            this.btnDown.BackColor = System.Drawing.Color.White;
            this.btnDown.BackgroundImage = global::BeeUi.Properties.Resources.Down_Button;
            this.btnDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDown.Enabled = false;
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDown.ForeColor = System.Drawing.Color.White;
            this.btnDown.Location = new System.Drawing.Point(714, 6);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(52, 49);
            this.btnDown.TabIndex = 7;
            this.btnDown.UseVisualStyleBackColor = false;
            this.btnDown.Click += new System.EventHandler(this.button1_Click);
            // 
            // dateTime
            // 
            this.dateTime.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTime.Enabled = false;
            this.dateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTime.Location = new System.Drawing.Point(66, 5);
            this.dateTime.MinDate = new System.DateTime(2019, 10, 10, 0, 0, 0, 0);
            this.dateTime.Name = "dateTime";
            this.dateTime.Size = new System.Drawing.Size(234, 49);
            this.dateTime.TabIndex = 5;
            this.dateTime.ValueChanged += new System.EventHandler(this.dateTime_ValueChanged);
            // 
            // btnDate
            // 
            this.btnDate.BackColor = System.Drawing.Color.Green;
            this.btnDate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDate.ForeColor = System.Drawing.Color.White;
            this.btnDate.Location = new System.Drawing.Point(3, 5);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(60, 50);
            this.btnDate.TabIndex = 4;
            this.btnDate.Text = "Date";
            this.btnDate.UseVisualStyleBackColor = false;
            this.btnDate.Click += new System.EventHandler(this.btnDate_Click);
            // 
            // btnIsModel
            // 
            this.btnIsModel.BackColor = System.Drawing.Color.Silver;
            this.btnIsModel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnIsModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIsModel.ForeColor = System.Drawing.Color.Crimson;
            this.btnIsModel.Location = new System.Drawing.Point(306, 5);
            this.btnIsModel.Name = "btnIsModel";
            this.btnIsModel.Size = new System.Drawing.Size(70, 49);
            this.btnIsModel.TabIndex = 4;
            this.btnIsModel.Text = "Model";
            this.btnIsModel.UseVisualStyleBackColor = false;
            this.btnIsModel.Click += new System.EventHandler(this.btnIsModel_Click);
            // 
            // cbModel
            // 
            this.cbModel.Enabled = false;
            this.cbModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbModel.FormattingEnabled = true;
            this.cbModel.Location = new System.Drawing.Point(376, 5);
            this.cbModel.Name = "cbModel";
            this.cbModel.Size = new System.Drawing.Size(390, 50);
            this.cbModel.TabIndex = 1;
            this.cbModel.SelectedIndexChanged += new System.EventHandler(this.cbModel_SelectedIndexChanged);
            this.cbModel.MouseLeave += new System.EventHandler(this.cbModel_MouseLeave);
            // 
            // wExport
            // 
            this.wExport.WorkerReportsProgress = true;
            this.wExport.WorkerSupportsCancellation = true;
            this.wExport.DoWork += new System.ComponentModel.DoWorkEventHandler(this.wExport_DoWork);
            this.wExport.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.wExport_ProgressChanged);
            this.wExport.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.wExport_RunWorkerCompleted);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataView);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 58);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(983, 468);
            this.panel2.TabIndex = 3;
            // 
            // dataView
            // 
            this.dataView.AllowUserToAddRows = false;
            this.dataView.AllowUserToOrderColumns = true;
            this.dataView.AllowUserToResizeColumns = false;
            this.dataView.AllowUserToResizeRows = false;
            this.dataView.BackgroundColor = System.Drawing.Color.Beige;
            this.dataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataView.Location = new System.Drawing.Point(0, 0);
            this.dataView.Name = "dataView";
            this.dataView.RowHeadersVisible = false;
            this.dataView.RowHeadersWidth = 35;
            this.dataView.Size = new System.Drawing.Size(983, 468);
            this.dataView.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.Image = global::BeeUi.Properties.Resources.Delete;
            this.btnDelete.Location = new System.Drawing.Point(871, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(109, 56);
            this.btnDelete.TabIndex = 20;
            this.btnDelete.Text = "Xóa Dữ Liệu";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // ucReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "ucReport";
            this.Size = new System.Drawing.Size(983, 617);
            this.Load += new System.EventHandler(this.Report_Load);
            this.VisibleChanged += new System.EventHandler(this.Report_VisibleChanged);
            this.Click += new System.EventHandler(this.Report_Click);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox ckNG;
        private System.Windows.Forms.CheckBox ckOK;
        private System.Windows.Forms.ComboBox cbModel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbOK;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button btnIsModel;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.DateTimePicker dateTime;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ckResult;
        private System.Windows.Forms.CheckBox ckRaw;
        private System.Windows.Forms.CheckBox ckDetail;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lbProcess;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar process;
        private System.ComponentModel.BackgroundWorker wExport;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.DataGridView dataView;
        private System.Windows.Forms.Label lbNG;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnDelete;
    }
}
