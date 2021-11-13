using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System.Collections.Generic;
using System.Web.Mvc;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC18")]
    public class TransferenciaPontoVendaController : BaseController
    {
        private ViewPrePropostaRepository _viewPrePropostaRepository { get; set; }
        private PrePropostaService _prePropostaService { get; set; }
        private PrePropostaRepository _prePropostaRepository { get; set; }
        public PontoVendaService _pontoVendaService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        //[BaseAuthorize("GEC03", "Atualizar")]
        //[Transaction(TransactionAttributeType.Required)]
        //public JsonResult TransferirSelecionados(List<long> idsPreProposta,long idViabilizador)
        //{
        //    var json = new JsonResponse();
        //    try
        //    {
        //       var numAlterados =  _prePropostaService.TransferirCarteira(idsPreProposta,idViabilizador);
        //
        //        json.Sucesso = true;
        //        json.Mensagens.Add(string.Format(GlobalMessages.RegistrosTransferidos, numAlterados));
        //    }
        //    catch (BusinessRuleException ex)
        //    {
        //        CurrentSession().Transaction.Rollback();
        //        json.Mensagens.AddRange(ex.Errors);
        //        json.Campos.AddRange(ex.ErrorsFields);
        //    }
        //
        //    return Json(json, JsonRequestBehavior.AllowGet);
        //}

        [BaseAuthorize("GEC18", "AtualizarPontoVenda")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult TransferirTodos(long idViabilizador, long idNovoViabilizador, long? idPontoVenda)
        {
            var json = new JsonResponse();
            try
            {
                var idsPreProposta = _prePropostaRepository.ListarIdsPrePropostaDeViabilizadorComEvs(idViabilizador, null, idPontoVenda);


                var numAlterados = _prePropostaService.TransferirCarteira(idsPreProposta, idNovoViabilizador);

                if (idPontoVenda.HasValue())
                {
                    _pontoVendaService.TrocarVializadorPontoVenda(idViabilizador, idNovoViabilizador, idPontoVenda.Value);
                }
                else
                {
                    _pontoVendaService.TrocarVializadorPontoVenda(idViabilizador, idNovoViabilizador);
                }

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