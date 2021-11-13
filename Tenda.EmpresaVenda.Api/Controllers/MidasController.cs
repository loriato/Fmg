using Europa.Resources;
using Europa.Web;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Midas;
using Tenda.EmpresaVenda.Domain.Services;
using Europa.Commons;
using Europa.Rest;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/midas")]
    public class MidasController : BaseApiController
    {
        private MidasService _midasService { get; set; }


        /**
         * O registra a ocorrência para posterior analise
         */
        [HttpPost]
        [Route("registrarOcorrencia")]
        [AuthenticateUserByToken]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage RegistrarOcorrencia(OcorrenciaRequestDto requestDto)
        {
            var data = new MidasResponseDto();
            var response = new BaseResponse();

            try
            {
                var msg = _midasService.RegistrarOcorrencia(requestDto);
                data.Success = true;
                data.Message = GlobalMessages.OcorrenciaRegistrada;
            }
            catch (ApiException apiException)
            {
                data.Success = false;
                foreach(var message in apiException.GetResponse().Messages)
                {
                    data.Message += message;
                }
            }

            response.SuccessResponse(data);
            return Response(response);
        }

        /**
         * O campo Data retorna o valor do Status solicitado no documento
         * O campo Status, poderá devolver os valores: Comissioned, NotComissioned ou 
            None.
         */
        [HttpGet]
        [Route("autorizarPagamento/{occurrenceId}")]
        [AuthenticateUserByToken]
        public HttpResponseMessage AutorizarPagamento(long occurrenceId)
        {
            var response = new BaseResponse();
            var data = new AutorizarPagamentoResponseDto();

            try
            {
                string[] res = _midasService.AutorizarPagamento(occurrenceId);
                data.Success = true;
                if (res[0] == "")
                {
                    data.Success = false;
                }
                data.Status = res[0];
                data.Message = res[1];

            }
            catch (ApiException apiException)
            {
                data.Success = false;
                data.Status = "";
                data.Message = "Infelizmente não foi possível consultar os dados. Tente novamente mais tarde.";
                
            }
            response.SuccessResponse(data);
            return Response(response);
        }

        /**
         * Registra a data de previsão do pagamento
         */
        [HttpPost]
        [Route("provisionarPagamento")]
        [AuthenticateUserByToken]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage ProvisionarPagamento(ProvisionarPagamentoRequestDto requestDto)
        {
            var response = new BaseResponse();
            var data = new MidasResponseDto();

            try
            {
                _midasService.ProvisionarPagamento(requestDto);
                data.Success = true;
                data.Message = string.Format("Previsão de pagamento {0}", requestDto.Date);
            }
            catch (ApiException apiException)
            {
                data.Success = false;
                foreach (var message in apiException.GetResponse().Messages)
                {
                    data.Message += message;
                }
            }

            response.SuccessResponse(data);

            return Response(response);
        }

    }
}