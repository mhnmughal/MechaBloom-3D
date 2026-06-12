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

        private int quarterTurns;
        private int rotationsUsed;

        public Transform RotatingVisual => rotatingVisual;
        public int StartQuarterTurns => startQuarterTurns;
        public float RotateDuration => rotateDuration;
        public Renderer[] Renderers => renderers;
        public Material NormalMaterial => normalMaterial;
        public Material SelectedMaterial => selectedMaterial;
        public int QuarterTurns => quarterTurns;
        public int RotationsUsed => rotationsUsed;
        public override bool CanRotate => true;

        private void Awake()
        {
            ResetState();
        }

        public override bool Rotate()
        {
            if (!CanRotate)
            {
                return false;
            }

            quarterTurns = (quarterTurns + 1) % 4;
            rotationsUsed++;
            ApplyRotation();
            return true;
        }

        public void SetSelected(bool selected)
        {
            var material = selected ? selectedMaterial : normalMaterial;
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

        public override void ResetState()
        {
            SetState(startQuarterTurns, 0);
        }

        public void SetState(int newQuarterTurns, int newRotationsUsed)
        {
            quarterTurns = ((newQuarterTurns % 4) + 4) % 4;
            rotationsUsed = Mathf.Max(0, newRotationsUsed);
            ApplyRotation();
            SetSelected(false);
        }

        protected virtual void ApplyRotation()
        {
            var target = rotatingVisual != null ? rotatingVisual : transform;
            target.localRotation = Quaternion.Euler(0f, quarterTurns * 90f, 0f);
        }
    }
}
