using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jobbr.Server.WebAPI.Model
{
    /// <summary>
    /// Static class for containing the default options for JSON.
    /// </summary>
    public static class DefaultJsonOptions
    {
        /// <summary>
        /// Default options for JSON.
        /// </summary>
        public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };
    }
}
