using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Jobbr.Server.WebAPI.Infrastructure
{
    /// <summary>
    /// Inspired by code from Michael Schnyder.
    /// Deserializes JSON to an .NET object with type <paramref name="TType"/>
    /// </summary>
    /// <typeparam name="TType">
    /// </typeparam>
    public class JsonTypeConverter<TType> : JsonConverter<TType>
    {
        private static ILogger<JsonTypeConverter<TType>> _logger;

        /// <summary>
        /// The cached types.
        /// </summary>
        private static List<Type> _possibleTypes;

        /// <summary>
        /// The prop selector name.
        /// </summary>
        private readonly string _propSelectorName;

        private readonly Func<List<Type>, string, Type> _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonTypeConverter{TType}"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        ///     The logger factory.
        /// </param>
        /// <param name="propSelectorName">
        ///     The prop selector name.
        /// </param>
        /// <param name="resolver">
        ///     The type resolver.
        /// </param>
        public JsonTypeConverter(ILoggerFactory loggerFactory, string propSelectorName,
            Func<List<Type>, string, Type> resolver)
        {
            _logger = loggerFactory.CreateLogger<JsonTypeConverter<TType>>();
            _propSelectorName = propSelectorName;
            _resolver = resolver;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonTypeConverter{TType}"/> class.
        /// </summary>
        /// <param name="propSelectorName">
        /// The prop selector name.
        /// </param>
        public JsonTypeConverter(string propSelectorName)
        {
            _propSelectorName = propSelectorName;
            _resolver = DefaultResolver;
        }

        public override void Write(Utf8JsonWriter writer, TType value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, CopyJsonSerializerOptions(options));
        }

        public override TType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var jsonDocument = JsonDocument.ParseValue(ref reader))
            {
                var type = GetTypeFromJsonProperty(jsonDocument.RootElement);
                var jsonObject = jsonDocument.RootElement.GetRawText();

                return (TType)JsonSerializer.Deserialize(jsonObject, type, CopyJsonSerializerOptions(options));
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(TType).IsAssignableFrom(objectType);
        }

        private static List<Type> GetTypesFromAllAssemblies()
        {
            try
            {
                if (_possibleTypes == null)
                {
                    _possibleTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.IsSubclassOf(typeof(TType)) || typeof(TType).IsAssignableFrom(t)).ToList();
                }
            }
            catch (ReflectionTypeLoadException e)
            {
                _logger.LogError(e, e.Message);

                foreach (var loaderException in e.LoaderExceptions)
                {
                    _logger.LogError(loaderException, loaderException?.Message);
                }

                throw;
            }

            return _possibleTypes;
        }

        private Type GetTypeFromJsonProperty(JsonElement jsonObject)
        {
            var property = jsonObject.EnumerateObject().FirstOrDefault(p => p.Name.ToLowerInvariant() == _propSelectorName.ToLowerInvariant());

            if (property.Value.ValueKind == JsonValueKind.Undefined)
            {
                throw new ArgumentException($"The json didn't contain a property named '{_propSelectorName}'!");
            }

            var typeValue = property.Value.GetString();

            if (typeValue == null)
            {
                throw new ArgumentException($"The property '{_propSelectorName}' was null!");
            }

            var typesFromAllAssemblies = GetTypesFromAllAssemblies();

            var type = _resolver(typesFromAllAssemblies, typeValue);

            if (type == null)
            {
                throw new ArgumentException($"Cannot create object for empty type {typeValue}");
            }

            return type;
        }

        private static Type DefaultResolver(IEnumerable<Type> typesFromAllAssemblies, string type)
        {
            // t.Name.ToLowerInvariant() == type.ToLowerInvariant()
            // dotTrace shows that calling ToLowerInvariant is slow.
            // https://msdn.microsoft.com/en-us/library/dd465121(v=vs.110).aspx says Use comparisons with StringComparison.Ordinal or StringComparison.OrdinalIgnoreCase for better performance.
            var types = typesFromAllAssemblies.Where(t => t.Name.Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!types.Any())
            {
                throw new ArgumentException($"The Message-type '{type}' is not supported!");
            }

            if (types.Count() > 1)
            {
                throw new ArgumentException($"multiple types for typename '{type}' found!");
            }

            return types.First();
        }

        private static JsonSerializerOptions CopyJsonSerializerOptions(JsonSerializerOptions options)
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = options.PropertyNameCaseInsensitive,
                PropertyNamingPolicy = options.PropertyNamingPolicy,
                DefaultIgnoreCondition = options.DefaultIgnoreCondition,
            };
        }
    }
}
