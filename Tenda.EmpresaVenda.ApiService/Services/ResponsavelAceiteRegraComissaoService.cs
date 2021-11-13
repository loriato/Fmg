using Europa.Extensions;
using Europa.Web;
using Flurl.Http;
using Tenda.EmpresaVenda.ApiService.Models.RegraComissao;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public BaseResponse ListarResponsavelAceiteRegraComissao(RegraComissaoDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("regraComissao", "listar");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }
        public BaseResponse IncluirResponsavelAceiteRegraComissao(RegraComissaoDto model)
        {
            var request = GetBaseRequest().AppendPathSegment("regraComissao");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse SuspenderResponsavelAceiteRegraComissao(RegraComissaoDto model)
            {
            var request = GetBaseRequest().AppendPathSegments("regraComissao", "suspender");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }
    }
}
