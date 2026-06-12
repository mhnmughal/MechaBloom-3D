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

        public bool IsOpen => isOpen;
        public override bool CanActivate => true;
        public override bool CanRotate => true;

        private void Start()
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

        private void SetOpen(bool value)
        {
            isOpen = value;
            if (valveHandle != null)
            {
                valveHandle.localRotation = Quaternion.Euler(0f, isOpen ? 90f : 0f, 0f);
            }

            var material = isOpen ? openMaterial : closedMaterial;
            if (material == null || renderers == null)
            {
                return;
            }

            foreach (var item in renderers)
            {
                if (item != null)
                {
                    item.sharedMaterial = material;
                }
            }
        }
    }
}
