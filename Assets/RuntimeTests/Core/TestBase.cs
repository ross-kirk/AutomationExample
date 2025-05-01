using System.Collections;
using UnityEngine.TestTools;

namespace RuntimeTests.Core
{
    public abstract class TestBase
    {
        [UnitySetUp]
        public virtual IEnumerator SetUp()
        {
            yield return null;
        }

        [UnityTearDown]
        public virtual IEnumerator TearDown()
        {
            yield return null;
        }
        
    }
}