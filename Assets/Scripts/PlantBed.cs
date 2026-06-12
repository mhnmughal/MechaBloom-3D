using UnityEngine;

namespace MechaBloom
{
    public sealed class PlantBed : MonoBehaviour
    {
        [SerializeField] private GardenTile tile;
        [SerializeField] private FlowType requiredFlow = FlowType.Water;
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private Material emptyMaterial;
        [SerializeField] private Material growingMaterial;
        [SerializeField] private Material bloomedMaterial;
        [SerializeField] private ParticleSystem bloomParticles;

        public GardenTile Tile => tile;
        public FlowType RequiredFlow => requiredFlow;
        public Renderer[] Renderers => renderers;
        public Material EmptyMaterial => emptyMaterial;
        public Material GrowingMaterial => growingMaterial;
        public Material BloomedMaterial => bloomedMaterial;
        public ParticleSystem BloomParticles => bloomParticles;
        public PlantGrowthStage Stage => PlantGrowthStage.Empty;
        public bool IsBloomed => false;

        public bool TryReceiveFlow(FlowType flowType)
        {
            return false;
        }

        public void ResetState()
        {
            // Plant state changes are intentionally deferred to gameplay.
        }
    }
}
