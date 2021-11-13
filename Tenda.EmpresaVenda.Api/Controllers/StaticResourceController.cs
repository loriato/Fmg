using System;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Arquivo;
using Tenda.EmpresaVenda.ApiService.Models.StaticResource;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/staticResources")]
    public class StaticResourceController : BaseApiController
    {
        private StaticResourceService _staticResourceService { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }


        [HttpGet]
        [Route("{id}")]
        [AuthenticateUserByToken("EVS10", "Visualizar")]
        public HttpResponseMessage LoadResourceGet(long id)
        {
            var dataSource = _staticResourceService.LoadResource(id);
            StaticResourceDTO staticResource = new StaticResourceDTO();
            staticResource.Id = id;
            staticResource.FileName = dataSource;
            staticResource.Url = _staticResourceService.CreateUrlApi(staticResource.FileName);
            staticResource.UrlThumbnail = _staticResourceService.CreateThumbnailUrlApi(staticResource.FileName);
            return Response(staticResource);
        }

        [HttpPost]
        [Route("loadResource")]
        [AuthenticateUserByToken("EVS10", "Visualizar")]
        [Obsolete("Substituir pelo GET")]
        public HttpResponseMessage LoadResource(FiltroStaticResourceDTO filtro)
        {
            var dataSource = _staticResourceService.LoadResource(filtro.Id);
            StaticResourceDTO staticResource = new StaticResourceDTO();
            staticResource.Id = filtro.Id;
            staticResource.FileName = dataSource;
            staticResource.Url = _staticResourceService.CreateUrlApi(staticResource.FileName);
            staticResource.UrlThumbnail = _staticResourceService.CreateThumbnailUrlApi(staticResource.FileName);
            return Response(staticResource);
        }

        [HttpPost]
        [Route("withNoContentAndNoThumbnail")]
        [AuthenticateUserByToken("EVS10", "Visualizar")]
        public HttpResponseMessage WithNoContentAndNoThumbnail(FiltroStaticResourceDTO filtro)
        {
            ArquivoDto dataSource = new ArquivoDto().FromDomain(_arquivoRepository.WithNoContentAndNoThumbnail(filtro.Id));

            return Response(dataSource);
        }
    }
}