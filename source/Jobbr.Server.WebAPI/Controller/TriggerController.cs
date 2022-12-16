using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.Server.WebAPI.Controller.Mapping;
using Jobbr.Server.WebAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace Jobbr.Server.WebAPI.Controller
{
    [ApiController]
    public class TriggerController : ControllerBase
    {
        private readonly IQueryService _queryService;
        private readonly IJobManagementService _jobManagementService;

        public TriggerController(IQueryService queryService, IJobManagementService jobManagementService)
        {
            _queryService = queryService;
            _jobManagementService = jobManagementService;
        }

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
                var trigger = TriggerMapper.ConvertToTrigger(recurringTriggerDto);
                trigger.Id = triggerId;
                trigger.JobId = jobId;

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
