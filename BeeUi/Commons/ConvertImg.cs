using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeUi.Commons
{
    public class ConvertImg
    {
        public static Bitmap ChangeToColor(Bitmap bmp, Color c, float opacity)
        {
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics g = Graphics.FromImage(bmp2))
            {
                float tr = c.R / 255f;
                float tg = c.G / 255f;
                float tb = c.B / 255f;

                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                  {
                 new float[] {0, 0, 0, 0, 0},
                 new float[] {0, 0, 0, 0, 0},
                 new float[] {0, 0, 0, 0, 0},
                 new float[] {0, 0, 0, opacity, 0},
                 new float[] {tr, tg, tb, 0, 1}  // kudos to OP!
                  });

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);

                g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            }
            return bmp2;
        }

    }
}
