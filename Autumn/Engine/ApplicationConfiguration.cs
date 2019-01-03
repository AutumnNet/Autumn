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
            
            
            IEnumerable<string> res = assembly
                .GetAutumnConfigurations()
                .Where(config => config.GetCustomAttributes(typeof(EnableAssemblyAttribute), true).Length > 0)
                .SelectMany(config => config.GetCustomAttribute<EnableAssemblyAttribute>().Values);

            res.ToList().ForEach(item => Console.WriteLine("NS:{0}", item));
            
            return res;
        }

        private void GetAssemblies(IEnumerable<string> newAssemblies)
        {
            
            
            allAssemblies
                .Where(item => newAssemblies.Any(str => str == item.GetName().Name))
                .ToList()
                .ForEach(item =>
                {
                    if (assemblies.Contains(item)) return;
                    assemblies.Add(item);
                    GetAssemblies(GetConfigurationAssembly(item));
                });
        }
        
        public IEnumerable<Assembly> ConfiguredAssemblies => assemblies;

        
        public ApplicationConfiguration(Assembly assembly)
        {
            assemblies = new HashSet<Assembly> { assembly };
            allAssemblies = AssemblyHelper.GetAssemblies();
            GetAssemblies(GetConfigurationAssembly(assembly));
        }
    }
}