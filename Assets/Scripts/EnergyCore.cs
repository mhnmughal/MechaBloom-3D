using UnityEngine;

namespace MechaBloom
{
    public sealed class EnergyCore : InteractableObject
    {
        [SerializeField] private bool startsActive = true;
        [SerializeField] private Renderer coreRenderer;
        [SerializeField] private Material activeMaterial;
        [SerializeField] private Material inactiveMaterial;

        private bool active;

        public bool StartsActive => startsActive;
        public Renderer CoreRenderer => coreRenderer;
        public Material ActiveMaterial => activeMaterial;
        public Material InactiveMaterial => inactiveMaterial;
        public bool Active => active;
        public override bool CanActivate => true;

        private void Awake()
        {
            ResetState();
        }

        public override bool Activate()
        {
            SetActive(!active);
            return true;
        }

        public override void ResetState()
        {
            SetActive(startsActive);
        }

        public void SetActive(bool value)
        {
            active = value;
            if (coreRenderer != null)
            {
                coreRenderer.sharedMaterial = active ? activeMaterial : inactiveMaterial;
            }
        }
    }
}
