using Jobbr.ComponentModel.Registration;

namespace Jobbr.Server.WebAPI
{
    /// <summary>
    /// Jobbr web API configuration.
    /// </summary>
    public class JobbrWebApiConfiguration : IFeatureConfiguration
    {
        /// <summary>
        /// Backend URL address.
        /// </summary>
        public string BackendAddress { get; set; }
    }
}