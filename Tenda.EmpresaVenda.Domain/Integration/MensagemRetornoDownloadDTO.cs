using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tenda.EmpresaVenda.Domain.Integration
{
    public class MensagemRetornoDownloadDTO
    {
        [JsonProperty("Sucesso")]
        public virtual bool Sucesso { get; set; }
        [JsonProperty("Objeto")]
        public virtual DownloadDTO Objeto { get; set; }
        [JsonProperty("Mensagens")]
        public virtual List<string> Mensagens { get; set; }
        [JsonProperty("Campos")]
        public virtual List<string> Campos { get; set; }

        public MensagemRetornoDownloadDTO()
        {
            Sucesso = false;
            Mensagens = new List<string>();
            Campos = new List<string>();
            Objeto = null;
        }
    }
}
