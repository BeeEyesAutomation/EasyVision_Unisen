using BeeGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
{
    public partial class AdjustBar : UserControl
    {
        private float _Value = 0;
        private float _Min = 0;
        private float _Max = 100;
        private float _Step = 1;
        public event Action<float> ValueChanged;
        // Appearance properties
        [Category("Value"), Description("Value")]
        public float Value
        {
            get => _Value;
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    Track.Value = _Value;
                    Num.Value = _Value;
                    ValueChanged?.Invoke(_Value); // Gọi event
                }
            }
        }
        [Category("Min"), Description("Min")]
        public float Min
        {
            get => _Min;
            set
            {
                _Min = value;
                if (Value < _Min) Value = _Min;
                Track.Min = _Min;
                Num.Minimum = _Min;
            }
        }
        [Category("Max"), Description("Max")]
        public float Max
        {
            get => _Max;
            set
            {
                _Max = value;
                if (Value > _Max) Value = _Max;
                Track.Max = _Max;
                Num.Maxnimum = _Max;
            }
        }
        [Category("Step"), Description("Step")]
        public float Step
        {
            get => _Step;
            set
            {
                _Step = value;
                Track.Step = _Step;
                Num.Step = _Step;

            }
        }
      
        public AdjustBar()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            InitializeComponent();
           
            
            Track.ValueChanged += Track_ValueChanged;
            Num.ValueChanged += Num_ValueChanged;
            this.SizeChanged += AdjustBar_SizeChanged;
           // this.HandleCreated += AdjustBar_HandleCreated;
          //  Gui.RoundRg(this, 10, Corner.Both);
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Gui.RoundRg(this, 10, Corner.Both);
        }
        private void AdjustBar_HandleCreated(object sender, EventArgs e)
        {
            //this.Invoke(new Action(() =>
            //{
            //    Gui.RoundRg(this, 10, Corner.Both);
            //}));
        }

        private void AdjustBar_SizeChanged(object sender, EventArgs e)
        {
          
                // Giả sử có tính toán gì đó ở đây...

                // Quay lại UI thread để bo góc
                //this.Invoke(new Action(() =>
                //{
                //    Gui.RoundRg(this, 10, Corner.Both);
                //}));
           
        }

        private void Num_ValueChanged(object sender, EventArgs e)
        {
            Value = Num.Value;
            Track.Value = Value;
        }

        private void Track_ValueChanged(float obj)
        {
            Value = Track.Value;
            Num.Value = Value;
        }

        private void lay_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Track_MouseMove(object sender, MouseEventArgs e)
        {
            Num.Width = 70;
        }

        private void AdjustBar_Load(object sender, EventArgs e)
        {
            //Gui.RoundRg(this, 10, Corner.Both);

        }
    }
}
