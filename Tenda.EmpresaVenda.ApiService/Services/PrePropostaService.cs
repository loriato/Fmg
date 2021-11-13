using Europa.Extensions;
using Europa.Web;
using Flurl.Http;
using Tenda.EmpresaVenda.ApiService.Models.PlanoPagamento;
using Tenda.EmpresaVenda.ApiService.Models.PreProposta;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public PrePropostaCreateDto BuscarPreProposta(FiltroPrePropostaDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "buscar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<PrePropostaCreateDto>(response);
        }

        public BaseResponse IncluirPreProposta(PrePropostaCreateDto model)
        {
            var request = GetBaseRequest().AppendPathSegment("preProposta");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse AlterarPreProposta(PrePropostaCreateDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "alterar");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public DataSourceResponse<PlanoPagamentoDto> ListarDetalhamentoFinanceiro(FilterIdDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "detalhamentosFinanceiros", "listar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<PlanoPagamentoDto>>(response);
        }

        public BaseResponse IncluirDetalhamentoFinanceiro(PlanoPagamentoDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "detalhamentosFinanceiros");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse AlterarDetalhamentoFinanceiro(PlanoPagamentoDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "detalhamentosFinanceiros", "alterar");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public PrePropostaCreateDto RecalcularTotalFinanceiroPreProposta(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", id, "recalcularTotalFinanceiro");
            var response = request.PostAsync().Result;
            return HandleResponse<PrePropostaCreateDto>(response);
        }

        public BaseResponse ExcluirDetalhamentoFinanceiro(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "detalhamentosFinanceiros", "excluir", id);
            var response = request.PostAsync().Result;
            return HandleResponse<BaseResponse>(response);
        }

        public DataSourceResponse<PlanoPagamentoDto> ListarItbiEmolumentos(FilterIdDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "itbiEmolumentos", "listar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<PlanoPagamentoDto>>(response);
        }

        public BaseResponse IncluirItbiEmolumento(PlanoPagamentoDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "itbiEmolumentos", "incluir");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse AlterarItbiEmolumento(PlanoPagamentoDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "itbiEmolumentos", "alterar");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse ExcluirItbiEmolumento(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "itbiEmolumentos", "excluir", id);
            var response = request.PostAsync().Result;
            return HandleResponse<BaseResponse>(response);
        }

    }
}
