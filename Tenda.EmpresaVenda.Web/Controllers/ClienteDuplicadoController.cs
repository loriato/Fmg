using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC14")]
    public class ClienteDuplicadoController : BaseController
    {
        private ViewClienteDuplicadoRepository _viewClienteDuplicadoRepository { get; set; }
        private ViewPrePropostaRepository _viewPrePropostaRepository { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [BaseAuthorize("GEC14", "Visualizar")]
        public JsonResult ListarClientes(DataSourceRequest request,HierarquiaCicloFinanceiroDTO filtro)
        {
            filtro.IdCoordenador = SessionAttributes.Current().UsuarioPortal.Id;
            var response = _viewClienteDuplicadoRepository.ListarClientes(request, filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("GEC14", "Visualizar")]
        public JsonResult ListarPreProposta(DataSourceRequest request, ConsultaPrePropostaDto filtro)
        {
            var response = _viewPrePropostaRepository.Listar(request, filtro, null);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}