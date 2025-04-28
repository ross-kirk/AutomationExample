using NUnit.Framework;
using Platformer.Gameplay;
using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.TestTools;

namespace Editor.Tests.GameComponents
{
    public class PlayerJumpTests
    {
        private PlayerController playerController;
        private JumpTestWrapper tester;
        
        [SetUp]
        public void SetUp()
        {
            var gameObject = new GameObject("Player");
            playerController = gameObject.AddComponent<PlayerController>();

            // add components to avoid null refs during test
            gameObject.AddComponent<BoxCollider2D>();
            gameObject.AddComponent<AudioSource>();
            gameObject.AddComponent<Health>();
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.AddComponent<Animator>();

            playerController.Init();
            tester = new JumpTestWrapper(playerController);
        }
        
        [TearDown]
        public void TearDown()
        {
            playerController = null;
            tester = null;
        }
        
        /// <summary>
        /// Use helper method to run until PrepareToJump
        /// Asserts if expected prep state is still handled correctly
        /// </summary>
        [Test]
        public void PlayerGrounded_PressJump_PreparesToJump()
        {
            tester.PrepareJump();
            
            Assert.AreEqual(PlayerController.JumpState.PrepareToJump, playerController.jumpState);
        }
        
        /// <summary>
        /// Use helper to prepare a jump, then tick over once on the jump state
        /// Assert jumping state is set as expected
        /// </summary>
        [Test]
        public void PlayerPreparesToJump_PressJump_IsJumping()
        {
            tester.PrepareJump();
            
            tester.TickJumpState();
            
            Assert.AreEqual(PlayerController.JumpState.Jumping, playerController.jumpState);
        }
        
        /// <summary>
        /// Use helper to start jumping
        /// Manually set grounded as false to simulate being in air then tick over jump state
        /// Assert the state & jumped event scheduled are linked as expected
        /// </summary>
        [Test]
        public void PlayerJumps_LeavesGround_IsInFlightAndJumped()
        {
            tester.StartJump();

            tester.SetGrounded(false);
            tester.TickJumpState();
            
            Assert.AreEqual(PlayerController.JumpState.InFlight, playerController.jumpState);
            Assert.IsTrue(tester.EventScheduled<PlayerJumped>());
        }
        
        /// <summary>
        /// Use helper to fully reach the air
        /// Set grounded true again to simulate being on ground
        /// Assert the state & landed event scheduled are linked as expected
        /// </summary>
        [Test]
        public void PlayerInFlight_HitsGround_Landed()
        {
            tester.ReachInFLight();

            tester.SetGrounded(true);
            tester.TickJumpState();

            Assert.AreEqual(PlayerController.JumpState.Landed, playerController.jumpState);
            Assert.IsTrue(tester.EventScheduled<PlayerLanded>());
        }

        /// <summary>
        /// Fully go through a jump start to finish
        /// Tick over a final jump state
        /// Assert the jump state reverts to grounded correctly
        /// </summary>
        [Test]
        public void PlayerLanded_TransitionsToGrounded()
        {
            tester.Land();
            
            tester.TickJumpState();
            
            Assert.AreEqual(PlayerController.JumpState.Grounded, playerController.jumpState);
        }
    }
}
