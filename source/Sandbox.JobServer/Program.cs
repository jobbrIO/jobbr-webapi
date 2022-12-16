using Jobbr.Server.Builder;
using Jobbr.Server.ForkedExecution;
using Jobbr.Server.JobRegistry;
using Jobbr.Server.WebAPI;
using Jobbr.Storage.MsSql;
using Microsoft.Extensions.Logging;
using Sandbox.JobRunner.Jobs;
using ServiceStack.OrmLite.SqlServer;
using System;
using System.IO;
using LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory;

namespace Sandbox.JobServer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddFilter(level => level >= LogLevel.Debug)
                .AddConsole());

            var jobbrBuilder = new JobbrBuilder(loggerFactory);

            jobbrBuilder.AddForkedExecution(config =>
            {
                config.JobRunDirectory = Path.GetTempPath();
                config.JobRunnerExecutable = "Sandbox.JobRunner.exe";
                config.MaxConcurrentProcesses = 4;
                config.IsRuntimeWaitingForDebugger = false;
            });

            jobbrBuilder.AddJobs(loggerFactory,
                repo =>
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
