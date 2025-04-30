using System.Collections;
using Platformer.Mechanics;
using RuntimeTests.Core;
using UnityEngine;
using static RuntimeTests.Core.TestCollisionListener2D;

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

        public IEnumerator WaitForContactWith(GameObject current, GameObject target, ContactType contactType, float timeout = 5f)
        {
            if (current == null || target == null)
            {
                Debug.LogError("WaitForContactWith: Current or Target objects are null");
                yield break;
            }

            var triggered = false;
            var collided = false;

            var listener = current.AddComponent<TestCollisionListener2D>();

            listener.OnTriggerEntered += other =>
            {
                if (other == target) triggered = true;
            };
            listener.OnCollisionEntered += other =>
            {
                if (other == target) collided = true;
            };

            var elapsed = 0f;
            try
            {
                yield return new WaitUntil(() =>
                {
                    elapsed += Time.deltaTime;
                    return contactType switch
                    {
                        ContactType.Trigger => triggered,
                        ContactType.Collision => collided,
                        ContactType.Any => triggered || collided,
                        _ => false
                    } || elapsed > timeout;
                });
            }
            finally
            {
                Object.Destroy(listener);
            }

            if ((contactType == ContactType.Trigger && !triggered) ||
                (contactType == ContactType.Collision && !triggered) ||
                (contactType == ContactType.Any && !(triggered || collided)))
            {
                Debug.LogError($"WaitForContactWith: No trigger entered with {target.name} after {timeout} seconds");
            }
        }

        public IEnumerator WaitForAnyContact(GameObject gameObject, ContactType contactType, float timeout = 5f)
        {
            if (gameObject == null)
            {
                Debug.LogError("WaitForAnyContact: Current object is null");
                yield break;
            }

            var triggered = false;
            var collided = false;

            var listener = gameObject.AddComponent<TestCollisionListener2D>();
            listener.OnTriggerEntered += _ => triggered = true;
            listener.OnCollisionEntered += _ => collided = true;

            var elapsed = 0f;
            try
            {
                yield return new WaitUntil(() =>
                {
                    elapsed += Time.deltaTime;
                    return contactType switch
                    {
                        ContactType.Trigger => triggered,
                        ContactType.Collision => collided,
                        ContactType.Any => triggered || collided,
                        _ => false
                    } || elapsed > timeout;
                });
            }
            finally
            {
                Object.Destroy(listener);
            }

            if ((contactType == ContactType.Trigger && !triggered) ||
                (contactType == ContactType.Collision && !triggered) ||
                (contactType == ContactType.Any && !(triggered || collided)))
            {
                Debug.LogError($"WaitForContactWith: No trigger entered with {gameObject.name} after {timeout} seconds");
            }
        }
    }
}