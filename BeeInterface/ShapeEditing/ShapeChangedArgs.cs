using System;
using BeeGlobal;

namespace BeeInterface.ShapeEditing
{
    /// <summary>
    /// Loại thay đổi mà ShapeEditor vừa thực hiện trên 1 RectRotate.
    /// Consumer (View.cs/host) dùng để biết có cần persist, refresh UI khác,
    /// hay chỉ là preview tạm thời.
    /// </summary>
    public enum ShapeChangeKind
    {
        /// <summary>Vừa tạo shape mới (Mouse Up sau khi drag tạo).</summary>
        Created = 0,

        /// <summary>Đã move toàn bộ shape.</summary>
        Moved = 1,

        /// <summary>Đã resize shape qua handle góc/cạnh.</summary>
        Resized = 2,

        /// <summary>Đã xoay shape qua handle Rotation.</summary>
        Rotated = 3,

        /// <summary>Polygon đã được đóng (closed).</summary>
        PolygonClosed = 4,

        /// <summary>Đã thêm 1 vertex vào polygon.</summary>
        VertexAdded = 5,

        /// <summary>Đã kéo 1 vertex của polygon/hexagon.</summary>
        VertexMoved = 6,

        /// <summary>Đã xoá shape.</summary>
        Deleted = 7,

        /// <summary>Preview trong khi đang drag (không nhất thiết phải persist).</summary>
        Preview = 100
    }

    /// <summary>
    /// Event args khi ShapeEditor thay đổi RectRotate. Truyền ra ngoài để
    /// host quyết định có save vào PropetyTool hay không.
    /// </summary>
    public class ShapeChangedArgs : EventArgs
    {
        /// <summary>RectRotate sau khi thay đổi (đã clone để consumer không sửa nhầm state nội bộ).</summary>
        public RectRotate Shape { get; }

        /// <summary>Loại crop của shape.</summary>
        public TypeCrop CropType { get; }

        /// <summary>Loại thay đổi.</summary>
        public ShapeChangeKind Kind { get; }

        public ShapeChangedArgs(RectRotate shape, TypeCrop cropType, ShapeChangeKind kind)
        {
            Shape = shape;
            CropType = cropType;
            Kind = kind;
        }
    }
}
