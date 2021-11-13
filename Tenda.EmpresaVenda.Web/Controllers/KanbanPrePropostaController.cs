using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class KanbanPrePropostaController : BaseController
    {
        public ActionResult Index()
        {
            var kanbanDto = EmpresaVendaApi.IndexKanbanPreProposta();
            return View(kanbanDto);
        }

        [HttpPost]
        public JsonResult SalvarAreaKanbanPreProposta(AreaKanbanPrePropostaDto areaKanbanPrePropostaDto)
        {
            var response = EmpresaVendaApi.SalvarAreaKanbanPreProposta(areaKanbanPrePropostaDto);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BuscarAreaKanbanPreProposta(long idAreaKanbanPreProposta)
        {
            var response = EmpresaVendaApi.BuscarAreaKanbanPreProposta(idAreaKanbanPreProposta);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarCardKanbanPreProposta(CardKanbanPrePropostaDto cardKanbanPrePropostaDto)
        {
            var response = EmpresaVendaApi.SalvarCardKanbanPreProposta(cardKanbanPrePropostaDto);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BuscarCardKanbanPreProposta(long idCardKanbanPreProposta)
        {
            var response = EmpresaVendaApi.BuscarCardKanbanPreProposta(idCardKanbanPreProposta);
            return Json(response, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public ActionResult ListarCardsKanbanPreProposta(long idAreaKanbanPreProposta)
        {
            var result = EmpresaVendaApi.ListarCardsKanbanPreProposta(idAreaKanbanPreProposta);
            return PartialView("_CardsKanbanPreProposta", result);
        }

        [HttpPost]
        public ActionResult ExcluirAreaKanbanPreProposta(long idAreaKanbanPreProposta)
        {
            var response = EmpresaVendaApi.ExcluirAreaKanbanPreProposta(idAreaKanbanPreProposta);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarSituacaoCardKanban(DataSourceRequest request,FiltroKanbanPrePropostaDto filtro)
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

        [HttpPost]
        public JsonResult AdicionarSituacaoCardKanban(CardKanbanSituacaoPrePropostaDto cardKanbanSituacaoPrePropostaDto)
        {
            var response = EmpresaVendaApi.AdicionarSituacaoCardKanban(cardKanbanSituacaoPrePropostaDto);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoverSituacaoCardKanban(long idCardKanbanSituacaoPreProposta)
        {
            var response = EmpresaVendaApi.RemoverSituacaoCardKanban(idCardKanbanSituacaoPreProposta);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoverCardKanbanPreProposta(long idCardKanbanPreProposta)
        {
            var response = EmpresaVendaApi.RemoverCardKanbanPreProposta(idCardKanbanPreProposta);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}