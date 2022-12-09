using Jobbr.ComponentModel.Registration;
using Jobbr.Server.WebAPI.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    public class Startup
    {
        /// <summary>
        /// The dependency resolver from the JobbrServer which needs to be passed through the OWIN stack to WebAPI
        /// </summary>
        private readonly IJobbrServiceProvider dependencyResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="serviceProvider">
        /// The dependency resolver.
        /// </param>
        public Startup(IJobbrServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentException("Please provide a service provider. See http://servercoredump.com/question/27246240/inject-current-user-owin-host-web-api-service for details", "serviceProvider");
            }

            this.dependencyResolver = serviceProvider;
        }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            // Set the resolved to the service provider that gets injected when constructing this component
            config.DependencyResolver = new DependencyResolverAdapter(this.dependencyResolver);

            // Add trace logger for exceptions
            config.Services.Add(typeof(IExceptionLogger), new TraceSourceExceptionLogger(new LoggerFactory()));

            // Controllers all have attributes
            config.MapHttpAttributeRoutes();

            // Prevent IE from caching GET requests
            config.Filters.Add(new DontCacheGetRequests());

            // Serialization
            var jsonSerializerSettings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore };
            jsonSerializerSettings.Converters.Add(new JsonTypeConverter<JobTriggerDtoBase>("TriggerType", this.JobTriggerTypeResolver));
            config.Formatters.JsonFormatter.SerializerSettings = jsonSerializerSettings;

            // Remove XML response format
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            // enable cors
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            // Finally attach WebApi to the pipeline with the given configuration
            app.UseWebApi(config);
            app.UseCors(CorsOptions.AllowAll);
        }

        private Type JobTriggerTypeResolver(List<Type> types, string typeValue)
        {
            if (typeValue.ToLowerInvariant() == RecurringTriggerDto.Type.ToLowerInvariant())
            {
                return typeof(RecurringTriggerDto);
            }

            if (typeValue.ToLowerInvariant() == ScheduledTriggerDto.Type.ToLowerInvariant())
            {
                return typeof(ScheduledTriggerDto);
            }

            if (typeValue.ToLowerInvariant() == InstantTriggerDto.Type.ToLowerInvariant())
            {
                return typeof(InstantTriggerDto);
            }

            return null;
        }
    }
}
