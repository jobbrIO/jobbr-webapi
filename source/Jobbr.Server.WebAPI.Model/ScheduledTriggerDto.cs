using System;

namespace Jobbr.Server.WebAPI.Model
{
    /// <summary>
    /// Data transfer object for scheduled trigger.
    /// </summary>
    public class ScheduledTriggerDto : JobTriggerDtoBase
    {
        /// <summary>
        /// Trigger type as string.
        /// </summary>
        public const string Type = "Scheduled";

        /// <inheritdoc/>
        public override string TriggerType => Type;

        /// <summary>
        /// Start time stamp in UTC.
        /// </summary>
        public DateTime StartDateTimeUtc { get; set; }
    }
}
