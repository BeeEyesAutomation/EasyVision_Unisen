using BeeCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public static class ResultItemHelper
{
    /// <summary>
    /// Sắp xếp theo thứ tự đọc ảnh:
    /// Y từ trên xuống dưới, trong cùng 1 hàng thì X từ trái qua phải.
    /// Có gom hàng theo rowTolerance.
    /// </summary>
    public static List<ResultItem> SortByCenterXY(List<ResultItem> input)
    {
        if (input == null || input.Count == 0)
            return new List<ResultItem>();

        var valid = input
            .Where(x => x != null && x.rot != null)
            .ToList();

        if (valid.Count == 0)
            return new List<ResultItem>();

        // 1. Sort theo Y trước
        valid = valid
            .OrderBy(x => GetCenter(x).Y)
            .ToList();

        List<List<ResultItem>> rows = new List<List<ResultItem>>();

        foreach (var item in valid)
        {
            PointF c = GetCenter(item);

            if (rows.Count == 0)
            {
                rows.Add(new List<ResultItem> { item });
                continue;
            }

            var lastRow = rows.Last();
            float rowY = lastRow.Average(r => GetCenter(r).Y);

            // khoảng cách Y trung bình giữa 2 item
            float dy = Math.Abs(c.Y - rowY);

            // nếu lệch nhỏ -> cùng hàng
            if (dy < 40)   // tự động nhận hàng
            {
                lastRow.Add(item);
            }
            else
            {
                rows.Add(new List<ResultItem> { item });
            }
        }

        // 2. Sort từng hàng theo X
        foreach (var row in rows)
        {
            row.Sort((a, b) => GetCenter(a).X.CompareTo(GetCenter(b).X));
        }

        // 3. Gộp lại
        List<ResultItem> result = new List<ResultItem>();

        foreach (var row in rows)
            result.AddRange(row);

        return result;
    }

    private static PointF GetCenter(ResultItem item)
    {
        return item.rot._PosCenter;
    }
}