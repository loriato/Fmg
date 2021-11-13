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
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("PAG04")]
    public class StatusContratoJunixController : BaseController
    {
        private FaseStatusContratoJunixRepository _faseStatusContratoJunixRepository { get; set; }
        private SinteseStatusContratoJunixRepository _sinteseStatusContratoJunixRepository { get; set; }
        private FaseStatusContratoJunixService _faseStatusContratoJunixService { get; set; }
        private SinteseStatusContratoJunixService _sinteseStatusContratoJunixService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("PAG04", "Visualizar")]
        public ActionResult ListarFases(DataSourceRequest request, JunixDTO filtro )
        {
            var result = _faseStatusContratoJunixRepository.ListarFases(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("PAG04", "Visualizar")]
        public ActionResult ListarSinteses(DataSourceRequest request, JunixDTO filtro)
        {
            var result = _sinteseStatusContratoJunixRepository.ListarSinteses(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("PAG04", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult IncluirFase(FaseStatusContratoJunix fase)
        {
            return SalvarFase(fase, false);
        }

        [BaseAuthorize("PAG04", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AlterarFase(FaseStatusContratoJunix fase)
        {
            return SalvarFase(fase, true);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult SalvarFase(FaseStatusContratoJunix fase, bool isUpdate)
        {
            var json = new JsonResponse();
            var bre = new BusinessRuleException();

            try
            {                
                _faseStatusContratoJunixService.Salvar(fase, bre);
                bre.ThrowIfHasError();
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, fase.ChaveCandidata(), isUpdate ? GlobalMessages.Alterado : GlobalMessages.Incluido));
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

        [BaseAuthorize("PAG04", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult ExcluirFase(long id)
        {
            var json = new JsonResponse();
            var obj = _faseStatusContratoJunixRepository.FindById(id);
            try
            {
                _faseStatusContratoJunixService.ExcluirPorId(obj);
                CurrentTransaction().Commit();
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, obj.ChaveCandidata(), GlobalMessages.Removido));
                json.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                json.Mensagens.AddRange(bre.Errors);
                json.Sucesso = false;
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    CurrentTransaction().Rollback();
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, obj.Fase));
                    json.Sucesso = false;
                }
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("PAG04", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult IncluirSintese(SinteseStatusContratoJunix sintese)
        {
            return SalvarSintese(sintese, false);
        }

        [BaseAuthorize("PAG04", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AlterarSintese(SinteseStatusContratoJunix sintese)
        {
            return SalvarSintese(sintese, true);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult SalvarSintese(SinteseStatusContratoJunix sintese, bool isUpdate)
        {
            var json = new JsonResponse();
            var bre = new BusinessRuleException();

            try
            {
                _sinteseStatusContratoJunixService.Salvar(sintese, bre);
                bre.ThrowIfHasError();
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, sintese.ChaveCandidata(), isUpdate ? GlobalMessages.Alterado : GlobalMessages.Incluido));
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

        [BaseAuthorize("PAG04", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult ExcluirSintese(long id)
        {
            var json = new JsonResponse();
            var obj = _sinteseStatusContratoJunixRepository.FindById(id);
            try
            {
                _sinteseStatusContratoJunixService.ExcluirPorId(obj);
                CurrentTransaction().Commit();
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, obj.ChaveCandidata(), GlobalMessages.Removido));
                json.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                json.Mensagens.AddRange(bre.Errors);
                json.Sucesso = false;
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    CurrentTransaction().Rollback();
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, obj.Sintese));
                    json.Sucesso = false;
                }
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}