using Jobbr.Server.WebAPI.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly JobbrWebApiConfiguration _configuration;

        public Startup(ILoggerFactory loggerFactory, JobbrWebApiConfiguration configuration)
        {
            _loggerFactory = loggerFactory;
            _configuration = configuration;
        }

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

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", config => config
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UsePathBase(new Uri(_configuration.BackendAddress).AbsolutePath);

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors();
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