using BeeGlobal;
using System;
using System.ComponentModel;

namespace BeeCore.Funtion.Engines
{
    public static class CornerEngineRunner
    {
        public static CornerViewState ReadFromOwner(PropetyTool owner, MeasureCorner propety)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (propety == null)
                throw new ArgumentNullException("propety");

            return new CornerViewState
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

        public static CornerRunResult Run(MeasureCorner propety, RectRotate rotArea, RectRotate rotMask, bool complete)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.DoWork(rotArea, rotMask);
            if (complete)
                propety.Complete();

            return CornerRunResult.From(propety);
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

    public sealed class CornerViewState
    {
        public float ScoreMin { get; set; }
        public float ScoreMax { get; set; }
        public float ScoreStep { get; set; }
        public float Score { get; set; }
    }

    public sealed class CornerRunResult
    {
        public Results Results { get; set; }
        public float ScoreResult { get; set; }

        public static CornerRunResult From(MeasureCorner propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            PropetyTool owner = Common.TryGetTool(propety.IndexThread, propety.Index);
            return new CornerRunResult
            {
                Results = owner != null ? owner.Results : Results.None,
                ScoreResult = owner != null ? owner.ScoreResult : 0
            };
        }
    }
}
