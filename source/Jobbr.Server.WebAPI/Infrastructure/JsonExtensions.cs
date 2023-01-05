using System.Text.Json;
using Jobbr.Server.WebAPI.Model;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    /// <summary>
    /// JSON extensions.
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Deserialize JSON string to a generic object.
        /// </summary>
        /// <param name="json">JSON string.</param>
        /// <returns>Generic object result.</returns>
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
