using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Jobbr.Server.WebAPI.Model;
using Newtonsoft.Json;

namespace Jobbr.Client
{
    public class JobbrClient
    {
        protected HttpClient HttpClient;

        public JobbrClient(string backend)
        {
            this.Backend = backend + (backend.EndsWith("/") ? string.Empty  : "/") + "api/";
            this.HttpClient = new HttpClient { BaseAddress = new Uri(this.Backend) };
        }

        public string Backend { get; }

        public JobDto GetJob(long id)
        {
            var url = $"jobs/{id}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = this.HttpClient.SendAsync(request).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = response.Content.ReadAsStringAsync().Result;

                var responseDto = JsonConvert.DeserializeObject<JobDto>(contentString);

                return responseDto;
            }

            return null;
        }

        public List<JobDto> GetAllJobs()
        {
            const string url = "jobs/";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = this.HttpClient.SendAsync(request).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = response.Content.ReadAsStringAsync().Result;

                var responseDto = JsonConvert.DeserializeObject<List<JobDto>>(contentString);

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
            var url = $"jobs/{uniqueName}/triggers/{triggerDto.Id}";
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

        public List<JobRunDto> GetJobRunsByTriggerId(long jobId, long triggerId)
        {
            var url = $"jobruns/?jobId={jobId}&triggerId={triggerId}";

            var requestResult = this.HttpClient.GetAsync(url).Result;

            if (requestResult.StatusCode == HttpStatusCode.OK)
            {
                var json = requestResult.Content.ReadAsStringAsync().Result;

                var runs = JsonConvert.DeserializeObject<List<JobRunDto>>(json);

                return runs;
            }

            return null;
        }

        public JobRunDto GetJobRunById(long jobRunId)
        {
            var url = $"jobruns/{jobRunId}";

            var requestResult = this.HttpClient.GetAsync(url).Result;

            if (requestResult.StatusCode == HttpStatusCode.OK)
            {
                var json = requestResult.Content.ReadAsStringAsync().Result;

                var run = JsonConvert.DeserializeObject<JobRunDto>(json);

                return run;
            }

            return null;
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

            var response = this.HttpClient.SendAsync(request).Result;

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
