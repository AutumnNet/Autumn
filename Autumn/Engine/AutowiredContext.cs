using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autumn.Annotation;
using Autumn.Tools;

namespace Autumn.Engine
{
    public class AutowiredContext
    {
        public QualifierAttribute Qualifier { get; set; }
        private Dictionary<string, object> Options { get; }
        public object Target { get; set; } 
        public ApplicationContext Ctx { get; set; }

        public object GetInstance(Type type, AutowiredContext ctx)
        {
            return Ctx.GetInstance(type, ctx);
        }
        
        public object GetOrDefault(string target, object def)
        {
            if (target == "context.target")
                return Target;
            Console.WriteLine("Key:{0} {1} {2}",target, target==null, Ctx.ApplicationParameter==null);
            return Options.ContainsKey(target) ? Options[target] : Ctx.ApplicationParameter.GetOrDefault(target, def);
        }

        public AutowiredContext(object o, ApplicationContext ctx, QualifierAttribute qualifier, FieldInfo fieldInfo): this(o, ctx, qualifier)
        {
            fieldInfo.GetOptions().ToList().ForEach(item => Options.Add(item.Name, item.Value));
        }

        
        public AutowiredContext(object o, ApplicationContext ctx, QualifierAttribute qualifier, ConstructorInfo constructorInfo): this(o, ctx, qualifier)
        {
            constructorInfo.GetOptions().ToList().ForEach(item => Options.Add(item.Name, item.Value));
        }

        
        public AutowiredContext(object o, ApplicationContext ctx, QualifierAttribute qualifier, MethodInfo methodInfo): this(o, ctx, qualifier)
        {
            methodInfo.GetOptions().ToList().ForEach(item => Options.Add(item.Name, item.Value));
        }

        public AutowiredContext(object o, ApplicationContext ctx, QualifierAttribute qualifier, PropertyInfo propertyInfo): this(o, ctx, qualifier)
        {
            propertyInfo.GetOptions().ToList().ForEach(item => Options.Add(item.Name, item.Value));
        }

        public AutowiredContext(object o, ApplicationContext ctx, QualifierAttribute qualifier, ParameterInfo parameterInfo) : this(o, ctx, qualifier)
        {
            parameterInfo.GetOptions().ToList().ForEach(item => Options.Add(item.Name, item.Value));
        }

        public AutowiredContext(object o, ApplicationContext ctx, QualifierAttribute qualifier)
        {
            Options = new Dictionary<string, object>();
            Target = o;
            Qualifier = qualifier;
            Ctx = ctx;
        }

    }
}