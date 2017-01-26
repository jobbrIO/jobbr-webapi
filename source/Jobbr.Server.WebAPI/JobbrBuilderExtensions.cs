using System;
using Jobbr.ComponentModel.Registration;

namespace Jobbr.Server.WebAPI
{
    public static class JobbrBuilderExtensions
    {
        public static void AddWebApi(this IJobbrBuilder builder)
        {
            builder.AddWebApi(configuration => { } );
        }

        public static void AddWebApi(this IJobbrBuilder builder, Action<JobbrWebApiConfiguration> config)
        {
            var customConfig = new JobbrWebApiConfiguration();

            config(customConfig);

            builder.Add<JobbrWebApiConfiguration>(customConfig);

            builder.Register<IJobbrComponent>(typeof(WebHost));
            builder.Register<IConfigurationValidator>(typeof(WebApiConfigurationValidator));
        }
    }
}
