using System;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class DocumentoProponenteDTO
    {
        public long IdDocumentoProponente { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public string Motivo { get; set; }
        public string Parecer { get; set; }
        public long IdPreProposta { get; set; }
        public string TipoDocumento { get; set; }
        public bool ExisteDocumento { get; set; }
    }
}