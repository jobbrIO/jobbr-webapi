using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.Server.WebAPI.Controller.Mapping;
using Jobbr.Server.WebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.Json;

namespace Jobbr.Server.WebAPI.Controller
{
    /// <summary>
    /// The job controller.
    /// </summary>
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IQueryService _queryService;
        private readonly IJobManagementService _jobManagementService;

        public JobController(IQueryService queryService, IJobManagementService jobManagementService)
        {
            _queryService = queryService;
            _jobManagementService = jobManagementService;
        }

        [HttpGet("jobs")]
        public IActionResult AllJobs(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, bool showDeleted = false, string sort = null)
        {
            return Ok(_queryService.GetJobs(page, pageSize, jobTypeFilter, jobUniqueNameFilter, query, showDeleted, sort?.Split(',')).ToPagedResult());
        }

        [HttpGet("jobs/{jobId}")]
        public IActionResult TriggersByJob(long jobId, int page = 1, int pageSize = 200, bool showDeleted = false)
        {
            var job = _queryService.GetJobById(jobId);

            if (job == null)
            {
                return NotFound();
            }

            var pagedResult = _queryService.GetTriggersByJobId(jobId, page, pageSize, showDeleted);

            var jobDto = job.ToDto();
            jobDto.Trigger = pagedResult.Items.Select(t => TriggerMapper.ConvertToDto(t as dynamic)).Cast<JobTriggerDtoBase>().ToList();

            return Ok(jobDto);
        }

        [HttpPost("jobs")]
        public IActionResult AddJob([FromBody] JobDto dto)
        {
            var identifier = dto.UniqueName;

            if (string.IsNullOrEmpty(dto.UniqueName))
            {
                return NotFound();
            }

            var existingJob = _queryService.GetJobByUniqueName(identifier);

            if (existingJob != null)
            {
                return Conflict();
            }

            var job = new Job { UniqueName = dto.UniqueName, Title = dto.Title, Type = dto.Type, Parameters = dto.Parameters != null ? JsonSerializer.Serialize(dto.Parameters, DefaultJsonOptions.Options) : null, };
            _jobManagementService.AddJob(job);

            return Created("jobs/" + job.Id, job.ToDto());
        }

        [HttpPost("jobs/{jobId:long}")]
        public IActionResult UpdateJob(long jobId, [FromBody] JobDto dto)
        {
            var existingJob = _queryService.GetJobById(jobId);

            if (existingJob == null)
            {
                return NotFound();
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new NotImplementedException());
        }

        [HttpGet("jobs/{jobId:int}/runs")]
        public IActionResult GetJobRunsForJobById(int jobId, int page = 1, int pageSize = 50, bool showDeleted = false, string sort = null)
        {
            return Ok(_queryService.GetJobRunsByJobId(jobId, page, pageSize, showDeleted, sort?.Split(',')).ToPagedResult());
        }

        [HttpGet("jobs/{uniqueName}/runs")]
        public IActionResult GetJobRunsForJobByUniqueName(string uniqueName, int page = 1, int pageSize = 50, bool showDeleted = false, string sort = null)
        {
            var job = _queryService.GetJobByUniqueName(uniqueName);

            if (job == null)
            {
                return NotFound();
            }

            var jobRuns = _queryService.GetJobRunsByJobId((int)job.Id, page, pageSize, showDeleted, sort?.Split(','));

            return Ok(jobRuns.ToPagedResult());
        }
    }
}
