using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("PAG09")]
    public class AprovarAdiantamentoEmergencialController : BaseController
    {
        private ViewRelatorioComissaoRepository _viewRelatorioComissaoRepository { get; set; }
        private PropostaSuatService _propostaSuatService { get; set; }

        [BaseAuthorize("PAG09", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listar(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            filtro.Faturado = 1;
            var result = _viewRelatorioComissaoRepository.ListarAdiantamentoEmergencialSolicitado(filtro);

            return Json(result.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("PAG09", "Aprovar")]
        public ActionResult Aprovar(List<ViewRelatorioComissao> model)
        {
            return AlterarStatus(model, StatusAdiantamentoPagamento.Aprovado);
        }

        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("PAG09", "Reprovar")]
        public ActionResult Reprovar(List<ViewRelatorioComissao> model)
        {
            return AlterarStatus(model, StatusAdiantamentoPagamento.Reprovado);
        }

        [Transaction(TransactionAttributeType.Required)]
        private ActionResult AlterarStatus(List<ViewRelatorioComissao> model, StatusAdiantamentoPagamento novoStatus)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var numAlterados = _propostaSuatService.AlterarStatus(model, novoStatus);
                jsonResponse.Sucesso = true;
                String situacaoStr = "";
                switch (novoStatus)
                {
                    case StatusAdiantamentoPagamento.Aprovado:
                        situacaoStr = GlobalMessages.Aprovados;
                        break;
                    case StatusAdiantamentoPagamento.Reprovado:
                        situacaoStr = GlobalMessages.Reprovados;
                        break;
                }
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.AlteracaoDeRegistros, situacaoStr.ToLower(), numAlterados,
                    model.Count()));
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