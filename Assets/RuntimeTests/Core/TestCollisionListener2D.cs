using System;
using UnityEngine;

namespace RuntimeTests.Core
{
    public class TestCollisionListener2D : MonoBehaviour
    {
        public event Action<GameObject> OnTriggerEntered;
        public event Action<GameObject> OnCollisionEntered;
        
        public bool Triggered { get; private set; }
        public bool Collided { get; private set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Triggered = true;
            OnTriggerEntered?.Invoke(other.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Collided = true;
            OnCollisionEntered?.Invoke(other.gameObject);
        }

        public enum ContactType
        {
            Trigger,
            Collision,
            Any
        }
    }
}