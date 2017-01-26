using Jobbr.ComponentModel.Registration;

namespace Jobbr.Server.WebAPI
{
    public class JobbrWebApiConfiguration : IFeatureConfiguration
    {
        public string BackendAddress { get; set; }
    }
}