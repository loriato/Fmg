using Europa.Commons;
using Europa.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Dto;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Models.Application;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class NovaRegraComissaoController : BaseController
    {
        private SetupRegraComissaoService _setupRegraComissaoService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult MontarMatriz(SetupRegraComissaoDTO setup)
        {
            var option = _setupRegraComissaoService.MontarMatriz(setup);

            //var option = _regraComissaoService.BuscarMatriz(idRegraComissao == null ? 0 : idRegraComissao.Value,
            //    listaEvs, regional, editable, novo, ultimaAtt, modalidade.Value);
            var str = JsonConvert.SerializeObject(option, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult GerarRegraComissao(SetupRegraComissaoDTO setup)
        {
            var response = new JsonResponse();

            try
            {
                setup.UsuarioPortal = SessionAttributes.Current().UsuarioPortal;
                var regraComissao = _setupRegraComissaoService.GerarRegraComissao(setup);
                response.Objeto = regraComissao;
                response.Sucesso = true;
                response.Mensagens.Add(string.Format("Regra de Comissão {0} criada com sucesso",setup.Descricao));
            }catch(BusinessRuleException bre)
            {
                CurrentSession().Transaction.Rollback();
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ValidarItemRegraComissao(List<ItemRegraComissao> itens)
        {
            var response = new JsonResponse();

            try
            {
                _setupRegraComissaoService.ValidarItemRegraComissao(itens);
                response.Sucesso = true;
            }catch(BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}