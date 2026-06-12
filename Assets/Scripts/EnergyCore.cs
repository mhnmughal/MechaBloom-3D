using UnityEngine;

namespace MechaBloom
{
    public sealed class EnergyCore : InteractableObject
    {
        [SerializeField] private bool startsActive = true;
        [SerializeField] private Renderer coreRenderer;
        [SerializeField] private Material activeMaterial;
        [SerializeField] private Material inactiveMaterial;

        private bool isActive;

        public bool IsActive => isActive;
        public override bool CanActivate => true;

        private void Start()
        {
            ResetState();
        }

        public override bool Activate()
        {
            SetActive(!isActive);
            return true;
        }

        public override void ResetState()
        {
            SetActive(startsActive);
        }

        private void SetActive(bool value)
        {
            isActive = value;
            if (coreRenderer != null)
            {
                coreRenderer.sharedMaterial = isActive ? activeMaterial : inactiveMaterial;
            }
        }
    }
}
