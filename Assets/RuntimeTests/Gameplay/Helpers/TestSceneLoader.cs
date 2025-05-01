using System;
using System.Collections;
using RuntimeTests.Gameplay.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RuntimeTests.Gameplay.Helpers
{
    public static class TestSceneLoader
    {
        public static IEnumerator UnloadActiveSceneAndReplaceWithFallback()
        {
            var scene = SceneManager.GetSceneByPath(GameDataPaths.Scenes.MainScene);
            if (!scene.IsValid() || !scene.isLoaded)
            {
                yield break;
            }

            if (SceneManager.sceneCount == 1 && SceneManager.GetSceneAt(0) == scene)
            {
                var fallback = SceneManager.CreateScene("FallbackScene_" + Guid.NewGuid());
                SceneManager.SetActiveScene(fallback);
                yield return null;
            }
            
            var unload = SceneManager.UnloadSceneAsync(scene);
            if(unload != null)
            {
                yield return new WaitUntil(() => unload.isDone);
            }
        }

        public static IEnumerator LoadMainScene()
        {
            var load = SceneManager.LoadSceneAsync(GameDataPaths.Scenes.MainScene, LoadSceneMode.Single);
            yield return new WaitUntil(() => load.isDone);

            var loadedScene = SceneManager.GetSceneByPath(GameDataPaths.Scenes.MainScene);
            if (!loadedScene.IsValid() || !loadedScene.isLoaded)
            {
                Debug.LogError($"Main scene couldn't be loaded from {GameDataPaths.Scenes.MainScene}");
                yield break;
            }

            SceneManager.SetActiveScene(loadedScene);
        }

        public static IEnumerator CreateAndSetTestScene(Action<Scene> onSceneCreated)
        {
            var scene = SceneManager.CreateScene("TestScene_" + Guid.NewGuid());
            SceneManager.SetActiveScene(scene);
            onSceneCreated?.Invoke(scene);
            yield return null;
        }
    }
}