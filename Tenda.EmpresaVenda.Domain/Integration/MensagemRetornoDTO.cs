using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tenda.EmpresaVenda.Domain.Integration
{
    public class MensagemRetornoDTO
    {
        [JsonProperty("Sucesso")]
        public virtual bool Sucesso { get; set; }
        [JsonProperty("Objeto")]
        public virtual object Objeto { get; set; }
        [JsonProperty("Mensagens")]
        public virtual List<string> Mensagens { get; set; }
        [JsonProperty("Campos")]
        public virtual List<string> Campos { get; set; }

        public MensagemRetornoDTO()
        {
            Sucesso = false;
            Mensagens = new List<string>();
            Campos = new List<string>();
            Objeto = null;
        }
    }

    //FIXME: Remover!
    public class ErrorList
    {
        public List<ErrorDTO> Errors { get; set; }
    }

    //FIXME: Remover!
    public class ErrorDTO
    {
        public string Message { get; set; }
    }

}
