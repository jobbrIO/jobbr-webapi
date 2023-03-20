using System;

namespace Jobbr.Server.WebAPI.Model
{
    /// <summary>
    /// Data transfer object for recurring trigger.
    /// </summary>
    public class RecurringTriggerDto : JobTriggerDtoBase
    {
        /// <summary>
        /// Trigger type as string.
        /// </summary>
        public const string Type = "Recurring";

        /// <inheritdoc/>
        public override string TriggerType => Type;

        /// <summary>
        /// Start time stamp in UTC.
        /// </summary>
        public DateTime? StartDateTimeUtc { get; set; }

        /// <summary>
        /// End time stamp in UTC.
        /// </summary>
        public DateTime? EndDateTimeUtc { get; set; }

        /// <summary>
        /// Definition.
        /// </summary>
        public string Definition { get; set; }
    }
}
