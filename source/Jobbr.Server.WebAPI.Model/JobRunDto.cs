using System;
using System.Collections.Generic;

namespace Jobbr.Server.WebAPI.Model
{
    /// <summary>
    /// The job run dto.
    /// </summary>
    public class JobRunDto
    {
        public long JobId { get; set; }

        public long TriggerId { get; set; }

        public string TriggerType { get; set; }

        public long JobRunId { get; set; }

        public object JobParameter { get; set; }

        public object InstanceParameter { get; set; }

        public string JobName { get; set; }

        public string JobType { get; set; }

        public string State { get; set; }

        public double? Progress { get; set; }

        public DateTime PlannedStartUtc { get; set; }

        public DateTime? ActualStartUtc { get; set; }

        public DateTime? EstimatedEndtUtc { get; set; }

        public DateTime? ActualEndUtc { get; set; }

        public List<JobRunArtefactDto> Artefacts { get; set; }

        public string JobTitle { get; set; }

        public string Comment { get; set; }

        public string UserId { get; set; }

        public string UserDisplayName { get; set; }

        public string Definition { get; set; }

        public bool Deleted { get; set; }
    }
}
