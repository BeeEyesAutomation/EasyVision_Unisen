using BeeGlobal;
using System;
using System.ComponentModel;

namespace BeeCore.Funtion.Engines
{
    public static class IntersectEngineRunner
    {
        public static IntersectViewState ReadFromOwner(PropetyTool owner, Intersect propety)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (propety == null)
                throw new ArgumentNullException("propety");

            return new IntersectViewState
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

        public static void BeginCalibration(Intersect propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.IsCalibs = true;
        }

        public static IntersectRunResult Run(Intersect propety, RectRotate rotArea, RectRotate rotMask, bool complete)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.DoWork(rotArea, rotMask);
            if (complete)
                propety.Complete();

            return IntersectRunResult.From(propety);
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

    public sealed class IntersectViewState
    {
        public float ScoreMin { get; set; }
        public float ScoreMax { get; set; }
        public float ScoreStep { get; set; }
        public float Score { get; set; }
    }

    public sealed class IntersectRunResult
    {
        public Results Results { get; set; }
        public float ScoreResult { get; set; }
        public string Location { get; set; }

        public static IntersectRunResult From(Intersect propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            PropetyTool owner = Common.TryGetTool(propety.IndexThread, propety.Index);
            return new IntersectRunResult
            {
                Results = owner != null ? owner.Results : Results.None,
                ScoreResult = owner != null ? owner.ScoreResult : 0,
                Location = owner != null ? owner.Location : string.Empty
            };
        }
    }
}
