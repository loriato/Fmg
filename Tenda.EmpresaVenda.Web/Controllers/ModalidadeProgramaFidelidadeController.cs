using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("CAD10")]
    public class ModalidadeProgramaFidelidadeController : BaseController
    {
        private ViewEmpreendimentoEnderecoRepository _viewEmpreendimentoEnderecoRepository { get; set; }
        private EmpreendimentoService _empreendimentoService { get; set; }

        [BaseAuthorize("CAD10", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("CAD10", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, FiltroEmpreendimentoDTO filtro)
        {
            var results =
                _viewEmpreendimentoEnderecoRepository.Listar(filtro);
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("CAD10", "Atualizar")]
        public ActionResult Fixa(long[] ids)
        {
            return AlterarTipoModalidade(ids, TipoModalidadeProgramaFidelidade.Fixa);
        }

        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("CAD10", "Atualizar")]
        public ActionResult Nominal(long[] ids)
        {
            return AlterarTipoModalidade(ids, TipoModalidadeProgramaFidelidade.Nominal);
        }

        [Transaction(TransactionAttributeType.Required)]
        private ActionResult AlterarTipoModalidade(long[] ids, TipoModalidadeProgramaFidelidade novoTipo)
        {
            var jsonResponse = new JsonResponse();

            try
            {
                var numAlterados = _empreendimentoService.AlterarTipoModalidadeProgramaFidelidade(ids, novoTipo);
                jsonResponse.Sucesso = true;
                String situacaoStr = "";
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.AlteracaoDeRegistros, GlobalMessages.Alterados.ToLower(), numAlterados,
                    ids.Count()));
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