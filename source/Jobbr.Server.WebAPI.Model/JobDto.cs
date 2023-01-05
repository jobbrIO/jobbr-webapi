using System;
using System.Collections.Generic;

namespace Jobbr.Server.WebAPI.Model
{
    /// <summary>
    /// Job data transfer object.
    /// </summary>
    public class JobDto
    {
        /// <summary>
        /// Job ID.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Unique name.
        /// </summary>
        public string UniqueName { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Parameters.
        /// </summary>
        public object Parameters { get; set; }

        /// <summary>
        /// Created timestamp in UTC.
        /// </summary>
        public DateTime? CreatedDateTimeUtc { get; set; }

        /// <summary>
        /// Updated timestamp in UTC.
        /// </summary>
        public DateTime? UpdatedDateTimeUtc { get; set; }

        /// <summary>
        /// Job trigger data transfer object base.
        /// </summary>
        public List<JobTriggerDtoBase> Trigger { get; set; }

        /// <summary>
        /// If job has been deleted.
        /// </summary>
        public bool Deleted { get; set; }
    }
}
