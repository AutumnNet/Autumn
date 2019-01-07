using System;
using System.Linq;
using System.Reflection;
using Autumn.Net.Annotation;
using Autumn.Net.Engine;
using Autumn.Net.Interfaces;
using Autumn.Net.Object;
using Autumn.Net.Tools;

namespace Autumn.Net
{
    /// <summary>
    /// Autumn application
    /// </summary>
    public class Application : IDisposable
    {
        public ApplicationContext Context { get; private set; }
        private readonly Assembly startAssembly;
        private readonly Type type;
        private string[] commandLineArgs = new string[0];
        private IDisposable disposableRunner = null;
            
        public Application(Type mainType)
        {
            type = mainType;
            startAssembly = mainType.Assembly;
        }
        
        public Application Start() => Start(new string[0]);

        public Application Start(string[] args)
        {
            commandLineArgs = args;
            return Start(new CommandLineApplicationParameter(args));
        }

        public Application Start(ApplicationParameter param) => Start(new ApplicationContext(param, startAssembly));
        
        public Application Start(ApplicationContext context)
        {
            Context = context;
            // Start Runners
            if (type.GetInterfaces().Contains(typeof(ICommandLineRunner)))
            {
                var constructor = type.GetAutumnConstructor();
                var obj = (ICommandLineRunner)constructor.Invoke(constructor.GetAutumnConstructorArguments(
                    new AutowiredContext(null, Context, constructor.GetCustomAttribute<QualifierAttribute>(),
                        constructor)
                ));
                context.Autowire(obj);
                context.PostConstruction(obj);
                if (obj.GetType().GetInterfaces().Contains(typeof(IDisposable)))
                    disposableRunner = obj as IDisposable;
                obj.Run(commandLineArgs);
                
            } // TODO: Another runners
            return this;
        }

        public void Dispose()
        {
            disposableRunner?.Dispose();
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