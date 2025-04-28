using System.Collections;
using Platformer.Mechanics;
using UnityEngine;

namespace RuntimeTests.Gameplay.Helpers
{
    public static class GameplayWaitHelper
    {
        public static IEnumerator WaitForTokenCollected(TokenInstance token, float timeout = 5f)
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
    }
}