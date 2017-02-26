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
        [Route("api/jobRuns/{jobRunId}")]
        public IHttpActionResult GetJonRun(long jobRunId)
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
        [Route("api/jobRuns/")]
        public IHttpActionResult GetJonRunsByUserId(long userId)
        {
            var jobRuns = this.queryService.GetJobRunsByUserOrderByIdDesc(userId);

            var jobRunDtos = jobRuns.Select(this.ConvertToDto);

            return this.Ok(jobRunDtos);
        }

        [HttpGet]
        [Route("api/jobRuns/")]
        public IHttpActionResult GetJonRunsByTriggerId(long triggerId)
        {
            var jobRuns = this.queryService.GetJobRunsByTriggerId(triggerId);

            var jobRunDtos = jobRuns.Select(this.ConvertToDto);

            return this.Ok(jobRunDtos);
        }

        [HttpGet]
        [Route("api/jobRuns/")]
        public IHttpActionResult GetJonRunsByUserName(string userName)
        {
            var jobRuns = this.queryService.GetJobRunsByUserNameOrderOrderByIdDesc(userName);

            var jobRunDtos = jobRuns.Select(this.ConvertToDto);

            return this.Ok(jobRunDtos);
        }

        [HttpGet]
        [Route("api/jobRuns/{jobRunId}/artefacts/{filename}")]
        public IHttpActionResult GetArtefact(long jobRunId, string filename)
        {
            var jobRun = this.queryService.GetJobRunById(jobRunId);

            if (jobRun == null)
            {
                return this.NotFound();
            }

            var fileStream = this.jobManagementService.GetArtefactAsStream(jobRun.UniqueId, filename);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            result.Content = new StreamContent(fileStream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return this.ResponseMessage(result);
        }
    
        private JobRunDto ConvertToDto(JobRun jobRun)
        {
            var jobParameter = jobRun.JobParameters != null ? JsonConvert.DeserializeObject(jobRun.JobParameters) : null;
            var instanceParameter = jobRun.InstanceParameters != null ? JsonConvert.DeserializeObject(jobRun.InstanceParameters) : null;

            var files = this.jobManagementService.GetArtefactForJob(jobRun.UniqueId);
            var filesList = files.Select(fileInfo => new JobRunArtefactDto() { Filename = fileInfo.Filename, Size = fileInfo.Size, }).ToList();

            var job = this.queryService.GetJobById(jobRun.JobId);

            var dto = new JobRunDto()
                          {
                              JobRunId = jobRun.Id,
                              JobId = jobRun.JobId,
                              JobName = job.UniqueName,
                              JobTitle = job.UniqueName,
                              TriggerId = jobRun.TriggerId,
                              UniqueId = jobRun.UniqueId,
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
