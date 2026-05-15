using OpenCvSharp;
using System;
using System.Collections.Generic;

namespace BeeCore.Algorithm
{
    internal static class ContourGeometry
    {
        internal static double DistancePointLine(Point2f p, FilletCornerMeasure.LineAB line)
        {
            return Math.Abs(line.A * p.X + line.B * p.Y + line.C);
        }

        internal static double DistancePointLine(Point2f p, FilletCornerMeasure2.LineAB line)
        {
            return Math.Abs(line.A * p.X + line.B * p.Y + line.C);
        }

        internal static int NearestIndex(Point[] contour, Point2f point)
        {
            int index = -1;
            double best = double.MaxValue;
            for (int i = 0; i < contour.Length; i++)
            {
                double dx = contour[i].X - point.X;
                double dy = contour[i].Y - point.Y;
                double distanceSquared = dx * dx + dy * dy;
                if (distanceSquared < best)
                {
                    best = distanceSquared;
                    index = i;
                }
            }

            return index;
        }

        internal static double ArcLengthBetween(Point[] contour, Point2f firstPoint, Point2f secondPoint)
        {
            int firstIndex = NearestIndex(contour, firstPoint);
            int secondIndex = NearestIndex(contour, secondPoint);
            if (firstIndex < 0 || secondIndex < 0) return double.NaN;

            double forwardLength = 0;
            for (int i = firstIndex; i != secondIndex; i = (i + 1) % contour.Length)
            {
                var a = contour[i];
                var b = contour[(i + 1) % contour.Length];
                forwardLength += Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
            }

            double backwardLength = 0;
            for (int i = secondIndex; i != firstIndex; i = (i + 1) % contour.Length)
            {
                var a = contour[i];
                var b = contour[(i + 1) % contour.Length];
                backwardLength += Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
            }

            return Math.Min(forwardLength, backwardLength);
        }

        internal static double LineDirectionDeg(FilletCornerMeasure.LineAB line)
        {
            return LineDirectionDeg(line.P1, line.P2);
        }

        internal static double LineDirectionDeg(FilletCornerMeasure2.LineAB line)
        {
            return LineDirectionDeg(line.P1, line.P2);
        }

        private static double LineDirectionDeg(Point2f firstPoint, Point2f secondPoint)
        {
            double angle = Math.Atan2(secondPoint.Y - firstPoint.Y, secondPoint.X - firstPoint.X) * 180.0 / Math.PI;
            if (angle < 0) angle += 180.0;
            return angle;
        }

        internal static double AngleDiffTo90(FilletCornerMeasure.LineAB firstLine, FilletCornerMeasure.LineAB secondLine)
        {
            return AngleDiffTo90(firstLine.P1, firstLine.P2, secondLine.P1, secondLine.P2);
        }

        internal static double AngleDiffTo90(FilletCornerMeasure2.LineAB firstLine, FilletCornerMeasure2.LineAB secondLine)
        {
            return AngleDiffTo90(firstLine.P1, firstLine.P2, secondLine.P1, secondLine.P2);
        }

        private static double AngleDiffTo90(Point2f firstStart, Point2f firstEnd, Point2f secondStart, Point2f secondEnd)
        {
            var firstVector = new Point2f(firstEnd.X - firstStart.X, firstEnd.Y - firstStart.Y);
            var secondVector = new Point2f(secondEnd.X - secondStart.X, secondEnd.Y - secondStart.Y);
            double dot = firstVector.X * secondVector.X + firstVector.Y * secondVector.Y;
            double firstNorm = Math.Sqrt(firstVector.X * firstVector.X + firstVector.Y * firstVector.Y);
            double secondNorm = Math.Sqrt(secondVector.X * secondVector.X + secondVector.Y * secondVector.Y);
            if (firstNorm < 1e-6 || secondNorm < 1e-6) return 0;

            double cosine = dot / (firstNorm * secondNorm);
            cosine = Math.Max(-1, Math.Min(1, cosine));
            double angle = Math.Acos(cosine) * 180.0 / Math.PI;
            return Math.Abs(angle - 90.0);
        }

        internal static List<Point2f> GetIntersections(FilletCornerMeasure.LineAB line, Point[] contour)
        {
            return GetIntersections(line.A, line.B, line.C, contour);
        }

        internal static List<Point2f> GetIntersections(FilletCornerMeasure2.LineAB line, Point[] contour)
        {
            return GetIntersections(line.A, line.B, line.C, contour);
        }

        private static List<Point2f> GetIntersections(float lineA, float lineB, float lineC, Point[] contour)
        {
            var intersections = new List<Point2f>();
            for (int i = 0; i < contour.Length; i++)
            {
                var firstPoint = (Point2f)contour[i];
                var secondPoint = (Point2f)contour[(i + 1) % contour.Length];
                if (LineSegmentIntersect(lineA, lineB, lineC, firstPoint, secondPoint, out var intersection))
                    intersections.Add(intersection);
            }

            return intersections;
        }

        private static bool LineSegmentIntersect(float lineA, float lineB, float lineC, Point2f firstPoint, Point2f secondPoint, out Point2f intersection)
        {
            intersection = default;
            double firstValue = lineA * firstPoint.X + lineB * firstPoint.Y + lineC;
            double secondValue = lineA * secondPoint.X + lineB * secondPoint.Y + lineC;

            if (Math.Abs(firstValue) < 1e-6) { intersection = firstPoint; return true; }
            if (Math.Abs(secondValue) < 1e-6) { intersection = secondPoint; return true; }

            if ((firstValue < 0 && secondValue > 0) || (firstValue > 0 && secondValue < 0))
            {
                double t = firstValue / (firstValue - secondValue);
                intersection = new Point2f(
                    (float)(firstPoint.X + t * (secondPoint.X - firstPoint.X)),
                    (float)(firstPoint.Y + t * (secondPoint.Y - firstPoint.Y))
                );
                return true;
            }

            return false;
        }
    }
}
