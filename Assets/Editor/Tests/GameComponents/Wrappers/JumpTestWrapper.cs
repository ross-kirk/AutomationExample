using System.Linq;
using System.Reflection;
using Moq;
using Platformer.Core;
using Platformer.Gameplay;
using Platformer.Mechanics;
using UnityEngine;

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
            
            schedulerMock.Setup(m => m.Schedule<PlayerJumped>(It.IsAny<float>()))
                .Returns(() => new PlayerJumped());
            schedulerMock.Setup(m => m.Schedule<PlayerLanded>(It.IsAny<float>()))
                .Returns(() => new PlayerLanded());
        }
        
        /// <summary>
        /// Reflection search for the backing field for the AutoProperty IsGrounded of the BaseType KinematicObject
        /// Sets the value of the field, allowing control of grounded state from this test wrapper
        /// </summary>
        /// <param name="grounded"></param>
        public void SetGrounded(bool grounded)
        {
            var isGrounded = playerController.GetType().BaseType?
                .GetField("<IsGrounded>k__BackingField",BindingFlags.Instance | BindingFlags.NonPublic);

            if (isGrounded == null)
            {
                Debug.LogError("Reflection call for IsGrounded not found on player controller");
                return;
            }
            
            isGrounded.SetValue(playerController, grounded);
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

        public void PrepareJump()
        {
            SetGrounded(true);
            SimulateJumpPressed();
            TickInput();
        }

        public void StartJump()
        {
            PrepareJump();
            TickJumpState();
        }

        public void ReachInFLight()
        {
            StartJump();
            SetGrounded(false);
            TickJumpState();
        }

        public void Land()
        {
            ReachInFLight();
            SetGrounded(true);
            TickJumpState();
        }

        public void Grounded()
        {
            Land();
            TickJumpState();
        }
    }
}