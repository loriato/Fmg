using Europa.Extensions;
using Europa.Web;
using Flurl.Http;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.AgrupamentoProcessoPreProposta;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public DataSourceResponse<ViewAgrupamentoSituacaoProcessoPreProposta> ListarAgrupamentoSituacaoProcessoPreProposta(AgrupamentoSituacaoProcessoPrePropostaFiltro filtro)
        {
            var requisicao = GetBaseRequest().AppendPathSegments("AgrupamentoSituacaoProcessoPreProposta", "Listar");
            var response = requisicao.PostJsonAsync(filtro).Result;
            var result = HandleResponse<DataSourceResponse<ViewAgrupamentoSituacaoProcessoPreProposta>>(response);
            return result;
        }

        public ViewAgrupamentoSituacaoProcessoPreProposta BuscarAgrupamentoSituacaoProcessoPreProposta(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("AgrupamentoProcessoPreProposta").AppendPathSegment("buscar").AppendPathSegment(id);
            var response = request.GetAsync().Result;
            return HandleResponse<ViewAgrupamentoSituacaoProcessoPreProposta>(response);
        }

        public BaseResponse SalvarAgrupamentoSituacaoProcessoPreProposta(ViewAgrupamentoSituacaoProcessoPreProposta filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("AgrupamentoSituacaoProcessoPreProposta", "Alterar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse RemoverAgrupamentoSituacaoProcessoPreProposta(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("AgrupamentoSituacaoProcessoPreProposta", "excluir", id);
            var response = request.PostAsync().Result;
            return HandleResponse<BaseResponse>(response);
        }
    }
}
