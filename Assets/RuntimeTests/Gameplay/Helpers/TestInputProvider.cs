using Platformer.Core;

namespace RuntimeTests.Gameplay.Helpers
{
    public class TestInputProvider : IPlayerInput
    {
        private readonly float horizontal;
        private readonly bool jumpPressed;
        private readonly bool jumpReleased;

        public TestInputProvider(float h, bool pressed, bool released)
        {
            horizontal = h;
            jumpPressed = pressed;
            jumpReleased = released;
        }

        public float Horizontal() => horizontal;

        public bool JumpPressed() => jumpPressed;

        public bool JumpReleased() => jumpReleased;
    }
}