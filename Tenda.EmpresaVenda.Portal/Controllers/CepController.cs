using Europa.Commons;
using Europa.Web;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using Tenda.Domain.Core.Services;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    [BaseAuthorize(true)]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CepController : Controller
    {
        // GET: Cep
        public JsonResult Index(string cep)
        {
            try
            {
                var cepDTO = CepService.ConsultaCEPWS(cep);
                return Json(cepDTO == null ? new Tenda.Domain.Core.Services.Models.CepDTO() : cepDTO, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                if (e is BusinessRuleException)
                {
                    var bre = (e as BusinessRuleException);
                    JsonResponse response = new JsonResponse();
                    response.Sucesso = false;
                    response.Mensagens.AddRange(bre.Errors);
                    response.Objeto = bre.Errors.First();
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    JsonResponse response = new JsonResponse();
                    response.Sucesso = false;
                    response.Mensagens.Add("Ocorreu um erro ao consultar o cep!");
                    response.Objeto = e.Message;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}