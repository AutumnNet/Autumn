using Autumn.Annotation;
using Autumn.Interfaces;
using NUnit.Framework;

namespace Autumn.Test
{
    [TestFixture]
    public class LoggingTest
    {
        [Test]
        public void Test()
        {
            var context = new Application(typeof(LoggingTest)).Start().Context;
            var service = context.GetInstance(typeof(ServiceC)) as ServiceC;
            Assert.NotNull(service);
            Assert.NotNull(service.log);
        }
    }

    [Configuration]
    [EnableAssembly("Autumn")]
    public class ServiceCConfiguration
    {
        
    }
    
    [Service]
    public class ServiceC
    {
        [Autowired]
        [Option("logger.name", "Test Logger")]
        public ILog log;

        [PostConstruct]
        public void PostConstruct()
        {
            log.Info("Hello Logging");
        }
    }
}
