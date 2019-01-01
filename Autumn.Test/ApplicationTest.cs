

using System;
using System.Reflection;
using Autumn.Annotation;
using Autumn.Engine;
using Autumn.Tools;
using NUnit.Framework;

namespace Autumn.Test
{
    [TestFixture]
    public class ApplicationTest
    {
        
        [Test]
        public void FirstTest()
        {
            var assemblys = AssemblyHelper.GetAssemblies("Autumn", "Autumn.Test");
            Assert.NotNull(assemblys);
            var context = new ApplicationContext(assemblys);
            var serviceA = context.GetInstance(typeof(TestServiceA)) as TestServiceA;
            Assert.NotNull(serviceA);
            Assert.NotNull(context.GetInstance(typeof(TestServiceB)));
            Assert.NotNull(serviceA.serviceB);
            Assert.NotNull(serviceA.serviceB.service);
            Assert.AreEqual(serviceA, serviceA.serviceB.service);
            Assert.AreEqual(context.GetInstance(typeof(TestServiceB)), serviceA.serviceB);
            Assert.AreEqual(context.GetInstance(typeof(TestServiceA)), serviceA);
        }
    }


    [Service]
    public class TestServiceA
    {
        public TestServiceB serviceB;

        [Autowired]
        public TestServiceA(TestServiceB service)
        {
            this.serviceB = service;
            Console.WriteLine($"A-Create with service {service}");
            
        }
    }
    
    
    [Service]
    public class TestServiceB
    {
        [Autowired]
        public TestServiceA service;

        [Autowired] private Demo demo;
        
        public TestServiceB()
        {
            Console.WriteLine($"B-Create with service [{service}]");
        }
        
        
        
    }

}