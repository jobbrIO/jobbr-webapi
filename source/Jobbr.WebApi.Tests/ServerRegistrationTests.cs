using Jobbr.Server;
using Jobbr.Server.Builder;
using Jobbr.Server.WebAPI;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;

namespace Jobbr.WebApi.Tests
{
    [TestClass]
    public class ServerRegistrationTests : IntegrationTestBase
    {
        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_WebServerIsAvailable()
        {
            using (GivenRunningServerWithWebApi())
            {
                var client = new HttpClient();
                var result = client.GetAsync(CreateUrl("status")).Result;

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            }
        }

        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_ConfigurationIsAvailable()
        {
            using (GivenRunningServerWithWebApi())
            {
                var client = new HttpClient();
                var result = client.GetAsync(CreateUrl("configuration")).Result;

                var response = result.Content.ReadAsStringAsync().Result;

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                Assert.IsTrue(response.Contains("{"), "There should be something looking like a serialize json object");

            }
        }

        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_ExceptionsDontAffectServer()
        {
            using (GivenRunningServerWithWebApi())
            {
                var client = new HttpClient();

                var faultyResult = client.GetAsync(CreateUrl("fail")).Result;
                Assert.AreEqual(HttpStatusCode.InternalServerError, faultyResult.StatusCode);

                var result = client.GetAsync(CreateUrl("status")).Result;

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            }
        }

        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_CanLoadSomeJobs()
        {
            using (GivenRunningServerWithWebApi())
            {
                var client = new HttpClient();

                var faultyResult = client.GetAsync(CreateUrl("jobs")).Result;
                Assert.AreEqual(HttpStatusCode.OK, faultyResult.StatusCode);
            }
        }

        [TestMethod]
        public void RegisteredAsComponent_WithoutConfiguration_DoesStart()
        {
            var builder = new JobbrBuilder(new NullLoggerFactory());
            builder.AddWebApi();

            var server = builder.Create();

            server.Start();

            using (server)
            {
                Assert.AreEqual(JobbrState.Running, server.State, "Server should be possible to start with default configuration");
            }
        }
    }
}
