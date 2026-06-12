using UnityEngine;

namespace MechaBloom
{
    public sealed class BrokenGear : GearController
    {
        [SerializeField] private int maxRotations = 2;
        private int rotationsUsed;

        public override bool CanRotate => base.CanRotate && rotationsUsed < maxRotations;

        public override bool Rotate()
        {
            if (!CanRotate || !base.Rotate())
            {
                return false;
            }

            rotationsUsed++;
            return true;
        }

        public override void ResetState()
        {
            base.ResetState();
            rotationsUsed = 0;
        }
    }
}
