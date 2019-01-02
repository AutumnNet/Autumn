using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autumn.Annotation;
using Autumn.Annotation.Base;
using Autumn.Engine;

namespace Autumn.Tools
{
    public static class AssemblyHelper
    {
        /// <summary>
        /// Return Components types
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <param name="includeConfiguration">Include Configuration (false)</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAutumnComponents(this Assembly assembly, bool includeConfiguration = false)
        {
            return assembly.GetTypes()
                .Where(type => 
                    type.GetCustomAttributes(typeof(ComponentAttribute), false).Length > 0 &&
                    (includeConfiguration || type.GetCustomAttributes(typeof(ConfigurationAttribute), false).Length == 0)
                );
        }
      
        /// <summary>
        /// Return Constructor for Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ConstructorInfo GetAutumnConstructor(this Type type)
        {
            ConstructorInfo resultConstructor = null;
            foreach (var constructor in type.GetConstructors())
            {
                if (constructor.GetCustomAttributes(typeof(AutowiredAttribute), false).Length > 0)
                    return constructor;
                if (constructor.GetParameters().Length == 0)
                    resultConstructor = constructor;
            }
            return resultConstructor;
        }
        
        /// <summary>
        /// Return Configurations
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAutumnConfigurations(this Assembly assembly)
        {
            return assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(ConfigurationAttribute), false).Length > 0);
        }

        /// <summary>
        /// Get Beans
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetAutumnBeans(this Type type)
        {
            return type.GetMethods().Where(method => method.GetCustomAttributes(typeof(BeanAttribute), true).Length > 0);
        }

        /// <summary>
        /// Get Inheritance Types
        /// </summary>
        /// <param name="type">Base Type</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetInheritanceTypes(this Type type)
        {
            var types = new List<Type> {type};
            types.AddRange(type.GetInterfaces());
            var cType = type.BaseType;
            while (cType != null)
            {
                types.Add(cType);
                types.AddRange(cType.GetInterfaces());
                cType = cType.BaseType;
            }
            return types;
        }

        public static bool IsAutumnConfiguration(this Type type)
        {
            return type.GetCustomAttributes(typeof(ConfigurationAttribute), false).Length > 0;
        }
        
        /// <summary>
        /// Default Assembly
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAssembly()
        {
            return new HashSet<Assembly>{Assembly.GetExecutingAssembly()}; 
        }

        public static IEnumerable<Assembly> GetAssemblies()
        {
            var names = new List<AssemblyName>
            {
                Assembly.GetCallingAssembly().GetName(), 
                Assembly.GetExecutingAssembly().GetName()
            };
            names.AddRange(Assembly.GetCallingAssembly().GetReferencedAssemblies());
            names.AddRange(Assembly.GetExecutingAssembly().GetReferencedAssemblies());
            return names.Select(Assembly.Load);
        }

        
        public static object[] GetAutumnConstructorArguments(this ConstructorInfo info, ApplicationContext ctx)
        {
            if (info == null)
                return null;

            var arguments =  new object[info.GetParameters().Length];
            for (var i = 0; i < arguments.Length; i++)
            {
                
                var pi = info.GetParameters()[i];
                //Value
                if (pi.GetCustomAttributes(typeof(ValueAttribute), false).Length > 0)
                {
                    var value = pi.GetCustomAttribute<ValueAttribute>();
                    arguments[i] = ConvertHelper.To(
                        pi.ParameterType, 
                        ctx.ApplicationParameter.GetOrDefault(value.Target, value.Default)
                    );
                }
                else //Autowired
                    arguments[i] = ctx.GetInstance(pi.ParameterType);
            }
            return arguments;
        }

        public static object[] GetAutumnMethodArguments(this MethodInfo info, ApplicationContext ctx)
        {
            if (info == null)
                return null;
            var arguments =  new object[info.GetParameters().Length];
            for (var i = 0; i < arguments.Length; i++)
            {
                
                var pi = info.GetParameters()[i];
                //Value
                if (pi.GetCustomAttributes(typeof(ValueAttribute), false).Length > 0)
                {
                    var value = pi.GetCustomAttribute<ValueAttribute>();
                    arguments[i] = ConvertHelper.To(
                        pi.ParameterType, 
                        ctx.ApplicationParameter.GetOrDefault(value.Target, value.Default)
                    );
                }
                else //Autowired
                    arguments[i] = ctx.GetInstance(pi.ParameterType);
            }
            return arguments;
        }

       

    }
}