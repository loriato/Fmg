using Europa.Commons;
using Flurl.Http;
using System.Net.Http;
using Tenda.EmpresaVenda.ApiService.Models.Login;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {

        public LoginResponseDto Login(LoginRequestDto model)
        {
            GenericFileLogUtil.DevLogWithDateOnBegin("chamada o apisertvice");
            var request = GetBaseRequest().AppendPathSegment("seg").AppendPathSegment("login");
            GenericFileLogUtil.DevLogWithDateOnBegin(request.Url);
            var response = request.PostJsonAsync(model).Result;
            GenericFileLogUtil.DevLogWithDateOnBegin("response");
            var result = HandleResponse<LoginResponseDto>(response);
            return result;
        }

        public void Logout()
        {
            var request = GetBaseRequest().AppendPathSegment("seg").AppendPathSegment("logout");
            var response = request.SendAsync(HttpMethod.Post).Result;
            HandleResponse<dynamic>(response);
        }

    }
}
