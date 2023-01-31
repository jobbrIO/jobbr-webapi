using Jobbr.ComponentModel.JobStorage;
using Jobbr.ComponentModel.Registration;

namespace Jobbr.WebApi.Tests
{
    public class ExposeStorageProvider : IJobbrComponent
    {
        public ExposeStorageProvider(IJobStorageProvider jobStorageProvider)
        {
            JobStorageProvider = jobStorageProvider;
            Instance = this;
        }

        public static ExposeStorageProvider Instance { get; private set; }

        internal IJobStorageProvider JobStorageProvider { get; }

        public void Dispose()
        {
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}