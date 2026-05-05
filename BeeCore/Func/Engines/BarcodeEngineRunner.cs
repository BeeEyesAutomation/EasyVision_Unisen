using BeeGlobal;
using System;

namespace BeeCore.Funtion.Engines
{
    public static class BarcodeEngineRunner
    {
        public static BarcodeViewState ReadFromOwner(PropetyTool owner, Barcode propety)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (propety == null)
                throw new ArgumentNullException("propety");

            return new BarcodeViewState
            {
                ScoreMin = owner.MinValue,
                ScoreMax = owner.MaxValue,
                ScoreStep = owner.StepValue,
                Score = owner.Score,
                IsSendResult = propety.IsSendResult,
                AddPLC = propety.AddPLC,
                ModeCheck = propety.ModeCheck,
                IndexProgChoose = propety.IndexProgChoose,
                OffSetArea = propety.OffSetArea
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

        public static BarcodeRunResult Run(Barcode propety, RectRotate rotArea, RectRotate rotMask, bool complete)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.DoWork(rotArea, rotMask);
            if (complete)
                propety.Complete();

            return BarcodeRunResult.From(propety);
        }

        public static void Scan(Barcode propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.IsScan = true;
            propety.Scan();
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

    public sealed class BarcodeViewState
    {
        public float ScoreMin { get; set; }
        public float ScoreMax { get; set; }
        public float ScoreStep { get; set; }
        public float Score { get; set; }
        public bool IsSendResult { get; set; }
        public string AddPLC { get; set; }
        public ModeCheck ModeCheck { get; set; }
        public int IndexProgChoose { get; set; }
        public int OffSetArea { get; set; }
    }

    public sealed class BarcodeRunResult
    {
        public Results Results { get; set; }
        public float ScoreResult { get; set; }
        public int ResultCount { get; set; }

        public static BarcodeRunResult From(Barcode propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            PropetyTool owner = Common.TryGetTool(propety.IndexThread, propety.Index);
            return new BarcodeRunResult
            {
                Results = owner != null ? owner.Results : Results.None,
                ScoreResult = owner != null ? owner.ScoreResult : 0,
                ResultCount = propety.rectRotates != null ? propety.rectRotates.Count : 0
            };
        }
    }
}
