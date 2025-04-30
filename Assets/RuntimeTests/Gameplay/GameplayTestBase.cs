using NUnit.Framework;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using RuntimeTests.Core;
using RuntimeTests.Gameplay.Helpers;
using Unity.Cinemachine;
using UnityEngine;

namespace RuntimeTests.Gameplay
{
    public abstract class GameplayTestBase : TestBase
    {
        protected GameplayMovementHelper movementHelper;
        protected GameplayTestSpawner testSpawner;
        protected GameplayWaitHelper waitHelper;
        protected TestInputProvider testInput = new ();
        protected GameController gameController;
        
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            gameController = new GameObject("GameController_TEST").AddComponent<GameController>();

            var spawnPoint = new GameObject("Spawn_TEST");
            spawnPoint.transform.position = Vector3.zero;
            
            var model = new PlatformerModel();
            model.spawnPoint = spawnPoint.transform;
            model.virtualCamera = new GameObject("VirtualCam_TEST").AddComponent<CinemachineCamera>();
            Simulation.SetModel(model);
            gameController.model = model;
            
            movementHelper = new GameplayMovementHelper(testInput);
            testSpawner = new GameplayTestSpawner();
            waitHelper = new GameplayWaitHelper();
        }
        
        [TearDown]
        public override void TearDown()
        {
            foreach (var obj in Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (obj.scene.IsValid() && obj.scene.isLoaded)
                {
                    Object.DestroyImmediate(obj);
                }
            }

            base.TearDown();
        }
    }
}