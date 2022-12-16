using Jobbr.Server.WebAPI.Model;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Jobbr.Server.WebAPI.Model.DefaultJsonOptions;

namespace Jobbr.Client
{
    public class JobbrClient : IJobbrClient
    {
        private readonly HttpClient _httpClient;

        public JobbrClient(string backend)
        {
            Backend = backend + (backend.EndsWith("/") ? string.Empty : "/");
            _httpClient = new HttpClient { BaseAddress = new Uri(Backend) };
        }

        public string Backend { get; }

        public JobDto GetJob(long id) => GetJobAsync(id).Result;
        public async Task<JobDto> GetJobAsync(long id)
        {
            var url = $"jobs/{id}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var responseDto = JsonSerializer.Deserialize<JobDto>(contentString, Options);

                return responseDto;
            }

            return null;
        }

        public bool IsAvailable() => IsAvailableAsync().Result;
        public async Task<bool> IsAvailableAsync()
        {
            const string url = "status";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public PagedResultDto<JobDto> QueryJobs(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null) => QueryJobsAsync(page, pageSize, jobTypeFilter, jobUniqueNameFilter, query, sort).Result;
        public async Task<PagedResultDto<JobDto>> QueryJobsAsync(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null)
        {
            var url = $"jobs?page={page}&pageSize={pageSize}&jobTypeFilter={jobTypeFilter}&jobUniqueNameFilter={jobUniqueNameFilter}&query={query}&sort={sort}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var responseDto = JsonSerializer.Deserialize<PagedResultDto<JobDto>>(contentString, Options);

                return responseDto;
            }

            return null;
        }

        public PagedResultDto<JobRunDto> QueryJobRuns(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null) => QueryJobRunsAsync(page, pageSize, jobTypeFilter, jobUniqueNameFilter, query, sort).Result;
        public async Task<PagedResultDto<JobRunDto>> QueryJobRunsAsync(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null)
        {
            var url = $"jobRuns?page={page}&pageSize={pageSize}&jobTypeFilter={jobTypeFilter}&jobUniqueNameFilter={jobUniqueNameFilter}&query={query}&sort={sort}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var responseDto = JsonSerializer.Deserialize<PagedResultDto<JobRunDto>>(contentString, Options);

                return responseDto;
            }

