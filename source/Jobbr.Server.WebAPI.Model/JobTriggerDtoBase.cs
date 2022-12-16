using System;

namespace Jobbr.Server.WebAPI.Model
{
    /// <summary>
    /// The job trigger base.
    /// </summary>
    public class JobTriggerDtoBase
    {
        public long Id { get; set; }

        public virtual string TriggerType { get; }

        public bool IsActive { get; set; }

        public object Parameters { get; set; }

        public string Comment { get; set; }

        public string UserId { get; set; }

        public string UserDisplayName { get; set; }

        public bool Deleted { get; set; }
    }

    public class RecurringTriggerDto : JobTriggerDtoBase
    {
        public static string Type = "Recurring";

        public override string TriggerType => Type;

        public DateTime? StartDateTimeUtc { get; set; }

        public DateTime? EndDateTimeUtc { get; set; }

        public string Definition { get; set; }
    }

    public class ScheduledTriggerDto : JobTriggerDtoBase
    {
        public static string Type = "Scheduled";

        public override string TriggerType => Type;

        public DateTime StartDateTimeUtc { get; set; }
    }

    public class InstantTriggerDto : JobTriggerDtoBase
    {
        public static string Type = "Instant";

        public override string TriggerType => Type;

        public int DelayedMinutes { get; set; }
    }
}
