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
using Tenda.EmpresaVenda.Domain.Integration.Zeus.Midas.Models;

namespace Tenda.EmpresaVenda.Domain.Integration.Zeus.Midas
{
    public class MidasApiService
    {
        private readonly IFlurlClient _flurlClient;

        public MidasApiService(IFlurlClientFactory flurlClientFac)
        {
            var baseUrl = ProjectProperties.MidasUrlZeusServiceEvsApi;
            _flurlClient = flurlClientFac.Get(baseUrl);

        }

        private IFlurlRequest GetBaseRequest()
        {
            var token = ProjectProperties.MidasTokenZeusServiceEvsApi;

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
                case HttpStatusCode.Unauthorized: //token inválido
                    var unauthorizedResponse =
                        Task.FromResult(responseMessage).ReceiveJson<T>().Result;
                    throw new UnauthorizedAccessException("Acesso não autorizado");
                case HttpStatusCode.BadRequest:
                    var errorResponse = Task.FromResult(responseMessage).ReceiveJson<T>().Result;
                    throw new ApiException("Falha na aprovação da ocorrência");

                case HttpStatusCode.Forbidden: //sem permissão para ufs
                    var forbiddenResponse = Task.FromResult(responseMessage).ReceiveJson<BaseResponse>().Result;
                    var data = JsonConvert.DeserializeObject<SemPermissaoDto>(
                        JsonConvert.SerializeObject(forbiddenResponse.Data));
                    throw new UnauthorizedPermissionException(forbiddenResponse.Messages.FirstOrDefault(),
                        data.UnidadeFuncional, data.Funcionalidade);

                case HttpStatusCode.NotFound: //Not found
                    var notFoundResponse =
                        Task.FromResult(responseMessage).ReceiveJson<T>().Result;
                    throw new ApiException("Endpoint não encontrado");

                case HttpStatusCode.InternalServerError:
                    var internalServerErrorResponse =
                        Task.FromResult(responseMessage).ReceiveJson<T>().Result;
                    throw new ApiException("Erro interno");
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

        public MidasApiResponseDto<AprovarIntegracaoResponseDto> AprovarIntegracaoOcorrencia(List<AprovarIntegracaoRequestDto> ocorrencias)
        {
            var request = GetBaseRequest().AppendPathSegments("send", "approval");
            var response = request.PostJsonAsync(ocorrencias).Result;
            return HandleResponse<MidasApiResponseDto<AprovarIntegracaoResponseDto>>(response);
        }
    }
}
