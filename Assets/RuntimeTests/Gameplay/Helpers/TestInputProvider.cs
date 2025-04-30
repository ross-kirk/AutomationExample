using Platformer.Core;

namespace RuntimeTests.Gameplay.Helpers
{
    public class TestInputProvider : IPlayerInput
    {
        private float horizontal;
        private bool jumpPressed;
        private bool jumpReleased;
        
        public float Horizontal() => horizontal;

        public bool JumpPressed() => jumpPressed;

        public bool JumpReleased() => jumpReleased;

        public void SetHorizontal(float value) => horizontal = value;

        public void PressJump()
        {
            jumpPressed = true;
            jumpReleased = false;
        }

        public void ReleaseJump()
        {
            jumpPressed = false;
            jumpReleased = true;
        }

        public void ClearInput()
        {
            jumpPressed = false;
            jumpReleased = false;
            horizontal = 0;
        }
    }
}