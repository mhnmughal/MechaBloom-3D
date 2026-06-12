using UnityEngine;

namespace MechaBloom
{
    public sealed class HintManager : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private FeedbackTextUI feedbackTextUI;
        [SerializeField] private AudioManager audioManager;

        public void ShowHint()
        {
            var level = levelManager != null ? levelManager.ActiveLevel : null;
            if (level == null)
            {
                return;
            }

            levelManager.MarkHintUsed();
            feedbackTextUI?.Show(level.HintText);
            audioManager?.PlayHint();
        }
    }
}
