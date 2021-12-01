using Europa.Extensions;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;

namespace Europa.Fmg.Portal.Controllers
{
    public class UsuarioController : BaseController
    {
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        private UsuarioPortalService _usuarioPortalService { get; set; }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Visualizar(long id)
        {
            var dto = _usuarioPortalRepository.FindById(id);
            return View(dto);
        }
        public ActionResult Listar(DataSourceRequest request, UsuarioPortal filtro)
        {
            var query = _usuarioPortalRepository.Listar(request, filtro);

            return Json(query, JsonRequestBehavior.AllowGet);
        }
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Salvar(UsuarioPortal usuario)
        {
            var baseResponse = new BaseResponse();
            try
            {
                _usuarioPortalService.Salvar(usuario);
                baseResponse.Success = true;
                baseResponse.SuccessResponse(string.Format("{0} cadastrado com sucesso", usuario.Nome));
            }
            catch (Exception e)
            {
                baseResponse.Success = false;
            }
            return Json(baseResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListarUsuariosAutocomplete(DataSourceRequest request)
        {
            var result = _usuarioPortalService.ListarUsuariosAutocomplete(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}