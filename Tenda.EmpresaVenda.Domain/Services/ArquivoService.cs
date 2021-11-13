using Europa.Commons;
using Newtonsoft.Json;
using Europa.Extensions;
using OfficeOpenXml;
using PdfSharp.Pdf.IO;
using System;
using System.IO;
using System.Linq;
using System.Web;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared.Commons;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using System.Collections.Generic;
using Europa.Data;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class ArquivoService : BaseService
    {
        public ArquivoRepository _arquivoRepository { get; set; }

        public Arquivo CreateFile(HttpPostedFileBase file)
        {
            return CreateFile(file, null);
        }

        public Arquivo CreateFile(HttpPostedFileBase file, string nomeEspecifico)
        {
            Arquivo arquivo = new Arquivo();
            arquivo.Nome = file.FileName;
            if (!nomeEspecifico.IsEmpty())
            {
                arquivo.Nome = nomeEspecifico;
            }
            arquivo.ContentType = file.ContentType;
            arquivo.ContentLength = file.ContentLength;
            arquivo.FileExtension = MimeMappingWrapper.GetDefaultExtension(file.ContentType);

            // Tentando resolver a extensão do arquivo caso o MimeMapping não o diga
            if (arquivo.FileExtension.IsEmpty()) { arquivo.FileExtension = MimeMappingWrapper.GetExtensionFromName(arquivo.Nome); }
            if (arquivo.FileExtension.IsEmpty()) { arquivo.FileExtension = MimeMappingWrapper.GetExtensionFromName(file.FileName); }

            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                arquivo.Content = binaryReader?.ReadBytes(file.ContentLength);
                // make a thumbnail
                if (arquivo.ContentType.ToLower().Contains("image"))
                {
                    arquivo.Thumbnail = ImageHelper.MakeThumbnail(arquivo.Content, 128);
                }
            }
            arquivo.Hash = HashUtil.SHA256(arquivo.Content);
            _arquivoRepository.Save(arquivo);
            return arquivo;
        }

        public Arquivo CreateFile(Arquivo arquivo, string nomeEspecifico)
        {
            if (!nomeEspecifico.IsEmpty())
            {
                arquivo.Nome = nomeEspecifico;
            }

            arquivo.Hash = HashUtil.SHA256(arquivo.Content);
            arquivo.FileExtension = MimeMappingWrapper.GetDefaultExtension(arquivo.ContentType);

            // Tentando resolver a extensão do arquivo caso o MimeMapping não o diga
            if (arquivo.FileExtension.IsEmpty())
            {
                arquivo.FileExtension = MimeMappingWrapper.GetExtensionFromName(arquivo.Nome);
            }

            // make a thumbnail
            if (arquivo.ContentType.ToLower().Contains("image"))
            {
                arquivo.Thumbnail = ImageHelper.MakeThumbnail(arquivo.Content, 200);
            }

            _arquivoRepository.Save(arquivo);
            return arquivo;
        }

        public Arquivo CreateFile(byte[] bytes, string nomeSalvar, string nomeArquivo, string contentType, int contentLength)
        {
            Arquivo arquivo = new Arquivo();
            arquivo.Nome = nomeSalvar;
            arquivo.ContentType = contentType;
            arquivo.ContentLength = contentLength;
            arquivo.FileExtension = MimeMappingWrapper.GetDefaultExtension(contentType);

            // Tentando resolver a extensão do arquivo caso o MimeMapping não o diga
            if (arquivo.FileExtension.IsEmpty()) { arquivo.FileExtension = MimeMappingWrapper.GetExtensionFromName(arquivo.Nome); }
            if (arquivo.FileExtension.IsEmpty()) { arquivo.FileExtension = MimeMappingWrapper.GetExtensionFromName(nomeArquivo); }

            arquivo.Content = bytes;
            // make a thumbnail
            if (arquivo.ContentType.ToLower().Contains("image"))
            {
                arquivo.Thumbnail = ImageHelper.MakeThumbnail(arquivo.Content, 128);
            }

            arquivo.Hash = HashUtil.SHA256(arquivo.Content);
            _arquivoRepository.Save(arquivo);
            return arquivo;
        }

        public long TotalLinhas(Arquivo arquivo)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(arquivo.Content, 0, arquivo.ContentLength);
            ms.Position = 0;
            ExcelPackage package = new ExcelPackage(ms);
            ExcelWorkbook workbook = package.Workbook;
            ExcelWorksheet worksheet = workbook.Worksheets.First();
            long rowCount = worksheet.Dimension.Rows;
            ms.Close();

            return rowCount - 1;
        }

        public void PreencherMetadadosDePdf(ref Arquivo arquivo)
        {
            var stream = new MemoryStream(arquivo.Content);

            try
            {
                var documentoArquivo = PdfReader.Open(stream);
                documentoArquivo.Info.Elements.Remove("/AAPL:Keywords");
                var keywords = documentoArquivo.Info.Keywords;
                documentoArquivo.Info.Elements.Remove("/Keywords");

                var objMetadados = new Metadado();

                objMetadados.Titulo = documentoArquivo.Info.Title;
                objMetadados.Autor = documentoArquivo.Info.Author;
                objMetadados.Assunto = documentoArquivo.Info.Subject;
                objMetadados.CriadoPor = documentoArquivo.Info.Creator;
                objMetadados.CriadoEm = documentoArquivo.Info.CreationDate.IsEmpty() ? "" : documentoArquivo.Info.CreationDate.ToDateTimeSeconds();
                objMetadados.ModificadoEm = documentoArquivo.Info.ModificationDate.IsEmpty() ? "" : documentoArquivo.Info.ModificationDate.ToDateTimeSeconds();
                objMetadados.ProduzidoPor = documentoArquivo.Info.Producer;
                objMetadados.PalavrasChave = keywords;
                objMetadados.ExtraidoPor = "PdfSharp";

                var jsonSettings = SecurityEntitySerializer.Current();

                objMetadados.Raw = JsonConvert.SerializeObject(documentoArquivo.Info, jsonSettings);

                var metadadosSerializados = objMetadados.IsEmpty() ? "" : JsonConvert.SerializeObject(objMetadados, Formatting.Indented);
                arquivo.Metadados = metadadosSerializados;
            }
            catch (Exception e)
            {
                //FIX-ME: azendo by pass na existencia de erro no SerializeObject
                if (!e.Message.Contains("JsonPropertyAttribute"))
                {
                    ExceptionUtility.LogException(e);                    
                }

                arquivo.Metadados = "";
                arquivo.FalhaExtracaoMetadados = true;
            }
        }

        public Arquivo CreateFile(HttpPostedFileBase file, Arquivo arquivo, string nomeEspecifico)
        {
            if (arquivo.IsEmpty())
            {
                arquivo = new Arquivo();
            }

            arquivo.Nome = file.FileName;
            if (!nomeEspecifico.IsEmpty())
            {
                arquivo.Nome = nomeEspecifico;
            }
            arquivo.ContentType = file.ContentType;
            arquivo.ContentLength = file.ContentLength;
            arquivo.FileExtension = MimeMappingWrapper.GetDefaultExtension(file.ContentType);

            // Tentando resolver a extensão do arquivo caso o MimeMapping não o diga
            if (arquivo.FileExtension.IsEmpty()) { arquivo.FileExtension = MimeMappingWrapper.GetExtensionFromName(arquivo.Nome); }
            if (arquivo.FileExtension.IsEmpty()) { arquivo.FileExtension = MimeMappingWrapper.GetExtensionFromName(file.FileName); }

            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                arquivo.Content = binaryReader?.ReadBytes(file.ContentLength);
                // make a thumbnail
                if (arquivo.ContentType.ToLower().Contains("image"))
                {
                    arquivo.Thumbnail = ImageHelper.MakeThumbnail(arquivo.Content, 128);
                }
            }
            arquivo.Hash = HashUtil.SHA256(arquivo.Content);
            _arquivoRepository.Save(arquivo);
            return arquivo;
        }
    }
}
