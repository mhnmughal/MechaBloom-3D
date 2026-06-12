using UnityEngine;

namespace MechaBloom
{
    public class GearController : InteractableObject
    {
        [SerializeField] private Transform rotatingVisual;
        [SerializeField] private int startQuarterTurns;
        [SerializeField] private float rotateDuration = 0.16f;
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private Material normalMaterial;
        [SerializeField] private Material selectedMaterial;

        public Transform RotatingVisual => rotatingVisual;
        public int StartQuarterTurns => startQuarterTurns;
        public float RotateDuration => rotateDuration;
        public Renderer[] Renderers => renderers;
        public Material NormalMaterial => normalMaterial;
        public Material SelectedMaterial => selectedMaterial;
        public override bool CanRotate => true;

        public override bool Rotate()
        {
            return false;
        }

        public void SetSelected(bool selected)
        {
            // Selection visuals are intentionally deferred to a gameplay milestone.
        }
    }
}
