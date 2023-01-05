namespace Jobbr.Server.WebAPI.Model
{
    /// <summary>
    /// Data transfer object for an instant trigger.
    /// </summary>
    public class InstantTriggerDto : JobTriggerDtoBase
    {
        /// <summary>
        /// Trigger type as string.
        /// </summary>
        public const string Type = "Instant";

        /// <inheritdoc/>
        public override string TriggerType => Type;

        /// <summary>
        /// The amount of delay in the trigger in minutes.
        /// </summary>
        public int DelayedMinutes { get; set; }
    }
}
