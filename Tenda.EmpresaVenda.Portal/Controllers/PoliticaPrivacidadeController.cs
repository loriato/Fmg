using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate.Exceptions;
using System;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    public class PoliticaPrivacidadeController : BaseController
    {
        private StaticResourceService _staticResourceService { get; set; }
        public ActionResult Index()
        {
            var banner = new BannerPortalEvDTO();

            var resourcePath = _staticResourceService.LoadResource(ProjectProperties.IdPoliticaPrivacidade);
            var urlPoliticaPrivacidade = _staticResourceService.CreateUrl(GetWebAppRoot(), resourcePath);
            banner.Url = urlPoliticaPrivacidade;

            return View(banner);
        }
    }
}