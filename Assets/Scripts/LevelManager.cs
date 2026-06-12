using UnityEngine;

namespace MechaBloom
{
    public sealed class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelConfig[] levels;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private FlowPathCalculator flowPathCalculator;
        [SerializeField] private UndoManager undoManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private StarRatingManager starRatingManager;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private TutorialManager tutorialManager;
        [SerializeField] private LevelSelectUI levelSelectUI;
        [SerializeField] private FeedbackTextUI feedbackTextUI;
        [SerializeField] private CameraShakeOnly cameraShake;

        private int activeLevelIndex = -1;
        private int actionsUsed;
        private int energyRemaining;
        private bool hintUsed;
        private int wrongActions;
        private bool levelComplete;
        private bool levelFailed;

        public LevelConfig ActiveLevel => levels != null && activeLevelIndex >= 0 && activeLevelIndex < levels.Length ? levels[activeLevelIndex] : null;
        public int ActionsUsed => actionsUsed;
        public int EnergyRemaining => energyRemaining;
        public bool HintUsed => hintUsed;
        public int WrongActions => wrongActions;
        public bool LevelComplete => levelComplete;
        public bool LevelFailed => levelFailed;

        public void LoadLevel(int levelNumber)
        {
            if (levels == null || levels.Length == 0)
            {
                return;
            }

            activeLevelIndex = Mathf.Clamp(levelNumber - 1, 0, levels.Length - 1);
            for (var i = 0; i < levels.Length; i++)
            {
                if (levels[i] != null)
                {
                    levels[i].gameObject.SetActive(i == activeLevelIndex);
                }
            }

            ResetActiveLevel();
            uiManager?.ShowGameplay();
            tutorialManager?.BeginTutorial();
        }

        public void RestartActiveLevel()
        {
            ResetActiveLevel();
        }

        public void RegisterAction(bool validAction)
        {
            if (levelComplete || levelFailed)
            {
                return;
            }

            actionsUsed++;
            energyRemaining = Mathf.Max(0, ActiveLevel != null ? ActiveLevel.EnergyBudget - actionsUsed : energyRemaining - 1);
            if (!validAction)
            {
                wrongActions++;
                audioManager?.PlayWrongAction();
                cameraShake?.Shake();
            }
        }

        public void RefreshObjectiveState()
        {
            var level = ActiveLevel;
            if (level == null)
            {
                return;
            }

            uiManager?.UpdateGameplay(level, actionsUsed, energyRemaining, flowPathCalculator != null ? flowPathCalculator.LastBloomedCount : CountBloomed(level), string.Empty);
        }

        public void MarkHintUsed()
        {
            hintUsed = true;
            RefreshObjectiveState();
        }

        public bool ExecuteAction(InteractableObject target, GameplayActionType actionType)
        {
            var level = ActiveLevel;
            if (level == null || target == null || levelComplete || levelFailed)
            {
                return false;
            }

            undoManager?.CaptureState();

            var valid = actionType == GameplayActionType.Rotate ? target.Rotate() : target.Activate();
            RegisterAction(valid);

            if (valid)
            {
                PlayActionAudio(target, actionType);
                flowPathCalculator?.Recalculate(true);
            }
            else
            {
                feedbackTextUI?.Show("That part cannot move now.");
                undoManager?.DiscardLatest();
                EvaluateLevelState();
            }

            RefreshObjectiveState(target.DisplayName);
            return valid;
        }

        public void EvaluateLevelState()
        {
            var level = ActiveLevel;
            if (level == null || levelComplete || levelFailed)
            {
                return;
            }

            var bloomed = CountBloomed(level);
            if (bloomed >= level.RequiredBloomCount)
            {
                CompleteLevel();
                return;
            }

            if (actionsUsed >= level.ActionLimit || energyRemaining <= 0)
            {
                FailLevel("No actions remain.");
            }
        }

