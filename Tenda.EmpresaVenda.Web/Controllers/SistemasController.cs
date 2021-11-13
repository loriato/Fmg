using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Security.Services;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class SistemasController : BaseController
    {
        private SistemaService _sistemaService { get; set; }
        public ActionResult Listar(DataSourceRequest request)
        {
            var result = _sistemaService.Listar(request);
            return Json(result.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }
    }
}