using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.WebAPI.Common.Models;
using Newtonsoft.Json;

namespace Jobbr.Server.WebAPI.Core.Controller
{
    public class JobRunController : ApiController
    {
        private readonly IQueryService queryService;
        private readonly IJobManagementService jobManagementService;

        public JobRunController(IQueryService queryService, IJobManagementService jobManagementService)
        {
            this.queryService = queryService;
            this.jobManagementService = jobManagementService;
        }

        [HttpGet]
        [Route("api/jobruns/{jobRunId}")]
        public IHttpActionResult GetJobRuns(long jobRunId)
        {
            var jobRun = this.queryService.GetJobRunById(jobRunId);

            if (jobRun == null)
            {
                return this.NotFound();
            }

            var dto = this.ConvertToDto(jobRun);

            return this.Ok(dto);
        }

        [HttpGet]
        [Route("api/jobruns/")]
        public IHttpActionResult GetJobRunsByUserId(string userId)
        {
            var jobRuns = this.queryService.GetJobRunsByUserIdOrderByIdDesc(userId);

            var jobRunDtos = jobRuns.Select(this.ConvertToDto);

            return this.Ok(jobRunDtos);
        }

        [HttpGet]
        [Route("api/jobruns/")]
        public IHttpActionResult GetJobRunsByTrigger(long jobId, long triggerId)
        {
            var jobRuns = this.queryService.GetJobRunsByTriggerId(jobId, triggerId);

            var jobRunDtos = jobRuns.Select(this.ConvertToDto);

            return this.Ok(jobRunDtos);
        }

        [HttpGet]
        [Route("api/jobruns/")]
        public IHttpActionResult GetJobRunsByUserDisplayName(string userDisplayName)
        {
            var jobRuns = this.queryService.GetJobRunsByUserDisplayNameOrderByIdDesc(userDisplayName);

            var jobRunDtos = jobRuns.Select(this.ConvertToDto);

            return this.Ok(jobRunDtos);
        }

        [HttpGet]
        [Route("api/jobruns/{jobRunId}/artefacts/{filename}")]
        public IHttpActionResult GetArtefact(long jobRunId, string filename)
        {
            var jobRun = this.queryService.GetJobRunById(jobRunId);

            if (jobRun == null)
            {
                return this.NotFound();
            }

            var fileStream = this.jobManagementService.GetArtefactAsStream(jobRun.Id, filename);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(fileStream)
            };

            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return this.ResponseMessage(result);
        }
    
        private JobRunDto ConvertToDto(JobRun jobRun)
        {
            var jobParameter = jobRun.JobParameters != null ? JsonConvert.DeserializeObject(jobRun.JobParameters) : null;
            var instanceParameter = jobRun.InstanceParameters != null ? JsonConvert.DeserializeObject(jobRun.InstanceParameters) : null;

            var files = this.jobManagementService.GetArtefactForJob(jobRun.Id);
            var filesList = files.Select(fileInfo => new JobRunArtefactDto() { Filename = fileInfo.Filename, Size = fileInfo.Size, }).ToList();

            var job = this.queryService.GetJobById(jobRun.JobId);

            var dto = new JobRunDto()
                          {
                              JobRunId = jobRun.Id,
                              JobId = jobRun.JobId,
                              JobName = job.UniqueName,
                              JobTitle = job.UniqueName,
                              TriggerId = jobRun.TriggerId,
                              JobParameter = jobParameter,
                              InstanceParameter = instanceParameter,
                              State = jobRun.State.ToString(),
                              Progress = jobRun.Progress,
                              PlannedStartUtc = jobRun.PlannedStartDateTimeUtc,
                              AuctualStartUtc = jobRun.ActualStartDateTimeUtc,
                              EstimatedEndtUtc = jobRun.EstimatedEndDateTimeUtc,
                              AuctualEndUtc = jobRun.ActualEndDateTimeUtc,
                              Artefacts = filesList.Any() ? filesList : null
                          };
            return dto;
        }
    }
}
