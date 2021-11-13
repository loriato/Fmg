using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC05")]
    public class LeadController : BaseController
    {

        private ViewLeadEmpresaVendaRepository _viewLeadEmpresaVendaRepository { get; set; }
        private LeadEmpresaVendaService _leadEmpresaVendaService { get; set; }
        private LeadService _leadService { get; set; }
        private LeadRepository _leadRepository { get; set; }

        [BaseAuthorize("GEC05", "Visualizar")]
        public ActionResult Index()
        {
            ViewBag.QuantidadeDiasExportarTodos = ProjectProperties.DiasExportarTodosLeads;
            return View();
        }

        public JsonResult Listar(DataSourceRequest request, FiltroLeadDTO filtro)
        {

            var result = _viewLeadEmpresaVendaRepository.ListarDatatable(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC05", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, FiltroLeadDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;
            byte[] file = _leadService.Exportar(modifiedRequest, filtro);
            string nomeArquivo = GlobalMessages.Lead;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
        [BaseAuthorize("GEC05", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, FiltroLeadDTO filtro)
        {
            byte[] file = _leadService.Exportar(request, filtro);
            string nomeArquivo = GlobalMessages.Lead;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [HttpGet]
        public JsonResult AutoCompletePacote(DataSourceRequest request)
        {
            var results = _leadRepository.ListarAutoCompletePacote(request)
                .Select(x => new Lead
                {
                    DescricaoPacote = x.DescricaoPacote
                });
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("GEC05", "Atualizar")]
        public ActionResult LiberarPacote(List<string> pacote)
        {
            var json = new JsonResponse();
            try
            {
                var bre = new BusinessRuleException();
                _leadService.LiberarPacote(pacote, bre);
                bre.ThrowIfHasError();
                json.Mensagens.Add(GlobalMessages.PacotesLiberadosSucesso);
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("GEC05", "Excluir")]
        public ActionResult ExcluirPacote(string pacote)
        {
            var json = new JsonResponse();
            try
            {
                var bre = new BusinessRuleException();
                _leadService.ExcluirPacote(pacote, bre);
                bre.ThrowIfHasError();
                json.Mensagens.Add(string.Format(GlobalMessages.PacoteExcluido, pacote));
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    CurrentTransaction().Rollback();
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, pacote));
                    json.Sucesso = false;
                }
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC05", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SalvarLeadEmpresaVenda(LeadEmpresaVenda leadEmpresaVenda)
        {
            var response = new JsonResponse();

            try
            {
                _leadEmpresaVendaService.SalvarLeadEmpresaVenda(leadEmpresaVenda);

                response.Sucesso = true;

                response.Mensagens.Add(GlobalMessages.SalvoSucesso);
            }
            catch (BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarTreePacote()
        {

            var pacote = _leadRepository.BuscarPacoteNaoLiberados();


            var results = pacote.Select(x => new TreeViewModel
            {
                text = x.DescricaoPacote,
                selectable = true,
                state = new TreeViewState
                {
                    @checked = false,
                    disabled = false,
                    expanded = false,
                    selected = false
                }
            }).OrderBy(x => x.text);
            return Json(results.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}