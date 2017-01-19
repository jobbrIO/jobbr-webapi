namespace Jobbr.Server.WebAPI
{
    public interface IJobbrWebApiConfiguration
    {
        string BackendAddress { get; }
    }

    public class JobbrWebApiConfiguration : IJobbrWebApiConfiguration
    {
        public string BackendAddress { get; set; }
    }
}