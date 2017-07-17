using System.Net;
using System.Net.Http;
using Jobbr.Server;
using Jobbr.Server.Builder;
using Jobbr.Server.WebAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jobbr.WebApi.Tests
{
    [TestClass]
    public class ServerRegistrationTests : IntegrationTestBase
    {
        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_WebServerIsAvailable()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new HttpClient();
                var result = client.GetAsync(this.CreateUrl("status")).Result;

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            }
        }

        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_ConfigurationIsAvailable()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new HttpClient();
                var result = client.GetAsync(this.CreateUrl("configuration")).Result;

                var response = result.Content.ReadAsStringAsync().Result;

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                Assert.IsTrue(response.Contains("{"), "There should be something looking like a serialize json object");

            }
        }

        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_ExceptionsDontAffectServer()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new HttpClient();

                var faultyResult = client.GetAsync(this.CreateUrl("fail")).Result;
                Assert.AreEqual(HttpStatusCode.InternalServerError, faultyResult.StatusCode);

                var result = client.GetAsync(this.CreateUrl("status")).Result;

                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            }
        }

        [TestMethod]
        public void RegisteredAsComponent_JobbrIsStarted_CanLoadSomeJobs()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new HttpClient();

                var faultyResult = client.GetAsync(this.CreateUrl("jobs")).Result;
                Assert.AreEqual(HttpStatusCode.OK, faultyResult.StatusCode);
            }
        }

        [TestMethod]
        public void RegisteredAsComponent_WithoutConfiguration_DoesStart()
        {
            var builder = new JobbrBuilder();
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
