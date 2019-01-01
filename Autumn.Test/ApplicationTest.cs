

using System;
using System.Reflection;
using Autumn.Annotation;
using Autumn.Engine;
using NUnit.Framework;

namespace Autumn.Test
{
    [TestFixture]
    public class ApplicationTest
    {
        
        [Test]
        public void FirstTest()
        {
            var assembly = Assembly.GetExecutingAssembly();
            Assert.NotNull(assembly);
            var context = new ApplicationContext(assembly);
            var service = context.GetInstance(typeof(TestServiceA)) as TestServiceA;
            Assert.NotNull(service);
            Assert.NotNull(context.GetInstance(typeof(TestServiceB)));
            Assert.NotNull(service.serviceB);
        }
    }


    [Service]
    public class TestServiceA
    {
        [Autowired] public TestServiceB serviceB;

        public TestServiceA()
        {
            Console.WriteLine("ACreate");
        }
    }
    
    
    [Service]
    public class TestServiceB
    {
        public TestServiceA service;
        
        [Autowired]
        public TestServiceB(TestServiceA service)
        {
            Console.WriteLine($"BCreate with service {service}");
            this.service = service;
        }
        
        
        
    }

}