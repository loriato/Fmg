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
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC12")]
    public class HierarquiaSupervisorViabilizadorController : BaseController
    {
        private ViewSupervisorRepository _viewSupervisorRepository { get; set; }
        private ViewViabilizadorRepository _viewViabilizadorRepository { get; set; }
        private SupervisorViabilizadorService _supervisorViabilizadorService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("GEC12", "Visualizar")]
        public JsonResult ListarSupervisores(DataSourceRequest request, SupervisorViabilizadorDTO filtro)
        {
            var response = _viewSupervisorRepository.Listar(request, filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC12", "Visualizar")]
        public JsonResult ListarViabilizadores(DataSourceRequest request, SupervisorViabilizadorDTO filtro)
        {
            var response = _viewViabilizadorRepository.Listar(request, filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("GEC12", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult OnJoinSupervisorViabilizador(SupervisorViabilizador supervisorViabilizador)
        {
            var response = new JsonResponse();

            try
            {
                if (supervisorViabilizador.Id.HasValue())
                {
                    _supervisorViabilizadorService.OnUnJoinSupervisorViabilizador(supervisorViabilizador);
                }
                else
                {
                    _supervisorViabilizadorService.OnJoinSupervisorViabilizador(supervisorViabilizador);
                }
                response.Sucesso = true;
            }catch(BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("GEC12","Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SelecionarTodosViabilizadores(List<SupervisorViabilizador> lista)
        {
            var response = new JsonResponse();

            try
            {
                var result = _supervisorViabilizadorService.SelecionarTodosViabilizadores(lista);

                if (result.CountAdicionados > 0)
                {
                    response.Mensagens.Add(string.Format("Foram {0} {1} {2} de {3} selecionados",GlobalMessages.Adicionados,result.CountAdicionados,GlobalMessages.Viabilizadores,lista.Count));
                }

                if (result.CountRemovidos > 0)
                {
                    response.Mensagens.Add(string.Format("Foram {0} {1} {2} de {3} selecionados", GlobalMessages.Removidos, result.CountRemovidos, GlobalMessages.Viabilizadores, lista.Count));
                }

                response.Sucesso = true;
            }catch(BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC13", "ExportarTodos")]
        public FileContentResult ExportarTudo(DataSourceRequest request)
        {
            byte[] file = _supervisorViabilizadorService.ExportarTudo(request);
            string nomeArquivo = GlobalMessages.HierarquiaSupervisores;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("GEC12", "ExportarPagina")]
        public FileContentResult ExportarSelecionadosViabilizador(DataSourceRequest request, SupervisorViabilizadorDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;
            byte[] file = _supervisorViabilizadorService.Exportar(modifiedRequest, filtro);
            string nomeArquivo = GlobalMessages.HierarquiaViabilizadores;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
    }
}