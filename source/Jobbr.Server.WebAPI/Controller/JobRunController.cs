using System;
using System.Linq;
using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.Server.WebAPI.Controller.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace Jobbr.Server.WebAPI.Controller
{
    /// <summary>
    /// Job run controller.
    /// </summary>
    [ApiController]
    public class JobRunController : ControllerBase
    {
        private readonly IQueryService _queryService;
        private readonly IJobManagementService _jobManagementService;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobRunController"/> class.
        /// </summary>
        /// <param name="queryService">Query service.</param>
        /// <param name="jobManagementService">Job management service.</param>
        public JobRunController(IQueryService queryService, IJobManagementService jobManagementService)
        {
            _queryService = queryService;
            _jobManagementService = jobManagementService;
        }

        /// <summary>
        /// Get job run with ID.
        /// </summary>
        /// <param name="jobRunId">Job run ID.</param>
        /// <returns>NotFound or the found job run.</returns>
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

        /// <summary>
        /// Get multiple job runs.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="jobTypeFilter">Job type filter.</param>
        /// <param name="jobUniqueNameFilter">Job unique name filter.</param>
        /// <param name="query">Query.</param>
        /// <param name="sort">Sort.</param>
        /// <param name="state">Job run state.</param>
        /// <param name="states">Job run states.</param>
        /// <param name="userDisplayName">User display name.</param>
        /// <param name="showDeleted">Show deleted job runs.</param>
        /// <returns>List of job runs.</returns>
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

        /// <summary>
        /// Get job runs by user ID.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="jobTypeFilter">Job type filter.</param>
        /// <param name="jobUniqueNameFilter">Job unique name filter.</param>
        /// <param name="sort">Sort.</param>
        /// <param name="showDeleted">Show deleted job runs.</param>
        /// <returns>List of job runs.</returns>
        [HttpGet("users/{userId}/jobruns/")]
        public IActionResult GetJobRunsByUserId(string userId, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string sort = null, bool showDeleted = false)
        {
            var jobRuns = _queryService.GetJobRunsByUserId(userId, page, pageSize, jobTypeFilter, jobUniqueNameFilter, showDeleted, sort?.Split(','));

            return Ok(jobRuns.ToPagedResult());
        }

        /// <summary>
        /// Get job runs by trigger.
        /// </summary>
        /// <param name="jobId">Job ID.</param>
        /// <param name="triggerId">Trigger ID.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="sort">Sort.</param>
        /// <param name="showDeleted">Show deleted job runs.</param>
        /// <returns>List of job runs.</returns>
        [HttpGet("jobs/{jobId}/triggers/{triggerId}/jobruns")]
        public IActionResult GetJobRunsByTrigger(long jobId, long triggerId, int page = 1, int pageSize = 50, string sort = null, bool showDeleted = false)
        {
            var jobRuns = _queryService.GetJobRunsByTriggerId(jobId, triggerId, page, pageSize, showDeleted, sort?.Split(','));

            return Ok(jobRuns.ToPagedResult());
        }

        /// <summary>
        /// Get job run artifact.
        /// </summary>
        /// <param name="jobRunId">Job run ID.</param>
        /// <param name="filename">Filename.</param>
        /// <returns>The artifact.</returns>
        [HttpGet("jobruns/{jobRunId}/artefacts/{filename}")]
        [Produces("application/octet-stream")]
        public IActionResult GetArtefact(long jobRunId, string filename)
        {
            var jobRun = _queryService.GetJobRunById(jobRunId);

            if (jobRun == null)
            {
                return NotFound();
            }

            var fileStream = _jobManagementService.GetArtefactAsStream(jobRun.Id, filename);

            return new FileStreamResult(fileStream, "application/octet-stream");
        }

        /// <summary>
        /// Soft delete job run.
        /// </summary>
        /// <param name="jobRunId">Job run ID.</param>
        /// <returns>Ok.</returns>
        [HttpDelete("jobruns/{jobRunId}")]
        public IActionResult SoftDeleteJobRun(long jobRunId)
        {
            _jobManagementService.DeleteJobRun(jobRunId);

            return Ok();
        }
    }
}