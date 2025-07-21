using BeeGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        // Appearance properties
        [Category("Value"), Description("Value")]
        public float Value
        {
            get => _Value;
            set
            {
                _Value = value;
                Track.Value = _Value;

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
            InitializeComponent();
            Track.ValueChanged += Track_ValueChanged;
            Num.ValueChanged += Num_ValueChanged;
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
    }
}
