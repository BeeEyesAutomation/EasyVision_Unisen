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
using BeeCore;

namespace BeeUi.Commons
{
    public partial class CustomNumeric : UserControl
    {
        private int value = 1;

        public CustomNumeric()
        {

            InitializeComponent();
            this.FontChanged += CustomNumeric_FontChanged;
            //InitUI();
        }

        private void CustomNumeric_FontChanged(object sender, EventArgs e)
        {
           txt.Font = Font;
        }

        public int maxnimum = 100,  minimum = 0,step=1;
        private TextBox txt;
        private int _value = 0;
        [Category("Maxnimum")]
        public int Maxnimum
        {
            get
            {
                return maxnimum;
            }
            set
            {
                maxnimum = value;
                if (_value > maxnimum) Value = maxnimum;


            }
        }
        [Category("Minimum")]
        public int Minimum
        {
            get
            {
                return minimum;
            }
            set
            {
                minimum = value;
                if (_value < minimum) Value = minimum;


            }
        }
        [Category("Step")]
        public int Step
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
        public int Value
        {
            get
            {
                return _value;
            }///lay
            set//gasn
            {
                _value = value;
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
        private Common.RJButton btnPlus;
        private Common.RJButton btnSub;
        private TableLayoutPanel lay;

     
        private void InitializeComponent()
        {
            this.lay = new System.Windows.Forms.TableLayoutPanel();
            this.btnSub = new BeeUi.Common.RJButton();
            this.btnPlus = new BeeUi.Common.RJButton();
            this.txt = new System.Windows.Forms.TextBox();
            this.lay.SuspendLayout();
            this.SuspendLayout();
            // 
            // lay
            // 
            this.lay.ColumnCount = 3;
            this.lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.lay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.lay.Controls.Add(this.btnSub, 0, 0);
            this.lay.Controls.Add(this.btnPlus, 2, 0);
            this.lay.Controls.Add(this.txt, 1, 0);
            this.lay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lay.Location = new System.Drawing.Point(0, 0);
            this.lay.Margin = new System.Windows.Forms.Padding(5);
            this.lay.Name = "lay";
            this.lay.RowCount = 1;
            this.lay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lay.Size = new System.Drawing.Size(160, 55);
            this.lay.TabIndex = 10;
            // 
            // btnSub
            // 
            this.btnSub.BackColor = System.Drawing.Color.Transparent;
            this.btnSub.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnSub.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSub.BorderColor = System.Drawing.Color.Transparent;
            this.btnSub.BorderRadius = 10;
            this.btnSub.BorderSize = 1;
            this.btnSub.ButtonImage = null;
            this.btnSub.Corner = BeeCore.Corner.Left;
            this.btnSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSub.FlatAppearance.BorderSize = 0;
            this.btnSub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSub.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSub.ForeColor = System.Drawing.Color.Black;
            this.btnSub.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSub.IsCLick = false;
            this.btnSub.IsNotChange = false;
            this.btnSub.IsRect = false;
            this.btnSub.IsUnGroup = false;
            this.btnSub.Location = new System.Drawing.Point(5, 3);
            this.btnSub.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.btnSub.Name = "btnSub";
            this.btnSub.Size = new System.Drawing.Size(43, 49);
            this.btnSub.TabIndex = 7;
            this.btnSub.Text = "-";
            this.btnSub.TextColor = System.Drawing.Color.Black;
            this.btnSub.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSub.UseVisualStyleBackColor = false;
            this.btnSub.Click += new System.EventHandler(this.btnSub_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.BackColor = System.Drawing.Color.Transparent;
            this.btnPlus.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnPlus.BorderColor = System.Drawing.Color.Transparent;
            this.btnPlus.BorderRadius = 10;
            this.btnPlus.BorderSize = 1;
            this.btnPlus.ButtonImage = null;
            this.btnPlus.Corner = BeeCore.Corner.Right;
            this.btnPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPlus.FlatAppearance.BorderSize = 0;
            this.btnPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlus.ForeColor = System.Drawing.Color.Black;
            this.btnPlus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPlus.IsCLick = true;
            this.btnPlus.IsNotChange = false;
            this.btnPlus.IsRect = false;
            this.btnPlus.IsUnGroup = false;
            this.btnPlus.Location = new System.Drawing.Point(112, 3);
            this.btnPlus.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(43, 49);
            this.btnPlus.TabIndex = 8;
            this.btnPlus.Text = "+";
            this.btnPlus.TextColor = System.Drawing.Color.Black;
            this.btnPlus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPlus.UseVisualStyleBackColor = false;
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // txt
            // 
            this.txt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt.Location = new System.Drawing.Point(51, 3);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(58, 49);
            this.txt.TabIndex = 9;
            this.txt.Text = "00";
            this.txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt.TextChanged += new System.EventHandler(this.txt_TextChanged);
            this.txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // CustomNumeric
            // 
            this.Controls.Add(this.lay);
            this.Name = "CustomNumeric";
            this.Size = new System.Drawing.Size(160, 55);
            this.lay.ResumeLayout(false);
            this.lay.PerformLayout();
            this.ResumeLayout(false);

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
          
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt.Text = txt.Text.Replace("\n", "");
                if (IsAllDigits(txt.Text))
                {
                    Value = Convert.ToInt32(txt.Text.Trim());
                }

            }
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            Value -= Step;

        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            Value += Step;

        }
    }

}
