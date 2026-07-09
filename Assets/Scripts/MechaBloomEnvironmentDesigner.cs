using System.Collections.Generic;
using UnityEngine;

namespace MechaBloom
{
    [ExecuteAlways]
    public sealed class MechaBloomEnvironmentDesigner : MonoBehaviour
    {
        private static readonly Color FloorTint = new(0.16f, 0.27f, 0.23f, 1f);
        private static readonly Color WallTint = new(0.24f, 0.36f, 0.31f, 1f);
        private static readonly Color PipeTint = new(0.62f, 0.51f, 0.34f, 1f);
        private static readonly Color BloomTint = new(0.28f, 0.82f, 0.5f, 1f);
        private static readonly Color AccentTint = new(1f, 0.8f, 0.32f, 1f);
        private static readonly Color BackdropTint = new(0.11f, 0.24f, 0.24f, 1f);
        private static readonly Color DistantTint = new(0.08f, 0.17f, 0.18f, 1f);

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
            if (Application.isPlaying)
            {
                return;
            }

            ResolveReferences();
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

        private void ConfigureLighting()
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.48f, 0.58f, 0.52f, 1f);
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.12f, 0.2f, 0.19f, 1f);
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.006f;

            if (sunLight == null)
            {
                return;
            }

            sunLight.transform.rotation = Quaternion.Euler(50f, -34f, 12f);
            sunLight.color = new Color(1f, 0.9f, 0.67f, 1f);
            sunLight.intensity = 2.15f;
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

            var stemPositions = new[]
            {
                -6f, -4.3f, -2.6f, -0.9f, 0.8f, 2.5f, 4.2f, 5.9f
            };

            for (var i = 0; i < stemPositions.Length; i++)
            {
                var x = stemPositions[i];
                EnsurePrimitive(root, $"BrassRail_{i + 1:00}", PrimitiveType.Cylinder, new Vector3(x, 0.18f, -5.72f), new Vector3(0.06f, 0.48f, 0.06f), Quaternion.Euler(90f, 0f, 0f), PipeTint);
                EnsurePrimitive(root, $"BloomLamp_{i + 1:00}", PrimitiveType.Sphere, new Vector3(x, 1.02f, 5.55f), new Vector3(0.18f, 0.18f, 0.18f), Quaternion.identity, AccentTint);
            }

            RemoveChild(root, "BloomLamp_09");
            RemoveChild(root, "BrassRail_09");

            EnsurePrimitive(root, "LeftHydroReservoir", PrimitiveType.Cylinder, new Vector3(-7.45f, 0.78f, 0.9f), new Vector3(0.58f, 0.78f, 0.58f), Quaternion.identity, new Color(0.16f, 0.5f, 0.48f, 1f));
            RemoveChild(root, "LeftHydroTank");
            EnsurePrimitive(root, "RightSeedSilo", PrimitiveType.Cylinder, new Vector3(7.45f, 0.86f, 0.9f), new Vector3(0.58f, 0.86f, 0.58f), Quaternion.identity, new Color(0.42f, 0.36f, 0.27f, 1f));
            BuildBackground(root);

            EnsureAccentLight(root, "WarmWorkbenchGlow", new Vector3(-4.2f, 2.4f, -3.8f), new Color(1f, 0.72f, 0.32f, 1f), 3.2f, 8f);
            EnsureAccentLight(root, "BloomFillGlow", new Vector3(4.4f, 2.1f, 3.4f), new Color(0.28f, 0.95f, 0.62f, 1f), 2.6f, 7f);
        }

        private static void BuildBackground(Transform root)
        {
            RemoveChild(root, "LandscapeBackplate");
            var background = GetOrCreateChild(root, "GreenhouseBackground");

            EnsurePrimitive(background, "RearWall", PrimitiveType.Cube, new Vector3(0f, 2.35f, 7.25f), new Vector3(17.5f, 4.6f, 0.24f), Quaternion.identity, BackdropTint);
            EnsurePrimitive(background, "LeftWing", PrimitiveType.Cube, new Vector3(-8.5f, 2.1f, 3.1f), new Vector3(0.24f, 4.2f, 8.2f), Quaternion.identity, BackdropTint);
            EnsurePrimitive(background, "RightWing", PrimitiveType.Cube, new Vector3(8.5f, 2.1f, 3.1f), new Vector3(0.24f, 4.2f, 8.2f), Quaternion.identity, BackdropTint);
            EnsurePrimitive(background, "GroundSkirt", PrimitiveType.Cube, new Vector3(0f, -0.3f, 3.2f), new Vector3(17.2f, 0.35f, 8.2f), Quaternion.identity, FloorTint);

            for (var i = 0; i < 7; i++)
            {
                var x = -7.2f + i * 2.4f;
                var towerHeight = 1.5f + (i % 3) * 0.55f;
                EnsurePrimitive(background, $"DistantPlanter_{i + 1:00}", PrimitiveType.Cube,
                    new Vector3(x, 0.65f + towerHeight * 0.5f, 6.82f),
                    new Vector3(1.45f, towerHeight, 0.35f), Quaternion.identity, DistantTint);
                EnsurePrimitive(background, $"WindowGlow_{i + 1:00}", PrimitiveType.Cube,
                    new Vector3(x, 2.9f, 6.65f), new Vector3(1.35f, 0.12f, 0.08f),
                    Quaternion.identity, i % 2 == 0 ? AccentTint : BloomTint);
            }

            for (var i = 0; i < 5; i++)
            {
                var x = -7.2f + i * 3.6f;
                EnsurePrimitive(background, $"CanopyBeam_{i + 1:00}", PrimitiveType.Cube,
                    new Vector3(x, 4.82f, 6.25f), new Vector3(0.12f, 0.12f, 2.7f),
                    Quaternion.Euler(0f, 0f, i < 2 ? -7f : i > 2 ? 7f : 0f), PipeTint);
            }

            EnsurePrimitive(background, "RoofHeader", PrimitiveType.Cube, new Vector3(0f, 4.72f, 6.9f), new Vector3(17.2f, 0.18f, 0.22f), Quaternion.identity, PipeTint);
            EnsurePrimitive(background, "LeftHeader", PrimitiveType.Cube, new Vector3(-8.2f, 4.5f, 5.6f), new Vector3(0.16f, 0.16f, 3.8f), Quaternion.identity, PipeTint);
            EnsurePrimitive(background, "RightHeader", PrimitiveType.Cube, new Vector3(8.2f, 4.5f, 5.6f), new Vector3(0.16f, 0.16f, 3.8f), Quaternion.identity, PipeTint);

            EnsureAccentLight(background, "BackdropWarmLight", new Vector3(-5.6f, 3.5f, 5.8f), new Color(1f, 0.68f, 0.3f, 1f), 1.8f, 5.5f);
            EnsureAccentLight(background, "BackdropGreenLight", new Vector3(5.6f, 3.5f, 5.8f), new Color(0.25f, 0.86f, 0.58f, 1f), 1.6f, 5.5f);
        }

        private static void RemoveChild(Transform parent, string childName)
        {
            var child = parent.Find(childName);
            if (child == null)
            {
                return;
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

        private Transform GetOrCreateChild(string childName)
        {
            return GetOrCreateChild(transform, childName);
        }

        private static Transform GetOrCreateChild(Transform parent, string childName)
        {
            var existing = parent.Find(childName);
            if (existing != null)
            {
                return existing;
            }

            var child = new GameObject(childName).transform;
            child.SetParent(parent, false);
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

            var collider = item.GetComponent<Collider>();
            if (collider != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(collider);
                }
                else
                {
                    DestroyImmediate(collider);
                }
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
