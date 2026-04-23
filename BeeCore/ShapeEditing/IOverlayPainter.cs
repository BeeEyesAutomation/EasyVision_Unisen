using System.Drawing;

namespace BeeCore.ShapeEditing
{
    public interface IOverlayPainter
    {
        void Paint(Graphics g, IImageCanvas canvas);
    }
}
