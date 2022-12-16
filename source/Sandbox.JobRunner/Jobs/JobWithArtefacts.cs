using System.IO;

namespace Sandbox.JobRunner.Jobs
{
    public class JobWithArtefacts
    {
        public void Run()
        {
            File.AppendAllText("random-artefact.txt", "lorem ipsum");
        }
    }
}