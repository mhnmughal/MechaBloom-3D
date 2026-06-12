using UnityEngine;

namespace MechaBloom
{
    public sealed class LockedRoot : MonoBehaviour
    {
        [SerializeField] private bool startsLocked = true;
        [SerializeField] private GameObject rootVisual;

        public bool StartsLocked => startsLocked;
        public GameObject RootVisual => rootVisual;
        public bool Locked => startsLocked;

        public void Unlock()
        {
            // Lock state changes are intentionally deferred to gameplay.
        }

        public void ResetState()
        {
            // Lock state changes are intentionally deferred to gameplay.
        }
    }
}
