using System;
using System.Collections.Generic;

namespace Jobbr.Server.WebAPI.Model
{
    /// <summary>
    /// The job run dto.
    /// </summary>
    public class JobRunDto
    {
        /// <summary>
        /// Job ID.
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// Trigger ID.
        /// </summary>
        public long TriggerId { get; set; }

        /// <summary>
        /// Trigger type.
        /// </summary>
        public string TriggerType { get; set; }

        /// <summary>
        /// Job run ID.
        /// </summary>
        public long JobRunId { get; set; }

        /// <summary>
        /// Job parameter.
        /// </summary>
        public object JobParameter { get; set; }

        /// <summary>
        /// Instance parameter.
        /// </summary>
        public object InstanceParameter { get; set; }

        /// <summary>
        /// Job name.
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// Job type.
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// Job run state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Job run progress.
        /// </summary>
        public double? Progress { get; set; }

        /// <summary>
        /// Planned start in UTC.
        /// </summary>
        public DateTime PlannedStartUtc { get; set; }

        /// <summary>
        /// Actual start in UTC.
        /// </summary>
        public DateTime? ActualStartUtc { get; set; }

        /// <summary>
        /// Estimated end time in UTC.
        /// </summary>
        public DateTime? EstimatedEndtUtc { get; set; }

        /// <summary>
        /// Actual end time in UTC.
        /// </summary>
        public DateTime? ActualEndUtc { get; set; }

        /// <summary>
        /// List of job run artifacts.
        /// </summary>
        public List<JobRunArtefactDto> Artefacts { get; set; }

        /// <summary>
        /// Job title.
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// Comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// User ID.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User display name.
        /// </summary>
        public string UserDisplayName { get; set; }

        /// <summary>
        /// Definition.
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// If job run has been deleted.
        /// </summary>
        public bool Deleted { get; set; }
    }
}
