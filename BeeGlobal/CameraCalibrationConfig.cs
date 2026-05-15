using System;
using System.Collections.Generic;
using System.Linq;

namespace BeeGlobal
{
    [Serializable]
    public class CameraCalibrationConfig
    {
        public string ActiveProfileId = string.Empty;
        public bool ApplyToPreview = false;
        public bool ApplyToInspectionInput = false;
        public List<CameraCalibrationProfile> Profiles = new List<CameraCalibrationProfile>();

        public CameraCalibrationProfile GetActiveProfile()
        {
            if (Profiles == null)
                Profiles = new List<CameraCalibrationProfile>();

            if (!string.IsNullOrWhiteSpace(ActiveProfileId))
            {
                CameraCalibrationProfile active = Profiles.FirstOrDefault(p => p != null && p.ProfileId == ActiveProfileId);
                if (active != null)
                    return active;
            }

            return Profiles.FirstOrDefault(p => p != null);
        }

        public CameraCalibrationProfile FindProfile(string cameraId, int width, int height)
        {
            if (Profiles == null)
                Profiles = new List<CameraCalibrationProfile>();

            return Profiles.FirstOrDefault(
                p => p != null &&
                     string.Equals(p.CameraId ?? string.Empty, cameraId ?? string.Empty, StringComparison.OrdinalIgnoreCase) &&
                     p.ImageWidth == width &&
                     p.ImageHeight == height);
        }
    }

    [Serializable]
    public class CameraCalibrationProfile
    {
        public string ProfileId = Guid.NewGuid().ToString("N");
        public string CameraId = string.Empty;
        public string CameraName = string.Empty;
        public int ImageWidth = 0;
        public int ImageHeight = 0;
        public string LensName = string.Empty;
        public double WorkingDistanceMm = 0.0;
        public int PatternType = 0;
        public int PatternRows = 0;
        public int PatternColumns = 0;
        public double PatternSpacingMm = 0.0;
        public double[] CameraMatrix = new double[0];
        public double[] DistortionCoefficients = new double[0];
        public double[] RectificationHomography = new double[0];
        public double MeanReprojectionError = 0.0;
        public double[] PerFrameErrors = new double[0];
        public double ScaleMmPerPixel = 0.0;
        public double ScaleSampleRealSizeMm = 0.0;
        public double ScaleSamplePixelSize = 0.0;
        public DateTime CreatedAt = DateTime.Now;
        public DateTime UpdatedAt = DateTime.Now;
        public bool IsValidatedForPreview = false;
        public bool IsValidatedForInspection = false;
    }
}
