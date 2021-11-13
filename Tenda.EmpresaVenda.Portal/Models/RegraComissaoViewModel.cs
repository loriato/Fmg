using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Portal.Models
{
    public class RegraComissaoViewModel
    {
        public string RegraComissao { get; set; }
        public List<SelectListItem> RegrasAceitas { get; set; }
        public long IdRegraAceiteMaisRecente { get; set; }
        public RegraComissaoEvs RegraComissaoEvs { get; set; }
        public RegraComissaoEvs RegraComissaoEvsSuspensa { get; set; }
    }
}