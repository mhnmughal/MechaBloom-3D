using TMPro;
using UnityEngine;

namespace MechaBloom
{
    public sealed class TutorialManager : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private TMP_Text tutorialMessageText;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private AudioManager audioManager;

        private int messageIndex;

        public void BeginTutorial()
        {
            if (saveManager != null && saveManager.TutorialSeen && levelManager != null && levelManager.ActiveLevel != null && levelManager.ActiveLevel.LevelNumber > 1)
            {
                uiManager?.ShowTutorial(false);
                return;
            }

            messageIndex = 0;
            uiManager?.ShowTutorial(true);
            ShowCurrentMessage();
        }

        public void Continue()
        {
            audioManager?.PlayUIButton();
            messageIndex++;
            ShowCurrentMessage();
        }

        public void Skip()
        {
            audioManager?.PlayUIButton();
            if (saveManager != null)
            {
                saveManager.TutorialSeen = true;
            }

            uiManager?.ShowTutorial(false);
        }

        private void ShowCurrentMessage()
        {
            var messages = levelManager != null && levelManager.ActiveLevel != null ? levelManager.ActiveLevel.TutorialMessages : null;
            if (messages == null || messageIndex >= messages.Length)
            {
                if (saveManager != null)
                {
                    saveManager.TutorialSeen = true;
                }

                uiManager?.ShowTutorial(false);
                return;
            }

            tutorialMessageText?.SetText(messages[messageIndex]);
        }
    }
}
