using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class FotoEmpresaVendaDTO
    {
        public HttpPostedFileBase Foto { get; set; }
        public long IdEmpresaVenda { get; set; }
    }
}