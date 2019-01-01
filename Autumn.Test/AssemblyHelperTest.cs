using Autumn.Tools;
using NUnit.Framework;

namespace Autumn.Test
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