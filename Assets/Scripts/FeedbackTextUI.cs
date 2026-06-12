using System.Collections;
using TMPro;
using UnityEngine;

namespace MechaBloom
{
    public sealed class FeedbackTextUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text feedbackText;
        [SerializeField] private float visibleSeconds = 1.5f;

        private Coroutine currentRoutine;

        public void Show(string message)
        {
            if (feedbackText == null)
            {
                return;
            }

            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
            }

            currentRoutine = StartCoroutine(ShowRoutine(message));
        }

        private IEnumerator ShowRoutine(string message)
        {
            feedbackText.text = message;
            yield return new WaitForSeconds(visibleSeconds);
            feedbackText.text = string.Empty;
            currentRoutine = null;
        }
    }
}
