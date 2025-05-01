using System.Collections;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using RuntimeTests.Core;
using RuntimeTests.Gameplay.Helpers;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.TestTools;

namespace RuntimeTests.Gameplay
{
    public abstract class GameplayTestBase : TestBase
    {
        protected GameplayMovementHelper movementHelper;
        protected GameplayTestSpawner testSpawner;
        protected GameplayWaitHelper waitHelper;
        protected TestInputProvider testInput = new ();
        protected GameController gameController;

        protected Scene testScene;
        
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
            
            var spawnPoint = new GameObject("Spawn_TEST");
            spawnPoint.transform.position = Vector3.zero;

            var model = new PlatformerModel
            {
                spawnPoint = spawnPoint.transform,
                virtualCamera = new GameObject("VirtualCam_TEST").AddComponent<CinemachineCamera>()
            };
            
            Simulation.SetModel(model);
            gameController = new GameObject("GameController_TEST").AddComponent<GameController>();
            gameController.model = model;
            
            movementHelper = new GameplayMovementHelper(testInput);
            testSpawner = new GameplayTestSpawner();
            waitHelper = new GameplayWaitHelper();
        }
        
        [UnityTearDown]
        public override IEnumerator TearDown()
        {
            Simulation.ClearPools();

            foreach (var obj in Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (obj.scene.IsValid() && obj.scene.isLoaded)
                {
                    Object.DestroyImmediate(obj);
                }
            }

            yield return base.TearDown();
        }
    }
}