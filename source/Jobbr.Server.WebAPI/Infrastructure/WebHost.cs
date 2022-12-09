using Jobbr.ComponentModel.Registration;
using Microsoft.Extensions.Logging;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Hosting.Services;
using Microsoft.Owin.Hosting.Starter;
using System;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    /// <summary>
    /// The web host.
    /// </summary>
    public class WebHost : IJobbrComponent
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<WebHost> _logger;

        /// <summary>
        /// The dependency resolver.
        /// </summary>
        private readonly IJobbrServiceProvider dependencyResolver;

        /// <summary>
        /// The configuration for this component
        /// </summary>
        private readonly JobbrWebApiConfiguration configuration;

        /// <summary>
        /// The webhost that serves for the OWIN WebAPI component
        /// </summary>
        private IDisposable webHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebHost"/> class.
        /// </summary>
        public WebHost(IJobbrServiceProvider dependencyResolver, ILoggerFactory loggerFactory, JobbrWebApiConfiguration configuration)
        {
            this.dependencyResolver = dependencyResolver;
            this.configuration = configuration;
            this._logger = loggerFactory.CreateLogger<WebHost>();
        }

        /// <summary>
        /// Start the component
        /// </summary>
        public void Start()
        {
            this.AssertBackendAddressIsValid();

            var services = (ServiceProvider)ServicesFactory.Create();
            var options = new StartOptions()
            {
                Urls = { this.configuration.BackendAddress },
                AppStartup = typeof(Startup).FullName
            };
            // Pass through the IJobbrServiceProvider to allow Startup-Classes to let them inject this dependency
            services.Add(typeof(IJobbrServiceProvider), () => this.dependencyResolver);

            var hostingStarter = services.GetService<IHostingStarter>();
            this.webHost = hostingStarter.Start(options);

            _logger.LogInformation($"Started OWIN-Host for WebAPI at '{this.configuration.BackendAddress}'.");
        }

        public void Stop()
        {
            this._logger.LogInformation("Stopping OWIN-Host for Web-Endpoints'");

            this.webHost?.Dispose();

            this.webHost = null;
        }

        public void Dispose()
        {
            this.webHost.Dispose();
            this.webHost = null;
        }

        private void AssertBackendAddressIsValid()
        {
            if (string.IsNullOrWhiteSpace(this.configuration.BackendAddress))
            {
                throw new ArgumentException("Unable to start WebServer when no BackendUrl is specified.");
            }
            var uri = new Uri(this.configuration.BackendAddress);
            if (uri.Scheme != Uri.UriSchemeHttp &&
                uri.Scheme != Uri.UriSchemeHttps)
            {
                throw new FormatException("No valid UriScheme was given. Please provide a scheme like http(s)");
            }
        }
    }
}