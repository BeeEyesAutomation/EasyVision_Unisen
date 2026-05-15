using BeeCore.Algorithm;
using BeeGlobal;
using System;

namespace BeeCore.Funtion.Engines
{
    public static class Edge2EngineRunner
    {
        public static Edge2ViewState ReadFromOwner(PropetyTool owner, Edge2 propety)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (propety == null)
                throw new ArgumentNullException("propety");

            return new Edge2ViewState
            {
                ScoreMin = owner.MinValue,
                ScoreMax = owner.MaxValue,
                ScoreStep = owner.StepValue,
                Score = owner.Score,
                ThresholdBinary = propety.ThresholdBinary,
                Scale = propety.Scale,
                AngleRange = propety.AngleRange,
                MinInliers = propety.MinInliers,
                MethordEdge = propety.MethordEdge,
                LineOrientation = propety.LineOrientation,
                LineDirScan = propety.LineDirScan,
                RansacThreshold = propety.RansacThreshold,
                RansacIterations = propety.RansacIterations,
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

        public static void BeginCalibration(Edge2 propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.IsCalibs = true;
        }

        public static Edge2RunResult Run(Edge2 propety, RectRotate rotArea, RectRotate rotMask, bool complete)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.DoWork(rotArea, rotMask);
            if (complete)
                propety.Complete();

            return Edge2RunResult.From(propety);
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

    public sealed class Edge2ViewState
    {
        public float ScoreMin { get; set; }
        public float ScoreMax { get; set; }
        public float ScoreStep { get; set; }
        public float Score { get; set; }
        public int ThresholdBinary { get; set; }
        public float Scale { get; set; }
        public int AngleRange { get; set; }
        public int MinInliers { get; set; }
        public MethordEdge MethordEdge { get; set; }
        public LineOrientation LineOrientation { get; set; }
        public LineDirScan LineDirScan { get; set; }
        public double RansacThreshold { get; set; }
        public int RansacIterations { get; set; }
        public int SizeClose { get; set; }
        public int SizeOpen { get; set; }
        public int SizeClearSmall { get; set; }
        public int SizeClearBig { get; set; }
        public bool IsClose { get; set; }
        public bool IsOpen { get; set; }
        public bool IsClearNoiseSmall { get; set; }
        public bool IsClearNoiseBig { get; set; }
    }

    public sealed class Edge2RunResult
    {
        public Results Results { get; set; }
        public float ScoreResult { get; set; }
        public float WidthResult { get; set; }

        public static Edge2RunResult From(Edge2 propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            PropetyTool owner = Common.TryGetTool(propety.IndexThread, propety.Index);
            return new Edge2RunResult
            {
                Results = owner != null ? owner.Results : Results.None,
                ScoreResult = owner != null ? owner.ScoreResult : 0,
                WidthResult = propety.WidthResult
            };
        }
    }
}
