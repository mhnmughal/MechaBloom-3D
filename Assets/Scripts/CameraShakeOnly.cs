using System.Collections;
using UnityEngine;

namespace MechaBloom
{
    public sealed class CameraShakeOnly : MonoBehaviour
    {
        [SerializeField] private float duration = 0.18f;
        [SerializeField] private float strength = 0.08f;

        private Vector3 originalPosition;
        private Coroutine shakeRoutine;

        private void Start()
        {
            originalPosition = transform.localPosition;
        }

        public void Shake()
        {
            if (shakeRoutine != null)
            {
                StopCoroutine(shakeRoutine);
                transform.localPosition = originalPosition;
            }

            shakeRoutine = StartCoroutine(ShakeRoutine());
        }

        private IEnumerator ShakeRoutine()
        {
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var offset = new Vector3(Random.Range(-strength, strength), Random.Range(-strength, strength), 0f);
                transform.localPosition = originalPosition + offset;
                yield return null;
            }

            transform.localPosition = originalPosition;
            shakeRoutine = null;
        }
    }
}
