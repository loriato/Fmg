using System;
using System.Web;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class DocumentoAvalistaDTO
    {
        public long IdDocumentoAvalista { get; set; }
        public long IdPreProposta { get; set; }
        public long IdAvalista { get; set; }
        public long IdTipoDocumento { get; set; }
        public string Motivo { get; set; }
        public string NomeTipoDocumento { get; set; }
        public string NomeAvalista { get; set; }
        public bool ExisteDocumento { get; set; }
        public DateTime DataExpiracao { get; set; }
        public string Parecer { get; set; }

    }
}
