using Europa.Extensions;
using Europa.Fmg.Domain.Dto.Viatura;
using Europa.Fmg.Domain.Repository;
using Europa.Fmg.Domain.Services;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Fmg.Models;
using Tenda.Domain.Security.Repository;

namespace Europa.Fmg.Portal.Controllers
{
    public class ViaturaController : BaseController
    {
        private ViaturaRepository _viaturaRepository { get; set; }
        private ViaturaService _viaturaService { get; set; }
        private ViaturaUsuarioRepository _viaturaUsuarioRepository { get; set; }
        private UsuarioRepository _usuarioRepository { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Listar(DataSourceRequest request, FiltroViaturaDto filtro)
        {
            var query = _viaturaRepository.Listar(request, filtro);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Salvar(Viatura viatura)
        {
            var baseResponse = new BaseResponse();
            try
            {
                _viaturaService.Salvar(viatura);
                baseResponse.SuccessResponse(string.Format("A Viatura {0} foi cadastrada com sucesso", viatura.Placa));
            }
            catch (Exception e)
            {
                baseResponse.Success = false;
            }
            return Json(baseResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Visualizar(long id)
        {
            var dto = _viaturaRepository.FindById(id);
            return View(dto);
        }
        public ActionResult ListarViaturaUsuario(DataSourceRequest request, ViaturaUsuario model)
        {
            var query = _viaturaUsuarioRepository.Listar(request, model);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Alocar(long idViatura, long idUsuario)
        {
            var baseResponse = new BaseResponse();
            try
            {
                var usuario = _usuarioRepository.FindById(idUsuario);
                var viatura = _viaturaRepository.FindById(idViatura);
                _viaturaService.Alocar(usuario, viatura);

                baseResponse.SuccessResponse(string.Format("A Viatura {0} - {1} foi alocado pelo {2} com sucesso", viatura.Modelo, viatura.Placa, usuario.Nome));

            }
            catch (Exception e)
            {
                baseResponse.Success = false;
            }

            return Json(baseResponse, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Desalocar(long idViatura, long quilometragem)
        {
            var baseResponse = new BaseResponse();
            try
            {
                var viatura = _viaturaRepository.FindById(idViatura);
                var viaturaUsuario = _viaturaService.Desalocar(viatura, quilometragem);

                baseResponse.SuccessResponse(string.Format("A Viatura {0} - {1} foi desalocado com sucesso", viatura.Modelo, viatura.Placa));

            }
            catch (Exception e)
            {
                baseResponse.Success = false;
            }

            return Json(baseResponse, JsonRequestBehavior.AllowGet);
        }
    }
}