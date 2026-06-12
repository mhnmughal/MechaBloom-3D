using System;
using UnityEngine;

namespace MechaBloom
{
    public enum FlowType
    {
        None,
        Water,
        Energy,
        Sunlight
    }

    public enum GridDirection
    {
        North,
        East,
        South,
        West
    }

    public enum PlantGrowthStage
    {
        Empty,
        Sprout,
        Growing,
        Bloomed
    }

    public enum GameplayActionType
    {
        Rotate,
        Activate
    }

    [Serializable]
    public struct FlowConnection
    {
        [SerializeField] private GridDirection from;
        [SerializeField] private GridDirection to;

        public GridDirection From => from;
        public GridDirection To => to;
    }

    public static class GridDirectionUtility
    {
        public static Vector2Int ToOffset(GridDirection direction)
        {
            return direction switch
            {
                GridDirection.North => new Vector2Int(0, 1),
                GridDirection.East => new Vector2Int(1, 0),
                GridDirection.South => new Vector2Int(0, -1),
                GridDirection.West => new Vector2Int(-1, 0),
                _ => Vector2Int.zero
            };
        }

        public static GridDirection Opposite(GridDirection direction)
        {
            return direction switch
            {
                GridDirection.North => GridDirection.South,
                GridDirection.East => GridDirection.West,
                GridDirection.South => GridDirection.North,
                GridDirection.West => GridDirection.East,
                _ => GridDirection.North
            };
        }
    }
}
