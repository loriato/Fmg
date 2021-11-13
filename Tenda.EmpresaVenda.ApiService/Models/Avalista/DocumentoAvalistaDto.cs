using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.ApiService.Models.Avalista
{
    public class DocumentoAvalistaDto
    {
        public long Id { get; set; }
        public long IdPreProposta { get; set; }
        public long IdArquivo { get; set; }
        public long IdTipoDocumento { get; set; }
        public SituacaoAprovacaoDocumento Situacao { get; set; }
        public string NomeAvalista { get; set; }
        public long IdAvalista { get; set; }
        public string NomeTipoDocumento { get; set; }
        public string Motivo { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public long IdParecerDocumentoProponente { get; set; }
        public DateTime? DataCriacaoParecer { get; set; }
        public string Parecer { get; set; }
        public DateTime? DataValidadeParecer { get; set; }
        public bool Anexado { get; set; }
    }
}
