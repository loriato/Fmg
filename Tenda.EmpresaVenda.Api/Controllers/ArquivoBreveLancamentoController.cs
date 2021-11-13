using Europa.Extensions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/arquivosBrevesLancamentos")]
    public class ArquivoBreveLancamentoController : BaseApiController

    {
        private ViewArquivoBreveLancamentoRepository _viewArquivoBreveLancamentoRepository { get; set; }

        [HttpGet]
        [Route("listar/{idBreveLancamento}")]
        [AuthenticateUserByToken("EVS07", "Visualizar")]
        public HttpResponseMessage Listar(long idBreveLancamento)
        {
            if (idBreveLancamento.IsEmpty()) return Response(HttpStatusCode.NotFound);
            var model = _viewArquivoBreveLancamentoRepository.Listar(idBreveLancamento);
            return Response(model);
        }

    }
}

