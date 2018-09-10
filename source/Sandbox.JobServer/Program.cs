using System;
using Jobbr.Server.Builder;
using Jobbr.Server.ForkedExecution;
using Jobbr.Server.JobRegistry;
using Jobbr.Server.WebAPI;
using Jobbr.Storage.MsSql;
using Sanbox.JobRunner.Jobs;
using ServiceStack.OrmLite.SqlServer;

namespace Sandbox.JobServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var jobbrBuilder = new JobbrBuilder();

            jobbrBuilder.AddForkedExecution(config =>
            {
                config.JobRunDirectory = "C:/temp";
                config.JobRunnerExecutable = "Sandbox.JobRunner.exe";
                config.MaxConcurrentProcesses = 4;
                config.IsRuntimeWaitingForDebugger = false;
            });

            jobbrBuilder.AddJobs(repo =>
            {
                repo.Define("ProgressJob", "Sandbox.JobRunner.Jobs.ProgressJob")
                    .WithTrigger("* * * * *")
                    .WithTrigger(DateTime.UtcNow.AddSeconds(30), new RunParameter { Param1 = "foo", Param2 = 1337 });

                repo.Define("ParameterizedJob", "Sandbox.JobRunner.Jobs.ParameterizedJob")
                    .WithParameter(new RunParameter { Param1 = "default job param", Param2 = 1000 })
                    .WithTrigger("* * * * *")
                    .WithTrigger(DateTime.UtcNow.AddSeconds(30), new RunParameter { Param1 = "customized", Param2 = 5000 });

                repo.Define("ArtefactJob", "Sandbox.JobRunner.Jobs.JobWithArtefacts")
                    .WithTrigger("* * * * *");
            });

            jobbrBuilder.AddWebApi(config =>
            {
                config.BackendAddress = "http://localhost:1337";
            });

            jobbrBuilder.AddMsSqlStorage(c =>
            {
                c.ConnectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=JobbrWebApiSandbox;Integrated Security=True";
                c.DialectProvider = new SqlServer2017OrmLiteDialectProvider();
                c.CreateTablesIfNotExists = true;
            });

            using (var server = jobbrBuilder.Create())
            {
                server.Start(20000);

                Console.ReadLine();

                server.Stop();
            }
        }
    }
}
