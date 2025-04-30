using NUnit.Framework;
using RuntimeTests.Core;
using RuntimeTests.Gameplay.Helpers;
using UnityEngine;

namespace RuntimeTests.Gameplay
{
    public abstract class GameplayTestBase : TestBase
    {
        protected GameplayMovementHelper movementHelper;
        protected GameplayTestSpawner testSpawner;
        protected GameplayWaitHelper waitHelper;
        
        [SetUp]
        public override void SetUp()
        {
            movementHelper = new GameplayMovementHelper();
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
        }
    }
}