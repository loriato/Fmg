using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tenda.EmpresaVenda.Domain.Integration.Suat.Models
{
    public class MensagemRetornoPropostaDTO
    {
        [JsonProperty("Sucesso")]
        public virtual bool Sucesso { get; set; }
        [JsonProperty("Objeto")]
        public virtual PropostaDTO Objeto { get; set; }
        [JsonProperty("Mensagens")]
        public virtual List<string> Mensagens { get; set; }
        [JsonProperty("Campos")]
        public virtual List<string> Campos { get; set; }

        public MensagemRetornoPropostaDTO()
        {
            Sucesso = false;
            Mensagens = new List<string>();
            Campos = new List<string>();
            Objeto = null;
        }
    }
}
