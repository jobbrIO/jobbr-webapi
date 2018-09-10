using System.Linq;
using Jobbr.Client;
using Jobbr.ComponentModel.JobStorage.Model;
using Jobbr.Server.WebAPI.Model;
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
        public void Query_Jobs_Total_Items()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                this.JobStorage.AddJob(new Job { Title = "title1", Type = "Some.Type1", UniqueName = "unique1" });
                this.JobStorage.AddJob(new Job { Title = "title2", Type = "Some.Type2", UniqueName = "unique2" });
                this.JobStorage.AddJob(new Job { Title = "title3", Type = "Some.Type3", UniqueName = "unique3" });
                this.JobStorage.AddJob(new Job { Title = "title4", Type = "Some.Type4", UniqueName = "unique4" });
                this.JobStorage.AddJob(new Job { Title = "title5", Type = "Some.Type5", UniqueName = "unique5" });

                var result = client.QueryJobs();

                Assert.AreEqual(5, result.TotalItems);
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

                var jobRun = new JobRun { Job = new Job { Id = job.Id }, Trigger = new RecurringTrigger { Id = trigger.Id } };
                this.JobStorage.AddJobRun(jobRun);

                var jobRunDto = client.GetJobRunById(jobRun.Id);

                Assert.IsNotNull(jobRunDto);
                Assert.AreEqual(jobRun.Id, jobRunDto.JobRunId);
                Assert.AreEqual(jobRun.Job.Id, jobRunDto.JobId);
                Assert.AreEqual(jobRun.Trigger.Id, jobRunDto.TriggerId);
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

                var jobRun = new JobRun { Job = new Job { Id = job.Id }, Trigger = new RecurringTrigger { Id = trigger.Id } };
                this.JobStorage.AddJobRun(jobRun);

                var jobRun2 = new JobRun { Job = new Job { Id = job.Id }, Trigger = new RecurringTrigger { Id = trigger.Id } };
                this.JobStorage.AddJobRun(jobRun2);

                var jobRuns = client.GetJobRunsByTriggerId(job.Id, trigger.Id);

                Assert.AreEqual(2, jobRuns.TotalItems);
            }
        }

        [TestMethod]
        public void Get_JobRuns_By_State()
        {
            using (this.GivenRunningServerWithWebApi())
            {
                var client = new JobbrClient(this.BackendAddress);

                var job = new Job();
                this.JobStorage.AddJob(job);

                var trigger = new RecurringTrigger();
                this.JobStorage.AddTrigger(job.Id, trigger);

                var jobRun = new JobRun { Job = new Job { Id = job.Id }, Trigger = new RecurringTrigger { Id = trigger.Id }, State = JobRunStates.Completed };
                this.JobStorage.AddJobRun(jobRun);

                var jobRun2 = new JobRun { Job = new Job { Id = job.Id }, Trigger = new RecurringTrigger { Id = trigger.Id }, State = JobRunStates.Completed };
                this.JobStorage.AddJobRun(jobRun2);

                var jobRun3 = new JobRun { Job = new Job { Id = job.Id }, Trigger = new RecurringTrigger { Id = trigger.Id }, State = JobRunStates.Failed };
                this.JobStorage.AddJobRun(jobRun3);

                var jobRuns = client.QueryJobRunsByState("Completed");

                Assert.AreEqual(2, jobRuns.TotalItems);
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

                var trigger2 = this.JobStorage.GetTriggerById(job.Id, trigger.Id);

                Assert.IsTrue(trigger2.IsActive);
            }
        }
    }
}
