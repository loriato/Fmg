using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class StaticResourceService : BaseService
    {
#pragma warning disable S1075 // URIs should not be hardcoded
        /// <summary>
        /// O caminho relativo de onde estarão os recursos que poderão ser copiados para dar suporte ao conteúdo estático
        /// </summary>
        private static readonly string UrlDefaultResourcePath = @"\static\images\";
#pragma warning restore S1075 // URIs should not be hardcoded

        private ArquivoRepository _arquivoRepository { get; set; }

        public string LoadResource(long resourceId)
        {
            Arquivo arquivo = _arquivoRepository.WithNoContentAndNoThumbnail(resourceId);
            _session.Evict(arquivo);

            string fileName = arquivo.Hash + arquivo.FileExtension;

            // File Not Found
            if (fileName.IsEmpty())
            {
                return "";
            }

            string targetFilePath = VirtualDirectoryFile.PhysicalPath + fileName;

            if (!File.Exists(targetFilePath))
            {
                arquivo = _arquivoRepository.FindById(resourceId);

                File.WriteAllBytes(targetFilePath, arquivo.Content);
                // make a thumbnail

                string thumbnailFilePath = VirtualDirectoryThumbail.PhysicalPath + fileName;
                if (!File.Exists(thumbnailFilePath))
                {

                    if (arquivo.ContentType.ToLower().Contains("image") && arquivo.Thumbnail != null && arquivo.Thumbnail.Length > 0)
                    {
                        File.WriteAllBytes(thumbnailFilePath, arquivo.Thumbnail);
                    }
                    else
                    {
                        var pathFor = PathFor(arquivo.FileExtension.Replace(".", ""));
                        if (pathFor.IsEmpty()) { pathFor = "text"; }
                        File.Copy(pathFor, thumbnailFilePath);
                    }
                }
            }
            return fileName;
        }

        public void ValidarPhysicalDirectoryFile()
        {
            BusinessRuleException bre = new BusinessRuleException();

            if (PhysicalDirectoryFile.Unprocessed.IsEmpty())
            {
                bre.AddError(GlobalMessages.ParametroInexistente).WithParam(PhysicalDirectoryFile.Unprocessed).Complete();
            }
            if (PhysicalDirectoryFile.Error.IsEmpty())
            {
                bre.AddError(GlobalMessages.ParametroInexistente).WithParam(PhysicalDirectoryFile.Error).Complete();
            }
            if (PhysicalDirectoryFile.Processed.IsEmpty())
            {
                bre.AddError(GlobalMessages.ParametroInexistente).WithParam(PhysicalDirectoryFile.Processed).Complete();
            }

            if (PhysicalDirectoryFile.Logs.IsEmpty())
            {
                bre.AddError(GlobalMessages.ParametroInexistente).WithParam(PhysicalDirectoryFile.Logs).Complete();
            }

            if (!Directory.Exists(PhysicalDirectoryFile.Unprocessed))
            {
                bre.AddError(GlobalMessages.DiretorioInexistente).WithParam(PhysicalDirectoryFile.Unprocessed).Complete();
            }

            if (!Directory.Exists(PhysicalDirectoryFile.Error))
            {
                bre.AddError(GlobalMessages.DiretorioInexistente).WithParam(PhysicalDirectoryFile.Error).Complete();
            }

            if (!Directory.Exists(PhysicalDirectoryFile.Processed))
            {
                bre.AddError(GlobalMessages.DiretorioInexistente).WithParam(PhysicalDirectoryFile.Processed).Complete();
            }

            if (!Directory.Exists(PhysicalDirectoryFile.Logs))
            {
                bre.AddError(GlobalMessages.DiretorioInexistente).WithParam(PhysicalDirectoryFile.Logs).Complete();
            }
            bre.ThrowIfHasError();
        }

        public List<FileInfo> SearchFiles()
        {

            var extensao = "xlsx";

            DirectoryInfo directory = new DirectoryInfo(PhysicalDirectoryFile.Unprocessed);

            var files = directory.GetFiles().Where(x => x.Extension.Contains(extensao));

            return files.ToList();
        }

        public string UploadFile(Arquivo arquivo)
        {

            string fileName = arquivo.Hash + arquivo.FileExtension;

            // File Not Found
            if (fileName.IsEmpty())
            {
                return "";
            }

            string targetFilePath = VirtualDirectoryFile.PhysicalPath + fileName;

            if (!File.Exists(targetFilePath))
            {

                File.WriteAllBytes(targetFilePath, arquivo.Content);
                // make a thumbnail

                string thumbnailFilePath = VirtualDirectoryThumbail.PhysicalPath + fileName;
                if (!File.Exists(thumbnailFilePath))
                {

                    if (arquivo.ContentType.ToLower().Contains("image") && arquivo.Thumbnail != null && arquivo.Thumbnail.Length > 0)
                    {
                        File.WriteAllBytes(thumbnailFilePath, arquivo.Thumbnail);
                    }
                    else
                    {
                        var pathFor = PathFor(arquivo.FileExtension.Replace(".", ""));
                        if (pathFor.IsEmpty()) { pathFor = "text"; }
                        File.Copy(pathFor, thumbnailFilePath);
                    }
                }
            }
            return fileName;
        }

        public string CreateUrlApi(string fileName)
        {
            var webRoot = ConfigurationWrapper.GetStringProperty("url_api_empresa_venda");
            return webRoot + VirtualDirectoryFile.Path + fileName;
        }

        public string CreateThumbnailUrlApi(string fileName)
        {
            var webRoot = ConfigurationWrapper.GetStringProperty("url_api_empresa_venda");
            return webRoot + VirtualDirectoryThumbail.Path + fileName;
        }

        public string CreateUrl(string webRoot, string fileName)
        {
            return webRoot + VirtualDirectoryFile.Path + fileName;
        }

        public string CreateUrl(string fileName)
        {
            return VirtualDirectoryFile.PhysicalPath + fileName;
        }

        public string CreateThumbnailUrl(string webRoot, string fileName)
        {
            return webRoot + VirtualDirectoryThumbail.Path + fileName;
        }

        public string CreateImageUrl(string webRoot, string fileName)
        {
            return webRoot + VirtualDirectoryFile.Path + fileName;
        }

        private static string PathFor(string resourceType)
        {
            if (resourceType.IsEmpty())
            {
                resourceType = "text";
            }
            return HostingEnvironment.ApplicationPhysicalPath + UrlDefaultResourcePath + resourceType + ".png";
        }


        /// <summary>
        /// Caminho no disco que representa o diretório onde ficarão os thubmnails de imagens e arquivos estáticos
        /// É obrigatório o parâmetro
        /// </summary>
        private VirtualDirectoryDto VirtualDirectoryThumbail { get { return ProjectProperties.VirtualDirectoryThumbnail; } }

        /// <summary>
        /// Caminho no disco que representa o diretório onde ficarão as imagens e arquivos estáticos
        /// É obrigatório o parâmetro
        /// </summary>
        private VirtualDirectoryDto VirtualDirectoryFile { get { return ProjectProperties.VirtualDirectoryFile; } }

        private PhysicalDirectoryFileDTO PhysicalDirectoryFile { get { return ProjectProperties.PhysicalDirectoryFile; } }

    }
}
