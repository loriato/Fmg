using Europa.Commons;
using Europa.Extensions;
using Europa.Web;
using NHibernate;
using System.Web.Mvc;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Security;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Security.Services.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("SEG02")]
    public class ParametroSistemaController : BaseController
    {
        private ParametroSistemaService _parametroSistemaService { get; set; }
        private SistemaService _sistemaService { get; set; }

        [BaseAuthorize("SEG02", "Visualizar")]
        public ActionResult Index()
        {
            var model = _parametroSistemaService.BuscarParametro(ApplicationInfo.CodigoSistema) ?? new ParametroSistema()
            {
                Sistema = _sistemaService.FindByCodigo(ApplicationInfo.CodigoSistema),
            };
            CurrentSession().Evict(model);
            return View("Index", model);
        }

        [BaseAuthorize("SEG02", "Visualizar")]
        public ActionResult Cancelar()
        {
            return RedirectToAction("Index");
        }

        [BaseAuthorize("SEG02", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Salvar(ParametroSistema entidadeParametroSistema)
        {
            var response = new JsonResponse();
            if (entidadeParametroSistema.PerfilInicial.Id.IsEmpty())
            {
                entidadeParametroSistema.PerfilInicial = null;
            }
            try
            {
                _parametroSistemaService.Salvar(entidadeParametroSistema);
                response.Objeto = entidadeParametroSistema;
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

    }
}