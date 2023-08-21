using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.Server.WebAPI.Controller.Mapping;
using Jobbr.Server.WebAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace Jobbr.Server.WebAPI.Controller
{
    /// <summary>
    /// Job trigger controller.
    /// </summary>
    [ApiController]
    public class TriggerController : ControllerBase
    {
        private readonly IQueryService _queryService;
        private readonly IJobManagementService _jobManagementService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerController"/> class.
        /// </summary>
        /// <param name="queryService">Query service for triggers.</param>
        /// <param name="jobManagementService">Job management service.</param>
        public TriggerController(IQueryService queryService, IJobManagementService jobManagementService)
        {
            _queryService = queryService;
            _jobManagementService = jobManagementService;
        }

        /// <summary>
        /// Get trigger by job ID.
        /// </summary>
        /// <param name="jobId">Job ID.</param>
        /// <param name="triggerId">Trigger ID.</param>
        /// <returns>Trigger DTO.</returns>
        [HttpGet("jobs/{jobId:long}/triggers/{triggerId:long}")]
        public IActionResult GetTriggerById(long jobId, long triggerId)
        {
            var trigger = _queryService.GetTriggerById(jobId, triggerId);

            if (trigger == null)
            {
                return NotFound();
            }

            return Ok(TriggerMapper.ConvertToDto((dynamic)trigger));
        }

        /// <summary>
        /// Update job trigger.
        /// </summary>
        /// <param name="jobId">Job ID.</param>
        /// <param name="triggerId">Trigger ID.</param>
        /// <param name="dto">Information for updating.</param>
        /// <returns>The given DTO.</returns>
        [HttpPatch("jobs/{jobId:long}/triggers/{triggerId:long}")]
        public IActionResult UpdateTrigger(long jobId, long triggerId, [FromBody] JobTriggerDtoBase dto)
        {
            var currentTrigger = _queryService.GetTriggerById(jobId, triggerId);

            if (currentTrigger == null)
            {
                return NotFound();
            }

            if (currentTrigger.IsActive && !dto.IsActive)
            {
                currentTrigger.IsActive = false;
                _jobManagementService.DisableTrigger(jobId, currentTrigger.Id);
            }
            else if (!currentTrigger.IsActive && dto.IsActive)
            {
                currentTrigger.IsActive = true;
                _jobManagementService.EnableTrigger(jobId, currentTrigger.Id);
            }

            if (dto is RecurringTriggerDto recurringTriggerDto)
            {
                var existingTrigger = (RecurringTrigger)currentTrigger;
                var trigger = TriggerMapper.ConvertToTrigger(recurringTriggerDto);
                trigger.Id = triggerId;
                trigger.JobId = jobId;
                trigger.Definition = recurringTriggerDto.Definition ?? existingTrigger.Definition;
                trigger.StartDateTimeUtc = recurringTriggerDto.StartDateTimeUtc ?? existingTrigger.StartDateTimeUtc;
                trigger.EndDateTimeUtc = recurringTriggerDto.EndDateTimeUtc ?? existingTrigger.EndDateTimeUtc;

                _jobManagementService.Update(trigger);
            }
            else if (dto is ScheduledTriggerDto scheduledTriggerDto)
            {
                var trigger = TriggerMapper.ConvertToTrigger(scheduledTriggerDto);
                trigger.Id = triggerId;
                trigger.JobId = jobId;

                _jobManagementService.Update(trigger);
            }

            return Ok(dto);
        }

        /// <summary>
        /// Get job triggers with job ID.
        /// </summary>
        /// <param name="jobId">Job ID.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="showDeleted">If deleted triggers should be shown.</param>
        /// <returns>Triggers as a paged result.</returns>
        [HttpGet("jobs/{jobId:long}/triggers")]
        public IActionResult GetTriggersForJob(long jobId, int page = 1, int pageSize = 200, bool showDeleted = false)
        {
            var job = _queryService.GetJobById(jobId);

            if (job == null)
            {
                return NotFound();
            }

            var triggers = _queryService.GetTriggersByJobId(jobId, page, pageSize, showDeleted);

            return Ok(triggers.ToPagedResult());
        }

        /// <summary>
        /// Get job triggers with unique name.
        /// </summary>
        /// <param name="uniqueName">Unique name.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="showDeleted">Show deleted triggers.</param>
        /// <returns>List of job triggers.</returns>
        [HttpGet("jobs/{uniqueName}/triggers")]
        public IActionResult GetTriggersForJob(string uniqueName, int page = 1, int pageSize = 200, bool showDeleted = false)
        {
            var job = _queryService.GetJobByUniqueName(uniqueName);

            if (job == null)
            {
                return NotFound();
            }

            return Ok(_queryService.GetTriggersByJobId(job.Id, page, pageSize, showDeleted));
        }

        /// <summary>
        /// Add trigger for job with job ID.
        /// </summary>
        /// <param name="jobId">Job ID.</param>
        /// <param name="triggerDto">Trigger data.</param>
        /// <returns>Multiple different types of result based on success.</returns>
        [HttpPost("jobs/{jobId:long}/triggers")]
        public IActionResult AddTriggerForJobId(long jobId, [FromBody] JobTriggerDtoBase triggerDto)
        {
            var job = _queryService.GetJobById(jobId);

            if (job == null)
            {
                return NotFound();
            }

            return AddTrigger(triggerDto, job);
        }

        /// <summary>
        /// Add trigger for job with unique name.
        /// </summary>
        /// <param name="uniqueName">Unique name.</param>
        /// <param name="triggerDto">Trigger data.</param>
        /// <returns>Multiple different types of result based on success.</returns>
        [HttpPost("jobs/{uniqueName}/triggers")]
        public IActionResult AddTriggerForJobUniqueName(string uniqueName, [FromBody] JobTriggerDtoBase triggerDto)
        {
            var job = _queryService.GetJobByUniqueName(uniqueName);

            if (job == null)
            {
                return NotFound();
            }

            return AddTrigger(triggerDto, job);
        }

        private IActionResult AddTrigger(JobTriggerDtoBase triggerDto, Job job)
        {
            if (triggerDto == null)
            {
                return BadRequest();
            }

            if (triggerDto is InstantTriggerDto)
            {
                triggerDto.IsActive = true;
            }

            var trigger = TriggerMapper.ConvertToTrigger(triggerDto as dynamic);
            ((IJobTrigger)trigger).JobId = job.Id;

            _jobManagementService.AddTrigger(job.Id, trigger);

            return Created(string.Format("jobs/{0}/triggers/{1}", job.Id, trigger.Id), TriggerMapper.ConvertToDto(trigger));
        }
    }
}
