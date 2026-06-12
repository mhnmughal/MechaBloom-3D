using UnityEngine;

namespace MechaBloom
{
    public sealed class StarRatingManager : MonoBehaviour
    {
        public int CalculateStars(LevelConfig level, int actionsUsed, int energyRemaining, int wrongActions, bool hintUsed)
        {
            if (level == null)
            {
                return 1;
            }

            var stars = 1;
            if (actionsUsed <= level.Star2ActionLimit)
            {
                stars = 2;
            }

            if (actionsUsed <= level.Star3ActionLimit && wrongActions == 0 && energyRemaining > 0 && !hintUsed)
            {
                stars = 3;
            }

            if (hintUsed)
            {
                stars = Mathf.Min(stars, 2);
            }

            return stars;
        }
    }
}
