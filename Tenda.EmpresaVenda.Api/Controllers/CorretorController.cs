using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Api.Controllers
{

    [RoutePrefix("api/corretor")]
    public class CorretorController : BaseApiController
    {
        private CorretorRepository _corretorRepository { get; set; }

        [HttpGet]
        [AuthenticateUserByToken]
        [Route("")]
        public HttpResponseMessage Corretores(DateTime? lastUpdate)
        {
            var queryable = _corretorRepository.Listar()
                .Where(reg => reg.Usuario != null)
                .Where(reg => reg.CPF != null);

            if (lastUpdate.HasValue)
            {
                queryable = queryable.Where(reg => reg.AtualizadoEm >= lastUpdate);
            }

            var corretores = queryable
                .Select(reg => new
                {
                    Nome = reg.Nome,
                    Login = reg.Usuario.Login,
                    Situacao = reg.Usuario.Situacao,
                    Cpf = reg.CPF
                })
                .ToList();

            return Request.CreateResponse(HttpStatusCode.OK, corretores);
        }

    }
}
