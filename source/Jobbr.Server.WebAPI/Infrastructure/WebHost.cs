using Jobbr.ComponentModel.Registration;
using Jobbr.Server.WebAPI.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    /// <summary>
    /// The web host.
    /// </summary>
    public class WebHost : IJobbrComponent
    {
        private WebApplication _webApp;
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly InstanceProducer[] _serviceCollection;
        private readonly JobbrWebApiConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebHost"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="container">The service injector container.</param>
        /// <param name="configuration">Web API configuration.</param>
        public WebHost(ILoggerFactory loggerFactory, Container container, JobbrWebApiConfiguration configuration)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<WebHost>();
            _serviceCollection = container.GetCurrentRegistrations();
            _configuration = configuration;
        }

        /// <summary>
        /// Start the web host.
        /// </summary>
        public void Start()
        {
            if (_webApp != null)
            {
                throw new InvalidOperationException("The server has already been started.");
            }

            AssertBackendAddressIsValid();

            var builder = WebApplication.CreateBuilder();

            foreach (var instanceProducer in _serviceCollection)
            {
                builder.Services.Add(new ServiceDescriptor(instanceProducer.ServiceType, instanceProducer.GetInstance()));
            }

            builder.Services.AddMvc(o =>
            {
                o.Filters.Add(new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None });
            });

            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = DefaultJsonOptions.Options.PropertyNamingPolicy;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = DefaultJsonOptions.Options.PropertyNameCaseInsensitive;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = DefaultJsonOptions.Options.DefaultIgnoreCondition;
                    options.JsonSerializerOptions.Converters.Add(new JsonTypeConverter<JobTriggerDtoBase>(_loggerFactory, "TriggerType", JobTriggerTypeResolver));
                })
                .AddApplicationPart(typeof(WebHost).Assembly);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", config => config
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            _webApp = builder.Build();

            _webApp.MapControllers();

            var backendAddressUri = new Uri(_configuration.BackendAddress);
            _webApp.UsePathBase(backendAddressUri.AbsolutePath);
            _webApp.Urls.Add(backendAddressUri.GetLeftPart(UriPartial.Authority));
            _webApp.UseRouting();

            _webApp.UseCors();

            Task.FromResult(_webApp.StartAsync());

            _logger.LogInformation("Started web host for WebAPI at '{backendAddress}'.", _configuration.BackendAddress);
        }

        public void Stop()
        {
            Task.FromResult(_webApp.StopAsync());
        }

        public void Dispose()
        {
            Task.FromResult(_webApp.StopAsync());
            Task.FromResult(_webApp.DisposeAsync());
        }

        private void AssertBackendAddressIsValid()
        {
            if (string.IsNullOrWhiteSpace(_configuration.BackendAddress))
            {
                throw new ArgumentException("Unable to start WebServer when no BackendUrl is specified.");
            }
            var uri = new Uri(_configuration.BackendAddress);
            if (uri.Scheme != Uri.UriSchemeHttp &&
                uri.Scheme != Uri.UriSchemeHttps)
            {
                throw new FormatException("No valid UriScheme was given. Please provide a scheme like http(s)");
            }
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