using UnityEngine;

namespace MechaBloom
{
    public sealed class StarRatingManager : MonoBehaviour
    {
        public int CalculateStars(LevelConfig level, int actionsUsed, int energyRemaining, int wrongActions, bool hintUsed)
        {
            if (level == null)
            {
                return 0;
            }

            var stars = actionsUsed <= level.Star3ActionLimit && wrongActions == 0 && energyRemaining > 0 ? 3 :
                actionsUsed <= level.Star2ActionLimit && wrongActions <= 1 ? 2 :
                actionsUsed <= level.Star1ActionLimit ? 1 : 1;

            if (hintUsed)
            {
                stars = Mathf.Min(stars, 2);
            }

            return Mathf.Clamp(stars, 1, 3);
        }
    }
}
