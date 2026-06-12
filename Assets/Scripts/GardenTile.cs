using UnityEngine;

namespace MechaBloom
{
    public sealed class GardenTile : MonoBehaviour
    {
        [SerializeField] private Vector2Int gridPosition;
        [SerializeField] private bool blocked;

        public Vector2Int GridPosition => gridPosition;
        public bool Blocked => blocked;

        public void Configure(Vector2Int position, bool isBlocked)
        {
            gridPosition = position;
            blocked = isBlocked;
        }
    }
}
