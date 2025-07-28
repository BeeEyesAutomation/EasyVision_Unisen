using BeeGlobal;
using BeeInterface;

namespace BeeUi.Unit
{
    partial class Cameras
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Cameras));
            this.pCamera = new System.Windows.Forms.TableLayoutPanel();
            this.btnCamera4 = new BeeInterface.RJButton();
            this.btnCamera1 = new BeeInterface.RJButton();
            this.btnCamera2 = new BeeInterface.RJButton();
            this.btnCamera3 = new BeeInterface.RJButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnHide = new BeeInterface.RJButton();
            this.pCamera.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pCamera
            // 
            this.pCamera.BackColor = System.Drawing.Color.LightGray;
            this.pCamera.ColumnCount = 4;
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pCamera.Controls.Add(this.btnCamera4, 3, 0);
            this.pCamera.Controls.Add(this.btnCamera1, 0, 0);
            this.pCamera.Controls.Add(this.btnCamera2, 1, 0);
            this.pCamera.Controls.Add(this.btnCamera3, 2, 0);
            this.pCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pCamera.Location = new System.Drawing.Point(5, 5);
            this.pCamera.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.pCamera.Name = "pCamera";
            this.pCamera.RowCount = 1;
            this.pCamera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pCamera.Size = new System.Drawing.Size(315, 63);
            this.pCamera.TabIndex = 31;
            this.pCamera.SizeChanged += new System.EventHandler(this.pCamera_SizeChanged);
            // 
            // btnCamera4
            // 
            this.btnCamera4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCamera4.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCamera4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCamera4.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCamera4.BorderRadius = 8;
            this.btnCamera4.BorderSize = 1;
            this.btnCamera4.ButtonImage = null;
            this.btnCamera4.Corner = BeeGlobal.Corner.Right;
            this.btnCamera4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCamera4.FlatAppearance.BorderSize = 0;
            this.btnCamera4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera4.ForeColor = System.Drawing.Color.Black;
            this.btnCamera4.Image = ((System.Drawing.Image)(resources.GetObject("btnCamera4.Image")));
            this.btnCamera4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCamera4.IsCLick = false;
            this.btnCamera4.IsNotChange = false;
            this.btnCamera4.IsRect = true;
            this.btnCamera4.IsUnGroup = false;
            this.btnCamera4.Location = new System.Drawing.Point(240, 5);
            this.btnCamera4.Margin = new System.Windows.Forms.Padding(1, 5, 5, 5);
            this.btnCamera4.Name = "btnCamera4";
            this.btnCamera4.Size = new System.Drawing.Size(70, 53);
            this.btnCamera4.TabIndex = 31;
            this.btnCamera4.Text = "Camera 4";
            this.btnCamera4.TextColor = System.Drawing.Color.Black;
            this.btnCamera4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCamera4.UseVisualStyleBackColor = false;
            this.btnCamera4.Click += new System.EventHandler(this.btnCamera4_Click);
            // 
            // btnCamera1
            // 
            this.btnCamera1.BackColor = System.Drawing.Color.Transparent;
            this.btnCamera1.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnCamera1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCamera1.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCamera1.BorderRadius = 8;
            this.btnCamera1.BorderSize = 1;
            this.btnCamera1.ButtonImage = null;
            this.btnCamera1.Corner = BeeGlobal.Corner.Left;
            this.btnCamera1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCamera1.FlatAppearance.BorderSize = 0;
            this.btnCamera1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera1.ForeColor = System.Drawing.Color.Black;
            this.btnCamera1.Image = ((System.Drawing.Image)(resources.GetObject("btnCamera1.Image")));
            this.btnCamera1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCamera1.IsCLick = true;
            this.btnCamera1.IsNotChange = false;
            this.btnCamera1.IsRect = true;
            this.btnCamera1.IsUnGroup = false;
            this.btnCamera1.Location = new System.Drawing.Point(5, 5);
            this.btnCamera1.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.btnCamera1.Name = "btnCamera1";
            this.btnCamera1.Size = new System.Drawing.Size(78, 53);
            this.btnCamera1.TabIndex = 20;
            this.btnCamera1.Text = "Camera 1";
            this.btnCamera1.TextColor = System.Drawing.Color.Black;
            this.btnCamera1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCamera1.UseVisualStyleBackColor = false;
            this.btnCamera1.Click += new System.EventHandler(this.btnCamera1_Click);
            // 
            // btnCamera2
            // 
            this.btnCamera2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCamera2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCamera2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCamera2.BorderColor = System.Drawing.Color.Transparent;
            this.btnCamera2.BorderRadius = 5;
            this.btnCamera2.BorderSize = 1;
            this.btnCamera2.ButtonImage = null;
            this.btnCamera2.Corner = BeeGlobal.Corner.None;
            this.btnCamera2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCamera2.FlatAppearance.BorderSize = 0;
            this.btnCamera2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera2.ForeColor = System.Drawing.Color.Black;
            this.btnCamera2.Image = ((System.Drawing.Image)(resources.GetObject("btnCamera2.Image")));
            this.btnCamera2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCamera2.IsCLick = false;
            this.btnCamera2.IsNotChange = false;
            this.btnCamera2.IsRect = true;
            this.btnCamera2.IsUnGroup = false;
            this.btnCamera2.Location = new System.Drawing.Point(83, 5);
            this.btnCamera2.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.btnCamera2.Name = "btnCamera2";
            this.btnCamera2.Size = new System.Drawing.Size(78, 53);
            this.btnCamera2.TabIndex = 19;
            this.btnCamera2.Text = "Camera 2";
            this.btnCamera2.TextColor = System.Drawing.Color.Black;
            this.btnCamera2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCamera2.UseVisualStyleBackColor = false;
            this.btnCamera2.Click += new System.EventHandler(this.btnCamera2_Click);
            // 
            // btnCamera3
            // 
            this.btnCamera3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCamera3.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCamera3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCamera3.BorderColor = System.Drawing.Color.Transparent;
            this.btnCamera3.BorderRadius = 5;
            this.btnCamera3.BorderSize = 1;
            this.btnCamera3.ButtonImage = null;
            this.btnCamera3.Corner = BeeGlobal.Corner.None;
            this.btnCamera3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCamera3.FlatAppearance.BorderSize = 0;
            this.btnCamera3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamera3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamera3.ForeColor = System.Drawing.Color.Black;
            this.btnCamera3.Image = ((System.Drawing.Image)(resources.GetObject("btnCamera3.Image")));
            this.btnCamera3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCamera3.IsCLick = false;
            this.btnCamera3.IsNotChange = false;
            this.btnCamera3.IsRect = true;
            this.btnCamera3.IsUnGroup = false;
            this.btnCamera3.Location = new System.Drawing.Point(161, 5);
            this.btnCamera3.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.btnCamera3.Name = "btnCamera3";
            this.btnCamera3.Size = new System.Drawing.Size(78, 53);
            this.btnCamera3.TabIndex = 29;
            this.btnCamera3.Text = "Camera 3";
            this.btnCamera3.TextColor = System.Drawing.Color.Black;
            this.btnCamera3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCamera3.UseVisualStyleBackColor = false;
            this.btnCamera3.Click += new System.EventHandler(this.btnCamera3_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnHide, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pCamera, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(370, 73);
            this.tableLayoutPanel1.TabIndex = 32;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // btnHide
            // 
            this.btnHide.BackColor = System.Drawing.Color.Transparent;
            this.btnHide.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnHide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHide.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(173)))), ((int)(((byte)(245)))));
            this.btnHide.BorderRadius = 8;
            this.btnHide.BorderSize = 1;
            this.btnHide.ButtonImage = null;
            this.btnHide.Corner = BeeGlobal.Corner.Right;
            this.btnHide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHide.FlatAppearance.BorderSize = 0;
            this.btnHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHide.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHide.ForeColor = System.Drawing.Color.Black;
            this.btnHide.Image = ((System.Drawing.Image)(resources.GetObject("btnHide.Image")));
            this.btnHide.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHide.IsCLick = false;
            this.btnHide.IsNotChange = false;
            this.btnHide.IsRect = false;
            this.btnHide.IsUnGroup = true;
            this.btnHide.Location = new System.Drawing.Point(321, 5);
            this.btnHide.Margin = new System.Windows.Forms.Padding(1, 5, 5, 5);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(44, 63);
            this.btnHide.TabIndex = 32;
            this.btnHide.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHide.TextColor = System.Drawing.Color.Black;
            this.btnHide.UseVisualStyleBackColor = false;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // Cameras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Cameras";
            this.Size = new System.Drawing.Size(370, 73);
            this.Load += new System.EventHandler(this.Cameras_Load);
            this.pCamera.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public RJButton btnHide;
        private System.Windows.Forms.TableLayoutPanel pCamera;
        public RJButton btnCamera4;
        public RJButton btnCamera1;
        public RJButton btnCamera2;
        public RJButton btnCamera3;
    }
}
