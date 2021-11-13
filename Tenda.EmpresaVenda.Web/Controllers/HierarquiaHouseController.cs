using Europa.Extensions;
using Europa.Rest;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;
using Tenda.EmpresaVenda.ApiService.Models.Loja;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC19")]
    public class HierarquiaHouseController : BaseController
    {
        public ActionResult Index()
        {
            var hierarquiaHouseDto = new HierarquiaHouseDto();

            var isCoordenador = SessionAttributes.Current().Perfis.Where(x => x.Id == ProjectProperties.IdPerfilCoordenadorHouse).Any();
            var isSupervisor = SessionAttributes.Current().Perfis.Where(x => x.Id == ProjectProperties.IdPerfilSupervisorHouse).Any();
            var isSuperior = SessionAttributes.Current().Perfis.Where(x => x.Id == ProjectProperties.IdPerfilSuperiorHouse).Any();

            if (isCoordenador)
            {
                hierarquiaHouseDto.IdCoordenadorHouse = SessionAttributes.Current().UsuarioPortal.Id;
                hierarquiaHouseDto.NomeCoordenadorHouse = SessionAttributes.Current().UsuarioPortal.Nome;
            }
            
            if (isSupervisor)
            {
                hierarquiaHouseDto.IdSupervisorHouse = SessionAttributes.Current().UsuarioPortal.Id;
                hierarquiaHouseDto.NomeSupervisorHouse = SessionAttributes.Current().UsuarioPortal.Nome;
            }

            if (isSuperior)
            {
                hierarquiaHouseDto.IdCoordenadorHouse = 0;
                hierarquiaHouseDto.NomeCoordenadorHouse = null;
                hierarquiaHouseDto.IdSupervisorHouse = 0;
                hierarquiaHouseDto.NomeSupervisorHouse = null;
            }

            hierarquiaHouseDto.IsSuperiorHouse = isSuperior;

            return View(hierarquiaHouseDto);
        }

        [HttpPost]
        [BaseAuthorize("GEC19", "Visualizar")]
        public JsonResult ListarSupervisorHouse(DataSourceRequest request, FiltroHierarquiaHouseDto filtro)
        {
            filtro.Request = request;

            var result = EmpresaVendaApi.ListarSupervisorHouse(filtro);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("GEC19", "Visualizar")]
        public JsonResult ListarAgenteVendaHouse(DataSourceRequest request, FiltroHierarquiaHouseDto filtro)
        {
            filtro.Request = request;

            var result = EmpresaVendaApi.ListarAgenteVendaHouse(filtro);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("GEC19", "Visualizar")]
        public JsonResult ListarHierarquiaHouse(DataSourceRequest request, FiltroHierarquiaHouseDto filtro)
        {
            filtro.Request = request;

            var result = EmpresaVendaApi.ListarHierarquiaHouse(filtro);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("GEC19", "Incluir")]
        public JsonResult VincularCoordenadorSupervisor(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var response = EmpresaVendaApi.VincularCoordenadorSupervisor(hierarquiaHouseDto);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
                
        [HttpPost]
        [BaseAuthorize("GEC19", "Atualizar")]
        public JsonResult DesvincularCoordenadorSupervisor(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var response = EmpresaVendaApi.DesvincularCoordenadorSupervisor(hierarquiaHouseDto);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [BaseAuthorize("GEC19", "Incluir")]
        public JsonResult VincularSupervisorAgenteVendaHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var response = EmpresaVendaApi.VincularSupervisorAgenteVendaHouse(hierarquiaHouseDto);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("GEC19", "Atualizar")]
        public JsonResult DesvincularSupervisorAgenteVendaHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var response = EmpresaVendaApi.DesvincularSupervisorAgenteVendaHouse(hierarquiaHouseDto);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteSupervisorHouse(DataSourceRequest request)
        {
            var filtro = new FiltroHierarquiaHouseDto();
            filtro.Request = request;

            var response = EmpresaVendaApi.AutoCompleteSupervisorHouse(filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteAgenteVendaHouse(DataSourceRequest request)
        {
            var filtro = new FiltroHierarquiaHouseDto();
            filtro.Request = request;

            var response = EmpresaVendaApi.AutoCompleteAgenteVendaHouse(filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteHouse(DataSourceRequest request)
        {
            var filtro = new FiltroHierarquiaHouseDto();
            filtro.Request = request;

            var response = EmpresaVendaApi.AutoCompleteHouse(filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteLojaPortal(DataSourceRequest request)
        {
            var filtro = new FiltroLojaPortalDto();
            filtro.DataSourceRequest = request;

            var response = EmpresaVendaApi.AutoCompleteLojaPortal(filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteAgenteVendaHouseConsulta(DataSourceRequest request)
        {
            var filtro = new FiltroHierarquiaHouseDto();
            filtro.Request = request;

            var response = EmpresaVendaApi.AutoCompleteAgenteVendaHouseConsulta(filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AutoCompleteCoordenadorHouse(DataSourceRequest request)
        {
            var filtro = new FiltroHierarquiaHouseDto();
            filtro.Request = request;

            var response = EmpresaVendaApi.AutoCompleteCoordenadorHouse(filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}