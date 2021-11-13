using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Planta3D;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/plantasTresD")]
    public class PlantaTresDController : BaseApiController
    {
        [HttpGet]
        [Route("links")]
        [AuthenticateUserByToken("EVS07", "Visualizar")]
        public HttpResponseMessage Links()
        {
            var links = new PlantaTresDLinksDto
            {
                Planta3DLink4 = Tenda.Domain.Shared.ProjectProperties.Planta3DLink4,
                Planta3DLink7 = Tenda.Domain.Shared.ProjectProperties.Planta3DLink7
            };
            return Response(links);
        }
    }
}