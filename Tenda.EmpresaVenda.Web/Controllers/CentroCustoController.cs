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
    [BaseAuthorize("CAD07")]
    public class CentroCustoController : BaseController
    {
        private CentroCustoRepository _centroCustoRepository { get; set; }
        private CentroCustoService _centroCustoService { get; set; }

        [BaseAuthorize("CAD07", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("CAD07", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request)
        {
            var results = _centroCustoRepository.Listar();
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarAutoComplete(DataSourceRequest request)
        {
            var results = _centroCustoRepository.ListarAutoComplete(request).Select(reg => new CentroCusto
            {
                Id = reg.Id,
                Codigo = reg.Codigo
            });
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD07", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Incluir(CentroCusto model)
        {
            return Salvar(model, false);
        }
        [BaseAuthorize("CAD07", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Alterar(CentroCusto model)
        {
            return Salvar(model, true);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Salvar(CentroCusto model, bool isUpdate)
        {
            var json = new JsonResponse();
            try
            {
                var bre = new BusinessRuleException();
                _centroCustoService.Salvar(model, bre);
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
        [BaseAuthorize("CAD07", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Remover(long id)
        {
            var json = new JsonResponse();
            var obj = _centroCustoRepository.FindById(id);
            try
            {
                _centroCustoService.Excluir(obj);
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
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, obj.Codigo));
                    json.Sucesso = false;
                }
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}