using System;
using Autumn.Annotation;
using Autumn.Scanner;
using NUnit.Framework;

namespace Autumn.Test
{
    [TestFixture]
    public class TestComponentStorage
    {

        /// <summary>
        /// Component Without Constructor
        /// </summary>
        [Service(Singleton = true)]
        public class TestComponentA 
        {
        }

        /// <summary>
        /// Component With empty Constructor
        /// </summary>
        [Service]
        public class TestComponentB 
        {
            public TestComponentB() {}
            public TestComponentB(int testVariable) {}
        }

        /// <summary>
        /// Component With non-empty Constructor
        /// </summary>
        [Service]
        public class TestComponentC 
        {
            public TestComponentC(int testVariable) {}

            [Autowired]
            public TestComponentC(int testVariableA, int testVariableB)
            {
                
            }
        }

        
        [Test]
        public void TestParamsConstructor()
        {
            var container = new ComponentContainer(typeof(TestComponentA));
            Assert.NotNull(container.Constructor);
            Assert.AreEqual(container.ConstructorParameterInfo.Length, 0);

            container = new ComponentContainer(typeof(TestComponentB));
            Assert.NotNull(container.Constructor);
            Assert.AreEqual(container.ConstructorParameterInfo.Length, 0);

            container = new ComponentContainer(typeof(TestComponentC));
            Assert.NotNull(container.Constructor);
            Assert.AreEqual(container.ConstructorParameterInfo.Length, 2);

        }

        [Test]
        public void TestInstantiate()
        {
            var storage = new ComponentStorage();
            
            var container = new ComponentContainer(typeof(TestComponentA));
            Assert.NotNull(container.Constructor);
            Assert.NotNull(container.Instantiate(storage));

            container = new ComponentContainer(typeof(TestComponentB));
            Assert.NotNull(container.Constructor);
            Assert.NotNull(container.Instantiate(storage));
            
 
        }
    }
}