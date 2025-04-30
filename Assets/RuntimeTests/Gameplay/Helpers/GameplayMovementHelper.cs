using System.Collections;
using Platformer.Mechanics;
using UnityEngine;

namespace RuntimeTests.Gameplay.Helpers
{
    public class GameplayMovementHelper
    {
        public IEnumerator MovePlayerToPosition(PlayerController player, Vector3 targetPos, float speed = 5f, float threshold = 0.2f, float timeout = 5f)
        {
            var elapsed = 0f;
            while (Vector3.Distance(player.transform.position, targetPos) > threshold && elapsed < timeout)
            {
                var direction = (targetPos - player.transform.position).normalized;
                player.input = new TestInputProvider(direction.x, false, false);
                elapsed += Time.deltaTime;
                yield return null;
            }

            player.input = new TestInputProvider(0, false, false);
        }

        public void MoveEnemyAlongPatrol(EnemyController enemy, PatrolPath path, float speed = 2f)
        {
            path.CreateMover(speed);
            enemy.path = path;
        }
    }
}