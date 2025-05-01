using System.Collections;
using NUnit.Framework;
using Platformer.Mechanics;
using UnityEngine;
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
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
            
            testSpawner.SpawnGround(new Vector3(0, -1f, 0));
            collectableToken = testSpawner.SpawnToken(collectablePosition);

            controller = new GameObject("TokenController_TEST").AddComponent<TokenController>();
            Assert.IsTrue(controller.tokens.Length > 0, "TokenController length doesn't match expected > 0");
        }
        
        /// <summary>
        /// Spawns player, moves player towards token, asserts if token was collected
        /// We yield for a token collected flag just incase trigger isn't occurring in the MovePlayerToPosition frames
        /// </summary>
        [UnityTest]
        public IEnumerator PlayerSpawned_PlayerMovesOverToken_TokenCollected()
        {
            player = testSpawner.SpawnPlayer(Vector3.zero);

            yield return movementHelper.MovePlayerToPosition(player, collectablePosition, threshold: 0.2f);
            yield return waitHelper.WaitForTokenCollected(collectableToken); 
            
            Assert.IsTrue(collectableToken.collected, "Expected player to collect token when moving towards token");
        }

        [UnityTest]
        public IEnumerator EnemySpawned_EnemyMovesOverToken_NoTokenCollected()
        {
            enemy = testSpawner.SpawnEnemy(Vector3.zero);
            var path = testSpawner.CreateEnemyPath(new Vector2(-1, 0), new Vector2(6, 0));
            movementHelper.MoveEnemyAlongPatrol(enemy, path);

            yield return waitHelper.WaitForOverlap(enemy.gameObject, collectableToken.gameObject);
            
            Assert.False(collectableToken.collected, "Expected token to not be collected by enemy");
        }
    }
}