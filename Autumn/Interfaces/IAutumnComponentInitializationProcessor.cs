using Autumn.Net.Engine;

namespace Autumn.Net.Interfaces
{
    /// <summary>
    /// CTX Processor
    /// </summary>
    public interface IAutumnComponentInitializationProcessor
    {
        /// <summary>
        /// Process
        /// </summary>
        /// <param name="o">Object</param>
        /// <param name="ctx">Application Context</param>
        void Process(object o, ApplicationContext ctx);
    }
}