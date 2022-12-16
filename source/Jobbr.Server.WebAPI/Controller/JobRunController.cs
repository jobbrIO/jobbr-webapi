using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.Server.WebAPI.Controller.Mapping;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Jobbr.Server.WebAPI.Controller
{
    [ApiController]
    public class JobRunController : ControllerBase
    {
        private readonly IQueryService _queryService;
        private readonly IJobManagementService _jobManagementService;

        public JobRunController(IQueryService queryService, IJobManagementService jobManagementService)
        {
            _queryService = queryService;
            _jobManagementService = jobManagementService;
        }

        [HttpGet("jobruns/{jobRunId}")]
        public IActionResult GetJobRun(long jobRunId)
        {
            var jobRun = _queryService.GetJobRunById(jobRunId);

            if (jobRun == null)
            {
                return NotFound();
            }

            var artefacts = _jobManagementService.GetArtefactForJob(jobRunId);

            return Ok(jobRun.ToDto(artefacts));
        }

        [HttpGet("jobruns")]
        public IActionResult GetJobRuns(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null, string state = null, string states = null, string userDisplayName = null, bool showDeleted = false)
        {
            PagedResult<JobRun> jobRuns;

            if (string.IsNullOrWhiteSpace(state) == false)
            {
                var success = Enum.TryParse(state, true, out JobRunStates enumValue);

                if (success == false)
                {
                    return BadRequest($"Unknown state: {state}");
                }

                jobRuns = _queryService.GetJobRunsByState(enumValue, page, pageSize, jobTypeFilter, jobUniqueNameFilter, query, showDeleted, sort?.Split(','));
            }
            else if (string.IsNullOrWhiteSpace(states) == false)
            {
                var stateAsEnums = states.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s =>
                {
                    var success = Enum.TryParse(s, true, out JobRunStates enumValue);

                    if (success == false)
                    {
                        return JobRunStates.Null;
                    }

                    return enumValue;
                }).ToArray();

                jobRuns = _queryService.GetJobRunsByStates(stateAsEnums, page, pageSize, jobTypeFilter, jobUniqueNameFilter, query, showDeleted, sort?.Split(','));
            }
            else if (string.IsNullOrWhiteSpace(userDisplayName) == false)
            {
                jobRuns = _queryService.GetJobRunsByUserDisplayName(userDisplayName, page, pageSize, jobTypeFilter, jobUniqueNameFilter, showDeleted, sort?.Split(','));
            }
            else
            {
                jobRuns = _queryService.GetJobRuns(page, pageSize, jobTypeFilter, jobUniqueNameFilter, query, showDeleted, sort?.Split(','));
            }

            return Ok(jobRuns.ToPagedResult());
        }

        [HttpGet("users/{userId}/jobruns/")]
        public IActionResult GetJobRunsByUserId(string userId, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string sort = null, bool showDeleted = false)
        {
            var jobRuns = _queryService.GetJobRunsByUserId(userId, page, pageSize, jobTypeFilter, jobUniqueNameFilter, showDeleted, sort?.Split(','));

            return Ok(jobRuns.ToPagedResult());
        }

        [HttpGet("jobs/{jobId}/triggers/{triggerId}/jobruns")]
        public IActionResult GetJobRunsByTrigger(long jobId, long triggerId, int page = 1, int pageSize = 50, string sort = null, bool showDeleted = false)
        {
            var jobRuns = _queryService.GetJobRunsByTriggerId(jobId, triggerId, page, pageSize, showDeleted, sort?.Split(','));

            return Ok(jobRuns.ToPagedResult());
        }

        [HttpGet("jobruns/{jobRunId}/artefacts/{filename}")]
        public IActionResult GetArtefact(long jobRunId, string filename)
        {
            var jobRun = _queryService.GetJobRunById(jobRunId);

            if (jobRun == null)
            {
                return NotFound();
            }

            var fileStream = _jobManagementService.GetArtefactAsStream(jobRun.Id, filename);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(fileStream)
            };

            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return Ok(result);
        }

        [HttpDelete("jobruns/{jobRunId}")]
        public IActionResult SoftDeleteJobRun(long jobRunId)
        {
            _jobManagementService.DeleteJobRun(jobRunId);

            return Ok();
        }
    }
}