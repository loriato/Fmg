using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.DocumentoProponente
{
    public class DocumentoProponenteCreateDto
    {
        public DocumentoProponenteDto Documento { get; set; }
        public FileDto File { get; set; }
    }
}
