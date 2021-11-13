using Europa.Extensions;
using Europa.Web;
using Flurl.Http;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.TiposDocumento;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public DataSourceResponse<ViewTiposDocumento> ListarDatatableTiposDocumento(FiltroTiposDocumentoDTO filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("tiposdocumento", "listar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ViewTiposDocumento>>(response);
        }

        public BaseResponse IncluirTipoDocumento(FiltroTiposDocumentoDTO documento)
        {
            var request = GetBaseRequest().AppendPathSegments("tiposdocumento", "salvar");
            var response = request.PostJsonAsync(documento).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse AlterarTipoDocumento(FiltroTiposDocumentoDTO documento)
        {
            var request = GetBaseRequest().AppendPathSegments("tiposdocumento", "alterar");
            var response = request.PostJsonAsync(documento).Result;
            return HandleResponse<BaseResponse>(response);
        }


        public BaseResponse ExcluirTipoDocumento(FiltroTiposDocumentoDTO documento)
        {
            var request = GetBaseRequest().AppendPathSegments("tiposdocumento", "excluir");
            var response = request.PostJsonAsync(documento).Result;
            return HandleResponse<BaseResponse>(response);
        }


        public BaseResponse ValidarExclusao(FiltroTiposDocumentoDTO documento)
        {
            var request = GetBaseRequest().AppendPathSegments("tiposdocumento", "validarexclusao");
            var response = request.PostJsonAsync(documento).Result;
            return HandleResponse<BaseResponse>(response);
        }
    }
}
