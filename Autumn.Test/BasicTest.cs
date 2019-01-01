using System.Reflection;
using Autumn.Annotation;
using Autumn.Annotation.Base;
using NUnit.Framework;

namespace Autumn.Test
{
    [TestFixture]
    public class BasicTest
    {
        [Service]
        public class TestService
        {
            
        }
        
        [Test]
        public void AttributeTest()
        {
            var ts = new TestService();
            Assert.IsTrue( ts.GetType().GetCustomAttributes(typeof(ComponentAttribute), true).Length > 0);
            Assert.IsTrue( ts.GetType().GetCustomAttributes(typeof(ComponentAttribute), false).Length > 0);
            Assert.IsTrue( ts.GetType().GetCustomAttributes(typeof(ServiceAttribute), true).Length > 0);
            Assert.IsTrue( ts.GetType().GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0);
            
            Assert.NotNull(ts.GetType().GetCustomAttribute<ComponentAttribute>());
            Assert.NotNull(ts.GetType().GetCustomAttribute<ServiceAttribute>());
        }
    }
}