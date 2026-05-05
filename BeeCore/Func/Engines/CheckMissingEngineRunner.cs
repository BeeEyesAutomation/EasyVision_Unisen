using BeeGlobal;
using System;

namespace BeeCore.Funtion.Engines
{
    public static class CheckMissingEngineRunner
    {
        public static CheckMissingViewState ReadFromOwner(PropetyTool owner, CheckMissing propety)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (propety == null)
                throw new ArgumentNullException("propety");

            return new CheckMissingViewState
            {
                ScoreMin = owner.MinValue,
                ScoreMax = owner.MaxValue,
                ScoreStep = owner.StepValue,
                Score = owner.Score,
                ScorePattern = propety.ScorePattern
            };
        }

        public static void MarkOwnerWaiting(PropetyTool owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            owner.StatusTool = StatusTool.WaitCheck;
        }

        public static void ApplyToleranceToOwner(PropetyTool owner, float tolerance)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            owner.Score = tolerance;
        }

        public static void ApplyPatternScore(CheckMissing propety, int score)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.ScorePattern = score;
        }

        public static CheckMissingRunResult Run(CheckMissing propety, RectRotate rotArea, RectRotate rotMask, bool complete)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.DoWork(rotArea, rotMask);
            if (complete)
                propety.Complete();

            return CheckMissingRunResult.From(propety);
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

    public sealed class CheckMissingViewState
    {
        public float ScoreMin { get; set; }
        public float ScoreMax { get; set; }
        public float ScoreStep { get; set; }
        public float Score { get; set; }
        public int ScorePattern { get; set; }
    }

    public sealed class CheckMissingRunResult
    {
        public Results Results { get; set; }
        public float ScoreResult { get; set; }
        public int ResultCount { get; set; }

        public static CheckMissingRunResult From(CheckMissing propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            PropetyTool owner = Common.TryGetTool(propety.IndexThread, propety.Index);
            return new CheckMissingRunResult
            {
                Results = owner != null ? owner.Results : Results.None,
                ScoreResult = owner != null ? owner.ScoreResult : 0,
                ResultCount = propety.rectRotates != null ? propety.rectRotates.Count : 0
            };
        }
    }
}
