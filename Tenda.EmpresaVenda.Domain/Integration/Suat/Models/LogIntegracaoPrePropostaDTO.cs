using System;

namespace Tenda.EmpresaVenda.Domain.Integration.Suat.Models
{
    public class LogIntegracaoPrePropostaDTO
    {
        public long Id { get; set; }
        public long CriadoPor { get; set; }
        public DateTime CriadoEm { get; set; }
        public long IdProposta { get; set; }
        public string Codigo { get; set; }
        public string PreProposta { get; set; }
        public string Mensagem { get; set; }
    }
}
