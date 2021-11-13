using Europa.Extensions;
using Europa.Web;
using Flurl.Http;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public KanbanPrePropostaDto IndexKanbanPreProposta()
        {
            var request = GetBaseRequest().AppendPathSegments("kanbanPreProposta", "index");
            var result = request.GetAsync().Result;
            return HandleResponse<KanbanPrePropostaDto>(result);

        }

        public BaseResponse SalvarAreaKanbanPreProposta(AreaKanbanPrePropostaDto areaKanbanPrePropostaDto)
        {
            var request = GetBaseRequest().AppendPathSegments("kanbanPreProposta", "salvarAreaKanbanPreProposta");
            var result = request.PostJsonAsync(areaKanbanPrePropostaDto).Result;
            return HandleResponse<BaseResponse>(result);
        }

        public AreaKanbanPrePropostaDto BuscarAreaKanbanPreProposta(long idAreaKanbanPreProposta)
        {
            var request = GetBaseRequest().AppendPathSegments("kanbanPreProposta", "buscarAreaKanbanPreProposta", idAreaKanbanPreProposta);
            var result = request.GetAsync().Result;
            return HandleResponse<AreaKanbanPrePropostaDto>(result);
        }

        public List<CardKanbanPrePropostaDto> ListarCardsKanbanPreProposta(long idAreaKanbanPreProposta)
        {
            var request = GetBaseRequest().AppendPathSegments("kanbanPreProposta", "listarCardsKanbanPreProposta", idAreaKanbanPreProposta);
            var result = request.GetAsync().Result;
            return HandleResponse<List<CardKanbanPrePropostaDto>>(result);
        }

        public DataSourceResponse<ViewPreProposta> ListarCardsComPreProposta(FiltroKanbanPrePropostaDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("kanbanPreProposta", "ListarCardsComPreProposta");
            var result = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ViewPreProposta>>(result);
        }

        public BaseResponse ExcluirAreaKanbanPreProposta(long idAreaKanbanPreProposta)
        {
            var request = GetBaseRequest().AppendPathSegments("kanbanPreProposta", "excluirAreaKanbanPreProposta", idAreaKanbanPreProposta);
            var result = request.PostAsync().Result;
            return HandleResponse<BaseResponse>(result);
        }

        public BaseResponse SalvarCardKanbanPreProposta(CardKanbanPrePropostaDto cardKanbanPrePropostaDto)
        {
            var request = GetBaseRequest().AppendPathSegments("kanbanPreProposta", "salvarCardKanbanPreProposta");
            var result = request.PostJsonAsync(cardKanbanPrePropostaDto).Result;
            return HandleResponse<BaseResponse>(result);
        }

        public CardKanbanPrePropostaDto BuscarCardKanbanPreProposta(long idCardKanbanPreProposta)
        {
            var request = GetBaseRequest().AppendPathSegments("kanbanPreProposta", "buscarCardKanbanPreProposta", idCardKanbanPreProposta);
            var result = request.GetAsync().Result;
            return HandleResponse<CardKanbanPrePropostaDto>(result);
        }

        public DataSourceResponse<ViewCardKanbanSituacaoPreProposta> ListarSituacaoCardKanban(FiltroKanbanPrePropostaDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("kanbanPreProposta", "listarSituacaoCardKanban");
            var result = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ViewCardKanbanSituacaoPreProposta>>(result);
        }

        public DataSourceResponse<EntityDto> AutoCompleteSituacaoKanbanPreProposta(FiltroKanbanPrePropostaDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("kanbanPreProposta", "autoCompleteSituacaoKanbanPreProposta");
            var result = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<EntityDto>>(result);
        }

        public BaseResponse AdicionarSituacaoCardKanban(CardKanbanSituacaoPrePropostaDto cardKanbanSituacaoPrePropostaDto)
        {
            var request = GetBaseRequest().AppendPathSegments("kanbanPreProposta", "adicionarSituacaoCardKanban");
            var result = request.PostJsonAsync(cardKanbanSituacaoPrePropostaDto).Result;
            return HandleResponse<BaseResponse>(result);
        }

        public BaseResponse RemoverSituacaoCardKanban(long idCardKanbanSituacaoPreProposta)
        {
            var request = GetBaseRequest().AppendPathSegments("kanbanPreProposta", "removerSituacaoCardKanban", idCardKanbanSituacaoPreProposta);
            var result = request.DeleteAsync().Result;
            return HandleResponse<BaseResponse>(result);
        }

        public BaseResponse RemoverCardKanbanPreProposta(long idCardKanbanPreProposta)
        {
            var request = GetBaseRequest().AppendPathSegments("kanbanPreProposta", "removerCardKanbanPreProposta", idCardKanbanPreProposta);
            var result = request.DeleteAsync().Result;
            return HandleResponse<BaseResponse>(result);
        }
    }
}
