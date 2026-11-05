using TMPro;
using System.Reflection;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MechaBloom.Editor
{
    internal static class MechaBloomSettingsPanelSetupTool
    {
        private static readonly Color PanelButton = new(0.92f, 0.72f, 0.25f, 0.96f);
        private static readonly Color PanelButtonSecondary = new(0.18f, 0.48f, 0.44f, 0.94f);
        private static readonly Color Track = new(0.03f, 0.08f, 0.08f, 0.88f);
        private static readonly Color Fill = new(0.32f, 0.92f, 0.48f, 0.95f);
        private static readonly Color Handle = new(1f, 0.86f, 0.24f, 1f);
        private static readonly Color Text = new(0.92f, 0.98f, 0.9f, 1f);

        [MenuItem("MechaBloom/Repair Settings Panel")]
        private static void RepairSettingsPanel()
        {
            var panel = FindSceneObject("GameScene/UI/Canvas/SettingsPanel") ?? FindSceneObject("UI/Canvas/SettingsPanel");
            if (panel == null)
            {
                Debug.LogWarning("SettingsPanel was not found.");
                return;
            }

            var controller = panel.GetComponent<SettingsPanelController>() ?? panel.AddComponent<SettingsPanelController>();
            var musicSlider = EnsureSlider(panel.transform, "MusicVolumeSlider");
            var sfxSlider = EnsureSlider(panel.transform, "SFXVolumeSlider");
            var vibrationToggle = EnsureToggle(panel.transform);
            var musicValue = EnsureText(panel.transform, "MusicValueText", "70%", 34, Text);
            var sfxValue = EnsureText(panel.transform, "SfxValueText", "80%", 34, Text);

            EnsureLabel(panel.transform, "SettingsTitle", "Settings", 54);
            EnsureLabel(panel.transform, "MusicLabel", "Music Volume", 34);
            EnsureLabel(panel.transform, "SFXLabel", "SFX Volume", 34);
            EnsureLabel(panel.transform, "VibrationLabel", "Vibration", 34);

            WireButton(EnsureButton(panel.transform, "MusicMinusButton", "-", PanelButtonSecondary), controller.DecreaseMusicVolume);
            WireButton(EnsureButton(panel.transform, "MusicPlusButton", "+", PanelButton), controller.IncreaseMusicVolume);
            WireButton(EnsureButton(panel.transform, "SfxMinusButton", "-", PanelButtonSecondary), controller.DecreaseSfxVolume);
            WireButton(EnsureButton(panel.transform, "SfxPlusButton", "+", PanelButton), controller.IncreaseSfxVolume);

            SetControllerReferences(controller, musicSlider, sfxSlider, vibrationToggle, musicValue, sfxValue);
            ApplyCanvasLayout();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            Debug.Log("Settings panel repaired with visible volume controls.");
        }

        private static Slider EnsureSlider(Transform parent, string name)
        {
            var sliderObject = EnsureRect(parent, name);
            var image = sliderObject.GetComponent<Image>() ?? sliderObject.gameObject.AddComponent<Image>();
            image.color = Track;

            var slider = sliderObject.GetComponent<Slider>() ?? sliderObject.gameObject.AddComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.wholeNumbers = false;

            var background = EnsureImage(sliderObject, "Background", Track);
            Stretch(background.rectTransform, Vector2.zero, Vector2.one);

            var fillArea = EnsureRect(sliderObject, "Fill Area");
            Stretch(fillArea, new Vector2(0f, 0.22f), new Vector2(1f, 0.78f));
            var fill = EnsureImage(fillArea, "Fill", Fill);
            Stretch(fill.rectTransform, Vector2.zero, Vector2.one);

            var handleArea = EnsureRect(sliderObject, "Handle Slide Area");
            Stretch(handleArea, Vector2.zero, Vector2.one);
            var handle = EnsureImage(handleArea, "Handle", Handle);
            handle.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            handle.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            handle.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            handle.rectTransform.sizeDelta = new Vector2(38f, 42f);

            slider.fillRect = fill.rectTransform;
            slider.handleRect = handle.rectTransform;
            slider.targetGraphic = handle;
            return slider;
        }

        private static Toggle EnsureToggle(Transform parent)
        {
            var oldToggle = parent.Find("VibrationPlaceholderToggle");
            if (oldToggle != null)
            {
                oldToggle.name = "VibrationToggle";
            }

            var toggleRect = EnsureRect(parent, "VibrationToggle");
            var background = EnsureImage(toggleRect, "Background", Track);
            Stretch(background.rectTransform, Vector2.zero, Vector2.one);
            var checkmark = EnsureImage(toggleRect, "Checkmark", Fill);
            checkmark.rectTransform.anchorMin = new Vector2(0.24f, 0.24f);
            checkmark.rectTransform.anchorMax = new Vector2(0.76f, 0.76f);
            checkmark.rectTransform.offsetMin = Vector2.zero;
            checkmark.rectTransform.offsetMax = Vector2.zero;

            var toggle = toggleRect.GetComponent<Toggle>() ?? toggleRect.gameObject.AddComponent<Toggle>();
            toggle.targetGraphic = background;
            toggle.graphic = checkmark;
            toggle.isOn = true;
            return toggle;
        }

        private static void SetControllerReferences(
            SettingsPanelController controller,
            Slider musicSlider,
            Slider sfxSlider,
            Toggle vibrationToggle,
            TMP_Text musicValue,
            TMP_Text sfxValue)
        {
            var serialized = new SerializedObject(controller);
            serialized.FindProperty("musicVolumeSlider").objectReferenceValue = musicSlider;
            serialized.FindProperty("sfxVolumeSlider").objectReferenceValue = sfxSlider;
            serialized.FindProperty("vibrationToggle").objectReferenceValue = vibrationToggle;
            serialized.FindProperty("musicValueText").objectReferenceValue = musicValue;
            serialized.FindProperty("sfxValueText").objectReferenceValue = sfxValue;
            serialized.FindProperty("audioManager").objectReferenceValue = FindObject<AudioManager>();
            serialized.FindProperty("saveManager").objectReferenceValue = FindObject<SaveManager>();
            serialized.ApplyModifiedProperties();
        }

        private static void WireButton(Button button, UnityEngine.Events.UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            UnityEventTools.AddPersistentListener(button.onClick, action);
        }

        private static TMP_Text EnsureLabel(Transform parent, string name, string label, int size)
        {
            return EnsureText(parent, name, label, size, Text);
        }

        private static TMP_Text EnsureText(Transform parent, string name, string value, int size, Color color)
        {
            var rect = EnsureRect(parent, name);
            var text = rect.GetComponent<TextMeshProUGUI>() ?? rect.gameObject.AddComponent<TextMeshProUGUI>();
            text.text = value;
            text.fontSize = size;
            text.alignment = TextAlignmentOptions.Center;
            text.color = color;
            text.enableWordWrapping = false;
            text.raycastTarget = false;
            return text;
        }

        private static Button EnsureButton(Transform parent, string name, string label, Color color)
        {
            var rect = EnsureRect(parent, name);
            var image = rect.GetComponent<Image>() ?? rect.gameObject.AddComponent<Image>();
            image.color = color;
            var button = rect.GetComponent<Button>() ?? rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;

            var labelText = EnsureText(rect, "Label", label, 44, Text);
            Stretch((RectTransform)labelText.transform, Vector2.zero, Vector2.one);
            return button;
        }

        private static Image EnsureImage(Transform parent, string name, Color color)
        {
            var rect = EnsureRect(parent, name);
            var image = rect.GetComponent<Image>() ?? rect.gameObject.AddComponent<Image>();
            image.color = color;
            return image;
        }

        private static RectTransform EnsureRect(Transform parent, string name)
        {
            var child = parent.Find(name);
            if (child != null)
            {
                return (RectTransform)child;
            }

            var created = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer));
            created.transform.SetParent(parent, false);
            return (RectTransform)created.transform;
        }

        private static void Stretch(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax)
        {
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private static T FindObject<T>() where T : Object
        {
            foreach (var candidate in Resources.FindObjectsOfTypeAll<T>())
            {
                if (candidate is Component component && component.gameObject.scene.IsValid())
                {
                    return candidate;
                }
            }

            return null;
        }

        private static void ApplyCanvasLayout()
        {
            var applyMethod = typeof(LandscapeCanvasLayout).GetMethod("Apply", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var layout in Resources.FindObjectsOfTypeAll<LandscapeCanvasLayout>())
            {
                if (layout.gameObject.scene.IsValid())
                {
                    applyMethod?.Invoke(layout, null);
                }
            }
        }

        private static GameObject FindSceneObject(string path)
        {
            var parts = path.Split('/');
            foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (root.name != parts[0])
                {
                    continue;
                }

                var current = root.transform;
                for (var i = 1; i < parts.Length; i++)
                {
                    current = current.Find(parts[i]);
                    if (current == null)
                    {
                        return null;
                    }
                }

                return current.gameObject;
            }

            return null;
        }
    }
}
