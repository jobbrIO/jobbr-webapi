using System;
using System.Collections.Generic;
using Jobbr.Server.WebAPI.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    /// <summary>
    /// Application startup.
    /// </summary>
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly JobbrWebApiConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">Web API configuration.</param>
        public Startup(ILoggerFactory loggerFactory, JobbrWebApiConfiguration configuration)
        {
            _loggerFactory = loggerFactory;
            _configuration = configuration;
        }

        /// <summary>
        /// Configure application services.
        /// </summary>
        /// <param name="services">Service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(o =>
            {
                o.Filters.Add(new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None });
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = DefaultJsonOptions.Options.PropertyNamingPolicy;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = DefaultJsonOptions.Options.PropertyNameCaseInsensitive;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = DefaultJsonOptions.Options.DefaultIgnoreCondition;
                    options.JsonSerializerOptions.Converters.Add(new JsonTypeConverter<JobTriggerDtoBase>(_loggerFactory, "TriggerType", JobTriggerTypeResolver));
                });
        }

        /// <summary>
        /// Configure application builder.
        /// </summary>
        /// <param name="app">Application builder.</param>
        public void Configure(IApplicationBuilder app)
        {
            app.UsePathBase(new Uri(_configuration.BackendAddress).AbsolutePath);

            app.UseRouting();

            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static Type JobTriggerTypeResolver(List<Type> types, string typeValue)
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