            return null;
        }

        public PagedResultDto<JobRunDto> QueryJobRunsByState(string state, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null) => QueryJobRunsByStateAsync(state, page, pageSize, jobTypeFilter, jobUniqueNameFilter, query, sort).Result;
        public async Task<PagedResultDto<JobRunDto>> QueryJobRunsByStateAsync(string state, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null)
        {
            var url = $"jobRuns?page={page}&pageSize={pageSize}&jobTypeFilter={jobTypeFilter}&jobUniqueNameFilter={jobUniqueNameFilter}&query={query}&sort={sort}&state={state}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var responseDto = JsonSerializer.Deserialize<PagedResultDto<JobRunDto>>(contentString, Options);

                return responseDto;
            }

            return null;
        }

        public PagedResultDto<JobRunDto> QueryJobRunsByStates(string states, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null) => QueryJobRunsByStatesAsync(states, page, pageSize, jobTypeFilter, jobUniqueNameFilter, query, sort).Result;
        public async Task<PagedResultDto<JobRunDto>> QueryJobRunsByStatesAsync(string states, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null)
        {
            var url = $"jobRuns?page={page}&pageSize={pageSize}&jobTypeFilter={jobTypeFilter}&jobUniqueNameFilter={jobUniqueNameFilter}&query={query}&sort={sort}&states={states}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var responseDto = JsonSerializer.Deserialize<PagedResultDto<JobRunDto>>(contentString, Options);

                return responseDto;
            }

            return null;
        }

        public PagedResultDto<JobRunDto> QueryJobRunsByUserId(string userId, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string sort = null) => QueryJobRunsByUserIdAsync(userId, page, pageSize, jobTypeFilter, jobUniqueNameFilter, sort).Result;
        public async Task<PagedResultDto<JobRunDto>> QueryJobRunsByUserIdAsync(string userId, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string sort = null)
        {
            var url = $"users/{userId}/jobruns/?page={page}&pageSize={pageSize}&jobTypeFilter={jobTypeFilter}&jobUniqueNameFilter={jobUniqueNameFilter}&sort={sort}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var responseDto = JsonSerializer.Deserialize<PagedResultDto<JobRunDto>>(contentString, Options);

                return responseDto;
            }

            return null;
        }

        public T AddTrigger<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase => AddTriggerAsync<T>(jobId, triggerDto).Result;
        public T AddTrigger<T>(string uniqueName, T triggerDto) where T : JobTriggerDtoBase => AddTriggerAsync<T>(uniqueName, triggerDto).Result;

        public async Task<T> AddTriggerAsync<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase
        {
            var url = $"jobs/{jobId}/triggers";
            return await PostTriggerAsync(triggerDto, url).ConfigureAwait(false);
        }
        public async Task<T> AddTriggerAsync<T>(string uniqueName, T triggerDto) where T : JobTriggerDtoBase
        {
            var url = $"jobs/{uniqueName}/triggers";
            return await PostTriggerAsync(triggerDto, url).ConfigureAwait(false);
        }

        public T UpdateTrigger<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase => UpdateTriggerAsync<T>(jobId, triggerDto).Result;
        public async Task<T> UpdateTriggerAsync<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase
        {
            var url = $"jobs/{jobId}/triggers/{triggerDto.Id}";
            return await PatchTriggerAsync(triggerDto, url).ConfigureAwait(false);
        }

        public T GetTriggerById<T>(long jobId, long triggerId) where T : JobTriggerDtoBase => GetTriggerByIdAsync<T>(jobId, triggerId).Result;
        public async Task<T> GetTriggerByIdAsync<T>(long jobId, long triggerId) where T : JobTriggerDtoBase
        {
            var url = $"jobs/{jobId}/triggers/{triggerId}";
            return await GetTrigger<T>(url).ConfigureAwait(false);
        }

        public PagedResultDto<JobRunDto> GetJobRunsByTriggerId(long jobId, long triggerId, int page = 1, int pageSize = 50, string sort = null) => GetJobRunsByTriggerIdAsync(jobId, triggerId, page, pageSize, sort).Result;
        public async Task<PagedResultDto<JobRunDto>> GetJobRunsByTriggerIdAsync(long jobId, long triggerId, int page = 1, int pageSize = 50, string sort = null)
        {
            var url = $"jobs/{jobId}/triggers/{triggerId}/jobruns?page={page}&pageSize={pageSize}&sort={sort}";

            var requestResult = await _httpClient.GetAsync(url).ConfigureAwait(false);

            if (requestResult.StatusCode == HttpStatusCode.OK)
            {
                var json = await requestResult.Content.ReadAsStringAsync().ConfigureAwait(false);

                var runs = JsonSerializer.Deserialize<PagedResultDto<JobRunDto>>(json, Options);

                return runs;
            }

            return null;
        }

        public JobRunDto GetJobRunById(long jobRunId) => GetJobRunByIdAsync(jobRunId).Result;
        public async Task<JobRunDto> GetJobRunByIdAsync(long jobRunId)
        {
            var url = $"jobruns/{jobRunId}";

            var requestResult = await _httpClient.GetAsync(url).ConfigureAwait(false);

            if (requestResult.StatusCode == HttpStatusCode.OK)
            {
                var json = await requestResult.Content.ReadAsStringAsync().ConfigureAwait(false);

                var run = JsonSerializer.Deserialize<JobRunDto>(json, Options);

                return run;
            }

            return null;
        }

        public bool DeleteJobRun(long jobRunId) => DeleteJobRunAsync(jobRunId).Result;
        public async Task<bool> DeleteJobRunAsync(long jobRunId)
        {
            var url = $"jobruns/{jobRunId}";

            var requestResult = await _httpClient.DeleteAsync(url).ConfigureAwait(false);

            return requestResult.StatusCode == HttpStatusCode.OK;
        }

        private async Task<T> PostTriggerAsync<T>(T triggerDto, string url) where T : JobTriggerDtoBase
        {
            return await ExecuteDtoRequest(url, triggerDto, HttpMethod.Post).ConfigureAwait(false);
        }

        private async Task<T> PatchTriggerAsync<T>(T triggerDto, string url) where T : JobTriggerDtoBase
        {
            return await ExecuteDtoRequest(url, triggerDto, new HttpMethod("PATCH")).ConfigureAwait(false);
        }

        private async Task<T> GetTrigger<T>(string url) where T : class
        {
            return await ExecuteDtoRequest<T>(url, null, HttpMethod.Get).ConfigureAwait(false);
        }

        private async Task<T> ExecuteDtoRequest<T>(string url, T dto, HttpMethod httpMethod) where T : class
        {
            var json = dto != null ? JsonSerializer.Serialize(dto, Options) : string.Empty;
            var request = new HttpRequestMessage(httpMethod, url);

            if (httpMethod != HttpMethod.Get)
            {
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var responseDto = JsonSerializer.Deserialize<T>(contentString, Options);

                return responseDto;
            }

            return null;
        }
    }
}
