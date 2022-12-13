using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jobbr.Server.WebAPI.Model
{
    public static class DefaultJsonOptions
    {
        public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };
    }
}
