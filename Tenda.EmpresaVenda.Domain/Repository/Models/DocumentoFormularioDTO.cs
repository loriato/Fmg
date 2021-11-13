using System.Web;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class DocumentoFormularioDTO
    {
        public long Id { get; set; }
        public long IdPreProposta { get; set; }
        public long IdResponsavel { get; set; }
        public string NomeDocumento { get; set; }
        public HttpPostedFileBase Formulario { get; set; }
    }
}
