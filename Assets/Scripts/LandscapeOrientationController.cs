using UnityEngine;

namespace MechaBloom
{
    public sealed class LandscapeOrientationController : MonoBehaviour
    {
        [SerializeField] private ScreenOrientation preferredOrientation = ScreenOrientation.LandscapeLeft;
        [SerializeField] private bool allowBothLandscapeDirections = true;

        private void Awake()
        {
            Apply();
        }

        private void OnEnable()
        {
            Apply();
        }

        private void Apply()
        {
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = allowBothLandscapeDirections;
            Screen.orientation = preferredOrientation;
        }
    }
}
