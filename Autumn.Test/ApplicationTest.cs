

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
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
//            var assemblies = AssemblyHelper.GetAssemblies();
//            Assert.NotNull(assemblies);
            var context = new Application(typeof(ApplicationTest)).Start().Context;
            var serviceA = context.GetInstance(typeof(TestServiceA)) as TestServiceA;
            Assert.NotNull(serviceA);
            Assert.NotNull(context.GetInstance(typeof(TestServiceB)));
            Assert.NotNull(serviceA.serviceB);
            Assert.NotNull(serviceA.serviceB.service);
            Assert.AreEqual(serviceA, serviceA.serviceB.service);
            Assert.AreEqual(context.GetInstance(typeof(TestServiceB)), serviceA.serviceB);
            Assert.AreEqual(context.GetInstance(typeof(TestServiceA)), serviceA);
            Assert.NotNull(serviceA.serviceB.node);
            Assert.NotNull(serviceA.doc);
            Assert.NotNull(serviceA.documents);
            Assert.AreEqual(serviceA.documents.Count, 2);
            Assert.NotNull(serviceA.documentsSet);
            Assert.AreEqual(serviceA.documentsSet.Count, 2);
            Assert.NotNull(serviceA.documentsIe);
            Assert.AreEqual(serviceA.documentsIe.Count(), 2);

            
        }
    }

    [Configuration]
    [EnableAssembly("Autumn")]
    public class ServiceConfiguration
    {
        [Bean(Name = "NodeA")]
        [Primary]
        public XmlDocument getNodeA()
        {
            Console.WriteLine("Create BeanA");
            return new XmlDocument();
        }

        
        [Bean(Name = "NodeB")]
        public XmlDocument getNodeB()
        {
            Console.WriteLine("Create BeanB");
            return new XmlDocument();
        }

    }

    [Service]
    public class TestServiceA
    {
        public TestServiceB serviceB;

        public XmlDocument doc;
        
        [Autowired]
        public List<XmlDocument> documents;

        [Autowired]
        public HashSet<XmlDocument> documentsSet;

        [Autowired]
        public IEnumerable<XmlDocument> documentsIe;
        
        
        [Autowired]
        public TestServiceA(TestServiceB service, [Qualifier("NodeB")] XmlDocument doc)
        {
            this.doc = doc;
            this.serviceB = service;
            Console.WriteLine($"A-Create with service {service}");
            
        }
    }
    
    
    [Service]
    public class TestServiceB
    {
        [Autowired]
        public TestServiceA service;

        [Autowired]
        [Qualifier("NodeA", "NodeC")]
        public XmlDocument node;

        [Autowired] private Demo demo;
        
        public TestServiceB()
        {
            Console.WriteLine($"B-Create with service [{service}]");
        }
        
    }

}