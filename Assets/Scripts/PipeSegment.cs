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

        public void SetFlowActive(bool active)
        {
            var material = active ? activeMaterial : inactiveMaterial;
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
