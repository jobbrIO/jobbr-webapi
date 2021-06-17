using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using Jobbr.Server.WebAPI.Model;
using Newtonsoft.Json;

namespace Jobbr.Client
{
    public class JobbrClient
    {
        private readonly HttpClient httpClient;

        public JobbrClient(string backend)
        {
            this.Backend = backend + (backend.EndsWith("/") ? string.Empty  : "/");
            this.httpClient = new HttpClient { BaseAddress = new Uri(this.Backend) };
        }

        public string Backend { get; }

        public JobDto GetJob(long id)
        {
            var url = $"jobs/{id}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = this.httpClient.SendAsync(request).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = response.Content.ReadAsStringAsync().Result;

                var responseDto = JsonConvert.DeserializeObject<JobDto>(contentString);

                return responseDto;
            }

            return null;
        }

        public bool IsAvailable()
        {
            const string url = "status";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = this.httpClient.SendAsync(request).Result;

            return response.StatusCode == HttpStatusCode.OK;
        }

        public PagedResultDto<JobDto> QueryJobs(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null)
        {
            var url = $"jobs?page={page}&pageSize={pageSize}&jobTypeFilter={jobTypeFilter}&jobUniqueNameFilter={jobUniqueNameFilter}&query={query}&sort={sort}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = this.httpClient.SendAsync(request).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = response.Content.ReadAsStringAsync().Result;

                var responseDto = JsonConvert.DeserializeObject<PagedResultDto<JobDto>>(contentString);

                return responseDto;
            }

            return null;
        }

        public PagedResultDto<JobRunDto> QueryJobRuns(int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null)
        {
            var url = $"jobRuns?page={page}&pageSize={pageSize}&jobTypeFilter={jobTypeFilter}&jobUniqueNameFilter={jobUniqueNameFilter}&query={query}&sort={sort}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = this.httpClient.SendAsync(request).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = response.Content.ReadAsStringAsync().Result;

                var responseDto = JsonConvert.DeserializeObject<PagedResultDto<JobRunDto>>(contentString);

                return responseDto;
            }

            return null;
        }

        public PagedResultDto<JobRunDto> QueryJobRunsByState(string state, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null)
        {
            var url = $"jobRuns?page={page}&pageSize={pageSize}&jobTypeFilter={jobTypeFilter}&jobUniqueNameFilter={jobUniqueNameFilter}&query={query}&sort={sort}&state={state}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = this.httpClient.SendAsync(request).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = response.Content.ReadAsStringAsync().Result;

                var responseDto = JsonConvert.DeserializeObject<PagedResultDto<JobRunDto>>(contentString);

                return responseDto;
            }

            return null;
        }

        public PagedResultDto<JobRunDto> QueryJobRunsByStates(string states, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string query = null, string sort = null)
        {
            var url = $"jobRuns?page={page}&pageSize={pageSize}&jobTypeFilter={jobTypeFilter}&jobUniqueNameFilter={jobUniqueNameFilter}&query={query}&sort={sort}&states={states}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = this.httpClient.SendAsync(request).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = response.Content.ReadAsStringAsync().Result;

                var responseDto = JsonConvert.DeserializeObject<PagedResultDto<JobRunDto>>(contentString);

                return responseDto;
            }

            return null;
        }

        public PagedResultDto<JobRunDto> QueryJobRunsByUserId(string userId, int page = 1, int pageSize = 50, string jobTypeFilter = null, string jobUniqueNameFilter = null, string sort = null)
        {
            var url = $"users/{userId}/jobruns/?page={page}&pageSize={pageSize}&jobTypeFilter={jobTypeFilter}&jobUniqueNameFilter={jobUniqueNameFilter}&sort={sort}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = this.httpClient.SendAsync(request).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = response.Content.ReadAsStringAsync().Result;

                var responseDto = JsonConvert.DeserializeObject<PagedResultDto<JobRunDto>>(contentString);

                return responseDto;
            }

            return null;
        }

        public T AddTrigger<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase
        {
            var url = $"jobs/{jobId}/triggers";
            return this.PostTrigger(triggerDto, url);
        }

        public T AddTrigger<T>(string uniqueName, T triggerDto) where T : JobTriggerDtoBase
        {
            var url = $"jobs/{uniqueName}/triggers";
            return this.PostTrigger(triggerDto, url);
        }

        public T UpdateTrigger<T>(long jobId, T triggerDto) where T : JobTriggerDtoBase
        {
            var url = $"jobs/{jobId}/triggers/{triggerDto.Id}";
            return this.PatchTrigger(triggerDto, url);
        }

        public T GetTriggerById<T>(long jobId, long triggerId) where T : JobTriggerDtoBase
        {
            var url = $"jobs/{jobId}/triggers/{triggerId}";
            return this.GetTrigger<T>(url);
        }

        public PagedResultDto<JobRunDto> GetJobRunsByTriggerId(long jobId, long triggerId, int page = 1, int pageSize = 50, string sort = null)
        {
            var url = $"jobs/{jobId}/triggers/{triggerId}/jobruns?page={page}&pageSize={pageSize}&sort={sort}";

            var requestResult = this.httpClient.GetAsync(url).Result;

            if (requestResult.StatusCode == HttpStatusCode.OK)
            {
                var json = requestResult.Content.ReadAsStringAsync().Result;

                var runs = JsonConvert.DeserializeObject<PagedResultDto<JobRunDto>>(json);

                return runs;
            }

            return null;
        }

        public JobRunDto GetJobRunById(long jobRunId)
        {
            var url = $"jobruns/{jobRunId}";

            var requestResult = this.httpClient.GetAsync(url).Result;

            if (requestResult.StatusCode == HttpStatusCode.OK)
            {
                var json = requestResult.Content.ReadAsStringAsync().Result;

                var run = JsonConvert.DeserializeObject<JobRunDto>(json);

                return run;
            }

            return null;
        }

        public bool DeleteJobRun(long jobRunId)
        {
            var url = $"jobruns/{jobRunId}";

            var requestResult = this.httpClient.DeleteAsync(url).Result;

            return requestResult.StatusCode == HttpStatusCode.OK;
        }

        private T PostTrigger<T>(T triggerDto, string url) where T : JobTriggerDtoBase
        {
            return this.ExecuteDtoRequest(url, triggerDto, HttpMethod.Post);
        }

        private T PatchTrigger<T>(T triggerDto, string url) where T : JobTriggerDtoBase
        {
            return this.ExecuteDtoRequest(url, triggerDto, new HttpMethod("PATCH"));
        }

        private T GetTrigger<T>(string url) where T : class
        {
            return this.ExecuteDtoRequest<T>(url, null, HttpMethod.Get);
        }

        private T GetTriggers<T>(string url) where T : class
        {
            return this.ExecuteDtoRequest<T>(url, null, HttpMethod.Get);
        }

        private T ExecuteDtoRequest<T>(string url, T dto, HttpMethod httpMethod) where T: class
        {
            var json = dto != null ? JsonConvert.SerializeObject(dto) : string.Empty;
            var request = new HttpRequestMessage(httpMethod, url);

            if (httpMethod != HttpMethod.Get)
            {
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = this.httpClient.SendAsync(request).Result;

            if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = response.Content.ReadAsStringAsync().Result;

                var responseDto = JsonConvert.DeserializeObject<T>(contentString);

                return responseDto;
            }

            return null;
        }
    }
}
