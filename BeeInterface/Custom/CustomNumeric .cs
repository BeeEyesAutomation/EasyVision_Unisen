using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using BeeGlobal;
using BeeCore.Func;
using BeeCore;
namespace BeeInterface
{
    public partial class CustomNumeric : UserControl
    {
        private float value = 1;

        public CustomNumeric()
        {

            InitializeComponent();
            txt.Font = new Font("Arial", 20);
            this.FontChanged += CustomNumeric_FontChanged;
            //InitUI();
        }

        private void CustomNumeric_FontChanged(object sender, EventArgs e)
        {
           txt.Font = Font;
        }

        public float maxnimum = 100,  minimum = 0,step=1;
        private TextBox txt;
        private float _value = 0;
        [Category("Maxnimum")]
        public float Maxnimum
        {
            get
            {
                return maxnimum;
            }
            set
            {
                maxnimum = value;
              

            }
        }
        [Category("Minimum")]
        public float Minimum
        {
            get
            {
                return minimum;
            }
            set
            {
                minimum = value;
              
            }
        }
        [Category("Step")]
        public float Step
        {
            get
            {
                return step;
            }
            set
            {
                step = value;
                if (step ==0) step = 1;

                if (step > maxnimum) step = maxnimum;
            }
        }
        [Category("Value")]
        public float Value
        {
            get
            {
                return _value;
            }///lay
            set//gasn
            {
                _value =(float)Math.Round( value,1);
                if (_value < minimum) _value = minimum;
                if (_value > maxnimum) _value = maxnimum;
                txt.Text = _value + ""; 
               
                OnValueChanged(EventArgs.Empty);
            }
        }
        // Sự kiện ValueChanged
        [Category("ValueChanged")]
        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }
        private RJButton btnPlus;
        private RJButton btnSub;
        private DbTableLayoutPanel lay;

