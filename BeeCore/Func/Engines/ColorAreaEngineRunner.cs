using BeeGlobal;
using System;

namespace BeeCore.Funtion.Engines
{
    public static class ColorAreaEngineRunner
    {
        public static ColorAreaViewState ReadFromOwner(PropetyTool owner, ColorArea propety)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (propety == null)
                throw new ArgumentNullException("propety");

            return new ColorAreaViewState
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

        public static void BeginCalibration(ColorArea propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.IsCalib = true;
        }

        public static ColorAreaRunResult Run(ColorArea propety, RectRotate rotArea, RectRotate rotMask, bool complete)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.DoWork(rotArea, rotMask);
            if (complete)
                propety.Complete();

            return ColorAreaRunResult.From(propety);
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

    public sealed class ColorAreaViewState
    {
        public float ScoreMin { get; set; }
        public float ScoreMax { get; set; }
        public float ScoreStep { get; set; }
        public float Score { get; set; }
    }

    public sealed class ColorAreaRunResult
    {
        public Results Results { get; set; }
        public float ScoreResult { get; set; }
        public int PixelResult { get; set; }

        public static ColorAreaRunResult From(ColorArea propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            PropetyTool owner = Common.TryGetTool(propety.IndexThread, propety.Index);
            return new ColorAreaRunResult
            {
                Results = owner != null ? owner.Results : Results.None,
                ScoreResult = owner != null ? owner.ScoreResult : 0,
                PixelResult = propety.pxRS
            };
        }
    }
}
