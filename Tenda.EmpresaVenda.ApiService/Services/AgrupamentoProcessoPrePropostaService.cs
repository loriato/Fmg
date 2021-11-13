using Europa.Extensions;
using Europa.Web;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.AgrupamentoProcessoPreProposta;
using Tenda.EmpresaVenda.ApiService.Models.StatusPreProposta;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public DataSourceResponse<ViewAgrupamentoProcessoPreProposta> ListarAgrupamentoProcessoPreProposta(AgrupamentoProcessoPrePropostaFiltro filtro)
        {
            var requisicao = GetBaseRequest().AppendPathSegments("agrupamentoprocessopreproposta", "listar");
            var response = requisicao.PostJsonAsync(filtro).Result;
            var result = HandleResponse<DataSourceResponse<ViewAgrupamentoProcessoPreProposta>>(response);
            return result;
        }
        public DataSourceResponse<AgrupamentoProcessoPrePropostaDto> AutoCompletePrePropostaHouseAgrupamentoProcessoPreProposta(AgrupamentoProcessoPrePropostaFiltro filtro)
        {
            var requisicao = GetBaseRequest().AppendPathSegments("agrupamentoprocessopreproposta", "AutoCompletePrePropostaHouse");
            var response = requisicao.PostJsonAsync(filtro).Result;
            var result = HandleResponse<DataSourceResponse<AgrupamentoProcessoPrePropostaDto>>(response);
            return result;
        }
        
        public ViewAgrupamentoProcessoPreProposta BuscarAgrupamentoProcessoPreProposta(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("agrupamentoprocessopreproposta").AppendPathSegment("buscar").AppendPathSegment(id);
            var response = request.GetAsync().Result;
            return HandleResponse<ViewAgrupamentoProcessoPreProposta>(response);
        }

        public BaseResponse SalvarAgrupamentoProcessoPreProposta(ViewAgrupamentoProcessoPreProposta filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("agrupamentoprocessopreproposta", "Alterar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse RemoverAgrupamentoProcessoPreProposta(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("agrupamentoprocessopreproposta", "excluir", id);
            var response = request.PostAsync().Result;
            return HandleResponse<BaseResponse>(response);
        }
    }
}
