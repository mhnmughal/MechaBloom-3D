using UnityEngine;

namespace MechaBloom
{
    public sealed class UndoManager : MonoBehaviour
    {
        [SerializeField] private FeedbackTextUI feedbackTextUI;

        public void UndoLastAction()
        {
            feedbackTextUI?.Show("Undo");
        }
    }
}
