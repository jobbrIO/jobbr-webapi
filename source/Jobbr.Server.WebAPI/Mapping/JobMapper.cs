using System;
using Jobbr.Server.Model;
using Newtonsoft.Json;
using WebAPI.Models;

namespace WebAPI.Mapping
{
    public class JobMapper
    {
        public static JobDto Map(Job job)
        {
            return new JobDto()
                       {
                           Id = job.Id,
                           UniqueName = job.UniqueName,
                           Title = job.Title,
                           Parameters = job.Parameters != null ? JsonConvert.DeserializeObject(job.Parameters) : null,
                           Type = job.Type,
                           UpdatedDateTimeUtc = job.UpdatedDateTimeUtc,
                           CreatedDateTimeUtc = job.CreatedDateTimeUtc
                       };
        }

        public static JobRunDto Map(JobRun jobRun)
        {
            return new JobRunDto()
                       {
                           JobRunId = jobRun.Id,
                           JobId = jobRun.JobId,
                           TriggerId = jobRun.TriggerId,
                           UniqueId = new Guid(jobRun.UniqueId),
                           State = jobRun.State.ToString(),
                           Progress = jobRun.Progress,
                           PlannedStartUtc = jobRun.PlannedStartDateTimeUtc,
                           AuctualStartUtc = jobRun.ActualStartDateTimeUtc,
                           AuctualEndUtc = jobRun.ActualEndDateTimeUtc,
                       };
        }
    }
}