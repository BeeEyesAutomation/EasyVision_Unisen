using BeeCore;
using BeeGlobal;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BeeInterface
{
    public partial class ToolCircle
    {
private void btnCropRect_Click(object sender, EventArgs e)
{
    Global.TypeCrop = TypeCrop.Crop;
    Propety.TypeCrop = Global.TypeCrop;
    btnElip.IsCLick = Propety.rotCrop.Shape == ShapeType.Ellipse ? true : false;
    btnRect.IsCLick = Propety.rotCrop.Shape == ShapeType.Rectangle ? true : false;
    btnHexagon.IsCLick = Propety.rotCrop.Shape == ShapeType.Hexagon ? true : false;
    btnPolygon.IsCLick = Propety.rotCrop.Shape == ShapeType.Polygon ? true : false;
    btnWhite.IsCLick = Propety.rotCrop.IsWhite;
    btnBlack.IsCLick = !Propety.rotCrop.IsWhite;

}

private void btnCropArea_Click(object sender, EventArgs e)
{
    Global.TypeCrop = TypeCrop.Area;
    Propety.TypeCrop = Global.TypeCrop;

    btnElip.IsCLick = Propety.rotArea.Shape == ShapeType.Ellipse ? true : false;
    btnRect.IsCLick = Propety.rotArea.Shape == ShapeType.Rectangle ? true : false;
    btnHexagon.IsCLick = Propety.rotArea.Shape == ShapeType.Hexagon ? true : false;
    btnPolygon.IsCLick = Propety.rotArea.Shape == ShapeType.Polygon ? true : false;
    btnWhite.IsCLick = Propety.rotArea.IsWhite;
    btnBlack.IsCLick = !Propety.rotArea.IsWhite;
}
private void btnClear_Click(object sender, EventArgs e)
{
    Global.TypeCrop = TypeCrop.Mask;
    Propety.TypeCrop = Global.TypeCrop;
    if (Propety.rotMask == null)
    {
        Propety.rotMask = DataTool.NewRotRect(TypeCrop.Mask); ;
    }
    btnElip.IsCLick = Propety.rotMask.Shape == ShapeType.Ellipse ? true : false;
    btnRect.IsCLick = Propety.rotMask.Shape == ShapeType.Rectangle ? true : false;
    btnHexagon.IsCLick = Propety.rotMask.Shape == ShapeType.Hexagon ? true : false;
    btnPolygon.IsCLick = Propety.rotMask.Shape == ShapeType.Polygon ? true : false;
    btnWhite.IsCLick = Propety.rotArea.IsWhite;
    btnBlack.IsCLick = !Propety.rotArea.IsWhite;


}
private void btnNone_Click(object sender, EventArgs e)
{
    switch (Global.TypeCrop)
    {
        //case TypeCrop.Crop:
        //    Propety.rotCrop.Shape= btnElip.IsCLick==true ? ShapeType.Ellipse: ShapeType.Rectangle;
        //    break;
        //case TypeCrop.Area:
        //    Propety.rotArea.Shape= btnElip.IsCLick==true ? ShapeType.Ellipse: ShapeType.Rectangle;
        //    break;
        case TypeCrop.Mask:
            Propety.rotMask = null;// = btnElip.IsCLick;
            break;

    }
    //  G.EditTool.View.imgView.Invalidate();
}

private void SetShapeFor(TypeCrop which, ShapeType shape)
{

    RectRotate rr = null;
    if (which == TypeCrop.Area) { if (Propety.rotArea == null) Propety.rotArea = new RectRotate(); rr = Propety.rotArea; }
    else if (which == TypeCrop.Mask) { if (Propety.rotMask == null) Propety.rotMask = new RectRotate(); rr = Propety.rotMask; }
    else { if (Propety.rotCrop == null) Propety.rotCrop = new RectRotate(); rr = Propety.rotCrop; }

    rr.Shape = shape;
    if (shape == ShapeType.Polygon)
    {
        if (rr.PolyLocalPoints == null || rr.PolyLocalPoints.Count() == 0)
            NewShape(shape);
        else
        {
            rr.UpdateFromPolygon(true);
        }
    }
    if (shape == ShapeType.Hexagon)
    {
        if (rr.HexVertexOffsets == null || rr.HexVertexOffsets.Count() == 0)
            NewShape(shape);
    }


    Global.TypeCrop = which;
    Global.StatusDraw = StatusDraw.None;
    Global.StatusDraw = StatusDraw.Edit;



}
private void NewShape(ShapeType newShape)
{
    // 1) Chốt shape hiện tại
    var prop = Common.TryGetTool(Global.IndexToolSelected).Propety2;
    RectRotate rr = null;
    if (Global.TypeCrop == TypeCrop.Area) rr = prop?.rotArea;
    else if (Global.TypeCrop == TypeCrop.Mask) rr = prop?.rotMask;
    else rr = prop?.rotCrop;

    if (rr != null)
    {
        // Nếu đang drag: chấm dứt
        rr._dragAnchor = AnchorPoint.None;
        rr.ActiveVertexIndex = -1;

        // Nếu là polygon đang dựng dở
        if (rr.Shape == ShapeType.Polygon && rr.IsPolygonClosed == false)
        {
            // CHỌN 1 TRONG 3 CHÍNH SÁCH:

            // (A) Giữ tạm nguyên trạng (không chuẩn hoá, không xoá điểm)
            // -> Không làm gì thêm

            // (B) Tự đóng & chuẩn hoá (nếu muốn)
            // nếu có >=3 điểm thì tự đóng:
            // if (rr.PolyLocalPoints != null && rr.PolyLocalPoints.Count >= 3) {
            //     var p0 = rr.PolyLocalPoints[0];
            //     rr.PolyLocalPoints.Add(p0);
            //     rr.IsPolygonClosed = true;
            //     rr.UpdateFromPolygon(updateAngle: rr.AutoOrientPolygon);
            // }

            // (C) Huỷ polygon đang dựng
            // rr.PolygonClear();
        }
    }



    // 3) Gán shape mới & chuẩn bị khung
    if (rr == null)
    {
        // tuỳ code lưu trữ của bạn mà tạo mới:
        rr = new RectRotate();
        if (Global.TypeCrop == TypeCrop.Area) prop.rotArea = rr;
        else if (Global.TypeCrop == TypeCrop.Mask) prop.rotMask = rr;
        else prop.rotCrop = rr;
    }

    rr.Shape = newShape;

    switch (newShape)
    {
        case ShapeType.Polygon:
            // Local sạch, xoá điểm cũ: chờ click điểm đầu tiên
            rr.ResetFrameForNewPolygonHard();
            rr.AutoOrientPolygon = false; // thường tắt lúc dựng, bạn có thể để true nếu quen
            break;

        case ShapeType.Rectangle:
        case ShapeType.Ellipse:
        case ShapeType.Hexagon:
            // Không cần xoá toàn bộ; chỉ đảm bảo không kéo theo trạng thái cũ
            rr._dragAnchor = AnchorPoint.None;
            rr.ActiveVertexIndex = -1;

            // Option: reset rotation cho phiên mới (tuỳ UX)
            // rr._rectRotation = 0f;

            // Để trống _rect: user kéo trái→phải để tạo mới theo logic MouseDown/Move của bạn
            rr._rect = RectangleF.Empty;

            // Hexagon: offsets về 0
            if (newShape == ShapeType.Hexagon)
            {
                if (rr.HexVertexOffsets == null || rr.HexVertexOffsets.Length != 6)
                    rr.HexVertexOffsets = new PointF[6];
                for (int i = 0; i < 6; i++) rr.HexVertexOffsets[i] = PointF.Empty;
            }

            break;
    }

    // Cập nhật về prop
    if (Global.TypeCrop == TypeCrop.Area) prop.rotArea = rr;
    else if (Global.TypeCrop == TypeCrop.Mask) prop.rotMask = rr;
    else prop.rotCrop = rr;


}

ShapeType ShapeType = ShapeType.Rectangle;
private void btnHexagon_Click(object sender, EventArgs e)
{
    ShapeType = ShapeType.Hexagon;
    SetShapeFor(Global.TypeCrop, ShapeType);
}

private void btnPolygon_Click(object sender, EventArgs e)
{
    ShapeType = ShapeType.Polygon;

    SetShapeFor(Global.TypeCrop, ShapeType);
}

private void btnNewShape_Click(object sender, EventArgs e)
{
    NewShape(ShapeType);
}
private void btnRect_Click(object sender, EventArgs e)
{
    ShapeType = ShapeType.Rectangle;
    SetShapeFor(Global.TypeCrop, ShapeType);

}

private void btnElip_Click(object sender, EventArgs e)
{
    ShapeType = ShapeType.Ellipse;
    SetShapeFor(Global.TypeCrop, ShapeType);

}

private void btnWhite_Click(object sender, EventArgs e)
{
    switch (Global.TypeCrop)
    {
        case TypeCrop.Area:
            Propety.rotArea.IsWhite = btnWhite.IsCLick;
            break;
        case TypeCrop.Crop:
            Propety.rotCrop.IsWhite = btnWhite.IsCLick;
            break;
    }

}

private void btnBlack_Click(object sender, EventArgs e)
{
    switch (Global.TypeCrop)
    {
        case TypeCrop.Area:
            Propety.rotArea.IsWhite = !btnBlack.IsCLick;
            break;
        case TypeCrop.Crop:
            Propety.rotCrop.IsWhite = !btnBlack.IsCLick;
            break;
    }

}

        bool IsFullSize = false;

        private void btnCropHalt_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;

           
        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-Global.Config.SizeCCD.Width / 2, -Global.Config.SizeCCD.Height / 2, Global.Config.SizeCCD.Width, Global.Config.SizeCCD.Height), new PointF(Global.Config.SizeCCD.Width / 2, Global.Config.SizeCCD.Height / 2), 0, AnchorPoint.None);

         
            Global.TypeCrop = TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;

            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;
        }

        private void btnMask_Click(object sender, EventArgs e)
        {
            Global.TypeCrop = TypeCrop.Mask;
            Propety.TypeCrop = Global.TypeCrop;
            if (Propety.rotMask == null)
            {
                Propety.rotMask = DataTool.NewRotRect(TypeCrop.Mask); 
            }
            btnElip.IsCLick = Propety.rotMask.Shape == ShapeType.Ellipse ? true : false;
            btnRect.IsCLick = Propety.rotMask.Shape == ShapeType.Rectangle ? true : false;

        }
    }
}

