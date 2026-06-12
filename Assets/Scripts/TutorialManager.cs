using TMPro;
using UnityEngine;

namespace MechaBloom
{
    public sealed class TutorialManager : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private TMP_Text tutorialMessageText;

        private int messageIndex;

        public void BeginTutorial()
        {
            messageIndex = 0;
            uiManager?.ShowTutorial(true);
            ShowCurrentMessage();
        }

        public void Continue()
        {
            messageIndex++;
            ShowCurrentMessage();
        }

        public void Skip()
        {
            uiManager?.ShowTutorial(false);
        }

        private void ShowCurrentMessage()
        {
            var messages = levelManager != null && levelManager.ActiveLevel != null ? levelManager.ActiveLevel.TutorialMessages : null;
            if (messages == null || messageIndex >= messages.Length)
            {
                uiManager?.ShowTutorial(false);
                return;
            }

            tutorialMessageText?.SetText(messages[messageIndex]);
        }
    }
}
