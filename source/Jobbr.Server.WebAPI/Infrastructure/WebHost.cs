using System;
using System.Threading.Tasks;
using Jobbr.ComponentModel.Registration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    /// <summary>
    /// The web host.
    /// </summary>
    public class WebHost : IJobbrComponent
    {
        private readonly ILogger _logger;
        private readonly InstanceProducer[] _serviceCollection;
        private readonly JobbrWebApiConfiguration _configuration;

        private IWebHost _webHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebHost"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="container">The service injector container.</param>
        /// <param name="configuration">Web API configuration.</param>
        public WebHost(ILoggerFactory loggerFactory, Container container, JobbrWebApiConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<WebHost>();
            _serviceCollection = container.GetCurrentRegistrations();
            _configuration = configuration;
        }

        /// <summary>
        /// Start the web host.
        /// </summary>
        public void Start()
        {
            if (_webHost != null)
            {
                throw new InvalidOperationException("The server has already been started.");
            }

            AssertBackendAddressIsValid();

            _webHost = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(new Uri(_configuration.BackendAddress).GetLeftPart(UriPartial.Authority))
                .ConfigureServices(services =>
                {
                    foreach (var instanceProducer in _serviceCollection)
                    {
                        services.Add(new ServiceDescriptor(instanceProducer.ServiceType, instanceProducer.GetInstance()));
                    }
                })
                .UseStartup<Startup>()
                .Build();

            _webHost.Start();

            _logger.LogInformation("Started web host for WebAPI at '{backendAddress}'.", _configuration.BackendAddress);
        }

        /// <summary>
        /// Stop web host.
        /// </summary>
        public void Stop()
        {
            Task.FromResult(_webHost.StopAsync());
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Conditional dispose.
        /// </summary>
        /// <param name="disposing">If should be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Task.Run(async () => await _webHost.StopAsync());
                _webHost?.Dispose();
            }
        }

        private void AssertBackendAddressIsValid()
        {
            if (string.IsNullOrWhiteSpace(_configuration.BackendAddress))
            {
                throw new ArgumentException("Unable to start WebServer when no BackendUrl is specified.");
            }

            var uri = new Uri(_configuration.BackendAddress);
            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            {
                throw new FormatException("No valid UriScheme was given. Please provide a scheme like http(s)");
            }
        }
    }
}