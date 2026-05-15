using BeeGlobal;
using System;

namespace BeeCore.Funtion.Engines
{
    public static class SegmentAIEngineRunner
    {
        public static SegmentAIRunResult Run(SegmentAI propety, RectRotate roiArea, RectRotate roiMask, bool complete)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.DoWork(roiArea, roiMask);
            if (complete)
                propety.Complete();

            return SegmentAIRunResult.From(propety);
        }
    }

    public sealed class SegmentAIRunResult
    {
        public bool IsOK { get; set; }
        public float Score { get; set; }
        public byte[] Mask { get; set; }
        public int MaskW { get; set; }
        public int MaskH { get; set; }
        public int DefectPixelCount { get; set; }

        public static SegmentAIRunResult From(SegmentAI propety)
        {
            return new SegmentAIRunResult
            {
                IsOK = propety.IsOK,
                Score = propety.lastScore,
                Mask = propety.lastMask,
                MaskW = propety.lastMaskW,
                MaskH = propety.lastMaskH,
                DefectPixelCount = propety.Counter
            };
        }
    }
}
