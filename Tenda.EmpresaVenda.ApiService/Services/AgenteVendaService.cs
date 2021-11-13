using Europa.Extensions;
using Europa.Web;
using Flurl.Http;
using Tenda.EmpresaVenda.ApiService.Models.AgenteVenda;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public DataSourceResponse<AgenteVendaDto> ListarDatatableUsuario(FiltroAgenteVendaDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("agentesVenda", "listar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<AgenteVendaDto>>(response);
        }

        public BaseResponse AtribuirUsuarioLoja(AgenteVendaDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("agentesVenda");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public DataSourceResponse<EntityDto> ListarAgentesVendaAutocomplete(DataSourceRequest dataSourceRequest)
        {
            var request = GetBaseRequest().AppendPathSegments("agentesVenda", "autocomplete");
            var response = request.PostJsonAsync(dataSourceRequest).Result;
            return HandleResponse<DataSourceResponse<EntityDto>>(response);
        }
    }
}
