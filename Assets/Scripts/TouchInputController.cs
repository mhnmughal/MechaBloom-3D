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
        [SerializeField] private AudioManager audioManager;

        private InteractableObject selected;

        public Camera GameplayCamera => gameplayCamera;
        public LevelManager LevelManager => levelManager;
        public FlowPathCalculator FlowPathCalculator => flowPathCalculator;
        public InteractableObject Selected => selected;

        private void Update()
        {
            HandlePointerSelection();
            HandleEditorShortcuts();
        }

        public void RotateSelected()
        {
            if (selected == null)
            {
                feedbackTextUI?.Show("Select a gear or valve first.");
                return;
            }

            levelManager?.ExecuteAction(selected, GameplayActionType.Rotate);
        }

        public void ActivateSelected()
        {
            if (selected == null)
            {
                feedbackTextUI?.Show("Select a valve or core first.");
                return;
            }

            levelManager?.ExecuteAction(selected, GameplayActionType.Activate);
        }

        public void ClearSelection()
        {
            Select(null);
        }

        private void Select(InteractableObject interactable)
        {
            if (selected is GearController previousGear)
            {
                previousGear.SetSelected(false);
            }

            selected = interactable;
            if (selected is GearController gear)
            {
                gear.SetSelected(true);
            }

            if (selectionRing != null)
            {
                selectionRing.gameObject.SetActive(selected != null);
                if (selected != null)
                {
                    selectionRing.position = selected.transform.position + Vector3.up * 0.08f;
                }
            }

            levelManager?.RefreshObjectiveState(selected != null ? selected.DisplayName : string.Empty);
            if (selected != null)
            {
                audioManager?.PlayObjectSelect();
            }
        }

        private void HandlePointerSelection()
        {
            if (gameplayCamera == null)
            {
                return;
            }

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    TrySelectAt(touch.position);
                }

                return;
            }

#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonDown(0))
            {
                TrySelectAt(Input.mousePosition);
            }
#endif
        }

        private void HandleEditorShortcuts()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateSelected();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                ActivateSelected();
            }
#endif
        }

        private void TrySelectAt(Vector2 screenPosition)
        {
            var ray = gameplayCamera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out var hit, 200f))
            {
                var interactable = hit.collider.GetComponentInParent<InteractableObject>();
                if (interactable != null)
                {
                    Select(interactable);
                    return;
                }
            }

            ClearSelection();
        }
    }
}
