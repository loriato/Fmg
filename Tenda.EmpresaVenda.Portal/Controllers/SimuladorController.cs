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
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.ApiService.Models.Simulador;
using Tenda.EmpresaVenda.Domain.Dto;
using Tenda.EmpresaVenda.Domain.Integration.Simulador;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Portal.Models.Application;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    public class SimuladorController : BaseController
    {
        private PrePropostaService _prePropostaService { get; set; }
        private SimuladorService _simuladorService { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private ClienteService _clienteService { get; set; }

        // GET: Simulador
        public ActionResult Index()
        {
            var tokenAcesso = SessionAttributes.Current().TokenAcessoSimulador;

            if (tokenAcesso.IsEmpty() || tokenAcesso.Contains("ERROR"))
            {
                var usuario = SessionAttributes.Current().UsuarioPortal;
                var autorizacao = SessionAttributes.Current().Acesso?.Autorizacao;

                var idEmpresaVenda = SessionAttributes.Current().EmpresaVendaId;
                var idSapLoja = _empresaVendaRepository.BuscarIdSapLojaPorIdEmpresaVenda(idEmpresaVenda);

                _simuladorService = new SimuladorService();
                tokenAcesso = _simuladorService.GerarTokenAcesso(usuario.Login, usuario.Senha,true,autorizacao,false,idSapLoja);

                SessionAttributes.Current().TokenAcessoSimulador = tokenAcesso;
            }

            var url = ProjectProperties.UrlSimulador + "integracao/landing?token=" + HttpUtility.UrlEncode(tokenAcesso) + "&returnUrl=index";

            return Redirect(url);
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

                if (tokenAcesso.IsEmpty() || tokenAcesso.Contains("ERROR"))
                {
                    var usuario = SessionAttributes.Current().UsuarioPortal;
                    var autorizacao = SessionAttributes.Current().Acesso?.Autorizacao;

                    var idEmpresaVenda = SessionAttributes.Current().EmpresaVendaId;
                    var idSapLoja = _empresaVendaRepository.BuscarIdSapLojaPorIdEmpresaVenda(idEmpresaVenda);

                    _simuladorService = new SimuladorService();
                    tokenAcesso = _simuladorService.GerarTokenAcesso(usuario.Login, usuario.Senha,true,autorizacao,true,idSapLoja);

                    SessionAttributes.Current().TokenAcessoSimulador = tokenAcesso;
                }

                var url = _prePropostaService.MontarUrlSimulador(tokenAcesso, idPreProposta);

                response.Sucesso = true;
                response.Objeto = url;
            }
            catch (BusinessRuleException bre)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MontarUrlSimuladorMenu()
        {
            var response = new JsonResponse();

            try
            {
                var tokenAcesso = SessionAttributes.Current().TokenAcessoSimulador;

                if (tokenAcesso.IsEmpty() || tokenAcesso.Contains("ERROR"))
                {
                    var usuario = SessionAttributes.Current().UsuarioPortal;
                    var autorizacao = SessionAttributes.Current().Acesso?.Autorizacao;

                    var idEmpresaVenda = SessionAttributes.Current().EmpresaVendaId;
                    var idSapLoja = _empresaVendaRepository.BuscarIdSapLojaPorIdEmpresaVenda(idEmpresaVenda);

                    _simuladorService = new SimuladorService();
                    tokenAcesso = _simuladorService.GerarTokenAcesso(usuario.Login, usuario.Senha, true, autorizacao, true,idSapLoja);

                    SessionAttributes.Current().TokenAcessoSimulador = tokenAcesso;
                }

                var url = ProjectProperties.UrlSimulador + "integracao/landing?token=" + HttpUtility.UrlEncode(tokenAcesso) + "&returnUrl=index";


                response.Sucesso = true;
                response.Objeto = url;
            }
            catch (BusinessRuleException bre)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DetalhamentoFinanceiroBySimulador(DataSourceRequest request, SimuladorDto parametro)
        {
            var response = _prePropostaService.DetalhamentoFinanceiroBySimulador(request, parametro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ItbiEmolumento(DataSourceRequest request, long idPreProposta)
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
                var observacoes = _prePropostaService.AplicarDetalhamentoFinanceiro(parametro);

                response.Sucesso = true;

                if (observacoes.HasValue()) {
                    response.Mensagens.Add(string.Format("{0} {1} com {2}.",
                    GlobalMessages.DetalhamentoFinanceiro,
                    GlobalMessages.Atualizado,
                    GlobalMessages.Observacoes));

                    response.Mensagens.AddRange(observacoes);
                }
                else
                {
                    response.Mensagens.Add(string.Format("{0} {1} com {2}.",
                    GlobalMessages.DetalhamentoFinanceiro,
                    GlobalMessages.Atualizado,
                    GlobalMessages.Sucesso));
                }

            }catch(BusinessRuleException bre)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
                _session.Transaction.Rollback();
            }
            catch(ApiException apiEx)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(apiEx.GetResponse().Messages);
                _session.Transaction.Rollback();
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
            }catch(Exception ex)
            {
                response.Sucesso = false;
                response.Mensagens.Add(ex.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrePropostaSimulador(PrePropostaDTO prePropostaDTO)
        {
            var preProposta = new PreProposta();

            try
            {
                prePropostaDTO.UsuarioPortal = SessionAttributes.Current().UsuarioPortal;
                prePropostaDTO.EmpresaVenda = SessionAttributes.Current().EmpresaVenda;
                prePropostaDTO.Corretor = SessionAttributes.Current().Corretor;
                preProposta = _prePropostaService.PrePropostaSimulador(prePropostaDTO);

            }catch(BusinessRuleException bre)
            {
                ViewBag.Message = "";
                ViewBag.StackTracer = bre.Errors;
                return View("Error");
            }

            return RedirectToAction("Index", "PreProposta", new { Id = preProposta.Id,IdCliente=preProposta.Cliente.Id });

        }

        #region Matriz de Oferta

        [HttpGet]
        public JsonResult MontarUrlMatrizOfertaMenu()
        {
            var response = new JsonResponse();

            try
            {
                var tokenAcesso = SessionAttributes.Current().TokenAcessoSimulador;

                if (tokenAcesso.IsEmpty() || tokenAcesso.Contains("ERROR"))
                {
                    var usuario = SessionAttributes.Current().UsuarioPortal;
                    var autorizacao = SessionAttributes.Current().Acesso?.Autorizacao;

                    var idEmpresaVenda = SessionAttributes.Current().EmpresaVendaId;
                    var idSapLoja = _empresaVendaRepository.BuscarIdSapLojaPorIdEmpresaVenda(idEmpresaVenda);

                    _simuladorService = new SimuladorService();
                    tokenAcesso = _simuladorService.GerarTokenAcesso(usuario.Login, usuario.Senha, true, autorizacao, true, idSapLoja);

                    SessionAttributes.Current().TokenAcessoSimulador = tokenAcesso;
                }

                var url = ProjectProperties.UrlSimulador + "integracao/landing?token=" + HttpUtility.UrlEncode(tokenAcesso) + "&returnUrl=matriz-oferta";

                response.Sucesso = true;
                response.Objeto = url;
            }
            catch (BusinessRuleException bre)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UrlMatrizOfertaCliente(ParametroSimuladorRequestDto requestDto)
        {
            var response = new BaseResponse();

            try
            {
                var tokenAcesso = SessionAttributes.Current().TokenAcessoSimulador;
                
                if (tokenAcesso.IsEmpty() || tokenAcesso.Contains("ERROR"))
                {
                    var usuario = SessionAttributes.Current().UsuarioPortal;
                    var autorizacao = SessionAttributes.Current().Acesso?.Autorizacao;

                    var idEmpresaVenda = SessionAttributes.Current().EmpresaVendaId;
                    var idSapLoja = _empresaVendaRepository.BuscarIdSapLojaPorIdEmpresaVenda(idEmpresaVenda);

                    _simuladorService = new SimuladorService();
                    tokenAcesso = _simuladorService.GerarTokenAcesso(usuario.Login, usuario.Senha, true, autorizacao, true, idSapLoja);

                    SessionAttributes.Current().TokenAcessoSimulador = tokenAcesso;
                }

                var url = ProjectProperties.UrlSimulador + "integracao/landing?token=" + HttpUtility.UrlEncode(tokenAcesso) + "&returnUrl=matriz-oferta";

                if (!tokenAcesso.Contains("ERROR"))
                {
                    var parametro = _clienteService.MontarParametroMatrizOferta(requestDto.IdCliente);
                    url += parametro;
                }

                response.Data = url;
                response.SuccessResponse(url);

            }
            catch (BusinessRuleException bre)
            {
                response.ErrorResponse(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarUrlMatrizOferta(SimuladorDto parametro)
        {
            var response = new BaseResponse();
            try
            {

                var tokenAcesso = SessionAttributes.Current().TokenAcessoSimulador;

                if (tokenAcesso.IsEmpty() || tokenAcesso.Contains("ERROR"))
                {
                    var usuario = SessionAttributes.Current().UsuarioPortal;
                    var autorizacao = SessionAttributes.Current().Acesso?.Autorizacao;

                    var idEmpresaVenda = SessionAttributes.Current().EmpresaVendaId;
                    var idSapLoja = _empresaVendaRepository.BuscarIdSapLojaPorIdEmpresaVenda(idEmpresaVenda);

                    _simuladorService = new SimuladorService();
                    tokenAcesso = _simuladorService.GerarTokenAcesso(usuario.Login, usuario.Senha, true, autorizacao, true, idSapLoja);

                    SessionAttributes.Current().TokenAcessoSimulador = tokenAcesso;
                }

                var url = ProjectProperties.UrlSimulador + "integracao/landing?token=" + HttpUtility.UrlEncode(tokenAcesso) + "&returnUrl=matriz-oferta";

                if (!tokenAcesso.Contains("ERROR"))
                {
                    url += _prePropostaService.ParametroMatrizOferta(parametro.IdPreProposta);
                }

                response.Data = url;
                response.SuccessResponse(url);
            }
            catch (ApiException apiEx)
            {
                response = apiEx.GetResponse();
                SessionAttributes.Current().TokenAcessoSimulador = null;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                SessionAttributes.Current().TokenAcessoSimulador = null;
                response.ErrorResponse(ex.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}