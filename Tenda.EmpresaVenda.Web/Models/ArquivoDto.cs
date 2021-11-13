using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class ArquivoDTO
    {
        public HttpPostedFileBase File { get; set; }
        public long TargetId { get; set; }
        public String YoutubeVideoCode { get; set; }
    }
}