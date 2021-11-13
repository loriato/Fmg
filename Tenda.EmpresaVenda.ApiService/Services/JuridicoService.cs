using Europa.Web;
using Flurl.Http;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public BaseResponse UploadDocumentoJuridico(FileDto fileDto)
        {
            var request = GetBaseRequest().AppendPathSegments("juridico", "uploadDocumentoJuridico");
            var result = request.PostJsonAsync(fileDto).Result;
            return HandleResponse<BaseResponse>(result);
        }
    }
}
