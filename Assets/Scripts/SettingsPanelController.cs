using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MechaBloom
{
    public sealed class SettingsPanelController : MonoBehaviour
    {
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Toggle vibrationToggle;
        [SerializeField] private TMP_Text musicValueText;
        [SerializeField] private TMP_Text sfxValueText;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private SaveManager saveManager;

        private const float VolumeStep = 0.1f;

        private bool syncing;

        private void Awake()
        {
            BindEvents();
            SyncControls();
        }

        private void OnEnable()
        {
            SyncControls();
        }

        private void BindEvents()
        {
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            }

            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
            }

            if (vibrationToggle != null)
            {
                vibrationToggle.onValueChanged.AddListener(SetVibrationEnabled);
            }
        }

        private void SyncControls()
        {
            syncing = true;
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.SetValueWithoutNotify(saveManager != null ? saveManager.MusicVolume : 0.7f);
            }

            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.SetValueWithoutNotify(saveManager != null ? saveManager.SfxVolume : 0.8f);
            }

            if (vibrationToggle != null)
            {
                vibrationToggle.SetIsOnWithoutNotify(saveManager == null || saveManager.VibrationEnabled);
            }

            UpdateValueLabels();
            syncing = false;
        }

        private void SetMusicVolume(float value)
        {
            audioManager?.SetMusicVolume(value);
            UpdateValueLabels();
        }

        private void SetSfxVolume(float value)
        {
            audioManager?.SetSfxVolume(value);
            UpdateValueLabels();
        }

        private void SetVibrationEnabled(bool enabled)
        {
            if (!syncing)
            {
                saveManager?.SetVibrationEnabled(enabled);
            }
        }

        public void DecreaseMusicVolume()
        {
            AdjustVolume(musicVolumeSlider, -VolumeStep);
        }

        public void IncreaseMusicVolume()
        {
            AdjustVolume(musicVolumeSlider, VolumeStep);
        }

        public void DecreaseSfxVolume()
        {
            AdjustVolume(sfxVolumeSlider, -VolumeStep);
        }

        public void IncreaseSfxVolume()
        {
            AdjustVolume(sfxVolumeSlider, VolumeStep);
        }

        private static void AdjustVolume(Slider slider, float delta)
        {
            if (slider != null)
            {
                slider.value = Mathf.Clamp01(slider.value + delta);
            }
        }

        private void UpdateValueLabels()
        {
            if (musicValueText != null && musicVolumeSlider != null)
            {
                musicValueText.SetText($"{Mathf.RoundToInt(musicVolumeSlider.value * 100f)}%");
            }

            if (sfxValueText != null && sfxVolumeSlider != null)
            {
                sfxValueText.SetText($"{Mathf.RoundToInt(sfxVolumeSlider.value * 100f)}%");
            }
        }
    }
}
