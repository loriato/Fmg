using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using System;
using System.Net.Http;
using System.Web.Http;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.PrePropostaWorkflow;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/prePropostaWorkflow")]
    public class PrePropostaWorkflowController : BaseApiController
    {
        private PrePropostaRepository _prePropostaRepository { get; set; }
        private PrePropostaService _prePropostaService { get; set; }
        private NotificacaoRepository _notificacaoRepository { get; set; }

        [HttpPost]
        [Route("aguardandoFluxoAguardandoAnaliseCompletaRule")]
        [Uow]
        public HttpResponseMessage AguardandoFluxoAguardandoAnaliseCompletaRule(PrePropostaWorkflowDto prePropostaWorkflowDto)
        {
            var response = new BaseResponse();

            try
            {
                var preProposta = _prePropostaRepository.FindById(prePropostaWorkflowDto.IdPreProposta);

                if (preProposta.IsEmpty())
                {
                    response.ErrorResponse(string.Format("Pré-Proposta não encontrada"));
                    return Response(response);
                }

                var historicoPreProposta = _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoAnaliseCompleta,
                            RequestState.UsuarioPortal);

                response.SuccessResponse(string.Format("A pré-proposta {0} foi avançada para {1}", preProposta.Codigo,
                    preProposta.SituacaoProposta.AsString()));
            }catch(BusinessRuleException bre)
            {
                response.ErrorResponse(bre.Errors);
            }catch(ApiException apiEx)
            {
                response = apiEx.GetResponse();
            }

            return Response(response);
        }

        [HttpPost]
        [Route("reenviarAnaliseCompletaAprovada")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage ReenviarAnaliseCompletaAprovada(ReenviarAnaliseCompletaDto dto)
        {
            var response = new BaseResponse();

            try
            {
                var preProposta = _prePropostaRepository.FindById(dto.IdPreProposta);
                preProposta.JustificativaReenvio = dto.Justificativa;
                _prePropostaService.ReenviarAnaliseCompletaAprovada(preProposta, SituacaoProposta.AnaliseCompletaAprovada, RequestState.UsuarioPortal, RequestState.Perfis);

                var model = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };

                response.SuccessResponse(string.Format(GlobalMessages.MsgAlteracaoEtapaPreProposta, SituacaoProposta.AnaliseCompletaAprovada.AsString()));
            }
            catch (BusinessRuleException e)
            {
                FromBusinessRuleException(e);
            }

            return Response(response);
        }


        [HttpPost]
        [Route("aguardandoIntegracao")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage AguardandoIntegracao(PrePropostaWorkflowDto prePropostaWorkflowDto)
        {
            var response = new BaseResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(prePropostaWorkflowDto.IdPreProposta);

                //caso a PPR esteja vindo pelo fluxo simplificado (Fluxo envido > aguardando integração) Sicaq previa é passado pro fixo
                if (preProposta.SituacaoProposta == SituacaoProposta.FluxoEnviado || preProposta.SituacaoProposta == SituacaoProposta.AguardandoFluxo)
                {
                    preProposta.DataSicaq = preProposta.DataSicaqPrevio;
                    preProposta.StatusSicaq = preProposta.StatusSicaqPrevio;
                    preProposta.ParcelaAprovada = preProposta.ParcelaAprovadaPrevio;
                    preProposta.FaixaUmMeio = preProposta.FaixaUmMeioPrevio;
                } 

                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoIntegracao, RequestState.UsuarioPortal, null, RequestState.Perfis);

                var notificacao = new Notificacao
                {
                    Titulo = string.Format(GlobalMessages.NotificacaoProposta_AguardandoIntegracaoTitulo, preProposta.Codigo, preProposta.Cliente.NomeCompleto),
                    Conteudo = GlobalMessages.NotificacaoProposta_AguardandoIntegracaoCorpo,
                    Usuario = preProposta.Viabilizador,
                    EmpresaVenda = preProposta.EmpresaVenda,
                    TipoNotificacao = TipoNotificacao.Comum,
                    DestinoNotificacao = DestinoNotificacao.Adm,
                };

                _notificacaoRepository.Save(notificacao);
                response.SuccessResponse(string.Format(GlobalMessages.MsgAlteracaoEtapaPreProposta, SituacaoProposta.AguardandoIntegracao.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                FromBusinessRuleException(ex);
            }
            return Response(response);
        }

        [HttpPost]
        [Route("workflow")]
        public HttpResponseMessage Workflow(long idPreProposta)
        {
            var response = new BaseResponse();

            try
            {

            }
            catch (ApiException apiEx)
            {
                response = apiEx.GetResponse();
                _session.Transaction.Rollback();
            }
            catch (BusinessRuleException bre)
            {
                response.ErrorResponse(bre.Errors);
                _session.Transaction.Rollback();
            }
            catch(Exception ex)
            {
                response.ErrorResponse(ex.Message);
                _session.Transaction.Rollback();
            }

            return Response(response);
        }
    }
}