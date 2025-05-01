using System;
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
        [MenuItem("Tools/Validate Paths In Current Scene")]
        public static void ValidatePathsInScene()
        {
            var mapCollision = Object.FindFirstObjectByType<TilemapCollider2D>();
            if (mapCollision == null)
            {
                Debug.LogError("No TilemapCollider2D found in scene!");
                return;
            }
            
            var paths = Object.FindObjectsByType<PatrolPath>(FindObjectsSortMode.None);
            var invalidPaths = new Dictionary<GameObject, PathValidationError>();

            foreach (var path in paths)
            {
                var originalName = path.gameObject.name
                    .Replace("[Obstructed] ", "")
                    .Replace("[MissingGround] ", "");
                
                PathValidator.ValidatePath(path, mapCollision, error =>
                {
                    invalidPaths[path.gameObject] = error;
                    path.gameObject.name = error switch
                    {
                        PathValidationError.Obstructed => $"[Obstructed] {originalName}",
                        PathValidationError.MissingGround => $"[MissingGround] {originalName}",
                        _ => originalName;
                    };
                });

                if (!invalidPaths.ContainsKey(path.gameObject))
                {
                    path.gameObject.name = path.gameObject.name
                        .Replace("[Obstructed] ", "")
                        .Replace("[MissingGround] ", "");
                }
            }

            if (invalidPaths.Any())
            {
                Selection.objects = invalidPaths.Keys.ToArray();

                foreach (var path in invalidPaths)
                {
                    Debug.LogError($"Invalid PatrolPath '{path.Key}': {path.Value}");
                }

                EditorUtility.DisplayDialog("Path Validator",
                    $"Found {invalidPaths.Count} invalid paths in scene, selected in Hierarchy", "Continue");
            }
            else
            {
                EditorUtility.DisplayDialog("Path Validator", "All Patrol Paths in scene are valid!", "Continue");
            }
        }
    }
}