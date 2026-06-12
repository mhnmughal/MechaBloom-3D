using UnityEngine;

namespace MechaBloom
{
    public sealed class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource uiSfxSource;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private AudioClip uiButtonClick;
        [SerializeField] private AudioClip objectSelect;
        [SerializeField] private AudioClip gearRotate;
        [SerializeField] private AudioClip valveOpen;
        [SerializeField] private AudioClip valveClose;
        [SerializeField] private AudioClip waterFlowStart;
        [SerializeField] private AudioClip energyFlowStart;
        [SerializeField] private AudioClip plantSprout;
        [SerializeField] private AudioClip plantBloom;
        [SerializeField] private AudioClip wrongAction;
        [SerializeField] private AudioClip blockedFlow;
        [SerializeField] private AudioClip notEnoughEnergy;
        [SerializeField] private AudioClip undo;
        [SerializeField] private AudioClip hint;
        [SerializeField] private AudioClip levelComplete;
        [SerializeField] private AudioClip gameOver;
        [SerializeField] private AudioClip starReward;

        private void Start()
        {
            ApplySavedVolumes();
        }

        public void SetMusicVolume(float value)
        {
            if (saveManager != null)
            {
                saveManager.MusicVolume = value;
            }

            if (musicSource != null)
            {
                musicSource.volume = Mathf.Clamp01(value);
            }
        }

        public void SetSfxVolume(float value)
        {
            if (saveManager != null)
            {
                saveManager.SfxVolume = value;
            }

            if (sfxSource != null)
            {
                sfxSource.volume = Mathf.Clamp01(value);
            }

            if (uiSfxSource != null)
            {
                uiSfxSource.volume = Mathf.Clamp01(value);
            }
        }

        public void PlayUIButton() => Play(uiSfxSource, uiButtonClick);
        public void PlayObjectSelect() => Play(sfxSource, objectSelect);
        public void PlayGearRotate() => Play(sfxSource, gearRotate);
        public void PlayValveOpen() => Play(sfxSource, valveOpen);
        public void PlayValveClose() => Play(sfxSource, valveClose);
        public void PlayWaterFlowStart() => Play(sfxSource, waterFlowStart);
        public void PlayEnergyFlowStart() => Play(sfxSource, energyFlowStart);
        public void PlayPlantSprout() => Play(sfxSource, plantSprout);
        public void PlayPlantBloom() => Play(sfxSource, plantBloom);
        public void PlayWrongAction() => Play(sfxSource, wrongAction);
        public void PlayBlockedFlow() => Play(sfxSource, blockedFlow);
        public void PlayNotEnoughEnergy() => Play(sfxSource, notEnoughEnergy);
        public void PlayUndo() => Play(sfxSource, undo);
        public void PlayHint() => Play(sfxSource, hint);
        public void PlayLevelComplete() => Play(sfxSource, levelComplete);
        public void PlayGameOver() => Play(sfxSource, gameOver);
        public void PlayStarReward() => Play(sfxSource, starReward);

        private void ApplySavedVolumes()
        {
            SetMusicVolume(saveManager != null ? saveManager.MusicVolume : 0.7f);
            SetSfxVolume(saveManager != null ? saveManager.SfxVolume : 0.8f);
        }

        private static void Play(AudioSource source, AudioClip clip)
        {
            if (source != null && clip != null)
            {
                source.PlayOneShot(clip);
            }
        }
    }
}
