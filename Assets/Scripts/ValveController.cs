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

        private bool isOpen;

        public bool StartsOpen => startsOpen;
        public Transform ValveHandle => valveHandle;
        public Renderer[] Renderers => renderers;
        public Material ClosedMaterial => closedMaterial;
        public Material OpenMaterial => openMaterial;
        public bool IsOpen => isOpen;
        public override bool CanActivate => true;
        public override bool CanRotate => true;

        private void Awake()
        {
            ResetState();
        }

        public override bool Activate()
        {
            SetOpen(!isOpen);
            return true;
        }

        public override bool Rotate()
        {
            return Activate();
        }

        public override void ResetState()
        {
            SetOpen(startsOpen);
        }

        public void SetOpen(bool open)
        {
            isOpen = open;
            if (valveHandle != null)
            {
                valveHandle.localRotation = Quaternion.Euler(0f, isOpen ? 90f : 0f, 0f);
            }

            var material = isOpen ? openMaterial : closedMaterial;
            if (material == null || renderers == null)
            {
                return;
            }

            foreach (var targetRenderer in renderers)
            {
                if (targetRenderer != null)
                {
                    targetRenderer.sharedMaterial = material;
                }
            }
        }
    }
}
