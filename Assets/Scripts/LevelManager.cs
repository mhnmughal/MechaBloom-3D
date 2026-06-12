using UnityEngine;

namespace MechaBloom
{
    public sealed class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelConfig[] levels;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private FlowPathCalculator flowPathCalculator;
        [SerializeField] private StarRatingManager starRatingManager;

        private int activeLevelIndex;
        private int actionsUsed;
        private int energyRemaining;
        private int wrongActions;
        private bool hintUsed;
        private bool levelEnded;

        public LevelConfig ActiveLevel => levels != null && activeLevelIndex >= 0 && activeLevelIndex < levels.Length ? levels[activeLevelIndex] : null;
        public int ActionsUsed => actionsUsed;

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
        }

        public void RestartActiveLevel()
        {
            ResetActiveLevel();
        }

        public void RegisterAction(bool validAction)
        {
            if (levelEnded || ActiveLevel == null)
            {
                return;
            }

            if (validAction)
            {
                actionsUsed++;
                energyRemaining = Mathf.Max(0, energyRemaining - 1);
            }
            else
            {
                wrongActions++;
            }

            RefreshObjectiveState();
        }

        public void RefreshObjectiveState()
        {
            var level = ActiveLevel;
            if (level == null)
            {
                return;
            }

            var bloomed = CountBloomed(level);
            uiManager?.UpdateGameplay(level, actionsUsed, energyRemaining, bloomed, string.Empty);

            if (!levelEnded && bloomed >= level.RequiredBloomCount)
            {
                CompleteLevel();
            }
            else if (!levelEnded && actionsUsed >= level.ActionLimit && bloomed < level.RequiredBloomCount)
            {
                FailLevel("Action limit exceeded");
            }
            else if (!levelEnded && energyRemaining <= 0 && bloomed < level.RequiredBloomCount)
            {
                FailLevel("Not enough energy");
            }
        }

        public void MarkHintUsed()
        {
            hintUsed = true;
        }

        private void ResetActiveLevel()
        {
            var level = ActiveLevel;
            if (level == null)
            {
                return;
            }

            actionsUsed = 0;
            wrongActions = 0;
            hintUsed = false;
            levelEnded = false;
            energyRemaining = level.EnergyBudget;

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

            foreach (var plantBed in level.PlantBeds)
            {
                plantBed?.ResetState();
            }

            flowPathCalculator?.Recalculate();
            RefreshObjectiveState();
        }

        private void CompleteLevel()
        {
            levelEnded = true;
            var level = ActiveLevel;
            var stars = starRatingManager != null ? starRatingManager.CalculateStars(level, actionsUsed, energyRemaining, wrongActions, hintUsed) : 1;
            if (saveManager != null && level != null)
            {
                saveManager.HighestUnlockedLevel = Mathf.Max(saveManager.HighestUnlockedLevel, level.LevelNumber + 1);
                saveManager.SetStars(level.LevelNumber, Mathf.Max(stars, saveManager.GetStars(level.LevelNumber)));
            }

            uiManager?.ShowLevelComplete($"{stars} stars | Actions {actionsUsed} | Energy {energyRemaining}");
        }

        private void FailLevel(string reason)
        {
            levelEnded = true;
            uiManager?.ShowGameOver(reason);
        }

        private static int CountBloomed(LevelConfig level)
        {
            var count = 0;
            foreach (var bed in level.PlantBeds)
            {
                if (bed != null && bed.IsBloomed)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
