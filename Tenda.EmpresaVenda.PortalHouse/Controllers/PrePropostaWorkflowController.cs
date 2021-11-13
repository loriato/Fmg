using Europa.Rest;
using Europa.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.ApiService.Models.PrePropostaWorkflow;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class PrePropostaWorkflowController : BaseController
    {
        public JsonResult AguardandoFluxoAguardandoAnaliseCompletaRule(PrePropostaWorkflowDto prePropostaWorkflowDto)
        {
            var reponse = EmpresaVendaApi.AguardandoFluxoAguardandoAnaliseCompletaRule(prePropostaWorkflowDto);
            return Json(reponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AguardandoIntegracao(PrePropostaWorkflowDto prePropostaWorkflowDto)
        {
            var reponse = EmpresaVendaApi.AguardandoIntegracao(prePropostaWorkflowDto);
            return Json(reponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReenviarAnaliseCompletaAprovada(ReenviarAnaliseCompletaDto dto)
        {
            var result = new JsonResponse();

            try
            {
                var response = EmpresaVendaApi.ReenviarAnaliseCompletaAprovada(dto);
                result.FromBaseResponse(response);
            }
            catch(ApiException e)
            {
                result.FromApiException(e);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AprovarAnaliseSimplificadaAprovadaProposta(long idPreProposta)
        {

            var result = new JsonResponse();

            try
            {
                var response = EmpresaVendaApi.AprovarAnaliseSimplificadaAprovadaProposta(idPreProposta);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromApiException(e);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AprovarAnaliseCompletaAprovadaProposta(long idPreProposta)
        {

            var result = new JsonResponse();

            try
            {
                var response = EmpresaVendaApi.AprovarAnaliseCompletaAprovadaProposta(idPreProposta);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromApiException(e);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}