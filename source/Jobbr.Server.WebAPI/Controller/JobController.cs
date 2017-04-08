using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.Server.WebAPI.Controller.Mapping;
using Jobbr.WebAPI.Common.Models;
using Newtonsoft.Json;

namespace Jobbr.Server.WebAPI.Controller
{
    /// <summary>
    /// The job controller.
    /// </summary>
    public class JobController : ApiController
    {
        private readonly IQueryService queryService;
        private readonly IJobManagementService jobManagementService;

        public JobController(IQueryService queryService, IJobManagementService jobManagementService)
        {
            this.queryService = queryService;
            this.jobManagementService = jobManagementService;
        }

        [HttpGet]
        [Route("api/jobs")]
        public IHttpActionResult AllJobs()
        {
            return this.Ok(this.queryService.GetJobs(0, int.MaxValue).Select(JobMapper.Map));
        }

        [HttpGet]
        [Route("api/jobs/{jobId}")]
        public IHttpActionResult SingleJob(long jobId)
        {
            var job = this.queryService.GetJobById(jobId);

            var triggers = this.queryService.GetTriggersByJobId(jobId);
            
            var jobDto = JobMapper.Map(job);
            jobDto.Trigger = triggers.Select(t => TriggerMapper.ConvertToDto(t as dynamic)).Cast<JobTriggerDtoBase>().ToList();

            return this.Ok(jobDto);
        }

        [HttpPost]
        [Route("api/jobs")]
        public IHttpActionResult AddJob([FromBody] JobDto dto)
        {
            var identifier = dto.UniqueName;

            if (string.IsNullOrEmpty(dto.UniqueName))
            {
                return this.StatusCode(HttpStatusCode.NotAcceptable);
            }

            var existingJob = this.queryService.GetJobByUniqueName(identifier);

            if (existingJob != null)
            {
                return this.Conflict();
            }

            var job = new Job() { UniqueName = dto.UniqueName, Title = dto.Title, Type = dto.Type, Parameters = dto.Parameters != null ? JsonConvert.SerializeObject(dto.Parameters) : null, };
            this.jobManagementService.AddJob(job);

            return this.Created("/api/jobs/" + job.Id, JobMapper.Map(job));
        }

        [HttpPost]
        [Route("api/jobs/{jobId:long}")]
        public IHttpActionResult UpdateJob(long jobId, [FromBody] JobDto dto)
        {
            var existingJob = this.queryService.GetJobById(jobId);

            if (existingJob == null)
            {
                return this.NotFound();
            }

            return this.InternalServerError(new NotImplementedException());
        }

        [HttpGet]
        [Route("api/jobs/{jobId:long}/runs")]
        public IHttpActionResult GetJobRunsForJobById(long jobId)
        {
            var jobRuns = this.queryService.GetJobRuns().Where(jr => jr.JobId == jobId);

            var list = jobRuns.Select(JobMapper.Map).ToList();

            return this.Ok(list);
        }

        [HttpGet]
        [Route("api/jobs/{uniqueName}/runs")]
        public IHttpActionResult GetJobRunsForJobByUniqueName(string uniqueName)
        {
            var job = this.queryService.GetJobByUniqueName(uniqueName);

            var jobRuns = this.queryService.GetJobRuns().Where(jr => jr.JobId == job.Id);

            var list = jobRuns.Select(JobMapper.Map).ToList();

            return this.Ok(list);
        }
    }
}
