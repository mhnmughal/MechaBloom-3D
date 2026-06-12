using UnityEngine;

namespace MechaBloom
{
    public sealed class LockedRoot : MonoBehaviour
    {
        [SerializeField] private bool startsLocked = true;
        [SerializeField] private GameObject rootVisual;

        private bool locked;

        public bool StartsLocked => startsLocked;
        public GameObject RootVisual => rootVisual;
        public bool Locked => locked;

        private void Awake()
        {
            ResetState();
        }

        public void Unlock()
        {
            SetLocked(false);
        }

        public void ResetState()
        {
            SetLocked(startsLocked);
        }

        public void SetLocked(bool value)
        {
            locked = value;
            if (rootVisual != null)
            {
                rootVisual.SetActive(locked);
            }
        }
    }
}
