using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.EnderecoEmpreendimento;
using System.Net;
using Europa.Extensions;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/enderecosEmpreendimentos")]
    public class EnderecoEmpreendimentoController : BaseApiController
    {
        private EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository { get; set; }

        [HttpGet]
        [Route("buscar/{id}")]
        [AuthenticateUserByToken("EVS10", "Visualizar")]
        public HttpResponseMessage Buscar(long id)
        {
            if (id.IsEmpty()) return Response(HttpStatusCode.NotFound);
            var dataSource = _enderecoEmpreendimentoRepository.FindByEmpreendimento(id);
            var viewModels = MontarDto(dataSource);
            return Response(viewModels);
        }

        private EnderecoEmpreendimentoDTO MontarDto(EnderecoEmpreendimento model)
        {
            var dto = new EnderecoEmpreendimentoDTO();
            dto.FromDomain(model);
            return dto;
        }
    }
}