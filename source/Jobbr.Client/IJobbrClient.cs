using System.Threading.Tasks;
using Jobbr.Server.WebAPI.Model;

namespace Jobbr.Client
{
    /// <summary>
    /// Jobbr client interface.
    /// </summary>
    public interface IJobbrClient
    {
        /// <summary>
        /// Backend URL address.
        /// </summary>
        string Backend { get; }

        /// <summary>
        /// Get <see cref="JobDto"/> with job ID.
        /// </summary>
        /// <param name="id">Target job ID.</param>
        /// <returns>DTO containing the job information.</returns>
        JobDto GetJob(long id);

        /// <summary>
        /// Asynchronously get <see cref="JobDto"/> with job ID.
        /// </summary>
        /// <param name="id">Target job ID.</param>
        /// <returns>DTO containing the job information.</returns>
        Task<JobDto> GetJobAsync(long id);

        /// <summary>
        /// If client is available.
        /// </summary>
        /// <returns>True if available and false if not.</returns>
        bool IsAvailable();

        /// <summary>
        /// If client is available (async).
        /// </summary>
        /// <returns>True if available and false if not.</returns>
        Task<bool> IsAvailableAsync();

        /// <summary>
        /// Query jobs.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="jobTypeFilter">Job type filter.</param>
        /// <param name="jobUniqueNameFilter">Job unique name filter.</param>
        /// <param name="query">Query.</param>
        /// <param name="sort">Sort.</param>
        /// <returns><see cref="JobDto"/>s as paged result.</returns>
        PagedResultDto<JobDto> QueryJobs(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);

        /// <summary>
        /// Asynchronously query jobs.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="jobTypeFilter">Job type filter.</param>
        /// <param name="jobUniqueNameFilter">Job unique name filter.</param>
        /// <param name="query">Query.</param>
        /// <param name="sort">Sort.</param>
        /// <returns><see cref="JobDto"/>s as paged result.</returns>
        Task<PagedResultDto<JobDto>> QueryJobsAsync(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);

        /// <summary>
        /// Query job runs.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="jobTypeFilter">Job type filter.</param>
        /// <param name="jobUniqueNameFilter">Job unique name filter.</param>
        /// <param name="query">Query.</param>
        /// <param name="sort">Sort.</param>
        /// <returns><see cref="JobRunDto"/>s as paged result.</returns>
        PagedResultDto<JobRunDto> QueryJobRuns(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);

        /// <summary>
        /// Asynchronously query job runs.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="jobTypeFilter">Job type filter.</param>
        /// <param name="jobUniqueNameFilter">Job unique name filter.</param>
        /// <param name="query">Query.</param>
        /// <param name="sort">Sort.</param>
        /// <returns><see cref="JobRunDto"/>s as paged result.</returns>
        Task<PagedResultDto<JobRunDto>> QueryJobRunsAsync(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);

        /// <summary>
        /// Query job runs by job run state.
        /// </summary>
        /// <param name="state">Target state.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="jobTypeFilter">Job type filter.</param>
        /// <param name="jobUniqueNameFilter">Job unique name filter.</param>
        /// <param name="query">Query.</param>
        /// <param name="sort">Sort.</param>
        /// <returns><see cref="JobRunDto"/>s as paged result.</returns>
        PagedResultDto<JobRunDto> QueryJobRunsByState(string state, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);

        /// <summary>
        /// Asynchronously query job runs by job run state.
        /// </summary>
        /// <param name="state">Target state.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="jobTypeFilter">Job type filter.</param>
        /// <param name="jobUniqueNameFilter">Job unique name filter.</param>
        /// <param name="query">Query.</param>
        /// <param name="sort">Sort.</param>
        /// <returns><see cref="JobRunDto"/>s as paged result.</returns>
        Task<PagedResultDto<JobRunDto>> QueryJobRunsByStateAsync(string state, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);

        /// <summary>
        /// Query job runs by multiple job run states.
        /// </summary>
        /// <param name="states">Target states.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="jobTypeFilter">Job type filter.</param>
        /// <param name="jobUniqueNameFilter">Job unique name filter.</param>
        /// <param name="query">Query.</param>
        /// <param name="sort">Sort.</param>
        /// <returns><see cref="JobRunDto"/>s as paged result.</returns>
        PagedResultDto<JobRunDto> QueryJobRunsByStates(string states, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);

        /// <summary>
        /// Asynchronously query job runs by multiple job run states.
        /// </summary>
        /// <param name="states">Target states.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="jobTypeFilter">Job type filter.</param>
        /// <param name="jobUniqueNameFilter">Job unique name filter.</param>
        /// <param name="query">Query.</param>
        /// <param name="sort">Sort.</param>
        /// <returns><see cref="JobRunDto"/>s as paged result.</returns>
        Task<PagedResultDto<JobRunDto>> QueryJobRunsByStatesAsync(string states, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null);

        /// <summary>
        /// Query job runs by user ID.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="jobTypeFilter">Job type filter.</param>
        /// <param name="jobUniqueNameFilter">Job unique name filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns><see cref="JobRunDto"/>s as paged result.</returns>
        PagedResultDto<JobRunDto> QueryJobRunsByUserId(string userId, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string sort = null);

