using System.IO;
using System.Web.Mvc;
using System.Web.SessionState;
using Tenda.EmpresaVenda.PortalHouse.Rest;
using Tenda.EmpresaVenda.PortalHouse.Security;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    [BaseAuthorize(true)]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BaseController : Controller
    {
        public EmpresaVendaApi EmpresaVendaApi { get; set; }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                var aux = sw.GetStringBuilder().ToString();
                return aux;
            }
        }
        
        protected override JsonResult Json(object data, 
            string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }

    }
}