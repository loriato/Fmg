using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Security;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.EmpresaVenda.Web.App_Start;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("BTC09")]
    public class ParametroRoboController : BaseController
    {

        private ViewLogExecucaoRepository _viewLogExecucaoRepository { get; set; }
        private QuartzService _quartzService { get; set; }
        private LogExecucaoService _logExecucaoService { get; set; }


        [BaseAuthorize("BTC09", "Visualizar")]
        public ActionResult Index()
        {
            ParametroRoboViewModel model = new ParametroRoboViewModel();
            model.Quartz = new QuartzConfiguration();
            model.Lista = _quartzService.ListarQuartz().OrderBy(x => x.Nome).ToList();
            return View(model);
        }

        [BaseAuthorize("BTC09", "Incluir")]
        [TransactionAttribute(TransactionAttributeType.Required)]
        public ActionResult Incluir(QuartzConfiguration param)
        {
            return Save(param);
        }

        [BaseAuthorize("BTC09", "Atualizar")]
        [TransactionAttribute(TransactionAttributeType.Required)]
        public ActionResult Atualizar(QuartzConfiguration param)
        {
            return Save(param);
        }

        private ActionResult Save(QuartzConfiguration param)
        {
            var response = new JsonResponse();
            try
            {
                _quartzService.Salvar(param);
                QuartzConfig config = new QuartzConfig(CurrentSession());
                config.ReconfigurarJob(param);
                response.Objeto = param;
                response.Sucesso = true;
            }
            catch (BusinessRuleException exception)
            {
                foreach (var mensagem in exception.Errors)
                {
                    response.Mensagens.Add(mensagem);
                }
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public ActionResult Reconfigurar(long idQuartzConfiguration)
        {
            QuartzConfiguration quartzConfiguration = _quartzService.BuscarQuartz(idQuartzConfiguration);
            QuartzConfig config = new QuartzConfig(CurrentSession());
            config.ReconfigurarJob(quartzConfiguration);

            return Json(new { message = quartzConfiguration.Nome + " reconfigurado!" }, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("BTC09", "Executar")]
        public ActionResult Executar(QuartzConfiguration param)
        {
            var response = new JsonResponse();
            try
            {
                QuartzConfig config = new QuartzConfig(CurrentSession());
                config.ExecutarJobAgora(param);
                response.Mensagens.Add(GlobalMessages.ExecucaoRoboIniciada);
                response.Sucesso = true;
            }
            catch (BusinessRuleException exc)
            {
                response.Sucesso = false;
                response.FromException(exc);
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public ActionResult ListarExecucoes(DataSourceRequest request, DateTime? dataInicio, DateTime? dataFim, long idQuartz)
        {
            var result = _viewLogExecucaoRepository.ListarExecucoesQuartzPorPeriodo(dataInicio, dataFim, idQuartz);
            return Json(result.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cancelar()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [BaseAuthorize("BTC09", "ExcluirLog")]
        [TransactionAttribute(TransactionAttributeType.Required)]
        public ActionResult ExcluirExecucoes(DataSourceRequest request, long id, DateTime? de, DateTime? ate)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                _logExecucaoService.Excluir(id, de, ate);
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(GlobalMessages.MsgSucessoRemovidos);
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.Sucesso = false;
                jsonResponse.FromException(bre);
            }
            return Json(jsonResponse);
        }

        public JsonResult BuscarQuartz(long idQuartz)
        {
            var result = _quartzService.BuscarQuartz(idQuartz);
            return Json(result);
        }

        public ActionResult ListaQuartzPorNome()
        {
            var results = _quartzService.ListarQuartz().OrderBy(x => x.Nome)
                .Select(x => new
                {
                    text = x.Nome,
                    id = x.Id
                }).ToList();

            return Json(results, JsonRequestBehavior.AllowGet);
        }

    }
}