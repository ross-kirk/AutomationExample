using System.Collections;
using NUnit.Framework;
using Platformer.Mechanics;
using RuntimeTests.Core;
using UnityEngine;
using UnityEngine.TestTools;

namespace RuntimeTests.Gameplay
{
    public class PlayerDeathTests : GameplayTestBase
    {
        private PlayerController player;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            player = testSpawner.SpawnPlayer(new Vector3(0, 0));
        }

        [UnityTest]
        public IEnumerator DeathZoneBelowPlayer_PlayerFallsIntoZone_PlayerDeath()
        {
            var deathZone = testSpawner.CreateDeathZone(new Vector3(0, -5), new Vector2(6, 1));

            yield return waitHelper.WaitForContactWith(
                player.gameObject,
                deathZone.gameObject,
                TestCollisionListener2D.ContactType.Trigger);

            // Assert with timeout for non hanging tests, safer than WaitUntil()
            Assert.That(
                () => !player.health.IsAlive,
                Is.True.After(5000, 100),
                "Expected player to die within 5s of falling into death zone");
        }

        [UnityTest]
        public IEnumerator PlayerInEnemyPath_EnemyCollidesWithPlayer_PlayerDeath()
        {
            testSpawner.SpawnGround(new Vector3(0, -1f));
            var enemy = testSpawner.SpawnEnemy(new Vector3(3, 0));
            var path = testSpawner.CreateEnemyPath(new Vector2(-2, 0), new Vector2(3, 0));
            movementHelper.MoveEnemyAlongPatrol(enemy, path);

            yield return waitHelper.WaitUntilPlayerDeath(player);

            Assert.That(
                () => !player.health.IsAlive,
                Is.True.After(5000, 100),
                "Expected player to die within 5s of enemy collision");
        }
    }
}
