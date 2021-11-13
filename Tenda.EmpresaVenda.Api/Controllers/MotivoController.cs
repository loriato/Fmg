using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Motivo;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/motivos")]
    public class MotivoController : BaseApiController
    {
        private MotivoRepository _motivoRepository { get; set; }

        [HttpPost]
        [Route("listar")]
        [AuthenticateUserByToken(true)]
        public HttpResponseMessage Listar(FiltroMotivoDto filtro)
        {
            var list = _motivoRepository.Listar(filtro);
            var dtos = list.Select(x => new EntityDto(x.Id, x.Descricao)).ToList();

            return Response(dtos);
        }
    }
}