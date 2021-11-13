using Europa.Extensions;
using Europa.Rest;
using Europa.Web;
using System;
using System.Linq;
using System.Web.Mvc;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;
using Tenda.EmpresaVenda.PortalHouse.Models.Application;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class HierarquiaHouseController : BaseController
    {

        public ActionResult Index()
        {
            var lojas = SessionAttributes.Current().UsuarioPortal.Lojas;

            if (lojas.HasValue()&& lojas.Count < 2)
            {
                SessionAttributes.Current().UsuarioPortal.IdLoja = lojas.FirstOrDefault().Id;
                SessionAttributes.Current().UsuarioPortal.NomeLoja = lojas.FirstOrDefault().Nome;
                SessionAttributes.Current().UsuarioPortal.EstadoLoja = lojas.FirstOrDefault().Estado;

                return RedirectToAction("Index", "Home");
            }

            return View(lojas);
        }

        [HttpPost]
        public JsonResult SelecionarLoja(long idloja)
        {
            var response = new BaseResponse();

            var apiEx = new ApiException();

            try
            {
                if (idloja.IsEmpty())
                {
                    apiEx.AddError(string.Format("Selecione uma loja"));
                    apiEx.ThrowIfHasError();
                }

                var loja = SessionAttributes.Current().UsuarioPortal.Lojas.Where(x=>x.Id==idloja).SingleOrDefault();

                if (loja.IsEmpty())
                {
                    apiEx.AddError(string.Format("Loja não autorizada"));
                    apiEx.ThrowIfHasError();
                }
                
                SessionAttributes.Current().UsuarioPortal.IdLoja = loja.Id;
                SessionAttributes.Current().UsuarioPortal.NomeLoja = loja.Nome;
                SessionAttributes.Current().UsuarioPortal.EstadoLoja = loja.Estado;

                //response.SuccessResponse(string.Format("Loja {0} selecioanda com sucesso!",loja.Nome));

                var hierarquiaHouseDto = new HierarquiaHouseDto();
                hierarquiaHouseDto.IdHouse = loja.Id;

                response= EmpresaVendaApi.TrocarHouse(hierarquiaHouseDto);

            }
            catch (ApiException ex)
            {
                response = ex.GetResponse();
            }catch(Exception ex)
            {
                ExceptionLogger.LogException(ex);
                response.ErrorResponse(ex.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteAgenteVendaHouseConsulta(DataSourceRequest request)
        {
            var filtro = new FiltroHierarquiaHouseDto();
            filtro.Request = request;

            var response = EmpresaVendaApi.AutoCompleteAgenteVendaHouseConsulta(filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}