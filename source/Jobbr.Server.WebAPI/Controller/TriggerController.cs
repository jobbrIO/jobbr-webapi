using System;
using System.Net;
using System.Web.Http;
using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.Server.WebAPI.Controller.Mapping;
using Jobbr.Server.WebAPI.Model;

namespace Jobbr.Server.WebAPI.Controller
{
    public class TriggerController : ApiController
    {
        private readonly IQueryService queryService;
        private readonly IJobManagementService jobManagementService;

        public TriggerController(IQueryService queryService, IJobManagementService jobManagementService)
        {
            this.queryService = queryService;
            this.jobManagementService = jobManagementService;
        }

        [HttpGet]
        [Route("jobs/{jobId:long}/triggers/{triggerId:long}")]
        public IHttpActionResult GetTriggerById(long jobId, long triggerId)
        {
            var trigger = this.queryService.GetTriggerById(jobId, triggerId);

            if (trigger == null)
            {
                return this.NotFound();
            }

            return this.Ok(TriggerMapper.ConvertToDto((dynamic)trigger));
        }

        [HttpPatch]
        [Route("jobs/{jobId:long}/triggers/{triggerId:long}")]
        public IHttpActionResult UpdateTrigger(long jobId, long triggerId, [FromBody] JobTriggerDtoBase dto)
        {
            var currentTrigger = this.queryService.GetTriggerById(jobId, triggerId);

            if (currentTrigger == null)
            {
                return this.NotFound();
            }

            if (currentTrigger.IsActive && !dto.IsActive)
            {
                currentTrigger.IsActive = false;
                this.jobManagementService.DisableTrigger(jobId, currentTrigger.Id);
            }
            else if (!currentTrigger.IsActive && dto.IsActive)
            {
                currentTrigger.IsActive = true;
                this.jobManagementService.EnableTrigger(jobId, currentTrigger.Id);
            }

            if (dto is RecurringTriggerDto recurringTriggerDto)
            {
                var trigger = TriggerMapper.ConvertToTrigger(recurringTriggerDto);
                trigger.Id = triggerId;
                trigger.JobId = jobId;

                this.jobManagementService.Update(trigger);
            }
            else if (dto is ScheduledTriggerDto scheduledTriggerDto)
            {
                var trigger = TriggerMapper.ConvertToTrigger(scheduledTriggerDto);
                trigger.Id = trigger.Id;
                trigger.JobId = trigger.JobId;

                this.jobManagementService.Update(trigger);
            }

            return this.Ok(dto);
        }

        [HttpGet]
        [Route("jobs/{jobId:long}/triggers")]
        public IHttpActionResult GetTriggersForJob(long jobId, int page = 1, int pageSize = 200)
        {
            var job = this.queryService.GetJobById(jobId);

            if (job == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.queryService.GetTriggersByJobId(jobId, page, pageSize));
        }

        [HttpGet]
        [Route("jobs/{uniqueName}/triggers")]
        public IHttpActionResult GetTriggersForJob(string uniqueName, int page = 1, int pageSize = 200)
        {
            var job = this.queryService.GetJobByUniqueName(uniqueName);

            if (job == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.queryService.GetTriggersByJobId(job.Id, page, pageSize));
        }

        [HttpPost]
        [Route("jobs/{jobId:long}/triggers")]
        public IHttpActionResult AddTriggerForJobId(long jobId, [FromBody] JobTriggerDtoBase triggerDto)
        {
            var job = this.queryService.GetJobById(jobId);

            if (job == null)
            {
                return this.NotFound();
            }

            return this.AddTrigger(triggerDto, job);
        }

        [HttpPost]
        [Route("jobs/{uniqueName}/triggers")]
        public IHttpActionResult AddTriggerForJobUniqueName(string uniqueName, [FromBody] JobTriggerDtoBase triggerDto)
        {
            var job = this.queryService.GetJobByUniqueName(uniqueName);

            if (job == null)
            {
                return this.NotFound();
            }

            return this.AddTrigger(triggerDto, job);
        }

        private IHttpActionResult AddTrigger(JobTriggerDtoBase triggerDto, Job job)
        {
            if (triggerDto == null)
            {
                return this.StatusCode(HttpStatusCode.BadRequest);
            }

            if (triggerDto is InstantTriggerDto)
            {
                triggerDto.IsActive = true;
            }

            var trigger = TriggerMapper.ConvertToTrigger(triggerDto as dynamic);
            ((IJobTrigger)trigger).JobId = job.Id;

            this.jobManagementService.AddTrigger(job.Id, trigger);

            return this.Created(string.Format("jobs/{0}/triggers/{1}", job.Id, trigger.Id), TriggerMapper.ConvertToDto(trigger));
        }
    }
}
