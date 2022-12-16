using System.Text.Json;
using Jobbr.Server.WebAPI.Model;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    public static class JsonExtensions
    {
        public static object Deserialize(this string json)
        {
            if (string.IsNullOrWhiteSpace(json) == false)
            {
                return JsonSerializer.Deserialize<object>(json, DefaultJsonOptions.Options);
            }

            return null;
        }
    }
}
