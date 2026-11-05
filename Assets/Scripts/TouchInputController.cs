using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
        [SerializeField] private UndoManager undoManager;
        [SerializeField] private GameManager gameManager;

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
                feedbackTextUI?.Show("Select an activatable object first.");
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

            var touchscreen = Touchscreen.current;
            if (touchscreen != null && touchscreen.primaryTouch.press.wasPressedThisFrame)
            {
                var touchPosition = touchscreen.primaryTouch.position.ReadValue();
                if (!IsScreenPositionOverUI(touchPosition))
                {
                    TrySelectAt(touchPosition);
                }

                return;
            }

#if UNITY_EDITOR || UNITY_STANDALONE
            var mouse = Mouse.current;
            if (mouse != null && mouse.leftButton.wasPressedThisFrame)
            {
                var mousePosition = mouse.position.ReadValue();
                if (!IsScreenPositionOverUI(mousePosition))
                {
                    TrySelectAt(mousePosition);
                }
            }
#endif
        }

        private void HandleEditorShortcuts()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            var keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            if (keyboard.rKey.wasPressedThisFrame)
            {
                RotateSelected();
            }

            if (keyboard.aKey.wasPressedThisFrame)
            {
                ActivateSelected();
            }

            if (keyboard.uKey.wasPressedThisFrame)
            {
                undoManager?.UndoLastAction();
            }

            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                gameManager?.TogglePause();
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

        private static bool IsScreenPositionOverUI(Vector2 screenPosition)
        {
            if (EventSystem.current == null)
            {
                return false;
            }

            var eventData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}
