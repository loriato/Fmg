using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC03")]
    public class TransferenciaCarteiraController : BaseController
    {
        private ViewPrePropostaRepository _viewPrePropostaRepository { get; set; }
        private PrePropostaService _prePropostaService { get; set; }
        private PrePropostaRepository _prePropostaRepository { get; set; }
        public PontoVendaService _pontoVendaService { get; set; }

        // GET: TransferenciaCarteira
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("GEC03", "Visualizar")]
        public JsonResult Listar(DataSourceRequest request, long? idViabilizador , List<long> idEmpresaVenda, long? idPontoVenda)
        {
            var results = _viewPrePropostaRepository.ListarCarteira(request, idViabilizador, idEmpresaVenda, idPontoVenda);


            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC03", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult TransferirSelecionados(List<long> idsPreProposta,long idViabilizador)
        {
            var json = new JsonResponse();
            try
            {
               var numAlterados =  _prePropostaService.TransferirCarteira(idsPreProposta,idViabilizador);

                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.RegistrosTransferidos, numAlterados));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC03", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult TransferirTodos(long? idViabilizador, long idNovoViabilizador, List<long> idEmpresaVenda)
        {
            var json = new JsonResponse();
            try
            {
                var idsPreProposta = _prePropostaRepository.ListarIdsPrePropostaDeViabilizadorComEvs(idViabilizador, idEmpresaVenda, null);
                    

                var numAlterados = _prePropostaService.TransferirCarteira(idsPreProposta, idNovoViabilizador);
                //_pontoVendaService.TrocarVializadorPontoVenda(idViabilizador, idNovoViabilizador);

                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.RegistrosTransferidos, numAlterados));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}