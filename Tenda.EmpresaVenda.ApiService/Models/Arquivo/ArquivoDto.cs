using Tenda.EmpresaVenda.ApiService.Models.StaticResource;

namespace Tenda.EmpresaVenda.ApiService.Models.Arquivo
{
    public class ArquivoDto
    {
        public string Nome { get; set; }
        public string Url { get; set; }
        public string UrlThumbnail { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
        public ArquivoDto FromDomain(Tenda.Domain.EmpresaVenda.Models.Arquivo model)
        {
            Nome = model.Nome;
            FileExtension = model.FileExtension;
            ContentType = model.ContentType;
            return this;
        }
        
        public void FromDomainVideo(Tenda.Domain.EmpresaVenda.Models.Views.ViewArquivoBreveLancamento arquivo)
        {
            Nome = arquivo.Nome;
            UrlThumbnail = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
            Url = UrlThumbnail;
            FileExtension = arquivo.FileExtension;
            ContentType = arquivo.ContentType;
        }
        
        public void FromDomainVideo(Tenda.Domain.EmpresaVenda.Models.Views.ViewArquivoEmpreendimento arquivo)
        {
            Nome = arquivo.Nome;
            UrlThumbnail = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
            Url = UrlThumbnail;
            FileExtension = arquivo.FileExtension;
            ContentType = arquivo.ContentType;
        }
        
        public void FromDto(StaticResourceDTO dto, string contentType, string fileExtension)
        {
            UrlThumbnail = dto.UrlThumbnail;
            Url = dto.Url;
            ContentType = contentType;
            FileExtension = fileExtension;
        }

        public bool IsVideo()
        {
            return ContentType?.ToLower().Contains("video") ?? false;
        }
    }    
}