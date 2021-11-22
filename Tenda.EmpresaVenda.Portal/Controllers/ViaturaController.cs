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

namespace Europa.Fmg.Portal.Controllers
{
    public class ViaturaController : BaseController
    {
        private ViaturaRepository _viaturaRepository { get; set; }
        private ViaturaService _viaturaService { get; set; }
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
                baseResponse.Success = true;
                baseResponse.SuccessResponse(string.Format("A Viatura {0} foi cadastrada com sucesso", viatura.Placa));
            }
            catch (Exception e)
            {
                baseResponse.Success = false;
            }
            return Json(baseResponse, JsonRequestBehavior.AllowGet);
        }
    }
}