using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class BancoController : BaseController
    {
        private BancoRepository _bancoRepository { get; set; }

        [HttpGet]
        public JsonResult Listar(DataSourceRequest request)
        {
            var result = _bancoRepository.Listar();
            return Json(result.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }
    }
}