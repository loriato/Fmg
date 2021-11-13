using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.ApiService.Models.DocumentoProponente
{
    public class DocumentoProponenteDto
    {
        public long Id { get; set; }
        public long IdTipoDocumento { get; set; }
        public string NomeTipoDocumento { get; set; }
        public long IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public long IdProponente { get; set; }
        public long IdPreProposta { get; set; }
        public long IdArquivo { get; set; }
        public SituacaoAprovacaoDocumento Situacao { get; set; }
        public string Motivo { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public long IdParecerDocumentoProponente { get; set; }
        public DateTime? DataCriacaoParecer { get; set; }
        public string Parecer { get; set; }
        public DateTime? DataValidadeParecer { get; set; }
        public bool Anexado { get; set; }

        public DocumentoProponenteDto FromDomain(ViewDocumentoProponente model)
        {
            Id = model.Id;
            IdTipoDocumento = model.IdTipoDocumento;
            NomeTipoDocumento = model.NomeTipoDocumento;
            IdCliente = model.IdCliente;
            NomeCliente = model.NomeCliente;
            IdProponente = model.IdProponente;
            IdPreProposta = model.IdPreProposta;
            IdArquivo = model.IdArquivo;
            Situacao = model.Situacao;
            Motivo = model.Motivo;
            DataExpiracao = model.DataExpiracao;
            IdParecerDocumentoProponente = model.IdParecerDocumentoProponente;
            DataCriacaoParecer = model.DataCriacaoParecer;
            Parecer = model.Parecer;
            DataValidadeParecer = model.DataValidadeParecer;
            Anexado = model.Anexado;

            return this;
        }
    }
}
