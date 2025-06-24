using System;
using System.Collections.Generic;
using OpenCvSharp;
using OpenCvSharp.XImgProc;
namespace BeeCore.Algorithm
{


    public static class LineDetector
    {
        //OpenCvSharp.XImgProc.StructuredEdgeDetection
        ///// <summary>
        ///// Phát hiện đoạn thẳng bằng EDLines rồi lọc theo góc.
        ///// </summary>
        ///// <param name="gray">Ảnh xám đầu vào (Mat GRAY).</param>
        ///// <param name="keepHorizontal">Giữ đường ngang? (≈ 0 °/180 °).</param>
        ///// <param name="keepVertical">Giữ đường dọc? (≈ 90 °).</param>
        ///// <param name="tolDeg">Sai số góc cho phép (độ).</param>
        ///// <param name="edParams">Tham số EdgeDrawing tuỳ chọn (null = mặc định).</param>
        ///// <returns>Mảng Vec4f(x1,y1,x2,y2) của các đoạn thẳng đạt tiêu chí.</returns>
        //public static Vec4f[] DetectHVLines(
        //    Mat gray,
        //    bool keepHorizontal = true,
        //    bool keepVertical = true,
        //    double tolDeg = 10.0,
        //    EdgeDrawing.Params? edParams = null)
        //{
        //    if (gray.Empty() || gray.Type() != MatType.CV_8UC1)
        //        throw new ArgumentException("Ảnh đầu vào phải là Mat xám 8‑bit.");

        //    // 1) Tạo EdgeDrawing với tham số người dùng (hoặc mặc định)
        //    var ed = edParams is null
        //             ? EdgeDrawing.Create()
        //             : EdgeDrawing.Create(edParams);

        //    // 2) EDLines
        //    ed.DetectEdges(gray);
        //    ed.DetectLines(out Vec4f[] allLines);

        //    // 3) Lọc theo góc
        //    var filtered = new List<Vec4f>(allLines.Length);
        //    foreach (var l in allLines)
        //    {
        //        double dx = l.Item2 - l.Item0;
        //        double dy = l.Item3 - l.Item1;
        //        double angle = Math.Abs(Math.Atan2(dy, dx) * 180.0 / Math.PI); // [0..180]

        //        bool isHorizontal = (angle < tolDeg) || (angle > 180 - tolDeg);
        //        bool isVertical = Math.Abs(angle - 90) < tolDeg;

        //        if ((keepHorizontal && isHorizontal) || (keepVertical && isVertical))
        //            filtered.Add(l);
        //    }

        //    return filtered.ToArray();
      //  }
    }

}
