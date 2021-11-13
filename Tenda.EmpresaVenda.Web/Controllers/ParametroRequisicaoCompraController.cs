using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
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
    [BaseAuthorize("PAG10")]
    public class ParametroRequisicaoCompraController : BaseController
    {
        private ParametroRequisicaoCompraRepository _parametroRequisicaoCompraRepository { get; set; }
        private ParametroRequisicaoCompraService _parametroRequisicaoCompraService { get; set; }

        [BaseAuthorize("PAG10", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }
        [BaseAuthorize("PAG10", "Visualizar")]
        public JsonResult Listar(DataSourceRequest request, string chave)
        {
            var results = _parametroRequisicaoCompraRepository.Listar(chave);

            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }
        [BaseAuthorize("CAD07", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Atualizar(ParametroRequisicaoCompra model)
        {
            return Salvar(model, true);
        }

        [BaseAuthorize("CAD07", "Atualizar")]
        public ActionResult Salvar(ParametroRequisicaoCompra model, bool isUpdate)
        {
            var json = new JsonResponse();
            try
            {
                var bre = new BusinessRuleException();
                _parametroRequisicaoCompraService.Salvar(model, bre);
                bre.ThrowIfHasError();
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, model.ChaveCandidata(), isUpdate ? GlobalMessages.Atualizado : GlobalMessages.Incluido));
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