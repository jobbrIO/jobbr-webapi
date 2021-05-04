using Jobbr.Server.WebAPI.Model;

namespace Jobbr.Client
{
    public interface IJobbrClient
    {
        string Backend { get; }

        JobDto GetJob(long id);
        PagedResultDto<JobDto> QueryJobs(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);
        PagedResultDto<JobRunDto> QueryJobRuns(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);
        PagedResultDto<JobRunDto> QueryJobRunsByState(string state, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);
        PagedResultDto<JobRunDto> QueryJobRunsByStates(string states, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);
        PagedResultDto<JobRunDto> QueryJobRunsByUserId(string userId, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string sort = null);
        T AddTrigger<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase;
        T AddTrigger<T>(string uniqueName, T triggerDto) where T : JobTriggerDtoBase;
        T UpdateTrigger<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase;
        T GetTriggerById<T>(long jobId, long triggerId) where T : JobTriggerDtoBase;
        PagedResultDto<JobRunDto> GetJobRunsByTriggerId(long jobId, long triggerId, int page = 1, int pageSize = 50, string sort = null);
        JobRunDto GetJobRunById(long jobRunId);
    }
}