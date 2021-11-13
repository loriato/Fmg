using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.Domain.Maps;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class HistoricoRegraComissaoController : BaseController
    {
        private ViewHistoricoRegraComissaoRepository _viewHistoricoRegraComissao { get; set; }
        // GET: HistoricoRegraComissao
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ListarHistorico(DataSourceRequest request,RegraComissaoDTO filtro)
        {
            var result = _viewHistoricoRegraComissao.ListarHistorico(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}