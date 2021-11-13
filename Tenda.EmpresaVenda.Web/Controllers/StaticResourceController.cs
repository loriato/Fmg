using Europa.Commons;
using Europa.Extensions;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared.Commons;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Web.Models;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class StaticResourceController : BaseController
    {
        private static readonly string TargetPath = @"/static/projeto/resources/";
        private static readonly string ThumbnailTargetPath = @"/static/projeto/resources/thumbs/";

        private ArquivoRepository _arquivoRepository { get; set; }


        public ActionResult TryLoadResource(long resourceId)
        {
            Arquivo arquivo = _arquivoRepository.WithNoContentAndNoThumbnail(resourceId);
            _session.Evict(arquivo);

            string fileName = arquivo.Hash + arquivo.FileExtension;

            // File Not Found
            if (fileName.IsEmpty())
            {
                return null;
            }

            string targetFilePath = HostingEnvironment.ApplicationPhysicalPath + TargetPath + fileName;

            if (!System.IO.File.Exists(targetFilePath))
            {
                arquivo = _arquivoRepository.FindById(resourceId);

                System.IO.File.WriteAllBytes(targetFilePath, arquivo.Content);
                // make a thumbnail
                if (arquivo.ContentType.ToLower().Contains("image") && arquivo.Thumbnail != null && arquivo.Thumbnail.Length > 0)
                {
                    string thumbnailFilePath = HostingEnvironment.ApplicationPhysicalPath + ThumbnailTargetPath + fileName;
                    System.IO.File.WriteAllBytes(thumbnailFilePath, arquivo.Thumbnail);
                }
            }
            return RedirectPermanent(GetWebAppRoot() + TargetPath + fileName);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Upload(FotoEmpresaVendaDTO dto)
        {
            Arquivo arquivo = new Arquivo();
            arquivo.Nome = dto.Foto.FileName;
            arquivo.ContentType = dto.Foto.ContentType;
            arquivo.ContentLength = dto.Foto.ContentLength;
            arquivo.FileExtension = MimeMappingWrapper.GetDefaultExtension(dto.Foto.ContentType);

            using (var binaryReader = new BinaryReader(dto.Foto.InputStream))
            {
                arquivo.Content = binaryReader?.ReadBytes(dto.Foto.ContentLength);
                // make a thumbnail
                if (arquivo.ContentType.ToLower().Contains("image"))
                {
                    arquivo.Thumbnail = ImageHelper.MakeThumbnail(arquivo.Content, 128);
                }
            }
            arquivo.Hash = HashUtil.SHA256(arquivo.Content);
            _arquivoRepository.Save(arquivo);
            return Json(arquivo.Id, JsonRequestBehavior.AllowGet);
        }
    }
}