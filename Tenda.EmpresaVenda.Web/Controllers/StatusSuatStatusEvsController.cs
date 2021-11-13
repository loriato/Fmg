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
    [BaseAuthorize("CAD01")]
    public class StatusSuatStatusEvsController : BaseController
    {
       private StatusSuatStatusEvsRepository _statusSuatStatusEvsRepository { get; set; }
       private StatusSuatStatusEvsService _statusSuatStatusEvsService { get; set; }
       private PrePropostaRepository _prePropostaRepository { get; set; }

        // GET: StatusSuatStatusEvs
        [BaseAuthorize("CAD01","Visualizar")]
        public ActionResult Index()
        {
            return View();
        }
        [BaseAuthorize("CAD01", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request,StatusSuatStatusEvsDTO filtro)
        {
            var results = _statusSuatStatusEvsRepository.Listar(filtro);
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD01", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Incluir(StatusSuatStatusEvs model)
        {
            return Salvar(model, false);
        }
        [BaseAuthorize("CAD01", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Alterar(StatusSuatStatusEvs model)
        {
            return Salvar(model, true);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Salvar(StatusSuatStatusEvs status,bool isUpdate)
        {
            var json = new JsonResponse();
            try
            {
                var bre = new BusinessRuleException();
                _statusSuatStatusEvsService.Salvar(status, bre);
                bre.ThrowIfHasError();
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso,status.ChaveCandidata(), isUpdate ? GlobalMessages.Alterado : GlobalMessages.Incluido));
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
        [BaseAuthorize("CAD01", "Remover")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Remover(long id)
        {
            var json = new JsonResponse();
            var obj = _statusSuatStatusEvsRepository.FindById(id);
            try
            {
                _statusSuatStatusEvsService.ExcluirPorId(obj);
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
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, obj.DescricaoSuat));
                    json.Sucesso = false;
                }
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}