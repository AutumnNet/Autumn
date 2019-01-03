using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Autumn.Annotation;
using Autumn.Tools;

namespace Autumn.Engine
{
    /// <summary>
    /// Scan and create Assembly for Application
    /// </summary>
    public class ApplicationConfiguration
    {
        private HashSet<Assembly> assemblies;
        private IEnumerable<Assembly> allAssemblies;

        private IEnumerable<string> GetConfigurationAssembly(Assembly assembly)
        {
            Console.WriteLine("Configuring Assembly {0}", assembly.GetName().Name);
            assembly
                .GetAutumnConfigurations()
                .ToList()
                .ForEach(item =>
                {
                    Console.WriteLine(" `-- {0}", item.FullName);
                    if (item.GetCustomAttribute<EnableAssemblyAttribute>() != null)
                        item.GetCustomAttribute<EnableAssemblyAttribute>().Values
                            .ToList()
                            .ForEach(val =>
                            {
                                Console.WriteLine("    `-- {0}", val);
                            });
                });
            
            IEnumerable<string> res = assembly
                .GetAutumnConfigurations()
                .Where(config => config.GetCustomAttributes(typeof(EnableAssemblyAttribute), true).Length > 0)
                .SelectMany(config => config.GetCustomAttribute<EnableAssemblyAttribute>().Values);

            res.ToList().ForEach(item => Console.WriteLine("NS:{0}", item));
            
            return res;
        }

        private void GetAssemblies(IEnumerable<string> newAssemblies)
        {
            newAssemblies.ToList().ForEach(item =>
            {
                Console.WriteLine("NewAssemblies:{0}", item);
                assemblies.Add(Assembly.Load(item));
            });
            
            allAssemblies
                .Select(item =>
                {
                    Console.WriteLine("Get Assemblies: {0} {1}", item.GetName().Name, newAssemblies.Any(str => str == item.GetName().Name));
                    return item;
                })
                .Where(item => newAssemblies.Any(str => str == item.GetName().Name))
                .ToList()
                .ForEach(item =>
                {
                    if (assemblies.Contains(item)) return;
                    assemblies.Add(item);
                    GetAssemblies(GetConfigurationAssembly(item));
                });
            
        }
        
        public IEnumerable<Assembly> ConfiguredAssemblies
        {
            get
            {
                Console.WriteLine("Configured Assemblies Count:{0}", assemblies.Count);
                assemblies.ToList().ForEach(item =>
                    {
                        Console.WriteLine("  `-- Configured Assemblies: {0}", item.GetName().Name);
                    }); 
                return assemblies;
            }
        }


        public ApplicationConfiguration(Assembly assembly)
        {
            Console.WriteLine("ApplicationConfiguration {0}", assembly.GetName().Name);
            assemblies = new HashSet<Assembly> { assembly };
            allAssemblies = AssemblyHelper.GetAssemblies();
            GetAssemblies(GetConfigurationAssembly(assembly));
        }
    }
}