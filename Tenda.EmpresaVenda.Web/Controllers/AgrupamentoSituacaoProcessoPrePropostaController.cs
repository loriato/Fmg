using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using Newtonsoft.Json.Linq;
using NHibernate.Exceptions;
using System.Linq;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.AgrupamentoProcessoPreProposta;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("CAD19", "Visualizar")]
    public class AgrupamentoSituacaoProcessoPrePropostaController : BaseController
    {
        [BaseAuthorize("CAD19", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("CAD19", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, AgrupamentoSituacaoProcessoPrePropostaFiltro filtro)
        {
            filtro.DataSourceRequest = request;
            var result = EmpresaVendaApi.ListarAgrupamentoSituacaoProcessoPreProposta(filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD19", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Incluir(ViewAgrupamentoSituacaoProcessoPreProposta model)
        {
            return Salvar(model, false);
        }

        [BaseAuthorize("CAD19", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Alterar(ViewAgrupamentoSituacaoProcessoPreProposta model)
        {
            return Salvar(model, true);
        }

        private ActionResult Salvar(ViewAgrupamentoSituacaoProcessoPreProposta model, bool isUpdate)
        {
            var json = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.SalvarAgrupamentoSituacaoProcessoPreProposta(model);
                if (response.Success)
                {
                    json.FromBaseResponse(response);
                    var dto = ((JObject)response.Data).ToObject<ViewAgrupamentoSituacaoProcessoPreProposta>();
                    TempData["SucessMessage"] = response.Messages.FirstOrDefault();
                    json.Sucesso = true;
                }
                else
                {
                    json.FromBaseResponse(response);
                    json.Sucesso = false;
                }
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }
            catch (ApiException apiEx)
            {
                json.Sucesso = false;
                json.Mensagens.AddRange(apiEx.GetResponse().Messages);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD19", "ExcluirCruzamento")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Excluir(long id)
        {
            var json = new JsonResponse();
            try
            {
                var obj = EmpresaVendaApi.RemoverAgrupamentoSituacaoProcessoPreProposta(id);
                CurrentTransaction().Commit();
                json.Mensagens.Add(GlobalMessages.RemovidoSucesso);
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
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, id));
                    json.Sucesso = false;
                }
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}