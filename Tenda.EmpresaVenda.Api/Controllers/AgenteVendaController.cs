using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.AgenteVenda;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/agentesVenda")]
    public class AgenteVendaController : BaseApiController
    {
        private ViewLojaPortalUsuarioRepository _viewLojaPortalUsuarioRepository{ get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private AgenteVendaService _agenteVendaService { get; set; }

        [HttpPost]
        [Route("listar")]
        [AuthenticateUserByToken]
        public HttpResponseMessage Listar(FiltroAgenteVendaDto filtro)
        {
            var dataSource = _viewLojaPortalUsuarioRepository.Listar(filtro);
            var viewModels = dataSource.records.Select(MontarDto).AsQueryable();
            var response = dataSource.CloneDataSourceResponse(viewModels.ToList());
            return Response(response);
        }
        
        [HttpPost]
        [Route("autocomplete")]
        [AuthenticateUserByToken]
        public HttpResponseMessage Autocomplete(DataSourceRequest request)
        {
            var dataSource = _corretorRepository.ListarPorTipoCorretor(request, TipoCorretor.AgenteVenda);
            var viewModels = dataSource.records.Select(x => new EntityDto(x.Id, x.Nome)).AsQueryable();
            var response = dataSource.CloneDataSourceResponse(viewModels.ToList());
            return Response(response);
        }

        private AgenteVendaDto MontarDto(ViewLojaPortalUsuario model)
        {
            var dto = new AgenteVendaDto();
            dto.FromDomain(model);
            return dto;
        }

        [HttpPost]
        [Route("")]
        [AuthenticateUserByToken("CAD16", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage AtribuirUsuarioLoja(AgenteVendaDto model)
        {
            var result = _agenteVendaService.AtribuirUsuarioLoja(model);
            var response = new BaseResponse();
            response.SuccessResponse(string.Format(GlobalMessages.RegistroSalvo, model.NomeUsuario,
                GlobalMessages.Salvo.ToLower()));
            return Response(response);
        }
    }
}