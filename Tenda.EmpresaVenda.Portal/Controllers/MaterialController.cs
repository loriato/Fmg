using Europa.Extensions;
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
    public class MaterialController : BaseController
    {
        private ConsumoRepository _consumoRepository { get; set; }
        private ConsumoService _consumoService { get; set; }
        private CautelaRepository _cautelaRepository { get; set; }
        private CautelaService _cautelaService { get; set; }
        private UsuarioRepository _usuarioRepository { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        #region cautela
        public ActionResult ListarMaterialCautela(DataSourceRequest request, string nome)
        {
            var result = _cautelaRepository.Listar(request, nome);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult IncluirMaterialCautela(Cautela model)
        {
            return SalvarMaterialCautela(model, true);
        }
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AlterarMaterialCautela(Cautela model)
        {
            return SalvarMaterialCautela(model, false);
        }
        private JsonResult SalvarMaterialCautela(Cautela model, bool incluir)
        {

            var baseResponse = new BaseResponse();
            try
            {
                _cautelaService.Salvar(model);
                baseResponse.Success = true;
                baseResponse.SuccessResponse(string.Format("Material {0} foi {1} com sucesso", model.Nome, incluir ? "cadastrado" : "alterado"));
            }
            catch (Exception e)
            {
                baseResponse.Success = false;
            }
            return Json(baseResponse, JsonRequestBehavior.AllowGet);

        }
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult PedidoCautela(long idMaterial, long idUsuario, long quantidade)
        {
            var baseResponse = new BaseResponse();
            var cautela = _cautelaRepository.FindById(idMaterial);
            var usuario = _usuarioRepository.FindById(idUsuario);

            try
            {
                _cautelaService.RealizarPedido(cautela, usuario, quantidade);
                baseResponse.SuccessResponse(string.Format("O pedido de {0} {1} do {2} foi realizado com sucesso", quantidade, cautela.Nome.ToLower(), usuario.Nome));

            }
            catch (Exception e)
            {
                baseResponse.ErrorResponse("Ocorreu algum erro");
            }
            return Json(baseResponse, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region consumo
        public ActionResult ListarMaterialConsumo(DataSourceRequest request, string nome)
        {
            var result = _consumoRepository.Listar(request, nome);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult IncluirMaterialConsumo(Consumo model)
        {
            return SalvarMaterialConsumo(model, true);
        }
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AlterarMaterialConsumo(Consumo model)
        {
            return SalvarMaterialConsumo(model, false);
        }
        private JsonResult SalvarMaterialConsumo(Consumo model, bool incluir)
        {

            var baseResponse = new BaseResponse();
            try
            {
                _consumoService.Salvar(model);
                baseResponse.Success = true;
                baseResponse.SuccessResponse(string.Format("Material {0} foi {1} com sucesso", model.Nome, incluir ? "cadastrado" : "alterado"));
            }
            catch (Exception e)
            {
                baseResponse.Success = false;
            }
            return Json(baseResponse, JsonRequestBehavior.AllowGet);

        }
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult PedidoConsumo(long idMaterial, long idUsuario, long quantidade)
        {
            var baseResponse = new BaseResponse();
            var consumo = _consumoRepository.FindById(idMaterial);
            var usuario = _usuarioRepository.FindById(idUsuario);

            try
            {
                _consumoService.RealizarPedido(consumo, usuario, quantidade);
                baseResponse.SuccessResponse(string.Format("O pedido de {0} {1} do {2} foi realizado com sucesso", quantidade, consumo.Nome.ToLower(), usuario.Nome));

            }
            catch (Exception e)
            {
                baseResponse.ErrorResponse("Ocorreu algum erro");
            }
            return Json(baseResponse, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}