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

    [Serializable]
    public struct FlowConnection
    {
        [SerializeField] private GridDirection from;
        [SerializeField] private GridDirection to;

        public GridDirection From => from;
        public GridDirection To => to;
    }
}
