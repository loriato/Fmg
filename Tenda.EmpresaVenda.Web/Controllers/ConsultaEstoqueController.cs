using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.Domain.Integration.Suat;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class ConsultaEstoqueController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Empreendimento(DataSourceRequest request, string divisao)
        {
            // FIXME: Create cache for big request?
            var results = ConsultaEstoqueService.EstoqueEmpreendimento(request, divisao);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Unidade(DataSourceRequest request, string divisao, string caracteristicas, DateTime previsaoEntrega, long idTorre)
        {
            // FIXME: Create cache for big request?

            // Ticket - EDV-226
            // A partir de agora esta liberado o acesso ao estoque inteiro e não só da torre selecionado na PPR. Então idTorre está setado como -1
            idTorre = -1;
            
            var results = ConsultaEstoqueService.EstoqueUnidade(request, divisao, caracteristicas, previsaoEntrega, idTorre);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

    }
}