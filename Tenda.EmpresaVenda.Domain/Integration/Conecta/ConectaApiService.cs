using Europa.Extensions;
using Europa.Rest;
using Europa.Web;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tenda.Domain.Security.Services.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Integration.Conecta.Models;

namespace Tenda.EmpresaVenda.Domain.Integration.Conecta
{
    public class ConectaApiService
    {
        private readonly IFlurlClient _flurlClient;

        public ConectaApiService(IFlurlClientFactory flurlClientFac)
        {
            var baseUrl = ProjectProperties.ConectaUrlBaseApi;

            _flurlClient = flurlClientFac.Get(baseUrl);

        }

        private IFlurlRequest GetBaseRequest()
        {
            var token = ProjectProperties.ConectaApiToken;


            var flur = _flurlClient.Request().AllowAnyHttpStatus();
            
            flur.Headers.Add("Authorization", token);

            return flur;
        }

        private T HandleResponse<T>(IFlurlResponse responseMessage) where T : new()
        {
            switch (responseMessage.ResponseMessage.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Task.FromResult(responseMessage).ReceiveJson<T>().Result;

                case HttpStatusCode.BadRequest:
                    var errorResponse = Task.FromResult(responseMessage).ReceiveJson<BaseResponse>().Result;
                    throw new ApiException(errorResponse);

                case HttpStatusCode.Unauthorized: //token inválido
                    var unauthorizedResponse = Task.FromResult(responseMessage).ReceiveJson<BaseResponse>().Result;
                    throw new UnauthorizedAccessException(unauthorizedResponse.Messages.FirstOrDefault());
                                   
                case HttpStatusCode.NotFound: //Not found
                    throw new NotFoundException();
                
                default: //outros erros
                    HandleError(responseMessage);
                    return default;
            }
        }

        private void HandleError(IFlurlResponse responseMessage)
        {
            var response = responseMessage.ResponseMessage.Content.ReadAsStringAsync().Result;
            try
            {
                var errorResponse = JsonConvert.DeserializeObject<ExceptionDto>(response);
                throw new InternalServerException(errorResponse);
            }
            catch (FlurlParsingException)
            {
                throw new Exception(response);
            }
        }

        public GerarTokenAcessoConectaResponseDto GerarTokenAcesso(LoginDto loginDto)
        {
            var request = GetBaseRequest().AppendPathSegment("api").AppendPathSegment("integracao")
                .AppendPathSegment("gerar-token-acesso");

            var response = request.PostJsonAsync(loginDto).Result;

            return HandleResponse<GerarTokenAcessoConectaResponseDto>(response);
        }

        public string BuscarUrlKanban()
        {
            var request = GetBaseRequest().AppendPathSegment("Conecta")
                .AppendPathSegment("buscarUrlKanban");

            var response = request.GetJsonAsync().Result;

            return HandleResponse<GerarTokenAcessoConectaResponseDto>(response);
        }

        public BaseResponse CriarLeadConecta(LeadConectaDto leadConectaDto)
        {
            var request = GetBaseRequest()
                .AppendPathSegment("api")
                .AppendPathSegment("leads-feirao");

            var response = request.PostJsonAsync(leadConectaDto).Result;

            return HandleResponse<BaseResponse>(response);
        }

        public LeadConectaResponseDto BuscarLeadConecta(FiltroLeadConectaDto filtro)
        {
            var request = GetBaseRequest()
                .AppendPathSegment("api")
                .AppendPathSegment("empresa-venda")
                .AppendPathSegment("lead")
                .AppendPathSegment(filtro.Telefone);

            var response = request.GetAsync().Result;

            return HandleResponse<LeadConectaResponseDto>(response);
        }

        public List<LeadConectaResponseDto> ListarLeadConectaNomeCompleto(FiltroLeadConectaDto filtro)
        {
            var request = GetBaseRequest()
                .AppendPathSegment("api")
                .AppendPathSegment("empresa-venda")
                .AppendPathSegment("lead")
                .AppendPathSegment("list");

            var response = request.PostJsonAsync(filtro).Result;

            return HandleResponse<List<LeadConectaResponseDto>>(response);
        }
        public BaseResponse AtualizarAtributosLead(string uuid,AtributosDinamicosLeadDto atributos)
        {
            var request = GetBaseRequest().AppendPathSegment("api")
                .AppendPathSegment("leads-feirao")
                .AppendPathSegment(uuid);

            var response = request.PutJsonAsync(atributos).Result;

            return HandleResponse<BaseResponse>(response);
        }
    }
}
