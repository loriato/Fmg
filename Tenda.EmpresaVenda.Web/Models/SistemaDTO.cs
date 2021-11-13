using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.Security.Models;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class SistemaDTO
    {
        public Sistema Sistema { get; set; }
        public List<Sistema> Sistemas { get; set; }
    }
}