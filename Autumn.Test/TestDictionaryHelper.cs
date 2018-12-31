using System.Collections.Generic;
using NUnit.Framework;

namespace Autumn.Test
{
    [TestFixture]
    public class TestDictionaryHelper
    {
        [Test]
        public void TestFlatString()
        {
            var d1 = Tools.DictionaryHelper.FlatString(new Dictionary<string, object>
            {
                {"a", new Dictionary<string, object> {
                    {"b", new Dictionary<string, object> {{"c", 5}, {"d", 7}}},
                    { "e", 12}
                    }
                },
                {"l", 15}
            });
            
            Assert.NotNull(d1);
            Assert.IsTrue(d1.ContainsKey("l"));
            Assert.AreEqual(d1["l"], 15);
            Assert.IsTrue(d1.ContainsKey("a.e"));
            Assert.AreEqual(d1["a.e"], 12);
            Assert.AreEqual(d1.Keys.Count, 4);
            Assert.IsTrue(d1.ContainsKey("a.b.c"));
            Assert.AreEqual(d1["a.b.c"], 5);
            Assert.IsTrue(d1.ContainsKey("a.b.d"));
            Assert.AreEqual(d1["a.b.d"], 7);
        }
    }
}