        /// <summary>
        /// Asynchronously query job runs by user ID.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="jobTypeFilter">Job type filter.</param>
        /// <param name="jobUniqueNameFilter">Job unique name filter.</param>
        /// <param name="sort">Sort.</param>
        /// <returns><see cref="JobRunDto"/>s as paged result.</returns>
        Task<PagedResultDto<JobRunDto>> QueryJobRunsByUserIdAsync(string userId, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string sort = null);

        /// <summary>
        /// Add job trigger with job ID.
        /// </summary>
        /// <typeparam name="T">Trigger type.</typeparam>
        /// <param name="jobId">Job ID.</param>
        /// <param name="triggerDto">Trigger data transfer object.</param>
        /// <returns>Trigger that was added.</returns>
        T AddTrigger<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase;

        /// <summary>
        /// Asynchronously add job trigger with job ID.
        /// </summary>
        /// <typeparam name="T">Trigger type.</typeparam>
        /// <param name="jobId">Job ID.</param>
        /// <param name="triggerDto">Trigger data transfer object.</param>
        /// <returns>Trigger that was added.</returns>
        Task<T> AddTriggerAsync<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase;

        /// <summary>
        /// Add job trigger with unique name.
        /// </summary>
        /// <typeparam name="T">Trigger type.</typeparam>
        /// <param name="uniqueName">Unique name.</param>
        /// <param name="triggerDto">Trigger data transfer object.</param>
        /// <returns>Trigger that was added.</returns>
        T AddTrigger<T>(string uniqueName, T triggerDto) where T : JobTriggerDtoBase;

        /// <summary>
        /// Asynchronously add job trigger with unique name.
        /// </summary>
        /// <typeparam name="T">Trigger type.</typeparam>
        /// <param name="uniqueName">Unique name.</param>
        /// <param name="triggerDto">Trigger data transfer object.</param>
        /// <returns>Trigger that was added.</returns>
        Task<T> AddTriggerAsync<T>(string uniqueName, T triggerDto) where T : JobTriggerDtoBase;

        /// <summary>
        /// Update job trigger with job ID.
        /// </summary>
        /// <typeparam name="T">Trigger type.</typeparam>
        /// <param name="jobId">Job ID.</param>
        /// <param name="triggerDto">Trigger data transfer object.</param>
        /// <returns>Trigger that was updated.</returns>
        T UpdateTrigger<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase;

        /// <summary>
        /// Asynchronously update job trigger with job ID.
        /// </summary>
        /// <typeparam name="T">Trigger type.</typeparam>
        /// <param name="jobId">Job ID.</param>
        /// <param name="triggerDto">Trigger data transfer object.</param>
        /// <returns>Trigger that was updated.</returns>
        Task<T> UpdateTriggerAsync<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase;

        /// <summary>
        /// Get job trigger with job ID.
        /// </summary>
        /// <typeparam name="T">Trigger type.</typeparam>
        /// <param name="jobId">Job ID.</param>
        /// <param name="triggerId">Trigger ID.</param>
        /// <returns>Trigger that was found.</returns>
        T GetTriggerById<T>(long jobId, long triggerId) where T : JobTriggerDtoBase;

        /// <summary>
        /// Asynchronously get job trigger with job ID.
        /// </summary>
        /// <typeparam name="T">Trigger type.</typeparam>
        /// <param name="jobId">Job ID.</param>
        /// <param name="triggerId">Trigger ID.</param>
        /// <returns>Trigger that was found.</returns>
        Task<T> GetTriggerByIdAsync<T>(long jobId, long triggerId) where T : JobTriggerDtoBase;

        /// <summary>
        /// Get job runs by trigger ID.
        /// </summary>
        /// <param name="jobId">Job ID.</param>
        /// <param name="triggerId">Trigger ID.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>Job run information.</returns>
        PagedResultDto<JobRunDto> GetJobRunsByTriggerId(long jobId, long triggerId, int page = 1, int pageSize = 50, string sort = null);

        /// <summary>
        /// Asynchronously get job runs by trigger ID.
        /// </summary>
        /// <param name="jobId">Job ID.</param>
        /// <param name="triggerId">Trigger ID.</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="sort">Sort.</param>
        /// <returns>Job run information.</returns>
        Task<PagedResultDto<JobRunDto>> GetJobRunsByTriggerIdAsync(long jobId, long triggerId, int page = 1, int pageSize = 50, string sort = null);

        /// <summary>
        /// Get job run by job run ID.
        /// </summary>
        /// <param name="jobRunId">Job run ID.</param>
        /// <returns><see cref="JobRunDto"/>.</returns>
        JobRunDto GetJobRunById(long jobRunId);

        /// <summary>
        /// Asynchronously get job run by job run ID.
        /// </summary>
        /// <param name="jobRunId">Job run ID.</param>
        /// <returns><see cref="JobRunDto"/>.</returns>
        Task<JobRunDto> GetJobRunByIdAsync(long jobRunId);

        /// <summary>
        /// Delete job run.
        /// </summary>
        /// <param name="jobRunId">Job run ID.</param>
        /// <returns>If the jon run deletion was successful.</returns>
        bool DeleteJobRun(long jobRunId);

        /// <summary>
        /// Asynchronously delete job run.
        /// </summary>
        /// <param name="jobRunId">Job run ID.</param>
        /// <returns>If the jon run deletion was successful.</returns>
        Task<bool> DeleteJobRunAsync(long jobRunId);
    }
}