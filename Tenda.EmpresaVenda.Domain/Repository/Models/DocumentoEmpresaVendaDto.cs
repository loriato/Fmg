using System.Web;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class DocumentoEmpresaVendaDto
    {
        public long Id { get; set; }
        public long IdEmpresaVenda { get; set; }
        public string NomeEmpresaVenda { get; set; }
        public long IdTipoDocumento { get; set; }
        public string NomeTipoDocumento { get; set; }
        public SituacaoAprovacaoDocumento Situacao { get; set; }
        public string NomeArquivo { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}
