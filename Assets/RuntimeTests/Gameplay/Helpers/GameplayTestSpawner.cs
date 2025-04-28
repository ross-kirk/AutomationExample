using Platformer.Mechanics;
using RuntimeTests.Gameplay.Data;
using UnityEngine;

namespace RuntimeTests.Gameplay.Helpers
{
    public static class GameplayTestSpawner
    {
        public static PlayerController SpawnPlayer(Vector3 position)
        {
            var prefab = Resources.Load<GameObject>(GameDataPaths.PlayerPrefab);
            var gameObj = Object.Instantiate(prefab, position, Quaternion.identity);
            gameObj.name = "Player_TEST";
            return gameObj.GetComponent<PlayerController>();
        }

        public static EnemyController SpawnEnemy(Vector3 position)
        {
            var prefab = Resources.Load<GameObject>(GameDataPaths.EnemyPrefab);
            var gameObj = Object.Instantiate(prefab, position, Quaternion.identity);
            gameObj.name = "Enemy_TEST";
            return gameObj.GetComponent<EnemyController>();
        }

        public static TokenInstance SpawnToken(Vector3 position)
        {
            var gameObj = new GameObject("Token_TEST");
            var token = gameObj.AddComponent<TokenInstance>();

            gameObj.AddComponent<BoxCollider2D>().isTrigger = true;
            gameObj.AddComponent<SpriteRenderer>();
            gameObj.transform.position = position;
            return token;
        }
    }
}