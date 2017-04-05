using Jobbr.Client;
using Jobbr.ComponentModel.JobStorage.Model;
using Jobbr.WebAPI.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jobbr.WebApi.Tests
{
    [TestClass]
    public class ClientTests : IntegrationTestBase
    {
        [TestMethod]
        public void RetrievingJobById()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                var job = new Job();
                this.JobStorage.AddJob(job);

                var jobDto = client.GetJob(job.Id);

                Assert.IsTrue(job.Id > 0);
                Assert.AreEqual(job.Id, jobDto.Id);
            }
        }

        [TestMethod]
        public void RetrievingAllJobs()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                this.JobStorage.AddJob(new Job());
                this.JobStorage.AddJob(new Job());
                this.JobStorage.AddJob(new Job());
                this.JobStorage.AddJob(new Job());
                this.JobStorage.AddJob(new Job());

                var jobs = client.GetAllJobs();

                Assert.AreEqual(5, jobs.Count);
            }
        }

        [TestMethod]
        public void GetInstantTrigger()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                var job = new Job();
                this.JobStorage.AddJob(job);

                var trigger = new InstantTrigger();
                this.JobStorage.AddTrigger(job.Id, trigger);

                var triggerDto = client.GetTriggerById<InstantTriggerDto>(job.Id, trigger.Id);

                Assert.IsNotNull(triggerDto);
                Assert.AreEqual(trigger.Id, triggerDto.Id);
            }
        }

        [TestMethod]
        public void GetScheduledTrigger()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                var job = new Job();
                this.JobStorage.AddJob(job);

                var trigger = new ScheduledTrigger();
                this.JobStorage.AddTrigger(job.Id, trigger);

                var triggerDto = client.GetTriggerById<ScheduledTriggerDto>(job.Id, trigger.Id);

                Assert.IsNotNull(triggerDto);
                Assert.AreEqual(trigger.Id, triggerDto.Id);
            }
        }

        [TestMethod]
        public void GetRecurringTrigger()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                var job = new Job();
                this.JobStorage.AddJob(job);

                var trigger = new RecurringTrigger();
                this.JobStorage.AddTrigger(job.Id, trigger);

                var triggerDto = client.GetTriggerById<RecurringTriggerDto>(job.Id, trigger.Id);

                Assert.IsNotNull(triggerDto);
                Assert.AreEqual(trigger.Id, triggerDto.Id);
            }
        }

        [TestMethod]
        public void AddInstantTrigger()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                var job = new Job();
                this.JobStorage.AddJob(job);

                var triggerDto = client.AddTrigger(job.Id, new InstantTriggerDto {Comment = "test"});

                Assert.IsNotNull(triggerDto);
                Assert.IsTrue(triggerDto.Id > 0);
            }
        }

        [TestMethod]
        public void AddScheduledTrigger()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                var job = new Job();
                this.JobStorage.AddJob(job);

                var triggerDto = client.AddTrigger(job.Id, new ScheduledTriggerDto { Comment = "test" });

                Assert.IsNotNull(triggerDto);
                Assert.IsTrue(triggerDto.Id > 0);
            }
        }

        [TestMethod]
        public void AddRecurringTrigger()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                var job = new Job();
                this.JobStorage.AddJob(job);

                var triggerDto = client.AddTrigger(job.Id, new RecurringTriggerDto { Comment = "test" });
                
                Assert.IsNotNull(triggerDto);
                Assert.IsTrue(triggerDto.Id > 0);
            }
        }

        [TestMethod]
        public void GetJobRunById()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                var job = new Job();
                this.JobStorage.AddJob(job);

                var trigger = new RecurringTrigger();
                this.JobStorage.AddTrigger(job.Id, trigger);

                var jobRun = new JobRun {JobId = job.Id, TriggerId = trigger.Id};
                this.JobStorage.AddJobRun(jobRun);

                var jobRunDto = client.GetJobRunById(jobRun.Id);

                Assert.IsNotNull(jobRunDto);
                Assert.AreEqual(jobRun.Id, jobRunDto.JobRunId);
                Assert.AreEqual(jobRun.JobId, jobRunDto.JobId);
                Assert.AreEqual(jobRun.TriggerId, jobRunDto.TriggerId);
            }
        }

        [TestMethod]
        public void GetJobRunsByTriggerId()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                var job = new Job();
                this.JobStorage.AddJob(job);

                var trigger = new RecurringTrigger();
                this.JobStorage.AddTrigger(job.Id, trigger);

                var jobRun = new JobRun { JobId = job.Id, TriggerId = trigger.Id };
                this.JobStorage.AddJobRun(jobRun);

                var jobRun2 = new JobRun { JobId = job.Id, TriggerId = trigger.Id };
                this.JobStorage.AddJobRun(jobRun2);

                var jobRuns = client.GetJobRunsByTriggerId(job.Id, trigger.Id);

                Assert.AreEqual(2, jobRuns.Count);
            }
        }

        [TestMethod]
        public void UpdateInstantTrigger()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                var job = new Job();
                this.JobStorage.AddJob(job);

                var trigger = new InstantTrigger();
                this.JobStorage.AddTrigger(job.Id, trigger);

                var triggerDto = client.UpdateTrigger(job.Id, new InstantTriggerDto { Id = trigger.Id, IsActive = true });

                Assert.IsNotNull(triggerDto);
                Assert.IsTrue(triggerDto.IsActive);
            }
        }

        [TestMethod]
        public void UpdateScheduledTrigger()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                var job = new Job();
                this.JobStorage.AddJob(job);

                var trigger = new ScheduledTrigger();
                this.JobStorage.AddTrigger(job.Id, trigger);

                var triggerDto = client.UpdateTrigger(job.Id, new ScheduledTriggerDto { Id = trigger.Id, IsActive = true });

                Assert.IsNotNull(triggerDto);
                Assert.IsTrue(triggerDto.IsActive);
            }
        }

        [TestMethod]
        public void UpdateRecurringTrigger()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                var job = new Job();
                this.JobStorage.AddJob(job);

                var trigger = new RecurringTrigger();
                this.JobStorage.AddTrigger(job.Id, trigger);

                var triggerDto = client.UpdateTrigger(job.Id, new RecurringTriggerDto { Id = trigger.Id, IsActive = true });

                Assert.IsNotNull(triggerDto);
                Assert.IsTrue(triggerDto.IsActive);
            }
        }
    }
}
