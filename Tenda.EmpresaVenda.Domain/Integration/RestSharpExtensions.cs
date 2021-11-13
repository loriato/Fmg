using Europa.Commons;
using Europa.Extensions;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using Tenda.Domain.Shared.Log;

namespace Tenda.EmpresaVenda.Domain.Integration
{
    public static class RestSharpExtensions
    {
        public static string GetBody(this IRestRequest request)
        {
            var bodyParameter = request.Parameters
                                       .FirstOrDefault(p => p.Type == ParameterType.RequestBody);
            return bodyParameter == null
                       ? null
                       : bodyParameter.Value.ToString();
        }

        /// <summary>
        /// Lança Exceção caso encontre algum erro na resposta ou caso o statusCode não seja Ok.
        /// </summary>
        /// <param name="responseBody"></param>
        public static void Validate(this IRestResponse responseBody)
        {
            if (responseBody.StatusCode == HttpStatusCode.OK)
            {
                return;
            }
            if (responseBody.StatusCode == HttpStatusCode.Unauthorized)
            {
                var errorList = JsonConvert.DeserializeObject<ErrorList>(responseBody.Content);
                var bre = new BusinessRuleException(errorList.Errors.First().Message);
                foreach (var message in errorList.Errors)
                {
                    bre.AddError(message.Message).Complete();
                }
                ExceptionLogger.LogException(bre);
                bre.ThrowIfHasError();
            }
            if (responseBody.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new Exception("Serviço indisponível");
            }
            try
            {
                // Se entender que é um erro esperado do SUAT, então vai subir uma BusinessRule
                ValidateSuatDefaultErrorDto(responseBody);

                if (responseBody.ErrorException != null && responseBody.Content.IsEmpty())
                {
                    ExceptionLogger.LogException(responseBody.ErrorException);
                    throw new Exception("Não foi possível conectar ao servidor ou o servidor não retornou uma resposta.");
                }
                // Caso contrário, tem que subir a exceção com mais detalghes
                throw new Exception(responseBody.Content);
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }
        }

        private static void ValidateSuatDefaultErrorDto(IRestResponse responseBody)
        {
            MensagemRetornoDTO errorList;
            try
            {
                errorList = JsonConvert.DeserializeObject<MensagemRetornoDTO>(responseBody.Content);
            }
            catch (Exception e)
            {
                // Não é um erro experado
                ExceptionLogger.LogException(e);
                return;
            }

            if (errorList == null)
            {
                // Não é um erro experado
                return;
            }
            var bre = new BusinessRuleException();
            foreach (var error in errorList.Mensagens)
            {
                bre.AddError(error).Complete();
            }
            foreach (var field in errorList.Campos)
            {
                bre.AddField(field);
            }
            // Está tentando converter, mas é um erro em formato diferente do MensagemRetornoDTO
            // FIXME: Redesenhar todo RestSharp, não está legal
            if (bre.Errors.IsEmpty() && bre.ErrorsFields.IsEmpty())
            {
                return;
            }
            throw bre;
        }
    }
}