        private void InitializeComponent()
        {
            this.lay = new BeeInterface.DbTableLayoutPanel();
            this.btnSub = new BeeInterface.RJButton();
            this.btnPlus = new BeeInterface.RJButton();
            this.txt = new System.Windows.Forms.TextBox();
            this.lay.SuspendLayout();
            this.SuspendLayout();
            // 
            // lay
            // 
            this.lay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.lay.ColumnCount = 3;
            this.lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.lay.Controls.Add(this.btnSub, 0, 0);
            this.lay.Controls.Add(this.btnPlus, 2, 0);
            this.lay.Controls.Add(this.txt, 1, 0);
            this.lay.Location = new System.Drawing.Point(0, 1);
            this.lay.Margin = new System.Windows.Forms.Padding(5);
            this.lay.Name = "lay";
            this.lay.RowCount = 1;
            this.lay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lay.Size = new System.Drawing.Size(69, 43);
            this.lay.TabIndex = 10;
            // 
            // btnSub
            // 
            this.btnSub.AutoFont = true;
            this.btnSub.AutoFontHeightRatio = 0.75F;
            this.btnSub.AutoFontMax = 100F;
            this.btnSub.AutoFontMin = 6F;
            this.btnSub.AutoFontWidthRatio = 0.92F;
            this.btnSub.AutoImage = true;
            this.btnSub.AutoImageMaxRatio = 0.75F;
            this.btnSub.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSub.AutoImageTint = true;
            this.btnSub.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnSub.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnSub.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSub.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnSub.BorderRadius = 10;
            this.btnSub.BorderSize = 1;
            this.btnSub.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSub.Corner = BeeGlobal.Corner.Left;
            this.btnSub.DebounceResizeMs = 16;
            this.btnSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSub.FlatAppearance.BorderSize = 0;
            this.btnSub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSub.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.34375F, System.Drawing.FontStyle.Bold);
            this.btnSub.ForeColor = System.Drawing.Color.Black;
            this.btnSub.Image = null;
            this.btnSub.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSub.ImageDisabled = null;
            this.btnSub.ImageHover = null;
            this.btnSub.ImageNormal = null;
            this.btnSub.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSub.ImagePressed = null;
            this.btnSub.ImageTextSpacing = 6;
            this.btnSub.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSub.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSub.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSub.ImageTintOpacity = 0.5F;
            this.btnSub.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSub.IsCLick = false;
            this.btnSub.IsNotChange = false;
            this.btnSub.IsRect = false;
            this.btnSub.IsUnGroup = false;
            this.btnSub.Location = new System.Drawing.Point(0, 0);
            this.btnSub.Margin = new System.Windows.Forms.Padding(0);
            this.btnSub.Multiline = false;
            this.btnSub.Name = "btnSub";
            this.btnSub.Size = new System.Drawing.Size(35, 43);
            this.btnSub.TabIndex = 7;
            this.btnSub.Text = "-";
            this.btnSub.TextColor = System.Drawing.Color.Black;
            this.btnSub.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSub.UseVisualStyleBackColor = false;
            this.btnSub.Click += new System.EventHandler(this.btnSub_Click);
            this.btnSub.MouseLeave += new System.EventHandler(this.btnSub_MouseLeave);
            this.btnSub.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnSub_MouseMove);
            // 
            // btnPlus
            // 
            this.btnPlus.AutoFont = true;
            this.btnPlus.AutoFontHeightRatio = 0.75F;
            this.btnPlus.AutoFontMax = 100F;
            this.btnPlus.AutoFontMin = 6F;
            this.btnPlus.AutoFontWidthRatio = 0.92F;
            this.btnPlus.AutoImage = true;
            this.btnPlus.AutoImageMaxRatio = 0.75F;
            this.btnPlus.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnPlus.AutoImageTint = true;
            this.btnPlus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnPlus.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnPlus.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnPlus.BorderRadius = 10;
            this.btnPlus.BorderSize = 1;
            this.btnPlus.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnPlus.Corner = BeeGlobal.Corner.Right;
            this.btnPlus.DebounceResizeMs = 16;
            this.btnPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPlus.FlatAppearance.BorderSize = 0;
            this.btnPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.40625F, System.Drawing.FontStyle.Bold);
            this.btnPlus.ForeColor = System.Drawing.Color.Black;
            this.btnPlus.Image = null;
            this.btnPlus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPlus.ImageDisabled = null;
            this.btnPlus.ImageHover = null;
            this.btnPlus.ImageNormal = null;
            this.btnPlus.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnPlus.ImagePressed = null;
            this.btnPlus.ImageTextSpacing = 6;
            this.btnPlus.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnPlus.ImageTintHover = System.Drawing.Color.Empty;
            this.btnPlus.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnPlus.ImageTintOpacity = 0.5F;
            this.btnPlus.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnPlus.IsCLick = true;
            this.btnPlus.IsNotChange = false;
            this.btnPlus.IsRect = false;
            this.btnPlus.IsUnGroup = false;
            this.btnPlus.Location = new System.Drawing.Point(34, 0);
            this.btnPlus.Margin = new System.Windows.Forms.Padding(0);
            this.btnPlus.Multiline = false;
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(35, 43);
            this.btnPlus.TabIndex = 8;
            this.btnPlus.Text = "+";
            this.btnPlus.TextColor = System.Drawing.Color.Black;
            this.btnPlus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPlus.UseVisualStyleBackColor = false;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            this.btnPlus.MouseLeave += new System.EventHandler(this.btnPlus_MouseLeave);
            this.btnPlus.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnPlus_MouseMove);
            // 
            // txt
            // 
            this.txt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt.Location = new System.Drawing.Point(35, 2);
            this.txt.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.txt.Multiline = true;
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(1, 41);
            this.txt.TabIndex = 9;
            this.txt.Text = "00";
            this.txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt.TextChanged += new System.EventHandler(this.txt_TextChanged);
            this.txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // CustomNumeric
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.Controls.Add(this.lay);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CustomNumeric";
            this.Size = new System.Drawing.Size(69, 45);
            this.MouseLeave += new System.EventHandler(this.CustomNumeric_MouseLeave);
            this.lay.ResumeLayout(false);
            this.lay.PerformLayout();
            this.ResumeLayout(false);

        }

        private void CustomNumeric_MouseLeave(object sender, EventArgs e)
        {
            this.Width =70;
        }

        bool IsAllDigits(string input)
        {
            return !string.IsNullOrEmpty(input) && input.All(char.IsDigit);
        }
        private void txt_Click(object sender, EventArgs e)
        {

        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            txt.Text = txt.Text.Replace("\n", "");
          
            if (IsAllDigits(txt.Text))
            {
                Value = Convert.ToInt32(txt.Text.Trim());

            }
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    txt.Text = txt.Text.Replace("\n", "");
            //    if (IsAllDigits(txt.Text))
            //    {
            //        Value = Convert.ToInt32(txt.Text.Trim());
                   
            //    }

            //}
        }

        private void btnSub_MouseMove(object sender, MouseEventArgs e)
        {
   
        }

        private void btnPlus_MouseMove(object sender, MouseEventArgs e)
        {
         

        }

        private void btnSub_MouseLeave(object sender, EventArgs e)
        {
           
        }

        private void btnPlus_MouseLeave(object sender, EventArgs e)
        {
            
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            Value -= Step;
            Size Sz = Cal.GetSizeText(Maxnimum+".0", txt.Font);
            this.Width = Sz.Width + 70;
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            Value += Step;
            Size Sz = Cal.GetSizeText(Maxnimum + ".0", txt.Font);
            this.Width = Sz.Width+70;
        }
    }

}
