using Newtonsoft.Json;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    public static class JsonExtensions
    {
        public static object Deserialize(this string json)
        {
            if (string.IsNullOrWhiteSpace(json) == false)
            {
                return JsonConvert.DeserializeObject(json);
            }

            return null;
        }
    }
}
