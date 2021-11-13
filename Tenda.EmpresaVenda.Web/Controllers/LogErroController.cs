using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Security.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class LogErroController : BaseController
    {
        public ErrorRepository _errorRepository { get; set; }


        [BaseAuthorize("SEG09", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar(DataSourceRequest request, LogErroDTO filtro)
        {
            var result = _errorRepository.Listar(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("SEG09", "ExcluirLog")]
        public ActionResult Excluir(DataSourceRequest request, LogErroDTO filtro)
        {
            var json = new JsonResponse();
            var numRegs = _errorRepository.ExcluirLogs(filtro);
            json.Mensagens.Add(string.Format(GlobalMessages.ForamRemovidosRegistrosLog, numRegs));
            json.Sucesso = true;
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}