using BeeCore;
using BeeGlobal;
using BeeInterface;

namespace BeeUi.Unit
{
    partial class ResultBar
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
            this.pStatus = new System.Windows.Forms.TableLayoutPanel();
            this.pInforTotal = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.lbPercent = new System.Windows.Forms.Label();
            this.lbTotalTime = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbCycleTrigger = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.pInfor = new System.Windows.Forms.TableLayoutPanel();
            this.lbSumOK = new System.Windows.Forms.Label();
            this.lbOK = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbTimes = new System.Windows.Forms.Label();
            this.lbSumNG = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.picChart = new System.Windows.Forms.PictureBox();
            this.btnResetQty = new RJButton();
            this.pStatus.SuspendLayout();
            this.pInforTotal.SuspendLayout();
            this.pInfor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picChart)).BeginInit();
            this.SuspendLayout();
            // 
            // pStatus
            // 
            this.pStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pStatus.BackColor = System.Drawing.Color.Silver;
            this.pStatus.ColumnCount = 5;
            this.pStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.pStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.pStatus.Controls.Add(this.pInforTotal, 3, 0);
            this.pStatus.Controls.Add(this.lbStatus, 0, 0);
            this.pStatus.Controls.Add(this.pInfor, 2, 0);
            this.pStatus.Controls.Add(this.picChart, 1, 0);
            this.pStatus.Controls.Add(this.btnResetQty, 4, 0);
            this.pStatus.Location = new System.Drawing.Point(0, 0);
            this.pStatus.Name = "pStatus";
            this.pStatus.RowCount = 1;
            this.pStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pStatus.Size = new System.Drawing.Size(1255, 101);
            this.pStatus.TabIndex = 0;
            this.pStatus.SizeChanged += new System.EventHandler(this.pStatus_SizeChanged);
            // 
            // pInforTotal
            // 
            this.pInforTotal.ColumnCount = 2;
            this.pInforTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.pInforTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.pInforTotal.Controls.Add(this.label5, 0, 0);
            this.pInforTotal.Controls.Add(this.lbPercent, 1, 2);
            this.pInforTotal.Controls.Add(this.lbTotalTime, 1, 1);
            this.pInforTotal.Controls.Add(this.label9, 0, 1);
            this.pInforTotal.Controls.Add(this.label6, 0, 2);
            this.pInforTotal.Controls.Add(this.lbCycleTrigger, 1, 0);
            this.pInforTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pInforTotal.Location = new System.Drawing.Point(988, 3);
            this.pInforTotal.Name = "pInforTotal";
            this.pInforTotal.RowCount = 3;
            this.pInforTotal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pInforTotal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pInforTotal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pInforTotal.Size = new System.Drawing.Size(174, 95);
            this.pInforTotal.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 31);
            this.label5.TabIndex = 8;
            this.label5.Text = " CT";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbPercent
            // 
            this.lbPercent.BackColor = System.Drawing.Color.Transparent;
            this.lbPercent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPercent.ForeColor = System.Drawing.Color.White;
            this.lbPercent.Location = new System.Drawing.Point(72, 62);
            this.lbPercent.Name = "lbPercent";
            this.lbPercent.Size = new System.Drawing.Size(99, 33);
            this.lbPercent.TabIndex = 10;
            this.lbPercent.Text = "----------";
            this.lbPercent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbTotalTime
            // 
            this.lbTotalTime.BackColor = System.Drawing.Color.Transparent;
            this.lbTotalTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTotalTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTotalTime.ForeColor = System.Drawing.Color.White;
            this.lbTotalTime.Location = new System.Drawing.Point(72, 31);
            this.lbTotalTime.Name = "lbTotalTime";
            this.lbTotalTime.Size = new System.Drawing.Size(99, 31);
            this.lbTotalTime.TabIndex = 11;
            this.lbTotalTime.Text = "----------";
            this.lbTotalTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label9.Location = new System.Drawing.Point(3, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 31);
            this.label9.TabIndex = 12;
            this.label9.Text = "CT cam";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label6.Location = new System.Drawing.Point(3, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 33);
            this.label6.TabIndex = 9;
            this.label6.Text = "% OK";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbCycleTrigger
            // 
            this.lbCycleTrigger.BackColor = System.Drawing.Color.Transparent;
            this.lbCycleTrigger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCycleTrigger.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCycleTrigger.ForeColor = System.Drawing.Color.White;
            this.lbCycleTrigger.Location = new System.Drawing.Point(72, 0);
            this.lbCycleTrigger.Name = "lbCycleTrigger";
            this.lbCycleTrigger.Size = new System.Drawing.Size(99, 31);
            this.lbCycleTrigger.TabIndex = 7;
            this.lbCycleTrigger.Text = "----------";
            this.lbCycleTrigger.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbStatus
            // 
            this.lbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.lbStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.ForeColor = System.Drawing.Color.White;
            this.lbStatus.Location = new System.Drawing.Point(5, 5);
            this.lbStatus.Margin = new System.Windows.Forms.Padding(5);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(240, 91);
            this.lbStatus.TabIndex = 0;
            this.lbStatus.Text = "---";
            this.lbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pInfor
            // 
            this.pInfor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pInfor.ColumnCount = 3;
            this.pInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.pInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.pInfor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pInfor.Controls.Add(this.lbSumOK, 1, 1);
            this.pInfor.Controls.Add(this.lbOK, 1, 0);
            this.pInfor.Controls.Add(this.label2, 0, 0);
            this.pInfor.Controls.Add(this.lbTimes, 0, 1);
            this.pInfor.Controls.Add(this.lbSumNG, 2, 1);
            this.pInfor.Controls.Add(this.label3, 2, 0);
            this.pInfor.Location = new System.Drawing.Point(394, 3);
            this.pInfor.Name = "pInfor";
            this.pInfor.RowCount = 2;
            this.pInfor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.pInfor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pInfor.Size = new System.Drawing.Size(588, 95);
            this.pInfor.TabIndex = 16;
            this.pInfor.SizeChanged += new System.EventHandler(this.pInfor_SizeChanged);
            // 
            // lbSumOK
            // 
            this.lbSumOK.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSumOK.BackColor = System.Drawing.Color.White;
            this.lbSumOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSumOK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(186)))), ((int)(((byte)(98)))));
            this.lbSumOK.Location = new System.Drawing.Point(240, 24);
            this.lbSumOK.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.lbSumOK.Name = "lbSumOK";
            this.lbSumOK.Size = new System.Drawing.Size(225, 66);
            this.lbSumOK.TabIndex = 3;
            this.lbSumOK.Text = "---";
            this.lbSumOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbOK
            // 
            this.lbOK.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbOK.BackColor = System.Drawing.Color.Transparent;
            this.lbOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOK.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbOK.Location = new System.Drawing.Point(240, 0);
            this.lbOK.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbOK.Name = "lbOK";
            this.lbOK.Size = new System.Drawing.Size(225, 24);
            this.lbOK.TabIndex = 7;
            this.lbOK.Text = "OK";
            this.lbOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(5, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(225, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Total Times";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbTimes
            // 
            this.lbTimes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTimes.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbTimes.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTimes.ForeColor = System.Drawing.Color.Blue;
            this.lbTimes.Location = new System.Drawing.Point(5, 24);
            this.lbTimes.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.lbTimes.Name = "lbTimes";
            this.lbTimes.Size = new System.Drawing.Size(225, 66);
            this.lbTimes.TabIndex = 14;
            this.lbTimes.Text = "---";
            this.lbTimes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSumNG
            // 
            this.lbSumNG.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSumNG.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbSumNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSumNG.ForeColor = System.Drawing.Color.OrangeRed;
            this.lbSumNG.Location = new System.Drawing.Point(475, 24);
            this.lbSumNG.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.lbSumNG.Name = "lbSumNG";
            this.lbSumNG.Size = new System.Drawing.Size(108, 66);
            this.lbSumNG.TabIndex = 5;
            this.lbSumNG.Text = "---";
            this.lbSumNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(475, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "NG";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picChart
            // 
            this.picChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picChart.Image = global::BeeUi.Properties.Resources.Chart2;
            this.picChart.Location = new System.Drawing.Point(253, 3);
            this.picChart.Name = "picChart";
            this.picChart.Size = new System.Drawing.Size(135, 95);
            this.picChart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picChart.TabIndex = 15;
            this.picChart.TabStop = false;
            this.picChart.Visible = false;
            // 
            // btnResetQty
            // 
            this.btnResetQty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetQty.BackColor = System.Drawing.Color.Transparent;
            this.btnResetQty.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnResetQty.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnResetQty.BorderRadius = 10;
            this.btnResetQty.BorderSize = 1;
            this.btnResetQty.ButtonImage = null;
            this.btnResetQty.Corner =Corner.Both;
            this.btnResetQty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetQty.ForeColor = System.Drawing.Color.Black;
            this.btnResetQty.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResetQty.IsCLick = false;
            this.btnResetQty.IsNotChange = true;
            this.btnResetQty.IsRect = false;
            this.btnResetQty.IsUnGroup = true;
            this.btnResetQty.Location = new System.Drawing.Point(1168, 3);
            this.btnResetQty.Name = "btnResetQty";
            this.btnResetQty.Size = new System.Drawing.Size(84, 95);
            this.btnResetQty.TabIndex = 17;
            this.btnResetQty.Text = "Reset Qty";
            this.btnResetQty.TextColor = System.Drawing.Color.Black;
            this.btnResetQty.UseVisualStyleBackColor = false;
            this.btnResetQty.Click += new System.EventHandler(this.btnResetQty_Click);
            // 
            // ResultBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pStatus);
            this.Name = "ResultBar";
            this.Size = new System.Drawing.Size(1255, 100);
            this.Load += new System.EventHandler(this.ResultBar_Load);
            this.pStatus.ResumeLayout(false);
            this.pInforTotal.ResumeLayout(false);
            this.pInfor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.TableLayoutPanel pStatus;
        public System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.Label lbTotalTime;
        public System.Windows.Forms.Label lbPercent;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.Label lbCycleTrigger;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TableLayoutPanel pInfor;
        public System.Windows.Forms.Label lbSumOK;
        public System.Windows.Forms.Label lbOK;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label lbTimes;
        public System.Windows.Forms.Label lbSumNG;
        private System.Windows.Forms.Label label3;
        public RJButton btnResetQty;
        public System.Windows.Forms.PictureBox picChart;
        private Common.StepEdit stepEdit1;
        public System.Windows.Forms.TableLayoutPanel pInforTotal;
    }
}
