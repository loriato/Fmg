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
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("CAD04")]
    public class TipoCustoController : BaseController
    {
        private TipoCustoRepository _tipoCustoRepository { get; set; }
        private TipoCustoService _tipoCustoService { get; set; }

        [BaseAuthorize("CAD04","Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("CAD04", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request)
        {
            var results = _tipoCustoRepository.Listar();
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult ListarAutoComplete(DataSourceRequest request)
        {
            var results = _tipoCustoRepository.ListarAutoComplete(request).Select(reg => new TipoCusto
            {
                Id = reg.Id,
                Descricao = reg.Descricao
            });
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }


        [BaseAuthorize("CAD04", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Incluir(TipoCusto model)
        {
            return Salvar(model, false);
        }
        [BaseAuthorize("CAD04", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Alterar(TipoCusto model)
        {
            return Salvar(model, true);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Salvar(TipoCusto model, bool isUpdate)
        {
            var json = new JsonResponse();
            try
            {
                var bre = new BusinessRuleException();
                _tipoCustoService.Salvar(model, bre);
                bre.ThrowIfHasError();
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, model.ChaveCandidata(), isUpdate ? GlobalMessages.Alterado : GlobalMessages.Incluido));
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
        [BaseAuthorize("CAD04", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Remover(long id)
        {
            var json = new JsonResponse();
            var obj = _tipoCustoRepository.FindById(id);
            try
            {
                _tipoCustoService.Excluir(obj);
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
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, obj.Descricao));
                    json.Sucesso = false;
                }
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}