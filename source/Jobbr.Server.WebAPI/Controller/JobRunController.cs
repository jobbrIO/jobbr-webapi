using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.Server.WebAPI.Controller.Mapping;

namespace Jobbr.Server.WebAPI.Controller
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
        [Route("jobruns/{jobRunId}")]
        public IHttpActionResult GetJobRuns(long jobRunId)
        {
            var jobRun = this.queryService.GetJobRunById(jobRunId);

            if (jobRun == null)
            {
                return this.NotFound();
            }

            var artefacts = this.jobManagementService.GetArtefactForJob(jobRunId);

            return this.Ok(jobRun.ToDto(artefacts));
        }

        [HttpGet]
        [Route("jobruns")]
        public IHttpActionResult GetJobRuns(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null, string state = null, string[] states = null, string userDisplayName = null)
        {
            PagedResult<JobRun> jobRuns;

            if (string.IsNullOrWhiteSpace(state) == false)
            {
                var success = Enum.TryParse(state, true, out JobRunStates enumValue);

                if (success == false)
                {
                    return this.BadRequest($"Unknown state: {state}");
                }

                jobRuns = this.queryService.GetJobRunsByState(enumValue, page, pageSize, jobTypeFilter, jobUniqueNameFilter, query, sort?.Split(','));
            }
            else if (states != null)
            {
                var stateAsEnums = states.Select(s =>
                {
                    var success = Enum.TryParse(s, true, out JobRunStates enumValue);

                    if (success == false)
                    {
                        return JobRunStates.Null;
                    }

                    return enumValue;
                }).ToArray();

                jobRuns = this.queryService.GetJobRunsByStates(stateAsEnums, page, pageSize, jobTypeFilter, jobUniqueNameFilter, query, sort?.Split(','));
            }
            else if (string.IsNullOrWhiteSpace(userDisplayName) == false)
            {
                jobRuns = this.queryService.GetJobRunsByUserDisplayName(userDisplayName, page, pageSize, jobTypeFilter, jobUniqueNameFilter, sort?.Split(','));
            }
            else
            {
                jobRuns = this.queryService.GetJobRuns(page, pageSize, jobTypeFilter, jobUniqueNameFilter, query, sort?.Split(','));
            }

            return this.Ok(jobRuns.ToPagedResult());
        }

        [HttpGet]
        [Route("users/{userId}/jobruns/")]
        public IHttpActionResult GetJobRunsByUserId(string userId, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string sort = null)
        {
            var jobRuns = this.queryService.GetJobRunsByUserId(userId, page, pageSize, jobTypeFilter, jobUniqueNameFilter, sort?.Split(','));

            return this.Ok(jobRuns.ToPagedResult());
        }

        [HttpGet]
        [Route("jobs/{jobId}/triggers/{triggerId}/jobruns")]
        public IHttpActionResult GetJobRunsByTrigger(long jobId, long triggerId, int page = 1, int pageSize = 50, string sort = null)
        {
            var jobRuns = this.queryService.GetJobRunsByTriggerId(jobId, triggerId, page, pageSize, sort?.Split(','));

            return this.Ok(jobRuns.ToPagedResult());
        }

        [HttpGet]
        [Route("jobruns/{jobRunId}/artefacts/{filename}")]
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
    }
}
