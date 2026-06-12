using UnityEngine;

namespace MechaBloom
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private UIManager uiManager;

        private bool paused;

        private void Start()
        {
            uiManager?.ShowTitle();
        }

        public void StartGame()
        {
            paused = false;
            Time.timeScale = 1f;
            levelManager?.LoadLevel(1);
        }

        public void OpenMainMenu()
        {
            paused = false;
            Time.timeScale = 1f;
            uiManager?.ShowMainMenu();
        }

        public void Pause()
        {
            paused = true;
            Time.timeScale = 0f;
            uiManager?.ShowPause(true);
        }

        public void Resume()
        {
            paused = false;
            Time.timeScale = 1f;
            uiManager?.ShowPause(false);
        }

        public void TogglePause()
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
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
