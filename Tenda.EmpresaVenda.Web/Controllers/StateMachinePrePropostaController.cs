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
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("CAD12")]
    public class StateMachinePrePropostaController : BaseController
    {
        private StateMachinePrePropostaService _stateMachinePrePropostaService { get; set; }
        private RuleMachinePrePropostaRepository _ruleMachinePrePropostaRepository { get; set; }
        private ViewDocumentoProponenteRuleMachinePrePropostaRepository _viewDocumentoProponenteRuleMachinePrePropostaRepository { get; set; }
        private TipoDocumentoRepository _tipoDocumentoRepository { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("CAD12", "Visualizar")]
        public JsonResult ListarRuleMachinePreProposta(DataSourceRequest request,FiltroRuleMachineDTO filtro)
        {
            var response = _ruleMachinePrePropostaRepository.ListarRuleMachinePreProposta(request, filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD12", "Visualizar")]
        public JsonResult ListarDocumentoProponenteRule(DataSourceRequest request,FiltroRuleMachineDTO filtro)
        {
            var response = _viewDocumentoProponenteRuleMachinePrePropostaRepository.ListarDocumentoProponenteRule(request, filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("CAD12", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult IncludeRule(RuleMachinePreProposta rule)
        {
            return SaveRuleMachinePreProposta(rule);
        }

        [HttpPost]
        [BaseAuthorize("CAD12", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult UpdateRule(RuleMachinePreProposta rule)
        {
            return SaveRuleMachinePreProposta(rule);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SaveRuleMachinePreProposta(RuleMachinePreProposta rule)
        {
            var response = new JsonResponse();

            try
            {
                var msg = string.Format(GlobalMessages.MsgTransicaoSucesso,
                    rule.Origem.AsString(), rule.Destino.AsString(),
                    rule.Id.IsEmpty() ? GlobalMessages.Criada : GlobalMessages.Atualizada, GlobalMessages.Sucesso);

                _stateMachinePrePropostaService.SalvarRuleMachinePreProposta(rule);
                response.Mensagens.Add(msg);
                response.Sucesso = true;

            }catch(BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
                response.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RenderTiposDocumentoDropDownList()
        {
            var tipos = _tipoDocumentoRepository.ListarTodos();

            var list = tipos.OrderBy(x => x.Id).Select(x => new SelectListItem
            {
                Text = x.Nome,
                Value = x.Id.ToString()
            });

            return PartialView("_TipoDocumentoDropDownList", list);
        }

        [HttpPost]
        [BaseAuthorize("CAD12", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ExcluirRule(long idRuleMachine)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            var entidade = _ruleMachinePrePropostaRepository.FindById(idRuleMachine);

            try
            {
                _stateMachinePrePropostaService.ExcluirRule(idRuleMachine);

                CurrentTransaction().Commit();
                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.MsgTransicaoSucesso,
                    entidade.Origem.AsString(), entidade.Destino.AsString(),
                    GlobalMessages.Excluido, GlobalMessages.Sucesso));

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

        [HttpPost]
        [BaseAuthorize("CAD12", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult OnJoinTipoDocumento(RuleMachineDTO documento)
        {
            var response = new JsonResponse();

            try
            {
                _stateMachinePrePropostaService.OnJoinTipoDocumento(documento);
                response.Mensagens.Add(GlobalMessages.ObrigatoriedadeAlterada);
                response.Sucesso = true;
            }catch(BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}