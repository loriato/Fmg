using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tenda.EmpresaVenda.Domain.Integration
{
    public class SuatErrorList
    {
        [JsonProperty("errors")]
        public List<SuatError> Errors { get; set; }
    }
}
