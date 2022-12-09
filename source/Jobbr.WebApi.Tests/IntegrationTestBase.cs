using Jobbr.ComponentModel.JobStorage;
using Jobbr.ComponentModel.Registration;
using Jobbr.Server;
using Jobbr.Server.Builder;
using Jobbr.Server.WebAPI;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;
using System.Net.Sockets;

namespace Jobbr.WebApi.Tests
{
    public class IntegrationTestBase
    {
        public string BackendAddress { get; private set; }

        public IJobStorageProvider JobStorage => ExposeStorageProvider.Instance.JobStorageProvider;

        protected JobbrServer GivenRunningServerWithWebApi(string url = "")
        {
            var builder = new JobbrBuilder(new NullLoggerFactory());

            if (string.IsNullOrWhiteSpace(url))
            {
                var nextTcpPort = NextFreeTcpPort();
                this.BackendAddress = $"http://localhost:{nextTcpPort}";
            }
            else
            {
                this.BackendAddress = url;
            }

            builder.AddWebApi(conf =>
            {
                conf.BackendAddress = this.BackendAddress;
            });

            builder.Register<IJobbrComponent>(typeof(ExposeStorageProvider));

            var server = builder.Create();

            server.Start();

            return server;
        }

        public static int NextFreeTcpPort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            var port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        protected string CreateUrl(string path)
        {
            return $"{this.BackendAddress}/{path}";
        }
    }
}