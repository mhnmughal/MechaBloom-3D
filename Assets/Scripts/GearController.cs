using System.Collections;
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

        private int currentQuarterTurns;
        private bool rotating;

        public int CurrentQuarterTurns => currentQuarterTurns;
        public override bool CanRotate => !rotating;

        private void Awake()
        {
            if (rotatingVisual == null)
            {
                rotatingVisual = transform;
            }
        }

        private void Start()
        {
            ResetState();
        }

        public override bool Rotate()
        {
            if (rotating)
            {
                return false;
            }

            currentQuarterTurns = (currentQuarterTurns + 1) % 4;
            StartCoroutine(RotateVisual());
            return true;
        }

        public void SetSelected(bool selected)
        {
            var material = selected ? selectedMaterial : normalMaterial;
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

        public override void ResetState()
        {
            currentQuarterTurns = Mathf.Abs(startQuarterTurns) % 4;
            if (rotatingVisual != null)
            {
                rotatingVisual.localRotation = Quaternion.Euler(0f, currentQuarterTurns * 90f, 0f);
            }
        }

        private IEnumerator RotateVisual()
        {
            rotating = true;
            var start = rotatingVisual.localRotation;
            var end = Quaternion.Euler(0f, currentQuarterTurns * 90f, 0f);
            var elapsed = 0f;

            while (elapsed < rotateDuration)
            {
                elapsed += Time.deltaTime;
                rotatingVisual.localRotation = Quaternion.Slerp(start, end, elapsed / rotateDuration);
                yield return null;
            }

            rotatingVisual.localRotation = end;
            rotating = false;
        }
    }
}
