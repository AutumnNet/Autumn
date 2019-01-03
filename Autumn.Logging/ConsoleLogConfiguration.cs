using System;
using Autumn.Annotation;

namespace Autumn.Logging
{
    [Configuration]
    public class ConsoleLogConfiguration
    {
        public class LogWrapper : Logging.ILog
        {
            private readonly ILog log;

            private readonly string target;

            private void Message(string level, object message)
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} {target} [{level}] {message}");
            }

            private void Message(string level, object message, Exception e)
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} {target} [{level}] {message}\n{e.StackTrace}\n");
            }

            
            public void Debug(object message) { Message("debug", message); }
            public void Debug(object message, Exception exception) { Message("debug", message, exception); }
            
            public void Info(object message) { Message("info ", message); }
            public void Info(object message, Exception exception) { Message("info ", message, exception); }
            
            public void Warn(object message) { Message("warn ", message); }
            public void Warn(object message, Exception exception) { Message("warn ", message, exception); }
            
            public void Error(object message) { Message("error", message); }
            public void Error(object message, Exception exception) { Message("error", message, exception); }
            

            public LogWrapper(string target)
            {
                this.target = target;
            }
        }

        [Bean(Singleton = false)]
        public ILog getILog([Value("{context.target}")] object o)
        {
            Console.WriteLine("Get Logger!");
            return new LogWrapper(o.GetType().FullName);
        }
    }
}