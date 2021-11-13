using Europa.Commons;
using Europa.Web;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.Domain.Core.Services;
using Tenda.EmpresaVenda.Api.Security;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/cep")]
    public class CepController : BaseApiController
    {
        [HttpGet]
        [Route("{cep}")]
        [AuthenticateUserByToken]
        public HttpResponseMessage ConsultaCEP(string cep)
        {
            var response = new BaseResponse();
            try
            {
                var cepDTO = CepService.ConsultaCEPWS(cep);
                response.Data = cepDTO == null ? new Tenda.Domain.Core.Services.Models.CepDTO() : cepDTO;
                response.Code = (int)HttpStatusCode.OK;

            }
            catch (Exception e)
            {
                if (e is BusinessRuleException)
                {
                    var bre = (e as BusinessRuleException);
                    response.Success = false;
                    response.Messages.AddRange(bre.Errors);
                    response.Data = bre.Errors.First();
                }
                else
                {
                    response.Success = false;
                    response.Messages.Add("Ocorreu um erro ao consultar o cep!");
                    response.Data = e.Message;
                }
            }
            return Response(response);
        }
    }
}