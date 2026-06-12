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

        private void Update()
        {
            HandlePointerInput();
            HandleEditorShortcuts();
        }

        public void RotateSelected()
        {
            if (selected == null || !selected.CanRotate)
            {
                feedbackTextUI?.Show("Wrong path");
                levelManager?.RegisterAction(false);
                return;
            }

            var changed = selected.Rotate();
            feedbackTextUI?.Show(changed ? "Gear rotated" : "Blocked flow");
            levelManager?.RegisterAction(changed);
            flowPathCalculator?.Recalculate();
        }

        public void ActivateSelected()
        {
            if (selected == null || !selected.CanActivate)
            {
                feedbackTextUI?.Show("Wrong path");
                levelManager?.RegisterAction(false);
                return;
            }

            var changed = selected.Activate();
            feedbackTextUI?.Show(changed ? "Energy connected" : "Blocked flow");
            levelManager?.RegisterAction(changed);
            flowPathCalculator?.Recalculate();
        }

        private void HandlePointerInput()
        {
            if (gameplayCamera == null)
            {
                return;
            }

            var pressed = false;
            var screenPosition = Vector2.zero;

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                pressed = true;
                screenPosition = Input.GetTouch(0).position;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                pressed = true;
                screenPosition = Input.mousePosition;
            }

            if (!pressed)
            {
                return;
            }

            var ray = gameplayCamera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out var hit, 100f))
            {
                Select(hit.collider.GetComponentInParent<InteractableObject>());
            }
            else
            {
                Select(null);
                feedbackTextUI?.Show("Wrong path");
            }
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

            feedbackTextUI?.Show(selected != null ? selected.DisplayName : "No selection");
        }

        private void HandleEditorShortcuts()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateSelected();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                ActivateSelected();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Pause is wired through UI in the scene; shortcut stays optional for editor testing.
            }
        }
    }
}
