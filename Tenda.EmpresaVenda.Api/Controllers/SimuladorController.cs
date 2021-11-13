using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Simulador;
using Tenda.EmpresaVenda.Domain.Integration.Simulador;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/simulador")]
    public class SimuladorController : BaseApiController
    {
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        private LogSimuladorRepository _logSimuladorRepository { get; set; }
        private PrePropostaService _prePropostaService { get; set; }
        private ClienteService _clienteService { get; set; }
        private SimuladorService _simuladorService { get; set; }

        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("buscarUrlMatrizOferta")]
        public HttpResponseMessage UrlMatrizOferta(ParametroSimuladorRequestDto requestDto)
        {
            var url = ProjectProperties.UrlSimulador + "integracao/landing?token=" + HttpUtility.UrlEncode(requestDto.Token) + "&returnUrl=matriz-oferta"+"&origemSimulacao="+requestDto.OrigemSimulacao;
            
            if (!requestDto.Token.Contains("ERROR"))
            {
                var parametro = _prePropostaService.ParametroMatrizOferta(requestDto.IdPreProposta);
                url += parametro;
            }

            LogarSimulador();

            return Request.CreateResponse(HttpStatusCode.OK, url);
        }

        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("buscarUrlMatrizOfertaCliente")]
        public HttpResponseMessage UrlMatrizOfertaCliente(ParametroSimuladorRequestDto requestDto)
        {
            var url = ProjectProperties.UrlSimulador + "integracao/landing?token=" + HttpUtility.UrlEncode(requestDto.Token) + "&returnUrl=matriz-oferta" + "&origemSimulacao=" + requestDto.OrigemSimulacao + "&isAgenteVenda=" + true;

            if (!requestDto.Token.Contains("ERROR"))
            {
                var parametro = _clienteService.MontarParametroMatrizOferta(requestDto.IdCliente);
                url += parametro;
            }

            LogarSimulador();

            return Request.CreateResponse(HttpStatusCode.OK, url);
        }

        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("buscarUrlSimulador")]
        public HttpResponseMessage BuscarUrlSimulador(ParametroSimuladorRequestDto requestDto)
        {
            var url = ProjectProperties.UrlSimulador + "integracao/landing?token=" + HttpUtility.UrlEncode(requestDto.Token) + "&returnUrl=simulador";
            
            if (!requestDto.Token.Contains("ERROR"))
            {
                url = _prePropostaService.MontarUrlSimulador(requestDto.Token,requestDto.IdPreProposta);
            }

            LogarSimulador();

            return Request.CreateResponse(HttpStatusCode.OK, url);
        }


        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("gerarTokenAcesso")]
        public HttpResponseMessage GerarTokenAcesso(GerarTokenAcessoSimuladorRequestDto requestDto)
        {
            var gerarToken = new GerarTokenAcessoSimuladorResponseDto();
            gerarToken.Token = "ERROR";

            try
            {
                _simuladorService = new SimuladorService();
                var token = _simuladorService.GerarTokenAcesso(requestDto.Login, requestDto.Senha, requestDto.SouCorretor, requestDto.Autorizacao, requestDto.ShowError);
                gerarToken.Token = token;

            }catch(BusinessRuleException bre)
            {
                return SendErrorResponse(HttpStatusCode.BadRequest, bre.Errors);
            }

            return Response(gerarToken);
        }

        [HttpGet]
        [AuthenticateUserByToken]
        [Uow]
        [Route("atualizarResultadosSimulacao/{codigoPreProposta}")]
        public HttpResponseMessage AtualizarResultadosSimulacao(string codigoPreProposta)
        {
            _simuladorService = new SimuladorService();
            var simulacoes = _simuladorService.BuscarSimulacaoFinalizadaPorPreProposta(codigoPreProposta);

            return Response(simulacoes);

        }

        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("detalhamentoFinanceiroBySimulador")]
        public HttpResponseMessage DetalhamentoFinanceiroBySimulador(SimuladorDto parametro)
        {
            var response = _prePropostaService.DetalhamentoFinanceiroBySimulador(parametro.DataSourceRequest, parametro);
            return Response(response);
        }

        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("aplicarDetalhamentoFinanceiro")]
        public HttpResponseMessage AplicarDetalhamentoFinanceiro(SimuladorDto parametro)
        {
            var response = new BaseResponse();

            var msgs = new List<string>();

            try
            {
                var observacoes = _prePropostaService.AplicarDetalhamentoFinanceiro(parametro);

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

                response.SuccessResponse(msgs);
                
            }
            catch (BusinessRuleException bre)
            {
                response.ErrorResponse(bre.Errors);
                response.Success = false;
                _session.Transaction.Rollback();
            }
            catch(ApiException apiEx)
            {
                response = apiEx.GetResponse();
                _session.Transaction.Rollback();
            }
            catch(Exception ex)
            {
                response.ErrorResponse(ex.Message);
                response.Success = false;
                _session.Transaction.Rollback();
            }

            return Response(response);
        }

        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("atualizarCodigoSimulacaoPreProposta")]        
        public HttpResponseMessage AtualizarCodigoSimulacaoPreProposta(SimuladorDto parametro)
        {
            var response = new BaseResponse();

            try
            {
                var preProposta = _prePropostaService.AtualizarCodigoSimulacaoPreProposta(parametro);

                response.SuccessResponse(string.Format("Pré-proposta {0} atualizada", parametro.CodigoPreProposta));

            }
            catch (ApiException ex)
            {
                response = ex.GetResponse();
            }

            return Response(response);
        }

        [HttpPost]
        [AuthenticateUserByToken]
        [Uow]
        [Route("aplicarSimulacaoAtual")]
        public HttpResponseMessage AplicarSimulacaoAtual(SimuladorDto parametro)
        {
            var response = new BaseResponse();

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

                response.SuccessResponse(msgs);

            }
            catch (BusinessRuleException bre)
            {
                response.ErrorResponse(bre.Errors);
            }
            catch (Exception ex)
            {
                response.ErrorResponse(ex.Message);
                CurrentTransaction().Rollback();
            }

            return Response(response);
        }

        private void LogarSimulador()
        {
            var usuario = _usuarioPortalRepository.FindById(RequestState.GetUserPrimaryKey());
            _logSimuladorRepository.IncluirSeNaoExistir(usuario, RequestState.CodigoSistema) ;
        }
    }
}

