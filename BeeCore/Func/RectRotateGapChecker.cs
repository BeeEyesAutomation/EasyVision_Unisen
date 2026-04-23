using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BeeGlobal;


public sealed class AdjacentGapInfo
{
    public int IndexA { get; set; }
    public int IndexB { get; set; }

    public RectRotate BoxA { get; set; }
    public RectRotate BoxB { get; set; }
    public RectRotate BoxNG { get; set; }

    public float Distance { get; set; }
    public float ScorePercent { get; set; }
    public bool IsNG { get; set; }
}

public sealed class AdjacentGapCheckResult
{
    public bool IsOK { get; set; }
    public float AverageDistance { get; set; }

    public List<RectRotate> SortedBoxes { get; set; } = new List<RectRotate>();
    public List<AdjacentGapInfo> Gaps { get; set; } = new List<AdjacentGapInfo>();

    public AdjacentGapInfo FirstFailed { get; set; }
    public RectRotate BoxNG { get; set; }
}

public static class RectRotateGapChecker
{
    public static AdjacentGapCheckResult CheckAdjacentCenterGap(
        List<RectRotate> boxes,
        LineOrientation direction,
        bool calib,
        float inputAverageDistance,
        float minScorePercent)
    {
        var result = new AdjacentGapCheckResult();

        if (boxes == null || boxes.Count <= 1)
        {
            result.IsOK = true;
            result.SortedBoxes = boxes?.Where(x => x != null).ToList() ?? new List<RectRotate>();
            result.AverageDistance = calib ? 0f : inputAverageDistance;
            return result;
        }

        var valid = boxes.Where(x => x != null).ToList();
        if (valid.Count <= 1)
        {
            result.IsOK = true;
            result.SortedBoxes = valid;
            result.AverageDistance = calib ? 0f : inputAverageDistance;
            return result;
        }

        List<RectRotate> sorted = (direction == LineOrientation.Horizontal)
            ? valid.OrderBy(x => x._PosCenter.X).ThenBy(x => x._PosCenter.Y).ToList()
            : valid.OrderBy(x => x._PosCenter.Y).ThenBy(x => x._PosCenter.X).ToList();

        result.SortedBoxes = sorted;

        var distances = new List<float>();
        for (int i = 0; i < sorted.Count - 1; i++)
        {
            PointF a = sorted[i]._PosCenter;
            PointF b = sorted[i + 1]._PosCenter;

            float d = (direction == LineOrientation.Horizontal)
                ? Math.Abs(b.X - a.X)
                : Math.Abs(b.Y - a.Y);

            distances.Add(d);
        }

        if (distances.Count == 0)
        {
            result.IsOK = true;
            result.AverageDistance = calib ? 0f : inputAverageDistance;
            return result;
        }

        float avg = calib ? distances.Average() : inputAverageDistance;
        result.AverageDistance = avg;

        for (int i = 0; i < sorted.Count - 1; i++)
        {
            float d = distances[i];

            float score;
            if (avg <= 1e-6f)
            {
                score = d <= 1e-6f ? 100f : 0f;
            }
            else
            {
                float diffPercent = Math.Abs(d - avg) / avg * 100f;
                score = diffPercent;
                if (score < 0f) score = 0f;
            }

            bool isNg = !calib && (score > minScorePercent);

            var info = new AdjacentGapInfo
            {
                IndexA = i,
                IndexB = i + 1,
                BoxA = sorted[i],
                BoxB = sorted[i + 1],
                BoxNG = isNg ? CreateNgBox(sorted[i], sorted[i + 1]) : null,
                Distance = d,
                ScorePercent = score,
                IsNG = isNg
            };

            result.Gaps.Add(info);

            if (isNg)
            {
                result.IsOK = false;
                result.FirstFailed = info;
                result.BoxNG = info.BoxNG;
                return result;
            }
        }

        result.IsOK = true;
        return result;
    }

    private static RectRotate CreateNgBox(RectRotate boxA, RectRotate boxB)
    {
        if (boxA == null || boxB == null) return null;

        float width = Math.Max(1f, (boxA._rect.Width + boxB._rect.Width) * 0.5f);
        float height = Math.Max(1f, (boxA._rect.Height + boxB._rect.Height) * 0.5f);
        var center = new PointF(
            (boxA._PosCenter.X + boxB._PosCenter.X) * 0.5f,
            (boxA._PosCenter.Y + boxB._PosCenter.Y) * 0.5f);
        float angle = AverageAngle(boxA._rectRotation, boxB._rectRotation);

        var boxNG = new RectRotate(
            new RectangleF(-width / 2f, -height / 2f, width, height),
            center,
            angle,
            AnchorPoint.None);
        boxNG.Name = "NG";
        boxNG.IsOK = false;
        return boxNG;
    }

    private static float AverageAngle(float angleA, float angleB)
    {
        double radA = angleA * Math.PI / 180.0;
        double radB = angleB * Math.PI / 180.0;
        double x = Math.Cos(radA) + Math.Cos(radB);
        double y = Math.Sin(radA) + Math.Sin(radB);

        if (Math.Abs(x) < 1e-12 && Math.Abs(y) < 1e-12)
            return angleA;

        return (float)(Math.Atan2(y, x) * 180.0 / Math.PI);
    }

    public static bool CheckAdjacentCenterGapSimple(
        List<RectRotate> boxes,
        bool isVertical,
        bool calib,
        float inputAverageDistance,
        float minScorePercent,
        out float averageDistance)
    {
        var rs = CheckAdjacentCenterGap(
            boxes,
            isVertical ? LineOrientation.Vertical : LineOrientation.Horizontal,
            calib,
            inputAverageDistance,
            minScorePercent
        );

        averageDistance = rs.AverageDistance;
        return rs.IsOK;
    }
}
