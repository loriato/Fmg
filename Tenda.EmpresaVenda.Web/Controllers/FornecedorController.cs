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
    [BaseAuthorize("CAD06")]
    public class FornecedorController : BaseController
    {
        private FornecedorRepository _fornecedorRepository { get; set; }
        private FornecedorService _fornecedorService { get; set; }

        [BaseAuthorize("CAD06", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("CAD06", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request)
        {
            var results = _fornecedorRepository.Listar();
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarAutoComplete(DataSourceRequest request)
        {
            var results = _fornecedorRepository.ListarAutoComplete(request).Select(reg => new Fornecedor
            {
                Id = reg.Id,
                NomeFantasia = reg.NomeFantasia
            });
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }


        [BaseAuthorize("CAD06", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Incluir(Fornecedor model)
        {
            return Salvar(model, false);
        }
        [BaseAuthorize("CAD06", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Alterar(Fornecedor model)
        {
            return Salvar(model, true);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Salvar(Fornecedor model, bool isUpdate)
        {
            var json = new JsonResponse();
            try
            {
                var bre = new BusinessRuleException();
                _fornecedorService.Salvar(model, bre);
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
        [BaseAuthorize("CAD06", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Remover(long id)
        {
            var json = new JsonResponse();
            var obj = _fornecedorRepository.FindById(id);
            try
            {
                _fornecedorService.Excluir(obj);
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
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, obj.NomeFantasia));
                    json.Sucesso = false;
                }
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}