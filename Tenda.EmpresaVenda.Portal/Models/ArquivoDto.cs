using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tenda.EmpresaVenda.Portal.Models
{
    public class ArquivoDto
    {
        public string Nome { get; set; }
        public string Url { get; set; }
        public string UrlThumbnail { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
    }
}