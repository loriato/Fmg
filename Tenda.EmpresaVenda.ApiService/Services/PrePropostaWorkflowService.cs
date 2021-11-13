using Europa.Web;
using Flurl.Http;
using Tenda.EmpresaVenda.ApiService.Models.PrePropostaWorkflow;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public BaseResponse AguardandoFluxoAguardandoAnaliseCompletaRule(PrePropostaWorkflowDto prePropostaWorkflowDto)
        {
            var request = GetBaseRequest().AppendPathSegments("prePropostaWorkflow")
                .AppendPathSegment("aguardandoFluxoAguardandoAnaliseCompletaRule");
            var response = request.PostJsonAsync(prePropostaWorkflowDto).Result;

            return HandleResponse<BaseResponse>(response);
        }
        public BaseResponse AguardandoIntegracao(PrePropostaWorkflowDto prePropostaWorkflowDto)
        {
            var request = GetBaseRequest().AppendPathSegments("prePropostaWorkflow")
                .AppendPathSegment("aguardandoIntegracao");
            var response = request.PostJsonAsync(prePropostaWorkflowDto).Result;

            return HandleResponse<BaseResponse>(response);
        }
        public BaseResponse ReenviarAnaliseCompletaAprovada(ReenviarAnaliseCompletaDto dto)
        {
            var request = GetBaseRequest().AppendPathSegments("prePropostaWorkflow", "reenviarAnaliseCompletaAprovada");
            var response = request.PostJsonAsync(dto).Result;
            return HandleResponse<BaseResponse>(response);
        }
        public BaseResponse AprovarAnaliseSimplificadaAprovadaProposta(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta",id, "AprovarAnaliseSimplificadaAprovadaProposta");
            var response = request.PostAsync().Result; 

            return HandleResponse<BaseResponse>(response);
        }
        public BaseResponse AprovarAnaliseCompletaAprovadaProposta(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", id, "AprovarAnaliseCompletaAprovadaProposta");
            var response = request.PostAsync().Result;

            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse CancelarPreProposta(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", id, "cancelar");
            var response = request.PostAsync().Result;
            return HandleResponse<BaseResponse>(response);
        }

    }
}
