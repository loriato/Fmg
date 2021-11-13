using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate;
using System;
using System.Web.Mvc;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class LogsExecucaoController : BaseController
    {
        private LogExecucaoRepository _logExecucaoRepository { get; set; }


        public ActionResult Index()
        {
            return PartialView("~/Views/LogExecucao/_DialogLogExecucao.cshtml");
        }

        public ActionResult BuscarExecucao(DataSourceRequest request, long idExecucao)
        {
            var result = _logExecucaoRepository.BuscarExecucaoPorIdExecucao(idExecucao);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarLogsExecucao(DataSourceRequest request, DateTime? dataInicio, DateTime? dataFim, TipoLog? tipo, string log, long idExecucao)
        {
            var result = _logExecucaoRepository.ListarLogsExecucao(request, dataInicio, dataFim, tipo, log, idExecucao);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}