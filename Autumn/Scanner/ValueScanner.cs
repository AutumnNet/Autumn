using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autumn.Annotation;
using Autumn.Engine;
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
            
            public void SetValue(ApplicationContext ctx)
            {
                SetValue(ctx.ApplicationParameter.GetOrNull(Value.Target));
            }

        }
        
        public class ValueProperty
        {
            public Value Value { get; set; }
            public object Target { get; set; }
            public PropertyInfo Property { get; set; }
            
            public void SetValue(object value)
            {
                Property.SetValue(Target, ConvertHelper.To(Property.PropertyType,value ?? Value.Default));
            }

            public void SetValue(ApplicationContext ctx)
            {
                SetValue(ctx.ApplicationParameter.GetOrNull(Value.Target));
            }
        }
        
        public class ValueMethod
        {
            public class ValueMethodParameter
            {
                public Value Value { get; set; }
                public ParameterInfo Parameter { get; set; }
                public object GetValue(ApplicationContext ctx)
                {
                    return GetValue(ctx.ApplicationParameter.GetOrNull(Value.Target));
                }
                
                public object GetValue(object value)
                {
                    return ConvertHelper.To(Parameter.ParameterType,value ?? value ?? Value.Default ?? Parameter.DefaultValue);
                }
            }
            
            public IEnumerable<ValueMethodParameter> ValueMethodParameters { get; set; }
            
            public MethodInfo Method { get; set; }
            
            public object Target { get; set; }
            
            public object[] GetValues(ApplicationContext ctx)
            {
                return ValueMethodParameters.Select(item => item.GetValue(ctx)).ToArray();
            }

            public object Invoke(ApplicationContext ctx)
            {
                return Method.Invoke(Target, GetValues(ctx));
            }
            
            public ValueMethod(object target, MethodInfo mi)
            {
                Target = target;
                Method = mi;
                var valueMethodParameters = new ValueMethodParameter[Method.GetParameters().Length];
                for (var i = 0; i < valueMethodParameters.Length; i++)
                {
                    valueMethodParameters[i] = new ValueMethodParameter();
                    var pi = Method.GetParameters()[i];
                    if (pi.GetCustomAttributes(typeof(Value), false).Length > 0)
                        valueMethodParameters[i].Value = pi.GetCustomAttribute<Value>();
                    valueMethodParameters[i].Parameter = pi;
                }
                ValueMethodParameters = valueMethodParameters;
            }
        }

        public class ValueConstructor
        {
            public IEnumerable<ValueMethod.ValueMethodParameter> ValueMethodParameters { get; set; }
            
            public ConstructorInfo Constructor { get; set; }

            public object[] GetValues(ApplicationContext ctx)
            {
                return ValueMethodParameters.Select(item => item.GetValue(ctx)).ToArray();
            }

            public object Instantiate(ApplicationContext ctx)
            {
                return Constructor.Invoke(GetValues(ctx));
            }
            
            public ValueConstructor(ConstructorInfo ci)
            {
                Constructor = ci;
                var valueMethodParameters = new ValueMethod.ValueMethodParameter[Constructor.GetParameters().Length];
                for (var i = 0; i < valueMethodParameters.Length; i++)
                {
                    valueMethodParameters[i] = new ValueMethod.ValueMethodParameter();
                    var pi = Constructor.GetParameters()[i];
                    if (pi.GetCustomAttributes(typeof(Value), false).Length > 0)
                        valueMethodParameters[i].Value = pi.GetCustomAttribute<Value>();
                    valueMethodParameters[i].Parameter = pi;
                }
                ValueMethodParameters = valueMethodParameters;
            }
        }
        
        public static IEnumerable<ValueField> ScanField(object o)
        {
            return o.GetType()
                .GetFields()
                .Where(value => value.GetCustomAttributes(typeof(Value), false).Length > 0)
                .Select(value => new ValueField {Value = value.GetCustomAttribute<Value>(), Field = value, Target = o});
        }
        
        public static IEnumerable<ValueProperty> ScanProperty(object o)
        {
            return o.GetType()
                .GetProperties()
                .Where(value => value.GetCustomAttributes(typeof(Value), false).Length > 0)
                .Select(value => new ValueProperty {Value = value.GetCustomAttribute<Value>(), Property = value, Target = o});
        }

        public static ValueMethod ScanMethod(object o, MethodInfo mi)
        {
            return new ValueMethod(o, mi);
        }


        public static ValueConstructor ScanConstructor(ConstructorInfo ci)
        {
            return new ValueConstructor(ci);
        }
    }
}