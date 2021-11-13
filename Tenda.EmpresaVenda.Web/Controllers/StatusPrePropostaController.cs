using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.ApiService.Models.StatusPreProposta;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("CAD17")]
    public class StatusPrePropostaController : BaseController
    {
        [BaseAuthorize("CAD17", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("CAD17", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, StatusPrePropostaFiltro filtro)
        {
            filtro.DataSourceRequest = request;
            var result = EmpresaVendaApi.ListarStatusPreProposta(filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD17", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Incluir(StatusPrePropostaDto model)
        {
            return Salvar(model, false);
        }

        [BaseAuthorize("CAD17", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Alterar(StatusPrePropostaDto model)
        {
            return Salvar(model, true);
        }

        private ActionResult Salvar(StatusPrePropostaDto model, bool isUpdate)
        {
            var json = new JsonResponse();
            try
            {
                var bre = new BusinessRuleException();
                var response = EmpresaVendaApi.SalvarStatusPreProposta(model);
                json.FromBaseResponse(response);
                var dto = ((JObject)response.Data).ToObject<StatusPrePropostaDto>();
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
    }
}