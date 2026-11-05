using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MechaBloom.EditorTools
{
    public static class MechaBloomScenePolishTool
    {
        private static readonly Color HydroBlue = new(0.04f, 0.55f, 0.92f, 1f);
        private static readonly Color WaterGlow = new(0.24f, 0.82f, 1f, 1f);
        private static readonly Color PlanterBrown = new(0.34f, 0.22f, 0.13f, 1f);
        private static readonly Color StemGreen = new(0.24f, 0.78f, 0.4f, 1f);
        private static readonly Color BloomYellow = new(1f, 0.82f, 0.24f, 1f);
        private static readonly Color Brass = new(0.68f, 0.52f, 0.27f, 1f);

        [MenuItem("Tools/MechaBloom/Polish Gameplay Scene View")]
        public static void PolishScene()
        {
            ConfigureCamera();
            PolishEnvironmentAccents();
            PolishLevelOneObjects();

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Tools/MechaBloom/Unpause Editor Play Mode")]
        public static void UnpauseEditorPlayMode()
        {
            EditorApplication.isPaused = false;
        }

        [MenuItem("Tools/MechaBloom/Play Mode Start Game")]
        public static void PlayModeStartGame()
        {
            if (!EditorApplication.isPlaying)
            {
                return;
            }

            var gameManager = Find("GameScene/Managers/GameManager");
            var levelManager = Find("GameScene/Managers/LevelManager");
            var uiManager = Find("GameScene/Managers/UIManager");
            if (levelManager != null)
            {
                levelManager.SendMessage("LoadLevel", 1, SendMessageOptions.DontRequireReceiver);
                uiManager?.SendMessage("ShowGameplay", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                gameManager?.SendMessage("PlayFirstUnlockedLevel", SendMessageOptions.DontRequireReceiver);
            }

            EditorApplication.isPaused = false;
        }

        private static void ConfigureCamera()
        {
            var camera = Camera.main;
            if (camera == null)
            {
                return;
            }

            camera.orthographic = true;
            camera.orthographicSize = 11.4f;
            camera.transform.position = new Vector3(7.2f, 10.2f, -7.2f);
            camera.transform.rotation = Quaternion.Euler(55f, 315f, 0f);
            camera.nearClipPlane = 0.3f;
            camera.farClipPlane = 1000f;
        }

        private static void PolishEnvironmentAccents()
        {
            var accents = Find("GameScene/Environment/LandscapeEnvironmentAccents");
            if (accents == null)
            {
                return;
            }

            var stemXs = new[] { -6f, -4.3f, -2.6f, -0.9f, 0.8f, 2.5f, 4.2f, 5.9f };
            for (var i = 0; i < stemXs.Length; i++)
            {
                var lamp = Find($"GameScene/Environment/LandscapeEnvironmentAccents/BloomLamp_{i + 1:00}");
                if (lamp != null)
                {
                    lamp.name = $"StemBloomNode_{i + 1:00}";
                    SetTransform(lamp.transform, new Vector3(stemXs[i], 1.02f, 5.55f), Quaternion.identity, new Vector3(0.18f, 0.18f, 0.18f));
                    SetColor(lamp, BloomYellow);
                }

                var rail = Find($"GameScene/Environment/LandscapeEnvironmentAccents/BrassRail_{i + 1:00}");
                if (rail != null)
                {
                    rail.name = $"OuterBrassRail_{i + 1:00}";
                    SetTransform(rail.transform, new Vector3(stemXs[i], 0.18f, -5.72f), Quaternion.Euler(90f, 0f, 0f), new Vector3(0.06f, 0.48f, 0.06f));
                    SetColor(rail, Brass);
                }
            }

            DestroyIfFound("GameScene/Environment/LandscapeEnvironmentAccents/BloomLamp_09");
            DestroyIfFound("GameScene/Environment/LandscapeEnvironmentAccents/BrassRail_09");
            DestroyIfFound("GameScene/Environment/LandscapeEnvironmentAccents/LeftHydroTank");

            var reservoir = Find("GameScene/Environment/LandscapeEnvironmentAccents/LeftHydroReservoir");
            if (reservoir != null)
            {
                SetTransform(reservoir.transform, new Vector3(-7.45f, 0.78f, 0.9f), Quaternion.identity, new Vector3(0.58f, 0.78f, 0.58f));
                SetColor(reservoir, new Color(0.16f, 0.5f, 0.48f, 1f));
            }

            var silo = Find("GameScene/Environment/LandscapeEnvironmentAccents/RightSeedSilo");
            if (silo != null)
            {
                SetTransform(silo.transform, new Vector3(7.45f, 0.86f, 0.9f), Quaternion.identity, new Vector3(0.58f, 0.86f, 0.58f));
            }

            MoveCanopyBehindBoard();
        }

        private static void MoveCanopyBehindBoard()
        {
            for (var i = 1; i <= 5; i++)
            {
                var beam = Find($"GameScene/Environment/LandscapeEnvironmentAccents/GreenhouseBackground/CanopyBeam_{i:00}");
                if (beam == null)
                {
                    continue;
                }

                var x = -7.2f + (i - 1) * 3.6f;
                SetTransform(beam.transform, new Vector3(x, 4.82f, 6.25f), Quaternion.Euler(0f, 0f, i < 3 ? -7f : i > 3 ? 7f : 0f), new Vector3(0.12f, 0.12f, 2.7f));
                SetColor(beam, Brass);
            }

            SetIfFound("GameScene/Environment/LandscapeEnvironmentAccents/GreenhouseBackground/LeftHeader", new Vector3(-8.2f, 4.5f, 5.6f), Quaternion.identity, new Vector3(0.16f, 0.16f, 3.8f), Brass);
            SetIfFound("GameScene/Environment/LandscapeEnvironmentAccents/GreenhouseBackground/RightHeader", new Vector3(8.2f, 4.5f, 5.6f), Quaternion.identity, new Vector3(0.16f, 0.16f, 3.8f), Brass);
            SetIfFound("GameScene/Environment/LandscapeEnvironmentAccents/GreenhouseBackground/RoofHeader", new Vector3(0f, 4.72f, 6.9f), Quaternion.identity, new Vector3(17.2f, 0.18f, 0.22f), Brass);
        }

        private static void PolishLevelOneObjects()
        {
            PolishTutorialPipes();

            var source = Find("GameScene/Levels/Level_01/WaterSources/WaterSource_01") ??
                Find("GameScene/Levels/Level_01/WaterSources/WaterSource_01_BlueHydroPump");
            if (source != null)
            {
                source.name = "WaterSource_01_BlueHydroPump";
                SetTransform(source.transform, new Vector3(-2.2f, 0.5f, 0f), Quaternion.identity, new Vector3(0.52f, 0.42f, 0.52f));
                SetColor(source, HydroBlue);
                SetChildPrimitive(source.transform, "HydroPumpBase", PrimitiveType.Cylinder, new Vector3(0f, -0.35f, 0f), Quaternion.identity, new Vector3(1.3f, 0.16f, 1.3f), Brass);
                SetChildPrimitive(source.transform, "HydroPumpCap", PrimitiveType.Sphere, new Vector3(0f, 0.95f, 0f), Quaternion.identity, new Vector3(0.9f, 0.35f, 0.9f), WaterGlow);

                var spout = source.transform.Find("Spout");
                if (spout != null)
                {
                    spout.name = "OutletSpout_ToPipe";
                    SetTransform(spout, new Vector3(1.12f, 0.05f, 0f), Quaternion.identity, new Vector3(1.36f, 0.22f, 0.18f), true);
                    SetColor(spout.gameObject, WaterGlow);
                }
            }

            var plant = Find("GameScene/Levels/Level_01/PlantBeds/PlantBed_01") ??
                Find("GameScene/Levels/Level_01/PlantBeds/PlantBed_01_BloomPlanter");
            if (plant != null)
            {
                plant.name = "PlantBed_01_BloomPlanter";
                SetTransform(plant.transform, new Vector3(2.2f, 0.42f, 0f), Quaternion.identity, new Vector3(0.76f, 0.25f, 0.76f));
                SetColor(plant, PlanterBrown);
                SetChildPrimitive(plant.transform, "PlanterRim", PrimitiveType.Cylinder, new Vector3(0f, 0.62f, 0f), Quaternion.identity, new Vector3(1.16f, 0.28f, 1.16f), Brass);

                var sprout = plant.transform.Find("SproutPreview");
                if (sprout != null)
                {
                    sprout.name = "AttachedGreenSprout";
                    SetTransform(sprout, new Vector3(0f, 1.7f, 0f), Quaternion.identity, new Vector3(0.22f, 1.9f, 0.22f), true);
                    SetColor(sprout.gameObject, StemGreen);
                }

                var bloom = plant.transform.Find("BloomPreview");
                if (bloom != null)
                {
                    bloom.name = "AttachedYellowBloom";
                    SetTransform(bloom, new Vector3(0f, 3f, 0f), Quaternion.identity, new Vector3(0.42f, 0.75f, 0.42f), true);
                    SetColor(bloom.gameObject, BloomYellow);
                }
            }
        }

        private static void PolishTutorialPipes()
        {
            for (var i = 1; i <= 3; i++)
            {
                var pipe = Find($"GameScene/Levels/Level_01/Pipes/Pipe_{i:00}") ??
                    Find($"GameScene/Levels/Level_01/Pipes/TutorialPipeSegment_{i:00}");
                if (pipe == null)
                {
                    continue;
                }

                var x = -1.1f + (i - 1) * 1.1f;
                pipe.name = $"TutorialPipeSegment_{i:00}";
                SetTransform(pipe.transform, new Vector3(x, 0.26f, 0f), Quaternion.Euler(0f, 0f, 90f), new Vector3(0.22f, 0.54f, 0.22f));
                SetColor(pipe, new Color(0.46f, 0.5f, 0.44f, 1f));

                var preview = pipe.transform.Find("WaterPreview");
                if (preview != null)
                {
                    SetTransform(preview, Vector3.zero, Quaternion.identity, new Vector3(0.72f, 1.02f, 0.72f), true);
                    SetColor(preview.gameObject, WaterGlow);
                }
            }
        }

        private static void SetIfFound(string path, Vector3 position, Quaternion rotation, Vector3 scale, Color color)
        {
            var go = Find(path);
            if (go == null)
            {
                return;
            }

            SetTransform(go.transform, position, rotation, scale);
            SetColor(go, color);
        }

        private static void SetChildPrimitive(Transform parent, string name, PrimitiveType type, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Color color)
        {
            var child = parent.Find(name);
            GameObject go;
            if (child == null)
            {
                go = GameObject.CreatePrimitive(type);
                go.name = name;
                go.transform.SetParent(parent, false);
                var collider = go.GetComponent<Collider>();
                if (collider != null)
                {
                    Object.DestroyImmediate(collider);
                }
            }
            else
            {
                go = child.gameObject;
            }

            SetTransform(go.transform, localPosition, localRotation, localScale, true);
            SetColor(go, color);
        }

        private static void SetTransform(Transform transform, Vector3 position, Quaternion rotation, Vector3 scale, bool local = false)
        {
            if (local)
            {
                transform.localPosition = position;
                transform.localRotation = rotation;
                transform.localScale = scale;
                return;
            }

            transform.SetPositionAndRotation(position, rotation);
            transform.localScale = scale;
        }

        private static void SetColor(GameObject go, Color color)
        {
            var renderer = go.GetComponent<Renderer>();
            if (renderer == null)
            {
                return;
            }

            var material = renderer.sharedMaterial;
            if (material == null || AssetDatabase.GetAssetPath(material).Length > 0)
            {
                material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                renderer.sharedMaterial = material;
            }

            material.color = color;
        }

        private static void DestroyIfFound(string path)
        {
            var go = Find(path);
            if (go != null)
            {
                Object.DestroyImmediate(go);
            }
        }

        private static GameObject Find(string path)
        {
            var parts = path.Split('/');
            if (parts.Length == 0)
            {
                return null;
            }

            GameObject current = null;
            foreach (var root in EditorSceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (root.name == parts[0])
                {
                    current = root;
                    break;
                }
            }

            for (var i = 1; i < parts.Length && current != null; i++)
            {
                var found = current.transform.Find(parts[i]);
                current = found != null ? found.gameObject : null;
            }

            return current;
        }
    }
}
