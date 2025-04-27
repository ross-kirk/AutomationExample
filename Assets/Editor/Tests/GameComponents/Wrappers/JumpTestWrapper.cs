using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Moq;
using Platformer.Core;
using Platformer.Mechanics;

namespace Editor.Tests.GameComponents
{
    public class JumpTestWrapper
    {
        private readonly PlayerController playerController;
        private readonly Mock<IPlayerInput> inputMock = new ();
        private readonly Mock<ISimulationScheduler> schedulerMock = new ();

        public JumpTestWrapper(PlayerController playerController)
        {
            this.playerController = playerController;
            playerController.input = inputMock.Object;
            playerController.scheduler = schedulerMock.Object;
        }

        public void SetGrounded(bool grounded)
        {
            var groundedProp = typeof(KinematicObject).GetProperty("IsGrounded", BindingFlags.Instance | BindingFlags.NonPublic);
            groundedProp?.SetValue(playerController, grounded);
        }

        public void SimulateJumpPressed()
        {
            inputMock.Setup(i => i.JumpPressed()).Returns(true);
            inputMock.Setup(i => i.JumpReleased()).Returns(false);
        }

        public void SimulateJumpReleased()
        {
            inputMock.Setup(i => i.JumpPressed()).Returns(false);
            inputMock.Setup(i => i.JumpReleased()).Returns(true);
        }

        public void TickInput()
        {
            playerController.HandleInput();
        }

        public void TickJumpState()
        {
            playerController.HandleJumpState();
        }

        public bool EventScheduled<T>() where T : Simulation.Event
        {
            return schedulerMock.Invocations.Any(i =>
                i.Method.Name == "Schedule" &&
                i.Method.IsGenericMethod &&
                i.Method.GetGenericArguments()[0] == typeof(T));
        }
    }
}