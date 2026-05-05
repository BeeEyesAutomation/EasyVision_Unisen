using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BeeInterface
{
    [DefaultEvent(nameof(ValueChanged))]
    public partial class ScoreThresholdBar : UserControl
    {
        public ScoreThresholdBar()
        {
            InitializeComponent();
            bar.ValueChanged += OnBarValueChanged;
        }

        public event EventHandler ValueChanged;

        [Category("Behavior")]
        public float Min
        {
            get { return bar.Min; }
            set { bar.Min = value; }
        }

        [Category("Behavior")]
        public float Max
        {
            get { return bar.Max; }
            set { bar.Max = value; }
        }

        [Category("Behavior")]
        public float Step
        {
            get { return bar.Step; }
            set { bar.Step = value; }
        }

        [Category("Behavior")]
        public float Value
        {
            get { return bar.Value; }
            set { bar.Value = value; }
        }

        [Category("Appearance")]
        [DefaultValue("Score")]
        public string Caption
        {
            get { return lblCaption.Text; }
            set { lblCaption.Text = string.IsNullOrWhiteSpace(value) ? "Score" : value; }
        }

        private void OnBarValueChanged(float value)
        {
            var handler = ValueChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
