using UnityEngine;

namespace MechaBloom
{
    public sealed class BrokenGear : GearController
    {
        [SerializeField] private int maxRotations = 2;

        public int MaxRotations => maxRotations;
    }
}
