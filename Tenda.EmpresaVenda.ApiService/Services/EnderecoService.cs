using Europa.Web;
using Flurl.Http;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {

        public BaseResponse ConsultarEnderecoPorCep(string cep)
        {
            var request = GetBaseRequest().AppendPathSegments("cep", cep);
            var response = request.GetAsync().Result;
            return HandleResponse<BaseResponse>(response);
        }

    }
}
