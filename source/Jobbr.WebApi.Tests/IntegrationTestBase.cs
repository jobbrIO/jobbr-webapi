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

        protected JobbrServer GivenRunningServerWithWebApi()
        {
            var builder = new JobbrBuilder();

            var nextTcpPort = NextFreeTcpPort();
            this.BackendAddress = $"http://localhost:{nextTcpPort}";

            builder.AddWebApi(conf =>
            {
                conf.BackendAddress = this.BackendAddress;
            });

            builder.Register<IJobbrComponent>(typeof(ExposeStorageProvider));

            var server = builder.Create();

            server.Start();

            return server;
        }

        private static int NextFreeTcpPort()
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