using UnityEngine;

namespace MechaBloom
{
    public sealed class WaterSource : MonoBehaviour
    {
        [SerializeField] private GardenTile tile;
        [SerializeField] private GridDirection outputDirection = GridDirection.East;
        [SerializeField] private bool active = true;

        public GardenTile Tile => tile;
        public GridDirection OutputDirection => outputDirection;
        public bool Active => active;

        public void SetRuntimeTile(GardenTile runtimeTile)
        {
            tile = runtimeTile;
        }
    }
}
