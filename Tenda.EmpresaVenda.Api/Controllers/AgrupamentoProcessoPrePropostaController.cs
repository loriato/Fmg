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

    [RoutePrefix("api/agrupamentoprocessopreproposta")]
    public class AgrupamentoProcessoPrePropostaController : BaseApiController
    {
        private AgrupamentoProcessoPrePropostaService _agrupamentoProcessoPrePropostaService { get; set; }

        [Route("listar")]
        public HttpResponseMessage Listar(AgrupamentoProcessoPrePropostaFiltro filtro)
        {
            var dataSource = _agrupamentoProcessoPrePropostaService.ListarDto(filtro);
            var response = dataSource.CloneDataSourceResponse(dataSource.records.ToList());
            return Response(response);
        }

        [Route("AutoCompletePrePropostaHouse")]
        public HttpResponseMessage AutoCompletePrePropostaHouse(AgrupamentoProcessoPrePropostaFiltro filtro)
        {
            var dataSource = _agrupamentoProcessoPrePropostaService.AutoCompletePrePropostaHouse(filtro);
            var response = dataSource.CloneDataSourceResponse(dataSource.records.ToList());
            return Response(response);
        }
        [HttpPost]
        [Route("Alterar")]
        [AuthenticateUserByToken("CAD19", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Salvar(ViewAgrupamentoProcessoPreProposta agrupamento)
        {
            var response = new BaseResponse();
            try
            {
                var inclusao = (agrupamento.Id == 0 ? true : false);
                var result = new ViewAgrupamentoProcessoPreProposta();
                agrupamento = _agrupamentoProcessoPrePropostaService.Salvar(agrupamento);
                response.Data = result;
                response.SuccessResponse(string.Format(GlobalMessages.RegistroSalvo, agrupamento.Nome,
                    (inclusao ? GlobalMessages.Incluido.ToLower():GlobalMessages.Alterado.ToLower())));
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
        [AuthenticateUserByToken("CAD19", "Visualizar")]
        public HttpResponseMessage Buscar(long idAgrupamentoProcessoPreProposta)
        {
            return Response(_agrupamentoProcessoPrePropostaService.BuscarId(idAgrupamentoProcessoPreProposta));
        }

        [HttpPost]
        [Route("excluir/{id}")]
        [AuthenticateUserByToken("CAD19", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Excluir(long id)
        {
            var response = new BaseResponse();
            try
            {                
                var agrupamento = _agrupamentoProcessoPrePropostaService.Remover(id);
                response.Data = agrupamento;
                response.SuccessResponse(string.Format(GlobalMessages.RemovidoSucesso, agrupamento.Nome,
                    GlobalMessages.Excluido.ToLower()));
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