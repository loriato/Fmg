using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("SEG18")]
    public class FilaEmailController : BaseController
    {
        private ViewFilaEmailRepository _viewEmailRepository { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("SEG18", "Visualizar")]
        public JsonResult ListarFilaEmail(DataSourceRequest request, FiltroEmailDTO filtro)
        {
            var response = _viewEmailRepository.ListarFilaEmail(request, filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}