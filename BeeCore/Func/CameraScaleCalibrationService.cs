using System;
using BeeCppCli;
using BeeGlobal;
using OpenCvSharp;

namespace BeeCore
{
    public class CameraScaleCalibrationService
    {
        public ScaleCalibrationCli DetectScaleSample(Mat frame, double realSizeMm)
        {
            if (frame == null || frame.Empty())
                return new ScaleCalibrationCli { Found = false, Message = "Input frame is empty." };

            return CameraCalibration.DetectScaleSample(
                frame.Data,
                frame.Width,
                frame.Height,
                (int)frame.Step(),
                frame.Channels(),
                realSizeMm);
        }

        public bool ApplyScaleToProfile(CameraCalibrationProfile profile, ScaleCalibrationCli scaleResult)
        {
            if (profile == null || scaleResult == null || !scaleResult.Found || scaleResult.MmPerPixel <= 0.0)
                return false;

            profile.ScaleMmPerPixel = scaleResult.MmPerPixel;
            profile.ScaleSampleRealSizeMm = scaleResult.RealSizeMm;
            profile.ScaleSamplePixelSize = scaleResult.PixelSize;
            profile.UpdatedAt = DateTime.Now;
            return true;
        }

        public bool ApplyScaleToGlobalConfig(CameraCalibrationProfile profile)
        {
            if (profile == null || profile.ScaleMmPerPixel <= 0.0 || Global.Config == null)
                return false;

            Global.Config.Scale = (float)profile.ScaleMmPerPixel;
            return true;
        }
    }
}
