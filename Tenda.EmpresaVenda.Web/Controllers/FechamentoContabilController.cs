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
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("PAG11")]
    public class FechamentoContabilController : BaseController
    {
        private FechamentoContabilService _fechamentoContabilService { get; set; }
        private FechamentoContabilRepository _fechamentoContabilRepository { get; set; }

        [BaseAuthorize("PAG11", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("PAG11", "Visualizar")]
        public JsonResult ListarFechamentoContabil(DataSourceRequest request,FiltroFechamentoContabilDto filtro)
        {
            var lista = _fechamentoContabilRepository.ListarFechamentoContabil(request,filtro);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("PAG11", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Inserir(FechamentoContabilDto fechamentoDto)
        {
            return Salvar(fechamentoDto);
        }

        [HttpPost]
        [BaseAuthorize("PAG11", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Atualizar(FechamentoContabilDto fechamentoDto)
        {
            return Salvar(fechamentoDto);
        }

        [HttpPost]
        [BaseAuthorize("PAG11", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Salvar(FechamentoContabilDto fechamentoDto)
        {
            var response = new JsonResponse();

            try
            {
                FechamentoContabil fechamentoContabil = _fechamentoContabilService.SalvarFechamento(fechamentoDto);
                response.Sucesso = true;
                response.Objeto = fechamentoContabil;
                response.Mensagens.Add(string.Format(GlobalMessages.MsgFechamnetoContabilSucesso,
                    fechamentoDto.InicioFechamento.ToDate(), fechamentoDto.TerminoFechamento.ToDate()));
            }catch(BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
                response.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("PAG11", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ExcluirFechamentoContabil(FechamentoContabilDto fechamentoDto)
        {
            var response = new JsonResponse();

            try
            {
                var fechamento = _fechamentoContabilService.ExcluirFechamentoContabil(fechamentoDto.IdFechamento);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format("{0} {1} com {2}",
                    fechamentoDto.Descricao, GlobalMessages.Excluido, GlobalMessages.Sucesso));
            }
            catch(BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ComunicarFechamentoContabil(FechamentoContabil fechamento)
        {
            var response = new JsonResponse();
            try
            {
                _fechamentoContabilService.ComunicarFechamentoContabil(fechamento);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format("{0} e {1} enviados com {2}",
                    GlobalMessages.Notificacao, GlobalMessages.Email, GlobalMessages.Sucesso));
            }
            catch(BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TemplateEmail()
        {
            return View();
        }
    }
}