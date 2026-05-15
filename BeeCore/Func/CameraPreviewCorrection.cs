using BeeCppCli;
using BeeGlobal;
using OpenCvSharp;

namespace BeeCore
{
    public class CameraPreviewCorrection
    {
        private readonly CameraCalibrationService _service = new CameraCalibrationService();

        public bool TryCorrectPreview(Mat frame, CameraCalibrationProfile profile, out Mat corrected)
        {
            corrected = null;
            if (frame == null || frame.Empty())
                return false;
            if (!_service.ValidateProfileForFrame(profile, frame.Width, frame.Height, profile?.CameraId))
                return false;

            corrected = new Mat(frame.Rows, frame.Cols, frame.Type());
            bool ok = CameraCalibration.UndistortPreview(
                frame.Data,
                frame.Width,
                frame.Height,
                (int)frame.Step(),
                frame.Channels(),
                CameraCalibrationService.ToCliProfile(profile),
                corrected.Data,
                (int)corrected.Step(),
                corrected.Channels());
            if (!ok)
            {
                corrected.Dispose();
                corrected = null;
            }

            return ok;
        }
    }
}
