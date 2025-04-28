using NUnit.Framework;
using UnityEngine;

namespace RuntimeTests.Gameplay
{
    public abstract class GameplayTestBase
    {
        [SetUp]
        public virtual void SetUp()
        {
        }
        
        [TearDown]
        public virtual void TearDown()
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