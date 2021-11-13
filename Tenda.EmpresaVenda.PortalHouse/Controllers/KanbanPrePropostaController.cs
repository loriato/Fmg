using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.ApiService.Models.Avalista;
using Tenda.EmpresaVenda.ApiService.Models.DocumentoProponente;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;
using Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta;
using Tenda.EmpresaVenda.ApiService.Models.PlanoPagamento;
using Tenda.EmpresaVenda.ApiService.Models.PreProposta;
using Tenda.EmpresaVenda.ApiService.Models.Proponente;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.PortalHouse.Models.Application;
using Tenda.EmpresaVenda.PortalHouse.Security;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class KanbanPrePropostaController : BaseController
    {
        // GET: KanbanPreProposta
        public ActionResult Index()
        {
            var kanbanDto = EmpresaVendaApi.IndexKanbanPreProposta();
            return View(kanbanDto);
        }

        [HttpGet]
        public JsonResult BuscarAreaKanbanPreProposta(long idAreaKanbanPreProposta)
        {
            var response = EmpresaVendaApi.BuscarAreaKanbanPreProposta(idAreaKanbanPreProposta);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BuscarCardKanbanPreProposta(long idCardKanbanPreProposta)
        {
            var response = EmpresaVendaApi.BuscarCardKanbanPreProposta(idCardKanbanPreProposta);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CarregaNivel2Kanban(long idAreaKanbanPreProposta)
        {
            var CardsKanbanPrePropostaDto = EmpresaVendaApi.ListarCardsKanbanPreProposta(idAreaKanbanPreProposta);
            return PartialView("_CardsKanbanPreProposta", CardsKanbanPrePropostaDto);
        }

        [HttpGet]
        public ActionResult FormularioAreaKanbanPreProposta(long idAreaKanbanPreProposta)
        {
            var result = EmpresaVendaApi.BuscarAreaKanbanPreProposta(idAreaKanbanPreProposta);
            return PartialView("_FormularioAreaKanbanPreProposta", result);
        }

        [HttpGet]
        public ActionResult FormularioCardKanbanPreProposta(long idCardKanbanPreProposta, long? idAreaKanbanPreProposta)
        {
            var result = EmpresaVendaApi.BuscarCardKanbanPreProposta(idCardKanbanPreProposta);

            if (idAreaKanbanPreProposta.HasValue())
            {
                result.IdAreaKanban = idAreaKanbanPreProposta.Value;
            }

            return PartialView("_FormularioCardKanbanPreProposta", result);
        }

        [HttpPost]
        public ActionResult ListarCardsKanbanPreProposta(DataSourceRequest request, FiltroKanbanPrePropostaDto filtro)
        {
            var result = EmpresaVendaApi.ListarCardsComPreProposta(filtro);

            return PartialView("_CardPreProposta", result);
        }


        [HttpPost]
        public JsonResult ListarSituacaoCardKanban(DataSourceRequest request, FiltroKanbanPrePropostaDto filtro)
        {
            filtro.DataSourceRequest = request;

            var response = EmpresaVendaApi.ListarSituacaoCardKanban(filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AutoCompleteSituacaoKanbanPreProposta(DataSourceRequest request)
        {
            var filtro = new FiltroKanbanPrePropostaDto();
            filtro.DataSourceRequest = request;

            var response = EmpresaVendaApi.AutoCompleteSituacaoKanbanPreProposta(filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}