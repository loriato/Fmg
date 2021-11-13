using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.ApiService.Models.Simulador;
using Tenda.EmpresaVenda.Domain.Integration.Simulador;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Models.Application;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class SimuladorController : BaseController
    {
        private PrePropostaService _prePropostaService { get; set; }
        private SimuladorService _simuladorService { get; set; }
        // GET: Simulador
        public ActionResult Index()
        {
            return View();
        }

        #region simulador
        [HttpPost]
        public JsonResult ResultadoSimulacao(SimuladorDto parametro)
        {
            var response = new JsonResponse();

            try
            {
                _simuladorService = new SimuladorService();
                var simulacao = _simuladorService.BuscarSimulacaoPorCodigo(parametro);

                response.Sucesso = simulacao.HasValue();
                response.Objeto = simulacao;
            }
            catch (Exception ex)
            {
                response.Sucesso = false;
                response.Mensagens.Add(ex.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AtualizarResultadosSimulacao(string preProposta)
        {
            var response = new JsonResponse();
            try
            {
                _simuladorService = new SimuladorService();
                var simulacoes = _simuladorService.BuscarSimulacaoFinalizadaPorPreProposta(preProposta);

                response.Sucesso = simulacoes.HasValue();

                if (simulacoes.HasValue())
                {
                    response.Objeto = simulacoes.OrderByDescending(x => x.Codigo).OrderByDescending(x => x.Digito).ToList();
                    response.Mensagens.Add("Simulações atualizadas");
                }
            }
            catch (Exception ex)
            {
                response.Sucesso = false;
                response.Mensagens.Add(ex.Message);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult MontarUrlSimulador(long idPreProposta)
        {
            var response = new JsonResponse();

            try
            {

                var tokenAcesso = SessionAttributes.Current().TokenAcessoSimulador;

                if (tokenAcesso.IsEmpty()||tokenAcesso.Contains("ERROR"))
                {
                    var usuario = SessionAttributes.Current().UsuarioPortal;

                    _simuladorService = new SimuladorService();
                    tokenAcesso = _simuladorService.GerarTokenAcesso(usuario.Login, usuario.Senha,false,null,true);

                    SessionAttributes.Current().TokenAcessoSimulador = tokenAcesso;
                }

                var url = _prePropostaService.MontarUrlSimulador(tokenAcesso, idPreProposta);

                response.Sucesso = true;
                response.Objeto = url;
            }
            catch (BusinessRuleException bre)
            {
                ExceptionLogger.LogException(bre);
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
                SessionAttributes.Current().TokenAcessoSimulador = "ERROR";
            }
            catch (Exception err)
            {
                ExceptionLogger.LogException(err);
                response.Sucesso = false;
                response.Mensagens.Add(err.Message);
                SessionAttributes.Current().TokenAcessoSimulador = "ERROR";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DetalhamentoFinanceiroBySimulador(DataSourceRequest request, SimuladorDto parametro)
        {
            var response = _prePropostaService.DetalhamentoFinanceiroBySimulador(request, parametro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ItbiEmolumentoBySimulador(DataSourceRequest request, long idPreProposta)
        {
            var response = _prePropostaService.ItbiEmolumento(request, idPreProposta);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DetalhamentoFinanceiro(DataSourceRequest request, long idPreProposta)
        {
            var response = _prePropostaService.DetalhamentoFinanceiro(request, idPreProposta);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AplicarDetalhamentoFinanceiro(SimuladorDto parametro)
        {
            var response = new JsonResponse();

            try
            {
                var result = EmpresaVendaApi.AplicarDetalhamentoFinanceiro(parametro);
                response.FromBaseResponse(result);
            }
            catch (ApiException e)
            {
                response.FromBaseResponse(e.GetResponse());
            }
            catch (Exception ex)
            {
                response.Mensagens.Add(ex.Message);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AplicarSimulacaoAtual(SimuladorDto parametro)
        {
            var response = new JsonResponse();

            var msgs = new List<string>();

            try
            {
                var observacoes = _prePropostaService.AplicarSimulacaoAtual(parametro);

                if (observacoes.HasValue())
                {
                    msgs.Add(string.Format("{0} {1} com {2}.",
                    GlobalMessages.DetalhamentoFinanceiro,
                    GlobalMessages.Atualizado,
                    GlobalMessages.Observacoes));

                    msgs.AddRange(observacoes);
                }
                else
                {
                    msgs.Add(string.Format("{0} {1} com {2}.",
                    GlobalMessages.DetalhamentoFinanceiro,
                    GlobalMessages.Atualizado,
                    GlobalMessages.Sucesso));
                }

                response.Mensagens.AddRange(msgs);
                response.Sucesso = true;

            }
            catch (BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
                response.Sucesso = false;
            }
            catch (Exception ex)
            {
                response.Mensagens.Add(ex.Message);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AtualizarTotalFinanceiro(long idPreProposta)
        {
            var response = new JsonResponse();
            try
            {
                _prePropostaService.AtualizarTotalFinanceiro(idPreProposta);

                response.Sucesso = true;
                response.Mensagens.Add(string.Format("{0} {1} {2}", GlobalMessages.DetalhamentoFinanceiro, GlobalMessages.Atualizado, GlobalMessages.Sucesso));
            }
            catch (Exception ex)
            {
                response.Sucesso = false;
                response.Mensagens.Add(ex.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BuscarUrlMatrizOferta(long idPreProposta)
        {
            var response = new JsonResponse();
            var url = "";
            try
            {

                var tokenAcesso = SessionAttributes.Current().TokenAcessoSimulador;

                if (tokenAcesso.IsEmpty() || tokenAcesso.Contains("ERROR"))
                {
                    var login = SessionAttributes.Current().UsuarioPortal.Login;
                    var autorizacao = "AD_ADMIN";
                    var senha = DateTime.Now.ToString();

                    var gerarToken = EmpresaVendaApi.GerarTokenAcessoSimulador(login, senha, false, autorizacao, true);

                    if (gerarToken.HasValue())
                    {
                        SessionAttributes.Current().TokenAcessoSimulador = gerarToken.Token;
                        tokenAcesso = gerarToken.Token;
                    }
                }

                url = EmpresaVendaApi.BuscarUrlMatrizOferta(tokenAcesso, idPreProposta, TipoOrigemSimulacao.Admin);

                response.Objeto = url;
                response.Sucesso = true;
            }
            catch (ApiException apiEx)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(apiEx.GetResponse().Messages);
                SessionAttributes.Current().TokenAcessoSimulador = null;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                SessionAttributes.Current().TokenAcessoSimulador = null;
                response.Mensagens.Add(ex.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}