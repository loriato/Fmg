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
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize(true)]
    public class CommonsController : BaseController
    {

        public JsonResult ClearCache()
        {
            ProjectProperties.ClearPropertiesCache();
            return Json(new { Message = "Cache limpo com sucesso" }, JsonRequestBehavior.AllowGet);
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

    }
}