using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate;
using PortalPosVenda.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Security.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class LogWebServiceController : BaseController
    {
        private WebServiceMessageRepository _webServiceRepository { get; set; }


        [BaseAuthorize("SEG08", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar(DataSourceRequest request, LogWebServiceDTO filtro)
        {
            var result = _webServiceRepository.Listar(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("SEG08", "ExcluirLog")]
        public ActionResult Excluir(DataSourceRequest request, LogWebServiceDTO filtro)
        {
            var json = new JsonResponse();
            var numRegs = _webServiceRepository.ExcluirLogs(filtro);
            json.Mensagens.Add(string.Format(GlobalMessages.ForamRemovidosRegistrosLog, numRegs));
            json.Sucesso = true;
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}