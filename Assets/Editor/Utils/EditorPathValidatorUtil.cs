using System.Collections.Generic;
using System.Linq;
using Platformer.Mechanics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using Object = UnityEngine.Object;
using PathValidationError = Utils.PathValidator.PathValidationError;

namespace Platformer.Utils
{
    public static class EditorPathValidatorUtil
    {
        [MenuItem("Tools/Level Editor/Validate Paths In Current Scene")]
        public static void ValidatePathsInScene()
        {
            RunValidation(Object.FindObjectsByType<PatrolPath>(FindObjectsSortMode.None));
        }

        [MenuItem("Tools/Level Editor/Validate Selected Patrol Path(s)")]
        public static void ValidateSelectedPath()
        {
            var selected = Selection.gameObjects
                .Select(gameObject => gameObject.GetComponent<PatrolPath>())
                .Where(path => path != null).ToArray();

            if (selected.Length == 0)
            {
                EditorUtility.DisplayDialog("Path Validator", 
                    "No paths selected, skipping validation.", "Continue");
                return;
            }
            
            RunValidation(selected);
        }

        private static void RunValidation(IEnumerable<PatrolPath> paths)
        {
            var mapCollision = Object.FindFirstObjectByType<TilemapCollider2D>();
            if (mapCollision == null)
            {
                Debug.LogError("No TilemapCollider2D found in scene!");
                return;
            }

            var invalidPaths = new Dictionary<GameObject, (PathValidationError,List<Vector2>)>();

            foreach (var path in paths)
            {
                var originalName = path.gameObject.name
                    .Replace("[Obstructed] ", "")
                    .Replace("[MissingGround] ", "");

                PathValidator.ValidatePath(
                    path,
                    mapCollision,
                    (error, points) =>
                    {
                        invalidPaths[path.gameObject] = (error, points.ToList());
                        var errorPrefix = error == PathValidationError.Obstructed
                            ? "[Obstructed]"
                            : "[MissingGround]";
                        path.gameObject.name = $"{errorPrefix} {originalName}";
                    });

                if (!invalidPaths.ContainsKey(path.gameObject))
                {
                    path.gameObject.name = originalName;
                }
            }

            if (invalidPaths.Any())
            {
                Selection.objects = invalidPaths.Keys.ToArray();

                foreach (var kvp in invalidPaths)
                {
                    var points = string.Join(", ", kvp.Value.Item2.Select(vec => $"({vec.x:0.##},{vec.y:0.##})"));
                    Debug.LogError($"Path '{kvp.Key.name}' -> {kvp.Value.Item1}: {points}");
                }

                EditorUtility.DisplayDialog("Path Validator",
                    $"Found {invalidPaths.Count} invalid patrol path(s). They have been selected in the Hierarchy view",
                    "Continue");
            }
            else
            {
                EditorUtility.DisplayDialog("Path Validator",
                    "All PatrolPaths are valid", "Continue");
            }
        }
    }
}