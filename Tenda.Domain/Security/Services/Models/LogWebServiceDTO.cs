using System;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.Security.Services.Models
{
    public class LogWebServiceDTO
    {
        public string Endpoint { get; set; }
        public string Operacao { get; set; }
        public DateTime HorarioInicio { get; set; }
        public DateTime HorarioFim { get; set; }
        public string Conteudo { get; set; }
        public SoapMessageStage Stage { get; set; }
    }
}
