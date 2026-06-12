using UnityEngine;

namespace MechaBloom
{
    public sealed class PipeSegment : MonoBehaviour
    {
        [SerializeField] private GardenTile tile;
        [SerializeField] private FlowType acceptedFlow = FlowType.Water;
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private Material inactiveMaterial;
        [SerializeField] private Material activeMaterial;

        public GardenTile Tile => tile;
        public FlowType AcceptedFlow => acceptedFlow;
        public Renderer[] Renderers => renderers;
        public Material InactiveMaterial => inactiveMaterial;
        public Material ActiveMaterial => activeMaterial;

        public void SetFlowActive(bool active)
        {
            var material = active ? activeMaterial : inactiveMaterial;
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
