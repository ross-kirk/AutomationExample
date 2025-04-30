using System;
using System.Collections.Generic;
using System.Linq;
using Platformer.Mechanics;
using UnityEditor;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;
using PathValidationError = Utils.PathValidator.PathValidationError;

namespace Platformer.Utils
{
    public static class EditorPathValidatorUtil
    {
        [MenuItem("Tools/Validate Paths In Current Scene")]
        public static void ValidatePathsInScene()
        {
            var paths = Object.FindObjectsByType<PatrolPath>(FindObjectsSortMode.None);
            var invalidPaths = new Dictionary<GameObject, PathValidationError>();

            foreach (var path in paths)
            {
                var originalName = path.gameObject.name;
                originalName = originalName.Replace("[Blocked] ", "").Replace("[MissingGround] ", "");

                PathValidator.ValidatePathByTile(path, error =>
                {
                    invalidPaths.TryAdd(path.gameObject, error);
                });

                if (invalidPaths.TryGetValue(path.gameObject, out var errorType))
                {
                    var prefix = errorType switch
                    {
                        PathValidationError.Blocked => "[Blocked] ",
                        PathValidationError.MissingGround => "[MissingGround]",
                        _ => "[Invalid] "
                    };
                    path.gameObject.name = prefix + originalName;
                }
                else
                {
                    path.gameObject.name = originalName;
                }
            }

            if (invalidPaths.Count > 0)
            {
                foreach (var kvp in invalidPaths)
                {
                    var reason = kvp.Value == PathValidationError.Blocked ? "Blocked by tile" : "Missing ground below";
                    Debug.LogError($"[PATH VALIDATOR] - Invalid Path: {kvp.Key.name} - {reason}", kvp.Key);
                }

                Selection.objects = invalidPaths.Keys.ToArray();
                EditorUtility.DisplayDialog("PATH VALIDATOR",
                    $"Highlighted {invalidPaths.Count} invalid patrol paths in scene", "Continue");
                Debug.Log($"[PATH VALIDATOR] - Highlighted {invalidPaths.Count} invalid patrol paths in scene");
            }
            else
            {
                EditorUtility.DisplayDialog("PATH VALIDATOR", "All paths valid!", "Continue");
            }
        }
    }
}