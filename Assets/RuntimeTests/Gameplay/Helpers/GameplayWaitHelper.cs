using System.Collections;
using Platformer.Mechanics;
using UnityEngine;

namespace RuntimeTests.Gameplay.Helpers
{
    public class GameplayWaitHelper
    {
        public IEnumerator WaitForTokenCollected(TokenInstance token, float timeout = 5f)
        {
            var timer = 0f;

            yield return new WaitUntil(() =>
            {
                timer += Time.deltaTime;
                return token.collected || timer > timeout;
            });

            if (!token.collected)
            {
                Debug.LogError($"Timeout: token not collected in {timeout} seconds.");
            }
        }

        public IEnumerator WaitForOverlap(GameObject obj1, GameObject obj2, float threshold = 0.5f, float timeout = 5f)
        {
            var timer = 0f;

            yield return new WaitUntil(() =>
            {
                timer += Time.deltaTime;
                var distance = Vector2.Distance(obj1.transform.position, obj2.transform.position);
                return distance < threshold || timer > timeout;
            });

            if (Vector2.Distance(obj1.transform.position, obj2.transform.position) >= threshold)
            {
                Debug.LogError("Timeout: objects never overlap within threshold.");
            }
        }
    }
}