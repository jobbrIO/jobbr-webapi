using System.Collections.Generic;
using System.Linq;
using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.Server.WebAPI.Infrastructure;
using Jobbr.Server.WebAPI.Model;
using Newtonsoft.Json;

namespace Jobbr.Server.WebAPI.Controller.Mapping
{
    public static class JobMapper
    {
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

        public static JobRunArtefactDto ToDto(this JobArtefact model)
        {
            return new JobRunArtefactDto
            {
                Filename = model.Filename,
                ContentType = model.Type,
                Size = model.Size
            };
        }

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