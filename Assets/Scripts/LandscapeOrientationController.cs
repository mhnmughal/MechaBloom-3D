using UnityEngine;

namespace MechaBloom
{
    public sealed class LandscapeOrientationController : MonoBehaviour
    {
        [SerializeField] private ScreenOrientation preferredOrientation = ScreenOrientation.LandscapeLeft;
        [SerializeField] private bool allowBothLandscapeDirections = true;
        [SerializeField, Range(30, 120)] private int targetFrameRate = 60;

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
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFrameRate;
        }
    }
}
