using UnityEngine;

namespace MechaBloom
{
    public sealed class BlockerTile : MonoBehaviour
    {
        [SerializeField] private GardenTile tile;

        public GardenTile Tile => tile;
    }
}
