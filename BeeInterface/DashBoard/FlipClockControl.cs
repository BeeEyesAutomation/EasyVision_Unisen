using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

public class FlipClockDashboard : Control
{
    string _time = "00:00:00";



    public Color DigitBackColor { get; set; } = Color.FromArgb(50,20, 20, 20);
    public Color DigitForeColor { get; set; } = Color.White;

    public int DigitSpacing { get; set; } = 6;

    public FlipClockDashboard()
    {
        DoubleBuffered = true;
        BackColor = Color.Black;
    }

    System.Windows.Forms.Timer timer;

    public void Start()
    {
        if (timer != null) return;

        timer = new System.Windows.Forms.Timer();
        timer.Interval = 1000; // 1 second

        timer.Tick += (s, e) =>
        {
            string t = DateTime.Now.ToString("HH:mm:ss");

            if (_time != t)
            {
                _time = t;
                Invalidate();
            }
        };

        timer.Start();
    }

    public void Stop()
    {
        if (timer == null) return;

        timer.Stop();
        timer.Dispose();
        timer = null;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(BackColor);

        int count = _time.Length;

        int digitWidth = (Width - DigitSpacing * (count + 1)) / count;
        int digitHeight = Height - DigitSpacing * 2;

        float fontSize = digitHeight * 0.8f;

        using (Font f = new Font("Consolas", fontSize, FontStyle.Bold))
        {



            for (int i = 0; i < count; i++)
            {
                int x = DigitSpacing + i * (digitWidth + DigitSpacing);
                int y = DigitSpacing;
                int offSet = 5;
                Rectangle rect = new Rectangle(x- offSet, y- offSet, digitWidth+ offSet*2, digitHeight+ offSet*2);

                if (_time[i] != ':')
                {
                    using (Brush b = new SolidBrush(DigitBackColor))
                        g.FillRectangle(b, rect);
                }

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                using (Brush b = new SolidBrush(DigitForeColor))
                    g.DrawString(_time[i].ToString(), f, b, rect, sf);
            }
        }
    }
}