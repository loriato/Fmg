using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using Newtonsoft.Json.Linq;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.ApiService.Models.AgrupamentoProcessoPreProposta;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("CAD19")]
    public class AgrupamentoProcessoPrePropostaController : BaseController
    {
        private SistemaRepository _sistemaRepository { get; set; }
        [BaseAuthorize("CAD19", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("CAD19", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, AgrupamentoProcessoPrePropostaFiltro filtro)
        {
            filtro.DataSourceRequest = request;
            var result = EmpresaVendaApi.ListarAgrupamentoProcessoPreProposta(filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD19", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Incluir(ViewAgrupamentoProcessoPreProposta model)
        {
            return Salvar(model, false);
        }

        [BaseAuthorize("CAD19", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Alterar(ViewAgrupamentoProcessoPreProposta model)
        {
            return Salvar(model, true);
        }

        private ActionResult Salvar(ViewAgrupamentoProcessoPreProposta model, bool isUpdate)
        {
            var json = new JsonResponse();
            try
            {
                var bre = new BusinessRuleException();
                //Colocar o sistema PORTAL HOME fixo, pois não será utilizado por outro sistema por enquanto
                model.IdSistemas = 1;
                var response = EmpresaVendaApi.SalvarAgrupamentoProcessoPreProposta(model);
                json.FromBaseResponse(response);
                var dto = ((JObject)response.Data).ToObject<ViewAgrupamentoProcessoPreProposta>();
                TempData["SucessMessage"] = response.Messages.FirstOrDefault();
                bre.ThrowIfHasError();
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

        [BaseAuthorize("CAD19", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Excluir(long id)
        {
            var json = new JsonResponse();
            try
            {
                var obj = EmpresaVendaApi.RemoverAgrupamentoProcessoPreProposta(id);
                CurrentTransaction().Commit();
                json.Mensagens = obj.Messages;
                json.Sucesso = obj.Success;
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
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, id));
                    json.Sucesso = false;
                }
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}