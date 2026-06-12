using UnityEngine;

namespace MechaBloom
{
    public sealed class BrokenGear : GearController
    {
        [SerializeField] private int maxRotations = 2;

        public int MaxRotations => maxRotations;
        public override bool CanRotate => RotationsUsed < maxRotations;
    }
}
