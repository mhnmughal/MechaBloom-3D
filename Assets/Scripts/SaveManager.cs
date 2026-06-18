using UnityEngine;

namespace MechaBloom
{
    public sealed class SaveManager : MonoBehaviour
    {
        private const string HighestUnlockedKey = "MechaBloom.HighestUnlockedLevel";
        private const string MusicVolumeKey = "MechaBloom.MusicVolume";
        private const string SfxVolumeKey = "MechaBloom.SfxVolume";
        private const string VibrationKey = "MechaBloom.Vibration";
        private const string TutorialSeenKey = "MechaBloom.TutorialSeen";

        public int HighestUnlockedLevel
        {
            get => PlayerPrefs.GetInt(HighestUnlockedKey, 1);
            set => PlayerPrefs.SetInt(HighestUnlockedKey, Mathf.Max(1, value));
        }

        public float MusicVolume
        {
            get => PlayerPrefs.GetFloat(MusicVolumeKey, 0.7f);
            set => PlayerPrefs.SetFloat(MusicVolumeKey, Mathf.Clamp01(value));
        }

        public float SfxVolume
        {
            get => PlayerPrefs.GetFloat(SfxVolumeKey, 0.8f);
            set => PlayerPrefs.SetFloat(SfxVolumeKey, Mathf.Clamp01(value));
        }

        public bool VibrationEnabled
        {
            get => PlayerPrefs.GetInt(VibrationKey, 1) == 1;
            set => PlayerPrefs.SetInt(VibrationKey, value ? 1 : 0);
        }

        public bool TutorialSeen
        {
            get => PlayerPrefs.GetInt(TutorialSeenKey, 0) == 1;
            set => PlayerPrefs.SetInt(TutorialSeenKey, value ? 1 : 0);
        }

        public int GetStars(int levelNumber)
        {
            return PlayerPrefs.GetInt(GetStarsKey(levelNumber), 0);
        }

        public void SetStars(int levelNumber, int stars)
        {
            var bestStars = Mathf.Max(GetStars(levelNumber), Mathf.Clamp(stars, 0, 3));
            PlayerPrefs.SetInt(GetStarsKey(levelNumber), bestStars);
            PlayerPrefs.Save();
        }

        public void UnlockLevel(int levelNumber)
        {
            HighestUnlockedLevel = Mathf.Max(HighestUnlockedLevel, levelNumber);
            PlayerPrefs.Save();
        }

        public void ResetProgress()
        {
            PlayerPrefs.DeleteKey(HighestUnlockedKey);
            PlayerPrefs.DeleteKey(TutorialSeenKey);
            for (var i = 1; i <= 12; i++)
            {
                PlayerPrefs.DeleteKey(GetStarsKey(i));
            }

            PlayerPrefs.Save();
        }

        public void SetVibrationEnabled(bool enabled)
        {
            VibrationEnabled = enabled;
            PlayerPrefs.Save();
        }

        private static string GetStarsKey(int levelNumber)
        {
            return $"MechaBloom.Level.{levelNumber}.Stars";
        }
    }
}
