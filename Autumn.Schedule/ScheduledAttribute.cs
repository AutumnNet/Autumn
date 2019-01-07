using System;

namespace Autumn.Net.Net.Schedule
{
    /// <summary>
    /// Scheduler
    /// </summary>
    public class ScheduledAttribute: Attribute
    {
        /// <summary>
        /// Which specifies the interval between method
        /// invocations measured from the start time of
        /// each invocation
        /// </summary>
        public int FixedRate { get; set; }
        
        /// <summary>
        /// Which specifies the interval between invocations
        /// measured from the completion of the task
        /// </summary>
        public int FixedDelay { get; set; }
        
        /// <summary>
        /// Value name
        /// Which specifies the interval between method
        /// invocations measured from the start time of
        /// each invocation 
        /// </summary>
        public string FixedDelayString { get; set; }
        
        /// <summary>
        /// Value name
        /// Which specifies the interval between invocations
        /// measured from the completion of the task
        /// </summary>
        public string FixedRateString { get; set; }
        
        /// <summary>
        /// Crontab Format
        /// </summary>
        public string Cron { get; set; }
    }
}