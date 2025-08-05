namespace BeeInterface
{
    partial class AdjustBar
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
            this.lay = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.Track = new BeeInterface.TrackBar2();
            this.Num = new BeeInterface.CustomNumeric();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lay.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // lay
            // 
            this.lay.BackColor = System.Drawing.Color.Transparent;
            this.lay.ColumnCount = 4;
            this.lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.lay.Controls.Add(this.panel2, 3, 0);
            this.lay.Controls.Add(this.tableLayoutPanel7, 1, 0);
            this.lay.Controls.Add(this.Num, 2, 0);
            this.lay.Controls.Add(this.panel1, 0, 0);
            this.lay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lay.Location = new System.Drawing.Point(0, 0);
            this.lay.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lay.Name = "lay";
            this.lay.RowCount = 1;
            this.lay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lay.Size = new System.Drawing.Size(404, 54);
            this.lay.TabIndex = 47;
            this.lay.Paint += new System.Windows.Forms.PaintEventHandler(this.lay_Paint);
            this.lay.MouseLeave += new System.EventHandler(this.lay_MouseLeave);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(394, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 54);
            this.panel2.TabIndex = 38;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.Track, 0, 1);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(10, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(314, 54);
            this.tableLayoutPanel7.TabIndex = 36;
            // 
            // Track
            // 
            this.Track.BackColor = System.Drawing.Color.Transparent;
            this.Track.ColorTrack = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Track.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Track.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Track.Location = new System.Drawing.Point(3, 5);
            this.Track.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.Track.Max = 30F;
            this.Track.Min = 0F;
            this.Track.Name = "Track";
            this.Track.Size = new System.Drawing.Size(308, 49);
            this.Track.Step = 1F;
            this.Track.TabIndex = 28;
            this.Track.Value = 10F;
            this.Track.ValueScore = 0F;
            this.Track.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Track_MouseMove);
            // 
            // Num
            // 
            this.Num.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.Num.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.Num.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Num.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Num.Location = new System.Drawing.Point(324, 0);
            this.Num.Margin = new System.Windows.Forms.Padding(0);
            this.Num.Maxnimum = 100F;
            this.Num.Minimum = 0F;
            this.Num.Name = "Num";
            this.Num.Size = new System.Drawing.Size(70, 54);
            this.Num.Step = 1F;
            this.Num.TabIndex = 35;
            this.Num.Value = 100F;
            this.Num.MouseLeave += new System.EventHandler(this.Num_MouseLeave);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 54);
            this.panel1.TabIndex = 37;
            // 
            // AdjustBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lay);
            this.Name = "AdjustBar";
            this.Size = new System.Drawing.Size(404, 54);
            this.Load += new System.EventHandler(this.AdjustBar_Load);
            this.lay.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public TrackBar2 Track;
        public CustomNumeric Num;
        private System.Windows.Forms.TableLayoutPanel lay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
