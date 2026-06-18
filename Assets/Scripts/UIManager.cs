using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MechaBloom
{
    public sealed class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject titleScreen;
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject levelSelectPanel;
        [SerializeField] private GameObject gameplayHud;
        [SerializeField] private GameObject mobileControlsPanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject levelCompletePanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject creditsPanel;
        [SerializeField] private GameObject tutorialPanel;

        [Header("Gameplay Text")]
        [SerializeField] private TMP_Text currentLevelText;
        [SerializeField] private TMP_Text objectiveText;
        [SerializeField] private TMP_Text resourceNumberText;
        [SerializeField] private TMP_Text actionCountText;
        [SerializeField] private TMP_Text bloomCountText;
        [SerializeField] private TMP_Text selectedObjectText;
        [SerializeField] private TMP_Text failureReasonText;
        [SerializeField] private TMP_Text levelCompleteSummaryText;
        [SerializeField] private Slider resourceMeter;

        public void ShowTitle() => ShowOnly(titleScreen);
        public void ShowMainMenu() => ShowOnly(mainMenuPanel);
        public void ShowLevelSelect() => ShowOnly(levelSelectPanel);
        public void ShowSettings() => ShowOnly(settingsPanel);
        public void ShowCredits() => ShowOnly(creditsPanel);

        public void ShowGameplay()
        {
            SetAllPanels(false);
            SetActive(gameplayHud, true);
            SetActive(mobileControlsPanel, true);
        }

        public void ShowPause(bool visible) => SetActive(pausePanel, visible);
        public void ShowTutorial(bool visible) => SetActive(tutorialPanel, visible);

        public void ShowLevelComplete(string summary)
        {
            levelCompleteSummaryText?.SetText(summary);
            SetActive(gameOverPanel, false);
            SetActive(pausePanel, false);
            SetActive(tutorialPanel, false);
            SetActive(levelCompletePanel, true);
        }

        public void ShowGameOver(string reason)
        {
            failureReasonText?.SetText(reason);
            SetActive(levelCompletePanel, false);
            SetActive(pausePanel, false);
            SetActive(tutorialPanel, false);
            SetActive(gameOverPanel, true);
        }

        public void UpdateGameplay(LevelConfig level, int actionsUsed, int energyRemaining, int bloomed, string selectedName)
        {
            currentLevelText?.SetText(level != null ? $"Level {level.LevelNumber:00}" : "Level --");
            objectiveText?.SetText(level != null ? level.ObjectiveText : string.Empty);
            resourceNumberText?.SetText(level != null ? $"{energyRemaining}/{level.EnergyBudget}" : "--");
            actionCountText?.SetText(level != null ? $"{actionsUsed}/{level.ActionLimit}" : "--");
            bloomCountText?.SetText(level != null ? $"{bloomed}/{level.RequiredBloomCount}" : "--");
            selectedObjectText?.SetText(string.IsNullOrEmpty(selectedName) ? "No selection" : selectedName);
            if (resourceMeter != null && level != null)
            {
                resourceMeter.value = level.EnergyBudget <= 0 ? 0f : (float)energyRemaining / level.EnergyBudget;
            }
        }

        private void ShowOnly(GameObject panel)
        {
            SetAllPanels(false);
            SetActive(panel, true);
        }

        private void SetAllPanels(bool active)
        {
            SetActive(titleScreen, active);
            SetActive(mainMenuPanel, active);
            SetActive(levelSelectPanel, active);
            SetActive(gameplayHud, active);
            SetActive(mobileControlsPanel, active);
            SetActive(pausePanel, active);
            SetActive(settingsPanel, active);
            SetActive(levelCompletePanel, active);
            SetActive(gameOverPanel, active);
            SetActive(creditsPanel, active);
            SetActive(tutorialPanel, active);
        }

        private static void SetActive(GameObject target, bool active)
        {
            if (target != null)
            {
                target.SetActive(active);
            }
        }
    }
}
