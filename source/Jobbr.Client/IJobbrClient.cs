using Jobbr.Server.WebAPI.Model;

using System.Threading.Tasks;

namespace Jobbr.Client
{
    public interface IJobbrClient
    {
        string Backend { get; }

        JobDto GetJob(long id);
        Task<JobDto> GetJobAsync(long id);

        bool IsAvailable();
        Task<bool> IsAvailableAsync();

        PagedResultDto<JobDto> QueryJobs(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);
        Task<PagedResultDto<JobDto>> QueryJobsAsync(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);

        PagedResultDto<JobRunDto> QueryJobRuns(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);
        Task<PagedResultDto<JobRunDto>> QueryJobRunsAsync(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);

        PagedResultDto<JobRunDto> QueryJobRunsByState(string state, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);
        Task<PagedResultDto<JobRunDto>> QueryJobRunsByStateAsync(string state, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);

        PagedResultDto<JobRunDto> QueryJobRunsByStates(string states, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);
        Task<PagedResultDto<JobRunDto>> QueryJobRunsByStatesAsync(string states, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);

        PagedResultDto<JobRunDto> QueryJobRunsByUserId(string userId, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string sort = null);
        Task<PagedResultDto<JobRunDto>> QueryJobRunsByUserIdAsync(string userId, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string sort = null);

        T AddTrigger<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase;
        T AddTrigger<T>(string uniqueName, T triggerDto) where T : JobTriggerDtoBase;
        Task<T> AddTriggerAsync<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase;
        Task<T> AddTriggerAsync<T>(string uniqueName, T triggerDto) where T : JobTriggerDtoBase;

        T UpdateTrigger<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase;
        Task<T> UpdateTriggerAsync<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase;

        T GetTriggerById<T>(long jobId, long triggerId) where T : JobTriggerDtoBase;
        Task<T> GetTriggerByIdAsync<T>(long jobId, long triggerId) where T : JobTriggerDtoBase;

        PagedResultDto<JobRunDto> GetJobRunsByTriggerId(long jobId, long triggerId, int page = 1, int pageSize = 50, string sort = null);
        Task<PagedResultDto<JobRunDto>> GetJobRunsByTriggerIdAsync(long jobId, long triggerId, int page = 1, int pageSize = 50, string sort = null);

        JobRunDto GetJobRunById(long jobRunId);
        Task<JobRunDto> GetJobRunByIdAsync(long jobRunId);
    }
}