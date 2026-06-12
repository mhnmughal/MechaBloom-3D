using UnityEngine;

namespace MechaBloom
{
    public sealed class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelConfig[] levels;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private FlowPathCalculator flowPathCalculator;

        private int activeLevelIndex = -1;
        private int actionsUsed;
        private int energyRemaining;
        private bool hintUsed;

        public LevelConfig ActiveLevel => levels != null && activeLevelIndex >= 0 && activeLevelIndex < levels.Length ? levels[activeLevelIndex] : null;
        public int ActionsUsed => actionsUsed;
        public int EnergyRemaining => energyRemaining;
        public bool HintUsed => hintUsed;

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
            // Gameplay action accounting is intentionally deferred to a later milestone.
        }

        public void RefreshObjectiveState()
        {
            var level = ActiveLevel;
            if (level == null)
            {
                return;
            }

            uiManager?.UpdateGameplay(level, actionsUsed, energyRemaining, 0, string.Empty);
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
            hintUsed = false;
            energyRemaining = level.EnergyBudget;

            flowPathCalculator?.Recalculate();
            RefreshObjectiveState();
        }
    }
}
