using Europa.Extensions;
using Europa.Web;
using Flurl.Http;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;
using Tenda.EmpresaVenda.ApiService.Models.Loja;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public DataSourceResponse<ViewCoordenadorSupervisorHouse> ListarSupervisorHouse(FiltroHierarquiaHouseDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("hierarquiaHouse", "listarSupervisorHouse");
            var result = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ViewCoordenadorSupervisorHouse>>(result);
        }
        
        public DataSourceResponse<ViewSupervisorAgenteVendaHouse> ListarAgenteVendaHouse(FiltroHierarquiaHouseDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("hierarquiaHouse", "listarAgenteVendaHouse");
            var result = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ViewSupervisorAgenteVendaHouse>>(result);
        }
        
        public DataSourceResponse<ViewHierarquiaHouse> ListarHierarquiaHouse(FiltroHierarquiaHouseDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("hierarquiaHouse", "listarHierarquiaHouse");
            var result = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ViewHierarquiaHouse>>(result);
        }

        public BaseResponse VincularCoordenadorSupervisor(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var request = GetBaseRequest().AppendPathSegments("hierarquiaHouse", "vincularCoordenadorSupervisor");
            var result = request.PostJsonAsync(hierarquiaHouseDto).Result;
            return HandleResponse<BaseResponse>(result);
        }
        
        public BaseResponse DesvincularCoordenadorSupervisor(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var request = GetBaseRequest().AppendPathSegments("hierarquiaHouse", "desvincularCoordenadorSupervisor");
            var result = request.PostJsonAsync(hierarquiaHouseDto).Result;
            return HandleResponse<BaseResponse>(result);
        }
        
        public BaseResponse VincularSupervisorAgenteVendaHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var request = GetBaseRequest().AppendPathSegments("hierarquiaHouse", "vincularSupervisorAgenteVendaHouse");
            var result = request.PostJsonAsync(hierarquiaHouseDto).Result;
            return HandleResponse<BaseResponse>(result);
        }
        
        public BaseResponse DesvincularSupervisorAgenteVendaHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var request = GetBaseRequest().AppendPathSegments("hierarquiaHouse", "desvincularSupervisorAgenteVendaHouse");
            var result = request.PostJsonAsync(hierarquiaHouseDto).Result;
            return HandleResponse<BaseResponse>(result);
        }
        
        public DataSourceResponse<ViewCoordenadorSupervisorHouse> AutoCompleteSupervisorHouse(FiltroHierarquiaHouseDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("hierarquiaHouse", "autoCompleteSupervisorHouse");
            var result = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ViewCoordenadorSupervisorHouse>>(result);
        }

        public DataSourceResponse<ViewSupervisorAgenteVendaHouse> AutoCompleteAgenteVendaHouse(FiltroHierarquiaHouseDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("hierarquiaHouse", "autoCompleteAgenteVendaHouse");
            var result = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ViewSupervisorAgenteVendaHouse>>(result);
        }

        public DataSourceResponse<ViewSupervisorAgenteVendaHouse> AutoCompleteHouse(FiltroHierarquiaHouseDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("hierarquiaHouse", "autoCompleteHouse");
            var result = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ViewSupervisorAgenteVendaHouse>>(result);
        }

        public DataSourceResponse<ViewLojasPortal> AutoCompleteLojaPortal(FiltroLojaPortalDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("hierarquiaHouse", "autoCompleteLojaPortal");
            var result = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ViewLojasPortal>>(result);
        }

        public DataSourceResponse<ViewHierarquiaHouse> AutoCompleteAgenteVendaHouseConsulta(FiltroHierarquiaHouseDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("hierarquiaHouse", "autoCompleteAgenteVendaHouseConsulta");
            var result = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ViewHierarquiaHouse>>(result);
        }
        
        public DataSourceResponse<ViewCoordenadorHouse> AutoCompleteCoordenadorHouse(FiltroHierarquiaHouseDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("hierarquiaHouse", "autoCompleteCoordenadorHouse");
            var result = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ViewCoordenadorHouse>>(result);
        }

        public BaseResponse TrocarHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var request = GetBaseRequest().AppendPathSegments("hierarquiaHouse", "trocarHouse");
            var result = request.PutJsonAsync(hierarquiaHouseDto).Result;
            return HandleResponse<BaseResponse>(result);
        }
    }
}
