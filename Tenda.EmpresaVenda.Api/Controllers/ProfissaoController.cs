using Europa.Extensions;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/profissoes")]
    public class ProfissaoController : BaseApiController
    {
        private ProfissaoRepository _profissaoRepository { get; set; }

        [HttpPost]
        [Route("autocomplete")]
        [AuthenticateUserByToken]
        public HttpResponseMessage Autocomplete(DataSourceRequest request)
        {
            var dataSource = _profissaoRepository.ListarProfissoes(request);
            var viewModels = dataSource.records.Select(x => new EntityDto(x.Id, x.Nome)).AsQueryable();
            var response = dataSource.CloneDataSourceResponse(viewModels.ToList());
            return Response(response);
        }
    }
}