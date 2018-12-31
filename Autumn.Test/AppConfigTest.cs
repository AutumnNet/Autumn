using NUnit.Framework;

namespace Autumn.Test
{
    [TestFixture]
    public class AppConfigTest
    {
        [Test]
        public void LoadAppConfig()
        {
           var val = System.Configuration.ConfigurationManager.AppSettings["test"];
           Assert.AreEqual(val, "7");
        }
    }
}