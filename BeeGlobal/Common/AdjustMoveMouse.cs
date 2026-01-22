using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeGlobal
{
    public class AdjustMoveMouse : UserControl
    {
        // C# 7.3 – không nullable
        public event Action<float, float> CenterDeltaChanged;

        public float Step { get; set; } = 1.0f;

        Button btnUp;
        Button btnDown;
        Button btnLeft;
        Button btnRight;

        public AdjustMoveMouse()
        {
            this.Size = new Size(78, 78);
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.BorderStyle = BorderStyle.FixedSingle;

            btnUp = CreateBtn("▲");
            btnDown = CreateBtn("▼");
            btnLeft = CreateBtn("◀");
            btnRight = CreateBtn("▶");

            // layout dạng dấu +
            btnUp.Location = new Point(26, 0);
            btnDown.Location = new Point(26, 52);
            btnLeft.Location = new Point(0, 26);
            btnRight.Location = new Point(52, 26);

            btnUp.Click += (s, e) => RaiseDelta(0, -Step);
            btnDown.Click += (s, e) => RaiseDelta(0, +Step);
            btnLeft.Click += (s, e) => RaiseDelta(-Step, 0);
            btnRight.Click += (s, e) => RaiseDelta(+Step, 0);

            Controls.AddRange(new Control[]
            {
                btnUp, btnDown, btnLeft, btnRight
            });
        }

        Button CreateBtn(string text)
        {
            return new Button
            {
                Text = text,
                Size = new Size(26, 26),
                Font = new Font("Arial", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                TabStop = false
            };
        }

        void RaiseDelta(float dx, float dy)
        {
            var handler = CenterDeltaChanged;
            if (handler != null)
                handler(dx, dy);
        }
    }
}
