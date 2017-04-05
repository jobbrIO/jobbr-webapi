using Jobbr.ComponentModel.JobStorage;
using Jobbr.ComponentModel.Registration;

namespace Jobbr.WebApi.Tests
{
    public class ExposeStorageProvider : IJobbrComponent
    {
        internal readonly IJobStorageProvider JobStorageProvider;

        public static ExposeStorageProvider Instance;

        public ExposeStorageProvider(IJobStorageProvider jobStorageProvider)
        {
            this.JobStorageProvider = jobStorageProvider;
            Instance = this;
        }
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