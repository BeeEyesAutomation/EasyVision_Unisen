using System;
using System.Collections.Generic;
using System.Linq;
using BeeCppCli;
using BeeGlobal;
using OpenCvSharp;

namespace BeeCore
{
    public class CameraCalibrationService
    {
        public CalibrationFrameCli DetectGrid(Mat frame, int patternType, int rows, int columns, double spacingMm, bool useFastCheck = true)
        {
            if (frame == null || frame.Empty())
                return new CalibrationFrameCli { Found = false, Message = "Input frame is empty." };

            return CameraCalibration.DetectGrid(
                frame.Data,
                frame.Width,
                frame.Height,
                (int)frame.Step(),
                frame.Channels(),
                CreateGridSpec(patternType, rows, columns, spacingMm, useFastCheck));
        }

        public CameraCalibrationProfile Solve(
            IEnumerable<CalibrationFrameCli> frames,
            string cameraId,
            string cameraName,
            int patternType,
            int rows,
            int columns,
            double spacingMm,
            bool buildRectificationHomography = true)
        {
            CalibrationFrameCli[] frameArray = frames == null ? new CalibrationFrameCli[0] : frames.Where(f => f != null).ToArray();
            CameraCalibrationProfileCli cliProfile = CameraCalibration.Solve(
                frameArray,
                CreateGridSpec(patternType, rows, columns, spacingMm, true),
                buildRectificationHomography);

            CameraCalibrationProfile profile = new CameraCalibrationProfile
            {
                ProfileId = Guid.NewGuid().ToString("N"),
                CameraId = cameraId ?? string.Empty,
                CameraName = cameraName ?? string.Empty,
                ImageWidth = cliProfile.ImageWidth,
                ImageHeight = cliProfile.ImageHeight,
                PatternType = patternType,
                PatternRows = rows,
                PatternColumns = columns,
                PatternSpacingMm = spacingMm,
                CameraMatrix = cliProfile.CameraMatrix ?? new double[0],
                DistortionCoefficients = cliProfile.DistortionCoefficients ?? new double[0],
                RectificationHomography = cliProfile.RectificationHomography ?? new double[0],
                MeanReprojectionError = cliProfile.MeanReprojectionError,
                PerFrameErrors = cliProfile.PerFrameErrors ?? new double[0],
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsValidatedForPreview = cliProfile.Success,
                IsValidatedForInspection = false
            };
            return profile;
        }

        public bool ValidateProfileForFrame(CameraCalibrationProfile profile, int width, int height, string cameraId)
        {
            if (profile == null)
                return false;
            if (width <= 0 || height <= 0)
                return false;
            if (profile.ImageWidth != width || profile.ImageHeight != height)
                return false;
            if (!string.IsNullOrWhiteSpace(profile.CameraId) &&
                !string.Equals(profile.CameraId, cameraId ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                return false;

            return profile.CameraMatrix != null &&
                   profile.CameraMatrix.Length == 9 &&
                   profile.DistortionCoefficients != null &&
                   profile.DistortionCoefficients.Length > 0;
        }

        public LiveGuidanceCli AnalyzeLiveGuidance(Mat frame, CameraCalibrationProfile profile, int patternType, int rows, int columns, double spacingMm)
        {
            if (frame == null || frame.Empty())
                return new LiveGuidanceCli { Found = false, Action = CalibrationGuidanceActionCli.TargetNotFound, Message = "Input frame is empty." };

            return CameraCalibration.AnalyzeLiveGuidance(
                frame.Data,
                frame.Width,
                frame.Height,
                (int)frame.Step(),
                frame.Channels(),
                CreateGridSpec(patternType, rows, columns, spacingMm, true),
                ToCliProfile(profile));
        }

        internal static CameraCalibrationProfileCli ToCliProfile(CameraCalibrationProfile profile)
        {
            if (profile == null)
                return null;

            return new CameraCalibrationProfileCli
            {
                Success = profile.IsValidatedForPreview || profile.IsValidatedForInspection,
                ImageWidth = profile.ImageWidth,
                ImageHeight = profile.ImageHeight,
                CameraMatrix = profile.CameraMatrix ?? new double[0],
                DistortionCoefficients = profile.DistortionCoefficients ?? new double[0],
                RectificationHomography = profile.RectificationHomography ?? new double[0],
                MeanReprojectionError = profile.MeanReprojectionError,
                PerFrameErrors = profile.PerFrameErrors ?? new double[0],
                ResidualVectors = new Point2dCli[0],
                Message = string.Empty
            };
        }

        private static CalibrationGridSpecCli CreateGridSpec(int patternType, int rows, int columns, double spacingMm, bool useFastCheck)
        {
            return new CalibrationGridSpecCli
            {
                PatternType = (CalibrationPatternTypeCli)patternType,
                Rows = rows,
                Columns = columns,
                SpacingMm = spacingMm,
                UseFastCheck = useFastCheck
            };
        }
    }
}
