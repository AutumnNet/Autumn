using System;
using Autumn.Annotation;
using log4net;


namespace Autumn.Log4Net
{
    [Configuration]
    public class LogConfiguration
    {
        public class LogWrapper : Logging.ILog
        {
            private readonly ILog log;
            
            public void Debug(object message) { log.Debug(message); }
            public void Debug(object message, Exception exception) { log.Debug(message, exception); }
            
            public void Info(object message) { log.Info(message); }
            public void Info(object message, Exception exception) { log.Info(message, exception); }
            
            public void Warn(object message) { log.Warn(message); }
            public void Warn(object message, Exception exception) { log.Warn(message, exception); }
            
            public void Error(object message) { log.Error(message); }
            public void Error(object message, Exception exception) { log.Error(message, exception); }
            

            public LogWrapper()
            {
                log = LogManager.GetLogger("LOGGER");
            }
            
            public LogWrapper(Type t)
            {
                log = LogManager.GetLogger(t.FullName);
            }

        }

        [Bean(Singleton = false)]
        [Primary]
        public Logging.ILog getILog([Value("context.target")] object o)
        {
            return new LogWrapper(o.GetType());
        }
    }
}