using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeUi.Commons
{
    public  class Func
    {
        public static void ScaleView(ref Matrix mat, float Scale,System.Drawing. Point pScale)
        {
            mat.Translate(pScale.X, pScale.Y);
            mat.Scale(G.Scale, G.Scale);
        }
    }
}
