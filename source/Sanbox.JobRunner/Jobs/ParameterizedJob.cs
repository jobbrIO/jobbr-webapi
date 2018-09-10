using System;
using Sanbox.JobRunner.Jobs;

namespace Sandbox.JobRunner.Jobs
{
    public class ParameterizedJob
    {
        public void Run(object jobParameters, RunParameter runParameters)
        {
            Console.WriteLine("Got the params {0} and {1}", jobParameters, runParameters);
        }
    }
}