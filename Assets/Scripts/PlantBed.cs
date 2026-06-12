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

        private PlantGrowthStage stage = PlantGrowthStage.Empty;

        public GardenTile Tile => tile;
        public FlowType RequiredFlow => requiredFlow;
        public Renderer[] Renderers => renderers;
        public Material EmptyMaterial => emptyMaterial;
        public Material GrowingMaterial => growingMaterial;
        public Material BloomedMaterial => bloomedMaterial;
        public ParticleSystem BloomParticles => bloomParticles;
        public PlantGrowthStage Stage => stage;
        public bool IsBloomed => stage == PlantGrowthStage.Bloomed;

        public bool TryReceiveFlow(FlowType flowType)
        {
            if (flowType != requiredFlow)
            {
                SetStage(PlantGrowthStage.Empty);
                return false;
            }

            var wasBloomed = IsBloomed;
            SetStage(PlantGrowthStage.Bloomed);
            if (!wasBloomed && bloomParticles != null)
            {
                bloomParticles.Play();
            }

            return true;
        }

        public void ResetState()
        {
            SetStage(PlantGrowthStage.Empty);
        }

        public void SetStage(PlantGrowthStage newStage)
        {
            stage = newStage;
            var material = stage switch
            {
                PlantGrowthStage.Bloomed => bloomedMaterial,
                PlantGrowthStage.Growing => growingMaterial,
                PlantGrowthStage.Sprout => growingMaterial,
                _ => emptyMaterial
            };

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
