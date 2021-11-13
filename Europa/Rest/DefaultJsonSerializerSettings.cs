using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Europa.Rest
{
    public static class DefaultJsonSerializerSettings
    {
        public static JsonSerializerSettings Default => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };
    }
}
