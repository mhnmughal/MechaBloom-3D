using UnityEngine;

namespace MechaBloom
{
    [RequireComponent(typeof(Collider))]
    public abstract class InteractableObject : MonoBehaviour
    {
        [SerializeField] private string displayName = "Interactable";
        [SerializeField] private GardenTile tile;

        public string DisplayName => displayName;
        public GardenTile Tile => tile;

        public virtual bool CanRotate => false;
        public virtual bool CanActivate => false;

        public virtual bool Rotate()
        {
            return false;
        }

        public virtual bool Activate()
        {
            return false;
        }

        public virtual void ResetState()
        {
        }

        public void SetRuntimeTile(GardenTile runtimeTile)
        {
            tile = runtimeTile;
        }
    }
}
