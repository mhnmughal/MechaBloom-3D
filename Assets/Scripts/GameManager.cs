using UnityEngine;

namespace MechaBloom
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private LevelSelectUI levelSelectUI;

        private bool paused;

        public bool Paused => paused;

        private void Start()
        {
            uiManager?.ShowTitle();
        }

        public void StartGame()
        {
            ShowLevelSelect();
        }

        public void PlayFirstUnlockedLevel()
        {
            paused = false;
            Time.timeScale = 1f;
            audioManager?.PlayUIButton();
            var levelNumber = saveManager != null ? Mathf.Clamp(saveManager.HighestUnlockedLevel, 1, 12) : 1;
            levelManager?.LoadLevel(levelNumber);
        }

        public void ShowLevelSelect()
        {
            paused = false;
            Time.timeScale = 1f;
            audioManager?.PlayUIButton();
            levelSelectUI?.Refresh();
            uiManager?.ShowLevelSelect();
        }

        public void OpenMainMenu()
        {
            paused = false;
            Time.timeScale = 1f;
            audioManager?.PlayUIButton();
            uiManager?.ShowMainMenu();
        }

        public void OpenSettings()
        {
            paused = false;
            Time.timeScale = 1f;
            audioManager?.PlayUIButton();
            uiManager?.ShowSettings();
        }

        public void OpenCredits()
        {
            paused = false;
            Time.timeScale = 1f;
            audioManager?.PlayUIButton();
            uiManager?.ShowCredits();
        }

        public void RestartLevel()
        {
            paused = false;
            Time.timeScale = 1f;
            audioManager?.PlayUIButton();
            uiManager?.ShowGameplay();
            levelManager?.RestartActiveLevel();
        }

        public void LoadNextLevel()
        {
            paused = false;
            Time.timeScale = 1f;
            audioManager?.PlayUIButton();
            levelManager?.LoadNextLevel();
        }

        public void ResetProgress()
        {
            audioManager?.PlayUIButton();
            saveManager?.ResetProgress();
            levelSelectUI?.Refresh();
        }

        public void Pause()
        {
            paused = true;
            Time.timeScale = 0f;
            audioManager?.PlayUIButton();
            uiManager?.ShowPause(true);
        }

        public void Resume()
        {
            paused = false;
            Time.timeScale = 1f;
            audioManager?.PlayUIButton();
            uiManager?.ShowPause(false);
        }

        public void TogglePause()
        {
            if (paused)
            {
                Resume();
                return;
            }

            Pause();
        }

        public void QuitForEditorTesting()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
