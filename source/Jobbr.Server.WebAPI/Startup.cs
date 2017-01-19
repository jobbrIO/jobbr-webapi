using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Jobbr.ComponentModel.Registration;
using Jobbr.Server.WebAPI.App_Packages.LibLog._3._1;
using Jobbr.WebAPI.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Jobbr.Server.WebAPI
{
    public class Startup
    {
        private static readonly ILog Logger = LogProvider.For<WebHost>();

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
            config.Services.Add(typeof(IExceptionLogger), new TraceSourceExceptionLogger(Logger));

            // Controllers all have attributes
            config.MapHttpAttributeRoutes();

            // Serialization
            var jsonSerializerSettings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore };
            jsonSerializerSettings.Converters.Add(new JsonTypeConverter<JobTriggerDtoBase>("TriggerType", this.JobTriggerTypeResolver));
            config.Formatters.JsonFormatter.SerializerSettings = jsonSerializerSettings;

            // Remove XML response format
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            // Finally attach WebApi to the pipeline with the given configuration
            app.UseWebApi(config);
        }

        private Type JobTriggerTypeResolver(List<Type> types, string typeValue)
        {
            if (typeValue.ToLowerInvariant() == RecurringTriggerDto.TypeName.ToLowerInvariant())
            {
                return typeof(RecurringTriggerDto);
            }

            if (typeValue.ToLowerInvariant() == ScheduledTriggerDto.TypeName.ToLowerInvariant())
            {
                return typeof(ScheduledTriggerDto);
            }

            if (typeValue.ToLowerInvariant() == InstantTriggerDto.TypeName.ToLowerInvariant())
            {
                return typeof(InstantTriggerDto);
            }

            return null;
        }
    }
}
