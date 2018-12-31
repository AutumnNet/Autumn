using System.Collections.Generic;
using Autumn.Annotation;
using Autumn.Scanner;
using NUnit.Framework;

namespace Autumn.Test
{
    [TestFixture]
    public class TestValueAttribute
    {
        public class TestClass
        {
            [Value("{alpha1.betta2.charlie:10}")]
            public int intField;
            
            [Value("{alpha1.betta2.charlie:10}")]
            public string stringField;
            
            [Value("{alpha3.betta1.charlie:12}")]
            public int IntProperty { get; set; }
            
            [Value("{alpha3.betta1.charlie:12}")]
            public string StringProperty { get; set; }
        }
        

        [Test]
        public void TestValueRegexp()
        {
            var tc = new TestClass();
            var fields = new List<ValueScanner.ValueField>(ValueScanner.ScanField(tc));
            Assert.NotNull(fields);
            Assert.AreEqual(fields.Count, 2);
            var v = fields[0];
            Assert.NotNull(v);
            Assert.NotNull(v.Value);
            Assert.AreEqual(v.Value.Target, "alpha1.betta2.charlie");
            Assert.AreEqual(v.Value.Default, "10");
            Assert.AreEqual(v.Target, tc);
        }

        [Test]
        public void TestValueField()
        {
            var tc = new TestClass();
            var fields = new List<ValueScanner.ValueField>(ValueScanner.ScanField(tc));
            Assert.NotNull(fields);
            Assert.AreEqual(fields.Count, 2);
            fields.ForEach(f => f.SetValue(null));
            Assert.AreEqual(tc.intField, 10);
            Assert.AreEqual(tc.stringField, "10");
            
            fields.ForEach(f => f.SetValue(25));
            Assert.AreEqual(tc.intField, 25);
            Assert.AreEqual(tc.stringField, "25");

        }
    }
}