using BeeGlobal;
using System;
using System.ComponentModel;

namespace BeeCore.Funtion.Engines
{
    public static class EdgePixelEngineRunner
    {
        public static EdgePixelViewState ReadFromOwner(PropetyTool owner, EdgePixel propety)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (propety == null)
                throw new ArgumentNullException("propety");

            return new EdgePixelViewState
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

        public static EdgePixelRunResult Run(EdgePixel propety, RectRotate rotArea, RectRotate rotMask, bool complete)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.DoWork(rotArea, rotMask);
            if (complete)
                propety.Complete();

            return EdgePixelRunResult.From(propety);
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

    public sealed class EdgePixelViewState
    {
        public float ScoreMin { get; set; }
        public float ScoreMax { get; set; }
        public float ScoreStep { get; set; }
        public float Score { get; set; }
    }

    public sealed class EdgePixelRunResult
    {
        public Results Results { get; set; }
        public float ScoreResult { get; set; }
        public int PixelResult { get; set; }

        public static EdgePixelRunResult From(EdgePixel propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            PropetyTool owner = Common.TryGetTool(propety.IndexThread, propety.Index);
            return new EdgePixelRunResult
            {
                Results = owner != null ? owner.Results : Results.None,
                ScoreResult = owner != null ? owner.ScoreResult : 0,
                PixelResult = propety.PxResult
            };
        }
    }
}
