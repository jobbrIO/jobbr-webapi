using System;
using Sandbox.JobRunner.Jobs;

namespace Sandbox.JobRunner.Jobs
{
    public class ParameterizedJob
    {
        public void Run(object jobParameters, RunParameter runParameters)
        {
            Console.WriteLine($"Got the params {jobParameters} and {runParameters}");
        }
    }
}