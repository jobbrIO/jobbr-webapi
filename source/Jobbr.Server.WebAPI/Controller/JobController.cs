using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.Server.WebAPI.Controller.Mapping;
using Jobbr.Server.WebAPI.Model;
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
        [Route("jobs")]
        public IHttpActionResult AllJobs(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, bool showDeleted = false, string sort = null)
        {
            return this.Ok(this.queryService.GetJobs(page, pageSize, jobTypeFilter, jobUniqueNameFilter, query, showDeleted, sort?.Split(',')).ToPagedResult());
        }

        [HttpGet]
        [Route("jobs/{jobId}")]
        public IHttpActionResult TriggersByJob(long jobId, int page = 1, int pageSize = 200, bool showDeleted = false)
        {
            var job = this.queryService.GetJobById(jobId);

            if (job == null)
            {
                return this.NotFound();
            }

            var pagedResult = this.queryService.GetTriggersByJobId(jobId, page, pageSize, showDeleted);

            var jobDto = job.ToDto();
            jobDto.Trigger = pagedResult.Items.Select(t => TriggerMapper.ConvertToDto(t as dynamic)).Cast<JobTriggerDtoBase>().ToList();

            return this.Ok(jobDto);
        }

        [HttpPost]
        [Route("jobs")]
        public IHttpActionResult AddJob([FromBody] JobDto dto)
        {
            var identifier = dto.UniqueName;

            if (string.IsNullOrEmpty(dto.UniqueName))
            {
                return this.StatusCode(HttpStatusCode.NotFound);
            }

            var existingJob = this.queryService.GetJobByUniqueName(identifier);

            if (existingJob != null)
            {
                return this.Conflict();
            }

            var job = new Job { UniqueName = dto.UniqueName, Title = dto.Title, Type = dto.Type, Parameters = dto.Parameters != null ? JsonConvert.SerializeObject(dto.Parameters) : null, };
            this.jobManagementService.AddJob(job);

            return this.Created("jobs/" + job.Id, job.ToDto());
        }

        [HttpPost]
        [Route("jobs/{jobId:long}")]
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
        [Route("jobs/{jobId:int}/runs")]
        public IHttpActionResult GetJobRunsForJobById(int jobId, int page = 1, int pageSize = 50, bool showDeleted = false, string sort = null)
        {
            return this.Ok(this.queryService.GetJobRunsByJobId(jobId, page, pageSize, showDeleted, sort?.Split(',')).ToPagedResult());
        }

        [HttpGet]
        [Route("jobs/{uniqueName}/runs")]
        public IHttpActionResult GetJobRunsForJobByUniqueName(string uniqueName, int page = 1, int pageSize = 50, bool showDeleted = false, string sort = null)
        {
            var job = this.queryService.GetJobByUniqueName(uniqueName);

            if (job == null)
            {
                return this.NotFound();
            }

            var jobRuns = this.queryService.GetJobRunsByJobId((int)job.Id, page, pageSize, showDeleted, sort?.Split(','));

            return this.Ok(jobRuns.ToPagedResult());
        }
    }
}
