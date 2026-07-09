using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MechaBloom.Editor
{
    [InitializeOnLoad]
    internal static class MechaBloomAppIconSetup
    {
        private const string IconPath = "Assets/Art/AppIcon/MechaBloom3D_AppIcon.png";
        private const string TitleLogoPath = "Assets/Art/Branding/MechaBloom3D_TitleLogo.png";
        private const string AppliedKey = "MechaBloom.BrandingApplied.v2";
        private const string CompanyName = "glasgoka";
        private const string ProductName = "MechaBloom 3D";
        private const string Version = "1.0.0";
        private const string BundleIdentifier = "com.glasgoka.mechabloom3d";

        static MechaBloomAppIconSetup()
        {
            EditorApplication.delayCall += ApplyIfNeeded;
        }

        [MenuItem("MechaBloom/Apply App Icon")]
        private static void ApplyFromMenu()
        {
            ApplyBranding();
        }

        [MenuItem("MechaBloom/Apply Branding Settings")]
        private static void ApplyBrandingFromMenu()
        {
            ApplyBranding();
        }

        private static void ApplyIfNeeded()
        {
            if (!SessionState.GetBool(AppliedKey, false))
            {
                ApplyBranding();
            }
        }

        private static void ApplyBranding()
        {
            ApplyPlayerSettings();
            ConfigureTitleLogoImporter();
            ApplyIcon();
            AddTitleScreenLogo();
            SessionState.SetBool(AppliedKey, true);
            AssetDatabase.SaveAssets();
            Debug.Log("MechaBloom 3D branding settings applied.");
        }

        private static void ApplyPlayerSettings()
        {
            PlayerSettings.companyName = CompanyName;
            PlayerSettings.productName = ProductName;
            PlayerSettings.bundleVersion = Version;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, BundleIdentifier);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, BundleIdentifier);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, BundleIdentifier);
            PlayerSettings.Android.bundleVersionCode = 1;
            PlayerSettings.iOS.buildNumber = "1";
        }

        private static void ApplyIcon()
        {
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);
            if (icon == null)
            {
                return;
            }

            ApplyToGroup(BuildTargetGroup.iOS, icon);
            ApplyToGroup(BuildTargetGroup.Android, icon);
            ApplyToGroup(BuildTargetGroup.Standalone, icon);
            Debug.Log($"MechaBloom 3D app icon applied from {IconPath}.");
        }

        private static void ConfigureTitleLogoImporter()
        {
            if (AssetImporter.GetAtPath(TitleLogoPath) is not TextureImporter importer)
            {
                AssetDatabase.ImportAsset(TitleLogoPath);
                importer = AssetImporter.GetAtPath(TitleLogoPath) as TextureImporter;
            }

            if (importer == null)
            {
                return;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.alphaIsTransparency = true;
            importer.mipmapEnabled = false;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.maxTextureSize = 2048;
            importer.SaveAndReimport();
        }

        private static void AddTitleScreenLogo()
        {
            var titleScreen = FindSceneObject("GameScene/UI/Canvas/TitleScreen") ?? FindSceneObject("UI/Canvas/TitleScreen");
            if (titleScreen == null)
            {
                return;
            }

            var logoSprite = AssetDatabase.LoadAssetAtPath<Sprite>(TitleLogoPath);
            if (logoSprite == null)
            {
                return;
            }

            var logo = titleScreen.transform.Find("GameLogoImage");
            if (logo == null)
            {
                var logoObject = new GameObject("GameLogoImage", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
                logoObject.transform.SetParent(titleScreen.transform, false);
                logo = logoObject.transform;
            }

            logo.SetSiblingIndex(0);
            var rect = (RectTransform)logo;
            rect.anchorMin = new Vector2(0.5f, 0.68f);
            rect.anchorMax = new Vector2(0.5f, 0.68f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(1180f, 360f);
            rect.anchoredPosition = Vector2.zero;

            var image = logo.GetComponent<Image>();
            image.sprite = logoSprite;
            image.preserveAspect = true;
            image.raycastTarget = false;
            image.color = Color.white;

            var titleText = titleScreen.transform.Find("TitleText");
            if (titleText != null)
            {
                titleText.gameObject.SetActive(false);
            }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        private static void ApplyToGroup(BuildTargetGroup group, Texture2D icon)
        {
            var iconSizes = PlayerSettings.GetIconSizesForTargetGroup(group);
            if (iconSizes == null || iconSizes.Length == 0)
            {
                PlayerSettings.SetIconsForTargetGroup(group, new[] { icon });
                return;
            }

            PlayerSettings.SetIconsForTargetGroup(group, Enumerable.Repeat(icon, iconSizes.Length).ToArray());
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
