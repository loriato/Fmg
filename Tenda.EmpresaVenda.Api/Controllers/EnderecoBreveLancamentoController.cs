using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.EnderecoBreveLancamento;
using Europa.Extensions;
using System.Net;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/enderecosBrevesLancamentos")]
    public class EnderecoBreveLancamentoController : BaseApiController
    {
        private EnderecoBreveLancamentoRepository _enderecoBreveLancamentoRepository { get; set; }
        private BreveLancamentoService _breveLancamentoService { get; set; }
        private CorretorRepository _corretorRepository { get; set; }

        [HttpPost]
        [Route("enderecosDeBrevesLancamentos")]
        [AuthenticateUserByToken("EVS10", "Visualizar")]
        public HttpResponseMessage EnderecosDeBrevesLancamentos(FilterDto filtro)
        {
            var empresaVenda = _corretorRepository.FindByIdUsuario(RequestState.UsuarioPortal.Id).EmpresaVenda;
            var brevesLancamentos = _breveLancamentoService.DisponiveisParaCatalogoNoEstado(empresaVenda);

            var dataSource = _enderecoBreveLancamentoRepository.EnderecosDeBrevesLancamentosQueryable(
                    brevesLancamentos.Select(x => x.Id).ToList(), empresaVenda);
            var viewModels = dataSource.Select(MontarDto).AsQueryable();
            return Response(viewModels.ToList());
        }

        [HttpGet]
        [Route("buscar/{id}")]
        [AuthenticateUserByToken("EVS10", "Visualizar")]
        public HttpResponseMessage Buscar(long id)
        {
            if (id.IsEmpty()) return Response(HttpStatusCode.NotFound);
            var dataSource = _enderecoBreveLancamentoRepository.FindByBreveLancamento(id);
            var viewModels = MontarDto(dataSource);
            return Response(viewModels);
        }
        private EnderecoBreveLancamentoDTO MontarDto(EnderecoBreveLancamento model)
        {
            var dto = new EnderecoBreveLancamentoDTO();
            dto.FromDomain(model);
            return dto;
        }
    }
}