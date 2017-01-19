using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using Jobbr.Server;
using Jobbr.Server.Builder;
using Jobbr.Server.WebAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jobbr.WebApi.Tests
{
    [TestClass]
    public class ServerTests
    {
        private static string confBackendAddress;

        private static JobbrServer GivenARunningServerWithWebApi()
        {
            var builder = new JobbrBuilder();

            var nextTcpPort = NextFreeTcpPort();
            confBackendAddress = $"http://localhost:{nextTcpPort}";

            builder.AddWebApi(conf =>
            {
                conf.BackendAddress = confBackendAddress;
            });

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

        private static string CreateUrl(string path)
        {
            return $"{confBackendAddress}/{path}";
        }

        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_WebServerIsAvailable()
        {
            using (GivenARunningServerWithWebApi())
            {
                var client = new HttpClient();
                var result = client.GetAsync(CreateUrl("api/status")).Result;

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            }
        }

        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_ConfigurationIsAvailable()
        {
            using (GivenARunningServerWithWebApi())
            {
                var client = new HttpClient();
                var result = client.GetAsync(CreateUrl("api/configuration")).Result;

                var response = result.Content.ReadAsStringAsync().Result;

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                Assert.IsTrue(response.Contains("{"), "There should be something looking like a serialize json object");

            }
        }

        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_ExceptionsDontAffectServer()
        {
            using (GivenARunningServerWithWebApi())
            {
                var client = new HttpClient();

                var faultyResult = client.GetAsync(CreateUrl("api/fail")).Result;
                Assert.AreEqual(HttpStatusCode.InternalServerError, faultyResult.StatusCode);

                var result = client.GetAsync(CreateUrl("api/status")).Result;

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            }
        }

        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_CanLoadSomeJobs()
        {
            using (GivenARunningServerWithWebApi())
            {
                var client = new HttpClient();

                var faultyResult = client.GetAsync(CreateUrl("api/jobs")).Result;
                Assert.AreEqual(HttpStatusCode.OK, faultyResult.StatusCode);
            }
        }
    }
}
