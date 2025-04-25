using UnityEngine;

namespace Platformer.Core
{
    public class UnityInputProvider : IPlayerInput
    {
        public float Horizontal() => Input.GetAxis("Horizontal");

        public bool JumpPressed() => Input.GetButtonDown("Jump");

        public bool JumpReleased() => Input.GetButtonUp("Jump");
    }
}