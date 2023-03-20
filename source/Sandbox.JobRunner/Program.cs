using Jobbr.Runtime;
using Jobbr.Runtime.ForkedExecution;
using Microsoft.Extensions.Logging;
using Sandbox.JobRunner.Jobs;
using System;
using System.IO;

namespace Sandbox.JobRunner
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                using var loggerFactory = LoggerFactory.Create(builder => builder
                    .AddFilter(level => level >= LogLevel.Debug));

                // Make sure the compiler does not remove the binding to this assembly
                var jobAssemblyToQueryJobs = typeof(ProgressJob).Assembly;

                // Set the default assembly to query for jobtypes
                var runtime = new ForkedRuntime(loggerFactory, new RuntimeConfiguration { JobTypeSearchAssemblies = new[] { jobAssemblyToQueryJobs } });

                // Pass the arguments of the forked execution to the runtime
                runtime.Run(args);
            }
            catch (Exception e)
            {
                File.WriteAllText("error.txt", e.ToString());
            }
        }
    }
}
