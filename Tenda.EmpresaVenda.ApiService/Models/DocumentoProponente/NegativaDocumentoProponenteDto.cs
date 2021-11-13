namespace Tenda.EmpresaVenda.ApiService.Models.DocumentoProponente
{
    public class NegativaDocumentoProponenteDto
    {
        public DocumentoProponenteDto Documento { get; set; }
        public long? IdMotivo { get; set; }
        public string DescricaoMotivo { get; set; }
    }
}
