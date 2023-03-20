using System;
using Jobbr.ComponentModel.Registration;
using Jobbr.Server.WebAPI.Infrastructure;

namespace Jobbr.Server.WebAPI
{
    /// <summary>
    /// Extension methods for <see cref="IJobbrBuilder"/>.
    /// </summary>
    public static class JobbrBuilderExtensions
    {
        /// <summary>
        /// Add Web API to builder without configurations.
        /// </summary>
        /// <param name="builder">Target builder.</param>
        public static void AddWebApi(this IJobbrBuilder builder)
        {
            builder.AddWebApi(_ => { });
        }

        /// <summary>
        /// Add Web API to builder with configurations.
        /// </summary>
        /// <param name="builder">Target builder.</param>
        public static void AddWebApi(this IJobbrBuilder builder, Action<JobbrWebApiConfiguration> config)
        {
            var customConfig = new JobbrWebApiConfiguration();

            config(customConfig);

            builder.Add<JobbrWebApiConfiguration>(customConfig);

            builder.RegisterForCollection<IJobbrComponent>(typeof(WebHost));
            builder.RegisterForCollection<IConfigurationValidator>(typeof(WebApiConfigurationValidator));
        }
    }
}
