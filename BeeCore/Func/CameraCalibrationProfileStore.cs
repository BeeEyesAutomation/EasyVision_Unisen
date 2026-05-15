using System;
using System.Collections.Generic;
using System.Linq;
using BeeGlobal;

namespace BeeCore
{
    public class CameraCalibrationProfileStore
    {
        public CameraCalibrationConfig EnsureLoaded()
        {
            if (Global.CameraCalibration == null)
                Global.CameraCalibration = LoadData.CameraCalibration();
            if (Global.CameraCalibration.Profiles == null)
                Global.CameraCalibration.Profiles = new List<CameraCalibrationProfile>();
            return Global.CameraCalibration;
        }

        public CameraCalibrationProfile GetActive()
        {
            return EnsureLoaded().GetActiveProfile();
        }

        public CameraCalibrationProfile FindProfile(string cameraId, int width, int height)
        {
            return EnsureLoaded().FindProfile(cameraId, width, height);
        }

        public CameraCalibrationProfile Upsert(CameraCalibrationProfile profile)
        {
            if (profile == null)
                return null;

            CameraCalibrationConfig config = EnsureLoaded();
            CameraCalibrationProfile existing = config.Profiles.FirstOrDefault(p => p != null && p.ProfileId == profile.ProfileId);
            if (existing == null)
            {
                if (string.IsNullOrWhiteSpace(profile.ProfileId))
                    profile.ProfileId = Guid.NewGuid().ToString("N");
                profile.CreatedAt = profile.CreatedAt == default(DateTime) ? DateTime.Now : profile.CreatedAt;
                config.Profiles.Add(profile);
                existing = profile;
            }
            else
            {
                int index = config.Profiles.IndexOf(existing);
                profile.CreatedAt = existing.CreatedAt == default(DateTime) ? DateTime.Now : existing.CreatedAt;
                config.Profiles[index] = profile;
                existing = profile;
            }

            existing.UpdatedAt = DateTime.Now;
            if (string.IsNullOrWhiteSpace(config.ActiveProfileId))
                config.ActiveProfileId = existing.ProfileId;
            return existing;
        }

        public void SetActive(string profileId)
        {
            CameraCalibrationConfig config = EnsureLoaded();
            if (config.Profiles.Any(p => p != null && p.ProfileId == profileId))
                config.ActiveProfileId = profileId ?? string.Empty;
        }

        public void Save()
        {
            SaveData.CameraCalibration(EnsureLoaded());
        }
    }
}
