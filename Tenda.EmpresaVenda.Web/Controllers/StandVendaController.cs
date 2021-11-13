using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using Microsoft.Ajax.Utilities;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("CAD11")]
    public class StandVendaController : BaseController
    {
        private StandVendaService _standVendaService { get; set; }
        private ViewStandVendaRepository _viewStandVendaRepository { get; set; }
        private StandVendaRepository _standVendaRepository { get; set; }
        private ViewStandVendaEmpresaVendaRepository _viewStandVendaEmpresaVendaRepository { get; set; }
        private StandVendaEmpresaVendaService _standVendaEmpresaVendaService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("CAD11", "Visualizar")]
        public ActionResult ListarDatatableStandVenda(DataSourceRequest request, FiltroStandVendaDTO filtro)
        {
            var response = _viewStandVendaRepository.ListarDatatable(request, filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("CAD11", "Incluir")]
        public ActionResult IncluirStandVenda(StandVenda standVenda)
        {
            return SalvarStandVenda(standVenda);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("CAD11", "Atualizar")]
        public ActionResult AtualizarStandVenda(StandVenda standVenda)
        {
            return SalvarStandVenda(standVenda);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult SalvarStandVenda(StandVenda standVenda)
        {
            var response = new JsonResponse();

            try
            {
                _standVendaService.SalvarStandVenda(standVenda);
                response.Mensagens.Add(string.Format("{0} foi {1} com {2}",standVenda.Nome,GlobalMessages.Salvo,GlobalMessages.Sucesso));
                response.Sucesso = true;
            }catch(BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
                response.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("CAD11", "Excluir")]
        public JsonResult ExcluirStandVenda(long idStandVenda)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            var entidade = _standVendaRepository.FindById(idStandVenda);

            try
            {
                _standVendaService.ExcluirStandVenda(idStandVenda);
                CurrentTransaction().Commit();
                json.Sucesso = true;
                json.Mensagens.Add(String.Format(GlobalMessages.RegistroSucesso, entidade.Nome, GlobalMessages.Excluido.ToLower()));

            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    CurrentTransaction().Rollback();
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, entidade.ChaveCandidata()));
                    json.Sucesso = false;
                }
            }
            
            return Json(json, JsonRequestBehavior.AllowGet);
            
        }

        [BaseAuthorize("CAD11", "Visualizar")]
        public ActionResult ListarDatatableStandVendaEmpresaVenda(DataSourceRequest request,FiltroStandVendaDTO filtro)
        {
            var response = _viewStandVendaEmpresaVendaRepository.ListarDatatableStandVendaEmpresaVenda(request, filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("CAD11", "Atualizar")]
        public ActionResult OnJoinStandEmpresaVenda(StandVendaDTO standEmpresaVendaDTO)
        {
            var response = new JsonResponse();

            try
            {

                standEmpresaVendaDTO.IdUsuario = SessionAttributes.Current().UsuarioPortal.Id;

                _standVendaEmpresaVendaService.OnJoinStandEmpresaVenda(standEmpresaVendaDTO);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format("{0} {1} stand {2} com sucesso", standEmpresaVendaDTO.NomeEmpresaVenda, (standEmpresaVendaDTO.Situacao == Situacao.Ativo ? "adicionado ao" : "removido do"), standEmpresaVendaDTO.NomeStandVenda));
            }catch(BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);

            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AutoCompleteStandVenda(DataSourceRequest request)
        {
            var response = _standVendaRepository.Queryable()
                .Select(x => new { Id = x.Id, Nome = x.Nome })
                .ToDataRequest(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}