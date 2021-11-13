using Europa.Extensions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/arquivosEmpreendimentos")]
    public class ArquivoEmpreendimentoController : BaseApiController

    {
        private ViewArquivoBreveLancamentoRepository _viewArquivoEmpreendimentoRepository { get; set; }

        [HttpGet]
        [Route("listar/{idEmpreendimento}")]
        [AuthenticateUserByToken("EVS07", "Visualizar")]
        public HttpResponseMessage Listar(long idEmpreendimento)
        {
            if (idEmpreendimento.IsEmpty()) return Response(HttpStatusCode.NotFound);
            var model = _viewArquivoEmpreendimentoRepository.Listar(idEmpreendimento);
            return Response(model);
        }

    }
}

