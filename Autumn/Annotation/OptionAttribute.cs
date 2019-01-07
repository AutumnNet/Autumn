using System;

namespace Autumn.Net.Annotation
{
    [AttributeUsage(AttributeTargets.Method | 
                    AttributeTargets.Property | 
                    AttributeTargets.Field | 
                    AttributeTargets.Constructor,
                    AllowMultiple = true
        )]
    public class OptionAttribute : Attribute, IOption
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public OptionAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}