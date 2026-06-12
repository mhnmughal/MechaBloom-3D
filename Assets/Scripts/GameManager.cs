using UnityEngine;

namespace MechaBloom
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private AudioManager audioManager;

        private bool paused;

        public bool Paused => paused;

        private void Start()
        {
            uiManager?.ShowTitle();
        }

        public void StartGame()
        {
            paused = false;
            Time.timeScale = 1f;
            audioManager?.PlayUIButton();
            uiManager?.ShowLevelSelect();
        }

        public void OpenMainMenu()
        {
            paused = false;
            Time.timeScale = 1f;
            audioManager?.PlayUIButton();
            uiManager?.ShowMainMenu();
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
