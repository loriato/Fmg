namespace Tenda.EmpresaVenda.ApiService.Models.DocumentoProponente
{
    public class DocumentoProponenteRepasseDto
    {
        public virtual string NomeTipoDocumento { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual long IdArquivo { get; set; }
        public virtual string Url { get; set; }
        public virtual string UrlThumbnail { get; set; }
        public virtual string FileExtension { get; set; }
    }
}
