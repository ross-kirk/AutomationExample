using System.Collections;
using Platformer.Mechanics;
using UnityEngine;

namespace RuntimeTests.Gameplay.Helpers
{
    public static class GameplayMovementHelper
    {
        public static IEnumerator MovePlayerToPosition(PlayerController player, Vector3 targetPos, float speed = 5f)
        {
            while (Vector3.Distance(player.transform.position, targetPos) > 0.05f)
            {
                var direction = (targetPos - player.transform.position).normalized;
                player.input = new TestInputProvider(direction.x, false, false);
                player.HandleInput();
                yield return null;
            }
        }

        public static IEnumerator MoveEnemyAlongPatrol(EnemyController enemy, PatrolPath path, float speed = 2f, float threshold = 0.1f)
        {
            var mover = path.CreateMover(speed);
            while (Vector3.Distance(enemy.transform.position, path.endPosition) > threshold)
            {
                enemy.transform.position = mover.Position;
                yield return null;
            }
        }
    }
}