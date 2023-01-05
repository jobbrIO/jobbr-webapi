using Jobbr.ComponentModel.JobStorage;
using Jobbr.ComponentModel.Registration;

namespace Jobbr.WebApi.Tests
{
    public class ExposeStorageProvider : IJobbrComponent
    {
        public static ExposeStorageProvider Instance;

        public ExposeStorageProvider(IJobStorageProvider jobStorageProvider)
        {
            JobStorageProvider = jobStorageProvider;
            Instance = this;
        }

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