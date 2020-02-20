using System;
using Autumn.Net.Annotation;

namespace Autumn.Net.Configuration
{
    [Configuration]
    public class ConsoleLogConfiguration
    {
        public class LogWrapper : Interfaces.ILog
        {
            private readonly string target;

            private void Message(string level, object message)
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} {target} [{level}] {message}");
            }

            private void Message(string level, object message, Exception e)
            {
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()} {target} [{level}] {message}\n{e.StackTrace}\n");
            }

            public void Debug(object message) { Message("DEBUG", message); }
            public void Debug(object message, Exception exception) { Message("DEBUG", message, exception); }
            
            public void Info(object message) { Message("INFO ", message); }
            public void Info(object message, Exception exception) { Message("INFO ", message, exception); }
            
            public void Warn(object message) { Message("WARN ", message); }
            public void Warn(object message, Exception exception) { Message("WARN ", message, exception); }
            
            public void Error(object message) { Message("ERROR", message); }
            public void Error(object message, Exception exception) { Message("ERROR", message, exception); }

            public bool IsDebugEnable => true;
            public bool IsInfoEnable => true;
            public bool IsWarnEnable => true;
            public bool IsErrorEnable => true;


            public LogWrapper(string target)
            {
                this.target = target;
            }
        }

        [Bean(Singleton = false)]
        public Interfaces.ILog getILog([Value("{context.target}")] object o, [Value("{logger.name:}")] string name = "")
        {            
            return new LogWrapper(string.IsNullOrEmpty(name) ? o.GetType().FullName : name);
        }
    }
}