using System.Net;
using System.Net.Http;
using Jobbr.Server;
using Jobbr.Server.Builder;
using Jobbr.Server.WebAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jobbr.WebApi.Tests
{
    [TestClass]
    public class ServerTests
    {
        private static JobbrServer GivenARunningServerWithWebApi()
        {
            var builder = new JobbrBuilder();
            builder.AddWebApi();
            var server = builder.Create();

            server.Start();

            return server;
        }

        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_WebServerIsAvailable()
        {
            using (GivenARunningServerWithWebApi())
            {
                var client = new HttpClient();
                var result = client.GetAsync("http://localhost/jobbr/api/status").Result;

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            }
        }

        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_ConfigurationIsAvailable()
        {
            using (GivenARunningServerWithWebApi())
            {
                var client = new HttpClient();
                var result = client.GetAsync("http://localhost/jobbr/api/configuration").Result;

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

                var faultyResult = client.GetAsync("http://localhost/jobbr/api/fail").Result;
                Assert.AreEqual(HttpStatusCode.InternalServerError, faultyResult.StatusCode);

                var result = client.GetAsync("http://localhost/jobbr/api/status").Result;

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            }
        }

        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_CanLoadDependencies()
        {
            using (GivenARunningServerWithWebApi())
            {
                var client = new HttpClient();

                var faultyResult = client.GetAsync("http://localhost/jobbr/api/jobs").Result;
                Assert.AreEqual(HttpStatusCode.OK, faultyResult.StatusCode);
            }
        }
    }
}
