using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/indique")]
    public class IndiqueController : BaseApiController
    {
        private IndiqueService _indiqueService { get; set; }

        [HttpGet]
        [Route("buscarPrePropostaPorCpfCliente")]
        public HttpResponseMessage BuscarPrePropostaPorCpfCliente(string cpf)
        {

            var proprostaDto = _indiqueService.BuscarPrePropostaPorCpfCliente(cpf);

            if (proprostaDto == null)
                return Response(HttpStatusCode.NotFound);

            return Response(proprostaDto);
        }

        [HttpGet]
        [Route("buscarPrePropostasClientesPorCpf")]
        public HttpResponseMessage BuscarPrePropostasClientesPorCpf([FromUri] string[] cpfs)
        {

            var proprostaDto = _indiqueService.BuscarPrePropostasClientesPorCpf(cpfs);

            if (proprostaDto == null)
                return Response(HttpStatusCode.NotFound);

            return Response(proprostaDto);
        }

        [HttpGet]
        [Route("buscarCarteiraHouse")]
        public HttpResponseMessage BuscarCarteiraHouse()
        {
            var carteira = _indiqueService.BuscarCarteiraHouse();
            return Response(carteira);
        }
        [HttpGet]
        [Route("buscarCarteiraEv")]
        public HttpResponseMessage BuscarCarteiraEv()
        {
            var carteira = _indiqueService.BuscarCarteiraEv();
            return Response(carteira);
        }
    }
}
