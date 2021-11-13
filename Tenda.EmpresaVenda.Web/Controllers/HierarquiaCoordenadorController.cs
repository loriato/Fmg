using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC13")]
    public class HierarquiaCoordenadorController : BaseController
    {
        private ViewCoordenadorRepository _viewCoordenadorRepository { get; set; }
        private ViewCoordenadorSupervisorRepository _viewCoordenadorSupervisorRepository { get; set; }
        private ViewCoordenadorViabilizadorRepository _viewCoordenadorViabilizadorRepository { get; set; }
        private HierarquiaCoordenadorService _hierarquiaCoordenadorService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("GEC13", "Visualizar")]
        public JsonResult ListarCoordenadores(DataSourceRequest request, HierarquiaCicloFinanceiroDTO filtro)
        {
            var response = _viewCoordenadorRepository.Listar(request, filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC13", "Visualizar")]
        public JsonResult ListarSupervisores(DataSourceRequest request, HierarquiaCicloFinanceiroDTO filtro)
        {
            var response = _viewCoordenadorSupervisorRepository.ListarSupervisores(request, filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC13", "Visualizar")]
        public JsonResult ListarViabilizadores(DataSourceRequest request, HierarquiaCicloFinanceiroDTO filtro)
        {
            var response = _viewCoordenadorViabilizadorRepository.ListarViabilizadores(request, filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("GEC13", "Incluir")]
        public JsonResult OnJoinCoordenadorSupervisor(CoordenadorSupervisor coordenadorSupervisor)
        {
            var response = new JsonResponse();

            try
            {
                _hierarquiaCoordenadorService.OnJoinCoordenadorSupervisor(coordenadorSupervisor);
                response.Sucesso = true;
            }catch(BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("GEC13", "Remover")]
        public JsonResult OnUnJoinCoordenadorSupervisor(CoordenadorSupervisor coordenadorSupervisor)
        {
            var response = new JsonResponse();

            try
            {
                _hierarquiaCoordenadorService.OnUnJoinCoordenadorSupervisor(coordenadorSupervisor);
                response.Objeto = _hierarquiaCoordenadorService.HierarquiaAtivaCoordenadorSupervisor(coordenadorSupervisor);
                response.Sucesso = true;
            }
            catch(BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("GEC13", "Incluir")]
        public JsonResult OnJoinCoordenadorViabilizador(CoordenadorViabilizador coordenadorViabilizador)
        {
            var response = new JsonResponse();

            try
            {
                _hierarquiaCoordenadorService.OnJoinCoordenadorViabilizador(coordenadorViabilizador);
                response.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("GEC13", "Remover")]
        public JsonResult OnUnJoinCoordenadorViabilizador(CoordenadorViabilizador coordenadorViabilizador)
        {
            var response = new JsonResponse();

            try
            {
                _hierarquiaCoordenadorService.OnUnJoinCoordenadorViabilizador(coordenadorViabilizador);
                response.Objeto = _hierarquiaCoordenadorService.HierarquiaAtivaCoordenadorViabilizador(coordenadorViabilizador);
                response.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC13", "ExportarPagina")]
        public FileContentResult ExportarSelecionadosViabilizador(DataSourceRequest request, HierarquiaCicloFinanceiroDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;
            byte[] file = _hierarquiaCoordenadorService.ExportarViabilizadores(modifiedRequest, filtro);
            string nomeArquivo = GlobalMessages.HierarquiaViabilizadores;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
        [BaseAuthorize("GEC13", "ExportarPagina")]
        public FileContentResult ExportarSelecionadosSupervisor(DataSourceRequest request, HierarquiaCicloFinanceiroDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;
            byte[] file = _hierarquiaCoordenadorService.ExportarSupervisores(modifiedRequest, filtro);
            string nomeArquivo = GlobalMessages.HierarquiaSupervisores;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("GEC13", "ExportarTodos")]
        public FileContentResult ExportarTudo(DataSourceRequest request)
        {
            byte[] file = _hierarquiaCoordenadorService.ExportarTudo(request);
            string nomeArquivo = GlobalMessages.HierarquiaCoordenadores;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
    }
}