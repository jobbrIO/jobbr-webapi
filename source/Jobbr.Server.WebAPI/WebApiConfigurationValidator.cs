using Jobbr.ComponentModel.Registration;
using System;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace Jobbr.Server.WebAPI
{
    internal class WebApiConfigurationValidator : IConfigurationValidator
    {
        private readonly ILogger<WebApiConfigurationValidator> _logger;

        public Type ConfigurationType { get; set; } = typeof(JobbrWebApiConfiguration);

        public WebApiConfigurationValidator(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WebApiConfigurationValidator>();
        }

        public bool Validate(object configuration)
        {
            var config = (JobbrWebApiConfiguration)configuration;

            if (config == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(config.BackendAddress))
            {
                // Fallback to automatic endpoint port
                _logger.LogWarning("There was no BackendAdress specified. Falling back to random port, which is not guaranteed to work in production scenarios");
                var port = NextFreeTcpPort();

                config.BackendAddress = $"http://localhost:{port}/";
            }

            return true;
        }

        private static int NextFreeTcpPort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            var port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}
