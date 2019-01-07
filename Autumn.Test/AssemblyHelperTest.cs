using Autumn.Net.Tools;
using Autumn.Net.Tools;
using NUnit.Framework;

namespace Autumn.Net.Net.Test
{
    [TestFixture]
    public class AssemblyHelperTest
    {
        [Test]
        public void TestLoadingAssemblies()
        {
            AssemblyHelper.GetAssemblies();
        }
    }
}