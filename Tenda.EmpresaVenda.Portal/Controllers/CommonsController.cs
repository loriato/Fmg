using Europa.Web;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Cache;
using Tenda.EmpresaVenda.Domain.Commons;
using Tenda.EmpresaVenda.Portal.Commons;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    [BaseAuthorize(true)]
    public class CommonsController : BaseController
    {
        public JsonResult ClearCache()
        {
            var currentData = ProjectProperties.CurrentProperties();
            ProjectProperties.ClearPropertiesCache();
            TemplateEmailFactory.InvalidateCache();
            return Json(currentData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CurrentProperties()
        {
            var properties = ProjectProperties.CurrentProperties();
            return Json(properties, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Refresh(string key, string newValue)
        {
            ProjectProperties.SetParameter(key, newValue);
            return CurrentProperties();
        }

        public ActionResult Template(string templateName)
        {
            Dictionary<string, string> toReplace = new Dictionary<string, string>();
            toReplace.Add("img-header", "TESTE");
            toReplace.Add("img-footer", "TESTE");
            toReplace.Add("nome", "NOME DA PESSOA");
            toReplace.Add("token", "TOKEN!");
            toReplace.Add("linkAtivacao", "TOKEN!");
            
            return Content(TemplateEmailFactory.ResolveTemplateWithReplace(templateName, toReplace));
        }

    }
}