using Europa.Rest;
using Europa.Web;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using Tenda.Domain.Core.Services.Models;
using Tenda.EmpresaVenda.PortalHouse.Security;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    [BaseAuthorize(true)]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CepController : BaseController
    {
        public JsonResult Index(string cep)
        {
            var result = new JsonResponse();

            try
            {
                var response = EmpresaVendaApi.ConsultarEnderecoPorCep(cep);
                result.FromBaseResponse(response);
                if (result.Sucesso)
                {
                    var dto = ((JObject)response.Data).ToObject<CepDTO>();
                    result.Objeto = dto;
                }
            }
            catch (ApiException e)
            {
                result.FromBaseResponse(e.GetResponse());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}