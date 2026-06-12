using System.Collections.Generic;
using UnityEngine;

namespace MechaBloom
{
    public sealed class GardenGridManager : MonoBehaviour
    {
        [SerializeField] private GardenTile[] tiles;

        private readonly Dictionary<Vector2Int, GardenTile> tileLookup = new();

        private void Awake()
        {
            RebuildLookup();
        }

        public void RebuildLookup()
        {
            tileLookup.Clear();
            if (tiles == null)
            {
                return;
            }

            foreach (var tile in tiles)
            {
                if (tile != null && !tileLookup.ContainsKey(tile.GridPosition))
                {
                    tileLookup.Add(tile.GridPosition, tile);
                }
            }
        }

        public bool TryGetTile(Vector2Int position, out GardenTile tile)
        {
            if (tileLookup.Count == 0)
            {
                RebuildLookup();
            }

            return tileLookup.TryGetValue(position, out tile);
        }
    }
}
