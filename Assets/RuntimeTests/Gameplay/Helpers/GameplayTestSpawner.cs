using Platformer.Mechanics;
using RuntimeTests.Gameplay.Data;
using UnityEngine;

namespace RuntimeTests.Gameplay.Helpers
{
    public static class GameplayTestSpawner
    {
        /// <summary>
        /// Spawns game object with ground collider 100x1 for tests
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static GameObject SpawnGround(Vector3 position)
        {
            var gameObj = new GameObject("Ground_TEST");
            var groundCollider = gameObj.AddComponent<BoxCollider2D>();
            groundCollider.size = new Vector2(100, 1);
            gameObj.transform.position = position;
            return gameObj;
        }
        
        /// <summary>
        /// Spawns player game object & adds PlayerController component.
        /// Created at position
        /// Returns the PlayerController component of the created object
        /// </summary>
        /// <param name="position"></param>
        public static PlayerController SpawnPlayer(Vector3 position)
        {
            var prefab = Resources.Load<GameObject>(GameDataPaths.PlayerPrefab);
            var gameObj = Object.Instantiate(prefab, position, Quaternion.identity);
            gameObj.name = "Player_TEST";
            gameObj.AddComponent<AudioListener>(); // stops complaining about no listeners in scene during test
            return gameObj.GetComponent<PlayerController>();
        }

        /// <summary>
        /// Spawns an enemy game object & adds EnemyController component.
        /// Created at position
        /// Returns the EnemyController component of the created object
        /// </summary>
        /// <param name="position"></param>
        public static EnemyController SpawnEnemy(Vector3 position)
        {
            var prefab = Resources.Load<GameObject>(GameDataPaths.EnemyPrefab);
            var gameObj = Object.Instantiate(prefab, position, Quaternion.identity);
            gameObj.name = "Enemy_TEST";
            return gameObj.GetComponent<EnemyController>();
        }

        /// <summary>
        /// Spawns a token game object & adds TokenInstance component
        /// Adds dummy sprite & animation
        /// Created at position
        /// Returns the TokenInstance component of the created object
        /// </summary>
        /// <param name="position"></param>
        public static TokenInstance SpawnToken(Vector3 position)
        {
            var gameObj = new GameObject("Token_TEST");
            gameObj.AddComponent<BoxCollider2D>().isTrigger = true;
            var renderer = gameObj.AddComponent<SpriteRenderer>();
            
            var token = gameObj.AddComponent<TokenInstance>();

            token._renderer = renderer;

            var dummySprite = CreateDummySprite();
            token.idleAnimation = new [] {dummySprite};
            token.collectedAnimation = new[] {dummySprite};
            token.sprites = token.idleAnimation;
                
            gameObj.transform.position = position;
            return token;
        }

        /// <summary>
        /// Creates a dummy sprite using very basic texture.
        /// Created in white 10x10 square unless specified otherwise
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="colour"></param>
        public static Sprite CreateDummySprite(int width = 10, int height = 10, Color? colour = null)
        {
            var texture = new Texture2D(width, height);
            var pixels = new Color[width * height];
            var fillColour = colour ?? Color.white;
            
            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i] = fillColour;
            }
            
            texture.SetPixels(pixels);
            texture.Apply();
            
            return Sprite.Create(texture, new Rect(0,0, width, height), new Vector2(0.5f, 0.5f));
        }

        public static PatrolPath CreateEnemyPath(Vector2 startPos, Vector2 endPos)
        {
            var gameObj = new GameObject("PatrolPath_TEST");
            gameObj.transform.position = Vector3.zero;
            
            var path = gameObj.AddComponent<PatrolPath>();
            path.startPosition = startPos;
            path.endPosition = endPos;

            return path;
        }
    }
}