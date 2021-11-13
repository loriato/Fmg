using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.Security.Models;

namespace Tenda.EmpresaVenda.Portal.Models
{
    public class ParametroDto : Parametro
    {
        public string Detalhe { get; set; }
    }
}