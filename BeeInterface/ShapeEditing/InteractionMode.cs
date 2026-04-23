using System;

namespace BeeInterface.ShapeEditing
{
    /// <summary>
    /// Trạng thái tương tác của ShapeEditor. Thay thế cho tổ hợp các cờ
    /// (_maybeCreate / _creatingNew / _drag / _polyDirtyDuringDrag) đang
    /// rải rác trong View.cs. Chỉ 1 nơi giữ trạng thái, chuyển state rõ ràng.
    /// </summary>
    public enum InteractionMode
    {
        /// <summary>Không có tương tác nào.</summary>
        Idle = 0,

        /// <summary>MouseDown trên vùng trống, có thể sẽ tạo shape mới nếu move vượt threshold.</summary>
        MaybeCreate = 1,

        /// <summary>Đang kéo tạo shape mới (đã vượt threshold).</summary>
        CreatingNew = 2,

        /// <summary>Đang kéo toàn bộ shape (anchor = Center).</summary>
        DraggingMove = 3,

        /// <summary>Đang resize shape theo 1 handle (anchor = TopLeft/TopRight/BottomLeft/BottomRight/Mid…).</summary>
        DraggingResize = 4,

        /// <summary>Đang xoay shape (anchor = Rotation).</summary>
        DraggingRotate = 5,

        /// <summary>Đang kéo 1 vertex của polygon/hexagon.</summary>
        DraggingVertex = 6,

        /// <summary>Chế độ pick màu — không edit shape.</summary>
        ColorPick = 7
    }
}
