using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autumn.Annotation;
using Autumn.Tools;

namespace Autumn.Scanner
{
    /// <summary>
    /// Scan Value in Object
    /// </summary>
    public class ValueScanner
    {
        public class ValueField
        {
            public Value Value { get; set; }
            public object Target { get; set; }
            public FieldInfo Field { get; set; }
            
            public void SetValue(object value)
            {
                Field.SetValue(Target, ConvertHelper.To(Field.FieldType,value ?? Value.Default));
            }
        }

        public static IEnumerable<ValueField> Scan(object o)
        {
            return o.GetType()
                .GetFields()
                .Where(field => field.GetCustomAttributes(typeof(Value), false).Length > 0)
                .Select(field => new ValueField {Value = field.GetCustomAttribute<Value>(), Field = field, Target = o});
        } 
    }
}