using UnityEngine;

namespace MechaBloom
{
    public sealed class LockedRoot : MonoBehaviour
    {
        [SerializeField] private bool startsLocked = true;
        [SerializeField] private GameObject rootVisual;

        private bool locked;

        public bool Locked => locked;

        private void Start()
        {
            ResetState();
        }

        public void Unlock()
        {
            locked = false;
            if (rootVisual != null)
            {
                rootVisual.SetActive(false);
            }
        }

        public void ResetState()
        {
            locked = startsLocked;
            if (rootVisual != null)
            {
                rootVisual.SetActive(locked);
            }
        }
    }
}
