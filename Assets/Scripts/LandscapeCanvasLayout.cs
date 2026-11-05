using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MechaBloom
{
    [ExecuteAlways]
    [RequireComponent(typeof(CanvasScaler))]
    public sealed class LandscapeCanvasLayout : MonoBehaviour
    {
        private static readonly Color PanelColor = new(0.035f, 0.055f, 0.06f, 0.9f);
        private static readonly Color TransparentContainerColor = new(0f, 0f, 0f, 0f);
        private static readonly Color ButtonColor = new(0.92f, 0.72f, 0.25f, 0.96f);
        private static readonly Color SecondaryButtonColor = new(0.18f, 0.48f, 0.44f, 0.94f);
        private static readonly Color TextColor = new(0.92f, 0.98f, 0.9f, 1f);
        private static readonly Color MutedTextColor = new(0.68f, 0.78f, 0.72f, 1f);

        [SerializeField] private Vector2 referenceResolution = new(1920f, 1080f);
        [SerializeField, Range(0f, 1f)] private float matchWidthOrHeight = 0.5f;

        private CanvasScaler scaler;
        private Vector2Int lastScreenSize;
#if UNITY_EDITOR
        private bool editorApplyQueued;
#endif

        private void Awake()
        {
            Apply();
        }

        private void OnEnable()
        {
            Apply();
        }

        private void Update()
        {
            var screenSize = new Vector2Int(Screen.width, Screen.height);
            if (screenSize != lastScreenSize)
            {
                Apply();
            }
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            QueueEditorApply();
#else
            Apply();
#endif
        }

#if UNITY_EDITOR
        private void QueueEditorApply()
        {
            if (editorApplyQueued)
            {
                return;
            }

            editorApplyQueued = true;
            EditorApplication.delayCall += ApplyAfterValidation;
        }

        private void ApplyAfterValidation()
        {
            editorApplyQueued = false;
            if (this == null || Application.isPlaying)
            {
                return;
            }

            Apply();
        }
#endif

        private void Apply()
        {
            scaler = scaler != null ? scaler : GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = referenceResolution;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = matchWidthOrHeight;

            lastScreenSize = new Vector2Int(Screen.width, Screen.height);
            StyleImages();
            StyleText();
            LayoutPanels();
        }

        private void LayoutPanels()
        {
            Stretch("TitleScreen");
            Stretch("MainMenuPanel");
            Stretch("LevelSelectPanel");
            Stretch("PausePanel");
            Stretch("SettingsPanel");
            Stretch("LevelCompletePanel");
            Stretch("GameOverPanel");
            Stretch("CreditsPanel");
            Stretch("TutorialPanel");

            Place("TitleScreen/GameLogoImage", new Vector2(0.5f, 0.68f), new Vector2(1180f, 360f));
            Place("TitleScreen/TitleText", new Vector2(0.5f, 0.68f), new Vector2(980f, 148f));
            Place("TitleScreen/SubtitleText", new Vector2(0.5f, 0.43f), new Vector2(900f, 78f));
            Place("TitleScreen/StartButton", new Vector2(0.5f, 0.25f), new Vector2(420f, 104f));

            Place("MainMenuPanel/TitleText", new Vector2(0.29f, 0.66f), new Vector2(640f, 132f));
            Place("MainMenuPanel/PlayButton", new Vector2(0.72f, 0.68f), new Vector2(380f, 92f));
            Place("MainMenuPanel/LevelSelectButton", new Vector2(0.72f, 0.54f), new Vector2(380f, 92f));
            Place("MainMenuPanel/SettingsButton", new Vector2(0.72f, 0.4f), new Vector2(380f, 92f));
            Place("MainMenuPanel/CreditsButton", new Vector2(0.72f, 0.26f), new Vector2(380f, 92f));

            LayoutGameplayHud();
            LayoutMobileControls();
            LayoutLevelSelect();
            LayoutSettings();
        }

        private void LayoutGameplayHud()
        {
            var hud = Find("GameplayHUD");
            if (hud == null)
            {
                return;
            }

            Stretch(hud);
            SetOffsets(hud, new Vector2(28f, -28f), new Vector2(-28f, 28f));

            Place("GameplayHUD/TopBar", new Vector2(0.5f, 0.94f), new Vector2(1680f, 96f));
            Place("GameplayHUD/ObjectivePanel", new Vector2(0.5f, 0.83f), new Vector2(1180f, 82f));
            Place("GameplayHUD/StatsPanel", new Vector2(0.12f, 0.52f), new Vector2(320f, 420f));
            Place("GameplayHUD/SelectedObjectPanel", new Vector2(0.88f, 0.52f), new Vector2(340f, 220f));
            Place("GameplayHUD/PauseButton", new Vector2(0.94f, 0.94f), new Vector2(88f, 88f));
            Place("FeedbackTextPanel", new Vector2(0.5f, 0.16f), new Vector2(880f, 72f));
        }

        private void LayoutMobileControls()
        {
            var controls = Find("MobileControlsPanel");
            if (controls == null)
            {
                return;
            }

            Stretch(controls);
            Place("MobileControlsPanel/UndoButton", new Vector2(0.1f, 0.085f), new Vector2(210f, 92f));
            Place("MobileControlsPanel/HintButton", new Vector2(0.26f, 0.085f), new Vector2(210f, 92f));
            Place("MobileControlsPanel/RotateSelectedButton", new Vector2(0.42f, 0.085f), new Vector2(210f, 92f));
            Place("MobileControlsPanel/ActivateSelectedButton", new Vector2(0.58f, 0.085f), new Vector2(210f, 92f));
            Place("MobileControlsPanel/RestartButton", new Vector2(0.74f, 0.085f), new Vector2(210f, 92f));
            Place("MobileControlsPanel/PauseButton", new Vector2(0.9f, 0.085f), new Vector2(210f, 92f));
        }

        private void LayoutLevelSelect()
        {
            Place("LevelSelectPanel/TitleText", new Vector2(0.5f, 0.86f), new Vector2(900f, 96f));
            Place("LevelSelectPanel/BackButton", new Vector2(0.08f, 0.88f), new Vector2(180f, 74f));

            var panel = Find("LevelSelectPanel");
            if (panel == null)
            {
                return;
            }

            var buttons = panel.GetComponentsInChildren<Button>(true);
            var index = 0;
            foreach (var button in buttons)
            {
                if (!button.name.Contains("Level"))
                {
                    continue;
                }

                var row = index / 6;
                var column = index % 6;
                Place(button.transform as RectTransform, new Vector2(0.24f + column * 0.105f, 0.66f - row * 0.18f), new Vector2(142f, 104f));
                index++;
            }
        }

        private void LayoutSettings()
        {
            Place("SettingsPanel/SettingsTitle", new Vector2(0.5f, 0.78f), new Vector2(760f, 96f));
            Place("SettingsPanel/MusicLabel", new Vector2(0.29f, 0.61f), new Vector2(300f, 68f));
            Place("SettingsPanel/MusicMinusButton", new Vector2(0.43f, 0.61f), new Vector2(72f, 72f));
            Place("SettingsPanel/MusicVolumeSlider", new Vector2(0.56f, 0.61f), new Vector2(360f, 56f));
            Place("SettingsPanel/MusicPlusButton", new Vector2(0.69f, 0.61f), new Vector2(72f, 72f));
            Place("SettingsPanel/MusicValueText", new Vector2(0.76f, 0.61f), new Vector2(120f, 60f));
            Place("SettingsPanel/SFXLabel", new Vector2(0.29f, 0.5f), new Vector2(300f, 68f));
            Place("SettingsPanel/SfxMinusButton", new Vector2(0.43f, 0.5f), new Vector2(72f, 72f));
            Place("SettingsPanel/SFXVolumeSlider", new Vector2(0.56f, 0.5f), new Vector2(360f, 56f));
            Place("SettingsPanel/SfxPlusButton", new Vector2(0.69f, 0.5f), new Vector2(72f, 72f));
            Place("SettingsPanel/SfxValueText", new Vector2(0.76f, 0.5f), new Vector2(120f, 60f));
            Place("SettingsPanel/VibrationLabel", new Vector2(0.42f, 0.39f), new Vector2(300f, 68f));
            Place("SettingsPanel/VibrationToggle", new Vector2(0.58f, 0.39f), new Vector2(72f, 72f));
            Place("SettingsPanel/VibrationPlaceholderToggle", new Vector2(0.58f, 0.39f), new Vector2(72f, 72f));
            Place("SettingsPanel/ResetProgressButton", new Vector2(0.5f, 0.27f), new Vector2(380f, 80f));
            Place("SettingsPanel/BackButton", new Vector2(0.5f, 0.15f), new Vector2(260f, 74f));
        }


        private void StyleImages()
        {
            foreach (var image in GetComponentsInChildren<Image>(true))
            {
                if (image == null)
                {
                    continue;
                }

                if (image.GetComponent<Button>() != null)
                {
                    image.color = image.gameObject.name.Contains("Back") || image.gameObject.name.Contains("Pause")
                        ? SecondaryButtonColor
                        : ButtonColor;
                }
                else if (image.gameObject.name == "GameplayHUD" || image.gameObject.name == "MobileControlsPanel")
                {
                    image.color = TransparentContainerColor;
                    image.raycastTarget = false;
                }
                else if (image.gameObject.name == "GameLogoImage")
                {
                    image.color = Color.white;
                    image.raycastTarget = false;
                    image.preserveAspect = true;
                }
                else if (image.gameObject.name.Contains("Panel"))
                {
                    image.color = PanelColor;
                }
            }
        }

        private void StyleText()
        {
            foreach (var text in GetComponentsInChildren<TMP_Text>(true))
            {
                text.color = text.name.Contains("Subtitle") || text.name.Contains("Objective") ? MutedTextColor : TextColor;
                text.enableWordWrapping = true;
                text.overflowMode = TextOverflowModes.Ellipsis;
            }
        }

        private void Stretch(string path) => Stretch(Find(path));

        private static void Stretch(RectTransform rect)
        {
            if (rect == null)
            {
                return;
            }

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private static void SetOffsets(RectTransform rect, Vector2 min, Vector2 max)
        {
            rect.offsetMin = min;
            rect.offsetMax = max;
        }

        private void Place(string path, Vector2 normalizedPosition, Vector2 size) => Place(Find(path), normalizedPosition, size);

        private static void Place(RectTransform rect, Vector2 normalizedPosition, Vector2 size)
        {
            if (rect == null)
            {
                return;
            }

            rect.anchorMin = normalizedPosition;
            rect.anchorMax = normalizedPosition;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = size;
            rect.anchoredPosition = Vector2.zero;
        }

        private RectTransform Find(string path)
        {
            var parts = path.Split('/');
            var current = transform;
            foreach (var part in parts)
            {
                current = FindChild(current, part);
                if (current == null)
                {
                    return null;
                }
            }

            return current as RectTransform;
        }

        private static Transform FindChild(Transform parent, string childName)
        {
            for (var i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.name == childName)
                {
                    return child;
                }
            }

            return null;
        }
    }
}
