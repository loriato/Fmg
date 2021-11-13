using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.ApiService.Models.AgrupamentoProcessoPreProposta;
using Tenda.EmpresaVenda.PortalHouse.Models.Application;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class AgrupamentoProcessoPrePropostaController : BaseController
    {
        [HttpGet]
        public ActionResult Listar(DataSourceRequest request, AgrupamentoProcessoPrePropostaFiltro filtro)
        {
            filtro.DataSourceRequest = request;
            filtro.CodigoSistema = ApplicationInfo.CodigoSistema;
            var result = EmpresaVendaApi.AutoCompletePrePropostaHouseAgrupamentoProcessoPreProposta(filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}