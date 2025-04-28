using System.Collections;
using NUnit.Framework;
using Platformer.Mechanics;
using RuntimeTests.Gameplay.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace RuntimeTests.Gameplay
{
    public class CollectableTests : GameplayTestBase
    {
        private TokenInstance collectableToken;
        private TokenController controller;
        private PlayerController player;
        private EnemyController enemy;

        private readonly Vector3 collectablePosition = new Vector3(5, 0, 0);
        
        /// <summary>
        /// Always create token for new test at the same position
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            
            GameplayTestSpawner.SpawnGround(new Vector3(0, -1f, 0));
            collectableToken = GameplayTestSpawner.SpawnToken(collectablePosition);

            controller = new GameObject("TokenController_TEST").AddComponent<TokenController>();
        }
        
        /// <summary>
        /// Spawns player, moves player towards token, asserts if token was collected
        /// We yield for a token collected flag just incase trigger isn't occurring in the MovePlayerToPosition frames
        /// </summary>
        [UnityTest]
        public IEnumerator PlayerSpawned_PlayerMovesOverToken_TokenCollected()
        {
            player = GameplayTestSpawner.SpawnPlayer(Vector3.zero);

            yield return GameplayMovementHelper.MovePlayerToPosition(player, collectablePosition, threshold: 0.2f);
            yield return GameplayWaitHelper.WaitForTokenCollected(collectableToken); 
            
            Assert.IsTrue(collectableToken.collected, "Expected player to collect token when moving towards token");
        }

        [UnityTest]
        public IEnumerator EnemySpawned_EnemyMovesOverToken_NoTokenCollected()
        {
            enemy = GameplayTestSpawner.SpawnEnemy(Vector3.zero);
            var path = GameplayTestSpawner.CreateEnemyPath(new Vector2(-1, 0), new Vector2(6, 0));
            GameplayMovementHelper.MoveEnemyAlongPatrol(enemy, path);

            yield return GameplayWaitHelper.WaitForOverlap(enemy.gameObject, collectableToken.gameObject);
            
            Assert.False(collectableToken.collected, "Expected token to not be collected by enemy");
        }
    }
}