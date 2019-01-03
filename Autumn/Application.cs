using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Autumn.Engine;
using Autumn.Object;

namespace Autumn
{
    /// <summary>
    /// Autumn application
    /// </summary>
    public class Application : IDisposable
    {
        public ApplicationContext Context { get; private set; }
        private readonly Assembly startAssembly;

        public Application(Type mainType)
        {
           startAssembly = mainType.Assembly;
        }
        
        public Application Start() => Start(new string[0]);

        public Application Start(IEnumerable<string> args) => Start(new CommandLineApplicationParameter(args));

        public Application Start(ApplicationParameter param) => Start(new ApplicationContext(param, startAssembly));
        
        public Application Start(ApplicationContext context)
        {
            Context = context;
            return this;
        }

        public void Dispose()
        {
            Context.PreDestroy();
            Context = null;
        }

        ~Application()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }
    }
}