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
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("PAG08")]
    public class AdiantamentoEmergencialController : BaseController
    {

        private ViewRelatorioComissaoRepository _viewRelatorioComissaoRepository { get; set; }
        private PropostaSuatService _propostaSuatService { get; set; }

        [BaseAuthorize("PAG08", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listar(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            filtro.Faturado = 1;
            var result = _viewRelatorioComissaoRepository.ListarAdiantamentoEmergencial(filtro);

            return Json(result.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("PAG08", "SolicitarAdiantamento")]
        public ActionResult SolicitarAdiantamento(List<ViewRelatorioComissao> model)
        {
            var json = new JsonResponse();

            try
            {
                _propostaSuatService.SolicitarAdiantamento(model);
                json.Sucesso = true;
                json.Mensagens.Add(GlobalMessages.MsgSolicitacaoAdiantamentoSolicitado);
            }
            catch (Exception ex)
            {
                json.Sucesso = false;
                json.Mensagens.Add(ex.Message);
            }

            return Json(json, JsonRequestBehavior.AllowGet);

        }
    }
}