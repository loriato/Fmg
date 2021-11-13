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
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class StatusConformidadeController : BaseController
    {
        // GET: StatusConformidade
        private StatusConformidadeRepository _statusConformidadeRepository { get; set; }
        private StatusConformidadeService _statusConformidadeService { get; set; }
        private PrePropostaRepository _prePropostaRepository { get; set; }

        // GET: StatusSuatStatusEvs
        [BaseAuthorize("CAD02", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }
        [BaseAuthorize("CAD02", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, StatusConformidadeDTO filtro)
        {
            var results = _statusConformidadeRepository.Listar(filtro);
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD02", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Incluir(StatusConformidade model)
        {
            return Salvar(model, false);
        }
        [BaseAuthorize("CAD02", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Alterar(StatusConformidade model)
        {
            return Salvar(model, true);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Salvar(StatusConformidade status, bool isUpdate)
        {
            var json = new JsonResponse();
            try
            {
                var bre = new BusinessRuleException();
                _statusConformidadeService.Salvar(status, bre);
                bre.ThrowIfHasError();
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, status.ChaveCandidata(), isUpdate ? GlobalMessages.Alterado : GlobalMessages.Incluido));
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
        [BaseAuthorize("CAD02", "Remover")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Remover(long id)
        {
            var json = new JsonResponse();
            var obj = _statusConformidadeRepository.FindById(id);
            try
            {
                _statusConformidadeService.ExcluirPorId(obj);
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
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, obj.DescricaoJunix));
                    json.Sucesso = false;
                }
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}