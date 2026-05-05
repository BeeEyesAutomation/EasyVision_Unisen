using BeeGlobal;
using System;

namespace BeeCore.Funtion.Engines
{
    public static class OCREngineRunner
    {
        public static OCRViewState ReadFromOwner(PropetyTool owner, OCR propety)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (propety == null)
                throw new ArgumentNullException("propety");

            return new OCRViewState
            {
                ScoreMin = owner.MinValue,
                ScoreMax = owner.MaxValue,
                ScoreStep = owner.StepValue,
                Score = owner.Score,
                Matching = propety.Matching,
                LimitArea = propety.LimitArea,
                Clahe = propety.Clahe,
                Sigma = propety.Sigma,
                Blur = propety.Blur,
                Allow = propety.sAllow,
                IsCompareNoFixed = propety.IsCompareNoFixed,
                AddPLC = propety.AddPLC
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

        public static OCRRunResult Run(OCR propety, RectRotate rotArea, RectRotate rotMask, bool complete)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.DoWork(rotArea, rotMask);
            if (complete)
                propety.Complete();

            return OCRRunResult.From(propety);
        }

        public static bool LoadModel(OCR propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            return propety.SetModelOCR();
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

    public sealed class OCRViewState
    {
        public float ScoreMin { get; set; }
        public float ScoreMax { get; set; }
        public float ScoreStep { get; set; }
        public float Score { get; set; }
        public string Matching { get; set; }
        public int LimitArea { get; set; }
        public int Clahe { get; set; }
        public int Sigma { get; set; }
        public int Blur { get; set; }
        public string Allow { get; set; }
        public bool IsCompareNoFixed { get; set; }
        public string AddPLC { get; set; }
    }

    public sealed class OCRRunResult
    {
        public Results Results { get; set; }
        public float ScoreResult { get; set; }
        public int ResultCount { get; set; }
        public string Content { get; set; }

        public static OCRRunResult From(OCR propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            PropetyTool owner = Common.TryGetTool(propety.IndexThread, propety.Index);
            return new OCRRunResult
            {
                Results = owner != null ? owner.Results : Results.None,
                ScoreResult = owner != null ? owner.ScoreResult : 0,
                ResultCount = propety.rectRotates != null ? propety.rectRotates.Count : 0,
                Content = propety.Content
            };
        }
    }
}
