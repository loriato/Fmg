using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/empresaVenda")]
    public class EmpresaVendaController : BaseApiController
    {
        private EmpresaVendaService _empresaVendaService { get; set; }

        [HttpGet]
        [Route("listarEmpresasVendasAtivas")]
        [AuthenticateUserByToken]
        public HttpResponseMessage ListarEmpresasVendasAtivas()
        {
            var list = _empresaVendaService.ListarEVSAtivas();
            return Response(list);
        }
    }
}