using System;
using System.Drawing;
using System.Windows.Forms;
using BeeGlobal;

namespace BeeInterface
{
    /// <summary>
    /// Gắn 3 button mới (UltraThin / Adaptive / DenoiseFirst) vào tableLayoutPanel của tool form.
    /// Dùng chung cho mọi ToolXxx có Extract Edge.
    /// </summary>
    public static class EdgeButtonsHelper
    {
        public sealed class ExtraButtons
        {
            public RJButton UltraThin;
            public RJButton Adaptive;
            public RJButton DenoiseFirst;

            public void ResetAll()
            {
                if (UltraThin != null)    UltraThin.IsCLick = false;
                if (Adaptive != null)     Adaptive.IsCLick = false;
                if (DenoiseFirst != null) DenoiseFirst.IsCLick = false;
            }

            public void Highlight(MethordEdge m)
            {
                ResetAll();
                switch (m)
                {
                    case MethordEdge.UltraThin:    if (UltraThin != null)    UltraThin.IsCLick = true; break;
                    case MethordEdge.Adaptive:     if (Adaptive != null)     Adaptive.IsCLick = true; break;
                    case MethordEdge.DenoiseFirst: if (DenoiseFirst != null) DenoiseFirst.IsCLick = true; break;
                }
            }
        }

        /// <summary>
        /// Thêm 3 button vào row mới của panel.
        /// onSelect được gọi khi user click 1 trong 3 button.
        /// </summary>
        public static ExtraButtons Attach(TableLayoutPanel panel, Action<MethordEdge> onSelect)
        {
            if (panel == null || onSelect == null) return new ExtraButtons();

            var thin    = Make("UltraThin");
            var adapt   = Make("Adaptive");
            var denoise = Make("Denoise");

            EventHandler thinH    = (s, e) => onSelect(MethordEdge.UltraThin);
            EventHandler adaptH   = (s, e) => onSelect(MethordEdge.Adaptive);
            EventHandler denoiseH = (s, e) => onSelect(MethordEdge.DenoiseFirst);

            thin.Click    += thinH;
            adapt.Click   += adaptH;
            denoise.Click += denoiseH;

            int newRow = panel.RowCount;
            panel.RowCount = newRow + 1;
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            panel.Controls.Add(thin,    0, newRow);
            panel.Controls.Add(adapt,   1, newRow);
            panel.Controls.Add(denoise, 2, newRow);

            return new ExtraButtons { UltraThin = thin, Adaptive = adapt, DenoiseFirst = denoise };
        }

        private static RJButton Make(string text)
        {
            return new RJButton
            {
                Text = text,
                Dock = DockStyle.Fill,
                AutoFont = true,
                BackColor = Color.White,
                BackgroundColor = Color.White,
                BorderColor = Color.White,
                BorderRadius = 10,
                BorderSize = 1,
                ClickBotColor = Color.FromArgb(247, 211, 139),
                ClickMidColor = Color.FromArgb(246, 204, 120),
                ClickTopColor = Color.FromArgb(244, 192, 89),
                ForeColor = Color.Black,
                IsCLick = false,
            };
        }
    }
}
