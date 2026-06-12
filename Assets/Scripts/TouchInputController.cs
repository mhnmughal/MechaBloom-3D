using UnityEngine;

namespace MechaBloom
{
    public sealed class TouchInputController : MonoBehaviour
    {
        [SerializeField] private Camera gameplayCamera;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private FlowPathCalculator flowPathCalculator;
        [SerializeField] private FeedbackTextUI feedbackTextUI;
        [SerializeField] private Transform selectionRing;

        private InteractableObject selected;

        public Camera GameplayCamera => gameplayCamera;
        public LevelManager LevelManager => levelManager;
        public FlowPathCalculator FlowPathCalculator => flowPathCalculator;
        public InteractableObject Selected => selected;

        public void RotateSelected()
        {
            feedbackTextUI?.Show("Rotate input reserved");
        }

        public void ActivateSelected()
        {
            feedbackTextUI?.Show("Activate input reserved");
        }

        public void ClearSelection()
        {
            Select(null);
        }

        private void Select(InteractableObject interactable)
        {
            selected = interactable;
            if (selectionRing != null)
            {
                selectionRing.gameObject.SetActive(selected != null);
                if (selected != null)
                {
                    selectionRing.position = selected.transform.position + Vector3.up * 0.08f;
                }
            }
        }
    }
}
