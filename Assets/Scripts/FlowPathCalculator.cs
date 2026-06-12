using UnityEngine;

namespace MechaBloom
{
    public sealed class FlowPathCalculator : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private GardenGridManager gardenGridManager;

        public LevelManager LevelManager => levelManager;
        public GardenGridManager GardenGridManager => gardenGridManager;

        public void Recalculate()
        {
            // Flow traversal is intentionally deferred to a gameplay milestone.
        }
    }
}
