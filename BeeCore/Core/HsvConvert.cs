using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;

namespace BeeCore.Core
{
 
    [Serializable()]
    public static class HsvConvert
    {
        // 1) OpenCV style: H:[0..179], S:[0..255], V:[0..255]
        public static Color FromHsvOpenCv(byte h, byte s, byte v, byte a = 255)
        {
            double H = h / 179.0 * 6.0;     // map 0..179 -> 0..6
            double S = s / 255.0;           // 0..1
            double V = v / 255.0;           // 0..1

            double r, g, b;
            if (s == 0)
            {
                r = g = b = V;
            }
            else
            {
                int i = (int)Math.Floor(H);           // sector 0..5
                double f = H - i;
                double p = V * (1.0 - S);
                double q = V * (1.0 - S * f);
                double t = V * (1.0 - S * (1.0 - f));

                switch (i % 6)
                {
                    case 0: r = V; g = t; b = p; break;
                    case 1: r = q; g = V; b = p; break;
                    case 2: r = p; g = V; b = t; break;
                    case 3: r = p; g = q; b = V; break;
                    case 4: r = t; g = p; b = V; break;
                    default: r = V; g = p; b = q; break; // i==5
                }
            }

            byte R = (byte)Math.Round(r * 255.0);
            byte G = (byte)Math.Round(g * 255.0);
            byte B = (byte)Math.Round(b * 255.0);
            return Color.FromArgb(a, R, G, B);
        }

        // 2) Common style: H:[0..360], S,V:[0..1]  (có overload % ở dưới)
        public static Color FromHsv(double hueDeg, double s01, double v01, byte a = 255)
        {
            hueDeg = (hueDeg % 360 + 360) % 360; // normalize
            double H = hueDeg / 60.0;            // 0..6
            double S = Math.Max(0, Math.Min(1, s01));
            double V = Math.Max(0, Math.Min(1, v01));

            double r, g, b;
            if (S <= 0.0)
            {
                r = g = b = V;
            }
            else
            {
                int i = (int)Math.Floor(H);
                double f = H - i;
                double p = V * (1.0 - S);
                double q = V * (1.0 - S * f);
                double t = V * (1.0 - S * (1.0 - f));
                switch (i % 6)
                {
                    case 0: r = V; g = t; b = p; break;
                    case 1: r = q; g = V; b = p; break;
                    case 2: r = p; g = V; b = t; break;
                    case 3: r = p; g = q; b = V; break;
                    case 4: r = t; g = p; b = V; break;
                    default: r = V; g = p; b = q; break;
                }
            }
            return Color.FromArgb(a,
                (byte)Math.Round(r * 255.0),
                (byte)Math.Round(g * 255.0),
                (byte)Math.Round(b * 255.0));
        }

        // tiện: S,V theo phần trăm
        public static Color FromHsvPercent(double hueDeg, double sPercent, double vPercent, byte a = 255) =>
            FromHsv(hueDeg, sPercent / 100.0, vPercent / 100.0, a);
    }

}
