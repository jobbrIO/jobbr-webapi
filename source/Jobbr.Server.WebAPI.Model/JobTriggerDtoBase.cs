namespace Jobbr.Server.WebAPI.Model
{
    /// <summary>
    /// The job trigger base.
    /// </summary>
    public class JobTriggerDtoBase
    {
        public long Id { get; set; }

        public virtual string TriggerType { get; }

        public bool IsActive { get; set; }

        public object Parameters { get; set; }

        public string Comment { get; set; }

        public string UserId { get; set; }

        public string UserDisplayName { get; set; }

        public bool Deleted { get; set; }
    }
}
