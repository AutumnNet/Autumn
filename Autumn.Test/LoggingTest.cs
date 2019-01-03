using Autumn.Annotation;
using Autumn.Engine;
using Autumn.Logging;
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
    [EnableAssembly(Values = new []{"Autumn.Logging"})]
    public class ServiceCConfiguration
    {
        
    }
    
    [Service]
    public class ServiceC
    {
        [Autowired]
        [Option("logger.subname", "testSubName")]
        public ILog log;

        [PostConstruct]
        public void PostConstruct()
        {
            log.Info("Hello Logging");
        }
    }
}