        public void RestoreRuntimeState(int restoredActionsUsed, int restoredEnergyRemaining, int restoredWrongActions, bool restoredHintUsed)
        {
            actionsUsed = Mathf.Max(0, restoredActionsUsed);
            energyRemaining = Mathf.Max(0, restoredEnergyRemaining);
            wrongActions = Mathf.Max(0, restoredWrongActions);
            hintUsed = restoredHintUsed;
            levelComplete = false;
            levelFailed = false;
            uiManager?.ShowGameplay();
            flowPathCalculator?.Recalculate(false);
            RefreshObjectiveState();
        }

        public void RefreshObjectiveState(string selectedName)
        {
            var level = ActiveLevel;
            if (level == null)
            {
                return;
            }

            uiManager?.UpdateGameplay(level, actionsUsed, energyRemaining, CountBloomed(level), selectedName);
        }

        public void LoadNextLevel()
        {
            if (ActiveLevel == null)
            {
                return;
            }

            LoadLevel(Mathf.Min(ActiveLevel.LevelNumber + 1, levels.Length));
        }

        private void ResetActiveLevel()
        {
            var level = ActiveLevel;
            if (level == null)
            {
                return;
            }

            actionsUsed = 0;
            hintUsed = false;
            wrongActions = 0;
            levelComplete = false;
            levelFailed = false;
            energyRemaining = level.EnergyBudget;

            ResetObjects(level);
            undoManager?.Clear();
            flowPathCalculator?.Recalculate(false);
            RefreshObjectiveState();
        }

        private void ResetObjects(LevelConfig level)
        {
            foreach (var gear in level.Gears)
            {
                gear?.ResetState();
            }

            foreach (var valve in level.Valves)
            {
                valve?.ResetState();
            }

            foreach (var core in level.EnergyCores)
            {
                core?.ResetState();
            }

            foreach (var bed in level.PlantBeds)
            {
                bed?.ResetState();
            }

            foreach (var root in level.GetComponentsInChildren<LockedRoot>(true))
            {
                root?.ResetState();
            }

            foreach (var pipe in level.Pipes)
            {
                pipe?.SetFlowActive(false);
            }

            foreach (var visual in level.FlowVisuals)
            {
                visual?.SetActive(false);
            }
        }

        private void CompleteLevel()
        {
            levelComplete = true;
            var level = ActiveLevel;
            var stars = starRatingManager != null ? starRatingManager.CalculateStars(level, actionsUsed, energyRemaining, wrongActions, hintUsed) : 1;
            saveManager?.SetStars(level.LevelNumber, stars);
            saveManager?.UnlockLevel(Mathf.Min(level.LevelNumber + 1, levels.Length));
            levelSelectUI?.Refresh();
            audioManager?.PlayLevelComplete();
            if (stars > 0)
            {
                audioManager?.PlayStarReward();
            }

            uiManager?.ShowLevelComplete($"{level.LevelName}\n{stars} star{(stars == 1 ? string.Empty : "s")} earned\nActions: {actionsUsed}/{level.ActionLimit}");
        }

        private void FailLevel(string reason)
        {
            levelFailed = true;
            audioManager?.PlayGameOver();
            cameraShake?.Shake();
            uiManager?.ShowGameOver(reason);
        }

        private static int CountBloomed(LevelConfig level)
        {
            if (level.PlantBeds == null)
            {
                return 0;
            }

            var count = 0;
            foreach (var plantBed in level.PlantBeds)
            {
                if (plantBed != null && plantBed.IsBloomed)
                {
                    count++;
                }
            }

            return count;
        }

        private void PlayActionAudio(InteractableObject target, GameplayActionType actionType)
        {
            if (actionType == GameplayActionType.Rotate)
            {
                audioManager?.PlayGearRotate();
                return;
            }

            if (target is ValveController valve)
            {
                if (valve.IsOpen)
                {
                    audioManager?.PlayValveOpen();
                }
                else
                {
                    audioManager?.PlayValveClose();
                }

                return;
            }

            if (target is EnergyCore)
            {
                audioManager?.PlayEnergyFlowStart();
            }
        }
    }
}
