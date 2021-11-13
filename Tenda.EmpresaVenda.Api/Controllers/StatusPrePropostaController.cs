using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.AgenteVenda;
using Tenda.EmpresaVenda.ApiService.Models.StatusPreProposta;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/StatusPreProposta")]
    public class StatusPrePropostaController : BaseApiController
    {
        private StatusPrePropostaService _statusPrePropostaService { get; set; }

        [Route("listar")]
        public HttpResponseMessage Listar(StatusPrePropostaFiltro filtro)
        {
            var dataSource = _statusPrePropostaService.Listar(filtro);
            var response = dataSource.CloneDataSourceResponse(dataSource.records.ToList());
            return Response(response);
        }
        [HttpPost]
        [Route("Alterar")]
        [AuthenticateUserByToken("CAD17", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Salvar(StatusPrePropostaDto status)
        {
            var response = new BaseResponse();
            try
            {
                var result = new StatusPrePropostaDto();
                status = _statusPrePropostaService.Salvar(status);
                response.Data = result;
                response.SuccessResponse(string.Format(GlobalMessages.RegistroSalvo, status.StatusPadrao,
                    GlobalMessages.Incluido.ToLower()));
            }
            catch (ApiException apiEx)
            {
                response = apiEx.GetResponse();
                CurrentTransaction().Rollback();
            }
            return Response(response);
        }
        [AuthenticateUserByToken("CAD17", "Visualizar")]
        public HttpResponseMessage Buscar(long idTranslateStatusPreProposta)
        {
            return Response(_statusPrePropostaService.BuscarId(idTranslateStatusPreProposta));
        }
        [AuthenticateUserByToken("CAD17", "Visualizar")]
        public HttpResponseMessage getTraducao(string statusPadrao)
        {
            return Response(_statusPrePropostaService.getTraducaoPortalHouse(statusPadrao));
        }
    }
}