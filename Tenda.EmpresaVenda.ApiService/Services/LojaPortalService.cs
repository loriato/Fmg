using Europa.Extensions;
using Europa.Web;
using Flurl.Http;
using System;
using Tenda.EmpresaVenda.ApiService.Models.Loja;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public DataSourceResponse<LojaPortalDto> ListarLojasPortal(FiltroLojaPortalDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("lojasPortal", "listar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<LojaPortalDto>>(response);
        }

        public LojaPortalDto BuscarLojaPortal(long id)
        {
            var request = GetBaseRequest().AppendPathSegment("lojasPortal").AppendPathSegment(id);
            var response = request.GetAsync().Result;
            return HandleResponse<LojaPortalDto>(response);
        }

        public BaseResponse IncluirLojaPortal(LojaPortalDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("lojasPortal", "incluir");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse EditarLojaPortal(LojaPortalDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("lojasPortal", "alterar");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse ExcluirLojaPortal(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("lojasPortal", "excluir", id);
            var response = request.PostAsync().Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse ReativarLoja(long[] ids)
        {
            var request = GetBaseRequest().AppendPathSegment("lojasPortal").AppendPathSegment("reativar");
            var response = request.PutJsonAsync(ids).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse SuspenderLoja(long[] ids)
        {
            var request = GetBaseRequest().AppendPathSegment("lojasPortal").AppendPathSegment("suspender");
            var response = request.PutJsonAsync(ids).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse CancelarLoja(long[] ids)
        {
            var request = GetBaseRequest().AppendPathSegment("lojasPortal").AppendPathSegment("cancelar");
            var response = request.PutJsonAsync(ids).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public FileDto ExportarPaginaLojasPortal(FiltroLojaPortalDto filtro)
        {
            var request = GetBaseRequest().WithTimeout(TimeSpan.FromMinutes(10)).AppendPathSegment("lojasPortal")
                .AppendPathSegment("exportarPagina");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<FileDto>(response);
        }

        public FileDto ExportarTodosLojasPortal(FiltroLojaPortalDto filtro)
        {
            var request = GetBaseRequest().WithTimeout(TimeSpan.FromMinutes(10)).AppendPathSegment("lojasPortal")
                .AppendPathSegment("exportarTodos");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<FileDto>(response);
        }

        public DataSourceResponse<EntityDto> ListarLojasAutocomplete(DataSourceRequest dataSourceRequest)
        {
            var request = GetBaseRequest().AppendPathSegments("lojasPortal", "autocomplete");
            var response = request.PostJsonAsync(dataSourceRequest).Result;
            return HandleResponse<DataSourceResponse<EntityDto>>(response);
        }
    }
}
