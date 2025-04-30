using System.Collections;
using Platformer.Mechanics;
using UnityEngine;

namespace RuntimeTests.Gameplay.Helpers
{
    public class GameplayMovementHelper
    {
        private readonly TestInputProvider input;

        public GameplayMovementHelper(TestInputProvider inputProvider)
        {
            input = inputProvider;
        }
        
        public IEnumerator MovePlayerToPosition(PlayerController player, Vector3 targetPos, float speed = 5f, float threshold = 0.2f, float timeout = 5f)
        {
            player.input = input;
            
            var elapsed = 0f;
            while (Vector3.Distance(player.transform.position, targetPos) > threshold)
            {
                if (elapsed >= timeout)
                {
                    Debug.LogError("MovePlayerToPosition: Timed out before reaching target position");
                }
                
                var direction = (targetPos - player.transform.position).normalized;
                input.SetHorizontal(direction.x);
                elapsed += Time.deltaTime;
                yield return null;
            }

            input.ClearInput();
        }

        public IEnumerator MovePlayerForSeconds(PlayerController player, Vector3 direction, float time, float speed = 5)
        {
            player.input = input;
            var elapsed = 0f;
            while (elapsed < time)
            {
                input.SetHorizontal(direction.x);
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            input.ClearInput();
        }

        public void MoveEnemyAlongPatrol(EnemyController enemy, PatrolPath path, float speed = 2f)
        {
            path.CreateMover(speed);
            enemy.path = path;
        }
    }
}