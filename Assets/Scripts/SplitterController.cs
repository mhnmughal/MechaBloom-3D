using UnityEngine;

namespace MechaBloom
{
    public sealed class SplitterController : MonoBehaviour
    {
        [SerializeField] private GardenTile tile;
        [SerializeField] private GridDirection[] outputDirections;

        public GardenTile Tile => tile;
        public GridDirection[] OutputDirections => outputDirections;
    }
}
