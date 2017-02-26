using System;
using System.Net;
using System.Web.Http;
using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.Server.WebAPI.Core.Mapping;
using Jobbr.WebAPI.Common.Models;

namespace Jobbr.Server.WebAPI.Core.Controller
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
        [Route("api/triggers/{triggerId}")]
        public IHttpActionResult GetTriggerById(long triggerId)
        {
            var trigger = this.queryService.GetTriggerById(triggerId);

            if (trigger == null)
            {
                return this.NotFound();
            }

            return this.Ok(TriggerMapper.ConvertToDto((dynamic)trigger));
        }

        [HttpPatch]
        [Route("api/triggers/{triggerId}")]
        public IHttpActionResult UpdateTrigger(long triggerId, [FromBody] JobTriggerDtoBase dto)
        {
            var currentTrigger = this.queryService.GetTriggerById(triggerId);

            if (currentTrigger == null)
            {
                return this.NotFound();
            }

            bool hadChanges = false;
            if (currentTrigger.IsActive && !dto.IsActive)
            {
                currentTrigger.IsActive = false;
                this.jobManagementService.DisableTrigger(currentTrigger.Id);
                hadChanges = true;
            }
            else if (!currentTrigger.IsActive && dto.IsActive)
            {
                currentTrigger.IsActive = true;
                this.jobManagementService.EnableTrigger(currentTrigger.Id);
                hadChanges = true;
            }

            var recurringTriggerDto = dto as RecurringTriggerDto;
            if (recurringTriggerDto != null && !string.IsNullOrEmpty(recurringTriggerDto.Definition) && recurringTriggerDto.Definition != ((RecurringTrigger)currentTrigger).Definition)
            {
                ((RecurringTrigger)currentTrigger).Definition = recurringTriggerDto.Definition;
                this.jobManagementService.UpdateTriggerDefinition(currentTrigger.Id, recurringTriggerDto.Definition);
                
                hadChanges = true;
            }

            var scheduledTriggerDto = dto as ScheduledTriggerDto;
            if (scheduledTriggerDto != null && scheduledTriggerDto.StartDateTimeUtc >= DateTime.UtcNow && scheduledTriggerDto.StartDateTimeUtc != ((ScheduledTrigger)currentTrigger).StartDateTimeUtc)
            {
                ((ScheduledTrigger)currentTrigger).StartDateTimeUtc = scheduledTriggerDto.StartDateTimeUtc;
                this.jobManagementService.UpdateTriggerStartTime(currentTrigger.Id, scheduledTriggerDto.StartDateTimeUtc);

                hadChanges = true;
            }

            if (hadChanges)
            {
                // Reload trigger
                currentTrigger = this.queryService.GetTriggerById(triggerId);

                return this.Ok(TriggerMapper.ConvertToDto((dynamic)currentTrigger));
            }
            
            return this.StatusCode(HttpStatusCode.NotModified);
        }

        [HttpGet]
        [Route("api/jobs/{jobId:long}/trigger")]
        public IHttpActionResult GetTriggersForJob(long jobId)
        {
            var job = this.queryService.GetJobById(jobId);

            if (job == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.queryService.GetTriggersByJobId(jobId));
        }

        [HttpGet]
        [Route("api/jobs/{uniqueName}/trigger")]
        public IHttpActionResult GetTriggersForJob(string uniqueName)
        {
            var job = this.queryService.GetJobByUniqueName(uniqueName);

            if (job == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.queryService.GetTriggersByJobId(job.Id));
        }

        [HttpPost]
        [Route("api/jobs/{jobId:long}/trigger")]
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
        [Route("api/jobs/{uniqueName}/trigger")]
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

            var triggerId = this.jobManagementService.AddTrigger(trigger);

            return this.Created(string.Format("api/trigger/{0}", triggerId), TriggerMapper.ConvertToDto(trigger));
        }
    }
}
