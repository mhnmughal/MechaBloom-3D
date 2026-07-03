using System.Collections.Generic;
using UnityEngine;

namespace MechaBloom
{
    [ExecuteAlways]
    public sealed class MechaBloomEnvironmentDesigner : MonoBehaviour
    {
        private static readonly Color FloorTint = new(0.07f, 0.12f, 0.1f, 1f);
        private static readonly Color WallTint = new(0.14f, 0.2f, 0.18f, 1f);
        private static readonly Color PipeTint = new(0.48f, 0.42f, 0.34f, 1f);
        private static readonly Color BloomTint = new(0.22f, 0.72f, 0.46f, 1f);
        private static readonly Color AccentTint = new(1f, 0.75f, 0.28f, 1f);

        [SerializeField] private Camera gameplayCamera;
        [SerializeField] private Light sunLight;
        [SerializeField] private bool rebuildDecorations = true;

#if UNITY_EDITOR
        private bool applyQueued;
#endif

        private void OnEnable()
        {
            Apply();
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (applyQueued)
                {
                    return;
                }

                applyQueued = true;
                UnityEditor.EditorApplication.delayCall += ApplyAfterValidation;
                return;
            }
#endif
            Apply();
        }

#if UNITY_EDITOR
        private void ApplyAfterValidation()
        {
            if (this == null)
            {
                return;
            }

            applyQueued = false;
            Apply();
        }
#endif

        private void Apply()
        {
            ResolveReferences();
            ConfigureCamera();
            ConfigureLighting();
            TintExistingEnvironment();

            if (rebuildDecorations)
            {
                BuildDecorations();
            }
        }

        private void ResolveReferences()
        {
            gameplayCamera = gameplayCamera != null ? gameplayCamera : Camera.main;
            if (sunLight == null)
            {
                sunLight = FindFirstObjectByType<Light>();
            }
        }

        private void ConfigureCamera()
        {
            if (gameplayCamera == null)
            {
                return;
            }

            gameplayCamera.transform.SetPositionAndRotation(new Vector3(7.6f, 10.5f, -6.2f), Quaternion.Euler(58f, -43f, 0f));
            gameplayCamera.fieldOfView = 42f;
            gameplayCamera.clearFlags = CameraClearFlags.Skybox;
        }

        private void ConfigureLighting()
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.28f, 0.36f, 0.31f, 1f);
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.09f, 0.14f, 0.13f, 1f);
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.018f;

            if (sunLight == null)
            {
                return;
            }

            sunLight.transform.rotation = Quaternion.Euler(50f, -34f, 12f);
            sunLight.color = new Color(1f, 0.9f, 0.67f, 1f);
            sunLight.intensity = 1.55f;
            sunLight.shadows = LightShadows.Soft;
        }

        private void TintExistingEnvironment()
        {
            foreach (var renderer in GetComponentsInChildren<Renderer>(true))
            {
                if (renderer == null || renderer.sharedMaterial == null)
                {
                    continue;
                }

                var name = renderer.gameObject.name;
                var color = name.Contains("Floor") || name.Contains("GardenBase") ? FloorTint :
                    name.Contains("Wall") ? WallTint :
                    name.Contains("Pipe") || name.Contains("Gear") ? PipeTint :
                    name.Contains("Stem") || name.Contains("Plant") ? BloomTint :
                    name.Contains("Glow") ? AccentTint :
                    renderer.sharedMaterial.color;

                renderer.sharedMaterial.color = color;
            }
        }

        private void BuildDecorations()
        {
            var root = GetOrCreateChild("LandscapeEnvironmentAccents");
            CleanupDuplicateChildren(root);

            for (var i = 0; i < 9; i++)
            {
                var x = -5.6f + i * 1.4f;
                EnsurePrimitive(root, $"BrassRail_{i + 1:00}", PrimitiveType.Cylinder, new Vector3(x, 0.22f, -5.85f), new Vector3(0.08f, 0.72f, 0.08f), Quaternion.Euler(90f, 0f, 0f), PipeTint);
                EnsurePrimitive(root, $"BloomLamp_{i + 1:00}", PrimitiveType.Sphere, new Vector3(x, 0.78f, 5.28f), new Vector3(0.22f, 0.22f, 0.22f), Quaternion.identity, AccentTint);
            }

            EnsurePrimitive(root, "LeftHydroTank", PrimitiveType.Cylinder, new Vector3(-7.35f, 1.08f, 0f), new Vector3(0.74f, 1.08f, 0.74f), Quaternion.identity, new Color(0.16f, 0.5f, 0.48f, 1f));
            EnsurePrimitive(root, "RightSeedSilo", PrimitiveType.Cylinder, new Vector3(7.35f, 1.18f, 0f), new Vector3(0.78f, 1.18f, 0.78f), Quaternion.identity, new Color(0.42f, 0.36f, 0.27f, 1f));
            EnsurePrimitive(root, "LandscapeBackplate", PrimitiveType.Cube, new Vector3(0f, 1.1f, 5.86f), new Vector3(13.8f, 1.8f, 0.18f), Quaternion.identity, WallTint);

            EnsureAccentLight(root, "WarmWorkbenchGlow", new Vector3(-4.2f, 2.4f, -3.8f), new Color(1f, 0.72f, 0.32f, 1f), 2.4f, 7f);
            EnsureAccentLight(root, "BloomFillGlow", new Vector3(4.4f, 2.1f, 3.4f), new Color(0.28f, 0.95f, 0.62f, 1f), 1.7f, 6f);
        }

        private Transform GetOrCreateChild(string childName)
        {
            var existing = transform.Find(childName);
            if (existing != null)
            {
                return existing;
            }

            var child = new GameObject(childName).transform;
            child.SetParent(transform, false);
            return child;
        }

        private static void CleanupDuplicateChildren(Transform root)
        {
            var seen = new HashSet<string>();
            for (var i = root.childCount - 1; i >= 0; i--)
            {
                var child = root.GetChild(i);
                if (seen.Add(child.name))
                {
                    continue;
                }

                if (Application.isPlaying)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }

        private static void EnsurePrimitive(Transform parent, string objectName, PrimitiveType type, Vector3 position, Vector3 scale, Quaternion rotation, Color color)
        {
            var existing = parent.Find(objectName);
            var item = existing != null ? existing.gameObject : GameObject.CreatePrimitive(type);
            item.name = objectName;
            item.transform.SetParent(parent, false);
            item.transform.SetLocalPositionAndRotation(position, rotation);
            item.transform.localScale = scale;

            var renderer = item.GetComponent<Renderer>();
            if (renderer != null)
            {
                var material = new Material(Shader.Find("Universal Render Pipeline/Lit"))
                {
                    color = color
                };
                renderer.sharedMaterial = material;
            }
        }

        private static void EnsureAccentLight(Transform parent, string objectName, Vector3 position, Color color, float intensity, float range)
        {
            var existing = parent.Find(objectName);
            var item = existing != null ? existing.gameObject : new GameObject(objectName);
            item.transform.SetParent(parent, false);
            item.transform.localPosition = position;
            var light = item.GetComponent<Light>() != null ? item.GetComponent<Light>() : item.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = color;
            light.intensity = intensity;
            light.range = range;
            light.shadows = LightShadows.None;
        }
    }
}
