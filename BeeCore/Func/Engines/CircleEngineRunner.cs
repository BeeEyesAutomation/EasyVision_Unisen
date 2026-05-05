using BeeCore.Algorithm;
using BeeGlobal;
using System;
using System.ComponentModel;

namespace BeeCore.Funtion.Engines
{
    public static class CircleEngineRunner
    {
        public static CircleViewState ReadFromOwner(PropetyTool owner, Circle propety)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (propety == null)
                throw new ArgumentNullException("propety");

            return new CircleViewState
            {
                ScoreMin = owner.MinValue,
                ScoreMax = owner.MaxValue,
                ScoreStep = owner.StepValue,
                Score = owner.Score,
                Scale = propety.Scale,
                Threshold = propety.Threshold,
                MinInliers = propety.MinInliers,
                Iterations = propety.Iterations,
                MinRadius = propety.MinRadius,
                MaxRadius = propety.MaxRadius,
                ThresholdBinary = propety.ThresholdBinary,
                SizeClose = propety.SizeClose,
                SizeOpen = propety.SizeOpen,
                SizeClearSmall = propety.SizeClearsmall,
                SizeClearBig = propety.SizeClearBig,
                IsClose = propety.IsClose,
                IsOpen = propety.IsOpen,
                IsClearNoiseSmall = propety.IsClearNoiseSmall,
                IsClearNoiseBig = propety.IsClearNoiseBig,
                MethordEdge = propety.MethordEdge,
                CircleScanDirection = propety.CircleScanDirection,
                AreaShape = propety.rotArea != null ? propety.rotArea.Shape : ShapeType.Rectangle,
                AreaIsWhite = propety.rotArea == null || propety.rotArea.IsWhite
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

        public static void BeginCalibration(Circle propety, bool isCalibrationEnabled)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.IsCalibs = isCalibrationEnabled;
        }

        public static CircleRunResult Run(Circle propety, RectRotate rotArea, RectRotate rotMask, bool complete)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.DoWork(rotArea, rotMask);
            if (complete)
                propety.Complete();

            return CircleRunResult.From(propety);
        }

        public static bool TryStartSelectedTool()
        {
            PropetyTool tool = Common.TryGetTool(Global.IndexToolSelected);
            return TryStartTool(tool);
        }

        public static bool TryStartTool(PropetyTool tool)
        {
            if (tool == null)
                return false;

            BackgroundWorker worker = tool.worker;
            if (worker == null || worker.IsBusy)
                return false;

            worker.RunWorkerAsync();
            return true;
        }
    }

    public sealed class CircleViewState
    {
        public float ScoreMin { get; set; }
        public float ScoreMax { get; set; }
        public float ScoreStep { get; set; }
        public float Score { get; set; }
        public float Scale { get; set; }
        public float Threshold { get; set; }
        public int MinInliers { get; set; }
        public int Iterations { get; set; }
        public float MinRadius { get; set; }
        public float MaxRadius { get; set; }
        public int ThresholdBinary { get; set; }
        public int SizeClose { get; set; }
        public int SizeOpen { get; set; }
        public int SizeClearSmall { get; set; }
        public int SizeClearBig { get; set; }
        public bool IsClose { get; set; }
        public bool IsOpen { get; set; }
        public bool IsClearNoiseSmall { get; set; }
        public bool IsClearNoiseBig { get; set; }
        public MethordEdge MethordEdge { get; set; }
        public CircleScanDirection CircleScanDirection { get; set; }
        public ShapeType AreaShape { get; set; }
        public bool AreaIsWhite { get; set; }
    }

    public sealed class CircleRunResult
    {
        public bool IsOk { get; set; }
        public float ScoreResult { get; set; }
        public float RadiusResult { get; set; }
        public int CircleCount { get; set; }
        public Exception Error { get; set; }

        public static CircleRunResult From(Circle propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            PropetyTool owner = Common.TryGetTool(propety.IndexThread, propety.Index);
            return new CircleRunResult
            {
                IsOk = propety.rectRotates != null && propety.rectRotates.Count > 0,
                ScoreResult = owner != null ? owner.ScoreResult : 0,
                RadiusResult = propety.RadiusResult,
                CircleCount = propety.rectRotates != null ? propety.rectRotates.Count : 0,
                Error = propety.exEr
            };
        }
    }
}
