using BeeGlobal;
using System;
using System.ComponentModel;

namespace BeeCore.Funtion.Engines
{
    public static class CropEngineRunner
    {
        public static CropViewState ReadFromOwner(PropetyTool owner, Crop propety)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (propety == null)
                throw new ArgumentNullException("propety");

            return new CropViewState
            {
                PathSaveImage = propety.PathSaveImage
            };
        }

        public static void MarkOwnerWaiting(PropetyTool owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            owner.StatusTool = StatusTool.WaitCheck;
        }

        public static CropRunResult Run(Crop propety, RectRotate rotArea, RectRotate rotMask, bool complete)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            propety.DoWork(rotArea, rotMask);
            if (complete)
                propety.Complete();

            return CropRunResult.From(propety);
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

    public sealed class CropViewState
    {
        public string PathSaveImage { get; set; }
    }

    public sealed class CropRunResult
    {
        public Results Results { get; set; }
        public bool HasImage { get; set; }

        public static CropRunResult From(Crop propety)
        {
            if (propety == null)
                throw new ArgumentNullException("propety");

            PropetyTool owner = Common.TryGetTool(propety.IndexThread, propety.Index);
            return new CropRunResult
            {
                Results = owner != null ? owner.Results : Results.None,
                HasImage = propety.matProcess != null && !propety.matProcess.Empty()
            };
        }
    }
}
