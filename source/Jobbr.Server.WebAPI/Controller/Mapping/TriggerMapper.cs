using System.Linq;
using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.Server.WebAPI.Model;
using Newtonsoft.Json;

namespace Jobbr.Server.WebAPI.Controller.Mapping
{
    public static class TriggerMapper
    {
        internal static ScheduledTriggerDto ConvertToDto(ScheduledTrigger trigger)
        {
            var dto = new ScheduledTriggerDto { StartDateTimeUtc = trigger.StartDateTimeUtc };
            return (ScheduledTriggerDto)MapCommonValues(trigger, dto);
        }

        internal static InstantTriggerDto ConvertToDto(InstantTrigger trigger)
        {
            var dto = new InstantTriggerDto { DelayedMinutes = trigger.DelayedMinutes };
            return (InstantTriggerDto)MapCommonValues(trigger, dto);
        }

        internal static RecurringTriggerDto ConvertToDto(RecurringTrigger trigger)
        {
            var dto = new RecurringTriggerDto { StartDateTimeUtc = trigger.StartDateTimeUtc, EndDateTimeUtc = trigger.EndDateTimeUtc, Definition = trigger.Definition };
            return (RecurringTriggerDto)MapCommonValues(trigger, dto);
        }

        internal static RecurringTrigger ConvertToTrigger(RecurringTriggerDto dto)
        {
            var trigger = new RecurringTrigger() { Definition = dto.Definition, StartDateTimeUtc = dto.StartDateTimeUtc, EndDateTimeUtc = dto.EndDateTimeUtc };
            return (RecurringTrigger)MapCommonValues(dto, trigger);
        }

        internal static ScheduledTrigger ConvertToTrigger(ScheduledTriggerDto dto)
        {
            var trigger = new ScheduledTrigger { StartDateTimeUtc = dto.StartDateTimeUtc };
            return (ScheduledTrigger)MapCommonValues(dto, trigger);
        }

        internal static InstantTrigger ConvertToTrigger(InstantTriggerDto dto)
        {
            var trigger = new InstantTrigger() { DelayedMinutes = dto.DelayedMinutes };
            return (InstantTrigger)MapCommonValues(dto, trigger);
        }

        internal static JobTriggerDtoBase MapCommonValues(IJobTrigger trigger, JobTriggerDtoBase dto)
        {
            dto.Id = trigger.Id;
            dto.Comment = trigger.Comment;
            dto.IsActive = trigger.IsActive;
            dto.Parameters = trigger.Parameters != null ? JsonConvert.DeserializeObject(trigger.Parameters) : null;
            dto.UserDisplayName = trigger.UserDisplayName;
            dto.UserId = trigger.UserId;

            return dto;
        }

        internal static IJobTrigger MapCommonValues(JobTriggerDtoBase dto, IJobTrigger trigger)
        {
            trigger.Comment = dto.Comment;
            trigger.IsActive = dto.IsActive;
            trigger.Parameters = JsonConvert.SerializeObject(dto.Parameters);
            trigger.UserDisplayName = dto.UserDisplayName;
            trigger.UserId = dto.UserId;

            return trigger;
        }

        internal static PagedResultDto<JobTriggerDtoBase> ToPagedResult(this PagedResult<IJobTrigger> data)
        {
            return new PagedResultDto<JobTriggerDtoBase>
            {
                Page = data.Page,
                PageSize = data.PageSize,
                Items = data.Items.Select(t => ConvertToDto((dynamic)t)).Cast<JobTriggerDtoBase>().ToList(),
                TotalItems = data.TotalItems
            };
        }
    }
}