using System.Net;
using System.Net.Sockets;
using Jobbr.ComponentModel.JobStorage;
using Jobbr.ComponentModel.Registration;
using Jobbr.Server;
using Jobbr.Server.Builder;
using Jobbr.Server.WebAPI;

namespace Jobbr.WebApi.Tests
{
    public class IntegrationTestBase
    {
        public string BackendAddress { get; private set; }

        public IJobStorageProvider JobStorage => ExposeStorageProvider.Instance.JobStorageProvider;

        protected JobbrServer GivenRunningServerWithWebApi(string url = "")
        {
            var builder = new JobbrBuilder();

            string backendAddress;

            if (string.IsNullOrWhiteSpace(url))
            {
                var nextTcpPort = NextFreeTcpPort();
                backendAddress = $"http://localhost:{nextTcpPort}";
            }
            else
            {
                backendAddress = url;
            }

            builder.AddWebApi(conf =>
            {
                conf.BackendAddress = backendAddress;
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