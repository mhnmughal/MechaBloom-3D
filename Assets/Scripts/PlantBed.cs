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
        public PlantGrowthStage Stage => stage;
        public bool IsBloomed => stage == PlantGrowthStage.Bloomed;

        private void Start()
        {
            ResetState();
        }

        public bool TryReceiveFlow(FlowType flowType)
        {
            if (flowType != requiredFlow || IsBloomed)
            {
                return false;
            }

            SetStage(PlantGrowthStage.Bloomed);
            if (bloomParticles != null)
            {
                bloomParticles.Play();
            }

            return true;
        }

        public void ResetState()
        {
            SetStage(PlantGrowthStage.Empty);
        }

        private void SetStage(PlantGrowthStage newStage)
        {
            stage = newStage;
            var material = emptyMaterial;
            if (stage == PlantGrowthStage.Growing)
            {
                material = growingMaterial;
            }
            else if (stage == PlantGrowthStage.Bloomed)
            {
                material = bloomedMaterial;
            }

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
