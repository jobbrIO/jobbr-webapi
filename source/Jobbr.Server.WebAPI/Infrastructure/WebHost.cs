using Jobbr.ComponentModel.Registration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    /// <summary>
    /// The web host.
    /// </summary>
    public class WebHost : IJobbrComponent
    {
        private WebApplication _webApp;
        private readonly ILogger _logger;
        private readonly IServiceCollection _serviceCollection;
        private readonly JobbrWebApiConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebHost"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="configuration">Web API configuration.</param>
        public WebHost(ILoggerFactory loggerFactory, IServiceCollection serviceCollection, JobbrWebApiConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<WebHost>();
            _logger.LogDebug("Constructing new WebHost");

            _serviceCollection = serviceCollection;
            _configuration = configuration;
        }

        /// <summary>
        /// Start the web host.
        /// </summary>
        public void Start()
        {
            _logger.LogDebug("Starting new WebHost");
            if (_webApp != null)
            {
                throw new InvalidOperationException("The server has already been started.");
            }

            AssertBackendAddressIsValid();

            var builder = WebApplication.CreateBuilder();

            foreach (var service in _serviceCollection)
            {
                builder.Services.Add(service);
            }

            //config.Filters.Add(new DontCacheGetRequests()); 
            //add others from startup

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", config => config
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            
            builder.Services
                .AddControllers()
                .AddApplicationPart(typeof(WebHost).Assembly);

            _webApp = builder.Build();
            _webApp.MapControllers();
            _webApp.Urls.Add(_configuration.BackendAddress);
            _webApp.UseCors();

            Task.FromResult(_webApp.StartAsync());

            _logger.LogInformation($"Started web host for WebAPI at '{_configuration.BackendAddress}'.");
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
    }
}