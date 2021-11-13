
using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.AgrupamentoProcessoPreProposta;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/AgrupamentoSituacaoProcessoPreProposta")]
    public class AgrupamentoSituacaoProcessoPrePropostaController : BaseApiController
    {
        private AgrupamentoSituacaoProcessoPrePropostaService _agrupamentoSituacaoProcessoPrePropostaService { get; set; }

        [Route("listar")]
        public HttpResponseMessage Listar(AgrupamentoSituacaoProcessoPrePropostaFiltro filtro)
        {
            var dataSource = _agrupamentoSituacaoProcessoPrePropostaService.ListarView(filtro);
            var response = dataSource.CloneDataSourceResponse(dataSource.records);
            return Response(response);
        }
        [HttpPost]
        [Route("Alterar")]
        [AuthenticateUserByToken("CAD19", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Salvar(ViewAgrupamentoSituacaoProcessoPreProposta agrupamentoSituacao)
        {
            var response = new BaseResponse();
            try
            {
                var result = new ViewAgrupamentoSituacaoProcessoPreProposta();
                agrupamentoSituacao = _agrupamentoSituacaoProcessoPrePropostaService.Salvar(agrupamentoSituacao);
                response.Data = result;
                response.SuccessResponse(string.Format(GlobalMessages.RegistroSalvo, agrupamentoSituacao.StatusPreProposta + " do agrupamento " + agrupamentoSituacao.NomeAgrupamento,
                    GlobalMessages.Incluido.ToLower()));
            }
            catch (ApiException apiEx)
            {
                response = apiEx.GetResponse();
                CurrentTransaction().Rollback();
            }
            catch (BusinessRuleException ex)
            {
                response.Messages.AddRange(ex.Errors);
                response.Success = false;
            }
            return Response(response);
        }
        [HttpPost]
        [Route("Excluir/{id}")]
        [AuthenticateUserByToken("CAD19", "ExcluirCruzamento")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Excluir(long id)
        {
            var response = new BaseResponse();
            try
            {
                var cruzamentoAgrupamento = _agrupamentoSituacaoProcessoPrePropostaService.Remover(id);
                response.Data = cruzamentoAgrupamento;
                response.SuccessResponse(String.Format(GlobalMessages.RemoverAgrupamentoStatusPreProposta, GlobalMessages.Sucesso, cruzamentoAgrupamento.StatusPreProposta.StatusPadrao,cruzamentoAgrupamento.Agrupamento.Nome));
            }
            catch (ApiException apiEx)
            {
                response = apiEx.GetResponse();
                CurrentTransaction().Rollback();
            }
            catch (BusinessRuleException ex)
            {
                response.Messages.AddRange(ex.Errors);
                response.Success = false;
            }
            return Response(response);
        }
    }
}