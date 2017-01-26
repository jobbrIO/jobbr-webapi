using System;
using Jobbr.ComponentModel.Registration;

namespace Jobbr.Server.WebAPI
{
    public static class JobbrBuilderExtensions
    {
        public static void AddWebApi(this IJobbrBuilder builder)
        {
            var config = new JobbrWebApiConfiguration()
            {
                BackendAddress = "http://localhost:80/jobbr"
            };

            AddWebApi(builder, config);
        }

        public static void AddWebApi(IJobbrBuilder builder, JobbrWebApiConfiguration config)
        {
            builder.Add<JobbrWebApiConfiguration>(config);

            builder.Register<IJobbrComponent>(typeof(WebHost));
        }

        public static void AddWebApi(this IJobbrBuilder builder, Action<JobbrWebApiConfiguration> config)
        {
            var customConfig = new JobbrWebApiConfiguration();

            config(customConfig);

            AddWebApi(builder, customConfig);
        }
    }
}
