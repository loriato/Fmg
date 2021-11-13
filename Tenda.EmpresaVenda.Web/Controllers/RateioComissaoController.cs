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
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("CAD08")]
    public class RateioComissaoController : BaseController
    {
        private RateioComissaoRepository _rateioComissaoRepository { get; set; }
        private ViewRateioComissaoRepository _viewRateioComissaoRepository { get; set; }
        private RateioComissaoService _rateioComissaoService { get; set; }

        // GET: RateioComissao
        public ActionResult Index()
        {
            return View();
        }
        [BaseAuthorize("CAD08", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, FiltroRateioComissaoDTO filtro)
        {
            var results = _viewRateioComissaoRepository.Listar(filtro);

            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD08", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Salvar(RateioComissao model)
        {
            var json = new JsonResponse();
            try
            {
                var bre = new BusinessRuleException();
                _rateioComissaoService.Salvar(model, bre);
                bre.ThrowIfHasError();

                json.Mensagens.Add(string.Format(GlobalMessages.RateioComissaoSucesso, GlobalMessages.Incluido.ToLower()));
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
        [BaseAuthorize("CAD08", "Ativar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Ativar(long id)
        {
            var jsonResponse = new JsonResponse();
            var obj = _rateioComissaoRepository.FindById(id);
            try
            {
                var bre = new BusinessRuleException();
                _rateioComissaoService.Ativar(obj, bre);
                bre.ThrowIfHasError();

                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.RateioComissaoSucesso, GlobalMessages.Ativado.ToLower()));
            }
            catch (BusinessRuleException ex)
            {
                jsonResponse.Mensagens.AddRange(ex.Errors);
                jsonResponse.Sucesso = false;
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD08", "Finalizar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Finalizar(long id)
        {
            var jsonResponse = new JsonResponse();
            var obj = _rateioComissaoRepository.FindById(id);
            try
            {
                _rateioComissaoService.Finalizar(obj);
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.RateioComissaoSucesso, GlobalMessages.Finalizado.ToLower()));
            }
            catch (Exception ex)
            {
                jsonResponse.Mensagens.Add(ex.Message);
                jsonResponse.Sucesso = false;
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD08", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Excluir(long id)
        {
            var jsonResponse = new JsonResponse();
            var obj = _rateioComissaoRepository.FindById(id);
            try
            {
                _rateioComissaoService.Excluir(obj);
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.RateioComissaoSucesso, GlobalMessages.Excluido.ToLower()));
            }
            catch (Exception ex)
            {
                jsonResponse.Mensagens.Add(ex.Message);
                jsonResponse.Sucesso = false;
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }
    }
}