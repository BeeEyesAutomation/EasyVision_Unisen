using System.Drawing;
using BeeGlobal;

namespace BeeCore.ShapeEditing
{
    public class ShapeEditState
    {
        public RectRotate Current { get; set; }
        public TypeCrop Type { get; set; } = TypeCrop.None;
        public StatusDraw Mode { get; set; } = StatusDraw.None;
        public InteractionMode Interaction { get; set; } = InteractionMode.Idle;
        public int IndexTool { get; set; } = -1;
        public int IndexProg { get; set; }
        public int IndexCamera { get; set; }
        public bool IsRun { get; set; }
        public bool IsGetColor { get; set; }
        public bool IsSetColor { get; set; }
        public Color ColorSample { get; set; } = Color.Empty;
        public ColorGp ColorMode { get; set; } = ColorGp.RGB;
    }
}
