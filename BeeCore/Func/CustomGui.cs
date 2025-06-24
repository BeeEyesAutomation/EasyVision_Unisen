using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design.Behavior;

namespace BeeCore
{
    public class CustomGui
    {

        public static Color BackColor(TypeCtr TypeCtr ,Color color)
        {
            int Alpha = 0;
            switch (TypeCtr)
            {
                case TypeCtr.Bar:
                    Alpha = 60;
                    break;
                case TypeCtr.BG:
                    Alpha = 2;
                    break;
                case TypeCtr.Menu:
                    Alpha = 150;
                    break;
                case TypeCtr.Text:
                    Alpha = 40;
                    break;
            }    
           return Color.FromArgb(Alpha, color.R, color.G, color.B);
        }
       
        private static GraphicsPath GetFigurePath(Rectangle rect, int curveSize, Corner _Corner)
        {
            GraphicsPath path = new GraphicsPath();

            switch (_Corner)
            {
                case Corner.Both:
                    // Bo cả 4 góc
                    path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90); // Top-left
                    path.AddLine(rect.X + curveSize, rect.Y, rect.Right - curveSize, rect.Y);
                    path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90); // Top-right
                    path.AddLine(rect.Right, rect.Y + curveSize, rect.Right, rect.Bottom - curveSize);
                    path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90); // Bottom-right
                    path.AddLine(rect.Right - curveSize, rect.Bottom, rect.X + curveSize, rect.Bottom);
                    path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90); // Bottom-left
                    path.AddLine(rect.X, rect.Bottom - curveSize, rect.X, rect.Y + curveSize);
                    break;

                case Corner.Left:
                    // Bo góc trái (trên + dưới)
                    path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90); // Top-left
                    path.AddLine(rect.X + curveSize, rect.Y, rect.Right, rect.Y); // Top
                    path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom); // Right
                    path.AddLine(rect.Right, rect.Bottom, rect.X + curveSize, rect.Bottom); // Bottom
                    path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90); // Bottom-left
                    path.AddLine(rect.X, rect.Bottom - curveSize, rect.X, rect.Y + curveSize); // Left
                    break;

                case Corner.Right:
                    // Bo góc phải (trên + dưới)
                    path.AddLine(rect.X, rect.Y, rect.Right - curveSize, rect.Y); // Top
                    path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90); // Top-right
                    path.AddLine(rect.Right, rect.Y + curveSize, rect.Right, rect.Bottom - curveSize); // Right
                    path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90); // Bottom-right
                    path.AddLine(rect.Right - curveSize, rect.Bottom, rect.X, rect.Bottom); // Bottom
                    path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y); // Left
                    break;
                case Corner.Top:
                    // Bo 2 góc trên
                    path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90); // Top-left
                    path.AddLine(rect.X + curveSize, rect.Y, rect.Right - curveSize, rect.Y);
                    path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90); // Top-right
                    path.AddLine(rect.Right, rect.Y + curveSize, rect.Right, rect.Bottom); // Right
                    path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom); // Bottom
                    path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y + curveSize); // Left
                    break;

                case Corner.Bottom:
                    // Bo 2 góc dưới
                    path.AddLine(rect.X, rect.Y, rect.Right, rect.Y); // Top
                    path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom - curveSize); // Right
                    path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90); // Bottom-right
                    path.AddLine(rect.Right - curveSize, rect.Bottom, rect.X + curveSize, rect.Bottom);
                    path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90); // Bottom-left
                    path.AddLine(rect.X, rect.Bottom - curveSize, rect.X, rect.Y); // Left
                    break;

                case Corner.None:
                    path.AddRectangle(rect);
                    break;
            }

            path.CloseFigure();
            return path;
        }
        public static void RoundRg( dynamic contr,int RoundRad, Corner _Corner= Corner.Both)
        {
            Rectangle rectSurface = contr.ClientRectangle;
            GraphicsPath path = GetFigurePath(rectSurface, RoundRad, _Corner);
            contr.Region = new Region(path);
        }
    }
}
