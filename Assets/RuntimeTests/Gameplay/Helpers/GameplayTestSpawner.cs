using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using RuntimeTests.Gameplay.Data;
using UnityEngine;

namespace RuntimeTests.Gameplay.Helpers
{
    public class GameplayTestSpawner
    {
        /// <summary>
        /// Spawn game object with ground collider of specific size for tests
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public GameObject SpawnGround(Vector3 position, Vector2 size)
        {
            var gameObj = new GameObject("Ground_TEST");
            var groundCollider = gameObj.AddComponent<BoxCollider2D>();
            groundCollider.size = size;
            gameObj.transform.position = position;
            return gameObj;
        }
        
        /// <summary>
        /// Spawns game object with ground collider 100x1 for tests
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public GameObject SpawnGround(Vector3 position)
        {
            return SpawnGround(position, new Vector2(100, 1));
        }
        
        /// <summary>
        /// Spawns player game object & adds PlayerController component.
        /// Created at position
        /// Returns the PlayerController component of the created object
        /// </summary>
        /// <param name="position"></param>
        public PlayerController SpawnPlayer(Vector3 position)
        {
            var prefab = Resources.Load<GameObject>(GameDataPaths.PlayerPrefab);
            var gameObj = Object.Instantiate(prefab, position, Quaternion.identity);
            gameObj.name = "Player_TEST";
            gameObj.AddComponent<AudioListener>(); // stops complaining about no listeners in scene during test
            var controller = gameObj.GetComponent<PlayerController>();
            Simulation.GetModel<PlatformerModel>().player = controller;
            return controller;
        }

        /// <summary>
        /// Spawns an enemy game object & adds EnemyController component.
        /// Created at position
        /// Returns the EnemyController component of the created object
        /// </summary>
        /// <param name="position"></param>
        public EnemyController SpawnEnemy(Vector3 position)
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
        public TokenInstance SpawnToken(Vector3 position)
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

            token.tokenCollectAudio = CreateDummyAudioClip();
            
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
        public Sprite CreateDummySprite(int width = 10, int height = 10, Color? colour = null)
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

        /// <summary>
        /// Create enemy path to use within the game
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        public PatrolPath CreateEnemyPath(Vector2 startPos, Vector2 endPos)
        {
            var gameObj = new GameObject("PatrolPath_TEST");
            gameObj.transform.position = Vector3.zero;
            
            var path = gameObj.AddComponent<PatrolPath>();
            path.startPosition = startPos;
            path.endPosition = endPos;

            return path;
        }

        /// <summary>
        /// Create death zone at specified position of specified size
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public DeathZone CreateDeathZone(Vector3 position, Vector2 size)
        {
            var gameObj = new GameObject("DeathZone_TEST");
            var collider = gameObj.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = size;
            gameObj.transform.position = position;
            
            return gameObj.AddComponent<DeathZone>();
        }

        /// <summary>
        /// Create dummy audio clip to avoid programmatically created objects raising nullrefs on PlayAudio calls
        /// </summary>
        /// <returns></returns>
        public AudioClip CreateDummyAudioClip()
        {
            return AudioClip.Create("TestClip", 44100, 1, 44100, false);
        }
    }
}