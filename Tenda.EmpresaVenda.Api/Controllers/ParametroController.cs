using System.Net.Http;
using System.Web.Http;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Api.Security;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/parametro")]
    public class ParametroController : BaseApiController
    {
        [HttpGet]
        [Route("suatBaseUrl")]
        [AuthenticateUserByToken]
        public HttpResponseMessage SuatBaseUrl()
        {
            var suatBaseUrl = ProjectProperties.SuatBaseUrl;
            return Response(suatBaseUrl);
        }
    }
}