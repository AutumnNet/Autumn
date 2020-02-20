using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autumn.Net.Annotation;
using Autumn.Net.Tools;

namespace Autumn.Net.Engine
{
    public class AutowiredContext
    {
        public QualifierAttribute Qualifier { get; }
        private Dictionary<string, object> Options { get; }
        public object Target { get; } 
        public ApplicationContext Ctx { get; }
        
        public AutowiredAttribute Autowired { get; }

        public bool AutowiredRequired => Autowired == null || Autowired.Required;
        
        public object GetInstance(Type type, AutowiredContext ctx)
        {
            return Ctx.GetInstance(type, ctx);
        }
        
        public object GetOrDefault(string target, object def)
        {
            if (target == "context.target")
                return Target;
            //Console.WriteLine("Key:{0} {1} {2}",target, target==null, Ctx.ApplicationParameter==null);
            return Options.ContainsKey(target) ? Options[target] : Ctx.ApplicationParameter.GetOrDefault(target, def);
        }

        public AutowiredContext(object o, ApplicationContext ctx, QualifierAttribute qualifier, FieldInfo fieldInfo, AutowiredAttribute autowired): this(o, ctx, qualifier, autowired)
        {
            fieldInfo.GetOptions().ToList().ForEach(item => Options.Add(item.Name, item.Value));
        }

        
        public AutowiredContext(object o, ApplicationContext ctx, QualifierAttribute qualifier, ConstructorInfo constructorInfo, AutowiredAttribute autowired): this(o, ctx, qualifier, autowired)
        {
            constructorInfo.GetOptions().ToList().ForEach(item => Options.Add(item.Name, item.Value));
        }

        
        public AutowiredContext(object o, ApplicationContext ctx, QualifierAttribute qualifier, MethodInfo methodInfo, AutowiredAttribute autowired): this(o, ctx, qualifier, autowired)
        {
            methodInfo.GetOptions().ToList().ForEach(item => Options.Add(item.Name, item.Value));
        }

        public AutowiredContext(object o, ApplicationContext ctx, QualifierAttribute qualifier, PropertyInfo propertyInfo, AutowiredAttribute autowired): this(o, ctx, qualifier, autowired)
        {
            propertyInfo.GetOptions().ToList().ForEach(item => Options.Add(item.Name, item.Value));
        }

        public AutowiredContext(object o, ApplicationContext ctx, QualifierAttribute qualifier, ParameterInfo parameterInfo, AutowiredAttribute autowired) : this(o, ctx, qualifier, autowired)
        {
            parameterInfo.GetOptions().ToList().ForEach(item => Options.Add(item.Name, item.Value));
        }

        public AutowiredContext(object o, ApplicationContext ctx, QualifierAttribute qualifier, AutowiredAttribute autowired)
        {
            Options = new Dictionary<string, object>();
            Target = o;
            Qualifier = qualifier;
            Ctx = ctx;
            Autowired = autowired;
        }


        public static AutowiredContext Empty()
        {
            return new AutowiredContext(null, null, null, null);
        }
        
        public static AutowiredContext Base(ApplicationContext ctx)
        {
            return new AutowiredContext(null, ctx, null, null);
        }

    }
}