using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.ApiService.Models.Util
{
    public class FileDto
    {
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public string NomeArquivo { get; set; }

        public Tenda.Domain.EmpresaVenda.Models.Arquivo ToDomain()
        {
            var model = new Tenda.Domain.EmpresaVenda.Models.Arquivo();
            model.Content = Bytes;
            model.ContentLength = Bytes?.Length ?? 0;
            model.ContentType = ContentType;
            model.Nome = FileName;
            return model;
        }

        public FileDto FromHttpFile(HttpPostedFileBase file)
        {
            FileName = file.FileName;
            ContentType = file.ContentType;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                Bytes = binaryReader.ReadBytes(file.ContentLength);
            }

            return this;
        }

        public void FromDomain(Tenda.Domain.EmpresaVenda.Models.Arquivo arquivo)
        {
            Bytes = arquivo.Content;
            FileName = arquivo.Nome;
            Extension = arquivo.FileExtension;
            ContentType = arquivo.ContentType;
        }
    }
}
