using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Lead;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/leads")]
    public class LeadController : BaseApiController
    {
        private LeadService _leadService { get; set; }
        
        [HttpPost]
        [Route("incluir")]
        [AuthenticateUserByToken("GEC05", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage IncluirLeads(LeadRequestDto leads)
        {
            if (!leads.HasValue()) return Response(HttpStatusCode.NotFound);
            
            var origem = RequestState.UsuarioPortal.Id;
            var mensagens = _leadService.IncluirLeads(origem,leads);
            var response = new BaseResponse();
            response.SuccessResponse(mensagens);
            return Response(response);
        }

    }
}