using System;
using System.IO;
using Jobbr.Runtime;
using Jobbr.Runtime.ForkedExecution;
using Sandbox.JobRunner.Jobs;

namespace Sanbox.JobRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Make sure the compiler does not remove the binding to this assembly
                var jobAssemblyToQueryJobs = typeof(ProgressJob).Assembly;

                // Set the default assembly to query for jobtypes
                var runtime = new ForkedRuntime(new RuntimeConfiguration {JobTypeSearchAssemblies = new[] { jobAssemblyToQueryJobs } });

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
