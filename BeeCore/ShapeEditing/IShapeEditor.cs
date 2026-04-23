using System;
using BeeGlobal;

namespace BeeCore.ShapeEditing
{
    public interface IShapeEditor
    {
        ShapeEditState State { get; set; }
        event EventHandler<ShapeChangedArgs> ShapeChanged;
        void BeginEdit(RectRotate shape, TypeCrop type, int idxProg, int idxTool);
        void Cancel();
    }
}
