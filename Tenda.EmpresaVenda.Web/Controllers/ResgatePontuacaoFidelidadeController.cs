using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC06")]
    public class ResgatePontuacaoFidelidadeController : BaseController
    {
        public ViewConsolidadoPontuacaoFidelidadeRepository _viewConsolidadoPontuacaoFidelidadeRepository { get; set; }
        public PontuacaoFidelidadeEmpresaVendaRepository _pontuacaoFidelidadeEmpresaVendaRepository { get; set; }
        public PontuacaoFidelidadeEmpresaVendaService _pontuacaoFidelidadeEmpresaVendaService { get; set; }
        public ViewResgatePontuacaoFidelidadeRepository _viewResgatePontuacaoFidelidadeRepository { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [BaseAuthorize("GEC06", "Visualizar")]
        public JsonResult ListarPontuacao(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            var result = _viewConsolidadoPontuacaoFidelidadeRepository.ListarPontuacao(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("GEC06", "Visualizar")]
        public JsonResult Totais(long idEmpresaVenda)
        {
            var response = new JsonResponse();
            var result = _pontuacaoFidelidadeEmpresaVendaRepository.FindByIdEmpresaVenda(idEmpresaVenda);

            response.Sucesso = result.HasValue();

            if (result.HasValue())
            {
                response.Objeto = new PontuacaoFidelidadeEmpresaVenda
                {
                    PontuacaoResgatada = result.PontuacaoResgatada,
                    PontuacaoDisponivel = result.PontuacaoDisponivel,
                    PontuacaoIndisponivel = result.PontuacaoIndisponivel,
                    PontuacaoTotal = result.PontuacaoTotal,
                    PontuacaoSolicitada = result.PontuacaoSolicitada
                };
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [BaseAuthorize("GEC06", "SolicitarResgate")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SolicitarResgate(PontuacaoFidelidadeDTO pontuacao)
        {
            var response = new JsonResponse();

            try
            {
                pontuacao.IdSolicitadoPor = SessionAttributes.Current().UsuarioPortal.Id;
                _pontuacaoFidelidadeEmpresaVendaService.SolicitarResgate(pontuacao);
                response.Sucesso = true;
                response.Mensagens.Add(GlobalMessages.ResgateSolicitadoSucesso);

            }catch(BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("GEC06", "Visualizar")]
        public JsonResult ListarResgate(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            var result = _viewResgatePontuacaoFidelidadeRepository.ListarResgate(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC06", "ExportarTodos")]
        public FileContentResult ExportarTodosPontuacao(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = _pontuacaoFidelidadeEmpresaVendaService.ExportarPontuacao(modifiedRequest, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"{GlobalMessages.Pontuacao} - {DateTime.Now}.xlsx");
        }

        [BaseAuthorize("GEC06", "ExportarPagina")]
        public FileContentResult ExportarPaginaPontuacao(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            byte[] file = _pontuacaoFidelidadeEmpresaVendaService.ExportarPontuacao(request, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"{GlobalMessages.Pontuacao} - {DateTime.Now}.xlsx");
        }

        [BaseAuthorize("GEC06", "ExportarTodos")]
        public FileContentResult ExportarTodosExtrato(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = _pontuacaoFidelidadeEmpresaVendaService.ExportarExtrato(modifiedRequest, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"{GlobalMessages.Extrato} - {DateTime.Now}.xlsx");
        }

        [BaseAuthorize("GEC06", "ExportarPagina")]
        public FileContentResult ExportarPaginaExtrato(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            byte[] file = _pontuacaoFidelidadeEmpresaVendaService.ExportarExtrato(request, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"{GlobalMessages.Extrato} - {DateTime.Now}.xlsx");
        }
    }
}