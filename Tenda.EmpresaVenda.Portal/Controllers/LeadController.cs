using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Portal.Models.Application;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    [BaseAuthorize("EVS17")]
    public class LeadController : BaseController
    {
        public ViewLeadEmpresaVendaRepository _viewLeadEmpresaVendaRepository { get; set; }
        public LeadEmpresaVendaRepository _leadEmpresaVendaRepository { get; set; }
        public LeadEmpresaVendaService _leadEmpresaVendaService { get; set; }

        [BaseAuthorize("EVS17", "Visualizar")]
        public ActionResult Index()
        {
            var ultimoLeadCorretor = _leadEmpresaVendaRepository.UltimoLeadCorretor(SessionAttributes.Current().Corretor.Id);
            var ultimoLeadEv = _leadEmpresaVendaRepository.UltimoLeadEv(SessionAttributes.Current().EmpresaVenda.Id);

            var leadDTO = new LeadDTO
            {
                Id = SessionAttributes.Current().Corretor.Id,
                NomeCorretor = SessionAttributes.Current().Corretor.Nome,
                Funcao = SessionAttributes.Current().Corretor.Funcao
            };

            if (!ultimoLeadEv.IsEmpty() && leadDTO.Funcao == TipoFuncao.Diretor)
            {
                leadDTO.Pacote = ultimoLeadEv.Lead.DescricaoPacote;
            }
            else if (!ultimoLeadCorretor.IsEmpty())
            {
                leadDTO.Pacote = ultimoLeadCorretor.Lead.DescricaoPacote;
            }

            return View(leadDTO);
        }

        [BaseAuthorize("EVS17", "Visualizar")]
        public JsonResult ListarDatatable(DataSourceRequest request, FiltroLeadDTO filtro)
        {
            filtro.IdEmpresaVenda = SessionAttributes.Current().EmpresaVendaId;
            var result = _viewLeadEmpresaVendaRepository.ListarDatatable(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS17", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AtualizarLeadEmpresaVenda(LeadEmpresaVenda leadEmpresaVenda)
        {
            return SalvarLeadEmpresaVenda(leadEmpresaVenda, true);
        }

        [BaseAuthorize("EVS17", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SalvarLeadEmpresaVenda(LeadEmpresaVenda leadEmpresaVenda, bool atualizar)
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

        #region diretor
        [HttpGet]
        [BaseAuthorize("EVS17", "Visualizar")]
        public JsonResult AutoCompletePacoteDiretor(DataSourceRequest request)
        {
            var results = _viewLeadEmpresaVendaRepository.ListarAutoCompletePacoteDiretor(request, SessionAttributes.Current().EmpresaVendaId)
                .Select(x => new ViewLeadEmpresaVenda
                {
                    Nome = x.Pacote
                });

            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS17", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AtribuirCorretorLeadsLote(LeadDTO leads)
        {
            var response = new JsonResponse();

            try
            {
                _leadEmpresaVendaService.AtribuirCorretorLeadsLote(leads);

                response.Sucesso = true;
                response.Mensagens.Add(string.Format("{0} leads atribuídos ao corretor {1}", leads.IdsLeadsEmpresasVendas.Count, leads.NomeCorretor));
            }
            catch (BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS17", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AtribuirCorretorLeadsLoteTodos(LeadDTO leads)
        {
            var response = new JsonResponse();

            try
            {
                var idEmpresaVenda = SessionAttributes.Current().EmpresaVenda.Id;
                leads.IdsLeadsEmpresasVendas = _leadEmpresaVendaRepository.BuscarIdsPorEvPacote(idEmpresaVenda, leads.Pacote);
                _leadEmpresaVendaService.AtribuirCorretorLeadsLote(leads);

                response.Sucesso = true;
                response.Mensagens.Add(string.Format("{0} leads atribuídos ao corretor {1}", leads.IdsLeadsEmpresasVendas.Count, leads.NomeCorretor));
            }
            catch (BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS17", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AtribuirCorretorLead(LeadDTO lead)
        {
            var response = new JsonResponse();

            try
            {
                _leadEmpresaVendaService.AtribuirCorretorLead(lead);

                response.Sucesso = true;
                response.Mensagens.Add(string.Format("Leads atribuídos ao corretor(a) {0}", lead.NomeCorretor));
            }
            catch (BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS17", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult RemoverAtribuicao(LeadDTO lead)
        {
            var response = new JsonResponse();

            try
            {
                _leadEmpresaVendaService.RemoverAtribuicao(lead);

                response.Sucesso = true;
                response.Mensagens.Add("Atribuição removida com sucesso");

            }
            catch (BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS17", "ExportarPagina")]
        public FileContentResult ExportarPaginaDiretor(DataSourceRequest request, FiltroLeadDTO filtro)
        {
            byte[] file = _leadEmpresaVendaService.ExpotarDiretor(request, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"Leads - {filtro.Pacote} - {DateTime.Now}.xlsx");
        }

        [HttpPost]
        [BaseAuthorize("EVS17", "ExportarTodos")]
        public FileContentResult ExportarTodosDiretor(DataSourceRequest request, FiltroLeadDTO filtro)
        {
            request.pageSize = 0;
            request.start = 0;

            byte[] file = _leadEmpresaVendaService.ExpotarDiretor(request, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"Leads - {filtro.Pacote} - {DateTime.Now}.xlsx");
        }
        #endregion

        #region corretor
        [HttpGet]
        [BaseAuthorize("EVS17", "Visualizar")]
        public JsonResult AutoCompletePacoteCorretor(DataSourceRequest request)
        {
            var results = _viewLeadEmpresaVendaRepository.ListarAutoCompletePacoteCorretor(request, SessionAttributes.Current().Corretor.Id)
                .Select(x => new ViewLeadEmpresaVenda
                {
                    Nome = x.Pacote
                });

            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS17", "ExportarPagina")]
        public FileContentResult ExportarPaginaCorretor(DataSourceRequest request, FiltroLeadDTO filtro)
        {
            byte[] file = _leadEmpresaVendaService.ExpotarCorretor(request, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"Leads - {filtro.NomeCorretor} - {DateTime.Now}.xlsx");
        }

        [HttpPost]
        [BaseAuthorize("EVS17", "ExportarTodos")]
        public FileContentResult ExportarTodosCorretor(DataSourceRequest request, FiltroLeadDTO filtro)
        {
            request.pageSize = 0;
            request.start = 0;

            byte[] file = _leadEmpresaVendaService.ExpotarCorretor(request, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"Leads - {filtro.NomeCorretor} - {DateTime.Now}.xlsx");
        }
        #endregion

    }
}