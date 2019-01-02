using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Autumn.Annotation;
using Autumn.Annotation.Base;
using Autumn.Engine;
using Autumn.Interfaces;

namespace Autumn.Schedule
{
    [Component]
    public class SchedulledComponentProcessor : IAutumnComponentInitializationProcessor
    {
        class MethodContainer : IDisposable
        {
            public object Target { get; set; }
            public MethodInfo Method { get; set; }
            public ScheduledAttribute Scheduled { get; set; }
            
            public Thread thread;
            

            private void Execute()
            {
                while (true)
                {
                    try {
                        Thread.Sleep(Scheduled.FixedRate);
                        Method.Invoke(Target, new object[0]);
                    } catch(Exception) { }
                }
            }
            
            public MethodContainer Start(ApplicationContext ctx)
            {
                thread = new Thread(Execute);
                thread.Start();
                return this;
            }

            public void Dispose()
            {
                thread.Abort();
            }
        }

        private ApplicationContext ctx;

        private List<MethodContainer> Methods { get; set; }

        [PreDestroy]
        public void PreDestroy()
        {
            Methods.ForEach(item => item.Dispose());
            Methods = null;
        }
        
        static IEnumerable<MethodInfo> IsAutumnScheduledAttribute(Type type)
        {
            return type
                .GetMethods()
                .Where(item => item.GetCustomAttributes(typeof(ScheduledAttribute), true).Length > 0);
        }
        
        public void Process(object o, ApplicationContext ctx)
        {
            this.ctx = ctx;
            Methods = new List<MethodContainer>();
            foreach (var methodInfo in IsAutumnScheduledAttribute(o.GetType())) {
               Methods.Add(new MethodContainer{
                   Target = o,
                   Method = methodInfo,
                   Scheduled = methodInfo.GetCustomAttribute<ScheduledAttribute>()
               }.Start(ctx)); 
            }
        }     
    }
}