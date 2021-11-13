using Newtonsoft.Json;

namespace Tenda.EmpresaVenda.Api.Models
{
    public class ApiVersion
    {
        [JsonProperty("version-info")]
        public string Version { get; set; }
    }
}