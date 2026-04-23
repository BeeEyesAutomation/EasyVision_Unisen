using System;
using BeeGlobal;

namespace BeeCore.ShapeEditing
{
    public sealed class ShapeChangedArgs : EventArgs
    {
        public ShapeChangedArgs(RectRotate @new, ShapeChangeKind kind)
        {
            New = @new;
            Kind = kind;
        }

        public RectRotate New { get; private set; }
        public ShapeChangeKind Kind { get; private set; }
    }
}
