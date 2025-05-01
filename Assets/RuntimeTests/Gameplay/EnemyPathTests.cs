using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Platformer.Mechanics;
using RuntimeTests.Gameplay.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;
using Utils;
using Object = UnityEngine.Object;

namespace RuntimeTests.Gameplay
{
    public class EnemyPathTests : TestBase
    {
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
            SceneManager.LoadScene(GameDataPaths.Scenes.MainScene);
        }

        [UnityTearDown]
        public override IEnumerator TearDown()
        {
            var scene = SceneManager.GetSceneByPath(GameDataPaths.Scenes.MainScene);
            yield return base.TearDown();
            {
                var unload = SceneManager.UnloadSceneAsync(scene);
                while (unload is {isDone: false}) { }
            }
            base.TearDown();
        }

        [UnityTest]
        public IEnumerator LoadMainScene_ValidateAllPatrolPaths_PatrolPathsValid()
        {
            yield return null;
            
            var enemyHeight = Object.FindFirstObjectByType<EnemyController>().gameObject.transform.localScale.y;
            var paths = Object.FindObjectsByType<PatrolPath>(FindObjectsSortMode.None);
            var collider = Object.FindFirstObjectByType<TilemapCollider2D>();
            
            Assert.IsNotNull(collider, "Expected collider in scene for level");
            Assert.IsNotEmpty(paths, "Expected PatrolPaths in scene");

            var invalid = new Dictionary<PatrolPath, (PathValidator.PathValidationError, IReadOnlyList<Vector2>)>();
            
            foreach (var path in paths)
            {
                var pathValid = PathValidator.ValidatePath(
                    path,
                    collider,
                    (error, points) => invalid[path] = (error, points),
                    enemyHeight);
                if(!pathValid) continue;
            }

            if (invalid.Count > 0)
            {
                foreach (var kvp in invalid)
                {
                    var point = string.Join(", ", kvp.Value.Item2);
                    Debug.LogError(
                        $"Path: '{kvp.Key.name}' -> {kvp.Value.Item1} at {point}");
                }
                Assert.Fail($"{invalid.Count} patrol path(s) are invalid, see test log for info.");
            }
            
            Assert.That(invalid.Count == 0);
        }
    }

}
