using System;
using System.Net;
using System.Net.Sockets;
using Jobbr.ComponentModel.Registration;
using Jobbr.Server.WebAPI.Core;
using Jobbr.Server.WebAPI.Logging;

namespace Jobbr.Server.WebAPI
{
    internal class WebApiConfigurationValidator : IConfigurationValidator
    {
        private static readonly ILog Logger = LogProvider.For<WebHost>();

        public Type ConfigurationType { get; set; } = typeof(JobbrWebApiConfiguration);

        public bool Validate(object configuration)
        {
            var config = configuration as JobbrWebApiConfiguration;

            if (config == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(config.BackendAddress))
            {
                // Fallback to automatic endpoint port
                Logger.Warn("There was no BackendAdress specified. Falling back to random port, which is not guaranteed to work in production scenarios");
                var port = NextFreeTcpPort();

                config.BackendAddress = $"http://localhost:{port}/api";
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
