using System;
using System.Drawing;
using BeeGlobal;

namespace BeeCore.ShapeEditing
{
    public sealed class ColorPickedArgs : EventArgs
    {
        public ColorPickedArgs(Color color, ColorGp colorMode)
        {
            Color = color;
            ColorMode = colorMode;
        }

        public Color Color { get; private set; }
        public ColorGp ColorMode { get; private set; }
    }
}
