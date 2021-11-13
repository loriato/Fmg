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
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.ApiService.Models.Funcionalidade;
using Tenda.EmpresaVenda.ApiService.Models.Simulador;

namespace Tenda.EmpresaVenda.Domain.Integration.Simulador
{
    public class SimuladorApiService
    {
        private readonly IFlurlClient _flurlClient;

        public SimuladorApiService(IFlurlClientFactory flurlClientFac)
        {
            var baseUrl = ProjectProperties.SimuladorBaseUrlApi;
            _flurlClient = flurlClientFac.Get(baseUrl);
        }

        private IFlurlRequest GetBaseRequest()
        {
            return _flurlClient.Request().WithAuthorization(ProjectProperties.SimuladorApiToken).AllowAnyHttpStatus();
        }

        private T HandleResponse<T>(IFlurlResponse responseMessage) where T : new()
        {
            switch (responseMessage.ResponseMessage.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Task.FromResult(responseMessage).ReceiveJson<T>().Result;
                case HttpStatusCode.Forbidden: //sem permissão para ufs
                    var forbiddenResponse = Task.FromResult(responseMessage).ReceiveJson<BaseResponse>().Result;
                    var data = JsonConvert.DeserializeObject<SemPermissaoDto>(
                        JsonConvert.SerializeObject(forbiddenResponse.Data));
                    throw new UnauthorizedPermissionException(forbiddenResponse.Messages.FirstOrDefault(),
                        data.UnidadeFuncional, data.Funcionalidade);
                case HttpStatusCode.Unauthorized: //token inválido
                    var unauthorizedResponse = Task.FromResult(responseMessage).ReceiveJson<BaseResponse>().Result;
                    throw new UnauthorizedAccessException(unauthorizedResponse.Messages.FirstOrDefault());
                case HttpStatusCode.NotFound: //Not found
                    var notFoundResponse = Task.FromResult(responseMessage).ReceiveJson<BaseResponse>().Result;
                    throw new ApiException(notFoundResponse);
                case HttpStatusCode.BadRequest:
                    var errorResponse = Task.FromResult(responseMessage).ReceiveJson<BaseResponse>().Result;
                    throw new ApiException(errorResponse);
                case HttpStatusCode.InternalServerError:
                    var internalServerErrorResponse =
                        Task.FromResult(responseMessage).ReceiveJson<BaseResponse>().Result;
                    throw new ApiException(internalServerErrorResponse);
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

        public BaseResponse GerarTokenAcesso(GerarTokenAcessoSimuladorRequestDto requestBody)
        {
            var request = GetBaseRequest().AppendPathSegments("integracao", "gerarTokenAcesso");
            var response = request.PostJsonAsync(requestBody).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public SimuladorDto BuscarSimulacaoPorCodigo(SimuladorDto parametro)
        {
            var request = GetBaseRequest().AppendPathSegments("simulador", "buscarSimulacaoPorCodigo");
            var response = request.PostJsonAsync(parametro).Result;
            return HandleResponse<SimuladorDto>(response);
        }

        public List<SimuladorDto> BuscarSimulacaoFinalizadaPorPreProposta(string preProposta)
        {
            var request = GetBaseRequest().AppendPathSegments("simulador","buscarSimulacaoFinalizadaPorPreProposta");
            var response = request.PostJsonAsync(preProposta).Result;
            return HandleResponse<List<SimuladorDto>>(response);
        }

        public SimuladorDto BuscarSimulacaoFinalizadaEmpresaVenda(SimuladorDto parametro)
        {
            var request = GetBaseRequest().AppendPathSegments("simulador", "buscarSimulacaoFinalizadaEmpresaVenda");
            var response = request.PostJsonAsync(parametro).Result;
            return HandleResponse<SimuladorDto>(response);
        }

        public SimuladorDto BuscarSimulacaoMatrizOfertaFinalizadaEmpresaVenda(SimuladorDto parametro)
        {
            var request = GetBaseRequest().AppendPathSegments("simulador","buscarSimulacaoMatrizOfertaFinalizadaEmpresaVenda");
            var response = request.PostJsonAsync(parametro).Result;
            return HandleResponse<SimuladorDto>(response);
        }


    }
}
