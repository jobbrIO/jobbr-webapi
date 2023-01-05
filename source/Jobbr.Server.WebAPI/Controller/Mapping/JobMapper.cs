using System.Collections.Generic;
using System.Linq;
using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.Server.WebAPI.Infrastructure;
using Jobbr.Server.WebAPI.Model;

namespace Jobbr.Server.WebAPI.Controller.Mapping
{
    /// <summary>
    /// Object mapper for different objects related to jobs.
    /// </summary>
    public static class JobMapper
    {
        /// <summary>
        /// Maps <see cref="Job"/> to a <see cref="JobDto"/>.
        /// </summary>
        /// <param name="job"><see cref="Job"/> object.</param>
        /// <returns><see cref="JobDto"/> object.</returns>
        public static JobDto ToDto(this Job job)
        {
            return new JobDto
            {
                Id = job.Id,
                UniqueName = job.UniqueName,
                Title = job.Title,
                Parameters = job.Parameters?.Deserialize(),
                Type = job.Type,
                UpdatedDateTimeUtc = job.UpdatedDateTimeUtc,
                CreatedDateTimeUtc = job.CreatedDateTimeUtc,
                Deleted = job.Deleted,
            };
        }

        /// <summary>
        /// Maps <see cref="JobRun"/> and a list of <see cref="JobArtefact"/>s to a <see cref="JobRunDto"/>.
        /// </summary>
        /// <param name="jobRun"><see cref="Job"/> object.</param>
        /// <param name="artefacts">List of artifacts.</param>
        /// <returns><see cref="JobDto"/> object.</returns>
        public static JobRunDto ToDto(this JobRun jobRun, List<JobArtefact> artefacts)
        {
            return new JobRunDto
            {
                JobRunId = jobRun.Id,
                JobId = jobRun.JobId,
                JobName = jobRun.JobName,
                JobType = jobRun.JobType,
                Comment = jobRun.Comment,
                UserId = jobRun.UserId,
                UserDisplayName = jobRun.UserDisplayName,
                TriggerId = jobRun.TriggerId,
                TriggerType = jobRun.TriggerType,
                State = jobRun.State.ToString(),
                Progress = jobRun.Progress,
                PlannedStartUtc = jobRun.PlannedStartDateTimeUtc,
                ActualStartUtc = jobRun.ActualStartDateTimeUtc,
                ActualEndUtc = jobRun.ActualEndDateTimeUtc,
                EstimatedEndtUtc = jobRun.EstimatedEndDateTimeUtc,
                InstanceParameter = jobRun.InstanceParameters?.Deserialize(),
                JobParameter = jobRun.JobParameters?.Deserialize(),
                Artefacts = artefacts?.Select(s => s.ToDto()).ToList(),
                Definition = jobRun.Definition,
                Deleted = jobRun.Deleted,
            };
        }

        /// <summary>
        /// Maps <see cref="JobArtefact"/> to a <see cref="JobRunArtefactDto"/>.
        /// </summary>
        /// <param name="model"><see cref="JobArtefact"/> object.</param>
        /// <returns><see cref="JobRunArtefactDto"/> object.</returns>
        public static JobRunArtefactDto ToDto(this JobArtefact model)
        {
            return new JobRunArtefactDto
            {
                Filename = model.Filename,
                ContentType = model.Type,
                Size = model.Size
            };
        }

        /// <summary>
        /// Maps a paged result of <see cref="Job"/>s to a paged result of <see cref="JobDto"/>s.
        /// </summary>
        /// <param name="data">Paged result of <see cref="Job"/>s.</param>
        /// <returns>Paged result of <see cref="JobDto"/>s.</returns>
        public static PagedResultDto<JobDto> ToPagedResult(this PagedResult<Job> data)
        {
            return new PagedResultDto<JobDto>
            {
                Page = data.Page,
                PageSize = data.PageSize,
                Items = data.Items.Select(s => s.ToDto()).ToList(),
                TotalItems = data.TotalItems
            };
        }

        /// <summary>
        /// Maps a paged result of <see cref="JobRun"/>s to a paged result of <see cref="JobRunDto"/>s.
        /// </summary>
        /// <param name="data">Paged result of <see cref="JobRun"/>s.</param>
        /// <returns>Paged result of <see cref="JobRunDto"/>s.</returns>
        public static PagedResultDto<JobRunDto> ToPagedResult(this PagedResult<JobRun> data)
        {
            return new PagedResultDto<JobRunDto>
            {
                Page = data.Page,
                PageSize = data.PageSize,
                Items = data.Items.Select(s => s.ToDto(null)).ToList(),
                TotalItems = data.TotalItems
            };
        }
    }
}