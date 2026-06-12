using UnityEngine;

namespace MechaBloom
{
    public sealed class EnergyCore : InteractableObject
    {
        [SerializeField] private bool startsActive = true;
        [SerializeField] private Renderer coreRenderer;
        [SerializeField] private Material activeMaterial;
        [SerializeField] private Material inactiveMaterial;

        public bool StartsActive => startsActive;
        public Renderer CoreRenderer => coreRenderer;
        public Material ActiveMaterial => activeMaterial;
        public Material InactiveMaterial => inactiveMaterial;
        public override bool CanActivate => true;

        public override bool Activate()
        {
            return false;
        }
    }
}
