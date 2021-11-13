using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Core.Enums;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Web.Security;
using Europa.Extensions;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class RegionaisController : BaseController
    {
        public RegionaisRepository _regionaisRepository { get; set; }
        // GET: Regionais
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Listar(DataSourceRequest request)
        {
            var results = _regionaisRepository.Listar(request);
            var json = Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

    }
}