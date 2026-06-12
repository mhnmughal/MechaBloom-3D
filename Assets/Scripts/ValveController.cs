using UnityEngine;

namespace MechaBloom
{
    public sealed class ValveController : InteractableObject
    {
        [SerializeField] private bool startsOpen;
        [SerializeField] private Transform valveHandle;
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private Material closedMaterial;
        [SerializeField] private Material openMaterial;

        public bool StartsOpen => startsOpen;
        public Transform ValveHandle => valveHandle;
        public Renderer[] Renderers => renderers;
        public Material ClosedMaterial => closedMaterial;
        public Material OpenMaterial => openMaterial;
        public bool IsOpen => startsOpen;
        public override bool CanActivate => true;
        public override bool CanRotate => true;

        public override bool Activate()
        {
            return false;
        }

        public override bool Rotate()
        {
            return Activate();
        }

        public override void ResetState()
        {
            // Valve state changes are intentionally deferred to gameplay.
        }
    }
}
