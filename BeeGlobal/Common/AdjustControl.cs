using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeGlobal
{
    public class AdjustControlMouse : UserControl
    {

        // C# 7.3: không dùng '?'
        public event Action<float> AngleDeltaChanged;

        public float Step { get; set; } = 0.2f;

        private Button btnPlus;
        private Button btnMinus;

        public AdjustControlMouse()
        {
            this.Size = new Size(60, 26);
            this.BackColor = Color.FromArgb(150,150,150);
            this.BorderStyle = BorderStyle.FixedSingle;

            btnMinus = new Button
            {
                Text = "−",
                Dock = DockStyle.Left,
                Width = 30,
                Font=new Font("Arial",11),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White
            };

            btnPlus = new Button
            {
                Text = "+",
                Width = 30,
                Font = new Font("Arial", 11),
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White
            };

            btnMinus.FlatAppearance.BorderSize = 0;
            btnPlus.FlatAppearance.BorderSize = 0;

            btnMinus.Click += (s, e) => RaiseDelta(-Step);
            btnPlus.Click += (s, e) => RaiseDelta(+Step);

            Controls.Add(btnPlus);
            Controls.Add(btnMinus);
        }

        private void RaiseDelta(float delta)
        {
            var handler = AngleDeltaChanged;
            if (handler != null) handler(delta);
        }
    }


}
