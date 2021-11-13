using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Security;
using Tenda.Domain.Security.Models;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class ParametroRoboViewModel
    {
        public List<QuartzConfiguration> Lista { get; set; }
        public QuartzConfiguration Quartz { get; set; }
    }
}