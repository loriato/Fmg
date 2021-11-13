using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class RegraComissaoDTO
    {
        public long? Id { get; set; }
        public HttpPostedFileBase Arquivo { get; set; }
        public string Descricao { get; set; }
        public string Regional { get; set; }

    }
}