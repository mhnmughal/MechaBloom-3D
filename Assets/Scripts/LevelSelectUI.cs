using UnityEngine;
using UnityEngine.UI;

namespace MechaBloom
{
    public sealed class LevelSelectUI : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private Button[] levelButtons;
        [SerializeField] private GameObject[] lockOverlays;

        private void OnEnable()
        {
            Refresh();
        }

        public void SelectLevel(int levelNumber)
        {
            if (saveManager != null && levelNumber > saveManager.HighestUnlockedLevel)
            {
                audioManager?.PlayWrongAction();
                return;
            }

            audioManager?.PlayUIButton();
            levelManager?.LoadLevel(levelNumber);
        }

        public void Refresh()
        {
            var highest = saveManager != null ? saveManager.HighestUnlockedLevel : 1;
            for (var i = 0; i < levelButtons.Length; i++)
            {
                var levelNumber = i + 1;
                if (levelButtons[i] != null)
                {
                    levelButtons[i].interactable = levelNumber <= highest;
                }

                if (lockOverlays != null && i < lockOverlays.Length && lockOverlays[i] != null)
                {
                    lockOverlays[i].SetActive(levelNumber > highest);
                }
            }
        }
    }
}
