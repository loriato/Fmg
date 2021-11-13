using Newtonsoft.Json;
using System;
using System.IO;
using System.Web.Http;
using System.Web.Http.Description;
using Tenda.EmpresaVenda.Api.Models;
using Tenda.EmpresaVenda.Api.Security;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("")]
    public class DefaultController : BaseApiController
    {
        [HttpGet]
        [Route("")]
        [AuthenticateUserByToken(true)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Index()
        {
            try
            {
                var fullPath = $@"{AppDomain.CurrentDomain.BaseDirectory}\App_Data\version.info";

                var versionText = File.ReadAllText(fullPath);

                if (versionText == null) { Json("develpment"); }

                var version = JsonConvert.DeserializeObject<ApiVersion>(versionText);

                if (version.Version == null) { version.Version = "development"; }

                return Json(version.Version);

            }
            catch (Exception err)
            {
                return Ok();
            }
        }
    }
}
