using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Portal.Models
{
    public class FotoEmpresaVendaDTO
    {
        public HttpPostedFileBase Foto { get; set; }
        public long IdEmpresaVenda { get; set; }
    }
}