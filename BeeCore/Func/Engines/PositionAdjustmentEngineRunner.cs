using BeeGlobal;
using System;

namespace BeeCore.Funtion.Engines
{
    public static class PositionAdjustmentEngineRunner
    {
        public static PositionAdjustmentViewState ReadFromOwner(PropetyTool owner, PositionAdj propety)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (propety == null)
                throw new ArgumentNullException("propety");

            return new PositionAdjustmentViewState
            {
                ScoreMin = owner.MinValue,
                ScoreMax = owner.MaxValue,
                ScoreStep = owner.StepValue,
                Score = owner.Score
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

        public static void BeginCalibration(PositionAdj propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.IsCalib = true;
        }

        public static PositionAdjustmentRunResult Run(PositionAdj propety, RectRotate rotArea, RectRotate rotMask, bool complete)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.DoWork(rotArea, rotMask);
            if (complete)
                propety.Complete();

            return PositionAdjustmentRunResult.From(propety);
        }

        public static bool TryRunSelectedTool()
        {
            PropetyTool tool = Common.TryGetTool(Global.IndexToolSelected);
            if (tool == null)
                return false;

            tool.RunToolAsync();
            return true;
        }
    }

    public sealed class PositionAdjustmentViewState
    {
        public float ScoreMin { get; set; }
        public float ScoreMax { get; set; }
        public float ScoreStep { get; set; }
        public float Score { get; set; }
    }

    public sealed class PositionAdjustmentRunResult
    {
        public Results Results { get; set; }
        public float ScoreResult { get; set; }
        public int ResultCount { get; set; }

        public static PositionAdjustmentRunResult From(PositionAdj propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            PropetyTool owner = Common.TryGetTool(propety.IndexThread, propety.Index);
            return new PositionAdjustmentRunResult
            {
                Results = owner != null ? owner.Results : Results.None,
                ScoreResult = owner != null ? owner.ScoreResult : 0,
                ResultCount = propety.rectRotates != null ? propety.rectRotates.Count : 0
            };
        }
    }
}
