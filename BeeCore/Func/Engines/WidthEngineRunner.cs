using BeeCore.Algorithm;
using BeeGlobal;
using System;

namespace BeeCore.Funtion.Engines
{
    public static class WidthEngineRunner
    {
        public static WidthViewState ReadFromOwner(PropetyTool owner, Width propety)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (propety == null)
                throw new ArgumentNullException("propety");

            return new WidthViewState
            {
                ScoreMin = owner.MinValue,
                ScoreMax = owner.MaxValue,
                ScoreStep = owner.StepValue,
                Score = owner.Score,
                ThresholdBinary = propety.ThresholdBinary,
                Scale = propety.Scale,
                MinInliers = propety.MinInliers,
                MaximumLine = propety.MaximumLine,
                RansacIterations = propety.RansacIterations,
                RansacThreshold = propety.RansacThreshold,
                MinLen = propety.MinLen,
                MaxLen = propety.MaxLen,
                MethordEdge = propety.MethordEdge,
                LineOrientation = propety.LineOrientation,
                SegmentStatType = propety.SegmentStatType,
                GapExtremum = propety.GapExtremum,
                SizeClose = propety.SizeClose,
                SizeOpen = propety.SizeOpen,
                SizeClearSmall = propety.SizeClearsmall,
                SizeClearBig = propety.SizeClearBig,
                IsClose = propety.IsClose,
                IsOpen = propety.IsOpen,
                IsClearNoiseSmall = propety.IsClearNoiseSmall,
                IsClearNoiseBig = propety.IsClearNoiseBig
            };
        }

        public static void MarkOwnerWaiting(PropetyTool owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            owner.StatusTool = StatusTool.WaitCheck;
        }

        public static void ApplyScoreToOwner(PropetyTool owner, float score)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            owner.Score = score;
        }

        public static void BeginCalibration(Width propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.IsCalibs = true;
        }

        public static WidthRunResult Run(Width propety, RectRotate rotArea, RectRotate rotMask, bool complete)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.DoWork(rotArea, rotMask);
            if (complete)
                propety.Complete();

            return WidthRunResult.From(propety);
        }

        public static bool TryStartSelectedTool()
        {
            PropetyTool tool = Common.TryGetTool(Global.IndexToolSelected);
            if (tool == null)
                return false;

            tool.RunToolAsync();
            return true;
        }
    }

    public sealed class WidthViewState
    {
        public float ScoreMin { get; set; }
        public float ScoreMax { get; set; }
        public float ScoreStep { get; set; }
        public float Score { get; set; }
        public int ThresholdBinary { get; set; }
        public float Scale { get; set; }
        public int MinInliers { get; set; }
        public int MaximumLine { get; set; }
        public int RansacIterations { get; set; }
        public double RansacThreshold { get; set; }
        public int MinLen { get; set; }
        public int MaxLen { get; set; }
        public MethordEdge MethordEdge { get; set; }
        public LineOrientation LineOrientation { get; set; }
        public SegmentStatType SegmentStatType { get; set; }
        public GapExtremum GapExtremum { get; set; }
        public int SizeClose { get; set; }
        public int SizeOpen { get; set; }
        public int SizeClearSmall { get; set; }
        public int SizeClearBig { get; set; }
        public bool IsClose { get; set; }
        public bool IsOpen { get; set; }
        public bool IsClearNoiseSmall { get; set; }
        public bool IsClearNoiseBig { get; set; }
    }

    public sealed class WidthRunResult
    {
        public bool IsOk { get; set; }
        public float ScoreResult { get; set; }
        public float WidthResult { get; set; }
        public int SegmentCount { get; set; }

        public static WidthRunResult From(Width propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            PropetyTool owner = Common.TryGetTool(propety.IndexThread, propety.Index);
            return new WidthRunResult
            {
                IsOk = propety.GapResult.line2Ds != null,
                ScoreResult = owner != null ? owner.ScoreResult : 0,
                WidthResult = propety.WidthResult,
                SegmentCount = propety.GapResult.line2Ds != null ? propety.GapResult.line2Ds.Count : 0
            };
        }
    }
}
