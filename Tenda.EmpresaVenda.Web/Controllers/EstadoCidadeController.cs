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
    [BaseAuthorize("CAD13")]
    public class EstadoCidadeController : BaseController
    {
        private EstadoCidadeService _estadoCidadeService { get; set; }
        private EstadoCidadeRepository _estadoCidadeRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [BaseAuthorize("CAD13", "IncluirCidade")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult IncluirCidade(EstadoCidade estadoCidade)
        {
            return Salvar(estadoCidade);
        }

        [HttpPost]
        [BaseAuthorize("CAD13", "AtualizarCidade")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AtualizarCidade(EstadoCidade estadoCidade)
        {
            return Salvar(estadoCidade);
        }

        [HttpPost]
        [BaseAuthorize("CAD13", "IncluirCidade")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Salvar(EstadoCidade estadoCidade)
        {
            var response = new JsonResponse();

            try
            {
                _estadoCidadeService.Salvar(estadoCidade);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format("Cidade {0} salva com sucesso", estadoCidade.Cidade));
            }catch(BusinessRuleException bre)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
                response.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("CAD13", "ExcluirCidade")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ExcluirCidade(EstadoCidade estadoCidade)
        {
            var response = new JsonResponse();

            try
            {
                _estadoCidadeService.ExcluirCidade(estadoCidade);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, estadoCidade.ChaveCandidata(), GlobalMessages.Removido));
            }
            catch(BusinessRuleException bre)
            {
                CurrentSession().Transaction.Rollback();
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD13", "VisualizarCidade")]
        public JsonResult ListarCidades(DataSourceRequest request,EstadoCidadeDTO filtro)
        {
            var response = _estadoCidadeRepository.ListarCidades(request, filtro);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD13", "VisualizarCidade")]
        public JsonResult ListarEstados(DataSourceRequest request, EstadoCidadeDTO filtro)
        {
            var response = _estadoCidadeRepository.ListarEstados(request, filtro);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}