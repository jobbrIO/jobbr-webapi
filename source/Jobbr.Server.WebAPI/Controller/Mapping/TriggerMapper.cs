using System.Linq;
using System.Text.Json;
using Jobbr.ComponentModel.Management;
using Jobbr.ComponentModel.Management.Model;
using Jobbr.Server.WebAPI.Model;

namespace Jobbr.Server.WebAPI.Controller.Mapping
{
    /// <summary>
    /// Object mapper for different objects related to triggers.
    /// </summary>
    public static class TriggerMapper
    {
        /// <summary>
        /// Maps <see cref="ScheduledTrigger"/> to a <see cref="ScheduledTriggerDto"/>.
        /// </summary>
        /// <param name="trigger">A <see cref="ScheduledTrigger"/> object.</param>
        /// <returns>A <see cref="ScheduledTriggerDto"/> object.</returns>
        internal static ScheduledTriggerDto ConvertToDto(ScheduledTrigger trigger)
        {
            var dto = new ScheduledTriggerDto { StartDateTimeUtc = trigger.StartDateTimeUtc };
            return (ScheduledTriggerDto)MapCommonValues(trigger, dto);
        }

        /// <summary>
        /// Maps <see cref="InstantTrigger"/> to a <see cref="InstantTriggerDto"/>.
        /// </summary>
        /// <param name="trigger">A <see cref="InstantTrigger"/> object.</param>
        /// <returns>A <see cref="InstantTriggerDto"/> object.</returns>
        internal static InstantTriggerDto ConvertToDto(InstantTrigger trigger)
        {
            var dto = new InstantTriggerDto { DelayedMinutes = trigger.DelayedMinutes };
            return (InstantTriggerDto)MapCommonValues(trigger, dto);
        }

        /// <summary>
        /// Maps <see cref="RecurringTrigger"/> to a <see cref="RecurringTriggerDto"/>.
        /// </summary>
        /// <param name="trigger">A <see cref="RecurringTrigger"/> object.</param>
        /// <returns>A <see cref="RecurringTriggerDto"/> object.</returns>
        internal static RecurringTriggerDto ConvertToDto(RecurringTrigger trigger)
        {
            var dto = new RecurringTriggerDto { StartDateTimeUtc = trigger.StartDateTimeUtc, EndDateTimeUtc = trigger.EndDateTimeUtc, Definition = trigger.Definition };
            return (RecurringTriggerDto)MapCommonValues(trigger, dto);
        }

        /// <summary>
        /// Maps <see cref="RecurringTriggerDto"/> to a <see cref="RecurringTrigger"/>.
        /// </summary>
        /// <param name="dto">A <see cref="RecurringTriggerDto"/> object.</param>
        /// <returns>A <see cref="RecurringTrigger"/> object.</returns>
        internal static RecurringTrigger ConvertToTrigger(RecurringTriggerDto dto)
        {
            var trigger = new RecurringTrigger { Definition = dto.Definition, StartDateTimeUtc = dto.StartDateTimeUtc, EndDateTimeUtc = dto.EndDateTimeUtc };
            return (RecurringTrigger)MapCommonValues(dto, trigger);
        }

        /// <summary>
        /// Maps <see cref="ScheduledTriggerDto"/> to a <see cref="ScheduledTrigger"/>.
        /// </summary>
        /// <param name="dto">A <see cref="ScheduledTriggerDto"/> object.</param>
        /// <returns>A <see cref="ScheduledTrigger"/> object.</returns>
        internal static ScheduledTrigger ConvertToTrigger(ScheduledTriggerDto dto)
        {
            var trigger = new ScheduledTrigger { StartDateTimeUtc = dto.StartDateTimeUtc };
            return (ScheduledTrigger)MapCommonValues(dto, trigger);
        }

        /// <summary>
        /// Maps <see cref="InstantTriggerDto"/> to a <see cref="InstantTrigger"/>.
        /// </summary>
        /// <param name="dto">A <see cref="InstantTriggerDto"/> object.</param>
        /// <returns>A <see cref="InstantTrigger"/> object.</returns>
        internal static InstantTrigger ConvertToTrigger(InstantTriggerDto dto)
        {
            var trigger = new InstantTrigger { DelayedMinutes = dto.DelayedMinutes };
            return (InstantTrigger)MapCommonValues(dto, trigger);
        }

        /// <summary>
        /// Maps common trigger values to a paged DTO result.
        /// </summary>
        /// <param name="data">Paged result of trigger that use the <see cref="IJobTrigger"/> interface.</param>
        /// <returns>Paged result of the base class with the trigger values.</returns>
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

        /// <summary>
        /// Maps common trigger values.
        /// </summary>
        /// <param name="trigger">Trigger that uses the <see cref="IJobTrigger"/> interface.</param>
        /// <param name="dto">Base class <see cref="JobTriggerDtoBase"/>.</param>
        /// <returns>Base class with the mapped values.</returns>
        private static JobTriggerDtoBase MapCommonValues(IJobTrigger trigger, JobTriggerDtoBase dto)
        {
            dto.Id = trigger.Id;
            dto.Comment = trigger.Comment;
            dto.IsActive = trigger.IsActive;
            dto.Parameters = trigger.Parameters != null ? JsonSerializer.Deserialize<JobTriggerDtoBase>(trigger.Parameters, DefaultJsonOptions.Options) : null;
            dto.UserDisplayName = trigger.UserDisplayName;
            dto.UserId = trigger.UserId;
            dto.Deleted = trigger.Deleted;

            return dto;
        }

        /// <summary>
        /// Maps common trigger values.
        /// </summary>
        /// <param name="dto">Base class <see cref="JobTriggerDtoBase"/>.</param>
        /// <param name="trigger">Trigger that uses the <see cref="IJobTrigger"/> interface.</param>
        /// <returns>Trigger class with the mapped values.</returns>
        private static IJobTrigger MapCommonValues(JobTriggerDtoBase dto, IJobTrigger trigger)
        {
            trigger.Comment = dto.Comment;
            trigger.IsActive = dto.IsActive;
            trigger.Parameters = trigger.Parameters is null ? null : JsonSerializer.Serialize(dto.Parameters, DefaultJsonOptions.Options);
            trigger.UserDisplayName = dto.UserDisplayName;
            trigger.UserId = dto.UserId;
            trigger.Deleted = dto.Deleted;

            return trigger;
        }
    }
}